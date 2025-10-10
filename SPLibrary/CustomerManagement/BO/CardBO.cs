using CoreFramework.DAO;
using CoreFramework.VO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Drawing;
using System.IO;
using SPLibrary.WebConfigInfo;
using System.Linq;
using System.Xml;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using ImportEXCEL;
using System.Runtime.Serialization.Json;
using SPLibrary.BusinessCardManagement.VO;
using System.Threading.Tasks;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using SPLibrary.BusinessCardManagement.BO;
using System.Threading;
using System.Globalization;
using System.Drawing.Imaging;

namespace SPLibrary.CustomerManagement.BO
{
    public class CardBO
    {
        public static bool istest = false;
        public static bool isOpenSend = false;
        public static bool isConpon = true;
        public string appid = "";
        public string secret = "";

        public string appidH5 = "wx9a65d7becbbb017a";
        public int AppType = 1;
        public int Type = 1;
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();
        public CardBO(CustomerProfile customerProfile, int apptype = 1)
        {
            this.CurrentCustomerProfile = customerProfile;
            AppVO AppVO = AppBO.GetApp(apptype);
            appid = AppVO.AppId;
            secret = AppVO.Secret;
            AppType = AppVO.AppType;
            Type = apptype;
        }
        /// <summary>
        /// 添加名片
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCard(CardDataVO vo)
        {
            try
            {
                ICardDataDAO rDAO = CustomerManagementDAOFactory.CreateCardDataDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int CardID = rDAO.Insert(vo);
                    return CardID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 添加马甲名片
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddDummyCard(CardDataVO vo)
        {
            try
            {
                ICardDataDAO rDAO = CustomerManagementDAOFactory.CreateCardDataDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    //创建会员账号
                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    CustomerVO cVO = new CustomerVO();
                    cVO.CustomerCode = uBO.GetCustomerCode();
                    string password = Utilities.MakePassword(8);
                    cVO.Password = Utilities.GetMD5(password);
                    cVO.Status = 1;
                    cVO.CreatedAt = DateTime.Now;
                    cVO.CustomerName = vo.Name;
                    cVO.Sex = true;
                    cVO.Leliao = true;
                    cVO.originType = "C_Service";
                    cVO.HeaderLogo = vo.Headimg;
                    cVO.AppType = AppType;
                    int customerId = uBO.Add(cVO);
                    vo.CustomerId = customerId;

                    vo.CreatedAt = DateTime.Now;
                    vo.Status = 1;//0:禁用，1:启用
                    vo.Collection = 0;
                    vo.ReadCount = 0;
                    vo.Forward = 0;
                    vo.isDummy = 1;
                    vo.style = 1;

                    /*
                    if (vo.Address != "")
                    {
                        WeiXinGeocoder Geocoder = getLatitudeAndLongitude(vo.Address);
                        if (Geocoder != null)
                        {
                            vo.latitude = Geocoder.result.location.lat;
                            vo.longitude = Geocoder.result.location.lng;
                        }
                    }
                    */
                    vo.AppType = AppType;

                    int CardID = rDAO.Insert(vo);
                    return CardID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新名片
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool Update(CardDataVO vo)
        {
            ICardDataDAO uDAO = CustomerManagementDAOFactory.CreateCardDataDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 删除名片
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public int DeleteById(int CardID)
        {
            ICardDataDAO uDAO = CustomerManagementDAOFactory.CreateCardDataDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("CardID = " + CardID+ " and AppType="+ AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCardCollection(CardCollectionVO vo)
        {
            try
            {
                ICardCollectionDAO rDAO = CustomerManagementDAOFactory.CreateCardCollectionDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int CollectionID = rDAO.Insert(vo);
                    return CollectionID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 添加名片组
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCardGroup(CardGroupVO vo)
        {
            try
            {
                ICardGroupDAO rDAO = CustomerManagementDAOFactory.CreateCardGroupDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int GroupID = rDAO.Insert(vo);
                    return GroupID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新名片组
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardGroup(CardGroupVO vo)
        {
            ICardGroupDAO uDAO = CustomerManagementDAOFactory.CreateCardGroupDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 获取名片组详情
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public CardGroupVO FindCardGroupById(Int64 GroupID)
        {
            ICardGroupDAO uDAO = CustomerManagementDAOFactory.CreateCardGroupDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(GroupID);
        }

        /// <summary>
        /// 添加名片到名片组
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCardToGroup(CardGroupCardVO vo)
        {
            try
            {
                ICardGroupCardDAO rDAO = CustomerManagementDAOFactory.CardGroupCardDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int GroupCardID = rDAO.Insert(vo);
                    return GroupCardID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新名片到名片组
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardToGroup(CardGroupCardVO vo)
        {
            ICardGroupCardDAO rDAO = CustomerManagementDAOFactory.CardGroupCardDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 更新名片到名片组
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardToGroup(string data, string condition)
        {
            ICardGroupCardDAO rDAO = CustomerManagementDAOFactory.CardGroupCardDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.Update(data, condition+ " and AppType="+ AppType);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 退出指定名片组
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public int DeleteCardToGroupByGroupID(int GroupID, int CustomerId)
        {
            ICardGroupCardDAO rDAO = CustomerManagementDAOFactory.CardGroupCardDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("GroupID = " + GroupID + " and CustomerId=" + CustomerId+ " and AppType="+ AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 删除名片组成员
        /// </summary>
        /// <param name="GroupCardID"></param>
        /// <returns></returns>
        public int DeleteByGroupCardID(int GroupCardID)
        {
            ICardGroupCardDAO rDAO = CustomerManagementDAOFactory.CardGroupCardDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("GroupCardID = " + GroupCardID+ " and AppType="+ AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取我的名片组
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardGroupViewVO> FindCardGroupViewByCustomerId(int CustomerId)
        {
            ICardGroupViewDAO uDAO = CustomerManagementDAOFactory.CardGroupViewDAO(this.CurrentCustomerProfile);
            ICardGroupCardViewDAO cDAO = CustomerManagementDAOFactory.CardGroupCardViewDAO(this.CurrentCustomerProfile);

            List<CardGroupViewVO> cVO = uDAO.FindByParams("CustomerId = " + CustomerId + " and Status<>0 and AppType="+ AppType+ " GROUP BY GroupID");

            for (int i = 0; i < cVO.Count; i++)
            {
                cVO[i].NumberOfPeople = FindTotalCount("GroupID = " + cVO[i].GroupID + " and Status<>0 and AppType="+ AppType);
                cVO[i].MessageCount = FindMessageCountByGroupID(cVO[i].GroupID, CustomerId);
                cVO[i].CardGroupCardViewList = cDAO.FindAllByPageIndex("GroupID = " + cVO[i].GroupID, 9);
            }

            //根据创建时间重新排序
            cVO.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));
            cVO.Reverse();

            return cVO;
        }

        /// <summary>
        /// 获取名片组列表
        /// </summary>
        public List<CardGroupViewViewVO> FindAllByPageIndexByGroup(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardGroupCardViewViewDAO uDAO = CustomerManagementDAOFactory.CardGroupCardViewViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr+ " and AppType="+ AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取名片组数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindGroupTotalCount(string condition, params object[] parameters)
        {
            ICardGroupCardViewViewDAO uDAO = CustomerManagementDAOFactory.CardGroupCardViewViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition+ " and AppType="+ AppType, parameters);
        }

        /// <summary>
        /// 获取名片组数量（视图）
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindGroupViewTotalCount(string condition, params object[] parameters)
        {
            ICardGroupViewDAO uDAO = CustomerManagementDAOFactory.CardGroupViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition+ " and AppType="+ AppType, parameters);
        }

        /// <summary>
        /// 获取名片组人数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindTotalCount(string condition, params object[] parameters)
        {
            ICardGroupCardViewDAO rDAO = CustomerManagementDAOFactory.CardGroupCardViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition+ " and AppType="+ AppType, parameters);
        }

        /// <summary>
        /// 获取名片组所有名片
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardGroupCardViewVO> FindCardGroupCardViewByGroupID(int GroupID)
        {
            ICardGroupCardViewDAO uDAO = CustomerManagementDAOFactory.CardGroupCardViewDAO(this.CurrentCustomerProfile);
            List<CardGroupCardViewVO> cVO = uDAO.FindByParams("GroupID = " + GroupID+ " and AppType="+ AppType);
            return cVO;
        }

        /// <summary>
        /// 获取名片组所有管理员
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardGroupCardViewVO> FindCardGroupAdminByGroupID(int GroupID)
        {
            ICardGroupCardViewDAO uDAO = CustomerManagementDAOFactory.CardGroupCardViewDAO(this.CurrentCustomerProfile);
            List<CardGroupCardViewVO> cVO = uDAO.FindByParams("GroupID = " + GroupID+ " and Status=3 and AppType="+ AppType);
            return cVO;
        }

        /// <summary>
        /// 获取名片组我的名片
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public CardGroupCardViewVO FindCardGroupCardViewByGroupID(int GroupID, int CustomerId)
        {
            ICardGroupCardViewDAO uDAO = CustomerManagementDAOFactory.CardGroupCardViewDAO(this.CurrentCustomerProfile);
            List<CardGroupCardViewVO> cVO = uDAO.FindByParams("GroupID = " + GroupID + " and CustomerId=" + CustomerId+ " and AppType="+ AppType);

            if (cVO.Count > 0)
            {
                return cVO[0];
            } else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取名片组我的名片
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public CardGroupCardVO FindCardGroupCardByGroupID(int GroupID, int CustomerId)
        {
            ICardGroupCardDAO uDAO = CustomerManagementDAOFactory.CardGroupCardDAO(this.CurrentCustomerProfile);
            List<CardGroupCardVO> cVO = uDAO.FindByParams("GroupID = " + GroupID + " and CustomerId=" + CustomerId+ " and AppType="+ AppType);

            if (cVO.Count > 0)
            {
                return cVO[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取名片组需要我审核的信息数量
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public int FindMessageCountByGroupID(int GroupID, int CustomerId)
        {
            ICardGroupCardViewDAO uDAO = CustomerManagementDAOFactory.CardGroupCardViewDAO(this.CurrentCustomerProfile);
            List<CardGroupCardViewVO> cVO = uDAO.FindByParams("GroupID = " + GroupID + " and CustomerId=" + CustomerId + " and Status=3 and AppType="+ AppType);

            if (cVO.Count > 0)
            {
                int Count = 0;
                for (int i = 0; i < cVO.Count; i++)
                {
                    List<CardGroupCardViewVO> cVO2 = uDAO.FindByParams("GroupID = " + GroupID + " and Status=0 and AppType="+ AppType);
                    Count += cVO2.Count;
                }
                return Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 判断是否已加入名片组
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns> m  
        public List<CardGroupCardViewVO> isJionCardGroup(int CustomerId, int GroupID)
        {
            ICardGroupCardViewDAO uDAO = CustomerManagementDAOFactory.CardGroupCardViewDAO(this.CurrentCustomerProfile);
            List<CardGroupCardViewVO> cVO = uDAO.FindByParams("CustomerId = " + CustomerId + " and GroupID=" + GroupID+ " and AppType="+ AppType);
            return cVO;
        }

        /// <summary>
        /// 获取名片组的名片详情
        /// </summary>
        /// <param name="GroupCardID"></param>
        /// <returns></returns>
        public CardGroupCardViewVO FindCardGroupCardByGroupCardID(int GroupCardID)
        {
            ICardGroupCardViewDAO uDAO = CustomerManagementDAOFactory.CardGroupCardViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(GroupCardID);
        }

        /// <summary>
        /// 获取名片组的名片详情
        /// </summary>
        /// <param name="GroupCardID"></param>
        /// <returns></returns>
        public CardGroupCardVO FindCardGroupCardByID(int GroupCardID)
        {
            ICardGroupCardDAO uDAO = CustomerManagementDAOFactory.CardGroupCardDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(GroupCardID);
        }

        /// <summary>
        /// 添加回递名片记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCardSend(CardSendVO vo)
        {
            try
            {
                ICardSendDAO rDAO = CustomerManagementDAOFactory.CreateCardSendDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int SendID = rDAO.Insert(vo);
                    return SendID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 查询是否已经递过该名片
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public bool isCardSend(int CustomerId, int CardID)
        {
            ICardSendDAO uDAO = CustomerManagementDAOFactory.CreateCardSendDAO(this.CurrentCustomerProfile);
            List<CardSendVO> sVO = uDAO.FindByParams("CustomerId = " + CustomerId + " and CardID = " + CardID+ " and AppType="+ AppType);
            if (sVO.Count > 0) {
                return true;
            } else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除该名片所有回递记录
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public int DeleteCardSendByCardID(int CardID)
        {
            ICardSendDAO uDAO = CustomerManagementDAOFactory.CreateCardSendDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("CardID = " + CardID+ " and AppType="+ AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public int DeleteCardCollectionByCardID(int CardID, int CustomerId)
        {
            ICardCollectionDAO uDAO = CustomerManagementDAOFactory.CreateCardCollectionDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("CardID = " + CardID + " and " + "CustomerId = " + CustomerId+ " and AppType="+ AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 删除所有收藏
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public int DeleteCardCollectionByCustomerId(int CustomerId)
        {
            ICardCollectionDAO uDAO = CustomerManagementDAOFactory.CreateCardCollectionDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("CustomerId = " + CustomerId+ " and AppType="+ AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        /// 获取我的名片收藏
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardDataVO> FindCardCollectionByCustomerId(int CustomerId,int isShare=0)
        {
            ICardCollectionViewDAO uDAO = CustomerManagementDAOFactory.CreateCardCollectionViewDAO(this.CurrentCustomerProfile);
            string sql = "t_CustomerId = " + CustomerId+ " and AppType="+ AppType;
            if (isShare == 1)
            {
                sql += " and isShare=1";
            }

            return uDAO.FindByParams(sql);
        }

        /// <summary>
        /// 获取共同人脉收藏
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardDataVO> FindTongCardByCustomerId(int CustomerId, int toCustomerId)
        {
            List<CardDataVO> oneList = FindCardCollectionByCustomerId(CustomerId);
            List<CardDataVO> towList = FindCardCollectionByCustomerId(toCustomerId);
            List<CardDataVO> newList = new List<CardDataVO>();

            if(CustomerId!= toCustomerId)
            {
                for (int i = 0; i < oneList.Count; i++)
                {
                    if (towList.Exists(p => p.CardID == oneList[i].CardID))
                    {
                        newList.Add(oneList[i]);
                    }
                }
            }

            return newList;
        }

        /// <summary>
        /// 获取我的名片收藏（分页）
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardDataVO> FindCardCollectionAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardCollectionViewDAO uDAO = CustomerManagementDAOFactory.CreateCardCollectionViewDAO(this.CurrentCustomerProfile);
            List<CardDataVO> cVO = uDAO.FindAllByPageIndex(conditionStr+ " and AppType="+ AppType, start, end, sortcolname, asc, parameters);
            return cVO;
        }

        /// <summary>
        /// 获取我的名片收藏（数量）
        /// </summary>
        /// <returns></returns>
        public int FindCardCollectionAllCount(string condition)
        {
            ICardCollectionViewDAO uDAO = CustomerManagementDAOFactory.CreateCardCollectionViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType);
        }

        /// <summary>
        /// 获取我的名片收藏（数量）
        /// </summary>
        /// <returns></returns>
        public int FindCardCollectionAllCountByGROUP(string condition)
        {
            ICardCollectionViewDAO uDAO = CustomerManagementDAOFactory.CreateCardCollectionViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCountByGROUP(condition + " and AppType=" + AppType);
        }

        /// <summary>
        /// 判断是否已有收藏
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public int isCardCollection(int CustomerId, int CardID)
        {
            ICardCollectionViewDAO uDAO = CustomerManagementDAOFactory.CreateCardCollectionViewDAO(this.CurrentCustomerProfile);
            return uDAO.CountByParams("t_CustomerId = " + CustomerId + " and CardID=" + CardID + " and AppType=" + AppType);
        }

        /// <summary>
        /// 获取递给我的名片
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardSendViewVO> FindCardSendByCustomerId(int CustomerId)
        {
            ICardSendViewDAO uDAO = CustomerManagementDAOFactory.CreateCardSendViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex("t_CustomerId = " + CustomerId + " and AppType=" + AppType, 1,50, "SendID", "desc");
        }

        /// <summary>
        /// 获取递给我的名片数量(未读)
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public int FindCardSendByCustomerIdCount(int CustomerId)
        {
            ICardSendViewDAO uDAO = CustomerManagementDAOFactory.CreateCardSendViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount("t_CustomerId = " + CustomerId + " and ReadStatus = 0" + " and AppType=" + AppType);
        }

        /// <summary>
        /// 已读所有递给我的名片
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public int setSendReadStatus(int CustomerId)
        {

            ICardSendDAO uDAO = CustomerManagementDAOFactory.CreateCardSendDAO(this.CurrentCustomerProfile);

            try
            {
                List<CardSendVO> sVO = uDAO.FindAllByPageIndex("CustomerId = " + CustomerId + " and ReadStatus = 0" + " and AppType=" + AppType);
                for (int i = 0; i < sVO.Count; i++)
                {
                    if (sVO[i].FormId != "")
                    {
                        sendTemplateMessage(sVO[i].TCardID, sVO[i]);
                    }
                }
            }
            catch
            {

            }

            try
            {
                return uDAO.Update("ReadStatus = 1", "CustomerId = " + CustomerId + " and AppType=" + AppType);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取名片详情
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public CardDataVO FindCardById(Int64 CardID)
        {
            ICardDataDAO uDAO = CustomerManagementDAOFactory.CreateCardDataDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(CardID);
        }

        /// <summary>
        /// 添加关键词记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCardKeyword(string Keyword)
        {
            try
            {
                ICardKeywordDAO rDAO = CustomerManagementDAOFactory.CardKeywordDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    CardKeywordVO vo = new CardKeywordVO();
                    vo.KeywordID = 0;
                    vo.Keyword = Keyword;
                    vo.AppType = AppType;
                    int SendID = rDAO.Insert(vo);
                    return SendID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 获取热门关键词
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardKeywordViewVO> GetKeywordList()
        {
            ICardKeywordViewDAO uDAO = CustomerManagementDAOFactory.CardKeywordViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams("AppType=" + AppType);
        }

        /// <summary>
        /// 获取名片列表
        /// </summary>
        public List<CardDataVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardDataDAO uDAO = CustomerManagementDAOFactory.CreateCardDataDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取名片数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardTotalCount(string condition, params object[] parameters)
        {
            ICardDataDAO uDAO = CustomerManagementDAOFactory.CreateCardDataDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取名片列表视图
        /// </summary>
        public List<CardDataViewVO> FindCardDataViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardDataViewDAO uDAO = CustomerManagementDAOFactory.CardDataViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取名片数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardDataViewTotalCount(string condition, params object[] parameters)
        {
            ICardDataViewDAO uDAO = CustomerManagementDAOFactory.CardDataViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取我的名片列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardDataVO> FindCardByCustomerId(Int64 CustomerId)
        {
            ICardDataDAO uDAO = CustomerManagementDAOFactory.CreateCardDataDAO(this.CurrentCustomerProfile);
            return uDAO.FindCardList("CustomerId = " + CustomerId+ " and isPublic=0" + " and AppType=" + AppType);
        }

        /// <summary>
        /// 获取我的公共名片列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardDataVO> FindCardByPublic(int CustomerId)
        {
            ICardDataDAO uDAO = CustomerManagementDAOFactory.CreateCardDataDAO(this.CurrentCustomerProfile);
            return uDAO.FindCardList("CustomerId = " + CustomerId + " and isPublic=1" + " and AppType=" + AppType);
        }

        /// <summary>
        /// 搜索名片列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardDataVO> FindCardByCondition(string Condition, int limit = 0)
        {
            ICardDataDAO uDAO = CustomerManagementDAOFactory.CreateCardDataDAO(this.CurrentCustomerProfile);
            return uDAO.FindCardList(Condition + " and AppType=" + AppType, limit);
        }

        /// <summary>
        /// 获取推荐名片
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardDataVO> GetRecommended(string Condition, int limit = 0)
        {
            ICardDataRecommendedViewDAO uDAO = CustomerManagementDAOFactory.CardDataRecommendedViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindCardList("AppType=" + AppType+ "  and  "+ Condition, limit);
        }


        /// <summary>
        /// 保存名片截图
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public string GenerateCardImage(int CardID)
        {
            Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.APIURLByHttp + "/CustomerManagement/CardIMG.aspx?CardID=" + CardID + "&AppType = " + Type, 693, 416, 693, 416);

            //保存
            string filePath = "";
            string folder = "/UploadFolder/CardFile/";
            string newFileName = CardID + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            return ConfigInfo.Instance.APIURL + filePath;
        }
        /// <summary>
        /// 获取名片二维码
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public string GetCardQR(int CardID)
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret="+secret+"";
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
            string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;
            DataJson = "{";
            DataJson += "\"scene\":\"" + CardID + "\",";
            DataJson += string.Format("\"width\":{0},", 430);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", "pages/ShowCard/ShowCard");//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
            DataJson += "\"line_color\":{";
            DataJson += string.Format("\"r\":\"{0}\",", "0");
            DataJson += string.Format("\"g\":\"{0}\",", "0");
            DataJson += string.Format("\"b\":\"{0}\"", "0");
            DataJson += "},";
            DataJson += "\"is_hyaline\":false";
            DataJson += "}";

            Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
            Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。

            //保存
            string filePath = "";
            string folder = "/UploadFolder/CardFile/";
            string newFileName = CardID + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;

            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string Cardimg = ConfigInfo.Instance.APIURL + filePath;

            CardDataVO cVO = new CardDataVO();
            cVO.CardID = CardID;
            cVO.CardImg = Cardimg;
            Update(cVO);
            return Cardimg;
        }

        /// <summary>
        /// 获取名片组二维码
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public string GetCardGroupQR(int GroupID)
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret="+secret+"";
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
            string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;
            DataJson = "{";
            DataJson += "\"scene\":\"" + GroupID + "\",";
            DataJson += string.Format("\"width\":{0},", 430);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", "pages/CardGroupJoin/CardGroupJoin");//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
            DataJson += "\"line_color\":{";
            DataJson += string.Format("\"r\":\"{0}\",", "0");
            DataJson += string.Format("\"g\":\"{0}\",", "0");
            DataJson += string.Format("\"b\":\"{0}\"", "0");
            DataJson += "},";
            DataJson += "\"is_hyaline\":false";
            DataJson += "}";

            Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
            Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。
            //保存
            string filePath = "";
            string folder = "/UploadFolder/CardGroupFile/";
            string newFileName = GroupID + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string Cardimg = ConfigInfo.Instance.APIURL + filePath;

            CardGroupVO cVO = new CardGroupVO();
            cVO.GroupID = GroupID;
            cVO.CardImg = Cardimg;
            UpdateCardGroup(cVO);
            return Cardimg;
        }

        /// <summary>
        /// 获取活动二维码
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public string GetCardPartyQR(int PartyID)
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+appid+"&secret="+secret+"";
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
            string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;


            string page = "pages/Party/PartyShow/PartyShow";
            if (Type == 4)
            {
                page = "package/package_party/PartyShow/PartyShow";
            }

            
            CardPartyVO pVO = FindPartyById(PartyID);
            if ((pVO.Type == 3|| pVO.isBlindBox == 1) && Type == 4)
            {
                page = "package/package_sweepstakes/PartyShow/PartyShow";
            }

            DataJson = "{";
            DataJson += "\"scene\":\"" + PartyID + "\",";
            DataJson += string.Format("\"width\":{0},", 1280);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", page);//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
            DataJson += "\"line_color\":{";
            DataJson += string.Format("\"r\":\"{0}\",", "0");
            DataJson += string.Format("\"g\":\"{0}\",", "0");
            DataJson += string.Format("\"b\":\"{0}\"", "0");
            DataJson += "},";
            DataJson += "\"is_hyaline\":false";
            DataJson += "}";

            Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
            Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。
            //保存
            string filePath = "";
            string folder = "/UploadFolder/CardPartyFile/";
            string newFileName = PartyID + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string Cardimg = ConfigInfo.Instance.APIURL + filePath;

            CardPartyVO cVO = new CardPartyVO();
            cVO.PartyID = PartyID;
            cVO.QRCodeImg = Cardimg;
            UpdateParty(cVO);
            return Cardimg;
        }

        /// <summary>
        /// 获取签到表二维码
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public string GetQuestionnaireQR(int QuestionnaireID)
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret + "";
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
            string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;

            string page = "pages/index/SignInFormByUser/SignInFormByUser";
            if (Type == 4)
            {
                page = "package/package_form/SignInFormByUser/SignInFormByUser";
            }

            DataJson = "{";
            DataJson += "\"scene\":\"" + QuestionnaireID + "\",";
            DataJson += string.Format("\"width\":{0},", 430);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", page);//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
            DataJson += "\"line_color\":{";
            DataJson += string.Format("\"r\":\"{0}\",", "0");
            DataJson += string.Format("\"g\":\"{0}\",", "0");
            DataJson += string.Format("\"b\":\"{0}\"", "0");
            DataJson += "},";
            DataJson += "\"is_hyaline\":false";
            DataJson += "}";

            Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
            Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。
            //保存
            string filePath = "";
            string folder = "/UploadFolder/QuestionnaireFile/";
            string newFileName = QuestionnaireID + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string Cardimg = ConfigInfo.Instance.APIURL + filePath;

            CardQuestionnaireVO cVO = new CardQuestionnaireVO();
            cVO.QuestionnaireID = Convert.ToInt32(QuestionnaireID);
            cVO.QRImg = Cardimg;
            UpdateCardQuestionnaire(cVO);
            return Cardimg;
        }

        /// <summary>
        /// 获取软文二维码
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public string GetSoftArticleQR(int SoftArticleID)
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret + "";
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
            string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;

            string page = "pages/MyCenter/Article/Article";
            if (Type == 4)
            {
                page = "package/package_article/Article/Article";
            }
            DataJson = "{";
            DataJson += "\"scene\":\"" + SoftArticleID + "\",";
            DataJson += string.Format("\"width\":{0},", 430);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", page);//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
            DataJson += "\"line_color\":{";
            DataJson += string.Format("\"r\":\"{0}\",", "0");
            DataJson += string.Format("\"g\":\"{0}\",", "0");
            DataJson += string.Format("\"b\":\"{0}\"", "0");
            DataJson += "},";
            DataJson += "\"is_hyaline\":false";
            DataJson += "}";

            Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
            Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。
            //保存
            string filePath = "";
            string folder = "/UploadFolder/SoftArticleFile/";
            string newFileName = SoftArticleID + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string Cardimg = ConfigInfo.Instance.APIURL + filePath;

            CardSoftArticleVO cVO = new CardSoftArticleVO();
            cVO.SoftArticleID = SoftArticleID;
            cVO.QRImg = Cardimg;
            UpdateSoftArticle(cVO);
            return Cardimg;
        }

        /// <summary>
        /// 获某个页面小程序码
        /// </summary>
        /// <returns></returns>
        public string GetQRIMG(string scene,string page)
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+appid+"&secret="+secret+"";
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
            string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;
            DataJson = "{";
            DataJson += "\"scene\":\""+ scene + "\",";
            DataJson += string.Format("\"width\":{0},", 500);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", ""+ page + "");//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
            DataJson += "\"line_color\":{";
            DataJson += string.Format("\"r\":\"{0}\",", "0");
            DataJson += string.Format("\"g\":\"{0}\",", "0");
            DataJson += string.Format("\"b\":\"{0}\"", "0");
            DataJson += "},";
            DataJson += "\"is_hyaline\":false";
            DataJson += "}";

            Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
            Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。
            //保存
            string filePath = "";
            string folder = "/UploadFolder/CardTemporaryFile/";
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff")+".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string Cardimg = ConfigInfo.Instance.APIURL + filePath;

    
            return Cardimg;
        }


        /// <summary>
        /// 获取活动二维码 携带用户信息
        /// </summary>
        /// <param name="PartyID"></param>
        /// <param name="CustomerId"></param>
        /// <returns>返回一个图片的名字xx.png</returns>
        public string GetCardPartyQRByMessage(int PartyID,Int64 CustomerId)
        {
            string jsonstr = "";
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret + "";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var result = new WeiXinAccessTokenResultDYH();
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                    LogBO _log = new LogBO(typeof(CardBO));
                    string strErrorMsg = "微信登陆错误:" + jsonStr;
                    _log.Error(strErrorMsg);
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }
                string DataJson = string.Empty;
                string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;

                string page = "pages/Party/PartyShow/PartyShow";
                if (Type == 4)
                {
                    page = "package/package_party/PartyShow/PartyShow";
                }


                CardPartyVO pVO = FindPartyById(PartyID);
                if ((pVO.Type == 3 || pVO.isBlindBox == 1) && Type == 4)
                {
                    page = "package/package_sweepstakes/PartyShow/PartyShow";
                }

                DataJson = "{";
                DataJson += "\"scene\":\"" + PartyID + "-" + CustomerId + "\",";
                //DataJson += "\"scene\":\"123456\",";
                DataJson += string.Format("\"width\":{0},", 1280);
                DataJson += "\"auto_color\":false,";
                DataJson += string.Format("\"page\":\"{0}\",", page);//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
                DataJson += "\"line_color\":{";
                DataJson += string.Format("\"r\":\"{0}\",", "0");
                DataJson += string.Format("\"g\":\"{0}\",", "0");
                DataJson += string.Format("\"b\":\"{0}\"", "0");
                DataJson += "},";
                DataJson += "\"is_hyaline\":false";
                DataJson += "}";

                Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
                Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。
                                                   //保存
                string filePath = "";
                string folder = "/UploadFolder/CardPartyQRTemporaryFile/";
                string newFileName = PartyID + "" + CustomerId + ".png";
                filePath = folder + newFileName;

                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string physicalPath = localPath + newFileName;
                m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

                string Cardimg = ConfigInfo.Instance.APIURL + filePath;

                return newFileName;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source+ " jsonstr:" + jsonstr;
                _log.Error(strErrorMsg);
                return "";
            }    
        }
        /// <summary>
        /// 获取入场券二维码
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <returns></returns>
        public string GetCardPartySignUpQR(int PartySignUpID)
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+appid+"&secret="+secret+"";
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
            string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;

            string page = "pages/Party/SignUpShow/SignUpShow";
            if (Type == 4)
            {
                page = "package/package_party/SignUpShow/SignUpShow";
            }

            DataJson = "{";
            DataJson += "\"scene\":\"" + PartySignUpID + "\",";
            DataJson += string.Format("\"width\":{0},", 430);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", page);//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
            DataJson += "\"line_color\":{";
            DataJson += string.Format("\"r\":\"{0}\",", "0");
            DataJson += string.Format("\"g\":\"{0}\",", "0");
            DataJson += string.Format("\"b\":\"{0}\"", "0");
            DataJson += "},";
            DataJson += "\"is_hyaline\":false";
            DataJson += "}";

            Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
            Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。
            //保存
            string filePath = "";
            string folder = "/UploadFolder/CardPartySignFile/";
            string newFileName = PartySignUpID + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string Cardimg = ConfigInfo.Instance.APIURL + filePath;

            CardPartySignUpVO cVO = new CardPartySignUpVO();
            cVO.PartySignUpID = PartySignUpID;
            cVO.QRCodeImg = Cardimg;
            UpdateSignUp(cVO);
            return Cardimg;
        }

        /// <summary>
        /// 获取核销二维码
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public string GetCardPartySignUpQRByUser(int PartyID)
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+appid+"&secret="+secret+"";
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
            string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;

            string page = "pages/Party/SignUpShowByUesr/SignUpShowByUesr";
            if (Type == 4)
            {
                page = "package/package_party/SignUpShowByUesr/SignUpShowByUesr";
            }

            DataJson = "{";
            DataJson += "\"scene\":\"" + PartyID + "\",";
            DataJson += string.Format("\"width\":{0},", 640);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", page);//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
            DataJson += "\"line_color\":{";
            DataJson += string.Format("\"r\":\"{0}\",", "0");
            DataJson += string.Format("\"g\":\"{0}\",", "0");
            DataJson += string.Format("\"b\":\"{0}\"", "0");
            DataJson += "},";
            DataJson += "\"is_hyaline\":false";
            DataJson += "}";

            Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
            Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。
            //保存
            string filePath = "";
            string folder = "/UploadFolder/CardPartySignByUserFile/";
            string newFileName = PartyID + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string Cardimg = ConfigInfo.Instance.APIURL + filePath;
            CardPartyVO cVO = new CardPartyVO();
            cVO.PartyID = PartyID;
            cVO.QRSignInImg = Cardimg;
            UpdateParty(cVO);
            return Cardimg;
        }

        /// <summary>
        /// 获取签到二维码 携带用户信息
        /// </summary>
        /// <param name="QuestionnaireID"></param>
        /// <param name="CustomerId"></param>
        /// <returns>返回一个图片的名字xx.png</returns>
        public string GetQuestionnaireSignupQR(Int64 QuestionnaireID, Int64 InviterCID)
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret + "";
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
            string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;

            string page = "pages/index/SignInFormByUser/SignInFormByUser";
            if (Type == 4)
            {
                page = "package/package_form/SignInFormByUser/SignInFormByUser";
            }

            DataJson = "{";
            DataJson += "\"scene\":\"" + QuestionnaireID + "-" + InviterCID + "\",";
            //DataJson += "\"scene\":\"123456\",";
            DataJson += string.Format("\"width\":{0},", 1280);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", page);//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
            DataJson += "\"line_color\":{";
            DataJson += string.Format("\"r\":\"{0}\",", "0");
            DataJson += string.Format("\"g\":\"{0}\",", "0");
            DataJson += string.Format("\"b\":\"{0}\"", "0");
            DataJson += "},";
            DataJson += "\"is_hyaline\":false";
            DataJson += "}";

            Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
            Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。
            //保存
            string filePath = "";
            string folder = "/UploadFolder/QuestionnaireSignupQRFile/";
            string newFileName = QuestionnaireID + "_" + InviterCID + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string Cardimg = ConfigInfo.Instance.APIURL + filePath;

            return Cardimg;
        }

        /// <summary>
        /// 获取会员推广海报
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public string GetCardPoster(int CustomerId)
        {
            try
            {
                string QRimg = GetCardPosterQR(CustomerId);
                Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.APIURLByHttp + "/CustomerManagement/CardIMGByPoster.aspx?QRimg=" + QRimg, 750, 1333, 750, 1333);

                string filePath = "";
                string folder = "/UploadFolder/GetCardPosterFile/";
                string newFileName = CustomerId + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                filePath = folder + newFileName;

                //可以修改为网络路径
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string physicalPath = localPath + newFileName;

                m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                string PosterImg = ConfigInfo.Instance.APIURL + filePath;
                return PosterImg;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取会员推广二维码
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public string GetCardPosterQR(int CustomerId)
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret + "";
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
            string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;
            DataJson = "{";
            DataJson += "\"scene\":\"" + CustomerId + "\",";
            DataJson += string.Format("\"width\":{0},", 640);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", "pages/index/index");//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
            DataJson += "\"line_color\":{";
            DataJson += string.Format("\"r\":\"{0}\",", "0");
            DataJson += string.Format("\"g\":\"{0}\",", "0");
            DataJson += string.Format("\"b\":\"{0}\"", "0");
            DataJson += "},";
            DataJson += "\"is_hyaline\":false";
            DataJson += "}";

            Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
            Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。
            //保存
            string filePath = "";
            string folder = "/UploadFolder/GetCardPosterQR/";
            string newFileName = CustomerId + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string Cardimg = ConfigInfo.Instance.APIURL + filePath;
            return Cardimg;
        }

        /// <summary>
        /// 获取会员推广VIP购买专属链接海报
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public string GetCardVipPoster(int CustomerId)
        {
            try
            {
                string QRimg = GetCardVipPosterQR(CustomerId);
                Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.APIURLByHttp + "/CustomerManagement/CardIMGByVipPoster.aspx?QRimg=" + QRimg+ "&customerId="+ CustomerId, 500, 762, 500, 762);

                string filePath = "";
                string folder = "/UploadFolder/GetCardVipPosterFile/";
                string newFileName = CustomerId + ".jpg";
                filePath = folder + newFileName;

                //可以修改为网络路径
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string physicalPath = localPath + newFileName;

                m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                string PosterImg = ConfigInfo.Instance.APIURL + filePath;
                return PosterImg;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取会员推广VIP购买专属链接二维码
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public string GetCardVipPosterQR(int CustomerId)
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret + "";
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
            string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;
            DataJson = "{";
            DataJson += "\"scene\":\"" + CustomerId + "\",";
            DataJson += string.Format("\"width\":{0},", 640);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", "package/package_vip/VipDistribution/VipDistribution");//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
            DataJson += "\"line_color\":{";
            DataJson += string.Format("\"r\":\"{0}\",", "0");
            DataJson += string.Format("\"g\":\"{0}\",", "0");
            DataJson += string.Format("\"b\":\"{0}\"", "0");
            DataJson += "},";
            DataJson += "\"is_hyaline\":false";
            DataJson += "}";

            Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
            Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。
            //保存
            string filePath = "";
            string folder = "/UploadFolder/GetCardVipPosterQR/";
            string newFileName = CustomerId + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string Cardimg = ConfigInfo.Instance.APIURL + filePath;
            return Cardimg;
        }

        /// <summary>
        /// 发送客服模板信息(收到回递名片后通知)
        /// </summary>
        /// <param name="CardID">要递的名片ID</param>
        /// <param name="CustomerId">接收人ID</param>
        /// <param name="formId"></param>
        /// <returns></returns>
        public string sendTemplateMessage(int CardID, CardSendVO sVO)
        {
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+appid+"&secret="+secret+"";
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
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + result.SuccessResult.access_token;

                CardDataVO cVO = FindCardById(CardID);


                if (sVO != null&&sVO.FormId!="null")
                {
                    DataJson = "{";
                    DataJson += "\"touser\": \"" + sVO.OpenId + "\",";
                    DataJson += "\"template_id\": \"gFqG8RXUc7eSim6Sn6R9JJUO40n1lAZO7NPJtJ10xhk\",";
                    DataJson += "\"page\": \"pages/ShowCard/ShowCard?scene=" + CardID + "\",";
                    DataJson += "\"form_id\": \"" + sVO.FormId + "\",";
                    DataJson += "\"data\": {";
                    DataJson += "\"keyword1\": {";
                    DataJson += "\"value\": \"" + cVO.Name + "\"";
                    DataJson += "},";
                    DataJson += "\"keyword2\": {";
                    DataJson += "\"value\": \"" + cVO.Position + "\"";
                    DataJson += "},";
                    DataJson += "\"keyword3\": {";
                    DataJson += "\"value\": \"" + cVO.CorporateName + "\"";
                    DataJson += "},";
                    DataJson += "\"keyword4\": {";
                    DataJson += "\"value\": \"" + cVO.Name + "接收了您回递的名片，点击查看\"";
                    DataJson += "}";
                    DataJson += "}";
                    DataJson += "}";

                    string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);
                    return str;
                }
                return "找不到会员";
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 发送模板信息（审核通过）
        /// </summary>
        /// <param name="GroupCardID"></param>
        /// <returns></returns>
        public string sendAllowMessage(int GroupCardID)
        {
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+appid+"&secret="+secret+"";
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
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + result.SuccessResult.access_token;


                CardGroupCardVO cVO = FindCardGroupCardByID(GroupCardID);
                AddCardMessage("管理员同意了你的进群申请", cVO.CustomerId, "进群申请", "/package/package_group/CardGroupDetail/CardGroupDetail?GroupID=" + cVO.GroupID);
                return "成功";
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 发送模板信息（不通过）
        /// </summary>
        /// <param name="GroupCardID"></param>
        /// <returns></returns>
        public string sendRefuseMessage(int GroupCardID)
        {
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+appid+"&secret="+secret+"";
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
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + result.SuccessResult.access_token;


                CardGroupCardVO cVO = FindCardGroupCardByID(GroupCardID);
                AddCardMessage("管理员拒绝了你的进群申请", cVO.CustomerId, "进群申请", "/pages/CardGroupJoin/CardGroupJoin?GroupID=" + cVO.GroupID);
                return "成功";
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 发送模板信息（报名成功通知）
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <returns></returns>
        public string sendSignUpMessage(int PartySignUpID)
        {
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+appid+"&secret="+secret+"";
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
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + result.SuccessResult.access_token;


                CardPartySignUpViewVO cVO = FindSignUpViewById(PartySignUpID);

                if (cVO != null && cVO.FormId != "null")
                {
                    DataJson = "{";
                    DataJson += "\"touser\": \"" + cVO.OpenId + "\",";
                    DataJson += "\"template_id\": \"i45GK4RaKNdaZ0sQ6hgJp4lz4vBVBSitxzHOGe5XKBY\",";
                    DataJson += "\"page\": \"pages/Party/SignUpShow/SignUpShow?PartySignUpID=" + PartySignUpID + "\",";
                    DataJson += "\"form_id\": \"" + cVO.FormId + "\",";
                    DataJson += "\"data\": {";
                    DataJson += "\"keyword1\": {";
                    DataJson += "\"value\": \"" + cVO.Title + "\"";
                    DataJson += "},";
                    DataJson += "\"keyword2\": {";
                    DataJson += "\"value\": \"" + cVO.StartTime.ToString("yyyy年MM月dd日 HH点mm分") + "\"";
                    DataJson += "},";
                    DataJson += "\"keyword3\": {";
                    DataJson += "\"value\": \"" + cVO.Address + "\"";
                    DataJson += "},";
                    DataJson += "\"keyword4\": {";
                    DataJson += "\"value\": \"" + cVO.Name + "\"";
                    DataJson += "},";
                    DataJson += "\"keyword5\": {";
                    DataJson += "\"value\": \"点击查看入场券\"";
                    DataJson += "},";
                    DataJson += "\"keyword6\": {";
                    DataJson += "\"value\": \"入场券\"";
                    DataJson += "}";
                    DataJson += "},";
                    DataJson += "\"emphasis_keyword\": \"keyword6.DATA\"";
                    DataJson += "}";

                    string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);
                    return str;
                }
                return "找不到会员";
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 发送模板信息（活动即将开始）
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <returns></returns>
        public string sendPartyStartMessage(int PartySignUpID)
        {
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+appid+"&secret="+secret+"";
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
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + result.SuccessResult.access_token;


                CardPartySignUpViewVO cVO = FindSignUpViewById(PartySignUpID);

                if (cVO != null && cVO.FormId != "null")
                {
                    DataJson = "{";
                    DataJson += "\"touser\": \"" + cVO.OpenId + "\",";
                    DataJson += "\"template_id\": \"jOE8ON-AoSwafRDWpxMw71UK7zCcbxILPFlLTNfRv3E\",";
                    DataJson += "\"page\": \"pages/Party/SignUpShow/SignUpShow?PartySignUpID=" + PartySignUpID + "\",";
                    DataJson += "\"form_id\": \"" + cVO.FormId + "\",";
                    DataJson += "\"data\": {";
                    DataJson += "\"keyword1\": {";
                    DataJson += "\"value\": \"" + cVO.Title + "\"";
                    DataJson += "},";
                    DataJson += "\"keyword2\": {";
                    DataJson += "\"value\": \"" + cVO.StartTime.ToString("yyyy年MM月dd日 HH点mm分") + "\"";
                    DataJson += "},";
                    DataJson += "\"keyword3\": {";
                    DataJson += "\"value\": \"" + cVO.Address + "\"";
                    DataJson += "},";
                    DataJson += "\"keyword4\": {";
                    DataJson += "\"value\": \"您所报名的活动即将开始，请注意时间安排 - 点击查看入场券\"";
                    DataJson += "}";
                    DataJson += "}";
                    DataJson += "}";

                    string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);
                    return str;
                }
                return "找不到会员";
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 发送模板信息（每日记录提醒）
        /// </summary>
        /// <param name="FormListID"></param>
        /// <returns></returns>
        public string sendDailyRecordMessage(int FormListID, string text)
        {
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+appid+"&secret="+secret+"";
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
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + result.SuccessResult.access_token;

                CardFormListVO fVO = FindFormListById(FormListID);


                if (fVO != null && fVO.FormId != "null")
                {
                    List<CardDataVO> cVO = FindCardByCustomerId(fVO.CustomerId);

                    if (cVO.Count > 0)
                    {
                        string Title = "";

                        DateTime dt = DateTime.Now;
                        int Hour = dt.Hour;

                        /*
                        string Hourstr = "";
                        if (Hour < 6)
                            Hourstr = "晚上";
                        else if (Hour >= 6 && Hour < 12)
                            Hourstr = "早上";
                        else if (Hour >= 12 && Hour < 14)
                            Hourstr = "中午";
                        else if (Hour >= 14 && Hour < 18)
                            Hourstr = "下午";
                        else if (Hour >= 18 && Hour <= 24)
                            Hourstr = "晚上";
                        */

                        if (cVO[0].ReadCount > 0)
                        {
                            Title = "您的名片累计有" + cVO[0].ReadCount + "位访客哦";
                        }
                        else
                        {
                            Title = "您的名片还没有访客哦";
                        }

                        DataJson = "{";
                        DataJson += "\"touser\": \"" + fVO.OpenId + "\",";
                        DataJson += "\"template_id\": \"-fw3LG_4PllhQ7P0T-0tC8bODGt56GzVB1KddqH10S8\",";
                        DataJson += "\"page\": \"pages/index/index\",";
                        DataJson += "\"form_id\": \"" + fVO.FormId + "\",";
                        DataJson += "\"data\": {";
                        DataJson += "\"keyword1\": {";
                        DataJson += "\"value\": \"" + text + "\"";
                        DataJson += "},";
                        DataJson += "\"keyword2\": {";
                        DataJson += "\"value\": \"" + Title + "\"";
                        DataJson += "},";
                        DataJson += "\"keyword3\": {";
                        DataJson += "\"value\": \"今天是" + string.Format("{0:yyyy-MM-dd dddd}", DateTime.Now) + "\"";
                        DataJson += "}";
                        DataJson += "}";
                        DataJson += "}";

                        string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);

                        DeleteFormIdById(FormListID);
                        return str;
                    }
                    else
                    {
                        return "找不到名片";
                    }

                }
                return "找不到FormID";
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "error";
            }
        }

        /// <summary>
        /// 发送模板信息（今日话题推送）
        /// </summary>
        /// <param name="FormListID"></param>
        /// <returns></returns>
        public string sendCardNewsMessage(int FormListID)
        {
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+appid+"&secret="+secret+"";
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
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + result.SuccessResult.access_token;

                CardFormListVO fVO = FindFormListById(FormListID);
                if (fVO != null && fVO.FormId != "null")
                {
                    List<CardNewsVO> cVO = FindCardNewsList();

                    if (cVO.Count > 0)
                    {
                        DataJson = "{";
                        DataJson += "\"touser\": \"" + fVO.OpenId + "\",";
                        DataJson += "\"template_id\": \"EHpRyekEn7zcDnzA7aMClrxZQjAKfQp4nI-no0hpcX8\",";
                        DataJson += "\"page\": \"pages/index/index\",";
                        DataJson += "\"form_id\": \"" + fVO.FormId + "\",";
                        DataJson += "\"data\": {";
                        DataJson += "\"keyword1\": {";
                        DataJson += "\"value\": \"" + cVO[0].Title + "\"";
                        DataJson += "},";
                        DataJson += "\"keyword2\": {";
                        DataJson += "\"value\": \"" + cVO[0].Synopsis + "\"";
                        DataJson += "}";
                        DataJson += "}";
                        DataJson += "}";

                        string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);

                        DeleteFormIdById(FormListID);
                        return str;
                    }
                    else
                    {
                        return "找不到新闻";
                    }

                }
                return "找不到FormID";
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "error";
            }
        }


        /// <summary>
        /// 发送模板信息（零钱到账通知）
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="formId"></param>
        /// <param name="money"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string sendMoneyToPacket(string openId,string formId,decimal money,string content)
        {
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+appid+"&secret="+secret+"";
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
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + result.SuccessResult.access_token;


                DataJson = "{";
                DataJson += "\"touser\": \"" + openId + "\",";
                DataJson += "\"template_id\": \"dAqzKy6Xw_4f3YGB2jVZxxCvMor0Dz4a-AiRIOBmWQ4\",";
                DataJson += "\"page\": \"pages/index/index\",";
                DataJson += "\"form_id\": \"" + formId + "\",";
                DataJson += "\"data\": {";
                DataJson += "\"keyword1\": {";
                DataJson += "\"value\": \"" + money + "元\"";
                DataJson += "},";
                DataJson += "\"keyword2\": {";
                DataJson += "\"value\": \"" + DateTime.Now + "\"";
                DataJson += "},";
                DataJson += "\"keyword3\": {";
                DataJson += "\"value\": \""+ content + "\"";
                DataJson += "}";
                DataJson += "}";
                DataJson += "}";

                string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);

                        return str;
                  
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "error";
            }
        }

        /// <summary>
        /// 提现成功通知
        /// </summary>
        /// <param name="PayOutHistoryId"></param>
        /// <returns></returns>
        public string sendPayOutMessage(int PayOutHistoryId)
        {
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+appid+"&secret="+secret+"";
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
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + result.SuccessResult.access_token;


                CardPayOutVO cVO = FindPayOutViewById(PayOutHistoryId);

                if (cVO != null)
                {
                    //站内提醒
                    AddCardMessage(cVO.Cost + "元提现成功!已转账，请注意查收！", cVO.CustomerId, "资金提现", "");

                    //发送通知
                    DataJson = "{";
                    DataJson += "\"touser\": \"" + cVO.OpenId + "\",";
                    DataJson += "\"template_id\": \"npTLBo0KJgR28DzENQ4Q_QOXNRTRhwCh9RNafmlHHEY\",";
                    DataJson += "\"page\": \"package/package_order/PayOutShow/PayOutShow?PayoutHistoryId=" + PayOutHistoryId + "\",";
                    DataJson += "\"form_id\": \"" + cVO.FormId + "\",";
                    DataJson += "\"data\": {";
                    DataJson += "\"keyword1\": {";
                    DataJson += "\"value\": \"" + cVO.BankAccount + "\"";
                    DataJson += "},";
                    DataJson += "\"keyword2\": {";
                    DataJson += "\"value\": \"" + cVO.BankName + "\"";
                    DataJson += "},";
                    DataJson += "\"keyword3\": {";
                    DataJson += "\"value\": \"" + cVO.AccountName + "\"";
                    DataJson += "},";
                    DataJson += "\"keyword4\": {";
                    DataJson += "\"value\": \"" + cVO.PayOutCost + "元\"";
                    DataJson += "},";
                    DataJson += "\"keyword5\": {";
                    DataJson += "\"value\": \"" + cVO.ServiceCharge + "元\"";
                    DataJson += "},";
                    DataJson += "\"keyword6\": {";
                    DataJson += "\"value\": \"" + cVO.Cost + "元\"";
                    DataJson += "},";
                    DataJson += "\"keyword7\": {";
                    DataJson += "\"value\": \"已转账，请注意查收\"";
                    DataJson += "}";
                    DataJson += "}";
                    DataJson += "}";

                    string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);
                    return str;
                }
                return "找不到会员";
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 提现失败通知
        /// </summary>
        /// <param name="PayOutHistoryId"></param>
        /// <returns></returns>
        public string sendPayOutMessageBYoff(int PayOutHistoryId)
        {
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+appid+"&secret="+secret+"";
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
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + result.SuccessResult.access_token;


                CardPayOutVO cVO = FindPayOutViewById(PayOutHistoryId);

                if (cVO != null)
                {
                    //站内提醒
                    AddCardMessage("提现失败!提现资金已退回至账户,点击查看原因！", cVO.CustomerId, "资金提现", "/package/package_order/PayOutShow/PayOutShow");

                    //发送模板
                    DataJson = "{";
                    DataJson += "\"touser\": \"" + cVO.OpenId + "\",";
                    DataJson += "\"template_id\": \"lDOTPNNc8Fy4-VemSd43DtvYwNcF84Y_IsI9OomsAEc\",";
                    DataJson += "\"page\": \"package/package_order/PayOutShow/PayOutShow?PayoutHistoryId=" + PayOutHistoryId + "\",";
                    DataJson += "\"form_id\": \"" + cVO.FormId + "\",";
                    DataJson += "\"data\": {";
                    DataJson += "\"keyword1\": {";
                    DataJson += "\"value\": \"" + cVO.PayOutCost + "元\"";
                    DataJson += "},";
                    DataJson += "\"keyword2\": {";
                    DataJson += "\"value\": \"" + cVO.HandleDate + "\"";
                    DataJson += "},";
                    DataJson += "\"keyword3\": {";
                    DataJson += "\"value\": \"提现资金已退回至账户\"";
                    DataJson += "},";
                    DataJson += "\"keyword4\": {";
                    DataJson += "\"value\": \"" + cVO.HandleComment + "\"";
                    DataJson += "}";
                    DataJson += "}";
                    DataJson += "}";

                    string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);
                    return str;
                }
                return "找不到会员";
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string getOpenId(string code)
        {
            string jsonStr = "";
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid="+appid+"&secret="+secret+"&js_code=" + code + "&grant_type=authorization_code";
                jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var result = new WeiXinAccessTokenResult();
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModel>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }
                return result.SuccessResult.openid;
            }
            catch(Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source+ "\r\n jsonStr="+ jsonStr+" \r\n code=" + code;
                _log.Error(strErrorMsg);
                return "error";
            }
        }

        /// <summary>
        /// 获取Access_token
        /// </summary>
        /// <returns></returns>
        public string GetAccess_token()
        {
            try
            {
                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+appid+"&secret="+secret+"";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var result = new WeiXinAccessTokenResult();
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModel>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }

                return result.SuccessResult.access_token;
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 审核文本是否合法,true为合法，false为非法
        /// </summary>
        /// <returns></returns>
        public bool msg_sec_check(object obj)
        {
            try
            {
                string content = ObjectToJson(obj);
                content = Regex.Replace(content,",", "隔断");
                content = Regex.Replace(content, @"[^\u4e00-\u9fa5]", "");
                content = Regex.Replace(content, "姓名", "");
                content = Regex.Replace(content, "手机", "");
                content = Regex.Replace(content, "工作单位", "");
                content = Regex.Replace(content, "单位地址", "");
                content = Regex.Replace(content, "职位", "");
                content = Regex.Replace(content, "性别", "");
                content = Regex.Replace(content, "微信", "");
                content = Regex.Replace(content, "人数", "");
                content = Regex.Replace(content, @"[隔断]+", "隔断");
                content = Regex.Replace(content, "隔断", "【隔断】");

                string url = "https://api.weixin.qq.com/wxa/msg_sec_check?access_token="+ GetAccess_token();
                string DataJson = "{";
                DataJson += "\"content\": \"" + content + "\"";
                DataJson += "}";

                string jsonStr = HttpHelper.HtmlFromUrlPost(url, DataJson);

                LogBO _log = new LogBO(typeof(CardBO));
                var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);

                if (errorResult.errcode == 87014)
                {
                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    ViolationVO ViolationVO = new ViolationVO();
                    ViolationVO.ViolationAt = DateTime.Now;
                    ViolationVO.ViolationText = content;
                    uBO.AddViolation(ViolationVO);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// 内存对象转换为json字符串
         /// </summary>
         /// <param name="obj"></param>
         /// <returns></returns>
         public static string ObjectToJson(object obj)
         {
            if (obj == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 获取经纬度
        /// </summary>
        /// <returns></returns>
        public WeiXinGeocoder getLatitudeAndLongitude(string address)
        {
            try
            {
                string url = "https://apis.map.qq.com/ws/geocoder/v1/?key=QHBBZ-ZDAWI-TRVGE-5GWEO-PHEKO-Q5BXC&address=" + address;

                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var errorResult = JsonConvert.DeserializeObject<WeiXinGeocoder>(responseString);

                if (errorResult.status == 0)
                    return errorResult;
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CompanyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 添加活动
        /// </summary>
        /// <param name="vo">活动VO</param>
        /// <param name="CardPartySignUpFormVOList">报名信息VO列表</param>
        /// <param name="ContactsList">联系人VO列表</param>
        /// <param name="CardPartyCostVOList">费用VO列表</param>
        /// <returns></returns>
        public int AddParty(CardPartyVO vo, List<CardPartyContactsVO> ContactsList, List<CardPartySignUpFormVO> CardPartySignUpFormVOList, List<CardPartyCostVO> CardPartyCostVOList,bool isAutoPay=false)
        {
            try
            {
                ICardPartyDAO pDAO = CustomerManagementDAOFactory.CardPartyDAO(this.CurrentCustomerProfile);
                ICardPartySignUpFormDAO pFormDAO = CustomerManagementDAOFactory.CardPartySignUpFormDAO(this.CurrentCustomerProfile);
                ICardPartyContactsDAO pContactsDAO = CustomerManagementDAOFactory.CardPartyContactsDAO(this.CurrentCustomerProfile);
                ICardPartyCostDAO pCostDAO = CustomerManagementDAOFactory.CardPartyCostDAO(this.CurrentCustomerProfile);


                CardPartySignUpFormVO fVO = new CardPartySignUpFormVO();
                fVO.Name = "姓名";
                fVO.must = 1;
                fVO.Status = 2;
                
                CardPartySignUpFormVO fVO2 = new CardPartySignUpFormVO();
                fVO2.Name = "手机";
                fVO2.must = 1;
                fVO2.Status = 2;

                bool isName = false;
                bool isPhone = false;

                foreach (CardPartySignUpFormVO item in CardPartySignUpFormVOList)
                {
                    if(item.Name== fVO.Name)
                    {
                        isName = true;
                    }
                    if (item.Name == fVO2.Name)
                    {
                        isPhone = true;
                    }
                }

                if (!isName) CardPartySignUpFormVOList.Add(fVO);
                if (!isPhone) CardPartySignUpFormVOList.Add(fVO2);

                if (CardPartyCostVOList == null)
                {
                    CardPartyCostVOList = new List<CardPartyCostVO>();
                }

                if (ContactsList == null)
                {
                    ContactsList = new List<CardPartyContactsVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int PartyID = pDAO.Insert(vo);

                    foreach (CardPartySignUpFormVO bcVO in CardPartySignUpFormVOList)
                    {
                        bcVO.PartyID = PartyID;
                        bcVO.AppType = AppType;
                    }

                    pFormDAO.InsertList(CardPartySignUpFormVOList, 100);

                    foreach (CardPartyCostVO pcVO in CardPartyCostVOList)
                    {
                        pcVO.PartyID = PartyID;
                        if (!isAutoPay)
                        {
                            pcVO.isAutoPay = 0;
                        }
                        pcVO.AppType = AppType;
                    }

                    pCostDAO.InsertList(CardPartyCostVOList, 100);

                    foreach (CardPartyContactsVO tcVO in ContactsList)
                    {
                        tcVO.PartyID = PartyID;
                        tcVO.AppType = AppType;
                    }

                    pContactsDAO.InsertList(ContactsList, 100);

                    return PartyID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新活动
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateParty(CardPartyVO vo)
        {
            ICardPartyDAO pDAO = CustomerManagementDAOFactory.CardPartyDAO(this.CurrentCustomerProfile);
            try
            {
                pDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 更新活动联系人
        /// </summary>
        /// <param name="vo">活动VO</param>
        /// <param name="CostList">费用VO列表</param>
        /// <param name="ContactsList">联系人VO列表</param>
        /// <returns></returns>
        public bool UpdateParty(CardPartyVO vo, List<CardPartyContactsVO> ContactsList)
        {
            try
            {
                ICardPartyDAO pDAO = CustomerManagementDAOFactory.CardPartyDAO(this.CurrentCustomerProfile);
                ICardPartyCostDAO pCostDAO = CustomerManagementDAOFactory.CardPartyCostDAO(this.CurrentCustomerProfile);
                ICardPartyContactsDAO pContactsDAO = CustomerManagementDAOFactory.CardPartyContactsDAO(this.CurrentCustomerProfile);
                /*
                if (CostList == null)
                {
                    CostList = new List<CardPartyCostVO>();
                }*/

                if (ContactsList == null)
                {
                    ContactsList = new List<CardPartyContactsVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    try
                    {
                        if (vo != null)
                            pDAO.UpdateById(vo);
                    }
                    catch
                    {

                    }


                    //删除不存在的，添加新增的
                    /*
                    List<CardPartyCostVO> rtcDBVOList = pCostDAO.FindByParams("PartyID = " + vo.PartyID);
                    List<CardPartyCostVO> rtcdeleteVOList = new List<CardPartyCostVO>();
                    foreach (CardPartyCostVO dbVO in rtcDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = CostList.Count - 1; i >= 0; i--)
                        {
                            CardPartyCostVO bcVO = CostList[i];
                            if (bcVO.PartyCostID == dbVO.PartyCostID)
                            {
                                CostList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            rtcdeleteVOList.Add(dbVO);
                        }
                    }
                    if (CostList != null)
                        pCostDAO.InsertList(CostList, 100);
                    foreach (CardPartyCostVO deleteVO in rtcdeleteVOList)
                    {
                        pCostDAO.DeleteById(deleteVO.PartyCostID);
                    }
                    */

                    //删除不存在的，添加新增的
                    List<CardPartyContactsVO> rcsDBVOList = pContactsDAO.FindByParams("PartyID = " + vo.PartyID);
                    List<CardPartyContactsVO> rcsdeleteVOList = new List<CardPartyContactsVO>();
                    foreach (CardPartyContactsVO dbVO in rcsDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = ContactsList.Count - 1; i >= 0; i--)
                        {
                            CardPartyContactsVO bcVO = ContactsList[i];
                            if (bcVO.PartyContactsID == dbVO.PartyContactsID)
                            {
                                ContactsList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            rcsdeleteVOList.Add(dbVO);
                        }
                    }
                    if (ContactsList != null)
                        pContactsDAO.InsertList(ContactsList, 100);
                    foreach (CardPartyContactsVO deleteVO in rcsdeleteVOList)
                    {
                        pContactsDAO.DeleteById(deleteVO.PartyContactsID);
                    }
                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 更新活动联系人
        /// </summary>
        /// <param name="vo">活动VO</param>
        /// <param name="CostList">费用VO列表</param>
        /// <param name="ContactsList">联系人VO列表</param>
        /// <returns></returns>
        public bool UpdateParty(CardPartyVO vo, List<CardPartyContactsVO> ContactsList, List<CardPartySignUpFormVO> CardPartySignUpFormVOList, List<CardPartyCostVO> CardPartyCostVOList)
        {
            try
            {
                ICardPartyDAO pDAO = CustomerManagementDAOFactory.CardPartyDAO(this.CurrentCustomerProfile);
                ICardPartySignUpFormDAO pFormDAO = CustomerManagementDAOFactory.CardPartySignUpFormDAO(this.CurrentCustomerProfile);
                ICardPartyContactsDAO pContactsDAO = CustomerManagementDAOFactory.CardPartyContactsDAO(this.CurrentCustomerProfile);
                ICardPartyCostDAO pCostDAO = CustomerManagementDAOFactory.CardPartyCostDAO(this.CurrentCustomerProfile);


                CardPartySignUpFormVO fVO = new CardPartySignUpFormVO();
                fVO.Name = "姓名";
                fVO.must = 1;
                fVO.Status = 2;

                CardPartySignUpFormVO fVO2 = new CardPartySignUpFormVO();
                fVO2.Name = "手机";
                fVO2.must = 1;
                fVO2.Status = 2;

                bool isName = false;
                bool isPhone = false;

                foreach (CardPartySignUpFormVO item in CardPartySignUpFormVOList)
                {
                    if (item.Name == fVO.Name)
                    {
                        isName = true;
                    }
                    if (item.Name == fVO2.Name)
                    {
                        isPhone = true;
                    }
                }

                if (!isName) CardPartySignUpFormVOList.Add(fVO);
                if (!isPhone) CardPartySignUpFormVOList.Add(fVO2);

                if (CardPartyCostVOList == null)
                {
                    CardPartyCostVOList = new List<CardPartyCostVO>();
                }

                if (ContactsList == null)
                {
                    ContactsList = new List<CardPartyContactsVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    try
                    {
                        if (vo != null)
                            pDAO.UpdateById(vo);
                    }
                    catch
                    {

                    }


                    //删除原来的项，添加新增的

                    List<CardPartySignUpFormVO> rtcDBVOList = pFormDAO.FindByParams("PartyID = " + vo.PartyID);
                    foreach (CardPartySignUpFormVO deleteVO in rtcDBVOList)
                    {
                        pFormDAO.DeleteById(deleteVO.SignUpFormID);
                    }
                    foreach (CardPartySignUpFormVO bcVO in CardPartySignUpFormVOList)
                    {
                        bcVO.PartyID = vo.PartyID;
                    }
                    if (CardPartySignUpFormVOList != null)
                        pFormDAO.InsertList(CardPartySignUpFormVOList, 100);

                    //删除原来的项，添加新增的

                    List<CardPartyCostVO> rCostDBVOList = pCostDAO.FindByParams("PartyID = " + vo.PartyID);
                    bool isAutoPay = false;
                    foreach (CardPartyCostVO deleteVO in rCostDBVOList)
                    {
                        if (deleteVO.isAutoPay == 1)
                        {
                            isAutoPay = true;
                        }
                        pCostDAO.DeleteById(deleteVO.PartyCostID);
                    }
                    foreach (CardPartyCostVO bcVO in CardPartyCostVOList)
                    {
                        if (!isAutoPay)
                        {
                            bcVO.isAutoPay = 0;
                        }
                        bcVO.PartyID = vo.PartyID;
                    }
                    if (CardPartyCostVOList != null)
                        pCostDAO.InsertList(CardPartyCostVOList, 100);


                    //删除不存在的，添加新增的
                    List<CardPartyContactsVO> rcsDBVOList = pContactsDAO.FindByParams("PartyID = " + vo.PartyID);
                    List<CardPartyContactsVO> rcsdeleteVOList = new List<CardPartyContactsVO>();
                    foreach (CardPartyContactsVO dbVO in rcsDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = ContactsList.Count - 1; i >= 0; i--)
                        {
                            CardPartyContactsVO bcVO = ContactsList[i];
                            if (bcVO.PartyContactsID == dbVO.PartyContactsID)
                            {
                                ContactsList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            rcsdeleteVOList.Add(dbVO);
                        }
                    }
                    if (ContactsList != null)
                        pContactsDAO.InsertList(ContactsList, 100);
                    foreach (CardPartyContactsVO deleteVO in rcsdeleteVOList)
                    {
                        pContactsDAO.DeleteById(deleteVO.PartyContactsID);
                    }
                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 活动信息VO转换为活动视图VO
        /// </summary>
        /// <param name="PartyVO"></param>
        /// <returns></returns>
        public CardPartyViewVO getPartyView(CardPartyVO PartyVO)
        {
            CardPartyViewVO ViewVO = new CardPartyViewVO();

            ViewVO.PartyID=PartyVO.PartyID;
            ViewVO.CustomerId = PartyVO.CustomerId;
            ViewVO.Title=PartyVO.Title;
            ViewVO.PartyTag=PartyVO.PartyTag;
            ViewVO.MainImg=PartyVO.MainImg;
            ViewVO.StartTime=PartyVO.StartTime;
            ViewVO.EndTime=PartyVO.EndTime;
            ViewVO.SignUpTime=PartyVO.SignUpTime;
            ViewVO.CreatedAt=PartyVO.CreatedAt;
            ViewVO.Address=PartyVO.Address;
            ViewVO.latitude=PartyVO.latitude;
            ViewVO.longitude=PartyVO.longitude;
            ViewVO.DetailedAddress = PartyVO.DetailedAddress;
            ViewVO.Details=PartyVO.Details;
            ViewVO.Details2=PartyVO.Details2;
            ViewVO.QRCodeImg=PartyVO.QRCodeImg;
            ViewVO.Status=PartyVO.Status;
            ViewVO.GroupID=PartyVO.GroupID;
            ViewVO.PosterImg=PartyVO.PosterImg;
            ViewVO.isDisplayContacts=PartyVO.isDisplayContacts;
            ViewVO.isDisplaySignup=PartyVO.isDisplaySignup;
            ViewVO.isClickSignup=PartyVO.isClickSignup;
            ViewVO.isDisplayCost=PartyVO.isDisplayCost;
            ViewVO.isPromotionAward=PartyVO.isPromotionAward;
            ViewVO.isStartTime=PartyVO.isStartTime;
            ViewVO.isEndTime=PartyVO.isEndTime;
            ViewVO.Host=PartyVO.Host;
            ViewVO.ReadCount=PartyVO.ReadCount;
            ViewVO.Forward=PartyVO.Forward;
            ViewVO.limitPeopleNum=PartyVO.limitPeopleNum;
            ViewVO.Content=PartyVO.Content;
            ViewVO.PersonalID=PartyVO.PersonalID;
            ViewVO.BusinessID=PartyVO.BusinessID;
            ViewVO.Audio=PartyVO.Audio;
            ViewVO.AudioName=PartyVO.AudioName;
            ViewVO.style=PartyVO.style;
            ViewVO.Type=PartyVO.Type;

            return ViewVO;
    }

        /// <summary>
        /// 是否是活动联系人
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool isPartyContacts(int PartyID, int CustomerId)
        {
            ICardPartyContactsViewDAO pContactsDAO = CustomerManagementDAOFactory.CardPartyContactsViewDAO(this.CurrentCustomerProfile);
            return pContactsDAO.FindTotalCount("PartyID=" + PartyID + " and CustomerId=" + CustomerId + " and AppType=" + AppType) > 0;
        }

        public bool UpdatePartyA(CardPartyVO vo)
        {
            ICardPartyDAO pDAO = CustomerManagementDAOFactory.CardPartyDAO(this.CurrentCustomerProfile);
            try
            {
                pDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public List<CardPartyContactsVO> FindPartyContacts(int PartyId)
        {
            ICardPartyContactsDAO pContactsDAO = CustomerManagementDAOFactory.CardPartyContactsDAO(this.CurrentCustomerProfile);
            return pContactsDAO.FindByParams("PartyID = " + PartyId);
        }

        public CardPartyVO FindPartyById(Int64 PartyId)
        {
            ICardPartyDAO pDAO = CustomerManagementDAOFactory.CardPartyDAO(this.CurrentCustomerProfile);
            return pDAO.FindById(PartyId);
        }

        public List<CardPartyVO> FindPartybycondtion(string condtion)
        {
            ICardPartyDAO pDAO = CustomerManagementDAOFactory.CardPartyDAO(this.CurrentCustomerProfile);
            return pDAO.FindByParams(condtion);
        }

        public CardPartyViewVO FindPartyViewById(Int64 PartyId)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            ICardPartyViewDAO pDAO = CustomerManagementDAOFactory.CardPartyViewDAO(this.CurrentCustomerProfile);
            CardPartyViewVO uVO = pDAO.FindById(PartyId);
            CustomerVO CVO = uBO.FindCustomenById(uVO.CustomerId);
            List<CardDataVO> CardDataVO = FindCardByCustomerId(uVO.CustomerId);

            if (CVO != null)
            {
                uVO.HeaderLogo = CVO.HeaderLogo;
                uVO.CustomerName = CVO.CustomerName;
            }

            if (CardDataVO.Count > 0)
            {
                uVO.Name = CardDataVO[0].Name;
                uVO.Headimg = CardDataVO[0].Headimg;
            }

            return uVO;
        }

        /// <summary>
        /// 获取即将开始活动列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartyViewVO> FindCardPartyViewByStart()
        {
            ICardPartyViewDAO pDAO = CustomerManagementDAOFactory.CardPartyViewDAO(this.CurrentCustomerProfile);
            return pDAO.FindByParams("StartTime <= DATE_ADD(NOW(),INTERVAL 3 HOUR) and StartTime >= NOW() and StartSendStatus=1 and Type=1 and AppType=" + AppType);
        }

        /// <summary>
        /// 获取一天后开始活动列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartyViewVO> FindCardPartyViewByDayStart()
        {
            ICardPartyViewDAO pDAO = CustomerManagementDAOFactory.CardPartyViewDAO(this.CurrentCustomerProfile);
            //string sql = "StartTime <= DATE_ADD(NOW(),INTERVAL 1 DAY) and StartTime >= NOW() and StartSendStatus=0 and Type=1 and AppType=" + AppType;
            string sql = "StartTime <= DATE_ADD(NOW(),INTERVAL 12 HOUR) and StartTime >= NOW() and StartSendStatus=0 and Type=1 and AppType=" + AppType;
            return pDAO.FindByParams(sql);
        }


        /// <summary>
        /// 获取截止报名活动列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartyViewVO> FindCardPartyViewBySignUp()
        {
            ICardPartyViewDAO pDAO = CustomerManagementDAOFactory.CardPartyViewDAO(this.CurrentCustomerProfile);
            return pDAO.FindByParams("SignUpTime <= NOW() and SignUpSendStatus=1 and Type=1 and AppType=" + AppType);
        }

        /// <summary>
        /// 获取一天后截止报名活动列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartyViewVO> FindCardPartyViewByDaySignUp()
        {
            ICardPartyViewDAO pDAO = CustomerManagementDAOFactory.CardPartyViewDAO(this.CurrentCustomerProfile);
            return pDAO.FindByParams("SignUpTime <= DATE_ADD(NOW(),INTERVAL 1 DAY) and SignUpTime >= NOW() and Type=1 and SignUpSendStatus=0 and AppType=" + AppType);
        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartyVO> FindCardPartyByCondtion(string condtion)
        {
            ICardPartyDAO pDAO = CustomerManagementDAOFactory.CardPartyDAO(this.CurrentCustomerProfile);
            return pDAO.FindByParams("AppType=" + AppType + " and " + condtion);
        }

        /// <summary>
        /// 获取热门活动列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartyVO> FindCardHotPartyByCondtion(string condtion)
        {
            ICardHotPartyDAO pDAO = CustomerManagementDAOFactory.CardHotPartyDAO(this.CurrentCustomerProfile);
            return pDAO.FindByParams("AppType=" + AppType + " and " + condtion);
        }

        /// <summary>
        /// 获取首页活动列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartyVO> FindCardIndexPartyByCondtion(string condtion)
        {
            ICardIndexPartyDAO pDAO = CustomerManagementDAOFactory.CardIndexPartyDAO(this.CurrentCustomerProfile);
            return pDAO.FindByParams("AppType=" + AppType + " and " + condtion);
        }

        /// <summary>
        /// 对即将开始的活动会员发送通知（定时器执行）
        /// </summary>
        public bool sendStartMess()
        {
            try
            {
                List<CardPartyViewVO> CardPartyViewVO = FindCardPartyViewByStart();
                for (int i = 0; i < CardPartyViewVO.Count; i++)
                {
                    try
                    {
                        List<CardPartySignUpVO> CardPartySignUpVO = FindSignUpByPartyID(CardPartyViewVO[i].PartyID);
                        for (int j = 0; j < CardPartySignUpVO.Count; j++)
                        {
                            try
                            {
                                sendPartyStartMessage(CardPartySignUpVO[j].PartySignUpID);
                            }
                            catch
                            {

                            }
                        }
                        CardPartyVO cpVO = new CardPartyVO();
                        cpVO.PartyID = CardPartyViewVO[i].PartyID;
                        cpVO.Status = 2;
                        UpdatePartyA(cpVO);
                    }
                    catch
                    {

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<CardPartyContactsViewVO> FindPartyContactsByPartyId(int PartyId)
        {
            ICardPartyContactsViewDAO uDAO = CustomerManagementDAOFactory.CardPartyContactsViewDAO(this.CurrentCustomerProfile);
            List<CardPartyContactsViewVO> cVO = uDAO.FindByParams("PartyId = " + PartyId);

            if (cVO.Count > 0)
            {
                return cVO;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 判断是否已报名活动
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns> m  
        public List<CardPartySignUpVO> isJionCardParty(int CustomerId, int PartyID)
        {
            ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
            List<CardPartySignUpVO> cVO = uDAO.FindByParams("CustomerId = " + CustomerId + " and PartyID=" + PartyID+ " and SignUpStatus<>2");
            return cVO;
        }

        /// <summary>
        /// 删除报名
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public int DeleteByPartySignUp(int PartyID)
        {
            ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("PartyID = " + PartyID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public int DeleteByParty(int PartyID)
        {
            ICardPartyDAO uDAO = CustomerManagementDAOFactory.CardPartyDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("PartyID = " + PartyID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取同个收费项所有报名
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardPartySignUpViewVO> PartyCostSignUpView(string CostName, int PartyID)
        {
            ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);
            List<CardPartySignUpViewVO> cVO = uDAO.FindByParams("CostName = '" + CostName + "' and PartyID=" + PartyID + " and PartySignUpID > 0 and SignUpStatus<>2 group by PartySignUpID ");
            return cVO;
        }

        /// <summary>
        /// 获取同个收费项所有报名
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardPartySignUpViewVO> PartyCostSignUpView(string CostName,decimal cost, int PartyID)
        {
            ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);
            List<CardPartySignUpViewVO> cVO = uDAO.FindByParams("CostName = '" + CostName + "' and Cost=" + cost + " and PartyID=" + PartyID + " and PartySignUpID > 0 and SignUpStatus<>2 group by PartySignUpID ");
            return cVO;
        }

        /// <summary>
        /// 获取活动邀约列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardPartySignUpViewVO> FindAllByPageIndexByInviter(int PartyID, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndexByInviter(PartyID, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取活动邀约数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindInviterNumTotalCount(int PartyID)
        {
            ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCountByInviter(PartyID);

        }

        /// <summary>
        /// 获取中奖的所有报名
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardPartySignUpVO> LuckDrawPartyCostSignUp(string LuckDrawNames, string LuckDrawContent, int PartyID)
        {
            ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
            List<CardPartySignUpVO> cVO = uDAO.FindByParams("LuckDrawNames = '" + LuckDrawNames + "' and LuckDrawContent = '"+ LuckDrawContent + "' and LuckDrawStatus=1 and PartyID=" + PartyID + " group by PartySignUpID ");
            return cVO;
        }

        /// <summary>
        /// 更新报名为未中奖
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public int LuckDrawSignUpIsNot(int PartyID)
        {
            ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
            return uDAO.Update("LuckDrawStatus=2", "LuckDrawStatus=0 and isAutoAdd=1 and PartyID="+ PartyID);
        }


        /// <summary>
        /// 添加报名记录到活动
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCardToParty(CardPartySignUpVO vo)
        {
            try
            {
                ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int GroupCardID = uDAO.Insert(vo);
                    return GroupCardID;
                };
                int result = t.Go();
                CardPartyVO cpvo = FindPartyById(vo.PartyID);
                if (cpvo != null)
                {
                    if (cpvo.RecordSignUpCount <= 1)
                    {
                        cpvo.RecordSignUpCount = FindCardPartSignInSumCount("Number", "PartyID=" + cpvo.PartyID + " and (SignUpStatus=1 or SignUpStatus=0)");
                    }else
                    {
                        cpvo.RecordSignUpCount += vo.Number;
                    }
                    UpdateParty(cpvo);
                }
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool UpdateSignUp(CardPartySignUpVO vo)
        {
            ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }


        /// <summary>
        /// 发送短信（座位号）
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool SendSMS(CardPartySignUpViewVO vo)
        {
            try
            {
               
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 添加活动订单
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddPartyOrder(CardPartyOrderVO vo)
        {
            try
            {
                ICardPartyOrderDAO uDAO = CustomerManagementDAOFactory.CardPartyOrderDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int PartyOrderID = uDAO.Insert(vo);
                    return PartyOrderID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool UpdatePartyOrder(CardPartyOrderVO vo)
        {
            ICardPartyOrderDAO uDAO = CustomerManagementDAOFactory.CardPartyOrderDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <returns></returns>
        public CardPartyOrderVO FindPartyOrderById(int PartyOrderID)
        {
            ICardPartyOrderDAO uDAO = CustomerManagementDAOFactory.CardPartyOrderDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(PartyOrderID);
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <returns></returns>
        public CardPartyOrderViewVO FindPartyOrderViewById(int PartyOrderID)
        {
            ICardPartyOrderViewDAO uDAO = CustomerManagementDAOFactory.CardPartyOrderViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(PartyOrderID);
        }

        public List<CardPartyOrderViewVO> GetPartyOrderViewVO(string condtion,bool isAppType=true)
        {
            ICardPartyOrderViewDAO uDAO = CustomerManagementDAOFactory.CardPartyOrderViewDAO(this.CurrentCustomerProfile);
            if (isAppType)
            {
                condtion = condtion + " and AppType=" + AppType;
            }
            return uDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取我的订单列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardPartyOrderViewVO> FindPartyOrderViewByCustomerId(int CustomerId)
        {
            try
            {
                ICardPartyOrderViewDAO uDAO = CustomerManagementDAOFactory.CardPartyOrderViewDAO(this.CurrentCustomerProfile);
                return uDAO.FindByParams("CustomerId = " + CustomerId + " and AppType=" + AppType);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取主办方收入订单列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardPartyOrderViewVO> FindPartyOrderViewByHost(int CustomerId)
        {
            try 
            {
                ICardPartyOrderViewDAO uDAO = CustomerManagementDAOFactory.CardPartyOrderViewDAO(this.CurrentCustomerProfile);
                return uDAO.FindByParams("HostCustomerId = " + CustomerId + " and Status=1" + " and AppType=" + AppType);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 减少余额
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public bool ReduceCardBalance(int customerId, decimal balance)
        {
            ICardBalanceDAO uDAO = CustomerManagementDAOFactory.CardBalanceDAO(this.CurrentCustomerProfile);
            return uDAO.ReduceBalance(customerId, balance,  AppType);
        }

        /// <summary>
        /// 增加余额
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public bool PlusCardBalance(int customerId, decimal balance)
        {
            ICardBalanceDAO uDAO = CustomerManagementDAOFactory.CardBalanceDAO(this.CurrentCustomerProfile);
            return uDAO.PlusBalance(customerId, balance, AppType);
        }

        /// <summary>
        /// 获取账户金额总数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public decimal FindBalanceSumCost(string condition, params object[] parameters)
        {
            ICardBalanceDAO uDAO = CustomerManagementDAOFactory.CardBalanceDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalSum("Balance", condition + " and AppType=" + AppType);

        }

        /// <summary>
        /// 判断活动余额是否足够提现
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="totalCommission"></param>
        /// <returns></returns>
        public bool IsHasMoreCardBalance(int customerId, decimal totalCommission)
        {
            ICardBalanceDAO uDAO = CustomerManagementDAOFactory.CardBalanceDAO(this.CurrentCustomerProfile);
            List<CardBalanceVO> voList = uDAO.FindByParams("CustomerId = " + customerId+ " and AppType="+ AppType);
            if (voList.Count > 0)
                return voList[0].Balance >= totalCommission;
            else
                return false;
        }

        /// <summary>
        /// 判断收入是否合法
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="totalCommission"></param>
        /// <returns></returns>
        public bool isLegitimate(int customerId)
        {
            decimal PayOutMoneyed = 0;//已提现
            decimal Cashing = 0; //正在提现
            
            //获取提现记录
            List<CardPayOutVO> uVO = FindPayOutByCustomerId(customerId);

            for (int i = 0; i < uVO.Count; i++)
            {
                if (uVO[i].PayOutStatus == 1)
                {
                    PayOutMoneyed += uVO[i].PayOutCost;
                }
                if (uVO[i].PayOutStatus == 0)
                {
                    Cashing += uVO[i].PayOutCost;
                }
            }
            if (AppType == 3)
            {
                decimal TotalMoney = FindBalanceByCustomerId(customerId);//总收入
                bool isLegitimate = TotalMoney - PayOutMoneyed - Cashing >= 0;
                return isLegitimate;
            }
            else
            {
                decimal TotalMoney = FindBalanceByCustomerId(customerId);//总收入
                decimal CardBalance = FindCardBalanceByCustomerId(customerId);//账户余额

                bool isLegitimate = TotalMoney - CardBalance - PayOutMoneyed - Cashing >= 0;

                return isLegitimate;
            }
            
        }

        /// <summary>
        /// 获取活动可提现余额
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public decimal FindCardBalanceByCustomerId(int customerId)
        {
            if (AppType == 3)
            {
                //一级返利金额
                decimal wyVipOneFrozenBalance = FindOrderSumByCondtion("OneRebateCost", "OneRebateCustomerId = " + customerId + " and OneRebateStatus=0 and Status=1 and payAt is not NULL");
                //二级返利金额
                decimal wyVipTwoFrozenBalance = FindOrderSumByCondtion("TwoRebateCost", "TwoRebateCustomerId = " + customerId + " and OneRebateStatus=0 and Status=1 and payAt is not NULL");
                return wyVipOneFrozenBalance + wyVipTwoFrozenBalance;
            }
            else
            {
                ICardBalanceDAO uDAO = CustomerManagementDAOFactory.CardBalanceDAO(this.CurrentCustomerProfile);
                List<CardBalanceVO> voList = uDAO.FindByParams("CustomerId=" + customerId + " and AppType=" + AppType);
                if (voList.Count > 0)
                    return voList[0].Balance;
                else
                    return 0;
            }
        }

        /// <summary>
        /// 获取活动暂-余额
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public FrozenBalanceVO FindFrozenBalanceByCustomerId(int customerId)
        {
            ICardPartyOrderViewDAO uDAO = CustomerManagementDAOFactory.CardPartyOrderViewDAO(this.CurrentCustomerProfile);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO uVO = uBO.FindCustomenById(customerId);

            if (uVO.AppType == 3)
            {
                //微云智推不在这里结算推广奖励
                return new FrozenBalanceVO();
            }

            //活动开始7天后结算
            //List<CardPartyOrderViewVO> cVO = uDAO.FindByParams("HostCustomerId = " + customerId + " and IsPayOut=0 and Status=1 and payAt is not NULL and  DATE_SUB(CURDATE(), INTERVAL 7 DAY) > date(StartTime)");

            //报名截止后结算
            List<CardPartyOrderViewVO> cVO = uDAO.FindByParams("HostCustomerId = " + customerId + " and IsPayOut=0 and Status=1 and  (ISNULL(sub_mchid)=1 or LENGTH(trim(sub_mchid))=0) and payAt is not NULL and  now() > SignUpTime and Type=1" + " and AppType=" + AppType);
            if (cVO.Count > 0)
            {
                decimal Balance = 0;
                for (int i = 0; i < cVO.Count; i++)
                {
                    try
                    {
                        CardPartyOrderVO Order = FindPartyOrderById(cVO[i].PartyOrderID);
                        if(Order.IsPayOut==0)
                        {
                            CardPartyOrderVO oVO = new CardPartyOrderVO();
                            oVO.PartyOrderID = cVO[i].PartyOrderID;
                            oVO.IsPayOut = 1;
                            if (UpdatePartyOrder(oVO))
                            {
                                Balance += cVO[i].Cost - cVO[i].PromotionAwardCost;
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                PlusCardBalance(customerId, Balance);
            }

            List<CardPartyOrderViewVO> FrozenVO = uDAO.FindByParams("HostCustomerId = " + customerId + " and IsPayOut=0 and Status=1 and  (ISNULL(sub_mchid)=1 or LENGTH(trim(sub_mchid))=0) and payAt is not NULL and  now() <= SignUpTime and Type=1" + " and AppType=" + AppType);

            decimal FrozenBalance = 0; //活动冻结金额
            for (int i = 0; i < FrozenVO.Count; i++)
            {
                FrozenBalance += FrozenVO[i].Cost - FrozenVO[i].PromotionAwardCost;
            }

            //活动推广奖励报名截止后结算
            List<CardPartyOrderViewVO> cVO2 = uDAO.FindByParams("InviterCID = " + customerId + " and PromotionAwardStatus=0 and PromotionAwardCost>0 and Status=1 and   payAt is not NULL and  now() > SignUpTime and Type=1" + " and AppType=" + AppType);

            if (cVO2.Count > 0)
            {
                decimal Balance2 = 0;
                for (int i = 0; i < cVO2.Count; i++)
                {
                    try
                    {
                        CardPartyOrderVO Order = FindPartyOrderById(cVO2[i].PartyOrderID);
                        if (Order.PromotionAwardStatus == 0)
                        {
                            CardPartyOrderVO oVO = new CardPartyOrderVO();
                            oVO.PartyOrderID = cVO2[i].PartyOrderID;
                            oVO.PromotionAwardStatus = 1;
                            if (UpdatePartyOrder(oVO))
                            {
                                Balance2 += cVO2[i].PromotionAwardCost;
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                PlusCardBalance(customerId, Balance2);
            }

            List<CardPartyOrderViewVO> FrozenVO2 = uDAO.FindByParams("InviterCID = " + customerId + " and PromotionAwardStatus=0 and Status=1 and  payAt is not NULL and  now() <= SignUpTime and Type=1" + " and AppType=" + AppType);

            decimal FrozenBalance2 = 0; //活动推广奖励冻结金额
            for (int i = 0; i < FrozenVO2.Count; i++)
            {
                FrozenBalance2 += FrozenVO2[i].PromotionAwardCost;
            }

            //商品下单48小时后结算
            List<CardPartyOrderViewVO> cVO3 = uDAO.FindByParams("HostCustomerId = " + customerId + " and IsPayOut=0 and Status=1 and  (ISNULL(sub_mchid)=1 or LENGTH(trim(sub_mchid))=0) and payAt is not NULL and  payAt < DATE_SUB(NOW(),INTERVAL 2 DAY) and Type=2" + " and AppType=" + AppType);
            if (cVO3.Count > 0)
            {
                decimal Balance3 = 0;
                for (int i = 0; i < cVO3.Count; i++)
                {
                    try
                    {
                        CardPartyOrderVO Order = FindPartyOrderById(cVO3[i].PartyOrderID);
                        if (Order.IsPayOut == 0)
                        {
                            CardPartyOrderVO oVO = new CardPartyOrderVO();
                            oVO.PartyOrderID = cVO3[i].PartyOrderID;
                            oVO.IsPayOut = 1;
                            if (UpdatePartyOrder(oVO))
                            {
                                Balance3 += cVO3[i].Cost - cVO3[i].PromotionAwardCost;
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                PlusCardBalance(customerId, Balance3);
            }

            List<CardPartyOrderViewVO> FrozenVO3 = uDAO.FindByParams("HostCustomerId = " + customerId + " and IsPayOut=0 and Status=1 and  (ISNULL(sub_mchid)=1 or LENGTH(trim(sub_mchid))=0) and payAt is not NULL  and  payAt > DATE_SUB(NOW(),INTERVAL 2 DAY) and Type=2" + " and AppType=" + AppType);

            decimal FrozenBalance3 = 0; //商品冻结金额
            for (int i = 0; i < FrozenVO3.Count; i++)
            {
                FrozenBalance3 += FrozenVO3[i].Cost - FrozenVO3[i].PromotionAwardCost;
            }

            //商品推广奖励48小时后结算
            List<CardPartyOrderViewVO> cVO4 = uDAO.FindByParams("InviterCID = " + customerId + " and PromotionAwardStatus=0 and PromotionAwardCost>0 and Status=1 and  payAt is not NULL and  payAt < DATE_SUB(NOW(),INTERVAL 2 DAY)  and Type=2" + " and AppType=" + AppType);

            if (cVO4.Count > 0)
            {
                decimal Balance4 = 0;
                for (int i = 0; i < cVO4.Count; i++)
                {
                    try
                    {
                        CardPartyOrderVO Order = FindPartyOrderById(cVO4[i].PartyOrderID);
                        if (Order.PromotionAwardStatus == 0)
                        {
                            CardPartyOrderVO oVO = new CardPartyOrderVO();
                            oVO.PartyOrderID = cVO4[i].PartyOrderID;
                            oVO.PromotionAwardStatus = 1;
                            if (UpdatePartyOrder(oVO))
                            {
                                Balance4 += cVO4[i].PromotionAwardCost;
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                PlusCardBalance(customerId, Balance4);
            }

            List<CardPartyOrderViewVO> FrozenVO4 = uDAO.FindByParams("InviterCID = " + customerId + " and PromotionAwardStatus=0 and Status=1 and  payAt is not NULL and  NOW() <= DATE_SUB(payAt,INTERVAL 2 DAY) and Type=2" + " and AppType=" + AppType);

            decimal FrozenBalance4 = 0; //商品推广奖励冻结金额
            for (int i = 0; i < FrozenVO4.Count; i++)
            {
                FrozenBalance4 += FrozenVO4[i].PromotionAwardCost;
            }

            //一级返利冻结金额
            decimal VipOneFrozenBalance = FindOrderSumByCondtion("OneRebateCost", "OneRebateCustomerId = " + customerId + " and OneRebateStatus=0 and payAt is not NULL and Status=1");
            //二级返利冻结金额
            decimal VipTwoFrozenBalance = FindOrderSumByCondtion("TwoRebateCost", "TwoRebateCustomerId = " + customerId + " and TwoRebateStatus=0 and payAt is not NULL and Status=1");

            //一级邀约人数
            int OneRebateCount = FindCardOrderTotalCount("OneRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL" + " and AppType=" + AppType);

            //二级邀约人数
            int TwoRebateCount = FindCardOrderTotalCount("TwoRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL" + " and AppType=" + AppType);

            //需要多少一级才能解冻二级
            int isTow = 3;
            int UnlockTow = isTow - OneRebateCount;
            if (UnlockTow <= 0) UnlockTow = 0;

            //如果大于150就结算
            //if (VipOneFrozenBalance+ VipTwoFrozenBalance >= 150&& uVO.isVip)
            if (uVO.isVip)//改为是会员就立即结算
            {
                List<CardOrderVO> CardOrderVO = FindOrderByCondtion("OneRebateCustomerId = " + customerId + " and OneRebateStatus=0 and Status=1 and payAt is not NULL" + " and AppType=" + AppType);
                if (CardOrderVO.Count > 0)
                {
                    decimal Balance = 0;
                    for (int i = 0; i < CardOrderVO.Count; i++)
                    {
                        try
                        {
                            CardOrderVO Order = FindOrderById(CardOrderVO[i].CardOrderID);
                            if (Order.OneRebateStatus == 0)
                            {
                                CardOrderVO oVO = new CardOrderVO();
                                oVO.CardOrderID = CardOrderVO[i].CardOrderID;
                                oVO.OneRebateStatus = 1;
                                if (UpdateOrder(oVO))
                                {
                                    Balance += CardOrderVO[i].OneRebateCost;
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                    PlusCardBalance(customerId, Balance);
                    VipOneFrozenBalance = 0;
                }

                if (1==1 || OneRebateCount >= isTow)//邀请会员超过3个人才能解冻二级佣金（取消设定）
                {
                    List<CardOrderVO> CardOrderVO2 = FindOrderByCondtion("TwoRebateCustomerId = " + customerId + " and TwoRebateStatus=0 and Status=1 and payAt is not NULL" + " and AppType=" + AppType);
                    if (CardOrderVO2.Count > 0)
                    {
                        decimal Balance = 0;
                        for (int i = 0; i < CardOrderVO2.Count; i++)
                        {
                            try
                            {
                                CardOrderVO Order = FindOrderById(CardOrderVO2[i].CardOrderID);
                                if (Order.TwoRebateStatus == 0)
                                {
                                    CardOrderVO oVO = new CardOrderVO();
                                    oVO.CardOrderID = CardOrderVO2[i].CardOrderID;
                                    oVO.TwoRebateStatus = 1;
                                    if (UpdateOrder(oVO))
                                    {
                                        Balance += CardOrderVO2[i].TwoRebateCost;
                                    }
                                }
                            }
                            catch
                            {

                            }
                        }
                        PlusCardBalance(customerId, Balance);
                        VipTwoFrozenBalance = 0;
                    }
                }
            }


            FrozenBalanceVO fVO = new FrozenBalanceVO();
            fVO.PartyFrozenBalance = FrozenBalance;
            fVO.PartyPromotionFrozenBalance = FrozenBalance2;
            fVO.ProductFrozenBalance = FrozenBalance3;
            fVO.ProductPromotionFrozenBalance = FrozenBalance4;
            fVO.VipOneFrozenBalance = VipOneFrozenBalance;
            fVO.VipTwoFrozenBalance = VipTwoFrozenBalance;
            fVO.OneRebateCount = OneRebateCount;
            fVO.TwoRebateCount = TwoRebateCount;
            fVO.UnlockTow = UnlockTow;
            fVO.SumBalance = fVO.PartyFrozenBalance + fVO.PartyPromotionFrozenBalance + fVO.ProductFrozenBalance + fVO.ProductPromotionFrozenBalance + fVO.VipOneFrozenBalance + fVO.VipTwoFrozenBalance;
            return fVO;
        }

        /// <summary>
        /// 获取7天内收入
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public decimal Find7DayBalanceByCustomerId(int customerId)
        {
            ICardPartyOrderViewDAO uDAO = CustomerManagementDAOFactory.CardPartyOrderViewDAO(this.CurrentCustomerProfile);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO uVO = uBO.FindCustomenById(customerId);

            List<CardPartyOrderViewVO> FrozenVO = uDAO.FindByParams("HostCustomerId = " + customerId + " and Status=1 and payAt is not NULL and  payAt > DATE_SUB(NOW(),INTERVAL 7 DAY)" + " and AppType=" + AppType);

            decimal FrozenBalance = 0; //活动金额
            for (int i = 0; i < FrozenVO.Count; i++)
            {
                FrozenBalance += FrozenVO[i].Cost- FrozenVO[i].PromotionAwardCost;
            }

            List<CardPartyOrderViewVO> FrozenVO2 = uDAO.FindByParams("InviterCID = " + customerId + " and Status=1 and payAt is not NULL and  payAt > DATE_SUB(NOW(),INTERVAL 7 DAY)" + " and AppType=" + AppType);

            decimal FrozenBalance2 = 0; //活动推广奖励金额
            for (int i = 0; i < FrozenVO2.Count; i++)
            {
                FrozenBalance2 += FrozenVO2[i].PromotionAwardCost;
            }

            //一级返利冻结金额
            decimal VipOneFrozenBalance = FindOrderSumByCondtion("OneRebateCost", "OneRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL and  payAt > DATE_SUB(NOW(),INTERVAL 7 DAY)");
            //二级返利冻结金额
            decimal VipTwoFrozenBalance = FindOrderSumByCondtion("TwoRebateCost", "TwoRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL and  payAt > DATE_SUB(NOW(),INTERVAL 7 DAY)");

            return FrozenBalance + FrozenBalance2 + VipOneFrozenBalance + VipTwoFrozenBalance;
        }

        /// <summary>
        /// 获取总收入
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public decimal FindBalanceByCustomerId(int customerId)
        {
            ICardPartyOrderViewDAO uDAO = CustomerManagementDAOFactory.CardPartyOrderViewDAO(this.CurrentCustomerProfile);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO uVO = uBO.FindCustomenById(customerId);

            List<CardPartyOrderViewVO> FrozenVO = uDAO.FindByParams("HostCustomerId = " + customerId + " and IsPayOut=1 and Status=1 and  (ISNULL(sub_mchid)=1 or LENGTH(trim(sub_mchid))=0) and payAt is not NULL " + " and AppType=" + AppType);

            decimal FrozenBalance = 0; //活动金额
            for (int i = 0; i < FrozenVO.Count; i++)
            {
                FrozenBalance += FrozenVO[i].Cost - FrozenVO[i].PromotionAwardCost;
            }

            List<CardPartyOrderViewVO> FrozenVO2 = uDAO.FindByParams("InviterCID = " + customerId + " and PromotionAwardStatus=1 and Status=1 and  (ISNULL(sub_mchid)=1 or LENGTH(trim(sub_mchid))=0) and payAt is not NULL" + " and AppType=" + AppType);

            decimal FrozenBalance2 = 0; //活动推广奖励金额
            for (int i = 0; i < FrozenVO2.Count; i++)
            {
                FrozenBalance2 += FrozenVO2[i].PromotionAwardCost;
            }

            //一级返利冻结金额
            decimal VipOneFrozenBalance = FindOrderSumByCondtion("OneRebateCost", "OneRebateCustomerId = " + customerId + " and OneRebateStatus=1  and Status=1 and payAt is not NULL");
            //二级返利冻结金额
            decimal VipTwoFrozenBalance = FindOrderSumByCondtion("TwoRebateCost", "TwoRebateCustomerId = " + customerId + " and TwoRebateStatus=1  and Status=1 and payAt is not NULL");


            decimal TransferBalance = FindTransferSumByCondtion("Cost", "Status=1 and CustomerId="+ customerId); //活动打赏收入
            decimal TransferBalanceOut = FindTransferSumByCondtion("Cost", "Status=1 and ToCustomerId=" + customerId); //活动打赏支出

            decimal SoftarticleBalance = FindSoftArticleOrderSumByCondtion("Cost", "OriginalCustomerId= " + customerId + " and Status=1" + " and AppType=" + AppType); //软文转发收入金额

            if (AppType == 3)
            {
                //一级返利金额
                decimal wyVipOneFrozenBalance = FindOrderSumByCondtion("OneRebateCost", "OneRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL");
                //二级返利金额
                decimal wyVipTwoFrozenBalance = FindOrderSumByCondtion("TwoRebateCost", "TwoRebateCustomerId = " + customerId + " and Status=1 and payAt is not NULL");
                return wyVipOneFrozenBalance + wyVipTwoFrozenBalance;
            }
            else
            {
                return FrozenBalance + FrozenBalance2 + VipOneFrozenBalance + VipTwoFrozenBalance + TransferBalance - TransferBalanceOut + SoftarticleBalance;
            }
           
        }


        /// <summary>
        /// 获取总收入明细列表
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public List<CardBalanceList> FindBalanceListByCustomerId(int customerId)
        {
            try
            {
                ICardPartyOrderViewDAO uDAO = CustomerManagementDAOFactory.CardPartyOrderViewDAO(this.CurrentCustomerProfile);
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                CustomerVO uVO = uBO.FindCustomenById(customerId);

                List<CardBalanceList> CardBalanceList = new List<VO.CardBalanceList>();
                List<CardPartyOrderViewVO> FrozenVO = uDAO.FindByParams("HostCustomerId = " + customerId + " and IsPayOut=1 and Status=1 and payAt is not NULL " + " and AppType=" + AppType);

                //活动金额
                for (int i = 0; i < FrozenVO.Count; i++)
                {
                    CardBalanceList cVO = new VO.CardBalanceList();
                    cVO.Cost = FrozenVO[i].Cost - FrozenVO[i].PromotionAwardCost;
                    cVO.CostName = FrozenVO[i].CostName;
                    cVO.CostStyle = "活动报名";
                    cVO.OrderNO = FrozenVO[i].OrderNO;
                    cVO.payAt = FrozenVO[i].payAt;
                    cVO.Title = FrozenVO[i].Title;
                    cVO.ToID = FrozenVO[i].PartyID;
                    cVO.PromotionAwardCost = FrozenVO[i].PromotionAwardCost;
                    CardBalanceList.Add(cVO);
                }

                List<CardPartyOrderViewVO> FrozenVO2 = uDAO.FindByParams("InviterCID = " + customerId + " and PromotionAwardStatus=1 and Status=1 and payAt is not NULL" + " and AppType=" + AppType);

                //活动推广奖励金额
                for (int i = 0; i < FrozenVO2.Count; i++)
                {
                    CardBalanceList cVO = new VO.CardBalanceList();
                    cVO.Cost = FrozenVO2[i].PromotionAwardCost;
                    cVO.CostName = FrozenVO2[i].CostName;
                    cVO.CostStyle = "活动推广分佣";
                    cVO.OrderNO = FrozenVO2[i].OrderNO;
                    cVO.payAt = FrozenVO2[i].payAt;
                    cVO.Title = FrozenVO2[i].Title;
                    cVO.ToID = FrozenVO2[i].PartyID;
                    CardBalanceList.Add(cVO);
                }

                List<CardOrderVO> Order1 = FindOrderByCondtion("OneRebateCustomerId = " + customerId + " and OneRebateStatus=1  and Status=1 and payAt is not NULL");

                //一级返利金额
                for (int i = 0; i < Order1.Count; i++)
                {
                    CardBalanceList cVO = new VO.CardBalanceList();
                    cVO.Cost = Order1[i].OneRebateCost;
                    cVO.CostName = Order1[i].Cost + "元VIP会员";
                    cVO.CostStyle = "VIP一级返利";
                    cVO.OrderNO = Order1[i].OrderNO;
                    cVO.payAt = Order1[i].payAt;

                    if (Order1[i].Type == 1)
                    {
                        cVO.Title = "乐聊名片1个月VIP会员";
                    }
                    if (Order1[i].Type == 2)
                    {
                        cVO.Title = "乐聊名片1年VIP会员";
                    }
                    if (Order1[i].Type == 3)
                    {
                        cVO.Title = "乐聊名片永久VIP会员";
                    }
                    if (Order1[i].Type == 5)
                    {
                        cVO.Title = "乐聊名片季度VIP会员";
                    }
                    if (Order1[i].Type == 6)
                    {
                        cVO.Title = "乐聊名片VIP合伙人";
                    }
                    if (Order1[i].Type == 7)
                    {
                        cVO.Title = "乐聊名片VIP分公司";
                    }
                    cVO.ToID = Order1[i].CardOrderID;
                    CardBalanceList.Add(cVO);
                }

                List<CardOrderVO> Order2 = FindOrderByCondtion("TwoRebateCustomerId = " + customerId + " and TwoRebateStatus=1  and Status=1 and payAt is not NULL");

                //一级返利金额
                for (int i = 0; i < Order2.Count; i++)
                {
                    CardBalanceList cVO = new VO.CardBalanceList();
                    cVO.Cost = Order2[i].TwoRebateCost;
                    cVO.CostName = Order2[i].Cost + "元VIP会员";
                    cVO.CostStyle = "VIP二级返利";
                    cVO.OrderNO = Order2[i].OrderNO;
                    cVO.payAt = Order2[i].payAt;

                    if (Order2[i].Type == 1)
                    {
                        cVO.Title = "乐聊名片1个月VIP会员";
                    }
                    if (Order2[i].Type == 2)
                    {
                        cVO.Title = "乐聊名片1年VIP会员";
                    }
                    if (Order2[i].Type == 3)
                    {
                        cVO.Title = "乐聊名片永久VIP会员";
                    }
                    if (Order2[i].Type == 5)
                    {
                        cVO.Title = "乐聊名片季度VIP会员";
                    }
                    if (Order2[i].Type == 6)
                    {
                        cVO.Title = "乐聊名片VIP合伙人";
                    }
                    if (Order2[i].Type == 7)
                    {
                        cVO.Title = "乐聊名片VIP分公司";
                    }
                    cVO.ToID = Order2[i].CardOrderID;
                    CardBalanceList.Add(cVO);
                }

                List<CardSoftArticleOrderViewVO> sVO = FindSoftArticleOrderViewByConditionStr("OriginalCustomerId= " + customerId + " and Status=1" + " and AppType=" + AppType);
                //软文转发收入金额
                for (int i = 0; i < sVO.Count; i++)
                {
                    CardBalanceList cVO = new VO.CardBalanceList();
                    cVO.Cost = sVO[i].Cost;
                    cVO.CostName = "付费转载";
                    cVO.CostStyle = "软文被转载";
                    cVO.OrderNO = sVO[i].OrderNO;
                    cVO.payAt = sVO[i].payAt;

                    CardSoftArticleVO CardSoftArticleVO = FindSoftArticleById(sVO[i].SoftArticleID);
                    cVO.Title = CardSoftArticleVO.Title;
                    cVO.ToID = sVO[i].SoftArticleID;
                    CardBalanceList.Add(cVO);
                }
                //根据创建时间重新排序
                CardBalanceList.Sort((a, b) => a.payAt.CompareTo(b.payAt));
                CardBalanceList.Reverse();
                return CardBalanceList;
            }
            catch(Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
            
        }



        /// <summary>
        /// 活动余额提现
        /// </summary>
        /// <param name="CardPayOutVO"></param>
        /// <returns></returns>
        public int AddCardPayoutHistoryVO(CardPayOutVO CardPayOutVO)
        {
            try
            {
                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    ICardPayOutDAO bbo = CustomerManagementDAOFactory.CardPayOutDAO(this.CurrentCustomerProfile);
                    CardPayOutVO.AppType = AppType;
                    int PayoutHistoryId = bbo.Insert(CardPayOutVO);

                    if (PayoutHistoryId > 0 && CardPayOutVO.PayOutStatus == 0)//提交申请，必须扣除余额；
                    {
                        ReduceCardBalance(CardPayOutVO.CustomerId, CardPayOutVO.PayOutCost);
                    }

                    try
                    {
                        if (AppType == 1)
                        {
                            MessageTool.SendMobileMsg(CardPayOutVO.AccountName + "申请提现" + CardPayOutVO.PayOutCost + "元【众销乐 -资源共享众包销售平台】", "13592808422");
                            MessageTool.SendMobileMsg(CardPayOutVO.AccountName + "申请提现" + CardPayOutVO.PayOutCost + "元【众销乐 -资源共享众包销售平台】", "18620584620");
                        }
                    }
                    catch
                    {

                    }

                    return PayoutHistoryId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }


        /// <summary>
        /// 活动余额提现到微信
        /// </summary>
        /// <param name="CardPayOutVO"></param>
        /// <returns></returns>
        public int AddCardPayoutHistoryVO2(CardPayOutVO CardPayOutVO)
        {
            if (!IsHasMoreCardBalance(CardPayOutVO.CustomerId, CardPayOutVO.PayOutCost))
            {
                return -1;
            }
            ReduceCardBalance(CardPayOutVO.CustomerId, CardPayOutVO.PayOutCost);//提交申请，必须扣除余额；
            try
            {
                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    ICardPayOutDAO bbo = CustomerManagementDAOFactory.CardPayOutDAO(this.CurrentCustomerProfile);
                    CardPayOutVO.AppType = AppType;
                    int PayoutHistoryId = bbo.Insert(CardPayOutVO);
                    return PayoutHistoryId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新提现
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardPayoutHistoryVO(CardPayOutVO vo)
        {
            ICardPayOutDAO bbo = CustomerManagementDAOFactory.CardPayOutDAO(this.CurrentCustomerProfile);
            try
            {
                bbo.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取提现详情
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <returns></returns>
        public CardPayOutVO FindPayOutViewById(int PayoutHistoryId)
        {
            ICardPayOutDAO bbo = CustomerManagementDAOFactory.CardPayOutDAO(this.CurrentCustomerProfile);
            return bbo.FindById(PayoutHistoryId);
        }

        /// <summary>
        /// 获取提现数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardPayoutHistoryTotalCount(string condition, params object[] parameters)
        {
            ICardPayOutDAO bbo = CustomerManagementDAOFactory.CardPayOutDAO(this.CurrentCustomerProfile);
            return bbo.FindTotalCount(condition+ " and AppType="+ AppType, parameters);
        }

        /// <summary>
        /// 获取提现列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardPayOutVO> FindCardPayoutHistoryAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardPayOutDAO bbo = CustomerManagementDAOFactory.CardPayOutDAO(this.CurrentCustomerProfile);
            return bbo.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 提现处理
        /// </summary>
        /// <param name="CardPayOutVO"></param>
        /// <returns></returns>
        public bool HandleCardPayOut(CardPayOutVO CardPayOutVO)
        {
            if (AppType == 3)
            {
                CardPayOutVO dVO = FindPayOutViewById(CardPayOutVO.PayOutHistoryId);
                if (CardPayOutVO.PayOutStatus == 1)
                {
                    PayoutSUCCESS(dVO.CustomerId);
                }
                else if (CardPayOutVO.PayOutStatus == -2)
                {
                    PayoutFAIL(dVO.CustomerId);
                }
                else
                {
                    return false;
                }

                CardPayOutVO cVO = new CardPayOutVO();
                cVO.PayOutHistoryId = CardPayOutVO.PayOutHistoryId;
                cVO.PayOutStatus = CardPayOutVO.PayOutStatus;
                cVO.ThirdOrder = CardPayOutVO.ThirdOrder;
                cVO.HandleComment = CardPayOutVO.HandleComment;
                cVO.HandleDate = DateTime.Now;

                bool Payout = UpdateCardPayoutHistoryVO(cVO);

                if (cVO.PayOutStatus == 1)
                {
                    sendPayOutMessage(cVO.PayOutHistoryId);
                }
                else
                {
                    sendPayOutMessageBYoff(cVO.PayOutHistoryId);
                }

                return Payout;
            }
            else
            {
                if (CardPayOutVO.PayOutStatus == 1)
                {

                }
                else if (CardPayOutVO.PayOutStatus == -2)
                {
                    CardPayOutVO dVO = FindPayOutViewById(CardPayOutVO.PayOutHistoryId);
                    bool res = PlusCardBalance(dVO.CustomerId, dVO.PayOutCost);
                    if (res == false)
                        return false;
                }
                else
                {
                    return false;
                }

                CardPayOutVO cVO = new CardPayOutVO();
                cVO.PayOutHistoryId = CardPayOutVO.PayOutHistoryId;
                cVO.PayOutStatus = CardPayOutVO.PayOutStatus;
                cVO.ThirdOrder = CardPayOutVO.ThirdOrder;
                cVO.HandleComment = CardPayOutVO.HandleComment;
                cVO.HandleDate = DateTime.Now;

                bool Payout = UpdateCardPayoutHistoryVO(cVO);

                if (cVO.PayOutStatus == 1)
                {
                    sendPayOutMessage(cVO.PayOutHistoryId);
                }
                else
                {
                    sendPayOutMessageBYoff(cVO.PayOutHistoryId);
                }

                return Payout;
            }
            
        }

        /// <summary>
        /// 微信提现处理 
        /// </summary>
        /// <param name="CardPayOutVO"></param>
        /// <returns></returns>
        public bool HandleCardPayOutByWxCash(CardPayOutVO CardPayOutVO)
        {
            CardPayOutVO cVO = new CardPayOutVO();
            cVO.PayOutHistoryId = CardPayOutVO.PayOutHistoryId;
            cVO.PayOutStatus = CardPayOutVO.PayOutStatus;
            cVO.ThirdOrder = CardPayOutVO.ThirdOrder;
            cVO.HandleComment = CardPayOutVO.HandleComment;
            cVO.HandleDate = DateTime.Now;
            CardPayOutVO dVO = FindPayOutViewById(CardPayOutVO.PayOutHistoryId);
            if (CardPayOutVO.PayOutStatus == 1)
            {

            }
            else if (CardPayOutVO.PayOutStatus == -2)
            {
                bool res = PlusCardBalance(dVO.CustomerId, dVO.PayOutCost);
                if (res) {
                    bool Payout2 = UpdateCardPayoutHistoryVO(cVO);
                    sendPayOutMessageBYoff(cVO.PayOutHistoryId);
                }
                return true;
            }
            else
            {
                return false;
            }

        

            //企业付款到微信零钱


            string resultdata= PayforWXUserCash(dVO.Cost, dVO.AccountName, dVO.OpenId, dVO.CustomerId);

            if (resultdata == "NAME_MISMATCH")//收款名不正确
            {
                return false;
            }
            if (resultdata == "FAIL")//内部异常
            {
                return false;
            }
            if (resultdata == "NOTENOUGH")//商户平台帐户余额不足
            {
                return false;
            }

           bool Payout = UpdateCardPayoutHistoryVO(cVO);

            if (cVO.PayOutStatus == 1)
            {
                //发送成功通知
                sendPayOutMessage(cVO.PayOutHistoryId);
            }
            else
            {
                //发送失败
                sendPayOutMessageBYoff(cVO.PayOutHistoryId);
            }

            return Payout;
        }

        /// <summary>
        /// 添加转账记录
        /// </summary>
        /// <param name="CardTransferVO"></param>
        /// <returns></returns>
        public int AddTransferHistory(CardTransferVO CardTransferVO)
        {
            try
            {
                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    ICardTransferDAO bbo = CustomerManagementDAOFactory.CardTransferDAO(this.CurrentCustomerProfile);
                    CardTransferVO.AppType = AppType;
                    int TransferHistoryId = bbo.Insert(CardTransferVO);

                    if (TransferHistoryId > 0)//完成转账；
                    {
                        if (ReduceCardBalance(CardTransferVO.CustomerId, CardTransferVO.Cost) && PlusCardBalance(CardTransferVO.ToCustomerId, CardTransferVO.Cost))
                        {
                            CardTransferVO.TransferHistoryId = TransferHistoryId;
                            CardTransferVO.Status = 1;
                            UpdateTransferHistory(CardTransferVO);
                        }
                        else {
                            return -1;
                        }
                    }
                    return TransferHistoryId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CustomerBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新转账记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateTransferHistory(CardTransferVO vo)
        {
            ICardTransferDAO bbo = CustomerManagementDAOFactory.CardTransferDAO(this.CurrentCustomerProfile);
            try
            {
                bbo.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取转账记录数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindTransferHistoryTotalCount(string condition, params object[] parameters)
        {
            ICardTransferDAO bbo = CustomerManagementDAOFactory.CardTransferDAO(this.CurrentCustomerProfile);
            return bbo.FindTotalCount(condition + " and AppType=" + AppType, parameters);

        }

        public decimal FindTransferSumByCondtion(string Sum, string condtion)
        {
            ICardTransferDAO bbo = CustomerManagementDAOFactory.CardTransferDAO(this.CurrentCustomerProfile);
            return bbo.FindTotalSum(Sum, condtion+ " and AppType="+ AppType);
        }

        /// <summary>
        /// 获取转账记录列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardTransferVO> FindTransferHistoryByCondtion(string condtion)
        {
            ICardTransferDAO bbo = CustomerManagementDAOFactory.CardTransferDAO(this.CurrentCustomerProfile);
            return bbo.FindByParams(condtion);
        }

        /// <summary>
        /// 获取我的提现记录列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardPayOutVO> FindPayOutByCustomerId(int CustomerId)
        {
            try
            {
                ICardPayOutDAO bbo = CustomerManagementDAOFactory.CardPayOutDAO(this.CurrentCustomerProfile);
                return bbo.FindByParams("CustomerId = " + CustomerId + " and AppType=" + AppType);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        public decimal FindPayOutSumByCondtion(string Sum, string condtion)
        {
            ICardPayOutDAO bbo = CustomerManagementDAOFactory.CardPayOutDAO(this.CurrentCustomerProfile);
            return bbo.FindTotalSum(Sum, condtion + " and AppType=" + AppType);
        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardPartyOrderViewVO> FindCardPayOutAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardPartyOrderViewDAO uDAO = CustomerManagementDAOFactory.CardPartyOrderViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);

        }


        /// <summary>
        /// 获取订单列表数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardPayOutTotalCount(string condition, params object[] parameters)
        {
            ICardPartyOrderViewDAO uDAO = CustomerManagementDAOFactory.CardPartyOrderViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);

        }

        /// <summary>
        /// 获取订单列表数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindPartyOrderTotalCount(string condition, params object[] parameters)
        {
            ICardPartyOrderDAO uDAO = CustomerManagementDAOFactory.CardPartyOrderDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);

        }

        /// <summary>
        /// 获取订单金额总数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public decimal FindPartyOrderSumCost(string condition, params object[] parameters)
        {
            ICardPartyOrderViewDAO uDAO = CustomerManagementDAOFactory.CardPartyOrderViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindSumCost(condition + " and AppType=" + AppType, parameters);

        }

        /// <summary>
        /// 获取报名详情
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <returns></returns>
        public CardPartySignUpViewVO FindSignUpViewById(int PartySignUpID)
        {
            ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(PartySignUpID);
        }

        /// <summary>
        /// 判断报名详情
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns> 
        public List<CardPartySignUpViewVO> FindSignUpViewById(int CustomerId, int PartyID)
        {
            ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);
            List<CardPartySignUpViewVO> cVO = uDAO.FindByParams("CustomerId = " + CustomerId + " and PartyID=" + PartyID + " and Status>0  and AppType="+ AppType + " GROUP BY PartySignUpID ORDER BY StartTime DESC, CreatedAt DESC");
            return cVO;
        }

        /// <summary>
        /// 获取我的报名列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardPartySignUpViewVO> FindSignUpViewByCustomerId(int CustomerId)
        {
            string sql = "CustomerId = " + CustomerId+ " and Status > 0  and SignUpStatus<>2 and isAutoAdd=0 and AppType=" + AppType;
            if (Type != 4)
            {
                sql += " and Type<>3";
            }
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = uBO.FindCustomenById(CustomerId);

            /*
            if (!CustomerVO.isVip)
            {
                sql += " and DATE_SUB(CURDATE(), INTERVAL 30 DAY) <= EndTime";
            }
            */

            try
            {
                ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);
                return uDAO.FindByParams(sql+" GROUP BY PartyID ORDER BY StartTime DESC, CreatedAt DESC");
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取我报名的活动过期数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindSignUpViewBeoverdueCount(int CustomerId)
        {
            ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);

            string sql = "CustomerId = " + CustomerId + " and Status > 0 and SignUpStatus<>2 and DATE_SUB(CURDATE(), INTERVAL 30 DAY) <= EndTime and AppType=" + AppType;
            if (Type != 4)
            {
                sql += " and Type<>3";
            }
            return uDAO.FindTotalCount(sql);
        }

        /// <summary>
        /// 获取我报名的抽奖活动数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindSignUpViewLuckDrawCount(int CustomerId)
        {
            ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);

            string sql = "CustomerId = " + CustomerId + " and Status > 0 and SignUpStatus<>2 and Type=3 and AppType=" + AppType;
            return uDAO.FindTotalCount(sql);
        }

        /// <summary>
        /// 获取我中奖数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindSignUpWinningCount(int CustomerId)
        {
            ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
            string sql = "CustomerId = " + CustomerId + " and LuckDrawStatus=1 and AppType=" + AppType;
            return uDAO.FindTotalCount(sql);
        }

        /// <summary>
        /// 获取我的报名列表和我发布的活动列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardPartySignUpViewVO> FindPartyAndSignUpByCustomerId(int CustomerId)
        {
            string sql = "(HostCustomerId = " + CustomerId + " or CustomerId = " + CustomerId + ") and Status<>0 and SignUpStatus<>2 and AppType="+ AppType;
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = uBO.FindCustomenById(CustomerId);
            if (!CustomerVO.isVip)
            {
                sql += " and DATE_SUB(CURDATE(), INTERVAL 30 DAY) <= EndTime";
            }
             
            try
            {
                ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);
                return uDAO.FindByParams(sql+" GROUP BY PartyID ORDER BY StartTime DESC, CreatedAt DESC");
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取活动的所有报名列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartySignUpVO> FindSignUpByPartyID(int PartyID)
        {
            ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams("PartyID = " + PartyID+ "  and SignUpStatus<>2 and AppType="+ AppType);
        }

        /// <summary>
        /// 获取活动的所有报名列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartySignUpVO> FindSignUpByCondtion(string condtion)
        {
            ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams("SignUpStatus<>2 and AppType=" + AppType+ " and "+ condtion);
        }

        /// <summary>
        /// 获取会员在该活动的报名列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartySignUpVO> FindSignUpByPartyID(int PartyID,int CustomerId)
        {
            ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams("PartyID = " + PartyID+ " and CustomerId = "+ CustomerId+ "  and SignUpStatus<>2 and AppType="+ AppType);
        }

        /// <summary>
        /// 获取会员在该活动的邀请列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartySignUpVO> FindSignUpByInviterCID(int PartyID, int InviterCID)
        {
            ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams("PartyID = " + PartyID + " and InviterCID = " + InviterCID + "  and SignUpStatus<>2 and AppType=" + AppType);
        }


        /// <summary>
        /// 获取活动的所有报名列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardPartySignUpVO> FindCardPartSignInNumAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex("AppType="+ AppType + " and " + conditionStr, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取名片组的所有组员
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardGroupCardViewVO> FindCardGroupJoinAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardGroupCardViewDAO uDAO = CustomerManagementDAOFactory.CardGroupCardViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }
        /// <summary>
        /// 获取名片组人数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardGroupJoinTotalCount(string condition, params object[] parameters)
        {
            ICardGroupCardViewDAO uDAO = CustomerManagementDAOFactory.CardGroupCardViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);

        }

        /// 获取活动的所有报名视图列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardPartySignUpViewVO> FindCardPartSignUpViewByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取活动的所有报名列表数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardPartSignInNumTotalCount(string condition, params object[] parameters)
        {
            ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取活动的报名人数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardPartSignInSumCount(string Sum, string condition, params object[] parameters)
        {
            ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalSum(Sum,condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取活动的所有报名视图列表数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardPartSignUpViewListTotalCount(string condition, params object[] parameters)
        {
            ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取活动的所有报名列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartySignUpViewVO> FindSignUpViewByPartyID(int PartyID,bool isDisplayRefund=false)
        {
            ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);
            string sql = "PartyID = " + PartyID + " and PartySignUpID > 0 and SignUpStatus<>2 and AppType="+ AppType + " group by PartySignUpID ";
            if (isDisplayRefund)
            {
                sql = "PartyID = " + PartyID + " and PartySignUpID > 0  and AppType=" + AppType + " group by PartySignUpID ";
            }

            return uDAO.FindByParams(sql);
        }

        /// <summary>
        /// 获取活动的所有报名列表(真实报名)
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartySignUpViewVO> FindSignUpViewByTrue(int PartyID)
        {
            ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);
            string sql = "PartyID = " + PartyID + " and PartySignUpID > 0 and isAutoAdd = 0 and SignUpStatus<>2 and AppType=" + AppType + " group by PartySignUpID ";
            return uDAO.FindByParams(sql);
        }

        /// <summary>
        /// 获取活动的邀请人邀请得报名列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <param name="InviterCID"></param>
        /// <returns></returns>
        public List<CardPartySignUpViewVO> FindSignUpViewByInviterCID(int PartyID,int InviterCID)
        {
            ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams("PartyID = " + PartyID + " and InviterCID="+ InviterCID + " and PartySignUpID > 0 and SignUpStatus<>2 and AppType=" + AppType + " group by PartySignUpID ");
        }

        /// <summary>
        /// 获取活动的所有邀请者列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartySignUpViewVO> FindSignUpViewInviterByPartyID(int PartyID)
        {
            ICardPartySignUpViewDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpViewDAO(this.CurrentCustomerProfile);
            List<CardPartySignUpViewVO> list = uDAO.FindSignUpViewInviterByCon("PartyID = " + PartyID + " and PartySignUpID > 0 and InviterCID!=0");

            return list;
        }


     

        /// <summary>
        /// 获取活动的所有报名填写信息列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartySignUpFormVO> FindSignUpFormByPartyID(int PartyID,int Status=0)
        {
            ICardPartySignUpFormDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpFormDAO(this.CurrentCustomerProfile);
            if (Status == 0)
            {
                return uDAO.FindByParams("PartyID = " + PartyID+ " and AppType="+ AppType);
            }
            else
            {
                return uDAO.FindByParams("PartyID = " + PartyID+ " and Status>0" + " and AppType=" + AppType);
            }
        }

        /// <summary>
        /// 获取用户填写报名信息列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartySignUpFormVO> FindSignUpFormByFormStr(string FormStr)
        {
            //<SignUpForm><Name>姓名</Name><Value>黄德陆</Value></SignUpForm><SignUpForm><Name>手机</Name><Value>13905001313</Value></SignUpForm><SignUpForm><Name>工作单位</Name><Value>上海锦天城律师事务所</Value></SignUpForm><SignUpForm><Name>单位地址</Name><Value>福州</Value></SignUpForm><SignUpForm><Name>职位</Name><Value>律师</Value></SignUpForm><SignUpForm><Name>性别</Name><Value>男</Value></SignUpForm><SignUpForm><Name>微信</Name><Value>13905001313</Value></SignUpForm><SignUpForm><Name>人数</Name><Value>1</Value></SignUpForm><SignUpForm><Name>班级</Name><Value>高三一班</Value></SignUpForm>
            List<CardPartySignUpFormVO> sVO = new List<CardPartySignUpFormVO>();

            if (FormStr == "")
                return sVO;

            Regex reg = new Regex(@"<SignUpForm>\s*.*?\s*<\/SignUpForm>");
            Match m = reg.Match(FormStr);
            while (m.Success)
            {
                CardPartySignUpFormVO cVO = new CardPartySignUpFormVO();
                cVO.Name = Regex.Match(m.Value, @"<Name>(?<text>[^>]+)<\/Name>").Groups["text"].Value.Trim();
                cVO.value = Regex.Match(m.Value, @"<Value>(?<text>[^>]+)<\/Value>").Groups["text"].Value.Trim();
                sVO.Add(cVO);

                m = m.NextMatch();
            }
            return sVO;
        }

        /// <summary>
        /// 获取活动的所有费用信息列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartyCostVO> FindCostByPartyID(int PartyID)
        {
            ICardPartyCostDAO uDAO = CustomerManagementDAOFactory.CardPartyCostDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams("PartyID = " + PartyID + " and AppType=" + AppType);
        }

        /// <summary>
        /// 获取活动的所有费用信息列表（头奖）
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartyCostVO> FindCostByFirstPrize(int PartyID)
        {
            ICardPartyCostDAO uDAO = CustomerManagementDAOFactory.CardPartyCostDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams("PartyID = " + PartyID + " and isFirstPrize=1 and AppType=" + AppType);
        }


        /// <summary>
        /// 获取活动的所有费用信息数量
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public decimal FindCostSumByPartyID(string sum,int PartyID)
        {
            ICardPartyCostDAO uDAO = CustomerManagementDAOFactory.CardPartyCostDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalSum(sum,"PartyID = " + PartyID + " and AppType=" + AppType);
        }

        /// <summary>
        /// 获取费用详情
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public CardPartyCostVO FindCostById(int PartyCostID)
        {
            ICardPartyCostDAO uDAO = CustomerManagementDAOFactory.CardPartyCostDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(PartyCostID);
        }

        /// <summary>
        /// 获取活动的所有费用信息列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<CardPartyCostVO> FindCostById(int PartyID,string Names, decimal Cost)
        {
            ICardPartyCostDAO uDAO = CustomerManagementDAOFactory.CardPartyCostDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams("PartyID = " + PartyID + " and Names='"+ Names + "' and Cost="+ Cost + " and AppType=" + AppType);
        }

        /// <summary>
        /// 更新费用
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public bool UpdateCost(CardPartyCostVO vo)
        {
            ICardPartyCostDAO uDAO = CustomerManagementDAOFactory.CardPartyCostDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取我的活动列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardPartyVO> FindParty(int CustomerId,int notType=0)
        {
            try
            {
                ICardPartyDAO pDAO = CustomerManagementDAOFactory.CardPartyDAO(this.CurrentCustomerProfile);
                string sql = "CustomerId = " + CustomerId + " and Status>0" + " and AppType=" + AppType;
                if (notType > 0)
                {
                    sql += " and Type<>"+ notType;
                }

                return pDAO.FindByParams(sql);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取我的活动列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardPartyVO> FindParty()
        {
            try
            {
                ICardPartyDAO pDAO = CustomerManagementDAOFactory.CardPartyDAO(this.CurrentCustomerProfile);
                return pDAO.FindByParams("1=1" + " and AppType=" + AppType);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取我的活动列表(视图)
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardPartyViewVO> FindPartyByCustomerId(int CustomerId)
        {
            string sql = "(CustomerId = " + CustomerId + " or ContactsCustomerId like '%#"+ CustomerId + "#%') and Status>0" + " and AppType=" + AppType;
            if (Type != 4)
            {
                sql += " and Type<>3";
            }

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = uBO.FindCustomenById(CustomerId);
            if (!CustomerVO.isVip)
            {
                sql += " and (DATE_SUB(CURDATE(), INTERVAL 30 DAY) <= EndTime or Type=2 or Type=3)";
            }
            try
            {
                ICardPartyViewDAO pDAO = CustomerManagementDAOFactory.CardPartyViewDAO(this.CurrentCustomerProfile);
                List<CardPartyViewVO> uVO = pDAO.FindByParams(sql);
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "获取我的活动列表:" + sql;
                _log.Error(strErrorMsg);
                for (int i = 0; i < uVO.Count; i++)
                {
                    CustomerVO CVO = uBO.FindCustomenById(uVO[i].CustomerId);
                    List<CardDataVO> CardDataVO = FindCardByCustomerId(uVO[i].CustomerId);

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


                return uVO;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取我的活动列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardPartyVO> FindPartyByCId(int CustomerId)
        {
            string sql = "CustomerId = " + CustomerId + " and Status>0" + " and AppType=" + AppType;
            if (Type != 4)
            {
                sql += " and Type<>3";
            }

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = uBO.FindCustomenById(CustomerId);
            if (!CustomerVO.isVip)
            {
                sql += " and (DATE_SUB(CURDATE(), INTERVAL 30 DAY) <= EndTime or Type=2)";
            }
            try
            {
                ICardPartyDAO pDAO = CustomerManagementDAOFactory.CardPartyDAO(this.CurrentCustomerProfile);
                List<CardPartyVO> uVO = pDAO.FindByParams(sql);
                return uVO;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取我的活动过期数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindMyPartyBeoverdueCount(int CustomerId)
        {
            ICardPartyViewDAO pDAO = CustomerManagementDAOFactory.CardPartyViewDAO(this.CurrentCustomerProfile);

            string sql = "CustomerId = " + CustomerId + " and Status > 0 and DATE_SUB(CURDATE(), INTERVAL 30 DAY) > EndTime" + " and AppType=" + AppType;
            if (Type != 4)
            {
                sql += " and Type<>3";
            }
            return pDAO.FindTotalCount(sql);
        }

        /// <summary>
        /// 获取我发布的活动和我作为联系人的活动列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardPartyViewVO> FindPartyByCustomerIdOrContactsCustomerId(int CustomerId)
        {
            try
            {
                ICardPartyViewDAO pDAO = CustomerManagementDAOFactory.CardPartyViewDAO(this.CurrentCustomerProfile);
                CustomerBO uBO = new CustomerBO(new CustomerProfile());

                List<CardPartyViewVO> uVO = pDAO.FindByParams("CustomerId = " + CustomerId + " or " + "ContactsCustomerId LIKE '%#" + CustomerId + "#%'" + " and AppType=" + AppType);
                for (int i = 0; i < uVO.Count; i++)
                {
                    CustomerVO CVO = uBO.FindCustomenById(uVO[i].CustomerId);
                    List<CardDataVO> CardDataVO = FindCardByCustomerId(uVO[i].CustomerId);

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

                    uVO[i].SignupCount = FindCardPartSignInNumTotalCount("PartyID = " + uVO[i].PartyID+ "  and SignUpStatus<>2");
                }

                return uVO;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        public List<CardPartyViewViewVO> FindAllByPageIndexByParty(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardPartyViewViewDAO pDAO = CustomerManagementDAOFactory.CardPartyViewViewDAO(this.CurrentCustomerProfile);
            return pDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取活动数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindPartyTotalCount(string condition, params object[] parameters)
        {
            ICardPartyViewViewDAO pDAO = CustomerManagementDAOFactory.CardPartyViewViewDAO(this.CurrentCustomerProfile);
            return pDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }


        /// <summary>
        /// 获取活动列表
        /// </summary>
        public List<CardPartyVO> newFindAllByPageIndexByParty(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardPartyDAO pDAO = CustomerManagementDAOFactory.CardPartyDAO(this.CurrentCustomerProfile);
            return pDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取活动数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int newFindPartyTotalCount(string condition, params object[] parameters)
        {
            ICardPartyDAO pDAO = CustomerManagementDAOFactory.CardPartyDAO(this.CurrentCustomerProfile);
            return pDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 生成活动海报
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public string getPosterByPartyID(int PartyID, int style, int CustomerId = 0)
        {
            try
            {
                ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
                Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.APIURLByHttp + "/CustomerManagement/CardIMG.aspx?PartyID=" + PartyID + "&style=" + style+ "&CustomerId="+ CustomerId + "&AppType=" + Type, 1080, 1920, 1080, 1920);
                if (style==12) { m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.APIURLByHttp + "/CustomerManagement/CardIMG.aspx?PartyID=" + PartyID + "&style=" + style + "&CustomerId=" + CustomerId + "&AppType=" + Type, 850, 1080, 850, 1080); }
                if (style == 13) { m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.APIURLByHttp + "/CustomerManagement/CardIMG.aspx?PartyID=" + PartyID + "&style=" + style + "&CustomerId=" + CustomerId + "&AppType=" + Type, 650, 722, 650, 722); }
                if (style == 31|| style == 32 || style == 34 || style == 37 || style == 38) { m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.APIURLByHttp + "/CustomerManagement/CardIMG.aspx?PartyID=" + PartyID + "&style=" + style + "&CustomerId=" + CustomerId + "&AppType=" + Type, 1080, 1080, 1080, 1080); }
                if (style == 38) { m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.APIURLByHttp + "/CustomerManagement/CardIMG.aspx?PartyID=" + PartyID + "&style=" + style + "&CustomerId=" + CustomerId + "&AppType=" + Type, 1080, 864, 1080, 864); }

                string filePath = "";
                string folder = "/UploadFolder/CardPartyPosterFile/";
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                filePath = folder + newFileName;

                try
                {//删除旧图片
                    CardBO cBO = new CardBO(new CustomerProfile());
                    CardPartyVO caVO = cBO.FindPartyById(PartyID);

                    if (caVO.PosterImg != "")
                    {
                        string FilePath = caVO.PosterImg;
                        FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                        FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                        File.Delete(FilePath);
                    }

                }
                catch (Exception ex)
                {
                    LogBO _log = new LogBO(typeof(CardBO));
                    string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                    _log.Error(strErrorMsg);
                }

                //可以修改为网络路径
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string physicalPath = localPath + newFileName;

                m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                string PosterImg = ConfigInfo.Instance.APIURL + filePath;

                CardPartyVO cVO = new CardPartyVO();
                cVO.PartyID = PartyID;
                cVO.PosterImg = PosterImg;
                UpdateParty(cVO);
                return PosterImg;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }



        /// <summary>
        /// 生成小程序码 名字头像二维码
        /// </summary>
        /// <param name="ID"></param>
        ///  <param name="IDType">1:名片，2:名片组，3:活动，5:签到表，6:软文，7:活动报名授权码</param>
        /// <returns></returns>
        public string getQRIMGByIDAndType(int ID, int IDType,int CustomerId=0)
        {
            try
            {
                ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);

                string imgurl = ConfigInfo.Instance.APIURLByHttp + "/CustomerManagement/CardIMG2QR.aspx?ID=" + ID + "&IDType=" + IDType + "&AppType=" + Type;
                if (CustomerId != 0)
                {
                    imgurl = ConfigInfo.Instance.APIURLByHttp + "/CustomerManagement/CardIMG2QR.aspx?ID=" + ID + "&IDType=" + IDType + "&CustomerId=" + CustomerId + "&AppType=" + Type;
                }

                Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(imgurl, 752, 974, 752, 974);

                string filePath = "";
                string folder = "";
                if (IDType == 1) { folder = "/UploadFolder/CardQRImg/"; }
                else if (IDType==2) { folder = "/UploadFolder/CardGroupQRImg/"; }
                else if (IDType == 3) { folder = "/UploadFolder/CardPartQRImg/"; }
                else if (IDType == 5) { folder = "/UploadFolder/CardQuestionnaireQRImg/"; }
                else if (IDType == 6) { folder = "/UploadFolder/CardSoftArticleQRImg/"; }
                else if (IDType == 7) { folder = "/UploadFolder/CardConditionsQRImg/"; }

                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                filePath = folder + newFileName;


                //可以修改为网络路径
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string physicalPath = localPath + newFileName;

                m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                string ImgUrl = ConfigInfo.Instance.APIURL + filePath;

                if (IDType == 1) {
                    CardDataVO cVO = new CardDataVO();
                    cVO.CardID = ID;
                    cVO.QRImg = ImgUrl;
                    Update(cVO);
                }
                else if (IDType == 2) {
                    CardGroupVO cgVO = new CardGroupVO();
                    cgVO.GroupID = ID;
                    cgVO.QRImg = ImgUrl;
                    UpdateCardGroup(cgVO);

                }
                else if (IDType == 3)
                {
                    CardPartyVO cpVO = new CardPartyVO();
                    cpVO.PartyID = ID;
                    cpVO.QRImg = ImgUrl;
                    UpdateParty(cpVO);
                }else if(IDType == 5){
                    CardQuestionnaireVO cVO = new CardQuestionnaireVO();
                    cVO.QuestionnaireID = ID;
                    cVO.QRImg = ImgUrl;
                    UpdateCardQuestionnaire(cVO);
                }
                else if (IDType == 7)
                {
                    CardPartyVO cpVO = new CardPartyVO();
                    cpVO.PartyID = ID;
                    cpVO.ConditionsQR = ImgUrl;
                    UpdateParty(cpVO);
                }


                return ImgUrl;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }


        /// <summary>
        /// 生成临时活动小程序码 携带用户信息参数
        /// </summary>
        /// <param name="ID"></param>
        ///  <param name="IDType">1:名片，2:名片组，3:活动</param>
        ///  <param name="TempQRImgName">临时活动二维码的名字</param>
        ///  <param name="CustomerId">CustomerId</param>
        /// <returns></returns>
        public string getPartyQRIMGByIDAndTypeByMesage(int ID, Int64 TempQRImgName,Int64 CustomerId)
        {
            try
            {
                ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
                Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.APIURLByHttp + "/CustomerManagement/CardIMG2QR.aspx?ID=" + ID + "&IDType=4&CustomerId="+ CustomerId + "&ImgName=" + TempQRImgName + "&AppType=" + Type, 752, 974, 752, 974);

                string filePath = "";
                string folder = "";
              
                folder = "/UploadFolder/CardPartTempQRImg/"; 

                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                filePath = folder + newFileName;


                //可以修改为网络路径
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string physicalPath = localPath + newFileName;

                m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                string ImgUrl = ConfigInfo.Instance.APIURL + filePath;

                return ImgUrl;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 生成名片海报
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public string getPosterByCardID(int CardID,int Style)
        {
            try
            {
                ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);

                int width = 1080;
                int height = 1766;

                if (Style > 0)
                    height = 1920;

                if (Style == 5)
                {
                    width = 750;
                    height = 1334;
                }

                if (Style == 39|| Style == 40)
                {
                    width = 1080;
                    height = 1080;
                }

                Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.APIURLByHttp + "/CustomerManagement/CardIMG2.aspx?CardID=" + CardID+ "&Style="+ Style + "&AppType=" + Type, width, height, width, height);


                string filePath = "";
                string folder = "/UploadFolder/CardPosterFile/";
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                filePath = folder + newFileName;

                try {//删除旧图片
                    CardBO cBO = new CardBO(new CustomerProfile());
                    CardDataVO caVO = cBO.FindCardById(CardID);

                    if (caVO.PosterImg != "")
                    {
                        string FilePath = caVO.PosterImg;
                        FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                        FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                        File.Delete(FilePath);
                    }

                }
                catch
                {

                }

                //可以修改为网络路径
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string physicalPath = localPath + newFileName;

                m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                string PosterImg = ConfigInfo.Instance.APIURL + filePath;

                CardDataVO cVO = new CardDataVO();
                cVO.CardID = CardID;
                cVO.PosterImg = PosterImg;
                Update(cVO);
                return PosterImg;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 生成名片海报（可更换背景）
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public string getPosterByCardID(int CardID, string Posterback, string IP)
        {
            try
            {
                ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);

                int width = 1080;
                int height = 1080;

                Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.APIURLByHttp + "/CustomerManagement/CardIMG3.aspx?CardID=" + CardID + "&Posterback=" + Posterback + "&AppType=" + Type+ "&IP="+ IP, width, height, width, height);


                string filePath = "";
                string folder = "/UploadFolder/CardPosterFile/";
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                filePath = folder + newFileName;

                try
                {//删除旧图片
                    CardBO cBO = new CardBO(new CustomerProfile());
                    CardDataVO caVO = cBO.FindCardById(CardID);

                    if (caVO.PosterImg != "")
                    {
                        string FilePath = caVO.PosterImg;
                        FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                        FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                        File.Delete(FilePath);
                    }

                }
                catch
                {

                }

                //可以修改为网络路径
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string physicalPath = localPath + newFileName;

                m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                string PosterImg = ConfigInfo.Instance.APIURL + filePath;

                CardDataVO cVO = new CardDataVO();
                cVO.CardID = CardID;
                cVO.PosterImg = PosterImg;
                Update(cVO);
                return PosterImg;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 名片识别（百度云）
        /// </summary>
        /// <param name="hfc"></param>
        /// <returns></returns>
        public Baidu_business_card_Result BusinessCardRecognitionBYbaidu(HttpFileCollection hfc)
        {
            var APP_ID = "14782561";
            var API_KEY = "Srd0Fme3MLD8uOyydIBZjKCC";
            var SECRET_KEY = "ab6myUYMkWZGFfqGrkmp1vHFvanZaOk0";
            var client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间
            try
            {
                //保存文件
                string folder = "/UploadFolder/CardRecognition/";
                FileInfo fi = new FileInfo(hfc[0].FileName);

                string ext = fi.Extension.ToLower();
                if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
                {
                    return null;
                }

                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fi.Extension;
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string PhysicalPath = localPath + newFileName;
                hfc[0].SaveAs(PhysicalPath);

                var image = File.ReadAllBytes(PhysicalPath);
                var jsonStr = client.BusinessCard(image);



                var result = new Baidu_business_card_Result();
                if (jsonStr.ToString().Contains("error_code"))
                {
                    var errorResult = JsonConvert.DeserializeObject<BaiduErrorMsg>(jsonStr.ToString());
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<business_card>(jsonStr.ToString());
                    CardDataVO cVO = new CardDataVO();
                    cVO.Name = model.words_result.NAME[0];
                    cVO.Phone = model.words_result.MOBILE[0];
                    cVO.Address = model.words_result.ADDR[0];
                    cVO.WebSite = model.words_result.URL[0];
                    cVO.Tel = model.words_result.TEL[0];
                    cVO.Email = model.words_result.EMAIL[0];
                    cVO.CorporateName = model.words_result.COMPANY[0];

                    //跟据地址识别出经纬度
                    CardBO cBO = new CardBO(new CustomerProfile());
                    WeiXinGeocoder Geocoder = cBO.getLatitudeAndLongitude(cVO.Address);
                    if (Geocoder != null)
                    {
                        cVO.latitude = Geocoder.result.location.lat;
                        cVO.longitude = Geocoder.result.location.lng;
                    }

                    result.SuccessResult = cVO;
                    result.Result = true;
                }

                FileInfo file = new FileInfo(PhysicalPath);//指定文件路径
                if (file.Exists)//判断文件是否存在
                {
                    file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                    file.Delete();//删除文件
                }

                if (result.Result)
                    return result;
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }
        /// <summary>
        /// 名片识别（腾讯云）
        /// </summary>
        /// <param name="hfc"></param>
        /// <returns></returns>
        public Baidu_business_card_Result BusinessCardRecognitionBYTencent(HttpFileCollection hfc)
        {
            //保存文件
            string folder = "/UploadFolder/CardRecognition/";
            FileInfo fi = new FileInfo(hfc[0].FileName);

            string ext = fi.Extension.ToLower();
            if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
            {
                return null;
            }
            try
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fi.Extension;
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string PhysicalPath = localPath + newFileName;
                hfc[0].SaveAs(PhysicalPath);

                var urlList = new List<string>();
                urlList.Add(ConfigInfo.Instance.APIURL + folder + newFileName);
                //请求参数
                var req = new { appid = "1258097819", url_list = urlList };
                var psotJson = JsonConvert.SerializeObject(req);

                TencentCloundPicHelper tencentHelper = new TencentCloundPicHelper();
                var info = tencentHelper.SendPost(psotJson);

                TencentBusinesscardResponse response = JsonConvert.DeserializeObject<TencentBusinesscardResponse>(info);

                FileInfo file = new FileInfo(PhysicalPath);//指定文件路径
                if (file.Exists)//判断文件是否存在
                {
                    file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                    file.Delete();//删除文件
                }
                if (response != null && response.result_list.Any())
                {

                    if (response.result_list.FirstOrDefault().code == 0)
                    {
                        var result = new Baidu_business_card_Result();
                        List<Data> resultdata = response.result_list.FirstOrDefault().data;
                        CardDataVO cVO = new CardDataVO();

                        List<Data> listII = new List<Data>();

                        for (int i = 0; i < resultdata.Count; i++)
                        {
                            Data idata = resultdata[i];
                            for (int j = 0; j < resultdata.Count; j++)
                            {
                                if (idata.item == resultdata[j].item && i != j)
                                {
                                    if (idata.confidence < resultdata[j].confidence)
                                    {
                                        idata = resultdata[j];
                                    }
                                }
                            }
                            listII.Add(idata);
                        }

                        for (int i = 0; i < listII.Count; i++)
                        {
                            Data idata = listII[i];
                            if (idata.item == "姓名")
                            {
                                cVO.Name = idata.value;
                            }
                            if (idata.item == "职位")
                            {
                                cVO.Position = idata.value;
                            }
                            if (idata.item == "公司")
                            {
                                cVO.CorporateName = idata.value;
                            }
                            if (idata.item == "地址")
                            {
                                cVO.Address = idata.value;
                            }
                            if (idata.item == "邮箱")
                            {
                                cVO.Email = idata.value;
                            }
                            if (idata.item == "网址")
                            {
                                cVO.WebSite = idata.value;
                            }
                            if (idata.item == "手机")
                            {
                                cVO.Phone = idata.value;
                            }
                            if (idata.item == "电话")
                            {
                                cVO.Tel = idata.value;
                            }
                            if (idata.item == "微信")
                            {
                                cVO.WeChat = idata.value;
                            }
                        }

                        //跟据地址识别出经纬度
                        CardBO cBO = new CardBO(new CustomerProfile());
                        WeiXinGeocoder Geocoder = cBO.getLatitudeAndLongitude(cVO.Address);
                        if (Geocoder != null)
                        {
                            cVO.latitude = Geocoder.result.location.lat;
                            cVO.longitude = Geocoder.result.location.lng;
                        }

                        result.SuccessResult = cVO;
                        result.Result = true;
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
            
        }

        /// <summary>
        /// 身份证识别（腾讯云）
        /// </summary>
        /// <param name="hfc"></param>
        /// <returns></returns>
        public async Task<bool> IdCardRecognitionBYTencent(string  img,int type)
        {

            TencentCloundPicHelper tencentHelper = new TencentCloundPicHelper();
            try
            {
                string CardSide = "FRONT";
                if (type==1)
                    CardSide = "FRONT";
                else
                    CardSide = "BACK";
                string request = await tencentHelper.IDCardOCR(img, CardSide);

                if (request.Contains("Error"))
                {
                    return false;
                }
                if (request.Contains("-9100"))
                {
                    return false;
                }
                if (request.Contains("-9102"))
                {
                    return false;
                }
                if (request.Contains("-9103"))
                {
                    return false;
                }
                if (request.Contains("-9104"))
                {
                    return false;
                }
                if (request.Contains("-9105"))
                {
                    return false;
                }
                if (request.Contains("-9106"))
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 身份证识别（腾讯云）
        /// </summary>
        /// <param name="hfc"></param>
        /// <returns></returns>
        public async Task<ResultObject> IdCardRecognitionBYTencent2(string img, int type)
        {
            TencentCloundPicHelper tencentHelper = new TencentCloundPicHelper();
            try
            {
                string CardSide = "FRONT";
                if (type == 1)
                    CardSide = "FRONT";
                else
                    CardSide = "BACK";
                string request = await tencentHelper.IDCardOCR(img, CardSide);

                if (request.Contains("Error"))
                {
                    return new ResultObject() { Flag = 0, Message = "无效身份证照片", Result = request };
                }
                if (request.Contains("-9100"))
                {
                    return new ResultObject() { Flag = 0, Message = "身份证有效日期不合法", Result = request };
                }
                if (request.Contains("-9101"))
                {
                    return new ResultObject() { Flag = 0, Message = "身份证边框不完整", Result = request };
                }
                if (request.Contains("-9102"))
                {
                    return new ResultObject() { Flag = 0, Message = "不能使用复印件", Result = request };
                }
                if (request.Contains("-9103"))
                {
                    return new ResultObject() { Flag = 0, Message = "身份证翻拍", Result = request };
                }
                if (request.Contains("-9104"))
                {
                    return new ResultObject() { Flag = 0, Message = "临时身份证", Result = request };
                }
                if (request.Contains("-9105"))
                {
                    return new ResultObject() { Flag = 0, Message = "身份证框内遮挡", Result = request };
                }
                if (request.Contains("-9106"))
                {
                    return new ResultObject() { Flag = 0, Message = "身份证 PS ", Result = request };
                }
                return new ResultObject() { Flag = 1, Message = "识别成功", Result = request };
            }
            catch(Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "身份证识别错误", Result = ex };
            }
        }

        /// <summary>
        /// 保存FormId记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddFormId(CardFormListVO vo)
        {
            try
            {
                ICardFormListDAO uDAO = CustomerManagementDAOFactory.CardFormListDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int FormListID = uDAO.Insert(vo);
                    return FormListID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 获取FormId详情
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public CardFormListVO FindFormListById(int FormListID)
        {
            ICardFormListDAO uDAO = CustomerManagementDAOFactory.CardFormListDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(FormListID);
        }

        /// <summary>
        /// 根据formId获取FormId详情
        /// </summary>
        /// <param name="FormId"></param>
        /// <returns></returns>
        public int GetFormListByFormIdCount(string FormId)
        {
            ICardFormListDAO uDAO = CustomerManagementDAOFactory.CardFormListDAO(this.CurrentCustomerProfile);
            //uDAO.FindByParams("FormId = '"+ FormId + "'" );
            return uDAO.FindTotalCount("FormId = '" + FormId + "'");
        }

        /// <summary>
        /// 删除FormId记录
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public int DeleteFormIdById(int FormListID)
        {
            ICardFormListDAO uDAO = CustomerManagementDAOFactory.CardFormListDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("FormListID = " + FormListID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        /// 删除FormId记录（临时表）
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public int DeleteFormIdTempById(int FormListID)
        {
            ICardFormListTempDAO uDAO = CustomerManagementDAOFactory.CardFormListTempDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("FormListID = " + FormListID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 删除过期FormId记录
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public int DeleteFormIdBydate()
        {
            ICardFormListDAO uDAO = CustomerManagementDAOFactory.CardFormListDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("((curdate() - interval 7 day) >= cast(`CreatedAt` as date))");
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取所有的FormId
        /// </summary>
        /// <returns></returns>
        public List<CardFormListViewVO> FindFormListView()
        {
            try
            {
                ICardFormListViewDAO pDAO = CustomerManagementDAOFactory.CardFormListViewDAO(this.CurrentCustomerProfile);
                return pDAO.FindByParams("1=1");
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的FormId
        /// </summary>
        /// <returns></returns>
        public List<CardFormListViewVO> FindFormListView(string condtion)
        {
            try
            {
                ICardFormListViewDAO pDAO = CustomerManagementDAOFactory.CardFormListViewDAO(this.CurrentCustomerProfile);
                return pDAO.FindByParams(condtion);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的FormId（临时表）
        /// </summary>
        /// <returns></returns>
        public List<CardFormListVO> FindFormListTemp(string condtion)
        {
            try
            {
                ICardFormListTempDAO pDAO = CustomerManagementDAOFactory.CardFormListTempDAO(this.CurrentCustomerProfile);
                return pDAO.FindByParams(condtion);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 添加通知
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddNotice(CardNoticeVO vo)
        {
            try
            {
                ICardNoticeDAO rDAO = CustomerManagementDAOFactory.CardNoticeDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int NoticeID = rDAO.Insert(vo);
                    return NoticeID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新通知
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateNotice(CardNoticeVO vo)
        {
            ICardNoticeDAO rDAO = CustomerManagementDAOFactory.CardNoticeDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 获取通知列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardNoticeVO> FindCardNoticeAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardNoticeDAO rDAO = CustomerManagementDAOFactory.CardNoticeDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr+ " and AppType="+ AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取当天通知
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public CardNoticeVO FindCardMessageByToday()
        {
            ICardNoticeDAO rDAO = CustomerManagementDAOFactory.CardNoticeDAO(this.CurrentCustomerProfile);
            List<CardNoticeVO> cVO = rDAO.FindAllByPageIndex("to_days(SendDate) = to_days(now()) and Status = 0 and AppType="+ AppType);
            if (cVO.Count > 0)
                return cVO[0];
            else
                return null;
        }

        /// <summary>
        /// 获取通知数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardMessageTotalCount(string condition, params object[] parameters)
        {
            ICardNoticeDAO rDAO = CustomerManagementDAOFactory.CardNoticeDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition+ " and AppType=" + AppType, parameters);
        }
        /// <summary>
        /// 获取通知详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public CardNoticeVO FindCardMessageById(int NoticeID)
        {
            ICardNoticeDAO rDAO = CustomerManagementDAOFactory.CardNoticeDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(NoticeID);
        }
        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public int DeleteNoticeById(int NoticeID)
        {
            ICardNoticeDAO rDAO = CustomerManagementDAOFactory.CardNoticeDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("NoticeID = " + NoticeID + " and AppType=" + AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 添加新闻
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddNews(CardNewsVO vo)
        {
            try
            {
                ICardNewsDAO uDAO = CustomerManagementDAOFactory.CardNewsDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int NewsID = uDAO.Insert(vo);
                    return NewsID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新新闻
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateNews(CardNewsVO vo)
        {
            ICardNewsDAO uDAO = CustomerManagementDAOFactory.CardNewsDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 删除新闻
        /// </summary>
        /// <param name="NewsID"></param>
        /// <returns></returns>
        public int DeleteNewsById(int NewsID)
        {
            ICardNewsDAO uDAO = CustomerManagementDAOFactory.CardNewsDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("NewsID = " + NewsID + " and AppType=" + AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取新闻列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardNewsVO> FindCardNewsAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardNewsDAO uDAO = CustomerManagementDAOFactory.CardNewsDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 将新闻全部设置为非弹窗
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardNewsToOff()
        {
            ICardNewsDAO uDAO = CustomerManagementDAOFactory.CardNewsDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.Update("isAlert=0", "isAlert>0" + " and AppType=" + AppType);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取新闻数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardNewsTotalCount(string condition, params object[] parameters)
        {
            ICardNewsDAO uDAO = CustomerManagementDAOFactory.CardNewsDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取新闻详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public CardNewsVO FindCardNewsById(int NewsID)
        {
            ICardNewsDAO uDAO = CustomerManagementDAOFactory.CardNewsDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(NewsID);
        }

        /// <summary>
        /// 获取默认新闻列表
        /// </summary>
        /// <returns></returns>
        public List<CardNewsVO> FindCardNewsList()
        {
            try
            {
                ICardNewsDAO uDAO = CustomerManagementDAOFactory.CardNewsDAO(this.CurrentCustomerProfile);
                return uDAO.FindByParams("isDefault=1 and AppType="+ AppType + " ORDER BY OrderNO DESC");
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取弹窗新闻列表
        /// </summary>
        /// <returns></returns>
        public List<CardNewsVO> FindCardNewsListByAlert()
        {
            try
            {
                ICardNewsDAO uDAO = CustomerManagementDAOFactory.CardNewsDAO(this.CurrentCustomerProfile);
                return uDAO.FindByParams("isAlert=1 and AppType="+ AppType + " ORDER BY OrderNO DESC");
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }




        /// <summary>
        /// 添加红包广告
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddRedPacket(CardRedPacketVO vo)
        {
            try
            {
                ICardRedPacketDAO uDAO = CustomerManagementDAOFactory.CardRedPacketDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int RPID = uDAO.Insert(vo);
                    return RPID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新红包
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardRedPacket(CardRedPacketVO vo)
        {
            ICardRedPacketDAO uDAO = CustomerManagementDAOFactory.CardRedPacketDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 删除红包
        /// </summary>
        /// <param name="NewsID"></param>
        /// <returns></returns>
        public int DeleteCardRedPacketById(int RedPacketID)
        {
            ICardRedPacketDAO uDAO = CustomerManagementDAOFactory.CardRedPacketDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("RedPacketID = " + RedPacketID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取当前启用的官方广告红包
        /// </summary>
        public List<CardRedPacketVO> getOneCardRedByStatysAndRpType() {

            ICardRedPacketDAO uDAO = CustomerManagementDAOFactory.CardRedPacketDAO(this.CurrentCustomerProfile);

            List<CardRedPacketVO> rpVO=uDAO.FindByParams("RPType=0 and Status=1");

            return rpVO;
        }



        /// <summary>
        /// 获取红包列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardRedPacketVO> FindCardRedPacketAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardRedPacketDAO uDAO = CustomerManagementDAOFactory.CardRedPacketDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }


        /// <summary>
        /// 统计已发红包金额
        /// </summary>
        /// <returns></returns>
        public List<CardRedPacketViewVO> CountRedPacketTotalCost(String condtion)
        {
            ICardRedPacketViewDAO uDAO = CustomerManagementDAOFactory.CardRedPacketViewDAO(this.CurrentCustomerProfile);
            List<CardRedPacketViewVO> list = uDAO.FindByParams(condtion);

            return list;

        }

        /// <summary>
        /// 获取红包视图列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardRedPacketViewVO> FindCardRedPacketViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardRedPacketViewDAO uDAO = CustomerManagementDAOFactory.CardRedPacketViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取红包视图列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardRedPacketViewVO> FindCardRedPacketViewAllByPageIndex(string conditionStr, params object[] parameters)
        {
            ICardRedPacketViewDAO uDAO = CustomerManagementDAOFactory.CardRedPacketViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindCardList(conditionStr, parameters);
        }

        /// <summary>
        /// 获取红包视图数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardRedPacketViewTotalCount(string condition, params object[] parameters)
        {
            ICardRedPacketViewDAO uDAO = CustomerManagementDAOFactory.CardRedPacketViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 获取红包分配列表视图
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardRedPacketListViewVO> FindCardRedPacketListViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardRedPacketListViewDAO uDAO = CustomerManagementDAOFactory.CardRedPacketListViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取所有红包分配列表视图2
        public List<CardRedPacketListViewVO> FindCardRedPacketListViewAllByPageIndex2(String condtion)
        {
            ICardRedPacketListViewDAO uDAO = CustomerManagementDAOFactory.CardRedPacketListViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取红包分配列表视图数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardRedPacketListViewTotalCount(string condition, params object[] parameters)
        {
            ICardRedPacketListViewDAO uDAO = CustomerManagementDAOFactory.CardRedPacketListViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 获取红包详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public CardRedPacketVO FindCardRedPacketById(int RPID)
        {
            ICardRedPacketDAO uDAO = CustomerManagementDAOFactory.CardRedPacketDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(RPID);
        }

        /// <summary>
        /// 获取红包视图详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public CardRedPacketViewVO FindCardRedPacketViewById(int RPID)
        {
            ICardRedPacketViewDAO uDAO = CustomerManagementDAOFactory.CardRedPacketViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(RPID);
        }

        /// <summary>
        /// 获取红包详情
        /// </summary>
        /// <param name="condtion">条件</param>
        /// <returns></returns>
        public List<CardRedPacketVO> GetCardRedPacketVO(string condtion)
        {
            ICardRedPacketDAO uDAO = CustomerManagementDAOFactory.CardRedPacketDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取红包数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardRedPacketTotalCount(string condition, params object[] parameters)
        {
            ICardRedPacketDAO uDAO = CustomerManagementDAOFactory.CardRedPacketDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }


        /// <summary>
        /// 获取详情数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardRedPacketDetailTotalCount(string condition, params object[] parameters)
        {
            ICardRedPacketDetailDAO uDAO = CustomerManagementDAOFactory.CardRedPacketDetailDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 获取红包领取记录数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardRedPacketDetailListTotalCount(string condition, params object[] parameters)
        {
            ICardRedPacketListDAO uDAO = CustomerManagementDAOFactory.CardRedPacketListDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 将官方红包设置为禁用状态
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateRedPacketStatus()
        {
            ICardRedPacketDAO uDAO = CustomerManagementDAOFactory.CardRedPacketDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.Update("Status=0", "Status=1 and RPType=0");
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }




        /// <summary>
        /// 添加红包记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddRedPacketDetail(CardRedPacketDetailVO vo)
        {
            try
            {
                ICardRedPacketDetailDAO uDAO = CustomerManagementDAOFactory.CardRedPacketDetailDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int RPDetailID = uDAO.Insert(vo);
                    return RPDetailID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新红包记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardRedPacketDetail(CardRedPacketDetailVO vo)
        {
            ICardRedPacketDetailDAO uDAO = CustomerManagementDAOFactory.CardRedPacketDetailDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 删除红包记录
        /// </summary>
        /// <param name="NewsID"></param>
        /// <returns></returns>
        public int DeleteCardRedPacketDetailById(int RedPacketDetailID)
        {
            ICardRedPacketDetailDAO uDAO = CustomerManagementDAOFactory.CardRedPacketDetailDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("RedPacketDetailID = " + RedPacketDetailID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }




        /// <summary>
        /// 获取红包记录列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardRedPacketDetailVO> FindCardRedPacketDetailAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardRedPacketDetailDAO uDAO = CustomerManagementDAOFactory.CardRedPacketDetailDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }


        /// <summary>
        /// 获取红包记录详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public CardRedPacketDetailVO FindCardRedPacketDetailById(int RPDetailID)
        {
            ICardRedPacketDetailDAO uDAO = CustomerManagementDAOFactory.CardRedPacketDetailDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(RPDetailID);
        }






        /// <summary>
        /// 添加红包分配列表广告
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddRedPacketList(CardRedPacketListVO vo)
        {
            try
            {
                ICardRedPacketListDAO uDAO = CustomerManagementDAOFactory.CardRedPacketListDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int RPID = uDAO.Insert(vo);
                    return RPID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新红包分配列表
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardRedPacketList(CardRedPacketListVO vo)
        {
            ICardRedPacketListDAO uDAO = CustomerManagementDAOFactory.CardRedPacketListDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取红包分配列表
        /// </summary>
        /// <param name="RedPacketId"></param>
        /// <returns></returns>
        public List<CardRedPacketListVO> GetCardRedPacketListByRPId(int RedPacketId)
        {
            ICardRedPacketListDAO uDAO = CustomerManagementDAOFactory.CardRedPacketListDAO(this.CurrentCustomerProfile);
            try
            {
                List<CardRedPacketListVO> clist= uDAO.FindByParams("RedPacketId = " + RedPacketId);
                return clist;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 查询用户是否领取过该红包  返回null则没有领取过
        /// </summary>
        /// <param name="RedPacketId"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CardRedPacketListVO> GetCardRedPacketListByRPIdAndCid(int RedPacketId,int CustomerId)
        {
            ICardRedPacketListDAO uDAO = CustomerManagementDAOFactory.CardRedPacketListDAO(this.CurrentCustomerProfile);

                List<CardRedPacketListVO> clist = uDAO.FindByParams("RedPacketId = " + RedPacketId+ " and CustomerId="+ CustomerId);
                    return clist;
           
        }

        /// <summary>
        /// 通过openId查询用户是否领取过该红包  领取红包查询专用 一般来说每个用户所有红包最多只能领两个
        /// </summary>
        /// <param name="OpenId"></param>
        /// <returns></returns>
        public List<CardRedPacketListVO> GetCardRedPacketListByOid(string OpenId)
        {
            ICardRedPacketListDAO uDAO = CustomerManagementDAOFactory.CardRedPacketListDAO(this.CurrentCustomerProfile);

            List<CardRedPacketListVO> clist = uDAO.FindByParams("OpenId=" + "'"+ OpenId + "'");
            return clist;

        }
        



        /// <summary>
        /// 删除红包分配列表
        /// </summary>
        /// <param name="RPListId"></param>
        /// <returns></returns>
        public int DeleteCardRedPacketListById(int RPListId)
        {
            ICardRedPacketListDAO uDAO = CustomerManagementDAOFactory.CardRedPacketListDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("RPListId = " + RPListId);
                return 1;
            }
            catch
            {
                return -1;
            }
        }




        /// <summary>
        /// 获取红包列表分配列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardRedPacketListVO> FindCardRedPacketListAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardRedPacketListDAO uDAO = CustomerManagementDAOFactory.CardRedPacketListDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }


        /// <summary>
        /// 获取红包分配列表详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public CardRedPacketListVO FindCardRedPacketListById(int RPListId)
        {
            ICardRedPacketListDAO uDAO = CustomerManagementDAOFactory.CardRedPacketListDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(RPListId);
        }



        /// <summary>
        /// 随机分配红包
        /// </summary>
        /// <param name="redpacketNum">红包数量</param>
        /// <param name="totalAmountm">红包总金额</param>
        /// <param name="minMoney">最低随机范围金额</param>
        /// <param name="CardRedPacketListVO">红包数据</param>
        /// <returns></returns>
        public Boolean RandomAllotRedPackets(int redpacketNum, Decimal totalAmountm, double minMoney, CardRedPacketListVO CardRedPacketListVO)
        {
            try
            {
                //人均最小金额
                double min = minMoney;
                if ((float)totalAmountm < redpacketNum * min)
                    return false;

                int num = redpacketNum;
                List<double> array = new List<double>();
                Random r = new Random();
                for (int i = 0; i < num; i++)
                {
                    if (redpacketNum == 1)
                    {
                        redpacketNum--;
                        array.Add(Convert.ToDouble(totalAmountm.ToString("0.0")));
                        // Console.WriteLine(string.Format("第{0}个红包：{1}元", i + 1, Convert.ToDouble(totalAmountm.ToString("0.0"))));
                        CardRedPacketListVO.RPOneCost = Convert.ToDecimal(Convert.ToDouble(totalAmountm.ToString("0.00")));
                        CardRedPacketListVO.isReceive = 0;
                        this.AddRedPacketList(CardRedPacketListVO);
                    }
                    else
                    {
                        //(totalAmountm - (redpacketNum - 1) * min)：保存剩余金额可以足够的去分配剩余的红包数量
                        double max = ((double)totalAmountm - (redpacketNum - 1) * min) / redpacketNum * 2;
                        double money = r.NextDouble() * max;
                        money = Convert.ToDouble((money <= min ? min : money).ToString("0.00"));
                        redpacketNum--;
                        totalAmountm -= (Decimal)money;
                        array.Add(money);
                        //  Console.WriteLine(string.Format("第{0}个红包：{1}元", i + 1, money));
                        CardRedPacketListVO.RPOneCost = Convert.ToDecimal(money);
                        CardRedPacketListVO.isReceive = 0;
                        this.AddRedPacketList(CardRedPacketListVO);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取销售业绩列表
        /// </summary>
        public List<CardAchievemenViewVO> FindAllByPageIndexByAchievemen(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardAchievemenViewDAO pDAO = CustomerManagementDAOFactory.CardAchievemenViewDAO(this.CurrentCustomerProfile);
            return pDAO.FindAllByPageIndex(conditionStr+ " and AppType="+ AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取销售业绩数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindAchievemenTotalCount(string condition, params object[] parameters)
        {
            ICardAchievemenViewDAO pDAO = CustomerManagementDAOFactory.CardAchievemenViewDAO(this.CurrentCustomerProfile);
            return pDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取会员列表（附带名片信息的）
        /// </summary>
        public List<CustomerCardViewVO> FindAllByPageIndexByCustomerCard(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICustomerCardViewDAO pDAO = CustomerManagementDAOFactory.CustomerCardViewDAO(this.CurrentCustomerProfile);
            return pDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取会员数量（附带名片信息的）
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCustomerCardTotalCount(string condition, params object[] parameters)
        {
            ICustomerCardViewDAO pDAO = CustomerManagementDAOFactory.CustomerCardViewDAO(this.CurrentCustomerProfile);
            return pDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }



        /// <summary>
        /// 企业支付到零钱
        /// </summary>
        /// <param name="money">发送的零钱</param>
        /// <param name="RPId">红包记录id</param>
        /// <param name="openid1">用户的openid</param>
        /// <returns></returns>
        public string PayforWXUser(Decimal money,int RPId, string openid1,string desc="")
        {

            if ((double)money<0.3|| RPId<0|| openid1==null) { return "FAIL"; }

            try
            {
                string KEY = ConfigInfo.Instance.PayKEY;
                string APPSECRET = ConfigInfo.Instance.PayAPPSECRET;//开发者密钥
                string PARTNER_KEY = ConfigInfo.Instance.PaySSLCERT_PASSWORD;//商户密钥

                string url;
                string mch_appid = "wx584477316879d7e9";//wx584477316879d7e9
                string mchid = ConfigInfo.Instance.PayMCHID;//商户号
                string nonce_str = Guid.NewGuid().ToString().Replace("-", ""); //随机字符串
                string openid = openid1;//olLaG5Ft7rcDtCWSgVSAXMNorF_I
                string partner_trade_no = mchid+getTimestamp()+ RPId;//商户订单号
                string check_name = "NO_CHECK";
                int total_fee = Convert.ToInt32(money*100);

                if (desc == "")
                {
                    desc = "恭喜你，成功领取乐聊名片新用户激励现金红包！";
                }
                string spbill_create_ip = "120.78.61.96";


                SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
               
                string wx_nonceStr = Guid.NewGuid().ToString().Replace("-", "");    //Interface_WxPay.getNoncestr();
                dic.Add("mch_appid", mch_appid);
                dic.Add("mchid", mchid);//财付通帐号商家
                dic.Add("nonce_str", nonce_str);
                dic.Add("partner_trade_no", partner_trade_no);
                dic.Add("openid", openid);
                dic.Add("check_name", check_name);
                dic.Add("amount", total_fee.ToString());
                dic.Add("desc", desc);//零钱描述
                dic.Add("spbill_create_ip", spbill_create_ip);   //用户的公网ip，不是商户服务器IP


                string sign = BuildRequest(dic, KEY);//商户秘钥


                string _req_data = "<xml>";
                _req_data += "<mch_appid>" + mch_appid + "</mch_appid>";
                _req_data += "<mchid>" + mchid + "</mchid>";
                _req_data += "<nonce_str>" + nonce_str + "</nonce_str>";
                _req_data += "<partner_trade_no>" + partner_trade_no + "</partner_trade_no>";
                _req_data += "<openid>" + openid + "</openid>";
                _req_data += "<check_name>"+ check_name + "</check_name>";
                _req_data += "<amount>" + total_fee + "</amount>";
                _req_data += "<desc>" + desc + "</desc>";
                _req_data += "<spbill_create_ip>"+ spbill_create_ip + "</spbill_create_ip>";
                _req_data += "<sign>" + sign + "</sign>";
                _req_data += "</xml>";



                url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";
                string jsonStr = HttpPost(url, _req_data.Trim(), true,Encoding.UTF8);
                ResultObject jo = new ResultObject() { Result=jsonStr };
                
                string nodeText = "";
                string nodeText2 = "";
                XmlDocument xx = new XmlDocument();
                xx.LoadXml(jo.Result.ToString());//加载xml
                XmlNodeList xxList = xx.GetElementsByTagName("result_code"); //取得节点名为result_code的XmlNode集合
                XmlNodeList xxList2 = xx.GetElementsByTagName("return_code"); //取得节点名为result_code的XmlNode集合
                foreach (XmlNode xxNode in xxList2)
                {
                    nodeText2 = xxNode.InnerText;//返回的是result_code的文字内容
                }
                foreach (XmlNode xxNode in xxList)
                {
                    nodeText = xxNode.InnerText;//返回的是result_code的文字内容
                }
                if (nodeText2== "FAIL"|| nodeText== "FAIL") { return "FAIL"; }

                return nodeText;

            }
            catch(Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "FAIL";
            }
        }



        /// <summary>
        /// 企业支付到零钱 (自动提现专用)
        /// </summary>
        /// <param name="money">发送的零钱</param>
        /// <param name="RealName">用户真实姓名</param>
        /// <param name="openid1">用户的openid</param>
        /// <returns></returns>
        public string PayforWXUserCash(Decimal money, string RealName,string openid1,int CustomerId,int PayType=1,string setdesc= "会员提现")
        {

            if ((double)money < 0.3 || openid1 == null|| RealName==null) { return "FAIL"; }

            try
            {
                AppVO AppVO = AppBO.GetApp(Type);

                string KEY = AppVO.KEY;
                string APPSECRET = AppVO.APPSECRET;//开发者密钥
                string PARTNER_KEY = AppVO.SSLCERT_PASSWORD;//商户密钥
                string mchid = AppVO.MCHID;//商户号

                string url;  
                string mch_appid = AppVO.AppId;
                
                string nonce_str = Guid.NewGuid().ToString().Replace("-", ""); //随机字符串
                string openid = openid1;//olLaG5Ft7rcDtCWSgVSAXMNorF_I
                string partner_trade_no = mchid + getTimestamp()+ CustomerId;//商户订单号
                string check_name = "FORCE_CHECK";
                int total_fee = Convert.ToInt32(money * 100);
                string desc = setdesc;
                string spbill_create_ip = "120.78.61.96";


                SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

                string wx_nonceStr = Guid.NewGuid().ToString().Replace("-", "");    //Interface_WxPay.getNoncestr();
                dic.Add("mch_appid", mch_appid);
                dic.Add("mchid", mchid);//财付通帐号商家
                dic.Add("nonce_str", nonce_str);
                dic.Add("partner_trade_no", partner_trade_no);
                dic.Add("openid", openid);
                dic.Add("check_name", check_name);
                dic.Add("re_user_name", RealName);
                dic.Add("amount", total_fee.ToString());
                dic.Add("desc", desc);//零钱描述
                dic.Add("spbill_create_ip", spbill_create_ip);   //用户的公网ip，不是商户服务器IP


                string sign = BuildRequest(dic, KEY);//商户秘钥


                string _req_data = "<xml>";
                _req_data += "<mch_appid>" + mch_appid + "</mch_appid>";
                _req_data += "<mchid>" + mchid + "</mchid>";
                _req_data += "<nonce_str>" + nonce_str + "</nonce_str>";
                _req_data += "<partner_trade_no>" + partner_trade_no + "</partner_trade_no>";
                _req_data += "<openid>" + openid + "</openid>";
                _req_data += "<check_name>" + check_name + "</check_name>";
                _req_data += "<re_user_name>" + RealName + "</re_user_name>";
                _req_data += "<amount>" + total_fee + "</amount>";
                _req_data += "<desc>" + desc + "</desc>";
                _req_data += "<spbill_create_ip>" + spbill_create_ip + "</spbill_create_ip>";
                _req_data += "<sign>" + sign + "</sign>";
                _req_data += "</xml>";

                

                url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";
                string jsonStr = HttpPost(url, _req_data.Trim(), true, Encoding.UTF8,AppType);

                ResultObject jo = new ResultObject() { Result = jsonStr };

                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "测试提现:" + jsonStr;
                _log.Error(strErrorMsg);

                string nodeText = "";
                string nodeText2 = "";
                string nodeText3 = "";
                string nodeText4 = "";
                XmlDocument xx = new XmlDocument();
                xx.LoadXml(jo.Result.ToString());//加载xml
                XmlNodeList xxList = xx.GetElementsByTagName("result_code"); //取得节点名为result_code的XmlNode集合
                XmlNodeList xxList2 = xx.GetElementsByTagName("return_code"); //取得节点名为result_code的XmlNode集合
                XmlNodeList xxList3 = xx.GetElementsByTagName("err_code"); //取得节点名为result_code的XmlNode集合
              

                foreach (XmlNode xxNode in xxList2)
                {
                    nodeText2 = xxNode.InnerText;//返回的是result_code的文字内容
                }
                foreach (XmlNode xxNode in xxList)
                {
                    nodeText = xxNode.InnerText;//返回的是result_code的文字内容
                }

                if (nodeText2 == "FAIL" || nodeText == "FAIL") {
                    foreach (XmlNode xxNode in xxList3)
                    {
                        nodeText3 = xxNode.InnerText;//返回的是result_code的文字内容
                    }
                    if (nodeText3 == "NOTENOUGH")//商户平台帐户余额不足
                    {
                        return "NOTENOUGH";
                    }
                    else if (nodeText3 == "NAME_MISMATCH")//微信绑定用户身份与填写的不一致
                    {
                        return "NAME_MISMATCH";
                    }
                    else if (nodeText3 == "AMOUNT_LIMIT")//金额超限，目前最低付款金额为1元，最高10万元，请确认是否付款金额超限
                    {
                        return "AMOUNT_LIMIT";
                    }
                    else if (nodeText3 == "MONEY_LIMIT")//已经达到今日付款总额上限/已达到付款给此用户额度上限
                    {
                        return "MONEY_LIMIT";
                    }
                    else if (nodeText3 == "V2_ACCOUNT_SIMPLE_BAN")//无法给未实名用户付款
                    {
                        return "V2_ACCOUNT_SIMPLE_BAN";
                    }
                    else {
                        return "FAIL";
                    }
                }

                return "SUCCESS";

            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "FAIL";
            }
        }

        /// <summary>
        /// 企业支付到零钱(粤省情抽奖)
        /// </summary>
        /// <param name="money">发送的零钱</param>
        /// <param name="RPId">红包记录id</param>
        /// <param name="openid1">用户的openid</param>
        /// <returns></returns>
        public string PayforWXLotteries(Decimal money, int lottery_id, int personal_id, string openid1,int AppType ,string desc = "")
        {

            if ((double)money < 0.3 || lottery_id < 0 || openid1 == null) { return "FAIL"; }

            try
            {
                AppVO AppVO = AppBO.GetApp(AppType);

                string KEY = AppVO.KEY;
                string APPSECRET = AppVO.APPSECRET;//开发者密钥
                string PARTNER_KEY = AppVO.SSLCERT_PASSWORD;//商户密钥
                string mchid = AppVO.MCHID;//商户号

                string url;
                string mch_appid = AppVO.AppId;
                string nonce_str = Guid.NewGuid().ToString().Replace("-", ""); //随机字符串
                string openid = openid1;//olLaG5Ft7rcDtCWSgVSAXMNorF_I
                string partner_trade_no = mchid + getTimestamp() + lottery_id + personal_id;//商户订单号
                string check_name = "NO_CHECK";
                int total_fee = Convert.ToInt32(money * 100);

                if (desc == "")
                {
                    desc = "恭喜你，成功领取问卷现金红包！";
                }
                string spbill_create_ip = "14.23.168.66";


                SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

                string wx_nonceStr = Guid.NewGuid().ToString().Replace("-", "");    //Interface_WxPay.getNoncestr();
                dic.Add("mch_appid", mch_appid);
                dic.Add("mchid", mchid);//财付通帐号商家
                dic.Add("nonce_str", nonce_str);
                dic.Add("partner_trade_no", partner_trade_no);
                dic.Add("openid", openid);
                dic.Add("check_name", check_name);
                dic.Add("amount", total_fee.ToString());
                dic.Add("desc", desc);//零钱描述
                dic.Add("spbill_create_ip", spbill_create_ip);   //用户的公网ip，不是商户服务器IP


                string sign = BuildRequest(dic, KEY);//商户秘钥


                string _req_data = "<xml>";
                _req_data += "<mch_appid>" + mch_appid + "</mch_appid>";
                _req_data += "<mchid>" + mchid + "</mchid>";
                _req_data += "<nonce_str>" + nonce_str + "</nonce_str>";
                _req_data += "<partner_trade_no>" + partner_trade_no + "</partner_trade_no>";
                _req_data += "<openid>" + openid + "</openid>";
                _req_data += "<check_name>" + check_name + "</check_name>";
                _req_data += "<amount>" + total_fee + "</amount>";
                _req_data += "<desc>" + desc + "</desc>";
                _req_data += "<spbill_create_ip>" + spbill_create_ip + "</spbill_create_ip>";
                _req_data += "<sign>" + sign + "</sign>";
                _req_data += "</xml>";



                url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";
                string jsonStr = BaseHttpPost(url, _req_data.Trim(), true, Encoding.UTF8, AppType);
                ResultObject jo = new ResultObject() { Result = jsonStr };
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "测试提现:" + jsonStr;
                _log.Error(strErrorMsg);
                string nodeText = "";
                string nodeText2 = "";
                XmlDocument xx = new XmlDocument();
                xx.LoadXml(jo.Result.ToString());//加载xml
                XmlNodeList xxList = xx.GetElementsByTagName("result_code"); //取得节点名为result_code的XmlNode集合
                XmlNodeList xxList2 = xx.GetElementsByTagName("return_code"); //取得节点名为result_code的XmlNode集合
                foreach (XmlNode xxNode in xxList2)
                {
                    nodeText2 = xxNode.InnerText;//返回的是result_code的文字内容
                }
                foreach (XmlNode xxNode in xxList)
                {
                    nodeText = xxNode.InnerText;//返回的是result_code的文字内容
                }
                if (nodeText2 == "FAIL" || nodeText == "FAIL") { return "FAIL"; }

                return nodeText;

            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "FAIL";
            }
        }


        public static string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <returns></returns>
        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string key)
        {
            //获取过滤后的数组
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = FilterPara(sParaTemp);

            //组合参数数组
            string prestr = CreateLinkString(dicPara);
            //拼接支付密钥
            string stringSignTemp = prestr + "&key=" + key;


            //获得加密结果
            string myMd5Str = GetMD5(stringSignTemp.Trim());

            //返回转换为大写的加密串
            return myMd5Str.ToUpper();
        }
        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key != "sign" && !string.IsNullOrEmpty(temp.Value))
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }

        //组合参数数组
        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        //加密
        public static string GetMD5(string pwd)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(pwd));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
          //post 提交数据xml格式
        public static string HttpPost(string postUrl, string paramData,bool isUseCert, Encoding dataEncode,int AppType=1)
        {
            string ret = string.Empty;
            string mchid = ConfigInfo.Instance.PayMCHID;//商户号
            string apiclient = "C:/web/ca/1494588702_20240312_cert/apiclient_cert.p12";

            if (AppType == 3) {
                mchid = "1494588702";
                apiclient = "C:/web/ca/1494588702_20240312_cert/apiclient_cert.p12";
            }
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;

                //是否使用证书
                if (isUseCert)
                {
                    string path = HttpContext.Current.Request.PhysicalApplicationPath;
                    X509Certificate2 cert = new X509Certificate2(apiclient, mchid, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);//线上发布需要添加
                    webReq.ClientCertificates.Add(cert);
                }

                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return ret;
        }

        public static string BaseHttpPost(string postUrl, string paramData, bool isUseCert, Encoding dataEncode, int AppType = 1)
        {
            string ret = string.Empty;
            AppVO AppVO = AppBO.GetApp(AppType);
            string mchid = AppVO.MCHID;
            string apiclient = AppVO.SSLCERT_PATH;

            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;

                //是否使用证书
                if (isUseCert)
                {
                    string path = HttpContext.Current.Request.PhysicalApplicationPath;
                    X509Certificate2 cert = new X509Certificate2(apiclient, mchid, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);//线上发布需要添加
                    webReq.ClientCertificates.Add(cert);
                }

                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// Datatable生成Excel表格并返回路径
        /// </summary>
        /// <param name="m_DataTable">Datatable</param>
        /// <param name="s_folder">自定义路径</param>
        /// <param name="s_FileName">文件名</param>
        /// <returns></returns>
        public string DataToExcel(System.Data.DataTable m_DataTable, string s_folder, string s_FileName)
        {
            try{
                string folder = "/UploadFolder/ExcelFile/"+ s_folder;
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                string ExcelUrl = ConfigInfo.Instance.APIURL + folder + s_FileName;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }

                EXCELHelper.ImportExcel(m_DataTable, localPath, s_FileName);
                return ExcelUrl;        //返回生成文件的绝对路径
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
   
                return null;
            }

        }


        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddLog(LogVO vo)
        {
            try
            {
                ILogDAO uDAO = CustomerManagementDAOFactory.LogDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int LogID = uDAO.Insert(vo);
                    return LogID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新日志
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateLog(LogVO vo)
        {
            ILogDAO uDAO = CustomerManagementDAOFactory.LogDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="LogID"></param>
        /// <returns></returns>
        public int DeleteLogById(int LogID)
        {
            ILogDAO uDAO = CustomerManagementDAOFactory.LogDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("LogID = " + LogID+ " and AppType="+ AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<LogVO> FindLogAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ILogDAO uDAO = CustomerManagementDAOFactory.LogDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取日志数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindLogCount(string condition, params object[] parameters)
        {
            ILogDAO uDAO = CustomerManagementDAOFactory.LogDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取日志详情
        /// </summary>
        /// <param name="LogID"></param>
        /// <returns></returns>
        public LogVO FindLogById(int LogID)
        {
            ILogDAO uDAO = CustomerManagementDAOFactory.LogDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(LogID);
        }

        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <param name="RedPacketId"></param>
        /// <returns></returns>
        public List<LogVO> GetLogVOList(string condtion)
        {
            ILogDAO uDAO = CustomerManagementDAOFactory.LogDAO(this.CurrentCustomerProfile);
            try
            {
                List<LogVO> clist = uDAO.FindByParams(condtion + " and AppType=" + AppType);
                return clist;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 添加VIP订单
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddOrder(CardOrderVO vo)
        {
            try
            {
                ICardOrderDAO rDAO = CustomerManagementDAOFactory.CardOrderDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int CardOrderID = rDAO.Insert(vo);
                    return CardOrderID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新VIP订单
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateOrder(CardOrderVO vo)
        {
            ICardOrderDAO rDAO = CustomerManagementDAOFactory.CardOrderDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 获取VIP订单
        /// </summary>
        /// <param name="CardOrderID"></param>
        /// <returns></returns>
        public CardOrderVO FindOrderById(int CardOrderID)
        {
            ICardOrderDAO rDAO = CustomerManagementDAOFactory.CardOrderDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(CardOrderID);
        }

        public List<CardOrderVO> FindOrderByCondtion(string condtion)
        {
            ICardOrderDAO rDAO = CustomerManagementDAOFactory.CardOrderDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        public List<CardOrderViewVO> FindOrderViewByCondtion(string condtion)
        {
            ICardOrderViewDAO rDAO = CustomerManagementDAOFactory.CardOrderViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion+ " and AppType="+ AppType);
        }

        public List<CardOrderViewVO> FindOrderViewByGROUP(string condtion)
        {
            ICardOrderViewDAO rDAO = CustomerManagementDAOFactory.CardOrderViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(condtion + " and AppType=" + AppType+ "  GROUP BY CustomerId");
        }

        /// <summary>
        /// 获取VIP订单列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardOrderViewVO> FindCardOrderViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardOrderViewDAO rDAO = CustomerManagementDAOFactory.CardOrderViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }


        /// <summary>
        /// 获取VIP订单列表数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardOrderViewTotalCount(string condition, params object[] parameters)
        {
            ICardOrderViewDAO rDAO = CustomerManagementDAOFactory.CardOrderViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取VIP订单列表数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardOrderTotalCount(string condition, params object[] parameters)
        {
            ICardOrderDAO rDAO = CustomerManagementDAOFactory.CardOrderDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        public decimal FindOrderSumByCondtion(string Sum, string condtion)
        {
            ICardOrderDAO rDAO = CustomerManagementDAOFactory.CardOrderDAO(this.CurrentCustomerProfile);
            string sql = condtion + " and AppType=" + AppType;
            if (Sum == "Cost")//不计算企业名片复制过来的订单
                sql += " and OrderNO not like '%BusinessCardOrder%'";
            return rDAO.FindTotalSum(Sum, sql);
        }

        /// <summary>
        /// 获取提现金额（微云智推）
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public decimal getPayoutCost(int CustomerId) {
            decimal PayoutCost = 0;
            try
            {
                List<CardOrderVO> cVO = FindOrderByCondtion("OneRebateCustomerId = " + CustomerId + " and Status=1 and payAt is not NULL and OneRebateStatus=0");
                foreach(CardOrderVO item in cVO)
                {
                    PayoutCost += item.OneRebateCost;
                    item.OneRebateStatus = 2;
                    UpdateOrder(item);
                }

                List<CardOrderVO> cVO2 = FindOrderByCondtion("TwoRebateCustomerId = " + CustomerId + " and Status=1 and payAt is not NULL and TwoRebateStatus=0");
                foreach (CardOrderVO item in cVO2)
                {
                    PayoutCost += item.TwoRebateCost;
                    item.TwoRebateStatus = 2;
                    UpdateOrder(item);
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
            return PayoutCost;
        }

        /// <summary>
        /// 提现失败（微云智推）
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public void PayoutFAIL(int CustomerId)
        {
            try
            {
                List<CardOrderVO> cVO = FindOrderByCondtion("OneRebateCustomerId = " + CustomerId + " and Status=1 and payAt is not NULL and OneRebateStatus=2");
                foreach (CardOrderVO item in cVO)
                {
                    item.OneRebateStatus = 0;
                    UpdateOrder(item);
                }

                List<CardOrderVO> cVO2 = FindOrderByCondtion("TwoRebateCustomerId = " + CustomerId + " and Status=1 and payAt is not NULL and TwoRebateStatus=2");
                foreach (CardOrderVO item in cVO2)
                {
                    item.TwoRebateStatus = 0;
                    UpdateOrder(item);
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
            }
        }

        /// <summary>
        /// 提现成功（微云智推）
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public void PayoutSUCCESS(int CustomerId)
        {
            try
            {
                List<AgentCostByPayout> AgentCost = new List<AgentCostByPayout>();
                List<CardOrderVO> cVO = FindOrderByCondtion("OneRebateCustomerId = " + CustomerId + " and Status=1 and payAt is not NULL and OneRebateStatus=2");
                foreach (CardOrderVO item in cVO)
                {
                    item.OneRebateStatus = 1;
                    UpdateOrder(item);
                    if (item.OneRebateAgentCustomerId > 0)
                    {
                        bool iscid = false;
                        for(int i=0;i< AgentCost.Count; i++)
                        {
                            if(AgentCost[i].AgentCustomerId== item.OneRebateAgentCustomerId)
                            {
                                AgentCost[i].Cost += item.OneRebateCost;
                                iscid = true;
                            }
                        }
                        if (!iscid)
                        {
                            AgentCostByPayout ap = new AgentCostByPayout();
                            ap.AgentCustomerId = item.OneRebateAgentCustomerId;
                            ap.Cost= item.OneRebateCost;
                            AgentCost.Add(ap);
                        }
                    }
                }

                List<CardOrderVO> cVO2 = FindOrderByCondtion("TwoRebateCustomerId = " + CustomerId + " and Status=1 and payAt is not NULL and TwoRebateStatus=2");
                foreach (CardOrderVO item in cVO2)
                {
                    item.TwoRebateStatus = 1;
                    UpdateOrder(item);
                    if (item.TwoRebateAgentCustomerId > 0)
                    {
                        bool iscid = false;
                        for (int i = 0; i < AgentCost.Count; i++)
                        {
                            if (AgentCost[i].AgentCustomerId == item.TwoRebateAgentCustomerId)
                            {
                                AgentCost[i].Cost += item.TwoRebateCost;
                                iscid = true;
                            }
                        }
                        if (!iscid)
                        {
                            AgentCostByPayout ap = new AgentCostByPayout();
                            ap.AgentCustomerId = item.TwoRebateAgentCustomerId;
                            ap.Cost = item.TwoRebateCost;
                            AgentCost.Add(ap);
                        }
                    }
                }
                foreach(AgentCostByPayout item in AgentCost)
                {
                    AddAgentDeposit(-item.Cost, item.AgentCustomerId, "会员提现", CustomerId);
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
            }
        }

        public class AgentCostByPayout
        {
            public int AgentCustomerId{get;set;}
            public decimal Cost { get; set; }
        }

        /// <summary>
        /// 添加访问记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddAccessrecords(CardAccessRecordsVO vo)
        {
            try
            {
                ICardAccessRecordsDAO rDAO = CustomerManagementDAOFactory.CardAccessRecordsDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int AccessRecordsID = rDAO.Insert(vo);
                    return AccessRecordsID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新访问记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateAccessrecords(CardAccessRecordsVO vo)
        {
            ICardAccessRecordsDAO rDAO = CustomerManagementDAOFactory.CardAccessRecordsDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 更新访问的会员CustomerId
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateAccessrecordsCustomerId(int CustomerId, string OpenID)
        {
            ICardAccessRecordsDAO rDAO = CustomerManagementDAOFactory.CardAccessRecordsDAO(this.CurrentCustomerProfile);
            try
            {
                int r=rDAO.UpdateCustomerId(CustomerId, OpenID, AppType);
                if (r >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取访问记录数量
        /// </summary>
        /// <returns></returns>
        public int FindAccessrecordsCount(string condition,bool isG=false)
        {
            ICardAccessRecordsDAO rDAO = CustomerManagementDAOFactory.CardAccessRecordsDAO(this.CurrentCustomerProfile);
            string sql = "";
            if (isG)
            {
                sql = condition + " and AppType=" + AppType + " GROUP BY Type,ById,OpenID";
            }
            else
            {
                sql = condition + " and AppType=" + AppType;
            }
            
            return rDAO.FindTotalCount(sql);
        }

        /// <summary>
        /// 判断是否满足邀约浏览的条件
        /// </summary>
        /// <returns></returns>
        public bool isPromotionRead(int customerId,int PartyID,CardPartyCostVO CostVO,CardPartyVO cpvo)
        {
            if (cpvo.isPromotionRead == 1 && CostVO.PromotionRead > 0)
            {
                int Read = FindAccessrecordsCount("ShareCustomerId=" + customerId + " and CustomerId<>" + customerId + " and ById=" + PartyID + " and Type='ReadParty'", true);
                if (Read >= CostVO.PromotionRead)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
            
        }

        /// <summary>
        /// 判断是否满足邀约报名的条件
        /// </summary>
        /// <returns></returns>
        public bool isPromotionSignup(int customerId, int PartyID, CardPartyCostVO CostVO, CardPartyVO cpvo)
        {
            if (cpvo.isPromotionSignup == 1 && CostVO.PromotionSignup > 0)
            {
                int Signup = FindPartyOrderTotalCount("InviterCID=" + customerId + " and CustomerId<>" + customerId + " and Status=1 and PartyID=" + PartyID);
                if (Signup >= CostVO.PromotionSignup)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 获取访问记录详情
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public CardAccessRecordsVO FindAccessrecordsByAccessrecordsID(int AccessrecordsID)
        {
            ICardAccessRecordsDAO rDAO = CustomerManagementDAOFactory.CardAccessRecordsDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(AccessrecordsID);
        }

        /// <summary>
        /// 获取访问记录列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<CardAccessRecordsVO> FindAccessrecordsByCondtion(string condtion)
        {
            ICardAccessRecordsDAO rDAO = CustomerManagementDAOFactory.CardAccessRecordsDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion+ " and AppType="+ AppType);
        }

        /// <summary>
        /// 获取访问记录列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<CardAccessRecordsVO> FindAccessrecordsByCondtion(string condtion,int LIMIT)
        {
            ICardAccessRecordsDAO rDAO = CustomerManagementDAOFactory.CardAccessRecordsDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion + " and AppType=" + AppType+ " GROUP BY CustomerId ORDER BY AccessAt DESC LIMIT "+ LIMIT);
        }

        /// <summary>
        /// 删除访问记录
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public int DeleteAccessrecords(string condtion)
        {
            ICardAccessRecordsDAO rDAO = CustomerManagementDAOFactory.CardAccessRecordsDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams(condtion);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 开通VIP
        /// </summary>
        /// <returns></returns>
        public bool OpeningVIP(int CardOrderID)
        {
            CardOrderVO CardOrderVO = FindOrderById(CardOrderID);
            if (CardOrderVO == null)
            {
                return false;
            }
            if (CardOrderVO.isUsed>0)
            {
                //该订单已使用过
                return false;
            }
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO uVO = uBO.FindCustomenById(CardOrderVO.CustomerId);
            if (uVO == null)
            {
                return false;
            }
            
            int month = 0;
            int OldVipLevel = uVO.VipLevel;

            if (CardOrderVO.Type == 1)
            {
                month = 1;
                uVO.VipLevel = 1;
            }
            else if(CardOrderVO.Type == 2)
            {
                month = 12;
                uVO.VipLevel = 1;
            }
            else if (CardOrderVO.Type == 3)
            {
                month = 1200;
                uVO.VipLevel = 1;
            }
            else if (CardOrderVO.Type == 5)
            {
                month = 3;
                uVO.VipLevel = 1;
            }
            else if (CardOrderVO.Type == 6)
            {
                month = 18;
                if (uVO.isVip && uVO.ExpirationAt > DateTime.Now)
                {
                    if (uVO.VipLevel == 1)
                        uVO.VipLevel = 2;
                }
                else
                {
                    uVO.VipLevel = 2;
                }
            }
            else if (CardOrderVO.Type == 7)
            {
                month = 18;
                uVO.VipLevel = 3;
            }

            else if (CardOrderVO.Type == 8)
            {
                month = 1;
                if(uVO.VipLevel!=2&& uVO.VipLevel!=3)
                    uVO.VipLevel = 4;
            }
            else if (CardOrderVO.Type == 9)
            {
                month = 12;
                if (uVO.VipLevel != 2 && uVO.VipLevel != 3)
                    uVO.VipLevel = 4;
            }
            else if (CardOrderVO.Type == 10)
            {
                month = 12;
                if (uVO.VipLevel != 2 && uVO.VipLevel != 3)
                    uVO.VipLevel = 5;
            }

            //判断是否是升级
            bool isUP = false;
            if (OldVipLevel == 4 && (uVO.VipLevel == 5 || uVO.VipLevel == 1))
                isUP = true;
            if (OldVipLevel == 5 && uVO.VipLevel == 1)
                isUP = true;

            //升级就按照新有效期
            //续费
            if (uVO.isVip && uVO.ExpirationAt > DateTime.Now && !isUP)
            {
                uVO.ExpirationAt = uVO.ExpirationAt.AddMonths(month);
            }
            //开通
            else
            {
                uVO.ExpirationAt = DateTime.Now.AddMonths(month);
            }

            uVO.isVip = true;

            if (uBO.Update(uVO))
            {
                string Message = "已为您开通了" + month + "个月的vip，感谢您的支持！";
                if (isUP) Message = "已为您升级了vip，感谢您的支持！";
                AddCardMessage(Message, uVO.CustomerId, "会员特权", "/pages/MyCenter/MyCenter/MyCenter", "switchTab");
                CardOrderVO.isUsed = 1;
                UpdateOrder(CardOrderVO);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 添加消息提醒
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCardMessage(string Title, int CustomerId,string Type,string Url,string NavigateType= "navigateTo")
        {
            CardMessageVO vo = new CardMessageVO();
            vo.CustomerId = CustomerId;
            vo.Title = Title;
            vo.Type = Type;
            vo.Url = Url;
            vo.NavigateType = NavigateType;
            vo.CreatedAt = DateTime.Now;
            vo.AppType = AppType;
            try
            {
                sendTemplateMessage(CustomerId, Type, Url, Title);
            }
            catch
            {

            }
            
            try
            {
                ICardMessageDAO rDAO = CustomerManagementDAOFactory.CardMessageDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int MessageID = rDAO.Insert(vo);
                    return MessageID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新消息提醒阅读状态
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardMessage(int MessageID)
        {
            CardMessageVO vo = FindCardMessageByMessageID(MessageID);
            vo.Status = true;
            ICardMessageDAO rDAO = CustomerManagementDAOFactory.CardMessageDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }


        /// <summary>
        /// 获取消息提醒列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardMessageVO> FindCardMessageAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardMessageDAO rDAO = CustomerManagementDAOFactory.CardMessageDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr+ " and AppType="+ AppType, start, end, sortcolname, asc, parameters);
        }

        public List<CardMessageVO> FindCardMessageByCondtion(string condtion)
        {
            ICardMessageDAO rDAO = CustomerManagementDAOFactory.CardMessageDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取消息提醒数量
        /// </summary>
        /// <returns></returns>
        public int FindCardMessageCount(string condition)
        {
            ICardMessageDAO rDAO = CustomerManagementDAOFactory.CardMessageDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 获取消息提醒详情
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public CardMessageVO FindCardMessageByMessageID(int MessageID)
        {
            ICardMessageDAO rDAO = CustomerManagementDAOFactory.CardMessageDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(MessageID);
        }


        /// <summary>
        /// 添加订阅消息会员
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddSubscription(int CustomerId, string OpenId)
        {
            ICardSubscriptionDAO rDAO = CustomerManagementDAOFactory.CardSubscriptionDAO(this.CurrentCustomerProfile);
            List<SubscriptionVO> sVOlist = rDAO.FindByParams("CustomerId=" + CustomerId+ " and Type="+ Type);

            if (sVOlist.Count > 0)
            {
                return sVOlist[0].SubscriptionID;
            }

            SubscriptionVO sVO = new SubscriptionVO();
            sVO.SubscriptionID = 0;
            sVO.CustomerId = CustomerId;
            sVO.OpenId = OpenId;
            sVO.CreatedAt = DateTime.Now;
            sVO.Type = Type;
            sVO.AppType = AppType;

            try
            {
                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int SubscriptionID = rDAO.Insert(sVO);
                    return SubscriptionID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 判断会员是否订阅消息
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool IsSubscription(int CustomerId)
        {
            ICardSubscriptionDAO rDAO = CustomerManagementDAOFactory.CardSubscriptionDAO(this.CurrentCustomerProfile);
            List<SubscriptionVO> sVOlist = rDAO.FindByParams("CustomerId=" + CustomerId + " and Type=" + Type + " and AppType="+ AppType);
            if (sVOlist.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取订阅会员
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public SubscriptionVO FindSubscriptionByCustomerId(int CustomerId)
        {
            ICardSubscriptionDAO rDAO = CustomerManagementDAOFactory.CardSubscriptionDAO(this.CurrentCustomerProfile);
            List<SubscriptionVO> sVOlist = rDAO.FindByParams("CustomerId=" + CustomerId + " and Type=" + Type + " and AppType=" + AppType);
            if (sVOlist.Count > 0)
            {
                return sVOlist[0];
            }
            return null;
        }

        /// <summary>
        /// 发送订阅消息（日志记录提醒）
        /// </summary>
        /// <param name="CustomerId">接收人ID</param>
        /// <param name="formId"></param>
        /// <returns></returns>
        public string sendTemplateMessage(int CustomerId,string Type, string page, string details)
        {
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret + "";
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
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token=" + result.SuccessResult.access_token;

                SubscriptionVO cVO = FindSubscriptionByCustomerId(CustomerId);

                details = details.Length >= 20 ? Convert.ToString(details).Substring(0, 17) + "..." : details;

                if (page == "")
                {
                    page = "pages/MyCenter/Message/Message";
                }else
                {
                    if(page.Substring(0, 1) == "/")
                    {
                        page = page.Substring(1);
                    }
                }

                if (cVO != null)
                {
                    string template_id = "ryAvM5WAefdJTRTwMtWFmGzHbAWbOYc4VArHyJgpYKk";
                    if (this.Type == 2)
                    {
                        template_id = "JZtTANkCtMXTtXiqzbvM-ARolrs0WgeQrCSfZbIdPrA";
                    }
                    if (this.Type == 4)
                    {
                        template_id = "RNz49A_h9m5FHmESxGgeh1uracu4qxvFWwKshINCo18";
                    }


                    DataJson = "{";
                    DataJson += "\"touser\": \"" + cVO.OpenId + "\",";
                    DataJson += "\"template_id\": \""+ template_id + "\",";
                    DataJson += "\"page\": \""+ page + "\",";
                    DataJson += "\"data\": {";
                    DataJson += "\"phrase1\": {";
                    DataJson += "\"value\": \"" + Type + "\"";
                    DataJson += "},";
                    DataJson += "\"thing4\": {";
                    DataJson += "\"value\": \""+ details + "\"";
                    DataJson += "},";
                    DataJson += "\"date2\": {";
                    DataJson += "\"value\": \"" + DateTime.Now.ToString("yyyy年MM月dd日 HH:mm") + "\"";
                    DataJson += "}";
                    DataJson += "}";
                    DataJson += "}";

                    string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);
                    LogBO _log = new LogBO(typeof(CardBO));
                    _log.Error(str);
                    return str;
                }
                return "找不到订阅会员";
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "error";
            }
        }

        /// <summary>
        /// 发送订阅消息（抽奖结果通知）
        /// </summary>
        /// <param name="CustomerId">接收人ID</param>
        /// <param name="formId"></param>
        /// <returns></returns>
        public string sendWinningMessage(string OpenId, int PartyID,string Title, string Names)
        {
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret + "";
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
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token=" + result.SuccessResult.access_token;

                string page = "package/package_sweepstakes/PartyShow/PartyShow?PartyID=" + PartyID;
                DataJson = "{";
                DataJson += "\"touser\": \"" + OpenId + "\",";
                DataJson += "\"template_id\": \"PrIzrJaNSZ70EXq5c39ab72gFZZv8SrRrPqPl5qmVMk\",";
                DataJson += "\"page\": \"" + page + "\",";
                DataJson += "\"data\": {";
                DataJson += "\"thing1\": {";
                DataJson += "\"value\": \"" + Title + "\"";
                DataJson += "},";
                DataJson += "\"name2\": {";
                DataJson += "\"value\": \"" + Names + "\"";
                DataJson += "},";
                DataJson += "\"date3\": {";
                DataJson += "\"value\": \"" + DateTime.Now.ToString("yyyy年MM月dd日") + "\"";
                DataJson += "},";
                DataJson += "\"thing5\": {";
                DataJson += "\"value\": \"开奖啦！赶紧戳进来看看大奖名单。\"";
                DataJson += "}";
                DataJson += "}";
                DataJson += "}";

                string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);
                /*
                LogBO _log = new LogBO(typeof(CardBO));
                _log.Error("发送开奖通知："+str);
                */
                return str;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "error";
            }
        }

        /// <summary>
        /// 发送留言消息（通用）
        /// </summary>
        /// <param name="CustomerId">接收人ID</param>
        /// <param name="formId"></param>
        /// <returns></returns>
        public string sendLeavingMessage(string Appid, string Secret, string template_id, string OpenId, string page, string Name, string Title,string tic= "点击进入查看")
        {
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + Appid + "&secret=" + Secret + "";
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
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token=" + result.SuccessResult.access_token;

                DataJson = "{";
                DataJson += "\"touser\": \"" + OpenId + "\",";
                DataJson += "\"template_id\": \""+ template_id + "\",";
                DataJson += "\"page\": \"" + page + "\",";
                DataJson += "\"data\": {";
                DataJson += "\"name3\": {";
                DataJson += "\"value\": \"" + Name + "\"";
                DataJson += "},";
                DataJson += "\"thing1\": {"; 
                DataJson += "\"value\": \"" + Title + "\"";
                DataJson += "},";
                DataJson += "\"time2\": {";
                DataJson += "\"value\": \"" + DateTime.Now.ToString("yyyy年MM月dd日") + "\"";
                DataJson += "},";
                DataJson += "\"thing4\": {";
                DataJson += "\"value\": \""+ tic + "\"";
                DataJson += "}";
                DataJson += "}";
                DataJson += "}";

                string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);
                LogBO _log = new LogBO(typeof(CardBO));
                _log.Error("发送留言通知结果："+str);
                return str;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "error";
            }
        }

        /// <summary>
        /// 发送留言信息
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool sendMessage(int sendCount,int Style=1, string condtion= "1=1")
        {
            if (!CardBO.isOpenSend)
            {
                return false;
            }
            CardBO.isOpenSend = false;
            try
            {
                List<CardFormListViewVO> cVO = FindFormListView(condtion+ " and Style="+ Style);
                if (cVO == null) return false;
                string Appid = appid;
                string Secret = secret;
                string page = "plugin-private://wx2b03c6e691cd7370/pages/live-player-plugin?room_id=4&custom_params=%7B%22path%22%3A%22pages%2Findex%2Findex%22%2C%22pid%22%3A1%7D";
                string Name = "销冠是怎么样炼成的";
                string Title = "销冠第一课堂，免费公开课，正在直播中";
                string template_id = "vxXB_zQd9qiDGWYgm7eh0twwNe0SwM7tEA_UurK4z3o";
                for(int i=0;i< cVO.Count&&i< sendCount; i++)
                {
                    try
                    {
                        sendLeavingMessage(Appid, Secret, template_id, cVO[i].OpenId, page, Name, Title,"点击立即观看");
                        DeleteFormIdById(cVO[i].FormListID);
                    }
                    catch
                    {

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }


        /// <summary>
        /// 添加签到表
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddQuestionnaire(CardQuestionnaireVO vo)
        {
            try
            {
                ICardQuestionnaireDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int AccessRecordsID = rDAO.Insert(vo);
                    return AccessRecordsID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新签到表
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardQuestionnaire(CardQuestionnaireVO vo)
        {
            ICardQuestionnaireDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 删除签到表
        /// </summary>
        /// <param name="QuestionnaireID"></param>
        /// <returns></returns>
        public int DeleteByQuestionnaireID(int QuestionnaireID)
        {
            ICardQuestionnaireDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("QuestionnaireID = " + QuestionnaireID+ " and AppType="+ AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取签到表详情
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public CardQuestionnaireVO FindCardQuestionnaireByQuestionnaireID(int QuestionnaireID)
        {
            ICardQuestionnaireDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(QuestionnaireID);
        }

        /// <summary>
        /// 获取签到表列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<CardQuestionnaireVO> FindCardQuestionnaireByCondtion(string condtion)
        {
            ICardQuestionnaireDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion+ " and AppType="+ AppType);
        }

        /// <summary>
        /// 获取签到表列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardQuestionnaireVO> FindCardQuestionnaireAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardQuestionnaireDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取签到表数量
        /// </summary>
        /// <returns></returns>
        public int FindCardQuestionnaire(string condition)
        {
            ICardQuestionnaireDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition + " and AppType=" + AppType);
        }

        /// <summary>
        /// 获取签到表列表(视图)
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardQuestionnaireViewVO> FindCardQuestionnaireViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardQuestionnaireViewDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取签到表数量(视图)
        /// </summary>
        /// <returns></returns>
        public int FindCardQuestionnaireView(string condition)
        {
            ICardQuestionnaireViewDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition + " and AppType=" + AppType);
        }

        /// <summary>
        /// 添加签到
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddQuestionnaireSignup(CardQuestionnaireSignupVO vo)
        {
            try
            {
                ICardQuestionnaireSignupDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireSignupDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int QuestionnaireSignupID = rDAO.Insert(vo);
                    return QuestionnaireSignupID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新签到
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardQuestionnaireSignup(CardQuestionnaireSignupVO vo)
        {
            ICardQuestionnaireSignupDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireSignupDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 删除签到
        /// </summary>
        /// <param name="QuestionnaireSignupID"></param>
        /// <returns></returns>
        public int DeleteByQuestionnaireSignupID(int QuestionnaireSignupID)
        {
            ICardQuestionnaireSignupDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireSignupDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("QuestionnaireSignupID = " + QuestionnaireSignupID+ " and AppType="+ AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取签到数量
        /// </summary>
        /// <returns></returns>
        public int FindCardQuestionnaireSignup(string condition)
        {
            ICardQuestionnaireSignupDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireSignupDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition + " and AppType=" + AppType);
        }

        /// <summary>
        /// 获取签到详情
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public CardQuestionnaireSignupVO FindCardQuestionnaireSignupByQuestionnaireSignupID(int QuestionnaireSignupID)
        {
            ICardQuestionnaireSignupDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireSignupDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(QuestionnaireSignupID);
        }

        /// <summary>
        /// 获取签到列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<CardQuestionnaireSignupVO> FindCardQuestionnaireSignupByCondtion(string condtion)
        {
            ICardQuestionnaireSignupDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireSignupDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion + " and AppType=" + AppType);
        }

        /// <summary>
        /// 获取签到列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardQuestionnaireSignupVO> FindCardQuestionnaireSignupAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardQuestionnaireSignupDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireSignupDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 添加软文
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddSoftArticle(CardSoftArticleVO vo)
        {
            try
            {
                ICardSoftArticleDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int NewsID = uDAO.Insert(vo);
                    return NewsID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新软文
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateSoftArticle(CardSoftArticleVO vo)
        {
            ICardSoftArticleDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取软文列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardSoftArticleVO> FindSoftArticleAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardSoftArticleDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr+ " and AppType="+ AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取软文数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindSoftArticleTotalCount(string condition, params object[] parameters)
        {
            ICardSoftArticleDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取软文列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardSoftArticleVO> FindSoftArticleAllToNotType(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardSoftArticleDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取软文数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindSoftArticleTotalCountToNotType(string condition, params object[] parameters)
        {
            ICardSoftArticleDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 获取软文列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardSoftArticleVO> FindSoftArticleByConditionStr(string condition)
        {
            ICardSoftArticleDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleDAO(this.CurrentCustomerProfile);
            List<CardSoftArticleVO> cVO = uDAO.FindByParams("AppType=" + AppType+" and "+ condition);
            return cVO;
        }

        /// <summary>
        /// 获取软文详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public CardSoftArticleVO FindSoftArticleById(Int64 SoftArticleID)
        {
            ICardSoftArticleDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(SoftArticleID);
        }

        /// <summary>
        /// 更新软文头条推荐活动
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public int SetCompanyPartyCard(int PartyID)
        {
            ICardSoftArticleDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleDAO(this.CurrentCustomerProfile);
            return uDAO.Update("PartyID="+ PartyID,"Status=2 and CustomerId=14");
        }


        /// <summary>
        /// 添加软文订单
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddSoftArticleOrder(CardSoftArticleOrderVO vo)
        {
            try
            {
                ICardSoftArticleOrderDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleOrderDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int NewsID = uDAO.Insert(vo);
                    return NewsID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新软文订单
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardSoftArticleOrder(CardSoftArticleOrderVO vo)
        {
            ICardSoftArticleOrderDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleOrderDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取软文订单列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardSoftArticleOrderVO> FindSoftArticleOrderAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardSoftArticleOrderDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleOrderDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr+ " and AppType="+ AppType, start, end, sortcolname, asc, parameters);
        }
        /// <summary>
        /// 获取软文订单列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardSoftArticleOrderVO> FindSoftArticleOrderByConditionStr(string condition,bool isAppType=true)
        {
            ICardSoftArticleOrderDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleOrderDAO(this.CurrentCustomerProfile);
            if (isAppType)
            {
                condition = condition + " and AppType=" + AppType;
            }
            List<CardSoftArticleOrderVO> cVO = uDAO.FindByParams(condition);
            return cVO;
        }

        /// <summary>
        /// 获取软文订单金额总数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public decimal FindSoftArticleOrderSumCost(string condition, params object[] parameters)
        {
            ICardSoftArticleOrderDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleOrderDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalSum("Cost", condition + " and AppType=" + AppType);

        }

        /// <summary>
        /// 获取软文订单视图列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardSoftArticleOrderViewVO> FindSoftArticleOrderViewByConditionStr(string condition)
        {
            ICardSoftArticleOrderViewDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleOrderViewDAO(this.CurrentCustomerProfile);
            List<CardSoftArticleOrderViewVO> cVO = uDAO.FindByParams(condition + " and AppType=" + AppType);
            return cVO;
        }

        /// <summary>
        /// 获取软文订单数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindSoftArticleOrderTotalCount(string condition, params object[] parameters)
        {
            ICardSoftArticleOrderDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleOrderDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取软文订单详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public CardSoftArticleOrderVO FindSoftArticleOrderById(int SoftArticleOrderID)
        {
            ICardSoftArticleOrderDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleOrderDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(SoftArticleOrderID);
        }

        public decimal FindSoftArticleOrderSumByCondtion(string Sum, string condtion)
        {
            ICardSoftArticleOrderViewDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleOrderViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalSum(Sum, condtion);
        }

        /// <summary>
        /// 添加软文投诉
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCardSoftArticleComplaint(CardSoftArticleComplaintVO vo)
        {
            try
            {
                ICardSoftArticleComplaintDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleComplaintDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int NewsID = uDAO.Insert(vo);
                    return NewsID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新软文投诉
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardSoftArticleComplaint(CardSoftArticleComplaintVO vo)
        {
            ICardSoftArticleComplaintDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleComplaintDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取软文投诉列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardSoftArticleComplaintVO> FindSoftArticleComplaintAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardSoftArticleComplaintDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleComplaintDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr+ " and AppType="+ AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取软文投诉数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindSoftArticleComplaintTotalCount(string condition, params object[] parameters)
        {
            ICardSoftArticleComplaintDAO uDAO = CustomerManagementDAOFactory.CardSoftArticleComplaintDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 添加帮助
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddHelp(CardHelpVO vo)
        {
            try
            {
                ICardHelpDAO uDAO = CustomerManagementDAOFactory.CardHelpDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int HelpID = uDAO.Insert(vo);
                    return HelpID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新帮助
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateHelp(CardHelpVO vo)
        {
            ICardHelpDAO uDAO = CustomerManagementDAOFactory.CardHelpDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 删除帮助
        /// </summary>
        /// <param name="HelpID"></param>
        /// <returns></returns>
        public int DeleteHelpById(int HelpID)
        {
            ICardHelpDAO uDAO = CustomerManagementDAOFactory.CardHelpDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("HelpID = " + HelpID+ " and AppType="+ AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取帮助列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardHelpVO> FindCardHelpAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardHelpDAO uDAO = CustomerManagementDAOFactory.CardHelpDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取帮助列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardHelpVO> FindCardHelpByConditionStr(string condition)
        {
            ICardHelpDAO uDAO = CustomerManagementDAOFactory.CardHelpDAO(this.CurrentCustomerProfile);
            List<CardHelpVO> cVO = uDAO.FindByParams(" AppType=" + AppType+ " and " + condition);
            return cVO;
        }

        /// <summary>
        /// 获取帮助数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardHelpTotalCount(string condition, params object[] parameters)
        {
            ICardHelpDAO uDAO = CustomerManagementDAOFactory.CardHelpDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取帮助详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public CardHelpVO FindCardHelpById(int HelpID)
        {
            ICardHelpDAO uDAO = CustomerManagementDAOFactory.CardHelpDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(HelpID);
        }

        /// <summary>
        /// 获取月度排行榜
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardRankingViewVO> GetRanking()
        {
            ICardRankingViewDAO uDAO = CustomerManagementDAOFactory.CardRankingViewDAO(this.CurrentCustomerProfile);
            List<CardRankingViewVO> cVO = uDAO.FindAllByPageIndex("AppType=" + AppType,10);
            return cVO;
        }

        /// <summary>
        /// 获取优惠价格
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public CardDiscountCodeVO GetDiscountCode(string Code)
        {
            ICardDiscountCodeDAO uDAO = CustomerManagementDAOFactory.CardDiscountCodeDAO(this.CurrentCustomerProfile);
            List<CardDiscountCodeVO> cVO = uDAO.FindByParams("(Code='"+ Code + "') and DATE_FORMAT(ExpirationAt,'%y-%m-%d')>=DATE_FORMAT(now(),'%y-%m-%d') and AppType=" + AppType);

            if (cVO.Count > 0)
            {
                if(cVO[cVO.Count - 1].Cost == 0)
                {
                    return null;
                }
                return cVO[cVO.Count-1];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取优惠码列表
        /// </summary>
        public List<CardDiscountCodeVO> FindAllByPageIndexByDiscountCode(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardDiscountCodeDAO uDAO = CustomerManagementDAOFactory.CardDiscountCodeDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取优惠码数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindDiscountCodeTotalCount(string condition, params object[] parameters)
        {
            ICardDiscountCodeDAO uDAO = CustomerManagementDAOFactory.CardDiscountCodeDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取优惠码详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public CardDiscountCodeVO FindCardDiscountCodeById(int DiscountCodeID)
        {
            ICardDiscountCodeDAO uDAO = CustomerManagementDAOFactory.CardDiscountCodeDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(DiscountCodeID);
        }

        /// <summary>
        /// 添加优惠码
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddDiscountCode(CardDiscountCodeVO vo)
        {
            try
            {
                ICardDiscountCodeDAO uDAO = CustomerManagementDAOFactory.CardDiscountCodeDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int NewsID = uDAO.Insert(vo);
                    return NewsID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新优惠码
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateDiscountCode(CardDiscountCodeVO vo)
        {
            ICardDiscountCodeDAO uDAO = CustomerManagementDAOFactory.CardDiscountCodeDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 删除优惠码
        /// </summary>
        /// <param name="NewsID"></param>
        /// <returns></returns>
        public int DeleteDiscountCodeById(int DiscountCodeID)
        {
            ICardDiscountCodeDAO uDAO = CustomerManagementDAOFactory.CardDiscountCodeDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("DiscountCodeID = " + DiscountCodeID + " and AppType=" + AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }


        /// <summary>
        /// 获取代理商列表
        /// </summary>
        public List<CardAgentVO> FindAllByPageIndexByCardAgent(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardAgentDAO uDAO = CustomerManagementDAOFactory.CardAgentDAO(this.CurrentCustomerProfile);
            List<CardAgentVO> ca= uDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
            return ca;
        }

        /// <summary>
        /// 获取代理商会员ID
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public int GetAgentCustomerIdByCity(string City)
        {
            ICardAgentViewDAO uDAO = CustomerManagementDAOFactory.CardAgentViewDAO(this.CurrentCustomerProfile);
            List<CardAgentViewVO> cVO = uDAO.FindByParams("CityName like '" + City + "%' and AppType=" + AppType);
            int CustomerId = 0;
            if (cVO.Count > 0)
            {
                CustomerId = cVO[0].CustomerId;
            }
            return CustomerId;
        }

        /// <summary>
        /// 获取代理商数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardAgentTotalCount(string condition, params object[] parameters)
        {
            ICardAgentDAO uDAO = CustomerManagementDAOFactory.CardAgentDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取代理商详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public CardAgentVO FindCardAgentById(int CardAgentID)
        {
            ICardAgentDAO uDAO = CustomerManagementDAOFactory.CardAgentDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(CardAgentID);
        }

        /// <summary>
        /// 获取代理商列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardAgentVO> GetCardAgentByCustomerId(int CustomerId)
        {
            ICardAgentDAO uDAO = CustomerManagementDAOFactory.CardAgentDAO(this.CurrentCustomerProfile);
            List<CardAgentVO> cVO = uDAO.FindByParams("CustomerId=" + CustomerId + " and AppType=" + AppType);
            return cVO;
        }

        /// <summary>
        /// 获取代理商附加信息
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public CardAgentVO FindCardAgentByVO(CardAgentVO vo)
        {
            try
            {
                if (vo.CustomerId > 0)
                {
                    List<CardDataVO> CardDataVO = FindCardByCustomerId(vo.CustomerId);
                    if (CardDataVO.Count > 0)
                    {
                        vo.Headimg = CardDataVO[0].Headimg;
                        vo.Name = CardDataVO[0].Name;
                    }
                    vo.DepositCost = FindAgentDepositSumCost("CustomerId=" + vo.CustomerId);

                    //总佣金
                    decimal OneRebateCost = FindOrderSumByCondtion("OneRebateCost", "OneRebateAgentCustomerId = " + vo.CustomerId + " and Status=1 and payAt is not NULL");
                    decimal TwoRebateCost = FindOrderSumByCondtion("TwoRebateCost", "TwoRebateAgentCustomerId = " + vo.CustomerId + " and Status=1 and payAt is not NULL");
                    vo.AgentCost = FindOrderSumByCondtion("AgentCost", "AgentCustomerId = " + vo.CustomerId + " and Status=1 and payAt is not NULL");
                    vo.TotalCommission = OneRebateCost + TwoRebateCost + vo.AgentCost;
                    vo.TotalCost = FindOrderSumByCondtion("Cost", "AgentCustomerId = " + vo.CustomerId + " and Status=1 and payAt is not NULL");

                    //应付佣金
                    decimal OneRebateCost2 = FindOrderSumByCondtion("OneRebateCost", "OneRebateAgentCustomerId = " + vo.CustomerId + " and Status=1 and payAt is not NULL and OneRebateStatus=0");
                    decimal TwoRebateCost2 = FindOrderSumByCondtion("TwoRebateCost", "TwoRebateAgentCustomerId = " + vo.CustomerId + " and Status=1 and payAt is not NULL and TwoRebateStatus=0");
                    vo.PayableCommission = OneRebateCost2 + TwoRebateCost2;

                    //已付佣金
                    vo.PaidCommission = OneRebateCost + TwoRebateCost - vo.PayableCommission;

                    //已结算佣金
                    vo.SettlementCost = FindFinanceSumByCondtion("SettlementCost", "CustomerId = " + vo.CustomerId + " and isSettlement=1");
                }

                CityBO sBO = new CityBO(new UserProfile());
                if (vo.CityId > 0)
                {
                    CityVO cVO = sBO.FindCityById(vo.CityId);
                    vo.City = cVO.CityName;
                    vo.Province = sBO.FindProvinceById(cVO.ProvinceId).ProvinceName;
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
            }
            return vo;
        }

        /// <summary>
        /// 获取代理商附加信息
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public int getCardAgentByUserCount(string City,string condtion)
        {
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());
            City = City.Replace("市","");
            City = City.Replace("自治县", "");
            City = City.Replace("自治州", "");
            City = City.Replace("盟", "");
            City = City.Replace("地区", "");
            City = City.Replace("县", "");
            return CustomerBO.GetCustomerCount("City LIKE '" + City + "%' and AppType="+ AppType+" and "+ condtion);
        }

        /// <summary>
        /// 代理商结算
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool setAgentFinance(int CustomerId)
        {
            try
            {
                List<CardOrderMonthVO> cVO = FindCardOrderMonthByConditionStr("(AgentCustomerId=" + CustomerId+ " or OneRebateAgentCustomerId="+CustomerId+ " or TwoRebateAgentCustomerId="+ CustomerId+")");

                foreach(CardOrderMonthVO Month in cVO)
                {
                    List<CardAgentFinanceVO> fVO = FindCardAgentFinanceByConditionStr("CustomerId=" + CustomerId + " and MONTH='"+ Month.MONTH + "'");
                    if (fVO.Count == 0&& Month.MONTH != DateTime.Now.ToString("yyyy-MM"))
                    {
                        CardAgentFinanceVO FinanceVO = new CardAgentFinanceVO();
                        FinanceVO.MONTH = Month.MONTH;
                        FinanceVO.CustomerId = CustomerId;

                        //总佣金
                        decimal OneRebateCost = FindOrderSumByCondtion("OneRebateCost", "OneRebateAgentCustomerId = " + FinanceVO.CustomerId + " and Status=1 and date_format(payAt, '%Y-%m')='"+ FinanceVO.MONTH + "'");
                        decimal TwoRebateCost = FindOrderSumByCondtion("TwoRebateCost", "TwoRebateAgentCustomerId = " + FinanceVO.CustomerId + " and Status=1 and date_format(payAt, '%Y-%m')='" + FinanceVO.MONTH + "'");
                        FinanceVO.AgentCost = FindOrderSumByCondtion("AgentCost", "AgentCustomerId = " + FinanceVO.CustomerId + " and Status=1 and payAt is not NULL");
                        FinanceVO.TotalCommission = OneRebateCost + TwoRebateCost + FinanceVO.AgentCost;

                        //应付佣金
                        decimal OneRebateCost2 = FindOrderSumByCondtion("OneRebateCost", "OneRebateAgentCustomerId = " + FinanceVO.CustomerId + " and Status=1  and date_format(payAt, '%Y-%m')='" + FinanceVO.MONTH + "' and OneRebateStatus=0");
                        decimal TwoRebateCost2 = FindOrderSumByCondtion("TwoRebateCost", "TwoRebateAgentCustomerId = " + FinanceVO.CustomerId + " and Status=1  and date_format(payAt, '%Y-%m')='" + FinanceVO.MONTH + "' and TwoRebateStatus=0");
                        FinanceVO.PayableCommission = OneRebateCost2 + TwoRebateCost2;

                        //已付佣金
                        FinanceVO.PaidCommission = OneRebateCost + TwoRebateCost - FinanceVO.PayableCommission;
                        AddCardAgentFinance(FinanceVO);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取vip订单代理商月度列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardOrderMonthVO> FindCardOrderMonthByConditionStr(string condition)
        {
            ICardOrderMonthDAO uDAO = CustomerManagementDAOFactory.CardOrderMonthDAO(this.CurrentCustomerProfile);
            List<CardOrderMonthVO> cVO = uDAO.FindByParams(condition + " and AppType=" + AppType);
            return cVO;
        }

        /// <summary>
        /// 获取代理商结算清单列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardAgentFinanceVO> FindCardAgentFinanceByConditionStr(string condition)
        {
            ICardAgentFinanceDAO uDAO = CustomerManagementDAOFactory.CardAgentFinanceDAO(this.CurrentCustomerProfile);
            List<CardAgentFinanceVO> cVO = uDAO.FindByParams(condition + " and AppType=" + AppType);
            return cVO;
        }

        /// <summary>
        /// 添加代理商结算清单
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCardAgentFinance(CardAgentFinanceVO vo)
        {
            try
            {
                ICardAgentFinanceDAO uDAO = CustomerManagementDAOFactory.CardAgentFinanceDAO(this.CurrentCustomerProfile);
                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int NewsID = uDAO.Insert(vo);
                    return NewsID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新代理商结算清单
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardAgentFinance(CardAgentFinanceVO vo)
        {
            ICardAgentFinanceDAO uDAO = CustomerManagementDAOFactory.CardAgentFinanceDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取代理商结算清单列表
        /// </summary>
        public List<CardAgentFinanceVO> FindAllByPageIndexByCardAgentFinance(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardAgentFinanceDAO uDAO = CustomerManagementDAOFactory.CardAgentFinanceDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取代理商结算清单数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardAgentFinanceTotalCount(string condition, params object[] parameters)
        {
            ICardAgentFinanceDAO uDAO = CustomerManagementDAOFactory.CardAgentFinanceDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        public decimal FindFinanceSumByCondtion(string Sum, string condtion)
        {
            ICardAgentFinanceDAO uDAO = CustomerManagementDAOFactory.CardAgentFinanceDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalSum(Sum, condtion + " and AppType=" + AppType);
        }

        /// <summary>
        /// 添加代理商
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCardAgent(CardAgentVO vo)
        {
            try
            {
                ICardAgentDAO uDAO = CustomerManagementDAOFactory.CardAgentDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int NewsID = uDAO.Insert(vo);
                    return NewsID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新代理商
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardAgent(CardAgentVO vo)
        {
            ICardAgentDAO uDAO = CustomerManagementDAOFactory.CardAgentDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 删除代理商
        /// </summary>
        /// <param name="NewsID"></param>
        /// <returns></returns>
        public int DeleteCardAgentById(int CardAgentID)
        {
            ICardAgentDAO uDAO = CustomerManagementDAOFactory.CardAgentDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("CardAgentID = " + CardAgentID + " and AppType=" + AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 代理商充值
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddAgentDeposit(decimal Cost,int CustomerId,string Type,int PayOutCustomerId=0)
        {
            CardAgentDepositVO vo = new CardAgentDepositVO();
            vo.Cost = Cost;
            vo.CustomerId = CustomerId;
            Random ran = new Random();
            vo.OrderNO = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);
            vo.Type = Type;
            vo.PayOutCustomerId = PayOutCustomerId;
            try
            {
                ICardAgentDepositDAO uDAO = CustomerManagementDAOFactory.CardAgentDepositDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int NewsID = uDAO.Insert(vo);
                    return NewsID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 获取代理商保证金金额总数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public decimal FindAgentDepositSumCost(string condition, params object[] parameters)
        {
            ICardAgentDepositDAO uDAO = CustomerManagementDAOFactory.CardAgentDepositDAO(this.CurrentCustomerProfile);
            return uDAO.FindSumCost(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取代理商保证金列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardAgentDepositVO> GetCardAgentDepositByCustomerId(int CustomerId)
        {
            ICardAgentDepositDAO uDAO = CustomerManagementDAOFactory.CardAgentDepositDAO(this.CurrentCustomerProfile);
            List<CardAgentDepositVO> cVO = uDAO.FindByParams("CustomerId=" + CustomerId + " and AppType=" + AppType);
            return cVO;
        }

        /// <summary>
        /// 添加代理商申请
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddAgentApply(CardAgentApplyVO vo)
        {
            try
            {
                ICardAgentApplyDAO rDAO = CustomerManagementDAOFactory.CardAgentApplyDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int AgentApplyID = rDAO.Insert(vo);
                    return AgentApplyID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新代理商申请
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateAgentApply(CardAgentApplyVO vo)
        {
            ICardAgentApplyDAO rDAO = CustomerManagementDAOFactory.CardAgentApplyDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取代理商申请列表
        /// </summary>
        public List<CardAgentApplyVO> FindAllByPageIndexByCardAgentApply(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardAgentApplyDAO rDAO = CustomerManagementDAOFactory.CardAgentApplyDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取代理商申请数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardAgentApplyTotalCount(string condition, params object[] parameters)
        {
            ICardAgentApplyDAO rDAO = CustomerManagementDAOFactory.CardAgentApplyDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 添加VIP申请
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddVipApply(CardVipApplyVO vo)
        {
            try
            {
                ICardVipApplyDAO rDAO = CustomerManagementDAOFactory.CardVipApplyDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int VipApplyID = rDAO.Insert(vo);
                    return VipApplyID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新VIP申请
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateVipApply(CardVipApplyVO vo)
        {
            ICardVipApplyDAO rDAO = CustomerManagementDAOFactory.CardVipApplyDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取VIP申请列表
        /// </summary>
        public List<CardVipApplyVO> FindAllByPageIndexByCardVipApply(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardVipApplyDAO rDAO = CustomerManagementDAOFactory.CardVipApplyDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取VIP申请数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardVipApplyTotalCount(string condition, params object[] parameters)
        {
            ICardVipApplyDAO rDAO = CustomerManagementDAOFactory.CardVipApplyDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 获取乐聊名片所有手机号
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardByPhoneVO> FindCardByPhone(string condition, params object[] parameters)
        {
            ICardByPhoneDAO uDAO = CustomerManagementDAOFactory.CardByPhoneDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams(condition);
        }

        /// <summary>
        /// 添加VIP兑换码
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddExchangeCode(CardExchangeCodeVO vo)
        {
            try
            {
                ICardExchangeCodeDAO rDAO = CustomerManagementDAOFactory.CardExchangeCodeDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int ExchangeCodeID = rDAO.Insert(vo);
                    return ExchangeCodeID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新VIP兑换码
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateExchangeCode(CardExchangeCodeVO vo)
        {
            ICardExchangeCodeDAO rDAO = CustomerManagementDAOFactory.CardExchangeCodeDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取VIP兑换码列表
        /// </summary>
        public List<CardExchangeCodeVO> FindAllByPageIndexByExchangeCode(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardExchangeCodeDAO rDAO = CustomerManagementDAOFactory.CardExchangeCodeDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取VIP兑换码数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindExchangeCodeTotalCount(string condition, params object[] parameters)
        {
            ICardExchangeCodeDAO rDAO = CustomerManagementDAOFactory.CardExchangeCodeDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 匹配兑换码
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CardExchangeCodeVO> GetCardExchangeCodeByCode(string Code)
        {
            ICardExchangeCodeDAO rDAO = CustomerManagementDAOFactory.CardExchangeCodeDAO(this.CurrentCustomerProfile);
            List<CardExchangeCodeVO> cVO = rDAO.FindByParams("Code='" + Code + "' and AppType=" + AppType);
            return cVO;
        }

        /// <summary>
        /// 获取代理商已兑换的7天试用会员且未续费的
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public void GetCardExchangeCodeByCustomerId(int customerId)
        {
            ICardExchangeCodeDAO rDAO = CustomerManagementDAOFactory.CardExchangeCodeDAO(this.CurrentCustomerProfile);
            List<CardExchangeCodeVO> CodeVO = rDAO.FindByParams("CustomerId = " + customerId + " and Status=1 and Type=1 and UsedAt < DATE_SUB(NOW(),INTERVAL 15 DAY)");
            List<CardExchangeCodeVO> newCodeVO = new List<CardExchangeCodeVO>();
            CustomerBO CustomerBO = new CustomerBO(new CustomerProfile());

            for (int i=0;i< CodeVO.Count; i++)
            {
                
                CustomerVO CustomerVO = CustomerBO.FindCustomenById(CodeVO[i].ToCustomerId);
                if (CustomerVO != null)
                {
                    if (CustomerVO.ExpirationAt < DateTime.Now && CodeVO[i].ExpirationAt> DateTime.Now)
                    {
                        CodeVO[i].ToCustomerId = 0;
                        CodeVO[i].ToCustomerName = "";
                        CodeVO[i].ToHeaderLogo = "";
                        CodeVO[i].Status = 0;
                        UpdateExchangeCode(CodeVO[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 生成数字和字母随机兑换码
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns>返回指定长度的数字和字母的随机串</returns>
        public string RndCode(int Length=10)
        {
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random randrom = new Random((int)DateTime.Now.Ticks);

            string Code = "";
            for (int i = 0; i < Length; i++)
            {
                Code += chars[randrom.Next(chars.Length)];
            }
            
            List<CardExchangeCodeVO> cvo = GetCardExchangeCodeByCode(Code);

            if (cvo.Count > 0)
            {
                return "";
            }

            return Code;
        }


        /// <summary>
        /// 添加签到表管理员
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddQuestionnaireAdmin(CardQuestionnaireAdminVO vo)
        {
            try
            {
                ICardQuestionnaireAdminDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireAdminDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int QuestionnaireAdminID = rDAO.Insert(vo);
                    return QuestionnaireAdminID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新签到表管理员
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateQuestionnaireAdmin(CardQuestionnaireAdminVO vo)
        {
            ICardQuestionnaireAdminDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireAdminDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 获取签到表管理员列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardQuestionnaireAdminVO> FindQuestionnaireAdminByCondition(string condition, params object[] parameters)
        {
            ICardQuestionnaireAdminDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireAdminDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condition + " and AppType=" + AppType);
        }

        /// <summary>
        /// 获取签到表管理员数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindQuestionnaireAdminTotalCount(string condition, params object[] parameters)
        {
            ICardQuestionnaireAdminDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireAdminDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }
        /// <summary>
        /// 获取签到表管理员详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public CardQuestionnaireAdminVO FindQuestionnaireAdminById(int QuestionnaireAdminID)
        {
            ICardQuestionnaireAdminDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireAdminDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(QuestionnaireAdminID);
        }

        /// <summary>
        /// 是否是签到表管理员
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool isQuestionnaireAdmin(int QuestionnaireID,Int64 CustomerId)
        {
            ICardQuestionnaireAdminDAO rDAO = CustomerManagementDAOFactory.CardQuestionnaireAdminDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount("QuestionnaireID=" + QuestionnaireID + " and CustomerId="+ CustomerId + " and AppType=" + AppType)>0;
        }

        /// <summary>
        /// 删除签到表管理员
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public int DeleteQuestionnaireAdminById(int QuestionnaireID)
        {
            ICardNoticeDAO rDAO = CustomerManagementDAOFactory.CardNoticeDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("QuestionnaireID = " + QuestionnaireID + " and AppType=" + AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 添加自定义海报背景
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCardPoter(CardPoterVO vo)
        {
            try
            {
                ICardPoterDAO rDAO = CustomerManagementDAOFactory.CardPoterDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int CardPoterID = rDAO.Insert(vo);
                    return CardPoterID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新自定义海报背景
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardPoter(CardPoterVO vo)
        {
            ICardPoterDAO rDAO = CustomerManagementDAOFactory.CardPoterDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 获取自定义海报背景列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardPoterVO> FindCardPoterByCondition(string condition, params object[] parameters)
        {
            ICardPoterDAO rDAO = CustomerManagementDAOFactory.CardPoterDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams("AppType=" + AppType+" and " + condition);
        }

        /// <summary>
        /// 获取自定义海报背景数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardPoterTotalCount(string condition, params object[] parameters)
        {
            ICardPoterDAO rDAO = CustomerManagementDAOFactory.CardPoterDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }
        /// <summary>
        /// 获取自定义海报背景详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public CardPoterVO FindCardPoterById(int CardPoterID)
        {
            ICardPoterDAO rDAO = CustomerManagementDAOFactory.CardPoterDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(CardPoterID);
        }

        /// <summary>
        /// 删除自定义海报背景
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public int DeleteCardPoterAdminById(int CardPoterID)
        {
            ICardPoterDAO rDAO = CustomerManagementDAOFactory.CardPoterDAO(this.CurrentCustomerProfile);

            try
            {//删除旧图片
                CardPoterVO caVO = FindCardPoterById(CardPoterID);

                if (caVO == null)
                {
                    return -1;
                }

                if (caVO.Url != "")
                {
                    string FilePath = caVO.Url;
                    FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }

            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
            }

            try
            {
                rDAO.DeleteByParams("CardPoterID = " + CardPoterID + " and AppType=" + AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取自定义海报列表
        /// </summary>
        public List<CardPoterVO> FindAllByPageIndexByCardPoter(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardPoterDAO rDAO = CustomerManagementDAOFactory.CardPoterDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取会员佣金统计列表
        /// </summary>
        public List<CardRebateViewVO> FindAllByPageIndexByCardRebate(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardRebateViewDAO pDAO = CustomerManagementDAOFactory.CardRebateViewDAO(this.CurrentCustomerProfile);
            return pDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取会员佣金统计数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindCardRebateTotalCount(string condition, params object[] parameters)
        {
            ICardRebateViewDAO pDAO = CustomerManagementDAOFactory.CardRebateViewDAO(this.CurrentCustomerProfile);
            return pDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }

        /// <summary>
        /// 抽奖活动开奖
        /// </summary>
        /// <returns></returns>
        public bool DrawAPrize(int PartyID)
        {
            CardPartyVO PartyVO = FindPartyById(PartyID);
            if (PartyVO == null) return false;
            if (PartyVO.Type!=3|| PartyVO.PartyLuckDrawStatus > 0) return false;

            string Host = "";
            if (PartyVO.Host != "")
            {
                Host = PartyVO.Host;
            }
            else
            {
                List<CardDataVO> dVO = FindCardByCustomerId(PartyVO.CustomerId);
                if (dVO.Count > 0)
                {
                    Host = dVO[0].Name;
                }
                else
                {
                    Host = "活动星选平台";
                }
            }

            PartyVO.PartyLuckDrawStatus = 1;
            PartyVO.LuckDrawAt = DateTime.Now;
            UpdateParty(PartyVO);
            //是否重复
            if (PartyVO.isRepeat == 1)
            {
                //暂停创建抽奖活动
               // CreateDailyLottery(PartyVO.PartyID);
            }

            //获取奖品列表
            List<CardPartyCostVO> CostVO = FindCostByPartyID(PartyID);
            //获取未开奖人员列表
            List<CardPartySignUpVO> SignUpVO = FindSignUpByCondtion("PartyID=" + PartyID + " and LuckDrawStatus=0 and isAutoAdd=0");

            //副奖
            List<PrizeVO> PrizeListVO = new List<PrizeVO>();
            //头奖
            List<PrizeVO> FirstPrizeListVO = new List<PrizeVO>();
            foreach (CardPartyCostVO item in CostVO)
            {
                for(int i=0;i< item.limitPeopleNum; i++)
                {
                    PrizeVO pVO = new PrizeVO();
                    pVO.Name = item.Names;
                    pVO.Content = item.Content;
                    pVO.Cost = item.Cost;
                    pVO.isAutoPay = item.isAutoPay;

                    if (item.isFirstPrize == 1)
                    {
                        FirstPrizeListVO.Add(pVO);
                    }
                    else
                    {
                        PrizeListVO.Add(pVO);
                    }
                }
            }

            //假开奖
            if (PartyVO.isNoDraw == 1)
            {
                List<CardDataVO> cVO = FindCardByCondition("ReadCount=0 and Collection=0 and Headimg<>'' and Forward=0 and CreatedAt<date_add(curdate(),interval -6 MONTH) and Headimg<>'' and Headimg<>'https://www.zhongxiaole.net/SPManager/Style/images/wxcard/noheadimg.jpg' and Phone<>''", 3000);
                cVO = cVO.OrderBy(f => Guid.NewGuid()).ToList();

                for (int i = 0; i < FirstPrizeListVO.Count; i++)
                {
                    CardPartySignUpVO suVO = new CardPartySignUpVO();

                    suVO.CardID = cVO[0].CardID;
                    suVO.CustomerId = cVO[0].CustomerId;
                    suVO.Name = cVO[0].Name;
                    suVO.Headimg = cVO[0].Headimg;
                    suVO.Phone = cVO[0].Phone;
                    suVO.PartyID = PartyID;
                    suVO.CreatedAt = RandomTime(PartyVO.CreatedAt, DateTime.Now);
                    suVO.SignUpForm = "<SignUpForm><Name>姓名</Name><Value>" + cVO[i].Name + "</Value></SignUpForm><SignUpForm><Name>手机</Name><Value>" + cVO[i].Phone + "</Value></SignUpForm>";
                    suVO.InviterCID = 0;
                    suVO.isAutoAdd = 1;
                    suVO.Number = 1;
                    suVO.LuckDrawStatus = 1;
                    suVO.LuckDrawNames = FirstPrizeListVO[i].Name;
                    suVO.LuckDrawContent = FirstPrizeListVO[i].Content;

                    int PartySignUpID = AddCardToParty(suVO);
                    cVO.RemoveAt(0);
                }

                for (int i = 0; i < PrizeListVO.Count; i++)
                {
                    CardPartySignUpVO suVO = new CardPartySignUpVO();

                    suVO.CardID = cVO[0].CardID;
                    suVO.CustomerId = cVO[0].CustomerId;
                    suVO.Name = cVO[0].Name;
                    suVO.Headimg = cVO[0].Headimg;
                    suVO.Phone = cVO[0].Phone;
                    suVO.PartyID = PartyID;
                    suVO.CreatedAt = RandomTime(PartyVO.CreatedAt, DateTime.Now);
                    suVO.SignUpForm = "<SignUpForm><Name>姓名</Name><Value>" + cVO[i].Name + "</Value></SignUpForm><SignUpForm><Name>手机</Name><Value>" + cVO[i].Phone + "</Value></SignUpForm>";
                    suVO.InviterCID = 0;
                    suVO.isAutoAdd = 1;
                    suVO.Number = 1;
                    suVO.LuckDrawStatus = 1;
                    suVO.LuckDrawNames = PrizeListVO[i].Name;
                    suVO.LuckDrawContent = PrizeListVO[i].Content;

                    int PartySignUpID = AddCardToParty(suVO);
                    cVO.RemoveAt(0);
                }
                foreach (CardPartySignUpVO item in SignUpVO)
                {
                    int index = SignUpVO.IndexOf(item);
                    //是否为现金抽奖
                    if (PartyVO.isCashLuckDraw == 1&& index <= 50)
                    {
                        try
                        {
                            item.LuckDrawStatus = 1;
                            item.LuckDrawNames = "参与奖";
                            item.LuckDrawContent = "瓜分现金红包";
                        
                            decimal Cost = 0.33M;
                            Random Rdm = new Random();
                            decimal iRdm = Rdm.Next(1, 30);
                            Cost = iRdm / 100;
                            string retu = PayforWXUserCash(Cost, item.Name, item.OpenId, item.CustomerId, 4, "活动星选中奖奖金");
                            if (retu == "SUCCESS")
                            {
                                item.LuckDrawPayStatus = 1;
                            }
                       
                            UpdateSignUp(item);
                            sendWinningMessage(item.OpenId, item.PartyID, PartyVO.Title, Host);
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        item.LuckDrawStatus = 2;
                        UpdateSignUp(item);
                        if (item.OpenId != "")
                        {
                            sendWinningMessage(item.OpenId, item.PartyID, PartyVO.Title, Host);
                        }
                    }
                    
                }

                try
                {
                    LuckDrawSignUpIsNot(PartyVO.PartyID);
                }
                catch
                {

                }

                return true;
            }

            //头奖开奖
            List<CardPartySignUpVO> ItemInviterVO = new List<CardPartySignUpVO>();
            foreach (CardPartySignUpVO item in SignUpVO)
            {
                foreach (CardPartySignUpVO j in SignUpVO)
                {
                    if (j.InviterCID == item.CustomerId && j.CustomerId != item.CustomerId)
                    {
                        ItemInviterVO.Add(item);
                    }
                }
                ItemInviterVO.Add(item);
            }
            ItemInviterVO = RandomSortList(ItemInviterVO);
            ItemInviterVO = RandomSortList(ItemInviterVO);
            FirstPrizeListVO = RandomSortList(FirstPrizeListVO);
            FirstPrizeListVO = RandomSortList(FirstPrizeListVO);

            List<CardPartySignUpVO> LuckDrawOrder = new List<CardPartySignUpVO>();
            foreach (CardPartySignUpVO item in ItemInviterVO)
            {
                if (item.LuckDrawOrder == 1)
                {
                    LuckDrawOrder.Add(item);
                }
            }

            if (LuckDrawOrder.Count > 0)
            {
                ItemInviterVO = LuckDrawOrder;
            }

            foreach (CardPartySignUpVO item in ItemInviterVO)
            {
                if (FirstPrizeListVO.Count > 0 && item.LuckDrawStatus == 0)
                {
                    item.LuckDrawStatus = 1;
                    item.LuckDrawNames = FirstPrizeListVO[0].Name;
                    item.LuckDrawContent = FirstPrizeListVO[0].Content;
                    try
                    {
                        if (FirstPrizeListVO[0].isAutoPay == 1 && FirstPrizeListVO[0].Cost > 0)
                        {
                            decimal Cost = FirstPrizeListVO[0].Cost;
                            if (Cost == 0.33M)
                            {
                                Random Rdm = new Random();
                                decimal iRdm = Rdm.Next(1, 30);
                                Cost = iRdm / 100;
                            }
                            string retu = PayforWXUserCash(Cost, item.Name, item.OpenId, item.CustomerId, 4, "活动星选中奖奖金");
                            if (retu == "SUCCESS")
                            {
                                item.LuckDrawPayStatus = 1;
                            }
                        }
                    }
                    catch
                    {

                    }
                    FirstPrizeListVO.RemoveAt(0);
                    UpdateSignUp(item);
                    sendWinningMessage(item.OpenId, item.PartyID, PartyVO.Title, Host);
                    SignUpVO.RemoveAll(n => n.PartySignUpID == item.PartySignUpID);
                    foreach (CardPartySignUpVO Statusitem in ItemInviterVO)
                    {
                        if(item.PartySignUpID== Statusitem.PartySignUpID)
                        {
                            Statusitem.LuckDrawStatus = 1;
                        }
                    }
                }
            }

            //副奖开奖
            //随机排序
            PrizeListVO = RandomSortList(PrizeListVO);
            SignUpVO = RandomSortList(SignUpVO);

            foreach(CardPartySignUpVO item in SignUpVO)
            {
                int index = SignUpVO.IndexOf(item);
                decimal Cost = PrizeListVO[0].Cost;

                if(Cost == 0.33M && PrizeListVO[0].isAutoPay == 1&& index > 50)
                {
                    item.LuckDrawStatus = 2;
                    UpdateSignUp(item);
                    continue;
                }
                if (PrizeListVO.Count > 0)
                {
                    item.LuckDrawStatus = 1;

                    item.LuckDrawNames = PrizeListVO[0].Name;   
                    item.LuckDrawContent = PrizeListVO[0].Content;
                    try
                    {
                        if (PrizeListVO[0].isAutoPay == 1 && PrizeListVO[0].Cost > 0)
                        {
                            if (Cost == 0.33M)
                            {
                                Random Rdm = new Random();
                                decimal iRdm = Rdm.Next(1, 30);
                                Cost = iRdm / 100;
                            }
                            string retu = PayforWXUserCash(Cost, item.Name, item.OpenId, item.CustomerId,4,"活动星选中奖奖金");
                            if(retu== "SUCCESS")
                            {
                                item.LuckDrawPayStatus = 1;
                            }
                        }
                    }
                    catch
                    {

                    }
                    PrizeListVO.RemoveAt(0);
                    UpdateSignUp(item);
                }
                else
                {
                    item.LuckDrawStatus = 2;
                    UpdateSignUp(item);
                }
                sendWinningMessage(item.OpenId, item.PartyID, PartyVO.Title, Host);
            }
            return true;
        }

        /// （在两个时间范围内）生成随机日期
        /// </summary>
        /// <param name="startime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns>返回随机日期，如（2014-12-25 00:00:00）</returns>
        public static DateTime RandomTime(DateTime startime, DateTime endtime)
        {
            Random rd = new Random();
            TimeSpan tsp = endtime - startime;
            int Seconds = rd.Next(0, tsp.Seconds);
            DateTime newtime = endtime.AddSeconds(-Seconds);
            return newtime;
        }

        /// <summary>
        /// 所有人变成未中奖
        /// </summary>
        /// <returns></returns>
        public bool DrawAPrizeNot(int PartyID)
        {
            CardPartyVO PartyVO = FindPartyById(PartyID);
            if (PartyVO == null) return false;
            if (PartyVO.Type != 3) return false;

            string Host = "";
            if (PartyVO.Host != "")
            {
                Host = PartyVO.Host;
            }
            else
            {
                List<CardDataVO> dVO = FindCardByCustomerId(PartyVO.CustomerId);
                if (dVO.Count > 0)
                {
                    Host = dVO[0].Name;
                }
                else
                {
                    Host = "活动星选平台";
                }
            }

            PartyVO.PartyLuckDrawStatus = 1;
            PartyVO.LuckDrawAt = DateTime.Now;
            UpdateParty(PartyVO);

            //获取奖品列表
            List<CardPartyCostVO> CostVO = FindCostByPartyID(PartyID);
            //获取未开奖人员列表
            List<CardPartySignUpVO> SignUpVO = FindSignUpByCondtion("PartyID=" + PartyID + " and LuckDrawStatus=0 and isAutoAdd=0");

            //假开奖
            if (PartyVO.isNoDraw == 1)
            {
                foreach (CardPartySignUpVO item in SignUpVO)
                {
                    item.LuckDrawStatus = 2;
                    UpdateSignUp(item);
                    if (item.OpenId != "")
                    {
                        sendWinningMessage(item.OpenId, item.PartyID, PartyVO.Title, Host);
                    }
                }
                return true;
            }
            if (PartyVO.isRepeat == 1)
            {
                CreateDailyLottery(PartyVO.PartyID);
            }
            return false;
        }

        /// <summary>
        /// 补发瓜分奖金
        /// </summary>
        /// <returns></returns>
        public bool SupplyAgain(int PartyID)
        {
            CardPartyVO PartyVO = FindPartyById(PartyID);
            if (PartyVO == null) return false;
            if (PartyVO.Type != 3) return false;

            PartyVO.PartyLuckDrawStatus = 1;
            UpdateParty(PartyVO);

            //获取未发放奖金人员列表
            List<CardPartySignUpVO> SignUpVO = FindSignUpByCondtion("PartyID=" + PartyID + " and LuckDrawPayStatus=0 and isAutoAdd=0");

            foreach (CardPartySignUpVO item in SignUpVO)
            {
                decimal Cost = 0;
                Random Rdm = new Random();
                decimal iRdm = Rdm.Next(30, 88);
                Cost = iRdm / 100;

                string retu = PayforWXUserCash(Cost, item.Name, item.OpenId, item.CustomerId, 4, "活动星选中奖奖金");
                if (retu == "SUCCESS")
                {
                    item.LuckDrawPayStatus = 1;
                }
                item.LuckDrawStatus = 1;

                item.LuckDrawNames = "参与奖";
                item.LuckDrawContent = "瓜分现金红包";
                UpdateSignUp(item);
            }
            return true;
        }


        /// <summary>
        /// 随机排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ListT"></param>
        /// <returns></returns>
        private List<T> RandomSortList<T>(List<T> ListT)
        {
            Random random = new Random();
            List<T> newList = new List<T>();
            foreach (T item in ListT)
            {
                newList.Insert(random.Next(newList.Count + 1), item);
            }
            return newList;
        }

        /// <summary>
        /// 自动创建日常抽奖活动
        /// </summary>
        /// <returns></returns>
        public bool CreateDailyLottery()
        {
            CardPartyVO PartyVO = new CardPartyVO();
            PartyVO.PartyID = 0;
            PartyVO.CustomerId = 345;
            PartyVO.Title = "每日现金抽奖，奖金直发微信";
            PartyVO.MainImg = "https://www.zhongxiaole.net/SPManager/UploadFolder/Image/202103/202103021159100330.jpg";
            PartyVO.CreatedAt = DateTime.Now;
            DateTime StartTime = DateTime.Now.AddDays(1);
            PartyVO.StartTime =new DateTime(StartTime.Year, StartTime.Month, StartTime.Day,12,0,0);
            PartyVO.SignUpTime = PartyVO.StartTime;
            PartyVO.Details = "<div>1.报名必须填写真实个人信息，名字需与微信实名一致，否则奖金无法到账，过后不再补发；\r\n2.活动真实有效，仅限本页面报名；\r\n3.新增分享加成机制，邀请越多好友报名，中大奖几率将越高；\r\n4.扫码识别加入活动星选官方微信群，接收每日最新抽奖资讯！</div></NTAG><img src='https://www.zhongxiaole.net/SPManager/UploadFolder/Image/202104/202104071033462272.jpg' ></img></NTAG>";
            PartyVO.Status = 1;
            PartyVO.isDisplayContacts = 0;
            PartyVO.isDisplaySignup = 0;
            PartyVO.isClickSignup = 1;
            PartyVO.isDisplayCost = 1;
            PartyVO.isPromotionAward = 0;
            PartyVO.isStartTime = 0;
            PartyVO.isEndTime = 0;
            PartyVO.Host = "活动星选平台";
            PartyVO.Content = "每天中午12点整准时开奖，微信现金红包天天抽";
            PartyVO.style = 15;
            PartyVO.Type = 3;
            PartyVO.AppType = 1;
            PartyVO.isHot = 0;

            List<CardPartyContactsVO> ContactsVO = new List<CardPartyContactsVO>();
            CardPartyContactsVO cVO = new CardPartyContactsVO();
            cVO.CardID = 27;
            ContactsVO.Add(cVO);

            List<CardPartySignUpFormVO> SignUpFormVO = new List<CardPartySignUpFormVO>();

            List<CardPartyCostVO> CostVO = new List<CardPartyCostVO>();

            CardPartyCostVO pcVO = new CardPartyCostVO();
            pcVO.Image = "https://www.zhongxiaole.net/SPManager/UploadFolder/Image/202103/202103051128328468.jpg";
            pcVO.Names = "一等奖";
            pcVO.Cost = 88.8M;
            pcVO.Status = 1;
            pcVO.limitPeopleNum = 1;
            pcVO.Content = "88.8元-微信现金红包";
            pcVO.isAutoPay = 1;
            pcVO.isFirstPrize = 1;
            CostVO.Add(pcVO);

            CardPartyCostVO pcVO2 = new CardPartyCostVO();
            pcVO2.Image = "https://www.zhongxiaole.net/SPManager/UploadFolder/Image/202103/202103051128478801.jpg";
            pcVO2.Names = "二等奖";
            pcVO2.Cost = 18.88M;
            pcVO2.Status = 1;
            pcVO2.limitPeopleNum = 2;
            pcVO2.Content = "18.88元-微信现金红包";
            pcVO2.isAutoPay = 1;
            CostVO.Add(pcVO2);

            CardPartyCostVO pcVO3 = new CardPartyCostVO();
            pcVO3.Image = "https://www.zhongxiaole.net/SPManager/UploadFolder/Image/202103/202103051129113929.jpg";
            pcVO3.Names = "三等奖";
            pcVO3.Cost = 8.88M;
            pcVO3.Status = 1;
            pcVO3.limitPeopleNum = 5;
            pcVO3.Content = "8.88元-微信现金红包";
            pcVO3.isAutoPay = 1;
            CostVO.Add(pcVO3);

            CardPartyCostVO pcVO4 = new CardPartyCostVO();
            pcVO4.Image = "https://www.zhongxiaole.net/SPManager/UploadFolder/Image/202103/202103051129349507.jpg";
            pcVO4.Names = "参与奖";

            pcVO4.Cost = 0.33M;
            pcVO4.Status = 1;
            pcVO4.limitPeopleNum = 300;
            pcVO4.Content = "瓜分1888元现金红包";
            pcVO4.isAutoPay = 1;
            CostVO.Add(pcVO4);

            try
            {
                //为活动创建名片组
                CardGroupVO CardGroupVO = new CardGroupVO();
                CardGroupVO.CreatedAt = DateTime.Now;
                CardGroupVO.CustomerId = 345;
                CardGroupVO.JoinSetUp = 1;
                CardGroupVO.GroupName = "活动-" + PartyVO.Title;

                int GroupID = AddCardGroup(CardGroupVO);
                if (GroupID > 0)
                {
                    //将联系人作为管理员加入名片组
                    PartyVO.GroupID = GroupID;
                    List<int> Cid = new List<int>();
                    for (int i = 0; i < ContactsVO.Count; i++)
                    {
                        CardDataVO CardDataVO = FindCardById(ContactsVO[i].CardID);
                        if (CardDataVO != null)
                        {
                            int CustomerId = CardDataVO.CustomerId;
                            if (!Cid.Contains(CustomerId))
                            {
                                CardGroupCardVO cgcVO = new CardGroupCardVO();
                                cgcVO.CustomerId = CustomerId;
                                cgcVO.GroupID = GroupID;
                                cgcVO.Status = 3;
                                cgcVO.CreatedAt = DateTime.Now;
                                cgcVO.CardID = ContactsVO[i].CardID;
                                AddCardToGroup(cgcVO);

                                Cid.Add(CustomerId);
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            if (AddParty(PartyVO, ContactsVO, SignUpFormVO, CostVO,true) > 0) {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 自动复制日常抽奖活动
        /// </summary>
        /// <returns></returns>
        public bool CreateDailyLottery(int PartyID)
        {
            CardPartyVO OldPartyVO = FindPartyById(PartyID);
            if (OldPartyVO == null) return false;

            CardPartyVO PartyVO = new CardPartyVO();
            PartyVO.PartyID = 0;
            PartyVO.CustomerId = OldPartyVO.CustomerId;
            PartyVO.Title = OldPartyVO.Title;
            PartyVO.MainImg = OldPartyVO.MainImg;
            PartyVO.CreatedAt = DateTime.Now;

            TimeSpan tsp = OldPartyVO.StartTime - OldPartyVO.CreatedAt;
            if (OldPartyVO.StartTime.Date != OldPartyVO.CreatedAt.Date)
            {  
                int Days = Convert.ToInt32(Math.Round(tsp.TotalDays));
                if (Days < 1) Days = 1;

                string tartTime = DateTime.Now.AddDays(Days).ToString("yyyy-MM-dd") + " " + OldPartyVO.StartTime.ToString("HH") + ":" + OldPartyVO.StartTime.ToString("mm") + ":00";
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy-MM-dd HH:mm:ss";
                DateTime dt = Convert.ToDateTime(tartTime, dtFormat);

                PartyVO.StartTime = dt;
            }else
            {
                PartyVO.StartTime = DateTime.Now.AddHours(tsp.Hours).AddMinutes(tsp.Minutes);
            }
            
            PartyVO.SignUpTime = PartyVO.StartTime;
            PartyVO.Details = OldPartyVO.Details;
            PartyVO.Status = 1;
            PartyVO.isDisplayContacts = OldPartyVO.isDisplayContacts;
            PartyVO.isDisplaySignup = OldPartyVO.isDisplaySignup;
            PartyVO.isClickSignup = OldPartyVO.isClickSignup;
            PartyVO.isDisplayCost = OldPartyVO.isDisplayCost;
            PartyVO.isPromotionAward = OldPartyVO.isPromotionAward;
            PartyVO.isStartTime = OldPartyVO.isStartTime;
            PartyVO.isEndTime = OldPartyVO.isEndTime;
            PartyVO.Host = OldPartyVO.Host;
            PartyVO.Content = OldPartyVO.Content;
            PartyVO.style = OldPartyVO.style;
            PartyVO.Type = OldPartyVO.Type;
            PartyVO.AppType = OldPartyVO.AppType;
            PartyVO.isHot = OldPartyVO.isHot;
            PartyVO.isIndex = OldPartyVO.isIndex;
            PartyVO.isNoDraw = OldPartyVO.isNoDraw;
            PartyVO.isRepeat = OldPartyVO.isRepeat;
            PartyVO.isCashLuckDraw = OldPartyVO.isCashLuckDraw;

            PartyVO.SendImg = OldPartyVO.SendImg;
            PartyVO.isSendImg = OldPartyVO.isSendImg;
            PartyVO.SignupConditions = OldPartyVO.SignupConditions;
            PartyVO.AuthorizationID = OldPartyVO.AuthorizationID;
            PartyVO.SignupKeyWord = OldPartyVO.SignupKeyWord;

            List<CardPartyCostVO> CostVO = FindCostByPartyID(OldPartyVO.PartyID);
            List<CardPartyContactsVO> ContactsVO = FindPartyContacts(OldPartyVO.PartyID);
            List<CardPartySignUpFormVO> SignUpFormVO = FindSignUpFormByPartyID(OldPartyVO.PartyID, 1);

            List<CardPartyCostVO> newCostVO = new List<CardPartyCostVO>();
            foreach (CardPartyCostVO item in CostVO)
            {
                CardPartyCostVO cost = new CardPartyCostVO();

                cost.PartyCostID = 0;
                cost.PartyID = 0;
                cost.Names = item.Names;
                cost.limitPeopleNum = item.limitPeopleNum;
                cost.isFirstPrize = item.isFirstPrize;
                cost.AppType = item.AppType;
                cost.Content = item.Content;
                cost.Cost = item.Cost;
                cost.DiscountCost = item.DiscountCost;
                cost.DiscountNum = item.DiscountNum;
                cost.DiscountTime = item.DiscountTime;
                cost.EffectiveTime = item.EffectiveTime;
                cost.Image = item.Image;
                cost.isAutoPay = item.isAutoPay;
                cost.isDiscount = item.isDiscount;
                cost.PromotionAward = item.PromotionAward;
                cost.QuantitySold = item.QuantitySold;

                newCostVO.Add(cost);
            }

            List<CardPartyContactsVO> newContactsVO = new List<CardPartyContactsVO>();
            foreach (CardPartyContactsVO item in ContactsVO)
            {
                CardPartyContactsVO Contacts = new CardPartyContactsVO();
                Contacts.PartyContactsID = 0;
                Contacts.CardID = item.CardID;
                Contacts.AppType = item.AppType;

                newContactsVO.Add(Contacts);
            }

            List<CardPartySignUpFormVO> newSignUpFormVO = new List<CardPartySignUpFormVO>();
            foreach (CardPartySignUpFormVO item in SignUpFormVO)
            {
                CardPartySignUpFormVO SignUpForm = new CardPartySignUpFormVO();
                SignUpForm.SignUpFormID = 0;
                SignUpForm.AppType = item.AppType;
                SignUpForm.AutioText = item.AutioText;
                SignUpForm.must = item.must;
                SignUpForm.Name = item.Name;
                SignUpForm.Status = item.Status;
                SignUpForm.value = item.value;

                newSignUpFormVO.Add(SignUpForm);
            }

            int RepeatPartyID = AddParty(PartyVO, newContactsVO, newSignUpFormVO, newCostVO, true);
            if (RepeatPartyID > 0)
            {
                OldPartyVO.RepeatPartyID = RepeatPartyID;
                UpdateParty(OldPartyVO);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 添加群发图文记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddMedia(MediaVO vo)
        {
            try
            {
                IMediaDAO rDAO = CustomerManagementDAOFactory.MediaDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int MediaID = rDAO.Insert(vo);
                    return MediaID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新群发图文记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateMedia(MediaVO vo)
        {
            IMediaDAO rDAO = CustomerManagementDAOFactory.MediaDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 获取群发图文记录列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<MediaVO> FindMediaByCondition(string condition, params object[] parameters)
        {
            IMediaDAO rDAO = CustomerManagementDAOFactory.MediaDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condition);
        }

        /// <summary>
        /// 获取群发图文记录数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindMediaTotalCount(string condition, params object[] parameters)
        {
            IMediaDAO rDAO = CustomerManagementDAOFactory.MediaDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 获取群发图文记录详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public MediaVO FindMediaById(int MediaID)
        {
            IMediaDAO rDAO = CustomerManagementDAOFactory.MediaDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(MediaID);
        }


        /// <summary>
        /// 生成图文链接
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Style">"Card":个人名片,"Party"：活动链接,"Questionnaire"：表格,"Softarticle"：软文(未完成，不可用)</param>
        /// <param name="isRestart">是否重新生成</param>
        /// <returns></returns>
        public string getArticleUrl(int ID, string Style, int isRestart = 0)
        {
            string title = "";
            string content = "";
            WX_JSSDK jssdk = new WX_JSSDK();
            if (Style == "Party")
            {
                CardPartyVO cvo = FindPartyById(ID);   
                if (cvo == null) { return ""; }
                if (cvo.MediaID > 0 && isRestart==0)
                {
                    MediaVO mVO = FindMediaById(cvo.MediaID);
                    if (mVO.Status == 1)
                    {
                        return mVO.ArticleUrl;
                    }else if(mVO.CreatedAt.CompareTo(DateTime.Now.AddMinutes(-1))>0)
                    {
                        return "SUCCESS";
                    }
                }

                title = cvo.Title;


                content = "<a data-miniprogram-appid='"+appid+"' data-miniprogram-path='pages/Party/PartyShow/PartyShow?PartyID=" + cvo.PartyID + "' href=''>" + getPartyHtml(cvo) + "</a>";

                if (cvo.Type == 3 || cvo.isBlindBox == 1)
                {
                    content = "<a data-miniprogram-appid='" + appid + "' data-miniprogram-path='package/package_sweepstakes/PartyShow/PartyShow?PartyID=" + cvo.PartyID + "' href=''>" + getLuckDrawHtml(cvo) + "</a>";
                }

                int MediaID = jssdk.add_news(title, content, cvo.CustomerId);
                if (MediaID <= 0)
                {
                    return "ERROR";
                }

                cvo.MediaID = MediaID;
                UpdateParty(cvo);

                Thread.Sleep(5000);

                MediaVO mVO2 = FindMediaById(cvo.MediaID);
                if (mVO2.Status == 1)
                {
                    return mVO2.ArticleUrl;
                }
                else
                {
                    return "SUCCESS";
                }
            }

            if (Style == "Card")
            {
                CardDataVO cvo = FindCardById(ID);
                if (cvo == null) { return ""; }
                if (cvo.MediaID > 0 && isRestart == 0)
                {
                    MediaVO mVO = FindMediaById(cvo.MediaID);
                    if (mVO.Status == 1)
                    {
                        return mVO.ArticleUrl;
                    }
                    else if (mVO.CreatedAt.CompareTo(DateTime.Now.AddMinutes(-1)) > 0)
                    {
                        return "SUCCESS";
                    }
                }

                title = cvo.Name+"的名片";
                content = "<a data-miniprogram-appid='" + appid + "' data-miniprogram-path='pages/ShowCard/ShowCard?CardID=" + cvo.CardID + "' href=''>" + getCardHtml(cvo) + "</a>";

                int MediaID = jssdk.add_news(title, content, cvo.CustomerId);
                if (MediaID <= 0)
                {
                    return "ERROR";
                }

                cvo.MediaID = MediaID;
                Update(cvo);

                Thread.Sleep(5000);

                MediaVO mVO2 = FindMediaById(cvo.MediaID);
                if (mVO2.Status == 1)
                {
                    return mVO2.ArticleUrl;
                }
                else
                {
                    return "SUCCESS";
                }
            }

            if (Style == "Questionnaire")
            {
                CardQuestionnaireVO cvo = FindCardQuestionnaireByQuestionnaireID(ID);
                if (cvo == null) { return ""; }
                if (cvo.MediaID > 0 && isRestart == 0)
                {
                    MediaVO mVO = FindMediaById(cvo.MediaID);
                    if (mVO.Status == 1)
                    {
                        return mVO.ArticleUrl;
                    }
                    else if (mVO.CreatedAt.CompareTo(DateTime.Now.AddMinutes(-1)) > 0)
                    {
                        return "SUCCESS";
                    }
                }

                title = cvo.Title;
                content = "<a data-miniprogram-appid='" + appid + "' data-miniprogram-path='pages/index/SignInFormByUser/SignInFormByUser?QuestionnaireID=" + cvo.QuestionnaireID + "' href=''>" + getQuestionnaireHtml(cvo) + "</a>";

                int MediaID = jssdk.add_news(title, content, cvo.CustomerId);
                if (MediaID <= 0)
                {
                    return "ERROR";
                }

                cvo.MediaID = MediaID;
                UpdateCardQuestionnaire(cvo);

                Thread.Sleep(5000);

                MediaVO mVO2 = FindMediaById(cvo.MediaID);
                if (mVO2.Status == 1)
                {
                    return mVO2.ArticleUrl;
                }
                else
                {
                    return "SUCCESS";
                }
            }

            if (Style == "Softarticle")
            {
                CardSoftArticleVO cvo = FindSoftArticleById(ID);
                
                if (cvo == null) { return ""; }
                if (cvo.MediaID > 0 && isRestart == 0)
                {
                    MediaVO mVO = FindMediaById(cvo.MediaID);
                    if (mVO.Status == 1)
                    {
                        return mVO.ArticleUrl;
                    }
                    else if (mVO.CreatedAt.CompareTo(DateTime.Now.AddMinutes(-1)) > 0)
                    {
                        return "SUCCESS";
                    }
                }
                
                title = cvo.Title;
                content = "<a data-miniprogram-appid='" + appid + "' data-miniprogram-path='pages/MyCenter/Article/Article?SoftArticleID=" + cvo.SoftArticleID + "' href=''>" + getSoftArticleHtml(cvo) + "</a>";

                int MediaID = jssdk.add_news(title, content, cvo.CustomerId);
                if (MediaID <= 0)
                {
                    return "ERROR";
                }

                cvo.MediaID = MediaID;
                UpdateSoftArticle(cvo);

                Thread.Sleep(5000);

                MediaVO mVO2 = FindMediaById(cvo.MediaID);
                if (mVO2.Status == 1)
                {
                    return mVO2.ArticleUrl;
                }
                else
                {
                    return "SUCCESS";
                }
            }

            return "ERROR";
        }

        /// <summary>
        /// 获取活动Html
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public string getPartyHtml(CardPartyVO pVO)
        {
            string Html="";

            Html += "<div style='background: url(http://mmbiz.qpic.cn/mmbiz_jpg/xdGD1MlRd5COvWgoNbTZXofk1lqkvcQRGwpvTwaXO77Ptp9n0AEhxqtE6ac4Nk4RvCAeVaOU6tzf5yCicLWl3xw/0) repeat-y top center /100%;padding: 4%;margin: 0 auto; '>";
            Html += "<div style='background: url(http://mmbiz.qpic.cn/mmbiz_jpg/xdGD1MlRd5COvWgoNbTZXofk1lqkvcQRwpoblmN7bZFfBdxKhXhyrFax3fUlyWZI4dCu1wClCagS8rXyYNlMRQ/0) top center /100%;padding: 6% 4%;margin: 0 auto; z-index:1;background-size: 16%; border-radius:5px;position:relative;'>";
            Html += "<div style='font-size:22px;color: #f00;font-weight: bold;width: 100%;text-align: center;line-height: 28px;text-align:center'>" + pVO.Title + "</div>";
            Html += "</div>";
            Html += "<div style='padding: 4%;margin: 0 auto;background: #ffffff; border-radius:5px;position:relative; margin-top:11px; z-index:0'>";
            Html += "<div style='font-size:16px; text-align:center; padding:2%;color: #333;'>开始时间:<font style='color: #fff;background:#f00; border-radius:3px; padding:0 2px; margin:0 1px;font-weight: bold;'>" + pVO.StartTime.ToString("MM") + "</font>月<font style='color: #fff;background:#f00; border-radius:3px; padding:0 2px; margin:0 1px;font-weight: bold;'>" + pVO.StartTime.ToString("dd") + "</font>日<font style='color: #fff;background:#f00; border-radius:3px; padding:0 2px; margin:0 1px;font-weight: bold;'>" + pVO.StartTime.ToString("HH") + "</font>点<font style='color: #fff;background:#f00; border-radius:3px; padding:0 2px; margin:0 1px;font-weight: bold;'>" + pVO.StartTime.ToString("mm") + "</font>分</div>";
            Html += "<div>";
            Html += "<div style='display:table;width:100%;color: #999; font-size:15px;margin-top:2%;'>";
            Html += "<div style='float:left;font-weight: 300;'>截止报名</div>";
            Html += "<div style='float:left; width:71%;color: #333; margin-left:3%'>"+ pVO.SignUpTime.ToString("yyyy年MM月dd日 HH:mm") + "</div>";
            Html += "</div>";
            if (pVO.Address != "")
            {
                Html += "<div style='display:table;width:100%;color: #999; font-size:15px;margin-top:2%;'>";
                Html += "<div style='float:left;font-weight: 300;'>导航地址</div>";
                Html += "<div style='float:left; width:71%;color: #333; margin-left:3%'>" + pVO.Address + "</div>";
                Html += "</div>";
            }
            if (pVO.DetailedAddress != "")
            {
                Html += "<div style='display:table;width:100%;color: #999; font-size:15px;margin-top:2%;'>";
                Html += "<div style='float:left;font-weight: 300;'>详细地址</div>";
                Html += "<div style='float:left; width:71%;color: #333; margin-left:3%'>" + pVO.DetailedAddress + "</div>";
                Html += "</div>";
            }
            if (pVO.Host != "")
            {
                Html += "<div style='display:table;width:100%;color: #999; font-size:15px;margin-top:2%;'>";
                Html += "<div style='float:left;font-weight: 300;'>主办单位</div>";
                Html += "<div style='float:left; width:71%;color: #333; margin-left:3%'>" + pVO.Host + "</div>";
                Html += "</div>";
            }
            Html += "</div>";
            Html += "</div>";
            Html += "</div>";
            Html += "<div style='width:60%;background: linear-gradient(145deg, #ff202a, #e80000,#ff9399); text-align:center; font-size:17px; color:#fff; line-height:37px; border-radius:37px; border-top-left-radius:0; margin:0 auto; margin-top:4%'>点击查看活动<svg style='overflow: visible;pointer-events: none;max-width: none !important;isolation: isolate;height:24px;width:24px;height: 15px;width: 15px;margin-left: 10px;' version='1.1' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' x='0' y='0' viewBox='0 0 18 18' xml:space='preserve'><g><g transform='translate(10 10)'><g style='pointer-events:visible;'><circle cx='1' opacity='0.4' cy='1' fill='#fff' r='11.7648'><animate attributeName='r' values='10.5;13;10.5' begin='0s' dur='1.2s' repeatCount='indefinite'></animate></circle><circle cx='1' cy='1' fill='#fff' r='4.16'></circle></g></g></g></svg></div>";

            return Html;
        }

        /// <summary>
        /// 获取抽奖Html
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public string getLuckDrawHtml(CardPartyVO pVO)
        {
            string Html = "";

            Html += "<div style='background: url(http://mmbiz.qpic.cn/mmbiz_jpg/xdGD1MlRd5DHZials7Cdl2gsUmn4BRD7C7iarvyjd2z24YeVczu7icN2GXd8hYgu6HgLOKqUh4vGaP1zjDUFbMib1A/0) repeat-y top center /100%;padding: 4%;margin: 0 auto; '>";
            Html += "<div style='background: url(http://mmbiz.qpic.cn/mmbiz_jpg/xdGD1MlRd5DHZials7Cdl2gsUmn4BRD7CWdt58Zl187wq4ajbBB523zaIouTcbb1WMNvmozrBRXrhUlgL4gx2lQ/0) top center /100%;padding-top: 31%;margin: 0 auto; z-index:1;border-radius:5px;position:relative; border-bottom-left-radius:0;border-bottom-right-radius:0;'></div>";
            Html += "<div style='padding:4%;margin: 0 auto;border-radius:5px;border-top-left-radius:0;border-top-right-radius:0;background: #ffffff; position:relative;  z-index:0'>";
            Html += "<div style='font-size:20px; color:#f00; font-weight:bold;line-height: 24px;margin: 1% 0;text-align:center'>" + pVO.Title + "</div>";

            if (pVO.LuckDrawType == 1)
            {
                Html += "<div style='font-size:14px; text-align:center; padding:2%;color: #333;'>开奖时间：<font style='color: #fff;background:#f00; border-radius:3px; padding:0 2px; margin:0 1px;font-weight: bold;'>" + pVO.StartTime.ToString("yyyy") + "</font>年<font style='color: #fff;background:#f00; border-radius:3px; padding:0 2px; margin:0 1px;font-weight: bold;'>" + pVO.StartTime.ToString("MM") + "</font>月<font style='color: #fff;background:#f00; border-radius:3px; padding:0 2px; margin:0 1px;font-weight: bold;'>" + pVO.StartTime.ToString("dd") + "</font>日 <font style='color: #fff;background:#f00; border-radius:3px; padding:0 2px; margin:0 1px;font-weight: bold;'>" + pVO.StartTime.ToString("HH") + "</font>:<font style='color: #fff;background:#f00; border-radius:3px; padding:0 2px; margin:0 1px;font-weight: bold;'>" + pVO.StartTime.ToString("mm") + "</font></div>";
            }
            else if(pVO.LuckDrawType == 2)
            {
                Html += "<div style='font-size:14px; text-align:center; padding:2%;color: #333;'>报名满<font style='color: #fff;background:#f00; border-radius:3px; padding:0 2px; margin:0 1px;font-weight: bold;'>" + pVO.limitPeopleNum + "</font>人就自动开奖啦</div>";
            }
            else if (pVO.LuckDrawType == 3)
            {
                Html += "<div style='font-size:14px; text-align:center; padding:2%;color: #333;'>由发起方主动开奖</div>";
            }

            List<CardPartyCostVO> CostVO = FindCostByPartyID(pVO.PartyID);

            foreach(CardPartyCostVO item in CostVO)
            {
                Html += "<div style='display:table;width:100%;color: #999; font-size:15px;margin-top:2%;'>";
                Html += "<div style='float:left;font-weight: 300;'>"+ item.Names + "</div>";
                Html += "<div style='float:left; width:71%;color: #333; margin-left:3%'>" + item.Content + "</div>";
                Html += "</div>";
            }

            Html += "</div>";
            Html += "</div>";
            Html += "<div style='width:60%;background: linear-gradient(145deg, #ff202a, #e80000,#ff9399); text-align:center; font-size:17px; color:#fff; line-height:37px; border-radius:37px; border-top-left-radius:0; margin:0 auto; margin-top:4%'>点击报名抽奖<svg style='overflow: visible;pointer-events: none;max-width: none !important;isolation: isolate;height:24px;width:24px;height: 15px;width: 15px;margin-left: 10px;' version='1.1' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' x='0' y='0' viewBox='0 0 18 18' xml:space='preserve'><g><g transform='translate(10 10)'><g style='pointer-events:visible;'><circle cx='1' opacity='0.7' cy='1' fill='#fff' r='11.7648'><animate attributeName='r' values='10.5;13;10.5' begin='0s' dur='1.2s' repeatCount='indefinite'></animate></circle><circle cx='1' cy='1' fill='#fff' r='4.16'></circle></g></g></g></svg></div>";

            return Html;
        }

        /// <summary>
        /// 获取名片Html
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public string getCardHtml(CardDataVO dVO,bool isWX=true)
        {
            string Html = "";
            string CardBack = "http://mmbiz.qpic.cn/mmbiz_jpg/xdGD1MlRd5DHZials7Cdl2gsUmn4BRD7C8qcicZPX7yzZkahkZAytKwcmBiap5D4TYuQsNO7hzpIzK9ASmCxcA7wA/0";
            string Phone = "http://mmbiz.qpic.cn/mmbiz_jpg/xdGD1MlRd5DHZials7Cdl2gsUmn4BRD7CoK9ct2As3Rbqn6rJWhp9ttU4fYiacR9icsyNb10gbg96XNaTvmgtKRkQ/0";
            string email = "http://mmbiz.qpic.cn/mmbiz_jpg/xdGD1MlRd5DHZials7Cdl2gsUmn4BRD7C3zX4k341aqvLIo37jXjtDIGquVvgYajRAibbd6Tdsb2eUdC0o29gmeQ/0";
            string adress = "http://mmbiz.qpic.cn/mmbiz_jpg/xdGD1MlRd5DHZials7Cdl2gsUmn4BRD7Chq3ttlN6OPWibkuXh96XbicrR2Ol7874eU93xC0yT3uF8FRlwx7mDQhg/0";

            if (!isWX)
            {
                CardBack = "http://zhongxiaole.net/SPManager/Style/images/ArticleUrl/CardBack.jpg";
                Phone = "http://zhongxiaole.net/SPManager/Style/images/ArticleUrl/Phone.jpg";
                email = "http://zhongxiaole.net/SPManager/Style/images/ArticleUrl/email.jpg";
                adress = "http://zhongxiaole.net/SPManager/Style/images/ArticleUrl/adress.jpg";
            }

            Html += "<div style='background:#fff url("+ CardBack + "); background-size:100% auto; padding:4%'>";
            Html += "<div style='background:#fff; padding:4%; border-radius:5px;'>";
            Html += "<div style='margin-bottom:10px;'>";
            Html += "<div style='font-size:17px; color:#333;font-weight:bold'>" + dVO.Name + "</div>";
            Html += "<div style='font-size:14px;color:#333'>" + dVO.Position + "</div>";
            Html += "</div>";
            Html += "<div style='margin-bottom:10px; display:table; width:100%;'><div style='width:10%; float:left; height:3px; background:#3f3637'></div><div style='width:10%; float:left; height:3px; background:#c8c9cb'></div><div style='width:10%; float:left; height:3px; background:#e19000'></div><div style='width:10%; float:left; height:3px; background:#b4b5b7'></div></div>";
            if(dVO.Business!="")
                Html += "<div style='font-size:14px;color:#333;margin-bottom:10px;'>"+ dVO.Business + "</div>";

            if(dVO.Phone != ""|| dVO.Email != ""||dVO.Address != "")
            {
                Html += "<div style='display:table; width:100%'>";
                if(dVO.Phone != "")
                    Html += "<div style='margin-top:10px;font-size:14px;color:#333; line-height:24px;background:url("+ Phone + ") no-repeat top left; background-size:24px; padding-left:30px'>"+ dVO.Phone + "</div>";
                if(dVO.Email != "")
                    Html += "<div style='margin-top:10px;font-size:14px;color:#333; line-height:24px;background:url("+ email + ") no-repeat top left; background-size:24px; padding-left:30px'>"+ dVO.Email + "</div>";
                if(dVO.Address != "")
                    Html += "<div style='margin-top:10px;font-size:14px;color:#333; line-height:24px;background:url("+ adress + ") no-repeat top left; background-size:24px; padding-left:30px'>"+ dVO.Address + "</div>";
                Html += "</div>";
            }
            Html += "</div>";
            Html += "</div>";
            Html += "<div style='width:60%;background: linear-gradient(145deg, #224299, #206eb8,#2170bb); text-align:center; font-size:17px; color:#fff; line-height:37px; border-radius:37px; border-top-left-radius:0; margin:0 auto; margin-top:4%'>点击查看名片<svg style='overflow: visible;pointer-events: none;max-width: none !important;isolation: isolate;height:24px;width:24px;height: 15px;width: 15px;margin-left: 10px;' version='1.1' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' x='0' y='0' viewBox='0 0 18 18' xml:space='preserve'><g><g transform='translate(10 10)'><g style='pointer-events:visible;'><circle cx='1' opacity='0.7' cy='1' fill='#fff' r='11.7648'><animate attributeName='r' values='10.5;13;10.5' begin='0s' dur='1.2s' repeatCount='indefinite'></animate></circle><circle cx='1' cy='1' fill='#fff' r='4.16'></circle></g></g></g></svg></div>";

            return Html;
        }

        /// <summary>
        /// 获取表格Html
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public string getQuestionnaireHtml(CardQuestionnaireVO cVO)
        {
            string Html = "";

            Html += "<div style='background: url(http://mmbiz.qpic.cn/mmbiz_jpg/xdGD1MlRd5BeU7xLLDbxjtsYesYFvDNJufuByPXeDIg1CXyiaegDVeO4ersoicbxmiaLbIYD57kyk6ibPNctKahvXQ/0) repeat-y top center /100%;padding: 4%;margin: 0 auto; '>";
            Html += "<div style='background: url(http://mmbiz.qpic.cn/mmbiz_jpg/xdGD1MlRd5BeU7xLLDbxjtsYesYFvDNJGLUjaqVB8UvJQu9E9GgibYFmthByAMIcHrFLYPXpXcbrXCOgFRFNR0Q/0) top center /100%;padding-top: 29%;margin: 0 auto; z-index:1;border-radius:5px;position:relative; border-bottom-left-radius:0;border-bottom-right-radius:0;'></div>";
            Html += "<div style='padding:4%;margin: 0 auto;border-radius:5px;border-top-left-radius:0;border-top-right-radius:0;background: #ffffff; position:relative;  z-index:0'>";
            Html += "<div style='font-size:20px; color:#f00; font-weight:bold;line-height: 24px;margin: 1% 0;text-align:center'>"+ cVO.Title + "</div>";
            Html += "<div style='font-size:14px; text-align:center; padding:2%;color: #999;'>" + cVO.CreatedAt.ToString("yyyy.MM.dd HH:mm") + "</div>";
            Html += "<div style='display:table;width:100%;color: #333; font-size:14px;margin-top:2%; line-height:16px;'>";

            var models = JsonConvert.DeserializeObject<List<QuestionnaireSigupForm>>(cVO.Form);
            for (int i = 0; i < models.Count&&i<10; i++)
            {
                Html += "<div style='float:left;font-weight: 300; padding:4px 5px; border:solid 1px #999; border-radius:3px; margin-right:10px; margin-bottom:10px;'>"+ models[i].Name + "</div>";
            }
            Html += "</div>";
            Html += "</div>";
            Html += "</div>";
            Html += "<div style='width:60%;background: linear-gradient(145deg, #4947db, #3b51cd,#4aa5ea); text-align:center; font-size:17px; color:#fff; line-height:37px; border-radius:37px; border-top-left-radius:0; margin:0 auto; margin-top:4%'>点击填写表格<svg style='overflow: visible;pointer-events: none;max-width: none !important;isolation: isolate;height:24px;width:24px;height: 15px;width: 15px;margin-left: 10px;' version='1.1' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' x='0' y='0' viewBox='0 0 18 18' xml:space='preserve'><g><g transform='translate(10 10)'><g style='pointer-events:visible;'><circle cx='1' opacity='0.7' cy='1' fill='#fff' r='11.7648'><animate attributeName='r' values='10.5;13;10.5' begin='0s' dur='1.2s' repeatCount='indefinite'></animate></circle><circle cx='1' cy='1' fill='#fff' r='4.16'></circle></g></g></g></svg></div>";

            return Html;
        }

        /// <summary>
        /// 获取软文Html
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public string getSoftArticleHtml(CardSoftArticleVO cVO)
        {
            WX_JSSDK jssdk = new WX_JSSDK();
            string Html = "";
            string wxuul = jssdk.uploadimg(cVO.Image);


            if (cVO.CardID > 0)
            {
                CardDataVO dVO = FindCardById(cVO.CardID);
                if (dVO != null)
                {
                    cVO.Card = dVO;
                }
                else
                {
                    List<CardDataVO> CardDataVO = FindCardByCustomerId(cVO.CustomerId);
                    if (CardDataVO.Count > 0)
                    {
                        cVO.Card = CardDataVO[0];
                    }
                }
            }
            else
            {
                List<CardDataVO> CardDataVO = FindCardByCustomerId(cVO.CustomerId);
                if (CardDataVO.Count > 0)
                {
                    cVO.Card = CardDataVO[0];
                }
            }
            string name = "";
            if (cVO.Card != null)
            {
                name = cVO.Card.Name;
            }
            if (name != "")
            {
                name += "：";
            }

            Html += "<div style='background: #f1f1f1;padding: 4%;margin: 0 auto; '>";
            Html += "<div style='background: url("+ wxuul + ") top center /100%; background-size:cover;padding-top: 63%;margin: 0 auto; z-index:1;border-radius:5px;position:relative; border-bottom-left-radius:0;border-bottom-right-radius:0;'></div>";
            Html += "<div style='padding:4%;margin: 0 auto;border-radius:5px;border-top-left-radius:0;border-top-right-radius:0;background: #ffffff; position:relative;  z-index:0'>";
            Html += "<div style='font-size:20px; color:#f00; font-weight:bold;line-height: 24px;margin: 1% 0;text-align:center'>" + cVO.Title + "</div>";
            Html += "<div style='font-size:14px; text-align:center; padding:2%;color: #333;'>"+ name + cVO.CreatedAt.ToString("yyyy年MM月dd日 HH:mm") + "</div>";
            Html += "</div>";
            Html += "</div>";
            Html += "<div style='width:60%;background: linear-gradient(145deg, #ff202a, #e80000,#ff9399); text-align:center; font-size:17px; color:#fff; line-height:37px; border-radius:37px; border-top-left-radius:0; margin:0 auto; margin-top:4%'>点击查看文章<svg style='overflow: visible;pointer-events: none;max-width: none !important;isolation: isolate;height:24px;width:24px;height: 15px;width: 15px;margin-left: 10px;' version='1.1' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' x='0' y='0' viewBox='0 0 18 18' xml:space='preserve'><g><g transform='translate(10 10)'><g style='pointer-events:visible;'><circle cx='1' opacity='0.7' cy='1' fill='#fff' r='11.7648'><animate attributeName='r' values='10.5;13;10.5' begin='0s' dur='1.2s' repeatCount='indefinite'></animate></circle><circle cx='1' cy='1' fill='#fff' r='4.16'></circle></g></g></g></svg></div>";

            return Html;
        }

        /// <summary>
        /// 更新首页数据
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateIndexData(IndexDataVO vo)
        {
            IIndexDataDAO rDAO = CustomerManagementDAOFactory.IndexDataDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取首页数据
        /// </summary>
        /// <returns></returns>
        public IndexDataVO FindIndexData()
        {
            IIndexDataDAO rDAO = CustomerManagementDAOFactory.IndexDataDAO(this.CurrentCustomerProfile);
            List<IndexDataVO> List = rDAO.FindByParams("1=1");
            if (List.Count > 0)
            {
                IndexDataVO dVO = List[0];
                TimeSpan tsp = DateTime.Now - dVO.UpDataAt;
                if (tsp.Days>=1)
                {
                    dVO.UpDataAt = DateTime.Now;
                    dVO.PartyCount = GetIndexDataRandom(dVO.PartyCount);
                    dVO.GoodsCount = GetIndexDataRandom(dVO.GoodsCount);
                    dVO.PosterCount = GetIndexDataRandom(dVO.PosterCount);
                    dVO.SoftarticleCount = GetIndexDataRandom(dVO.SoftarticleCount);
                    dVO.QuestionnaireCount = GetIndexDataRandom(dVO.QuestionnaireCount);
                    dVO.GroupCount = GetIndexDataRandom(dVO.GroupCount);
                    dVO.DemandCount = GetIndexDataRandom(dVO.DemandCount);
                    dVO.BusinessCardCount = GetIndexDataRandom(dVO.BusinessCardCount);
                    dVO.CompanyCount = GetIndexDataRandom(dVO.CompanyCount);
                    dVO.LuckDrawCount = GetIndexDataRandom(dVO.LuckDrawCount);
                }
                UpdateIndexData(dVO);

                return dVO;
            }else
            {
                return null;
            }

        }
        /// <summary>
        /// 获取上下10%波动
        /// </summary>
        /// <returns></returns>
        int GetIndexDataRandom(int Count)
        {
            Random Rdm = new Random();
            int up = Convert.ToInt32(Count * 0.1);
            return Rdm.Next(Count- up, Count+ up);
        }


        /// <summary>
        /// 下载网络图片
        /// </summary>
        /// <returns></returns>
        public string downloadImages(string imgurl)
        {
            try
            {
                //下载头像
                string folder = "/UploadFolder/Image/" + DateTime.Now.ToString("yyyyMM") + "/";
                string imgPath = "";
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                //可以修改为网络路径
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string PhysicalPath = localPath + newFileName;
                imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;
                WebRequest wreq = WebRequest.Create(imgurl);
                HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                Stream s = wresp.GetResponseStream();
                System.Drawing.Image img;
                img = System.Drawing.Image.FromStream(s);
                img.Save(PhysicalPath, ImageFormat.Jpeg);   //保存
                return imgPath;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 批量下载微信文章图片
        /// </summary>
        /// <returns></returns>
        public string downloadwxImages(string content)
        {
            try
            {
                string newcontent = content;
                MatchCollection match = Regex.Matches(newcontent, "data-src=\"(?<text>[^\f\n\r\t\v\"]*)\"");

                foreach (Match item in match)
                {
                    newcontent=newcontent.Replace("data-src=\""+ item.Groups["text"].Value.Trim()+"\"", "src=\""+ downloadImages(item.Groups["text"].Value.Trim()) + "\"");
                }
                return newcontent;
            }
            catch
            {
                return content;
            }
        }

        /// <summary>
        /// 获取最新抽奖广告落地页活动ID
        /// </summary>
        /// <returns></returns>
        public int GetADPartyID()
        {
            int PartyID = GetPartyID(4682);
            return PartyID;
        }

        int GetPartyID(int PartyID)
        {
            CardPartyVO cVO = FindPartyById(PartyID);
            if (cVO.RepeatPartyID > 0)
            {
                return GetPartyID(cVO.RepeatPartyID);
            }else
            {
                return cVO.PartyID;
            }
        }

        /// <summary>
        /// 添加来源统计
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddLaunch(CardLaunchVO vo)
        {
            try
            {
                ICardLaunchDAO rDAO = CustomerManagementDAOFactory.CardLaunchDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int LaunchID = rDAO.Insert(vo);
                    return LaunchID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新来源统计
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateLaunch(CardLaunchVO vo)
        {
            ICardLaunchDAO rDAO = CustomerManagementDAOFactory.CardLaunchDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取来源列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardLaunchVO> FindCardLaunchAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardLaunchDAO rDAO = CustomerManagementDAOFactory.CardLaunchDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public List<CardLaunchVO> FindCardLaunchByCondtion(string condtion)
        {
            ICardLaunchDAO rDAO = CustomerManagementDAOFactory.CardLaunchDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取来源数量
        /// </summary>
        /// <returns></returns>
        public int FindCardLaunchCount(string condition)
        {
            ICardLaunchDAO rDAO = CustomerManagementDAOFactory.CardLaunchDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }


        /// <summary>
        /// 获取外部跳转到小程序链接（Link）
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public string GetUrlLink(string path,string query,int isPermanent = 0)
        {
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret + "";
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

                if (result.Result)
                {
                    string wxaurl = "https://api.weixin.qq.com/wxa/generate_urllink?access_token=" + result.SuccessResult.access_token;
                    DataJson = "{";
                    DataJson += "\"path\": \"" + path + "\",";
                    DataJson += "\"query\": \"" + query + "\",";
                    if(isPermanent==0)
                        DataJson += "\"is_expire\": true,";
                    else
                        DataJson += "\"is_expire\": false,";
                    DataJson += "\"expire_type\": 1,";
                    DataJson += "\"expire_interval\": 180";
                    DataJson += "}";

                    string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);

                    dynamic resultContent = JsonConvert.DeserializeObject(str, new { errcode = 0, url_link = "" }.GetType());
                    if(resultContent.errcode==0)
                    {
                        return resultContent.url_link;
                    }
                    else
                    {
                        return "";
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "";
            }
        }

        /// <summary>
        /// 获取外部跳转到小程序链接（Scheme）
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public string GetUrlScheme(string path, string query, int isPermanent = 0)
        {
            try
            {
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret + "";
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

                if (result.Result)
                {
                    string wxaurl = "https://api.weixin.qq.com/wxa/generatescheme?access_token=" + result.SuccessResult.access_token;
                    DataJson = "{";
                    DataJson += "\"jump_wxa\": {";
                    DataJson += "\"path\": \"" + path + "\",";
                    DataJson += "\"query\": \"" + query + "\"";
                    DataJson += "},";
                    if (isPermanent == 0)
                        DataJson += "\"is_expire\": true,";
                    else
                        DataJson += "\"is_expire\": false,";
                    DataJson += "\"expire_time\": " + ConvertDateTimeInt(DateTime.Now.AddDays(180)) + "";
                    DataJson += "}";

                    string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);

                    dynamic resultContent = JsonConvert.DeserializeObject(str, new { errcode = 0, openlink = "" }.GetType());
                    if (resultContent.errcode == 0)
                    {
                        return resultContent.openlink;
                    }
                    else
                    {
                        return "";
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return ex.Message.ToString();
            }
        }
        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddMilliseconds(d);
            return time;
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式,返回格式：1468482273277
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            //double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            //intResult = (time- startTime).TotalMilliseconds;
            long t = (time.Ticks - startTime.Ticks) / 10000000;            //除10000调整为13位
            return t;
        }

        /// <summary>
        /// 获取访问列表视图
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardAccessRecordsViewVO> FindCardAccessRecordsViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardAccessRecordsViewDAO rDAO = CustomerManagementDAOFactory.CardAccessRecordsViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex("AppType=" + AppType+ " and " + conditionStr, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取访问列表视图
        /// </summary>
        public List<CardAccessRecordsViewVO> FindCardAccessRecordsViewByCondtion(string condtion)
        {
            ICardAccessRecordsViewDAO rDAO = CustomerManagementDAOFactory.CardAccessRecordsViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取访问列表视图数量
        /// </summary>
        /// <returns></returns>
        public int FindCardAccessRecordsViewCount(string condition)
        {
            ICardAccessRecordsViewDAO rDAO = CustomerManagementDAOFactory.CardAccessRecordsViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 获取下线数量
        /// </summary>
        /// <returns></returns>
        public int FindOriginCustomerCount(string condition)
        {
            IOriginCustomerIdViewDAO rDAO = CustomerManagementDAOFactory.OriginCustomerIdViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 是否为VIP
        /// </summary>
        /// <returns></returns>
        public bool isVIP(int CustomerId)
        {
            try
            {
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                CustomerVO CustomerVO = uBO.FindCustomenById(CustomerId);

                if (CustomerVO == null)
                {
                    return false;
                }

                if (CustomerVO.isVip && CustomerVO.ExpirationAt > DateTime.Now)
                {
                    return true;
                }

                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                //判断是否有企业名片的个人信息
                PersonalVO pVO = cBO.FindPersonalByCustomerId(CustomerVO.CustomerId);
                if (pVO != null)
                {
                    if (pVO.BusinessID != 0)
                    {
                        BusinessCardVO bVO = cBO.FindBusinessCardById(pVO.BusinessID);
                        if (bVO != null && bVO.ExpirationAt > DateTime.Now)
                        {
                            return true;
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
            return false;
        }
        /// <summary>
        /// 获取增长百分比
        /// </summary>
        /// <returns></returns>
        public decimal getPercentage(decimal today, decimal Before)
        {
            decimal Percentage = 0;
            if (today > Before)
            {
                if (Before == 0)
                {
                    Before = 1;
                }
                Percentage = (today - Before) / Before * 100;//上周营收增加百分比
                Percentage = Math.Round(Percentage, 2);
            }
            else
            {
                if (Before == 0 && today == 0)
                {
                    Percentage = 0;
                }
                else
                {
                    if (Before == 0)
                    {
                        Before = 1;
                    }
                    Percentage = (Before - today) / Before * 100;//上周营收增加百分比
                    Percentage = -Math.Round(Percentage, 2);
                }
            }
            return Percentage;
        }

        /// <summary>
        /// 获取报名信息
        /// </summary>
        /// <param name="PartyId"></param>
        /// <returns></returns>
        public CardPartySignUpVO FindPartySignUpById(Int64 PartySignUpID)
        {
            ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(PartySignUpID);
        }

        /// <summary>
        /// 更新报名
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardPartySignUp(CardPartySignUpVO vo)
        {
            ICardPartySignUpDAO uDAO = CustomerManagementDAOFactory.CardPartySignUpDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
    }
}
