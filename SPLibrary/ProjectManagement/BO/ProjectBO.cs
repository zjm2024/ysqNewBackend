using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.ProjectManagement.DAO;
using SPLibrary.ProjectManagement.VO;
using SPLibrary.RequireManagement.BO;
using SPLibrary.RequireManagement.DAO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.ProjectManagement.BO
{
    public class ProjectBO
    {
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();

        public ProjectBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }

        public int AddProject(ProjectVO vo)
        {
            try
            {
                IProjectDAO rDAO = ProjectManagementDAOFactory.CreateProjectDAO(this.CurrentCustomerProfile);
                MessageBO mBO = new MessageBO(new CustomerProfile());
                //return rDAO.Insert(vo);
                //生成项目，更新任务状态
                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int projectId = rDAO.Insert(vo);

                    /*
                    RequireBO ubo = new RequireBO(new CustomerProfile());
                    RequirementVO bVO = new RequirementVO();
                    bVO.RequirementId = vo.RequirementId;

                    int agencySum=ubo.FindRequireById(vo.RequirementId).agencySum;
                    int agencyCount = ubo.FindRequireById(vo.RequirementId).agencyCount+1;
                    if(agencyCount>= agencySum)
                    {
                        bVO.Status = 4;
                    }
                    else
                    {
                        bVO.Status = 1;
                    }
                    bVO.agencyCount = agencyCount;

                    bool isSuccess = new RequireBO(new CustomerProfile()).UpdateRequirement(bVO);
                    */

                    //发送站内信息
                    AgencyBO aBO = new AgencyBO(new CustomerProfile());
                    AgencyViewVO aViewVO = aBO.FindAgencyByCustomerId(vo.CustomerId);

                    RequireBO rBO = new RequireBO(new CustomerProfile());
                    RequirementViewVO rVO = rBO.FindRequireById(vo.RequirementId);

                    BusinessBO uBO = new BusinessBO(new CustomerProfile());
                    BusinessViewVO bViewVO = uBO.FindBusinessByCustomerId(rVO.CustomerId);
                    //发送给销售

                    mBO.SendMessage("项目创建", "  " + aViewVO.CustomerName + ":" + bViewVO.CustomerName + "已经雇佣您了！", vo.CustomerId, MessageType.Project);
                    //发送给雇主

                    mBO.SendMessage("项目创建", "  " + bViewVO.CustomerName + ":您已经雇佣了" + aViewVO.CustomerName + ",请尽快托管酬金！", rVO.CustomerId, MessageType.Project);

                    //添加双方为IM好友
                    IMBO imBO = new IMBO(new CustomerProfile());
                    imBO.AddFriend(bViewVO.CustomerId, aViewVO.CustomerId);
                    
                    //string imFrom = imBO.GetIMCustomerByCustomer(bViewVO.CustomerId).IMId;
                    //string imTo = imBO.GetIMCustomerByCustomer(aViewVO.CustomerId).IMId;
                    //if (!string.IsNullOrEmpty(imFrom) && !string.IsNullOrEmpty(imTo))
                    //    imBO.AddIMUserFriend(imFrom, imTo);

                    return projectId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool UpdateProject(ProjectVO vo)
        {
            try
            {
                IProjectDAO rDAO = ProjectManagementDAOFactory.CreateProjectDAO(this.CurrentCustomerProfile);

                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public ProjectViewVO FindProjectById(int projectId)
        {
            IProjectViewDAO rDAO = ProjectManagementDAOFactory.CreateProjectViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(projectId);
        }

        public ProjectViewVO FindProjectByRequireId(int requireId)
        {
            IProjectViewDAO rDAO = ProjectManagementDAOFactory.CreateProjectViewDAO(this.CurrentCustomerProfile);
            List<ProjectViewVO> voList = rDAO.FindByParams("Status <> 6 and RequirementId = " + requireId);
            if (voList.Count > 0)
                return voList[0];
            else
                return null;
        }

        public List<ProjectViewVO> FindProjectByBusiness(int busniessId, int excludeStatus)
        {
            IProjectViewDAO rDAO = ProjectManagementDAOFactory.CreateProjectViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams("BusinessId = " + busniessId + " and Status <> " + excludeStatus);
        }

        public List<ProjectViewVO> FindProjectByAgency(int agencyId,int excludeStatus)
        {
            IProjectViewDAO rDAO = ProjectManagementDAOFactory.CreateProjectViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams("AgencyId = " + agencyId + " and Status <> " + excludeStatus);
        }

        public ProjectVO FindProjectByContract(int contractId)
        {
            IProjectDAO rDAO = ProjectManagementDAOFactory.CreateProjectDAO(this.CurrentCustomerProfile);
            List<ProjectVO> volist = rDAO.FindByParams("Status <> 6 and ContractId = " + contractId);
            if (volist.Count > 0)
                return volist[0];
            else
                return null;
        }

        public List<ProjectViewVO> FindProjectAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IProjectViewDAO rDAO = ProjectManagementDAOFactory.CreateProjectViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindProjectTotalCount(string condition, params object[] parameters)
        {
            IProjectViewDAO rDAO = ProjectManagementDAOFactory.CreateProjectViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }

        public string GetProjectCode()
        {
            var customerCode = new StringBuilder();
            for (var i = 0; i < 10; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                customerCode.Append(r.Next(0, 10));
            }
            return "P" + customerCode.ToString();
        }

        public bool TrusteeshipFunds(int projectId,int CustomerId, decimal Commission, string Purpose)
        {
            try
            {
                IProjectDAO rDAO = ProjectManagementDAOFactory.CreateProjectDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    //扣除已托管酬金
                    CommissionDelegationVO commissionDelegationVO = new CommissionDelegationVO();
                    commissionDelegationVO.Commission = Commission;
                    commissionDelegationVO.ProjectId = projectId;

                    ProjectViewVO pVO = FindProjectById(projectId);

                    commissionDelegationVO.CustomerId = CustomerId;
                    commissionDelegationVO.PayDate = DateTime.Now;
                    commissionDelegationVO.DelegationDate = DateTime.Now;
                    commissionDelegationVO.Status = 0;
                    commissionDelegationVO.Purpose = Purpose;

                    SystemBO sBO = new SystemBO(new UserProfile());
                    ConfigVO vo = sBO.FindConfig();
                    decimal platformCommission = pVO.Commission * vo.CommissionPercentage / 100;//平台抽佣
                    commissionDelegationVO.TotalCommission = pVO.Commission + platformCommission;
                    commissionDelegationVO.PlatformCommission = platformCommission;

                    int deleId = AddCommissionDelegation(commissionDelegationVO);
                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool ApproveProject(int projectId, int status, string reason)
        {
            try
            {
                IProjectDAO rDAO = ProjectManagementDAOFactory.CreateProjectDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    ProjectVO pVO = new ProjectVO();
                    pVO.ProjectId = projectId;
                    pVO.Status = status;

                    rDAO.UpdateById(pVO);
                    //判断Status，如果是拒绝，直接更新状态，如果是同意，先更新状态，支付余额
                    if (status == 1)
                    {
                        //添加跟进信息
                        ProjectActionVO projectActionVO = new ProjectActionVO();
                        projectActionVO.ActionBy = CurrentCustomerProfile.CustomerId;
                        projectActionVO.ActionDate = DateTime.Now;
                        projectActionVO.ActionType = "System";
                        projectActionVO.Description = "雇主拒绝完工：" + reason;
                        projectActionVO.ProjectId = projectId;
                        AddProjectAction(projectActionVO);

                        //发送站内消息
                        ProjectViewVO pViewVO = FindProjectById(projectId);
                        MessageBO mBO = new MessageBO(new CustomerProfile());
                        BusinessBO bBO = new BusinessBO(new CustomerProfile());
                        BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);

                        AgencyBO aBO = new AgencyBO(new CustomerProfile());
                        AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                        mBO.SendMessage("项目进度", "  " + aViewVO.CustomerName + ":雇主拒绝完工：" + reason, aViewVO.CustomerId, MessageType.Project);

                    }
                    else if (status == 2)
                    {
                        //添加跟进信息
                        ProjectActionVO projectActionVO = new ProjectActionVO();
                        projectActionVO.ActionBy = CurrentCustomerProfile.CustomerId;
                        projectActionVO.ActionDate = DateTime.Now;
                        projectActionVO.ActionType = "System";
                        projectActionVO.Description = "雇主确认完工。";
                        projectActionVO.ProjectId = projectId;
                        AddProjectAction(projectActionVO);

                        //发送站内消息
                        ProjectViewVO pViewVO = FindProjectById(projectId);
                        MessageBO mBO = new MessageBO(new CustomerProfile());
                        BusinessBO bBO = new BusinessBO(new CustomerProfile());
                        BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);

                        AgencyBO aBO = new AgencyBO(new CustomerProfile());
                        AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                        mBO.SendMessage("项目进度", "  " + aViewVO.CustomerName + ":雇主确认完工。请及时给雇主评价。", aViewVO.CustomerId, MessageType.Project);
                        mBO.SendMessage("项目进度", "  " + bViewVO.CustomerName + ":项目已完工。请及时给销售评价。", bViewVO.CustomerId, MessageType.Project);

                        //支付尾款
                        //添加一条支付信息
                        //更新销售余额
                        decimal remainCommission = FindRemainCommission(projectId);

                        ProjectCommissionVO pCommissionVO = new ProjectCommissionVO();
                        pCommissionVO.CreatedAt = DateTime.Now;
                        pCommissionVO.CreatedBy = CurrentCustomerProfile.CustomerId;
                        pCommissionVO.Commission = remainCommission;
                        pCommissionVO.PayDate = DateTime.Now;
                        pCommissionVO.ProjectId = projectId;
                        pCommissionVO.Status = 3;

                        AddProjectCommission(pCommissionVO);

                        ProjectVO pVOTemp = rDAO.FindById(projectId);

                        CustomerBO cBO = new CustomerBO(new CustomerProfile());
                        if (cBO.PlusBalance(pVOTemp.CustomerId, remainCommission))
                        {
                            CommissionIncomeVO ciVO = new CommissionIncomeVO();
                            ciVO.Commission = remainCommission;
                            ciVO.CustomerId = pVOTemp.CustomerId;
                            ciVO.PayDate = DateTime.Now;
                            ciVO.ProjectId = projectId;
                            ciVO.Purpose = "项目酬金尾款";
                            cBO.InsertCommissionIncome(ciVO);
                        }

                        //将平台抽佣添加到平台抽佣表
                        SystemBO sBO = new SystemBO(new UserProfile());
                        ConfigVO conVO = sBO.FindConfig();
                        CommissionVO commVO = new CommissionVO();
                        commVO.CommissionDate = DateTime.Now;
                        commVO.CommissionPercentage = conVO.CommissionPercentage;

                        IProjectDAO pDAO = ProjectManagementDAOFactory.CreateProjectDAO(new UserProfile());

                        commVO.ProjectCommission = pDAO.FindPlatformCommission(projectId);
                        commVO.ProjectId = projectId;
                        commVO.Status = 1;
                        sBO.AddCommission(commVO);

                        //更新平台总酬金和成交案例总数量
                        conVO.ProjectTotal = conVO.ProjectTotal + 1;
                        conVO.CommissionTotal = conVO.CommissionTotal + pViewVO.Commission;

                        sBO.ConfigUpdate(conVO);

                        //添加到销售我的项目经验
                        AgencyExperienceVO aeVO = new AgencyExperienceVO();
                        aeVO.AgencyId = pViewVO.AgencyId;
                        aeVO.Title = pViewVO.Title;
                        aeVO.ProjectDate = DateTime.Now;
                        aeVO.Description = pViewVO.StartDate.ToString("yyyy-MM-dd") + " 至 " + DateTime.Now.ToString("yyyy-MM-dd") + " 完成了雇主" + bViewVO.CustomerName + "的任务。";
                        aBO.AddAgencyExperience(aeVO, new List<AgencyExperienceImageVO>());
                    }
                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        #region Project Action
        public List<ProjectActionViewVO> FindProjectActionByProject(int projectId)
        {
            IProjectActionViewDAO bcDAO = ProjectManagementDAOFactory.CreateProjectActionViewDAO(this.CurrentCustomerProfile);
            IProjectActionFileDAO pafDAO = ProjectManagementDAOFactory.CreateProjectActionFileDAO(this.CurrentCustomerProfile);
            List<ProjectActionViewVO> projectActionList =  bcDAO.FindByParams("ProjectId = " + projectId);
            if (projectActionList != null)
            {
                foreach (ProjectActionViewVO vo in projectActionList)
                {
                    vo.ProjectActionFileList = pafDAO.FindByParams("ProjectActionId = " + vo.ProjectActionId);
                }
                return projectActionList;
            }
            else
                return null;
        }
        public ProjectActionVO FindProjectActionById(int projectActionId)
        {
            IProjectActionDAO bcDAO = ProjectManagementDAOFactory.CreateProjectActionDAO(this.CurrentCustomerProfile);
            return bcDAO.FindById(projectActionId);
        }
        public int AddProjectAction(ProjectActionVO vo)
        {
            try
            {
                IProjectActionDAO rDAO = ProjectManagementDAOFactory.CreateProjectActionDAO(this.CurrentCustomerProfile);
                IProjectActionFileDAO pafDAO = ProjectManagementDAOFactory.CreateProjectActionFileDAO(this.CurrentCustomerProfile);

                List<ProjectActionFileVO> projectActionFileList = vo.ProjectActionFileList;

                if(projectActionFileList == null)
                {
                    projectActionFileList = new List<ProjectActionFileVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int projectActionId = rDAO.Insert(vo);

                    foreach (ProjectActionFileVO bcVO in projectActionFileList)
                    {
                        bcVO.ProjectActionId = projectActionId;
                    }
                    pafDAO.InsertList(projectActionFileList, 100);

                    return projectActionId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool UpdateProjectAction(ProjectActionVO vo)
        {
            try
            {
                IProjectActionDAO rDAO = ProjectManagementDAOFactory.CreateProjectActionDAO(this.CurrentCustomerProfile);
                IProjectActionFileDAO pafDAO = ProjectManagementDAOFactory.CreateProjectActionFileDAO(this.CurrentCustomerProfile);

                List<ProjectActionFileVO> projectActionFileList = vo.ProjectActionFileList;

                if (projectActionFileList == null)
                {
                    projectActionFileList = new List<ProjectActionFileVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    rDAO.UpdateById(vo);

                    //删除不存在的，添加新增的
                    List<ProjectActionFileVO> bcDBVOList = pafDAO.FindByParams("ProjectActionId = " + vo.ProjectActionId);
                    List<ProjectActionFileVO> bcdeleteVOList = new List<ProjectActionFileVO>();
                    foreach (ProjectActionFileVO dbVO in bcDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = projectActionFileList.Count - 1; i >= 0; i--)
                        {
                            ProjectActionFileVO bcVO = projectActionFileList[i];
                            if (bcVO.FilePath == dbVO.FilePath)
                            {
                                projectActionFileList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            bcdeleteVOList.Add(dbVO);
                        }
                    }
                    if (projectActionFileList != null)
                        pafDAO.InsertList(projectActionFileList, 100);
                    foreach (ProjectActionFileVO deleteVO in bcdeleteVOList)
                    {
                        pafDAO.DeleteById(deleteVO.ProjectActionFileId);
                    }

                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public bool DeleteProjectAction(int projectActionid)
        {
            try
            {
                IProjectActionDAO rDAO = ProjectManagementDAOFactory.CreateProjectActionDAO(this.CurrentCustomerProfile);
                IProjectActionFileDAO pafDAO = ProjectManagementDAOFactory.CreateProjectActionFileDAO(this.CurrentCustomerProfile);
               
                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {

                    pafDAO.DeleteByParams("ProjectActionid = " + projectActionid);

                    rDAO.DeleteById(projectActionid);

                };
                int result = t.Go();

                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        #endregion

        #region Project Commission
        public List<ProjectCommissionViewVO> FindProjectCommissionByProject(int projectId)
        {
            IProjectCommissionViewDAO bcDAO = ProjectManagementDAOFactory.CreateProjectCommissionViewDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("ProjectId = " + projectId);
        }
        public List<ProjectCommissionViewVO> FindProjectCommissionByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IProjectCommissionViewDAO rDAO = ProjectManagementDAOFactory.CreateProjectCommissionViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }
        public ProjectCommissionVO FindProjectCommissionById(int projectCommissionId)
        {
            IProjectCommissionDAO bcDAO = ProjectManagementDAOFactory.CreateProjectCommissionDAO(this.CurrentCustomerProfile);
            return bcDAO.FindById(projectCommissionId);
        }
        public int AddProjectCommission(ProjectCommissionVO vo)
        {
            try
            {
                IProjectCommissionDAO rDAO = ProjectManagementDAOFactory.CreateProjectCommissionDAO(this.CurrentCustomerProfile);

                return rDAO.Insert(vo);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool UpdateProjectCommission(ProjectCommissionVO vo)
        {
            try
            {
                IProjectCommissionDAO rDAO = ProjectManagementDAOFactory.CreateProjectCommissionDAO(this.CurrentCustomerProfile);

                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public bool DeleteProjectCommission(int projectCommissionId)
        {
            try
            {
                IProjectCommissionDAO rDAO = ProjectManagementDAOFactory.CreateProjectCommissionDAO(this.CurrentCustomerProfile);

                rDAO.DeleteById(projectCommissionId);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public ProjectCommissionViewVO FindLatestProjectCommission(int projectId)
        {
            IProjectCommissionViewDAO bcDAO = ProjectManagementDAOFactory.CreateProjectCommissionViewDAO(this.CurrentCustomerProfile);
            List<ProjectCommissionViewVO> voList = bcDAO.FindByParams("ProjectId = " + projectId + " and Status = 1");
            if (voList.Count > 0)
                return voList[0];
            else
                return null;
        }

        public decimal FindRemainCommission(int projectId)
        {
            IProjectCommissionDAO rDAO = ProjectManagementDAOFactory.CreateProjectCommissionDAO(this.CurrentCustomerProfile);
            return rDAO.FindRemainCommission(projectId);
        }

        public bool UpdateProjectCommissionStatus(ProjectCommissionVO vo)
        {
            try
            {
                IProjectCommissionDAO rDAO = ProjectManagementDAOFactory.CreateProjectCommissionDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    //判断Status，如果是拒绝，直接更新状态，如果是同意，先更新状态，再更新销售的余额
                    if (vo.Status == 3)
                    {
                        vo.PayDate = DateTime.Now;
                    }
                    else if (vo.Status == 2)
                    {
                        vo.RejectAt = DateTime.Now;
                        vo.RejectBy = CurrentCustomerProfile.CustomerId;
                    }
                    rDAO.UpdateById(vo);
                    if (vo.Status == 3)
                    {
                        ProjectCommissionVO pcVO = rDAO.FindById(vo.ProjectCommissionId);

                        CustomerBO cBO = new CustomerBO(new CustomerProfile());
                        if (cBO.PlusBalance(pcVO.CreatedBy, pcVO.Commission))
                        {
                            CommissionIncomeVO ciVO = new CommissionIncomeVO();
                            ciVO.Commission = pcVO.Commission;
                            ciVO.CustomerId = pcVO.CreatedBy;
                            ciVO.PayDate = DateTime.Now;
                            ciVO.ProjectId = vo.ProjectId;
                            ciVO.Purpose = "项目酬金收入";
                            cBO.InsertCommissionIncome(ciVO);

                            CustomerBO _bo = new CustomerBO(new CustomerProfile());
                            //发放乐币
                            List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("获得酬金");
                            if (_bo.ZXBFindRequireCount("CustomerId = " + pcVO.CreatedBy + " and type=" + zVO[0].ZxbConfigID) == 0)
                            {
                                //发放乐币奖励
                                _bo.ZXBAddrequire(pcVO.CreatedBy, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID);
                            }
                        }

                    }

                    //添加跟进信息
                    ProjectActionVO projectActionVO = new ProjectActionVO();
                    projectActionVO.ActionBy = CurrentCustomerProfile.CustomerId;
                    projectActionVO.ActionDate = DateTime.Now;
                    projectActionVO.ActionType = "System";
                    if (vo.Status == 3)
                    {
                        projectActionVO.Description = "雇主同意了支付";
                    }
                    else if (vo.Status == 2)
                    {
                        projectActionVO.Description = "雇主拒绝支付";
                    }

                    projectActionVO.ProjectId = vo.ProjectId;

                    AddProjectAction(projectActionVO);

                    //发送站内消息

                    ProjectViewVO pViewVO = FindProjectById(vo.ProjectId);
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    //BusinessBO bBO = new BusinessBO(new CustomerProfile());
                    //BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);

                    AgencyBO aBO = new AgencyBO(new CustomerProfile());
                    AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                    if (vo.Status == 3)
                    {
                        mBO.SendMessage("项目进度", "  " + aViewVO.CustomerName + ":雇主同意了支付申请！", aViewVO.CustomerId, MessageType.Project);
                    }
                    else if (vo.Status == 2)
                    {
                        mBO.SendMessage("项目进度", "  " + aViewVO.CustomerName + ":雇主拒绝了支付申请！", aViewVO.CustomerId, MessageType.Project);
                    }

                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        #endregion


        #region 申请更改酬金
        public int AddProjectChange(ProjectChangeVO vo)
        {
            try
            {
                IProjectChangeDAO rDAO = ProjectManagementDAOFactory.CreateProjectChangeDAO(this.CurrentCustomerProfile);
                return rDAO.Insert(vo);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool UpdateProjectChange(ProjectChangeVO vo)
        {
            try
            {
                IProjectChangeDAO rDAO = ProjectManagementDAOFactory.CreateProjectChangeDAO(this.CurrentCustomerProfile);

                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public bool DeleteProjectChange(int ProjectId)
        {
            try
            {
                IProjectChangeDAO rDAO = ProjectManagementDAOFactory.CreateProjectChangeDAO(this.CurrentCustomerProfile);
                rDAO.DeleteByParams("ProjectId="+ ProjectId+ " and Status=0");
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public ProjectChangeVO FindProjectChangeById(int ProjectChangeId)
        {
            IProjectChangeDAO bcDAO = ProjectManagementDAOFactory.CreateProjectChangeDAO(this.CurrentCustomerProfile);
            return bcDAO.FindById(ProjectChangeId);
        }
        public List<ProjectChangeVO> FindProjectChangeByProject(int projectId,string conditionStr)
        {
            IProjectChangeDAO bcDAO = ProjectManagementDAOFactory.CreateProjectChangeDAO(this.CurrentCustomerProfile);
            return bcDAO.FindAllByPage("ProjectId = " + projectId+" and "+ conditionStr, "CreatedAt","desc");
        }
        #endregion

        #region 申请关闭项目

        public int AddProjectRefund(ProjectRefundVO vo)
        {
            try
            {
                IProjectRefundDAO rDAO = ProjectManagementDAOFactory.CreateProjectRefundDAO(this.CurrentCustomerProfile);
                return rDAO.Insert(vo);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool UpdateProjectRefund(ProjectRefundVO vo)
        {
            try
            {
                IProjectRefundDAO rDAO = ProjectManagementDAOFactory.CreateProjectRefundDAO(this.CurrentCustomerProfile);

                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public bool DeleteProjectRefund(int ProjectId)
        {
            try
            {
                IProjectRefundDAO rDAO = ProjectManagementDAOFactory.CreateProjectRefundDAO(this.CurrentCustomerProfile);
                rDAO.DeleteByParams("ProjectId=" + ProjectId + " and Status=0");
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public ProjectRefundVO FindProjectRefundById(int ProjectChangeId)
        {
            IProjectRefundDAO rDAO = ProjectManagementDAOFactory.CreateProjectRefundDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(ProjectChangeId);
        }
        public List<ProjectRefundVO> FindProjectRefundByProject(int projectId, string conditionStr)
        {
            IProjectRefundDAO rDAO = ProjectManagementDAOFactory.CreateProjectRefundDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPage("ProjectId = " + projectId + " and " + conditionStr, "CreatedAt", "desc");
        }

        #endregion

        #region Project File
        public List<ProjectFileViewVO> FindProjectFileByProject(int projectId)
        {
            IProjectFileViewDAO bcDAO = ProjectManagementDAOFactory.CreateProjectFileViewDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("ProjectId = " + projectId);
        }
        public ProjectFileVO FindProjectFileById(int projectCommissionId)
        {
            IProjectFileDAO bcDAO = ProjectManagementDAOFactory.CreateProjectFileDAO(this.CurrentCustomerProfile);
            return bcDAO.FindById(projectCommissionId);
        }
        public int AddProjectFile(ProjectFileVO vo)
        {
            try
            {
                IProjectFileDAO rDAO = ProjectManagementDAOFactory.CreateProjectFileDAO(this.CurrentCustomerProfile);

                return rDAO.Insert(vo);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool UpdateProjectFile(ProjectFileVO vo)
        {
            try
            {
                IProjectFileDAO rDAO = ProjectManagementDAOFactory.CreateProjectFileDAO(this.CurrentCustomerProfile);

                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public bool DeleteProjectFile(int projectCommissionId)
        {
            try
            {
                IProjectFileDAO rDAO = ProjectManagementDAOFactory.CreateProjectFileDAO(this.CurrentCustomerProfile);

                rDAO.DeleteById(projectCommissionId);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        #endregion

        #region Project Report File
        public List<ProjectReportFileVO> FindProjectReportFileByProject(int projectId)
        {
            IProjectReportFileDAO bcDAO = ProjectManagementDAOFactory.CreateProjectReportFileDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("ProjectId = " + projectId);
        }
        public ProjectReportFileVO FindProjectReportFileById(int projectCommissionId)
        {
            IProjectReportFileDAO bcDAO = ProjectManagementDAOFactory.CreateProjectReportFileDAO(this.CurrentCustomerProfile);
            return bcDAO.FindById(projectCommissionId);
        }
        public int AddProjectReportFile(ProjectReportFileVO vo)
        {
            try
            {
                IProjectReportFileDAO rDAO = ProjectManagementDAOFactory.CreateProjectReportFileDAO(this.CurrentCustomerProfile);

                return rDAO.Insert(vo);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool UpdateProjectReportFile(ProjectReportFileVO vo)
        {
            try
            {
                IProjectFileDAO rDAO = ProjectManagementDAOFactory.CreateProjectFileDAO(this.CurrentCustomerProfile);

                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public bool DeleteProjectReportFile(int projectCommissionId)
        {
            try
            {
                IProjectReportFileDAO rDAO = ProjectManagementDAOFactory.CreateProjectReportFileDAO(this.CurrentCustomerProfile);

                rDAO.DeleteById(projectCommissionId);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        #endregion
        #region CommissionDelegation
        public List<CommissionDelegationVO> FindCommissionDelegationByProject(int projectId)
        {
            ICommissionDelegationDAO bcDAO = ProjectManagementDAOFactory.CreateCommissionDelegationDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("ProjectId = " + projectId);
        }
        public CommissionDelegationVO FindCommissionDelegationById(int projectCommissionId)
        {
            ICommissionDelegationDAO bcDAO = ProjectManagementDAOFactory.CreateCommissionDelegationDAO(this.CurrentCustomerProfile);
            return bcDAO.FindById(projectCommissionId);
        }
        public int AddCommissionDelegation(CommissionDelegationVO vo)
        {
            try
            {
                ICommissionDelegationDAO rDAO = ProjectManagementDAOFactory.CreateCommissionDelegationDAO(this.CurrentCustomerProfile);

                //return rDAO.Insert(vo);
                //更新余额，添加委托信息

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    CustomerBO cBO = new CustomerBO(new CustomerProfile());
                    decimal Commission = vo.Commission;
                    if (Commission < 0)
                    {
                        int commssionDelegateId = rDAO.Insert(vo);

                        ProjectVO pVO = new ProjectVO();
                        pVO.ProjectId = vo.ProjectId;
                        ProjectViewVO pVO2 = FindProjectById(vo.ProjectId);
                        if (pVO2.Status == 0)
                        {
                            pVO.Status = 1;
                        }
                        bool isSuccess = this.UpdateProject(pVO);

                        return commssionDelegateId;
                    }
                    else {
                        if (cBO.ReduceBalance(vo.CustomerId, vo.Commission))
                        {
                            int commssionDelegateId = rDAO.Insert(vo);

                            ProjectVO pVO = new ProjectVO();
                            pVO.ProjectId = vo.ProjectId;
                            ProjectViewVO pVO2 = FindProjectById(vo.ProjectId);
                            if (pVO2.Status == 0)
                            {
                                pVO.Status = 1;
                            }
                            bool isSuccess = this.UpdateProject(pVO);

                            return commssionDelegateId;
                        }
                        else
                            return -1;
                    }
                    

                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool UpdateCommissionDelegation(CommissionDelegationVO vo)
        {
            try
            {
                ICommissionDelegationDAO rDAO = ProjectManagementDAOFactory.CreateCommissionDelegationDAO(this.CurrentCustomerProfile);

                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public bool DeleteCommissionDelegation(int commissionId)
        {
            try
            {
                ICommissionDelegationDAO rDAO = ProjectManagementDAOFactory.CreateCommissionDelegationDAO(this.CurrentCustomerProfile);

                rDAO.DeleteById(commissionId);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public decimal FindCommisionDelegationTotal(int projectId)
        {
            ICommissionDelegationDAO bcDAO = ProjectManagementDAOFactory.CreateCommissionDelegationDAO(this.CurrentCustomerProfile);
            List<CommissionDelegationVO> rcdVOList = bcDAO.FindByParams("ProjectId = " + projectId);

            decimal commissionTotal = 0m;
            foreach (CommissionDelegationVO vo in rcdVOList)
            {
                commissionTotal += vo.Commission;
            }

            return commissionTotal;

        }

        public List<CommissionDelegationViewVO> FindCommissionDelegationViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICommissionDelegationViewDAO rDAO = ProjectManagementDAOFactory.CreateCommissionDelegationViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindCommissionDelegationViewTotalCount(string condition, params object[] parameters)
        {
            ICommissionDelegationViewDAO rDAO = ProjectManagementDAOFactory.CreateCommissionDelegationViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }

        public decimal FindCommissionDelegationViewTotalSumCommission(string condition, params object[] parameters)
        {
            ICommissionDelegationViewDAO rDAO = ProjectManagementDAOFactory.CreateCommissionDelegationViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalSumCommission(condition, parameters);
        }


        #endregion

        #region Project Refunds
        public List<ProjectRefundsViewVO> FindProjectRefundsByProject(int projectId)
        {
            IProjectRefundsViewDAO bcDAO = ProjectManagementDAOFactory.CreateProjectRefundsViewDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("ProjectId = " + projectId);
        }
        public ProjectRefundsVO FindProjectRefundsById(int projectCommissionId)
        {
            IProjectRefundsDAO bcDAO = ProjectManagementDAOFactory.CreateProjectRefundsDAO(this.CurrentCustomerProfile);
            return bcDAO.FindById(projectCommissionId);
        }
        public int AddProjectRefunds(ProjectRefundsVO vo)
        {
            try
            {
                IProjectRefundsDAO rDAO = ProjectManagementDAOFactory.CreateProjectRefundsDAO(this.CurrentCustomerProfile);

                return rDAO.Insert(vo);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool UpdateProjectRefunds(ProjectRefundsVO vo)
        {
            try
            {
                IProjectRefundsDAO rDAO = ProjectManagementDAOFactory.CreateProjectRefundsDAO(this.CurrentCustomerProfile);

                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public bool DeleteProjectRefunds(int projectCommissionId)
        {
            try
            {
                IProjectRefundsDAO rDAO = ProjectManagementDAOFactory.CreateProjectRefundsDAO(this.CurrentCustomerProfile);

                rDAO.DeleteById(projectCommissionId);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        #endregion

        #region Complaints
        public List<ComplaintsViewVO> FindComplaintsByProject(int projectId)
        {
            IComplaintsViewDAO bcDAO = ProjectManagementDAOFactory.CreateComplaintsViewDAO(this.CurrentCustomerProfile);
            List<ComplaintsViewVO> list = bcDAO.FindByParams("ProjectId = " + projectId);
            foreach (ComplaintsViewVO vo in list)
            {
                vo.ComplaintsImgList = FindComplaintsImgByComplaints(vo.ComplaintsId);
            }
            return list;
        }
        public ComplaintsViewVO FindComplaintsById(int complaintsId)
        {
            IComplaintsViewDAO bcDAO = ProjectManagementDAOFactory.CreateComplaintsViewDAO(this.CurrentCustomerProfile);
            ComplaintsViewVO vo = bcDAO.FindById(complaintsId);
            vo.ComplaintsImgList = FindComplaintsImgByComplaints(vo.ComplaintsId);
            return vo;
        }

        public List<ComplaintsViewVO> FindComplaintsAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IComplaintsViewDAO rDAO = ProjectManagementDAOFactory.CreateComplaintsViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindComplaintsTotalCount(string condition, params object[] parameters)
        {
            IComplaintsViewDAO rDAO = ProjectManagementDAOFactory.CreateComplaintsViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }

        public List<ComplaintsImgVO> FindComplaintsImgByComplaints(int complaintsId)
        {
            IComplaintsImgDAO bcDAO = ProjectManagementDAOFactory.CreateComplaintsImgDAO(this.CurrentCustomerProfile);
            return bcDAO.FindByParams("ComplaintsId = " + complaintsId);
        }
        public int AddComplaints(ComplaintsVO vo, List<ComplaintsImgVO> complaintsImgVOList)
        {
            try
            {
                IComplaintsDAO bcDAO = ProjectManagementDAOFactory.CreateComplaintsDAO(this.CurrentCustomerProfile);
                IComplaintsImgDAO ciDAO = ProjectManagementDAOFactory.CreateComplaintsImgDAO(this.CurrentCustomerProfile);

                if (complaintsImgVOList == null)
                {
                    complaintsImgVOList = new List<ComplaintsImgVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int complaintsId = bcDAO.Insert(vo);

                    foreach (ComplaintsImgVO bcVO in complaintsImgVOList)
                    {
                        bcVO.ComplaintsId = complaintsId;
                    }

                    ciDAO.InsertList(complaintsImgVOList, 100);

                    return complaintsId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool UpdateComplaints(ComplaintsVO vo, List<ComplaintsImgVO> complaintsImgVOList)
        {
            try
            {
                IComplaintsDAO bcDAO = ProjectManagementDAOFactory.CreateComplaintsDAO(this.CurrentCustomerProfile);
                IComplaintsImgDAO ciDAO = ProjectManagementDAOFactory.CreateComplaintsImgDAO(this.CurrentCustomerProfile);

                if (complaintsImgVOList == null)
                {
                    complaintsImgVOList = new List<ComplaintsImgVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    bcDAO.UpdateById(vo);

                    //删除不存在的，添加新增的
                    List<ComplaintsImgVO> rtcDBVOList = ciDAO.FindByParams("ComplaintsId = " + vo.ComplaintsId);
                    List<ComplaintsImgVO> rtcdeleteVOList = new List<ComplaintsImgVO>();
                    foreach (ComplaintsImgVO dbVO in rtcDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = complaintsImgVOList.Count - 1; i >= 0; i--)
                        {
                            ComplaintsImgVO bcVO = complaintsImgVOList[i];
                            if (bcVO.ImagePath == dbVO.ImagePath)
                            {
                                complaintsImgVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            rtcdeleteVOList.Add(dbVO);
                        }
                    }
                    if (complaintsImgVOList != null)
                        ciDAO.InsertList(complaintsImgVOList, 100);
                    foreach (ComplaintsImgVO deleteVO in rtcdeleteVOList)
                    {
                        ciDAO.DeleteById(deleteVO.ComplaintsImgId);
                    }
                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool UpdateComplaints(ComplaintsVO vo)
        {
            try
            {
                IComplaintsDAO bcDAO = ProjectManagementDAOFactory.CreateComplaintsDAO(this.CurrentCustomerProfile);

                bcDAO.UpdateById(vo);

                //发送站内消息通知申请人状态修改
                MessageBO mBO = new MessageBO(new CustomerProfile());
                CustomerBO cBO = new CustomerBO(new CustomerProfile());
                CustomerViewVO cVO = cBO.FindById(bcDAO.FindById(vo.ComplaintsId).Creator);
                if (vo.Status == 1)
                    mBO.SendMessage("维权申请", "  " + cVO.CustomerName + ":您的维权申请已跟进。", cVO.CustomerId, MessageType.Project);
                else if (vo.Status == 2)
                    mBO.SendMessage("维权申请", "  " + cVO.CustomerName + ":您的维权申请已处理。", cVO.CustomerId, MessageType.Project);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public bool DeleteComplaints(int projectCommissionId)
        {
            try
            {
                IComplaintsDAO rDAO = ProjectManagementDAOFactory.CreateComplaintsDAO(this.CurrentCustomerProfile);

                rDAO.DeleteById(projectCommissionId);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool IsHasComplaints(int projectId, int customerId)
        {
            IComplaintsViewDAO bcDAO = ProjectManagementDAOFactory.CreateComplaintsViewDAO(this.CurrentCustomerProfile);
            List<ComplaintsViewVO> list = bcDAO.FindByParams("ProjectId = " + projectId + " and Creator = " + customerId);            
            return list.Count > 0;
        }
        #endregion


        #region AgencyReview
        public List<AgencyReviewVO> FindAgencyReviewByProject(int projectId)
        {
            IAgencyReviewDAO bcDAO = ProjectManagementDAOFactory.CreateAgencyReviewDAO(this.CurrentCustomerProfile);
            IAgencyReviewDetailDAO bcdDAO = ProjectManagementDAOFactory.CreateAgencyReviewDetailDAO(this.CurrentCustomerProfile);
            List<AgencyReviewVO> voList = bcDAO.FindByParams("ProjectId = " + projectId);
            foreach (AgencyReviewVO vo in voList)
            {
                vo.AgencyReviewDetailList = bcdDAO.FindByParams("AgencyReviewId = " + vo.AgencyReviewId);
            }
            return voList;
        }


        public List<AgencySumReviewVO> FindAgencySumReviewByAgencyId(int agencyId)
        {
            IAgencySumReviewDAO bcDAO = ProjectManagementDAOFactory.CreateAgencySumReviewDAO(this.CurrentCustomerProfile);
            List<AgencySumReviewVO> voList = bcDAO.FindByParams("AgencyId = " + agencyId);
            return voList;
        }
        public List<AllAgencyAverageScoreViewVO> FindAllAgencyAverageScoreView()
        {
            IAllAgencyAverageScoreViewDAO bcDAO = ProjectManagementDAOFactory.CreateAllAgencyAverageScoreViewDAO(this.CurrentCustomerProfile);
            List<AllAgencyAverageScoreViewVO> voList = bcDAO.FindByParams("1 = 1 ");
            return voList;
        }
        public List<AllBusinessAverageScoreViewVO> FindAllBusinessAverageScoreView()
        {
            IAllBusinessAverageScoreViewDAO bcDAO = ProjectManagementDAOFactory.CreateAllBusinessAverageScoreViewDAO(this.CurrentCustomerProfile);
            List<AllBusinessAverageScoreViewVO> voList = bcDAO.FindByParams("1 = 1 ");
            return voList;
        }
        public AgencyReviewViewVO FindAgencyReviewById(int agencyReviewId)
        {
            IAgencyReviewViewDAO bcDAO = ProjectManagementDAOFactory.CreateAgencyReviewViewDAO(this.CurrentCustomerProfile);
            IAgencyReviewDetailDAO bcdDAO = ProjectManagementDAOFactory.CreateAgencyReviewDetailDAO(this.CurrentCustomerProfile);
            AgencyReviewViewVO vo = bcDAO.FindById(agencyReviewId);
            vo.AgencyReviewDetailList = bcdDAO.FindByParams("AgencyReviewId = " + vo.AgencyReviewId);
            return vo;
        }
        public int AddAgencyReview(AgencyReviewVO vo, List<AgencyReviewDetailVO> agencyReviewDetailVOList)
        {
            try
            {
                IAgencyReviewDAO rDAO = ProjectManagementDAOFactory.CreateAgencyReviewDAO(this.CurrentCustomerProfile);
                IAgencyReviewDetailDAO ciDAO = ProjectManagementDAOFactory.CreateAgencyReviewDetailDAO(this.CurrentCustomerProfile);

                if (agencyReviewDetailVOList == null)
                {
                    agencyReviewDetailVOList = new List<AgencyReviewDetailVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int agencyReviewId = rDAO.Insert(vo);

                    foreach (AgencyReviewDetailVO bcVO in agencyReviewDetailVOList)
                    {
                        bcVO.AgencyReviewId = agencyReviewId;
                    }

                    ciDAO.InsertList(agencyReviewDetailVOList, 100);

                    return agencyReviewId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }


        public bool UpdateAgencyReview(AgencyReviewVO vo, List<AgencyReviewDetailVO> agencyReviewDetailVOList)
        {
            try
            {
                IAgencyReviewDAO rDAO = ProjectManagementDAOFactory.CreateAgencyReviewDAO(this.CurrentCustomerProfile);
                IAgencyReviewDetailDAO ciDAO = ProjectManagementDAOFactory.CreateAgencyReviewDetailDAO(this.CurrentCustomerProfile);

                if (agencyReviewDetailVOList == null)
                {
                    agencyReviewDetailVOList = new List<AgencyReviewDetailVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    rDAO.UpdateById(vo);

                    //先删除再添加
                    ciDAO.DeleteByParams("AgencyReviewId = " + vo.AgencyReviewId);

                    if (agencyReviewDetailVOList != null)
                        ciDAO.InsertList(agencyReviewDetailVOList, 100);

                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool UpdateAgencyReview(AgencyReviewVO vo)
        {
            try
            {
                IAgencyReviewDAO rDAO = ProjectManagementDAOFactory.CreateAgencyReviewDAO(this.CurrentCustomerProfile);

                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public bool DeleteAgencyReview(int agencyReviewId)
        {
            try
            {
                IAgencyReviewDAO rDAO = ProjectManagementDAOFactory.CreateAgencyReviewDAO(this.CurrentCustomerProfile);
                IAgencyReviewDetailDAO ardDAO = ProjectManagementDAOFactory.CreateAgencyReviewDetailDAO(this.CurrentCustomerProfile);

                ardDAO.DeleteByParams("AgencyReviewId = " + agencyReviewId);
                rDAO.DeleteById(agencyReviewId);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        #endregion

        #region BusinessReview
        public List<BusinessReviewVO> FindBusinessReviewByProject(int projectId)
        {
            IBusinessReviewDAO bcDAO = ProjectManagementDAOFactory.CreateBusinessReviewDAO(this.CurrentCustomerProfile);
            IBusinessReviewDetailDAO bcdDAO = ProjectManagementDAOFactory.CreateBusinessReviewDetailDAO(this.CurrentCustomerProfile);
            List<BusinessReviewVO> voList = bcDAO.FindByParams("ProjectId = " + projectId);
            foreach (BusinessReviewVO vo in voList)
            {
                vo.BusinessReviewDetailList = bcdDAO.FindByParams("BusinessReviewId = " + vo.BusinessReviewId);
            }
            return voList;
        }

        public List<BusinessSumReviewVO> FindBusinessSumReviewByBusinessId(int BusinessId)
        {
            IBusinessSumReviewDAO bcDAO = ProjectManagementDAOFactory.CreateBusinessSumReviewDAO(this.CurrentCustomerProfile);
            List<BusinessSumReviewVO> voList = bcDAO.FindByParams("BusinessId = " + BusinessId);

            return voList;
        }
        public BusinessReviewViewVO FindBusinessReviewById(int businessReviewId)
        {
            IBusinessReviewViewDAO bcDAO = ProjectManagementDAOFactory.CreateBusinessReviewViewDAO(this.CurrentCustomerProfile);
            IBusinessReviewDetailDAO bcdDAO = ProjectManagementDAOFactory.CreateBusinessReviewDetailDAO(this.CurrentCustomerProfile);
            BusinessReviewViewVO vo = bcDAO.FindById(businessReviewId);
            vo.BusinessReviewDetailList = bcdDAO.FindByParams("BusinessReviewId = " + vo.BusinessReviewId);

            return vo;
        }
        public int AddBusinessReview(BusinessReviewVO vo, List<BusinessReviewDetailVO> businessReviewDetailVOList)
        {
            try
            {
                IBusinessReviewDAO rDAO = ProjectManagementDAOFactory.CreateBusinessReviewDAO(this.CurrentCustomerProfile);
                IBusinessReviewDetailDAO ciDAO = ProjectManagementDAOFactory.CreateBusinessReviewDetailDAO(this.CurrentCustomerProfile);

                if (businessReviewDetailVOList == null)
                {
                    businessReviewDetailVOList = new List<BusinessReviewDetailVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int businessReviewId = rDAO.Insert(vo);

                    foreach (BusinessReviewDetailVO bcVO in businessReviewDetailVOList)
                    {
                        bcVO.BusinessReviewId = businessReviewId;
                    }

                    ciDAO.InsertList(businessReviewDetailVOList, 100);

                    return businessReviewId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool UpdateBusinessReview(BusinessReviewVO vo, List<BusinessReviewDetailVO> businessReviewDetailVOList)
        {
            try
            {
                IBusinessReviewDAO rDAO = ProjectManagementDAOFactory.CreateBusinessReviewDAO(this.CurrentCustomerProfile);
                IBusinessReviewDetailDAO ciDAO = ProjectManagementDAOFactory.CreateBusinessReviewDetailDAO(this.CurrentCustomerProfile);

                if (businessReviewDetailVOList == null)
                {
                    businessReviewDetailVOList = new List<BusinessReviewDetailVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    rDAO.UpdateById(vo);

                    //先删除再添加
                    ciDAO.DeleteByParams("BusinessReviewId = " + vo.BusinessReviewId);

                    if (businessReviewDetailVOList != null)
                        ciDAO.InsertList(businessReviewDetailVOList, 100);

                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool UpdateBusinessReview(BusinessReviewVO vo)
        {
            try
            {
                IBusinessReviewDAO rDAO = ProjectManagementDAOFactory.CreateBusinessReviewDAO(this.CurrentCustomerProfile);

                rDAO.UpdateById(vo);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public bool DeleteBusinessReview(int businessReviewId)
        {
            try
            {
                IBusinessReviewDAO rDAO = ProjectManagementDAOFactory.CreateBusinessReviewDAO(this.CurrentCustomerProfile);
                IBusinessReviewDetailDAO ardDAO = ProjectManagementDAOFactory.CreateBusinessReviewDetailDAO(this.CurrentCustomerProfile);

                ardDAO.DeleteByParams("AgencyReviewId = " + businessReviewId);
                rDAO.DeleteById(businessReviewId);

                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public int FindAgencyReviewTotalCount(string condition, params object[] parameters)
        {
            IAgencyReviewViewDAO rDAO = ProjectManagementDAOFactory.CreateAgencyReviewViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }

        public List<AgencyReviewViewVO> FindAgencyReviewByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IAgencyReviewViewDAO rDAO = ProjectManagementDAOFactory.CreateAgencyReviewViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindBusinessReviewTotalCount(string condition, params object[] parameters)
        {
            IBusinessReviewViewDAO rDAO = ProjectManagementDAOFactory.CreateBusinessReviewViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }

        public List<BusinessReviewViewVO> FindBusinessReviewByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IBusinessReviewViewDAO rDAO = ProjectManagementDAOFactory.CreateBusinessReviewViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }




        #endregion


        #region 三方协议
        public int AddContract(ContractVO vo, List<ContractStepsVO> contractStepsVOList, List<ContractFileVO> fileVOList)
        {
            try
            {
                IContractDAO rDAO = ProjectManagementDAOFactory.CreateContractDAO(this.CurrentCustomerProfile);
                IContractStepsDAO rtcDAO = ProjectManagementDAOFactory.CreateContractStepsDAO(this.CurrentCustomerProfile);
                IContractFileDAO rfDAO = ProjectManagementDAOFactory.CreateContractFileDAO(this.CurrentCustomerProfile);

                if (contractStepsVOList == null)
                {
                    contractStepsVOList = new List<ContractStepsVO>();
                }                

                if (fileVOList == null)
                {
                    fileVOList = new List<ContractFileVO>();
                }


                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int contractId = rDAO.Insert(vo);

                    foreach (ContractStepsVO bcVO in contractStepsVOList)
                    {
                        bcVO.ContractId = contractId;
                    }

                    rtcDAO.InsertList(contractStepsVOList, 100);
                   

                    foreach (ContractFileVO tcityVO in fileVOList)
                    {
                        tcityVO.ContractId = contractId;
                    }

                    rfDAO.InsertList(fileVOList, 100);

                    return contractId;
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

        public bool UpdateContract(ContractVO vo, List<ContractStepsVO> contractStepsVOList, List<ContractFileVO> fileVOList)
        {
            try
            {
                IContractDAO rDAO = ProjectManagementDAOFactory.CreateContractDAO(this.CurrentCustomerProfile);
                IContractStepsDAO rtcDAO = ProjectManagementDAOFactory.CreateContractStepsDAO(this.CurrentCustomerProfile);
                IContractFileDAO rfDAO = ProjectManagementDAOFactory.CreateContractFileDAO(this.CurrentCustomerProfile);

                if (contractStepsVOList == null)
                {
                    contractStepsVOList = new List<ContractStepsVO>();
                }

                if (fileVOList == null)
                {
                    fileVOList = new List<ContractFileVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    rDAO.UpdateById(vo);

                    //删除不存在的，添加新增的
                    rtcDAO.DeleteByParams("ContractId = " + vo.ContractId);
                    if (contractStepsVOList != null)
                        rtcDAO.InsertList(contractStepsVOList, 100);
                                  

                    //删除不存在的，添加新增的
                    List<ContractFileVO> rfDBVOList = rfDAO.FindByParams("ContractId = " + vo.ContractId);
                    List<ContractFileVO> rfdeleteVOList = new List<ContractFileVO>();
                    foreach (ContractFileVO dbVO in rfDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = fileVOList.Count - 1; i >= 0; i--)
                        {
                            ContractFileVO rfVO = fileVOList[i];
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
                    foreach (ContractFileVO deleteVO in rfdeleteVOList)
                    {
                        rfDAO.DeleteById(deleteVO.ContractFileId);
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

        public bool UpdateContract(ContractVO vo)
        {
            IContractDAO rDAO = ProjectManagementDAOFactory.CreateContractDAO(this.CurrentCustomerProfile);

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


        public ContractViewVO FindContractById(int ContractId)
        {
            IContractViewDAO rDAO = ProjectManagementDAOFactory.CreateContractViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(ContractId);
        }

        public List<ContractViewVO> FindContractAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IContractViewDAO rDAO = ProjectManagementDAOFactory.CreateContractViewDAO(this.CurrentCustomerProfile);
            IContractFileDAO cfDAO = ProjectManagementDAOFactory.CreateContractFileDAO(this.CurrentCustomerProfile);
            List<ContractViewVO> voList = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            foreach (ContractViewVO vo in voList)
            {
                vo.ContractFileList = cfDAO.FindByParams("ContractId = " + vo.ContractId);
            }
            return voList;
        }

        public int FindContractTotalCount(string condition, params object[] parameters)
        {
            IContractViewDAO rDAO = ProjectManagementDAOFactory.CreateContractViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }

        public List<ContractFileVO> FindContractFileList(int contractId)
        {
            IContractFileDAO cfDAO = ProjectManagementDAOFactory.CreateContractFileDAO(new UserProfile());
            return cfDAO.FindByParams("ContractId = " + contractId);
        }

        public List<ContractStepsVO> FindContractStepsList(int contractId)
        {
            IContractStepsDAO cfDAO = ProjectManagementDAOFactory.CreateContractStepsDAO(new UserProfile());
            return cfDAO.FindByParams("ContractId = " + contractId);
        }

        /// <summary>
        /// 生成合同图片，并添加到合同柜
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public int GenerateContractImage(Bitmap m_Bitmap, int customerId,string projectName)
        {
            string filePath = "";
            string folder = "/UploadFolder/CustomerFile/"+ customerId + "/ContractBox/";

            string newFileName = DateTime.Now.ToString("yyyyMMddhhssmm") + ".png";

            filePath = folder + newFileName;
            //可以修改为网络路径
            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;

            //FileInfoVO fileVO = new FileInfoVO();
            //fileVO.FileName = hfc[0].FileName;
            //fileVO.FilePath = "~" + filePath;
            //Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.SITEURL + "ProjectManagement/GenerateContractIMG.aspx?ContractId=" + contractId, 1200, 1200, 1000, 1200);
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            ToolFileVO tfVO = new ToolFileVO();
            tfVO.FileName = projectName;
            tfVO.Description = "自动生成";
            tfVO.FilePath = ConfigInfo.Instance.APIURL + filePath;
            tfVO.TypeId = 1;//合同
            tfVO.CreatedDate = DateTime.Now;
            tfVO.CreatedBy = customerId;

            CustomerBO uBO = new CustomerBO(new CustomerProfile());

            int res = uBO.AddToolFile(tfVO);

            return res;
        }

        public Bitmap GenerateContractImage(int contractId)
        {
            Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.SITEURL + "ProjectManagement/GenerateContractIMG.aspx?ContractId=" + contractId, 1100, 4000, 1100, 4000);
            return m_Bitmap;
        }

        public int UpdateProjectCommission(int requireId, int projectId)
        {
            //添加酬金到项目，任务委托酬金清零
            try
            {
                ICommissionDelegationDAO rDAO = ProjectManagementDAOFactory.CreateCommissionDelegationDAO(this.CurrentCustomerProfile);
                RequireBO rBO = new RequireBO(new CustomerProfile());

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {

                    CommissionDelegationVO vo = new CommissionDelegationVO();
                    vo.Commission = rBO.FindRequireDelegateCommisionTotal(requireId);
                    vo.CustomerId = rBO.FindRequireById(requireId).CustomerId;

                    ProjectViewVO pVO = FindProjectById(projectId);

                    vo.PlatformCommission = CacheSystemConfig.GetSystemConfig().CommissionPercentage * pVO.Commission / 100;
                    vo.ProjectId = projectId;

                    vo.TotalCommission = vo.PlatformCommission + pVO.Commission;
                    vo.PayDate = DateTime.Now;
                    vo.DelegationDate = DateTime.Now;
                    vo.Status = 0;
                    vo.Purpose = "项目酬金委托";
                    
                    int commssionDelegateId = rDAO.Insert(vo);

                    IRequireCommissionDelegationDAO rcdDAO = RequireManagementDAOFactory.CreateRequireCommissionDelegationDAO(new UserProfile());

                    List<RequireCommissionDelegationVO> rcdVOList = rcdDAO.FindByParams("RequirementId = " + requireId);
                    foreach (RequireCommissionDelegationVO rcdVO in rcdVOList)
                    {
                        rcdVO.Commission = 0;
                        rcdDAO.UpdateById(rcdVO);
                    }
                    //rcdDAO.UpdateById(rcdVOList);

                    //如果委托的超过了20%，直接到第二步。
                    /*
                    if (vo.Commission >= vo.TotalCommission * 0.2m)
                    {
                        ProjectVO proVO = new ProjectVO();
                        proVO.ProjectId = projectId;
                        proVO.Status = 1;
                        bool isSuccess = UpdateProject(proVO);
                    }
                    */
                    return commssionDelegateId;


                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(ProjectBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool RemoveComissionDelegation(int projectId)
        {
            ICommissionDelegationDAO rDAO = ProjectManagementDAOFactory.CreateCommissionDelegationDAO(this.CurrentCustomerProfile);
            List<CommissionDelegationVO> rcdVOList = rDAO.FindByParams("ProjectId = " + projectId);
            foreach (CommissionDelegationVO rcdVO in rcdVOList)
            {
                rcdVO.Commission = 0;
                rDAO.UpdateById(rcdVO);
            }
            //rDAO.UpdateById(rcdVOList);

            return false;
        }

        public bool IsHasPayed(int projectId)
        {
            IProjectCommissionViewDAO bcDAO = ProjectManagementDAOFactory.CreateProjectCommissionViewDAO(this.CurrentCustomerProfile);
            List<ProjectCommissionViewVO> voList = bcDAO.FindByParams("ProjectId = " + projectId + " and Status = 3");
            if (voList.Count > 0)
                return true;
            else
                return false;
        }
        #endregion

        public bool IsGenerateProject(int requireId)
        {
            //查询是否存在未作废的项目
            IProjectViewDAO rDAO = ProjectManagementDAOFactory.CreateProjectViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams("Status <> 6  and RequirementId = " + requireId).Count > 0;
        }

        public bool IsGenerateContract(int requireId)
        {
            //查询是否存在未作废的合同
            IContractViewDAO rDAO = ProjectManagementDAOFactory.CreateContractViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams("Status = 1 and RequirementId = " + requireId).Count > 0;
        }

        public bool CancelContract(int requireId)
        {
            try
            {
                IContractDAO rDAO = ProjectManagementDAOFactory.CreateContractDAO(this.CurrentCustomerProfile);
                List<ContractVO> voList = rDAO.FindByParams("Status = 1 and RequirementId = " + requireId);

                foreach (ContractVO vo in voList)
                {
                    ContractVO nVO = new ContractVO();
                    nVO.ContractId = vo.ContractId;
                    nVO.Status = 0;
                    rDAO.UpdateById(nVO);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
