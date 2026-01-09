using CoreFramework.VO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.UserManagement.DAO;
using SPLibrary.UserManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.UserManagement.BO
{
    public class SystemBO
    {
        private UserProfile CurrentUserProfile = new UserProfile();

        public SystemBO(UserProfile userProfile)
        {
            this.CurrentUserProfile = userProfile;
        }

        public int HelpDocAdd(HelpDocVO helpVO)
        {
            try
            {
                IHelpDocDAO hdDAO = UserManagementDAOFactory.CreateHelpDocDAO(this.CurrentUserProfile);
                return hdDAO.Insert(helpVO);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(SystemBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool HelpDocUpdate(HelpDocVO helpVO)
        {
            IHelpDocDAO hdDAO = UserManagementDAOFactory.CreateHelpDocDAO(this.CurrentUserProfile);
            try
            {
                hdDAO.UpdateById(helpVO);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(SystemBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        
        public List<HelpDocViewVO> FindHelpDocAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IHelpDocViewDAO uDAO = UserManagementDAOFactory.CreateHelpDocViewDAO(this.CurrentUserProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindHelpDocTotalCount(string condition, params object[] parameters)
        {
            IHelpDocViewDAO uDAO = UserManagementDAOFactory.CreateHelpDocViewDAO(this.CurrentUserProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        public List<HelpDocTypeVO> FindHelpDocTypeList(int parentId,bool isEnable)
        {
            IHelpDocTypeDAO uDAO = UserManagementDAOFactory.CreateHelpDocTypeDAO(this.CurrentUserProfile);
            if(parentId > 0)
            return uDAO.FindByParams("ParentHelpDocTypeId = " + parentId + (isEnable ? " and Status = true" :""));
            else
                return uDAO.FindByParams("ParentHelpDocTypeId is null " + (isEnable ? " and Status = true" : ""));
        }

        public HelpDocViewVO FindHelpDocByType(int helpDocTypeId, bool isEnable)
        {
            IHelpDocViewDAO uDAO = UserManagementDAOFactory.CreateHelpDocViewDAO(this.CurrentUserProfile);
            List<HelpDocViewVO> voList = uDAO.FindByParams("HelpDocTypeId = " + helpDocTypeId + (isEnable ? " and Status = true" : ""));
            if (voList.Count > 0)
                return voList[0];
            else
                return null;
        }

        public HelpDocViewVO FindHelpDocByTypeName(string helpDocTypeName)
        {
            IHelpDocViewDAO uDAO = UserManagementDAOFactory.CreateHelpDocViewDAO(this.CurrentUserProfile);
            List<HelpDocViewVO> voList = uDAO.FindByParams("HelpDocTypeName = N'" + helpDocTypeName + "'");
            if (voList.Count > 0)
                return voList[0];
            else
                return null;
        }

        public int ConfigUpdate(ConfigVO configVO)
        {
            if (configVO == null)
                return -1;
            IConfigDAO cDAO = UserManagementDAOFactory.CreateConfigDAO(this.CurrentUserProfile);
            try
            {
                if (configVO.ConfigId < 1)
                {
                    return cDAO.Insert(configVO);
                }
                else
                {

                    cDAO.UpdateById(configVO);
                    return configVO.ConfigId;
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(SystemBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public ConfigVO FindConfig()
        {
            IConfigDAO cDAO = UserManagementDAOFactory.CreateConfigDAO(this.CurrentUserProfile);

            //List<ConfigVO> voList = cDAO.FindByParams("CompanyId = " + this.CurrentUserProfile.CompanyId);
            List<ConfigVO> voList = cDAO.FindByParams("1=1");
            if (voList.Count > 0)
                return voList[0];
            else
                return null;
        }


        public int SuggestionAdd(SuggestionVO suggestionVO)
        {
            try
            {
                ISuggestionDAO hdDAO = UserManagementDAOFactory.CreateSuggestionDAO(this.CurrentUserProfile);
                return hdDAO.Insert(suggestionVO);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(SystemBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool SuggestionDelete(int sugestionId)
        {
            ISuggestionDAO hdDAO = UserManagementDAOFactory.CreateSuggestionDAO(this.CurrentUserProfile);
            try
            {
                hdDAO.DeleteById(sugestionId);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(SystemBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        

        public List<SuggestionVO> FindSuggestionAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ISuggestionDAO hdDAO = UserManagementDAOFactory.CreateSuggestionDAO(this.CurrentUserProfile);
            return hdDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindSuggestionTotalCount(string condition, params object[] parameters)
        {
            ISuggestionDAO hdDAO = UserManagementDAOFactory.CreateSuggestionDAO(this.CurrentUserProfile);
            return hdDAO.FindTotalCount(condition, parameters);
        }        

        public SuggestionVO FindSuggestionById(int suggestionId)
        {
            ISuggestionDAO hdDAO = UserManagementDAOFactory.CreateSuggestionDAO(this.CurrentUserProfile);
            return hdDAO.FindById(suggestionId);
        }


        public CarouselVO FindCarouselById(int CarouselID)
        {
            ICarouselDAO hdDAO = UserManagementDAOFactory.CreateCarouselDAO(this.CurrentUserProfile);
            return hdDAO.FindById(CarouselID);
        }
        public int CarouselAdd(CarouselVO carouselVO)
        {
            try
            {
                ICarouselDAO hdDAO = UserManagementDAOFactory.CreateCarouselDAO(this.CurrentUserProfile);
                return hdDAO.Insert(carouselVO);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(SystemBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool CarouselUpdate(CarouselVO carouselVO)
        {
            ICarouselDAO hdDAO = UserManagementDAOFactory.CreateCarouselDAO(this.CurrentUserProfile);
            try
            {
                hdDAO.UpdateById(carouselVO);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(SystemBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool CarouselDelete(int CarouselID)
        {
            ICarouselDAO hdDAO = UserManagementDAOFactory.CreateCarouselDAO(this.CurrentUserProfile);
            try
            {
                hdDAO.DeleteById(CarouselID);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(SystemBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public List<CarouselVO> FindCarouselAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICarouselDAO hdDAO = UserManagementDAOFactory.CreateCarouselDAO(this.CurrentUserProfile);
            return hdDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int AddSystemMessage(SystemMessageVO vo)
        {
            try
            {
                ISystemMessageDAO hdDAO = UserManagementDAOFactory.CreateSystemMessageDAO(this.CurrentUserProfile);
                int systemMessageId = hdDAO.Insert(vo);

                if(systemMessageId > 0)
                {
                    //发给所有的会员
                    CustomerBO cBO = new CustomerBO(new CustomerProfile());
                    List<CustomerVO> cVOList = cBO.FindListByParams("Status = 1");
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    if(cVOList.Count>0)
                    {
                        foreach (CustomerVO cVO in cVOList)
                        {
                            MessageVO mVO = new MessageVO();
                            mVO.Title = vo.Title;
                            mVO.Message = vo.Message;
                            mVO.MessageTypeId = vo.MessageTypeId;
                            mVO.SendTo = cVO.CustomerId;
                            mVO.SendAt = DateTime.Now;
                            mVO.Status = 0;

                            mBO.AddMessage(mVO);
                        }
                    }else
                    {
                        return -1;
                    }
                    
                }
                return systemMessageId;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(SystemBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool DeleteSystemMessage(int systemMessageId)
        {
            ISystemMessageDAO hdDAO = UserManagementDAOFactory.CreateSystemMessageDAO(this.CurrentUserProfile);
            try
            {
                hdDAO.DeleteById(systemMessageId);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(SystemBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public List<SystemMessageViewVO> FindSystemMessageAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ISystemMessageViewDAO hdDAO = UserManagementDAOFactory.CreateSystemMessageViewDAO(this.CurrentUserProfile);
            return hdDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindSystemMessageTotalCount(string condition, params object[] parameters)
        {
            ISystemMessageViewDAO hdDAO = UserManagementDAOFactory.CreateSystemMessageViewDAO(this.CurrentUserProfile);
            return hdDAO.FindTotalCount(condition, parameters);
        }


        public List<PlatformCommissionViewVO> FindPlatformCommissionAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IProjectCommissionViewDAO cDAO = UserManagementDAOFactory.CreateProjectCommissionViewDAO(new UserProfile());
            return cDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindPlatformCommissionTotalCount(string condition, params object[] parameters)
        {
            IProjectCommissionViewDAO cDAO = UserManagementDAOFactory.CreateProjectCommissionViewDAO(new UserProfile());
            return cDAO.FindTotalCount(condition, parameters);
        }

        public SystemMessageViewVO FindSystemMessageById(int systemMessageId)
        {
            ISystemMessageViewDAO hdDAO = UserManagementDAOFactory.CreateSystemMessageViewDAO(this.CurrentUserProfile);
            return hdDAO.FindById(systemMessageId);
        }
            
        /// <summary>
        /// 添加平台抽佣信息
        /// </summary>
        /// <param name="commissionVO"></param>
        /// <returns></returns>
        public int AddCommission(CommissionVO commissionVO)
        {
            ICommissionDAO cDAO = UserManagementDAOFactory.CreateCommissionDAO(new UserProfile());
            return cDAO.Insert(commissionVO);
        }

        public decimal GetTotalBalance()
        {
            IBalanceDAO bDAO = CustomerManagementDAOFactory.CreateBalanceDAO(new UserProfile());
            return bDAO.GetTotalBalance();

        }
        public decimal PlatformTotalCommission()
        {
            ICommissionDAO cDAO = UserManagementDAOFactory.CreateCommissionDAO(new UserProfile());
            return cDAO.PlatformTotalCommission();
        }
    }
}
