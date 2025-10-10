using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.RequireManagement.DAO;
using SPLibrary.RequireManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SPLibrary.RequireManagement.BO
{
    public class DemandBO
    {
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();
        public DemandBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }
        public int AddDemand(DemandVO vo)//添加需求信息
        {
            try
            {
                IDemandDAO rDAO = RequireManagementDAOFactory.CreateDemandDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int demandId = rDAO.Insert(vo);
                    return demandId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(DemandBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public int AddDemandOffer(DemandOfferVO vo)//添加留言
        {
            try
            {
                IDemandOfferDAO rDAO = RequireManagementDAOFactory.CreateDemandOfferDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int demandId = rDAO.Insert(vo);
                    return demandId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(DemandBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public int FindDemandOfferCount(string condition, params object[] parameters)//获取留言数量
        {
            IDemandOfferDAO rDAO = RequireManagementDAOFactory.CreateDemandOfferDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }
        public List<DemandOfferVO> FindOfferByDemand(int DemandId)//获取留言列表
        {
            IDemandOfferDAO rDAO = RequireManagementDAOFactory.CreateDemandOfferDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams("DemandId = " + DemandId);
        }
        public bool UpdateDemand(DemandVO vo)//更新需求信息
        {
            IDemandDAO rDAO = RequireManagementDAOFactory.CreateDemandDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(DemandBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public DemandViewVO FindDemandById(int DemandId)//获取需求详情
        {
            IDemandViewDAO rDAO = RequireManagementDAOFactory.CreateDemandViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(DemandId);
        }
        public List<DemandCategoryVO> FindCategory(params object[] parameters)//获取需求分类
        {
            IDemandCategoryDAO uDAO = RequireManagementDAOFactory.CreateDemandCategoryDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex("CategoryStatus=1", 0, 0, "Order_Info", "desc", parameters);
        }

        //获取分页列表
        public List<DemandViewVO> FindDemandAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IDemandViewDAO rDAO = RequireManagementDAOFactory.CreateDemandViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }
        //获取乐聊名片商机分页列表
        public List<CardDemandViewVO> FindCardDemandViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardDemandViewDAO rDAO = CustomerManagementDAOFactory.CardDemandViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }
        public int FindCardDemandViewCount(string condition, params object[] parameters)//获取留言数量
        {
            ICardDemandViewDAO rDAO = CustomerManagementDAOFactory.CardDemandViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }
        public List<DemandViewVO> FindDemandByCustomerId(int customerId)
        {
            IDemandViewDAO rDAO = RequireManagementDAOFactory.CreateDemandViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams("Status <> 0 and Status <> 2 and TO_DAYS(EffectiveEndDate) - TO_DAYS(now()) >=0 and CustomerId = " + customerId);
        }
        //获取乐聊名片我的商机列表
        public List<CardDemandViewVO> FindCardDemandViewByCustomerId(int customerId)
        {
            ICardDemandViewDAO rDAO = CustomerManagementDAOFactory.CardDemandViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams("CustomerId = " + customerId);
        }
        //获取乐聊名片指定会员的商机列表
        public List<CardDemandViewVO> FindCardDemandViewByCustomerId2(int customerId)
        {
            ICardDemandViewDAO rDAO = CustomerManagementDAOFactory.CardDemandViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(" Status <> 0 and Status <> 2 and (TO_DAYS(EffectiveEndDate) - TO_DAYS(now()) >=0 or isEndDate=0) and " + "CustomerId = " + customerId);
        }
        //获取乐聊名片我留言的商机列表
        public List<CardOfferDemandViewVO> FindCardDemandViewByOfferCustomerId(int customerId)
        {
            ICardDemandOfferViewDAO rDAO = CustomerManagementDAOFactory.CardDemandOfferViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex("OfferCustomerId = " + customerId);
        }
        public CardDemandViewVO FindCardDemandViewById(int DemandId)//获取乐聊名片商机详情
        {
            ICardDemandViewDAO rDAO = CustomerManagementDAOFactory.CardDemandViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(DemandId);
        }
        //获取留言分页列表
        public List<DemandOfferViewVO> FindDemandOfferViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IDemandOfferDAO rDAO = RequireManagementDAOFactory.CreateDemandOfferDAO(this.CurrentCustomerProfile);
            return rDAO.FindDemandOfferViewAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }
        public string GetDemandIMG(int DemandID) {
            AgencyBO aBO = new AgencyBO(new CustomerProfile());
            BusinessBO bBO = new BusinessBO(new CustomerProfile());
            CustomerBO cBO = new CustomerBO(new CustomerProfile());
            DemandBO dBO = new DemandBO(new CustomerProfile());
            DemandViewVO dVO = dBO.FindDemandById(DemandID);
            CustomerViewVO cVO = cBO.FindById(dVO.CustomerId);
            string Img = ConfigInfo.Instance.NoImg;

            string HeaderLogo = cVO.HeaderLogo;
            string CompanyLogo = "";
            string PersonalCard = "";

            if (cVO.BusinessId > 0)
            {
                BusinessViewVO bVO = bBO.FindBusinessById(cVO.BusinessId);
                CompanyLogo = bVO.CompanyLogo;
            }

            if (cVO.AgencyId > 0)
            {
                AgencyViewVO aVO = aBO.FindAgencyById(cVO.AgencyId);
                PersonalCard = aVO.PersonalCard;
            }

            if (HeaderLogo != "")
            {
                return HeaderLogo;
            }
            if (CompanyLogo != "")
            {
                return CompanyLogo;
            }
            if (PersonalCard != "")
            {
                return PersonalCard;
            }
            return Img;
        }
    }
}
