using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using SPLibrary.WebConfigInfo;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPLibrary.CoreFramework;
using SPLibrary.CustomerManagement.BO;

namespace SPLibrary.BussinessManagement.BO
{
    public class BusinessBO
    {
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();

        public BusinessBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }
        public List<BusinessViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IBusinessViewDAO uDAO = CustomerManagementDAOFactory.CreateBusinessViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindTotalCount(string condition, params object[] parameters)
        {
            IBusinessViewDAO uDAO = CustomerManagementDAOFactory.CreateBusinessViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        public int Add(BusinessVO businessVO, List<BusinessCategoryVO> businessCategoryVOList
            , List<TargetCategoryVO> targetCategoryVOList, List<TargetCityVO> targetCityVOList, List<BusinessIdcardVO> businessIdcardVOList)
        {
            try
            {
                IBusinessDAO bDAO = CustomerManagementDAOFactory.CreateBusinessDAO(this.CurrentCustomerProfile);
                IBusinessCategoryDAO bcDAO = CustomerManagementDAOFactory.CreateBusinessCategoryDAO(this.CurrentCustomerProfile);
                ITargetCategoryDAO tcDAO = CustomerManagementDAOFactory.CreateTargetCategoryDAO(this.CurrentCustomerProfile);
                ITargetCityDAO tcityDAO = CustomerManagementDAOFactory.CreateTargetCityDAO(this.CurrentCustomerProfile);
                IBusinessClientDAO bClientDAO = CustomerManagementDAOFactory.CreateBusinessClientDAO(this.CurrentCustomerProfile);
                IBusinessIdcardDAO idcDAO = CustomerManagementDAOFactory.CreateBusinessIdcardDAO(this.CurrentCustomerProfile);

                if (businessCategoryVOList == null)
                {
                    businessCategoryVOList = new List<BusinessCategoryVO>();
                }

                if (targetCategoryVOList == null)
                {
                    targetCategoryVOList = new List<TargetCategoryVO>();
                }

                if (targetCityVOList == null)
                {
                    targetCityVOList = new List<TargetCityVO>();
                }

                //if (businessClientVOList == null)
                //{
                //    businessClientVOList = new List<BusinessClientVO>();
                //}
                if (businessIdcardVOList == null)
                {
                    businessIdcardVOList = new List<BusinessIdcardVO>();
                }


                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int businessId = bDAO.Insert(businessVO);

                    foreach (BusinessCategoryVO bcVO in businessCategoryVOList)
                    {
                        bcVO.BusinessId = businessId;
                    }

                    bcDAO.InsertList(businessCategoryVOList, 100);

                    foreach (TargetCategoryVO tcVO in targetCategoryVOList)
                    {
                        tcVO.BusinessId = businessId;
                    }

                    tcDAO.InsertList(targetCategoryVOList, 100);

                    foreach (TargetCityVO tcityVO in targetCityVOList)
                    {
                        tcityVO.BusinessId = businessId;
                    }

                    tcityDAO.InsertList(targetCityVOList, 100);

                    //foreach (BusinessClientVO bClientVO in businessClientVOList)
                    //{
                    //    bClientVO.BusinessId = businessId;
                    //}

                    //bClientDAO.InsertList(businessClientVOList, 100);



                    foreach (BusinessIdcardVO idcVO in businessIdcardVOList)
                    {
                        idcVO.BusinessId = businessId;
                    }

                    idcDAO.InsertList(businessIdcardVOList, 100);

                    return businessId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public int Add(BusinessVO businessVO)
        {
            try
            {
                IBusinessDAO bDAO = CustomerManagementDAOFactory.CreateBusinessDAO(this.CurrentCustomerProfile);
                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int businessId = bDAO.Insert(businessVO);
                    return businessId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool Update(BusinessVO businessVO, List<BusinessCategoryVO> businessCategoryVOList
            , List<TargetCategoryVO> targetCategoryVOList, List<TargetCityVO> targetCityVOList, List<BusinessIdcardVO> businessIdcardVOList)
        {
            IBusinessDAO bDAO = CustomerManagementDAOFactory.CreateBusinessDAO(this.CurrentCustomerProfile);
            IBusinessCategoryDAO bcDAO = CustomerManagementDAOFactory.CreateBusinessCategoryDAO(this.CurrentCustomerProfile);
            ITargetCategoryDAO tcDAO = CustomerManagementDAOFactory.CreateTargetCategoryDAO(this.CurrentCustomerProfile);
            ITargetCityDAO tcityDAO = CustomerManagementDAOFactory.CreateTargetCityDAO(this.CurrentCustomerProfile);
            IBusinessClientDAO bClientDAO = CustomerManagementDAOFactory.CreateBusinessClientDAO(this.CurrentCustomerProfile);
            IBusinessIdcardDAO idcDAO = CustomerManagementDAOFactory.CreateBusinessIdcardDAO(this.CurrentCustomerProfile);
            try
            {
                if (businessCategoryVOList == null)
                {
                    businessCategoryVOList = new List<BusinessCategoryVO>();
                }

                if (targetCategoryVOList == null)
                {
                    targetCategoryVOList = new List<TargetCategoryVO>();
                }

                if (targetCityVOList == null)
                {
                    targetCityVOList = new List<TargetCityVO>();
                }

                //if (businessClientVOList == null)
                //{
                //    businessClientVOList = new List<BusinessClientVO>();
                //}
                if (businessIdcardVOList == null)
                {
                    businessIdcardVOList = new List<BusinessIdcardVO>();
                }
                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    bDAO.UpdateById(businessVO);

                    //删除不存在的，添加新增的
                    List<BusinessCategoryVO> bcDBVOList = bcDAO.FindByParams("BusinessId = " + businessVO.BusinessId);
                    List<BusinessCategoryVO> bcdeleteVOList = new List<BusinessCategoryVO>();
                    foreach (BusinessCategoryVO dbVO in bcDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = businessCategoryVOList.Count - 1; i >= 0; i--)
                        {
                            BusinessCategoryVO bcVO = businessCategoryVOList[i];
                            if (bcVO.CategoryId == dbVO.CategoryId)
                            {
                                businessCategoryVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            bcdeleteVOList.Add(dbVO);
                        }
                    }
                    if (businessCategoryVOList != null)
                        bcDAO.InsertList(businessCategoryVOList, 100);
                    foreach (BusinessCategoryVO deleteVO in bcdeleteVOList)
                    {
                        bcDAO.DeleteById(deleteVO.BusinessCategoryId);
                    }

                    //删除不存在的，添加新增的
                    List<TargetCategoryVO> tcDBVOList = tcDAO.FindByParams("BusinessId = " + businessVO.BusinessId);
                    List<TargetCategoryVO> tcdeleteVOList = new List<TargetCategoryVO>();
                    foreach (TargetCategoryVO dbVO in tcDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = targetCategoryVOList.Count - 1; i >= 0; i--)
                        {
                            TargetCategoryVO tcVO = targetCategoryVOList[i];
                            if (tcVO.CategoryId == dbVO.CategoryId)
                            {
                                targetCategoryVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            tcdeleteVOList.Add(dbVO);
                        }
                    }
                    if (targetCategoryVOList != null)
                        tcDAO.InsertList(targetCategoryVOList, 100);
                    foreach (TargetCategoryVO deleteVO in tcdeleteVOList)
                    {
                        tcDAO.DeleteById(deleteVO.TargetCategoryId);
                    }

                    //删除不存在的，添加新增的
                    List<TargetCityVO> tcityDBVOList = tcityDAO.FindByParams("BusinessId = " + businessVO.BusinessId);
                    List<TargetCityVO> tcitydeleteVOList = new List<TargetCityVO>();
                    foreach (TargetCityVO dbVO in tcityDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = targetCityVOList.Count - 1; i >= 0; i--)
                        {
                            TargetCityVO tcityVO = targetCityVOList[i];
                            if (tcityVO.CityId == dbVO.CityId)
                            {
                                targetCityVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            tcitydeleteVOList.Add(dbVO);
                        }
                    }
                    if (targetCityVOList != null)
                        tcityDAO.InsertList(targetCityVOList, 100);
                    foreach (TargetCityVO deleteVO in tcitydeleteVOList)
                    {
                        tcityDAO.DeleteById(deleteVO.TargetCityId);
                    }


                    ////删除不存在的，添加新增的
                    //List<BusinessClientVO> bClientDBVOList = bClientDAO.FindByParams("BusinessId = " + businessVO.BusinessId);
                    //List<BusinessClientVO> bClientdeleteVOList = new List<BusinessClientVO>();
                    //foreach (BusinessClientVO dbVO in bClientDBVOList)
                    //{
                    //    bool isDelete = true;
                    //    for (int i = businessClientVOList.Count - 1; i >= 0; i--)
                    //    {
                    //        BusinessClientVO bClientVO = businessClientVOList[i];
                    //        if (bClientVO.CompanyName == dbVO.CompanyName)
                    //        {
                    //            businessClientVOList.RemoveAt(i);
                    //            isDelete = false;
                    //            break;
                    //        }
                    //    }
                    //    if (isDelete)
                    //    {
                    //        bClientdeleteVOList.Add(dbVO);
                    //    }
                    //}
                    //if (businessClientVOList != null)
                    //    bClientDAO.InsertList(businessClientVOList, 100);
                    //foreach (BusinessClientVO deleteVO in bClientdeleteVOList)
                    //{
                    //    bClientDAO.DeleteById(deleteVO.BusinessClientId);
                    //}



                    //删除不存在的，添加新增的
                    List<BusinessIdcardVO> idCDBVOList = idcDAO.FindByParams("BusinessId = " + businessVO.BusinessId);
                    List<BusinessIdcardVO> idCdeleteVOList = new List<BusinessIdcardVO>();
                    foreach (BusinessIdcardVO dbVO in idCDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = businessIdcardVOList.Count - 1; i >= 0; i--)
                        {
                            BusinessIdcardVO idcVO = businessIdcardVOList[i];
                            if (idcVO.IDCardImg == dbVO.IDCardImg)
                            {
                                businessIdcardVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            idCdeleteVOList.Add(dbVO);
                        }
                    }
                    if (businessIdcardVOList != null)
                        idcDAO.InsertList(businessIdcardVOList, 100);
                    foreach (BusinessIdcardVO deleteVO in idCdeleteVOList)
                    {
                        idcDAO.DeleteById(deleteVO.BusinessIDCardId);
                    }

                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool Update(BusinessVO businessVO)
        {
            IBusinessDAO bDAO = CustomerManagementDAOFactory.CreateBusinessDAO(this.CurrentCustomerProfile);

            try
            {
                bDAO.UpdateById(businessVO);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public List<BusinessCategoryViewVO> FindBusinessCategoryByBusiness(int businessId)
        {
            IBusinessCategoryViewDAO bcDAO = CustomerManagementDAOFactory.CreateBusinessCategoryViewDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("BusinessId = " + businessId);
        }

        public List<TargetCategoryViewVO> FindTargetCategoryByBusiness(int businessId)
        {
            ITargetCategoryViewDAO tcDAO = CustomerManagementDAOFactory.CreateTargetCategoryViewDAO(this.CurrentCustomerProfile);
            return tcDAO.FindByParams("BusinessId = " + businessId);
        }

        public List<TargetCityViewVO> FindTargetCityByBusiness(int businessId)
        {
            ITargetCityViewDAO tcityDAO = CustomerManagementDAOFactory.CreateTargetCityViewDAO(this.CurrentCustomerProfile);
            return tcityDAO.FindByParams("BusinessId = " + businessId);
        }

        public BusinessViewVO FindBusinessById(int businessId)
        {
            IBusinessViewDAO bDAO = CustomerManagementDAOFactory.CreateBusinessViewDAO(this.CurrentCustomerProfile);
            return bDAO.FindById(businessId);
        }

        public BusinessViewVO FindBusinessByCustomerId(int customerId)
        {
            IBusinessViewDAO bDAO = CustomerManagementDAOFactory.CreateBusinessViewDAO(this.CurrentCustomerProfile);
            List<BusinessViewVO> voList = bDAO.FindByParams("CustomerId = " + customerId);
            if (voList.Count > 0)
                return voList[0];
            else
                return null;
        }

        public List<BusinessViewVO> FindBusiness()
        {
            IBusinessViewDAO bDAO = CustomerManagementDAOFactory.CreateBusinessViewDAO(this.CurrentCustomerProfile);
            List<BusinessViewVO> voList = bDAO.FindByParams("1=1");
            if (voList.Count > 0)
                return voList;
            else
                return null;
        }

        public int AddApproveHistory(BusinessApproveHistoryVO approveHistoryVO)
        {
            IBusinessApproveHistoryDAO bahDAO = CustomerManagementDAOFactory.CreateBusinessApproveHistoryDAO(this.CurrentCustomerProfile);
            return bahDAO.Insert(approveHistoryVO);
        }

        public BusinessApproveHistoryVO FindApproveHistoryByBusiness(int businessId)
        {
            IBusinessApproveHistoryDAO bahDAO = CustomerManagementDAOFactory.CreateBusinessApproveHistoryDAO(this.CurrentCustomerProfile);
            return bahDAO.FindLatestApprove(businessId);
        }

        public int AddBusinessClient(BusinessClientVO businessClientVO)
        {
            try
            {
                IBusinessClientDAO aeDAO = CustomerManagementDAOFactory.CreateBusinessClientDAO(this.CurrentCustomerProfile);

                int businessClientId = aeDAO.Insert(businessClientVO);

                return businessClientId;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool UpdateBusinessClient(BusinessClientVO businessClientVO)
        {
            try
            {
                IBusinessClientDAO aeDAO = CustomerManagementDAOFactory.CreateBusinessClientDAO(this.CurrentCustomerProfile);

                aeDAO.UpdateById(businessClientVO);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public List<BusinessClientVO> FindBusinessClientByBusiness(int businessId)
        {
            IBusinessClientDAO aeDAO = CustomerManagementDAOFactory.CreateBusinessClientDAO(this.CurrentCustomerProfile);

            List<BusinessClientVO> businessClientVOList = aeDAO.FindByParams("BusinessId = " + businessId);

            return businessClientVOList;
        }

        public List<BusinessIdcardVO> FindBusinessIdcardByBusiness(int businessId)
        {
            IBusinessIdcardDAO aeDAO = CustomerManagementDAOFactory.CreateBusinessIdcardDAO(this.CurrentCustomerProfile);

            List<BusinessIdcardVO> businessIdcardList = aeDAO.FindByParams("BusinessId = " + businessId);

            return businessIdcardList;
        }
        public BusinessClientVO FindBusinessClientById(int businessClientId)
        {
            IBusinessClientDAO aeDAO = CustomerManagementDAOFactory.CreateBusinessClientDAO(this.CurrentCustomerProfile);

            BusinessClientVO businessClientVO = aeDAO.FindById(businessClientId);

            return businessClientVO;
        }

        public bool DeleteBusinessClient(int businessClientId)
        {
            IBusinessClientDAO aeDAO = CustomerManagementDAOFactory.CreateBusinessClientDAO(this.CurrentCustomerProfile);
            try
            {
                aeDAO.DeleteById(businessClientId);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public List<AgencyViewVO> FindAllMyAgencyByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IBusinessViewDAO uDAO = CustomerManagementDAOFactory.CreateBusinessViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllMyAgencyByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindMyAgencyTotalCount(string condition, params object[] parameters)
        {
            IBusinessViewDAO uDAO = CustomerManagementDAOFactory.CreateBusinessViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindMyAgencyTotalCount(condition, parameters);
        }

        /// <summary>
        /// 获取雇主图片
        /// </summary>
        public string getBusinessIMG(int BusinessID) {
            BusinessViewVO uVO = FindBusinessById(BusinessID);
            AgencyBO aBO = new AgencyBO(new CustomerProfile());
            CustomerBO cBO = new CustomerBO(new CustomerProfile());

            string Img = ConfigInfo.Instance.NoImg;
            if (uVO != null)
            {
                string CompanyLogo = uVO.CompanyLogo;
                string HeaderLogo = "";
                string PersonalCard = "";
            
                CustomerViewVO cVO = cBO.FindById(uVO.CustomerId);
                if (cVO != null)
                {
                    if (cVO.HeaderLogo != "")
                    {
                        HeaderLogo = cVO.HeaderLogo;
                    }
                    if (cVO.AgencyId > 0)
                    {
                        AgencyViewVO aVO = aBO.FindAgencyById(cVO.AgencyId);
                        if (aVO != null)
                        {
                            if (aVO.PersonalCard != "")
                            {
                                PersonalCard = aVO.PersonalCard;
                            }
                        }
                    }
                }
                if (CompanyLogo != "")
                {
                    return CompanyLogo;
                }
                if (HeaderLogo != "")
                {
                    return HeaderLogo;
                }
                if (PersonalCard != "")
                {
                    return PersonalCard;
                }
            }           
            return Img;
        }
    }
}
