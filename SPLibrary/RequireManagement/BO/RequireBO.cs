using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.RequireManagement.DAO;
using SPLibrary.RequireManagement.VO;
using System;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.BO;
using SPLibrary.ProjectManagement.VO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.CoreFramework.BO;
using SPLibrary.WebConfigInfo;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;

namespace SPLibrary.RequireManagement.BO
{
    public class RequireBO
    {
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();

        public RequireBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }
        
        public int AddRequirement(RequirementVO vo,List<RequirementTargetCategoryVO> targetCategoryVOList,List<RequirementTargetCityVO> targetCityVOList
            ,List<RequirementFileVO> fileVOList, List<RequirementTargetClientVO> requireClientVOList)
        {
            try
            {
                IRequirementDAO rDAO = RequireManagementDAOFactory.CreateRequirementDAO(this.CurrentCustomerProfile);
                IRequirementTargetCategoryDAO rtcDAO = RequireManagementDAOFactory.CreateRequirementTargetCategoryDAO(this.CurrentCustomerProfile);
                IRequirementTargetCityDAO rtcityDAO = RequireManagementDAOFactory.CreateRequirementTargetCityDAO(this.CurrentCustomerProfile);
                IRequirementFileDAO rfDAO = RequireManagementDAOFactory.CreateRequirementFileDAO(this.CurrentCustomerProfile);
                IRequirementTargetClientDAO rtclientDAO = RequireManagementDAOFactory.CreateRequirementTargetClientDAO(this.CurrentCustomerProfile);
                
                if (targetCategoryVOList == null)
                {
                    targetCategoryVOList = new List<RequirementTargetCategoryVO>();
                }

                if (targetCityVOList == null)
                {
                    targetCityVOList = new List<RequirementTargetCityVO>();
                }

                if (fileVOList == null)
                {
                    fileVOList = new List<RequirementFileVO>();
                }

                if (requireClientVOList == null)
                {
                    requireClientVOList = new List<RequirementTargetClientVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int requireId = rDAO.Insert(vo);

                    foreach (RequirementTargetCategoryVO bcVO in targetCategoryVOList)
                    {
                        bcVO.RequirementId = requireId;
                    }

                    rtcDAO.InsertList(targetCategoryVOList, 100);

                    foreach (RequirementTargetCityVO tcVO in targetCityVOList)
                    {
                        tcVO.RequirementId = requireId;
                    }

                    rtcityDAO.InsertList(targetCityVOList, 100);

                    foreach (RequirementFileVO tcityVO in fileVOList)
                    {
                        tcityVO.RequirementId = requireId;
                    }

                    rfDAO.InsertList(fileVOList, 100);

                    foreach (RequirementTargetClientVO bClientVO in requireClientVOList)
                    {
                        bClientVO.RequirementId = requireId;
                    }

                    rtclientDAO.InsertList(requireClientVOList, 100);

                    return requireId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(RequireBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool UpdateRequirement(RequirementVO vo, List<RequirementTargetCategoryVO> targetCategoryVOList, List<RequirementTargetCityVO> targetCityVOList
            , List<RequirementFileVO> fileVOList, List<RequirementTargetClientVO> requireClientVOList)
        {
            try
            {
                IRequirementDAO rDAO = RequireManagementDAOFactory.CreateRequirementDAO(this.CurrentCustomerProfile);
                IRequirementTargetCategoryDAO rtcDAO = RequireManagementDAOFactory.CreateRequirementTargetCategoryDAO(this.CurrentCustomerProfile);
                IRequirementTargetCityDAO rtcityDAO = RequireManagementDAOFactory.CreateRequirementTargetCityDAO(this.CurrentCustomerProfile);
                IRequirementFileDAO rfDAO = RequireManagementDAOFactory.CreateRequirementFileDAO(this.CurrentCustomerProfile);
                IRequirementTargetClientDAO rtclientDAO = RequireManagementDAOFactory.CreateRequirementTargetClientDAO(this.CurrentCustomerProfile);

                if (targetCategoryVOList == null)
                {
                    targetCategoryVOList = new List<RequirementTargetCategoryVO>();
                }

                if (targetCityVOList == null)
                {
                    targetCityVOList = new List<RequirementTargetCityVO>();
                }

                if (fileVOList == null)
                {
                    fileVOList = new List<RequirementFileVO>();
                }

                if (requireClientVOList == null)
                {
                    requireClientVOList = new List<RequirementTargetClientVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    rDAO.UpdateById(vo);

                    //删除不存在的，添加新增的
                    List<RequirementTargetCategoryVO> rtcDBVOList = rtcDAO.FindByParams("RequirementId = " + vo.RequirementId);
                    List<RequirementTargetCategoryVO> rtcdeleteVOList = new List<RequirementTargetCategoryVO>();
                    foreach (RequirementTargetCategoryVO dbVO in rtcDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = targetCategoryVOList.Count - 1; i >= 0; i--)
                        {
                            RequirementTargetCategoryVO bcVO = targetCategoryVOList[i];
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
                    foreach (RequirementTargetCategoryVO deleteVO in rtcdeleteVOList)
                    {
                        rtcDAO.DeleteById(deleteVO.RequirementTargetCategoryId);
                    }

                    //删除不存在的，添加新增的
                    List<RequirementTargetCityVO> rtcityDBVOList = rtcityDAO.FindByParams("RequirementId = " + vo.RequirementId);
                    List<RequirementTargetCityVO> rtcitydeleteVOList = new List<RequirementTargetCityVO>();
                    foreach (RequirementTargetCityVO dbVO in rtcityDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = targetCityVOList.Count - 1; i >= 0; i--)
                        {
                            RequirementTargetCityVO tcVO = targetCityVOList[i];
                            if (tcVO.CityId == dbVO.CityId)
                            {
                                targetCityVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            rtcitydeleteVOList.Add(dbVO);
                        }
                    }
                    if (targetCityVOList != null)
                        rtcityDAO.InsertList(targetCityVOList, 100);
                    foreach (RequirementTargetCityVO deleteVO in rtcitydeleteVOList)
                    {
                        rtcityDAO.DeleteById(deleteVO.RequirementTargetCityId);
                    }

                    //删除不存在的，添加新增的
                    List<RequirementFileVO> rfDBVOList = rfDAO.FindByParams("RequirementId = " + vo.RequirementId);
                    List<RequirementFileVO> rfdeleteVOList = new List<RequirementFileVO>();
                    foreach (RequirementFileVO dbVO in rfDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = fileVOList.Count - 1; i >= 0; i--)
                        {
                            RequirementFileVO rfVO = fileVOList[i];
                            if (rfVO.FilePath == dbVO.FilePath)
                            {
                                fileVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            rfdeleteVOList.Add(dbVO);
                        }
                    }
                    if (fileVOList != null)
                        rfDAO.InsertList(fileVOList, 100);
                    foreach (RequirementFileVO deleteVO in rfdeleteVOList)
                    {
                        rfDAO.DeleteById(deleteVO.RequirementFileId);
                    }

                    //删除不存在的，添加新增的
                    List<RequirementTargetClientVO> bClientDBVOList = rtclientDAO.FindByParams("RequirementId = " + vo.RequirementId);
                    List<RequirementTargetClientVO> bClientdeleteVOList = new List<RequirementTargetClientVO>();
                    foreach (RequirementTargetClientVO dbVO in bClientDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = requireClientVOList.Count - 1; i >= 0; i--)
                        {
                            RequirementTargetClientVO bClientVO = requireClientVOList[i];
                            if (bClientVO.CompanyName == dbVO.CompanyName)
                            {
                                requireClientVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            bClientdeleteVOList.Add(dbVO);
                        }
                    }
                    if (requireClientVOList != null)
                        rtclientDAO.InsertList(requireClientVOList, 100);
                    foreach (RequirementTargetClientVO deleteVO in bClientdeleteVOList)
                    {
                        rtclientDAO.DeleteById(deleteVO.RequireClientId);
                    }

                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(RequireBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool UpdateRequirement(RequirementVO vo)
        {
            IRequirementDAO rDAO = RequireManagementDAOFactory.CreateRequirementDAO(this.CurrentCustomerProfile);

            try
            {
                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(RequireBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }


        public RequirementViewVO FindRequireById(int requirementId)
        {
            IRequirementViewDAO rDAO = RequireManagementDAOFactory.CreateRequirementViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(requirementId);
        }

        public List<RequirementViewVO> FindRequireAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IRequirementViewDAO rDAO = RequireManagementDAOFactory.CreateRequirementViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindRequireSumByCondtion(string Sum, string condtion)
        {
            IRequirementViewDAO rDAO = RequireManagementDAOFactory.CreateRequirementViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalSum(Sum, condtion);
        }

        public int FindRequireTotalCount(string condition, params object[] parameters)
        {
            IRequirementViewDAO rDAO = RequireManagementDAOFactory.CreateRequirementViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }        

        public string GetRequireCode()
        {
            var customerCode = new StringBuilder();
            for (var i = 0; i < 10; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                customerCode.Append(r.Next(0, 10));
            }
            return "R" + customerCode.ToString();
        }

        public List<RequirementTargetCategoryViewVO> FindTargetCategoryByRequire(int requireId)
        {
            IRequirementTargetCategoryViewDAO bcDAO = RequireManagementDAOFactory.CreateRequirementTargetCategoryViewDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("RequirementId = " + requireId);
        }

        public List<RequirementTargetCityViewVO> FindTargetCityByRequire(int requireId)
        {
            IRequirementTargetCityViewDAO bcDAO = RequireManagementDAOFactory.CreateRequirementTargetCityViewDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("RequirementId = " + requireId);
        }

        public List<RequirementFileVO> FindRequireFileByRequire(int requireId)
        {
            IRequirementFileDAO bcDAO = RequireManagementDAOFactory.CreateRequirementFileDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("RequirementId = " + requireId);
        }


        public List<TenderInfoViewVO> FindTenderInfoByRequire(int requireId)
        {
            ITenderInfoViewDAO bcDAO = RequireManagementDAOFactory.CreateTenderInfoViewDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("RequirementId = " + requireId);
        }

        public int AddTenderInfo(TenderInfoVO tenderInfoVO)
        {
            ITenderInfoDAO bcDAO = RequireManagementDAOFactory.CreateTenderInfoDAO(this.CurrentCustomerProfile);
            return bcDAO.Insert(tenderInfoVO);
        }

        public int DeleteTenderInfo(int customerId, int requireId)
        {
            ITenderInfoDAO bcDAO = RequireManagementDAOFactory.CreateTenderInfoDAO(this.CurrentCustomerProfile);
            try
            {
                bcDAO.DeleteByParams("CustomerId = " + customerId + " and RequirementId = " + requireId);
                return 1;
            }catch
            {
                return -1;
            }
        }

        public List<TenderInviteViewVO> FindTenderInviteByRequire(int requireId)
        {
            ITenderInviteViewDAO bcDAO = RequireManagementDAOFactory.CreateTenderInviteViewDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("RequirementId = " + requireId);
        }

        public bool IsAlreadyTenderInfo(TenderInfoVO tenderInfoVO)
        {
            ITenderInfoDAO bcDAO = RequireManagementDAOFactory.CreateTenderInfoDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("RequirementId = " + tenderInfoVO.RequirementId + " and CustomerId = " + tenderInfoVO.CustomerId).Count > 0;
        }

        public bool IsAlreadyTenderInvite(TenderInviteVO tenderInviteVO)
        {
            ITenderInviteDAO bcDAO = RequireManagementDAOFactory.CreateTenderInviteDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("RequirementId = " + tenderInviteVO.RequirementId + " and CustomerId = " + tenderInviteVO.CustomerId).Count > 0;
        }

        public int AddTenderInvite(TenderInviteVO tenderInviteVO)
        {
            ITenderInviteDAO bcDAO = RequireManagementDAOFactory.CreateTenderInviteDAO(this.CurrentCustomerProfile);
            return bcDAO.Insert(tenderInviteVO);
        }

        public List<TenderInfoViewVO> FindTenderInfoAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ITenderInfoViewDAO rDAO = RequireManagementDAOFactory.CreateTenderInfoViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindTenderInfoTotalCount(string condition, params object[] parameters)
        {
            ITenderInfoViewDAO rDAO = RequireManagementDAOFactory.CreateTenderInfoViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }

        public List<TenderInviteViewVO> FindTenderInviteAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ITenderInviteViewDAO rDAO = RequireManagementDAOFactory.CreateTenderInviteViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindTenderInviteTotalCount(string condition, params object[] parameters)
        {
            ITenderInviteViewDAO rDAO = RequireManagementDAOFactory.CreateTenderInviteViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }


        public List<TenderInfoRequirementViewVO> FindTenderInfoRequirementAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ITenderInfoRequirementViewDAO rDAO = RequireManagementDAOFactory.CreateTenderInfoRequirementViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindTenderInfoRequirementTotalCount(string condition, params object[] parameters)
        {
            ITenderInfoRequirementViewDAO rDAO = RequireManagementDAOFactory.CreateTenderInfoRequirementViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }

        public List<TenderInviteRequirementViewVO> FindTenderInviteRequirementAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ITenderInviteRequirementViewDAO rDAO = RequireManagementDAOFactory.CreateTenderInviteRequirementViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindTenderInviteRequirementTotalCount(string condition, params object[] parameters)
        {
            ITenderInviteRequirementViewDAO rDAO = RequireManagementDAOFactory.CreateTenderInviteRequirementViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }


        public List<RequirementViewVO> FindRequireByCustomerId(int customerId,int status)
        {
            IRequirementViewDAO rDAO = RequireManagementDAOFactory.CreateRequirementViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams("CustomerId = " + customerId + " and Status = " + status);
        }

        public List<RequirementViewVO> FindRequireByCustomerIdCount(int customerId)
        {
            IRequirementViewDAO rDAO = RequireManagementDAOFactory.CreateRequirementViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams("CustomerId = " + customerId);
        }

        public int AddRequireDelegateCommission(RequireCommissionDelegationVO rcdVO)
        {
            IRequireCommissionDelegationDAO rcdDAO = RequireManagementDAOFactory.CreateRequireCommissionDelegationDAO(new UserProfile());
            
            CommonTranscation t = new CommonTranscation();
            t.TranscationContextWithReturn += delegate ()
            {
                RequirementViewVO rVO = FindRequireById(rcdVO.RequirementId);
                RequireBO rBO = new RequireBO(new CustomerProfile());
                CustomerBO cBO = new CustomerBO(new CustomerProfile());

                decimal Total = 0;//已托管金额
                if (rVO.Status == 4)
                {
                    ProjectBO pBO = new ProjectBO(new CustomerProfile());
                    ProjectViewVO pVO = pBO.FindProjectByRequireId(rVO.RequirementId);
                    Total = pBO.FindCommisionDelegationTotal(pVO.ProjectId);
                }
                else
                {
                    Total = rBO.FindRequireDelegateCommisionTotal(rVO.RequirementId);
                }
                if (Total <= 0 && rcdVO.Commission < cBO.GetFirstMandates(rVO.Commission))
                {
                    return -1;
                }

                //先判断余额是否充足，否则托管不成功
                
                if (cBO.ReduceBalance(rVO.CustomerId, rcdVO.Commission))
                {
                    int commssionDelegateId = rcdDAO.Insert(rcdVO);
                    
                    return commssionDelegateId;
                }
                else
                    return -1;
            };
            int result = t.Go();
            return Convert.ToInt32(t.TranscationReturnValue);
        }

        public int ReduceRequireDelegateCommision(int requireId)
        {
            IRequireCommissionDelegationDAO rcdDAO = RequireManagementDAOFactory.CreateRequireCommissionDelegationDAO(new UserProfile());
            List<RequireCommissionDelegationVO> rcdVOList = rcdDAO.FindByParams("RequirementId = " + requireId);
            RequirementViewVO rVO = FindRequireById(requireId);

            decimal commissionTotal = 0m;
            foreach (RequireCommissionDelegationVO vo in rcdVOList)
            {
                commissionTotal += vo.Commission;
            }

            CustomerBO cBO = new CustomerBO(new CustomerProfile());
            if (cBO.PlusBalance(rVO.CustomerId, commissionTotal))
            {
                //删掉任务酬金委托
                rcdDAO.DeleteByParams("RequirementId = " + requireId);

                return 1;
            }
            else
                return -1;

        }

        public decimal FindRequireDelegateCommisionTotal(int requireId)
        {
            IRequireCommissionDelegationDAO rcdDAO = RequireManagementDAOFactory.CreateRequireCommissionDelegationDAO(new UserProfile());
            List<RequireCommissionDelegationVO> rcdVOList = rcdDAO.FindByParams("RequirementId = " + requireId);

            decimal commissionTotal = 0m;
            foreach (RequireCommissionDelegationVO vo in rcdVOList)
            {
                commissionTotal += vo.Commission;
            }

            return commissionTotal;
        }

        public List<RequireCommissionDelegationviewVO> FindRequireDelegateCommisiondelegationView(int CustomerId)
        {
            IRequireCommissionDelegationviewDAO rcdDAO = RequireManagementDAOFactory.CreateRequireCommissionDelegationviewDAO(new UserProfile());
            List<RequireCommissionDelegationviewVO> rcdVOList = rcdDAO.FindByParams("CustomerId = " + CustomerId);
            return rcdVOList;
        }

        public List<RequireCommissionDelegationviewVO> FindRequireDelegateCommisiondelegationAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IRequireCommissionDelegationviewDAO rcdDAO = RequireManagementDAOFactory.CreateRequireCommissionDelegationviewDAO(new UserProfile());
            return rcdDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public decimal FindRequireCommissionDelegationViewTotalSumCommission(string condition, params object[] parameters)
        {
            IRequireCommissionDelegationviewDAO rDAO = RequireManagementDAOFactory.CreateRequireCommissionDelegationviewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalSumCommission(condition, parameters);
        }

        public int AddRequireClient(RequirementTargetClientVO requireClientVO)
        {
            try
            {
                IRequirementTargetClientDAO aeDAO = RequireManagementDAOFactory.CreateRequirementTargetClientDAO(this.CurrentCustomerProfile);

                int requireClientId = aeDAO.Insert(requireClientVO);

                return requireClientId;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(RequireBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool UpdateRequireClient(RequirementTargetClientVO requireClientVO)
        {
            try
            {
                IRequirementTargetClientDAO aeDAO = RequireManagementDAOFactory.CreateRequirementTargetClientDAO(this.CurrentCustomerProfile);

                aeDAO.UpdateById(requireClientVO);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(RequireBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public List<RequirementTargetClientVO> FindRequireClientByRequire(int requireId)
        {
            IRequirementTargetClientDAO aeDAO = RequireManagementDAOFactory.CreateRequirementTargetClientDAO(this.CurrentCustomerProfile);

            List<RequirementTargetClientVO> requireClientVOList = aeDAO.FindByParams("RequirementId = " + requireId);

            return requireClientVOList;
        }

        public RequirementTargetClientVO FindRequireClientById(int requireClientId)
        {
            IRequirementTargetClientDAO aeDAO = RequireManagementDAOFactory.CreateRequirementTargetClientDAO(this.CurrentCustomerProfile);

            RequirementTargetClientVO requireClientVO = aeDAO.FindById(requireClientId);

            return requireClientVO;
        }

        public bool DeleteRequireClient(int requireClientId)
        {
            IRequirementTargetClientDAO aeDAO = RequireManagementDAOFactory.CreateRequirementTargetClientDAO(this.CurrentCustomerProfile);
            try
            {
                aeDAO.DeleteById(requireClientId);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(RequireBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public List<RequirementViewVO> FindMatchRequireByPageIndex(int agencyId,string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IRequirementViewDAO rDAO = RequireManagementDAOFactory.CreateRequirementViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindMatchRequireByPageIndex(agencyId,conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindMatchRequireTotalCount(int agencyId, string condition, params object[] parameters)
        {
            IRequirementViewDAO rDAO = RequireManagementDAOFactory.CreateRequirementViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindMatchRequireTotalCount(agencyId,condition, parameters);
        }

        /// <summary>
        /// 根据项目ID获取任务快照
        /// </summary>
        public List<RequirementCopiesVO> FindRequirementCopiesByProjectId(int ProjectId)
        {
            IRequirementCopiesDAO aeDAO = RequireManagementDAOFactory.CreateRequirementCopiesDAO(this.CurrentCustomerProfile);
            return aeDAO.FindRequirementCopiesByProjectId(ProjectId);
        }
        /// <summary>
        /// 将任务复制到副本
        /// </summary>
        public bool RequirementCopy(int ProjectId)
        {
            try
            {
                LogBO _log = new LogBO(typeof(RequireBO));
                _log.Error(ProjectId);

                ProjectBO pBO = new ProjectBO(new CustomerProfile());
                RequireBO rBO = new RequireBO(new CustomerProfile());
                ProjectViewVO pVO = pBO.FindProjectById(ProjectId);

                RequirementCopiesVO rcVO = new RequirementCopiesVO();
                RequirementViewVO rVO = rBO.FindRequireById(pVO.RequirementId);

                rcVO.ProjectId = pVO.ProjectId;
                rcVO.RequirementId = rVO.RequirementId;
                rcVO.CityId = rVO.CityId;
                rcVO.CategoryId = rVO.CategoryId;
                rcVO.CustomerId = rVO.CustomerId;
                rcVO.RequirementCode = rVO.RequirementCode;
                rcVO.Title = rVO.Title;
                rcVO.MainImg = rVO.MainImg;
                rcVO.CommissionType = rVO.CommissionType;
                rcVO.Commission = rVO.Commission;
                rcVO.Phone = rVO.Phone;
                rcVO.Description = rVO.Description;
                rcVO.CreatedAt = rVO.CreatedAt;
                rcVO.PublishAt = rVO.PublishAt;
                rcVO.Cost = rVO.Cost;
                rcVO.TargetAgency = rVO.TargetAgency;
                rcVO.AgencyCondition = rVO.AgencyCondition;
                rcVO.ContactPerson = rVO.ContactPerson;
                rcVO.ContactPhone = rVO.ContactPhone;
                rcVO.CommissionDescription = rVO.CommissionDescription;

                IRequirementCopiesDAO aeDAO = RequireManagementDAOFactory.CreateRequirementCopiesDAO(this.CurrentCustomerProfile);
                aeDAO.Insert(rcVO);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(RequireBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 获取任务图片
        /// </summary>
        public string getRequireIMG(int RequireID) {
            AgencyBO aBO = new AgencyBO(new CustomerProfile());
            CustomerBO cBO = new CustomerBO(new CustomerProfile());
            RequirementViewVO rVO = FindRequireById(RequireID);

            string MainImg = rVO.MainImg;
            string CompanyLogo = rVO.CompanyLogo;
            string PersonalCard = "";
            string HeaderLogo = "";
            string Img = ConfigInfo.Instance.NoImg;
            CustomerViewVO cVO = cBO.FindById(rVO.CustomerId);

            if (cVO != null)
            {
                HeaderLogo = cVO.HeaderLogo;
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
            if (MainImg != "") {
                return MainImg;
            }
            if (CompanyLogo != "") {
                return CompanyLogo;
            }
            if (PersonalCard != "") {
                return PersonalCard;
            }else if(HeaderLogo!="") {
                return HeaderLogo;
            }
            return Img;
        }
        /// <summary>
        /// 获取任务二维码
        /// </summary>
        /// <param name="RequirementId"></param>
        /// <returns></returns>
        public string GetRequirementQR(int RequirementId)
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
            DataJson += "\"scene\":\"" + RequirementId + "\",";
            DataJson += string.Format("\"width\":{0},", 430);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", "pages/Require_show/Require_show");//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
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
            string folder = "/UploadFolder/RequirementQRFile/";
            string newFileName = RequirementId + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string Cardimg = ConfigInfo.Instance.APIURL + filePath;

            RequirementVO rVO = new RequirementVO();
            rVO.RequirementId = RequirementId;
            rVO.QRCodeImg = Cardimg;
            UpdateRequirement(rVO);

            return Cardimg;
        }
    }
}
