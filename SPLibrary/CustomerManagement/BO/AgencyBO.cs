using CoreFramework.DAO;
using CoreFramework.VO;
using Newtonsoft.Json;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.BussinessManagement.BO
{
    public class AgencyBO
    {
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();

        public AgencyBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }
        public List<AgencyViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IAgencyViewDAO uDAO = CustomerManagementDAOFactory.CreateAgencyViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindTotalCount(string condition, params object[] parameters)
        {
            IAgencyViewDAO uDAO = CustomerManagementDAOFactory.CreateAgencyViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        public int Add(AgencyVO agencyVO, List<AgencyCategoryVO> agencyCategoryVOList, List<AgencyCityVO> agencyCityVOList
            , List<AgencyIdCardVO> agencyIdCardVOList, List<AgencyTechnicalVO> agencyTechnicalVOList
            , List<AgencySuperClientVO> agencySuperClientVOList, List<AgencySolutionVO> agencySolutionVOList)
        {
            try
            {
                IAgencyDAO aDAO = CustomerManagementDAOFactory.CreateAgencyDAO(this.CurrentCustomerProfile);
                IAgencyCategoryDAO bcDAO = CustomerManagementDAOFactory.CreateAgencyCategoryDAO(this.CurrentCustomerProfile);
                IAgencyCityDAO acDAO = CustomerManagementDAOFactory.CreateAgencyCityDAO(this.CurrentCustomerProfile);
                IAgencyIdCardDAO aiCDAO = CustomerManagementDAOFactory.CreateAgencyIdCardDAO(this.CurrentCustomerProfile);
                IAgencyTechnicalDAO atDAO = CustomerManagementDAOFactory.CreateAgencyTechnicalDAO(this.CurrentCustomerProfile);
                IAgencySuperClientDAO ascDAO = CustomerManagementDAOFactory.CreateAgencySuperClientDAO(this.CurrentCustomerProfile);
                IAgencySolutionDAO asDAO = CustomerManagementDAOFactory.CreateAgencySolutionDAO(this.CurrentCustomerProfile);
                IAgencySolutionFileDAO asfDAO = CustomerManagementDAOFactory.CreateAgencySolutionFileDAO(this.CurrentCustomerProfile);

                if (agencyCategoryVOList == null)
                {
                    agencyCategoryVOList = new List<AgencyCategoryVO>();
                }
                if (agencyCityVOList == null)
                {
                    agencyCityVOList = new List<AgencyCityVO>();
                }
                if (agencyIdCardVOList == null)
                {
                    agencyIdCardVOList = new List<AgencyIdCardVO>();
                }
                if (agencyTechnicalVOList == null)
                {
                    agencyTechnicalVOList = new List<AgencyTechnicalVO>();
                }
                if (agencySuperClientVOList == null)
                {
                    agencySuperClientVOList = new List<AgencySuperClientVO>();
                }
                if (agencySolutionVOList == null)
                {
                    agencySolutionVOList = new List<AgencySolutionVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int agencyId = aDAO.Insert(agencyVO);

                    foreach (AgencyCategoryVO bcVO in agencyCategoryVOList)
                    {
                        bcVO.AgencyId = agencyId;
                    }
                    bcDAO.InsertList(agencyCategoryVOList, 100);

                    foreach (AgencyCityVO bcVO in agencyCityVOList)
                    {
                        bcVO.AgencyId = agencyId;
                    }
                    acDAO.InsertList(agencyCityVOList, 100);

                    foreach (AgencyIdCardVO bcVO in agencyIdCardVOList)
                    {
                        bcVO.AgencyId = agencyId;
                    }
                    aiCDAO.InsertList(agencyIdCardVOList, 100);

                    foreach (AgencyTechnicalVO bcVO in agencyTechnicalVOList)
                    {
                        bcVO.AgencyId = agencyId;
                    }
                    atDAO.InsertList(agencyTechnicalVOList, 100);

                    foreach (AgencySuperClientVO bcVO in agencySuperClientVOList)
                    {
                        bcVO.AgencyId = agencyId;
                    }
                    ascDAO.InsertList(agencySuperClientVOList, 100);


                    foreach (AgencySolutionVO bcVO in agencySolutionVOList)
                    {
                        bcVO.AgencyId = agencyId;
                        int agencySolutionId = asDAO.Insert(bcVO);
                        if (bcVO.AgencySolutionFileList != null)
                        {
                            foreach (AgencySolutionFileVO fileVO in bcVO.AgencySolutionFileList)
                            {
                                fileVO.AgencySolutionId = agencySolutionId;
                            }
                            asfDAO.InsertList(bcVO.AgencySolutionFileList, 100);
                        }
                    }


                    return agencyId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AgencyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool Update(AgencyVO agencyVO, List<AgencyCategoryVO> agencyCategoryVOList, List<AgencyCityVO> agencyCityVOList
            , List<AgencyIdCardVO> agencyIdCardVOList, List<AgencyTechnicalVO> agencyTechnicalVOList
            , List<AgencySuperClientVO> agencySuperClientVOList, List<AgencySolutionVO> agencySolutionVOList)
        {
            try
            {
                IAgencyDAO aDAO = CustomerManagementDAOFactory.CreateAgencyDAO(this.CurrentCustomerProfile);
                IAgencyCategoryDAO bcDAO = CustomerManagementDAOFactory.CreateAgencyCategoryDAO(this.CurrentCustomerProfile);
                IAgencyCityDAO acDAO = CustomerManagementDAOFactory.CreateAgencyCityDAO(this.CurrentCustomerProfile);
                IAgencyIdCardDAO aiCDAO = CustomerManagementDAOFactory.CreateAgencyIdCardDAO(this.CurrentCustomerProfile);
                IAgencyTechnicalDAO atDAO = CustomerManagementDAOFactory.CreateAgencyTechnicalDAO(this.CurrentCustomerProfile);
                IAgencySuperClientDAO ascDAO = CustomerManagementDAOFactory.CreateAgencySuperClientDAO(this.CurrentCustomerProfile);
                IAgencySolutionDAO asDAO = CustomerManagementDAOFactory.CreateAgencySolutionDAO(this.CurrentCustomerProfile);
                IAgencySolutionFileDAO asfDAO = CustomerManagementDAOFactory.CreateAgencySolutionFileDAO(this.CurrentCustomerProfile);

                if (agencyCategoryVOList == null)
                {
                    agencyCategoryVOList = new List<AgencyCategoryVO>();
                }
                if (agencyCityVOList == null)
                {
                    agencyCityVOList = new List<AgencyCityVO>();
                }
                if (agencyIdCardVOList == null)
                {
                    agencyIdCardVOList = new List<AgencyIdCardVO>();
                }
                if (agencyTechnicalVOList == null)
                {
                    agencyTechnicalVOList = new List<AgencyTechnicalVO>();
                }
                if (agencySuperClientVOList == null)
                {
                    agencySuperClientVOList = new List<AgencySuperClientVO>();
                }
                if (agencySolutionVOList == null)
                {
                    agencySolutionVOList = new List<AgencySolutionVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    aDAO.UpdateById(agencyVO);

                    //删除不存在的，添加新增的
                    List<AgencyCategoryVO> bcDBVOList = bcDAO.FindByParams("AgencyId = " + agencyVO.AgencyId);
                    List<AgencyCategoryVO> bcdeleteVOList = new List<AgencyCategoryVO>();
                    foreach (AgencyCategoryVO dbVO in bcDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = agencyCategoryVOList.Count - 1; i >= 0; i--)
                        {
                            AgencyCategoryVO bcVO = agencyCategoryVOList[i];
                            if (bcVO.CategoryId == dbVO.CategoryId)
                            {
                                agencyCategoryVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            bcdeleteVOList.Add(dbVO);
                        }
                    }
                    if (agencyCategoryVOList != null)
                        bcDAO.InsertList(agencyCategoryVOList, 100);
                    foreach (AgencyCategoryVO deleteVO in bcdeleteVOList)
                    {
                        bcDAO.DeleteById(deleteVO.AgencyCategoryId);
                    }

                    //删除不存在的，添加新增的
                    List<AgencyCityVO> acDBVOList = acDAO.FindByParams("AgencyId = " + agencyVO.AgencyId);
                    List<AgencyCityVO> acdeleteVOList = new List<AgencyCityVO>();
                    foreach (AgencyCityVO dbVO in acDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = agencyCityVOList.Count - 1; i >= 0; i--)
                        {
                            AgencyCityVO acVO = agencyCityVOList[i];
                            if (acVO.CityId == dbVO.CityId)
                            {
                                agencyCityVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            acdeleteVOList.Add(dbVO);
                        }
                    }
                    if (agencyCityVOList != null)
                        acDAO.InsertList(agencyCityVOList, 100);
                    foreach (AgencyCityVO deleteVO in acdeleteVOList)
                    {
                        acDAO.DeleteById(deleteVO.AgencyCityId);
                    }

                    //删除不存在的，添加新增的
                    List<AgencyIdCardVO> aiCDBVOList = aiCDAO.FindByParams("AgencyId = " + agencyVO.AgencyId);
                    List<AgencyIdCardVO> aiCdeleteVOList = new List<AgencyIdCardVO>();
                    foreach (AgencyIdCardVO dbVO in aiCDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = agencyIdCardVOList.Count - 1; i >= 0; i--)
                        {
                            AgencyIdCardVO aiCVO = agencyIdCardVOList[i];
                            if (aiCVO.IDCardImg == dbVO.IDCardImg)
                            {
                                agencyIdCardVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            aiCdeleteVOList.Add(dbVO);
                        }
                    }
                    if (agencyIdCardVOList != null)
                        aiCDAO.InsertList(agencyIdCardVOList, 100);
                    foreach (AgencyIdCardVO deleteVO in aiCdeleteVOList)
                    {
                        aiCDAO.DeleteById(deleteVO.AgencyIDCardId);
                    }

                    //删除不存在的，添加新增的
                    List<AgencyTechnicalVO> atDBVOList = atDAO.FindByParams("AgencyId = " + agencyVO.AgencyId);
                    List<AgencyTechnicalVO> atdeleteVOList = new List<AgencyTechnicalVO>();
                    foreach (AgencyTechnicalVO dbVO in atDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = agencyTechnicalVOList.Count - 1; i >= 0; i--)
                        {
                            AgencyTechnicalVO atVO = agencyTechnicalVOList[i];
                            if (atVO.TechnicalImg == dbVO.TechnicalImg)
                            {
                                agencyTechnicalVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            atdeleteVOList.Add(dbVO);
                        }
                    }
                    if (agencyTechnicalVOList != null)
                        atDAO.InsertList(agencyTechnicalVOList, 100);
                    foreach (AgencyTechnicalVO deleteVO in atdeleteVOList)
                    {
                        atDAO.DeleteById(deleteVO.AgencyTechnicalId);
                    }

                    //删除不存在的，添加新增的
                    List<AgencySuperClientVO> ascDBVOList = ascDAO.FindByParams("AgencyId = " + agencyVO.AgencyId);
                    List<AgencySuperClientVO> ascdeleteVOList = new List<AgencySuperClientVO>();
                    foreach (AgencySuperClientVO dbVO in ascDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = agencySuperClientVOList.Count - 1; i >= 0; i--)
                        {
                            AgencySuperClientVO atVO = agencySuperClientVOList[i];
                            if (atVO.SuperClientName == dbVO.SuperClientName)
                            {
                                agencySuperClientVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            ascdeleteVOList.Add(dbVO);
                        }
                    }
                    if (agencySuperClientVOList != null)
                        ascDAO.InsertList(agencySuperClientVOList, 100);
                    foreach (AgencySuperClientVO deleteVO in ascdeleteVOList)
                    {
                        ascDAO.DeleteById(deleteVO.AgencySuperClientId);
                    }

                    //删除不存在的，添加新增的
                    List<AgencySolutionVO> asDBVOList = asDAO.FindByParams("AgencyId = " + agencyVO.AgencyId);
                    List<AgencySolutionVO> asdeleteVOList = new List<AgencySolutionVO>();
                    foreach (AgencySolutionVO dbVO in asDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = agencySolutionVOList.Count - 1; i >= 0; i--)
                        {
                            AgencySolutionVO atVO = agencySolutionVOList[i];
                            if (atVO.ProjectName == dbVO.ProjectName)
                            {
                                agencySolutionVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            asdeleteVOList.Add(dbVO);
                        }
                    }
                    if (agencySolutionVOList != null)
                    {
                        //asDAO.InsertList(agencySolutionVOList, 100);
                        foreach (AgencySolutionVO asVO in agencySolutionVOList)
                        {
                            int agencySolutionId = asDAO.Insert(asVO);
                            if (asVO.AgencySolutionFileList != null)
                            {
                                foreach (AgencySolutionFileVO fileVO in asVO.AgencySolutionFileList)
                                {
                                    fileVO.AgencySolutionId = agencySolutionId;
                                }
                                asfDAO.InsertList(asVO.AgencySolutionFileList, 100);
                            }
                        }
                    }
                    foreach (AgencySolutionVO deleteVO in asdeleteVOList)
                    {
                        asfDAO.DeleteByParams("AgencySolutionId = " + deleteVO.AgencySolutionId);
                        asDAO.DeleteById(deleteVO.AgencySolutionId);
                    }
                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AgencyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool Update(AgencyVO agencyVO)
        {
            IAgencyDAO bDAO = CustomerManagementDAOFactory.CreateAgencyDAO(this.CurrentCustomerProfile);

            try
            {
                bDAO.UpdateById(agencyVO);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AgencyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public List<AgencyCategoryViewVO> FindAgencyCategoryByAgency(int agencyId)
        {
            IAgencyCategoryViewDAO bcDAO = CustomerManagementDAOFactory.CreateAgencyCategoryViewDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("AgencyId = " + agencyId);
        }

        public List<AgencyCityViewVO> FindAgencyCityByAgency(int agencyId)
        {
            IAgencyCityViewDAO tcityDAO = CustomerManagementDAOFactory.CreateAgencyCityViewDAO(this.CurrentCustomerProfile);
            return tcityDAO.FindByParams("AgencyId = " + agencyId);
        }

        public List<AgencyIdCardVO> FindAgencyIdCardByAgency(int agencyId)
        {
            IAgencyIdCardDAO tcDAO = CustomerManagementDAOFactory.CreateAgencyIdCardDAO(this.CurrentCustomerProfile);
            return tcDAO.FindByParams("AgencyId = " + agencyId);
        }
        public List<AgencyTechnicalVO> FindAgencyTechnicalByAgency(int agencyId)
        {
            IAgencyTechnicalDAO tcDAO = CustomerManagementDAOFactory.CreateAgencyTechnicalDAO(this.CurrentCustomerProfile);
            return tcDAO.FindByParams("AgencyId = " + agencyId);
        }


        public AgencyViewVO FindAgencyById(int agencyId)
        {
            IAgencyViewDAO bDAO = CustomerManagementDAOFactory.CreateAgencyViewDAO(this.CurrentCustomerProfile);
            return bDAO.FindById(agencyId);
        }

        public AgencyViewVO FindAgencyByCustomerId(int customerId)
        {
            IAgencyViewDAO bDAO = CustomerManagementDAOFactory.CreateAgencyViewDAO(this.CurrentCustomerProfile);
            List<AgencyViewVO> voList = bDAO.FindByParams("CustomerId = " + customerId);
            if (voList.Count > 0)
                return voList[0];
            else
                return null;
        }

        public int AddApproveHistory(AgencyApproveHistoryVO approveHistoryVO)
        {
            IAgencyApproveHistoryDAO bahDAO = CustomerManagementDAOFactory.CreateAgencyApproveHistoryDAO(this.CurrentCustomerProfile);
            return bahDAO.Insert(approveHistoryVO);
        }

        public int AddAgencyExperience(AgencyExperienceVO agencyExperienceVO, List<AgencyExperienceImageVO> agencyExperienceVOList)
        {
            try
            {
                IAgencyExperienceDAO aeDAO = CustomerManagementDAOFactory.CreateAgencyExperienceDAO(this.CurrentCustomerProfile);
                IAgencyExperienceImageDAO aeiDAO = CustomerManagementDAOFactory.CreateAgencyExperienceImageDAO(this.CurrentCustomerProfile);

                if (agencyExperienceVOList == null)
                {
                    agencyExperienceVOList = new List<AgencyExperienceImageVO>();
                }


                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int agencyExperienceId = aeDAO.Insert(agencyExperienceVO);

                    foreach (AgencyExperienceImageVO bcVO in agencyExperienceVOList)
                    {
                        bcVO.AgencyExperienceId = agencyExperienceId;
                    }
                    aeiDAO.InsertList(agencyExperienceVOList, 100);

                    return agencyExperienceId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AgencyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool UpdateAgencyExperience(AgencyExperienceVO agencyExperienceVO, List<AgencyExperienceImageVO> agencyExperienceVOList)
        {
            try
            {
                IAgencyExperienceDAO aeDAO = CustomerManagementDAOFactory.CreateAgencyExperienceDAO(this.CurrentCustomerProfile);
                IAgencyExperienceImageDAO aeiDAO = CustomerManagementDAOFactory.CreateAgencyExperienceImageDAO(this.CurrentCustomerProfile);

                if (agencyExperienceVOList == null)
                {
                    agencyExperienceVOList = new List<AgencyExperienceImageVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    aeDAO.UpdateById(agencyExperienceVO);

                    //删除不存在的，添加新增的
                    List<AgencyExperienceImageVO> bcDBVOList = aeiDAO.FindByParams("AgencyExperienceId = " + agencyExperienceVO.AgencyExperienceId);
                    List<AgencyExperienceImageVO> bcdeleteVOList = new List<AgencyExperienceImageVO>();
                    foreach (AgencyExperienceImageVO dbVO in bcDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = agencyExperienceVOList.Count - 1; i >= 0; i--)
                        {
                            AgencyExperienceImageVO bcVO = agencyExperienceVOList[i];
                            if (bcVO.FileName == dbVO.FileName)
                            {
                                agencyExperienceVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            bcdeleteVOList.Add(dbVO);
                        }
                    }
                    if (agencyExperienceVOList != null)
                        aeiDAO.InsertList(agencyExperienceVOList, 100);
                    foreach (AgencyExperienceImageVO deleteVO in bcdeleteVOList)
                    {
                        aeiDAO.DeleteById(deleteVO.AgencyExperienceImageId);
                    }

                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AgencyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool UpdateAgencyExperience(AgencyExperienceVO agencyExperienceVO)
        {
            try
            {
                IAgencyExperienceDAO aeDAO = CustomerManagementDAOFactory.CreateAgencyExperienceDAO(this.CurrentCustomerProfile);

                aeDAO.UpdateById(agencyExperienceVO);

                return true;

            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AgencyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public List<AgencyExperienceViewVO> FindAgencyExperienceByAgency(int agencyId)
        {
            IAgencyExperienceViewDAO aeDAO = CustomerManagementDAOFactory.CreateAgencyExperienceViewDAO(this.CurrentCustomerProfile);
            IAgencyExperienceImageDAO aeiDAO = CustomerManagementDAOFactory.CreateAgencyExperienceImageDAO(this.CurrentCustomerProfile);

            List<AgencyExperienceViewVO> agencyExperienceVOList = aeDAO.FindByParams("AgencyId = " + agencyId);
            if (agencyExperienceVOList != null)
            {
                foreach (AgencyExperienceViewVO vo in agencyExperienceVOList)
                {
                    vo.AgencyExperienceImageList = aeiDAO.FindByParams("AgencyExperienceId = " + vo.AgencyExperienceId);
                }
                return agencyExperienceVOList;
            }
            else
                return null;

        }

        public AgencyExperienceViewVO FindAgencyExperienceById(int agencyExperienceyId)
        {
            IAgencyExperienceViewDAO aeDAO = CustomerManagementDAOFactory.CreateAgencyExperienceViewDAO(this.CurrentCustomerProfile);
            IAgencyExperienceImageDAO aeiDAO = CustomerManagementDAOFactory.CreateAgencyExperienceImageDAO(this.CurrentCustomerProfile);

            List<AgencyExperienceViewVO> agencyExperienceVOList = aeDAO.FindByParams("AgencyExperienceId = " + agencyExperienceyId);
            if (agencyExperienceVOList != null)
            {
                foreach (AgencyExperienceViewVO vo in agencyExperienceVOList)
                {
                    vo.AgencyExperienceImageList = aeiDAO.FindByParams("AgencyExperienceId = " + vo.AgencyExperienceId);
                }
                return agencyExperienceVOList[0];
            }
            else
                return null;

        }

        public bool DeleteAgencyExperience(int agencyExperienceId)
        {
            IAgencyExperienceDAO aeDAO = CustomerManagementDAOFactory.CreateAgencyExperienceDAO(this.CurrentCustomerProfile);
            IAgencyExperienceImageDAO aeiDAO = CustomerManagementDAOFactory.CreateAgencyExperienceImageDAO(this.CurrentCustomerProfile);

            try
            {
                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {

                    aeiDAO.DeleteByParams("AgencyExperienceId = " + agencyExperienceId);

                    aeDAO.DeleteById(agencyExperienceId);

                    //删除图片

                };
                int result = t.Go();

                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AgencyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public List<AgencyExperienceViewVO> FindAllExperienceByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IAgencyExperienceViewDAO aeDAO = CustomerManagementDAOFactory.CreateAgencyExperienceViewDAO(this.CurrentCustomerProfile);
            return aeDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindExperienceTotalCount(string condition, params object[] parameters)
        {
            IAgencyExperienceViewDAO aeDAO = CustomerManagementDAOFactory.CreateAgencyExperienceViewDAO(this.CurrentCustomerProfile);
            return aeDAO.FindTotalCount(condition, parameters);
        }

        public int AddExperienceApproveHistory(AgencyExperienceApproveHistoryVO approveExperienceHistoryVO)
        {
            IAgencyExperienceApproveHistoryDAO bahDAO = CustomerManagementDAOFactory.CreateAgencyExperienceApproveHistoryDAO(this.CurrentCustomerProfile);
            return bahDAO.Insert(approveExperienceHistoryVO);
        }

        public AgencyExperienceApproveHistoryVO FindExperienceApproveHistoryByAgencyExperience(int agencyExperienceId)
        {
            IAgencyExperienceApproveHistoryDAO bahDAO = CustomerManagementDAOFactory.CreateAgencyExperienceApproveHistoryDAO(this.CurrentCustomerProfile);
            return bahDAO.FindLatestApprove(agencyExperienceId);
        }

        public AgencyApproveHistoryVO FindApproveHistoryByAgency(int agencyId)
        {
            IAgencyApproveHistoryDAO bahDAO = CustomerManagementDAOFactory.CreateAgencyApproveHistoryDAO(this.CurrentCustomerProfile);
            return bahDAO.FindLatestApprove(agencyId);
        }


        public int AddAgencySuperClient(AgencySuperClientVO agencySuperClientVO)
        {
            try
            {
                IAgencySuperClientDAO aeDAO = CustomerManagementDAOFactory.CreateAgencySuperClientDAO(this.CurrentCustomerProfile);

                int agencySuperClientId = aeDAO.Insert(agencySuperClientVO);

                return agencySuperClientId;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AgencyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool UpdateAgencySuperClient(AgencySuperClientVO agencySuperClientVO)
        {
            try
            {
                IAgencySuperClientDAO aeDAO = CustomerManagementDAOFactory.CreateAgencySuperClientDAO(this.CurrentCustomerProfile);

                aeDAO.UpdateById(agencySuperClientVO);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AgencyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public List<AgencySuperClientVO> FindAgencySuperClientByAgency(int agencyId)
        {
            IAgencySuperClientDAO aeDAO = CustomerManagementDAOFactory.CreateAgencySuperClientDAO(this.CurrentCustomerProfile);

            List<AgencySuperClientVO> agencySuperClientVOList = aeDAO.FindByParams("AgencyId = " + agencyId);

            return agencySuperClientVOList;
        }

        public AgencySuperClientVO FindAgencySuperClientById(int agencySuperClientId)
        {
            IAgencySuperClientDAO aeDAO = CustomerManagementDAOFactory.CreateAgencySuperClientDAO(this.CurrentCustomerProfile);

            AgencySuperClientVO agencySuperClientVO = aeDAO.FindById(agencySuperClientId);

            return agencySuperClientVO;
        }

        public bool DeleteAgencySuperClient(int agencySuperClientId)
        {
            IAgencySuperClientDAO aeDAO = CustomerManagementDAOFactory.CreateAgencySuperClientDAO(this.CurrentCustomerProfile);
            try
            {
                aeDAO.DeleteById(agencySuperClientId);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AgencyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public int AddAgencySolution(AgencySolutionVO agencySolutionVO, List<AgencySolutionFileVO> agencySolutionFileVOList)
        {
            try
            {
                IAgencySolutionDAO aeDAO = CustomerManagementDAOFactory.CreateAgencySolutionDAO(this.CurrentCustomerProfile);
                IAgencySolutionFileDAO aeiDAO = CustomerManagementDAOFactory.CreateAgencySolutionFileDAO(this.CurrentCustomerProfile);

                if (agencySolutionFileVOList == null)
                {
                    agencySolutionFileVOList = new List<AgencySolutionFileVO>();
                }


                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int agencySolutionId = aeDAO.Insert(agencySolutionVO);

                    foreach (AgencySolutionFileVO bcVO in agencySolutionFileVOList)
                    {
                        bcVO.AgencySolutionId = agencySolutionId;
                    }
                    aeiDAO.InsertList(agencySolutionFileVOList, 100);

                    return agencySolutionId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AgencyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool UpdateAgencySolution(AgencySolutionVO agencySolutionVO, List<AgencySolutionFileVO> agencySolutionFileVOList)
        {
            try
            {
                IAgencySolutionDAO aeDAO = CustomerManagementDAOFactory.CreateAgencySolutionDAO(this.CurrentCustomerProfile);
                IAgencySolutionFileDAO aeiDAO = CustomerManagementDAOFactory.CreateAgencySolutionFileDAO(this.CurrentCustomerProfile);

                if (agencySolutionFileVOList == null)
                {
                    agencySolutionFileVOList = new List<AgencySolutionFileVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    aeDAO.UpdateById(agencySolutionVO);

                    //删除不存在的，添加新增的
                    List<AgencySolutionFileVO> bcDBVOList = aeiDAO.FindByParams("AgencySolutionId = " + agencySolutionVO.AgencySolutionId);
                    List<AgencySolutionFileVO> bcdeleteVOList = new List<AgencySolutionFileVO>();
                    foreach (AgencySolutionFileVO dbVO in bcDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = agencySolutionFileVOList.Count - 1; i >= 0; i--)
                        {
                            AgencySolutionFileVO bcVO = agencySolutionFileVOList[i];
                            if (bcVO.FilePath == dbVO.FilePath)
                            {
                                agencySolutionFileVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            bcdeleteVOList.Add(dbVO);
                        }
                    }
                    if (agencySolutionFileVOList != null)
                        aeiDAO.InsertList(agencySolutionFileVOList, 100);
                    foreach (AgencySolutionFileVO deleteVO in bcdeleteVOList)
                    {
                        aeiDAO.DeleteById(deleteVO.AgencySolutionFileId);
                    }

                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AgencyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public List<AgencySolutionVO> FindAgencySolutionByAgency(int agencyId)
        {
            IAgencySolutionDAO aeDAO = CustomerManagementDAOFactory.CreateAgencySolutionDAO(this.CurrentCustomerProfile);
            IAgencySolutionFileDAO aeiDAO = CustomerManagementDAOFactory.CreateAgencySolutionFileDAO(this.CurrentCustomerProfile);

            List<AgencySolutionVO> agencySolutionVOList = aeDAO.FindByParams("AgencyId = " + agencyId);
            if (agencySolutionVOList != null)
            {
                foreach (AgencySolutionVO vo in agencySolutionVOList)
                {
                    vo.AgencySolutionFileList = aeiDAO.FindByParams("AgencySolutionId = " + vo.AgencySolutionId);
                }
                return agencySolutionVOList;
            }
            else
                return null;

        }

        public AgencySolutionVO FindAgencySolutionById(int agencySolutionyId)
        {
            IAgencySolutionDAO aeDAO = CustomerManagementDAOFactory.CreateAgencySolutionDAO(this.CurrentCustomerProfile);
            IAgencySolutionFileDAO aeiDAO = CustomerManagementDAOFactory.CreateAgencySolutionFileDAO(this.CurrentCustomerProfile);

            AgencySolutionVO agencySolutionVO = aeDAO.FindById(agencySolutionyId);
            if (agencySolutionVO != null)
            {

                agencySolutionVO.AgencySolutionFileList = aeiDAO.FindByParams("AgencySolutionId = " + agencySolutionVO.AgencySolutionId);

                return agencySolutionVO;
            }
            else
                return null;

        }

        public bool DeleteAgencySolution(int agencySolutionId)
        {
            IAgencySolutionDAO aeDAO = CustomerManagementDAOFactory.CreateAgencySolutionDAO(this.CurrentCustomerProfile);
            IAgencySolutionFileDAO aeiDAO = CustomerManagementDAOFactory.CreateAgencySolutionFileDAO(this.CurrentCustomerProfile);

            try
            {
                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {

                    aeiDAO.DeleteByParams("AgencySolutionId = " + agencySolutionId);

                    aeDAO.DeleteById(agencySolutionId);

                };
                int result = t.Go();

                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AgencyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }


        public List<BusinessViewVO> FindAllMyBusinessByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IAgencyViewDAO uDAO = CustomerManagementDAOFactory.CreateAgencyViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllMyBusinessByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindMyBusinessTotalCount(string condition, params object[] parameters)
        {
            IAgencyViewDAO uDAO = CustomerManagementDAOFactory.CreateAgencyViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindMyBusinessTotalCount(condition, parameters);
        }

        public string FindAgencyCategoryIds(int agencyId)
        {
            IAgencyCategoryDAO acDAO = CustomerManagementDAOFactory.CreateAgencyCategoryDAO(new UserProfile());

            return acDAO.FindAgencyCategoryIds(agencyId);
        }

        public List<AgencyViewVO> FindMatchAgencyByPageIndex(int requireId, string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IAgencyViewDAO uDAO = CustomerManagementDAOFactory.CreateAgencyViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindMatchAgencyByPageIndex(requireId, conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindMatchAgencyTotalCount(int requireId, string condition, params object[] parameters)
        {
            IAgencyViewDAO uDAO = CustomerManagementDAOFactory.CreateAgencyViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindMatchAgencyTotalCount(requireId, condition, parameters);
        }

        /// <summary>
        /// 获取销售图片
        /// </summary>
        public string getAgencyIMG(int AgencyID)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            AgencyViewVO uVO = uBO.FindAgencyById(AgencyID);
            BusinessBO bBO = new BusinessBO(new CustomerProfile());
            CustomerBO cBO = new CustomerBO(new CustomerProfile());
            string Img = ConfigInfo.Instance.NoImg;

            if (uVO != null)
            {
                string PersonalCard = uVO.PersonalCard;
                string CompanyLogo = "";
                string HeaderLogo = uVO.HeaderLogo;
                

                CustomerViewVO cVO = cBO.FindById(uVO.CustomerId);
                if (cVO != null)
                {
                    if (cVO.BusinessId > 0)
                    {
                        BusinessViewVO bVO = bBO.FindBusinessById(cVO.BusinessId);
                        if (bVO != null)
                        {
                            if (bVO.CompanyLogo != "")
                            {
                                CompanyLogo = bVO.CompanyLogo;
                            }
                        }
                    }
                }

                if (PersonalCard != "")
                {
                    return PersonalCard;
                }
                else if (CompanyLogo != "")
                {
                    return CompanyLogo;
                }
                else if (HeaderLogo != "")
                {
                    return HeaderLogo;
                }
            }
            return Img;
        }

        /// <summary>
        /// 获取销售二维码
        /// </summary>
        /// <param name="AgencyId"></param>
        /// <returns></returns>
        public string GetAgencyQR(int AgencyId)
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wxd90e86e2ec343eae&secret=58ce21e763870071474c8ee531013e7e";
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
            DataJson += "\"scene\":\"" + AgencyId + "\",";
            DataJson += string.Format("\"width\":{0},", 430);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", "pages/Agency_show/Agency_show");//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
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
            string folder = "/UploadFolder/AgencyQRFile/";
            string newFileName = AgencyId + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string Cardimg = ConfigInfo.Instance.APIURL + filePath;

            AgencyVO aVO = new AgencyVO();
            aVO.AgencyId = AgencyId;
            aVO.QRCodeImg = Cardimg;
            Update(aVO);

            return Cardimg;
        }
    }
}
