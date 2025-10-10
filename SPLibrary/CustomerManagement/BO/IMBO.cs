using CoreFramework.VO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace SPLibrary.CustomerManagement.BO
{
    public class IMBO
    {
        private const string DEFAULTIMGROUPNAME = "_Default";
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();
        public IMBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }

        /// <summary>
        /// 获取 环信 Token，Token保存在DB
        /// </summary>
        /// <returns></returns>
        public string GetIMToken()
        {
            //将IM Token保存到数据库或者Cache，整个系统共用，而不是一个用户一个。            
            if (HttpRuntime.Cache["PASSPORT.IMTOKEN"] == null)
            {
                Dictionary<string, string> imTokenDic = new Dictionary<string, string>();
                imTokenDic.Add("IMToken", "");
                imTokenDic.Add("Expire", "");
                imTokenDic.Add("SetDate", "");
                imTokenDic.Add("Application", "");

                //从数据库获取IMToken
                IIMTokenDAO imTDAO = CustomerManagementDAOFactory.CreateIMTokenDAO(new UserProfile());
                List<IMTokenVO> tVOList = imTDAO.FindByParams("1=1");
                if(tVOList.Count > 0)
                {
                    imTokenDic["IMToken"] = tVOList[0].IMToken;
                    imTokenDic["Expire"] = tVOList[0].Expire.ToString();
                    imTokenDic["SetDate"] = tVOList[0].SetDate.ToString("yyyy-MM-dd HH:mm:ss");
                    imTokenDic["Application"] = tVOList[0].Application;
                }

                //Cache的过期时间为 令牌过期时间*2
                HttpRuntime.Cache.Insert("PASSPORT.IMTOKEN", imTokenDic, null, DateTime.MaxValue, TimeSpan.FromDays(7 * 2));
            }
            Dictionary<string, string> access_tokenDic = HttpRuntime.Cache["PASSPORT.IMTOKEN"] as Dictionary<string, string>;
            //不存在，为空，过期，都需要重新获取一次
            bool isGetNew = false;
            if (access_tokenDic == null || string.IsNullOrEmpty(access_tokenDic["IMToken"]))
            {
                isGetNew = true;
            }
            else
            {
                DateTime startDate = Convert.ToDateTime(access_tokenDic["SetDate"]);
                int expireSecond = Convert.ToInt32(access_tokenDic["Expire"]);
                if (startDate.AddSeconds(expireSecond - 3600) <= DateTime.Now)
                {
                    isGetNew = true;
                }
                else
                {
                    //调用一次方法，判断是否有效,查询默认账户在线状态，如果返回401，则需要重新获取token
                    List<string> header = new List<string>();
                    header.Add("Bearer " + access_tokenDic["IMToken"]);
                    string testResult = HttpHelper.HtmlFromUrlGet(ConfigInfo.Instance.IMHXURL + ConfigInfo.Instance.IMOrgName + "/" + ConfigInfo.Instance.IMAppName + "/users/" + "jimmy" + "/status", header);
                    ResultObject rObj = JsonConvert.DeserializeObject<ResultObject>(testResult);
                    if (rObj != null && rObj.Message != null && rObj.Flag == 0)
                    {
                        isGetNew = true;
                    }
                }
            }
            string accessToken = "";
            if (isGetNew)
            {
                string url = ConfigInfo.Instance.IMHXURL + ConfigInfo.Instance.IMOrgName + "/" + ConfigInfo.Instance.IMAppName + "/token";
                string bodyStr = "{\"grant_type\": \"client_credentials\",\"client_id\": \"" + ConfigInfo.Instance.IMClientId + "\",\"client_secret\": \"" + ConfigInfo.Instance.IMClientSecret + "\"}";
                string result = HttpHelper.HtmlFromUrlPost(url, bodyStr);

                var resultObj = JsonConvert.DeserializeObject(result) as JObject;
                string token = (string)resultObj["access_token"];
                string expire = (string)resultObj["expires_in"];
                string app = (string)resultObj["application"];

                //更新到Token
                if (access_tokenDic == null)
                {
                    access_tokenDic = new Dictionary<string, string>();
                    access_tokenDic.Add("IMToken", "");
                    access_tokenDic.Add("Expire", "");
                    access_tokenDic.Add("SetDate", "");
                    access_tokenDic.Add("Application", "");
                }
                access_tokenDic["IMToken"] = token;
                access_tokenDic["Expire"] = expire;
                access_tokenDic["SetDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                access_tokenDic["Application"] = app;

                HttpRuntime.Cache["PASSPORT.IMTOKEN"] = access_tokenDic;

                IIMTokenDAO imTDAO = CustomerManagementDAOFactory.CreateIMTokenDAO(new UserProfile());
                IMTokenVO imTVO = new IMTokenVO();
                imTVO.IMToken = token;
                imTVO.Expire = Convert.ToInt32(expire);
                imTVO.SetDate = DateTime.Now;
                imTVO.Application = app;

                List<IMTokenVO> tVOList = imTDAO.FindByParams("1=1");
                if (tVOList.Count > 0)
                {
                    imTVO.IMTokenId = tVOList[0].IMTokenId;

                    imTDAO.UpdateById(imTVO);
                }
                else
                {
                    imTDAO.Insert(imTVO);
                }

                accessToken = token;
            }
            else
            {
                accessToken = access_tokenDic["IMToken"];
            }

            return accessToken;
        }

        /// <summary>
        /// 注册新用户
        /// </summary>
        /// <param name="imUserName">用户名称</param>
        /// <param name="imPassword">密码</param>
        /// <param name="imNickName">昵称</param>
        /// <returns></returns>
        public int RegisterIMUser(int customerId, string imUserName, string imPassword, string imNickName)
        {
            ICustomerIMDAO cIMDAO = CustomerManagementDAOFactory.CreateCustomerIMDAO(new UserProfile());
            //判断imUserName是否存在，存在则不添加
            List<CustomerIMVO> exVOList = cIMDAO.FindByParams("IMId = '" + imUserName + "'");
            if (exVOList.Count > 0)
                return -1;

            string imToken = GetIMToken();

            string url = ConfigInfo.Instance.IMHXURL + ConfigInfo.Instance.IMOrgName + "/" + ConfigInfo.Instance.IMAppName + "/users";
            string bodyStr = "{\"username\":\"" + imUserName + "\",\"password\":\"" + imPassword + "\", \"nickname\":\"" + imNickName + "\"}";
            List<string> header = new List<string>();
            header.Add("Bearer " + imToken);
            string result = HttpHelper.HtmlFromUrlPost(url, bodyStr, header);
            ResultObject rObj = JsonConvert.DeserializeObject<ResultObject>(result);
            if (rObj != null && rObj.Message != null && rObj.Flag == 0)
            {
                //用户已存在、用户名或密码为空、用户名不合法[见用户名规则]
                //return -1;
            }
            //注册成功，添加DB Mapping信息,创建默认组，环信没有分组概念，可以考虑不需要这个概念            

            CustomerIMVO cIMVO = new CustomerIMVO();
            cIMVO.CustomerId = customerId;
            cIMVO.IMId = imUserName;
            cIMVO.IMPWD = imPassword;
            cIMVO.NickName = imNickName;
            cIMVO.Sign = "";
            cIMVO.HeaderLogo = "";
            cIMVO.Status = 0;

            int customerIMId =  cIMDAO.Insert(cIMVO);

            if(customerIMId > 0)
            {
                //添加默认分组
                ICustomerIMGroupDAO cIMGDAO = CustomerManagementDAOFactory.CreateCustomerIMGroupDAO(new UserProfile());
                CustomerIMGroupVO cIMGVO = new CustomerIMGroupVO();
                cIMGVO.CustomerIMId = customerIMId;
                cIMGVO.IMGroupName = DEFAULTIMGROUPNAME;

                cIMGDAO.Insert(cIMGVO);
            }

            return customerIMId;
        }
        /// <summary>
        /// 环信端增加好友
        /// </summary>
        /// <returns></returns>
        public bool AddIMUserFriend(string ownerUserId,string friendUserId)
        {

            //这个功能貌似不需要，在本地做好好友功能就行了。
            string imToken = GetIMToken();
            string url = ConfigInfo.Instance.IMHXURL + ConfigInfo.Instance.IMOrgName + "/" + ConfigInfo.Instance.IMAppName + "/users/" + ownerUserId + "/contacts/users/" + friendUserId;            
            List<string> header = new List<string>();
            header.Add("Bearer " + imToken);
            string result = HttpHelper.HtmlFromUrlPost(url, "", header);
            ResultObject rObj = JsonConvert.DeserializeObject<ResultObject>(result);
            if (rObj != null && rObj.Message != null && rObj.Flag == 0)
            {
                //此IM用户或被添加的好友不存在
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// 获取好友（前提保证服务端和第三方是统一的）
        /// </summary>
        /// <returns></returns>
        public List<RosterViewVO> GetRoster(int customerId)
        {
            IRosterViewDAO rvDAO = CustomerManagementDAOFactory.CreateRosterViewDAO(new UserProfile());
            return rvDAO.FindByParams("OwnerCustomerId = " + customerId);
        }

        /// <summary>
        /// 添加好友，在服务端添加好友，同时在第三方服务器添加好友
        /// </summary>
        /// <returns></returns>
        public bool AddFriend(int ownerCustomerId, int friendCustomerId)
        {
            // 互相添加我好友，各自将对方加入到自己的好友组
            //如果还没有匹配，先注册
            //如果已经是好友则不添加

            ICustomerIMDAO cIMDAO = CustomerManagementDAOFactory.CreateCustomerIMDAO(new UserProfile());
            ICustomerIMGroupDAO cIMGDAO = CustomerManagementDAOFactory.CreateCustomerIMGroupDAO(new UserProfile());
            ICustomerIMGroupUserDAO cIMGUDAO = CustomerManagementDAOFactory.CreateCustomerIMGroupUserDAO(new UserProfile());

            CustomerBO cBO = new CustomerBO(new CustomerProfile());

            int ownerCustomerIMId = GetIMCustomerByCustomer(ownerCustomerId).CustomerIMId;
            int friendCustomerIMId = GetIMCustomerByCustomer(friendCustomerId).CustomerIMId;

            if(ownerCustomerIMId < 1)
            {
                CustomerViewVO cVVO =  cBO.FindById(ownerCustomerId);

                ownerCustomerIMId =  RegisterIMUser(ownerCustomerId, cVVO.CustomerCode, "$" + cVVO.CustomerCode, cVVO.CustomerName);
            }
            if(friendCustomerIMId < 1)
            {
                CustomerViewVO cVVO = cBO.FindById(friendCustomerId);

                friendCustomerIMId = RegisterIMUser(friendCustomerId, cVVO.CustomerCode, "$" + cVVO.CustomerCode, cVVO.CustomerName);
            }


            //Add friend to owner
            List<CustomerIMGroupVO> cimgVOList = cIMGDAO.FindByParams("CustomerIMId = " + ownerCustomerIMId + " and IMGroupName = '" + DEFAULTIMGROUPNAME + "'");
            int ownerIMGroupId = 0;
            if (cimgVOList.Count < 1)
            {
                CustomerIMGroupVO cIMGVO = new CustomerIMGroupVO();
                cIMGVO.CustomerIMId = ownerCustomerIMId;
                cIMGVO.IMGroupName = DEFAULTIMGROUPNAME;

                ownerIMGroupId = cIMGDAO.Insert(cIMGVO);
            }
            else
            {
                ownerIMGroupId = cimgVOList[0].CustomerIMGroupId;
            }
            
            CustomerIMGroupUserVO vo = new CustomerIMGroupUserVO();
            vo.CustomerIMGroupId = ownerIMGroupId;
            vo.CustomerIMId = friendCustomerIMId;

            List<CustomerIMGroupUserVO> cimgList = cIMGUDAO.FindByParams("CustomerIMGroupId = " + ownerIMGroupId + " And CustomerIMId = " + friendCustomerIMId);
            if (cimgList.Count < 1)
                cIMGUDAO.Insert(vo);


            //Add owner to friend
            List<CustomerIMGroupVO> friendVOList = cIMGDAO.FindByParams("CustomerIMId = " + friendCustomerIMId + " and IMGroupName = '" + DEFAULTIMGROUPNAME + "'");
            int friendIMGroupId = 0;
            if (friendVOList.Count < 1)
            {
                CustomerIMGroupVO cIMGVO = new CustomerIMGroupVO();
                cIMGVO.CustomerIMId = friendCustomerIMId;
                cIMGVO.IMGroupName = DEFAULTIMGROUPNAME;

                friendIMGroupId = cIMGDAO.Insert(cIMGVO);
            }
            else
            {
                friendIMGroupId = friendVOList[0].CustomerIMGroupId;
            }

            CustomerIMGroupUserVO friendvo = new CustomerIMGroupUserVO();
            friendvo.CustomerIMGroupId = friendIMGroupId;
            friendvo.CustomerIMId = ownerCustomerIMId;

            cimgList = cIMGUDAO.FindByParams("CustomerIMGroupId = " + friendIMGroupId + " And CustomerIMId = " + ownerCustomerIMId);
            if (cimgList.Count < 1)
                cIMGUDAO.Insert(friendvo);
            
            return true;
        }
        /// <summary>
        /// 删除好友，客户端发送请求，第三方确认后更新服务端
        /// </summary>
        /// <returns></returns>
        public bool DeleteFriend(int customerId, List<int> friendIds)
        {
            ICustomerIMDAO cIMDAO = CustomerManagementDAOFactory.CreateCustomerIMDAO(new UserProfile());
            ICustomerIMGroupDAO cIMGDAO = CustomerManagementDAOFactory.CreateCustomerIMGroupDAO(new UserProfile());
            ICustomerIMGroupUserDAO cIMGUDAO = CustomerManagementDAOFactory.CreateCustomerIMGroupUserDAO(new UserProfile());

            List<CustomerIMVO> imVOList = cIMDAO.FindByParams("CustomerId = " + customerId);
            if (imVOList.Count < 1)
                return false;
            List<CustomerIMGroupVO> cimgVOList = cIMGDAO.FindByParams("CustomerIMId = " + imVOList[0].CustomerIMId + " and IMGroupName = '" + DEFAULTIMGROUPNAME + "'");
            int imgroupId = 0;
            if (cimgVOList.Count < 1)
            {
                CustomerIMGroupVO cIMGVO = new CustomerIMGroupVO();
                cIMGVO.CustomerIMId = imVOList[0].CustomerIMId;
                cIMGVO.IMGroupName = DEFAULTIMGROUPNAME;

                imgroupId = cIMGDAO.Insert(cIMGVO);
            }
            else
            {
                imgroupId = cimgVOList[0].CustomerIMGroupId;
            }

            foreach (int friendid in friendIds)
            {
                cIMGUDAO.DeleteByParams("CustomerIMGroupId = " + imgroupId + " and CustomerIMId = " + friendid);
            }
            return true;
        }

        /// <summary>
        /// 客户端负责发送和接收。服务端负责保存，以便查询消息历史
        /// </summary>
        /// <returns></returns>
        public int AddMessage(IMMessageVO imMessageVO)
        {
            IIMMessageDAO imMDAO = CustomerManagementDAOFactory.CreateIMMessageDAO(new UserProfile());
            return imMDAO.Insert(imMessageVO);
        }

        /// <summary>
        /// 删除消息历史
        /// </summary>
        /// <returns></returns>
        public bool DeleteMessage(List<int> imMessageIds)
        {
            IIMMessageDAO imMDAO = CustomerManagementDAOFactory.CreateIMMessageDAO(new UserProfile());
            foreach (int messageId in imMessageIds)
            {
                imMDAO.DeleteById(messageId);
            }
            return true;
        }
        /// <summary>
        /// 获取消息历史记录
        /// </summary>
        /// <param name="fromId">发送人</param>
        /// <param name="toId">接收人</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public List<IMMessageHistoryVO> GetIMMessage(int fromId,int toId,int pageIndex)
        {
            IIMMessageDAO imMDAO = CustomerManagementDAOFactory.CreateIMMessageDAO(new UserProfile());
            return imMDAO.FindAllByPageIndex("(MessageFrom = " + fromId + " And MessageTo = " + toId + ") or (MessageFrom = " + toId + " And MessageTo = " + fromId + ")", (pageIndex - 1) * 100 + 1, pageIndex * 100, "timestamp", "asc");
        }
        /// <summary>
        /// 根据IM UserId查询 匹配信息
        /// </summary>
        /// <param name="imUserName"></param>
        /// <returns></returns>
        public CustomerIMVO GetIMCustomerByIMUser(string imUserName)
        {
            ICustomerIMDAO cIMDAO = CustomerManagementDAOFactory.CreateCustomerIMDAO(new UserProfile());
            List<CustomerIMVO> voList = cIMDAO.FindByParams("IMId = '" + imUserName + "'");
            if (voList.Count > 0)
            {
                return voList[0];
            }
            else
                return new CustomerIMVO();
        }

        /// <summary>
        /// 根据CustomerId查询 匹配信息
        /// </summary>
        /// <param name="imUserName"></param>
        /// <returns></returns>
        public CustomerIMVO GetIMCustomerByCustomer(int customerId)
        {
            ICustomerIMDAO cIMDAO = CustomerManagementDAOFactory.CreateCustomerIMDAO(new UserProfile());
            List<CustomerIMVO> voList = cIMDAO.FindByParams("CustomerId = '" + customerId + "'");
            if (voList.Count > 0)
            {
                return voList[0];
            }
            else
                return new CustomerIMVO();
        }

        public bool UpdateIMCustomer(CustomerIMVO customerIMVO)
        {
            ICustomerIMDAO cIMDAO = CustomerManagementDAOFactory.CreateCustomerIMDAO(new UserProfile());
            cIMDAO.UpdateById(customerIMVO);
            return true;
        }

        public int AddGroup()
        {
            //待定
            return 0;
        }

        public bool DeleteGroup()
        {
            //待定
            return true;
        }
    }
}
