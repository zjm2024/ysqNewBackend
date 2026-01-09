using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.RequireManagement.DAO;
using SPLibrary.RequireManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.RequireManagement.BO
{
    public class ServicesBO
    {
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();

        public ServicesBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }
        
        public int AddServices(ServicesVO vo,List<ServicesCategoryVO> targetCategoryVOList)
        {
            try
            {
                IServicesDAO rDAO = RequireManagementDAOFactory.CreateServicesDAO(this.CurrentCustomerProfile);
                IServicesCategoryDAO rtcDAO = RequireManagementDAOFactory.CreateServicesCategoryDAO(this.CurrentCustomerProfile);
                
                
                if (targetCategoryVOList == null)
                {
                    targetCategoryVOList = new List<ServicesCategoryVO>();
                }
               


                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int servicesId = rDAO.Insert(vo);

                    foreach (ServicesCategoryVO bcVO in targetCategoryVOList)
                    {
                        bcVO.ServicesId = servicesId;
                    }

                    rtcDAO.InsertList(targetCategoryVOList, 100);
                    
                    return servicesId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ServicesBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool UpdateServices(ServicesVO vo, List<ServicesCategoryVO> targetCategoryVOList)
        {
            try
            {
                IServicesDAO rDAO = RequireManagementDAOFactory.CreateServicesDAO(this.CurrentCustomerProfile);
                IServicesCategoryDAO rtcDAO = RequireManagementDAOFactory.CreateServicesCategoryDAO(this.CurrentCustomerProfile);


                if (targetCategoryVOList == null)
                {
                    targetCategoryVOList = new List<ServicesCategoryVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    rDAO.UpdateById(vo);

                    //删除不存在的，添加新增的
                    List<ServicesCategoryVO> rtcDBVOList = rtcDAO.FindByParams("ServicesId = " + vo.ServicesId);
                    List<ServicesCategoryVO> rtcdeleteVOList = new List<ServicesCategoryVO>();
                    foreach (ServicesCategoryVO dbVO in rtcDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = targetCategoryVOList.Count - 1; i >= 0; i--)
                        {
                            ServicesCategoryVO bcVO = targetCategoryVOList[i];
                            if (bcVO.CategoryId == dbVO.CategoryId)
                            {
                                targetCategoryVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            rtcdeleteVOList.Add(dbVO);
                        }
                    }
                    if (targetCategoryVOList != null)
                        rtcDAO.InsertList(targetCategoryVOList, 100);
                    foreach (ServicesCategoryVO deleteVO in rtcdeleteVOList)
                    {
                        rtcDAO.DeleteById(deleteVO.ServicesCategoryId);
                    }                  

                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ServicesBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool UpdateServices(ServicesVO vo)
        {
            IServicesDAO rDAO = RequireManagementDAOFactory.CreateServicesDAO(this.CurrentCustomerProfile);

            try
            {
                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ServicesBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }


        public ServicesViewVO FindServicesById(int servicesId)
        {
            IServicesViewDAO rDAO = RequireManagementDAOFactory.CreateServicesViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(servicesId);
        }

        public List<ServicesViewVO> FindServicesAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IServicesViewDAO rDAO = RequireManagementDAOFactory.CreateServicesViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindServicesTotalCount(string condition, params object[] parameters)
        {
            IServicesViewDAO rDAO = RequireManagementDAOFactory.CreateServicesViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }        

        public string GetServicesCode()
        {
            var customerCode = new StringBuilder();
            for (var i = 0; i < 10; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                customerCode.Append(r.Next(0, 10));
            }
            return "S" + customerCode.ToString();
        }

        public List<ServicesCategoryViewVO> FindTargetCategoryByServices(int servicesId)
        {
            IServicesCategoryViewDAO bcDAO = RequireManagementDAOFactory.CreateServicesCategoryViewDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("ServicesId = " + servicesId);
        } 
    }
}
