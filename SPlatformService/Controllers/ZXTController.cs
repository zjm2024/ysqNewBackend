using CoreFramework.VO;
using SPlatformService.Models;
using SPlatformService.TokenMange;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.Logging.BO;

namespace SPlatformService.Controllers
{
    [RoutePrefix("SPWebAPI/ZXTIM")]
    [TokenProjector]
    public class ZXTController : ApiController
    {

        private static Dictionary<string, WebSocket> CONNECT_POOL = new Dictionary<string, WebSocket>();//用户连接池

        /// <summary>
        /// 根据会员ID创建WebSocket连接
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("OpenWebSocket"), HttpGet, Anonymous]
        public HttpResponseMessage OpenWebSocket(string token)
        {
            if (HttpContext.Current.IsWebSocketRequest)
            {
                CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
                string user = cProfile.CustomerId.ToString();
                if (!CONNECT_POOL.ContainsKey(user))
                {
                    HttpContext.Current.AcceptWebSocketRequest(ProcessChat);
                }
                else {
                    ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
                    buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes("AnotherPlace"));
                    WebSocket destSocket = CONNECT_POOL[user];
                    if (destSocket != null && destSocket.State == WebSocketState.Open)
                    {
                        destSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                        destSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "AnotherPlace", CancellationToken.None);
                    }
                    HttpContext.Current.AcceptWebSocketRequest(ProcessChat);
                }
                   
            }
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols); //构造同意切换至Web Socket的Response.
        }

        private async Task ProcessChat(AspNetWebSocketContext context)
        {
            WebSocket socket = context.WebSocket;
            string token = context.QueryString["token"].ToString();
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            string user = cProfile.CustomerId.ToString();

            try
            {
                #region 用户添加连接池
                //第一次open时，添加到连接池中
                if (!CONNECT_POOL.ContainsKey(user))
                    CONNECT_POOL.Add(user, socket);//不存在，添加
                else
                {
                    if (socket != CONNECT_POOL[user])//当前对象不一致，更新
                    {
                        CONNECT_POOL[user] = socket;
                    } 
                }
                #endregion

                while (true)
                {
                    if (socket.State == WebSocketState.Open)
                    {
                        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
                        WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                        #region 消息处理（字符截取、消息转发）
                        try
                        {
                            #region 关闭Socket处理，删除连接池
                            if (socket.State != WebSocketState.Open)//连接关闭
                            {
                                if (CONNECT_POOL.ContainsKey(user)) CONNECT_POOL.Remove(user);//删除连接池
                                break;
                            }
                            #endregion
                        }
                        catch (Exception exs)
                        {
                            //消息转发异常处理，本次消息忽略 继续监听接下来的消息
                        }
                        #endregion
                    }
                    else
                    {
                        break;
                    }
                }//while end
            }
            catch (Exception ex)
            {
                //整体异常处理
                if (CONNECT_POOL.ContainsKey(user)) CONNECT_POOL.Remove(user);
            }
        }

        /// <summary>
        /// 根据会员ID获取会员信息
        /// </summary>
        /// <param name="customerId">会员ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetCustomer"), HttpGet]
        public ResultObject GetCustomer(int customerId, string token)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerViewVO uVO = uBO.FindById(customerId);

            ZXTCustomerVO zVO = new ZXTCustomerVO();
            zVO.AgencyId = uVO.AgencyId;
            zVO.AgencyStatus = uVO.AgencyStatus;
            zVO.Birthday = uVO.Birthday;
            zVO.BusinessId = uVO.BusinessId;
            zVO.BusinessStatus = uVO.BusinessStatus;
            zVO.CustomerCode = uVO.CustomerCode;
            zVO.CustomerId = uVO.CustomerId;
            zVO.CustomerName = uVO.CustomerName;
            zVO.Description = uVO.Description;
            zVO.HeaderLogo = uVO.HeaderLogo;
            zVO.Sex = uVO.Sex;
            zVO.SexName = uVO.SexName;
            zVO.Status = uVO.Status;
            zVO.StatusName = uVO.StatusName;

            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = zVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取全部联系人
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAllFriend"), HttpGet]
        public ResultObject GetAllFriend(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;


            ZXTBO zBO= new ZXTBO(cProfile);

            List<ZXTFriendViewVO> voList = zBO.FindZXTFriendViewByCustomer("CustomerId = " + customerId);

            if (voList.Count > 0)
                return new ResultObject() { Flag = 1, Message = "获取成功", Result = voList};
            else
                return new ResultObject() { Flag = 0, Message = "获取失败", Result = null };

        }

        /// <summary>
        /// 获取聊天信息列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMessageListByCustomer"), HttpPost]
        public ResultObject GetMessageListByCustomer([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            //只显示与会员有关的。
            string conditionStr = " (MessageFrom = " + cProfile.CustomerId + " or MessageTo = " + cProfile.CustomerId + ") and (" + condition.Filter.Result()+")";
            Paging pageInfo = condition.PageInfo;
            ZXTBO zBO = new ZXTBO(cProfile);
            List<ZXTMessageViewVO> list = zBO.FindZXTMessageAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
        }

        /// <summary>
        /// 获取最近联系聊天信息列表
        /// </summary>
        /// <param name="limit">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetLatelyMessagaeByCustomer"), HttpGet]
        public ResultObject GetLatelyMessagaeByCustomer(int limit, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            if (limit == 0)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            //只显示与会员有关的。
            string conditionStr = " MessageFrom = " + cProfile.CustomerId + " or MessageTo = " + cProfile.CustomerId;
            ZXTBO zBO = new ZXTBO(cProfile);
            List<ZXTMessageViewVO> list = zBO.FindLatelyMessagaeByPageIndex(conditionStr, limit, "SendAt","DESC");
            List<ZXTMessageViewVO> list2 = list;

            //移除重复
            for (int i = 0; i < list.Count; i++) {
                list2.RemoveAll(j=> {
                    if (j.MessageFrom == list[i].MessageTo && j.MessageTo == list[i].MessageFrom)
                    {
                        return true;
                    }
                    else {
                        return false;
                    }
                });
            }
            for (int i = 0; i < list2.Count; i++)
            {

                if (list2[i].Status == 0 && list2[i].MessageTo == cProfile.CustomerId)
                {
                    list2[i].UnreadCount = zBO.FindZXTMessageCount("Status = 0 and MessageFrom=" + list2[i].MessageFrom + " and MessageTo=" + cProfile.CustomerId);
                }
            }
            try
            {
                MessageBO uBO = new MessageBO(cProfile);
                //系统消息
                List<MessageViewVO> SYSMessagelist = uBO.FindMessageByCustomerId(cProfile.CustomerId, (int)MessageType.SYS, "SendAt", "desc");
                if (SYSMessagelist.Count > 0)
                {
                    MessageViewVO SYSMessage = SYSMessagelist[0];
                    ZXTMessageViewVO SYSZXTMessage = new ZXTMessageViewVO();
                    SYSZXTMessage.MessageID = (int)MessageType.SYS;
                    SYSZXTMessage.MessageFrom = 0;
                    SYSZXTMessage.MessageTo = cProfile.CustomerId;
                    SYSZXTMessage.MessageType = SYSMessage.MessageTypeName;
                    SYSZXTMessage.MeCustomerName = SYSMessage.Title;
                    SYSZXTMessage.MeHeaderLogo = ConfigInfo.Instance.SITEURL + "Style/images/newpc/SYSMessage.png";
                    SYSZXTMessage.Message = SYSMessage.Message;
                    SYSZXTMessage.SendAt = SYSMessage.SendAt;
                    SYSZXTMessage.UnreadCount = uBO.FindMessageTotalCount("Status = 0 and SendTo=" + cProfile.CustomerId + " and MessageTypeId=" + (int)MessageType.SYS);
                    list2.Add(SYSZXTMessage);
                }

                //交易提醒
                List<MessageViewVO> ProjectMessagelist = uBO.FindMessageByCustomerId(cProfile.CustomerId, (int)MessageType.Project, "SendAt", "desc");
                if (ProjectMessagelist.Count > 0)
                {
                    MessageViewVO ProjectMessage = ProjectMessagelist[0];
                    ZXTMessageViewVO ProjectZXTMessage = new ZXTMessageViewVO();
                    ProjectZXTMessage.MessageID = (int)MessageType.Project;
                    ProjectZXTMessage.MessageFrom = 0;
                    ProjectZXTMessage.MessageTo = cProfile.CustomerId;
                    ProjectZXTMessage.MessageType = ProjectMessage.MessageTypeName;
                    ProjectZXTMessage.MeCustomerName = ProjectMessage.Title;
                    ProjectZXTMessage.MeHeaderLogo = ConfigInfo.Instance.SITEURL + "Style/images/newpc/ProjectMessage.png";
                    ProjectZXTMessage.Message = ProjectMessage.Message;
                    ProjectZXTMessage.SendAt = ProjectMessage.SendAt;
                    ProjectZXTMessage.UnreadCount = uBO.FindMessageTotalCount("Status = 0 and SendTo=" + cProfile.CustomerId + " and MessageTypeId=" + (int)MessageType.Project);
                    list2.Add(ProjectZXTMessage);
                }

                //认证申请
                List<MessageViewVO> RZSQMessagelist = uBO.FindMessageByCustomerId(cProfile.CustomerId, (int)MessageType.RZSQ, "SendAt", "desc");
                if (RZSQMessagelist.Count > 0)
                {
                    MessageViewVO RZSQMessage = RZSQMessagelist[0];
                    ZXTMessageViewVO RZSQZXTMessage = new ZXTMessageViewVO();
                    RZSQZXTMessage.MessageID = (int)MessageType.RZSQ;
                    RZSQZXTMessage.MessageFrom = 0;
                    RZSQZXTMessage.MessageTo = cProfile.CustomerId;
                    RZSQZXTMessage.MessageType = RZSQMessage.MessageTypeName;
                    RZSQZXTMessage.MeCustomerName = RZSQMessage.Title;
                    RZSQZXTMessage.MeHeaderLogo = ConfigInfo.Instance.SITEURL + "Style/images/newpc/RZSQMessage.png";
                    RZSQZXTMessage.Message = RZSQMessage.Message;
                    RZSQZXTMessage.SendAt = RZSQMessage.SendAt;
                    RZSQZXTMessage.UnreadCount = uBO.FindMessageTotalCount("Status = 0 and SendTo=" + cProfile.CustomerId + " and MessageTypeId=" + (int)MessageType.RZSQ);
                    list2.Add(RZSQZXTMessage);
                }

                //投标信息
                List<MessageViewVO> TenderMessagelist = uBO.FindMessageByCustomerId(cProfile.CustomerId, (int)MessageType.Tender, "SendAt", "desc");
                if (TenderMessagelist.Count > 0)
                {
                    MessageViewVO TenderMessage = TenderMessagelist[0];
                    ZXTMessageViewVO TenderZXTMessage = new ZXTMessageViewVO();
                    TenderZXTMessage.MessageID = (int)MessageType.Tender;
                    TenderZXTMessage.MessageFrom = 0;
                    TenderZXTMessage.MessageTo = cProfile.CustomerId;
                    TenderZXTMessage.MessageType = TenderMessage.MessageTypeName;
                    TenderZXTMessage.MeCustomerName = TenderMessage.Title;
                    TenderZXTMessage.MeHeaderLogo = ConfigInfo.Instance.SITEURL + "Style/images/newpc/TenderMessage.png";
                    TenderZXTMessage.Message = TenderMessage.Message;
                    TenderZXTMessage.SendAt = TenderMessage.SendAt;
                    TenderZXTMessage.UnreadCount = uBO.FindMessageTotalCount("Status = 0 and SendTo=" + cProfile.CustomerId + " and MessageTypeId=" + (int)MessageType.Tender);
                    list2.Add(TenderZXTMessage);
                }
                list2.Sort(delegate (ZXTMessageViewVO p1, ZXTMessageViewVO p2) { return Comparer<DateTime>.Default.Compare(p2.SendAt, p1.SendAt); });
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(MessageBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
            }
            //list2.OrderBy(t => t.SendAt);
            //list2.Reverse();
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list2 };
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="ZXTMessageVO">消息VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddMessage"), HttpPost]
        public ResultObject AddMessage([FromBody] ZXTMessageVO ZXTMessageVO, string token)
        {
            if (ZXTMessageVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            if (ZXTMessageVO.MessageTo == 0)
            {
                return new ResultObject() { Flag = 0, Message = "接收Id为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            ZXTBO uBO = new ZXTBO(new CustomerProfile());
            ZXTMessageVO.MessageFrom = customerId;
            ZXTMessageVO.SendAt = DateTime.Now;
            ZXTMessageVO.Status = 0;//0:未读，1:已读

            
            string MessageText = ZXTMessageVO.Message;
            MessageText = MessageText.Replace("<br/>", "[Enter]");
            MessageText = Clear(MessageText);
            MessageText = MessageText.Replace("[Enter]", "<br/>");
            ZXTMessageVO.Message = MessageText;

            int MessageId = uBO.AddZXTMessage(ZXTMessageVO);
            if (MessageId > 0)
            {
                int Count = uBO.FindZXTFriendCount("CustomerId="+ customerId+ " and FriendTo="+ ZXTMessageVO.MessageTo);
                if (Count == 0) {
                    //如果接收人不是会员的好友的话，则加为好友
                    ZXTFriendVO zVO =new ZXTFriendVO();
                    zVO.CustomerId = customerId;
                    zVO.FriendTo = ZXTMessageVO.MessageTo;
                    uBO.AddZXTFriend(zVO);
                }

                if (CONNECT_POOL.ContainsKey(ZXTMessageVO.MessageTo.ToString()))//判断客户端是否在线
                {
                    ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
                    buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes("NewMessage"));
                    WebSocket destSocket = CONNECT_POOL[ZXTMessageVO.MessageTo.ToString()];
                    if (destSocket != null && destSocket.State == WebSocketState.Open)
                        destSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }

                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = MessageId};
            }
            else
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
        }

        /// <summary>
        /// 更新消息状态
        /// </summary>
        /// <param name="MessageID">消息ID</param>
        /// <param name="status">状态</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRequireStatus"), HttpPost]
        public ResultObject UpdateRequireStatus(string MessageID, int status, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            try
            {
                ZXTBO uBO = new ZXTBO(new CustomerProfile());
                if (!string.IsNullOrEmpty(MessageID))
                {
                    string[] bIdArr = MessageID.Split(',');
                    if (bIdArr.Length > 0)
                    {
                        ZXTMessageViewVO zVO = uBO.FindZXTMessageByMessageID(Convert.ToInt32(bIdArr[0]));
                        uBO.setMessageStatus(zVO.MessageFrom,zVO.MessageTo);
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = zVO };
                    }else
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }
                    /*
                    string[] bIdArr = MessageID.Split(',');
                    bool isAllUpdate = true;

                    for (int i = 0; i < bIdArr.Length; i++)
                    {
                        try
                        {
                            ZXTMessageVO bVO = new ZXTMessageVO();
                            bVO.MessageId = Convert.ToInt32(bIdArr[i]);
                            bVO.Status = status;
                            bool isSuccess = uBO.UpdateZXTMessage(bVO);
                        }
                        catch
                        {
                            isAllUpdate = false;
                        }
                    }
                    if (isAllUpdate)
                    {
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分更新成功!", Result = null };
                    }
                    */
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="FriendTo">好友ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelFriend"), HttpPost]
        public ResultObject DelFriend(int FriendTo, string token)
        {
            if (FriendTo == 0)
            {
                return new ResultObject() { Flag = 0, Message = "好友Id为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            ZXTBO uBO = new ZXTBO(new CustomerProfile());
            List<ZXTFriendVO> voList = uBO.FindZXTFriendByCustomer("CustomerId=" + customerId+ " and FriendTo="+ FriendTo);

            if (voList.Count > 0)
            {
                int i = uBO.DelZXTFriend(voList,"");
                if (i > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "删除成功", Result = null };
                }
                else {
                    return new ResultObject() { Flag = 0, Message = "删除失败", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败", Result = null };
            }
            
        }

        /// <summary>
        /// 根据昵称/销售名称/雇主名称搜索会员
        /// </summary>
        /// <param name="Keyword">关键字</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SearchFriend"), HttpGet]
        public ResultObject SearchFriend(string Keyword, string token)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<CustomerViewVO> uVO = uBO.FindByKeyword(Keyword);

            if (uVO.Count > 0)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "搜索不到相关信息", Result = null };
            }
           
        }
        /// <summary>
        /// 清除文本中Html的标签
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public string Clear(string Content)
        {
            Content = Zxj_ReplaceHtml("&#[^>]*;", "", Content);
            Content = Zxj_ReplaceHtml("</?marquee[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?object[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?param[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?embed[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?table[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml(" ", "", Content);
            Content = Zxj_ReplaceHtml("</?tr[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?th[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?p[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?a[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?img[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?tbody[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?li[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?span[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?div[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?th[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?td[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?script[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("(javascript|jscript|vbscript|vbs):", "", Content);
            Content = Zxj_ReplaceHtml("on(mouse|exit|error|click|key)", "", Content);
            Content = Zxj_ReplaceHtml("<\\?xml[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("<\\/?[a-z]+:[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?font[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?b[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?u[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?i[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?strong[^>]*>", "", Content);
            string clearHtml = Content;
            return clearHtml;
        }
        /// <summary>
        /// 清除文本中的Html标签
        /// </summary>
        /// <param name="patrn">要替换的标签正则表达式</param>
        /// <param name="strRep">替换为的内容</param>
        /// <param name="content">要替换的内容</param>
        /// <returns></returns>
        private string Zxj_ReplaceHtml(string patrn, string strRep, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                content = "";
            }
            Regex rgEx = new Regex(patrn, RegexOptions.IgnoreCase);
            string strTxt = rgEx.Replace(content, strRep);
            return strTxt;
        }
    }
}