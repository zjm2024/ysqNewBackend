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
using SPLibrary.CoreFramework.BO;
using System.Runtime.Serialization.Json;
using BroadSky.WeChatAppDecrypt;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;
using System.Configuration;
using static SPLibrary.WebConfigInfo.TencentCloundPicHelper;
using System.Diagnostics;
using SPLibrary.CoreFramework.Logging.BO;
using static System.Net.Mime.MediaTypeNames;
using System.Xml;
using System.Data;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using Newtonsoft.Json.Linq;
using SPLibrary.RequireManagement.BO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.UserManagement.VO;
using SPLibrary.UserManagement.DAO;
using SPLibrary.UserManagement.BO;
using ImportEXCEL;
using System.Globalization;
using SPLibrary.WxEcommerce;
using System.Web.Script.Serialization;
using TencentCloud.Ame.V20190916.Models;
using System.Security.Policy;
using Google.Protobuf.WellKnownTypes;
using TencentCloud.Scf.V20180416.Models;


namespace SPlatformService.Controllers
{
    [RoutePrefix("SPWebAPI/Card")]
    [TokenProjector]
    public class CardController : ApiController
    {
        /// <summary>
        /// 添加或更新名片
        /// </summary>
        /// <param name="CardDataVO">名片VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateCard"), HttpPost]
        public ResultObject UpdateCard([FromBody] CardDataVO CardDataVO, string token)
        {
            if (CardDataVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            if (CardDataVO.Name == "" || CardDataVO.Name == null)
            {
                return new ResultObject() { Flag = 0, Message = "姓名不能为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);

            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            /*审核文本是否合法*/
            if (!cBO.msg_sec_check(CardDataVO))
            {
                return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
            }
            /*审核文本是否合法*/

            if (CardDataVO.CardID > 0)
            {
                CardDataVO cVO = cBO.FindCardById(CardDataVO.CardID);
                if (cVO.CustomerId != customerId)
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }
                CardDataVO.Collection = cVO.Collection;
                CardDataVO.ReadCount = cVO.ReadCount;
                CardDataVO.Forward = cVO.Forward;

                CardDataVO oldcVO = cBO.FindCardById(CardDataVO.CardID);

                if (oldcVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }

                if (CardDataVO.Address != "" && oldcVO.Address != CardDataVO.Address)
                {
                    WeiXinGeocoder Geocoder = cBO.getLatitudeAndLongitude(CardDataVO.Address);
                    if (Geocoder != null)
                    {
                        CardDataVO.latitude = Geocoder.result.location.lat;
                        CardDataVO.longitude = Geocoder.result.location.lng;
                    }
                }
                if (CardDataVO.Headimg == "undefined")
                {
                    CardDataVO.Headimg = "";
                }
                if (CustomerVO.CustomerName == "微信用户")
                {
                    CustomerVO.CustomerName = CardDataVO.Name;
                    CustomerVO.HeaderLogo = CardDataVO.Headimg;
                    CustomerBO.Update(CustomerVO);
                }
                if ((CustomerVO.HeaderLogo == "" || CustomerVO.HeaderLogo == "undefined") && CardDataVO.Headimg != "" && CardDataVO.Headimg != "undefined")
                {
                    CustomerVO.HeaderLogo = CardDataVO.Headimg;
                    CustomerBO.Update(CustomerVO);
                }
                if (cBO.Update(CardDataVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardDataVO.CardID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                CardDataVO.CreatedAt = DateTime.Now;
                CardDataVO.Status = 1;//0:禁用，1:启用
                CardDataVO.CustomerId = customerId;
                CardDataVO.Collection = 0;
                CardDataVO.ReadCount = 0;
                CardDataVO.Forward = 0;

                if (CardDataVO.Address != "")
                {
                    WeiXinGeocoder Geocoder = cBO.getLatitudeAndLongitude(CardDataVO.Address);
                    if (Geocoder != null)
                    {
                        CardDataVO.latitude = Geocoder.result.location.lat;
                        CardDataVO.longitude = Geocoder.result.location.lng;
                    }
                }
                if (CardDataVO.Headimg == "undefined")
                {
                    CardDataVO.Headimg = "";
                }

                if (CustomerVO.CustomerName == "微信用户")
                {
                    CustomerVO.CustomerName = CardDataVO.Name;
                    CustomerVO.HeaderLogo = CardDataVO.Headimg;
                    CustomerBO.Update(CustomerVO);
                }
                if ((CustomerVO.HeaderLogo == "" || CustomerVO.HeaderLogo == "undefined") && CardDataVO.Headimg != "" && CardDataVO.Headimg != "undefined")
                {
                    CustomerVO.HeaderLogo = CardDataVO.Headimg;
                    CustomerBO.Update(CustomerVO);
                }

                int CardId = cBO.AddCard(CardDataVO);
                if (CardId > 0)
                {
                    CardDataVO cVO = new CardDataVO();
                    cVO.CardID = CardId;
                    cBO.Update(cVO);

                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = CardId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新名片马甲
        /// </summary>
        /// <param name="CardDataVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddDummyCard"), HttpPost]
        public ResultObject AddDummyCard([FromBody] CardDataVO CardDataVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (CardDataVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            if (CardDataVO.CardID > 0)
            {
                if (cBO.Update(CardDataVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardDataVO.CardID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                int CardId = cBO.AddDummyCard(CardDataVO);
                if (CardId > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = CardId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /*
        /// <summary>
        /// 批量添加名片马甲
        /// </summary>
        /// <returns></returns>
        [Route("AddDummyCardList"),HttpGet,Anonymous]
        public ResultObject AddDummyCardList()
        {
            
            if (!CompanyBO.isAddDummy)
            {
                CompanyBO.isAddDummy = true;
                CardBO cBO = new CardBO(new CustomerProfile(), 3);
                CompanyBO coBO = new CompanyBO(new CustomerProfile());
                List<SPLibrary.CustomerManagement.VO.CompanyVO> CompanyVO = coBO.FindCompanyUPByCondtion("1=1");

                foreach (SPLibrary.CustomerManagement.VO.CompanyVO item in CompanyVO)
                {
                    CardDataVO CardDataVO = new CardDataVO();
                    CardDataVO.CardID = 0;
                    CardDataVO.Name = item.Contacts;
                    List<myFileInfo> CVO = coBO.GetFileJson(1);
                    if (CVO.Count > 0)
                    {
                        CardDataVO.Headimg = CVO[0].Url;
                    }
                    CardDataVO.Phone = item.Tel;
                    CardDataVO.CorporateName = item.CompanyName;
                    CardDataVO.Details = item.Description.Replace("&nbsp;", "");
                    CardDataVO.Address = item.Address;
                    cBO.AddDummyCard(CardDataVO);
                }

                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = null };
            }
            return new ResultObject() { Flag = 0, Message = "请勿重复!", Result = null };
            
    }
    */
        /// <summary>
        /// 回递名片
        /// </summary>
        /// <param name="CardID">要递的名片ID</param>
        /// <param name="CustomerId">接收人ID</param>
        /// <returns></returns>
        [Route("SendCard"), HttpGet, Anonymous]
        public ResultObject SendCard(int CardID, int CustomerId, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            if (!cBO.isCardSend(CustomerId, CardID))
            {
                CardSendVO sVO = new CardSendVO();
                sVO.CustomerId = CustomerId;
                sVO.CardID = CardID;
                sVO.CreatedAt = DateTime.Now;
                int SendID = cBO.AddCardSend(sVO);
                if (SendID > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "递交名片成功!", Result = SendID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "递交名片失败!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 2, Message = "您已经递过这张名片了!", Result = null };
            }
        }


        /// <summary>
        /// 回递名片
        /// </summary>
        /// <param name="CardID">要递的名片ID</param>
        /// <param name="TCardID">接收人的名片ID</param>
        /// <param name="CustomerId">接收人ID</param>
        /// <param name="FormId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("SendCard"), HttpGet, Anonymous]
        public ResultObject SendCard(int CardID, int TCardID, int CustomerId, string FormId, string code, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardDataVO CVO = cBO.FindCardById(CardID);
            if (CVO == null) { return new ResultObject() { Flag = 0, Message = "递交名片失败!", Result = null }; }
            if (CVO.Name == "微信用户") { return new ResultObject() { Flag = 3, Message = "不能使用“微信用户”等默认名称，请先完善您的名片信息!", Result = null }; }


            if (!cBO.isCardSend(CustomerId, CardID))
            {
                CardSendVO sVO = new CardSendVO();
                sVO.CustomerId = CustomerId;
                sVO.CardID = CardID;
                sVO.TCardID = TCardID;
                sVO.CreatedAt = DateTime.Now;
                sVO.FormId = FormId;
                sVO.OpenId = cBO.getOpenId(code);
                int SendID = cBO.AddCardSend(sVO);
                if (SendID > 0)
                {

                    string goType = "switchTab";
                    if (AppType == 3)
                    {
                        goType = "navigateTo";
                    }
                    cBO.AddCardMessage(CVO.Name + "刚刚给您递交了名片！", CustomerId, "收到名片", "/pages/BusinessCardHolder/BusinessCardHolder?currentTab=1", goType);
                    return new ResultObject() { Flag = 1, Message = "递交名片成功!", Result = SendID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "递交名片失败!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 2, Message = "您已经递过这张名片了!", Result = null };
            }
        }
        /// <summary>
        /// 判断是否回递过名片
        /// </summary>
        /// <param name="CustomerId">接收人ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("isSendCard"), HttpGet]
        public ResultObject isSendCard(int CustomerId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            //获取发送人名片
            List<CardDataVO> uVO = cBO.FindCardByCustomerId(customerId);
            //获取接收人收藏夹
            List<CardDataVO> cVO = cBO.FindCardCollectionByCustomerId(CustomerId);

            bool isSend = false;

            for (int i = 0; i < uVO.Count; i++)
            {
                if (cBO.isCardSend(CustomerId, uVO[i].CardID))
                {
                    isSend = true;
                }
                for (int j = 0; j < cVO.Count; j++)
                {
                    if (cVO[j].CardID == uVO[i].CardID)
                    {
                        isSend = true;
                    }
                }
            }

            return new ResultObject() { Flag = 1, Message = "查询成功!", Result = isSend };
        }

        /// <summary>
        /// 下载递给我的名片
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DownloadSendLists"), HttpGet]
        public ResultObject DownloadSendLists(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);

            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            List<CardSendViewVO> uVO = cBO.FindCardSendByCustomerId(customerId);

            if (uVO != null)
            {
                if (uVO.Count > 0)
                {
                    cBO.setSendReadStatus(customerId);
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "没有名片!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 下载递给我的名片未读数量和需要处理名片组消息数量
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DownloadSendCount"), HttpGet]
        public ResultObject DownloadSendCount(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);

            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            int SendCount = cBO.FindCardSendByCustomerIdCount(customerId);
            ZXTBO zBO = new ZXTBO(cProfile);
            int LeliaoCount = zBO.FindZXTMessageCount("Status = 0 and MessageTo=" + customerId);

            List<CardGroupViewVO> cVO = cBO.FindCardGroupViewByCustomerId(customerId);

            int GroupCount = 0;
            for (int i = 0; i < cVO.Count; i++)
            {
                GroupCount += cVO[i].MessageCount;
            }
            MessageCount mC = new MessageCount();

            mC.SendCount = SendCount;
            mC.GroupCount = GroupCount;
            mC.LeliaoCount = LeliaoCount;

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = mC };
        }

        /// <summary>
        /// 保存收藏
        /// </summary>
        /// <param name="CardDataVO">名片VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SaveCollectionLists"), HttpPost]
        public ResultObject SaveCollectionLists([FromBody] CardDataVO CardDataVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            if (cBO.isCardCollection(customerId, CardDataVO.CardID) == 0)
            {
                if (CardDataVO.CardID > 0)
                {
                    CardCollectionVO cCVO = new CardCollectionVO();
                    cCVO.CardID = CardDataVO.CardID;
                    cCVO.CustomerId = customerId;
                    if (cBO.AddCardCollection(cCVO) > 0)
                    {
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = null };
                    }
                }
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                if (cBO.DeleteCardCollectionByCardID(CardDataVO.CardID, customerId) > 0)
                {
                    return new ResultObject() { Flag = 2, Message = "删除成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
                }
            }
        }

        /// <summary>
        /// 判断是否有收藏
        /// </summary>
        /// <param name="CardID">名片id</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("isCollection"), HttpGet]
        public ResultObject isCollection(int CardID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            if (cBO.isCardCollection(customerId, CardID) == 0)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = false };
            }
            else
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = true };
            }
        }

        /// <summary>
        /// 获取首页数据
        /// </summary>
        /// <returns></returns>
        [Route("GetIndexData"), HttpGet, Anonymous]
        public ResultObject GetIndexData()
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            IndexDataVO IndexDataVO = cBO.FindIndexData();
            if (IndexDataVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = IndexDataVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="CardID">名片ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("delCollectionLists"), HttpPost]
        public ResultObject delCollectionLists(int CardID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            if (cBO.DeleteCardCollectionByCardID(CardID, customerId) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除所有收藏
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("delCollectionLists"), HttpPost]
        public ResultObject delCollectionLists(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            cBO.DeleteCardCollectionByCustomerId(customerId);
            return new ResultObject() { Flag = 2, Message = "删除成功!", Result = null };
        }

        /// <summary>
        /// 下载名片夹
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DownloadCollectionLists"), HttpGet]
        public ResultObject DownloadCollectionLists(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            List<CardDataVO> uVO = cBO.FindCardCollectionByCustomerId(customerId);

            if (uVO != null)
            {
                if (uVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "没有名片!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 批量递交名片夹的名片
        /// </summary>
        /// <param name="CardID"></param>
        /// <param name="ToCustomerId"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SendCollectionLists"), HttpGet]
        public ResultObject SendCollectionLists(string CardID, int ToCustomerId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            try
            {
                if (!string.IsNullOrEmpty(CardID))
                {
                    string[] messageIdArr = CardID.Split(',');
                    bool isAllDelete = true;
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {
                            int cardid = Convert.ToInt32(messageIdArr[i]);
                            if (cBO.isCardCollection(ToCustomerId, cardid) == 0)
                            {
                                CardCollectionVO cCVO = new CardCollectionVO();
                                cCVO.CardID = cardid;
                                cCVO.CustomerId = ToCustomerId;
                                cBO.AddCardCollection(cCVO);
                            }
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "递交名片成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "递交名片成功!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "递交名片失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取可能感兴趣的人
        /// </summary>
        /// <param name="CardID">被访问的名片ID</param>
        /// <returns></returns>
        [Route("getInterestedCard"), HttpGet, Anonymous]
        public ResultObject getInterestedCard(int CardID, int AppType = 1)
        {
            return new ResultObject() { Flag = 0, Message = "接口暂停使用!", Result = null };
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            CardDataVO cVO = cBO.FindCardById(CardID);
            if (cVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
            List<CardDataVO> uVO = cBO.FindCardCollectionByCustomerId(cVO.CustomerId, 1);

            if (uVO != null)
            {
                if (uVO.Count < 5)
                {
                    uVO = cBO.FindAllByPageIndex("(Position like '%经理%' or Position like '%总监%' or Position like '%总裁%' or Position like '%负责人%')", 1, 100, "CreatedAt", "desc");
                }

                if (uVO.Count >= 5)
                {
                    List<CardDataVO> newVO = new List<CardDataVO>();

                    Random random = new Random();
                    List<int> tempList = new List<int>();
                    int temp = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        temp = random.Next(uVO.Count);//将产生的随机数作为被抽list的索引
                        if (!tempList.Contains(temp) && uVO[temp].CustomerId != cVO.CustomerId)
                        {
                            tempList.Add(temp);
                            newVO.Add(uVO[temp]);
                        }
                        else
                        {
                            i--;
                        }
                    }
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newVO };
                }
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }


        /// <summary>
        /// 获取共同人脉列表
        /// </summary>
        /// <param name="CustomerId">对方会员ID</param>
        /// <returns></returns>
        [Route("getTongCard"), HttpGet]
        public ResultObject getTongCard(int CustomerId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            List<CardDataVO> uVO = cBO.FindTongCardByCustomerId(CustomerId, customerId);

            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO, Count = uVO.Count };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }


        /// <summary>
        /// 下载名片夹
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DownloadCollectionLists"), HttpPost]
        public ResultObject DownloadCollectionLists([FromBody] List<CardDataVO> CardDataVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            //获取服务器收藏
            List<CardDataVO> uVO = cBO.FindCardCollectionByCustomerId(customerId);
            try
            {
                if (CardDataVO != null)
                {
                    for (int i = 0; i < CardDataVO.Count; i++)
                    {
                        bool isseva = false;
                        for (int j = 0; j < uVO.Count; j++)
                        {
                            if (CardDataVO[i].CardID == uVO[j].CardID)
                            {
                                //去除本地与服务器重复的名片
                                isseva = true;
                            }
                        }
                        if (!isseva && CardDataVO[i].CardID > 0)
                        {
                            CardCollectionVO cCVO = new CardCollectionVO();
                            cCVO.CardID = CardDataVO[i].CardID;
                            cCVO.CustomerId = customerId;
                            cBO.AddCardCollection(cCVO);
                        }
                    }
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = CardDataVO };
            }
            List<CardDataVO> cVO = cBO.FindCardCollectionByCustomerId(customerId);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
        }

        /// <summary>
        /// 获取名片夹数量
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DownloadCollectionByCount"), HttpGet]
        public ResultObject DownloadCollectionByCount(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            List<CardDataVO> uVO = cBO.FindCardCollectionByCustomerId(customerId);

            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO.Count };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = 0 };
            }
        }

        /// <summary>
        /// 下载名片夹,分页
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DownloadCollectionListsByPageIndex"), HttpPost]
        public ResultObject DownloadCollectionListsByPageIndex([FromBody] CollectionListVO CollectionList, string token)
        {
            if (CollectionList.Condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (CollectionList.Condition.Filter == null || CollectionList.Condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            try
            {
                string conditionStr = "t_CustomerId = " + customerId + " and (" + CollectionList.Condition.Filter.Result() + ")";
                Paging pageInfo = CollectionList.Condition.PageInfo;
                List<CardDataVO> list = cBO.FindCardCollectionAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);

                foreach (CardDataVO item in list)
                {
                    CustomerVO CuVO = CustomerBO.FindCustomenById(item.CustomerId);
                    if (CuVO.isVip && CuVO.ExpirationAt > DateTime.Now)
                    {
                        item.isVip = true;
                    }
                    else
                    {
                        item.isVip = false;
                    }
                }

                int count = cBO.FindCardCollectionAllCount(conditionStr);
                /*
                for (int i = 0; i < list.Count; i++) {
                    list[i].TongCardCount = cBO.FindTongCardByCustomerId(list[i].CustomerId, customerId).Count;
                }
                */
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count, Subsidiary = count };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = CollectionList };
            }

        }

        /// <summary>
        /// 下载我的业绩列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAchievemenList"), HttpPost]
        public ResultObject GetAchievemenList([FromBody] ConditionModel Condition, string token)
        {
            if (Condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (Condition.Filter == null || Condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            try
            {
                string conditionStr = "originCustomerId = " + customerId + " and " + Condition.Filter.Result();
                Paging pageInfo = Condition.PageInfo;
                List<CardAchievemenViewVO> list = cBO.FindAllByPageIndexByAchievemen(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
                int count = cBO.FindAchievemenTotalCount(conditionStr);

                try
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        //一级用户数
                        list[i].FirstlevelUsers = uBO.FindCustomerCount("originCustomerId=" + customerId + " and " + "CreatedAt like '" + list[i].MONTH + "%' and AppType=" + CustomerVO.AppType);
                        List<CustomerVO> cVO = uBO.FindListByParams("originCustomerId=" + customerId + " and " + "CreatedAt like '" + list[i].MONTH + "%' and AppType=" + CustomerVO.AppType);
                        int countNum = 0;
                        for (int j = 0; j < cVO.Count; j++)
                        {
                            List<CardDataVO> cardDataVO = cBO.FindCardByCustomerId(cVO[j].CustomerId);
                            if (cardDataVO.Count > 0)
                            {
                                if (cardDataVO[0].Name != "" && cardDataVO[0].Phone != "" && cardDataVO[0].Position != "" && cardDataVO[0].CorporateName != "")
                                {
                                    countNum++;
                                }
                            }
                        }
                        //一级用户合格数
                        list[i].QualifiedFirstlevelUsersof = countNum;


                        //二级用户数
                        int CustomerCount2 = 0;

                        //二级合格用户数
                        int countNum2 = 0;
                        List<CustomerViewVO> customerVOList = uBO.FindByCondition("originCustomerId=" + customerId + " and AppType=" + CustomerVO.AppType);
                        for (int j = 0; j < customerVOList.Count; j++)
                        {
                            CustomerCount2 += uBO.FindCustomerCount("originCustomerId=" + customerVOList[j].CustomerId + " and " + "CreatedAt like '" + list[i].MONTH + "%'" + " and AppType=" + CustomerVO.AppType);

                            List<CustomerVO> cVO1 = uBO.FindListByParams("originCustomerId=" + customerVOList[j].CustomerId + " and " + "CreatedAt like '" + list[i].MONTH + "%'" + " and AppType=" + CustomerVO.AppType);
                            for (int f = 0; f < cVO1.Count; f++)
                            {
                                List<CardDataVO> cardDataVO = cBO.FindCardByCustomerId(cVO1[f].CustomerId);
                                if (cardDataVO.Count > 0)
                                {
                                    if (cardDataVO[0].Name != "" && cardDataVO[0].Phone != "" && cardDataVO[0].Position != "" && cardDataVO[0].CorporateName != "")
                                    {
                                        countNum2++;
                                    }
                                }
                            }
                        }

                        list[i].SecondaryUsers = CustomerCount2;
                        list[i].QualifiedSecondaryUsers = countNum2;

                        list[i].Reward = (countNum * 1 + countNum2 * 0.1);
                    }
                }
                catch
                {

                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }

        }

        /// <summary>
        /// 获取我邀请来的会员列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <param name="islastmonth">是否加载上月数量</param>
        /// <param name="listType">1:查询全部 2：上一周 3：上一月 4：会员</param>
        /// <returns></returns>
        [Route("GetMyOriginCustomer"), HttpGet]
        public ResultObject GetMyOriginCustomer(int PageCount, int PageIndex, string token, int islastmonth = 1, int listType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());

            string sql = "originCustomerId=" + customerId + " and CustomerId <> " + customerId + " and AppType=" + CustomerVO.AppType;

            if (listType == 2)
            {
                sql += " and YEARWEEK(date_format(CreatedAt,'%Y-%m-%d')) = YEARWEEK(now())-1";
            }
            if (listType == 3)
            {
                sql += " and date_format(CreatedAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 1 MONTH),'%Y-%m')";
            }
            if (listType == 4)
            {
                sql += " and isVip=1";
            }

            List<CustomerVO> list = uBO.FindAllByPageIndex2(sql, (PageIndex - 1) * PageCount + 1, PageIndex * PageCount, "CustomerId", "desc");
            for (int f = 0; f < list.Count; f++)
            {
                List<CardDataVO> cardDataVO = cBO.FindCardByCustomerId(list[f].CustomerId);
                if (cardDataVO.Count > 0)
                {
                    list[f].CustomerName = cardDataVO[0].Name;
                    list[f].HeaderLogo = cardDataVO[0].Headimg;
                    list[f].CardID = cardDataVO[0].CardID;
                    if (cardDataVO[0].Phone != null)
                    {
                        list[f].CardPhone = cardDataVO[0].Phone;
                    }
                    else
                    {
                        list[f].CardPhone = "";
                    }

                }
            }

            //获取数量
            int Count = uBO.GetCustomerCount(sql);

            int lastmonth = 0;
            if (islastmonth == 1)
            {
                //获取上个月数量
                DateTime dt = DateTime.Now;
                lastmonth = uBO.GetCustomerCount("originCustomerId=" + customerId + " and CustomerId <> " + customerId + " and AppType=" + CustomerVO.AppType + "  AND DATE_FORMAT(CreatedAt,'%y-%m-%d')>=DATE_FORMAT('" + dt.AddMonths(-1).Year + "-" + dt.AddMonths(-1).Month + "-01','%y-%m-%d') AND DATE_FORMAT(CreatedAt, '%y-%m-%d') < DATE_FORMAT('" + dt.Year + "-" + dt.Month + "-01', '%y-%m-%d')");
            }

            if (list != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = Count, Subsidiary = lastmonth };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 取消体验会员
        /// </summary>
        /// <param name="ParisOntrial">体验会员标记</param>
        /// <returns></returns>
        [Route("CancellationVip"), HttpGet, Anonymous]
        public ResultObject CancellationVip(int ParisOntrial)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            try
            {
                //试用会员用户集合
                List<CustomerVO> cVO = uBO.FindListByParams("isOntrial=" + ParisOntrial);
                bool isAllDelete = true;
                for (int j = 0; j < cVO.Count; j++)
                {

                    try
                    {
                        cVO[j].isOntrial = false;
                        if (cVO[j].ontrialtime > 0)
                        {
                            cVO[j].ExpirationAt = DateTime.Now.AddDays(cVO[j].ontrialtime);
                            cVO[j].ontrialtime = 0;
                        }
                        else
                        {
                            cVO[j].ExpirationAt = DateTime.Now;
                            cVO[j].VipLevel = 0;
                            cVO[j].isVip = false;
                        }
                        uBO.Update(cVO[j]);
                    }
                    catch
                    {
                        isAllDelete = false;
                    }

                }

                if (isAllDelete)
                {
                    if (cVO.Count > 0)
                    {
                        return new ResultObject() { Flag = 1, Message = "操作成功!体验会员已经取消体验资格", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "没有体验会员, 不需要操作", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 1, Message = "部分操作成功!", Result = null };
                }

            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
            }
        }

        /// <summary>
        /// 领取优惠券
        /// </summary>
        /// <param name="ParcustomerId">客户id</param>
        /// <returns></returns>
        [Route("ToCoupon"), HttpGet, Anonymous]
        public ResultObject ToCoupon(int ParcustomerId)
        {
            if (ParcustomerId < 1)
            {
                return new ResultObject() { Flag = 0, Message = "客户ID为空!", Result = null };
            }

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(ParcustomerId);

            if (CustomerVO.CouponAt > DateTime.Now && CustomerVO.Couponcost > 0)
            {
                return new ResultObject() { Flag = 0, Message = "已有优惠券！不再领取", Result = null };
            }


            if (CustomerVO != null)
            {
                Random random = new Random();
                int randomNumber = random.Next(1, 10);
                CustomerVO.Couponcost = randomNumber;
                CustomerVO.CouponAt = DateTime.Now.AddDays(30);
                CustomerBO.Update(CustomerVO);
                return new ResultObject() { Flag = 1, Message = "领取成功!", Result = null };


            }

            return new ResultObject() { Flag = 0, Message = "领取失败!", Result = null };
        }


        /// <summary>
        /// 领取体验会员
        /// </summary>
        /// <param name="ParcustomerId">客户id</param>
        /// <returns></returns>
        [Route("VipOntrial"), HttpGet, Anonymous]
        public ResultObject VipOntrial(int ParcustomerId)
        {
            if (ParcustomerId < 1)
            {
                return new ResultObject() { Flag = 0, Message = "客户ID为空!", Result = null };
            }

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(ParcustomerId);

            if (CustomerVO.isVip && CustomerVO.ExpirationAt > DateTime.Now && (CustomerVO.VipLevel == 2 || CustomerVO.VipLevel == 3))
            {
                return new ResultObject() { Flag = 0, Message = "您已经是合伙人或分公司Vip，暂时不需要领取！", Result = null };
            }


            if (CustomerVO != null && !CustomerVO.isOntrial)
            {
                if (CustomerVO.ExpirationAt != null)
                {
                    System.TimeSpan timelent = CustomerVO.ExpirationAt - DateTime.Now;  //两个时间相减 。默认得到的是 两个时间之间的天数
                    Int32 getDay = timelent.Days; //将这个天数转换成天数, 返回值是double类型的（其实不必转换，因为t3默认就是天数）
                    if (getDay > 0)
                    {
                        CustomerVO.ontrialtime = getDay;
                        CustomerVO.ExpirationAt = CustomerVO.ExpirationAt.AddDays(365);
                    }
                    else
                    {
                        CustomerVO.ExpirationAt = DateTime.Now.AddDays(365);
                    }

                    CustomerVO.isOntrial = true;
                    CustomerVO.VipLevel = 1;
                    CustomerVO.isVip = true;
                    CustomerBO.Update(CustomerVO);
                    return new ResultObject() { Flag = 1, Message = "领取成功!", Result = null };

                }
                else
                {

                    CustomerVO.ExpirationAt = DateTime.Now.AddDays(365);
                    CustomerVO.VipLevel = 1;
                    CustomerVO.isOntrial = true;
                    CustomerVO.isVip = true;
                    CustomerBO.Update(CustomerVO);
                    return new ResultObject() { Flag = 1, Message = "领取成功!", Result = null };
                }

            }

            return new ResultObject() { Flag = 0, Message = "领取失败 您如果已经领取过了 就无法再次领取!", Result = null };
        }

        /// <summary>
        /// 获取我的名片列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyCardlist"), HttpGet]
        public ResultObject getMyCardlist(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            List<CardDataVO> uVO = cBO.FindCardByCustomerId(customerId);
            if (uVO != null)
            {
                if (uVO.Count > 0)
                {
                    foreach (CardDataVO item in uVO)
                    {
                        if (CustomerVO.isVip && CustomerVO.ExpirationAt > DateTime.Now)
                        {
                            item.isVip = true;
                        }
                        else
                        {
                            item.isVip = false;
                        }

                    }

                    //获取最新消息提醒
                    List<CardMessageVO> CardMessageVO = cBO.FindCardMessageByCondtion("CustomerId = " + customerId + " ORDER BY CreatedAt desc limit 5");
                    int CardMessageCount = cBO.FindCardMessageCount("CustomerId = " + customerId + " and Status=0");

                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO, Subsidiary = CardMessageVO, Count = CardMessageCount };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "没有名片!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我的公共名片列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyPublicCardlist"), HttpGet]
        public ResultObject getMyPublicCardlist(int PageCount, int PageIndex, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            List<CardDataVO> uVO = cBO.FindCardByPublic(customerId);
            if (uVO != null)
            {
                if (uVO.Count > 0)
                {
                    uVO.Reverse();
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = uVO.Count };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "没有名片!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 根据CustomerId获取名片列表
        /// </summary>
        /// <param name="CustomerId">CustomerId</param>
        /// <returns></returns>
        [Route("getCardlistByCid"), HttpGet, Anonymous]
        public ResultObject getCardlistByCid(int CustomerId, int AppType = 1)
        {
            // UserProfile uProfile = CacheManager.GetUserProfile(token);
            // CustomerProfile cProfile = uProfile as CustomerProfile;
            //  int customerId = cProfile.CustomerId;


            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            List<CardDataVO> uVO = cBO.FindCardByCustomerId(CustomerId);

            if (uVO != null)
            {
                if (uVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "没有名片!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 设置默认名片
        /// </summary>
        /// <param name="CardID">名片ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SetDefaultCard"), HttpGet]
        public ResultObject SetDefaultCard(int CardID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            CardDataVO uVO = cBO.FindCardById(CardID);
            if (uVO != null)
            {
                if (uVO.CustomerId != customerId)
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限设置该名片!", Result = null };
                }
                try
                {
                    //先将其他名片取消默认
                    List<CardDataVO> lVO = cBO.FindCardByCustomerId(customerId);
                    for (int i = 0; i < lVO.Count; i++)
                    {
                        if (uVO.CardID == lVO[i].CardID)
                        {
                            CardDataVO cVO = new CardDataVO();
                            cVO.CardID = lVO[i].CardID;
                            cVO.DefaultCard = 1;
                            cBO.Update(cVO);
                        }
                        else
                        {
                            CardDataVO cVO = new CardDataVO();
                            cVO.CardID = lVO[i].CardID;
                            cVO.DefaultCard = 0;
                            cBO.Update(cVO);
                        }
                    }
                }
                catch
                {
                    return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };
                }

                return new ResultObject() { Flag = 1, Message = "设置成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "该名片已被删除，无法更新设置!", Result = null };
            }

        }

        /// <summary>
        /// 获取名片详情，匿名
        /// </summary>
        /// <param name="CardID">名片ID</param>
        /// <returns></returns>
        [Route("GetCardSite"), HttpGet, Anonymous]
        public ResultObject GetCardSite(int CardID, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());

            //如果CardID为负,识别为会员id,并返回默认名片
            if (CardID < 0)
            {
                List<CardDataVO> cardList = cBO.FindCardByCustomerId(0 - CardID);
                if (cardList.Count > 0)
                {
                    CardID = cardList[0].CardID;
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "他还未创建名片，无法获取内容", Result = null };
                }
            }

            CardDataVO uVO = cBO.FindCardById(CardID);
            if (uVO != null)
            {
                if (uVO.CardImg == "")
                {
                    uVO.CardImg = cBO.GetCardQR(CardID);
                }
                CustomerVO CustomerVO = CustomerBO.FindCustomenById(uVO.CustomerId);
                if (CustomerVO.isVip && CustomerVO.ExpirationAt > DateTime.Now)
                {
                    uVO.isVip = true;
                }

                List<CardDemandViewVO> list = new List<CardDemandViewVO>();

                if (uVO.isPublic == 0)
                {
                    string conditionStr = "Status <> 0 and Status <> 2 and (TO_DAYS(EffectiveEndDate) - TO_DAYS(now()) >=0 or isEndDate=0) and CustomerId=" + uVO.CustomerId;
                    DemandBO uBO = new DemandBO(new CustomerProfile());
                    list = uBO.FindCardDemandViewAllByPageIndex(conditionStr, 1, 10, "CreatedAt", "desc");
                }


                string Condtion = "Type='ReadCard' and ById=" + CardID + " and CustomerId<>" + uVO.CustomerId;
                List<CardAccessRecordsVO> aVO = cBO.FindAccessrecordsByCondtion(Condtion, 5);
                List<CardDataVO> cVO = new List<CardDataVO>();
                aVO.Reverse();

                for (int i = 0; i < aVO.Count; i++)
                {
                    CardDataVO CardDataVO = new CardDataVO();
                    List<CardDataVO> clVO = cBO.FindCardByCustomerId(aVO[i].CustomerId);

                    if (clVO.Count > 0)
                    {
                        CardDataVO = clVO[0];
                        CardDataVO.CreatedAt = aVO[i].AccessAt;
                        cVO.Add(CardDataVO);
                    }
                    else
                    {
                        if (aVO[i].CustomerId > 0)
                        {
                            CustomerVO cuVO = CustomerBO.FindCustomenById(aVO[i].CustomerId);
                            if (uVO != null)
                            {
                                CardDataVO.CustomerId = uVO.CustomerId;
                                CardDataVO.Name = cuVO.CustomerName;
                                CardDataVO.Headimg = cuVO.HeaderLogo;
                                CardDataVO.CreatedAt = aVO[i].AccessAt;
                                cVO.Add(CardDataVO);
                            }
                        }
                    }
                }

                List<CardPartyVO> pVO = cBO.FindPartyByCId(CustomerVO.CustomerId);
                if (pVO != null)
                {
                    pVO.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
                    pVO.Reverse();
                }
                int PageIndex = 1;
                int PageCount = 5;

                string conditionStr2 = " Status  > 0  and Status  <> 2  and CustomerId = " + CustomerVO.CustomerId + " ";
                List<CardSoftArticleVO> sVO = cBO.FindSoftArticleAllByPageIndex(conditionStr2, (PageIndex - 1) * PageCount + 1, PageIndex * PageCount, "CreatedAt", "desc");

                List<InfoListVO> InfoListVO = new List<InfoListVO>();
                for (int i = 0; i < pVO.Count; i++)
                {
                    InfoListVO iVO = new InfoListVO();
                    iVO.ID = pVO[i].PartyID;
                    iVO.Title = pVO[i].Title;
                    iVO.Images = pVO[i].MainImg;
                    iVO.CreatedAt = pVO[i].CreatedAt;
                    iVO.StartTime = pVO[i].StartTime;
                    iVO.SignUpTime = pVO[i].SignUpTime;
                    iVO.EndTime = pVO[i].EndTime;
                    iVO.Content = pVO[i].Content;
                    iVO.Type = "Party";
                    iVO.PartyType = pVO[i].Type;
                    InfoListVO.Add(iVO);
                }

                for (int i = 0; i < sVO.Count; i++)
                {
                    InfoListVO iVO = new InfoListVO();
                    iVO.ID = sVO[i].SoftArticleID;
                    iVO.Title = sVO[i].Title;
                    iVO.Images = sVO[i].Image;
                    iVO.CreatedAt = sVO[i].CreatedAt;
                    iVO.Content = sVO[i].Description;
                    iVO.Type = "SoftArticle";
                    InfoListVO.Add(iVO);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    InfoListVO iVO = new InfoListVO();
                    iVO.ID = list[i].DemandId;
                    iVO.Title = list[i].Title;
                    iVO.Images = list[i].Image;
                    iVO.CreatedAt = list[i].CreatedAt;
                    iVO.Content = list[i].Description;
                    iVO.Type = "Demand";
                    InfoListVO.Add(iVO);
                }

                InfoListVO.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));
                InfoListVO.Reverse();


                BusinessCardBO BusinessCardBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO PersonalVO = BusinessCardBO.FindPersonalByCustomerId(CustomerVO.CustomerId);
                BusinessCardVO bVO = new BusinessCardVO();
                if (PersonalVO != null)
                {
                    if (PersonalVO.BusinessID != 0)
                    {
                        bVO = BusinessCardBO.FindBusinessCardById(PersonalVO.BusinessID);
                        //清除营业执照等保密信息

                        if (bVO != null)
                        {
                            bVO.BusinessLicenseImg = "";
                        }
                    }
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO, Subsidiary = list, Subsidiary2 = new { AccessList = cVO, InfoList = InfoListVO.Skip((PageIndex - 1) * PageCount).Take(PageCount), BusinessCard = bVO } };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "他还未创建名片，无法获取内容", Result = null };
            }
        }

        /// <summary>
        /// 删除名片
        /// </summary>
        /// <param name="CardID">名片ID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("DelCard"), HttpGet]
        public ResultObject DelCard(int CardID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            List<CardDataVO> cvoList = cBO.FindCardByCustomerId(customerId);
            if (cvoList.Count <= 1) { return new ResultObject() { Flag = 0, Message = "您必须保留最后一张名片！", Result = null }; }

            CardDataVO uVO = cBO.FindCardById(CardID);

            if (uVO.CustomerId != customerId)
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限删除该名片!", Result = null };
            }
            if (cBO.DeleteById(CardID) > 0)
            {
                //cBO.DeleteCardSendByCardID(CardID);
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 改变名片的转发，收藏，浏览
        /// </summary>
        /// <param name="CardID">名片ID</param>
        /// <param name="field">操作字段</param>
        /// <param name="sum">数量</param>
        /// <returns></returns>
        [Route("editCardReadData"), HttpGet, Anonymous]
        public ResultObject editCardReadData(int CardID, string field, int sum, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            CardDataVO cVO = cBO.FindCardById(CardID);
            if (field == "Collection")
            {
                cVO.Collection += sum;
                if (cVO.Collection < 0)
                    cVO.Collection = 0;
            }
            if (field == "ReadCount")
            {
                cVO.ReadCount += sum;
                if (cVO.ReadCount < 0)
                    cVO.ReadCount = 0;
            }
            if (field == "Forward")
            {
                cVO.Forward += sum;
                if (cVO.Forward < 0)
                    cVO.Forward = 0;
            }
            if (cBO.Update(cVO))
            {
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

        /// <summary>
        /// 手机解密并保存
        /// </summary>
        /// <param name="wxPhoneVO">手机VO</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetPhoneCard"), HttpPost, Anonymous]
        public ResultObject GetPhoneCard([FromBody] wxPhoneVO wxPhoneVO, string code, string token = "", int type = 0, int AppType = 1)
        {
            try
            {
                CardBO CardBO = new CardBO(new CustomerProfile(), AppType);
                //乐聊名片
                string appid = CardBO.appid;
                string secret = CardBO.secret;

                if (type == 1)
                {
                    //企业名片
                    appid = AppBO.GetApp(0).AppId;
                    secret = AppBO.GetApp(0).Secret;
                }

                if (type == 2)
                {
                    //众销乐小程序
                    appid = "wxd90e86e2ec343eae";
                    secret = "58ce21e763870071474c8ee531013e7e";
                }

                if (type == 3)
                {
                    //引流王
                    appid = AppBO.GetApp(2).AppId;
                    secret = AppBO.GetApp(2).Secret;
                }

                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);

                WeChatAppDecrypt un = new WeChatAppDecrypt(appid, secret);
                WechatPhoneData wui = un.DecryptByPhone(wxPhoneVO.encryptedData, wxPhoneVO.iv, readConfig.session_key);

                if (token != "")
                {
                    UserProfile uProfile = CacheManager.GetUserProfile(token);
                    CustomerProfile cProfile = uProfile as CustomerProfile;
                    int customerId = cProfile.CustomerId;

                    CustomerBO cBO = new CustomerBO(new CustomerProfile());

                    if (cBO.FindById(customerId).Phone == "")
                    {
                        CustomerVO cVO = new CustomerVO();
                        cVO.CustomerId = customerId;
                        cVO.Phone = wui.phoneNumber;
                        cVO.CustomerAccount = wui.phoneNumber;
                        try
                        {
                            if (AppType == 3)
                            {
                                GetCityVO GetCityVO = CsharpTest_GetCity.Main(wui.phoneNumber);
                                if (GetCityVO.code == 200)
                                {
                                    cVO.City = GetCityVO.data.city;
                                    cVO.Prov = GetCityVO.data.prov;
                                }
                            }
                        }
                        catch
                        {

                        }
                        cBO.Update(cVO);
                    }
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = wui.phoneNumber };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败,请重试!", Result = null };
            }
        }

        /// <summary>
        /// 获取名片二维码
        /// </summary>
        /// <param name="CardID">分享路径</param>
        /// <returns></returns>
        [Route("GetCardQR"), HttpGet, Anonymous]
        public ResultObject GetCardQR(int CardID, int AppType = 1)
        {
            try
            {
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);
                string str = cBO.GetCardQR(CardID);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = str };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取名片海报
        /// </summary>
        /// <param name="CardID">分享路径</param>
        /// <returns></returns>
        [Route("GetCardPoster"), HttpGet, Anonymous]
        public ResultObject GetCardPoster(int CardID, int isRestart = 0, int Style = 0, int AppType = 1)
        {
            try
            {
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);
                CardDataVO cVO = cBO.FindCardById(CardID);
                if (cVO.CardImg == "" || isRestart == 1)
                    cVO.CardImg = cBO.GetCardQR(CardID);
                string str = cBO.getPosterByCardID(CardID, Style);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = str };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取名片海报(可更换背景)
        /// </summary>
        /// <param name="CardID">分享路径</param>
        /// <returns></returns>
        [Route("GetCardPosterByBack"), HttpGet, Anonymous]
        public ResultObject GetCardPosterByBack(int CardID, int isRestart = 0, string Posterback = "", int AppType = 1)
        {
            try
            {
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);
                CardDataVO cVO = cBO.FindCardById(CardID);
                if (cVO.CardImg == "" || isRestart == 1)
                    cVO.CardImg = cBO.GetCardQR(CardID);
                string str = cBO.getPosterByCardID(CardID, Posterback, HttpContext.Current.Request.UserHostAddress);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = str };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新名片组
        /// </summary>
        /// <param name="CardGroupVO">名片VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateCardGroup"), HttpPost]
        public ResultObject UpdateCardGroup([FromBody] CardGroupVO CardGroupVO, string token, int AppType = 1)
        {
            if (CardGroupVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);

            if (AppType == 1)
            {
                AppType = CustomerVO.AppType;
            }
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            //判断数量限制
            if (!CustomerVO.isVip)
            {
                int Count = cBO.FindGroupViewTotalCount("CustomerId = " + customerId + " and HCustomerId  = " + customerId + " and Status<>0");
                if (Count >= 20)
                {
                    return new ResultObject() { Flag = 0, Message = "您已创建超过20个名片群，无法继续创建，建议升级为VIP", Result = null };
                }
            }


            /*审核文本是否合法*/
            if (!cBO.msg_sec_check(CardGroupVO))
            {
                return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
            }
            /*审核文本是否合法*/

            if (CardGroupVO.GroupID > 0)
            {
                List<CardGroupCardViewVO> admincVO = cBO.isJionCardGroup(customerId, CardGroupVO.GroupID);
                if (admincVO.Count > 0)
                {
                    if (admincVO[0].Status != 3)
                    {
                        return new ResultObject() { Flag = 0, Message = "你没有权限修改名片组!", Result = null };
                    }
                    if (cBO.UpdateCardGroup(CardGroupVO))
                    {
                        CardGroupVO.QRImg = cBO.getQRIMGByIDAndType(CardGroupVO.GroupID, 2);
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardGroupVO.GroupID, Count = CardGroupVO };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限修改名片组!", Result = null };
                }

            }
            else
            {
                CardGroupVO.CreatedAt = DateTime.Now;
                CardGroupVO.CustomerId = customerId;

                List<CardDataVO> cVO = cBO.FindCardByCustomerId(customerId);
                if (cVO.Count <= 0)
                {
                    return new ResultObject() { Flag = 2, Message = "请先创建名片!", Result = null };
                }

                int GroupID = cBO.AddCardGroup(CardGroupVO);
                if (GroupID > 0)
                {
                    //将创建者作为管理员加入名片组
                    CardGroupCardVO cgcVO = new CardGroupCardVO();
                    cgcVO.CustomerId = customerId;
                    cgcVO.GroupID = GroupID;
                    cgcVO.Status = 3;
                    cgcVO.CreatedAt = DateTime.Now;
                    cgcVO.CardID = cVO[0].CardID;
                    cBO.AddCardToGroup(cgcVO);

                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = GroupID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新名片组
        /// </summary>
        /// <param name="CardGroupVO">名片VO</param>
        /// <param name="CardID">需要添加进组的名片</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateCardGroup"), HttpPost]
        public ResultObject UpdateCardGroup([FromBody] CardGroupVO CardGroupVO, string CardID, string token)
        {
            if (CardGroupVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            //判断数量限制
            if (!CustomerVO.isVip)
            {
                int Count = cBO.FindGroupViewTotalCount("CustomerId = " + customerId + " and HCustomerId  = " + customerId + " and Status<>0");
                if (Count >= 20)
                {
                    return new ResultObject() { Flag = 0, Message = "您已创建超过20个名片群，无法继续创建，建议升级为VIP", Result = null };
                }
            }


            /*审核文本是否合法*/
            if (!cBO.msg_sec_check(CardGroupVO))
            {
                return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
            }
            /*审核文本是否合法*/


            if (CardGroupVO.GroupID > 0)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
            else
            {
                CardGroupVO.CreatedAt = DateTime.Now;
                CardGroupVO.CustomerId = customerId;

                List<CardDataVO> cVO = cBO.FindCardByCustomerId(customerId);
                if (cVO.Count <= 0)
                {
                    return new ResultObject() { Flag = 2, Message = "请先创建名片!", Result = null };
                }

                int GroupID = cBO.AddCardGroup(CardGroupVO);
                if (GroupID > 0)
                {
                    //将创建者作为管理员加入名片组
                    CardGroupCardVO cgcVO = new CardGroupCardVO();
                    cgcVO.CustomerId = customerId;
                    cgcVO.GroupID = GroupID;
                    cgcVO.Status = 3;
                    cgcVO.CreatedAt = DateTime.Now;
                    cgcVO.CardID = cVO[0].CardID;
                    cBO.AddCardToGroup(cgcVO);
                    try
                    {
                        AddGroupCard(CardID, GroupID, token);
                    }
                    catch
                    {

                    }
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = GroupID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我的名片组
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyGrouplist"), HttpGet]
        public ResultObject getMyGrouplist(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;


            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            List<CardGroupViewVO> uVO = cBO.FindCardGroupViewByCustomerId(customerId);

            if (uVO != null)
            {

                if (uVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "没有加入名片组!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我在名片组的名片
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyGroupCard"), HttpGet]
        public ResultObject getMyGroupCard(int GroupID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            CardGroupCardViewVO uVO = cBO.FindCardGroupCardViewByGroupID(GroupID, customerId);

            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 判断我是否在名片组里
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyCardByGroup"), HttpGet]
        public ResultObject getMyCardByGroup(int GroupID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            CardGroupCardViewVO uVO = cBO.FindCardGroupCardViewByGroupID(GroupID, customerId);

            if (uVO != null)
            {
                if (uVO.Status == 0)
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取名片组所有名片
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getGroupCardlist"), HttpGet]
        public ResultObject getGroupCardlist(int GroupID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            List<CardGroupCardViewVO> uVO = cBO.FindCardGroupCardViewByGroupID(GroupID);

            if (uVO != null)
            {
                if (uVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无名片!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取名片组所有名片，匿名
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        [Route("getGroupCardlist"), HttpGet, Anonymous]
        public ResultObject getGroupCardlist(int GroupID, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            List<CardGroupCardViewVO> uVO = cBO.FindCardGroupCardViewByGroupID(GroupID);

            if (uVO != null)
            {
                if (uVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无名片!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取名片组详情，匿名
        /// </summary>
        /// <param name="GroupID">名片组ID</param>
        /// <returns></returns>
        [Route("GetGroupCardSite"), HttpGet, Anonymous]
        public ResultObject GetGroupCardSite(int GroupID, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardGroupVO uVO = cBO.FindCardGroupById(GroupID);

            if (uVO != null)
            {
                GroupVO gVO = new GroupVO();
                gVO.CardGroupVO = uVO;
                if (uVO.CardImg == "")
                {
                    gVO.CardGroupVO.CardImg = cBO.GetCardGroupQR(GroupID);
                }
                gVO.NumberOfPeople = cBO.FindTotalCount("GroupID = " + GroupID + " and Status<>0");

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = gVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取通过分享创建名片组的详情,匿名
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetGroupCardSiteByShare"), HttpGet]
        public ResultObject GetGroupCardSiteByShare(string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);

            if (AppType == 1)
            {
                AppType = CustomerVO.AppType;
            }
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            //判断数量限制
            /*
            if (!CustomerVO.isVip)
            {
                int Count = cBO.FindGroupViewTotalCount("CustomerId = " + customerId + " and HCustomerId  = " + customerId + " and Status<>0");
                if (Count >= 20)
                {
                    return new ResultObject() { Flag = 0, Message = "您已创建超过20个名片群，无法继续创建，建议升级为VIP", Result = null };
                }
            }*/


            CardGroupVO CardGroupVO = new CardGroupVO();

            CardGroupVO.CreatedAt = DateTime.Now;
            CardGroupVO.CustomerId = customerId;

            List<CardDataVO> cVO = cBO.FindCardByCustomerId(customerId);
            if (cVO.Count <= 0)
            {
                return new ResultObject() { Flag = 2, Message = "请先创建名片!", Result = null };
            }
            CardGroupVO.JoinSetUp = 0;
            CardGroupVO.GroupName = cVO[0].Name + "的名片群";


            int GroupID = cBO.AddCardGroup(CardGroupVO);
            if (GroupID > 0)
            {
                //将创建者作为管理员加入名片组
                CardGroupCardVO cgcVO = new CardGroupCardVO();
                cgcVO.CustomerId = customerId;
                cgcVO.GroupID = GroupID;
                cgcVO.Status = 3;
                cgcVO.CreatedAt = DateTime.Now;
                cgcVO.CardID = cVO[0].CardID;
                cBO.AddCardToGroup(cgcVO);

                CardGroupVO uVO = cBO.FindCardGroupById(GroupID);
                if (uVO.CardImg == "")
                {
                    uVO.CardImg = cBO.GetCardGroupQR(GroupID);
                }
                if (uVO.QRImg == "")
                {
                    uVO.QRImg = cBO.getQRIMGByIDAndType(GroupID, 2);
                }
                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = uVO };
            }
            else
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
        }

        /// <summary>
        /// 申请加入名片组
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="CardID"></param>
        /// <param name="FormId"></param>
        /// <param name="code"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("JoinCardGroup"), HttpGet]
        public ResultObject JoinCardGroup(int GroupID, int CardID, string FormId, string code, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CardDataVO CardDataVO = cBO.FindCardById(CardID);

            List<CardGroupCardViewVO> cVO = cBO.isJionCardGroup(customerId, GroupID);
            if (cVO.Count > 0)
            {
                if (cVO[0].Status == 0)
                    return new ResultObject() { Flag = 3, Message = "您已经申请过该名片组，请等待审核!", Result = GroupID };
                else
                    return new ResultObject() { Flag = 4, Message = "您已经是该名片组成员!", Result = GroupID };
            }

            CardGroupVO uVO = cBO.FindCardGroupById(GroupID);
            CustomerVO CustomerVO = uBO.FindCustomenById(uVO.CustomerId);
            int NumberOfPeople = cBO.FindTotalCount("GroupID = " + GroupID + " and Status<>0");
            if (NumberOfPeople >= 1000)
            {
                return new ResultObject() { Flag = 0, Message = "名片群人数已满，无法加入!", Result = null };
            }

            if (CardDataVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "请重新选择名片!", Result = null };
            }
            if (CardDataVO.Name == "微信用户")
            {
                return new ResultObject() { Flag = 5, Message = "不能使用“微信用户”等默认名称，请先完善您的名片信息!", Result = null };
            }


            CardGroupVO gVO = cBO.FindCardGroupById(GroupID);

            CardGroupCardVO cgcVO = new CardGroupCardVO();
            cgcVO.CustomerId = customerId;
            cgcVO.GroupID = GroupID;
            cgcVO.FormId = FormId;
            cgcVO.OpenId = cBO.getOpenId(code);

            if (gVO.JoinSetUp == 0)
            {
                cgcVO.Status = 1;
            }
            else
            {
                cgcVO.Status = 0;
            }

            cgcVO.CreatedAt = DateTime.Now;
            cgcVO.CardID = CardID;
            int GroupCardID = cBO.AddCardToGroup(cgcVO);

            if (GroupCardID > 0)
            {
                if (gVO.JoinSetUp == 0)
                {
                    return new ResultObject() { Flag = 1, Message = "加入成功!", Result = GroupID };
                }
                else
                {
                    try
                    {

                        List<CardGroupCardViewVO> GroupCard = cBO.FindCardGroupAdminByGroupID(gVO.GroupID);
                        for (int i = 0; i < GroupCard.Count; i++)
                        {
                            cBO.AddCardMessage(CardDataVO.Name + "申请加入" + gVO.GroupName, GroupCard[i].CustomerId, "进群申请", "/package/package_group/CardGroupDetail/CardGroupDetail?GroupID=" + gVO.GroupID);
                        }
                    }
                    catch
                    {

                    }
                    return new ResultObject() { Flag = 2, Message = "申请成功，请等待管理员审批!", Result = GroupID };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "操作失败，请重试!", Result = null };
            }
        }

        /// <summary>
        /// 同意或拒绝加入名片组
        /// </summary>
        /// <param name="GroupCardID"></param>
        /// <param name="Status"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ChangeGroupCardStatus"), HttpGet]
        public ResultObject ChangeGroupCardStatus(int GroupCardID, int Status, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            CardGroupCardViewVO cVO = cBO.FindCardGroupCardByGroupCardID(GroupCardID);
            if (cVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "操作失败，请退出重试!", Result = null };
            }
            List<CardGroupCardViewVO> admincVO = cBO.isJionCardGroup(customerId, cVO.GroupID);

            if (admincVO.Count > 0)
            {
                if (admincVO[0].Status != 3)
                {
                    return new ResultObject() { Flag = 0, Message = "需要管理员才能审核!", Result = null };
                }
                else
                {
                    if (Status > 0)
                    {
                        CardGroupCardVO VO = new CardGroupCardVO();
                        VO.GroupCardID = GroupCardID;
                        VO.Status = Status;
                        if (cBO.UpdateCardToGroup(VO))
                        {
                            cBO.sendAllowMessage(GroupCardID);
                            return new ResultObject() { Flag = 1, Message = "操作成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "操作失败，请退出重试!", Result = null };
                        }
                    }
                    else
                    {
                        cBO.sendRefuseMessage(GroupCardID);
                        cBO.DeleteCardToGroupByGroupID(cVO.GroupID, cVO.CustomerId);
                        return new ResultObject() { Flag = 1, Message = "已拒绝加入!", Result = null };
                    }
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "需要管理员才能审核!", Result = null };
            }
        }

        /// <summary>
        /// 批量同意或拒绝加入名片组
        /// </summary>
        /// <param name="GroupCardID"></param>
        /// <param name="GroupID"></param>
        /// <param name="Status"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ExamineAnyGroupCard"), HttpGet]
        public ResultObject ExamineAnyGroupCard(string GroupCardID, int GroupID, int Status, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            List<CardGroupCardViewVO> admincVO = cBO.isJionCardGroup(customerId, GroupID);

            if (admincVO.Count > 0)
            {
                if (admincVO[0].Status != 3)
                {
                    return new ResultObject() { Flag = 0, Message = "需要管理员才审核成员!", Result = null };
                }
                else
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(GroupCardID))
                        {
                            string[] messageIdArr = GroupCardID.Split(',');
                            bool isAllDelete = true;



                            for (int i = 0; i < messageIdArr.Length; i++)
                            {
                                try
                                {
                                    CardGroupCardVO aVO = new CardGroupCardVO();
                                    aVO.Status = Status;
                                    aVO.GroupCardID = Convert.ToInt32(messageIdArr[i]);
                                    cBO.UpdateCardToGroup(aVO);
                                    //cBO.sendAllowMessage(Convert.ToInt32(messageIdArr[i]));
                                }
                                catch
                                {
                                    isAllDelete = false;
                                }
                            }
                            if (isAllDelete)
                            {
                                return new ResultObject() { Flag = 1, Message = "审核成功!", Result = null };
                            }
                            else
                            {
                                return new ResultObject() { Flag = 1, Message = "部分审核成功!", Result = null };
                            }
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                        }
                    }
                    catch
                    {
                        return new ResultObject() { Flag = 0, Message = "审核失败!", Result = null };
                    }
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "需要管理员才能审核成员!", Result = null };
            }
        }
        /// <summary>
        /// 退出名片组
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SignOutCardGroup"), HttpGet]
        public ResultObject SignOutCardGroup(int GroupID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            int i = cBO.DeleteCardToGroupByGroupID(GroupID, customerId);

            if (i > 0)
            {
                return new ResultObject() { Flag = 1, Message = "退出成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "退出失败，请重试!", Result = null };
            }
        }

        /// <summary>
        /// 踢出名片组成员
        /// </summary>
        /// <param name="GroupCardID"></param>
        /// <param name="GroupID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteGroupCard"), HttpGet]
        public ResultObject DeleteGroupCard(string GroupCardID, int GroupID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            List<CardGroupCardViewVO> admincVO = cBO.isJionCardGroup(customerId, GroupID);

            if (admincVO.Count > 0)
            {
                if (admincVO[0].Status != 3)
                {
                    return new ResultObject() { Flag = 0, Message = "需要管理员才能踢出成员!", Result = null };
                }
                else
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(GroupCardID))
                        {
                            string[] messageIdArr = GroupCardID.Split(',');
                            bool isAllDelete = true;
                            for (int i = 0; i < messageIdArr.Length; i++)
                            {
                                try
                                {
                                    cBO.DeleteByGroupCardID(Convert.ToInt32(messageIdArr[i]));
                                }
                                catch
                                {
                                    isAllDelete = false;
                                }
                            }
                            if (isAllDelete)
                            {
                                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                            }
                            else
                            {
                                return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                            }

                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                        }
                    }
                    catch
                    {
                        return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
                    }
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "需要管理员才能踢出成员!", Result = null };
            }
        }


        /// <summary>
        /// 设置管理员
        /// </summary>
        /// <param name="GroupCardID"></param>
        /// <param name="GroupID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SetCardGroupAdmin"), HttpGet]
        public ResultObject SetCardGroupAdmin(string GroupCardID, int GroupID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            List<CardGroupCardViewVO> admincVO = cBO.isJionCardGroup(customerId, GroupID);

            if (admincVO.Count > 0)
            {
                if (admincVO[0].Status != 3)
                {
                    return new ResultObject() { Flag = 0, Message = "需要管理员才能设置!", Result = null };
                }
                else
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(GroupCardID))
                        {
                            string[] messageIdArr = GroupCardID.Split(',');
                            bool isAllDelete = true;
                            if (messageIdArr.Length > 4)
                            {
                                return new ResultObject() { Flag = 0, Message = "最多只能设置4个管理员!", Result = null };
                            }
                            cBO.UpdateCardToGroup("Status=1", "GroupID=" + GroupID + " and Status=3");//先将所有管理员设置为普通成员
                            CardGroupCardVO aVO = new CardGroupCardVO();
                            aVO.GroupCardID = admincVO[0].GroupCardID;
                            aVO.Status = 3;
                            cBO.UpdateCardToGroup(aVO);//将设置者改回管理员

                            for (int i = 0; i < messageIdArr.Length; i++)
                            {
                                try
                                {
                                    CardGroupCardVO cVO = new CardGroupCardVO();
                                    cVO.GroupCardID = Convert.ToInt32(messageIdArr[i]);
                                    cVO.Status = 3;

                                    cBO.UpdateCardToGroup(cVO);
                                }
                                catch
                                {
                                    isAllDelete = false;
                                }
                            }
                            if (isAllDelete)
                            {
                                return new ResultObject() { Flag = 1, Message = "设置成功!", Result = null };
                            }
                            else
                            {
                                return new ResultObject() { Flag = 1, Message = "部分设置成功!", Result = null };
                            }

                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                        }
                    }
                    catch
                    {
                        return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };
                    }
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "需要管理员才能设置!", Result = null };
            }
        }


        /// <summary>
        /// 添加名片到名片组
        /// </summary>
        /// <param name="CardID"></param>
        /// <param name="GroupID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddGroupCard"), HttpGet]
        public ResultObject AddGroupCard(string CardID, int GroupID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardGroupVO gVO = cBO.FindCardGroupById(GroupID);
            try
            {
                if (!string.IsNullOrEmpty(CardID))
                {
                    string[] messageIdArr = CardID.Split(',');
                    bool isAllDelete = true;
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {
                            CardDataVO cVO = cBO.FindCardById(Convert.ToInt32(messageIdArr[i]));
                            if (cVO != null)
                            {
                                List<CardGroupCardViewVO> cgVO = cBO.isJionCardGroup(cVO.CustomerId, GroupID);
                                if (cgVO.Count <= 0)
                                {
                                    CardGroupCardVO cgcVO = new CardGroupCardVO();
                                    cgcVO.CustomerId = cVO.CustomerId;
                                    cgcVO.GroupID = GroupID;
                                    if (gVO.JoinSetUp == 0)
                                    {
                                        cgcVO.Status = 1;
                                    }
                                    else
                                    {
                                        cgcVO.Status = 0;
                                    }
                                    cgcVO.CreatedAt = DateTime.Now;
                                    cgcVO.CardID = cVO.CardID;
                                    cBO.AddCardToGroup(cgcVO);
                                }
                            }
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分添加成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 改变我在名片组展示的名片
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="CardID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ChangeCardGroupCard"), HttpGet]
        public ResultObject ChangeCardGroupCard(int GroupID, int CardID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardGroupCardVO uVO = cBO.FindCardGroupCardByGroupID(GroupID, customerId);

            if (uVO != null)
            {
                CardGroupCardVO cVO = new CardGroupCardVO();
                cVO.GroupCardID = uVO.GroupCardID;
                cVO.CardID = CardID;
                if (cBO.UpdateCardToGroup(cVO))
                {
                    return new ResultObject() { Flag = 1, Message = "设置成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };
            }
        }


        /// <summary>
        /// 收款码上传
        /// </summary>
        /// <param name="PartyID">活动id</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        [Route("upPartyimg"), HttpGet]
        public ResultObject upPartyimg(int PartyID, string payimgurl, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            //CardBO cBO = new CardBO(new CustomerProfile());
            CardPartyVO rVO = cBO.FindPartyById(PartyID);
            if (rVO != null)
            {
                if (cProfile != null)
                {
                    //如果提交者并非该任务所属的会员，直接禁止更新
                    if (cProfile.CustomerId != rVO.CustomerId)
                        return new ResultObject() { Flag = 0, Message = "上传失败，只有活动创建者才可以上传!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "上传失败，只有活动创建者才可以上传!", Result = null };
                }
            }

            rVO.PaymentCode = payimgurl;

            bool isSuccess = cBO.UpdateParty(rVO);
            if (isSuccess)
                return new ResultObject() { Flag = 1, Message = "上传成功!", Result = rVO.PartyID };
            else
                return new ResultObject() { Flag = 0, Message = "上传失败!", Result = null };
        }

        /// <summary>
        /// 添加或更新活动
        /// </summary>
        /// <param name="partyModelVO">任务VO</param>
        /// <param name="ContactsListID">联系人ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateParty"), HttpPost]
        public ResultObject UpdateParty([FromBody] PartyModel partyModelVO, string ContactsListID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            if (partyModelVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            /*审核文本是否合法*/
            if (!cBO.msg_sec_check(partyModelVO) && cProfile.CustomerId != 19894)
            {
                return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
            }
            /*审核文本是否合法*/

            CardPartyVO cardPartyVO = partyModelVO.CardParty;


            if (cardPartyVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            if (cardPartyVO.StartTime < DateTime.Now && cardPartyVO.Type == 1)
            {
                return new ResultObject() { Flag = 0, Message = "开始时间不能小于当前时间!", Result = null };
            }

            if (cardPartyVO.Address != "")
            {
                /*
                WeiXinGeocoder Geocoder = cBO.getLatitudeAndLongitude(cardPartyVO.Address);
                if (Geocoder != null)
                {
                    cardPartyVO.latitude = Geocoder.result.location.lat;
                    cardPartyVO.longitude = Geocoder.result.location.lng;
                } else
                {
                    cardPartyVO.latitude = 0;
                    cardPartyVO.longitude = 0;
                }*/
            }
            List<CardPartySignUpFormVO> CardPartySignUpFormVOList = partyModelVO.CardPartySignUpForm;
            List<CardPartyCostVO> CardPartyCost = partyModelVO.CardPartyCost;
            List<CardPartyContactsVO> CardPartyContactsVOList = new List<CardPartyContactsVO>();

            if (ContactsListID != "")
            {
                string[] ContactsList = ContactsListID.Split(',');
                List<CardPartyContactsVO> ContactsVO = new List<CardPartyContactsVO>();
                for (int i = 0; i < ContactsList.Length; i++)
                {
                    CardPartyContactsVO cVO = new CardPartyContactsVO();
                    cVO.PartyContactsID = 0;
                    cVO.CardID = Convert.ToInt32(ContactsList[i]);
                    cVO.PartyID = cardPartyVO.PartyID;
                    ContactsVO.Add(cVO);
                }
                CardPartyContactsVOList = ContactsVO;
            }
            else
            {
                CardPartyContactsVOList = partyModelVO.CardPartyContacts;
            }

            if (cardPartyVO != null)
            {
                if (cardPartyVO.PartyID < 1)
                {
                    cardPartyVO.CreatedAt = DateTime.Now;
                    cardPartyVO.Status = 1;


                    if (cProfile != null)
                        cardPartyVO.CustomerId = cProfile.CustomerId;

                    try
                    {
                        //为活动创建名片组
                        CardGroupVO CardGroupVO = new CardGroupVO();
                        CardGroupVO.CreatedAt = DateTime.Now;
                        CardGroupVO.CustomerId = cProfile.CustomerId;
                        CardGroupVO.JoinSetUp = 1;
                        CardGroupVO.GroupName = "活动-" + cardPartyVO.Title;

                        int GroupID = cBO.AddCardGroup(CardGroupVO);
                        if (GroupID > 0)
                        {
                            //将联系人作为管理员加入名片组
                            cardPartyVO.GroupID = GroupID;
                            List<int> Cid = new List<int>();
                            for (int i = 0; i < CardPartyContactsVOList.Count; i++)
                            {
                                CardDataVO cVO = cBO.FindCardById(CardPartyContactsVOList[i].CardID);
                                if (cVO != null)
                                {
                                    int CustomerId = cVO.CustomerId;
                                    if (!Cid.Contains(CustomerId))
                                    {
                                        CardGroupCardVO cgcVO = new CardGroupCardVO();
                                        cgcVO.CustomerId = CustomerId;
                                        cgcVO.GroupID = GroupID;
                                        cgcVO.Status = 3;
                                        cgcVO.CreatedAt = DateTime.Now;
                                        cgcVO.CardID = CardPartyContactsVOList[i].CardID;
                                        cBO.AddCardToGroup(cgcVO);

                                        Cid.Add(CustomerId);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {

                    }


                    int requireId = cBO.AddParty(cardPartyVO, CardPartyContactsVOList, CardPartySignUpFormVOList, CardPartyCost);
                    if (requireId > 0)
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = requireId };
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
                else
                {
                    CardPartyVO rVO = cBO.FindPartyById(cardPartyVO.PartyID);
                    if (rVO == null)
                    {
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                    }
                    if (rVO.SignUpTime < DateTime.Now && rVO.Type == 1)
                    {
                        return new ResultObject() { Flag = 0, Message = "报名已截止，不能修改活动!", Result = null };
                    }

                    if (cProfile == null)
                    {
                        return new ResultObject() { Flag = 0, Message = "您不是活动发布者，更新失败!", Result = null };
                    }
                    bool isSuccess = false;
                    if (cProfile.CustomerId == rVO.CustomerId)
                    {
                        if (cardPartyVO.limitPeopleNum == 0)
                        {
                            cardPartyVO.limitPeopleNum = 0;
                        }
                        isSuccess = cBO.UpdateParty(cardPartyVO, CardPartyContactsVOList, CardPartySignUpFormVOList, CardPartyCost);
                    }
                    else if (cBO.isPartyContacts(cardPartyVO.PartyID, cProfile.CustomerId))
                    {
                        CardPartyVO cpVO = new CardPartyVO();
                        cpVO.PartyID = cardPartyVO.PartyID;
                        cpVO.Title = cardPartyVO.Title;
                        cpVO.Content = cardPartyVO.Content;
                        cpVO.Details = cardPartyVO.Details;
                        cpVO.Details2 = cardPartyVO.Details2;
                        cpVO.Host = cardPartyVO.Host;
                        cpVO.PartyTag = cardPartyVO.PartyTag;
                        cpVO.MainImg = cardPartyVO.MainImg;
                        cpVO.Address = cardPartyVO.Address;
                        cpVO.latitude = cardPartyVO.latitude;
                        cpVO.longitude = cardPartyVO.longitude;
                        cpVO.style = cardPartyVO.style;
                        cpVO.Audio = cardPartyVO.Audio;
                        cpVO.AudioName = cardPartyVO.AudioName;

                        isSuccess = cBO.UpdateParty(cpVO);
                    }

                    if (isSuccess)
                    {
                        try
                        {
                            if (rVO.Type == 3)
                            {
                                CardPartyVO cpVO = new CardPartyVO();
                                cpVO.PartyID = rVO.PartyID;
                                cpVO.LuckDrawShareImg = cBO.getPosterByPartyID(rVO.PartyID, 38);
                                cBO.UpdateParty(cpVO);
                            }
                        }
                        catch
                        {

                        }

                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = cardPartyVO.PartyID };
                    }

                    else
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
        }

        /// <summary>
        /// 更新活动（修改显示隐藏联系人等设置）
        /// </summary>
        /// <param name="cardPartyVO">活动VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateParty"), HttpPost]
        public ResultObject UpdateParty([FromBody] CardPartyVO cardPartyVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            if (cardPartyVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            if (cardPartyVO != null)
            {
                CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
                CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
                CardPartyVO rVO = cBO.FindPartyById(cardPartyVO.PartyID);
                if (rVO != null)
                {
                    if (cProfile != null)
                    {
                        //如果提交者并非该任务所属的会员，直接禁止更新
                        if (cProfile.CustomerId != rVO.CustomerId)
                            return new ResultObject() { Flag = 0, Message = "您不是活动发布者，更新失败!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "您不是活动发布者，更新失败!", Result = null };
                    }
                }

                rVO.isDisplayContacts = cardPartyVO.isDisplayContacts;
                rVO.isDisplayCost = cardPartyVO.isDisplayCost;
                rVO.isDisplaySignup = cardPartyVO.isDisplaySignup;
                rVO.isPromotionAward = cardPartyVO.isPromotionAward;
                rVO.isPromotionSignup = cardPartyVO.isPromotionSignup;
                rVO.isPromotionRead = cardPartyVO.isPromotionRead;
                rVO.isClickSignup = cardPartyVO.isClickSignup;

                bool isSuccess = cBO.UpdateParty(rVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = rVO.PartyID };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelParty"), HttpGet]
        public ResultObject DelParty(int PartyID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardPartyVO rVO = cBO.FindPartyById(PartyID);
            if (rVO != null)
            {
                if (cProfile != null)
                {
                    //如果提交者并非该任务所属的会员，直接禁止更新
                    if (cProfile.CustomerId != rVO.CustomerId)
                        return new ResultObject() { Flag = 0, Message = "删除失败，只有活动创建者才可以删除!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "删除失败，只有活动创建者才可以删除!", Result = null };
                }
            }

            rVO.Status = 0;

            bool isSuccess = cBO.UpdateParty(rVO);
            if (isSuccess)
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = rVO.PartyID };
            else
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
        }

        /// <summary>
        /// 上传活动视频
        /// </summary>
        /// <returns></returns>
        [Route("UploadPartyVideo"), HttpPost]
        public ResultObject UploadPartyVideo(string token, int duration)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            if (duration > 300)
            {
                return new ResultObject() { Flag = 0, Message = "最大只能上传5分钟长度的视频!", Result = null };
            }

            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string folder = "/UploadFolder/PartyVideo/" + DateTime.Now.ToString("yyyyMM") + "/";
            string imgPath = "";
            if (hfc.Count > 0)
            {
                try
                {
                    FileInfo fi = new FileInfo(hfc[0].FileName);

                    string ext = fi.Extension.ToLower();
                    if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
                    {
                        return new ResultObject() { Flag = 0, Message = "文件类型错误", Result = null };
                    }

                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fi.Extension;

                    //本地路径
                    string localPath = ConfigInfo.Instance.UploadFolder + folder;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string PhysicalPath = localPath + newFileName;
                    imgPath = "~" + folder + newFileName;
                    hfc[0].SaveAs(PhysicalPath);


                    //网络路径
                    string url = ConfigInfo.Instance.APIURL + folder + newFileName;

                    return new ResultObject() { Flag = 1, Message = "上传成功", Result = url };
                }
                catch (Exception er)
                {
                    return new ResultObject() { Flag = 0, Message = "上传失败", Result = er.Message };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "上传失败", Result = "文件为空" };
            }
        }

        /// <summary>
        /// 测试上传活动音乐
        /// </summary>
        /// <returns></returns>
        [Route("UploadPartyAudiocs"), HttpPost, Anonymous]
        public ResultObject UploadPartyAudiocs(int size)
        {
            //UserProfile uProfile = CacheManager.GetUserProfile(token);
            //CustomerProfile cProfile = uProfile as CustomerProfile;
            //int customerId = cProfile.CustomerId;
            int customerId = 1;


            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO cVO = uBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), cVO.AppType);


            if (size > 10485760)
            {
                return new ResultObject() { Flag = 0, Message = "最大只能上传10M的文件!", Result = null };
            }

            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string folder = "/UploadFolder/PartyAudio/" + DateTime.Now.ToString("yyyyMM") + "/";
            string imgPath = "";
            if (hfc.Count > 0)
            {
                try
                {
                    FileInfo fi = new FileInfo(hfc[0].FileName);

                    string ext = fi.Extension.ToLower();
                    if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
                    {
                        return new ResultObject() { Flag = 0, Message = "文件类型错误", Result = null };
                    }

                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fi.Extension;

                    //本地路径
                    string localPath = ConfigInfo.Instance.UploadFolder + folder;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string PhysicalPath = localPath + newFileName;
                    imgPath = "~" + folder + newFileName;
                    hfc[0].SaveAs(PhysicalPath);

                    string filena = hfc[0].FileName;
                    byte[] filenamebyte = Encoding.Default.GetBytes(filena);
                    string utffilname = Encoding.UTF8.GetString(filenamebyte);

                    //网络路径
                    string url = ConfigInfo.Instance.APIURL + folder + newFileName;

                    imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;

                    CardPoterVO CardPoterVO = new CardPoterVO();
                    CardPoterVO.CardPoterID = 0;
                    CardPoterVO.CustomerId = customerId;
                    CardPoterVO.FileName = utffilname;
                    CardPoterVO.Url = imgPath;
                    CardPoterVO.UploadPc = 1;
                    cBO.AddCardPoter(CardPoterVO);

                    return new ResultObject() { Flag = 1, Message = "上传成功", Result = url };
                }
                catch (Exception er)
                {
                    return new ResultObject() { Flag = 0, Message = "上传失败", Result = er.Message };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "上传失败", Result = "文件为空" };
            }
        }

        /// <summary>
        /// 上传活动音乐
        /// </summary>
        /// <returns></returns>
        [Route("UploadPartyAudio"), HttpPost]
        public ResultObject UploadPartyAudio(string token, int size)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO cVO = uBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), cVO.AppType);

            if (size > 10485760)
            {
                return new ResultObject() { Flag = 0, Message = "最大只能上传10M的文件!", Result = null };
            }

            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string folder = "/UploadFolder/PartyAudio/" + DateTime.Now.ToString("yyyyMM") + "/";
            string imgPath = "";
            if (hfc.Count > 0)
            {
                try
                {
                    FileInfo fi = new FileInfo(hfc[0].FileName);

                    string ext = fi.Extension.ToLower();
                    if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
                    {
                        return new ResultObject() { Flag = 0, Message = "文件类型错误", Result = null };
                    }

                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fi.Extension;

                    //本地路径
                    string localPath = ConfigInfo.Instance.UploadFolder + folder;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string PhysicalPath = localPath + newFileName;
                    imgPath = "~" + folder + newFileName;
                    hfc[0].SaveAs(PhysicalPath);


                    //网络路径
                    string url = ConfigInfo.Instance.APIURL + folder + newFileName;

                    imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;

                    CardPoterVO CardPoterVO = new CardPoterVO();
                    CardPoterVO.CardPoterID = 0;
                    CardPoterVO.CustomerId = 0;
                    CardPoterVO.SizeType = 3;
                    CardPoterVO.FileName = hfc[0].FileName;
                    CardPoterVO.Url = imgPath;
                    CardPoterVO.UploadPc = 1;
                    cBO.AddCardPoter(CardPoterVO);

                    return new ResultObject() { Flag = 1, Message = "上传成功", Result = url };
                }
                catch (Exception er)
                {
                    return new ResultObject() { Flag = 0, Message = "上传失败", Result = er.Message };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "上传失败", Result = "文件为空" };
            }
        }

        /// <summary>
        /// 上传活动音乐(上传文件的路径)
        /// </summary>
        /// <returns></returns>
        [Route("UploadPartyAudioSpPath"), HttpPost, Anonymous]
        public ResultObject UploadPartyAudioSpPath(string pathurl,string filename,string token, int size)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO cVO = uBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), cVO.AppType);
            try
            {
                CardPoterVO CardPoterVO = new CardPoterVO();
                CardPoterVO.CardPoterID = 0;
                CardPoterVO.CustomerId = 0;
                CardPoterVO.SizeType = 3;
                CardPoterVO.FileName = filename;
                CardPoterVO.Url = pathurl;
                CardPoterVO.UploadPc = 1;
                cBO.AddCardPoter(CardPoterVO);

                return new ResultObject() { Flag = 1, Message = "上传成功", Result = null };

            }
            catch (Exception er)
            {
                return new ResultObject() { Flag = 0, Message = "上传失败", Result = er.Message };
            }
        }




        /// <summary>
        /// 上传活动音乐
        /// </summary>
        /// <returns></returns>
        [Route("Uploadzip"), HttpPost, Anonymous]
        public ResultObject Uploadzip()
        {
            //UserProfile uProfile = CacheManager.GetUserProfile(token);
            //CustomerProfile cProfile = uProfile as CustomerProfile;
            //int customerId = cProfile.CustomerId;

            //CustomerBO uBO = new CustomerBO(new CustomerProfile());
            //CustomerVO cVO = uBO.FindCustomenById(customerId);
            //CardBO cBO = new CardBO(new CustomerProfile(), cVO.AppType);

            //if (size > 10485760)
            //{
            //    return new ResultObject() { Flag = 0, Message = "最大只能上传10M的文件!", Result = null };
            //}
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            //var formstr = System.Web.HttpContext.Current.Request.Form;
            
            string folder = "/UploadFolder/PartyAudio/" + DateTime.Now.ToString("yyyyMM") + "/";
            string imgPath = "";
            if (hfc.Count > 0)
            {
                try
                {
                    FileInfo fi = new FileInfo(hfc[0].FileName);

                    string ext = fi.Extension.ToLower();
                    if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
                    {
                        return new ResultObject() { Flag = 0, Message = "文件类型错误", Result = null };
                    }

                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fi.Extension;

                    //本地路径
                    string localPath = ConfigInfo.Instance.UploadFolder + folder;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string PhysicalPath = localPath + newFileName;
                    imgPath = "~" + folder + newFileName;
                    hfc[0].SaveAs(PhysicalPath);


                    //网络路径
                    string url = ConfigInfo.Instance.APIURL + folder + newFileName;

                    imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;

                    

                    return new ResultObject() { Flag = 1, Message = "上传成功", Result = imgPath };
                }
                catch (Exception er)
                {
                    return new ResultObject() { Flag = 0, Message = "上传失败", Result = er.Message };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "上传失败", Result = "文件为空" };
            }
        }

        /// <summary>
        /// 测试删除自定义上传活动音乐
        /// </summary>
        /// <returns></returns>
        [Route("delPartyAudiocs"), HttpGet, Anonymous]
        public ResultObject delPartyAudiocs()
        {
            CardBO cBO = new CardBO(new CustomerProfile());

            try
            {
                //自定义上传音乐集合
                string sql = "UploadPc = 1";
                List<CardPoterVO> PoterList = cBO.FindCardPoterByCondition(sql);
                for (int j = 0; j < PoterList.Count; j++)
                {

                    try
                    {
                        cBO.DeleteCardPoterAdminById(Convert.ToInt32(PoterList[j].CardPoterID));
                        //删除音乐
                        if (PoterList[j].Url != "")
                        {
                            //String url = "/UploadFolder/CardPartyQRTemporaryFile/" + QRurl;
                            //string FilePath = url;
                            //FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                            //FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                            //File.Delete(FilePath);

                            //String url = "/UploadFolder/PartyAudio/" + QRurl;
                            //string FilePath = url;
                            string FilePath = PoterList[j].Url;
                            FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                            FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                            File.Delete(FilePath);
                        }

                    }
                    catch
                    {
                        return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
                    }


                }
                if (PoterList.Count > 0)
                {
                    return new ResultObject() { Flag = 0, Message = "删除成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "没有自定义音乐无需删除!", Result = null };
                }


            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除活动视频
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelPartyVideo"), HttpGet]
        public ResultObject DelPartyVideo(int PartyID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardPartyVO rVO = cBO.FindPartyById(PartyID);
            if (rVO != null)
            {
                if (cProfile != null)
                {
                    //如果提交者并非该任务所属的会员，直接禁止更新
                    if (cProfile.CustomerId != rVO.CustomerId)
                        return new ResultObject() { Flag = 0, Message = "删除失败，只有活动创建者才可以删除!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "删除失败，只有活动创建者才可以删除!", Result = null };
                }
            }

            //删除视频
            if (rVO.Details2 != "")
            {
                string FilePath = rVO.Details2;
                FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                File.Delete(FilePath);
            }

            rVO.Details2 = "";

            bool isSuccess = cBO.UpdateParty(rVO);
            if (isSuccess)
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = rVO.PartyID };
            else
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
        }

        /// <summary>
        /// 删除活动音乐
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelPartyAudio"), HttpGet]
        public ResultObject DelPartyAudio(int PartyID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardPartyVO rVO = cBO.FindPartyById(PartyID);
            if (rVO != null)
            {
                if (cProfile != null)
                {
                    //如果提交者并非该任务所属的会员，直接禁止更新
                    if (cProfile.CustomerId != rVO.CustomerId)
                        return new ResultObject() { Flag = 0, Message = "删除失败，只有活动创建者才可以删除!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "删除失败，只有活动创建者才可以删除!", Result = null };
                }
            }

            //删除视频
            if (rVO.Audio != "")
            {
                string FilePath = rVO.Audio;
                FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                File.Delete(FilePath);
            }

            rVO.Audio = "";

            bool isSuccess = cBO.UpdateParty(rVO);
            if (isSuccess)
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = rVO.PartyID };
            else
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
        }

        /// <summary>
        /// 设置活动联系人
        /// </summary>
        /// <param name="CardID"></param>
        /// <param name="PartyID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SetCardPartyAdmin"), HttpGet]
        public ResultObject SetCardPartyAdmin(string CardID, int PartyID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardPartyVO cpVO = cBO.FindPartyById(PartyID);

            if (cpVO == null)
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            if (cpVO.CustomerId != customerId)
                return new ResultObject() { Flag = 0, Message = "对不起，你没有权限修改联系人!", Result = null };

            try
            {
                if (!string.IsNullOrEmpty(CardID))
                {
                    string[] messageIdArr = CardID.Split(',');
                    List<CardPartyContactsVO> ContactsVO = new List<CardPartyContactsVO>();
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {
                            CardPartyContactsVO cVO = new CardPartyContactsVO();
                            cVO.PartyContactsID = 0;
                            cVO.CardID = Convert.ToInt32(messageIdArr[i]);
                            cVO.PartyID = cpVO.PartyID;
                            ContactsVO.Add(cVO);
                        }
                        catch
                        {

                        }
                    }

                    bool isSuccess = cBO.UpdateParty(cpVO, ContactsVO);
                    if (isSuccess)
                    {
                        return new ResultObject() { Flag = 1, Message = "设置成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "设置失败!", Result = ContactsVO };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取活动详情，匿名
        /// </summary>
        /// <param name="PartyID">活动ID</param>
        /// <returns></returns>
        [Route("GetPartySite"), HttpGet, Anonymous]
        public ResultObject GetPartySite(int PartyID, int AppType = 1, int InviterCID = 0)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            if (PartyID == 1)
            {
                PartyID = cBO.GetADPartyID();
            }

            CardPartyVO cVO = cBO.FindPartyById(PartyID);
            if (cVO != null)
            {

                if (cVO.QRCodeImg == "")
                {
                    cVO.QRCodeImg = cBO.GetCardPartyQR(PartyID);
                }

                if (cVO.QRSignInImg == "")
                {
                    cVO.QRSignInImg = cBO.GetCardPartySignUpQRByUser(PartyID);
                }

                if (cVO.SignupConditions == 1 && cVO.ConditionsQR == "")
                {
                    cVO.ConditionsQR = cBO.getQRIMGByIDAndType(PartyID, 7);
                }

                if (cVO.LuckDrawShareImg == "" && cVO.Type == 3)
                {
                    cVO.LuckDrawShareImg = cBO.getPosterByPartyID(PartyID, 38);
                }

                PartyModel partyModelVO = new PartyModel();
                partyModelVO.CardParty = cVO;
                partyModelVO.CardPartySignUp = cBO.FindSignUpByCondtion("PartyID=" + PartyID + " GROUP BY CustomerId ORDER BY CreatedAt desc LIMIT 30");

                if (cVO.RecordSignUpCount <= 1)
                {
                    cVO.RecordSignUpCount = cBO.FindCardPartSignInSumCount("Number", "PartyID=" + cVO.PartyID + " and (SignUpStatus=1 or SignUpStatus=0)");
                    cBO.UpdateParty(cVO);
                }

                partyModelVO.CardPartySignCount = cVO.RecordSignUpCount;
                if (partyModelVO.CardPartySignUp.Count > 50)
                {
                    List<CardPartySignUpVO> CardPartySignUpVO = new List<CardPartySignUpVO>();
                    //保留前50条数据
                    for (int i = 0; i < partyModelVO.CardPartySignUp.Count && i < 50; i++)
                    {
                        CardPartySignUpVO.Add(partyModelVO.CardPartySignUp[i]);
                    }
                    partyModelVO.CardPartySignUp = CardPartySignUpVO;
                }

                partyModelVO.CardPartyCost = cBO.FindCostByPartyID(PartyID);
                partyModelVO.CardPartySignUpForm = cBO.FindSignUpFormByPartyID(PartyID);
                if (cVO.Type != 3)
                {
                    partyModelVO.CardPartyInviterList = cBO.FindSignUpViewInviterByPartyID(PartyID);
                    partyModelVO.CardPartyContactsView = cBO.FindPartyContactsByPartyId(PartyID);
                }
                else
                {
                    partyModelVO.CardPartyInviterList = new List<CardPartySignUpViewVO>();
                    partyModelVO.CardPartyContactsView = new List<CardPartyContactsViewVO>();
                }


                List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(cVO.CustomerId);
                if (CardDataVO.Count > 0)
                {
                    partyModelVO.CardData = CardDataVO[0];
                }

                BusinessCardBO BusinessCardBO = new BusinessCardBO(new CustomerProfile());
                BusinessCardVO bVO = new BusinessCardVO();
                if (cVO.BusinessID != 0)
                {
                    bVO = BusinessCardBO.FindBusinessCardById(cVO.BusinessID);
                    //清除营业执照等保密信息

                    if (bVO != null)
                    {
                        bVO.BusinessLicenseImg = "";
                    }
                }

                CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(cVO.CustomerId);
                int ChangePartyID = 0;
                CardPartyVO AdParty = null;
                List<CardPartyVO> PartyVO = new List<CardPartyVO>();
                if (cVO.Type == 3)
                {
                    try
                    {
                        PartyVO = cBO.FindCardPartyByCondtion("Type = 3 and PartyLuckDrawStatus = 0 and (isHot = 1 or isIndex = 1) and SignUpTime > now()");
                        Random Rdm = new Random();
                        if (PartyVO.Count > 0)
                        {
                            ChangePartyID = PartyVO[Rdm.Next(0, PartyVO.Count - 1)].PartyID;
                            AdParty = PartyVO[Rdm.Next(0, PartyVO.Count - 1)];
                        }

                        List<CardPartyCostVO> FirstPrizeList = cBO.FindCostByFirstPrize(AdParty.PartyID);
                        if (FirstPrizeList.Count > 0)
                        {
                            AdParty.FirstPrize = FirstPrizeList[0];
                        }
                        else
                        {
                            List<CardPartyCostVO> Cost = cBO.FindCostByPartyID(AdParty.PartyID);
                            if (Cost.Count > 0)
                            {
                                AdParty.FirstPrize = Cost[0];
                            }
                        }
                    }
                    catch
                    {

                    }

                }

                CardDataVO InviterDataVO = null;

                if (InviterCID > 0)
                {
                    List<CardDataVO> InviterCardDataVO = cBO.FindCardByCustomerId(InviterCID);
                    if (InviterCardDataVO.Count > 0)
                    {
                        InviterDataVO = InviterCardDataVO[0];
                    }
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = partyModelVO, Subsidiary = bVO, Subsidiary2 = new { CustomerVO2.isIdCard, ChangePartyID, AdParty, InviterDataVO } };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取测试
        /// </summary>
        /// <param name="PartyID">活动ID</param>
        /// <returns></returns>
        [Route("test1"), HttpGet, Anonymous]
        public ResultObject test1(int PartyID)
        {
            CardBO cBO = new CardBO(new CustomerProfile());

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = null };
        }

        /// <summary>
        /// 获取活动海报
        /// </summary>
        /// <param name="PartyID">活动ID</param>
        /// <param name="isRestart">0：不重新生成，1：重新生成海报</param>
        /// <param name="style">海报样式</param>
        /// <returns></returns>
        [Route("GetPartyPoster"), HttpGet, Anonymous]
        public ResultObject GetPartyPoster(int PartyID, int isRestart = 0, int style = 0, int CustomerId = 0, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardPartyVO cVO = cBO.FindPartyById(PartyID);

            if (cVO != null)
            {
                if (cVO.PosterImg == "")
                {
                    cVO.PosterImg = cBO.getPosterByPartyID(PartyID, style, CustomerId);
                }
                else if (isRestart == 1)
                {
                    cVO.PosterImg = cBO.getPosterByPartyID(PartyID, style, CustomerId);
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO.PosterImg };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取活动报名填写项，匿名
        /// </summary>
        /// <param name="PartyID">活动ID</param>
        /// <returns></returns>
        [Route("GetPartySignUpForm"), HttpGet, Anonymous]
        public ResultObject GetPartySignUpForm(int PartyID, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            List<CardPartySignUpFormVO> cVO = cBO.FindSignUpFormByPartyID(PartyID);

            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "无报名信息!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取活动报名费用，匿名
        /// </summary>
        /// <param name="PartyID">活动ID</param>
        /// <returns></returns>
        [Route("GetPartyCost"), HttpGet, Anonymous]
        public ResultObject GetPartyCost(int PartyID, int AppType = 1, string token = "")
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            List<CardPartyCostVO> cVO = cBO.FindCostByPartyID(PartyID);
            CardPartyVO PartyVO = cBO.FindPartyById(PartyID);
            int customerId = 0;
            if (token != "")
            {
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                customerId = cProfile.CustomerId;
            }

            foreach (CardPartyCostVO item in cVO)
            {
                List<CardPartySignUpViewVO> CostSignUpVO = cBO.PartyCostSignUpView(item.Names, PartyID);
                item.QuantitySold = CostSignUpVO.Sum(p => p.Number);

                if (PartyVO.isPromotionRead == 1 && item.PromotionRead > 0)
                {
                    item.isPromotionRead = 1;
                    if (customerId > 0)
                        item.MyPromotionRead = cBO.FindAccessrecordsCount("ShareCustomerId=" + customerId + " and CustomerId<>" + customerId + " and ById=" + PartyID + " and Type='ReadParty'", true);
                    else
                    {
                        item.MyPromotionRead = 0;
                    }
                }

                if (PartyVO.isPromotionSignup == 1 && item.PromotionSignup > 0)
                {
                    item.isPromotionSignup = 1;
                    if (customerId > 0)
                        item.MyPromotionSignup = cBO.FindPartyOrderTotalCount("InviterCID=" + customerId + " and CustomerId<>" + customerId + " and Status=1 and PartyID=" + PartyID);
                    else
                    {
                        item.MyPromotionSignup = 0;
                    }
                }
            }

            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    int MyReadSigUp = cBO.FindPartyOrderTotalCount("CustomerId=" + customerId + " and Status=1 and PartyID=" + PartyID + " and (PromotionReadStatus=1 or PromotionSignupStatus=1)");
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO, Subsidiary = MyReadSigUp };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "无费用信息!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 设置活动报名费用推广奖励
        /// </summary>
        /// <param name="PartyCostID">费用ID</param>
        /// <param name="PromotionAward">返佣比例</param>
        /// <returns></returns>
        [Route("GetPartyCost"), HttpGet]
        public ResultObject GetPartyCost(int PartyCostID, int PromotionAward, string token, string Promotion = "Award")
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            CardPartyCostVO CostVO = cBO.FindCostById(PartyCostID);
            if (CostVO == null || PromotionAward < 0 || PromotionAward > 100)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误", Result = null };
            }

            CardPartyVO rVO = cBO.FindPartyById(CostVO.PartyID);
            if (rVO.CustomerId != customerId)
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限", Result = null };
            }


            if (Promotion == "Award")
            {
                //如果是独立商户
                EcommerceBO eBO = new EcommerceBO();
                wxMerchantVO mVO = eBO.getMyMerchant(customerId);
                if (mVO != null)
                {
                    int p = 30 - mVO.SplitProportion;
                    if (p - PromotionAward < 0)
                    {
                        return new ResultObject() { Flag = 0, Message = "由于您是独立商户，目前只支持最大分佣比例" + p + "%", Result = null };
                    }
                }
                CostVO.PromotionAward = PromotionAward;
            }
            if (Promotion == "Signup")
            {
                CostVO.PromotionSignup = PromotionAward;
            }
            if (Promotion == "Read")
            {
                CostVO.PromotionRead = PromotionAward;
            }

            if (cBO.UpdateCost(CostVO))
            {
                return new ResultObject() { Flag = 1, Message = "设置成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };
            }
        }

        /// <summary>
        /// 判断是否已经报名
        /// </summary>
        /// <param name="PartyID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("isJoinCardParty"), HttpGet]
        public ResultObject isJoinCardParty(int PartyID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            List<CardPartySignUpVO> cVO = cBO.isJionCardParty(customerId, PartyID);
            if (cVO.Count > 0)
            {
                return new ResultObject() { Flag = 1, Message = "您已经报名了该活动!", Result = cVO[0].PartySignUpID, Subsidiary = cVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "您还未报名!", Result = null };
            }


        }

        /// <summary>
        /// 改变活动的转发，收藏，浏览
        /// </summary>
        /// <param name="PartID">名片ID</param>
        /// <param name="field">操作字段</param>
        /// <param name="sum">数量</param>
        /// <returns></returns>
        [Route("PartReadData"), HttpGet, Anonymous]
        public ResultObject PartReadData(int PartID, string field, int sum, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            CardPartyVO pVO = cBO.FindPartyById(PartID);


            if (field == "ReadCount")
            {
                pVO.ReadCount += sum;
            }
            if (field == "Forward")
            {
                pVO.Forward += sum;
            }
            if (cBO.UpdateParty(pVO))
            {
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

        /*
        /// <summary>
        /// 报名活动
        /// </summary>
        /// <param name="PartyID"></param>
        /// <param name="CardID"></param>
        /// <param name="FormId"></param>
        /// <param name="code"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        
        [Route("JoinCardParty"), HttpGet]
        public ResultObject JoinCardParty(int PartyID, int CardID, string FormId, string code, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CardBO cBO = new CardBO(new CustomerProfile());

            List<CardPartySignUpVO> cVO = cBO.isJionCardParty(customerId, PartyID);
            if (cVO.Count > 0)
            {
                return new ResultObject() { Flag = 4, Message = "您已经报名了该活动，请勿重复操作!", Result = PartyID };
            }

            CardDataVO cdVO = cBO.FindCardById(CardID);

            if(cdVO==null)
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = PartyID };

            CardPartySignUpVO suVO = new CardPartySignUpVO();

            suVO.CardID = cdVO.CardID;
            suVO.CustomerId = customerId;
            suVO.Name = cdVO.Name;
            suVO.Headimg = cdVO.Headimg;
            suVO.Phone = cdVO.Phone;
            suVO.PartyID = PartyID;
            suVO.FormId = FormId;
            suVO.OpenId = cBO.getOpenId(code);
            suVO.CreatedAt = DateTime.Now;

            int PartySignUpID = cBO.AddCardToParty(suVO);

            if (PartySignUpID > 0)
            {
                cBO.sendSignUpMessage(PartySignUpID);
                /*
                try
                {
                    //将报名的名片加入活动的名片组
                    CardPartyVO cpVO = cBO.FindPartyById(PartyID);
                    if (cpVO != null&& cpVO.GroupID!=0)
                    {
                        List<CardGroupCardViewVO> cgVO = cBO.isJionCardGroup(cdVO.CustomerId, cpVO.GroupID);
                        if (cgVO.Count <= 0)
                        {
                            CardGroupCardVO cgcVO = new CardGroupCardVO();
                            cgcVO.CustomerId = cdVO.CustomerId;
                            cgcVO.GroupID = cpVO.GroupID;
                            cgcVO.Status = 1;
                            cgcVO.CreatedAt = DateTime.Now;
                            cgcVO.CardID = cdVO.CardID;
                            cBO.AddCardToGroup(cgcVO);
                        }
                    }
                }
                catch
                {
                    
                }*//*
                return new ResultObject() { Flag = 1, Message = "报名成功!", Result = PartySignUpID };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "操作失败，请重试!", Result = null };
            }
        }
        */
        /// <summary>
        /// 报名活动
        /// </summary>
        /// <param name="CardPartySignUpFormVO">报名信息</param>
        /// <param name="PartyID"></param>
        /// <param name="FormId"></param>
        /// <param name="code"></param>
        /// <param name="token">口令</param>
        /// <param name="Headimg">头像</param>
        ///<param name="InviterCID">邀请者ID</param>
        ///<param name="Number">报名数量</param>
        /// <returns></returns>
        [Route("JoinCardParty"), HttpPost]
        public ResultObject JoinCardParty([FromBody] List<CardPartySignUpFormVO> CardPartySignUpFormVO, int PartyID, string FjUrl, string FormId, string code, string token, string Headimg, int InviterCID, int Number = 1, int PayType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);

            int AppType = CustomerVO2.AppType;
            if (PayType != 1)
            {
                AppType = PayType;
            }

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            int oldCount = cBO.FindCardPartSignInNumTotalCount("CustomerId = " + customerId + " and PartyID=" + PartyID + " and NOW()-CreatedAt<30");
            if (oldCount > 0)
            {
                return new ResultObject() { Flag = 0, Message = "操作过于频繁，请稍后!", Result = null };
            }

            /*审核文本是否合法*/
            if (!cBO.msg_sec_check(CardPartySignUpFormVO))
            {
                return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
            }
            /*审核文本是否合法*/

            if (Number <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "请输入购买数量", Result = PartyID };
            }

            CardPartyVO cpvo = cBO.FindPartyById(PartyID);
            List<CardPartySignUpViewVO> cpsuVO = cBO.FindSignUpViewByPartyID(PartyID);
            int _NumberSum = cpsuVO.Sum(p => p.Number);


            if (cpvo.limitPeopleNum != 0 && cpvo.limitPeopleNum < _NumberSum + Number) { return new ResultObject() { Flag = 0, Message = "报名人数已满", Result = null }; }

            if (cpvo.SignUpTime < DateTime.Now && cpvo.Type == 1)
            {
                return new ResultObject() { Flag = 0, Message = "报名已截止!", Result = null };
            }

            if (cpvo.Status == 0)
            {
                return new ResultObject() { Flag = 0, Message = "该活动已被主办方删除，请勿报名!", Result = null };
            }

            List<CardPartyCostVO> CostVO = cBO.FindCostByPartyID(PartyID);
            if (CostVO.Count > 0 && cpvo.Type != 3)
            {
                return new ResultObject() { Flag = 0, Message = "请选择购买类型", Result = PartyID };
            }

            if (cpvo.Type == 3)
            {
                List<CardPartySignUpVO> cVO = cBO.isJionCardParty(customerId, PartyID);
                if (cVO.Count > 0)
                {
                    return new ResultObject() { Flag = 4, Message = "您已经报名了抽奖，请勿重复操作!", Result = PartyID };
                }

                //发放游戏奖励
                FarmGameBO fBO = new FarmGameBO(new CustomerProfile());
                fBO.IssueTaskReward(customerId, "SignLuckDraw");

                int SignInNum = cBO.FindCardPartSignUpViewListTotalCount("Type=3 and CustomerId=" + customerId + " and DATE_FORMAT(CreatedAt,'%y-%m-%d')=DATE_FORMAT(now(),'%y-%m-%d')");
                if (SignInNum >= 2)
                {
                    fBO.IssueTaskReward(customerId, "SignThreeLuckDraw");
                }

                if (InviterCID > 0)
                {
                    fBO.IssueTaskReward(InviterCID, "HelpLuckDraw");
                }
            }

            List<CardDataVO> uVO = cBO.FindCardByCustomerId(customerId);
            CardPartySignUpVO suVO = new CardPartySignUpVO();

            int CardID = 0;
            string name = "";
            string phone = "";
            string SignUpForm = "";
            string Position = "";
            string CorporateName = "";
            string Address = "";
            decimal latitude = 0;
            decimal longitude = 0;
            string WeChat = "";

            for (int i = 0; i < CardPartySignUpFormVO.Count; i++)
            {
                if (CardPartySignUpFormVO[i].Name == "姓名")
                    name = CardPartySignUpFormVO[i].value;
                if (CardPartySignUpFormVO[i].Name == "手机")
                    phone = CardPartySignUpFormVO[i].value;
                if (CardPartySignUpFormVO[i].Name == "职位")
                    Position = CardPartySignUpFormVO[i].value;
                if (CardPartySignUpFormVO[i].Name == "工作单位")
                    CorporateName = CardPartySignUpFormVO[i].value;
                if (CardPartySignUpFormVO[i].Name == "微信")
                    WeChat = CardPartySignUpFormVO[i].value;
                if (CardPartySignUpFormVO[i].Name == "单位地址")
                {
                    Address = CardPartySignUpFormVO[i].value;
                    WeiXinGeocoder Geocoder = cBO.getLatitudeAndLongitude(CardPartySignUpFormVO[i].value);
                    if (Geocoder != null)
                    {
                        latitude = Geocoder.result.location.lat;
                        longitude = Geocoder.result.location.lng;
                    }
                }

                if (CardPartySignUpFormVO[i].Status > 0)
                {
                    SignUpForm += "<SignUpForm>" + "<Name>" + CardPartySignUpFormVO[i].Name + "</Name>" + "<Value>" + CardPartySignUpFormVO[i].value + "</Value>" + "</SignUpForm>";
                }
            }

            if (uVO.Count > 0)
            {
                Headimg = uVO[0].Headimg;
                CardID = uVO[0].CardID;

                if (name == "")
                {
                    name = uVO[0].Name;
                }

                if (uVO[0].Phone == "" && phone != "")
                {
                    uVO[0].Phone = phone;
                }
                if (uVO[0].Position == "" && Position != "")
                {
                    uVO[0].Position = Position;
                }
                if (uVO[0].CorporateName == "" && CorporateName != "")
                {
                    uVO[0].CorporateName = CorporateName;
                }
                if (uVO[0].WeChat == "" && WeChat != "")
                {
                    uVO[0].WeChat = WeChat;
                }
                if (uVO[0].Address == "" && Address != "")
                {
                    uVO[0].Address = Address;
                }

                cBO.Update(uVO[0]);
            }
            else
            {
                if (name == "")
                {
                    name = CustomerVO2.CustomerName;
                }

                CardDataVO CardDataVO = new CardDataVO();
                CardDataVO.Name = name;
                CardDataVO.Phone = phone;
                CardDataVO.Position = Position;
                CardDataVO.CorporateName = CorporateName;
                CardDataVO.Headimg = Headimg;
                CardDataVO.Address = Address;
                CardDataVO.WeChat = WeChat;
                CardDataVO.latitude = latitude;
                CardDataVO.longitude = longitude;

                CardDataVO.CreatedAt = DateTime.Now;
                CardDataVO.Status = 1;//0:禁用，1:启用
                CardDataVO.CustomerId = customerId;
                CardDataVO.isParty = 1;

                CardID = cBO.AddCard(CardDataVO);
            }

            suVO.CardID = CardID;
            suVO.CustomerId = customerId;
            suVO.Name = name;
            suVO.Headimg = Headimg;
            suVO.Phone = phone;
            suVO.PartyID = PartyID;
            suVO.FormId = FormId;
            suVO.FjUrl = FjUrl;
            suVO.OpenId = cBO.getOpenId(code);
            suVO.CreatedAt = DateTime.Now;
            suVO.SignUpForm = SignUpForm;
            suVO.InviterCID = InviterCID;
            suVO.Number = Number;
            if (cpvo.Type == 3)
            {
                suVO.LuckDrawNumber = cBO.RndCode();
            }


            int PartySignUpID = cBO.AddCardToParty(suVO);
            if (PartySignUpID > 0)
            {
                cBO.sendSignUpMessage(PartySignUpID);


                //人满自动开奖
                if (cpvo.Type == 3 && cpvo.LuckDrawType == 2 && cpvo.limitPeopleNum <= _NumberSum + Number)
                {
                    cBO.DrawAPrize(PartyID);
                }

                CardDataVO CardDataVO = cBO.FindCardById(CardID);
                string url = "/pages/Party/SignUpList/SignUpList?isadmin=true&PartyID=" + cpvo.PartyID;
                if (cpvo.Type == 3)
                {
                    url = "/package/package_party/SignUpList/SignUpList?isadmin=true&PartyID=" + cpvo.PartyID;
                }
                cBO.AddCardMessage(CardDataVO.Name + "报名了活动《" + cpvo.Title + "》", cpvo.CustomerId, "活动报名", url);

                if (AppType == 1 || AppType == 2)
                {
                    string Title = "您成功报名了活动《" + cpvo.Title + "》";
                    if (cpvo.Type == 2)
                    {
                        Title = "您成功购买了商品《" + cpvo.Title + "》";
                    }
                    cBO.AddCardMessage(Title, customerId, "活动报名", "/pages/Party/PartyShow/PartyShow?PartyID=" + cpvo.PartyID);
                }

                return new ResultObject() { Flag = 1, Message = "报名成功!", Result = PartySignUpID };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "操作失败，请重试!", Result = null };
            }
        }

        /// <summary>
        /// 报名活动（下单）
        /// </summary>
        /// <param name="CardPartySignUpFormVO">报名信息</param>
        /// <param name="PartyID"></param>
        /// <param name="FormId"></param>
        /// <param name="code"></param>
        /// <param name="token">口令</param>
        /// <param name="Headimg">头像</param>
        /// <param name="PartyCostID">费用信息ID</param>
        /// <param name="InviterCID">InviterCID</param>
        ///<param name="Number">报名数量</param>
        /// <returns></returns>
        [Route("PlaceAnOrder"), HttpPost]
        public ResultObject PlaceAnOrder([FromBody] List<CardPartySignUpFormVO> CardPartySignUpFormVO, int PartyID, string FormId, string code, string token, string Headimg, int PartyCostID, int InviterCID, int Number = 1, int PayType = 1, int isH5 = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);

            int AppType = CustomerVO2.AppType;
            if (PayType != 1)
            {
                AppType = PayType;
            }

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            if (Number <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "请输入购买数量", Result = PartyID };
            }

            /*审核文本是否合法*/
            if (!cBO.msg_sec_check(CardPartySignUpFormVO))
            {
                return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
            }
            /*审核文本是否合法*/

            CardPartyVO cpvo = cBO.FindPartyById(PartyID);

            if (cpvo.Type == 3)
            {
                return new ResultObject() { Flag = 0, Message = "抽奖活动无法下单!", Result = PartyID };
            }

            List<CardPartySignUpViewVO> cpsuVO = cBO.FindSignUpViewByPartyID(PartyID);
            int _NumberSum = cpsuVO.Sum(p => p.Number);

            if (cpvo.limitPeopleNum != 0 && cpvo.limitPeopleNum < _NumberSum + Number) { return new ResultObject() { Flag = 0, Message = "报名人数已满", Result = null }; }

            if (cpvo.Status == 0)
            {
                return new ResultObject() { Flag = 0, Message = "该活动已被主办方删除，请勿报名!", Result = null };
            }

            if (PartyCostID <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "请选择购买类型", Result = PartyID };
            }

            if (cpvo.SignUpTime < DateTime.Now && cpvo.Type == 1)
            {
                return new ResultObject() { Flag = 0, Message = "报名已截止!", Result = null };
            }

            CardPartyCostVO CostVO = cBO.FindCostById(PartyCostID);
            if (CostVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "请选择购买类型", Result = PartyID };
            }


            /*活动发起人未实名无法报名*/
            /*
            if(cpvo.Type!=3&& CostVO.Cost>0)
            {
                string st1 = "2021-05-26 00:00:00";
                DateTime dt1 = Convert.ToDateTime(st1);
                if(DateTime.Compare(dt1, cpvo.CreatedAt) > 0)
                {
                    CustomerVO HostCVO = CustomerBO.FindCustomenById(cpvo.CustomerId);
                    if (!HostCVO.isIdCard)
                    {
                        return new ResultObject() { Flag = 0, Message = "该活动商家未经过实名认证，不得进行收费！", Result = PartyID };
                    }
                }
            }*/


            List<CardPartySignUpViewVO> CostSignUpVO = cBO.PartyCostSignUpView(CostVO.Names, PartyID);
            int _CostNumberSum = CostSignUpVO.Sum(p => p.Number);

            if (CostVO.limitPeopleNum != 0 && CostVO.limitPeopleNum < _CostNumberSum + Number) { return new ResultObject() { Flag = 0, Message = "该类型报名人数已满，请选择其他购买类型", Result = new { CostVO = CostVO, CostSignUpVO = CostSignUpVO } }; }
            if (CostVO.EffectiveTime.Year > 1900 && CostVO.EffectiveTime < DateTime.Now)
            {
                return new ResultObject() { Flag = 0, Message = "该类型报名已截止,请选择其他购买类型!", Result = CostVO.EffectiveTime };
            }


            if (CostVO.PartyID != PartyID)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误", Result = PartyID };
            }

            /*
            List<CardPartySignUpVO> cVO = cBO.isJionCardParty(customerId, PartyID);
            if (cVO.Count > 0)
            {
                return new ResultObject() { Flag = 4, Message = "您已经报名了该活动，请勿重复操作!", Result = PartyID };
            }*/



            List<CardDataVO> uVO = cBO.FindCardByCustomerId(customerId);
            CardPartyOrderVO OrderVO = new CardPartyOrderVO();

            int CardID = 0;
            string name = "";
            string phone = "";
            string SignUpForm = "";
            string Position = "";
            string CorporateName = "";
            string Address = "";
            decimal latitude = 0;
            decimal longitude = 0;
            string WeChat = "";

            for (int i = 0; i < CardPartySignUpFormVO.Count; i++)
            {
                if (CardPartySignUpFormVO[i].Name == "姓名")
                    name = CardPartySignUpFormVO[i].value;
                if (CardPartySignUpFormVO[i].Name == "手机")
                    phone = CardPartySignUpFormVO[i].value;
                if (CardPartySignUpFormVO[i].Name == "职位")
                    Position = CardPartySignUpFormVO[i].value;
                if (CardPartySignUpFormVO[i].Name == "工作单位")
                    CorporateName = CardPartySignUpFormVO[i].value;
                if (CardPartySignUpFormVO[i].Name == "微信")
                    WeChat = CardPartySignUpFormVO[i].value;
                if (CardPartySignUpFormVO[i].Name == "单位地址")
                {
                    Address = CardPartySignUpFormVO[i].value;
                    WeiXinGeocoder Geocoder = cBO.getLatitudeAndLongitude(CardPartySignUpFormVO[i].value);
                    if (Geocoder != null)
                    {
                        latitude = Geocoder.result.location.lat;
                        longitude = Geocoder.result.location.lng;
                    }
                }

                if (CardPartySignUpFormVO[i].Status > 0)
                {
                    SignUpForm += "<SignUpForm>" + "<Name>" + CardPartySignUpFormVO[i].Name + "</Name>" + "<Value>" + CardPartySignUpFormVO[i].value + "</Value>" + "</SignUpForm>";
                }
            }

            if (uVO.Count > 0)
            {
                Headimg = uVO[0].Headimg;
                CardID = uVO[0].CardID;

                if (name == "")
                {
                    name = uVO[0].Name;
                }

                if (uVO[0].Phone == "" && phone != "")
                {
                    uVO[0].Phone = phone;
                }
                if (uVO[0].Position == "" && Position != "")
                {
                    uVO[0].Position = Position;
                }
                if (uVO[0].CorporateName == "" && CorporateName != "")
                {
                    uVO[0].CorporateName = CorporateName;
                }
                if (uVO[0].WeChat == "" && WeChat != "")
                {
                    uVO[0].WeChat = WeChat;
                }
                if (uVO[0].Address == "" && Address != "")
                {
                    uVO[0].Address = Address;
                }

                cBO.Update(uVO[0]);
            }
            else
            {
                if (name == "")
                {
                    name = CustomerVO2.CustomerName;
                }

                CardDataVO CardDataVO = new CardDataVO();
                CardDataVO.Name = name;
                CardDataVO.Phone = phone;
                CardDataVO.Position = Position;
                CardDataVO.CorporateName = CorporateName;
                CardDataVO.Address = Address;
                CardDataVO.WeChat = WeChat;
                CardDataVO.latitude = latitude;
                CardDataVO.longitude = longitude;


                CardDataVO.Headimg = Headimg;
                CardDataVO.CreatedAt = DateTime.Now;
                CardDataVO.Status = 1;//0:禁用，1:启用
                CardDataVO.CustomerId = customerId;
                CardDataVO.isParty = 1;

                CardID = cBO.AddCard(CardDataVO);
            }

            OrderVO.CardID = CardID;
            OrderVO.CustomerId = customerId;
            OrderVO.Name = name;
            OrderVO.Headimg = Headimg;
            OrderVO.Phone = phone;
            OrderVO.PartyID = PartyID;
            OrderVO.FormId = FormId;

            string OpenId = "";
            if (isH5 == 1)
            {
                ////获取微信的用户信息
                CustomerController customerCon = new CustomerController();
                ResultObject result = customerCon.GetThirdPartUserInfo("2", code, "", "2");
                WeiXinHelper.WeiXinUserInfoResult userInfo = result.Result as WeiXinHelper.WeiXinUserInfoResult;
                //用户信息（判断是否已经获取到用户的微信用户信息）
                if (userInfo.Result && userInfo.UserInfo.openid != "")
                {
                    OpenId = userInfo.UserInfo.openid;
                }
            }
            else
            {
                OpenId = cBO.getOpenId(code);
            }

            OrderVO.OpenId = OpenId;
            OrderVO.CreatedAt = DateTime.Now;
            OrderVO.SignUpForm = SignUpForm;
            OrderVO.CostName = CostVO.Names;

            OrderVO.Cost = CostVO.Cost * Number;
            if (CostVO.isDiscount == 1 && (CostVO.DiscountTime > DateTime.Now || CostVO.DiscountTime < DateTime.Now.AddYears(-20)))
            {
                List<CardPartySignUpViewVO> CostSignUpVO2 = cBO.PartyCostSignUpView(CostVO.Names, CostVO.DiscountCost, PartyID);
                int DiscountNumberSum = CostSignUpVO2.Sum(p => p.Number);
                if (CostVO.DiscountNum >= DiscountNumberSum + Number || CostVO.DiscountNum == 0)
                {
                    OrderVO.Cost = CostVO.DiscountCost * Number;
                }
            }



            //满足分享浏览条件则免费
            try
            {
                if ((cpvo.isPromotionRead == 1 && CostVO.PromotionRead > 0) || (cpvo.isPromotionSignup == 1 && CostVO.PromotionSignup > 0))
                {
                    int MyReadSigUp = cBO.FindPartyOrderTotalCount("CustomerId=" + customerId + " and Status=1 and PartyID=" + PartyID + " and (PromotionReadStatus=1 or PromotionSignupStatus=1)");
                    if (MyReadSigUp == 0)
                    {
                        if (cBO.isPromotionRead(customerId, PartyID, CostVO, cpvo))
                        {
                            OrderVO.Cost = 0;
                            OrderVO.PromotionReadStatus = 1;
                            Number = 1;
                        }
                        if (cBO.isPromotionSignup(customerId, PartyID, CostVO, cpvo))
                        {
                            OrderVO.Cost = 0;
                            OrderVO.PromotionSignupStatus = 1;
                            Number = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogBO _log2 = new LogBO(typeof(CardBO));
                string strErrorMsg2 = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log2.Error(strErrorMsg2);
            }


            OrderVO.InviterCID = InviterCID;
            OrderVO.Number = Number;
            OrderVO.sub_mchid = "";


            //如果是二级商户
            EcommerceBO eBO = new EcommerceBO();
            wxMerchantVO mVO = eBO.getMyMerchant(cpvo.CustomerId);

            if (mVO != null)
            {
                OrderVO.sub_mchid = mVO.sub_mchid;
                if (mVO.SplitProportion > 0)
                {
                    OrderVO.SplitCost = OrderVO.Cost * mVO.SplitProportion / 100;
                }
            }

            if (InviterCID > 0 && cpvo.isPromotionAward == 1 && CostVO.PromotionAward > 0)
            {
                OrderVO.PromotionAward = CostVO.PromotionAward;
                OrderVO.PromotionAwardCost = OrderVO.Cost * CostVO.PromotionAward / 100;
            }

            Random ran = new Random();
            OrderVO.OrderNO = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);

            int PartyOrderID = cBO.AddPartyOrder(OrderVO);
            if (PartyOrderID > 0)
            {
                return new ResultObject() { Flag = 1, Message = "下单成功!", Result = PartyOrderID };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "操作失败，请重试!", Result = null };
            }
        }

        /// <summary>
        /// 后台自动添加报名
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("autoSignUpByPartyID"), HttpGet, Anonymous]
        public ResultObject autoSignUpByPartyID(int PartyID, int Count, int AppType = 1)
        {
            try
            {
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);
                List<CardDataVO> cVO = cBO.FindCardByCondition("ReadCount=0 and Collection=0 and Headimg<>'https://www.zhongxiaole.net/SPManager/Style/images/wxcard/noheadimg.jpg' and Headimg<>'' and Forward=0 and CreatedAt<date_add(curdate(),interval -6 MONTH) and Headimg<>'' and Phone<>''", 3000);
                cVO = cVO.OrderBy(f => Guid.NewGuid()).ToList();

                if (cVO != null)
                {
                    CardPartyOrderVO OrderVO = new CardPartyOrderVO();
                    if (Count > cVO.Count)
                    {
                        Count = cVO.Count;
                    }
                    CardPartyVO PartyVO = cBO.FindPartyById(PartyID);

                    for (int i = 0; i < Count; i++)
                    {
                        CardPartySignUpVO suVO = new CardPartySignUpVO();

                        suVO.CardID = cVO[i].CardID;
                        suVO.CustomerId = cVO[i].CustomerId;
                        suVO.Name = cVO[i].Name;
                        suVO.Headimg = cVO[i].Headimg;
                        suVO.Phone = cVO[i].Phone;
                        suVO.PartyID = PartyID;
                        suVO.CreatedAt = RandomTime(PartyVO.CreatedAt, DateTime.Now, i);
                        suVO.SignUpForm = "<SignUpForm><Name>姓名</Name><Value>" + cVO[i].Name + "</Value></SignUpForm><SignUpForm><Name>手机</Name><Value>" + cVO[i].Phone + "</Value></SignUpForm>";
                        suVO.InviterCID = 0;
                        suVO.Number = 1;
                        suVO.isAutoAdd = 1;

                        int PartySignUpID = cBO.AddCardToParty(suVO);
                    }

                    PartyVO.ReadCount += Count * 10;
                    cBO.UpdateParty(PartyVO);

                    return new ResultObject() { Flag = 1, Message = "设置成功!", Result = cVO };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = ex };
            }
        }
        /// <summary>
        /// 增加活动浏览次数
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("addLookByPartyID"), HttpGet, Anonymous]
        public ResultObject addLookByPartyID(int PartyID, int Count, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId == 3 || uProfile.UserId == 5 || uProfile.UserId == 7)
            {
                try
                {
                    CardBO cBO = new CardBO(new CustomerProfile(), AppType);
                    CardPartyVO PartyVO = cBO.FindPartyById(PartyID);

                    PartyVO.ReadCount += Count;
                    cBO.UpdateParty(PartyVO);

                    return new ResultObject() { Flag = 1, Message = "设置成功!", Result = PartyVO };
                }
                catch (Exception ex)
                {
                    return new ResultObject() { Flag = 0, Message = "设置失败!", Result = ex };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "权限不足!", Result = null };
            }

        }
        /// <summary>
        /// （在两个时间范围内）生成随机日期
        /// </summary>
        /// <param name="startime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns>返回随机日期，如（2014-12-25 00:00:00）</returns>
        public static DateTime RandomTime(DateTime startime, DateTime endtime, int Seed)
        {
            Random rd = new Random(Seed);
            TimeSpan tsp = endtime - startime;
            int Minutes = rd.Next(0, Convert.ToInt32(tsp.TotalMinutes));
            DateTime newtime = endtime.AddMinutes(-Minutes);
            return newtime;
        }


        /// <summary>
        /// 获取活动的所有报名列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("getSignUplist"), HttpGet, Anonymous]
        public ResultObject getSignUplist(int PartyID, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            List<CardPartySignUpViewVO> cVO = cBO.FindSignUpViewByPartyID(PartyID);
            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    cVO.Reverse();
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无报名!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }


        /// <summary>
        /// 获取活动的所有报名列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <param name="PageCount"></param>
        /// <param name="PageIndex"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("getSignUplistByShow"), HttpGet]
        public ResultObject getSignUplistByShow(int PartyID, int PageIndex, int PageCount, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            List<CardPartySignUpViewVO> cVO = cBO.FindSignUpViewByPartyID(PartyID, false);
            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    cVO.Reverse();
                    int numberOfPeople = cBO.FindCardPartSignInSumCount("Number", "PartyID=" + PartyID + " and (SignUpStatus=1 or SignUpStatus=0)"); //总人数
                    decimal Earning = 0; //总金额
                    List<CostItem> CostList = new List<CostItem>();

                    for (int i = 0; i < cVO.Count; i++)
                    {
                        if (cVO[i].OrderStatus == 1 || cVO[i].PartyOrderID == 0)
                        {
                            string CostName = cVO[i].CostName;

                            if (CostName == "")
                            {
                                CostName = "免费";
                            }

                            if (CostList.Exists(p => p.CostName == CostName))
                            {
                                CostList.FirstOrDefault(p => p.CostName == CostName).Cost += cVO[i].Cost;
                                CostList.FirstOrDefault(p => p.CostName == CostName).People += 1;
                            }
                            else
                            {
                                CostItem CostItem = new CostItem();
                                CostItem.CostName = CostName;
                                CostItem.Cost = cVO[i].Cost;
                                CostItem.People = 1;
                                CostList.Add(CostItem);
                            }
                            Earning += cVO[i].Cost;
                        }
                    }

                    /*
                    for (int i = 0; i < cVO.Count; i++)  //外循环是循环的次数
                    {
                        for (int j = cVO.Count - 1; j > i; j--)  //内循环是 外循环一次比较的次数
                        {

                            if (cVO[i].Name == cVO[j].Name&& cVO[i].CustomerId == cVO[j].CustomerId)
                            {
                                cVO.RemoveAt(j);
                            }

                        }
                    }*/

                    int Costcount = CostList.Count;//总收费项数量
                    bool isHost = false;//是否是主办方
                    CardPartyVO pVO = cBO.FindPartyById(PartyID);
                    if (pVO != null)
                    {
                        isHost = pVO.CustomerId == customerId;
                    }

                    cVO.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));
                    cVO.Reverse();

                    int ReadCount = cBO.FindAccessrecordsCount("Type='ReadParty' and ById=" + PartyID);
                    int ForwardCount = cBO.FindAccessrecordsCount("Type='ForwardParty' and ById=" + PartyID);
                    int SignUpPartyCount = cBO.FindAccessrecordsCount("Type='SignUpParty' and ById=" + PartyID);

                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = cVO.Count, Subsidiary = new { numberOfPeople = numberOfPeople, Earning = Earning, CostList = CostList, Costcount = Costcount, isHost = isHost, ForwardCount = ForwardCount, SignUpPartyCount = SignUpPartyCount, ReadCount = ReadCount } };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无报名!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取活动的中奖名单
        /// </summary>
        /// <param name="PartyID"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("getDrawAPrizelist"), HttpGet]
        public ResultObject getDrawAPrizelist(int PartyID, int PageIndex, int PageCount, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            List<CardPartySignUpVO> SignUpVO = cBO.FindSignUpByCondtion("PartyID=" + PartyID + " and LuckDrawStatus=1");
            if (SignUpVO != null)
            {
                if (SignUpVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = SignUpVO.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = SignUpVO.Count };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无中奖名单!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取名片组所有名片的Excel文件
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getGroupCardToExcel"), HttpGet]
        public ResultObject getGroupCardToExcel(int GroupID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            List<CardGroupCardViewVO> uVO = cBO.FindCardGroupCardViewByGroupID(GroupID);

            if (uVO != null)
            {
                if (uVO.Count > 0)
                {
                    DataTable dt = new DataTable();

                    dt.Columns.Add("序号", typeof(Int32));
                    dt.Columns.Add("姓名", typeof(String));
                    dt.Columns.Add("手机", typeof(String));
                    dt.Columns.Add("单位名称", typeof(String));
                    dt.Columns.Add("主营业务", typeof(String));
                    dt.Columns.Add("职位", typeof(String));
                    dt.Columns.Add("地址", typeof(String));
                    dt.Columns.Add("邮箱", typeof(String));
                    dt.Columns.Add("固定电话", typeof(String));
                    dt.Columns.Add("微信", typeof(String));
                    dt.Columns.Add("官网", typeof(String));

                    int j = 1;
                    for (int i = 0; i < uVO.Count; i++)
                    {
                        if (uVO[i].Status > 0)
                        {
                            DataRow row = dt.NewRow();
                            row["序号"] = j;
                            row["姓名"] = uVO[i].Name;
                            row["手机"] = uVO[i].Phone;
                            row["单位名称"] = uVO[i].CorporateName;
                            row["主营业务"] = uVO[i].Business;
                            row["职位"] = uVO[i].Position;
                            row["地址"] = uVO[i].Address;
                            row["邮箱"] = uVO[i].Email;
                            row["固定电话"] = uVO[i].Tel;
                            row["微信"] = uVO[i].WeChat;
                            row["官网"] = uVO[i].WebSite;
                            dt.Rows.Add(row);
                            j++;
                        }
                    }

                    string FileName = cBO.DataToExcel(dt, "GroupCardExcel/", GroupID + ".xls");
                    if (FileName != null)
                    {
                        return new ResultObject() { Flag = 1, Message = "获取成功!", Result = FileName };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无名片!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取搜客的Excel文件
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("companyupExcel"), HttpGet, Anonymous]
        public ResultObject companyupExcel()
        {
            CompanyBO CompanyBO = new CompanyBO(new CustomerProfile());
            List<SPLibrary.CustomerManagement.VO.CompanyVO> uVO = CompanyBO.FindCompanyUPByCondtion("1=1");

            if (uVO != null)
            {
                if (uVO.Count > 0)
                {
                    DataTable dt = new DataTable();

                    dt.Columns.Add("序号", typeof(Int32));
                    dt.Columns.Add("姓名", typeof(String));
                    dt.Columns.Add("手机", typeof(String));
                    dt.Columns.Add("单位名称", typeof(String));
                    dt.Columns.Add("地区", typeof(String));
                    dt.Columns.Add("地址", typeof(String));

                    int j = 1;
                    for (int i = 0; i < uVO.Count; i++)
                    {
                        DataRow row = dt.NewRow();
                        row["序号"] = j;
                        row["姓名"] = uVO[i].Contacts;
                        row["手机"] = uVO[i].Tel;
                        row["单位名称"] = uVO[i].CompanyName;
                        row["地址"] = uVO[i].Address;
                        row["地区"] = uVO[i].Location;
                        dt.Rows.Add(row);
                        j++;
                    }
                    CardBO cBO = new CardBO(new CustomerProfile());
                    string FileName = cBO.DataToExcel(dt, "GroupCardExcel/", "22654.xls");
                    if (FileName != null)
                    {
                        return new ResultObject() { Flag = 1, Message = "获取成功!", Result = FileName };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无名片!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 下载活动所有报名的Excel文件
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("getSignUplistToExcel"), HttpGet]
        public ResultObject getSignUplistToExcel(int PartyID, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardPartyVO CardPartyVO = cBO.FindPartyById(PartyID);

            if (CardPartyVO == null) { return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null }; }
            if (uProfile.CustomerId != CardPartyVO.CustomerId) { return new ResultObject() { Flag = 0, Message = "权限不足，下载失败!", Result = null }; }

            List<CardPartySignUpViewVO> cVO = cBO.FindSignUpViewByPartyID(PartyID);
            List<CardPartyCostVO> CardPartyCost = cBO.FindCostByPartyID(PartyID);

            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    DataTable dt = new DataTable();

                    dt.Columns.Add("序号", typeof(Int32));
                    dt.Columns.Add("姓名", typeof(String));
                    dt.Columns.Add("手机", typeof(String));
                    dt.Columns.Add("报名时间", typeof(DateTime));
                    dt.Columns.Add("状态", typeof(String));

                    if (CardPartyCost.Count > 0)
                    {
                        dt.Columns.Add("报名项目", typeof(String));
                        dt.Columns.Add("报名金额", typeof(Decimal));
                    }
                    try
                    {
                        //获取所有填写信息
                        List<CardPartySignUpFormVO> SignUpFormVO = cBO.FindSignUpFormByPartyID(PartyID, 1);
                        for (int i = 0; i < SignUpFormVO.Count; i++)
                        {
                            if (SignUpFormVO[i].Name != "姓名" && SignUpFormVO[i].Name != "手机" && SignUpFormVO[i].Name != "")
                            {
                                string ColumnsName = SignUpFormVO[i].Name;
                                if (dt.Columns.IndexOf(ColumnsName) > -1)
                                {
                                    ColumnsName = ColumnsName + cBO.RndCode(4) + i;
                                }
                                dt.Columns.Add(ColumnsName, typeof(String));
                            }
                        }
                        dt.Columns.Add("邀约人(分享来源)", typeof(String));
                        for (int i = 0; i < cVO.Count; i++)
                        {

                            DataRow row = dt.NewRow();
                            row["序号"] = i + 1;
                            row["姓名"] = cVO[i].Name;
                            row["手机"] = cVO[i].Phone;
                            row["报名时间"] = cVO[i].CreatedAt;

                            if (cVO[i].Type == 1)
                            {
                                if (cVO[i].SignUpStatus == 0)
                                {
                                    row["状态"] = "未核销";
                                }
                                else if (cVO[i].SignUpStatus == 2)
                                {
                                    row["状态"] = "已退费";
                                }
                                else
                                {
                                    row["状态"] = "已核销";
                                }
                            }

                            if (CardPartyCost.Count > 0)
                            {
                                row["报名项目"] = cVO[i].CostName;
                                row["报名金额"] = cVO[i].Cost;
                            }

                            List<CardPartySignUpFormVO> svO = cBO.FindSignUpFormByFormStr(cVO[i].SignUpForm);
                            for (int j = 0; j < svO.Count; j++)
                            {
                                if (svO[j].Name != "姓名" && svO[j].Name != "手机" && svO[j].Name != "")
                                {
                                    if (!dt.Columns.Contains(svO[j].Name))
                                    {
                                        dt.Columns.Add(svO[j].Name, typeof(String));
                                    }
                                    row[svO[j].Name] = svO[j].value;
                                }

                            }


                            if (cVO[i].InviterCID > 0)
                            {
                                CustomerVO CVO = CustomerBO.FindCustomenById(cVO[i].InviterCID);
                                List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(cVO[i].InviterCID);

                                if (CVO != null)
                                {
                                    row["邀约人(分享来源)"] = CVO.CustomerName;
                                }

                                if (CardDataVO.Count > 0)
                                {
                                    row["邀约人(分享来源)"] = CardDataVO[0].Name;
                                }
                            }

                            dt.Rows.Add(row);//这样就可以添加了 
                        }

                        string FileName = cBO.DataToExcel(dt, "PartySignUpExcel/", PartyID + ".xls");

                        if (FileName != null)
                        {
                            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = FileName };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                        }
                    }
                    catch (Exception ex)
                    {
                        LogBO _log = new LogBO(typeof(CardBO));
                        string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                        _log.Error(strErrorMsg);
                        return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "暂无报名!", Result = null };
                }

            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }


        /// <summary>
        /// 下载活动所有报名的Excel文件(后台专用)
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("getSignUplistToExcel2"), HttpGet]
        public ResultObject getSignUplistToExcel2(int PartyID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId > 0)
            {
                CardBO cBO = new CardBO(new CustomerProfile(), 1);
                CardPartyVO CardPartyVO = cBO.FindPartyById(PartyID);

                if (CardPartyVO == null) { return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null }; }

                List<CardPartySignUpViewVO> cVO = cBO.FindSignUpViewByTrue(PartyID);
                List<CardPartyCostVO> CardPartyCost = cBO.FindCostByPartyID(PartyID);

                if (cVO != null)
                {
                    if (cVO.Count > 0)
                    {
                        DataTable dt = new DataTable();

                        dt.Columns.Add("序号", typeof(Int32));
                        dt.Columns.Add("姓名", typeof(String));
                        dt.Columns.Add("手机", typeof(String));
                        dt.Columns.Add("附件地址", typeof(String));
                        dt.Columns.Add("报名时间", typeof(DateTime));

                        if (CardPartyCost.Count > 0)
                        {
                            dt.Columns.Add("报名项目", typeof(String));
                            dt.Columns.Add("报名金额", typeof(Decimal));
                        }

                        //获取所有填写信息
                        List<CardPartySignUpFormVO> SignUpFormVO = cBO.FindSignUpFormByPartyID(PartyID, 1);
                        for (int i = 0; i < SignUpFormVO.Count; i++)
                        {
                            if (SignUpFormVO[i].Name != "姓名" && SignUpFormVO[i].Name != "手机")
                                dt.Columns.Add(SignUpFormVO[i].Name, typeof(String));
                        }

                        for (int i = 0; i < cVO.Count; i++)
                        {

                            DataRow row = dt.NewRow();
                            row["序号"] = i + 1;
                            row["姓名"] = cVO[i].Name;
                            row["手机"] = cVO[i].Phone;
                            row["报名时间"] = cVO[i].CreatedAt;
                            if (cVO[i].FjUrl == "")
                            {
                                row["附件地址"] = "未上传";
                            }
                            else {
                                row["附件地址"] = cVO[i].FjUrl;
                            }
                            
                            if (CardPartyCost.Count > 0)
                            {
                                row["报名项目"] = cVO[i].CostName;
                                row["报名金额"] = cVO[i].Cost;
                            }

                            try
                            {
                                List<CardPartySignUpFormVO> svO = cBO.FindSignUpFormByFormStr(cVO[i].SignUpForm);
                                for (int j = 0; j < svO.Count; j++)
                                {
                                    if (svO[j].Name != "姓名" && svO[j].Name != "手机")
                                    {
                                        if (!dt.Columns.Contains(svO[j].Name))
                                        {
                                            dt.Columns.Add(svO[j].Name, typeof(String));
                                        }
                                        row[svO[j].Name] = svO[j].value;
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                LogBO _log = new LogBO(typeof(CardBO));
                                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                                _log.Error(strErrorMsg);
                            }

                            dt.Rows.Add(row);//这样就可以添加了 
                        }

                        string FileName = cBO.DataToExcel(dt, "PartySignUpExcel/", PartyID + ".xls");

                        if (FileName != null)
                        {
                            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = FileName };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                        }
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "暂无报名!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }

        }

        /// <summary>
        /// 下载活动所有报名的Excel文件(多彩乡村网页端)
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("getSignUplistToExcel4"), HttpGet]
        public ResultObject getSignUplistToExcel4(int PartyID = 13900)
        {
            if (PartyID > 0)
            {
                CardBO cBO = new CardBO(new CustomerProfile(), 1);
                CardPartyVO CardPartyVO = cBO.FindPartyById(PartyID);

                if (CardPartyVO == null) { return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null }; }

                List<CardPartySignUpViewVO> cVO = cBO.FindSignUpViewByTrue(PartyID);
                List<CardPartyCostVO> CardPartyCost = cBO.FindCostByPartyID(PartyID);

                if (cVO != null)
                {
                    if (cVO.Count > 0)
                    {
                        DataTable dt = new DataTable();

                        dt.Columns.Add("序号", typeof(Int32));
                        dt.Columns.Add("姓名", typeof(String));
                        dt.Columns.Add("手机", typeof(String));
                        dt.Columns.Add("附件地址", typeof(String));
                        dt.Columns.Add("报名时间", typeof(DateTime));

                        if (CardPartyCost.Count > 0)
                        {
                            dt.Columns.Add("报名项目", typeof(String));
                            dt.Columns.Add("报名金额", typeof(Decimal));
                        }

                        //获取所有填写信息
                        List<CardPartySignUpFormVO> SignUpFormVO = cBO.FindSignUpFormByPartyID(PartyID, 1);
                        for (int i = 0; i < SignUpFormVO.Count; i++)
                        {
                            if (SignUpFormVO[i].Name != "姓名" && SignUpFormVO[i].Name != "手机")
                                dt.Columns.Add(SignUpFormVO[i].Name, typeof(String));
                        }

                        for (int i = 0; i < cVO.Count; i++)
                        {

                            DataRow row = dt.NewRow();
                            row["序号"] = i + 1;
                            row["姓名"] = cVO[i].Name;
                            row["手机"] = cVO[i].Phone;
                            row["报名时间"] = cVO[i].CreatedAt;
                            if (cVO[i].FjUrl == "")
                            {
                                row["附件地址"] = "未上传";
                            }
                            else
                            {
                                row["附件地址"] = cVO[i].FjUrl;
                            }

                            if (CardPartyCost.Count > 0)
                            {
                                row["报名项目"] = cVO[i].CostName;
                                row["报名金额"] = cVO[i].Cost;
                            }

                            try
                            {
                                List<CardPartySignUpFormVO> svO = cBO.FindSignUpFormByFormStr(cVO[i].SignUpForm);
                                for (int j = 0; j < svO.Count; j++)
                                {
                                    if (svO[j].Name != "姓名" && svO[j].Name != "手机")
                                    {
                                        if (!dt.Columns.Contains(svO[j].Name))
                                        {
                                            dt.Columns.Add(svO[j].Name, typeof(String));
                                        }
                                        row[svO[j].Name] = svO[j].value;
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                LogBO _log = new LogBO(typeof(CardBO));
                                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                                _log.Error(strErrorMsg);
                            }

                            dt.Rows.Add(row);//这样就可以添加了 
                        }

                        string FileName = cBO.DataToExcel(dt, "PartySignUpExcel/", PartyID + ".xls");

                        if (FileName != null)
                        {
                            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = FileName };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                        }
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "暂无报名!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }

        }


        /// <summary>
        /// 获取活动的邀请人列表数
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("getSignUpInviterlist"), HttpGet, Anonymous]
        public ResultObject getSignUpInviterlist(int PartyID, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            List<CardPartySignUpViewVO> cVO = cBO.FindSignUpViewInviterByPartyID(PartyID);
            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无数据!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取邀请人列表
        /// </summary>
        ///  <param name="InviterCID"></param>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("GetSignUpInviterlistData"), HttpGet, Anonymous]
        public ResultObject GetSignUpInviterlistData(int PartyID, int InviterCID, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            List<CardPartySignUpViewVO> cVO = cBO.FindSignUpViewByInviterCID(PartyID, InviterCID);
            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无数据!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }


        /// <summary>
        /// 获取活动分享浏览记录列表
        /// </summary>
        ///  <param name="InviterCID"></param>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("GetReadPartyData"), HttpGet, Anonymous]
        public ResultObject GetReadPartyData(int PartyID, int InviterCID, int PageCount, int PageIndex, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            List<CardPartySignUpViewVO> cVO = cBO.FindSignUpViewByInviterCID(PartyID, InviterCID);
            List<CardAccessRecordsViewVO> AccessVO = cBO.FindCardAccessRecordsViewByCondtion("ById=" + InviterCID + "  and (Type='ReadParty' or Type='ForwardParty')");


            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无数据!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }


        /// <summary>
        /// 更新报名列表备注
        /// </summary>
        /// <param name="remarkName">备注名</param>
        /// <param name="PartySignUpID">报名列表Id</param>
        /// <param name="HostCustomenId">主办方Id</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateSignUpBySUID"), HttpGet]
        public ResultObject UpdateSignUpBySUID(string remarkName, int PartySignUpID, int HostCustomenId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            if (uProfile.CustomerId != HostCustomenId) { return new ResultObject() { Flag = 2, Message = "权限不足，修改失败!", Result = null }; }

            try
            {
                CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
                CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
                CardPartySignUpVO csvo = new CardPartySignUpVO();
                csvo.remarkName = remarkName;
                csvo.PartySignUpID = PartySignUpID;

                Boolean ret = cBO.UpdateSignUp(csvo);
                if (ret) { return new ResultObject() { Flag = 1, Message = "修改成功!", Result = null }; }
                else { return new ResultObject() { Flag = 0, Message = "修改失败!", Result = null }; }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "修改失败!", Result = ex };
            }


        }

        /// <summary>
        /// 更新发货状态
        /// </summary>
        /// <param name="LogisticsOrderNo">备注名</param>
        /// <param name="PartySignUpID">报名列表Id</param>
        /// <param name="HostCustomenId">主办方Id</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateSignUpByDeliver"), HttpGet]
        public ResultObject UpdateSignUpByDeliver(string LogisticsOrderNo, int PartySignUpID, int HostCustomenId, string token, int AppType = 1)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            if (uProfile.CustomerId != HostCustomenId) { return new ResultObject() { Flag = 2, Message = "权限不足，发货失败!", Result = null }; }
            if (LogisticsOrderNo == "") { return new ResultObject() { Flag = 0, Message = "物流单号不能为空!", Result = null }; }

            try
            {
                CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);

                CardBO cBO = new CardBO(new CustomerProfile(), AppType);
                CardPartySignUpVO csvo = new CardPartySignUpVO();
                csvo.PartySignUpID = PartySignUpID;
                csvo.LogisticsOrderNo = LogisticsOrderNo;
                csvo.isDeliver = 1;
                csvo.DeliverAt = DateTime.Now;

                Boolean ret = cBO.UpdateSignUp(csvo);
                if (ret)
                {
                    CardPartySignUpViewVO sVO = cBO.FindSignUpViewById(PartySignUpID);
                    int r = cBO.AddCardMessage("您购买的商品已经发货", sVO.CustomerId, "活动报名", "/package/package_order/OderDetail/OderDetail?PartyOrderID=" + sVO.PartyOrderID);
                    return new ResultObject() { Flag = 1, Message = "发货成功!", Result = r };
                }
                else { return new ResultObject() { Flag = 0, Message = "发货失败!", Result = null }; }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "发货失败!", Result = ex };
            }


        }



        /// <summary>
        /// 获取活动专属名片组
        /// </summary>
        /// <param name="PartyID">活动ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetGroupByParty"), HttpGet, Anonymous]
        public ResultObject GetGroupByParty(int PartyID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            CardPartyVO cpVO = cBO.FindPartyById(PartyID);
            if (cpVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }

            if (cpVO.GroupID == 0)
            {
                return new ResultObject() { Flag = 0, Message = "本活动无名片组!", Result = null };
            }

            List<CardGroupCardViewVO> CardGroupCardList = cBO.FindCardGroupCardViewByGroupID(cpVO.GroupID);
            if (CardGroupCardList != null)
            {
                bool isJionGroup = false;

                for (int i = 0; i < CardGroupCardList.Count; i++)
                {
                    if (CardGroupCardList[i].CustomerId == customerId && CardGroupCardList[i].Status != 0)
                    {
                        isJionGroup = true;
                    }
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { CardGroupCardList, isJionGroup } };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我发布的活动
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyPartylist"), HttpGet]
        public ResultObject getMyPartylist(string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            if (AppType == 1)
            {
                AppType = CustomerVO2.AppType;
            }
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            List<CardPartyViewVO> uVO = cBO.FindPartyByCustomerId(customerId);

            if (uVO != null)
            {
                for (int i = 0; i < uVO.Count; i++)
                {
                    //uVO[i].CardPartySignUp = cBO.FindSignUpByPartyID(uVO[i].PartyID);
                    //uVO[i].CardPartyContactsView = cBO.FindPartyContactsByPartyId(uVO[i].PartyID);
                    //uVO[i].CardPartySignUpForm = cBO.FindSignUpFormByPartyID(uVO[i].PartyID);
                    //uVO[i].CardPartyCost = cBO.FindCostByPartyID(uVO[i].PartyID);
                    //uVO[i].SignupCount = uVO[i].CardPartySignUp.Count;

                    CustomerVO CustomerVO = uBO.FindCustomenById(uVO[i].CustomerId);
                    List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(uVO[i].CustomerId);

                    if (CustomerVO != null)
                    {
                        uVO[i].HeaderLogo = CustomerVO.HeaderLogo;
                        uVO[i].CustomerName = CustomerVO.CustomerName;
                    }

                    if (CardDataVO.Count > 0)
                    {
                        uVO[i].Name = CardDataVO[0].Name;
                        uVO[i].Headimg = CardDataVO[0].Headimg;
                    }
                }
                int BeoverdueCount = cBO.FindMyPartyBeoverdueCount(customerId);
                if (uVO.Count > 0)
                {
                    uVO.Sort((a, b) => a.StartTime.CompareTo(b.CreatedAt));
                    uVO.Reverse();

                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO, Subsidiary = BeoverdueCount };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "最近没有发布活动!", Result = null, Subsidiary = BeoverdueCount };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我发布的活动 分页
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyPartylist"), HttpGet]
        public ResultObject getMyPartylist(int PageCount, int PageIndex, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);

            if (AppType == 1)
            {
                AppType = CustomerVO2.AppType;
            }
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            List<CardPartyViewVO> uVO = cBO.FindPartyByCustomerId(customerId);

            if (uVO != null)
            {
                uVO.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));
                uVO.Reverse();
                IEnumerable<CardPartyViewVO> newVO = uVO.Skip((PageIndex - 1) * PageCount).Take(PageCount);
                foreach (CardPartyViewVO item in newVO)
                {
                    //item.CardPartySignUp = cBO.FindSignUpByPartyID(item.PartyID);
                    //item.CardPartyContactsView = cBO.FindPartyContactsByPartyId(item.PartyID);
                    //item.CardPartySignUpForm = cBO.FindSignUpFormByPartyID(item.PartyID);
                    //item.CardPartyCost = cBO.FindCostByPartyID(item.PartyID);
                    //item.SignupCount = item.CardPartySignUp.Count;

                    CustomerVO CustomerVO = uBO.FindCustomenById(item.CustomerId);
                    List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(item.CustomerId);

                    if (CustomerVO != null)
                    {
                        item.HeaderLogo = CustomerVO.HeaderLogo;
                        item.CustomerName = CustomerVO.CustomerName;
                    }

                    if (CardDataVO.Count > 0)
                    {
                        item.Name = CardDataVO[0].Name;
                        item.Headimg = CardDataVO[0].Headimg;
                    }
                }

                int BeoverdueCount = cBO.FindMyPartyBeoverdueCount(customerId);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newVO, Subsidiary = BeoverdueCount, Count = uVO.Count };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取指定会员发布的活动
        /// </summary>
        /// <returns></returns>
        [Route("getPartylistByCustomerId"), HttpGet, Anonymous]
        public ResultObject getPartylistByCustomerId(int PageCount, int PageIndex, int CustomerId, int AppType = 1)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            List<CardPartyViewVO> uVO = cBO.FindPartyByCustomerId(CustomerId);

            if (uVO != null)
            {
                uVO.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
                uVO.Reverse();
                IEnumerable<CardPartyViewVO> newVO = uVO.Skip((PageIndex - 1) * PageCount).Take(PageCount);
                foreach (CardPartyViewVO item in newVO)
                {
                    //item.CardPartySignUp = cBO.FindSignUpByPartyID(item.PartyID);
                    //item.CardPartyContactsView = cBO.FindPartyContactsByPartyId(item.PartyID);
                    //item.CardPartySignUpForm = cBO.FindSignUpFormByPartyID(item.PartyID);
                    //item.CardPartyCost = cBO.FindCostByPartyID(item.PartyID);
                    //item.SignupCount = item.CardPartySignUp.Count;

                    CustomerVO CustomerVO = uBO.FindCustomenById(item.CustomerId);
                    List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(item.CustomerId);

                    if (CustomerVO != null)
                    {
                        item.HeaderLogo = CustomerVO.HeaderLogo;
                        item.CustomerName = CustomerVO.CustomerName;
                    }

                    if (CardDataVO.Count > 0)
                    {
                        item.Name = CardDataVO[0].Name;
                        item.Headimg = CardDataVO[0].Headimg;
                    }
                }

                int BeoverdueCount = cBO.FindMyPartyBeoverdueCount(CustomerId);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newVO, Subsidiary = BeoverdueCount, Count = uVO.Count };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取加载可以复制的活动
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyCopyPartylist"), HttpGet]
        public ResultObject getMyCopyPartylist(string token, string keyword = "")
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            //先查询自己发布的活动
            List<CardPartyVO> uVO = cBO.FindParty(customerId, 3);

            //查询是否有企业名片，如果有就把同企业成员的活动添加上列表
            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                if (pVO.BusinessID > 0)
                {
                    List<PersonalVO> Personal = bBO.FindPersonalByBusinessID(pVO.BusinessID);
                    for (int i = 0; i < Personal.Count; i++)
                    {
                        if (Personal[i].CustomerId != customerId)
                        {
                            List<CardPartyVO> cpVO = cBO.FindParty(Personal[i].CustomerId, 3);
                            uVO.AddRange(cpVO);
                        }
                    }
                }
            }

            //根据创建时间重新排序
            uVO.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));
            uVO.Reverse();
            List<CardPartyViewVO> CardPartyVO = new List<CardPartyViewVO>();
            if (keyword == "")
            {
                //保留前50条数据
                for (int i = 0; i < uVO.Count && i < 50; i++)
                {
                    CardPartyVO.Add(cBO.getPartyView(uVO[i]));
                }

            }
            else
            {
                //保留符合关键字的活动
                for (int i = 0; i < uVO.Count; i++)
                {
                    if (uVO[i].Title.IndexOf(keyword) > -1)
                        CardPartyVO.Add(cBO.getPartyView(uVO[i]));
                }
            }

            if (CardPartyVO != null)
            {
                if (CardPartyVO.Count > 0)
                {
                    foreach (CardPartyViewVO item in CardPartyVO)
                    {
                        item.CardPartyCost = cBO.FindCostByPartyID(item.PartyID);
                        item.CardPartySignUpForm = cBO.FindSignUpFormByPartyID(item.PartyID);
                    }
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CardPartyVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "没有参加活动!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取企业名片
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyBusinessCard"), HttpGet]
        public ResultObject getMyBusinessCard(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            //查询是否有企业名片，如果有就把同企业成员的活动添加上列表
            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = bBO.FindPersonalByCustomerId(customerId);
            if (pVO != null)
            {
                BusinessCardVO bVO = new BusinessCardVO();

                if (pVO.BusinessID == 0)
                {
                    bVO = null;
                }
                else
                {
                    bVO = bBO.FindBusinessCardById(pVO.BusinessID);
                }
                //清除营业执照等保密信息
                bVO.BusinessLicenseImg = "";

                List<SecondBusinessViewVO> ListSecondBusiness = bBO.FindSecondBusinessViewByPersonalID(pVO.PersonalID);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = pVO, Subsidiary = new { ListSecondBusiness = ListSecondBusiness, BusinessCard = bVO } };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "没有企业名片!", Result = null };
            }
        }

        /// <summary>
        /// 获取我报名的活动 分页
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMySignUpPartylist"), HttpGet]
        public ResultObject getMySignUpPartylist(int PageCount, int PageIndex, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            if (AppType == 1)
            {
                AppType = CustomerVO2.AppType;
            }
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<CardPartySignUpViewVO> uVO = cBO.FindSignUpViewByCustomerId(customerId);
            if (uVO != null)
            {
                uVO.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
                uVO.Reverse();
                IEnumerable<CardPartySignUpViewVO> newVO = uVO.Skip((PageIndex - 1) * PageCount).Take(PageCount);
                foreach (CardPartySignUpViewVO item in newVO)
                {
                    //item.CardPartySignUp = cBO.FindSignUpByPartyID(item.PartyID);

                    CustomerVO CVO = uBO.FindCustomenById(item.HostCustomerId);
                    List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(item.HostCustomerId);

                    if (CVO != null)
                    {
                        item.HeaderLogo = CVO.HeaderLogo;
                        item.CustomerName = CVO.CustomerName;
                    }

                    if (CardDataVO.Count > 0)
                    {
                        item.Name = CardDataVO[0].Name;
                        item.Headimg = CardDataVO[0].Headimg;
                    }
                }
                int BeoverdueCount = cBO.FindSignUpViewBeoverdueCount(customerId);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newVO, Count = uVO.Count, Subsidiary = BeoverdueCount };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我报名的活动
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMySignUpPartylist"), HttpGet]
        public ResultObject getMySignUpPartylist(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            List<CardPartySignUpViewVO> uVO = cBO.FindSignUpViewByCustomerId(customerId);

            if (uVO != null)
            {
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                for (int i = 0; i < uVO.Count; i++)
                {
                    //uVO[i].CardPartySignUp = cBO.FindSignUpByPartyID(uVO[i].PartyID);

                    CustomerVO CVO = uBO.FindCustomenById(uVO[i].HostCustomerId);
                    List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(uVO[i].HostCustomerId);

                    if (CVO != null)
                    {
                        uVO[i].HeaderLogo = CVO.HeaderLogo;
                        uVO[i].CustomerName = CVO.CustomerName;
                    }

                    if (CardDataVO.Count > 0)
                    {
                        uVO[i].Name = CardDataVO[0].Name;
                        uVO[i].Headimg = CardDataVO[0].Headimg;
                    }
                }
                int BeoverdueCount = cBO.FindSignUpViewBeoverdueCount(customerId);
                if (uVO.Count > 0)
                {
                    uVO.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
                    uVO.Reverse();

                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO, Subsidiary = BeoverdueCount };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "没有参加活动!", Result = null, Subsidiary = BeoverdueCount };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我的报名列表和我发布的活动列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FindPartyAndSignUpByCustomerId"), HttpGet]
        public ResultObject FindPartyAndSignUpByCustomerId(string token, string keyword = "")
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            List<CardPartySignUpViewVO> uVO = cBO.FindPartyAndSignUpByCustomerId(customerId);
            List<CardPartySignUpViewVO> CardPartyVO = new List<CardPartySignUpViewVO>();

            if (keyword == "")
            {
                //保留前50条数据
                for (int i = 0; i < uVO.Count && i < 50; i++)
                {
                    CardPartyVO.Add(uVO[i]);
                }

            }
            else
            {
                //保留符合关键字的活动
                for (int i = 0; i < uVO.Count; i++)
                {
                    if (uVO[i].Title.IndexOf(keyword) > -1)
                        CardPartyVO.Add(uVO[i]);
                }
            }

            if (CardPartyVO != null)
            {
                if (CardPartyVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CardPartyVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "没有活动!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我的订单列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyPartyOrderlist"), HttpGet]
        public ResultObject getMyPartyOrderlist(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            List<CardPartyOrderViewVO> uVO = cBO.FindPartyOrderViewByCustomerId(customerId);

            if (uVO != null)
            {
                if (uVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无订单!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我的订单数量
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyPartyOrderCount"), HttpGet]
        public ResultObject getMyPartyOrderCount(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            List<CardPartyOrderViewVO> uVO = cBO.FindPartyOrderViewByCustomerId(customerId);

            if (uVO != null)
            {
                if (uVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO.Count };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无订单!", Result = 0 };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = 0 };
            }
        }

        /// <summary>
        /// 获取活动订单
        /// </summary>
        /// <param name="PartyOrderID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getPartyOrderDetails"), HttpGet]
        public ResultObject getPartyOrderDetails(int PartyOrderID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardPartyOrderViewVO cVO = cBO.FindPartyOrderViewById(PartyOrderID);

            if (cVO == null)
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };


            if (cVO.CustomerId != customerId)
                return new ResultObject() { Flag = 0, Message = "只有本人才能查看订单!", Result = null };


            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
        }


        /// <summary>
        /// 活动订单对账
        /// </summary>
        /// <param name="HelpID">帮助ID</param>
        /// <returns></returns>
        [Route("Reconciliation"), HttpGet, Anonymous]
        public ResultObject Reconciliation(int customerId)
        {
            try
            {
                JsApiPay Ja = new JsApiPay();

                CardBO cBO = new CardBO(new CustomerProfile());
                List<CardPartyOrderViewVO> oeder = cBO.FindPartyOrderViewByHost(customerId);
                decimal total = 0;
                List<OrderQueryData> rightOrder = new List<OrderQueryData>();
                List<CardPartyOrderViewVO> errOrder = new List<CardPartyOrderViewVO>();
                foreach (CardPartyOrderViewVO item in oeder)
                {
                    OrderQueryData wp = Ja.OrderQueryResult(item.OrderNO);
                    if (wp != null)
                    {
                        total += wp.total;
                        rightOrder.Add(wp);
                    }
                    else
                    {
                        errOrder.Add(item);
                    }

                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = "活动订单总收入：" + total / 100, Subsidiary = errOrder };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 企业订单对账
        /// </summary>
        /// <param name="HelpID">帮助ID</param>
        /// <returns></returns>
        [Route("BusinessReconciliation"), HttpGet, Anonymous]
        public ResultObject BusinessReconciliation(int BusinessID)
        {
            try
            {
                JsApiPay Ja = new JsApiPay();
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                List<OrderViewVO> oeder = cBO.FindOrderViewList("ProdustsBusinessID = " + BusinessID + " and Status=1 and isAgentBuy=0 and isEcommerceBuy=0");
                decimal total = 0;
                List<OrderQueryData> rightOrder = new List<OrderQueryData>();
                List<OrderViewVO> errOrder = new List<OrderViewVO>();
                foreach (OrderViewVO item in oeder)
                {
                    OrderQueryData wp = Ja.OrderQueryResult(item.OrderNO);
                    if (wp != null)
                    {
                        total += wp.total;
                        rightOrder.Add(wp);
                    }
                    else
                    {
                        OrderVO newOrderVO = new OrderVO();
                        newOrderVO.OrderID = item.OrderID;
                        newOrderVO.isAgentBuy = 1;
                        cBO.UpdateOrder(newOrderVO);
                        errOrder.Add(item);
                    }
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = "企业订单总收入：" + total / 100, Subsidiary = errOrder };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }


        }

        /// <summary>
        /// 获取活动已退款订单
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("getPartyPayoutOrder"), HttpGet, Anonymous]
        public async Task<ResultObject> getPartyPayoutOrder()
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            EcommerceBO eBO = new EcommerceBO();
            List<CardPartyOrderViewVO> cVO = cBO.GetPartyOrderViewVO("Status=2 and RefundStatus=0 and IsPayOut=0");

            string s = "";
            foreach (CardPartyOrderViewVO item in cVO)
            {
                JsApiPay Ja = new JsApiPay();

                if (item.Cost > 0)
                {
                    string total_fee_1 = Convert.ToInt32((item.Cost * 100)).ToString();

                    if (item.sub_mchid != "")
                    {
                        ResultCode response = await eBO.Refund(item.sp_appid, item.sub_mchid, item.OrderNO, Convert.ToInt32((item.Cost * 100)));
                        if (response.code == "SUCCESS")
                        {
                            CardPartyOrderVO cpVO = cBO.FindPartyOrderById(item.PartyOrderID);
                            cpVO.Status = 2;
                            cpVO.RefundAt = DateTime.Now;
                            cpVO.RefundStatus = 1;
                            s += item.OrderNO + ":退款成功\r\n";
                            cBO.UpdatePartyOrder(cpVO);
                        }
                    }
                    else
                    {
                        WxPayData wp = Ja.GetRefundResult(item.OpenId, total_fee_1, total_fee_1, item.OrderNO, item.AppType);

                        if (wp != null)
                        {
                            string reslut = Ja.GetJsApiParameters(wp, item.AppType);
                            if (reslut != "")
                            {
                                CardPartyOrderVO cpVO = cBO.FindPartyOrderById(item.PartyOrderID);
                                cpVO.Status = 2;
                                cpVO.RefundAt = DateTime.Now;
                                if (wp.GetValue("result_code").ToString() == "SUCCESS")
                                {
                                    cpVO.RefundStatus = 1;
                                    s += item.OrderNO + ":退款成功\r\n";
                                }
                                cBO.UpdatePartyOrder(cpVO);
                            }
                        }
                    }
                }
            }
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO, Subsidiary = s };
        }

        /// <summary>
        /// 获取活动举办方查询收入订单
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getPartyOrderListByHost"), HttpGet]
        public ResultObject getPartyOrderListByHost(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            List<CardOrderListVO> CardOrderList = new List<CardOrderListVO>();

            //活动收入
            List<CardPartyOrderViewVO> uVO = cBO.FindPartyOrderViewByHost(customerId);

            if (uVO != null)
            {
                for (int i = 0; i < uVO.Count; i++)
                {
                    CardOrderListVO CardOrderListVO = new CardOrderListVO();
                    CardOrderListVO.Title = uVO[i].CostName;
                    CardOrderListVO.Name = uVO[i].Name;
                    CardOrderListVO.Headimg = uVO[i].Headimg;
                    CardOrderListVO.Cost = uVO[i].Cost - uVO[i].PromotionAwardCost;
                    CardOrderListVO.CreatedAt = uVO[i].CreatedAt;
                    CardOrderListVO.Type = "活动收入";
                    CardOrderListVO.CostName = uVO[i].CostName;
                    CardOrderListVO.Status = uVO[i].Status;

                    CardOrderList.Add(CardOrderListVO);
                }
            }

            //活动邀约提成
            List<CardPartyOrderViewVO> uVO2 = cBO.GetPartyOrderViewVO("InviterCID=" + customerId + " and PromotionAwardCost>0 and PromotionAwardStatus=1");

            if (uVO2 != null)
            {
                for (int i = 0; i < uVO2.Count; i++)
                {
                    CardOrderListVO CardOrderListVO = new CardOrderListVO();
                    CardOrderListVO.Title = uVO2[i].CostName;
                    CardOrderListVO.Name = uVO2[i].Name;
                    CardOrderListVO.Headimg = uVO2[i].Headimg;
                    CardOrderListVO.Cost = uVO2[i].PromotionAwardCost;
                    CardOrderListVO.CreatedAt = uVO2[i].CreatedAt;
                    CardOrderListVO.Type = "活动邀约";
                    CardOrderList.Add(CardOrderListVO);
                }
            }

            //返利收入
            List<CardOrderViewVO> cVO = cBO.FindOrderViewByCondtion("(OneRebateCustomerId=" + customerId + " or TwoRebateCustomerId=" + customerId + ") and Status=1 and payAt is not NULL");

            if (cVO != null)
            {
                for (int i = 0; i < cVO.Count; i++)
                {
                    CardOrderListVO CardOrderListVO = new CardOrderListVO();
                    if (cVO[i].OneRebateCustomerId == customerId)
                    {
                        CardOrderListVO.Title = "一级邀约返利";
                        CardOrderListVO.Cost = cVO[i].OneRebateCost;
                        CardOrderListVO.CostName = "一级邀约返利";
                    }
                    else if (cVO[i].TwoRebateCustomerId == customerId)
                    {
                        CardOrderListVO.Title = "二级邀约返利";
                        CardOrderListVO.Cost = cVO[i].TwoRebateCost;
                        CardOrderListVO.CostName = "二级邀约返利";
                    }
                    CardOrderListVO.Name = cVO[i].CustomerName;
                    CardOrderListVO.Headimg = cVO[i].HeaderLogo;
                    CardOrderListVO.CreatedAt = cVO[i].CreatedAt;
                    CardOrderListVO.Type = "邀约返利";
                    CardOrderListVO.Status = cVO[i].Status;

                    CardOrderList.Add(CardOrderListVO);
                }
            }

            //软文收入
            List<CardSoftArticleOrderViewVO> sVO = cBO.FindSoftArticleOrderViewByConditionStr("OriginalCustomerId=" + customerId + " and Status=1 and payAt is not NULL");

            if (sVO != null)
            {
                for (int i = 0; i < sVO.Count; i++)
                {
                    CardOrderListVO CardOrderListVO = new CardOrderListVO();
                    CardOrderListVO.Title = "软文转载收入";
                    CardOrderListVO.Cost = sVO[i].Cost;
                    CardOrderListVO.CostName = sVO[i].Title;
                    CardOrderListVO.Name = sVO[i].CustomerName;
                    CardOrderListVO.Headimg = sVO[i].HeaderLogo;
                    CardOrderListVO.CreatedAt = sVO[i].CreatedAt;
                    CardOrderListVO.Type = "软文收入";
                    CardOrderListVO.Status = sVO[i].Status;

                    CardOrderList.Add(CardOrderListVO);
                }
            }

            CardOrderList.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));
            CardOrderList.Reverse();


            if (CardOrderList != null)
            {
                if (CardOrderList.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CardOrderList };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无订单!", Result = 0 };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = 0 };
            }
        }

        /// <summary>
        /// 查询收入订单
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getOrderList"), HttpGet]
        public ResultObject getOrderList(int PageCount, int PageIndex, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            List<CardOrderListVO> CardOrderList = new List<CardOrderListVO>();

            //活动收入
            List<CardPartyOrderViewVO> uVO = cBO.FindPartyOrderViewByHost(customerId);

            if (uVO != null)
            {
                for (int i = 0; i < uVO.Count; i++)
                {
                    CardOrderListVO CardOrderListVO = new CardOrderListVO();
                    CardOrderListVO.Title = uVO[i].CostName;
                    CardOrderListVO.Name = uVO[i].Name;
                    CardOrderListVO.Headimg = uVO[i].Headimg;
                    CardOrderListVO.Cost = uVO[i].Cost - uVO[i].PromotionAwardCost;
                    CardOrderListVO.CreatedAt = uVO[i].CreatedAt;
                    CardOrderListVO.Type = "活动收入";
                    CardOrderListVO.Status = 1;
                    CardOrderList.Add(CardOrderListVO);
                }
            }

            //活动邀约提成
            List<CardPartyOrderViewVO> uVO2 = cBO.GetPartyOrderViewVO("InviterCID=" + customerId + " and PromotionAwardCost>0 and PromotionAwardStatus=1");

            if (uVO2 != null)
            {
                for (int i = 0; i < uVO2.Count; i++)
                {
                    CardOrderListVO CardOrderListVO = new CardOrderListVO();
                    CardOrderListVO.Title = uVO2[i].CostName;
                    CardOrderListVO.Name = uVO2[i].Name;
                    CardOrderListVO.Headimg = uVO2[i].Headimg;
                    CardOrderListVO.Cost = uVO2[i].PromotionAwardCost;
                    CardOrderListVO.CreatedAt = uVO2[i].CreatedAt;
                    CardOrderListVO.Type = "邀约返利";
                    CardOrderListVO.Status = 1;
                    CardOrderList.Add(CardOrderListVO);
                }
            }

            //返利收入
            List<CardOrderViewVO> cVO = cBO.FindOrderViewByCondtion("(OneRebateCustomerId=" + customerId + " or TwoRebateCustomerId=" + customerId + ") and Status=1 and payAt is not NULL");

            if (cVO != null)
            {
                for (int i = 0; i < cVO.Count; i++)
                {
                    CardOrderListVO CardOrderListVO = new CardOrderListVO();
                    if (cVO[i].OneRebateCustomerId == customerId)
                    {
                        CardOrderListVO.Title = "一级邀约返利";
                        CardOrderListVO.Cost = cVO[i].OneRebateCost;
                    }
                    else if (cVO[i].TwoRebateCustomerId == customerId)
                    {
                        CardOrderListVO.Title = "二级邀约返利";
                        CardOrderListVO.Cost = cVO[i].TwoRebateCost;
                    }
                    CardOrderListVO.Name = cVO[i].CustomerName;
                    CardOrderListVO.Headimg = cVO[i].HeaderLogo;
                    CardOrderListVO.CreatedAt = cVO[i].CreatedAt;
                    CardOrderListVO.Type = "邀约返利";
                    CardOrderListVO.Status = 1;

                    CardOrderList.Add(CardOrderListVO);
                }
            }

            //软文收入
            List<CardSoftArticleOrderViewVO> sVO = cBO.FindSoftArticleOrderViewByConditionStr("OriginalCustomerId=" + customerId + " and Status=1 and Cost>0 and payAt is not NULL");

            if (sVO != null)
            {
                for (int i = 0; i < sVO.Count; i++)
                {
                    CardOrderListVO CardOrderListVO = new CardOrderListVO();
                    CardOrderListVO.Title = "软文转载收入";
                    CardOrderListVO.Cost = sVO[i].Cost;
                    CardOrderListVO.CostName = sVO[i].Title;
                    CardOrderListVO.Name = sVO[i].CustomerName;
                    CardOrderListVO.Headimg = sVO[i].HeaderLogo;
                    CardOrderListVO.CreatedAt = sVO[i].CreatedAt;
                    CardOrderListVO.Type = "软文收入";
                    CardOrderListVO.Status = sVO[i].Status;

                    CardOrderList.Add(CardOrderListVO);
                }
            }

            CardOrderList.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));
            CardOrderList.Reverse();

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CardOrderList.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = CardOrderList.Count };
        }

        
        /// <summary>
        /// 获取收入余额
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyCardBalance"), HttpGet]
        public ResultObject getMyCardBalance(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            //先查询并结算冻结金额
            FrozenBalanceVO fVO = cBO.FindFrozenBalanceByCustomerId(customerId);
            decimal FrozenBalance = fVO.SumBalance;
            decimal CardBalance = cBO.FindCardBalanceByCustomerId(customerId);

            int SignUpCount = cBO.FindSignUpViewLuckDrawCount(customerId);
            int PartyCount = cBO.FindPartyTotalCount("CustomerId=" + customerId + " and Status>0 and Type=3");
            int WinningCount = cBO.FindSignUpWinningCount(customerId);

            if (FrozenBalance == 0)
            {
                fVO.IsReward = true;
            }

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { FrozenBalance = FrozenBalance, CardBalance = CardBalance, SignUpCount = SignUpCount, PartyCount = PartyCount, WinningCount = WinningCount, FrozenBalanceVO = fVO } };
        }

        /// <summary>
        /// 获取收入余额
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyCardBalance1"), HttpGet, Anonymous]
        public ResultObject getMyCardBalance1(int customerId)
        {
            //UserProfile uProfile = CacheManager.GetUserProfile(token);
            //CustomerProfile cProfile = uProfile as CustomerProfile;
            //int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            //先查询并结算冻结金额
            FrozenBalanceVO fVO = cBO.FindFrozenBalanceByCustomerId(customerId);
            decimal FrozenBalance = fVO.SumBalance;
            decimal CardBalance = cBO.FindCardBalanceByCustomerId(customerId);

            int SignUpCount = cBO.FindSignUpViewLuckDrawCount(customerId);
            int PartyCount = cBO.FindPartyTotalCount("CustomerId=" + customerId + " and Status>0 and Type=3");
            int WinningCount = cBO.FindSignUpWinningCount(customerId);

            if (FrozenBalance == 0)
            {
                fVO.IsReward = true;
            }

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { FrozenBalance = FrozenBalance, CardBalance = CardBalance, SignUpCount = SignUpCount, PartyCount = PartyCount, WinningCount = WinningCount, FrozenBalanceVO = fVO } };
        }


        /// <summary>
        /// 添加活动收入提现记录信息
        /// </summary>
        /// <param name="PayOutFromBody">登录与提现信息</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <param name="BankAccountId">银行卡ID</param>
        /// <returns></returns>
        [Route("AddPayoutHistory"), HttpPost, Anonymous]
        public ResultObject AddPayoutHistory([FromBody] PayOutFromBody PayOutFromBody, string code, int BankAccountId, string formId, int PayType = 1, int AppType = 1, int isH5 = 0)
        {
            if (PayType != 1)
            {
                AppType = PayType;
            }
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            try
            {
                string appid = cBO.appid;
                string secret = cBO.secret;

                if (PayType == 2)
                {
                    //引流王
                    appid = "wxbe6347ce9f00fd0b";
                    secret = "936b0905c776a207174039336a217bcb";
                }

                if (isH5 == 1)
                {
                    ////获取微信的用户信息
                    CustomerController customerCon = new CustomerController();
                    ResultObject result = customerCon.GetThirdPartUserInfo("2", code, "", "2");
                    WeiXinHelper.WeiXinUserInfoResult userInfo = result.Result as WeiXinHelper.WeiXinUserInfoResult;
                    //用户信息（判断是否已经获取到用户的微信用户信息）
                    if (userInfo.Result && userInfo.UserInfo.openid != "" && userInfo.UserInfo.unionid != "")
                    {
                        //_bo.Info("userInfo.UserInfo.openid = " + userInfo.UserInfo.openid);
                        string OpenId = userInfo.UserInfo.openid;
                        string UnionID = userInfo.UserInfo.unionid;
                        //根据OpenID判断是否已经注册，有则直接跳过。否则需要输入手机号码才能正常使用                    
                        CustomerBO uBO = new CustomerBO(new CustomerProfile());
                        CustomerViewVO customerVO = uBO.FindCustomerByOpenId(OpenId, UnionID, "2");
                        if (customerVO != null)
                        {
                            if (!customerVO.isIdCard)
                            {
                                return new ResultObject() { Flag = 0, Message = "请先进行实名认证，然后再提现！", Result = null };
                            }

                            int count = cBO.FindCardPayoutHistoryTotalCount("CustomerId=" + customerVO.CustomerId + " and NOW() - PayOutDate<60");
                            if (count > 0)
                            {
                                return new ResultObject() { Flag = 0, Message = "提现失败，60秒内不能重复申请!", Result = null };
                            }

                            CardPayOutVO pVO = new CardPayOutVO();

                            //申请金额
                            pVO.PayOutCost = PayOutFromBody.CardPayOutVO.PayOutCost;
                            //收款户名
                            pVO.AccountName = PayOutFromBody.CardPayOutVO.AccountName;
                            //收款账号
                            pVO.BankAccount = PayOutFromBody.CardPayOutVO.BankAccount;
                            //开户银行
                            pVO.BankName = PayOutFromBody.CardPayOutVO.BankName;

                            //会员ID
                            pVO.CustomerId = customerVO.CustomerId;
                            //申请时间
                            pVO.PayOutDate = DateTime.Now;

                            //提现类型
                            pVO.Type = PayOutFromBody.CardPayOutVO.Type;

                            pVO.FormId = formId;
                            pVO.OpenId = "";

                            //服务费
                            double PayOutCost = Convert.ToDouble(pVO.PayOutCost);

                            double rate = 0.006;
                            double ServiceCharge = Math.Round(PayOutCost * rate, 2);
                            if (pVO.Type == 0 && pVO.BankName != "微信零钱")
                            {
                                double Cost = PayOutCost - ServiceCharge;
                                //转账手续费
                                double ServiceCharge2 = Math.Round(Cost * 0.003, 2);
                                if (ServiceCharge2 < 1)
                                {
                                    ServiceCharge2 = 1;
                                }
                                if (ServiceCharge2 > 100)
                                {
                                    ServiceCharge2 = 100;
                                }
                                ServiceCharge += ServiceCharge2;
                            }
                            if (pVO.Type == 2)
                            {
                                double Cost = PayOutCost - ServiceCharge;
                                //转账手续费
                                double ServiceCharge2 = Math.Round(Cost * 0.004, 2);
                                if (ServiceCharge2 < 1)
                                {
                                    ServiceCharge2 = 1;
                                }
                                if (ServiceCharge2 > 500)
                                {
                                    ServiceCharge2 = 500;
                                }
                                ServiceCharge += ServiceCharge2;
                            }

                            pVO.ServiceCharge = Convert.ToDecimal(ServiceCharge);

                            //实转金额
                            pVO.Cost = pVO.PayOutCost - pVO.ServiceCharge;

                            if (pVO.PayOutCost < 10)
                            {
                                return new ResultObject() { Flag = 0, Message = "单笔提现金额不能低于10元!", Result = null };
                            }

                            if (pVO.BankName == "微信零钱")
                            {
                                return new ResultObject() { Flag = 0, Message = "暂不支持提现到微信，请提现至银行卡!", Result = null };
                            }

                            if (pVO.PayOutCost > 5000 && pVO.BankName == "微信零钱")
                            {
                                return new ResultObject() { Flag = 0, Message = "提现到微信每天最多只能提现5000元!", Result = null };
                            }

                            if (!cBO.IsHasMoreCardBalance(pVO.CustomerId, pVO.PayOutCost))
                            {
                                return new ResultObject() { Flag = 0, Message = "余额不足!", Result = null };
                            }

                            int PayoutHistoryId = cBO.AddCardPayoutHistoryVO(pVO);
                            if (PayoutHistoryId > 0)
                            {
                                try
                                {
                                    if (BankAccountId == 0)
                                    {
                                        BankAccountVO bVO = new BankAccountVO();
                                        bVO.CustomerId = pVO.CustomerId;
                                        bVO.AccountName = pVO.AccountName;
                                        bVO.BankAccount = pVO.BankAccount;
                                        bVO.BankName = pVO.BankName;
                                        if (PayOutFromBody.CardPayOutVO.Type == 2)
                                        {
                                            bVO.Type = 1;
                                        }
                                        uBO.AddBankAccount(bVO);
                                    }
                                }
                                catch
                                {

                                }
                                return new ResultObject() { Flag = 1, Message = "提交申请成功!", Result = PayoutHistoryId };
                            }
                            else
                            {
                                return new ResultObject() { Flag = 0, Message = "提交失败,请重试!", Result = null };
                            }
                        }
                    }

                    return new ResultObject() { Flag = 0, Message = "提交失败,请重新登陆!", Result = null };
                }

                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);

                if (readConfig.unionid != "")
                {
                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    CustomerViewVO customerVO = uBO.FindCustomerByOpenId(readConfig.openid, readConfig.unionid, "2", AppType);
                    if (customerVO != null)
                    {
                        //登录成功
                        if (!customerVO.isIdCard)
                        {
                            return new ResultObject() { Flag = 0, Message = "请先进行实名认证，然后再提现！", Result = null };
                        }

                        int count = cBO.FindCardPayoutHistoryTotalCount("CustomerId=" + customerVO.CustomerId + " and NOW() - PayOutDate<60");
                        if (count > 0)
                        {
                            return new ResultObject() { Flag = 0, Message = "提现失败，60秒内不能重复申请!", Result = null };
                        }

                        CardPayOutVO pVO = new CardPayOutVO();

                        //申请金额
                        pVO.PayOutCost = PayOutFromBody.CardPayOutVO.PayOutCost;
                        //收款户名
                        pVO.AccountName = PayOutFromBody.CardPayOutVO.AccountName;
                        //收款账号
                        pVO.BankAccount = PayOutFromBody.CardPayOutVO.BankAccount;
                        //开户银行
                        pVO.BankName = PayOutFromBody.CardPayOutVO.BankName;

                        //会员ID
                        pVO.CustomerId = customerVO.CustomerId;
                        //申请时间
                        pVO.PayOutDate = DateTime.Now;

                        //提现类型
                        pVO.Type = PayOutFromBody.CardPayOutVO.Type;

                        pVO.FormId = formId;
                        pVO.OpenId = readConfig.openid;


                        //服务费
                        double PayOutCost = Convert.ToDouble(pVO.PayOutCost);
                        double rate = 0.006;
                        double ServiceCharge = Math.Round(PayOutCost * rate, 2);
                        if (pVO.Type == 0 && pVO.BankName != "微信零钱")
                        {
                            double Cost = PayOutCost - ServiceCharge;
                            //转账手续费

                            double ServiceCharge2 = Math.Round(Cost * 0.003, 2);
                            if (ServiceCharge2 < 1)
                            {
                                ServiceCharge2 = 1;
                            }
                            if (ServiceCharge2 > 100)
                            {
                                ServiceCharge2 = 100;
                            }
                            ServiceCharge += ServiceCharge2;
                        }
                        if (pVO.Type == 2)
                        {
                            double Cost = PayOutCost - ServiceCharge;
                            //转账手续费
                            double ServiceCharge2 = Math.Round(Cost * 0.004, 2);
                            if (ServiceCharge2 < 1)
                            {
                                ServiceCharge2 = 1;
                            }
                            if (ServiceCharge2 > 500)
                            {
                                ServiceCharge2 = 500;
                            }
                            ServiceCharge += ServiceCharge2;
                        }

                        pVO.ServiceCharge = Convert.ToDecimal(ServiceCharge);

                        //实转金额
                        pVO.Cost = pVO.PayOutCost - pVO.ServiceCharge;

                        if (pVO.PayOutCost < 10)
                        {
                            return new ResultObject() { Flag = 0, Message = "单笔提现金额不能低于10元!", Result = null };
                        }
                        if (pVO.BankName == "微信零钱")
                        {
                            return new ResultObject() { Flag = 0, Message = "暂不支持提现到微信，请提现至银行卡!", Result = null };
                        }
                        if (pVO.PayOutCost > 5000 && pVO.BankName == "微信零钱")
                        {
                            return new ResultObject() { Flag = 0, Message = "提现到微信每天最多只能提现5000元!", Result = null };
                        }

                        if (!cBO.IsHasMoreCardBalance(pVO.CustomerId, pVO.PayOutCost))
                        {
                            return new ResultObject() { Flag = 0, Message = "余额不足!", Result = null };
                        }

                        int PayoutHistoryId = cBO.AddCardPayoutHistoryVO(pVO);
                        if (PayoutHistoryId > 0)
                        {
                            try
                            {
                                if (BankAccountId == 0)
                                {
                                    BankAccountVO bVO = new BankAccountVO();
                                    bVO.CustomerId = pVO.CustomerId;
                                    bVO.AccountName = pVO.AccountName;
                                    bVO.BankAccount = pVO.BankAccount;
                                    bVO.BankName = pVO.BankName;
                                    if (PayOutFromBody.CardPayOutVO.Type == 2)
                                    {
                                        bVO.Type = 1;
                                    }
                                    uBO.AddBankAccount(bVO);
                                }
                            }
                            catch
                            {

                            }
                            return new ResultObject() { Flag = 1, Message = "提交申请成功!", Result = PayoutHistoryId };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "提交失败,请重试!", Result = null };
                        }
                    }
                }
                return new ResultObject() { Flag = 0, Message = "提交失败,请重试!", Result = null };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "提交失败,请重试!", Result = null };
            }
        }

        /// <summary>
        /// 添加活动收入提现记录信息(提现到微信零钱)
        /// </summary>
        /// <param name="PayOutFromBody">登录与提现信息</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <param name="formId">formId</param>
        /// <returns></returns>
        [Route("AddPayoutHistory2WXPacket"), HttpPost, Anonymous]
        public ResultObject AddPayoutHistory2WXPacket([FromBody] PayOutFromBody PayOutFromBody, string code, string formId, int PayType = 1, int AppType = 1)
        {
            return new ResultObject() { Flag = 0, Message = "提现即时到账功能正在维护中!", Result = null };
            if (PayType != 1)
            {
                AppType = PayType;
            }
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            try
            {
                string appid = cBO.appid;
                string secret = cBO.secret;

                if (PayType == 2)
                {
                    //引流王
                    appid = "wxbe6347ce9f00fd0b";
                    secret = "936b0905c776a207174039336a217bcb";
                }

                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);

                if (readConfig.unionid != "")
                {
                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    CustomerViewVO customerVO = uBO.FindCustomerByOpenId(readConfig.openid, readConfig.unionid, "2", AppType);
                    if (customerVO != null)
                    {
                        if (!customerVO.isIdCard)
                        {
                            return new ResultObject() { Flag = 0, Message = "请先进行实名认证，然后再提现！", Result = null };
                        }

                        //登录成功
                        DateTime now = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                        int count = cBO.FindCardPayoutHistoryTotalCount("CustomerId=" + customerVO.CustomerId + " and Type=1 and PayOutStatus=1 and to_days(PayOutDate) = to_days(now())");
                        if (count > 0)
                        {
                            return new ResultObject() { Flag = 2, Message = "提现失败，每天只可以使用微信提现一次!", Result = null };
                        }
                        /*
                         List<CardPayOutVO> list= cBO.FindPayOutByCustomerId(customerVO.CustomerId);
                         foreach (CardPayOutVO cardpayoutVO in list) {
                             DateTime theday = Convert.ToDateTime(cardpayoutVO.PayOutDate.ToShortDateString());
                             TimeSpan ts = theday - now;
                             //判断每天只可以提现一次
                             if (ts.Days== 0&& cardpayoutVO.Type == 1&& cardpayoutVO.PayOutStatus==1) {
                                 return new ResultObject() { Flag = 2, Message = "提现失败，每天只可以使用微信提现一次!", Result =null };
                             }
                         }
                         */

                        CardPayOutVO pVO = new CardPayOutVO();

                        //申请金额
                        pVO.PayOutCost = PayOutFromBody.CardPayOutVO.PayOutCost;

                        //收款户名
                        pVO.AccountName = PayOutFromBody.CardPayOutVO.AccountName;

                        //服务费
                        double PayOutCost = Convert.ToDouble(pVO.PayOutCost);
                        double ServiceCharge = Math.Round(PayOutCost * 0.006, 2);

                        pVO.ServiceCharge = Convert.ToDecimal(ServiceCharge);
                        //实转金额
                        pVO.Cost = pVO.PayOutCost - pVO.ServiceCharge;
                        //会员ID
                        pVO.CustomerId = customerVO.CustomerId;
                        //申请时间
                        pVO.PayOutDate = DateTime.Now;

                        pVO.FormId = formId;
                        pVO.OpenId = readConfig.openid;
                        pVO.Type = 1;//微信提现
                        pVO.PayOutStatus = 0;

                        if (pVO.PayOutCost < 2 || pVO.PayOutCost > 100)
                        {
                            return new ResultObject() { Flag = 0, Message = "单笔提现金额不能低于2元或超过100元!", Result = null };
                        }

                        if (!cBO.IsHasMoreCardBalance(pVO.CustomerId, pVO.PayOutCost))
                        {
                            return new ResultObject() { Flag = 0, Message = "余额不足!", Result = null };
                        }

                        if (!cBO.isLegitimate(pVO.CustomerId))
                        {
                            return new ResultObject() { Flag = 0, Message = "您的账户余额与平台收入不一致，联系客服处理!", Result = null };
                        }

                        int PayoutHistoryId = cBO.AddCardPayoutHistoryVO2(pVO);
                        if (PayoutHistoryId > 0)
                        {
                            pVO.PayOutHistoryId = PayoutHistoryId;
                            //企业付款到零钱
                            string resultbyPay = cBO.PayforWXUserCash(pVO.Cost, PayOutFromBody.CardPayOutVO.AccountName, readConfig.openid, customerVO.CustomerId, PayType);
                            if (resultbyPay == "NAME_MISMATCH")
                            {
                                pVO.PayOutStatus = -2;
                                cBO.HandleCardPayOut(pVO);
                                return new ResultObject() { Flag = 0, Message = "用户真实姓名填写错误，请重试！", Result = resultbyPay };
                            }
                            if (resultbyPay == "FAIL")
                            {
                                pVO.PayOutStatus = -2;
                                cBO.HandleCardPayOut(pVO);
                                return new ResultObject() { Flag = 0, Message = "提现失败,请重试!", Result = resultbyPay };
                            }
                            if (resultbyPay == "NOTENOUGH")
                            {
                                MessageTool.SendMobileMsg(PayOutFromBody.CardPayOutVO.AccountName + "发起微信提现，商户平台余额不足，请管理员前往充值后在后台进行操作！【众销乐 -资源共享众包销售平台】", "18620584620");//
                                MessageTool.SendMobileMsg(PayOutFromBody.CardPayOutVO.AccountName + "发起微信提现，商户平台余额不足，请管理员前往充值后在后台进行操作！【众销乐 -资源共享众包销售平台】", "13592808422");//
                                return new ResultObject() { Flag = 1, Message = "提现成功，提现金额将会在24小时内到达，请留意服务通知！", Result = PayoutHistoryId };
                            }
                            if (resultbyPay == "SUCCESS")
                            {
                                pVO.HandleDate = DateTime.Now;
                                pVO.PayOutStatus = 1;//微信提现成功
                                cBO.UpdateCardPayoutHistoryVO(pVO);
                            }

                            cBO.sendPayOutMessage(PayoutHistoryId);
                            return new ResultObject() { Flag = 1, Message = "提现成功，请留意微信通知！", Result = PayoutHistoryId };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "提现失败,请重试!", Result = null };
                        }
                    }
                }
                return new ResultObject() { Flag = 0, Message = "提现失败,请重试!", Result = null };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "提现失败,请重试!", Result = null };
            }
        }

        /// <summary>
        /// 微云智推会员提现
        /// </summary>
        /// <param name="PayOutFromBody">登录与提现信息</param>
        /// <param name="code">第一次拉手返回Code</param>
        /// <param name="formId">formId</param>
        /// <returns></returns>
        [Route("WYZTPayout"), HttpPost, Anonymous]
        public ResultObject WYZTPayout([FromBody] PayOutFromBody PayOutFromBody, int BankAccountId, string code, string formId)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), 3);
            try
            {
                string appid = cBO.appid;
                string secret = cBO.secret;

                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);

                if (readConfig.unionid != "")
                {
                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    CustomerViewVO customerVO = uBO.FindCustomerByOpenId(readConfig.openid, readConfig.unionid, "2", 3);
                    if (customerVO != null)
                    {
                        //登录成功
                        if (customerVO.isVip == false || customerVO.ExpirationAt < DateTime.Now)
                        {
                            return new ResultObject() { Flag = 0, Message = "只有VIP会员才能提现推广奖金哦!", Result = null };
                        }

                        int count = cBO.FindCardPayoutHistoryTotalCount("CustomerId=" + customerVO.CustomerId + " and NOW() - PayOutDate<60");
                        if (count > 0)
                        {
                            return new ResultObject() { Flag = 0, Message = "提现失败，60秒内不能重复申请!", Result = null };
                        }

                        CardPayOutVO pVO = new CardPayOutVO();

                        //申请金额
                        pVO.PayOutCost = cBO.getPayoutCost(customerVO.CustomerId);
                        if (pVO.PayOutCost <= 0)
                        {
                            return new ResultObject() { Flag = 0, Message = "提现失败，余额不足!", Result = null };
                        }

                        //收款户名
                        pVO.AccountName = PayOutFromBody.CardPayOutVO.AccountName;
                        //收款账号
                        pVO.BankAccount = PayOutFromBody.CardPayOutVO.BankAccount;
                        //开户银行
                        pVO.BankName = PayOutFromBody.CardPayOutVO.BankName;

                        //会员ID
                        pVO.CustomerId = customerVO.CustomerId;
                        //申请时间
                        pVO.PayOutDate = DateTime.Now;

                        //提现类型
                        pVO.Type = PayOutFromBody.CardPayOutVO.Type;

                        pVO.FormId = formId;
                        pVO.OpenId = readConfig.openid;

                        //服务费
                        double PayOutCost = Convert.ToDouble(pVO.PayOutCost);
                        double rate = 0.006;
                        double ServiceCharge = Math.Round(PayOutCost * rate, 2);
                        if (pVO.Type == 0 && pVO.BankName != "微信零钱")
                        {
                            double Cost = PayOutCost - ServiceCharge;
                            //转账手续费
                            double ServiceCharge2 = Math.Round(Cost * 0.003, 2);
                            if (ServiceCharge2 < 1)
                            {
                                ServiceCharge2 = 1;
                            }
                            if (ServiceCharge2 > 100)
                            {
                                ServiceCharge2 = 100;
                            }
                            ServiceCharge += ServiceCharge2;
                        }
                        if (pVO.Type == 2)
                        {
                            double Cost = PayOutCost - ServiceCharge;
                            //转账手续费
                            double ServiceCharge2 = Math.Round(Cost * 0.004, 2);
                            if (ServiceCharge2 < 1)
                            {
                                ServiceCharge2 = 1;
                            }
                            if (ServiceCharge2 > 500)
                            {
                                ServiceCharge2 = 500;
                            }
                            ServiceCharge += ServiceCharge2;
                        }
                        pVO.ServiceCharge = Convert.ToDecimal(ServiceCharge);

                        //实转金额
                        pVO.Cost = pVO.PayOutCost - pVO.ServiceCharge;

                        /*
                        if (pVO.PayOutCost < 10)
                        {
                            return new ResultObject() { Flag = 0, Message = "单笔提现金额不能低于10元!", Result = null };
                        }*/

                        if (pVO.PayOutCost > 5000 && pVO.BankName == "微信零钱")
                        {
                            return new ResultObject() { Flag = 0, Message = "提现到微信每天最多只能提现5000元!", Result = null };
                        }

                        int PayoutHistoryId = cBO.AddCardPayoutHistoryVO(pVO);
                        if (PayoutHistoryId > 0)
                        {
                            try
                            {
                                if (BankAccountId == 0 && pVO.BankName != "微信零钱")
                                {
                                    BankAccountVO bVO = new BankAccountVO();
                                    bVO.CustomerId = pVO.CustomerId;
                                    bVO.AccountName = pVO.AccountName;
                                    bVO.BankAccount = pVO.BankAccount;
                                    bVO.BankName = pVO.BankName;
                                    if (PayOutFromBody.CardPayOutVO.Type == 2)
                                    {
                                        bVO.Type = 1;
                                    }
                                    uBO.AddBankAccount(bVO);
                                }
                            }
                            catch
                            {

                            }
                            return new ResultObject() { Flag = 1, Message = "提交申请成功!", Result = PayoutHistoryId };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "提交失败,请重试!", Result = null };
                        }
                    }
                }
                return new ResultObject() { Flag = 0, Message = "提交失败,请重试!", Result = null };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "提交失败,请重试!", Result = null };
            }
        }

        /// <summary>
        /// 获取提现详情
        /// </summary>
        /// <param name="PayoutHistoryId"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getPayoutDetails"), HttpGet]
        public ResultObject getPayoutDetails(int PayoutHistoryId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardPayOutVO cVO = cBO.FindPayOutViewById(PayoutHistoryId);

            if (cVO == null)
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

            if (cVO.CustomerId != customerId)
                return new ResultObject() { Flag = 0, Message = "只有本人才能查看!", Result = null };

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
        }

        /// <summary>
        /// 获取提现详情(后台专用)
        /// </summary>
        /// <param name="PayoutHistoryId"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AdminGetPayoutDetails"), HttpGet]
        public ResultObject AdminGetPayoutDetails(int PayoutHistoryId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId > 0)
            {
                IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
                UserViewVO uVO = uDAO.FindById(uProfile.UserId);
                CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);
                CardPayOutVO cVO = cBO.FindPayOutViewById(PayoutHistoryId);

                var customerId = cVO.CustomerId;
                bool isLegitimate = cBO.isLegitimate(customerId);
                if (cVO == null)
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO, Subsidiary = isLegitimate };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 处理会员提现操作(后台专用)
        /// </summary>
        /// <param name="CardPayOutVO">提现记录</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("HandleCustomerPayOut"), HttpPost]
        public ResultObject HandleCustomerPayOut([FromBody] CardPayOutVO CardPayOutVO, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);

            if (uProfile.UserId > 0)
            {
                IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
                UserViewVO uVO = uDAO.FindById(uProfile.UserId);
                CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);
                bool result;
                /*
                if (CardPayOutVO.Type == 1) {
                    result = cBO.HandleCardPayOutByWxCash(CardPayOutVO);
                }
                else{
                    result = cBO.HandleCardPayOut(CardPayOutVO);
                }
                */
                result = cBO.HandleCardPayOut(CardPayOutVO);

                if (result)
                    return new ResultObject() { Flag = 1, Message = "处理成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "处理失败!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "处理失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取提现详情(后台专用)
        /// </summary>
        /// <param name="PayoutHistoryId"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("BcAdminGetPayoutDetails"), HttpGet]
        public ResultObject BcAdminGetPayoutDetails(int PayoutHistoryId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId > 0)
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                BcPayOutHistoryVO cVO = cBO.FindBcPayOutHistoryById(PayoutHistoryId);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 处理会员提现操作(后台专用)
        /// </summary>
        /// <param name="BcPayOutHistoryVO">提现记录</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("BcHandleCustomerPayOut"), HttpPost]
        public ResultObject BcHandleCustomerPayOut([FromBody] BcPayOutHistoryVO BcPayOutHistoryVO, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);

            if (uProfile.UserId > 0)
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                bool result;
                if (BcPayOutHistoryVO.HandleComment == "")
                {
                    BcPayOutHistoryVO.HandleComment = "已发放至微信零钱！";
                }

                BcPayOutHistoryVO bcVO = cBO.FindBcPayOutHistoryById(BcPayOutHistoryVO.PayOutHistoryId);
                bcVO.ThirdOrder = BcPayOutHistoryVO.ThirdOrder;
                bcVO.PayOutStatus = BcPayOutHistoryVO.PayOutStatus;
                bcVO.HandleComment = BcPayOutHistoryVO.HandleComment;
                result = cBO.HandlePayOut(bcVO, 0);
                if (result)
                    return new ResultObject() { Flag = 1, Message = "处理成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "处理失败!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "处理失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我的提现记录
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyPayoutlist"), HttpGet]
        public ResultObject getMyPayoutlist(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            List<CardPayOutVO> uVO = cBO.FindPayOutByCustomerId(customerId);

            if (uVO != null)
            {
                if (uVO.Count > 0)
                {
                    uVO.Reverse();
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无记录!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取微信支付数据
        /// </summary>
        /// <param name="PartyOrderID">活动订单id</param>
        /// <param name="token"></param>
        ///<param name="InviterCID">邀请者id</param>
        ///<param name="PayType">1：乐聊名片，2：引流王</param>
        /// <returns></returns>
        [Route("GetUnifiedOrderResult"), HttpGet]
        public async Task<ResultObject> GetUnifiedOrderResult(int PartyOrderID, string token, int InviterCID, int PayType = 1, int isH5 = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);

            int AppType = CustomerVO2.AppType;
            if (PayType != 1)
            {
                AppType = PayType;
            }

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            string appid = cBO.appid;
            CardPartyOrderViewVO cVO = cBO.FindPartyOrderViewById(PartyOrderID);
            if (cVO == null)
                return new ResultObject() { Flag = 0, Message = "获取订单失败!", Result = null };
            if (cVO.CreatedAt.AddMinutes(30) < DateTime.Now)
            {
                return new ResultObject() { Flag = 0, Message = "订单已过期，请重新报名!", Result = null };
            }

            /*
            List<CardPartySignUpVO> SignUpVO = cBO.isJionCardParty(customerId, cVO.PartyID);
            if (SignUpVO.Count > 0)
            {
                return new ResultObject() { Flag = 4, Message = "您已经报名了该活动，请勿重复操作!", Result = null };
            }*/

            JsApiPay Ja = new JsApiPay();

            if (cVO.Cost < 0)
            {
                return new ResultObject() { Flag = 0, Message = "价格错误", Result = null };
            }
            if (cVO.Cost == 0)
            {
                //完成报名
                CardPartySignUpVO suVO = new CardPartySignUpVO();
                suVO.CardID = cVO.CardID;
                suVO.CustomerId = cVO.CustomerId;
                suVO.Name = cVO.Name;
                suVO.Headimg = cVO.Headimg;
                suVO.Phone = cVO.Phone;
                suVO.PartyID = cVO.PartyID;
                suVO.FormId = cVO.FormId;
                suVO.OpenId = cVO.OpenId;
                suVO.CreatedAt = DateTime.Now;
                suVO.SignUpForm = cVO.SignUpForm;
                suVO.InviterCID = cVO.InviterCID;
                suVO.Number = cVO.Number;
                if (cVO.Type == 2)
                {
                    suVO.isDeliver = 0;
                }

                int PartySignUpID = cBO.AddCardToParty(suVO);
                if (PartySignUpID > 0)
                {
                    cBO.sendSignUpMessage(PartySignUpID);
                }
                //修改订单
                CardPartyOrderVO cpVO = new CardPartyOrderVO();
                cpVO.InviterCID = cVO.InviterCID;
                cpVO.PartyOrderID = cVO.PartyOrderID;
                cpVO.Status = 1;
                cpVO.payAt = DateTime.Now;
                cpVO.PartySignUpID = PartySignUpID;
                cBO.UpdatePartyOrder(cpVO);

                return new ResultObject() { Flag = 5, Message = "0元报名成功", Result = PartySignUpID };
            }


            int total_fee_1 = Convert.ToInt32((cVO.Cost * 100));
            string NOTIFY_URL = "https://api.leliaomp.com/Pay/Party_Notify_Url.aspx";

            string title = Regex.Replace(cVO.Title, @"\p{Cs}", "");
            string costName = Regex.Replace(cVO.CostName, @"\p{Cs}", "");

            if (isH5 == 1)
            {
                appid = cBO.appidH5;
            }

            //如果有个人二级商户，就用二级商户结账
            EcommerceBO eBO = new EcommerceBO();
            if (cVO.sub_mchid != "")
            {
                NOTIFY_URL = "https://api.leliaomp.com/Pay/Ecommerce_Party_Notify_Url.aspx";
                string description = costName + "*" + cVO.Number;
                bool profit_sharing = false;
                if (cVO.SplitCost > 0) profit_sharing = true;

                AppletsPayDataVO res = await eBO.GetPay(appid, cVO.sub_mchid, description, cVO.OrderNO, total_fee_1, cVO.OpenId, NOTIFY_URL, profit_sharing);
                if (res == null)
                {
                    return new ResultObject() { Flag = 0, Message = "支付失败，请咨询客服", Result = null };
                }

                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                string s = jsonSerializer.Serialize(res);
                return new ResultObject() { Flag = 1, Message = "成功", Result = s };
            }

            WxPayData wp = Ja.GetUnifiedOrderResult(appid, cVO.OrderNO, cVO.OpenId, total_fee_1.ToString(), "报名收费", costName, "报名收费", NOTIFY_URL, AppType);

            if (wp != null)
            {
                string reslut = Ja.GetJsApiParameters(wp, AppType);
                if (reslut != "")
                {
                    return new ResultObject() { Flag = 1, Message = "成功", Result = reslut };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "支付失败，请重新下单", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "支付失败，请重新下单", Result = null };
            }
        }

        /// <summary>
        /// 申请退款（活动订单）
        /// </summary>
        /// <param name="PartyOrderID">活动订单id</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("GetRefundResult"), HttpGet]
        public async Task<ResultObject> GetRefundResult(int PartyOrderID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO2 = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardPartyOrderViewVO cVO = cBO2.FindPartyOrderViewById(PartyOrderID);

            CardBO cBO = new CardBO(new CustomerProfile(), cVO.AppType);

            if (cVO == null)
                return new ResultObject() { Flag = 0, Message = "获取订单失败!", Result = null };
            if (cVO.Type != 1)
                return new ResultObject() { Flag = 0, Message = "退款失败！商品暂不支持退款", Result = null };
            if (cVO.CustomerId != customerId)
                return new ResultObject() { Flag = 0, Message = "退款失败！非本人操作", Result = null };
            if (cVO.isRefund != 1)
                return new ResultObject() { Flag = 0, Message = "退款失败！本活动不支持退款", Result = null };
            if (cVO.Status != 1)
                return new ResultObject() { Flag = 0, Message = "退款失败！订单未支付或已退款", Result = null };
            if (cVO.IsPayOut != 0 && cVO.sub_mchid == "")
                return new ResultObject() { Flag = 0, Message = "退款失败！订单金额已到账活动举办方，请联系举办方退款", Result = null };
            if (cVO.SignUpTime.AddMinutes(-60) < DateTime.Now)
            {
                return new ResultObject() { Flag = 0, Message = "距离报名截止时间不足60分钟的情况下不可退款!", Result = null };
            }

            JsApiPay Ja = new JsApiPay();

            if (cVO.Cost <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "0元订单不可退款", Result = null };
            }

            string total_fee_1 = Convert.ToInt32((cVO.Cost * 100)).ToString();

            if (cVO.sub_mchid != "")
            {
                //二级商户订单退款
                EcommerceBO eBO = new EcommerceBO();
                ResultCode response = await eBO.Refund(cVO.sp_appid, cVO.sub_mchid, cVO.OrderNO, Convert.ToInt32((cVO.Cost * 100)));

                if (cVO.PromotionAwardCost > 0 || cVO.SplitCost > 0)
                {
                    return new ResultObject() { Flag = 0, Message = "已分成的订单无法自动退款，请联系客服处理", Result = null };
                }

                CardPartyOrderVO cpVO = cBO.FindPartyOrderById(cVO.PartyOrderID);
                cpVO.Status = 2;
                cpVO.RefundAt = DateTime.Now;

                if (response.code == "SUCCESS")
                {
                    cpVO.RefundStatus = 1;
                }


                cBO.UpdatePartyOrder(cpVO);

                CardPartySignUpVO cuVO = new CardPartySignUpVO();
                cuVO.PartySignUpID = cVO.PartySignUpID;
                cuVO.SignUpStatus = 2;
                cBO.UpdateSignUp(cuVO);

                if (response.code == "NOT_ENOUGH")
                {
                    cBO.AddCardMessage("因余额不足，您的客户申请退款失败，请及时前往商家助手充值", cVO.HostCustomerId, "活动报名", "");
                }
                else
                {
                    string WeeklyReport = cVO.Name + "已从《" + cVO.Title + "》申请退款了";
                    cBO.AddCardMessage(WeeklyReport, cVO.HostCustomerId, "活动报名", "");
                }

                return new ResultObject() { Flag = 1, Message = "申请退款成功，将在24小时以内到账，如未收到账，请联系客服处理！", Result = null };
            }
            else
            {
                //平台订单退款
                WxPayData wp = Ja.GetRefundResult(cVO.OpenId, total_fee_1, total_fee_1, cVO.OrderNO, cVO.AppType);
                if (wp != null)
                {
                    string reslut = Ja.GetJsApiParameters(wp, cVO.AppType);
                    if (reslut != "")
                    {
                        CardPartyOrderVO cpVO = cBO.FindPartyOrderById(cVO.PartyOrderID);
                        cpVO.Status = 2;
                        cpVO.RefundAt = DateTime.Now;

                        if (wp.GetValue("result_code").ToString() == "SUCCESS")
                        {
                            cpVO.RefundStatus = 1;
                        }

                        cBO.UpdatePartyOrder(cpVO);

                        CardPartySignUpVO cuVO = new CardPartySignUpVO();
                        cuVO.PartySignUpID = cVO.PartySignUpID;
                        cuVO.SignUpStatus = 2;
                        cBO.UpdateSignUp(cuVO);

                        CardPartyVO pVO = new CardPartyVO();
                        pVO.RecordSignUpCount -= cVO.Number;
                        pVO.PartyID = cVO.PartyID;
                        cBO.UpdateParty(pVO);

                        string WeeklyReport = cVO.Name + "已从《" + cVO.Title + "》申请退款了";
                        cBO.AddCardMessage(WeeklyReport, cVO.HostCustomerId, "活动报名", "");

                        return new ResultObject() { Flag = 1, Message = "申请退款成功，将在24小时以内到账，如未收到账，请联系客服处理！", Result = reslut, Subsidiary = wp.GetValue("result_code") };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "申请退款失败", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "申请退款失败", Result = null };
                }
            }
        }

        /// <summary>
        /// 新支付接口，获取微信支付数据,在线开通VIP
        /// </summary>
        /// <param name="Type">1:超级会员包月  2：超级会员包年  3：超级会员永久 6:合伙人 7:分公司 8:星钻会员一个月 9：星钻会员一年 10：白金会员一年</param>
        /// <param name="token"></param>
        /// <param name="PayType">1:乐聊名片，2：引流王</param>
        /// <param name="InviterCID">邀请人id</param>
        /// <returns></returns>
        [Route("GetBuyNewVip"), HttpGet]
        public ResultObject GetBuyNewVip(int Type, string code, string token, int PayType = 1, int InviterCID = 0, string DiscountCode = "", int isH5 = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);

            int AppType = CustomerVO2.AppType;
            if (PayType != 1)
            {
                AppType = PayType;
            }

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardOrderVO oVO = new CardOrderVO();
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO uVO = uBO.FindCustomenById(customerId);
            string appid = cBO.appid;

            if (Type != 1 && Type != 2 && Type != 3 && Type != 6 && Type != 7 && Type != 8 && Type != 9 && Type != 10)
            {
                return new ResultObject() { Flag = 0, Message = "购买类型错误，请重新选择!", Result = null };
            }

            if (Type == 8)
            {
                if (cBO.FindCardOrderTotalCount("CustomerId=" + customerId + " and Status=1 and Type=8") > 0)
                {
                    return new ResultObject() { Flag = 2, Message = "星钻会员体验仅限购买一次，请购买其他级别的会员吧", Result = null };
                }
            }


            oVO.CustomerId = customerId;
            oVO.InviterCID = InviterCID;
            string OpenId = "";
            if (isH5 == 1)
            {
                ////获取微信的用户信息
                CustomerController customerCon = new CustomerController();
                ResultObject result = customerCon.GetThirdPartUserInfo("2", code, "", "2");
                WeiXinHelper.WeiXinUserInfoResult userInfo = result.Result as WeiXinHelper.WeiXinUserInfoResult;
                //用户信息（判断是否已经获取到用户的微信用户信息）
                if (userInfo.Result && userInfo.UserInfo.openid != "")
                {
                    OpenId = userInfo.UserInfo.openid;
                }
            }
            else
            {
                OpenId = cBO.getOpenId(code);
            }
            oVO.OpenId = OpenId;
            oVO.CreatedAt = DateTime.Now;
            oVO.Status = 0;
            oVO.Type = Type;
            if (Type == 1 || Type == 2 || Type == 3 || Type == 5)
            {
                if (uVO.isVip && uVO.ExpirationAt > DateTime.Now && (uVO.VipLevel == 2 || uVO.VipLevel == 3))
                {
                    return new ResultObject() { Flag = 0, Message = "您已经是合伙人或分公司Vip，如想降级为普通Vip请等当前VIP到期后再开通！", Result = null };
                }
            }
            if (Type == 8 || Type == 9 || Type == 10)
            {
                if (uVO.isVip && uVO.ExpirationAt > DateTime.Now && (uVO.VipLevel == 1 || uVO.VipLevel == 2 || uVO.VipLevel == 3))
                {
                    return new ResultObject() { Flag = 3, Message = "您已经是五星会员，是否再次续费？", Result = null };
                }
            }

            string body = "";
            if (Type == 1)
            {
                if (CustomerVO2.Couponcost > 0 && CustomerVO2.CouponAt > DateTime.Now)
                {
                    oVO.Cost = 40 - CustomerVO2.Couponcost;
                    body = "乐聊名片1个月五星会员";
                }
                else
                {
                    oVO.Cost = 40;
                    body = "乐聊名片1个月五星会员";
                }

            }
            else if (Type == 2)
            {
                if (CustomerVO2.Couponcost > 0 && CustomerVO2.CouponAt > DateTime.Now)
                {
                    oVO.Cost = 365 - CustomerVO2.Couponcost;
                    body = "乐聊名片1年五星会员";
                }
                else
                {
                    oVO.Cost = 365;
                    body = "乐聊名片1年五星会员";
                }

                ////会员升级
                //if (uVO.isVip && uVO.ExpirationAt > DateTime.Now && (uVO.VipLevel == 4 || uVO.VipLevel == 5))
                //{
                //    TimeSpan c = uVO.ExpirationAt - DateTime.Now;
                //    int day = Convert.ToInt32(c.TotalDays);

                //    decimal OldCost = 0;
                //    if (uVO.VipLevel == 4) OldCost = 68;
                //    if (uVO.VipLevel == 5) OldCost = 298;

                //    oVO.Cost = oVO.Cost - (OldCost / 365) * day;
                //    body = "乐聊名片升级五星会员";
                //}

                //if (cBO.AppType == 3)
                //{
                //    oVO.Cost = 1980;
                //    if (DiscountCode != "")
                //    {
                //        CardDiscountCodeVO CodeVO = cBO.GetDiscountCode(DiscountCode);
                //        if (CodeVO != null)
                //        {
                //            oVO.Cost = CodeVO.Cost;
                //        }
                //    }
                //    body = "微云智推包年VIP会员";
                //}
            }
            //else if (Type == 3)
            //{
            //    oVO.Cost = 1688;
            //    body = "乐聊名片永久五星会员";
            //}
            else if (Type == 3)
            {
                if (CustomerVO2.Couponcost > 0 && CustomerVO2.CouponAt > DateTime.Now)
                {
                    oVO.Cost = 1000 - CustomerVO2.Couponcost;
                    body = "五项裂变训练套餐";
                }
                else
                {
                    oVO.Cost = 1000;
                    body = "五项裂变训练套餐";
                }

            }
            else if (Type == 5)
            {
                oVO.Cost = 68;
                body = "乐聊名片季度五星会员";
            }
            else if (Type == 6)
            {
                oVO.Cost = 10000;
                body = "乐聊名片合伙人";
            }
            else if (Type == 7)
            {
                oVO.Cost = 50000;
                body = "乐聊名片分公司";
            }
            else if (Type == 8)
            {
                oVO.Cost = 9;
                body = "乐聊名片三星会员月费";
            }
            else if (Type == 9)
            {
                oVO.Cost = 68;
                body = "乐聊名片三星会员年费";
            }
            else if (Type == 10)
            {
                oVO.Cost = 298;
                body = "乐聊名片四星会员年费";
                if (uVO.isVip && uVO.ExpirationAt > DateTime.Now && uVO.VipLevel == 4)
                {
                    TimeSpan c = uVO.ExpirationAt - DateTime.Now;
                    int day = Convert.ToInt32(c.TotalDays);

                    decimal OldCost = 0;
                    if (uVO.VipLevel == 4) OldCost = 68;

                    oVO.Cost = oVO.Cost - (OldCost / 365) * day;
                    body = "乐聊名片升级四星会员";
                }
            }

            Random ran = new Random();
            oVO.OrderNO = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);

            int OrderID = cBO.AddOrder(oVO);
            if (OrderID > 0)
            {
                CardOrderVO OrderVO = cBO.FindOrderById(OrderID);
                JsApiPay Ja = new JsApiPay();

                if (OrderVO.Cost <= 0)
                {
                    return new ResultObject() { Flag = 0, Message = "价格错误", Result = null };
                }
                string total_fee_1 = Convert.ToInt32((OrderVO.Cost * 100)).ToString();
                string NOTIFY_URL = "http://api.leliaomp.com/Pay/Card_Notify_Url.aspx";

                String costName = "开通VIP会员";

                if (isH5 == 1)
                {
                    appid = cBO.appidH5;
                }

                try
                {
                    WxPayData wp = Ja.GetUnifiedOrderResult(appid, OrderVO.OrderNO, OrderVO.OpenId, total_fee_1, body, costName, body, NOTIFY_URL, AppType);


                    if (wp != null)
                    {
                        string reslut = Ja.GetJsApiParameters(wp, AppType);
                        if (reslut != "")
                        {
                            return new ResultObject() { Flag = 1, Message = "成功", Result = reslut };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "失败", Result = null };
                        }
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "失败", Result = null };
                    }
                }
                catch (Exception ex)
                {
                    return new ResultObject() { Flag = 0, Message = "失败", Result = ex };
                }

            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "操作失败，请重试!", Result = null };
            }
        }

        /// <summary>
        /// 获取微信支付数据,在线开通VIP
        /// </summary>
        /// <param name="Type">1:超级会员包月  2：超级会员包年  3：超级会员永久 6:合伙人 7:分公司 8:星钻会员一个月 9：星钻会员一年 10：白金会员一年</param>
        /// <param name="token"></param>
        /// <param name="PayType">1:乐聊名片，2：引流王</param>
        /// <param name="InviterCID">邀请人id</param>
        /// <returns></returns>
        [Route("GetUnifiedOrderResult"), HttpGet]
        public ResultObject GetUnifiedOrderResult(int Type, string code, string token, int PayType = 1, int InviterCID = 0, string DiscountCode = "", int isH5 = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);

            int AppType = CustomerVO2.AppType;
            if (PayType != 1)
            {
                AppType = PayType;
            }

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardOrderVO oVO = new CardOrderVO();
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO uVO = uBO.FindCustomenById(customerId);
            string appid = cBO.appid;

            if (Type != 1 && Type != 2 && Type != 3 && Type != 6 && Type != 7 && Type != 8 && Type != 9 && Type != 10)
            {
                return new ResultObject() { Flag = 0, Message = "购买类型错误，请重新选择!", Result = null };
            }

            if (Type == 8)
            {
                if (cBO.FindCardOrderTotalCount("CustomerId=" + customerId + " and Status=1 and Type=8") > 0)
                {
                    return new ResultObject() { Flag = 2, Message = "星钻会员体验仅限购买一次，请购买其他级别的会员吧", Result = null };
                }
            }


            oVO.CustomerId = customerId;
            oVO.InviterCID = InviterCID;
            string OpenId = "";
            if (isH5 == 1)
            {
                ////获取微信的用户信息
                CustomerController customerCon = new CustomerController();
                ResultObject result = customerCon.GetThirdPartUserInfo("2", code, "", "2");
                WeiXinHelper.WeiXinUserInfoResult userInfo = result.Result as WeiXinHelper.WeiXinUserInfoResult;
                //用户信息（判断是否已经获取到用户的微信用户信息）
                if (userInfo.Result && userInfo.UserInfo.openid != "")
                {
                    OpenId = userInfo.UserInfo.openid;
                }
            }
            else
            {
                OpenId = cBO.getOpenId(code);
            }
            oVO.OpenId = OpenId;
            oVO.CreatedAt = DateTime.Now;
            oVO.Status = 0;
            oVO.Type = Type;
            if (Type == 1 || Type == 2 || Type == 3 || Type == 5)
            {
                if (uVO.isVip && uVO.ExpirationAt > DateTime.Now && (uVO.VipLevel == 2 || uVO.VipLevel == 3))
                {
                    return new ResultObject() { Flag = 0, Message = "您已经是合伙人或分公司Vip，如想降级为普通Vip请等当前VIP到期后再开通！", Result = null };
                }
            }
            if (Type == 8 || Type == 9 || Type == 10)
            {
                if (uVO.isVip && uVO.ExpirationAt > DateTime.Now && (uVO.VipLevel == 1 || uVO.VipLevel == 2 || uVO.VipLevel == 3))
                {
                    return new ResultObject() { Flag = 3, Message = "您已经是五星会员，是否再次续费？", Result = null };
                }
            }

            string body = "";
            if (Type == 1)
            {
                oVO.Cost = 39;
                body = "乐聊名片1个月五星会员";
            }
            else if (Type == 2)
            {
                //oVO.Cost = 298;
                oVO.Cost = 365;
                body = "乐聊名片1年五星会员";
                //会员升级
                if (uVO.isVip && uVO.ExpirationAt > DateTime.Now && (uVO.VipLevel == 4 || uVO.VipLevel == 5))
                {
                    TimeSpan c = uVO.ExpirationAt - DateTime.Now;
                    int day = Convert.ToInt32(c.TotalDays);

                    decimal OldCost = 0;
                    if (uVO.VipLevel == 4) OldCost = 68;
                    //if (uVO.VipLevel == 5) OldCost = 298;
                    if (uVO.VipLevel == 5) OldCost = 365;

                    oVO.Cost = oVO.Cost - (OldCost / 365) * day;
                    body = "乐聊名片升级五星会员";
                }

                if (cBO.AppType == 3)
                {
                    oVO.Cost = 1980;
                    if (DiscountCode != "")
                    {
                        CardDiscountCodeVO CodeVO = cBO.GetDiscountCode(DiscountCode);
                        if (CodeVO != null)
                        {
                            oVO.Cost = CodeVO.Cost;
                        }
                    }
                    body = "微云智推包年VIP会员";
                }
            }
            else if (Type == 3)
            {
                oVO.Cost = 998;
                body = "乐聊名片永久五星会员";
            }

            else if (Type == 5)
            {
                oVO.Cost = 68;
                body = "乐聊名片季度五星会员";
            }
            else if (Type == 6)
            {
                oVO.Cost = 10000;
                body = "乐聊名片合伙人";
            }
            else if (Type == 7)
            {
                oVO.Cost = 50000;
                body = "乐聊名片分公司";
            }
            else if (Type == 8)
            {
                oVO.Cost = 9;
                body = "乐聊名片三星会员月费";
            }
            else if (Type == 9)
            {
                oVO.Cost = 68;
                body = "乐聊名片三星会员年费";
            }
            else if (Type == 10)
            {
                oVO.Cost = 298;
                body = "乐聊名片四星会员年费";
                if (uVO.isVip && uVO.ExpirationAt > DateTime.Now && uVO.VipLevel == 4)
                {
                    TimeSpan c = uVO.ExpirationAt - DateTime.Now;
                    int day = Convert.ToInt32(c.TotalDays);

                    decimal OldCost = 0;
                    if (uVO.VipLevel == 4) OldCost = 68;

                    oVO.Cost = oVO.Cost - (OldCost / 365) * day;
                    body = "乐聊名片升级四星会员";
                }
            }

            Random ran = new Random();
            oVO.OrderNO = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);

            int OrderID = cBO.AddOrder(oVO);
            if (OrderID > 0)
            {
                CardOrderVO OrderVO = cBO.FindOrderById(OrderID);
                JsApiPay Ja = new JsApiPay();

                if (OrderVO.Cost <= 0)
                {
                    return new ResultObject() { Flag = 0, Message = "价格错误", Result = null };
                }
                string total_fee_1 = Convert.ToInt32((OrderVO.Cost * 100)).ToString();
                string NOTIFY_URL = "http://api.leliaomp.com/Pay/Card_Notify_Url.aspx";

                String costName = "开通VIP会员";

                if (isH5 == 1)
                {
                    appid = cBO.appidH5;
                }

                try
                {
                    WxPayData wp = Ja.GetUnifiedOrderResult(appid, OrderVO.OrderNO, OrderVO.OpenId, total_fee_1, body, costName, body, NOTIFY_URL, AppType);


                    if (wp != null)
                    {
                        string reslut = Ja.GetJsApiParameters(wp, AppType);
                        if (reslut != "")
                        {
                            return new ResultObject() { Flag = 1, Message = "成功", Result = reslut };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "失败", Result = null };
                        }
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "失败", Result = null };
                    }
                }
                catch (Exception ex)
                {
                    return new ResultObject() { Flag = 0, Message = "失败", Result = ex };
                }

            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "操作失败，请重试!", Result = null };
            }
        }


        /// <summary>
        /// 更改活动发起人(后台专用)
        /// </summary>
        /// <param name="PartID"></param>
        /// <param name="CustomerId"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SelectPartyCard"), HttpGet]
        public ResultObject SelectPartyCard(int PartID, int CustomerId, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId == 3 || uProfile.UserId == 5 || uProfile.UserId == 7)
            {
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);
                CardPartyVO pVO = cBO.FindPartyById(PartID);
                CustomerVO uVO = uBO.FindCustomenById(CustomerId);

                if (pVO == null || uVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }

                if (pVO.GroupID > 0)
                {
                    CardGroupVO gVO = cBO.FindCardGroupById(pVO.GroupID);
                    if (gVO != null)
                    {
                        int i = cBO.DeleteCardToGroupByGroupID(pVO.GroupID, pVO.CustomerId);
                        CardGroupCardVO cgcVO = new CardGroupCardVO();
                        cgcVO.CustomerId = uVO.CustomerId;
                        cgcVO.GroupID = pVO.GroupID;
                        cgcVO.Status = 3;
                        cgcVO.CreatedAt = DateTime.Now;

                        List<CardDataVO> cVO = cBO.FindCardByCustomerId(uVO.CustomerId);
                        if (cVO.Count > 0)
                        {
                            cgcVO.CardID = cVO[0].CardID;
                        }
                        cBO.AddCardToGroup(cgcVO);

                        gVO.CustomerId = uVO.CustomerId;
                        cBO.UpdateCardGroup(gVO);
                    }
                }

                pVO.CustomerId = uVO.CustomerId;
                if (cBO.UpdateParty(pVO))
                    return new ResultObject() { Flag = 1, Message = "设置成功!", Result = null };

                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }
        }

        /// <summary>
        /// 手工发放推广奖励 (后台专用)
        /// </summary>
        /// <param name="PartID"></param>
        /// <param name="CustomerId"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DistributePromotionAwards"), HttpGet]
        public ResultObject DistributePromotionAwards(int CustomerId, decimal RebateCost, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId == 3)
            {
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                CardBO cBO = new CardBO(new CustomerProfile());
                CustomerVO uVO = uBO.FindCustomenById(CustomerId);
                if (uVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }

                CardOrderVO cVO = new CardOrderVO();
                cVO.CustomerId = 2551;
                cVO.CreatedAt = DateTime.Now;
                cVO.OrderNO = "兼职佣金";
                cVO.Cost = 0;
                cVO.Status = 0;
                cVO.Type = 1;
                cVO.isUsed = 1;
                cVO.OneRebateCustomerId = CustomerId;
                cVO.OneRebateCost = RebateCost;

                if (cBO.AddOrder(cVO) > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "设置成功!", Result = null };
                }
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }
        }

        /// <summary>
        /// 设置已付款 (后台专用)
        /// </summary>
        /// <param name="CardOrderID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SetCardOrder"), HttpGet]
        public ResultObject SetCardOrder(int CardOrderID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId == 3)
            {
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                CardBO cBO = new CardBO(new CustomerProfile());
                CardOrderVO cVO = cBO.FindOrderById(CardOrderID);
                if (cVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }

                cVO.Status = 1;
                cVO.payAt = DateTime.Now;

                if (cBO.UpdateOrder(cVO))
                {
                    return new ResultObject() { Flag = 1, Message = "设置成功!", Result = null };
                }
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }
        }

        /// <summary>
        /// 获取入场券
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMySignUpDetails"), HttpGet]
        public ResultObject getMySignUpDetails(int PartySignUpID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            if (AppType == 1)
            {
                AppType = CustomerVO2.AppType;
            }
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            CardPartySignUpViewVO cVO = cBO.FindSignUpViewById(PartySignUpID);

            if (cVO == null)
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };


            if (cVO.SignUpQRCodeImg == "")
            {
                cVO.SignUpQRCodeImg = cBO.GetCardPartySignUpQR(cVO.PartySignUpID);
            }

            cVO.isSponsor = false;
            if (cVO.HostCustomerId == customerId)
                cVO.isSponsor = true;
            else
            {
                List<CardPartyContactsViewVO> ContactsVO = cBO.FindPartyContactsByPartyId(cVO.PartyID);
                for (int i = 0; i < ContactsVO.Count; i++)
                {
                    if (customerId == ContactsVO[i].CustomerId)
                    {
                        cVO.isSponsor = true;
                    }
                }
            }

            if (cVO.CustomerId != customerId && !cVO.isSponsor)
            {
                if (cVO.Type == 3)
                    return new ResultObject() { Flag = 0, Message = "只有本人和举办方才能查看抽奖券!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "只有本人和举办方才能查看入场券!", Result = null };
            }

            List<CardPartySignUpVO> sVO = cBO.FindSignUpByPartyID(cVO.PartyID);
            sVO.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));

            int number = 1;
            for (int i = 0; i < sVO.Count; i++)
            {
                if (sVO[i].PartySignUpID == PartySignUpID)
                {
                    number = i + 1;
                }
            }

            cVO.CardPartyContactsView = cBO.FindPartyContactsByPartyId(cVO.PartyID);
            cVO.Sequence = number;

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
        }


        /// <summary>
        /// 获取抽奖入场券
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getLuckDrawDetails"), HttpGet]
        public ResultObject getLuckDrawDetails(int PartySignUpID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            if (AppType == 1)
            {
                AppType = CustomerVO2.AppType;
            }
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            CardPartySignUpViewVO cVO = cBO.FindSignUpViewById(PartySignUpID);

            if (cVO == null)
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

            if (cVO.SignUpQRCodeImg == "")
            {
                cVO.SignUpQRCodeImg = cBO.GetCardPartySignUpQR(cVO.PartySignUpID);
            }

            cVO.isSponsor = false;
            if (cVO.HostCustomerId == customerId)
                cVO.isSponsor = true;
            else
            {
                List<CardPartyContactsViewVO> ContactsVO = cBO.FindPartyContactsByPartyId(cVO.PartyID);
                for (int i = 0; i < ContactsVO.Count; i++)
                {
                    if (customerId == ContactsVO[i].CustomerId)
                    {
                        cVO.isSponsor = true;
                    }
                }
            }

            if (cVO.CustomerId != customerId && !cVO.isSponsor)
            {
                if (cVO.Type == 3)
                    return new ResultObject() { Flag = 0, Message = "只有本人和举办方才能查看抽奖券!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "只有本人和举办方才能查看入场券!", Result = null };
            }

            List<CardPartySignUpVO> sVO = cBO.FindSignUpByPartyID(cVO.PartyID);

            CardPartyCostVO FirstPrize = new CardPartyCostVO();
            List<CardPartyCostVO> FirstPrizeCostVO = cBO.FindCostByFirstPrize(cVO.PartyID);

            if (FirstPrizeCostVO.Count > 0)
            {
                FirstPrize = FirstPrizeCostVO[0];
            }
            else
            {
                List<CardPartyCostVO> CostVO = cBO.FindCostByPartyID(cVO.PartyID);
                if (CostVO.Count > 0)
                {
                    FirstPrize = CostVO[0];
                }
            }

            decimal AverageWinningRate = (decimal)FirstPrize.limitPeopleNum / (decimal)sVO.Count;
            if (AverageWinningRate > 1)
                AverageWinningRate = 1;
            AverageWinningRate = decimal.Round(AverageWinningRate * 100, 2);

            List<CardPartySignUpVO> InviterVO = new List<CardPartySignUpVO>();

            foreach (CardPartySignUpVO item in sVO)
            {
                if (item.InviterCID == cVO.CustomerId && item.CustomerId != cVO.CustomerId)
                {
                    CardPartySignUpVO SignUpVO = new CardPartySignUpVO();
                    SignUpVO.Name = item.Name;
                    SignUpVO.CreatedAt = item.CreatedAt;
                    SignUpVO.CustomerId = item.CustomerId;
                    SignUpVO.Headimg = item.Headimg;
                    InviterVO.Add(SignUpVO);
                }
            }

            int ranking = 0;
            int RateCount = 0;
            foreach (CardPartySignUpVO item in sVO)
            {
                List<CardPartySignUpVO> ItemInviterVO = new List<CardPartySignUpVO>();
                foreach (CardPartySignUpVO j in sVO)
                {
                    if (j.InviterCID == item.CustomerId && j.CustomerId != item.CustomerId)
                    {
                        ItemInviterVO.Add(j);
                    }
                }

                RateCount += ItemInviterVO.Count + 1;

                if (ItemInviterVO.Count >= InviterVO.Count)
                {
                    ranking++;
                }
            }

            decimal WinningRate = (decimal)(InviterVO.Count + 1) / RateCount;
            WinningRate = decimal.Round(WinningRate * 100, 2);


            decimal Defeated = (decimal)(sVO.Count - ranking) / sVO.Count;
            if (sVO.Count == 1)
            {
                Defeated = 1;
            }
            Defeated = decimal.Round(Defeated * 100, 2);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { SignUp = sVO, AverageWinningRate = AverageWinningRate, WinningRate = WinningRate, Defeated = Defeated, InviterList = InviterVO, FirstPrize = FirstPrize } };
        }


        /// <summary>
        /// 获取入场券
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMySignUpDetailsByCustomerId"), HttpGet]
        public ResultObject getMySignUpDetailsByCustomerId(int PartyID, int CustomerId, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);

            if (AppType == 1)
            {
                AppType = CustomerVO2.AppType;
            }

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardPartyVO pVO = cBO.FindPartyById(PartyID);
            List<CardPartySignUpViewVO> cVO = cBO.FindSignUpViewById(CustomerId, PartyID);

            if (cVO == null || cVO.Count <= 0)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };

            List<CardPartySignUpVO> sVO = cBO.FindSignUpByPartyID(PartyID);
            sVO.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));

            for (int i = 0; i < cVO.Count; i++)
            {
                if (cVO[i].SignUpQRCodeImg == "")
                {
                    cVO[i].SignUpQRCodeImg = cBO.GetCardPartySignUpQR(cVO[i].PartySignUpID);
                }

                cVO[i].isSponsor = false;
                if (cVO[i].HostCustomerId == customerId)
                    cVO[i].isSponsor = true;
                else
                {
                    List<CardPartyContactsViewVO> ContactsVO = cBO.FindPartyContactsByPartyId(cVO[i].PartyID);
                    for (int j = 0; j < ContactsVO.Count; j++)
                    {
                        if (customerId == ContactsVO[j].CustomerId)
                        {
                            cVO[i].isSponsor = true;
                        }
                    }
                }
                cVO[i].CardPartyContactsView = cBO.FindPartyContactsByPartyId(cVO[i].PartyID);

                int number = 1;
                for (int j = 0; j < sVO.Count; j++)
                {
                    if (sVO[j].PartySignUpID == cVO[i].PartySignUpID)
                    {
                        number = j + 1;
                    }
                }
                cVO[i].Sequence = number;
            }

            if (cVO[0].CustomerId != customerId && !cVO[0].isSponsor)
            {
                if (pVO.Type == 3)
                    return new ResultObject() { Flag = 0, Message = "只有本人和举办方才能查看抽奖券!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "只有本人和举办方才能查看入场券!", Result = null };
            }

            List<CardPartySignUpViewVO> newCVO = new List<CardPartySignUpViewVO>();
            if (pVO.Type == 3)
                newCVO.Add(cVO[0]);
            else
                newCVO = cVO;

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newCVO };
        }

        /// <summary>
        /// 核销入场券
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("VerificationSignUp"), HttpGet]
        public ResultObject VerificationSignUp(int PartySignUpID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardPartySignUpViewVO cVO = cBO.FindSignUpViewById(PartySignUpID);

            if (cVO == null)
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };

            cVO.isSponsor = false;
            if (cVO.HostCustomerId == customerId)
                cVO.isSponsor = true;
            else
            {
                List<CardPartyContactsViewVO> ContactsVO = cBO.FindPartyContactsByPartyId(cVO.PartyID);
                for (int i = 0; i < ContactsVO.Count; i++)
                {
                    if (customerId == ContactsVO[i].CustomerId)
                    {
                        cVO.isSponsor = true;
                    }
                }
            }

            if (!cVO.isSponsor)
                return new ResultObject() { Flag = 0, Message = "只有举办方才能核销入场券!", Result = null };
            if (cVO.SignUpStatus == 1)
                return new ResultObject() { Flag = 0, Message = "该券已经核销过了，请勿重复操作!", Result = null };
            if (cVO.SignUpStatus == 2)
                return new ResultObject() { Flag = 0, Message = "该入场券已退款，无法核销!", Result = null };

            CardPartySignUpVO cuVO = new CardPartySignUpVO();
            cuVO.PartySignUpID = PartySignUpID;
            cuVO.SignUpStatus = 1;
            cBO.UpdateSignUp(cuVO);

            if (cBO.UpdateSignUp(cuVO))
            {
                try
                {
                    //将报名的名片加入活动的名片组
                    CardPartyVO cpVO = cBO.FindPartyById(cVO.PartyID);
                    if (cpVO != null && cpVO.GroupID != 0)
                    {
                        List<CardGroupCardViewVO> cgVO = cBO.isJionCardGroup(cVO.CustomerId, cpVO.GroupID);
                        if (cgVO.Count <= 0)
                        {
                            CardGroupCardVO cgcVO = new CardGroupCardVO();
                            cgcVO.CustomerId = cVO.CustomerId;
                            cgcVO.GroupID = cVO.GroupID;

                            CardGroupVO gVO = cBO.FindCardGroupById(cpVO.GroupID);
                            cgcVO.Status = 1;
                            cgcVO.CreatedAt = DateTime.Now;
                            cgcVO.CardID = cVO.CardID;
                            cBO.AddCardToGroup(cgcVO);
                        }
                        else if (cgVO[0].Status == 0)
                        {
                            CardGroupCardVO cgcVO = new CardGroupCardVO();
                            cgcVO.GroupCardID = cgVO[0].GroupCardID;

                            CardGroupVO gVO = cBO.FindCardGroupById(cpVO.GroupID);
                            cgcVO.Status = 1;

                            cBO.UpdateCardToGroup(cgcVO);
                        }
                    }
                }
                catch
                {

                }
                //if (!string.IsNullOrEmpty(cVO.SeatNo)) {
                //    cBO.SendSMS(cVO);
                //}
                return new ResultObject() { Flag = 1, Message = "核销成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "核销失败，请重试!", Result = null };
            }
        }

        /// <summary>
        /// 用户本人核销入场券
        /// </summary>
        /// <param name="PartyID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("VerificationSignUpByUser"), HttpGet]
        public ResultObject VerificationSignUpByUser(int PartyID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            List<CardPartySignUpVO> cuVO = cBO.FindSignUpByPartyID(PartyID, customerId);

            if (cuVO.Count <= 0)
                return new ResultObject() { Flag = 0, Message = "您并未报名本活动!", Result = null };

            CardPartySignUpViewVO cVO = cBO.FindSignUpViewById(cuVO[0].PartySignUpID);

            if (cVO.SignUpStatus == 1)
                return new ResultObject() { Flag = 0, Message = "已经签到过了，请勿重复操作!", Result = null };
            if (cVO.SignUpStatus == 2)
                return new ResultObject() { Flag = 0, Message = "该入场券已退款，无法核销!", Result = null };

            for (int i = 0; i < cuVO.Count; i++)
            {
                CardPartySignUpVO csVO = new CardPartySignUpVO();
                csVO.PartySignUpID = cuVO[i].PartySignUpID;
                csVO.SignUpStatus = 1;
                cBO.UpdateSignUp(csVO);
            }

            try
            {
                //将报名的名片加入活动的名片组
                CardPartyVO cpVO = cBO.FindPartyById(cVO.PartyID);
                if (cpVO != null && cpVO.GroupID != 0)
                {
                    List<CardGroupCardViewVO> cgVO = cBO.isJionCardGroup(cVO.CustomerId, cpVO.GroupID);
                    if (cgVO.Count <= 0)
                    {
                        CardGroupCardVO cgcVO = new CardGroupCardVO();
                        cgcVO.CustomerId = cVO.CustomerId;
                        cgcVO.GroupID = cVO.GroupID;

                        CardGroupVO gVO = cBO.FindCardGroupById(cpVO.GroupID);
                        cgcVO.Status = 1;

                        cgcVO.CreatedAt = DateTime.Now;
                        cgcVO.CardID = cVO.CardID;
                        cBO.AddCardToGroup(cgcVO);
                    }
                    else if (cgVO[0].Status == 0)
                    {
                        CardGroupCardVO cgcVO = new CardGroupCardVO();
                        cgcVO.GroupCardID = cgVO[0].GroupCardID;

                        CardGroupVO gVO = cBO.FindCardGroupById(cpVO.GroupID);
                        cgcVO.Status = 1;

                        cBO.UpdateCardToGroup(cgcVO);
                    }
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "签到失败，请重试!", Result = null };
            }
            return new ResultObject() { Flag = 1, Message = "签到成功!", Result = null };
        }

        /// <summary>
        /// 名片识别
        /// </summary>
        /// <returns></returns>
        [Route("BusinessCardRecognition"), HttpPost, Anonymous]
        public ResultObject BusinessCardRecognition(int AppType = 1)
        {
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);
                var result = cBO.BusinessCardRecognitionBYTencent(hfc);
                if (result != null)
                {
                    return new ResultObject() { Flag = 1, Message = "识别成功", Result = result };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "识别失败，请重试", Result = result };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "上传图片失败", Result = "文件为空" };
            }
        }

        /// <summary>
        /// 名片识别
        /// </summary>
        /// <returns></returns>
        [Route("BusinessCardRecognitionByToken"), HttpPost]
        public ResultObject BusinessCardRecognitionByToken(string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);
                var result = cBO.BusinessCardRecognitionBYTencent(hfc);
                if (result != null)
                {
                    return new ResultObject() { Flag = 1, Message = "识别成功", Result = result };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "识别失败，请重试", Result = result };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "上传图片失败", Result = "文件为空" };
            }
        }

        /// <summary>
        /// 测试导出短信excel
        /// </summary>
        /// <returns></returns>
        [Route("testexcel"), HttpGet, Anonymous]
        public ResultObject testexcel()
        {
            if (!CardBO.istest)
            {
                try
                {
                    CardBO cBO = new CardBO(new CustomerProfile(), 1);
                    List<CardByPhoneVO> cVO = cBO.FindCardByPhone("");

                    DataTable dt = new DataTable();
                    dt.Columns.Add("手机号", typeof(String));
                    dt.Columns.Add("name", typeof(String));
                    dt.Columns.Add("Title", typeof(String));
                    dt.Columns.Add("Time", typeof(String));
                    dt.Columns.Add("Address", typeof(String));
                    dt.Columns.Add("PartyID", typeof(String));

                    for (int i = 0; i < cVO.Count; i++)
                    {
                        if (cVO[i].Phone != "")
                        {
                            DataRow row = dt.NewRow();
                            row["手机号"] = cVO[i].Phone.ToString();
                            row["name"] = cVO[i].Name;
                            row["Title"] = "社交资源联盟成立暨乐聊名片答谢宴";
                            row["Time"] = "本周六（26号）";
                            row["Address"] = "共同见证广东省文促汇授牌以乐聊名片为核心工具的社交资源联盟成立";
                            row["PartyID"] = "2601";
                            dt.Rows.Add(row);
                        }
                    }


                    string url = cBO.DataToExcel(dt, "", "CardPhone3.xlsx");

                    return new ResultObject() { Flag = 1, Message = "发送成功", Result = url, Count = cVO.Count };
                }
                catch (Exception ex)
                {
                    return new ResultObject() { Flag = 0, Message = "发送失败", Result = ex };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "正在执行中", Result = null };
            }
        }


        /// <summary>
        /// 获取外部跳转小程序链接
        /// </summary>
        /// <returns></returns>
        [Route("GetUrlScheme"), HttpGet, Anonymous]
        public ResultObject GetUrlScheme(string path, string query, int UrlType = 1, int AppType = 1, int isPermanent = 0)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            string url = "";
            url = cBO.getWxUrl(path, query, AppType);
            if (url != "")
            {
                return new ResultObject() { Flag = 1, Message = "获取成功", Result = url };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败", Result = null };
            }
        }


        /// <summary>
        /// 获取视频号扩展链接
        /// </summary>
        /// <param name="ID">CardID，PartyID，QuestionnaireID</param>
        /// <param name="Style">"Card":个人名片,"Party"：活动链接,"Questionnaire"：表格,"Softarticle"：软文</param>
        /// <param name="isRestart">是否重新生成</param>
        /// <returns></returns>
        [Route("getExtendedLinks"), HttpGet]
        public ResultObject getExtendedLinks(int ID, string Style, string token, int isRestart = 0, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            try
            {
                if (Style == "Card")
                {
                    CardDataVO cvo = cBO.FindCardById(ID);
                    if (cvo == null) { return new ResultObject() { Flag = 0, Message = "参数错误！", Result = null }; }
                    if (cvo.MediaID > 0 && isRestart == 0)
                    {
                        MediaVO mVO = cBO.FindMediaById(cvo.MediaID);
                        if (mVO.Status == 1)
                        {
                            return new ResultObject() { Flag = 1, Message = "获取成功！", Result = mVO.ArticleUrl };
                        }
                    }
                }

                if (Style == "Party")
                {
                    CardPartyVO cvo = cBO.FindPartyById(ID);
                    if (cvo == null) { return new ResultObject() { Flag = 0, Message = "参数错误！", Result = null }; }
                    if (cvo.MediaID > 0 && isRestart == 0)
                    {
                        MediaVO mVO = cBO.FindMediaById(cvo.MediaID);
                        if (mVO.Status == 1)
                        {
                            return new ResultObject() { Flag = 1, Message = "获取成功！", Result = mVO.ArticleUrl };
                        }
                    }
                }

                if (Style == "Questionnaire")
                {
                    CardQuestionnaireVO cvo = cBO.FindCardQuestionnaireByQuestionnaireID(ID);
                    if (cvo == null) { return new ResultObject() { Flag = 0, Message = "参数错误！", Result = null }; }
                    if (cvo.MediaID > 0 && isRestart == 0)
                    {
                        MediaVO mVO = cBO.FindMediaById(cvo.MediaID);
                        if (mVO.Status == 1)
                        {
                            return new ResultObject() { Flag = 1, Message = "获取成功！", Result = mVO.ArticleUrl };
                        }
                    }
                }

                if (Style == "Softarticle")
                {
                    CardSoftArticleVO cvo = cBO.FindSoftArticleById(ID);
                    if (cvo == null) { return new ResultObject() { Flag = 0, Message = "参数错误！", Result = null }; }
                    if (cvo.MediaID > 0 && isRestart == 0)
                    {
                        MediaVO mVO = cBO.FindMediaById(cvo.MediaID);
                        if (mVO.Status == 1)
                        {
                            return new ResultObject() { Flag = 1, Message = "获取成功！", Result = mVO.ArticleUrl };
                        }
                    }
                }

                int MediaCount = cBO.FindMediaTotalCount("Status=1 and date(CreatedAt)=curdate()");
                if (MediaCount >= 100) { return new ResultObject() { Flag = 0, Message = "今日生成链接的名额已被抢光，请明天再来吧！", Result = null }; }
                int MyMediaCount = cBO.FindMediaTotalCount("CustomerId=" + customerId + " and Status=1 and date(CreatedAt)=curdate()");
                if (MyMediaCount >= 5) { return new ResultObject() { Flag = 0, Message = "每人每天只能生成5条链接，请明天再来吧！", Result = null }; }

                string back = cBO.getArticleUrl(ID, Style, isRestart);

                if (back == "SUCCESS")
                {
                    return new ResultObject() { Flag = 2, Message = "正在生成中，请稍后再来查看！", Result = null };
                }

                if (back == "ERROR")
                {
                    return new ResultObject() { Flag = 0, Message = "生成失败，请重试！", Result = null };
                }

                return new ResultObject() { Flag = 1, Message = "生成成功", Result = back };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "生成失败，请重试！", Result = ex };
            }
        }


        /// <summary>
        /// 获取头条新闻存入本地数据库
        /// </summary>
        /// <returns></returns>
        [Route("GetCsharpNews"), HttpGet, Anonymous]
        public ResultObject GetCsharpNews()
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            try
            {
                CsharpVO CsharpVO = CsharpTest.Main("新闻", 0);
                if (CsharpVO != null)
                {
                    List<CsharpList> cL = CsharpVO.result.list;
                    for (int j = 0; j < cL.Count; j++)
                    {
                        if (cBO.FindSoftArticleTotalCount("Title = '" + cL[j].title + "'") <= 0)
                        {
                            CardSoftArticleVO sVO = new CardSoftArticleVO();
                            sVO.Title = cL[j].title;
                            sVO.CreatedAt = DateTime.Now;
                            sVO.Image = cL[j].pic;
                            sVO.Description = ConvertCsharpNews(cL[j].content);
                            sVO.OriginalMedia = cL[j].src;
                            sVO.IsOriginal = false;
                            sVO.Status = 2;
                            sVO.CustomerId = 14;
                            sVO.OriginalCustomerId = 14;
                            sVO.CardID = 337;

                            SystemBO sBO = new SystemBO(new UserProfile());
                            ConfigVO vo = sBO.FindConfig();
                            sVO.PartyID = vo.CompanyPartyID;

                            Random rd = new Random();
                            int ReadCount = rd.Next(2000, 5000);

                            sVO.ReadCount = ReadCount;
                            sVO.GoodCount = rd.Next(ReadCount / 50, ReadCount / 10);
                            sVO.ReprintCount = rd.Next(ReadCount / 100, ReadCount / 20);
                            sVO.ExposureCount = ReadCount * 3;
                            cBO.AddSoftArticle(sVO);
                        }
                    }

                }

                return new ResultObject() { Flag = 1, Message = "获取成功", Result = CsharpVO };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }
        /// <summary>
        /// 转换头条新闻格式
        /// </summary>
        /// <returns></returns>
        [Route("ConvertCsharpNews"), HttpGet, Anonymous]
        public string ConvertCsharpNews(string html)
        {
            html = html.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace("</p>", "</div></NTAG>").Replace("</h2>", "</div></NTAG>");
            Regex r = new Regex("<p.*?>", RegexOptions.IgnoreCase);
            html = r.Replace(html, "<div>");
            r = new Regex("<h2.*?>", RegexOptions.IgnoreCase);
            html = r.Replace(html, "<div>");
            r = new Regex("<div.*?>", RegexOptions.IgnoreCase);
            html = r.Replace(html, "<div>");
            r = new Regex("class=\".*?\"", RegexOptions.IgnoreCase);
            html = r.Replace(html, "");
            r = new Regex("alt=\".*?\"", RegexOptions.IgnoreCase);
            html = r.Replace(html, "");
            r = new Regex("<div>原标题.*?</div></NTAG>", RegexOptions.IgnoreCase);
            html = r.Replace(html, "");
            r = new Regex("<!-- content end -->", RegexOptions.IgnoreCase);
            html = r.Replace(html, "");

            HtmlParser HtmlParser = new HtmlParser(html.Trim());
            HtmlParser.KeepTag(new string[] { "div", "NTAG", "img" });
            html = HtmlParser.Text();
            html = html.Replace("<div></div>", "");
            html = html.Replace("<div><img", "<img");
            html = html.Replace("</NTAG></div>", "</NTAG>");

            return html;
        }


        /// <summary>
        /// 保存FormId
        /// </summary>
        /// <param name="FormId"></param>
        /// <param name="code"></param>
        /// <param name="token">口令</param>
        /// <param name="Style">类型</param>
        /// <returns></returns>
        [Route("AddFormId"), HttpPost]
        public ResultObject AddFormId(string FormId, string code, string token, int Style)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardFormListVO suVO = new CardFormListVO();

            if (FormId == "the formId is a mock one")
            {
                return new ResultObject() { Flag = 0, Message = "无效FormId!", Result = null };
            }

            int num = cBO.GetFormListByFormIdCount(FormId);

            if (num > 0) { return new ResultObject() { Flag = 0, Message = "重复FormId！", Result = null }; }

            suVO.Style = Style;
            suVO.FormId = FormId;
            suVO.CustomerId = customerId;
            suVO.CreatedAt = DateTime.Now;
            suVO.OpenId = cBO.getOpenId(code);

            int FormListID = cBO.AddFormId(suVO);
            if (FormListID > 0)
            {
                return new ResultObject() { Flag = 1, Message = "FormId保存成功!", Result = FormListID };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "保存失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新每日通知(后台专用)
        /// </summary>
        /// <param name="CardNoticeVO">每日通知VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddCardMessage"), HttpPost]
        public ResultObject AddCardMessage([FromBody] CardNoticeVO CardNoticeVO, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (CardNoticeVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            if (CardNoticeVO.NoticeID > 0)
            {
                if (cBO.UpdateNotice(CardNoticeVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardNoticeVO.NoticeID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                int NoticeID = cBO.AddNotice(CardNoticeVO);
                if (NoticeID > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = NoticeID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取每日通知
        /// </summary>
        /// <param name="NoticeID">每日通知ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetCardMessage"), HttpGet]
        public ResultObject GetCardMessage(int NoticeID, string token, int AppType = 1)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            CardNoticeVO vo = cBO.FindCardMessageById(NoticeID);
            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 删除每日通知
        /// </summary>
        /// <param name="NoticeID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteCardMessage"), HttpGet]
        public ResultObject DeleteCardMessage(string NoticeID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            try
            {
                if (!string.IsNullOrEmpty(NoticeID))
                {
                    string[] messageIdArr = NoticeID.Split(',');
                    bool isAllDelete = true;
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {

                            cBO.DeleteNoticeById(Convert.ToInt32(messageIdArr[i]));
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }

        }

        /// <summary>
        /// 添加或更新今日话题
        /// </summary>
        /// <param name="CardNewsVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddCardNews"), HttpPost]
        public ResultObject AddCardNews([FromBody] CardNewsVO CardNewsVO, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (CardNewsVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            if (CardNewsVO.NewsID > 0)
            {
                if (cBO.UpdateNews(CardNewsVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardNewsVO.NewsID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                CardNewsVO.isSend = 0;
                CardNewsVO.CreatedAt = DateTime.Now;
                int NewsID = cBO.AddNews(CardNewsVO);
                if (NewsID > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = NewsID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }



        /// <summary>
        /// 获取今日话题
        /// </summary>
        /// <param name="NewsID">今日话题ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetCardNews"), HttpGet]
        public ResultObject GetCardNews(int NewsID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            CardNewsVO vo = cBO.FindCardNewsById(NewsID);
            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 增加今日话题点击数
        /// </summary>
        /// <param name="NewsID">今日话题ID</param>
        /// <returns></returns>
        [Route("ClickCardNews"), HttpGet, Anonymous]
        public ResultObject ClickCardNews(int NewsID, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            CardNewsVO vo = cBO.FindCardNewsById(NewsID);
            if (vo != null)
            {
                vo.ClickCount += 1;
                if (cBO.UpdateNews(vo))
                {
                    return new ResultObject() { Flag = 1, Message = "点击成功!", Result = vo };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "点击失败!", Result = null };
                }
            }
            else
                return new ResultObject() { Flag = 0, Message = "点击失败!", Result = null };

        }

        /// <summary>
        /// 删除今日话题
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteCardNews"), HttpGet]
        public ResultObject DeleteCardNews(string NewsID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);
            try
            {
                if (!string.IsNullOrEmpty(NewsID))
                {
                    string[] messageIdArr = NewsID.Split(',');
                    bool isAllDelete = true;
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {
                            cBO.DeleteNewsById(Convert.ToInt32(messageIdArr[i]));
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }

        }

        /// <summary>
        /// 设置今日话题为展示
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("EditDefaultCardNews"), HttpGet]
        public ResultObject EditDefaultCardNews(int NewsID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            CardNewsVO vo = cBO.FindCardNewsById(NewsID);

            if (vo != null)
            {
                //cBO.UpdateCardNewsToOff();
                vo.isDefault = 1;
                cBO.UpdateNews(vo);
                return new ResultObject() { Flag = 1, Message = "设置成功!", Result = vo };
            }

            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };

        }

        /// <summary>
        /// 设置今日话题为不展示
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelDefaultCardNews"), HttpGet]
        public ResultObject DelDefaultCardNews(int NewsID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            CardNewsVO vo = cBO.FindCardNewsById(NewsID);

            if (vo != null)
            {
                //cBO.UpdateCardNewsToOff();
                vo.isDefault = 0;
                cBO.UpdateNews(vo);
                return new ResultObject() { Flag = 1, Message = "设置成功!", Result = vo };
            }

            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };

        }

        /// <summary>
        /// 设置今日话题为弹窗
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("EditAlertCardNews"), HttpGet]
        public ResultObject EditAlertCardNews(int NewsID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            CardNewsVO vo = cBO.FindCardNewsById(NewsID);

            if (vo != null)
            {
                cBO.UpdateCardNewsToOff();
                vo.isAlert = 1;
                cBO.UpdateNews(vo);
                return new ResultObject() { Flag = 1, Message = "设置成功!", Result = vo };
            }

            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };

        }

        /// <summary>
        /// 设置今日话题为不弹窗
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelAlertCardNews"), HttpGet]
        public ResultObject DelAlertCardNews(int NewsID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            CardNewsVO vo = cBO.FindCardNewsById(NewsID);

            if (vo != null)
            {
                //cBO.UpdateCardNewsToOff();
                vo.isAlert = 0;
                cBO.UpdateNews(vo);
                return new ResultObject() { Flag = 1, Message = "设置成功!", Result = vo };
            }

            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };

        }

        /// <summary>
        /// 获取默认今日话题
        /// </summary>
        /// <returns></returns>
        [Route("EditDefaultCardNews"), HttpGet, Anonymous]
        public ResultObject EditDefaultCardNews(int isList = 0, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            List<CardNewsVO> vo = cBO.FindCardNewsList();
            if (vo.Count > 0)
            {

                List<CardNewsVO> vo2 = cBO.FindCardNewsListByAlert();

                CardNewsVO Subsidiary = new CardNewsVO();

                if (vo2.Count > 0)
                {
                    Subsidiary = vo2[0];
                }
                else
                {
                    Subsidiary = null;
                }

                if (isList == 0)
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo[0], Subsidiary = Subsidiary };
                else
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo, Subsidiary = Subsidiary };
            }
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 发送模板消息 每发一条延迟一秒
        /// </summary>
        /// <returns></returns>
        public async void Delay(int FormListID, string Content)
        {
            Thread.Sleep(1000);
            CardBO cBO = new CardBO(new CustomerProfile());
            cBO.sendDailyRecordMessage(FormListID, Content);
        }


        /// <summary>
        ///名片新用户发送指定模板消息
        /// </summary>
        /// <returns></returns>
        public void SendNewCardMessage(int Days, int FormListID)
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            if (Days == 1)
            {
                cBO.sendDailyRecordMessage(FormListID, "好东西要分享的，别私藏您的乐聊名片呀，群发出去或把名片海报分享到朋友圈会有意外惊喜呢。");
            }
            if (Days == 4)
            {
                cBO.sendDailyRecordMessage(FormListID, "名片浏览量不高呀，以您身份若换个皮肤或发起个聚会或建个同学录，收藏量马上就飙到上千了。");
            }
            if (Days == 6)
            {
                cBO.sendDailyRecordMessage(FormListID, "乐聊名片招城市合伙人、区域运营商等合作伙伴，关注公众号“乐聊名片”了解详情吧");
            }
        }

        /// <summary>
        ///活动新用户发送指定模板消息
        /// </summary>
        /// <returns></returns>
        public void SendNewCardByPartyMessage(int Days, int FormListID)
        {
            if (Days == 1)
            {
                Delay(FormListID, "您报名后，我们已为您生成免费智能名片，赶快去完善信息，用乐聊名片推介自己吧");
            }
            if (Days == 3)
            {
                Delay(FormListID, "浏览量不高呀，以您身份若换个名片皮肤或搞个聚会或建个同学录或名片海报群发，收藏量马上就飙到上千了。");
            }
            if (Days == 6)
            {
                Delay(FormListID, "乐聊名片招城市合伙人、区域运营商等合作伙伴，关注公众号“乐聊名片”了解详情吧");
            }
        }
        /// <summary>
        /// 更新用户来源 每个用户只更新一次
        /// </summary>
        /// <param name="originCustomerId">来源Id</param>
        /// <param name="CustomerId">用户Id</param>
        ///  <param name="token">口令</param>
        /// <returns></returns>
        /// 
        [Route("updateCustomerOriginId"), HttpGet]
        public ResultObject updateCustomerOriginId(int originCustomerId, int CustomerId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            int customerId = uProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            if (uProfile.CustomerId != CustomerId) { return new ResultObject() { Flag = 0, Message = "权限不足，更新失败！", Result = null }; }

            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerVO cvo = new CustomerVO();
            CustomerVO cvo2 = cuBO.FindCustomenById(CustomerId);
            if (cvo2.originCustomerId != 0) { return new ResultObject() { Flag = 2, Message = "来源ID已经存在！", Result = cvo2.originCustomerId }; }

            //以前的不会修改

            DateTime theDay = Convert.ToDateTime("2019-03-04");
            DateTime Nowday = DateTime.Now;
            DateTime createdAt = Convert.ToDateTime(cvo2.CreatedAt.ToShortDateString());
            TimeSpan compNum = theDay - createdAt;

            TimeSpan compNum2 = Nowday - createdAt;

            if (compNum.Days > 0 || originCustomerId == CustomerId || originCustomerId > CustomerId || compNum2.Days > 7) { return new ResultObject() { Flag = 0, Message = "不更新", Result = null }; }

            cvo.originCustomerId = originCustomerId;
            cvo.CustomerId = CustomerId;
            bool ret = cuBO.Update(cvo);

            if (ret) { return new ResultObject() { Flag = 1, Message = "更新成功！", Result = null }; }
            else { return new ResultObject() { Flag = 0, Message = "更新失败！", Result = null }; }
        }




        /// <summary>
        /// 测试更新注册日期
        /// </summary>
        /// <param name="CustomerId">用户Id</param>
        /// <returns></returns>
        [Route("updateCustomerCreateDate"), HttpGet, Anonymous]
        public Boolean updateCustomerCreateDate(int CustomerId)
        {
            if (CustomerId == 0) { return false; }
            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerVO cvo = new CustomerVO();
            //DateTime.Now.AddDays(-1).ToShortDateString() 昨天
            cvo.SetValue("CreatedAt", DateTime.Now);
            cvo.SetValue("CustomerId", CustomerId);//2551
            Boolean ret = cuBO.Update(cvo);
            return ret;
        }

        /// <summary>
        /// 测试发送留言提醒（推广通知）
        /// </summary>
        /// <returns></returns>
        [Route("sendMessageTest"), HttpGet, Anonymous]
        public ResultObject sendMessageTest(int isOpenSend = 0, int sendCount = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), 1);
            if (isOpenSend == 1)
            {
                CardBO.isOpenSend = true;
                return new ResultObject() { Flag = 1, Message = "开启成功!", Result = CardBO.isOpenSend };
            }

            try
            {
                return new ResultObject() { Flag = 1, Message = "发送成功!", Result = cBO.sendMessage(sendCount, 1, "CustomerId=2551") };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "发送失败!", Result = null };
            }

        }

        /// <summary>
        /// 发送每日记录提醒
        /// </summary>
        /// <returns></returns>
        [Route("sendDailyRecordMessage"), HttpGet, Anonymous]
        public string sendDailyRecordMessage()
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerViewVO cvo = new CustomerViewVO();
            try
            {

                //获取当天的通知
                CardNoticeVO cnVO = cBO.FindCardMessageByToday();

                if (cnVO == null)
                {
                    List<CardFormListViewVO> cVO = cBO.FindFormListView();
                    try
                    {
                        for (int i = 0; i < cVO.Count; i++)
                        {
                            cvo = cuBO.FindById(cVO[i].CustomerId);
                            DateTime now = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                            DateTime createdAt = Convert.ToDateTime(cvo.CreatedAt.ToShortDateString());
                            TimeSpan ts = now - createdAt;
                            //活动新用户
                            if (cVO[i].Style == 2) { SendNewCardByPartyMessage(ts.Days, cVO[i].FormListID); }
                            else { SendNewCardMessage(ts.Days, cVO[i].FormListID); }

                        }
                    }
                    catch { }
                }

                if (cnVO != null)
                {
                    List<CardFormListViewVO> cVO = cBO.FindFormListView();
                    for (int i = 0; i < cVO.Count; i++)
                    {
                        cvo = cuBO.FindById(cVO[i].CustomerId);
                        DateTime now = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        DateTime createdAt = Convert.ToDateTime(cvo.CreatedAt.ToShortDateString());
                        TimeSpan ts = now - createdAt;

                        try
                        {
                            //仅发送给活动用户
                            if (cnVO.Style == 1)
                            {
                                if (cVO[i].Style == 2)
                                {
                                    Delay(cVO[i].FormListID, cnVO.Content);
                                }
                            }
                            else //发送给全体成员
                            {
                                Delay(cVO[i].FormListID, cnVO.Content);
                            }
                        }
                        catch
                        {

                        }
                    }
                    cnVO.Status = 1;
                    cBO.UpdateNotice(cnVO);
                }

                // 发送VIP即将到期提醒



                return "成功发送提醒";
            }
            catch
            {
                return "发送失败!";
            }

        }





        /// <summary>
        /// 发送每日记录提醒
        /// </summary>
        /// <returns></returns>
        [Route("sendDailyRecordMessage2"), HttpGet, Anonymous]
        public string sendDailyRecordMessage2(int FormListID, string Content)
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            try
            {
                //获取当天的通知
                cBO.sendDailyRecordMessage(FormListID, Content);
                return "成功发送提醒";
            }
            catch
            {
                return "发送失败!";
            }

        }


        /// <summary>
        /// 发送活动提醒(每个小时执行一次)
        /// </summary>
        /// <returns></returns>
        [Route("sendPartyMessage"), HttpGet, Anonymous]
        public string sendPartyMessage(int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            /*
            try
            {
                //获取1天后即将截止报名的活动
                List<CardPartyViewVO> CardPartyViewVO = cBO.FindCardPartyViewByDaySignUp();
                for (int i = 0; i < CardPartyViewVO.Count; i++)
                {
                    try
                    {
                        cBO.AddCardMessage("《" + CardPartyViewVO[i].Title + "》1天后即将截止报名", CardPartyViewVO[i].CustomerId, "活动报名", "/pages/Party/PartyShow/PartyShow?PartyID=" + CardPartyViewVO[i].PartyID);

                        CardPartyVO cpVO = new CardPartyVO();
                        cpVO.PartyID = CardPartyViewVO[i].PartyID;
                        cpVO.SignUpSendStatus = 1;
                        cBO.UpdatePartyA(cpVO);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }

            try
            {
                //获取截止报名的活动
                List<CardPartyViewVO> CardPartyViewVO = cBO.FindCardPartyViewBySignUp();
                for (int i = 0; i < CardPartyViewVO.Count; i++)
                {
                    try
                    {
                        cBO.AddCardMessage("《" + CardPartyViewVO[i].Title + "》已截止报名", CardPartyViewVO[i].CustomerId, "活动报名", "/pages/Party/PartyShow/PartyShow?PartyID=" + CardPartyViewVO[i].PartyID);

                        CardPartyVO cpVO = new CardPartyVO();
                        cpVO.PartyID = CardPartyViewVO[i].PartyID;
                        cpVO.SignUpSendStatus = 2;
                        cBO.UpdatePartyA(cpVO);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
            */
            try
            {
                //获取一天后开始活动列表
                List<CardPartyViewVO> CardPartyViewVO = cBO.FindCardPartyViewByDayStart();
                for (int i = 0; i < CardPartyViewVO.Count; i++)
                {
                    try
                    {
                        List<CardPartySignUpVO> CardPartySignUpVO = cBO.FindSignUpByPartyID(CardPartyViewVO[i].PartyID);
                        for (int j = 0; j < CardPartySignUpVO.Count; j++)
                        {
                            try
                            {
                                cBO.AddCardMessage("距离开始还剩12小时—《" + CardPartyViewVO[i].Title + "》", CardPartySignUpVO[j].CustomerId, "活动报名", "/pages/Party/SignUpShow/SignUpShow?PartySignUpID=" + CardPartySignUpVO[j].PartySignUpID);
                            }
                            catch
                            {

                            }
                        }
                        CardPartyVO cpVO = new CardPartyVO();
                        cpVO.PartyID = CardPartyViewVO[i].PartyID;
                        cpVO.StartSendStatus = 1;
                        cBO.UpdatePartyA(cpVO);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }

            try
            {
                //获取两个小时后开始活动列表
                List<CardPartyViewVO> CardPartyViewVO = cBO.FindCardPartyViewByStart();
                for (int i = 0; i < CardPartyViewVO.Count; i++)
                {
                    try
                    {
                        List<CardPartySignUpVO> CardPartySignUpVO = cBO.FindSignUpByPartyID(CardPartyViewVO[i].PartyID);
                        for (int j = 0; j < CardPartySignUpVO.Count; j++)
                        {
                            try
                            {
                                cBO.AddCardMessage("距离开始还剩3小时—《" + CardPartyViewVO[i].Title + "》", CardPartySignUpVO[j].CustomerId, "活动报名", "/pages/Party/SignUpShow/SignUpShow?PartySignUpID=" + CardPartySignUpVO[j].PartySignUpID);
                                /*
                                if (CardPartySignUpVO[j].Phone != "")
                                {
                                    MessageTool.ALSendSms(CardPartySignUpVO[j].Phone, "SMS_202806251", "{\"Title\":\"" + CardPartyViewVO[i].Title + "\"}");
                                }
                                */
                            }
                            catch
                            {

                            }
                        }
                        CardPartyVO cpVO = new CardPartyVO();
                        cpVO.PartyID = CardPartyViewVO[i].PartyID;
                        cpVO.StartSendStatus = 2;
                        cBO.UpdatePartyA(cpVO);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
            /*
            try
            {
                //获取截止报名的活动
                List<CardPartyViewVO> CardPartyViewVO = cBO.FindCardPartyViewBySignUp();
                for (int i = 0; i < CardPartyViewVO.Count; i++)
                {
                    try
                    {
                        cBO.AddCardMessage("《" + CardPartyViewVO[i].Title + "》已截止报名", CardPartyViewVO[i].CustomerId, "活动报名", "/pages/Party/PartyShow/PartyShow?PartyID=" + CardPartyViewVO[i].PartyID);

                        CardPartyVO cpVO = new CardPartyVO();
                        cpVO.PartyID = CardPartyViewVO[i].PartyID;
                        cpVO.SignUpSendStatus = 2;
                        cBO.UpdatePartyA(cpVO);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
            */

            try
            {
                //获取7天后VIP到期的会员
                List<CustomerVO> CustomerVO = uBO.FindListByParams("ExpirationAt <= DATE_ADD(NOW(),INTERVAL 7 DAY) and ExpirationAt >= NOW() and ExpirationSendStatus=0");
                for (int i = 0; i < CustomerVO.Count; i++)
                {
                    try
                    {
                        cBO.AddCardMessage("您的会员特权将于7天后到期哦！", CustomerVO[i].CustomerId, "会员特权", "/pages/MyCenter/MyCenter/MyCenter", "switchTab");

                        CustomerVO cpVO = new CustomerVO();
                        cpVO.CustomerId = CustomerVO[i].CustomerId;
                        cpVO.ExpirationSendStatus = 1;
                        uBO.Update(cpVO);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }


            try
            {
                //获取3天后VIP到期的会员
                List<CustomerVO> CustomerVO = uBO.FindListByParams("ExpirationAt <= DATE_ADD(NOW(),INTERVAL 3 DAY) and ExpirationAt >= NOW() and ExpirationSendStatus=1");
                for (int i = 0; i < CustomerVO.Count; i++)
                {
                    try
                    {
                        cBO.AddCardMessage("您的会员特权将于3天后到期哦！", CustomerVO[i].CustomerId, "会员特权", "/pages/MyCenter/MyCenter/MyCenter", "switchTab");

                        CustomerVO cpVO = new CustomerVO();
                        cpVO.CustomerId = CustomerVO[i].CustomerId;
                        cpVO.ExpirationSendStatus = 2;
                        uBO.Update(cpVO);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
            return "发送成功!";
        }

        /// <summary>
        /// 发送每周报告
        /// </summary>
        /// <returns></returns>
        [Route("sendWeeklyReport"), HttpGet, Anonymous]
        public ResultObject sendWeeklyReport(int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            try
            {
                //获取最近一周登录的VIP
                List<CustomerRecentLoginViewVO> LoginList = uBO.GetCustomerRecentLoginView("LoginAt > DATE_SUB(NOW(),INTERVAL 7 DAY) and isVip=1");

                for (int i = 0; i < LoginList.Count; i++)
                {
                    try
                    {
                        int CustomerId = LoginList[i].CustomerId;
                        int ForwardCardCount = cBO.FindAccessrecordsCount("AccessAt > DATE_SUB(NOW(),INTERVAL 7 DAY) and ToCustomerId=" + CustomerId + " and Type='ForwardCard'");//被转发
                        int ToCollectionCardCount = cBO.FindAccessrecordsCount("AccessAt > DATE_SUB(NOW(),INTERVAL 7 DAY) and CustomerId=" + CustomerId + " and Type='CollectionCard'");//收藏
                        int CollectionCardCount = cBO.FindAccessrecordsCount("AccessAt > DATE_SUB(NOW(),INTERVAL 7 DAY) and ToCustomerId=" + CustomerId + " and Type='CollectionCard'");//被收藏
                        int ReadCardCount = cBO.FindAccessrecordsCount("AccessAt > DATE_SUB(NOW(),INTERVAL 7 DAY) and ToCustomerId=" + CustomerId + " and Type='ReadCard'");//被浏览
                        int DepositInPhoneCount = cBO.FindAccessrecordsCount("AccessAt > DATE_SUB(NOW(),INTERVAL 7 DAY) and ToCustomerId=" + CustomerId + " and Type='DepositInPhone'");//被存入手机
                        int CallCardCount = cBO.FindAccessrecordsCount("AccessAt > DATE_SUB(NOW(),INTERVAL 7 DAY) and ToCustomerId=" + CustomerId + " and Type='CallCard'");//被拨打手机
                        int ReleasePartyCount = cBO.FindPartyTotalCount("CreatedAt > DATE_SUB(NOW(),INTERVAL 7 DAY) and CustomerId=" + CustomerId);//发布活动
                        int ReadPartyCount = cBO.FindAccessrecordsCount("AccessAt > DATE_SUB(NOW(),INTERVAL 7 DAY) and ToCustomerId=" + CustomerId + " and Type='ReadParty'");//被浏览活动
                        int ForwardPartyCount = cBO.FindAccessrecordsCount("AccessAt > DATE_SUB(NOW(),INTERVAL 7 DAY) and ToCustomerId=" + CustomerId + " and Type='ForwardParty'");//被转发活动
                        int ParticipatePartyCount = cBO.FindCardPartSignInNumTotalCount("CreatedAt > DATE_SUB(NOW(),INTERVAL 7 DAY) and CustomerId=" + CustomerId);//参加活动次数
                        decimal IncomeSum = cBO.Find7DayBalanceByCustomerId(CustomerId);//收入
                        int ActivitylevelCount = uBO.FindLoginHistoryTotalCount("CustomerId=" + CustomerId + " and LoginAt > DATE_SUB(NOW(),INTERVAL 7 DAY)");//活跃度

                        string WeeklyReport = "过去一周数据报告：\n";

                        if (ReadCardCount > 0)
                        {
                            WeeklyReport += "您的名片被浏览了" + ReadCardCount + "次\n";
                        }
                        if (ForwardCardCount > 0)
                        {
                            WeeklyReport += "您的名片共被转发了" + ForwardCardCount + "次\n";
                        }
                        if (ToCollectionCardCount > 0)
                        {
                            WeeklyReport += "您收藏了" + ToCollectionCardCount + "张名片\n";
                        }
                        if (CollectionCardCount > 0)
                        {
                            WeeklyReport += "有" + CollectionCardCount + "人收藏了您的名片\n";
                        }
                        if (DepositInPhoneCount > 0)
                        {
                            WeeklyReport += "有" + DepositInPhoneCount + "人保存了您的手机号\n";
                        }
                        if (CallCardCount > 0)
                        {
                            WeeklyReport += "有" + CallCardCount + "人通过乐聊名片拨打了您的手机\n";
                        }
                        if (ReleasePartyCount > 0)
                        {
                            WeeklyReport += "过去一周发布了" + ReleasePartyCount + "个活动\n";
                        }
                        if (ReadPartyCount > 0)
                        {
                            WeeklyReport += "您的活动被浏览了" + ReadPartyCount + "次\n";
                        }
                        if (ForwardPartyCount > 0)
                        {
                            WeeklyReport += "您的活动被转发了" + ForwardPartyCount + "次\n";
                        }
                        if (ParticipatePartyCount > 0)
                        {
                            WeeklyReport += "您总共参加了" + ParticipatePartyCount + "个活动\n";
                        }
                        if (IncomeSum > 0)
                        {
                            WeeklyReport += "本周总收入" + IncomeSum + "元\n";
                        }
                        WeeklyReport += "本周活跃度" + ActivitylevelCount;

                        cBO.AddCardMessage(WeeklyReport, CustomerId, "每周报告", "");
                    }
                    catch
                    {

                    }
                }
                return new ResultObject() { Flag = 1, Message = "发送成功!", Result = null };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "发送失败!", Result = null };
            }
        }

        /// <summary>
        /// 发送每日报告
        /// </summary>
        /// <returns></returns>
        [Route("sendEverydayReport"), HttpGet, Anonymous]
        public ResultObject sendEverydayReport(int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            try
            {
                //获取最近一周登录的VIP
                List<CustomerRecentLoginViewVO> LoginList = uBO.GetCustomerRecentLoginView("LoginAt > DATE_SUB(NOW(),INTERVAL 7 DAY) and isVip=1");

                for (int i = 0; i < LoginList.Count; i++)
                {
                    try
                    {
                        int CustomerId = LoginList[i].CustomerId;
                        List<CustomerVO> cVO = uBO.FindListByParams("originCustomerId=" + CustomerId + " and " + "DATEDIFF(CreatedAt,NOW())=-1");//新用户数量
                        int NewCustomerCardNum = 0;//新用户创建名片数量
                        for (int j = 0; j < cVO.Count; j++)
                        {
                            List<CardDataVO> cardDataVO = cBO.FindCardByCustomerId(cVO[j].CustomerId);
                            if (cardDataVO.Count > 0)
                            {
                                NewCustomerCardNum++;
                            }
                        }

                        List<CustomerVO> customerVOList = uBO.FindListByParams("originCustomerId=" + CustomerId);//一级用户数量
                        int CustomerCount2 = 0;

                        for (int j = 0; j < customerVOList.Count; j++)
                        {
                            CustomerCount2 += uBO.FindCustomerCount("originCustomerId=" + customerVOList[j].CustomerId);
                        }


                        string EverydayReport = "昨日数据报告：\n";

                        EverydayReport += "昨天通过我的分享，拉了" + cVO.Count + "个新用户，其中" + NewCustomerCardNum + "人创建了名片\n";
                        EverydayReport += "累计拉新" + customerVOList.Count + "人（一级）；累计间接拉新" + CustomerCount2 + "人（二级）";

                        cBO.AddCardMessage(EverydayReport, CustomerId, "每日报告", "");
                    }
                    catch
                    {

                    }
                }
                return new ResultObject() { Flag = 1, Message = "发送成功!", Result = null };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "发送失败!", Result = null };
            }
        }

        /// <summary>
        /// 回收试用兑换券
        /// </summary>
        /// <returns></returns>
        [Route("sendExchangeCode"), HttpGet, Anonymous]
        public ResultObject sendExchangeCode()
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            try
            {
                //获取使用试用兑换券在7天后没有续费的会员，消除兑换记录，兑换码可重新使用
                List<CustomerVO> CustomerVO = uBO.FindListByParams("Agent=1");
                for (int i = 0; i < CustomerVO.Count; i++)
                {
                    try
                    {
                        cBO.GetCardExchangeCodeByCustomerId(CustomerVO[i].CustomerId);
                    }
                    catch
                    {

                    }
                }

                return new ResultObject() { Flag = 1, Message = "发送成功!", Result = null };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "发送失败!", Result = null };
            }
        }

        /// <summary>
        /// 发送点击企业名片通知
        /// </summary>
        /// <returns></returns>
        [Route("sendBrowseNotification"), HttpGet]
        public ResultObject sendBrowseNotification(int sendCustomerId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            if (CustomerVO2.AppType == 3)
            {
                return new ResultObject() { Flag = 0, Message = "发送失败!", Result = null };
            }
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            try
            {
                CustomerVO cVO = uBO.FindCustomenById(customerId);
                cBO.AddCardMessage(cVO.CustomerName + "十分想了解您的企业，但是您尚未开通企业名片，马上了解", sendCustomerId, "浏览通知", "/pages/index/CardConsult/CardConsult");

                return new ResultObject() { Flag = 1, Message = "发送成功!", Result = null };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "发送失败!", Result = null };
            }
        }

        /// <summary>
        /// 发送话题提醒
        /// </summary>
        /// <returns></returns>
        [Route("sendCardNewsMessage"), HttpGet, Anonymous]
        public ResultObject sendCardNewsMessage(int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            try
            {

                List<CardNewsVO> cnVO = cBO.FindCardNewsList();

                if (cnVO.Count > 0)
                {
                    /*
                    if (cnVO[0].isSend > 0)
                    {
                        return new ResultObject() { Flag = 0, Message = "该文章已推送过了!", Result = null };
                    }
                    */
                    List<CardFormListViewVO> cVO = cBO.FindFormListView();
                    for (int i = 0; i < cVO.Count; i++)
                    {
                        try
                        {
                            cBO.sendCardNewsMessage(cVO[i].FormListID);
                        }
                        catch
                        {

                        }
                    }
                    cnVO[0].isSend = 1;
                    cBO.UpdateNews(cnVO[0]);
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "请先设置展示的文章!", Result = null };
                }
                return new ResultObject() { Flag = 1, Message = "发送成功!", Result = null };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "发送失败!", Result = null };
            }

        }
        /// <summary>
        /// 接口测试
        /// </summary>
        /// <returns></returns>
        [Route("sendADMessage"), HttpGet, Anonymous]
        public ResultObject sendADMessage(string CLICK_ID)
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wx67216e7509d25ecc&secret=a60d0b6b0987e80b95f4bc1110ea26a3";
            string jsonStr = HttpHelper.HtmlFromUrlGet(url);
            var result = new WeiXinAccessTokenResultDYH();
            if (jsonStr.Contains("errcode"))
            {
                var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                result.ErrorResult = errorResult;
                result.Result = false;
            }
            else
            {
                var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                result.SuccessResult = model;
                result.Result = true;
            }
            string DataJson = string.Empty;
            string wxaurl = "https://api.weixin.qq.com/marketing/user_actions/add?version=v1.0&access_token=" + result.SuccessResult.access_token;

            DataJson += "{";
            DataJson += "\"type\": \"WEB\",";
            DataJson += "\"name\": \"创建名片\",";
            DataJson += "\"description\": \"用户创建名片时回传\"";
            DataJson += "}";

            DataJson += "{";
            DataJson += "\"actions\":[";
            DataJson += "{";
            DataJson += "\"user_action_set_id\":\"1108132765\",";
            DataJson += "\"url\":\"pages/index/index\",";
            DataJson += "\"action_type\":\"COMPLETE_ORDER\",";
            DataJson += "\"trace\":{";
            DataJson += "\"click_id\":\"" + CLICK_ID + "\"";
            DataJson += "},";
            DataJson += "\"action_param\":{";
            DataJson += "\"value\": 1";
            DataJson += "}";
            DataJson += "}";
            DataJson += "]";
            DataJson += "}";
            string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);

            return new ResultObject() { Flag = 1, Message = "发送成功!", Result = str };

        }

        /// <summary>
        /// 下载小程序直播回放
        /// </summary>
        /// <returns></returns>
        [Route("getliveinfo"), HttpGet, Anonymous]
        public ResultObject getliveinfo(int room_id, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + cBO.appid + "&secret=" + cBO.secret;
            string jsonStr = HttpHelper.HtmlFromUrlGet(url);
            var result = new WeiXinAccessTokenResultDYH();
            if (jsonStr.Contains("errcode"))
            {
                var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                result.ErrorResult = errorResult;
                result.Result = false;
            }
            else
            {
                var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                result.SuccessResult = model;
                result.Result = true;
            }
            string DataJson = string.Empty;
            string wxaurl = "https://api.weixin.qq.com/wxa/business/getliveinfo?access_token=" + result.SuccessResult.access_token;

            DataJson += "{";
            DataJson += "\"action\": \"get_replay\",";
            DataJson += "\"room_id\":" + room_id + ",";
            DataJson += "\"start\": 0,";
            DataJson += "\"limit\": 10";
            DataJson += "}";
            string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);


            dynamic resultContent = JsonConvert.DeserializeObject<getliveinfoVO>(str);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = resultContent };

        }



        /// <summary>
        /// 添加或更新用户红包广告
        /// </summary>
        /// <param name="CardRedPacketVO">VO</param>
        /// <param name="txtRPOneCost">最低金额</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddCardRedPacket"), HttpPost]
        public ResultObject AddCardRedPacket([FromBody] CardRedPacketVO CardRedPacketVO, double txtRPOneCost, string token)
        {
            try
            {
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                int userId = uProfile.UserId;

                if (userId <= 0) { return new ResultObject() { Flag = 0, Message = "改用户没有权限!", Result = null }; }



                Decimal cost = Convert.ToDecimal(CardRedPacketVO.RPCost);
                int num = Convert.ToInt32(CardRedPacketVO.RPNum);



                if (CardRedPacketVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }


                CardBO cBO = new CardBO(new CustomerProfile());


                if (cost <= 0) { return new ResultObject() { Flag = 0, Message = "红包金额不正确!", Result = null }; }
                if (num <= 0) { return new ResultObject() { Flag = 0, Message = "红包个数必须大于0!", Result = null }; }
                if (txtRPOneCost <= 0) { return new ResultObject() { Flag = 0, Message = "单个红包金额必须大于0!", Result = null }; }
                if ((double)cost < num * txtRPOneCost) { return new ResultObject() { Flag = 0, Message = "最低金额不能大于红包的平均金额", Result = null }; }

                if (CardRedPacketVO.RedPacketId > 0)
                {

                    if (cBO.UpdateCardRedPacket(CardRedPacketVO))
                    {
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardRedPacketVO.RedPacketId };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                    }
                }
                else
                {

                    CardRedPacketVO.CustomerId = 0;
                    CardRedPacketVO.RPResidueNum = num;
                    CardRedPacketVO.RPResidueCost = cost;
                    CardRedPacketVO.RPNum = num;
                    CardRedPacketVO.RPContent = CardRedPacketVO.RPContent;
                    CardRedPacketVO.isEqually = 0;
                    CardRedPacketVO.RPCost = cost;
                    CardRedPacketVO.RPCreateDate = DateTime.Now;
                    CardRedPacketVO.Status = 1;//1进行中 0还没开始 2已发完
                    CardRedPacketVO.RPType = 1;//1用户广告 0官方广告


                    int RedPacketID = cBO.AddRedPacket(CardRedPacketVO);
                    if (RedPacketID > 0)
                    {
                        CardRedPacketListVO CardRedPacketListVO = new CardRedPacketListVO();
                        CardRedPacketListVO.RedPacketId = RedPacketID;

                        Boolean suc = cBO.RandomAllotRedPackets(CardRedPacketVO.RPResidueNum, CardRedPacketVO.RPCost, txtRPOneCost, CardRedPacketListVO);


                        if (!suc) { return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null }; }

                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = RedPacketID };
                    }
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "异常", Result = ex };
            }
        }


        /// <summary>
        /// 获取微信支付数据 用户发红包支付
        /// </summary>
        /// <param name="CardRedPacketVO">红包数据</param>
        /// <param name="code">code</param>
        /// <param name="txtRPOneCost">单个红包最低金额</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("GetUnifiedRedPacketOrderResult"), HttpPost]
        public ResultObject GetUnifiedRedPacketOrderResult([FromBody] CardRedPacketVO CardRedPacketVO, String code, double txtRPOneCost, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            string appid = "wx584477316879d7e9";
            CardBO cBO = new CardBO(new CustomerProfile());
            String OpenId = cBO.getOpenId(code);

            Random ran = new Random();
            String OrderNO = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);

            CardRedPacketVO.CustomerId = customerId;
            CardRedPacketVO.RPResidueNum = CardRedPacketVO.RPNum;

            double RPCost = Convert.ToDouble(CardRedPacketVO.RPCost);
            double ServiceCharge = Math.Round(RPCost * 0.006, 2);
            CardRedPacketVO.ServiceCharge = Convert.ToDecimal(ServiceCharge);
            CardRedPacketVO.RPResidueCost = CardRedPacketVO.RPCost - CardRedPacketVO.ServiceCharge;
            CardRedPacketVO.isEqually = 0;
            CardRedPacketVO.RPCreateDate = DateTime.Now;
            CardRedPacketVO.Status = 0;//1进行中 0还没开始 2已发完
            CardRedPacketVO.RPType = 1;//1用户广告 0官方广告
            CardRedPacketVO.OrderNO = OrderNO;
            CardRedPacketVO.OpenId = OpenId;

            if (CardRedPacketVO.CustomerId != customerId) { return new ResultObject() { Flag = 0, Message = "权限不足!", Result = null }; }
            if (CardRedPacketVO == null) { return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null }; }
            if (CardRedPacketVO.RPResidueCost <= 0) { return new ResultObject() { Flag = 0, Message = "红包金额不正确!", Result = null }; }
            if (CardRedPacketVO.RPNum <= 0) { return new ResultObject() { Flag = 0, Message = "红包个数必须大于0!", Result = null }; }
            if (txtRPOneCost <= 0) { return new ResultObject() { Flag = 0, Message = "单个红包金额必须大于0!", Result = null }; }
            if ((double)CardRedPacketVO.RPResidueCost < CardRedPacketVO.RPNum * txtRPOneCost) { return new ResultObject() { Flag = 0, Message = "最低金额不能大于红包的平均金额", Result = null }; }

            int RedPacketID = cBO.AddRedPacket(CardRedPacketVO);

            if (RedPacketID <= 0) { return new ResultObject() { Flag = 0, Message = "红包添加失败", Result = null }; }

            CardRedPacketListVO CardRedPacketListVO = new CardRedPacketListVO();
            CardRedPacketListVO.RedPacketId = RedPacketID;

            Boolean suc = cBO.RandomAllotRedPackets(CardRedPacketVO.RPResidueNum, CardRedPacketVO.RPResidueCost, txtRPOneCost, CardRedPacketListVO);

            if (!suc) { return new ResultObject() { Flag = 0, Message = "红包分配失败", Result = null }; }

            JsApiPay Ja = new JsApiPay();
            string total_fee_1 = Convert.ToInt32((CardRedPacketVO.RPCost * 100)).ToString();
            string NOTIFY_URL = "http://api.leliaomp.com/Pay/CardRedPacket_Url.aspx";

            String RPContent = Regex.Replace(CardRedPacketVO.RPContent, @"\p{Cs}", "");

            WxPayData wp = Ja.GetUnifiedOrderResult(appid, OrderNO, OpenId, total_fee_1, "任务红包支付-" + CardRedPacketVO.RedPacketId, RPContent, "任务红包支付", NOTIFY_URL);

            if (wp != null)
            {
                string reslut = Ja.GetJsApiParameters(wp);
                if (reslut != "")
                {
                    return new ResultObject() { Flag = 1, Message = "成功", Result = reslut };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "失败", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "失败", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新官方红包广告
        /// </summary>
        /// <param name="CardRedPacketVO">VO</param>
        /// <param name="token">口令</param>
        /// <param name="txtRPOneCost">红包最低金额</param>
        /// <returns></returns>
        [Route("AddOfficialCardRedPacket"), HttpPost]
        public ResultObject AddOfficialCardRedPacket([FromBody] CardRedPacketVO CardRedPacketVO, double txtRPOneCost, string token)
        {
            try
            {
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                int userId = uProfile.UserId;

                if (userId <= 0) { return new ResultObject() { Flag = 0, Message = "改用户没有权限!", Result = null }; }
                Decimal cost = Convert.ToDecimal(CardRedPacketVO.RPCost);
                int num = Convert.ToInt32(CardRedPacketVO.RPNum);
                if (CardRedPacketVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
                CardBO cBO = new CardBO(new CustomerProfile());
                if (cost <= 0 || cost > 5000) { return new ResultObject() { Flag = 0, Message = "红包金额不正确或超出限制!", Result = null }; }
                if (num <= 0) { return new ResultObject() { Flag = 0, Message = "红包个数必须大于0!", Result = null }; }
                if (txtRPOneCost < 0.3) { return new ResultObject() { Flag = 0, Message = "单个红包金额必须大于0.3!", Result = null }; }
                if ((double)cost < num * txtRPOneCost) { return new ResultObject() { Flag = 0, Message = "最低金额不能大于红包的平均金额", Result = null }; }

                if (CardRedPacketVO.RedPacketId > 0)
                {

                    if (cBO.UpdateCardRedPacket(CardRedPacketVO))
                    {
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardRedPacketVO.RedPacketId };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                    }
                }
                else
                {

                    CardRedPacketVO.CustomerId = 0;
                    CardRedPacketVO.RPResidueNum = num;
                    CardRedPacketVO.RPResidueCost = cost;
                    CardRedPacketVO.RPNum = num;
                    CardRedPacketVO.RPContent = CardRedPacketVO.RPContent;
                    CardRedPacketVO.isEqually = 0;
                    CardRedPacketVO.RPCost = cost;
                    CardRedPacketVO.RPCreateDate = DateTime.Now;
                    CardRedPacketVO.Status = 0;//1进行中 0还没开始 2已发完
                    CardRedPacketVO.RPType = 0;//1用户广告 0官方广告


                    int RedPacketID = cBO.AddRedPacket(CardRedPacketVO);
                    if (RedPacketID > 0)
                    {
                        CardRedPacketListVO CardRedPacketListVO = new CardRedPacketListVO();
                        CardRedPacketListVO.RedPacketId = RedPacketID;

                        Boolean suc = cBO.RandomAllotRedPackets(CardRedPacketVO.RPResidueNum, CardRedPacketVO.RPCost, txtRPOneCost, CardRedPacketListVO);


                        if (!suc) { return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null }; }

                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = RedPacketID };
                    }
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "异常", Result = ex };
            }
        }


        /// <summary>
        /// 开启红包广告
        /// </summary>
        /// <param name="RedPacketId">要开启的红包id</param>
        /// <param name="status">0禁用 1开启</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("TurnOffCardRedPacket"), HttpGet]
        public ResultObject TurnOffCardRedPacket(int RedPacketId, int status, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            int UserId = uProfile.UserId;
            if (UserId <= 0) { return new ResultObject() { Flag = 0, Message = "用户没有权限！", Result = null }; }


            CardBO cBO = new CardBO(new CustomerProfile());

            CardRedPacketVO crpVo = new CardRedPacketVO();
            cBO.UpdateRedPacketStatus();
            crpVo.RedPacketId = RedPacketId;
            crpVo.Status = status;
            bool ret = cBO.UpdateCardRedPacket(crpVo);

            if (!ret) { return new ResultObject() { Flag = 0, Message = "更新失败！", Result = null }; }
            return new ResultObject() { Flag = 1, Message = "更新成功！", Result = null };
        }


        /// <summary>
        /// 删除红包广告
        /// </summary>
        /// <param name="RedPacketId">要开启的红包id</param>
        /// <param name="status">0禁用 1开启</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelCardRedPacket"), HttpGet]
        public ResultObject DelCardRedPacket(int RedPacketId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            int UserId = uProfile.UserId;
            if (UserId <= 0) { return new ResultObject() { Flag = 0, Message = "用户没有权限！", Result = null }; }


            CardBO cBO = new CardBO(new CustomerProfile());
            CardRedPacketVO cardredVO = cBO.FindCardRedPacketById(RedPacketId);

            int ret = -1;
            if (cardredVO.RPNum == cardredVO.RPResidueNum && cardredVO.RPCost == cardredVO.RPResidueCost)
            {
                ret = cBO.DeleteCardRedPacketById(RedPacketId);
            }

            if (ret == -1) { return new ResultObject() { Flag = 0, Message = "删除失败，改红包已经有人领过，不能删除！！", Result = null }; }
            return new ResultObject() { Flag = 1, Message = "删除成功！", Result = null };
        }



        /// <summary>
        /// 用户领取红包 
        /// </summary>
        /// <param name="CustomerId">CustomerId</param>
        ///<param name="CardRedPacketId">红包Id</param>
        ///<param name="Code">code</param>
        ///<param name="formId">模板id</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ReceiveRedPacket"), HttpPost]
        public ResultObject ReceiveRedPacket(int CustomerId, int CardRedPacketId, string Code, string formId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CardBO cBO = new CardBO(new CustomerProfile());
            CustomerBO customerBO = new CustomerBO(new CustomerProfile());
            int customerId_token = uProfile.CustomerId;


            if (CustomerId <= 0 || CardRedPacketId <= 0 || Code == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            if (customerId_token != CustomerId) { return new ResultObject() { Flag = 0, Message = "用户数据错误!", Result = null }; }

            string openid = cBO.getOpenId(Code);
            CustomerViewVO CustomerViewVO = customerBO.FindById(CustomerId);
            CardRedPacketVO crpVo = cBO.FindCardRedPacketById(CardRedPacketId);
            List<CardDataVO> cardDataVo = cBO.FindCardByCustomerId(CustomerId);

            CardRedPacketVO crpVO = cBO.FindCardRedPacketById(CardRedPacketId);
            if (crpVO.Status == 0) { return new ResultObject() { Flag = 2, Message = "广告还没有开始!", Result = null }; }
            if (crpVO.Status == 2) { return new ResultObject() { Flag = 2, Message = "红包已经领完了", Result = null }; }


            List<CardRedPacketListVO> cardredlist2 = cBO.GetCardRedPacketListByOid(openid);

            if (cardredlist2.Count > 1) { return new ResultObject() { Flag = 4, Message = "每个新用户最多可以领取两个红包！", Result = null }; }


            //判断用户是否为新用户 从红包创建时间开始算 之后注册的会员的则是新用户
            if (DateTime.Compare(crpVo.RPCreateDate, CustomerViewVO.CreatedAt) > 0) { return new ResultObject() { Flag = 3, Message = "您不是新用户，不满足领取条件!", Result = cardredlist2 }; }

            if (cardDataVo.Count <= 0) { return new ResultObject() { Flag = 5, Message = "您不满足领取条件,请先创建您的电子名片!", Result = null }; }




            bool temp = false;

            if (cardDataVo[0].Name == "" || cardDataVo[0].Phone == "" || cardDataVo[0].Position == "" || cardDataVo[0].CorporateName == "") { return new ResultObject() { Flag = 3, Message = "您不满足领取条件,请填写完整信息的名片!", Result = null }; }

            if (cardredlist2.Count == 1)
            {
                for (int i = 0; i < cardDataVo.Count; i++)
                {
                    if (cardDataVo[i].ReadCount >= 10)
                    {
                        temp = true;
                        break;
                    }
                }
                if (!temp) { return new ResultObject() { Flag = 6, Message = "您不满足领取条件,任意一张名片浏览量必须达到10以上!", Result = null }; }
            }


            List<CardRedPacketListVO> cardredlist = cBO.GetCardRedPacketListByRPId(CardRedPacketId);
            if (cardredlist.Count == 0) { return new ResultObject() { Flag = 0, Message = "该红包不存在!", Result = null }; }


            int RPListId = 0;
            CardRedPacketListVO crplVO = new CardRedPacketListVO();
            for (int i = 0; i < cardredlist.Count; i++)
            {
                if (cardredlist[i].isReceive == 0)
                {
                    RPListId = cardredlist[i].RPListId;
                    crplVO = cardredlist[i];
                    break;
                }
            }



            //更新红包状态为2 已领完
            if (RPListId == 0)
            {
                crpVO.RedPacketId = CardRedPacketId;
                crpVO.Status = 2;
                cBO.UpdateCardRedPacket(crpVO);
                return new ResultObject() { Flag = 2, Message = "红包已经被领完了,下次早点来!", Result = null };
            }


            crplVO.SetValue("RPListId", RPListId);
            crplVO.SetValue("CustomerId", CustomerId);
            crplVO.SetValue("ReceiveDate", DateTime.Now);
            crplVO.SetValue("isReceive", 1);//0未领取 1已领取 2已领取红包未发送到用户零钱
            crplVO.SetValue("OpenId", openid);
            crplVO.SetValue("FormId", formId);



            bool ret = cBO.UpdateCardRedPacketList(crplVO);


            if (!ret) { return new ResultObject() { Flag = 0, Message = "领取失败!", Result = null }; }

            crpVO.RedPacketId = CardRedPacketId;
            crpVO.RPResidueCost = crpVO.RPResidueCost - crplVO.RPOneCost;
            crpVO.RPResidueNum = crpVO.RPResidueNum - 1;
            cBO.UpdateCardRedPacket(crpVO);//更新红包信息



            for (int i = 0; i < cardredlist.Count; i++)
            {
                if (cardredlist[i].isReceive == 2)//循环发送领完没有发送零钱到微信帐户的
                {
                    String resu = cBO.PayforWXUser(cardredlist[i].RPOneCost, cardredlist[i].RPListId, cardredlist[i].OpenId);
                    if (resu == "SUCCESS")
                    {
                        CardRedPacketListVO crplVO1 = new CardRedPacketListVO();
                        crplVO1.SetValue("RPListId", cardredlist[i].RPListId);
                        crplVO1.SetValue("isReceive", 1);//0未领取 1已领取 2已领取红包未发送到用户零钱
                        cBO.UpdateCardRedPacketList(crplVO1);
                        cBO.sendMoneyToPacket(cardredlist[i].OpenId, cardredlist[i].FormId, cardredlist[i].RPOneCost, "乐聊名片现金红包已存入零钱，分享名片可领更多现金红包哦"); //发送零钱到账通知
                    }
                }
            }

            String resultData = cBO.PayforWXUser(crplVO.RPOneCost, RPListId, openid);//直接发送奖励到零钱
            if (resultData == "FAIL")
            {
                CardRedPacketListVO crplVO2 = new CardRedPacketListVO();
                crplVO2.SetValue("RPListId", RPListId);
                crplVO2.SetValue("isReceive", 2);//0未领取 1已领取 2已领取红包未发送到用户零钱
                cBO.UpdateCardRedPacketList(crplVO2);
                return new ResultObject() { Flag = 1, Message = "领取成功，红包可能会延迟发送!", Result = crplVO };
            }


            cBO.sendMoneyToPacket(openid, formId, crplVO.RPOneCost, "乐聊名片现金红包已存入零钱，分享名片可领更多现金红包哦"); //发送零钱到账通知

            return new ResultObject() { Flag = 1, Message = "领取成功!", Result = crplVO };


        }
        /// <summary>
        /// 获取红包列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("getCardPacketListByCRPid"), HttpPost, Anonymous]
        public ResultObject getCardPacketListByCRPid([FromBody] ConditionModel condition)
        {
            try
            {
                CardBO cBO = new CardBO(new CustomerProfile());
                if (condition == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
                else if (condition.Filter == null || condition.PageInfo == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
                string conditionStr = condition.Filter.Result();

                Paging pageInfo = condition.PageInfo;
                List<CardRedPacketListViewVO> list = cBO.FindCardRedPacketListViewAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "服务器异常!", Result = ex };
            }


        }


        /// <summary>
        /// 领取红包后点赞
        /// </summary>
        /// <param name="LikeType">LikeType</param>
        /// <param name="RedPacketId">RedPacketId</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        [Route("updateCardPacketLikeType"), HttpPost]
        public ResultObject updateCardPacketLikeType(int LikeType, int RedPacketId, string token)
        {
            try
            {

                if (RedPacketId <= 0 || LikeType < 0) { return new ResultObject() { Flag = 0, Message = "参数错误", Result = null }; }

                CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
                CardBO cBO = new CardBO(new CustomerProfile());
                CustomerBO customerBO = new CustomerBO(new CustomerProfile());
                int customerId_token = uProfile.CustomerId;
                List<CardRedPacketListVO> list = cBO.GetCardRedPacketListByRPIdAndCid(RedPacketId, customerId_token);
                CardRedPacketListVO vo = new CardRedPacketListVO();
                vo.LikeType = LikeType;
                bool ret = false;
                for (int i = 0; i < list.Count; i++)
                {
                    vo.RPListId = list[i].RPListId;
                    ret = cBO.UpdateCardRedPacketList(vo);
                }
                if (!ret) { return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null }; }
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "服务器异常!", Result = ex };
            }


        }


        /// <summary>
        /// 查询用户已经完成哪一个 哪一个没完成
        /// </summary>
        /// <returns></returns>
        [Route("SelectIsRecCardRedPacket"), HttpGet, Anonymous]
        public ResultObject SelectIsRecCardRedPacket(int CustomerId)
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            CustomerBO customerBO = new CustomerBO(new CustomerProfile());
            try
            {
                if (CustomerId > 0)
                {
                    CustomerViewVO CustomerViewVO = customerBO.FindById(CustomerId);
                    List<CardDataVO> cdv = cBO.FindCardByCustomerId(CustomerId);

                    if (cdv[0].Name == "" || cdv[0].Phone == "" || cdv[0].Position == "" || cdv[0].CorporateName == "") { return new ResultObject() { Flag = 2, Message = "您不满足领取条件,请填写完整信息的名片!", Result = null }; }
                    bool temp = false;
                    for (int i = 0; i < cdv.Count; i++)
                    {
                        if (cdv[i].ReadCount >= 10)
                        {
                            temp = true;
                            break;
                        }
                    }
                    if (!temp) { return new ResultObject() { Flag = 3, Message = "任意一张名片浏览量必须大于10！", Result = null }; }

                    return new ResultObject() { Flag = 1, Message = "获取成功！", Result = null };

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败！", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "服务器异常！", Result = null };
            }
        }



        /// <summary>
        /// 获取红包广告
        /// </summary>
        /// <returns></returns>
        [Route("getCardPacketOneByStatus"), HttpGet, Anonymous]
        public ResultObject getCardPacketOneByStatus(int CustomerId)
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            CustomerBO customerBO = new CustomerBO(new CustomerProfile());
            try
            {

                List<CardRedPacketVO> cardRedPacketList = cBO.getOneCardRedByStatysAndRpType();
                if (cardRedPacketList.Count > 0)
                {
                    if (CustomerId > 0)
                    {
                        CustomerViewVO CustomerViewVO = customerBO.FindById(CustomerId);
                        if (DateTime.Compare(cardRedPacketList[0].RPCreateDate, CustomerViewVO.CreatedAt) <= 0)
                        {
                            List<CardRedPacketListVO> cardredlist2 = cBO.GetCardRedPacketListByRPIdAndCid(cardRedPacketList[0].RedPacketId, CustomerId);
                            if (cardredlist2.Count == 2) { return new ResultObject() { Flag = 2, Message = "获取成功，用户已经领完两个红包！", Result = cardRedPacketList[0] }; }
                            return new ResultObject() { Flag = 1, Message = "获取成功！", Result = cardRedPacketList[0] };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 2, Message = "获取成功，该用户不是新用户！", Result = cardRedPacketList[0] };
                        }
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "获取成功！", Result = cardRedPacketList[0] };
                    }
                }
                return new ResultObject() { Flag = 0, Message = "获取失败！", Result = null };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "服务器异常！", Result = null };
            }
        }


        /// <summary>
        /// 用户获取我的红包列表
        /// </summary>
        /// <returns></returns>
        [Route("getCardPacketList"), HttpGet]
        public ResultObject getCardPacketList(string token, string type, int id)
        {
            try
            {
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                string sql = "";
                if (type == "Card")
                {
                    sql = " and CardID=" + id;
                }
                if (type == "Party")
                {
                    sql = " and PartyID=" + id;
                }

                CardBO cBO = new CardBO(new CustomerProfile());
                List<CardRedPacketViewVO> list = cBO.FindCardRedPacketViewAllByPageIndex("CustomerId = " + customerId + sql + " and (Status=1 or Status=2)");

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "服务器异常!", Result = ex };
            }
        }

        /// <summary>
        /// 用户获取我的红包列表
        /// </summary>
        /// <returns></returns>
        [Route("getCardPacketList"), HttpGet, Anonymous]
        public ResultObject getCardPacketList(string type, int id)
        {
            try
            {
                string sql = "";
                if (type == "Card")
                {
                    sql = "CardID=" + id;
                }
                if (type == "Party")
                {
                    sql = "PartyID=" + id;
                }

                CardBO cBO = new CardBO(new CustomerProfile());
                List<CardRedPacketViewVO> list = cBO.FindCardRedPacketViewAllByPageIndex(sql + " and (Status=1 or Status=2)");

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "服务器异常!", Result = ex };
            }
        }

        /// <summary>
        /// 用户获取红包详情
        /// </summary>
        /// <returns></returns>
        [Route("getCardPacketView"), HttpGet, Anonymous]
        public ResultObject getCardPacketView(int RedPacketId)
        {
            try
            {
                CardBO cBO = new CardBO(new CustomerProfile());
                CardRedPacketViewVO cVO = cBO.FindCardRedPacketViewById(RedPacketId);
                List<CardRedPacketListViewVO> crplViewVO = cBO.FindCardRedPacketListViewAllByPageIndex2("RedPacketId=" + RedPacketId + " and isReceive=1");
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { RedPacket = cVO, PacketList = crplViewVO } };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "服务器异常!", Result = ex };
            }
        }

        /// <summary>
        /// 用户领取红包(非官方红包) 
        /// </summary>
        ///<param name="CardRedPacketId">红包Id</param>
        ///<param name="Code">code</param>
        ///<param name="formId">模板id</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ReceiveRedPacketbyUser"), HttpPost]
        public ResultObject ReceiveRedPacketbyUser(int CardRedPacketId, string Code, string formId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CardBO cBO = new CardBO(new CustomerProfile());
            CustomerBO customerBO = new CustomerBO(new CustomerProfile());
            int CustomerId = uProfile.CustomerId;


            if (CustomerId <= 0 || CardRedPacketId <= 0 || Code == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string openid = cBO.getOpenId(Code);
            CustomerViewVO CustomerViewVO = customerBO.FindById(CustomerId);
            CardRedPacketVO crpVo = cBO.FindCardRedPacketById(CardRedPacketId);
            List<CardDataVO> cardDataVo = cBO.FindCardByCustomerId(CustomerId);

            CardRedPacketVO crpVO = cBO.FindCardRedPacketById(CardRedPacketId);
            if (crpVO.Status == 0) { return new ResultObject() { Flag = 0, Message = "广告还没有开始!", Result = null }; }
            if (crpVO.Status == 2) { return new ResultObject() { Flag = 2, Message = "红包已经领完了", Result = null }; }
            if (crpVO.RPType == 0) { return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null }; }

            List<CardRedPacketListVO> cardredlist = cBO.GetCardRedPacketListByRPId(CardRedPacketId);
            if (cardredlist.Count == 0) { return new ResultObject() { Flag = 0, Message = "该红包不存在!", Result = null }; }

            List<CardRedPacketListVO> cardredlist2 = cBO.GetCardRedPacketListByRPIdAndCid(CardRedPacketId, CustomerId);
            if (cardredlist2.Count > 0) { return new ResultObject() { Flag = 4, Message = "你已经领过这个红包了！", Result = null }; }

            int RPListId = 0;
            CardRedPacketListVO crplVO = new CardRedPacketListVO();
            for (int i = 0; i < cardredlist.Count; i++)
            {
                if (cardredlist[i].isReceive == 0)
                {
                    RPListId = cardredlist[i].RPListId;
                    crplVO = cardredlist[i];
                    break;
                }
            }

            //更新红包状态为2 已领完
            if (RPListId == 0)
            {
                crpVO.RedPacketId = CardRedPacketId;
                crpVO.Status = 2;
                cBO.UpdateCardRedPacket(crpVO);
                return new ResultObject() { Flag = 2, Message = "红包已经被领完了,下次早点来!", Result = null };
            }


            crplVO.SetValue("RPListId", RPListId);
            crplVO.SetValue("CustomerId", CustomerId);
            crplVO.SetValue("ReceiveDate", DateTime.Now);
            crplVO.SetValue("isReceive", 1);//0未领取 1已领取 2已领取红包未发送到用户零钱
            crplVO.SetValue("OpenId", openid);
            crplVO.SetValue("FormId", formId);

            bool ret = cBO.UpdateCardRedPacketList(crplVO);


            if (!ret) { return new ResultObject() { Flag = 0, Message = "领取失败!", Result = null }; }

            crpVO.RedPacketId = CardRedPacketId;
            crpVO.RPResidueCost = crpVO.RPResidueCost - crplVO.RPOneCost;
            crpVO.RPResidueNum = crpVO.RPResidueNum - 1;
            cBO.UpdateCardRedPacket(crpVO);//更新红包信息

            for (int i = 0; i < cardredlist.Count; i++)
            {
                if (cardredlist[i].isReceive == 2)//循环发送领完没有发送零钱到微信帐户的
                {
                    String resu = cBO.PayforWXUser(cardredlist[i].RPOneCost, cardredlist[i].RPListId, cardredlist[i].OpenId);
                    if (resu == "SUCCESS")
                    {
                        CardRedPacketListVO crplVO1 = new CardRedPacketListVO();
                        crplVO1.SetValue("RPListId", cardredlist[i].RPListId);
                        crplVO1.SetValue("isReceive", 1);//0未领取 1已领取 2已领取红包未发送到用户零钱
                        cBO.UpdateCardRedPacketList(crplVO1);
                        cBO.sendMoneyToPacket(cardredlist[i].OpenId, cardredlist[i].FormId, cardredlist[i].RPOneCost, "乐聊名片现金红包已存入零钱，分享名片可领更多现金红包哦"); //发送零钱到账通知
                    }
                }
            }

            String resultData = cBO.PayforWXUser(crplVO.RPOneCost, RPListId, openid);//直接发送奖励到零钱

            List<CardRedPacketListViewVO> crplViewVO = cBO.FindCardRedPacketListViewAllByPageIndex2("RedPacketId=" + CardRedPacketId + " and isReceive=1");
            if (resultData == "FAIL")
            {
                CardRedPacketListVO crplVO2 = new CardRedPacketListVO();
                crplVO2.SetValue("RPListId", RPListId);
                crplVO2.SetValue("isReceive", 2);//0未领取 1已领取 2已领取红包未发送到用户零钱
                cBO.UpdateCardRedPacketList(crplVO2);
                return new ResultObject() { Flag = 1, Message = "领取成功，红包可能会延迟发送!", Result = new { Cost = crplVO.RPOneCost, PacketList = crplViewVO } };
            }
            cBO.sendMoneyToPacket(openid, formId, crplVO.RPOneCost, "乐聊名片现金红包已存入零钱，分享名片可领更多现金红包哦"); //发送零钱到账通知
            return new ResultObject() { Flag = 1, Message = "领取成功!", Result = new { Cost = crplVO.RPOneCost, PacketList = crplViewVO } };
        }


        /// <summary>
        /// 用户退回剩下的红包(非官方红包) 
        /// </summary>
        ///<param name="CardRedPacketId">红包Id</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ReturnRedPacketbyUser"), HttpGet]
        public ResultObject ReturnRedPacketbyUser(int CardRedPacketId, string token)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CardBO cBO = new CardBO(new CustomerProfile());
            CustomerBO customerBO = new CustomerBO(new CustomerProfile());
            int CustomerId = uProfile.CustomerId;

            CardRedPacketVO crpVO = cBO.FindCardRedPacketById(CardRedPacketId);
            if (crpVO.Status == 0 || crpVO.payAt == null) { return new ResultObject() { Flag = 0, Message = "该红包未付款!", Result = null }; }
            if (crpVO.CustomerId != CustomerId) { return new ResultObject() { Flag = 0, Message = "这不是你发布的红包，禁止操作!", Result = null }; }

            List<CardRedPacketListVO> cardredlist = cBO.GetCardRedPacketListByRPId(CardRedPacketId);
            if (cardredlist.Count == 0) { return new ResultObject() { Flag = 0, Message = "该红包不存在!", Result = null }; }

            decimal Cost = 0;

            CardRedPacketListVO crplVO = new CardRedPacketListVO();
            for (int i = 0; i < cardredlist.Count; i++)
            {
                if (cardredlist[i].isReceive == 0)
                {
                    crplVO = cardredlist[i];
                    Cost += cardredlist[i].RPOneCost;
                }
            }

            if (Cost <= 0)
            {
                return new ResultObject() { Flag = 2, Message = "红包已经领完了", Result = null };
            }

            String resultData = cBO.PayforWXUser(Cost, crplVO.RPListId, crpVO.OpenId, "退回红包未领余额");//直接发送奖励到零钱
            if (resultData == "FAIL")
            {
                return new ResultObject() { Flag = 0, Message = "平台余额不足，请联系乐聊官方客服!", Result = null };
            }

            for (int i = 0; i < cardredlist.Count; i++)
            {
                if (cardredlist[i].isReceive == 0)
                {
                    try
                    {
                        crplVO = cardredlist[i];
                        crplVO.SetValue("RPListId", cardredlist[i].RPListId);
                        crplVO.SetValue("CustomerId", crpVO.CustomerId);
                        crplVO.SetValue("ReceiveDate", DateTime.Now);
                        crplVO.SetValue("isReceive", 1);//0未领取 1已领取 2已领取红包未发送到用户零钱
                        crplVO.SetValue("OpenId", crpVO.OpenId);

                        bool ret = cBO.UpdateCardRedPacketList(crplVO);
                    }
                    catch
                    {

                    }
                }
            }
            crpVO.RedPacketId = CardRedPacketId;
            crpVO.Status = 2;
            cBO.UpdateCardRedPacket(crpVO);

            return new ResultObject() { Flag = 1, Message = "红包未领余额已退回至您的微信钱包", Result = null };
        }


        /// <summary>
        /// 红包测试余额减掉
        /// </summary>
        /// <returns></returns>
        [Route("TestApi"), HttpGet, Anonymous]
        public ResultObject TestApi()
        {

            CardBO cBO = new CardBO(new CustomerProfile());
            CustomerBO customerBO = new CustomerBO(new CustomerProfile());
            //ReduceCardBalance(320, 10);//减少余额
            //cBO.PlusCardBalance(320, 10);//增加余额
            return new ResultObject() { Flag = 1, Message = "成功!", Result = null };

        }
        //https://www.zhongxiaole.net/SPManager/SPWebAPI/Card/GetQRImg


        /// <summary>
        /// 更新名片QRImg
        /// </summary>
        /// <returns></returns>
        [Route("UpdateQRImg"), HttpGet, Anonymous]
        public ResultObject UpdateQRImg(int CardID, int AppType = 1)
        {

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CustomerBO customerBO = new CustomerBO(new CustomerProfile());
            try
            {
                CardDataVO cardData = new CardDataVO();
                String QRurl = cBO.getQRIMGByIDAndType(CardID, 1);
                cardData.CardID = CardID;
                cardData.QRImg = QRurl;

                bool ret = cBO.Update(cardData);
                if (!ret) { return new ResultObject() { Flag = 0, Message = "更新QRImg失败!", Result = null }; }
                return new ResultObject() { Flag = 1, Message = "更新QRImg成功!", Result = cardData };

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "失败!", Result = ex };
            }


        }




        /// <summary>
        /// 获取名片QRImg
        /// </summary>
        /// <returns></returns>
        [Route("GetQRImg"), HttpGet, Anonymous]
        public ResultObject GetQRImg(int CardID, int AppType = 1)
        {

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CustomerBO customerBO = new CustomerBO(new CustomerProfile());
            try
            {
                CardDataVO cardData = cBO.FindCardById(CardID);
                String QRurl = cardData.QRImg;
                if (cardData.CardImg == "" || cardData.CardImg == null)
                {
                    cardData.CardImg = cBO.GetCardQR(CardID);
                }

                if (QRurl == "" || QRurl == null)
                {
                    QRurl = cBO.getQRIMGByIDAndType(CardID, 1);
                    cardData.QRImg = QRurl;
                }
                cardData.QRImg = cBO.getQRIMGByIDAndType(CardID, 1);
                return new ResultObject() { Flag = 1, Message = "成功!", Result = cardData };

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "失败!", Result = ex };
            }


        }
        /// <summary>
        /// 获取活动QRImg 携带用户Id参数的
        /// </summary>
        /// <returns></returns>
        [Route("GetPartQRImgByMessage"), HttpGet]
        public ResultObject GetPartQRImgByMessage(int PartyID, string token, int AppType = 1)
        {

            try
            {
                CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
                int customerId_token = uProfile.CustomerId;

                CustomerBO customerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = customerBO.FindCustomenById(customerId_token);
                if (AppType == 1)
                {
                    AppType = CustomerVO2.AppType;
                }
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);
                string TempQrurl = cBO.GetCardPartyQRByMessage(PartyID, customerId_token);//获取临时生成带参数小程序二维码地址 返回图片名字xx.png

                string[] temp = TempQrurl.Split('.');

                CardPartyVO cvo = cBO.FindPartyById(PartyID);
                String QRurl = cBO.getPartyQRIMGByIDAndTypeByMesage(PartyID, Convert.ToInt64(temp[0]), customerId_token);


                try
                {//删除旧图片
                    String url = "/UploadFolder/CardPartyQRTemporaryFile/" + QRurl;
                    string FilePath = url;
                    //FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }
                catch
                {

                }

                if (QRurl == null)
                {
                    return new ResultObject() { Flag = 0, Message = "失败!", Result = null };
                }
                return new ResultObject() { Flag = 1, Message = "成功!", Result = QRurl };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "失败!", Result = ex };
            }
        }

        /// <summary>
        /// 获取活动QRImg 携带用户Id参数的
        /// </summary>
        /// <returns></returns>
        [Route("GetPartQRImgByMessage"), HttpGet, Anonymous]
        public ResultObject GetPartQRImgByMessage(int PartyID, Int64 CustomerId)
        {
            try
            {
                Int64 customerId_token = CustomerId;
                CustomerBO customerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = customerBO.FindCustomenById(customerId_token);
                CardBO cBO = new CardBO(new CustomerProfile());
                string TempQrurl = cBO.GetCardPartyQRByMessage(PartyID, customerId_token);//获取临时生成带参数小程序二维码地址 返回图片名字xx.png

                string[] temp = TempQrurl.Split('.');

                CardPartyVO cvo = cBO.FindPartyById(PartyID);
                String QRurl = cBO.getPartyQRIMGByIDAndTypeByMesage(PartyID, Convert.ToInt64(temp[0]), customerId_token);


                try
                {//删除旧图片
                    String url = "/UploadFolder/CardPartyQRTemporaryFile/" + QRurl;
                    string FilePath = url;
                    //FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }
                catch
                {

                }

                if (QRurl == null)
                {
                    return new ResultObject() { Flag = 0, Message = "失败!", Result = null };
                }
                return new ResultObject() { Flag = 1, Message = "成功!", Result = QRurl };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "失败!", Result = ex };
            }
        }

        /// <summary>
        /// 获取活动QRImg
        /// </summary>
        /// <returns></returns>
        [Route("GetPartQRImg"), HttpGet, Anonymous]
        public ResultObject GetPartQRImg(int PartyID, int AppType = 1)
        {

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CustomerBO customerBO = new CustomerBO(new CustomerProfile());
            try
            {
                CardPartyVO cvo = cBO.FindPartyById(PartyID);
                string QRurl = cvo.QRImg;
                if (QRurl == "")
                {
                    QRurl = cBO.getQRIMGByIDAndType(PartyID, 3);
                }
                return new ResultObject() { Flag = 1, Message = "成功!", Result = QRurl };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "失败!", Result = ex };
            }


        }
        /// <summary>
        /// 更新活动QRImg
        /// </summary>
        /// <returns></returns>
        [Route("UpdatePartQRImg"), HttpGet, Anonymous]
        public ResultObject UpdatePartQRImg(int PartyID, int AppType = 1)
        {

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CustomerBO customerBO = new CustomerBO(new CustomerProfile());
            try
            {
                CardPartyVO cardParty = new CardPartyVO();
                CardPartyVO cardParty2 = cBO.FindPartyById(PartyID);

                if (cardParty2.QRCodeImg != "")
                {
                    String QRurl = cBO.getQRIMGByIDAndType(PartyID, 3);
                    cardParty.PartyID = PartyID;
                    cardParty.QRImg = QRurl;

                    bool ret = cBO.UpdateParty(cardParty);
                    if (!ret) { return new ResultObject() { Flag = 0, Message = "更新QRImg失败!", Result = null }; }
                    return new ResultObject() { Flag = 1, Message = "更新QRImg成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "QRCodeImg还没生成,则不更新！", Result = null };
                }


            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "失败!", Result = ex };
            }


        }


        /// <summary>
        /// 获取名片群QRImg
        /// </summary>
        /// <returns></returns>
        [Route("GetGroupQRImg"), HttpGet, Anonymous]
        public ResultObject GetGroupQRImg(int GroupID, int AppType = 1)
        {

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CustomerBO customerBO = new CustomerBO(new CustomerProfile());
            try
            {
                CardGroupVO cvo = cBO.FindCardGroupById(GroupID);
                string QRurl = cvo.QRImg;
                if (QRurl == "")
                {
                    QRurl = cBO.getQRIMGByIDAndType(GroupID, 2);
                }
                return new ResultObject() { Flag = 1, Message = "成功!", Result = QRurl };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "失败!", Result = ex };
            }


        }

        /// <summary>
        /// 更新名片群QRImg
        /// </summary>
        /// <returns></returns>
        [Route("UpdateGroupQRImg"), HttpGet, Anonymous]
        public ResultObject UpdateGroupQRImg(int GroupID, int AppType = 1)
        {

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CustomerBO customerBO = new CustomerBO(new CustomerProfile());
            try
            {
                CardGroupVO cardGroupVO = new CardGroupVO();
                String QRurl = cBO.getQRIMGByIDAndType(GroupID, 2);
                cardGroupVO.GroupID = GroupID;
                cardGroupVO.QRImg = QRurl;

                bool ret = cBO.UpdateCardGroup(cardGroupVO);
                if (!ret) { return new ResultObject() { Flag = 0, Message = "更新QRImg失败!", Result = null }; }
                return new ResultObject() { Flag = 1, Message = "更新QRImg成功!", Result = null };

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "失败!", Result = ex };
            }


        }

        /// <summary>
        /// 测试获取活动小程序码
        /// </summary>
        /// <returns></returns>
        [Route("GetPartQR"), HttpGet, Anonymous]
        public ResultObject GetPartQR()
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            List<CardPartyVO> pVO = cBO.FindParty();
            for (int i = 0; i < pVO.Count; i++)
            {
                if (pVO[i].Content == "")
                {
                    pVO[i].Content = pVO[i].Title;
                    cBO.UpdateParty(pVO[i]);
                }
            }
            return new ResultObject() { Flag = 0, Message = "成功", Result = null };
        }


        /// <summary>
        /// 测试获取二维码
        /// </summary>
        /// <returns></returns>
        [Route("GetQR"), HttpGet, Anonymous]
        public ResultObject GetQR(string scene, string page)
        {
            CardBO cBO = new CardBO(new CustomerProfile());

            return new ResultObject() { Flag = 0, Message = "成功", Result = cBO.GetQRIMG(scene, page) };
        }

        /// <summary>
        /// 获取背景音乐
        /// </summary>
        /// <param name="PageCount">每页数量</param>
        /// <param name="PageIndex">第几页</param>
        /// <returns></returns>
        [Route("GetBGM"), HttpGet, Anonymous]
        public ResultObject GetBGM(int PageCount, int PageIndex)
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            List<myFileInfo> FileList = new List<myFileInfo>();
            try
            {
                string sql = "CustomerId = 0 and SizeType=3 order by Order_info desc,CardPoterID desc";
                List<CardPoterVO> PoterList = cBO.FindCardPoterByCondition(sql);
                foreach (CardPoterVO fi in PoterList)
                {
                    myFileInfo fVO = new myFileInfo();
                    fVO.FileName = fi.FileName;
                    fVO.Url = fi.Url;
                    fVO.Type = fi.Type;
                    fVO.index = fi.Order_info;
                    FileList.Add(fVO);
                }
            }
            catch
            {

            }

            List<myFileInfo> newFileList = FileList;

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newFileList.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = FileList.Count };
        }


        /// <summary>
        /// 获取锁屏海报
        /// </summary>
        /// <param name="PageCount">每页数量</param>
        /// <param name="PageIndex">第几页</param>
        /// <returns></returns>
        [Route("GetLockScreenPosters"), HttpGet, Anonymous]
        public ResultObject GetLockScreenPosters(int PageCount, int PageIndex)
        {
            List<myFileInfo> FileList = new List<myFileInfo>();
            try
            {
                //海报背景目录
                string path = "C:/web/ServicesPlatform/Style/images/wxcard/LockScreenPosters";
                DirectoryInfo di = new DirectoryInfo(path);
                //找到该目录下的文件 
                FileInfo[] fis = di.GetFiles();

                foreach (FileInfo fi in fis)
                {
                    myFileInfo fVO = new myFileInfo();

                    fVO.FileName = fi.Name.Split('.')[0];
                    fVO.Url = "https://www.zhongxiaole.net/SPManager/Style/images/wxcard/LockScreenPosters/" + HttpUtility.UrlEncode(fi.Name);

                    FileList.Add(fVO);
                }
            }
            catch
            {

            }

            List<myFileInfo> newFileList = FileList;

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newFileList.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = FileList.Count };
        }


        /// <summary>
        /// 获取活动海报背景
        /// </summary>
        /// <param name="PageCount">每页数量</param>
        /// <param name="PageIndex">第几页</param>
        /// <returns></returns>
        [Route("GetCustomPartyPoter"), HttpGet, Anonymous]
        public ResultObject GetCustomPartyPoter(int PageCount, int PageIndex)
        {
            List<myFileInfo> FileList = new List<myFileInfo>();
            try
            {
                //海报背景目录
                string path = "C:/web/ServicesPlatform/Style/images/wxcard/CustomPartyPoter";
                DirectoryInfo di = new DirectoryInfo(path);
                //找到该目录下的文件 
                FileInfo[] fis = di.GetFiles();

                foreach (FileInfo fi in fis)
                {
                    myFileInfo fVO = new myFileInfo();

                    fVO.FileName = fi.Name.Split('.')[0];
                    fVO.Url = "https://www.zhongxiaole.net/SPManager/Style/images/wxcard/CustomPartyPoter/" + HttpUtility.UrlEncode(fi.Name);

                    FileList.Add(fVO);
                }
            }
            catch
            {

            }

            List<myFileInfo> newFileList = FileList;

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newFileList.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = FileList.Count };
        }

        /// <summary>
        /// 获取抽奖海报背景
        /// </summary>
        /// <param name="PageCount">每页数量</param>
        /// <param name="PageIndex">第几页</param>
        /// <returns></returns>
        [Route("GetLuckDrawPoter"), HttpGet, Anonymous]
        public ResultObject GetLuckDrawPoter(int PageCount, int PageIndex, int AppType = 1)
        {
            List<myFileInfo> FileList = new List<myFileInfo>();
            try
            {
                //海报背景目录

                string path = "C:/web/ServicesPlatform/Style/images/wxcard/LuckDrawPoster";
                string Url = "https://www.zhongxiaole.net/SPManager/Style/images/wxcard/LuckDrawPoster/";

                DirectoryInfo di = new DirectoryInfo(path);
                //找到该目录下的文件 
                FileInfo[] fis = di.GetFiles();

                foreach (FileInfo fi in fis)
                {
                    myFileInfo fVO = new myFileInfo();

                    string FileName = fi.Name.Split('.')[0];
                    if (FileName.Split('_').Length > 1)
                    {
                        fVO.FileName = FileName.Split('_')[0];
                        fVO.index = Int32.Parse(FileName.Split('_')[1]);
                    }
                    else
                    {
                        fVO.FileName = FileName;
                        fVO.index = Int32.Parse(fVO.FileName);
                    }
                    fVO.Url = Url + HttpUtility.UrlEncode(fi.Name);

                    FileList.Add(fVO);
                }

                FileList.Sort((a, b) => a.index.CompareTo(b.index));
                FileList.Reverse();

                myFileInfo myFileInfo = FileList[FileList.Count - 1];
                FileList.Remove(myFileInfo);
                List<myFileInfo> newFile = new List<myFileInfo>();
                newFile.Add(myFileInfo);
                newFile.AddRange(FileList);
                FileList = newFile;
            }
            catch
            {

            }

            List<myFileInfo> newFileList = FileList;

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newFileList.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = FileList.Count };
        }

        /// <summary>
        /// 获取名片海报背景
        /// </summary>
        /// <param name="PageCount">每页数量</param>
        /// <param name="PageIndex">第几页</param>
        /// <returns></returns>
        [Route("GetCustomCardPoter"), HttpGet]
        public ResultObject GetCustomCardPoter(int PageCount, int PageIndex, string token, string Type = "")
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO cVO = uBO.FindCustomenById(cProfile.CustomerId);
            CardBO cBO = new CardBO(new CustomerProfile(), cVO.AppType);

            List<myFileInfo> FileList = new List<myFileInfo>();

            List<CardPoterVO> CardPoterList = cBO.FindCardPoterByCondition("CustomerId = " + customerId + " and SizeType=1");

            foreach (CardPoterVO item in CardPoterList)
            {
                myFileInfo fVO = new myFileInfo();

                fVO.FileName = item.FileName;
                fVO.Url = item.Url;
                fVO.CardPoterID = item.CardPoterID;

                FileList.Add(fVO);
            }

            FileList.Reverse();

            List<myFileInfo> LocalFileList = new List<myFileInfo>();
            try
            {
                string sql = "CustomerId = 0 and SizeType=1 order by Order_info desc,CardPoterID desc";
                if (Type != "" && Type != null)
                {
                    sql = " Type='" + Type + "' and " + sql;
                }
                List<CardPoterVO> fis = cBO.FindCardPoterByCondition(sql);
                foreach (CardPoterVO fi in fis)
                {
                    myFileInfo fVO = new myFileInfo();
                    fVO.FileName = fi.FileName;
                    fVO.Url = fi.Url;
                    fVO.Type = fi.Type;
                    fVO.index = fi.Order_info;

                    LocalFileList.Add(fVO);
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
            }

            List<myFileInfo> newFileList = new List<myFileInfo>();
            if (Type == "" || Type == null)
            {
                FileList.AddRange(LocalFileList);
                newFileList = FileList;
            }
            else
            {
                newFileList = LocalFileList;
            }


            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newFileList.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = FileList.Count, Subsidiary = Type };
        }


        /// <summary>
        /// 测试4
        /// </summary>
        /// <param name="PageCount">每页数量</param>
        /// <param name="PageIndex">第几页</param>
        /// <returns></returns>
        [Route("test4"), HttpGet, Anonymous]
        public ResultObject test4()
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            //海报背景目录
            string path = "C:/web/ServicesPlatform/Style/images/wxcard/CustomCardPoter";
            DirectoryInfo di = new DirectoryInfo(path);
            //找到该目录下的文件 
            FileInfo[] fis = di.GetFiles();

            List<myFileInfo> LocalFileList = new List<myFileInfo>();
            foreach (FileInfo fi in fis)
            {
                myFileInfo fVO = new myFileInfo();
                fVO.FileName = fi.Name;
                fVO.Url = "https://www.zhongxiaole.net/SPManager/Style/images/wxcard/CustomCardPoter/" + HttpUtility.UrlEncode(fi.Name);

                fVO.index = Int32.Parse(fVO.FileName.Split('_')[0]);
                fVO.Type = fi.Name.Split('.')[0].Split('_')[1];
                LocalFileList.Add(fVO);
            }
            LocalFileList.Sort((a, b) => a.index.CompareTo(b.index));
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = LocalFileList };
        }


        /// <summary>
        /// 上传自定义海报背景
        /// </summary>
        /// <returns></returns>
        [Route("UploadCardPoter"), HttpPost]
        public ResultObject UploadCardPoter(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO cVO = uBO.FindCustomenById(cProfile.CustomerId);
            CardBO cBO = new CardBO(new CustomerProfile(), cVO.AppType);

            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string folder = "/UploadFolder/CardPoterImage/" + DateTime.Now.ToString("yyyyMM") + "/";
            string imgPath = "";
            if (hfc.Count > 0)
            {
                try
                {
                    FileInfo fi = new FileInfo(hfc[0].FileName);

                    string ext = fi.Extension.ToLower();
                    if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
                    {
                        return new ResultObject() { Flag = 0, Message = "文件类型错误", Result = null };
                    }

                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fi.Extension;

                    //可以修改为网络路径
                    string localPath = ConfigInfo.Instance.UploadFolder + folder;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string PhysicalPath = localPath + newFileName;

                    hfc[0].SaveAs(PhysicalPath);

                    imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;

                    CardPoterVO CardPoterVO = new CardPoterVO();
                    CardPoterVO.CardPoterID = 0;
                    CardPoterVO.CustomerId = customerId;
                    CardPoterVO.FileName = newFileName;
                    CardPoterVO.Url = imgPath;
                    cBO.AddCardPoter(CardPoterVO);

                    return new ResultObject() { Flag = 1, Message = "上传成功", Result = imgPath };
                }
                catch (Exception er)
                {
                    return new ResultObject() { Flag = 0, Message = "上传失败", Result = er.Message };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "上传失败", Result = "文件为空" };
            }
        }

        /// <summary>
        /// 删除自定义海报
        /// </summary>
        /// <param name="CardPoterID">海报ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("delCardPoter"), HttpGet]
        public ResultObject delCardPoter(int CardPoterID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            if (cBO.DeleteCardPoterAdminById(CardPoterID) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 后台删除海报
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteCardPoterByAdmin"), HttpGet]
        public ResultObject DeleteCardPoterByAdmin(string NewsID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);
            try
            {
                if (!string.IsNullOrEmpty(NewsID))
                {
                    string[] messageIdArr = NewsID.Split(',');
                    bool isAllDelete = true;
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {
                            cBO.DeleteCardPoterAdminById(Convert.ToInt32(messageIdArr[i]));
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }

        }
        /// <summary>
        /// 修改海报排序（后台专用）
        /// </summary>
        /// <param name="Sum"></param>
        /// <param name="CardPoterID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("setCardPoterOrder"), HttpGet]
        public ResultObject setCardPoterOrder(int Sum, int CardPoterID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            if (Sum < 0) { return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null }; }
            if (CardPoterID < 0) { return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null }; }

            CardPoterVO CardPoterVO = new CardPoterVO();
            CardPoterVO.CardPoterID = CardPoterID;
            CardPoterVO.Order_info = Sum;
            if (cBO.UpdateCardPoter(CardPoterVO))
            {
                return new ResultObject() { Flag = 1, Message = "修改成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "修改失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取海报文件
        /// </summary>
        /// <param name="PageCount">每页数量</param>
        /// <param name="PageIndex">第几页</param>
        /// <returns></returns>
        [Route("GetPosterImg"), HttpGet, Anonymous]
        public ResultObject GetPosterImg(int PageCount, int PageIndex, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            List<myFileInfo> FileList = new List<myFileInfo>();
            try
            {
                //海报背景目录

                string path = "C:/web/ServicesPlatform/Style/images/wxcard/CardPoster";
                string Url = "https://www.zhongxiaole.net/SPManager/Style/images/wxcard/CardPoster/";

                if (AppType == 3)
                {
                    path = "C:/web/ServicesPlatform/Style/images/wxcard/CardPosterWYZT";
                    Url = "https://www.zhongxiaole.net/SPManager/Style/images/wxcard/CardPosterWYZT/";
                }

                DirectoryInfo di = new DirectoryInfo(path);
                //找到该目录下的文件 
                FileInfo[] fis = di.GetFiles();

                foreach (FileInfo fi in fis)
                {
                    myFileInfo fVO = new myFileInfo();

                    string FileName = fi.Name.Split('.')[0];
                    if (FileName.Split('_').Length > 1)
                    {
                        fVO.FileName = FileName.Split('_')[0];
                        fVO.index = Int32.Parse(FileName.Split('_')[1]);
                    }
                    else
                    {
                        fVO.FileName = FileName;
                        fVO.index = Int32.Parse(fVO.FileName);
                    }
                    fVO.Url = Url + HttpUtility.UrlEncode(fi.Name);

                    FileList.Add(fVO);
                }

                FileList.Sort((a, b) => a.index.CompareTo(b.index));
                FileList.Reverse();

                myFileInfo myFileInfo = FileList[FileList.Count - 1];
                FileList.Remove(myFileInfo);
                List<myFileInfo> newFile = new List<myFileInfo>();
                newFile.Add(myFileInfo);


                string sql = "CustomerId = 0 and SizeType=2 order by Order_info desc,CardPoterID desc";
                List<myFileInfo> LocalFileList = new List<myFileInfo>();
                List<CardPoterVO> PoterList = cBO.FindCardPoterByCondition(sql);
                foreach (CardPoterVO fi in PoterList)
                {
                    myFileInfo fVO = new myFileInfo();
                    fVO.FileName = fi.CardPoterID.ToString();
                    fVO.Url = fi.Url;
                    fVO.Type = fi.Type;
                    fVO.index = fi.Order_info;

                    LocalFileList.Add(fVO);
                }
                newFile.AddRange(LocalFileList);
                newFile.AddRange(FileList);

                FileList = newFile;
            }
            catch
            {

            }

            List<myFileInfo> newFileList = FileList;

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newFileList.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = FileList.Count };
        }

        /// <summary>
        /// 获取操作视频
        /// </summary>
        /// <param name="PageCount">每页数量</param>
        /// <param name="PageIndex">第几页</param>
        /// <returns></returns>
        [Route("Getvideo"), HttpGet, Anonymous]
        public ResultObject Getvideo(int PageCount, int PageIndex)
        {
            List<myFileInfo> FileList = new List<myFileInfo>();
            try
            {
                //海报背景目录
                string path = "C:/web/ServicesPlatform/Style/video";
                DirectoryInfo di = new DirectoryInfo(path);
                //找到该目录下的文件 
                FileInfo[] fis = di.GetFiles();

                foreach (FileInfo fi in fis)
                {
                    myFileInfo fVO = new myFileInfo();

                    fVO.FileName = fi.Name.Split('.')[0];
                    fVO.Url = "https://www.zhongxiaole.net/SPManager/Style/video/" + HttpUtility.UrlEncode(fi.Name);

                    FileList.Add(fVO);
                }
            }
            catch
            {

            }

            List<myFileInfo> newFileList = FileList;

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newFileList.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = FileList.Count };
        }

        /// <summary>
        /// 获取海报预览图
        /// </summary>
        /// <param name="PageCount">每页数量</param>
        /// <param name="PageIndex">第几页</param>
        /// <returns></returns>
        [Route("GetPreview"), HttpGet, Anonymous]
        public ResultObject GetPreview(int PageCount, int PageIndex)
        {
            List<myFileInfo> FileList = new List<myFileInfo>();
            try
            {
                //海报背景目录
                string path = "C:/web/ServicesPlatform/Style/images/wxcard/PartyPoster";
                DirectoryInfo di = new DirectoryInfo(path);
                //找到该目录下的文件 
                FileInfo[] fis = di.GetFiles();

                foreach (FileInfo fi in fis)
                {
                    myFileInfo fVO = new myFileInfo();

                    string FileName = fi.Name.Split('.')[0];
                    if (FileName.Split('_').Length > 1)
                    {
                        fVO.FileName = FileName.Split('_')[0];
                        fVO.index = Int32.Parse(FileName.Split('_')[1]);
                    }
                    else
                    {
                        fVO.FileName = FileName;
                        fVO.index = Int32.Parse(fVO.FileName);
                    }
                    if (fVO.FileName == "8" || fVO.FileName == "9" || fVO.FileName == "10" || fVO.FileName == "11" || fVO.FileName == "33" || fVO.FileName == "15" || fVO.FileName == "36" || fVO.FileName == "37")
                    {
                        fVO.isVip = false;
                    }
                    else
                    {
                        fVO.isVip = true;
                    }


                    fVO.Url = "https://www.zhongxiaole.net/SPManager/Style/images/wxcard/PartyPoster/" + HttpUtility.UrlEncode(fi.Name);

                    FileList.Add(fVO);
                }

                FileList.Sort((a, b) => a.index.CompareTo(b.index));
                FileList.Reverse();
            }
            catch
            {

            }

            List<myFileInfo> newFileList = FileList;

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newFileList.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = FileList.Count };
        }


        /// <summary>
        /// 获取站内提醒信息列表
        /// </summary>
        /// <param name="PageCount">每页数量</param>
        /// <param name="PageIndex">第几页</param>
        /// <returns></returns>
        [Route("GetCardMessage"), HttpGet]
        public ResultObject GetCardMessage(int PageCount, int PageIndex, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO customerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = customerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            List<CardMessageVO> FileList = cBO.FindCardMessageByCondtion("CustomerId=" + customerId + "  ORDER BY CreatedAt desc");

            for (int i = 0; i < FileList.Count; i++)
            {
                cBO.UpdateCardMessage(FileList[i].MessageID);
            }

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = FileList.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = FileList.Count };
        }


        /// <summary>
        /// 添加或更新日志
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateLog"), HttpPost]
        public ResultObject UpdateLog([FromBody] LogVO LogVO, string token)
        {
            if (LogVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO cVO = uBO.FindCustomenById(cProfile.CustomerId);
            CardBO cBO = new CardBO(new CustomerProfile(), cVO.AppType);

            if (!cVO.isAdmin)
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }
            else
            {
                if (LogVO.LogID > 0)
                {
                    LogVO.Type = "LLMP";
                    if (cBO.UpdateLog(LogVO))
                    {
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = LogVO.LogID };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                    }
                }
                else
                {
                    LogVO.Type = "LLMP";
                    LogVO.CreatedAt = DateTime.Now;
                    int LogID = cBO.AddLog(LogVO);
                    if (LogID > 0)
                    {
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = LogID };
                    }
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
            }
        }

        /// <summary>
        /// 获取日志详情
        /// </summary>
        /// <returns></returns>
        [Route("GetLogByLogID"), HttpGet, Anonymous]
        public ResultObject GetLogByLogID(int LogID, int AppType = 1)
        {

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            try
            {
                LogVO cvo = cBO.FindLogById(LogID);
                return new ResultObject() { Flag = 1, Message = "成功!", Result = cvo };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "失败!", Result = ex };
            }
        }

        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <returns></returns>
        [Route("getLoglist"), HttpGet, Anonymous]
        public ResultObject getLoglist(int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            List<LogVO> cVO = cBO.GetLogVOList("Type='LLMP'");
            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO };
                }
                else
                {
                    return new ResultObject() { Flag = 2, Message = "暂无信息!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }


        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="LogID">ID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("DelLog"), HttpGet]
        public ResultObject DelLog(int LogID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;


            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO cVO = uBO.FindCustomenById(cProfile.CustomerId);

            if (!cVO.isAdmin)
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            CardBO cBO = new CardBO(new CustomerProfile(), cVO.AppType);

            if (cBO.DeleteLogById(LogID) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 测试审核文本
        /// </summary>
        /// <returns></returns>
        [Route("TestReviewText"), HttpPost, Anonymous]
        public ResultObject TestReviewText([FromBody] string LogVO)
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            /*审核文本是否合法*/
            if (!cBO.msg_sec_check(LogVO))
            {
                return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "审核通过!", Result = null };
            }
        }

        /// <summary>
        /// 添加访问记录
        /// </summary>
        /// <returns></returns>
        [Route("AddAccessrecords"), HttpGet, Anonymous]
        public ResultObject AddAccessrecords(string Type, int ById, int CustomerId, string code, int ToCustomerId = 0, int ShareCustomerId = 0, int PayType = 1, int AppType = 1)
        {
            if (PayType != 1)
            {
                AppType = PayType;
            }
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardAccessRecordsVO aVO = new CardAccessRecordsVO();
            aVO.AccessRecordsID = 0;
            aVO.ById = ById;
            aVO.Type = Type;
            aVO.CustomerId = CustomerId;
            aVO.ShareCustomerId = ShareCustomerId;
            aVO.AccessAt = DateTime.Now;
            string OpenID = "Null";
            try
            {
                OpenID = cBO.getOpenId(code);
            }
            catch
            {

            }
            if (OpenID == "error")
            {
                OpenID = "Null";
            }
            aVO.OpenID = OpenID;

            if (ToCustomerId == 0)
            {
                if (Type == "ReadCard" || Type == "CollectionCard" || Type == "ForwardCard" || Type == "DepositInPhone" || Type == "CallCard")
                {
                    CardDataVO cVO = cBO.FindCardById(ById);
                    if (cVO != null)
                    {
                        aVO.ToCustomerId = cVO.CustomerId;
                    }
                }
                if (Type == "ReadParty" || Type == "ForwardParty")
                {
                    CardPartyVO CardPartyVO = cBO.FindPartyById(ById);
                    if (CardPartyVO != null)
                    {
                        aVO.ToCustomerId = CardPartyVO.CustomerId;
                    }
                }
            }
            else
            {
                aVO.ToCustomerId = ToCustomerId;
            }

            aVO.Counts = cBO.FindAccessrecordsCount("OpenID='" + OpenID + "' and OpenID<>'Null' and OpenID<>'' and Type='" + Type + "' and ToCustomerId=" + ToCustomerId) + 1;
            aVO.LoginIP = HttpContext.Current.Request.UserHostAddress;

            try
            {
                string jsonstr = HttpHelper.HtmlFromUrlGet("http://apis.map.qq.com/ws/location/v1/ip?key=QHBBZ-ZDAWI-TRVGE-5GWEO-PHEKO-Q5BXC&ip=" + aVO.LoginIP);
                JObject jo = (JObject)JsonConvert.DeserializeObject(jsonstr);
                aVO.Nation = jo["result"]["ad_info"]["nation"].ToString();
                aVO.Province = jo["result"]["ad_info"]["province"].ToString();
                aVO.City = jo["result"]["ad_info"]["city"].ToString();
                aVO.District = jo["result"]["ad_info"]["district"].ToString();
                aVO.lat = Convert.ToDecimal(jo["result"]["location"]["lat"]);
                aVO.lng = Convert.ToDecimal(jo["result"]["location"]["lng"]);
            }
            catch
            {

            }


            int AccessRecordsID = cBO.AddAccessrecords(aVO);
            if (AccessRecordsID > 0)
                return new ResultObject() { Flag = 1, Message = "添加成功!", Result = AccessRecordsID };
            else
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
        }

        /// <summary>
        /// 修改访问记录停留时间
        /// </summary>
        /// <returns></returns>
        [Route("EditAccessrecords"), HttpGet, Anonymous]
        public ResultObject EditAccessrecords(int AccessRecordsID, int ResidenceAt, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardAccessRecordsVO aVO = cBO.FindAccessrecordsByAccessrecordsID(AccessRecordsID);
            if (aVO != null)
            {
                aVO.ResidenceAt = ResidenceAt;
                cBO.UpdateAccessrecords(aVO);
                return new ResultObject() { Flag = 1, Message = "保存停留时间成功!", Result = AccessRecordsID };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "保存停留时间失败!", Result = AccessRecordsID };
            }
        }


        /// <summary>
        /// 获取访问记录列表
        /// </summary>
        /// <returns></returns>
        [Route("GetAccessrecords"), HttpGet]
        public ResultObject GetAccessrecords(string Type, int ById, int PageIndex, int PageCount, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CustomerBO uBo = new CustomerBO(new CustomerProfile());

            string Condtion = "Type='" + Type + "' and ById=" + ById + " and ToCustomerId=" + customerId;
            List<CardAccessRecordsVO> aVO = cBO.FindAccessrecordsByCondtion(Condtion);
            if (aVO != null)
            {
                try
                {
                    List<CardDataVO> cVO = new List<CardDataVO>();
                    aVO.Reverse();

                    for (int i = 0; i < aVO.Count; i++)
                    {
                        if (cVO.Exists(p => p.CustomerId == aVO[i].CustomerId && CustomerVO2.AppType != 3))
                        {
                            continue;
                        }

                        CardDataVO CardDataVO = new CardDataVO();
                        List<CardDataVO> clVO = cBO.FindCardByCustomerId(aVO[i].CustomerId);

                        if (clVO.Count > 0)
                        {
                            CardDataVO = clVO[0];
                            CardDataVO.CreatedAt = aVO[i].AccessAt;
                            cVO.Add(CardDataVO);
                        }
                        else
                        {
                            if (aVO[i].CustomerId > 0)
                            {
                                CustomerVO uVO = uBo.FindCustomenById(aVO[i].CustomerId);
                                if (uVO != null)
                                {
                                    CardDataVO.CustomerId = uVO.CustomerId;
                                    CardDataVO.Name = uVO.CustomerName;
                                    CardDataVO.Headimg = uVO.HeaderLogo;
                                    CardDataVO.CreatedAt = aVO[i].AccessAt;
                                    cVO.Add(CardDataVO);
                                }
                            }
                        }
                    }

                    //匿名访问人数
                    int AnonymousCount = cBO.FindAccessrecordsCount(Condtion + " and CustomerId=0", true);

                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = cVO.Count, Subsidiary = AnonymousCount };
                }
                catch (Exception ex)
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex, Subsidiary = null };
                }

            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SubscriptionMessage"), HttpGet]
        public ResultObject SubscriptionMessage(string OpenId, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            CardFormListVO suVO = new CardFormListVO();
            suVO.Style = AppType;
            suVO.FormId = "";
            suVO.CustomerId = customerId;
            suVO.CreatedAt = DateTime.Now;
            suVO.OpenId = OpenId;
            int FormListID = cBO.AddFormId(suVO);

            if (cBO.AddSubscription(customerId, OpenId) > 0)
            {
                return new ResultObject() { Flag = 1, Message = "订阅成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "订阅失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新签到表
        /// </summary>
        /// <param name="CardQuestionnaireVO"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateQuestionnaire"), HttpPost]
        public ResultObject UpdateQuestionnaire([FromBody] CardQuestionnaireVO CardQuestionnaireVO, string token)
        {
            if (CardQuestionnaireVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            /*审核文本是否合法*/
            if (!cBO.msg_sec_check(CardQuestionnaireVO))
            {
                return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
            }
            /*审核文本是否合法*/

            if (CardQuestionnaireVO.QuestionnaireID > 0)
            {

                if (CardQuestionnaireVO.CustomerId != customerId && !cBO.isQuestionnaireAdmin(CardQuestionnaireVO.QuestionnaireID, customerId))
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败,你没有权限!", Result = null };
                }

                if (cBO.UpdateCardQuestionnaire(CardQuestionnaireVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardQuestionnaireVO.QuestionnaireID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
            else
            {
                CardQuestionnaireVO.CreatedAt = DateTime.Now;
                CardQuestionnaireVO.CustomerId = customerId;

                int QuestionnaireID = cBO.AddQuestionnaire(CardQuestionnaireVO);
                if (QuestionnaireID > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = QuestionnaireID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新签到表管理员
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="QuestionnaireID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateQuestionnaireAdmin"), HttpGet]
        public ResultObject UpdateQuestionnaireAdmin(string CustomerId, int QuestionnaireID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            CardQuestionnaireVO CardQuestionnaireVO = cBO.FindCardQuestionnaireByQuestionnaireID(QuestionnaireID);
            if (CardQuestionnaireVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }
            if (CardQuestionnaireVO.CustomerId != customerId)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败,你没有权限!", Result = null };
            }
            try
            {
                if (!string.IsNullOrEmpty(CustomerId))
                {
                    string[] messageIdArr = CustomerId.Split(',');
                    bool isAllDelete = true;
                    cBO.DeleteQuestionnaireAdminById(QuestionnaireID);
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {
                            int customerid = Convert.ToInt32(messageIdArr[i]);
                            if (!cBO.isQuestionnaireAdmin(QuestionnaireID, customerid))
                            {
                                CardQuestionnaireAdminVO cCVO = new CardQuestionnaireAdminVO();
                                cCVO.QuestionnaireID = QuestionnaireID;
                                cCVO.CustomerId = customerid;
                                cBO.AddQuestionnaireAdmin(cCVO);
                            }
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "修改管理员成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "修改管理员成功!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "修改管理员失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我的签到表列表
        /// </summary>
        ///<param name="PageIndex"></param>
        ///<param name="PageCount"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyQuestionnaireList"), HttpGet]
        public ResultObject getMyQuestionnaireList(int PageIndex, int PageCount, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            List<CardQuestionnaireVO> qVO = cBO.FindCardQuestionnaireByCondtion("CustomerId = " + customerId + " and isTemplate=0");


            //仅作为管理员的签到表
            List<CardQuestionnaireVO> newlVO = new List<CardQuestionnaireVO>();
            List<CardQuestionnaireAdminVO> aVO = cBO.FindQuestionnaireAdminByCondition("CustomerId = " + customerId);

            for (int i = 0; i < aVO.Count; i++)
            {
                try
                {
                    if (!qVO.Exists(p => p.QuestionnaireID == aVO[i].QuestionnaireID))
                    {
                        CardQuestionnaireVO cVo = cBO.FindCardQuestionnaireByQuestionnaireID(aVO[i].QuestionnaireID);

                        if (cVo != null)
                        {
                            newlVO.Add(cVo);
                        }
                    }
                }
                catch
                {

                }
            }

            if (qVO.Count > 0)
                qVO.AddRange(newlVO);
            else
                qVO = newlVO;

            IEnumerable<CardQuestionnaireVO> newqVO = qVO.OrderBy(f => f.CreatedAt).Reverse().Skip((PageIndex - 1) * PageCount).Take(PageCount);

            foreach (CardQuestionnaireVO item in newqVO)
            {
                try
                {
                    item.PeopleCount = cBO.FindCardQuestionnaireSignup("QuestionnaireID = " + item.QuestionnaireID + " and Status=1");
                }
                catch
                {

                }
            }

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newqVO, Count = qVO.Count + newlVO.Count };
        }

        /// <summary>
        /// 获取签到表模板列表
        /// </summary>
        /// <returns></returns>
        [Route("getQuestionnaireByTemplate"), HttpGet, Anonymous]
        public ResultObject getQuestionnaireByTemplate()
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            List<CardQuestionnaireVO> qVO = cBO.FindCardQuestionnaireByCondtion("isTemplate = 1");
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = qVO, };
        }

        /// <summary>
        /// 获取签到表详情
        /// </summary>
        ///<param name="QuestionnaireID"></param>
        /// <returns></returns>
        [Route("getQuestionnaire"), HttpGet, Anonymous]
        public ResultObject getQuestionnaire(int QuestionnaireID, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardQuestionnaireVO qVO = cBO.FindCardQuestionnaireByQuestionnaireID(QuestionnaireID);

            if (qVO != null)
            {
                if (qVO.QRImg == "")
                {
                    qVO.QRImg = cBO.GetQuestionnaireQR(QuestionnaireID);
                }

                BusinessCardBO BusinessCardBO = new BusinessCardBO(new CustomerProfile());
                BusinessCardVO bVO = new BusinessCardVO();
                if (qVO.BusinessID != 0)
                {
                    bVO = BusinessCardBO.FindBusinessCardById(qVO.BusinessID);
                }

                List<CardQuestionnaireAdminVO> aVO = cBO.FindQuestionnaireAdminByCondition("QuestionnaireID = " + QuestionnaireID);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = qVO, Subsidiary = bVO, Subsidiary2 = aVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除签到表
        /// </summary>
        /// <param name="QuestionnaireID">签到表ID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("DelQuestionnaire"), HttpGet]
        public ResultObject DelQuestionnaire(int QuestionnaireID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardQuestionnaireVO uVO = cBO.FindCardQuestionnaireByQuestionnaireID(QuestionnaireID);


            if (uVO != null)
            {
                if (uVO.CustomerId != customerId)
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限删除!", Result = null };
                }
                //删除所有签到
                List<CardQuestionnaireSignupVO> qsVO = cBO.FindCardQuestionnaireSignupByCondtion("QuestionnaireID = " + QuestionnaireID);
                for (int i = 0; i < qsVO.Count; i++)
                {
                    cBO.DeleteByQuestionnaireSignupID(qsVO[i].QuestionnaireSignupID);
                }
                cBO.DeleteByQuestionnaireID(QuestionnaireID);

                //删除管理员
                cBO.DeleteQuestionnaireAdminById(QuestionnaireID);

                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新签到
        /// </summary>
        /// <param name="CardQuestionnaireSignupVO"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateQuestionnaireSignup"), HttpPost]
        public ResultObject UpdateQuestionnaire([FromBody] CardQuestionnaireSignupVO CardQuestionnaireSignupVO, string token)
        {
            if (CardQuestionnaireSignupVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            int oldCount = cBO.FindCardQuestionnaireSignup("CustomerId = " + customerId + " and QuestionnaireID=" + CardQuestionnaireSignupVO.QuestionnaireID + " and NOW()-CreatedAt<30");
            if (oldCount > 0)
            {
                return new ResultObject() { Flag = 0, Message = "操作过于频繁，请稍后!", Result = null };
            }
            try
            {


                /*审核文本是否合法*/
                if (!cBO.msg_sec_check(CardQuestionnaireSignupVO))
                {
                    return new ResultObject() { Flag = 0, Message = "有政治敏感或违法关键词，请重新填写!", Result = null };
                }
                /*审核文本是否合法*/

                if (CardQuestionnaireSignupVO.QuestionnaireSignupID > 0)
                {
                    if (cBO.UpdateCardQuestionnaireSignup(CardQuestionnaireSignupVO))
                    {
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardQuestionnaireSignupVO.QuestionnaireSignupID };
                    }
                    else
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
                else
                {
                    CardQuestionnaireVO qVO = cBO.FindCardQuestionnaireByQuestionnaireID(CardQuestionnaireSignupVO.QuestionnaireID);

                    if (qVO == null)
                    {
                        return new ResultObject() { Flag = 0, Message = "签到失败，该签到项目已被删除!", Result = null };
                    }

                    if (qVO.limitPeopleNum > 0)
                    {
                        int PeopleNum = cBO.FindCardQuestionnaireSignup("QuestionnaireID=" + qVO.QuestionnaireID + " and Status=1");
                        if (PeopleNum >= qVO.limitPeopleNum)
                        {
                            return new ResultObject() { Flag = 2, Message = "签到失败，签到人数已满!", Result = null };
                        }
                    }

                    if (!qVO.isRepeat)
                    {
                        int PeopleNum = cBO.FindCardQuestionnaireSignup("QuestionnaireID=" + qVO.QuestionnaireID + " and CustomerId=" + customerId + " and Status=1");
                        if (PeopleNum > 0)
                        {
                            return new ResultObject() { Flag = 2, Message = "签到失败，您已经提交过了!", Result = null };
                        }
                    }

                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    CustomerVO uVO = uBO.FindCustomenById(customerId);
                    CardQuestionnaireSignupVO.CreatedAt = DateTime.Now;
                    CardQuestionnaireSignupVO.CustomerId = customerId;
                    CardQuestionnaireSignupVO.Headimg = uVO.HeaderLogo;

                    var models = JsonConvert.DeserializeObject<List<QuestionnaireSigupForm>>(CardQuestionnaireSignupVO.SigupForm);
                    for (int i = 0; i < models.Count; i++)
                    {
                        if (models[i].Name == "姓名")
                            CardQuestionnaireSignupVO.Name = models[i].value;
                        if (models[i].Name == "手机")
                            CardQuestionnaireSignupVO.Phone = models[i].value;
                    }

                    if (CardQuestionnaireSignupVO.Name == "")
                    {
                        CardQuestionnaireSignupVO.Name = uVO.CustomerName;
                    }
                    if (CardQuestionnaireSignupVO.Phone == "")
                    {
                        CardQuestionnaireSignupVO.Phone = uVO.Phone;
                    }

                    int QuestionnaireSignupID = cBO.AddQuestionnaireSignup(CardQuestionnaireSignupVO);
                    if (QuestionnaireSignupID > 0)
                    {
                        List<CardDataVO> cVO = cBO.FindCardByCustomerId(customerId);
                        if (cVO.Count <= 0)
                        {
                            CardDataVO CardDataVO = new CardDataVO();
                            CardDataVO.Name = CardQuestionnaireSignupVO.Name;
                            CardDataVO.Phone = CardQuestionnaireSignupVO.Phone;
                            CardDataVO.Headimg = CardQuestionnaireSignupVO.Headimg;

                            try
                            {

                                string Position = "";
                                string CorporateName = "";
                                string Address = "";
                                decimal latitude = 0;
                                decimal longitude = 0;
                                string WeChat = "";

                                var model = JsonConvert.DeserializeObject<List<QuestionnaireSigupForm>>(CardQuestionnaireSignupVO.SigupForm);

                                for (int i = 0; i < model.Count; i++)
                                {
                                    if (model[i].Name == "职位")
                                        Position = model[i].value;
                                    if (model[i].Name == "工作单位")
                                        CorporateName = model[i].value;
                                    if (model[i].Name == "微信")
                                        WeChat = model[i].value;
                                    if (model[i].Name == "单位地址")
                                    {
                                        Address = model[i].value;
                                        WeiXinGeocoder Geocoder = cBO.getLatitudeAndLongitude(model[i].value);
                                        if (Geocoder != null)
                                        {
                                            latitude = Geocoder.result.location.lat;
                                            longitude = Geocoder.result.location.lng;
                                        }
                                    }
                                }

                                CardDataVO.Position = Position;
                                CardDataVO.CorporateName = CorporateName;
                                CardDataVO.Address = Address;
                                CardDataVO.WeChat = WeChat;
                                CardDataVO.latitude = latitude;
                                CardDataVO.longitude = longitude;
                            }
                            catch
                            {
                            }

                            CardDataVO.CreatedAt = DateTime.Now;
                            CardDataVO.Status = 1;//0:禁用，1:启用
                            CardDataVO.CustomerId = customerId;
                            CardDataVO.isQuestionnaire = 1;
                            cBO.AddCard(CardDataVO);
                        }
                        cBO.AddCardMessage(CustomerVO2.CustomerName + "填写了表格《" + qVO.Title + "》", qVO.CustomerId, "表格签到", "/pages/index/SignInFormByUserList/SignInFormByUserList?QuestionnaireID=" + qVO.QuestionnaireID);
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = QuestionnaireSignupID };
                    }
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = ex };
            }
        }

        /// <summary>
        /// 删除签到
        /// </summary>
        /// <param name="QuestionnaireSignupID">签到表ID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("DelQuestionnaireSignup"), HttpGet]
        public ResultObject DelQuestionnaireSignup(int QuestionnaireSignupID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            CardQuestionnaireSignupVO uVO = cBO.FindCardQuestionnaireSignupByQuestionnaireSignupID(QuestionnaireSignupID);

            if (uVO != null)
            {
                uVO.Status = 0;
                cBO.UpdateCardQuestionnaireSignup(uVO);
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 清空签到列表
        /// </summary>
        ///<param name="QuestionnaireID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("delQuestionnaireSignupByQuestionnaireID"), HttpGet]
        public ResultObject getQuestionnaireSignupByQuestionnaireID(int QuestionnaireID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardQuestionnaireVO qVO = cBO.FindCardQuestionnaireByQuestionnaireID(QuestionnaireID);
            if (qVO != null)
            {
                //删除所有签到
                List<CardQuestionnaireSignupVO> qsVO = cBO.FindCardQuestionnaireSignupByCondtion("QuestionnaireID = " + QuestionnaireID);
                for (int i = 0; i < qsVO.Count; i++)
                {
                    qsVO[i].Status = 0;
                    cBO.UpdateCardQuestionnaireSignup(qsVO[i]);
                }
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取签到列表
        /// </summary>
        ///<param name="PageIndex"></param>
        ///<param name="PageCount"></param>
        ///<param name="QuestionnaireID"></param>
        /// <param name="token">口令</param>
        /// <param name="Time"></param>
        /// <returns></returns>
        [Route("getQuestionnaireSignupByQuestionnaireID"), HttpGet]
        public ResultObject getQuestionnaireSignupByQuestionnaireID(int PageIndex, int PageCount, int QuestionnaireID, string token, string Time = "")
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardQuestionnaireVO qVO = cBO.FindCardQuestionnaireByQuestionnaireID(QuestionnaireID);
            if (qVO != null)
            {
                string sql = "QuestionnaireID = " + QuestionnaireID + " and Status=1";
                if (Time != "")
                {
                    sql += " and to_days(CreatedAt) = to_days('" + Time + "')";
                }
                List<CardQuestionnaireSignupVO> qsVO = cBO.FindCardQuestionnaireSignupByCondtion(sql);
                qsVO.Reverse();
                qVO.PeopleCount = qsVO.Count;
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = qsVO.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = qsVO.Count, Subsidiary = qVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }



        /// <summary>
        /// 获取签到二维码
        /// </summary>
        /// <param name="QuestionnaireID">分享路径</param>
        /// <returns></returns>
        [Route("GetQuestionnaireSignupQR"), HttpGet]
        public ResultObject GetQuestionnaireSignupQR(int QuestionnaireID, string token, int AppType = 1)
        {
            try
            {
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
                if (AppType == 1)
                {
                    AppType = CustomerVO2.AppType;
                }
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);
                string str = cBO.getQRIMGByIDAndType(QuestionnaireID, 5, customerId);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = str };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 下载签到表所有报名的Excel文件
        /// </summary>
        /// <param name="QuestionnaireID"></param>
        /// <returns></returns>
        [Route("getQuestionnaireSignupToExcel"), HttpGet]
        public ResultObject getQuestionnaireSignupToExcel(int QuestionnaireID, string token)
        {

            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(uProfile.CustomerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);
            CardQuestionnaireVO qVO = cBO.FindCardQuestionnaireByQuestionnaireID(QuestionnaireID);

            if (qVO == null) { return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null }; }
            if (uProfile.CustomerId != qVO.CustomerId) { return new ResultObject() { Flag = 0, Message = "权限不足，下载失败!", Result = null }; }

            List<CardQuestionnaireSignupVO> cVO = cBO.FindCardQuestionnaireSignupByCondtion("QuestionnaireID = " + QuestionnaireID + " and Status = 1");

            if (cVO != null)
            {
                if (cVO.Count > 0)
                {
                    DataTable dt = new DataTable();

                    dt.Columns.Add("序号", typeof(Int32));
                    dt.Columns.Add("姓名", typeof(String));
                    dt.Columns.Add("手机", typeof(String));
                    dt.Columns.Add("填写时间", typeof(DateTime));

                    for (int i = 0; i < cVO.Count; i++)
                    {

                        DataRow row = dt.NewRow();
                        row["序号"] = i + 1;
                        row["姓名"] = cVO[i].Name;
                        row["手机"] = cVO[i].Phone;
                        row["填写时间"] = cVO[i].CreatedAt;

                        try
                        {


                            var model = JsonConvert.DeserializeObject<List<QuestionnaireSigupForm>>(cVO[i].SigupForm);

                            for (int j = 0; j < model.Count; j++)
                            {
                                if (model[j].Name != "姓名" && model[j].Name != "手机")
                                {
                                    if (!dt.Columns.Contains(model[j].Name))
                                    {
                                        dt.Columns.Add(model[j].Name, typeof(String));
                                    }
                                    if (model[j].Type == 4) {
                                        row[model[j].Name] = (model[j].UrlList != null && model[j].UrlList.Count > 0)
                                       ? model[j].UrlList[0].url
                                       : string.Empty;
                                    }
                                    else {
                                        row[model[j].Name] = model[j].value ?? string.Empty;
                                    }
                                   
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogBO _log = new LogBO(typeof(CardBO));
                            string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                            _log.Error(strErrorMsg);
                        }

                        dt.Rows.Add(row);//这样就可以添加了 
                    }

                    string FileName = cBO.DataToExcel(dt, "QuestionnaireSignUpExcel/", QuestionnaireID + ".xls");

                    if (FileName != null)
                    {
                        return new ResultObject() { Flag = 1, Message = "获取成功!", Result = FileName };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "暂无报名!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取推广奖励数据
        /// </summary>
        ///<param name="PageIndex"></param>
        ///<param name="PageCount"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getPromotionAwards"), HttpGet]
        public ResultObject getPromotionAwards(int PageIndex, int PageCount, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            try
            {
                CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
                CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

                if (CustomerVO2.AppType == 3)
                {
                    List<CardOrderViewVO> wy_cVO = cBO.FindOrderViewByCondtion("OneRebateCustomerId=" + customerId + "  and Status=1 and payAt is not NULL");
                    foreach (CardOrderViewVO item in wy_cVO)
                    {
                        item.RebateCost = item.OneRebateCost;
                        item.RebateName = "直推奖";
                    }
                    List<CardOrderViewVO> wy_cVO2 = cBO.FindOrderViewByCondtion("TwoRebateCustomerId=" + customerId + "  and Status=1 and payAt is not NULL");
                    foreach (CardOrderViewVO item in wy_cVO2)
                    {
                        item.RebateCost = item.TwoRebateCost;
                        item.RebateName = "间推奖";
                    }
                    wy_cVO.AddRange(wy_cVO2);
                    wy_cVO.Sort((a, b) => a.CreatedAt.CompareTo(b.payAt));
                    wy_cVO.Reverse();

                    IEnumerable<CardOrderViewVO> wy_newqVO = wy_cVO.Skip((PageIndex - 1) * PageCount).Take(PageCount);

                    foreach (CardOrderViewVO item in wy_newqVO)
                    {
                        List<CardDataVO> uVO = cBO.FindCardByCustomerId(item.CustomerId);
                        if (uVO.Count > 0)
                        {
                            item.CustomerName = uVO[0].Name;
                            item.HeaderLogo = uVO[0].Headimg;
                            item.CorporateName = uVO[0].CorporateName;
                            item.Position = uVO[0].Position;
                        }
                    }

                    decimal wy_VipOneFrozenBalance = cBO.FindOrderSumByCondtion("OneRebateCost", "OneRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL");
                    decimal wy_VipTwoFrozenBalance = cBO.FindOrderSumByCondtion("TwoRebateCost", "TwoRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL");
                    decimal wy_PayoutVipOneFrozenBalance = cBO.FindOrderSumByCondtion("OneRebateCost", "OneRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL and OneRebateStatus=0");
                    decimal wy_PayoutVipTwoFrozenBalance = cBO.FindOrderSumByCondtion("TwoRebateCost", "TwoRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL and TwoRebateStatus=0");

                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = wy_newqVO, Count = wy_cVO.Count, Subsidiary = new { PayoutBalance = wy_PayoutVipOneFrozenBalance + wy_PayoutVipTwoFrozenBalance, AccumulatesBalance = wy_VipOneFrozenBalance + wy_VipTwoFrozenBalance } };
                }

                List<CardOrderViewVO> cVO = cBO.FindOrderViewByCondtion("OneRebateCustomerId=" + customerId + "  and Status=1 and payAt is not NULL");

                List<CardOrderViewVO> cVO2 = cBO.FindOrderViewByCondtion("TwoRebateCustomerId=" + customerId + "  and Status=1 and payAt is not NULL");

                for (int i = 0; i < cVO2.Count; i++)
                {
                    CardOrderViewVO CardOrderViewVO = new CardOrderViewVO();
                    CardOrderViewVO.OneRebateCost = cVO2[i].TwoRebateCost;
                    CardOrderViewVO.Type = cVO2[i].Type;
                    CardOrderViewVO.CreatedAt = cVO2[i].CreatedAt;
                    CardOrderViewVO.CustomerId = cVO2[i].CustomerId;
                    CardOrderViewVO.Cost = cVO2[i].Cost;
                    CardOrderViewVO.CustomerName = cVO2[i].CustomerName;
                    CardOrderViewVO.HeaderLogo = cVO2[i].HeaderLogo;
                    CardOrderViewVO.payAt = cVO2[i].payAt;
                    cVO.Add(CardOrderViewVO);
                }

                List<CardPartyOrderViewVO> FrozenVO2 = cBO.GetPartyOrderViewVO("InviterCID = " + customerId + " and PromotionAwardCost>0 and Status=1 and payAt is not NULL");

                decimal PromotionAwardCost = 0;
                for (int i = 0; i < FrozenVO2.Count; i++)
                {
                    CardOrderViewVO CardOrderViewVO = new CardOrderViewVO();
                    CardOrderViewVO.OneRebateCost = FrozenVO2[i].PromotionAwardCost;
                    CardOrderViewVO.Type = 4;
                    CardOrderViewVO.CreatedAt = FrozenVO2[i].CreatedAt;
                    CardOrderViewVO.CustomerId = FrozenVO2[i].CustomerId;
                    CardOrderViewVO.Cost = FrozenVO2[i].Cost;
                    CardOrderViewVO.CustomerName = FrozenVO2[i].Name;
                    CardOrderViewVO.HeaderLogo = FrozenVO2[i].Headimg;
                    CardOrderViewVO.payAt = FrozenVO2[i].payAt;
                    PromotionAwardCost += FrozenVO2[i].PromotionAwardCost;

                    cVO.Add(CardOrderViewVO);
                }

                cVO.Sort((a, b) => a.CreatedAt.CompareTo(b.payAt));
                cVO.Reverse();

                IEnumerable<CardOrderViewVO> newqVO = cVO.Skip((PageIndex - 1) * PageCount).Take(PageCount);


                foreach (CardOrderViewVO item in newqVO)
                {
                    List<CardDataVO> uVO = cBO.FindCardByCustomerId(item.CustomerId);
                    if (uVO.Count > 0)
                    {
                        item.CustomerName = uVO[0].Name;
                        item.HeaderLogo = uVO[0].Headimg;
                        item.CorporateName = uVO[0].CorporateName;
                        item.Position = uVO[0].Position;
                    }
                }

                decimal VipOneFrozenBalance = cBO.FindOrderSumByCondtion("OneRebateCost", "OneRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL") + PromotionAwardCost;
                decimal VipTwoFrozenBalance = cBO.FindOrderSumByCondtion("TwoRebateCost", "TwoRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL");

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newqVO, Count = cVO.Count, Subsidiary = new { PayoutBalance = cBO.FindCardBalanceByCustomerId(customerId), AccumulatesBalance = VipOneFrozenBalance + VipTwoFrozenBalance } };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }

        }


        /// <summary>
        /// 获取推广奖励金额（微云智推）
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getRebateCost"), HttpGet]
        public ResultObject getRebateCost(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            decimal VipOneBalance = cBO.FindOrderSumByCondtion("OneRebateCost", "OneRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL");
            decimal VipTwoBalance = cBO.FindOrderSumByCondtion("TwoRebateCost", "TwoRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL");

            decimal VipOneFrozenBalance = cBO.FindOrderSumByCondtion("OneRebateCost", "OneRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL and OneRebateStatus=0");
            decimal VipTwoFrozenBalance = cBO.FindOrderSumByCondtion("TwoRebateCost", "TwoRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL and TwoRebateStatus=0");

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { AccumulatedAmount = VipOneBalance + VipTwoBalance, WithdrawalAmount = VipOneFrozenBalance + VipTwoFrozenBalance } };
        }

        /// <summary>
        /// 获取代理商佣金数据（微云智推）
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getAgentBalance"), HttpGet]
        public ResultObject getAgentBalance(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            List<CardAgentVO> AgentListVO = cBO.GetCardAgentByCustomerId(customerId);
            List<string> Region = new List<string>();
            string Headimg = "";
            string Name = "";
            decimal TotalCommission = 0;
            decimal PayableCommission = 0;
            decimal PaidCommission = 0;
            decimal DepositCost = 0;
            decimal AgentCost = 0;

            for (int i = 0; i < AgentListVO.Count; i++)
            {
                CardAgentVO aVO = cBO.FindCardAgentByVO(AgentListVO[i]);
                Region.Add(aVO.Province + "-" + aVO.City);
                Headimg = aVO.Headimg;
                Name = aVO.Name;
                TotalCommission = aVO.TotalCommission;
                PayableCommission = aVO.PayableCommission;
                PaidCommission = aVO.PaidCommission;
                DepositCost = aVO.DepositCost;
                AgentCost = aVO.AgentCost;
            }


            List<CardOrderViewVO> CuListVO = cBO.FindOrderViewByGROUP("AgentCustomerId=" + customerId + " and Status=1 and payAt is not NULL");
            int NumberOfPeople = CuListVO.Count;

            //根据创建时间重新排序

            CuListVO.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));
            CuListVO.Reverse();

            List<CardOrderViewVO> CardList = new List<CardOrderViewVO>();
            for (int i = 0; i < CuListVO.Count && i < 50; i++)
            {
                CardOrderViewVO CardOrder = CuListVO[i];
                List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(CuListVO[i].CustomerId);
                if (CardDataVO.Count > 0)
                {
                    CardOrder.HeaderLogo = CardDataVO[0].Headimg;
                    CardOrder.CustomerName = CardDataVO[0].Name;
                    CardOrder.CorporateName = CardDataVO[0].CorporateName;
                    CardOrder.Position = CardDataVO[0].Position;
                    CardOrder.CardID = CardDataVO[0].CardID;
                }
                CardList.Add(CardOrder);
            }

            return new ResultObject()
            {
                Flag = 1,
                Message = "获取成功!",
                Result = new
                {
                    data = new
                    {
                        Headimg = Headimg,//代理商头像
                        Name = Name,//代理商名称
                        Region = Region,//代理城市列表
                        TotalCommission = TotalCommission,//总佣金
                        PayableCommission = PayableCommission,//应付佣金
                        PaidCommission = PaidCommission,//已付佣金
                        ActualCommission = AgentCost,//实际佣金
                        DepositCost = DepositCost,//佣金预存
                        NumberOfPeople = NumberOfPeople//代理区域总人数
                    },
                    list = CardList//用户列表，只显示最新50人
                }
            };
        }

        /// <summary>
        /// 获取代理商所属用户列表
        /// </summary>
        /// <param name="PageCount"></param>
        /// <param name="PageIndex"></param>
        /// <param name="Type">1:总佣金，2:应付佣金，3：已付佣金，4：实际佣金，5：保证金</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgentOrderList"), HttpGet]
        public ResultObject GetAgentOrderList(int PageCount, int PageIndex, string token, int Type = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

            List<CardAgentVO> AgentListVO = cBO.GetCardAgentByCustomerId(customerId);
            if (AgentListVO.Count <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败,您不是代理商!", Result = null };
            }
            CardAgentVO aVO = cBO.FindCardAgentByVO(AgentListVO[0]);

            string sql = "AgentCustomerId = " + customerId + " and Status=1 and payAt is not NULL";
            List<CardOrderViewVO> CuListVO = new List<CardOrderViewVO>();
            if (Type == 1)
            {
                CuListVO = cBO.FindOrderViewByCondtion("AgentCustomerId=" + customerId + " and Status=1 and payAt is not NULL");
                foreach (CardOrderViewVO item in CuListVO)
                {
                    item.RebateCost = item.AgentCost;
                    item.RebateName = "区域奖";
                }
                List<CardOrderViewVO> CuList2VO = cBO.FindOrderViewByCondtion("OneRebateAgentCustomerId=" + customerId + " and Status=1 and payAt is not NULL");
                foreach (CardOrderViewVO item in CuList2VO)
                {
                    item.RebateCost = item.OneRebateCost;
                    item.RebateName = "直推奖";
                }
                CuListVO.AddRange(CuList2VO);
                List<CardOrderViewVO> CuList3VO = cBO.FindOrderViewByCondtion("TwoRebateAgentCustomerId=" + customerId + " and Status=1 and payAt is not NULL");
                foreach (CardOrderViewVO item in CuList3VO)
                {
                    item.RebateCost = item.TwoRebateCost;
                    item.RebateName = "间推奖";
                }
                CuListVO.AddRange(CuList3VO);
            }
            else if (Type == 2)
            {
                CuListVO = cBO.FindOrderViewByCondtion("OneRebateAgentCustomerId=" + customerId + " and Status=1 and payAt is not NULL and (OneRebateStatus=0 or OneRebateStatus=2)");
                foreach (CardOrderViewVO item in CuListVO)
                {
                    item.RebateCost = item.OneRebateCost;
                    item.RebateName = "直推奖";
                }
                List<CardOrderViewVO> CuList2VO = cBO.FindOrderViewByCondtion("TwoRebateAgentCustomerId=" + customerId + " and Status=1 and payAt is not NULL and (TwoRebateStatus=0 or TwoRebateStatus=2)");
                foreach (CardOrderViewVO item in CuList2VO)
                {
                    item.RebateCost = item.TwoRebateCost;
                    item.RebateName = "间推奖";
                }
                CuListVO.AddRange(CuList2VO);
            }
            else if (Type == 3)
            {
                CuListVO = cBO.FindOrderViewByCondtion("OneRebateAgentCustomerId=" + customerId + " and Status=1 and payAt is not NULL and OneRebateStatus=1");
                foreach (CardOrderViewVO item in CuListVO)
                {
                    item.RebateCost = item.OneRebateCost;
                    item.RebateName = "直推奖";
                }
                List<CardOrderViewVO> CuList2VO = cBO.FindOrderViewByCondtion("TwoRebateAgentCustomerId=" + customerId + " and Status=1 and payAt is not NULL and TwoRebateStatus=1");
                foreach (CardOrderViewVO item in CuList2VO)
                {
                    item.RebateCost = item.TwoRebateCost;
                    item.RebateName = "间推奖";
                }
                CuListVO.AddRange(CuList2VO);
            }
            else if (Type == 4)
            {
                CuListVO = cBO.FindOrderViewByCondtion("AgentCustomerId=" + customerId + " and Status=1 and payAt is not NULL");
                foreach (CardOrderViewVO item in CuListVO)
                {
                    item.RebateCost = item.AgentCost;
                    item.RebateName = "区域奖";
                }
            }
            else if (Type == 5)
            {
                //保证金
                decimal Recharge = cBO.FindAgentDepositSumCost("CustomerId=" + customerId + " and (Type='系统充值' or Type='系统扣除')");
                decimal Withdrawal = cBO.FindAgentDepositSumCost("CustomerId=" + customerId + " and Type='会员提现'");
                decimal DepositCost = aVO.DepositCost;
                decimal PayableCommission = aVO.PayableCommission;

                List<CardAgentDepositVO> adList = cBO.GetCardAgentDepositByCustomerId(customerId);
                //根据创建时间重新排序
                adList.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));
                adList.Reverse();

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = adList.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = adList.Count, Subsidiary = new { Recharge = Recharge, Withdrawal = Withdrawal, DepositCost = DepositCost, PayableCommission = PayableCommission } };
            }

            //根据创建时间重新排序
            CuListVO.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));
            CuListVO.Reverse();

            if (CuListVO != null)
            {
                IEnumerable<CardOrderViewVO> newqVO = CuListVO.Skip((PageIndex - 1) * PageCount).Take(PageCount);

                foreach (CardOrderViewVO item in newqVO)
                {
                    int CustomerId = item.CustomerId;
                    if (item.RebateName == "直推奖")
                    {
                        CustomerId = item.OneRebateCustomerId;
                    }
                    if (item.RebateName == "间推奖")
                    {
                        CustomerId = item.TwoRebateCustomerId;
                    }

                    List<CardDataVO> cVO = cBO.FindCardByCustomerId(CustomerId);
                    if (cVO.Count > 0)
                    {
                        item.HeaderLogo = cVO[0].Headimg;
                        item.CustomerName = cVO[0].Name;
                        item.CorporateName = cVO[0].CorporateName;
                        item.Position = cVO[0].Position;
                        item.CardID = cVO[0].CardID;
                    }
                    else
                    {
                        CustomerVO CVO2 = CustomerBO.FindCustomenById(CustomerId);
                        item.HeaderLogo = CVO2.HeaderLogo;
                        item.CustomerName = CVO2.CustomerName;
                    }
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newqVO, Count = CuListVO.Count, Subsidiary = aVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }




        /// <summary>
        /// 打赏（转账）
        /// </summary>
        /// <returns></returns>
        [Route("AddTransfer"), HttpPost, Anonymous]
        public ResultObject AddTransfer([FromBody] wxUserInfoVO wxUserInfoVO, string code, int ToCustomerId, decimal Cost, int PayType = 1, int AppType = 1)
        {
            return new ResultObject() { Flag = 0, Message = "打赏功能已暂停使用!", Result = null };
            try
            {
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);
                string appid = cBO.appid;
                string secret = cBO.secret;

                if (PayType == 2)
                {
                    //引流王
                    appid = "wxbe6347ce9f00fd0b";
                    secret = "936b0905c776a207174039336a217bcb";
                }

                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var serializer = new DataContractJsonSerializer(typeof(OpenIdAndSessionKey));
                var mStream = new MemoryStream(Encoding.Default.GetBytes(jsonStr));
                OpenIdAndSessionKey readConfig = (OpenIdAndSessionKey)serializer.ReadObject(mStream);
                if (readConfig.unionid != "")
                {
                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    CustomerViewVO customerVO = uBO.FindCustomerByOpenId(readConfig.openid, readConfig.unionid, "2");
                    if (customerVO != null)
                    {
                        //登录成功

                        int oldCount = cBO.FindTransferHistoryTotalCount("CustomerId = " + customerVO.CustomerId + " and NOW()-CreatedAt<60");
                        if (oldCount > 0)
                        {
                            return new ResultObject() { Flag = 0, Message = "您刚刚打赏过一次，请勿频繁重复操作，请稍后!", Result = null };
                        }

                        CardTransferVO pVO = new CardTransferVO();

                        //转账金额
                        pVO.Cost = Cost;
                        //收款会员
                        pVO.ToCustomerId = ToCustomerId;
                        //会员ID
                        pVO.CustomerId = customerVO.CustomerId;
                        //申请时间
                        pVO.TransferDate = DateTime.Now;

                        //转账类型
                        pVO.Type = "活动打赏";

                        if (pVO.Cost <= 0)
                        {
                            return new ResultObject() { Flag = 0, Message = "金额错误!", Result = null };
                        }

                        if (!cBO.IsHasMoreCardBalance(pVO.CustomerId, pVO.Cost))
                        {
                            return new ResultObject() { Flag = 0, Message = "余额不足!", Result = null };
                        }

                        if (!cBO.isLegitimate(pVO.CustomerId))
                        {
                            return new ResultObject() { Flag = 0, Message = "您的账户余额与平台收入不一致，联系客服处理!", Result = null };
                        }

                        int TransferHistoryId = cBO.AddTransferHistory(pVO);
                        if (TransferHistoryId > 0)
                        {
                            cBO.AddCardMessage(customerVO.CustomerName + "给您打赏了" + pVO.Cost + "元！", pVO.ToCustomerId, "活动打赏", "/pages/MyCenter/MyCenter/MyCenter", "switchTab");
                            return new ResultObject() { Flag = 1, Message = "转账成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "转账失败,请重试!", Result = null };
                        }
                    }
                }
                return new ResultObject() { Flag = 0, Message = "转账失败,请重试!", Result = null };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "转账失败,请重试!", Result = null };
            }
        }

        /// <summary>
        /// 上传身份证自动识别
        /// </summary>
        /// <param name="IdCardVO"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateIdCard"), HttpPost]
        public async Task<ResultObject> UpdateIdCard([FromBody] IdCardVO IdCardVO, string token)
        {
            if (IdCardVO == null || IdCardVO.IdCard_A == "" || IdCardVO.IdCard_B == "")
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO cVO = uBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), cVO.AppType);

            if (await cBO.IdCardRecognitionBYTencent(IdCardVO.IdCard_A, 1) && await cBO.IdCardRecognitionBYTencent(IdCardVO.IdCard_B, 2))
            {
                cVO.isIdCard = true;
                cVO.IdCard_A = IdCardVO.IdCard_A;
                cVO.IdCard_B = IdCardVO.IdCard_B;

                //赠送7天会员
                int day = 7;
                //续费
                if (cVO.isVip && cVO.ExpirationAt > DateTime.Now)
                {
                    cVO.ExpirationAt = cVO.ExpirationAt.AddDays(day);
                }
                //开通
                else
                {
                    cVO.ExpirationAt = DateTime.Now.AddDays(day);
                }
                cVO.isVip = true;
                uBO.Update(cVO);
                return new ResultObject() { Flag = 1, Message = "实名认证成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "身份证照片错误!", Result = null };
            }
        }

        /// <summary>
        /// 人脸识别，身份证识别
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateFaceId"), HttpPost]
        public ResultObject UpdateFaceId(string name, string number, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO cVO = uBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), cVO.AppType);

            if (name == "") return new ResultObject() { Flag = 0, Message = "请上传姓名", Result = null };
            if (number == "") return new ResultObject() { Flag = 0, Message = "请上传身份证号码", Result = null };

            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string folder = "/UploadFolder/FaceID/" + DateTime.Now.ToString("yyyyMM") + "/";
            string imgPath = "";
            if (hfc.Count > 0)
            {
                try
                {
                    FileInfo fi = new FileInfo(hfc[0].FileName);
                    string ext = fi.Extension.ToLower();
                    if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
                    {
                        return new ResultObject() { Flag = 0, Message = "文件类型错误", Result = null };
                    }
                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fi.Extension;
                    //可以修改为网络路径
                    string localPath = ConfigInfo.Instance.UploadFolder + folder;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string PhysicalPath = localPath + newFileName;
                    hfc[0].SaveAs(PhysicalPath);

                    imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;

                    //人脸识别
                    EntityVO eVO = TencentFaceIdEntity.Main(PhysicalPath, name, number);

                    if (eVO.error_code == 0)
                    {

                        //-1,身份证和姓名不一致 
                        //-2,公安库中无此身份证记录 
                        //-3,公安身份证库中没有此号码的照片
                        //-4 照片参数不合格 
                        //-5 照片相片体积过大 
                        //-6,请检查图片编码 
                        //-7,照片相片体积过小 
                        //1,系统分析为同一人 ，
                        //2,系统分析可能为同一人 
                        //3, 系统分析为不是同人 
                        //4,没检测到人脸 
                        //5,疑似非活体 
                        //6,出现多张脸 
                        //7,身份证和姓名一致，官方人脸比对失败
                        if (eVO.result.Validate_Result == 1 || eVO.result.Validate_Result == 2)
                        {
                            cVO.isIdCard = true;
                            cVO.FaceIdImg = imgPath;
                            cVO.IdCardName = name;
                            cVO.IdCardNumber = number;
                            uBO.Update(cVO);
                            return new ResultObject() { Flag = 1, Message = "实名认证成功!", Result = null };
                        }
                        else if (eVO.result.Validate_Result == -1)
                        {
                            return new ResultObject() { Flag = 0, Message = "身份证和姓名不一致", Result = null };
                        }
                        else if (eVO.result.Validate_Result == -2)
                        {
                            return new ResultObject() { Flag = 0, Message = "公安库中无此身份证记录", Result = null };
                        }
                        else if (eVO.result.Validate_Result == -3)
                        {
                            return new ResultObject() { Flag = 0, Message = "公安身份证库中没有此号码的照片", Result = null };
                        }
                        else if (eVO.result.Validate_Result == -4)
                        {
                            return new ResultObject() { Flag = 0, Message = "照片参数不合格", Result = null };
                        }
                        else if (eVO.result.Validate_Result == -5)
                        {
                            return new ResultObject() { Flag = 0, Message = "照片相片体积过大", Result = null };
                        }
                        else if (eVO.result.Validate_Result == -6)
                        {
                            return new ResultObject() { Flag = 0, Message = "图片编码错误", Result = null };
                        }
                        else if (eVO.result.Validate_Result == -7)
                        {
                            return new ResultObject() { Flag = 0, Message = "照片相片体积过小", Result = null };
                        }
                        else if (eVO.result.Validate_Result == 3)
                        {
                            return new ResultObject() { Flag = 0, Message = "身份证和照片不是同一个人", Result = null };
                        }
                        else if (eVO.result.Validate_Result == 4)
                        {
                            return new ResultObject() { Flag = 0, Message = "没检测到人脸", Result = null };
                        }
                        else if (eVO.result.Validate_Result == 5)
                        {
                            return new ResultObject() { Flag = 0, Message = "疑似非活体，请拍摄清晰可见的脸部照片", Result = null };
                        }
                        else if (eVO.result.Validate_Result == 6)
                        {
                            return new ResultObject() { Flag = 0, Message = "出现多张脸", Result = null };
                        }
                        return new ResultObject() { Flag = 0, Message = "人脸比对失败", Result = null };
                    }
                    else if (eVO.error_code == 10028) {
                        return new ResultObject() { Flag = 0, Message = "证件位数不是18位", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "公安系统维护中，请稍后再认证!", Result = eVO };
                    }
                }
                catch (Exception er)
                {
                    return new ResultObject() { Flag = 0, Message = "上传失败", Result = er.ToString() };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "上传失败", Result = "文件为空" };
            }
        }


        /// <summary>
        /// 获取商机分类列表，匿名
        /// </summary>
        /// <returns></returns>
        [Route("GetCategory"), HttpGet, Anonymous]
        public ResultObject GetDemandSite()
        {
            DemandBO uBO = new DemandBO(new CustomerProfile());
            List<DemandCategoryVO> uVO = uBO.FindCategory();
            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新商机
        /// </summary>
        /// <param name="demandVO">需求VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateDemand"), HttpPost]
        public ResultObject UpdateDemand([FromBody] DemandVO demandVO, string token)
        {
            if (demandVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            DemandBO uBO = new DemandBO(new CustomerProfile());

            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            if (demandVO.DemandId < 1)
            {
                demandVO.CreatedAt = DateTime.Now;
                demandVO.Status = 1;
                demandVO.CustomerId = customerId;
                List<CardDataVO> cVO = cBO.FindCardByCustomerId(customerId);
                if (cVO.Count > 0)
                {
                    demandVO.Name = cVO[0].Name;
                    demandVO.Headimg = cVO[0].Headimg;
                }
                else
                {
                    CustomerVO cuVO = cuBO.FindCustomenById(customerId);
                    demandVO.Name = cuVO.CustomerName;
                    demandVO.Headimg = cuVO.HeaderLogo;
                }

                try
                {
                    string jsonstr = HttpHelper.HtmlFromUrlGet("http://apis.map.qq.com/ws/location/v1/ip?key=QHBBZ-ZDAWI-TRVGE-5GWEO-PHEKO-Q5BXC&ip=" + HttpContext.Current.Request.UserHostAddress);
                    JObject jo = (JObject)JsonConvert.DeserializeObject(jsonstr);
                    demandVO.Province = jo["result"]["ad_info"]["province"].ToString();
                    demandVO.City = jo["result"]["ad_info"]["city"].ToString();
                }
                catch
                {

                }

                int demandId = uBO.AddDemand(demandVO);
                if (demandId > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = demandId };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                demandVO.CreatedAt = DateTime.Now;
                bool isSuccess = uBO.UpdateDemand(demandVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = demandVO.DemandId };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加商机留言
        /// </summary>
        /// <param name="demandofferVO">留言VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddDemandOffer"), HttpPost]
        public ResultObject AddDemandOffer([FromBody] DemandOfferVO demandofferVO, string token)
        {

            if (demandofferVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            DemandBO uBO = new DemandBO(new CustomerProfile());
            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            if (demandofferVO.OfferId < 1)
            {
                demandofferVO.CreatedAt = DateTime.Now;
                demandofferVO.CustomerId = customerId;

                List<CardDataVO> cVO = cBO.FindCardByCustomerId(customerId);
                if (cVO.Count > 0)
                {
                    demandofferVO.Name = cVO[0].Name;
                    demandofferVO.Headimg = cVO[0].Headimg;
                }
                else
                {
                    CustomerVO cuVO = cuBO.FindCustomenById(customerId);
                    demandofferVO.Name = cuVO.CustomerName;
                    demandofferVO.Headimg = cuVO.HeaderLogo;
                }

                //避免重复提交的情况，1分钟内不得添加两次
                int oldCount = uBO.FindDemandOfferCount("CustomerId = " + customerId + " and DemandId=" + demandofferVO.DemandId + " and NOW()-CreatedAt<60");
                if (oldCount > 0)
                {
                    return new ResultObject() { Flag = 0, Message = "你在刚刚已经提交了该需求的留言，请1分钟后再提交!", Result = null };
                }
                int demandId = uBO.AddDemandOffer(demandofferVO);
                if (demandId > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = demandId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
        }

        /// <summary>
        /// 获取商机留言列表
        /// </summary>
        /// <param name="DemandId">DemandId</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetOfferByDemand"), HttpGet, Anonymous]
        public ResultObject GetOfferByDemand(int DemandId, int PageCount, int PageIndex, int AppType = 1)
        {
            DemandBO uBO = new DemandBO(new CustomerProfile());
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            List<DemandOfferVO> tenderInviteList = uBO.FindOfferByDemand(DemandId);
            if (tenderInviteList != null)
            {
                IEnumerable<DemandOfferVO> newqVO = tenderInviteList.Skip((PageIndex - 1) * PageCount).Take(PageCount);

                foreach (DemandOfferVO item in newqVO)
                {
                    List<CardDataVO> cVO = cBO.FindCardByCustomerId(item.CustomerId);
                    if (cVO.Count > 0)
                    {
                        item.CardID = cVO[0].CardID;
                    }
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = newqVO, Count = tenderInviteList.Count };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取发布的商机列表，匿名
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetDemandList"), HttpPost, Anonymous]
        public ResultObject GetDemandList([FromBody] ConditionModel condition)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            //只显示发布和暂停投标的。
            //做一下修改，显示所有非保存并未过期的需求
            string conditionStr = " Status <> 0 and Status <> 2 and (TO_DAYS(EffectiveEndDate) - TO_DAYS(now()) >=0 or isEndDate=0) and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            DemandBO uBO = new DemandBO(new CustomerProfile());

            try
            {
                List<CardDemandViewVO> list = uBO.FindCardDemandViewAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
                int count = uBO.FindCardDemandViewCount(conditionStr);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
            }
            catch (Exception ex)
            {
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = strErrorMsg };
            }

        }

        /// <summary>
        /// 获取我的商机列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyDemandlist"), HttpGet]
        public ResultObject getMyDemandlist(int PageCount, int PageIndex, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            DemandBO uBO = new DemandBO(new CustomerProfile());

            List<CardDemandViewVO> uVO = uBO.FindCardDemandViewByCustomerId(customerId);
            uVO.Reverse();
            if (uVO != null)
            {

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = uVO.Count };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取指定会员的商机列表
        /// </summary>
        /// <returns></returns>
        [Route("getDemandlistByCustomerId"), HttpGet, Anonymous]
        public ResultObject getDemandlistByCustomerId(int PageCount, int PageIndex, int CustomerId, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            DemandBO uBO = new DemandBO(new CustomerProfile());

            List<CardDemandViewVO> uVO = uBO.FindCardDemandViewByCustomerId2(CustomerId);
            uVO.Reverse();
            if (uVO != null)
            {

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = uVO.Count };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我留言的商机列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyOfferDemandlist"), HttpGet]
        public ResultObject getMyOfferDemandlist(int PageCount, int PageIndex, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);
            DemandBO uBO = new DemandBO(new CustomerProfile());

            List<CardOfferDemandViewVO> uVO = uBO.FindCardDemandViewByOfferCustomerId(customerId);

            if (uVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = uVO.Count };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取商机详情，匿名
        /// </summary>
        /// <param name="DemandId">需求ID</param>
        /// <returns></returns>
        [Route("GetDemandSite"), HttpGet, Anonymous]
        public ResultObject GetDemandSite(int DemandId)
        {
            DemandBO uBO = new DemandBO(new CustomerProfile());
            CardDemandViewVO uVO = uBO.FindCardDemandViewById(DemandId);
            if (uVO != null)
            {
                DemandVO demandVO = new DemandVO();
                demandVO.DemandId = uVO.DemandId;
                demandVO.ReadCount = uVO.ReadCount + 1;
                uBO.UpdateDemand(demandVO);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }


        /// <summary>
        /// 添加或更新软文
        /// </summary>
        /// <param name="CardSoftArticleVO">软文VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateSoftArticle"), HttpPost]
        public ResultObject UpdateSoftArticle([FromBody] CardSoftArticleVO CardSoftArticleVO, string token)
        {
            if (CardSoftArticleVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            /*审核文本是否合法*/
            if (!cBO.msg_sec_check(CardSoftArticleVO))
            {
                return new ResultObject() { Flag = 0, Message = "文章有政治敏感或违法关键词，请重新填写!", Result = null };
            }
            /*审核文本是否合法*/

            if (CardSoftArticleVO.SoftArticleID > 0)
            {
                CardSoftArticleVO cVO = cBO.FindSoftArticleById(CardSoftArticleVO.SoftArticleID);
                if (cVO.CustomerId != customerId)
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }

                CardSoftArticleVO.ReadCount = cVO.ReadCount;
                CardSoftArticleVO.ReprintCount = cVO.ReprintCount;
                CardSoftArticleVO.OriginalCustomerId = cVO.OriginalCustomerId;

                if (cBO.UpdateSoftArticle(CardSoftArticleVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardSoftArticleVO.SoftArticleID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                CardSoftArticleVO.CreatedAt = DateTime.Now;
                CardSoftArticleVO.CustomerId = customerId;
                CardSoftArticleVO.OriginalCustomerId = customerId;
                CardSoftArticleVO.ReadCount = 0;
                CardSoftArticleVO.ReprintCount = 0;

                int SoftArticleID = cBO.AddSoftArticle(CardSoftArticleVO);
                if (SoftArticleID > 0)
                {

                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = SoftArticleID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 转载文章更新
        /// </summary>
        /// <param name="SoftArticleID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateSoftArticleByOriginal"), HttpGet]
        public ResultObject UpdateSoftArticleByOriginal(int SoftArticleID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            CardSoftArticleVO cVO = cBO.FindSoftArticleById(SoftArticleID);
            if (cVO.CustomerId != customerId)
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }

            if (cVO.OriginalSoftArticleID > 0)
            {
                CardSoftArticleVO ocVO = cBO.FindSoftArticleById(cVO.OriginalSoftArticleID);
                cVO.Description = ocVO.Description;
            }

            if (cBO.UpdateSoftArticle(cVO))
            {
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = cVO.SoftArticleID };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

        /// <summary>
        /// 更新软文绑定名片id
        /// </summary>
        /// <param name="CardID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateSoftArticleCardID"), HttpPost]
        public ResultObject UpdateSoftArticleCardID(int CardID, int SoftArticleID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            CardSoftArticleVO cVO = cBO.FindSoftArticleById(SoftArticleID);
            if (cVO.CustomerId != customerId)
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }
            cVO.CardID = CardID;

            if (cBO.UpdateSoftArticle(cVO))
            {
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = cVO.SoftArticleID };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取软文详情，匿名
        /// </summary>
        /// <param name="SoftArticleID">软文ID</param>
        /// <returns></returns>
        [Route("GetSoftArticleSite"), HttpGet, Anonymous]
        public ResultObject GetSoftArticleSite(int SoftArticleID, int AppType = 1, int isNext = 0)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardSoftArticleVO cVO = cBO.FindSoftArticleById(SoftArticleID);
            if (cVO != null)
            {
                CardSoftArticleVO CardSoftArticleVO = new CardSoftArticleVO();
                CardSoftArticleVO.SoftArticleID = cVO.SoftArticleID;
                CardSoftArticleVO.ExposureCount = cVO.ExposureCount + 3;

                //原文是否有更新
                bool isUP = false;

                if (cVO.OriginalSoftArticleID > 0)
                {
                    CardSoftArticleVO ocVO = cBO.FindSoftArticleById(cVO.OriginalSoftArticleID);

                    ocVO.ReadCount += 1;
                    cBO.UpdateSoftArticle(ocVO);

                    if (ocVO.Status == 2 && AppType == 3)
                    {
                        ocVO.CustomerId = 47718;
                        ocVO.CardID = 17343;
                    }

                    cVO.ReadCount = ocVO.ReadCount;
                    cVO.ReprintCount = ocVO.ReprintCount;
                    cVO.GoodCount = ocVO.GoodCount;

                    if (ocVO.CardID > 0)
                    {

                        CardDataVO dVO = cBO.FindCardById(ocVO.CardID);
                        if (dVO != null)
                        {
                            cVO.OriginalCard = dVO;
                        }
                        else
                        {
                            List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(ocVO.CustomerId);
                            if (CardDataVO.Count > 0)
                            {
                                cVO.OriginalCard = CardDataVO[0];
                            }
                        }
                    }
                    else
                    {
                        List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(ocVO.CustomerId);
                        if (CardDataVO.Count > 0)
                        {
                            cVO.OriginalCard = CardDataVO[0];
                        }
                    }

                    if (ocVO.Description != cVO.Description)
                    {
                        isUP = true;
                    }
                }
                else
                {
                    CardSoftArticleVO.ReadCount += cVO.ReadCount + 1;
                }

                cBO.UpdateSoftArticle(CardSoftArticleVO);


                if (cVO.Status == 2 && AppType == 3)
                {
                    cVO.OriginalCustomerId = 47718;
                    cVO.CardID = 17343;
                }

                if (cVO.CardID > 0)
                {
                    CardDataVO dVO = cBO.FindCardById(cVO.CardID);
                    if (dVO != null)
                    {
                        cVO.Card = dVO;
                    }
                    else
                    {
                        List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(cVO.CustomerId);
                        if (CardDataVO.Count > 0)
                        {
                            cVO.Card = CardDataVO[0];
                        }
                    }
                }
                else
                {
                    List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(cVO.CustomerId);
                    if (CardDataVO.Count > 0)
                    {
                        cVO.Card = CardDataVO[0];
                    }
                }

                if (cVO.QRImg == "")
                {
                    cVO.QRImg = cBO.GetSoftArticleQR(cVO.SoftArticleID);
                }

                BusinessCardBO BusinessCardBO = new BusinessCardBO(new CustomerProfile());
                BusinessCardVO bVO = new BusinessCardVO();
                if (cVO.BusinessID != 0)
                {
                    bVO = BusinessCardBO.FindBusinessCardById(cVO.BusinessID);
                }
                ResultObject ro = new ResultObject();
                if (cVO.isVideo == 1 && isNext == 0)
                {
                    List<CardSoftArticleVO> svoList = cBO.FindSoftArticleByConditionStr("CustomerId=" + cVO.CustomerId + " and Status=1 and isVideo=1 and SoftArticleID<" + cVO.SoftArticleID + " order by SoftArticleID desc  limit 1");
                    if (svoList.Count > 0)
                    {
                        ro = GetSoftArticleSite(svoList[0].SoftArticleID, AppType, 1);
                    }
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO, Subsidiary = isUP, Count = bVO, Subsidiary2 = ro };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 软文点赞，匿名
        /// </summary>
        /// <param name="SoftArticleID">软文ID</param>
        /// <returns></returns>
        [Route("GoodSoftArticleSite"), HttpGet, Anonymous]
        public ResultObject GoodSoftArticleSite(int SoftArticleID, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardSoftArticleVO cVO = cBO.FindSoftArticleById(SoftArticleID);
            if (cVO.OriginalSoftArticleID > 0)
            {
                cVO = cBO.FindSoftArticleById(cVO.OriginalSoftArticleID);
            }

            if (cVO != null)
            {
                CardSoftArticleVO CardSoftArticleVO = new CardSoftArticleVO();
                CardSoftArticleVO.SoftArticleID = cVO.SoftArticleID;
                CardSoftArticleVO.GoodCount = cVO.GoodCount + 1;
                cBO.UpdateSoftArticle(CardSoftArticleVO);

                return new ResultObject() { Flag = 1, Message = "点赞成功!", Result = CardSoftArticleVO.GoodCount };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "点赞失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除软文(后台专用)
        /// </summary>
        /// <param name="SoftArticleID">软文ID</param>
        /// <returns></returns>
        [Route("delSoftArticle"), HttpGet]
        public ResultObject delSoftArticle(int SoftArticleID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId > 0)
            {
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);

                CardSoftArticleVO cVO = cBO.FindSoftArticleById(SoftArticleID);
                if (cVO.OriginalSoftArticleID > 0)
                {
                    cVO = cBO.FindSoftArticleById(cVO.OriginalSoftArticleID);
                }
                cVO.Status = -1;
                cBO.UpdateSoftArticle(cVO);

                List<CardSoftArticleVO> List = cBO.FindSoftArticleByConditionStr(" OriginalSoftArticleID =" + cVO.SoftArticleID);

                for (int i = 0; i < List.Count; i++)
                {
                    List[i].Status = -1;
                    cBO.UpdateSoftArticle(List[i]);
                }

                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = cVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除软文
        /// </summary>
        /// <param name="SoftArticleID">软文ID</param>
        /// <returns></returns>
        [Route("CustomerDelSoftArticle"), HttpGet]
        public ResultObject CustomerDelSoftArticle(int SoftArticleID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            if (customerId > 0)
            {
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);

                CardSoftArticleVO cVO = cBO.FindSoftArticleById(SoftArticleID);
                if (cVO.CustomerId != customerId)
                {
                    return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
                }
                cVO.Status = -1;
                cBO.UpdateSoftArticle(cVO);

                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = cVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 修改软文浏览量(后台专用)
        /// </summary>
        /// <param name="SoftArticleID">软文ID</param>
        /// <returns></returns>
        [Route("SoftArticleEditReadCount"), HttpGet]
        public ResultObject SoftArticleEditReadCount(int SoftArticleID, int ReadCount, int ReprintCount, int GoodCount, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId > 0)
            {
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);
                CardSoftArticleVO cVO = cBO.FindSoftArticleById(SoftArticleID);
                cVO.ReadCount = ReadCount;
                cVO.ReprintCount = ReprintCount;
                cVO.GoodCount = GoodCount;
                cBO.UpdateSoftArticle(cVO);
                return new ResultObject() { Flag = 1, Message = "修改成功!", Result = cVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取发布的软文列表，匿名
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetSoftArticleList"), HttpPost, Anonymous]
        public ResultObject GetSoftArticleList([FromBody] ConditionModel condition, int isHeadlines = 1, int AppType = 1)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            //只显示发布和暂停投标的。
            //做一下修改，显示所有非保存并未过期的需求

            Paging pageInfo = condition.PageInfo;

            string conditionStr = "";
            if (AppType == 3)
            {
                if (isHeadlines == 0)
                {
                    conditionStr = " (Status = 1 or Status = 2)  and CustomerId = OriginalCustomerId and ( AppType=1 or AppType=3) and " + condition.Filter.Result();
                }
                else
                {
                    conditionStr = " Status = 2  and CustomerId = OriginalCustomerId  and ( AppType=1 or AppType=3) and " + condition.Filter.Result();
                }
            }
            else
            {
                if (isHeadlines == 0)
                {
                    conditionStr = " Status = 1  and CustomerId = OriginalCustomerId and AppType=1 and " + condition.Filter.Result();
                }
                else
                {
                    conditionStr = " Status = 2  and CustomerId = OriginalCustomerId and AppType=1 and " + condition.Filter.Result();
                }
            }

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            try
            {
                List<CardSoftArticleVO> list = cBO.FindSoftArticleAllToNotType(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Status == 2 && AppType == 3)
                    {
                        list[i].CustomerId = 47718;
                        list[i].CardID = 17343;
                    }
                    if (list[i].OriginalSoftArticleID > 0)
                    {
                        CardSoftArticleVO ocVO = cBO.FindSoftArticleById(list[i].OriginalSoftArticleID);

                        list[i].ReadCount = ocVO.ReadCount;
                        list[i].ReprintCount = ocVO.ReprintCount;
                        list[i].GoodCount = ocVO.GoodCount;
                    }

                    if (list[i].CardID > 0)
                    {
                        list[i].Card = cBO.FindCardById(list[i].CardID);
                    }
                    else
                    {
                        List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(list[i].CustomerId);
                        if (CardDataVO.Count > 0)
                        {
                            list[i].Card = CardDataVO[0];
                        }
                    }

                    if (list[i].CustomerId != list[i].OriginalCustomerId)
                    {
                        List<CardDataVO> CardDataVO2 = cBO.FindCardByCustomerId(list[i].OriginalCustomerId);
                        if (CardDataVO2.Count > 0)
                        {
                            list[i].OriginalCard = CardDataVO2[0];
                        }
                    }
                }

                int count = cBO.FindSoftArticleTotalCountToNotType(conditionStr);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
            }
            catch (Exception ex)
            {
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = strErrorMsg };
            }
        }

        /// <summary>
        /// 获取我的软文列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMySoftArticleList"), HttpGet]
        public ResultObject getMySoftArticleList(int PageCount, int PageIndex, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            DemandBO uBO = new DemandBO(new CustomerProfile());

            string conditionStr = " Status <> -1  and  Status <> 2  and  OriginalCustomerId = " + customerId + " and CustomerId = " + customerId + " ";

            List<CardSoftArticleVO> list = cBO.FindSoftArticleAllByPageIndex(conditionStr, (PageIndex - 1) * PageCount + 1, PageIndex * PageCount, "CreatedAt", "desc");

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].CardID > 0)
                {

                    CardDataVO dVO = cBO.FindCardById(list[i].CardID);
                    if (dVO != null)
                    {
                        list[i].Card = dVO;
                    }
                    else
                    {
                        List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(list[i].CustomerId);
                        if (CardDataVO.Count > 0)
                        {
                            list[i].Card = CardDataVO[0];
                        }
                    }
                }
                else
                {
                    List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(list[i].CustomerId);
                    if (CardDataVO.Count > 0)
                    {
                        list[i].Card = CardDataVO[0];
                    }
                }


                if (list[i].CustomerId != list[i].OriginalCustomerId)
                {
                    List<CardDataVO> CardDataVO2 = cBO.FindCardByCustomerId(list[i].OriginalCustomerId);
                    if (CardDataVO2.Count > 0)
                    {
                        list[i].OriginalCard = CardDataVO2[0];
                    }
                }
            }

            int count = cBO.FindSoftArticleTotalCount(conditionStr);
            if (list != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取指定会员的软文列表
        /// </summary>
        /// <param name="CustomerId">会员id</param>
        /// <returns></returns>
        [Route("getSoftArticleListByCustomerId"), HttpGet, Anonymous]
        public ResultObject getSoftArticleListByCustomerId(int PageCount, int PageIndex, int CustomerId, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            DemandBO uBO = new DemandBO(new CustomerProfile());

            string conditionStr = " Status  > 0  and Status  <> 2  and CustomerId = " + CustomerId + " ";
            List<CardSoftArticleVO> list = cBO.FindSoftArticleAllByPageIndex(conditionStr, (PageIndex - 1) * PageCount + 1, PageIndex * PageCount, "CreatedAt", "desc");

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].CardID > 0)
                {
                    CardDataVO dVO = cBO.FindCardById(list[i].CardID);
                    if (dVO != null)
                    {
                        list[i].Card = dVO;
                    }
                    else
                    {
                        List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(list[i].CustomerId);
                        if (CardDataVO.Count > 0)
                        {
                            list[i].Card = CardDataVO[0];
                        }
                    }
                }
                else
                {
                    List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(list[i].CustomerId);
                    if (CardDataVO.Count > 0)
                    {
                        list[i].Card = CardDataVO[0];
                    }
                }

                if (list[i].CustomerId != list[i].OriginalCustomerId)
                {
                    List<CardDataVO> CardDataVO2 = cBO.FindCardByCustomerId(list[i].OriginalCustomerId);
                    if (CardDataVO2.Count > 0)
                    {
                        list[i].OriginalCard = CardDataVO2[0];
                    }
                }
            }

            int count = cBO.FindSoftArticleTotalCount(conditionStr);
            if (list != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我转载的软文列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyReprintSoftArticleList"), HttpGet]
        public ResultObject getMyReprintSoftArticleList(int PageCount, int PageIndex, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            DemandBO uBO = new DemandBO(new CustomerProfile());

            string conditionStr = " Status = 1  and  OriginalCustomerId <> " + customerId + " and CustomerId = " + customerId + " ";

            List<CardSoftArticleVO> list = cBO.FindSoftArticleAllByPageIndex(conditionStr, (PageIndex - 1) * PageCount + 1, PageIndex * PageCount, "CreatedAt", "desc");

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].OriginalSoftArticleID > 0)
                {
                    CardSoftArticleVO ocVO = cBO.FindSoftArticleById(list[i].OriginalSoftArticleID);
                    list[i].ReadCount = ocVO.ReadCount;
                    list[i].ReprintCount = ocVO.ReprintCount;
                    list[i].GoodCount = ocVO.GoodCount;
                }

                if (list[i].CardID > 0)
                {
                    CardDataVO dVO = cBO.FindCardById(list[i].CardID);
                    if (dVO != null)
                    {
                        list[i].Card = dVO;
                    }
                    else
                    {
                        List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(list[i].CustomerId);
                        if (CardDataVO.Count > 0)
                        {
                            list[i].Card = CardDataVO[0];
                        }
                    }
                }
                else
                {
                    List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(list[i].CustomerId);
                    if (CardDataVO.Count > 0)
                    {
                        list[i].Card = CardDataVO[0];
                    }
                }

                if (list[i].CustomerId != list[i].OriginalCustomerId)
                {
                    List<CardDataVO> CardDataVO2 = cBO.FindCardByCustomerId(list[i].OriginalCustomerId);
                    if (CardDataVO2.Count > 0)
                    {
                        list[i].OriginalCard = CardDataVO2[0];
                    }
                }


            }

            int count = cBO.FindSoftArticleTotalCount(conditionStr);
            if (list != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取软文二维码
        /// </summary>
        /// <param name="SoftArticleID">分享路径</param>
        /// <returns></returns>
        [Route("GetSoftArticleQR"), HttpGet, Anonymous]
        public ResultObject GetSoftArticleQR(int SoftArticleID, int AppType = 1)
        {
            try
            {
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);
                string str = cBO.getQRIMGByIDAndType(SoftArticleID, 6);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = str };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取会员推广海报
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("GetCardPoster"), HttpGet]
        public ResultObject GetCardPoster(string token, int AppType)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            try
            {
                CustomerBO cuBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
                if (AppType == 1)
                {
                    AppType = CustomerVO.AppType;
                }
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);

                string str = cBO.GetCardPoster(customerId);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = str };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取会员推广Vip海报
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("GetCardVipPoster"), HttpGet]
        public ResultObject GetCardVipPoster(string token, int AppType)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            try
            {
                CustomerBO cuBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
                if (AppType == 1)
                {
                    AppType = CustomerVO.AppType;
                }
                CardBO cBO = new CardBO(new CustomerProfile(), AppType);

                string str = cBO.GetCardVipPoster(customerId);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = str };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }



        /// <summary>
        /// 获取微信支付数据,转载软文
        /// </summary>
        /// <returns></returns>
        [Route("ReprintSoftArticle"), HttpGet]
        public ResultObject ReprintSoftArticle(int SoftArticleID, string code, string token, int PayType = 1, int AppType = 1, int isH5 = 0)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO cuVO = uBO.FindCustomenById(customerId);
            AppType = cuVO.AppType;
            if (PayType != 1)
            {
                AppType = PayType;
            }

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            string appid = cBO.appid;
            if (PayType == 2)
            {
                appid = "wxbe6347ce9f00fd0b";
            }
            CardSoftArticleVO cVO = cBO.FindSoftArticleById(SoftArticleID);
            if (cVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }
            if (cVO.OriginalSoftArticleID > 0)
            {
                cVO = cBO.FindSoftArticleById(cVO.OriginalSoftArticleID);
            }

            string OpenId = "";

            if (isH5 == 1)
            {
                ////获取微信的用户信息
                CustomerController customerCon = new CustomerController();
                ResultObject result = customerCon.GetThirdPartUserInfo("2", code, "", "2");
                WeiXinHelper.WeiXinUserInfoResult userInfo = result.Result as WeiXinHelper.WeiXinUserInfoResult;
                //用户信息（判断是否已经获取到用户的微信用户信息）
                if (userInfo.Result && userInfo.UserInfo.openid == "")
                {
                    return new ResultObject() { Flag = 0, Message = "失败", Result = null };
                }

                OpenId = userInfo.UserInfo.openid;
            }
            else
            {
                OpenId = cBO.getOpenId(code);
            }

            int OrderID = 0;
            try
            {
                CardSoftArticleOrderVO oVO = new CardSoftArticleOrderVO();
                oVO.CustomerId = customerId;
                oVO.OpenId = OpenId;
                oVO.CreatedAt = DateTime.Now;
                oVO.Status = 0;
                oVO.Cost = cVO.Cost;
                oVO.SoftArticleID = cVO.SoftArticleID;
                oVO.SoftArticleOrderID = 0;

                Random ran = new Random();
                oVO.OrderNO = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);
                OrderID = cBO.AddSoftArticleOrder(oVO);


                if (OrderID > 0)
                {


                    CardSoftArticleOrderVO OrderVO = cBO.FindSoftArticleOrderById(OrderID);
                    if (OrderVO.Cost == 0 || !cVO.IsCost)
                    {
                        //修改订单
                        CardSoftArticleOrderVO cpVO = new CardSoftArticleOrderVO();
                        cpVO.SoftArticleOrderID = OrderVO.SoftArticleOrderID;
                        cpVO.Status = 1;
                        cpVO.payAt = DateTime.Now;
                        cBO.UpdateCardSoftArticleOrder(cpVO);

                        //转载文章
                        CardSoftArticleVO sVO = cBO.FindSoftArticleById(OrderVO.SoftArticleID);

                        if (sVO.Status == 2 && AppType == 3)
                        {
                            sVO.OriginalCustomerId = 47718;
                        }

                        sVO.ReprintCount += 1;
                        cBO.UpdateSoftArticle(sVO);

                        CardSoftArticleVO SoftArticleVO = new CardSoftArticleVO();
                        SoftArticleVO.SoftArticleID = 0;
                        SoftArticleVO.CustomerId = OrderVO.CustomerId;
                        SoftArticleVO.CreatedAt = DateTime.Now;
                        SoftArticleVO.Title = sVO.Title;
                        SoftArticleVO.Image = sVO.Image;
                        SoftArticleVO.Description = sVO.Description;
                        SoftArticleVO.IsCost = sVO.IsCost;
                        SoftArticleVO.Cost = sVO.Cost;
                        SoftArticleVO.PartyID = sVO.PartyID;
                        SoftArticleVO.OriginalCustomerId = sVO.OriginalCustomerId;
                        SoftArticleVO.OriginalSoftArticleID = OrderVO.SoftArticleID;
                        SoftArticleVO.IsOriginal = sVO.IsOriginal;
                        SoftArticleVO.OriginalName = sVO.OriginalName;
                        SoftArticleVO.OriginalPlatform = sVO.OriginalPlatform;
                        SoftArticleVO.OriginalMedia = sVO.OriginalMedia;
                        SoftArticleVO.Video = sVO.Video;
                        SoftArticleVO.isVideo = sVO.isVideo;

                        int NewSoftArticleID = cBO.AddSoftArticle(SoftArticleVO);

                        cBO.AddCardMessage(cuVO.CustomerName + "转载了文章《" + SoftArticleVO.Title + "》", SoftArticleVO.OriginalCustomerId, "软文转载", "/package/package_order/HostOrderList/HostOrderList");

                        return new ResultObject() { Flag = 5, Message = "0元报名转载", Result = NewSoftArticleID };
                    }
                    JsApiPay Ja = new JsApiPay();

                    if (OrderVO.Cost <= 0)
                    {
                        return new ResultObject() { Flag = 0, Message = "价格错误", Result = null };
                    }
                    string total_fee_1 = Convert.ToInt32((OrderVO.Cost * 100)).ToString();
                    string NOTIFY_URL = "http://api.leliaomp.com/Pay/SoftArticle_Notify_Url.aspx";

                    String costName = "软文付费转载";

                    string body = "乐聊名片软文付费转载";

                    if (isH5 == 1)
                    {
                        appid = cBO.appidH5;
                    }


                    WxPayData wp = Ja.GetUnifiedOrderResult(appid, OrderVO.OrderNO, OrderVO.OpenId, total_fee_1, body, costName, body, NOTIFY_URL, AppType);


                    if (wp != null)
                    {
                        string reslut = Ja.GetJsApiParameters(wp, AppType);
                        if (reslut != "")
                        {
                            return new ResultObject() { Flag = 1, Message = "成功", Result = reslut, Subsidiary = OrderVO.SoftArticleOrderID };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "失败", Result = null };
                        }
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "失败", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "操作失败，请重试!", Result = null };
                }

            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
            }
        }

        /// <summary>
        /// 通过软文订单id获取新文章id，匿名
        /// </summary>
        /// <param name="SoftArticleOrderID">软文ID</param>
        /// <returns></returns>
        [Route("getSoftArticleID"), HttpGet, Anonymous]
        public ResultObject getSoftArticleID(int SoftArticleOrderID, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardSoftArticleOrderVO OrderVO = cBO.FindSoftArticleOrderById(SoftArticleOrderID);
            if (OrderVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = OrderVO.NewSoftArticleID };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新软文投诉
        /// </summary>
        /// <param name="CardSoftArticleComplaintVO">软文投诉VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateSoftArticleComplaint"), HttpPost]
        public ResultObject UpdateSoftArticle([FromBody] CardSoftArticleComplaintVO CardSoftArticleComplaintVO, string token)
        {
            if (CardSoftArticleComplaintVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            if (CardSoftArticleComplaintVO.ComplaintID > 0)
            {

                if (cBO.UpdateCardSoftArticleComplaint(CardSoftArticleComplaintVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardSoftArticleComplaintVO.ComplaintID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                CardSoftArticleComplaintVO.CreatedAt = DateTime.Now;
                CardSoftArticleComplaintVO.CustomerId = customerId;

                int ComplaintID = cBO.AddCardSoftArticleComplaint(CardSoftArticleComplaintVO);
                if (ComplaintID > 0)
                {

                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = ComplaintID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }


        /// <summary>
        /// 添加或更新帮助
        /// </summary>
        /// <param name="CardHelpVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddCardHelp"), HttpPost]
        public ResultObject AddCardHelp([FromBody] CardHelpVO CardHelpVO, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (CardHelpVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            if (CardHelpVO.HelpID > 0)
            {
                if (cBO.UpdateHelp(CardHelpVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardHelpVO.HelpID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                CardHelpVO.CreatedAt = DateTime.Now;
                int HelpID = cBO.AddHelp(CardHelpVO);
                if (HelpID > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = HelpID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取帮助
        /// </summary>
        /// <param name="HelpID">帮助ID</param>
        /// <returns></returns>
        [Route("GetCardHelp"), HttpGet, Anonymous]
        public ResultObject GetCardHelp(int HelpID, int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            CardHelpVO vo = cBO.FindCardHelpById(HelpID);
            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 获取全部帮助
        /// </summary>
        /// <param name="HelpID">帮助ID</param>
        /// <returns></returns>
        [Route("GetCardHelpList"), HttpGet, Anonymous]
        public ResultObject GetCardHelpList(int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            List<CardHelpVO> vo = cBO.FindCardHelpByConditionStr("Type = 1 ORDER BY Order_Num desc");
            List<CardHelpVO> vo2 = cBO.FindCardHelpByConditionStr("Type = 2 order by Order_Num desc");
            List<CardHelpVO> vo3 = cBO.FindCardHelpByConditionStr("Type = 3 order by Order_Num desc");

            List<HelpList> List = new List<HelpList>();

            HelpList HelpList = new HelpList();
            HelpList.Title = "名片功能相关";
            HelpList.List = vo;
            List.Add(HelpList);

            HelpList HelpList2 = new HelpList();
            HelpList2.Title = "活动功能相关";
            HelpList2.List = vo2;
            List.Add(HelpList2);

            HelpList HelpList3 = new HelpList();
            HelpList3.Title = "其他功能";
            HelpList3.List = vo3;
            List.Add(HelpList3);

            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = List };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 删除帮助
        /// </summary>
        /// <param name="HelpID">帮助ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteCardHelp"), HttpGet]
        public ResultObject DeleteCardHelp(string HelpID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);
            try
            {
                if (!string.IsNullOrEmpty(HelpID))
                {
                    string[] messageIdArr = HelpID.Split(',');
                    bool isAllDelete = true;
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {
                            cBO.DeleteHelpById(Convert.ToInt32(messageIdArr[i]));
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }

        }

        /// <summary>
        /// 添加或更新企业名录
        /// </summary>
        /// <param name="CompanyVO"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateCompany"), HttpPost, Anonymous]
        public ResultObject UpdateCompany([FromBody] SPLibrary.CustomerManagement.VO.CompanyVO CompanyVO)
        {
            CompanyBO cBO = new CompanyBO(new CustomerProfile());
            if (CompanyVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            if (CompanyVO.CompanyID > 0)
            {

                if (cBO.UpdateCompany(CompanyVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CompanyVO.CompanyID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {

                int CompanyID = cBO.AddCompany(CompanyVO);
                if (CompanyID > 0)
                {

                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = CompanyID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }
        /// <summary>
        /// 获取企业名录数量
        /// </summary>
        /// <returns></returns>
        [Route("GetCompanyCount"), HttpGet, Anonymous]
        public ResultObject GetCompanyCount()
        {
            CompanyBO cBO = new CompanyBO(new CustomerProfile());

            int count = cBO.FindCompanyCount("1=1");
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }
        /// <summary>
        /// 搜索企业名录
        /// </summary>
        /// <returns></returns>
        [Route("GetSearchCompany"), HttpGet, Anonymous]
        public ResultObject GetSearchCompany(string key, string location, int isLongitude = 0, int AppType = 1, decimal latitude = 0, decimal longitude = 0)
        {
            CompanyBO cBO = new CompanyBO(new CustomerProfile());
            IpLocation IpLocation = cBO.getIpLocation(HttpContext.Current.Request.UserHostAddress);

            List<SPLibrary.CustomerManagement.VO.CompanyVO> CVO = new List<SPLibrary.CustomerManagement.VO.CompanyVO>();
            if (IpLocation.location == location)
            {
                CVO = cBO.FindCompanyBySearch(key, location, AppType, latitude, longitude);
            }
            else
            {
                CVO = cBO.FindCompanyBySearch(key, location, AppType);
            }

            if (isLongitude == 1)
            {
                //CVO = cBO.UpdateCompanyLongitude(CVO);
            }
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CVO };
        }

        /// <summary>
        /// 更新企业名录经纬度
        /// </summary>
        /// <returns></returns>
        [Route("GetSearchCompanylocation"), HttpGet, Anonymous]
        public ResultObject GetSearchCompanylocation(string Location = "")
        {
            CompanyBO cBO = new CompanyBO(new CustomerProfile());
            IpLocation IpLocation = cBO.getIpLocation(HttpContext.Current.Request.UserHostAddress);

            string sql = "latitude=0";
            if (Location != "")
            {
                sql += " and Location like '" + Location + "%'";
            }

            List<SPLibrary.CustomerManagement.VO.CompanyVO> CVO = cBO.FindCompanyByCondtion(sql);

            cBO.UpdateCompanyLongitudeByThread(CVO);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = null };
        }

        /// <summary>
        /// 更新企业名录头像
        /// </summary>
        /// <returns></returns>
        [Route("GetCompanyHeadimg"), HttpGet, Anonymous]
        public ResultObject GetCompanyHeadimg(string headsFile)
        {
            CompanyBO cBO = new CompanyBO(new CustomerProfile());
            //目录
            string path = "C:/web/ServicesPlatform/UploadFolder/Headimg/" + headsFile;
            DirectoryInfo di = new DirectoryInfo(path);
            //找到该目录下的文件 
            FileInfo[] fis = di.GetFiles();
            List<myFileInfo> FileList = new List<myFileInfo>();

            foreach (FileInfo fi in fis)
            {
                myFileInfo fVO = new myFileInfo();
                fVO.Url = "https://www.zhongxiaole.net/SPManager/UploadFolder/Headimg/" + headsFile + "/" + HttpUtility.UrlEncode(fi.Name);
                FileList.Add(fVO);
            }


            string sql = "Headimg IS NULL";
            List<SPLibrary.CustomerManagement.VO.CompanyVO> CVO = cBO.FindCompanyByCondtion(sql, FileList.Count);

            cBO.UpdateCompanyHeadimgByThread(CVO, FileList);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CVO.Count, Subsidiary = FileList.Count };
        }

        /// <summary>
        /// 获取企业名录头像数量
        /// </summary>
        /// <returns></returns>
        [Route("GetCompanyHeadimgCount"), HttpGet, Anonymous]
        public ResultObject GetCompanyHeadimgCount()
        {
            CompanyBO cBO = new CompanyBO(new CustomerProfile());
            string sql = "Headimg IS NOT NULL";
            int count = cBO.FindCompanyCount(sql);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }

        /// <summary>
        /// 获取随机头像
        /// </summary>
        /// <returns></returns>
        [Route("GetRandomHeadimg"), HttpGet, Anonymous]
        public ResultObject GetRandomHeadimg()
        {
            CompanyBO cBO = new CompanyBO(new CustomerProfile());
            List<myFileInfo> CVO = cBO.GetFileJson(1);
            if (CVO.Count > 0)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CVO[0].Url };
            }
            return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
        }


        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [Route("CompanyTest"), HttpGet, Anonymous]
        public ResultObject CompanyTest()
        {
            CompanyBO cBO = new CompanyBO(new CustomerProfile());
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CsharpTest_GetWeather.Main(HttpContext.Current.Request.UserHostAddress) };
        }


        /// <summary>
        /// 获取企业名录地区
        /// </summary>
        /// <returns></returns>
        [Route("GetCompanyLocation"), HttpGet, Anonymous]
        public ResultObject GetCompanyLocation(int isLongitude = 0, decimal latitude = 0, decimal longitude = 0)
        {
            try
            {
                List<string> Province = new List<string>();
                Province.Add("北京");
                Province.Add("上海");
                Province.Add("广东");
                Province.Add("天津");
                Province.Add("重庆");
                Province.Add("香港");
                Province.Add("澳门");
                Province.Add("吉林");
                Province.Add("四川");
                Province.Add("宁夏");
                Province.Add("安徽");
                Province.Add("山东");
                Province.Add("山西");
                Province.Add("广西");
                Province.Add("新疆");
                Province.Add("江苏");
                Province.Add("江西");
                Province.Add("河北");
                Province.Add("河南");
                Province.Add("浙江");
                Province.Add("海南");
                Province.Add("湖北");
                Province.Add("湖南");
                Province.Add("甘肃");
                Province.Add("福建");
                Province.Add("西藏");
                Province.Add("贵州");
                Province.Add("辽宁");
                Province.Add("陕西");
                Province.Add("青海");
                Province.Add("黑龙江");
                Province.Add("内蒙古");
                Province.Add("云南");
                Province.Add("台湾");

                CompanyBO cBO = new CompanyBO(new CustomerProfile());
                List<CompanyLocationViewVO> CVO = cBO.FindCompanyLocationByCondtion("1=1");

                List<CompanyLocation> Location = new List<CompanyLocation>();

                for (int i = 0; i < Province.Count; i++)
                {
                    CompanyLocation lVO = new CompanyLocation();
                    lVO.City = new List<string>();
                    lVO.Province = Province[i];

                    if (lVO.Province == "北京" || lVO.Province == "上海" || lVO.Province == "天津" || lVO.Province == "重庆" || lVO.Province == "香港" || lVO.Province == "澳门")
                    {
                        lVO.City.Add(lVO.Province);
                    }
                    for (int j = 0; j < CVO.Count; j++)
                    {
                        if (CVO[j].Location.Contains(lVO.Province))
                        {
                            string City = "";
                            if (CVO[j].Location != "吉林吉林市")
                            {
                                City = CVO[j].Location.Replace(lVO.Province, "");
                            }
                            else
                            {
                                City = "吉林市";
                            }
                            if (City != "")
                            {
                                lVO.City.Add(City);
                            }
                        }
                    }
                    Location.Add(lVO);
                }

                IpLocation IpLocation = cBO.getIpLocation(HttpContext.Current.Request.UserHostAddress);

                List<SPLibrary.CustomerManagement.VO.CompanyVO> CompanyVO = cBO.FindCompanyBySearch("", IpLocation.location, 1, latitude, longitude);

                if (isLongitude == 1)
                {
                    //CompanyVO = cBO.UpdateCompanyLongitude(CompanyVO);
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { Location = Location, CompanyList = CompanyVO, ipProvince = IpLocation.ipProvince, ipCity = IpLocation.ipCity } };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }

        }

        /// <summary>
        /// 人脉广场首页列表
        /// </summary>
        /// <returns></returns>
        [Route("GetSquareList"), HttpGet, Anonymous]
        public ResultObject GetSquareList()
        {
            CompanyBO cBO = new CompanyBO(new CustomerProfile());
            IpLocation IpLocation = cBO.getIpLocation(HttpContext.Current.Request.UserHostAddress);
            List<SPLibrary.CustomerManagement.VO.CompanyVO> CompanyVO = cBO.FindCompanyBySearch("", IpLocation.location);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { CompanyList = CompanyVO } };
        }

        /// <summary>
        /// 根据行业获取企业列表
        /// </summary>
        /// <param name="Industry">1，IT互联网；2，农林牧渔；3，政府/非盈利；4，能源矿产；5，文化传媒；6，服务业；7，交通物流；8，加工制造；9，工艺美术；10，文化/教育</param>
        /// <returns></returns>
        [Route("GetIndustryList"), HttpGet, Anonymous]
        public ResultObject GetIndustryList(int Industry, string location, int PageCount = 50, int PageIndex = 1)
        {
            CompanyBO cBO = new CompanyBO(new CustomerProfile());
            IpLocation IpLocation = cBO.getIpLocation(HttpContext.Current.Request.UserHostAddress);
            CompanyList CompanyVO = cBO.GetIndustryList(Industry, location, PageCount, PageIndex);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { CompanyList = CompanyVO } };
        }

        /// <summary>
        /// 获取企业信息详情
        /// </summary>
        /// <returns></returns>
        [Route("GetCompanySite"), HttpGet, Anonymous]
        public ResultObject GetCompanySite(int CompanyID)
        {
            CompanyBO cBO = new CompanyBO(new CustomerProfile());
            SPLibrary.CustomerManagement.VO.CompanyVO CVO = cBO.FindCompanyByCompanyID(CompanyID);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CVO };
        }

        /// <summary>
        /// 获取搜索默认关键词
        /// </summary>
        /// <returns></returns>
        [Route("GetCompanyKeyword"), HttpGet, Anonymous]
        public ResultObject GetCompanyKeyword()
        {
            List<string> Keyword = new List<string>();
            Keyword.Add("建材");
            Keyword.Add("装饰");
            Keyword.Add("贸易");
            Keyword.Add("电子");
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = Keyword };
        }

        /// <summary>
        /// 获取月度排行榜
        /// </summary>
        /// <param name="AppType"></param>
        /// <returns></returns>
        [Route("GetRanking"), HttpGet, Anonymous]
        public ResultObject GetRanking(int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            List<CardRankingViewVO> Ranking = cBO.GetRanking();
            foreach (CardRankingViewVO item in Ranking)
            {
                List<CardDataVO> CardData = cBO.FindCardByCustomerId(item.CustomerId);
                if (CardData.Count > 0)
                {
                    item.Headimg = CardData[0].Headimg;
                    item.Name = CardData[0].Name;
                    item.CardData = CardData[0];
                }
                else
                {
                    item.CardData = null;
                }

            }

            if (Ranking != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = Ranking };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 获取推荐名片
        /// </summary>
        /// <param name="AppType"></param>
        /// <returns></returns>
        [Route("GetRecommended"), HttpGet, Anonymous]
        public ResultObject GetRecommended(int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            List<CardDataVO> CardData = cBO.GetRecommended("isShare=1", 3000);

            if (CardData.Count > 100)
            {
                Random random = new Random();
                List<CardDataVO> newList = new List<CardDataVO>();
                foreach (CardDataVO item in CardData)
                {
                    newList.Insert(random.Next(newList.Count + 1), item);
                }
                var list = newList.Take(100);
                CardData = new List<CardDataVO>(list);
            }
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CardData };
        }

        /// <summary>
        /// 获取附近的名片
        /// </summary>
        /// <param name="AppType"></param>
        /// <returns></returns>
        [Route("GetNearby"), HttpGet, Anonymous]
        public ResultObject GetNearby(int PageCount, int PageIndex, decimal latitude = 0, decimal longitude = 0, int AppType = 1)
        {
            if (latitude == 0 || longitude == 0)
            {
                string LoginIP = HttpContext.Current.Request.UserHostAddress;

                try
                {
                    string jsonstr = HttpHelper.HtmlFromUrlGet("http://apis.map.qq.com/ws/location/v1/ip?key=QHBBZ-ZDAWI-TRVGE-5GWEO-PHEKO-Q5BXC&ip=" + LoginIP);
                    JObject jo = (JObject)JsonConvert.DeserializeObject(jsonstr);
                    latitude = Convert.ToDecimal(jo["result"]["location"]["lat"]);
                    longitude = Convert.ToDecimal(jo["result"]["location"]["lng"]);
                }
                catch
                {

                }
            }

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);

            List<CardDataVO> CardData = cBO.GetRecommended("isShare=1 order by power(ABS(latitude-" + latitude + "),2)+power(ABS(longitude-" + longitude + "),2) asc");

            /*
            if (CardData.Count > 50)
            {
                Random random = new Random();
                List<CardDataVO> newList = new List<CardDataVO>();
                foreach (CardDataVO item in CardData)
                {
                    newList.Insert(random.Next(newList.Count + 1), item);
                }
                var list = newList.Take(50);
                CardData = new List<CardDataVO>(list);
            }
            */
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CardData.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = CardData.Count };
        }


        /// <summary>
        /// 获取名片马甲
        /// </summary>
        /// <param name="AppType"></param>
        /// <returns></returns>
        [Route("GetDummy"), HttpGet, Anonymous]
        public ResultObject GetDummy(int PageCount, int PageIndex)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), 3);
            List<CardDataVO> CardData = cBO.FindCardByCondition("isDummy=1");

            for (int i = 0; i < CardData.Count; i++)
            {
                if (CardData[i].latitude == 0 && CardData[i].Address != "")
                {
                    WeiXinGeocoder Geocoder = cBO.getLatitudeAndLongitude(CardData[i].Address);
                    if (Geocoder != null)
                    {
                        CardData[i].latitude = Geocoder.result.location.lat;
                        CardData[i].longitude = Geocoder.result.location.lng;
                        cBO.Update(CardData[i]);
                    }
                }
            }

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CardData.Skip((PageIndex - 1) * PageCount).Take(PageCount), Count = CardData.Count };
        }

        /// <summary>
        /// 搜索全部名片，匿名
        /// </summary>
        /// <returns></returns>
        [Route("SearchCard"), HttpGet, Anonymous]
        public ResultObject SearchCard(string Keyword, int PageCount = 50, int PageIndex = 1, int AppType = 1)
        {
            if (Keyword == "")
            {
                return new ResultObject() { Flag = 0, Message = "关键词不能为空!", Result = null };
            }

            string conditionStr = "(locate('" + Keyword + "', Name) > 0 or locate('" + Keyword + "', Position) > 0 or locate('" + Keyword + "', CorporateName) > 0 or locate('" + Keyword + "', Business) > 0) and isShare=1";
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            if (PageIndex == 1 && Keyword != "undefined")
            {
                cBO.AddCardKeyword(Keyword);
            }

            try
            {
                List<CardDataVO> list = cBO.FindAllByPageIndex(conditionStr, (PageIndex - 1) * PageCount + 1, PageIndex * PageCount, "CreatedAt", "desc");
                int count = cBO.FindCardTotalCount(conditionStr);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
            }
            catch (Exception ex)
            {
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = strErrorMsg };
            }
        }

        /// <summary>
        /// 获取热门关键词
        /// </summary>
        /// <returns></returns>
        [Route("GetKeywordList"), HttpGet, Anonymous]
        public ResultObject GetKeywordList(int AppType = 1)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            try
            {
                List<CardKeywordViewVO> list = cBO.GetKeywordList();
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
            }
            catch (Exception ex)
            {
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = strErrorMsg };
            }
        }

        /// <summary>
        /// 获取优惠码
        /// </summary>
        /// <param name="DiscountCodeID">今日话题ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetCardDiscountCode"), HttpGet]
        public ResultObject GetCardDiscountCode(int DiscountCodeID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            CardDiscountCodeVO vo = cBO.FindCardDiscountCodeById(DiscountCodeID);
            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 添加或更新优惠码
        /// </summary>
        /// <param name="CardDiscountCodeVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddCardDiscountCode"), HttpPost]
        public ResultObject AddCardDiscountCode([FromBody] CardDiscountCodeVO CardDiscountCodeVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (CardDiscountCodeVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            if (CardDiscountCodeVO.DiscountCodeID > 0)
            {
                if (cBO.UpdateDiscountCode(CardDiscountCodeVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardDiscountCodeVO.DiscountCodeID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                if (CardDiscountCodeVO.ExpirationAt.Year == 1900)
                {
                    CardDiscountCodeVO.ExpirationAt = DateTime.Now.AddMonths(1);
                }
                int DiscountCodeID = cBO.AddDiscountCode(CardDiscountCodeVO);
                if (DiscountCodeID > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = CardDiscountCodeVO.ExpirationAt };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除优惠码
        /// </summary>
        /// <param name="DiscountCodeID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteCardDiscountCode"), HttpGet]
        public ResultObject DeleteCardDiscountCode(string DiscountCodeID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);
            try
            {
                if (!string.IsNullOrEmpty(DiscountCodeID))
                {
                    string[] messageIdArr = DiscountCodeID.Split(',');
                    bool isAllDelete = true;
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {
                            cBO.DeleteDiscountCodeById(Convert.ToInt32(messageIdArr[i]));
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }

        }

        /// <summary>
        /// 获取代理商
        /// </summary>
        /// <param name="CardAgentID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetCardAgent"), HttpGet]
        public ResultObject GetCardAgent(int CardAgentID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            CardAgentVO vo = cBO.FindCardAgentById(CardAgentID);
            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 获取代理商（前端）
        /// </summary>
        /// <param name="CardAgentID"></param>
        /// <param name="AppType"></param>
        /// <returns></returns>
        [Route("GetCardAgent"), HttpGet, Anonymous]
        public ResultObject GetCardAgent(int CardAgentID, int AppType)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardAgentVO vo = cBO.FindCardAgentById(CardAgentID);
            if (vo != null)
            {
                CardAgentVO CardAgentVO = cBO.FindCardAgentByVO(vo);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CardAgentVO };
            }
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
        }

        /// <summary>
        /// 绑定代理商（前端）
        /// </summary>
        /// <param name="CardAgentID"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("BindCardAgent"), HttpGet]
        public ResultObject BindCardAgent(int CardAgentID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            CardAgentVO vo = cBO.FindCardAgentById(CardAgentID);
            if (vo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
            }
            if (vo.CustomerId > 0)
            {
                return new ResultObject() { Flag = 0, Message = "该代理区域已经绑定了代理会员，请勿重复绑定!", Result = null };
            }
            vo.CustomerId = customerId;

            if (cBO.UpdateCardAgent(vo))
            {
                CustomerVO.Agent = true;
                cuBO.Update(CustomerVO);
                return new ResultObject() { Flag = 1, Message = "绑定成功!", Result = vo };
            }
            else
                return new ResultObject() { Flag = 0, Message = "绑定失败!", Result = null };
        }

        /// <summary>
        /// 添加或更新代理商
        /// </summary>
        /// <param name="CardAgentVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddCardAgent"), HttpPost]
        public ResultObject AddCardAgent([FromBody] CardAgentVO CardAgentVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (CardAgentVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            if (CardAgentVO.AgentName == "")
            {
                return new ResultObject() { Flag = 0, Message = "请输入代理名称!", Result = null };
            }

            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            if (CardAgentVO.CardAgentID > 0)
            {
                if (cBO.UpdateCardAgent(CardAgentVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardAgentVO.CardAgentID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                CardAgentVO.CreatedAt = DateTime.Now;
                if (cBO.FindCardAgentTotalCount("CityId=" + CardAgentVO.CityId) > 0)
                {
                    return new ResultObject() { Flag = 0, Message = "该地区已创建过代理商，不能重复添加!", Result = null };
                }
                int CardAgentID = cBO.AddCardAgent(CardAgentVO);
                if (CardAgentID > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = null };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }
        /// <summary>
        /// 获取代理商绑定二维码
        /// </summary>
        /// <param name="CardAgentID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getCardAgentQR"), HttpGet]
        public ResultObject getCardAgentQR(int CardAgentID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);
            try
            {
                string str = cBO.GetQRIMG(CardAgentID.ToString(), "pages/MyCenter/OpenAgency/OpenAgency");
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = str };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }
        /// <summary>
        /// 代理商解除绑定会员
        /// </summary>
        /// <param name="CardAgentID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("delAgentBind"), HttpGet]
        public ResultObject delAgentBind(int CardAgentID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);
            try
            {
                CardAgentVO CardAgentVO = cBO.FindCardAgentById(CardAgentID);
                int customerId = CardAgentVO.CustomerId;
                CardAgentVO.CustomerId = 0;
                if (cBO.UpdateCardAgent(CardAgentVO))
                {
                    CustomerBO cuBO = new CustomerBO(new CustomerProfile());
                    CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
                    if (CustomerVO != null)
                    {
                        CustomerVO.Agent = false;
                        cuBO.Update(CustomerVO);
                    }
                    return new ResultObject() { Flag = 1, Message = "解除成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "解除失败!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "解除失败!", Result = null };
            }
        }
        /// <summary>
        /// 代理商充值保证金
        /// </summary>
        /// <param name="Cost"></param>
        /// <param name="CustomerId"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AgentRecharge"), HttpGet]
        public ResultObject AgentRecharge(decimal Cost, int CustomerId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);
            string Message = "充值";
            if (Cost < 0)
            {
                Message = "扣除";
            }
            try
            {
                if (cBO.AddAgentDeposit(Cost, CustomerId, "系统" + Message) > 0)
                {
                    return new ResultObject() { Flag = 1, Message = Message + "成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = Message + "失败!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = Message + "失败!", Result = null };
            }
        }

        /// <summary>
        /// 代理商结算
        /// </summary>
        /// <param name="Cost"></param>
        /// <param name="FinanceID"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("setFinance"), HttpGet]
        public ResultObject setFinance(decimal Cost, int FinanceID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            CardAgentFinanceVO fVO = new CardAgentFinanceVO();
            fVO.FinanceID = FinanceID;
            fVO.isSettlement = true;
            fVO.SettlementCost = Cost;
            try
            {
                if (cBO.UpdateCardAgentFinance(fVO))
                {
                    return new ResultObject() { Flag = 1, Message = "修改成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "修改失败!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "修改失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新代理商申请
        /// </summary>
        /// <param name="CardAgentApplyVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateAgentApply"), HttpPost]
        public ResultObject UpdateAgentApply([FromBody] CardAgentApplyVO CardAgentApplyVO, string token)
        {
            if (CardAgentApplyVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            if (CardAgentApplyVO.AgentApplyID > 0)
            {
                if (cBO.UpdateAgentApply(CardAgentApplyVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardAgentApplyVO.AgentApplyID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                CardAgentApplyVO.CustomerId = customerId;
                int AgentApplyID = cBO.AddAgentApply(CardAgentApplyVO);
                if (AgentApplyID > 0)
                {

                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = AgentApplyID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新VIP申请
        /// </summary>
        /// <param name="CardVipApplyVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateVipApply"), HttpPost]
        public ResultObject UpdateVipApply([FromBody] CardVipApplyVO CardVipApplyVO, string token)
        {
            if (CardVipApplyVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO cuBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = cuBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            if (CardVipApplyVO.VipApplyID > 0)
            {
                if (cBO.UpdateVipApply(CardVipApplyVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardVipApplyVO.VipApplyID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                CardVipApplyVO.CustomerId = customerId;
                int VipApplyID = cBO.AddVipApply(CardVipApplyVO);
                if (VipApplyID > 0)
                {

                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = VipApplyID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 发放兑换码（后台专用）
        /// </summary>
        /// <param name="Sum"></param>
        /// <param name="CustomerId"></param>
        /// <param name="Type">0:一年会员 1：7天</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddExchangeCode"), HttpGet]
        public ResultObject AddExchangeCode(int Sum, int CustomerId, int Type, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            if (Sum < 1) { return new ResultObject() { Flag = 0, Message = "发放数量错误!", Result = null }; }
            if (CustomerId < 1) { return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null }; }

            bool isAllDelete = true;

            for (int i = 0; i < Sum; i++)
            {
                try
                {
                    string Code = cBO.RndCode();
                    if (Code != "")
                    {
                        CardExchangeCodeVO eVO = new CardExchangeCodeVO();
                        eVO.ExchangeCodeID = 0;
                        eVO.CustomerId = CustomerId;
                        eVO.ExpirationAt = DateTime.Now.AddYears(1);
                        eVO.Code = cBO.RndCode();
                        eVO.Type = Type;

                        cBO.AddExchangeCode(eVO);
                    }
                    else
                    {
                        isAllDelete = false;
                    }
                }
                catch
                {
                    isAllDelete = false;
                }
            }
            if (isAllDelete)
            {
                return new ResultObject() { Flag = 1, Message = "发放成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 1, Message = "部分发放成功!", Result = null };
            }
        }

        /// <summary>
        /// 获取我的兑换码
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyExchangeCode"), HttpGet]
        public ResultObject getMyExchangeCode(int PageCount, int PageIndex, int Status, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            string listsql = "CustomerId = " + customerId + " and Status=" + Status;
            if (Status == 0)
            {
                listsql += " and ExpirationAt>now()";
            }
            List<CardExchangeCodeVO> list = cBO.FindAllByPageIndexByExchangeCode(listsql, (PageIndex - 1) * PageCount + 1, PageIndex * PageCount, "CreatedAt", "desc");
            foreach (CardExchangeCodeVO vo in list)
            {
                if (vo.ToCustomerId > 0)
                {
                    CustomerVO cVO = CustomerBO.FindCustomenById(vo.ToCustomerId);
                    if (cVO != null)
                    {
                        vo.ToCustomerName = cVO.CustomerName;
                        vo.ToHeaderLogo = cVO.HeaderLogo;
                        List<CardDataVO> CardVO = cBO.FindCardByCustomerId(cVO.CustomerId);
                        if (CardVO.Count > 0)
                        {
                            vo.CardID = CardVO[0].CardID;
                        }
                        else
                        {
                            vo.CardID = 0;
                        }
                    }
                }
            }
            //获取数量
            string CountSql = "CustomerId = " + customerId + " and Status=" + Status;
            if (Status == 0)
            {
                CountSql += " and ExpirationAt>now()";
            }
            int Count = cBO.FindExchangeCodeTotalCount(CountSql);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = Count };
        }

        /// <summary>
        /// 使用兑换码
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UsedExchangeCode"), HttpGet]
        public ResultObject UsedExchangeCode(string Code, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = CustomerBO.FindCustomenById(customerId);
            CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO.AppType);

            List<CardExchangeCodeVO> CodeList = cBO.GetCardExchangeCodeByCode(Code);

            if (CodeList.Count == 0)
            {
                return new ResultObject() { Flag = 0, Message = "兑换失败！请输入正确的活动兑换码!", Result = null };
            }
            CardExchangeCodeVO CodeVO = CodeList[0];
            if (CodeVO.Status == 1)
            {
                return new ResultObject() { Flag = 0, Message = "该兑换码已使用过了!", Result = null };
            }

            if (CodeVO.ExpirationAt < DateTime.Now)
            {
                return new ResultObject() { Flag = 0, Message = "该兑换码已过期!", Result = null };
            }
            int Days = 0;

            if (CodeVO.Type == 0)
            {
                Days = 365;//赠送时间365天
            }
            else
            {
                Days = 7;//赠送时间7天
            }


            //续费
            if (CustomerVO.isVip && CustomerVO.ExpirationAt > DateTime.Now)
            {
                CustomerVO.ExpirationAt = CustomerVO.ExpirationAt.AddDays(Days);
            }
            //开通
            else
            {
                CustomerVO.ExpirationAt = DateTime.Now.AddDays(Days);
            }
            CustomerVO.isVip = true;
            if (CustomerVO.VipLevel != 2 && CustomerVO.VipLevel != 3)
                CustomerVO.VipLevel = 1;

            if (CustomerVO.CustomerId != CodeVO.CustomerId)
            {
                CustomerVO.originCustomerId = CodeVO.CustomerId;
            }


            if (CustomerBO.Update(CustomerVO))
            {
                CodeVO.Status = 1;
                CodeVO.ToCustomerId = customerId;
                CodeVO.UsedAt = DateTime.Now;
                cBO.UpdateExchangeCode(CodeVO);
                return new ResultObject() { Flag = 1, Message = "兑换成功！您成功获得乐聊名片认证会员特权\n（有效期：" + Days + "天）", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "兑换失败，请重试!", Result = null };
            }
        }

        /// <summary>
        /// 微信分享数据包
        /// </summary>
        /// <returns></returns>
        [Route("getSignPackage"), HttpGet, Anonymous]
        public ResultObject getSignPackage(string url)
        {
            WX_JSSDK jssdk = new WX_JSSDK();
            ViewBag ViewBag = jssdk.getSignPackage(url);
            return new ResultObject() { Flag = 1, Message = "获取成功", Result = ViewBag };
        }

        /// <summary>
        /// 获取当前天气
        /// </summary>
        /// <returns></returns>
        [Route("GetWeather"), HttpGet, Anonymous]
        public ResultObject GetWeather()
        {
            GetWeatherVO GetWeatherVO = CsharpTest_GetWeather.Main(HttpContext.Current.Request.UserHostAddress);
            return new ResultObject() { Flag = 1, Message = "获取成功", Result = GetWeatherVO };
        }

        /// <summary>
        /// 手动开奖
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("ManualDrawAPrize"), HttpGet]
        public ResultObject ManualDrawAPrize(int PartyID, string token, int AppType = 1)
        {
            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);

            if (AppType == 1)
            {
                AppType = CustomerVO2.AppType;
            }

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardPartyVO CardPartyVO = cBO.FindPartyById(PartyID);

            if (CardPartyVO == null) { return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null }; }
            if (uProfile.CustomerId != CardPartyVO.CustomerId) { return new ResultObject() { Flag = 0, Message = "权限不足!", Result = null }; }
            if (CardPartyVO.Type != 3) { return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null }; }
            if (CardPartyVO.PartyLuckDrawStatus > 0) { return new ResultObject() { Flag = 0, Message = "该抽奖活动已开过奖了!", Result = null }; }

            if (cBO.DrawAPrize(CardPartyVO.PartyID))
            {
                return new ResultObject() { Flag = 1, Message = "开奖成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "开奖失败!", Result = null };
            }
        }

        /// <summary>
        /// 所有人变成未中奖
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("DrawAPrizeNot"), HttpGet, Anonymous]
        public ResultObject DrawAPrizeNot(int PartyID)
        {

            CardBO cBO = new CardBO(new CustomerProfile());
            if (cBO.DrawAPrizeNot(PartyID))
            {
                return new ResultObject() { Flag = 1, Message = "开奖成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "开奖失败!", Result = null };
            }
        }

        /// <summary>
        /// 复制抽奖
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("CreateDailyLottery"), HttpGet, Anonymous]
        public ResultObject CreateDailyLottery(int PartyID)
        {

            CardBO cBO = new CardBO(new CustomerProfile());
            try
            {
                cBO.CreateDailyLottery(PartyID);
                return new ResultObject() { Flag = 1, Message = "复制成功!", Result = null };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 1, Message = "复制失败!", Result = ex };
            }

        }

        /// <summary>
        /// 自动开奖
        /// </summary>
        /// <returns></returns>
        [Route("AutoDrawAPrize"), HttpGet, Anonymous]
        public ResultObject AutoDrawAPrize()
        {
            CardBO cBO = new CardBO(new CustomerProfile(), 4);
            List<CardPartyVO> PartyVO = cBO.FindCardPartyByCondtion("Type = 3 and PartyLuckDrawStatus=0 and LuckDrawType = 1");
            foreach (CardPartyVO item in PartyVO)
            {
                if (item.StartTime <= DateTime.Now)
                {
                    cBO.DrawAPrize(item.PartyID);
                }
                else if (item.isNoDraw == 1)
                {
                    Random Rdm = new Random();
                    int iRdm = Rdm.Next(1, 25);
                    autoSignUpByPartyID(item.PartyID, iRdm);
                }
            }


            return new ResultObject() { Flag = 1, Message = "开奖成功!", Result = null };
        }

        /// <summary>
        /// 获取活动星选首页
        /// </summary>
        /// <returns></returns> 
        [Route("getHotDrawList"), HttpGet, Anonymous]
        public ResultObject getHotDrawList()
        {
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            CardBO cBO = new CardBO(new CustomerProfile(), 4);

            //推荐活动
            List<CardPartyVO> PartyVO = cBO.FindCardHotPartyByCondtion("1=1 order by ReadCount desc");
            foreach (CardPartyVO item in PartyVO)
            {
                CustomerVO CustomerVO = CustomerBO.FindCustomenById(item.CustomerId);
                List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(item.CustomerId);

                if (CustomerVO != null)
                {
                    item.Name = CustomerVO.HeaderLogo;
                    item.Headimg = CustomerVO.CustomerName;
                }

                if (CardDataVO.Count > 0)
                {
                    item.Name = CardDataVO[0].Name;
                    item.Headimg = CardDataVO[0].Headimg;
                }

                List<CardPartyCostVO> FirstPrizeList = cBO.FindCostByFirstPrize(item.PartyID);
                if (FirstPrizeList.Count > 0)
                {
                    item.FirstPrize = FirstPrizeList[0];
                }
                else
                {
                    List<CardPartyCostVO> Cost = cBO.FindCostByPartyID(item.PartyID);
                    if (Cost.Count > 0)
                    {
                        item.FirstPrize = Cost[0];
                    }
                }
                item.LuckDrawCount = cBO.FindCostSumByPartyID("limitPeopleNum", item.PartyID);
            }

            //精彩活动
            List<CardPartyVO> PartyVO2 = cBO.FindCardIndexPartyByCondtion("1=1 order by ReadCount desc");
            foreach (CardPartyVO item in PartyVO2)
            {
                CustomerVO CustomerVO = CustomerBO.FindCustomenById(item.CustomerId);
                List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(item.CustomerId);

                if (CustomerVO != null)
                {
                    item.Name = CustomerVO.HeaderLogo;
                    item.Headimg = CustomerVO.CustomerName;
                }

                if (CardDataVO.Count > 0)
                {
                    item.Name = CardDataVO[0].Name;
                    item.Headimg = CardDataVO[0].Headimg;
                }

                List<CardPartyCostVO> FirstPrizeList = cBO.FindCostByFirstPrize(item.PartyID);
                if (FirstPrizeList.Count > 0)
                {
                    item.FirstPrize = FirstPrizeList[0];
                }
                else
                {
                    List<CardPartyCostVO> Cost = cBO.FindCostByPartyID(item.PartyID);
                    if (Cost.Count > 0)
                    {
                        item.FirstPrize = Cost[0];
                    }
                }
                item.LuckDrawCount = cBO.FindCostSumByPartyID("limitPeopleNum", item.PartyID);
            }


            //顶部广告
            List<PartyAdVO> TopAD = new List<PartyAdVO>();
            PartyAdVO aditem = new PartyAdVO();
            aditem.imgurl = "https://www.zhongxiaole.net/SPManager/Style/images/AD_c258b8604928e7696d75d23bfcf227e.jpg";
            aditem.PartyID = 0;
            aditem.Type = 0;
            aditem.GoUrl = "/pages/AD/CLL/CLL";
            TopAD.Add(aditem);
            aditem = new PartyAdVO();
            aditem.imgurl = "https://www.zhongxiaole.net/SPManager/Style/images/AD_2dd03433076930883c392567a33894f.jpg";
            aditem.PartyID = 0;
            aditem.Type = 0;
            aditem.GoUrl = "";
            TopAD.Add(aditem);


            //中部广告
            List<PartyAdVO> MiddleAD = new List<PartyAdVO>();
            PartyAdVO aditem5 = new PartyAdVO();
            aditem5.imgurl = "https://www.zhongxiaole.net/SPManager/Style/images/AD_20210414173057.jpg";
            aditem5.PartyID = 0;
            aditem5.GoUrl = "";
            MiddleAD.Add(aditem5);

            PartyAdVO aditem4 = new PartyAdVO();
            aditem4.imgurl = "https://www.zhongxiaole.net/SPManager/Style/images/AD_20210414173106.jpg";
            aditem4.PartyID = 0;
            aditem4.GoUrl = "";
            MiddleAD.Add(aditem4);

            bool isConpon = CardBO.isConpon;//是否打开优惠券功能

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = PartyVO, Subsidiary = PartyVO2, Subsidiary2 = new { TopAD = TopAD, MiddleAD = MiddleAD, isConpon = isConpon } };
        }


        /// <summary>
        /// 打开关闭优惠券入口
        /// </summary>
        /// <returns></returns> 
        [Route("OpenisConpon"), HttpGet, Anonymous]
        public ResultObject OpenisConpon()
        {
            if (CardBO.isConpon)
            {
                CardBO.isConpon = false;
                return new ResultObject() { Flag = 1, Message = "关闭成功!", Result = CardBO.isConpon };
            }
            else
            {
                CardBO.isConpon = true;
                return new ResultObject() { Flag = 1, Message = "打开成功!", Result = CardBO.isConpon };
            }
        }

        /// <summary>
        /// 获取中奖名单
        /// </summary>
        /// <returns></returns>
        [Route("getWinninglist"), HttpGet, Anonymous]
        public ResultObject getWinninglist(int PartyID)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), 4);
            List<CardPartyCostVO> cVO = cBO.FindCostByPartyID(PartyID);

            List<Winning> WinningList = new List<Winning>();
            foreach (CardPartyCostVO item in cVO)
            {
                List<CardPartySignUpVO> CostSignUpVO = cBO.LuckDrawPartyCostSignUp(item.Names, item.Content, PartyID);
                Winning Winning = new Winning();
                Winning.PartyCost = item;
                Winning.PartySignUp = CostSignUpVO;
                WinningList.Add(Winning);
            }
            CardPartyVO PartyVO = cBO.FindPartyById(PartyID);
            CardPartyVO RepeatParty = null;
            if (PartyVO.RepeatPartyID > 0)
            {
                RepeatParty = cBO.FindPartyById(PartyVO.RepeatPartyID);
                List<CardPartyCostVO> FirstPrizeList = cBO.FindCostByFirstPrize(RepeatParty.PartyID);
                if (FirstPrizeList.Count > 0)
                {
                    RepeatParty.FirstPrize = FirstPrizeList[0];
                }
            }

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = WinningList, Subsidiary = RepeatParty };
        }

        /// <summary>
        /// 设置推荐活动
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("setHotParty"), HttpGet]
        public ResultObject setHotParty(int PartyID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            CardPartyVO vo = cBO.FindPartyById(PartyID);

            if (vo != null)
            {
                //cBO.UpdateCardNewsToOff();
                vo.isHot = 1;
                cBO.UpdateParty(vo);
                return new ResultObject() { Flag = 1, Message = "推荐成功!", Result = vo };
            }

            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };

        }

        /// <summary>
        /// 设置头条推荐活动
        /// </summary>
        /// <param name="PartyID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SetCompanyPartyCard"), HttpGet]
        public ResultObject SetCompanyPartyCard(int PartyID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);


            int c = cBO.SetCompanyPartyCard(PartyID);

            SystemBO sBO = new SystemBO(new UserProfile());
            ConfigVO vo = sBO.FindConfig();
            vo.CompanyPartyID = PartyID;
            sBO.ConfigUpdate(vo);

            if (c >= 0)
            {
                return new ResultObject() { Flag = 1, Message = "推荐成功!", Result = null };
            }

            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };

        }

        /// <summary>
        /// 取消推荐活动
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("delHotParty"), HttpGet]
        public ResultObject delHotParty(int PartyID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            CardPartyVO vo = cBO.FindPartyById(PartyID);

            if (vo != null)
            {
                vo.isHot = 0;
                cBO.UpdateParty(vo);
                return new ResultObject() { Flag = 1, Message = "取消成功!", Result = vo };
            }

            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };

        }

        /// <summary>
        /// 设置活动首页展示
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("setIndexParty"), HttpGet]
        public ResultObject setIndexParty(int PartyID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            CardPartyVO vo = cBO.FindPartyById(PartyID);

            if (vo != null)
            {
                //cBO.UpdateCardNewsToOff();
                vo.isIndex = 1;
                cBO.UpdateParty(vo);
                return new ResultObject() { Flag = 1, Message = "设置成功!", Result = vo };
            }

            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };

        }

        /// <summary>
        /// 取消活动首页展示
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("delIndexParty"), HttpGet]
        public ResultObject delIndexParty(int PartyID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            CardPartyVO vo = cBO.FindPartyById(PartyID);

            if (vo != null)
            {
                vo.isIndex = 0;
                cBO.UpdateParty(vo);
                return new ResultObject() { Flag = 1, Message = "取消成功!", Result = vo };
            }

            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };

        }

        /// <summary>
        /// 设置官方抽奖
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("setNoDrawParty"), HttpGet]
        public ResultObject setNoDrawParty(int PartyID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            CardPartyVO vo = cBO.FindPartyById(PartyID);

            if (vo != null)
            {
                //cBO.UpdateCardNewsToOff();
                vo.isNoDraw = 1;
                cBO.UpdateParty(vo);
                return new ResultObject() { Flag = 1, Message = "设置成功!", Result = vo };
            }

            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };

        }

        /// <summary>
        /// 取消官方抽奖
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("delNoDrawParty"), HttpGet]
        public ResultObject delNoDrawParty(int PartyID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            CardPartyVO vo = cBO.FindPartyById(PartyID);

            if (vo != null)
            {
                vo.isNoDraw = 0;
                cBO.UpdateParty(vo);
                return new ResultObject() { Flag = 1, Message = "取消成功!", Result = vo };
            }

            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };

        }

        /// <summary>
        /// 设置循环开奖
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("setRepeatParty"), HttpGet]
        public ResultObject setRepeatParty(int PartyID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            CardPartyVO vo = cBO.FindPartyById(PartyID);

            if (vo != null)
            {
                //cBO.UpdateCardNewsToOff();
                vo.isRepeat = 1;
                cBO.UpdateParty(vo);
                return new ResultObject() { Flag = 1, Message = "设置成功!", Result = vo };
            }

            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };

        }

        /// <summary>
        /// 取消循环开奖
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("delRepeatParty"), HttpGet]
        public ResultObject delRepeatParty(int PartyID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            CardPartyVO vo = cBO.FindPartyById(PartyID);
            if (vo != null)
            {
                vo.isRepeat = 0;
                cBO.UpdateParty(vo);
                return new ResultObject() { Flag = 1, Message = "取消成功!", Result = vo };
            }
            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };
        }

        /// <summary>
        /// 复制抽奖
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("CopyPartyCard"), HttpGet]
        public ResultObject CopyPartyCard(int PartyID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            try
            {
                if (cBO.CreateDailyLottery(PartyID))
                {
                    return new ResultObject() { Flag = 1, Message = "复制成功!", Result = null };
                }

                else
                    return new ResultObject() { Flag = 0, Message = "复制失败!", Result = null };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "复制失败!", Result = ex };
            }


        }


        /// <summary>
        /// 获取经纬度
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getLocation"), HttpGet, Anonymous]
        public ResultObject getLocation(string Address)
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            WeiXinGeocoder Geocoder = cBO.getLatitudeAndLongitude(Address);
            if (Geocoder != null)
            {
                return new ResultObject() { Flag = 0, Message = "获取成功!", Result = Geocoder };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 创建每日抽奖
        /// </summary>
        /// <param name="pass">口令</param>
        /// <returns></returns>
        [Route("CreateDailyLottery"), HttpGet, Anonymous]
        public ResultObject CreateDailyLottery(string pass)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), 4);

            if (pass != "huashun8")
            {
                return new ResultObject() { Flag = 0, Message = "密码错误!", Result = null };
            }
            else
            {
                if (cBO.CreateDailyLottery())
                {
                    return new ResultObject() { Flag = 1, Message = "创建成功!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "创建失败!", Result = null };
                }
            }
        }

        /// <summary>
        /// 补发奖金
        /// </summary>
        /// <param name="PartyID">口令</param>
        /// <returns></returns>
        [Route("SupplyAgain"), HttpGet, Anonymous]
        public ResultObject SupplyAgain(int PartyID)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), 4);

            if (cBO.SupplyAgain(PartyID))
            {
                return new ResultObject() { Flag = 1, Message = "补发成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "补发失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取我的公众号授权列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyAuthorization"), HttpGet]
        public ResultObject getMyAuthorization(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            WxThirdPartyBO wBO = new WxThirdPartyBO(new CustomerProfile());
            List<AuthorizationVO> aVO = wBO.FindAuthorizationByCondition("CustomerId=" + customerId);
            if (aVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = aVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取公众号授权链接
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getAuthorizationUrl"), HttpGet]
        public ResultObject getAuthorizationUrl(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            WxThirdPartyBO wBO = new WxThirdPartyBO(new CustomerProfile());
            string url = wBO.GetAuthorizationUrl(customerId);
            if (url != "")
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = url };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取物流信息
        /// </summary>
        /// <param name="num">物流单号</param>
        /// <returns></returns>
        [Route("getExpress"), HttpGet, Anonymous]
        public ResultObject getExpress(string num)
        {
            ExpressVO eVO = TencentExpress.Main(num);
            if (eVO.code != "-1")
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = eVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = eVO.msg, Result = null };
            }
        }

        /// <summary>
        /// 添加来源统计
        /// </summary>
        /// <param name="CardLaunchVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddLaunch"), HttpPost, Anonymous]
        public ResultObject AddLaunch([FromBody] CardLaunchVO CardLaunchVO, string code, int AppType = 1)
        {
            if (CardLaunchVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            CardLaunchVO.openId = cBO.getOpenId(code);
            if (CardLaunchVO.LaunchID > 0)
            {
                if (cBO.UpdateLaunch(CardLaunchVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = CardLaunchVO.LaunchID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                CardLaunchVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
                int CardId = cBO.AddLaunch(CardLaunchVO);
                if (CardId > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = CardId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 导出来源统计Excel
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="appId"></param>
        /// <param name="Type"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        [Route("GetLaunchExcel"), HttpGet, Anonymous]
        public ResultObject GetLaunchExcel(string StartDate, string EndDate, string appId, string Type, string scene)
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            try
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("小程序", typeof(String));
                dt.Columns.Add("来源小程序", typeof(String));
                dt.Columns.Add("页面路径", typeof(String));
                dt.Columns.Add("场景", typeof(String));
                dt.Columns.Add("openId", typeof(String));
                dt.Columns.Add("登录时间", typeof(DateTime));
                dt.Columns.Add("登录IP", typeof(String));

                string sql = "1=1";
                if (StartDate != "" && StartDate != null)
                {
                    sql += " and CreatedAt >= '" + StartDate + "'";
                }
                if (EndDate != "" && EndDate != null)
                {
                    sql += " and CreatedAt < '" + EndDate + "'";
                }

                if (appId != "" && appId != null)
                {
                    sql += " and appId = '" + appId + "'";
                }

                if (Type != "" && Type != null)
                {
                    sql += " and Type = " + Type + "";
                }

                if (scene != "" && scene != null)
                {
                    sql += " and scene = " + scene + "";
                }

                sql += " GROUP BY openId";

                List<CardLaunchVO> LaunchList = cBO.FindCardLaunchByCondtion(sql);
                foreach (CardLaunchVO Launch in LaunchList)
                {

                    DataRow row = dt.NewRow();

                    if (Launch.Type == 0)
                    {
                        row["小程序"] = "企业名片";
                    }
                    else if (Launch.Type == 1)
                    {
                        row["小程序"] = "乐聊名片";
                    }
                    else if (Launch.Type == 2)
                    {
                        row["小程序"] = "引流王";
                    }
                    else if (Launch.Type == 4)
                    {
                        row["小程序"] = "活动星选";
                    }
                    else
                    {
                        row["小程序"] = "其他";
                    }

                    if (Launch.appId == "wx125c4d21e07ea73b")
                    {
                        row["来源小程序"] = "微养鸡(" + Launch.appId + ")";
                    }
                    else
                    {
                        row["来源小程序"] = "未知";
                    }

                    if (Launch.scene == 1037)
                    {
                        row["场景"] = "其他小程序";
                    }
                    else if (Launch.scene == 1038)
                    {
                        row["场景"] = "其他小程序返回";
                    }
                    else
                    {
                        row["场景"] = "未知";
                    }
                    row["页面路径"] = Launch.path;
                    row["openId"] = Launch.openId;
                    row["登录时间"] = Launch.CreatedAt;
                    row["登录IP"] = Launch.LoginIP;

                    dt.Rows.Add(row);//这样就可以添加了
                }
                string FileName = cBO.DataToExcel(dt, "LaunchExcel/", "来源统计.xls");

                return new ResultObject() { Flag = 1, Message = "生成成功", Result = FileName };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "生成失败", Result = ex };
            }
        }

        /// <summary>
        /// 获取微信省市区编号对照表
        /// </summary>
        /// <returns></returns>
        [Route("GetProvinceAndCity"), HttpGet, Anonymous]
        public ResultObject GetProvinceAndCity(string selectProvince = "")
        {
            try
            {
                DataTable dt = EXCELHelper.GetExcelDatatable("C:/web/ServicesPlatform/Style/省市区编号对照表.xlsx");

                DataTable Newdt = new DataTable();
                Newdt.Columns.Add("编号", typeof(String));
                Newdt.Columns.Add("省", typeof(String));
                Newdt.Columns.Add("市", typeof(String));
                Newdt.Columns.Add("区", typeof(String));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string name = dt.Rows[i]["地区名称"].ToString();
                    string[] names = name.Split(',');

                    DataRow dr = Newdt.NewRow();
                    dr["编号"] = dt.Rows[i]["省市编码"].ToString();
                    if (names.Length >= 2) dr["省"] = names[1];
                    if (names.Length >= 3) dr["市"] = names[2];
                    if (names.Length >= 4) dr["区"] = names[3];

                    Newdt.Rows.Add(dr);
                }

                DataTable Resultdt = new DataTable();
                Resultdt.Columns.Add("name", typeof(String));
                Resultdt.Columns.Add("code", typeof(String));

                for (int i = 0; i < Newdt.Rows.Count; i++)
                {
                    string province = Newdt.Rows[i]["省"].ToString();
                    string city = Newdt.Rows[i]["市"].ToString();
                    string code = Newdt.Rows[i]["编号"].ToString();

                    if (province == "")
                    {
                        province = city;
                    }

                    if (selectProvince != "")
                    {
                        //如果有选择的省份就获取该省所有城市
                        if (selectProvince == province && city != "")
                        {
                            if (Resultdt.Select("name = '" + city + "'").Count() <= 0)
                            {
                                DataRow dr = Resultdt.NewRow();
                                dr["name"] = city;
                                dr["code"] = code;
                                Resultdt.Rows.Add(dr);
                            }
                        }
                    }
                    else
                    {
                        //获取所有省份
                        if (Resultdt.Select("name = '" + province + "'").Count() <= 0)
                        {
                            DataRow dr = Resultdt.NewRow();
                            dr["name"] = province;
                            dr["code"] = code;
                            Resultdt.Rows.Add(dr);
                        }
                    }
                }

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = Resultdt };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 1, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 获取微信开户银行
        /// </summary>
        /// <returns></returns>
        [Route("GetWXBank"), HttpGet, Anonymous]
        public ResultObject GetWXBank()
        {
            try
            {
                List<string> Bank = new List<string>();
                Bank.Add("工商银行");
                Bank.Add("招商银行");
                Bank.Add("民生银行");
                Bank.Add("交通银行");
                Bank.Add("中信银行");
                Bank.Add("浦发银行");
                Bank.Add("兴业银行");
                Bank.Add("光大银行");
                Bank.Add("广发银行");
                Bank.Add("平安银行");
                Bank.Add("北京银行");
                Bank.Add("华夏银行");
                Bank.Add("农业银行");
                Bank.Add("建设银行");
                Bank.Add("邮政储蓄银行");
                Bank.Add("中国银行");
                Bank.Add("宁波银行");
                //Bank.Add("其他银行");

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = Bank };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 1, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 获取申请状态
        /// </summary>
        /// <returns></returns>
        [Route("GetApplyments"), HttpGet]
        public async Task<ResultObject> GetApplyments(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            try
            {
                EcommerceBO eBO = new EcommerceBO();
                List<wxMerchantVO> wVO = eBO.FindMerchantByCondition("CustomerId=" + customerId + " and applyment_id<>'' and Status=1");
                if (wVO.Count > 0)
                {
                    ResultCode res = await eBO.GetApplyments(wVO[0].applyment_id);
                    getapplyment_ResultVO Result = JsonConvert.DeserializeObject<getapplyment_ResultVO>(res.ResultStr);
                    if (Result.sub_mchid != "")
                    {
                        wVO[0].sub_mchid = Result.sub_mchid;
                    }
                    wVO[0].applyment_state = Result.applyment_state;
                    eBO.UpdateMerchant(wVO[0]);
                    string account_number = "";
                    if (wVO[0].account_number != "")
                    {
                        account_number = wVO[0].account_number;
                        account_number = account_number.Substring(0, 4) + "***********" + account_number.Substring(account_number.Length - 4, 4);
                    }
                    return new ResultObject() { Flag = 1, Message = "获取成功", Result = res, Subsidiary = account_number };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "未开通商户", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败", Result = ex };
            }
        }

        /// <summary>
        /// 二级商户进件
        /// </summary>
        /// <returns></returns>
        [Route("Applyments"), HttpPost]
        public async Task<ResultObject> Applyments([FromBody] wxMerchantVO wxMerchantVO, string token)
        {

            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            EcommerceBO eBO = new EcommerceBO();
            try
            {
                //如果已经开通了，就直接通过
                if (eBO.FindMerchantByCondition("CustomerId=" + customerId + " and applyment_id<>'' and applyment_state<>'REJECTED' and Status=1").Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "提交成功", Result = null };
                }

                if (wxMerchantVO.id_card_copy == "" || wxMerchantVO.id_card_national == "" || wxMerchantVO.mobile_phone == "" || wxMerchantVO.store_name == "")
                {
                    return new ResultObject() { Flag = 0, Message = "请填写完整信息", Result = null };
                }
                CardBO cBO = new CardBO(new CustomerProfile());
                ResultObject id_card_copyResult = await cBO.IdCardRecognitionBYTencent2(wxMerchantVO.id_card_copy, 1);
                if (id_card_copyResult.Flag == 0)
                {
                    return new ResultObject() { Flag = 0, Message = id_card_copyResult.Message + ",请重新上传", Result = id_card_copyResult.Result };
                }

                var serializer = new JavaScriptSerializer();
                IdCardData id_card_copy = serializer.Deserialize<IdCardData>(id_card_copyResult.Result.ToString());

                ResultObject id_card_nationalResult = await cBO.IdCardRecognitionBYTencent2(wxMerchantVO.id_card_national, 2);
                if (id_card_nationalResult.Flag == 0)
                {
                    return new ResultObject() { Flag = 0, Message = id_card_nationalResult.Message + ",请重新上传", Result = id_card_nationalResult.Result };
                }

                IdCardData id_card_national = serializer.Deserialize<IdCardData>(id_card_nationalResult.Result.ToString());

                MerchantVO mVO = new MerchantVO();
                Random ran = new Random();
                mVO.out_request_no = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);
                mVO.organization_type = "2401";

                //个人身份证
                mVO.id_card_info = new id_card_info();
                mVO.id_card_info.id_card_name = id_card_copy.Name;
                mVO.id_card_info.id_card_number = id_card_copy.IdNum;

                //处理有效期
                string ValidDate = id_card_national.ValidDate;
                string id_card_valid_time_begin = DateTime.ParseExact(ValidDate.Substring(0, 10), "yyyy.MM.dd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                string id_card_valid_time = "";
                if (ValidDate.Contains("长期")) id_card_valid_time = "长期";
                else
                    id_card_valid_time = DateTime.ParseExact(ValidDate.Substring(11, 10), "yyyy.MM.dd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");

                mVO.id_card_info.id_card_valid_time_begin = id_card_valid_time_begin;
                mVO.id_card_info.id_card_valid_time = id_card_valid_time;
                mVO.id_card_info.id_card_national = wxMerchantVO.id_card_national;
                mVO.id_card_info.id_card_copy = wxMerchantVO.id_card_copy;

                //超级管理员
                mVO.contact_info = new contact_info();
                mVO.contact_info.contact_type = "65";
                mVO.contact_info.contact_name = mVO.id_card_info.id_card_name;
                mVO.contact_info.contact_id_card_number = mVO.id_card_info.id_card_number;
                mVO.contact_info.mobile_phone = wxMerchantVO.mobile_phone;

                mVO.sales_scene_info = new sales_scene_info();
                mVO.sales_scene_info.store_name = wxMerchantVO.store_name;
                mVO.merchant_shortname = wxMerchantVO.store_name;
                mVO.sales_scene_info.store_url = "https://zhongxiaole.net/Cardh5/index.html?#/pages/ShowCard/ShowCard?InviterCID=" + customerId + "&CardID=-" + customerId;

                //银行结算
                mVO.account_info = new account_info();
                mVO.account_info.bank_account_type = "75";
                mVO.account_info.account_bank = wxMerchantVO.account_bank;
                mVO.account_info.account_name = mVO.id_card_info.id_card_name;
                mVO.account_info.bank_address_code = wxMerchantVO.bank_address_code;
                mVO.account_info.account_number = wxMerchantVO.account_number;

                wxMerchantVO newMVO = new wxMerchantVO();
                newMVO.MerchantID = 0;
                newMVO.CustomerId = customerId;
                newMVO.out_request_no = mVO.out_request_no;
                newMVO.organization_type = mVO.organization_type;
                newMVO.id_card_copy = mVO.id_card_info.id_card_copy;
                newMVO.id_card_national = mVO.id_card_info.id_card_national;
                newMVO.id_card_name = mVO.id_card_info.id_card_name;
                newMVO.id_card_number = mVO.id_card_info.id_card_number;
                newMVO.id_card_valid_time = mVO.id_card_info.id_card_valid_time;
                newMVO.id_card_valid_time_begin = mVO.id_card_info.id_card_valid_time_begin;
                newMVO.contact_type = mVO.contact_info.contact_type;
                newMVO.contact_name = mVO.contact_info.contact_name;
                newMVO.contact_id_card_number = mVO.contact_info.contact_id_card_number;
                newMVO.mobile_phone = mVO.contact_info.mobile_phone;
                newMVO.store_name = mVO.sales_scene_info.store_name;
                newMVO.store_url = mVO.sales_scene_info.store_url;
                newMVO.account_bank = mVO.account_info.account_bank;
                newMVO.bank_address_code = mVO.account_info.bank_address_code;
                newMVO.account_number = mVO.account_info.account_number;

                ResultCode res = await eBO.Applyments(mVO);
                if (res.code == "SUCCESS")
                {
                    applyment_ResultVO ResultObj = JsonConvert.DeserializeObject<applyment_ResultVO>(res.ResultStr);
                    newMVO.applyment_id = ResultObj.applyment_id;
                    //将之前申请的账户关闭
                    eBO.UpdateMerchant("Status=0", "CustomerId=" + customerId);
                    eBO.AddMerchant(newMVO);
                    return new ResultObject() { Flag = 1, Message = "提交成功", Result = null };
                }
                else
                {
                    LogBO _log = new LogBO(typeof(EcommerceBO));
                    string strErrorMsg = "Message:" + res.ResultStr + "\r\n";
                    _log.Error(strErrorMsg);
                    return new ResultObject() { Flag = 0, Message = "提交失败," + res.message, Result = mVO };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "提交失败", Result = ex };
            }
        }

        /// <summary>
        /// 获取平台证书
        /// </summary>
        /// <returns></returns>
        [Route("GetPlatformCert"), HttpGet, Anonymous]
        public async Task<ResultObject> GetPlatformCert()
        {
            try
            {
                EcommerceBO eBO = new EcommerceBO();
                ResultCode rc = await eBO.GetPlatformCert();
                return new ResultObject() { Flag = 1, Message = "读取成功", Result = await eBO.GetPlatformCert() };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "读取失败", Result = ex };
            }
        }

        /// <summary>
        /// 修改结算账号
        /// </summary>
        /// <returns></returns>
        [Route("ModifySettlement"), HttpPost]
        public async Task<ResultObject> ModifySettlement([FromBody] wxMerchantVO wxMerchantVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            EcommerceBO eBO = new EcommerceBO();
            try
            {
                List<wxMerchantVO> wVO = eBO.FindMerchantByCondition("CustomerId=" + customerId + " and applyment_id<>'' and Status=1");
                if (wVO.Count > 0)
                {
                    ModifySettlementVO mVO = new ModifySettlementVO();
                    string sub_mchid = wVO[0].sub_mchid;
                    mVO.account_type = "75";
                    mVO.account_bank = wxMerchantVO.account_bank;
                    mVO.bank_address_code = wxMerchantVO.bank_address_code;
                    mVO.account_number = wxMerchantVO.account_number;

                    ResultCode res = await eBO.ModifySettlement(sub_mchid, mVO);
                    if (res.code == "SUCCESS")
                    {
                        wVO[0].account_bank = wxMerchantVO.account_bank;
                        wVO[0].bank_address_code = wxMerchantVO.bank_address_code;
                        wVO[0].account_number = wxMerchantVO.account_number;
                        eBO.UpdateMerchant(wVO[0]);
                        return new ResultObject() { Flag = 1, Message = "修改成功", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "修改失败," + res.message, Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "未开通商户", Result = null };
                }

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "修改失败", Result = ex };
            }
        }

        /// <summary>
        /// 获取结算账号
        /// </summary>
        /// <returns></returns>
        [Route("GetSettlement"), HttpGet, Anonymous]
        public async Task<ResultObject> GetSettlement()
        {
            try
            {
                EcommerceBO eBO = new EcommerceBO();
                ResultCode res = await eBO.GetSettlement("1612018050");
                return new ResultObject() { Flag = 1, Message = "读取成功", Result = res };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "读取失败", Result = ex };
            }
        }

        /// <summary>
        /// 查询二级商户账户实时余额
        /// </summary>
        /// <returns></returns>
        [Route("GetBalance"), HttpGet, Anonymous]
        public async Task<ResultObject> GetBalance()
        {
            try
            {
                EcommerceBO eBO = new EcommerceBO();
                ResultCode res = await eBO.GetBalance("1612018050");
                return new ResultObject() { Flag = 1, Message = "读取成功", Result = res };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "读取失败", Result = ex };
            }
        }

        /// <summary>
        /// 统一分账
        /// </summary>
        /// <returns></returns>
        [Route("GetProfitsharing"), HttpGet, Anonymous]
        public async Task<ResultObject> GetProfitsharing()
        {
            try
            {
                EcommerceBO eBO = new EcommerceBO();
                CardBO cBO = new CardBO(new CustomerProfile());
                List<CardPartyOrderViewVO> cVO = cBO.GetPartyOrderViewVO("Status=1 and SplitCost>0 and IsSplitOut=0");
                foreach (CardPartyOrderViewVO item in cVO)
                {
                    try
                    {
                        ResultCode response = await eBO.GetProfitsharing(item.sp_appid, item.sub_mchid, item.transaction_id, item.OrderNO, Convert.ToInt32(item.SplitCost * 100) + Convert.ToInt32(item.PromotionAwardCost * 100));
                        if (response.code == "SUCCESS")
                        {
                            CardPartyOrderVO cpVO = new CardPartyOrderVO();
                            cpVO.PartyOrderID = item.PartyOrderID;
                            cpVO.IsSplitOut = 1;
                            cBO.UpdatePartyOrder(cpVO);
                        }
                    }
                    catch
                    {

                    }
                }
                return new ResultObject() { Flag = 1, Message = "读取成功", Result = null };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "读取失败", Result = ex };
            }
        }

        /// <summary>
        /// 获取VIP推广页数据
        /// </summary>
        ///<param name="PageIndex"></param>
        ///<param name="PageCount"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getVIPShare"), HttpGet]
        public ResultObject getVIPShare(int PageIndex, int PageCount, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            try
            {
                CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
                CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

                List<CardAccessRecordsViewVO> cVO = cBO.FindCardAccessRecordsViewAllByPageIndex("ById=" + customerId + "  and Type='OpenVip'", (PageIndex - 1) * PageCount + 1, PageIndex * PageCount, "AccessAt", "desc");
                int Count = cBO.FindCardAccessRecordsViewCount("ById=" + customerId + "  and Type='OpenVip'");

                decimal VipOneFrozenBalance = cBO.FindOrderSumByCondtion("OneRebateCost", "OneRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL");
                decimal VipTwoFrozenBalance = cBO.FindOrderSumByCondtion("TwoRebateCost", "TwoRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL");

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO, Count = Count, Subsidiary = new { AccumulatesBalance = VipOneFrozenBalance + VipTwoFrozenBalance } };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }

        }

        /// <summary>
        /// 获取活动分享推广数据
        /// </summary>
        ///<param name="PageIndex"></param>
        ///<param name="PageCount"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getPartyShare"), HttpGet]
        public ResultObject getPartyShare(int PartyID, int PageIndex, int PageCount, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            try
            {
                CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO2 = CustomerBO.FindCustomenById(customerId);
                CardBO cBO = new CardBO(new CustomerProfile(), CustomerVO2.AppType);

                string sql = "ById=" + PartyID + " and ShareCustomerId=" + customerId + "  and (Type='ReadParty' or  Type='ForwardParty' or  Type='SignUpParty')";
                List<CardAccessRecordsViewVO> cVO = cBO.FindCardAccessRecordsViewAllByPageIndex(sql, (PageIndex - 1) * PageCount + 1, PageIndex * PageCount, "AccessAt", "desc");
                int Count = cBO.FindCardAccessRecordsViewCount(sql);

                List<CardPartySignUpViewVO> csVO = cBO.FindSignUpViewByInviterCID(PartyID, customerId);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = cVO, Count = Count, Subsidiary = new { SignCount = csVO.Count } };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 删除抽奖活动(后台专用)
        /// </summary>
        /// <param name="PayoutHistoryId"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelLuckDrawParty"), HttpGet]
        public ResultObject DelLuckDrawParty(int PartyID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId > 0)
            {
                IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
                UserViewVO uVO = uDAO.FindById(uProfile.UserId);
                CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);
                CardPartyVO cVO = cBO.FindPartyById(PartyID);
                if (cVO == null)
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

                if (cVO.Type != 3) return new ResultObject() { Flag = 0, Message = "不能删除非抽奖活动!", Result = null };
                if (cVO.isCashLuckDraw == 1) return new ResultObject() { Flag = 0, Message = "不能删除现金抽奖活动!", Result = null };
                if (cVO.isNoDraw != 1) return new ResultObject() { Flag = 0, Message = "不能删除非官方抽奖活动!", Result = null };
                if (cVO.PartyLuckDrawStatus == 0) return new ResultObject() { Flag = 0, Message = "不能删除未开奖活动!", Result = null };

                //删除所有报名
                cBO.DeleteByPartySignUp(cVO.PartyID);
                //删除活动
                cBO.DeleteByParty(cVO.PartyID);
                //删除访问列表
                cBO.DeleteAccessrecords("ById=" + cVO.PartyID + " and (Type='ReadParty' or Type='ForwardParty' or Type='SignUpParty')");

                //删除所有同名
                List<CardPartyVO> lcVO = cBO.FindPartybycondtion("Title='" + cVO.Title + "' and Type=3 and isCashLuckDraw=0 and isNoDraw=1 and PartyLuckDrawStatus=1");
                foreach (CardPartyVO item in lcVO)
                {
                    //删除所有报名
                    cBO.DeleteByPartySignUp(item.PartyID);
                    //删除活动
                    cBO.DeleteByParty(item.PartyID);
                    //删除访问列表
                    cBO.DeleteAccessrecords("ById=" + item.PartyID + " and (Type='ReadParty' or Type='ForwardParty' or Type='SignUpParty')");
                }

                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = cVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 批量删除活动(后台专用)
        /// </summary>
        /// <returns></returns>
        [Route("DelLuckDrawParty"), HttpGet, Anonymous]
        public ResultObject DelLuckDrawParty()
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            List<CardPartyVO> lcVO = cBO.FindPartybycondtion("(Title='Switch 掌上游戏机 国行版' or Title='九阳电饭煲 家用迷你型3L' or Title='雅诗兰黛眼霜小棕瓶精华 15ml装' or Title='美的家用小型吸尘器' or Title='网易云音乐 ME07 蓝牙耳机 黑色' or Title='生物酶漱口水 300ml*2瓶') and Type=3 and isCashLuckDraw=0 and isNoDraw=1 and PartyLuckDrawStatus=1");

            foreach (CardPartyVO item in lcVO)
            {
                //删除所有报名
                cBO.DeleteByPartySignUp(item.PartyID);
                //删除活动
                cBO.DeleteByParty(item.PartyID);
                //删除访问列表
                cBO.DeleteAccessrecords("ById=" + item.PartyID + " and (Type='ReadParty' or Type='ForwardParty' or Type='SignUpParty')");
            }
            return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
        }

        /// <summary>
        /// 获取我的农场数据
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyFarmGame"), HttpGet]
        public ResultObject getMyFarmGame(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            try
            {
                FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());
                FarmGameVO fVO = FarmGameBO.FindFarmGameByCustomerId(customerId);
                if (fVO != null)
                {

                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = fVO, Subsidiary = FGConfig.getVO() };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }


        /// <summary>
        /// 农场播种
        /// </summary>
        ///<param name="FieldsID">土地ID</param>
        ///<param name="Type">种子类型</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FarmGame_Sow"), HttpGet]
        public ResultObject FarmGame_Sow(int FieldsID, string Type, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            try
            {
                FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());
                FarmGameVO fVO = FarmGameBO.FindFarmGameByCustomerId(customerId);
                if (fVO != null)
                {
                    int money = 0;

                    if (Type == "Carrot_seed")
                        money = FGConfig.Carrot_seed;
                    if (Type == "Tomato_seed")
                        money = FGConfig.Tomato_seed;
                    if (Type == "Eggplant_seed")
                        money = FGConfig.Eggplant_seed;
                    if (Type == "Apple_seed")
                        money = FGConfig.Apple_seed;

                    if (money > fVO.Gold) return new ResultObject() { Flag = 0, Message = "金币不足!", Result = null };

                    string fType = fVO.GetType().GetProperty("FieldsType" + FieldsID).GetValue(fVO, null).ToString();

                    if (fType != "") return new ResultObject() { Flag = 0, Message = "只能在空地播种!", Result = null };

                    //扣除金币
                    fVO.Gold -= money;
                    fVO.GetType().GetProperty("FieldsType" + FieldsID).SetValue(fVO, Type);

                    FarmGameBO.UpdateFarmGame(fVO);
                    return new ResultObject() { Flag = 1, Message = "播种成功!", Result = fVO, Subsidiary = FGConfig.getVO() };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "播种失败!", Result = null };
                }

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "播种失败!", Result = ex };
            }
        }

        /// <summary>
        /// 兑换钻石
        /// </summary>
        ///<param name="Sum">数量</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FarmGame_Diamond"), HttpGet]
        public ResultObject FarmGame_Diamond(int Sum, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            try
            {
                FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());
                FarmGameVO fVO = FarmGameBO.FindFarmGameByCustomerId(customerId);
                if (fVO != null)
                {
                    int money = Sum * FGConfig.Diamond_Money;

                    if (money > fVO.Gold) return new ResultObject() { Flag = 0, Message = "金币不足!", Result = null };

                    //增加钻石
                    fVO.Diamond += Sum;

                    //扣除金币
                    fVO.Gold -= money;

                    FarmGameBO.UpdateFarmGame(fVO);
                    return new ResultObject() { Flag = 1, Message = "兑换成功!", Result = fVO, Subsidiary = FGConfig.getVO() };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "兑换失败!", Result = null };
                }

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "兑换失败!", Result = ex };
            }
        }


        /// <summary>
        /// 农场浇水或者施肥
        /// </summary>
        ///<param name="FieldsID">土地ID</param>
        ///<param name="Type">Watering 浇水，Fertilizer 施肥</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FarmGame_Watering"), HttpGet]
        public ResultObject FarmGame_Watering(int FieldsID, string Type, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            try
            {
                FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());
                FarmGameVO fVO = FarmGameBO.FindFarmGameByCustomerId(customerId);
                if (fVO != null)
                {
                    int Growth = 0;
                    string TypeName = "";
                    if (Type == "Watering")
                    {
                        TypeName = "浇水";
                        if (fVO.Water <= 0) return new ResultObject() { Flag = 0, Message = "水滴不足!", Result = null };
                        fVO.Water -= 1;
                        Growth = FGConfig.Water_Growth;
                    }

                    if (Type == "Fertilizer")
                    {
                        TypeName = "施肥";
                        if (fVO.Fertilizer <= 0) return new ResultObject() { Flag = 0, Message = "肥料不足!", Result = null };
                        fVO.Fertilizer -= 1;
                        Growth = FGConfig.Fertilizer_Growth;
                    }

                    int fSum = Convert.ToInt32(fVO.GetType().GetProperty("FieldsSum" + FieldsID).GetValue(fVO, null));
                    string fType = fVO.GetType().GetProperty("FieldsType" + FieldsID).GetValue(fVO, null).ToString();

                    if (!fType.Contains("seed")) return new ResultObject() { Flag = 0, Message = "只能在种子期浇水或施肥!", Result = null };

                    //10%概率加三倍
                    bool isCriticalHit = false;
                    Random Random = new Random();
                    int r = Random.Next(100);
                    if (r >= (100 - FGConfig.CriticalHit))
                    {
                        isCriticalHit = true;
                        Growth *= FGConfig.CriticalHit_Times;
                    }


                    fSum += Growth;
                    if (fType == "Carrot_seed")
                    {
                        if (fSum >= FGConfig.Carrot_Growth)
                            fType = "Carrot_ripening";
                    }
                    if (fType == "Tomato_seed")
                    {
                        if (fSum >= FGConfig.Tomato_Growth)
                            fType = "Tomato_ripening";
                    }
                    if (fType == "Eggplant_seed")
                    {
                        if (fSum >= FGConfig.Eggplant_Growth)
                            fType = "Eggplant_ripening";
                    }
                    if (fType == "Apple_seed")
                    {
                        if (fSum >= FGConfig.Apple_Growth)
                            fType = "Apple_ripening";
                    }

                    fVO.GetType().GetProperty("FieldsSum" + FieldsID).SetValue(fVO, fSum);
                    fVO.GetType().GetProperty("FieldsType" + FieldsID).SetValue(fVO, fType);

                    FarmGameBO.UpdateFarmGame(fVO);

                    string Message = TypeName + "成功！成长值 +" + Growth;

                    if (isCriticalHit) Message = "暴击！！！成长值 +" + Growth;

                    return new ResultObject() { Flag = 1, Message = Message, Result = fVO, Subsidiary = FGConfig.getVO() };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }
        /// <summary>
        /// 农场收获
        /// </summary>
        ///<param name="FieldsID">土地ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FarmGame_Harvest"), HttpGet]
        public ResultObject FarmGame_Harvest(int FieldsID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            try
            {
                FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());
                FarmGameVO fVO = FarmGameBO.FindFarmGameByCustomerId(customerId);
                if (fVO != null)
                {
                    if (FieldsID > 0)
                    {
                        string fType = fVO.GetType().GetProperty("FieldsType" + FieldsID).GetValue(fVO, null).ToString();
                        if (!fType.Contains("ripening")) return new ResultObject() { Flag = 0, Message = "只能在成熟期收获!", Result = null };

                        int money = 0;
                        if (fType == "Carrot_ripening")
                        {
                            money = FGConfig.Carrot_ripening;
                        }
                        if (fType == "Tomato_ripening")
                        {
                            money = FGConfig.Tomato_ripening;
                        }
                        if (fType == "Eggplant_ripening")
                        {
                            money = FGConfig.Eggplant_ripening;
                        }
                        if (fType == "Apple_ripening")
                        {
                            money = FGConfig.Apple_ripening;
                        }

                        fVO.GetType().GetProperty("FieldsSum" + FieldsID).SetValue(fVO, 0);
                        fVO.GetType().GetProperty("FieldsType" + FieldsID).SetValue(fVO, "");
                        fVO.Gold += money;
                        FarmGameBO.UpdateFarmGame(fVO);
                        return new ResultObject() { Flag = 1, Message = "收获成功！金币+" + money, Result = fVO, Subsidiary = FGConfig.getVO() };
                    }
                    else//一键收获
                    {
                        int SumMoney = 0;
                        for (int i = 1; i <= 8; i++)
                        {
                            string fType = fVO.GetType().GetProperty("FieldsType" + i).GetValue(fVO, null).ToString();
                            int money = 0;
                            if (fType.Contains("ripening"))
                            {
                                if (fType == "Carrot_ripening")
                                {
                                    money = FGConfig.Carrot_ripening;
                                }
                                if (fType == "Tomato_ripening")
                                {
                                    money = FGConfig.Tomato_ripening;
                                }
                                if (fType == "Eggplant_ripening")
                                {
                                    money = FGConfig.Eggplant_ripening;
                                }
                                if (fType == "Apple_ripening")
                                {
                                    money = FGConfig.Apple_ripening;
                                }
                                fVO.GetType().GetProperty("FieldsSum" + i).SetValue(fVO, 0);
                                fVO.GetType().GetProperty("FieldsType" + i).SetValue(fVO, "");
                                SumMoney += money;
                            }
                        }

                        fVO.Gold += SumMoney;


                        if (SumMoney > 0)
                        {
                            FarmGameBO.UpdateFarmGame(fVO);
                            return new ResultObject() { Flag = 1, Message = "收获成功！金币+" + SumMoney, Result = fVO, Subsidiary = FGConfig.getVO() };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 1, Message = "还未成熟，请等等！", Result = fVO, Subsidiary = FGConfig.getVO() };
                        }

                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "收获失败!", Result = null };
                }

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 开垦土地
        /// </summary>
        ///<param name="FieldsID">土地ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FarmGame_Purchaseland"), HttpGet]
        public ResultObject FarmGame_Purchaseland(int FieldsID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            try
            {
                FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());
                FarmGameVO fVO = FarmGameBO.FindFarmGameByCustomerId(customerId);
                if (fVO != null)
                {
                    string fType = fVO.GetType().GetProperty("FieldsType" + FieldsID).GetValue(fVO, null).ToString();
                    if (!fType.Contains("Lock")) return new ResultObject() { Flag = 0, Message = "该土地已开垦!", Result = null };

                    int noFields = 0;
                    if (fVO.FieldsType4.Contains("Lock")) noFields++;
                    if (fVO.FieldsType5.Contains("Lock")) noFields++;
                    if (fVO.FieldsType6.Contains("Lock")) noFields++;
                    if (fVO.FieldsType7.Contains("Lock")) noFields++;
                    if (fVO.FieldsType8.Contains("Lock")) noFields++;


                    int money = 0;
                    if (noFields == 1) { money = FGConfig.Fields5_Money; }
                    if (noFields == 2) { money = FGConfig.Fields4_Money; }
                    if (noFields == 3) { money = FGConfig.Fields3_Money; }
                    if (noFields == 4) { money = FGConfig.Fields2_Money; }
                    if (noFields == 5) { money = FGConfig.Fields1_Money; }

                    fVO.GetType().GetProperty("FieldsSum" + FieldsID).SetValue(fVO, 0);
                    fVO.GetType().GetProperty("FieldsType" + FieldsID).SetValue(fVO, "");
                    if (money > fVO.Gold) return new ResultObject() { Flag = 0, Message = "金币不足!", Result = null };
                    fVO.Gold -= money;
                    FarmGameBO.UpdateFarmGame(fVO);
                    return new ResultObject() { Flag = 1, Message = "土地已开垦!", Result = fVO, Subsidiary = FGConfig.getVO() };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "开垦失败!", Result = null };
                }

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 发放游戏奖励
        /// </summary>
        ///<param name="Type">任务类型</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FarmGame_IssueTaskReward"), HttpGet]
        public ResultObject FarmGame_IssueTaskReward(string Type, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            try
            {
                FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());
                if (FarmGameBO.IssueTaskReward(customerId, Type))
                {
                    List<FarmGameTaskVO> TaskVOList = FarmGameBO.FindFarmGameTaskByCustomerId(customerId);
                    return new ResultObject() { Flag = 1, Message = "发放成功!", Result = TaskVOList };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "发放失败!", Result = null };
                }

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "发放失败!", Result = ex };
            }
        }

        /// <summary>
        /// 获取当天任务
        /// </summary>
        ///<param name="Type">任务类型</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FarmGame_GetTaskOfTheDay"), HttpGet]
        public ResultObject FarmGame_GetTaskOfTheDay(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            try
            {
                FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());
                List<FarmGameTaskVO> fVO = FarmGameBO.FindFarmGameTaskByCustomerId(customerId);
                if (fVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = fVO };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 领取任务奖励
        /// </summary>
        ///<param name="TaskID">任务ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("FarmGame_ReceiveReward"), HttpGet]
        public ResultObject FarmGame_ReceiveReward(int TaskID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            try
            {
                FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());
                FarmGameTaskVO tVO = FarmGameBO.FindFarmGameTaskByTaskID(TaskID);
                if (tVO != null)
                {
                    if (tVO.Status == 0) return new ResultObject() { Flag = 0, Message = "任务未完成!", Result = null };
                    if (tVO.Status == 2) return new ResultObject() { Flag = 0, Message = "已领取奖励了!", Result = null };

                    FarmGameVO fVO = FarmGameBO.FindFarmGameByCustomerId(customerId);
                    string str = "";
                    if (tVO.Water > 0)
                    {
                        str = "水滴+" + tVO.Water + ";";
                        fVO.Water += tVO.Water;
                    }
                    if (tVO.Fertilizer > 0)
                    {
                        str += "肥料+" + tVO.Fertilizer + ";";
                        fVO.Fertilizer += tVO.Fertilizer;
                    }
                    if (tVO.Gold > 0)
                    {
                        str += "金币+" + tVO.Gold + ";";
                        fVO.Gold += tVO.Gold;
                    }
                    tVO.Status = 2;
                    FarmGameBO.UpdateFarmGameTask(tVO);
                    FarmGameBO.UpdateFarmGame(fVO);
                    List<FarmGameTaskVO> TaskVOList = FarmGameBO.FindFarmGameTaskByCustomerId(customerId);
                    return new ResultObject() { Flag = 1, Message = "领取成功!" + str, Result = TaskVOList, Subsidiary = fVO };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }

        /// <summary>
        /// 每小时自动增加成长值
        /// </summary>
        /// <returns></returns>
        [Route("AutoAddGrowth"), HttpGet, Anonymous]
        public ResultObject AutoAddGrowth()
        {
            try
            {
                FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());
                List<FarmGameVO> fVO = FarmGameBO.FindFarmGameByCondition("FieldsType1 Like '%seed' or FieldsType2 Like '%seed' or FieldsType3 Like '%seed' or FieldsType4 Like '%seed' or FieldsType5 Like '%seed' or FieldsType6 Like '%seed' or FieldsType7 Like '%seed' or FieldsType8 Like '%seed'");
                foreach (FarmGameVO item in fVO)
                {
                    try
                    {
                        for (int i = 1; i <= 8; i++)
                        {
                            int fSum = Convert.ToInt32(item.GetType().GetProperty("FieldsSum" + i).GetValue(item, null));
                            string fType = item.GetType().GetProperty("FieldsType" + i).GetValue(item, null).ToString();

                            if (fType.Contains("seed"))
                            {
                                fSum += FGConfig.PerHour_Growth;

                                if (fType == "Carrot_seed")
                                {
                                    if (fSum >= FGConfig.Carrot_Growth)
                                        fType = "Carrot_ripening";
                                }
                                if (fType == "Tomato_seed")
                                {
                                    if (fSum >= FGConfig.Tomato_Growth)
                                        fType = "Tomato_ripening";
                                }
                                if (fType == "Eggplant_seed")
                                {
                                    if (fSum >= FGConfig.Eggplant_Growth)
                                        fType = "Eggplant_ripening";
                                }
                                if (fType == "Apple_seed")
                                {
                                    if (fSum >= FGConfig.Apple_Growth)
                                        fType = "Apple_ripening";
                                }

                                item.GetType().GetProperty("FieldsSum" + i).SetValue(item, fSum);
                                item.GetType().GetProperty("FieldsType" + i).SetValue(item, fType);

                                FarmGameBO.UpdateFarmGame(item);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                return new ResultObject() { Flag = 1, Message = "成功!", Result = null };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "失败!", Result = ex };
            }
        }

        /// <summary>
        /// 获取奖品详情
        /// </summary>
        /// <param name="PrizeID">奖品ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetFarmgamePrize"), HttpGet]
        public ResultObject GetFarmgamePrize(int PrizeID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());

            FarmgamePrizeVO vo = FarmGameBO.FindFarmgamePrizeByPrizeID(PrizeID);
            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 获取奖品列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetFarmgamePrizeList"), HttpGet]
        public ResultObject GetFarmgamePrizeList(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());

            List<FarmgamePrizeVO> vo = FarmGameBO.FindFarmGamePrizeByCondition("Status=1");
            if (vo != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };

        }

        /// <summary>
        /// 添加或更新奖品
        /// </summary>
        /// <param name="FarmgamePrizeVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddFarmgamePrize"), HttpPost]
        public ResultObject AddFarmgamePrize([FromBody] FarmgamePrizeVO FarmgamePrizeVO, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (FarmgamePrizeVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());

            if (FarmgamePrizeVO.PrizeID > 0)
            {
                if (FarmGameBO.UpdateFarmgamePrize(FarmgamePrizeVO))
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = FarmgamePrizeVO.PrizeID };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            else
            {
                FarmgamePrizeVO.Status = 1;
                FarmgamePrizeVO.CreatedAt = DateTime.Now;
                int NewsID = FarmGameBO.AddFarmgamePrize(FarmgamePrizeVO);
                if (NewsID > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = NewsID };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 兑换奖品
        /// </summary>
        /// <param name="FarmgamePrizeVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddFarmgamePrize"), HttpGet]
        public ResultObject AddFarmgamePrize(int PrizeID, string Name, string Phone, string Address, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            return new ResultObject() { Flag = 0, Message = "兑换失败，因广州疫情影响，奖品暂时无法发货!", Result = null };

            if (Name == "" || Phone == "" || Address == "")
            {
                return new ResultObject() { Flag = 0, Message = "请输入完整的收货信息!", Result = null };
            }


            FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());
            FarmgamePrizeVO fVO = FarmGameBO.FindFarmgamePrizeByPrizeID(PrizeID);

            if (fVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "请选择兑换的奖品!", Result = null };
            }
            FarmGameVO gVO = FarmGameBO.FindFarmGameByCustomerId(customerId);

            if (fVO.Price > gVO.Diamond)
            {
                return new ResultObject() { Flag = 0, Message = "钻石不足!", Result = null };
            }

            gVO.Diamond -= fVO.Price;
            FarmGameBO.UpdateFarmGame(gVO);

            FarmgamePrizeOrderVO oVO = new FarmgamePrizeOrderVO();
            oVO.PrizeID = fVO.PrizeID;
            oVO.Price = fVO.Price;
            oVO.CustomerId = customerId;
            oVO.Name = Name;
            oVO.Phone = Phone;
            oVO.Address = Address;
            oVO.Status = 1;
            oVO.CreatedAt = DateTime.Now;
            int PrizeOrderID = FarmGameBO.AddFarmgamePrizeOrder(oVO);

            if (PrizeOrderID > 0)
            {
                return new ResultObject() { Flag = 1, Message = "兑换成功，请注意接收快递!", Result = PrizeOrderID };
            }
            else
                return new ResultObject() { Flag = 0, Message = "兑换失败!", Result = null };
        }


        /// <summary>
        /// 上架或下架奖品
        /// </summary>
        /// <param name="PrizeID">奖品ID</param>
        /// <param name="Status">状态：0，1</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("SetShelves"), HttpGet]
        public ResultObject SetShelves(int PrizeID, int Status, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());

            FarmgamePrizeVO vo = FarmGameBO.FindFarmgamePrizeByPrizeID(PrizeID);
            if (vo != null)
            {
                vo.Status = Status;
                FarmGameBO.UpdateFarmgamePrize(vo);
                return new ResultObject() { Flag = 1, Message = "设置成功!", Result = vo };
            }
            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };

        }

        /// <summary>
        /// 奖品订单确认发货
        /// </summary>
        /// <param name="PrizeOrderID">奖品订单ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("PrizeDeliver"), HttpGet]
        public ResultObject PrizeDeliver(int PrizeOrderID, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());

            FarmgamePrizeOrderVO vo = FarmGameBO.FindFarmgamePrizeOrderByPrizeOrderID(PrizeOrderID);
            if (vo != null)
            {
                vo.Status = 2;
                FarmGameBO.UpdateFarmgamePrizeOrder(vo);
                return new ResultObject() { Flag = 1, Message = "发货成功!", Result = vo };
            }
            else
                return new ResultObject() { Flag = 0, Message = "设置失败!", Result = null };

        }

        /// <summary>
        /// 删除奖品
        /// </summary>
        /// <param name="NewsID">系统消息ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteFarmgamePrize"), HttpGet]
        public ResultObject DeleteFarmgamePrize(string PrizeID, string token, int AppType = 1)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            FarmGameBO FarmGameBO = new FarmGameBO(new CustomerProfile());
            try
            {
                if (!string.IsNullOrEmpty(PrizeID))
                {
                    string[] messageIdArr = PrizeID.Split(',');
                    bool isAllDelete = true;
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {
                            FarmGameBO.DeleteFarmgamePrize(Convert.ToInt32(messageIdArr[i]));
                        }
                        catch
                        {
                            isAllDelete = false;
                        }
                    }
                    if (isAllDelete)
                    {
                        return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }

        }

        /// <summary>
        /// 添加或更新小程序(后台专用)
        /// </summary>
        /// <param name="wxMiniprogramsVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddMiniprograms"), HttpPost]
        public ResultObject AddMiniprograms([FromBody] wxMiniprogramsVO wxMiniprogramsVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId == 3)
            {
                if (wxMiniprogramsVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
                MiniprogramsBO cBO = new MiniprogramsBO();

                if (wxMiniprogramsVO.AppType > 0)
                {
                    if (cBO.UpdateMiniprograms(wxMiniprogramsVO))
                    {
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = wxMiniprogramsVO.AppType };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                    }
                }
                else
                {
                    wxMiniprogramsVO.CreatedAt = DateTime.Now;
                    int AppType = cBO.AddMiniprograms(wxMiniprogramsVO);
                    if (AppType > 0)
                    {
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = AppType };
                    }
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "你没有权限!", Result = null };
            }
        }

        /// <summary>
        /// 获取小程序(后台专用)
        /// </summary>
        /// <param name="AppType">ID</param>
        /// <returns></returns>
        [Route("GetMiniprograms"), HttpGet]
        public ResultObject GetMiniprograms(int AppType, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId == 3)
            {
                MiniprogramsBO cBO = new MiniprogramsBO();
                wxMiniprogramsVO vo = cBO.FindMiniprogramsById(AppType);
                if (vo != null)
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = vo };
                else
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
            else
                return new ResultObject() { Flag = 0, Message = "你没有查看权限!", Result = null };
        }


        /// <summary>
        /// 获取推广奖励数据
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getPromotionAwards2"), HttpGet]
        public ResultObject getPromotionAwards2(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            CardBO cBO = new CardBO(new CustomerProfile());
            //一级推广奖金
            decimal VipOneFrozenBalance = cBO.FindOrderSumByCondtion("OneRebateCost", "OneRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL");
            //二级推广奖金
            decimal VipTwoFrozenBalance = cBO.FindOrderSumByCondtion("TwoRebateCost", "TwoRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL");
            //一级拉新
            int CustomerCount = cBO.FindOriginCustomerCount("originCustomerId=" + customerId);
            //二级拉新
            int CustomerCount2 = cBO.FindOriginCustomerCount("originCustomerId2=" + customerId);
            //一级vip
            int VIPCustomerCount = cBO.FindOriginCustomerCount("originCustomerId=" + customerId + " and isVip=1");
            //二级vip
            int VIPCustomerCount2 = cBO.FindOriginCustomerCount("originCustomerId2=" + customerId + " and isVip=1");

            //昨日拉新
            int YesterdayCount = cBO.FindOriginCustomerCount("(originCustomerId=" + customerId + " or originCustomerId2=" + customerId + ") and TO_DAYS(NOW()) - TO_DAYS(CreatedAt) = 1");
            //前天拉新
            int BeforeCount = cBO.FindOriginCustomerCount("(originCustomerId=" + customerId + " or originCustomerId2=" + customerId + ") and TO_DAYS(NOW()) - TO_DAYS(CreatedAt) = 2");
            //昨日拉新增加百分比
            decimal YesterdayPercentage = cBO.getPercentage(YesterdayCount, BeforeCount);

            //昨日vip
            int VIPYesterdayCount = cBO.FindOriginCustomerCount("(originCustomerId=" + customerId + " or originCustomerId2=" + customerId + ") and TO_DAYS(NOW()) - TO_DAYS(CreatedAt) = 1" + " and isVip=1");
            //前天vip
            int VIPBeforeCount = cBO.FindOriginCustomerCount("(originCustomerId=" + customerId + " or originCustomerId2=" + customerId + ") and TO_DAYS(NOW()) - TO_DAYS(CreatedAt) = 2" + " and isVip=1");
            //昨日vip增加百分比
            decimal VIPYesterdayPercentage = cBO.getPercentage(VIPYesterdayCount, VIPBeforeCount);


            //上周拉新
            int LastweekCount = cBO.FindOriginCustomerCount("(originCustomerId=" + customerId + " or originCustomerId2=" + customerId + ")  and YEARWEEK(date_format(CreatedAt,'%Y-%m-%d')) = YEARWEEK(now())-1");
            //上上周拉新
            int BeforeLastweekCount = cBO.FindOriginCustomerCount("(originCustomerId=" + customerId + " or originCustomerId2=" + customerId + ") and YEARWEEK(date_format(CreatedAt,'%Y-%m-%d')) = YEARWEEK(now())-2");
            //上周拉新增加百分比
            decimal LastweekPercentage = cBO.getPercentage(YesterdayCount, BeforeCount);

            //上周vip
            int VIPLastweekCount = cBO.FindOriginCustomerCount("(originCustomerId=" + customerId + " or originCustomerId2=" + customerId + ")  and YEARWEEK(date_format(CreatedAt,'%Y-%m-%d')) = YEARWEEK(now())-1" + " and isVip=1");
            //上上周vip
            int VIPBeforeLastweekCount = cBO.FindOriginCustomerCount("(originCustomerId=" + customerId + " or originCustomerId2=" + customerId + ")  and YEARWEEK(date_format(CreatedAt,'%Y-%m-%d')) = YEARWEEK(now())-2" + " and isVip=1");
            //上周vip增加百分比
            decimal VIPLastweekPercentage = cBO.getPercentage(VIPLastweekCount, VIPBeforeLastweekCount);

            //上月拉新
            int MonthCount = cBO.FindOriginCustomerCount("(originCustomerId=" + customerId + " or originCustomerId2=" + customerId + ")  and date_format(CreatedAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 1 MONTH),'%Y-%m')");
            //上上月拉新
            int BeforeMonthCount = cBO.FindOriginCustomerCount("(originCustomerId=" + customerId + " or originCustomerId2=" + customerId + ")   and date_format(CreatedAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 2 MONTH),'%Y-%m')");
            //上月拉新增加百分比
            decimal MonthPercentage = cBO.getPercentage(MonthCount, BeforeMonthCount);

            //上月vip
            int VIPMonthCount = cBO.FindOriginCustomerCount("(originCustomerId=" + customerId + " or originCustomerId2=" + customerId + ")  and date_format(CreatedAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 1 MONTH),'%Y-%m')" + " and isVip=1");
            //上上月vip
            int VIPBeforeMonthCount = cBO.FindOriginCustomerCount("(originCustomerId=" + customerId + " or originCustomerId2=" + customerId + ")   and date_format(CreatedAt,'%Y-%m')=date_format(DATE_SUB(curdate(), INTERVAL 2 MONTH),'%Y-%m')" + " and isVip=1");
            //上月vip增加百分比
            decimal VIPMonthPercentage = cBO.getPercentage(VIPMonthCount, VIPBeforeMonthCount);



            object Subsidiarydata = new
            {
                YesterdayCount,
                YesterdayPercentage,
                VIPYesterdayCount,
                VIPYesterdayPercentage,
                LastweekCount,
                LastweekPercentage,
                VIPLastweekCount,
                VIPLastweekPercentage,
                MonthCount,
                MonthPercentage,
                VIPMonthCount,
                VIPMonthPercentage,
                Datatime = DateTime.Now.ToString("yyyy/MM/dd")
            };


            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = new { VipOneFrozenBalance, VipTwoFrozenBalance, CustomerCount, CustomerCount2, VIPCustomerCount, VIPCustomerCount2 }, Subsidiary = Subsidiarydata };
        }

        /// <summary>
        /// 更改VIP订单的邀请人（后台专用）
        /// </summary>
        /// <param name="CardOrderID"></param>
        /// <param name="CustomerId"></param>
        /// <param name="Type">1:OneRebateCustomerId 2：TwoRebateCustomerId</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ChangeRebateCustomerId"), HttpGet]
        public ResultObject ChangeRebateCustomerId(int CardOrderID, int CustomerId, int Type, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
            UserViewVO uVO = uDAO.FindById(uProfile.UserId);
            if (uProfile.UserId != 3)
            {
                return new ResultObject() { Flag = 0, Message = "权限不足!", Result = null };
            }
            CardBO cBO = new CardBO(new CustomerProfile(), uVO.AppType);

            if (CustomerId < 0) { return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null }; }

            CardOrderVO cVO = cBO.FindOrderById(CardOrderID);
            if (cVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "订单错误!", Result = null };
            }
            if (Type == 1)
            {
                if (cVO.OneRebateStatus == 1)
                {
                    return new ResultObject() { Flag = 0, Message = "该订单的一级佣金已被结算，无法修改邀请人!", Result = null };
                }
                cVO.OneRebateCustomerId = CustomerId;
                cBO.UpdateOrder(cVO);
                return new ResultObject() { Flag = 1, Message = "修改成功!", Result = null };
            }
            if (Type == 2)
            {
                if (cVO.TwoRebateStatus == 1)
                {
                    return new ResultObject() { Flag = 0, Message = "该订单的二级佣金已被结算，无法修改邀请人!", Result = null };
                }
                cVO.TwoRebateCustomerId = CustomerId;
                cBO.UpdateOrder(cVO);
                return new ResultObject() { Flag = 1, Message = "修改成功!", Result = null };
            }

            return new ResultObject() { Flag = 0, Message = "Type参数错误!", Result = null };
        }

        /// <summary>
        /// 更改报名座位号
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <returns></returns>
        [Route("updateSeatNo"), HttpPost]
        public ResultObject updateSeatNo(int PartySignUpID, string value, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CardBO cBO = new CardBO(new CustomerProfile());
            CardPartySignUpVO cVO = cBO.FindPartySignUpById(PartySignUpID);
            if (cVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "查询失败!", Result = null };
            }
            cVO.SeatNo = value;
            cBO.UpdateCardPartySignUp(cVO);
            return new ResultObject() { Flag = 1, Message = "修改成功!", Result = null };
        }

        /// <summary>
        /// 硅基流动 DeepSeek API
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("RequestDeepDeek"), HttpPost, Anonymous]
        public async Task<ResultObject> RequestDeepDeek([FromBody] AiVO vo)
        {
            //硅基流动
            //string apiUrl = "https://api.siliconflow.cn/v1/chat/completions";
            //string apiKey = "sk-vvtjhndxfilidonutwoymjpncmthrhvicrehrevwcfrmlsoo";
            //var requestData = new
            //{
            //    model = "Pro/deepseek-ai/DeepSeek-R1",//
            //    messages = new[] { new{
            //      role ="user",
            //      content = vo.value
            //    }},
            //    stream = false,
            //    max_tokens = 3000,
            //    temperature = 0.7,
            //    top_p = 0.7,
            //    top_k = 50,
            //    frequency_penalty = 0.5,
            //    n = 1,
            //    response_format = new { type = "text" }
            //};

            //deep seek官网
            string apiUrl = "https://api.deepseek.com/chat/completions";
            string apiKey = "sk-d03a190a09584522903c4e07b7378396";

            var requestData = new
            {
                model = "deepseek-reasoner",
                messages = new[] { new{
                  role ="system",
                  content = "You are a helpful assistant."
                },
                new{
                  role ="user",
                  content = vo.value
                }},
                frequency_penalty = 0,
                max_tokens = 2048,
                presence_penalty = 0,
                response_format = new { type = "text" },
                stream = false,
                temperature = 1,
                top_p = 1,
                tool_choice = "none",
                logprobs = false,
            };

            // 将请求数据序列化为 JSON 字符串 
            var jsonData = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                // 添加 API Key 到请求头 
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                // 发送 POST 请求 
                var response = await client.PostAsync(apiUrl, content);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return new ResultObject() { Flag = 1, Message = "请求失败：" + response.ReasonPhrase, Result = null };
                }
                // 读取响应内容 
                var responseBody = await response.Content.ReadAsStringAsync();
                LogBO _log = new LogBO(typeof(EcommerceBO));
                _log.Error("【【【【AI请求返回数据:" + responseBody + "】】】】");
                // 反序列化响应 JSON 数据 
                dynamic result = JsonConvert.DeserializeObject(responseBody);

                return new ResultObject() { Flag = 1, Message = result.choices[0].message.content, Result = result };
            }

        }
    }
}
public class CollectionListVO
{
    public List<CardDataVO> CardDataVO { get; set; }
    public ConditionModel Condition { get; set; }
}

public class getliveinfoVO
{
    public List<live_replay> live_replay { get; set; }
    public int errcode { get; set; }
    public int total { get; set; }
    public string errmsg { get; set; }
}

public class live_replay
{
    public string expire_time { get; set; }
    public string create_time { get; set; }
    public string media_url { get; set; }
}

public class AiVO
{
    public string value { get; set; }
}