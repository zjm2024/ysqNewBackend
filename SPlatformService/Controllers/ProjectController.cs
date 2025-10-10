using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using CoreFramework.VO;
using SPlatformService.Models;
using SPlatformService.TokenMange;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.ProjectManagement.BO;
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
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SPlatformService.Controllers
{
    /// <summary>
    /// 项目 API
    /// </summary>
    [RoutePrefix("SPWebAPI/Project")]
    [TokenProjector]
    public class ProjectController : ApiController
    {
        /// <summary>
        /// 添加或更新项目信息
        /// </summary>
        /// <param name="projectVO">项目VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateProject"), HttpPost]
        public ResultObject UpdateProject([FromBody] ProjectVO projectVO, string token)
        {
            if (projectVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);


            ProjectBO uBO = new ProjectBO(new CustomerProfile());

            if (projectVO.ProjectId < 1)
            {
                RequireBO rBO = new RequireBO(new CustomerProfile());
                RequirementViewVO requireVO = rBO.FindRequireById(projectVO.RequirementId);
                
                if (requireVO.Status == 4)
                {
                    return new ResultObject() { Flag = 0, Message = "已经选定销售，不能重复指定!", Result = null };
                }

                

                projectVO.CreatedAt = DateTime.Now;
                projectVO.ProjectCode = uBO.GetProjectCode();
                CustomerProfile cProfile = uProfile as CustomerProfile;

                int projectId = uBO.AddProject(projectVO);
                if (projectId > 0)
                {
                    //将任务复制到副本
                    rBO.RequirementCopy(projectId);
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = projectId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                ProjectViewVO pVO = uBO.FindProjectById(projectVO.ProjectId);
                projectVO.Commission = pVO.Commission;
                projectVO.Restore("CreatedAt");
                bool isSuccess = uBO.UpdateProject(projectVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProject"), HttpGet]
        public ResultObject GetProject(int projectId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            ProjectViewVO uVO = uBO.FindProjectById(projectId);
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
        /// 获取会员相关项目数目
        /// </summary>
        /// <param name="customerId">会员Id</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetCustomerProjectCount"), HttpGet]
        public ResultObject GetCustomerProjectCount(int customerId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            int count = uBO.FindProjectTotalCount("BusinessCustomerId = " + customerId + " or AgencyCustomerId = " + customerId);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }

        /// <summary>
        /// 获取雇主所有的项目
        /// </summary>
        /// <param name="businessId">雇主ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProjectByBusiness"), HttpGet]
        public ResultObject GetProjectByBusiness(int businessId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ProjectViewVO> uVO = uBO.FindProjectByBusiness(businessId, 6);
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
        /// 获取销售所有的项目
        /// </summary>
        /// <param name="agencyId">销售ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProjectByAgency"), HttpGet]
        public ResultObject GetProjectByAgency(int agencyId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ProjectViewVO> uVO = uBO.FindProjectByAgency(agencyId, 6);
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
        /// 项目完工申请
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("CompleteProject"), HttpGet]
        public ResultObject CompleteProject(int projectId, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            ProjectBO uBO = new ProjectBO(cProfile);

            ProjectVO pVO = new ProjectVO();
            pVO.ProjectId = projectId;
            pVO.Status = 3;

            bool isSuccess = uBO.UpdateProject(pVO);

            if (isSuccess)
            {
                //添加跟进信息
                ProjectActionVO projectActionVO = new ProjectActionVO();
                projectActionVO.ActionBy = cProfile.CustomerId;
                projectActionVO.ActionDate = DateTime.Now;
                projectActionVO.ActionType = "System";
                projectActionVO.Description = "销售申请项目完工。";
                projectActionVO.ProjectId = projectId;
                uBO.AddProjectAction(projectActionVO);

                //发送站内消息
                ProjectViewVO pViewVO = uBO.FindProjectById(projectId);
                MessageBO mBO = new MessageBO(new CustomerProfile());
                BusinessBO bBO = new BusinessBO(new CustomerProfile());
                BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);

                AgencyBO aBO = new AgencyBO(new CustomerProfile());
                AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);


                string msgContent = "尊敬的"+ bViewVO.CustomerName + "：销售" + aViewVO.AgencyName + "已经提交项目完工申请，请尽快登陆平台确认！【众销乐 -资源共享众包销售平台】";
                string result = MessageTool.SendMobileMsg(msgContent, bViewVO.Phone);
                mBO.SendMessage("项目进度", "  " + bViewVO.CustomerName + "：销售" + aViewVO.AgencyName + "已经提交项目完工申请，请尽快登陆平台确认！", bViewVO.CustomerId, MessageType.Project);


                return new ResultObject() { Flag = 1, Message = "提交成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "提交失败!", Result = null };
            }
        }

        /// <summary>
        /// 处理项目完工
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="status">状态</param>
        /// <param name="reason">原因</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ApproveProject"), HttpGet]
        public ResultObject ApproveProject(int projectId, int status, string reason, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            ProjectBO uBO = new ProjectBO(cProfile);

            decimal total = uBO.FindCommisionDelegationTotal(projectId);//已托管金额

            ProjectViewVO pViewVO = uBO.FindProjectById(projectId);
            SystemBO sBO = new SystemBO(new UserProfile());
            ConfigVO vo = sBO.FindConfig();
            decimal platformCommission = pViewVO.Commission * (vo.CommissionPercentage / 100);//平台抽佣
            decimal remainCommission = uBO.FindRemainCommission(projectId);//剩余酬金

            if (total < (remainCommission + platformCommission) && status == 2)
            {
                return new ResultObject() { Flag = 0, Message = "已托管的酬金不足以支付“剩余酬金+平台抽佣”!", Result = null };
            }

            //修改的
            ProjectVO pVO = new ProjectVO();
            pVO.ProjectId = projectId;
            pVO.Status = status;
            bool isSuccess = uBO.UpdateProject(pVO);
            if (isSuccess)
            {
                if (status == 1)
                {
                    //添加跟进信息
                    ProjectActionVO projectActionVO = new ProjectActionVO();
                    projectActionVO.ActionBy = cProfile.CustomerId;
                    projectActionVO.ActionDate = DateTime.Now;
                    projectActionVO.ActionType = "System";
                    projectActionVO.Description = "雇主拒绝完工：" + reason;
                    projectActionVO.ProjectId = projectId;
                    uBO.AddProjectAction(projectActionVO);

                    //发送站内消息
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    BusinessBO bBO = new BusinessBO(new CustomerProfile());
                    BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);

                    AgencyBO aBO = new AgencyBO(new CustomerProfile());
                    AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                    string msgContent = "尊敬的"+ aViewVO.CustomerName + "：雇主拒绝了您的完工申请：" + reason + "【众销乐 -资源共享众包销售平台】";
                    string result = MessageTool.SendMobileMsg(msgContent, aViewVO.Phone);
                    mBO.SendMessage("项目进度", "  " + aViewVO.CustomerName + ":雇主拒绝了您的完工申请：" + reason, aViewVO.CustomerId, MessageType.Project);

                }
                else if (status == 2)
                {
                    //添加跟进信息
                    ProjectActionVO projectActionVO = new ProjectActionVO();
                    projectActionVO.ActionBy = cProfile.CustomerId;
                    projectActionVO.ActionDate = DateTime.Now;
                    projectActionVO.ActionType = "System";
                    projectActionVO.Description = "雇主确认完工。";
                    projectActionVO.ProjectId = projectId;
                    uBO.AddProjectAction(projectActionVO);

                    //发送站内消息
                    
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    BusinessBO bBO = new BusinessBO(new CustomerProfile());
                    BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);

                    AgencyBO aBO = new AgencyBO(new CustomerProfile());
                    AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                    string msgContent =  "尊敬的"+ aViewVO.CustomerName + "：雇主已经确认项目完工。请登陆平台及时对雇主进行评价。【众销乐 -资源共享众包销售平台】";
                    string result = MessageTool.SendMobileMsg(msgContent, aViewVO.Phone);
                    string msgContent2 = "尊敬的"+ bViewVO.CustomerName + "：项目《"+ pViewVO.ProjectName + "》已完工。请登陆平台及时对销售进行评价。【众销乐 -资源共享众包销售平台】";
                    string result2 = MessageTool.SendMobileMsg(msgContent2, bViewVO.Phone);


                    mBO.SendMessage("项目进度", "  " + aViewVO.CustomerName + ":雇主已经确认项目完工。请登陆平台及时对雇主进行评价。", aViewVO.CustomerId, MessageType.Project);
                    mBO.SendMessage("项目进度", "  " + bViewVO.CustomerName + ":项目《" + pViewVO.ProjectName + "》已完工。请登陆平台及时对销售进行评价。", bViewVO.CustomerId, MessageType.Project);

                    CustomerBO _bo = new CustomerBO(new CustomerProfile());
                    //发放乐币
                    List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("雇主完成任务");
                    if (_bo.ZXBFindRequireCount("CustomerId = " + bViewVO.CustomerId + " and type=" + zVO[0].ZxbConfigID) == 0)
                    {
                        //发放乐币奖励
                        _bo.ZXBAddrequire(bViewVO.CustomerId, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID);
                    }
                    List<ZxbConfigVO> zVO2 = _bo.ZXBAddrequirebyCode("销售完成任务");
                    if (_bo.ZXBFindRequireCount("CustomerId = " + aViewVO.CustomerId + " and type=" + zVO2[0].ZxbConfigID) == 0)
                    {
                        //发放乐币奖励
                        _bo.ZXBAddrequire(aViewVO.CustomerId, zVO2[0].Cost, zVO2[0].Purpose, zVO2[0].ZxbConfigID);
                    }

                    //支付尾款
                    //添加一条支付信息
                    //更新销售余额

                    ProjectCommissionVO pCommissionVO = new ProjectCommissionVO();
                    pCommissionVO.CreatedAt = DateTime.Now;
                    pCommissionVO.CreatedBy = cProfile.CustomerId;
                    pCommissionVO.Commission = remainCommission;
                    pCommissionVO.PayDate = DateTime.Now;
                    pCommissionVO.ProjectId = projectId;
                    pCommissionVO.Status = 3;                                                  

                    uBO.AddProjectCommission(pCommissionVO);

                    ProjectViewVO pVOTemp = uBO.FindProjectById(projectId);

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
                    ConfigVO conVO = sBO.FindConfig();
                    CommissionVO commVO = new CommissionVO();
                    commVO.CommissionDate = DateTime.Now;
                    commVO.CommissionPercentage = conVO.CommissionPercentage;

                    IProjectDAO pDAO = ProjectManagementDAOFactory.CreateProjectDAO(new UserProfile());

                    commVO.ProjectCommission = pDAO.FindPlatformCommission(projectId);
                    commVO.ProjectId = projectId;
                    commVO.Status = 1;
                    sBO.AddCommission(commVO);


                    //扣除雇主委托酬金-剩余酬金
                    uBO.TrusteeshipFunds(projectId, cProfile.CustomerId, -remainCommission, "剩余酬金付款");
                    //扣除雇主委托酬金-平台抽佣
                    uBO.TrusteeshipFunds(projectId, cProfile.CustomerId, -platformCommission, "平台抽佣付款");

                    decimal btotal = uBO.FindCommisionDelegationTotal(projectId);//已托管金额

                    //如果托管金额还有剩余则返回给雇主钱包
                    if (btotal > 0) {
                        //扣除雇主委托酬金-剩余部分
                        if (uBO.TrusteeshipFunds(projectId, cProfile.CustomerId, -btotal, "剩余托管酬金返还钱包")) {
                            if (cBO.PlusBalance(cProfile.CustomerId, btotal))
                            {
                                CommissionIncomeVO ciVO = new CommissionIncomeVO();
                                ciVO.Commission = btotal;
                                ciVO.CustomerId = cProfile.CustomerId;
                                ciVO.PayDate = DateTime.Now;
                                ciVO.ProjectId = projectId;
                                ciVO.Purpose = "剩余托管酬金返还";
                                cBO.InsertCommissionIncome(ciVO);
                            }
                        }
                    }

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
            }
            //修改结束

            //修改前
            //bool isSuccess = uBO.ApproveProject(projectId, status, reason);

            if (isSuccess)
            {
                return new ResultObject() { Flag = 1, Message = "操作成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
            }
        }

        #region Project Action
        /// <summary>
        /// 添加或更新项目跟进信息
        /// </summary>
        /// <param name="projectActionVO">项目跟进VO，根据ID确定添加或更新</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateProjectAction"), HttpPost]
        public ResultObject UpdateProjectAction([FromBody] ProjectActionVO projectActionVO, string token)
        {
            if (projectActionVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);

            ProjectBO uBO = new ProjectBO(new CustomerProfile());

            if (projectActionVO.ProjectActionId < 1)
            {
                projectActionVO.ActionDate = DateTime.Now;

                CustomerProfile cProfile = uProfile as CustomerProfile;
                projectActionVO.ActionBy = cProfile.CustomerId;

                int projectId = uBO.AddProjectAction(projectActionVO);
                if (projectId > 0)
                {
                    //发送站内消息
                    ProjectViewVO pViewVO = uBO.FindProjectById(projectActionVO.ProjectId);
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    BusinessBO bBO = new BusinessBO(new CustomerProfile());
                    BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);

                    AgencyBO aBO = new AgencyBO(new CustomerProfile());
                    AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                    mBO.SendMessage("项目进度", "  " + bViewVO.CustomerName + ":" + aViewVO.CustomerName + "为项目" + pViewVO.ProjectCode + "添加了一条跟进！", bViewVO.CustomerId, MessageType.Project);

                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = projectId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                projectActionVO.Restore("ActionDate");
                projectActionVO.Restore("ActionBy");
                bool isSuccess = uBO.UpdateProjectAction(projectActionVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 获取项目跟进信息
        /// </summary>
        /// <param name="projectActionId">项目跟进ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProjectAction"), HttpGet]
        public ResultObject GetProjectAction(int projectActionId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            ProjectActionVO uVO = uBO.FindProjectActionById(projectActionId);
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
        /// 获取项目跟进列表
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProjectActionByProject"), HttpGet]
        public ResultObject GetProjectActionByProject(int projectId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ProjectActionViewVO> uVO = uBO.FindProjectActionByProject(projectId);
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
        /// 批量删除项目跟进
        /// </summary>
        /// <param name="projectActionIds">项目跟进ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteProjectAction"), HttpGet]
        public ResultObject DeleteProjectAction(string projectActionIds, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            ProjectBO uBO = new ProjectBO((CustomerProfile)uProfile);
            try
            {
                if (!string.IsNullOrEmpty(projectActionIds))
                {
                    string[] bIdArr = projectActionIds.Split(',');
                    bool isAllUpdate = true;

                    for (int i = 0; i < bIdArr.Length; i++)
                    {
                        try
                        {
                            bool result = uBO.DeleteProjectAction(Convert.ToInt32(bIdArr[i]));

                        }
                        catch
                        {
                            isAllUpdate = false;
                        }
                    }
                    if (isAllUpdate)
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
        #endregion

        #region  Commission Delegation        
        /// <summary>
        /// 添加或更新酬金托管
        /// </summary>
        /// <param name="commissionDelegationVO">酬金托管VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateCommissionDelegation"), HttpPost]
        public ResultObject UpdateCommissionDelegation([FromBody] CommissionDelegationVO commissionDelegationVO, string token)
        {
            //先判断余额是否足够，如果不足够，返回告知要先充值
            //如果足够，进行扣除，并添加酬金托管数据
            if (commissionDelegationVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);

            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            ProjectViewVO pVO = uBO.FindProjectById(commissionDelegationVO.ProjectId);

            if (pVO.Status == 2) {
                return new ResultObject() { Flag = 0, Message = "项目已经完工，不能追加托管!", Result = null };
            }
            //已委托金额
            decimal total = uBO.FindCommisionDelegationTotal(commissionDelegationVO.ProjectId);

            SystemBO sBO = new SystemBO(new UserProfile());
            ConfigVO vo = sBO.FindConfig();
            decimal platformCommission = commissionDelegationVO.Commission * vo.CommissionPercentage / 100;//平台抽佣

            CustomerBO cBO = new CustomerBO(new CustomerProfile());
            if (total <= 0&& commissionDelegationVO.Commission< cBO.GetFirstMandates(pVO.Commission))
            {
                return new ResultObject() { Flag = 0, Message = "首次委托金额不能少于酬金的"+ cBO.GetFirstMandates() + "%！", Result = null };
            }

            if (commissionDelegationVO.CommissionDelegationId < 1)
            {
                commissionDelegationVO.PayDate = DateTime.Now;
                commissionDelegationVO.DelegationDate = DateTime.Now;
                commissionDelegationVO.Status = 0;
                commissionDelegationVO.Purpose = "项目酬金委托";

                if (!cBO.IsHasMoreBalance(commissionDelegationVO.CustomerId, commissionDelegationVO.Commission))
                {
                    return new ResultObject() { Flag = 2, Message = "余额不足，请先充值!", Result = null };
                }
                
                int deleId = uBO.AddCommissionDelegation(commissionDelegationVO);


                if (deleId > 0)
                {
                    //添加跟进信息
                    ProjectActionVO projectActionVO = new ProjectActionVO();
                    projectActionVO.ActionBy = commissionDelegationVO.CustomerId;
                    projectActionVO.ActionDate = DateTime.Now;
                    projectActionVO.ActionType = "System";
                    projectActionVO.Description = "雇主委托了"+ commissionDelegationVO.Commission + "元酬金";
                    projectActionVO.ProjectId = commissionDelegationVO.ProjectId;
                    uBO.AddProjectAction(projectActionVO);

                    //发送站内信息
                    ProjectViewVO pViewVO = uBO.FindProjectById(commissionDelegationVO.ProjectId);
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    AgencyBO aBO = new AgencyBO(new CustomerProfile());
                    AgencyViewVO bViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                    mBO.SendMessage("项目进度", "  " + bViewVO.CustomerName + ":雇主已经委托了酬金，请跟进！", bViewVO.CustomerId, MessageType.Project);


                    return new ResultObject() { Flag = 1, Message = "委托成功!", Result = deleId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "委托失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateCommissionDelegation(commissionDelegationVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 获取酬金托管信息
        /// </summary>
        /// <param name="commissionDelegationId">酬金托管ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetCommissionDelegation"), HttpGet]
        public ResultObject GetCommissionDelegation(int commissionDelegationId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            CommissionDelegationVO uVO = uBO.FindCommissionDelegationById(commissionDelegationId);
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
        /// 获取酬金托管列表
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetCommissionDelegationByProject"), HttpGet]
        public ResultObject GetCommissionDelegationByProject(int projectId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<CommissionDelegationVO> uVO = uBO.FindCommissionDelegationByProject(projectId);
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
        /// 获取项目已委托金额
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProjectDelegateCommission"), HttpGet]
        public ResultObject GetProjectDelegateCommission(int projectId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());

            decimal total = uBO.FindCommisionDelegationTotal(projectId);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = total };

        }  
        #endregion

        #region Project File
        /// <summary>
        /// 添加或更新项目文件
        /// </summary>
        /// <param name="projectFileVO">项目文件VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateProjectFile"), HttpPost]
        public ResultObject UpdateProjectFile([FromBody] ProjectFileVO projectFileVO, string token)
        {
            if (projectFileVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);

            ProjectBO uBO = new ProjectBO(new CustomerProfile());

            if (projectFileVO.ProjectFileId < 1)
            {
                projectFileVO.CreatedDate = DateTime.Now;
                int fileId = uBO.AddProjectFile(projectFileVO);
                if (fileId > 0)
                {
                    ProjectViewVO pViewVO = uBO.FindProjectById(projectFileVO.ProjectId);
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    BusinessBO bBO = new BusinessBO(new CustomerProfile());
                    BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);
                    AgencyBO aBO = new AgencyBO(new CustomerProfile());
                    AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);
                    int creator = (uProfile as CustomerProfile).CustomerId;

                    //添加跟进信息
                    ProjectActionVO projectActionVO = new ProjectActionVO();
                    projectActionVO.ActionBy = projectFileVO.CreatedBy;
                    projectActionVO.ActionDate = DateTime.Now;
                    projectActionVO.ActionType = "System";
                    if (creator == aViewVO.CustomerId)
                        projectActionVO.Description = aViewVO.CustomerName + "上传了附件！";
                    else if (creator == bViewVO.CustomerId)
                        projectActionVO.Description = bViewVO.CustomerName + "上传了附件！";
                    projectActionVO.ProjectId = projectFileVO.ProjectId;
                    uBO.AddProjectAction(projectActionVO);

                    //发送站内消息                    
                    if (creator == aViewVO.CustomerId)
                        mBO.SendMessage("项目进度", "  " + aViewVO.CustomerName + "为项目" + pViewVO.ProjectCode + "上传了附件！", bViewVO.CustomerId, MessageType.Project);
                    else if (creator == bViewVO.CustomerId)
                        mBO.SendMessage("项目进度", "  " + bViewVO.CustomerName + "为项目" + pViewVO.ProjectCode + "上传了附件！", aViewVO.CustomerId, MessageType.Project);

                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = fileId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateProjectFile(projectFileVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 获取项目文件信息
        /// </summary>
        /// <param name="projectFileId">项目文件ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProjectFile"), HttpGet]
        public ResultObject GetProjectFile(int projectFileId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            ProjectFileVO uVO = uBO.FindProjectFileById(projectFileId);
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
        /// 获取项目文件列表
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProjectFileByProject"), HttpGet]
        public ResultObject GetProjectFileByProject(int projectId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ProjectFileViewVO> uVO = uBO.FindProjectFileByProject(projectId);
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
        /// 批量删除项目文件
        /// </summary>
        /// <param name="projectFileIds">项目文件ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteProjectFile"), HttpGet]
        public ResultObject DeleteProjectFile(string projectFileIds, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            ProjectBO uBO = new ProjectBO((CustomerProfile)uProfile);
            try
            {
                if (!string.IsNullOrEmpty(projectFileIds))
                {
                    string[] bIdArr = projectFileIds.Split(',');
                    bool isAllUpdate = true;

                    for (int i = 0; i < bIdArr.Length; i++)
                    {
                        try
                        {
                            bool result = uBO.DeleteProjectFile(Convert.ToInt32(bIdArr[i]));

                        }
                        catch
                        {
                            isAllUpdate = false;
                        }
                    }
                    if (isAllUpdate)
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
        /// 添加或更新项目文件
        /// </summary>
        /// <param name="projectFileVO">项目文件VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateProjectReportFile"), HttpPost]
        public ResultObject UpdateProjectReportFile([FromBody] ProjectReportFileVO projectReportFileVO, string token)
        {
            if (projectReportFileVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);

            ProjectBO uBO = new ProjectBO(new CustomerProfile());

            if (projectReportFileVO.ProjectReportFileId < 1)
            {
                projectReportFileVO.CreatedDate = DateTime.Now;
                int fileId = uBO.AddProjectReportFile(projectReportFileVO);
                if (fileId > 0)
                {
                    ProjectViewVO pViewVO = uBO.FindProjectById(projectReportFileVO.ProjectId);
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    BusinessBO bBO = new BusinessBO(new CustomerProfile());
                    BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);
                    AgencyBO aBO = new AgencyBO(new CustomerProfile());
                    AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);
                    int creator = (uProfile as CustomerProfile).CustomerId;

                    //添加跟进信息
                    ProjectActionVO projectActionVO = new ProjectActionVO();
                    projectActionVO.ActionBy = projectReportFileVO.CreatedBy;
                    projectActionVO.ActionDate = DateTime.Now;
                    projectActionVO.ActionType = "System";

                    if (creator == aViewVO.CustomerId)
                        projectActionVO.Description = aViewVO.CustomerName + "上传了报表！";
                    else if (creator == bViewVO.CustomerId)
                        projectActionVO.Description = bViewVO.CustomerName + "上传了报表！";
                    projectActionVO.ProjectId = projectReportFileVO.ProjectId;
                    uBO.AddProjectAction(projectActionVO);

                    //发送站内消息                    
                    if (creator == aViewVO.CustomerId)
                        mBO.SendMessage("项目进度", "  " + aViewVO.CustomerName + "为项目" + pViewVO.ProjectCode + "上传了报表！", bViewVO.CustomerId, MessageType.Project);
                    else if (creator == bViewVO.CustomerId)
                        mBO.SendMessage("项目进度", "  " + bViewVO.CustomerName + "为项目" + pViewVO.ProjectCode + "上传了报表！", aViewVO.CustomerId, MessageType.Project);

                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = fileId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateProjectReportFile(projectReportFileVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }



        /// <summary>
        /// 获取项目报表文件信息
        /// </summary>
        /// <param name="projectReportFileId">项目报表文件ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProjectReportFile"), HttpGet]
        public ResultObject GetProjectReportFile(int projectReportFileId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            ProjectReportFileVO uVO = uBO.FindProjectReportFileById(projectReportFileId);
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
        /// 获取项目文报表件列表
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProjectReportFileByProject"), HttpGet]
        public ResultObject GetProjectReportFileByProject(int projectId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ProjectReportFileVO> uVO = uBO.FindProjectReportFileByProject(projectId);
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
        /// 批量删除项目报表文件
        /// </summary>
        /// <param name="projectReportFileIds">项目报表文件ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteProjectReportFile"), HttpGet]
        public ResultObject DeleteProjectReportFile(string projectReportFileIds, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            ProjectBO uBO = new ProjectBO((CustomerProfile)uProfile);
            try
            {
                if (!string.IsNullOrEmpty(projectReportFileIds))
                {
                    string[] bIdArr = projectReportFileIds.Split(',');
                    bool isAllUpdate = true;

                    for (int i = 0; i < bIdArr.Length; i++)
                    {
                        try
                        {
                            bool result = uBO.DeleteProjectReportFile(Convert.ToInt32(bIdArr[i]));

                        }
                        catch
                        {
                            isAllUpdate = false;
                        }
                    }
                    if (isAllUpdate)
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
        #endregion

        #region ProjectChange
        /// <summary>
        /// 雇主添加或更新申请更改酬金信息
        /// </summary>
        /// <param name="projectChangeVO">阶段付款VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateProjectChange"), HttpPost]
        public ResultObject UpdateProjectChange([FromBody] ProjectChangeVO projectChangeVO, string token)
        {
            if (projectChangeVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            CustomerProfile uProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            decimal remainCommission = uBO.FindRemainCommission(projectChangeVO.ProjectId);//剩余酬金
            ProjectViewVO pViewVO = uBO.FindProjectById(projectChangeVO.ProjectId);

            decimal payCommission = pViewVO.Commission - remainCommission;//已支付酬金

            if (pViewVO.BusinessCustomerId != uProfile.CustomerId)
            {
                return new ResultObject() { Flag = 0, Message = "只有本项目雇主才能申请更改酬金!", Result = null };
            }
            if (projectChangeVO.Commission <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "请输入正确的酬金金额!", Result = null };
            }
            if (projectChangeVO.Commission < payCommission)
            {
                return new ResultObject() { Flag = 0, Message = "总酬金金额不能少于已经支付销售的酬金!", Result = null };
            }

            if (projectChangeVO.ProjectChangeId < 1)
            {
                //删除之前销售没有确认的申请
                uBO.DeleteProjectChange(projectChangeVO.ProjectId);

                projectChangeVO.CreatedAt = DateTime.Now;
                projectChangeVO.Status = 0;
                projectChangeVO.CreatedBy = uProfile.CustomerId;
                int pcId = uBO.AddProjectChange(projectChangeVO);


                if (pcId > 0)
                {
                    //添加跟进信息
                    ProjectActionVO projectActionVO = new ProjectActionVO();
                    projectActionVO.ActionBy = projectChangeVO.CreatedBy;
                    projectActionVO.ActionDate = DateTime.Now;
                    projectActionVO.ActionType = "System";
                    projectActionVO.Description = "雇主申请将总酬金从"+ pViewVO.Commission+"元改为"+ projectChangeVO.Commission+"元";
                    projectActionVO.ProjectId = projectChangeVO.ProjectId;
                    uBO.AddProjectAction(projectActionVO);
                    //发送站内消息
                    
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    BusinessBO bBO = new BusinessBO(new CustomerProfile());
                    BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);

                    AgencyBO aBO = new AgencyBO(new CustomerProfile());
                    AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                    mBO.SendMessage("项目进度", "  " + aViewVO.CustomerName + ":" + bViewVO.CompanyName + "申请将总酬金从" + pViewVO.Commission + "元改为" + projectChangeVO.Commission + "元，请前往项目工作台确认。项目：" + pViewVO.ProjectCode + "！", aViewVO.CustomerId, MessageType.Project);

                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = pcId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateProjectChange(projectChangeVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }      
        }

        /// <summary>
        /// 销售拒绝或同意酬金更改
        /// </summary>
        /// <param name="projectChangeVO">申请VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateProjectChangeStatus"), HttpPost]
        public ResultObject UpdateProjectChangeStatus([FromBody] ProjectChangeVO projectChangeVO, string token)
        {
            ProjectBO uBO = new ProjectBO((CustomerProfile)CacheManager.GetUserProfile(token));
            CustomerProfile uProfile = (CustomerProfile)CacheManager.GetUserProfile(token);

            

            ProjectChangeVO pcVO = uBO.FindProjectChangeById(projectChangeVO.ProjectChangeId);
            ProjectViewVO pViewVO = uBO.FindProjectById(pcVO.ProjectId);
            decimal remainCommission = uBO.FindRemainCommission(pcVO.ProjectId);//剩余酬金
            decimal payCommission = pViewVO.Commission - remainCommission;//已支付酬金

            pcVO.RejectAt = DateTime.Now;
            pcVO.RejectBy = uProfile.CustomerId;
            pcVO.RejectReason = projectChangeVO.RejectReason;

            if (pViewVO.AgencyCustomerId != uProfile.CustomerId)
            {
                return new ResultObject() { Flag = 0, Message = "只有本项目销售才能同意更改酬金!", Result = null };
            }

            if (projectChangeVO.Status == 1)
            {
                if (pcVO.Commission <= 0)
                {
                    return new ResultObject() { Flag = 0, Message = "请输入正确的申请金额!", Result = null };
                }
                if (pcVO.Commission < payCommission)
                {
                    return new ResultObject() { Flag = 0, Message = "总酬金金额不能少于已经支付销售的酬金!", Result = null };
                }

                //更改项目酬金
                ProjectVO pVO = new ProjectVO();
                pVO.ProjectId = pcVO.ProjectId;
                pVO.Commission = pcVO.Commission;

                if (!uBO.UpdateProject(pVO))
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }

                pcVO.Status = 1;

                //添加跟进信息
                ProjectActionVO projectActionVO = new ProjectActionVO();
                projectActionVO.ActionBy = pcVO.RejectBy;
                projectActionVO.ActionDate = DateTime.Now;
                projectActionVO.ActionType = "System";
                projectActionVO.Description = "销售同意将总酬金从" + pViewVO.Commission + "元改为" + pcVO.Commission + "元";
                projectActionVO.ProjectId = pcVO.ProjectId;
                uBO.AddProjectAction(projectActionVO);
                //发送站内消息

                MessageBO mBO = new MessageBO(new CustomerProfile());
                BusinessBO bBO = new BusinessBO(new CustomerProfile());
                BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);

                AgencyBO aBO = new AgencyBO(new CustomerProfile());
                AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                mBO.SendMessage("项目进度", "  " + bViewVO.CustomerName + ":" + aViewVO.CustomerName + "同意将总酬金从" + pViewVO.Commission + "元改为" + pcVO.Commission + "元，项目：" + pViewVO.ProjectCode + "！", aViewVO.CustomerId, MessageType.Project);
            }
            else if (projectChangeVO.Status == 2)
            {
                pcVO.Status = 2;

                //添加跟进信息
                ProjectActionVO projectActionVO = new ProjectActionVO();
                projectActionVO.ActionBy = pcVO.RejectBy;
                projectActionVO.ActionDate = DateTime.Now;
                projectActionVO.ActionType = "System";
                projectActionVO.Description = "销售拒绝将总酬金从" + pViewVO.Commission + "元改为" + pcVO.Commission + "元";
                projectActionVO.ProjectId = pcVO.ProjectId;
                uBO.AddProjectAction(projectActionVO);
                //发送站内消息

                MessageBO mBO = new MessageBO(new CustomerProfile());
                BusinessBO bBO = new BusinessBO(new CustomerProfile());
                BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);

                AgencyBO aBO = new AgencyBO(new CustomerProfile());
                AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                mBO.SendMessage("项目进度", "  " + bViewVO.CustomerName + ":" + aViewVO.CustomerName + "拒绝将总酬金从" + pViewVO.Commission + "元改为" + pcVO.Commission + "元，项目：" + pViewVO.ProjectCode + "！", aViewVO.CustomerId, MessageType.Project);
            }
            else {
                pcVO.Status = projectChangeVO.Status;
            }



            bool isSuccess = uBO.UpdateProjectChange(pcVO);

            if (isSuccess)
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };

        }

        /// <summary>
        /// 获取更改酬金的请求
        /// </summary>
        /// <param name="ProjectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProjectChange"), HttpGet]
        public ResultObject GetProjectChange(int ProjectId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ProjectChangeVO> pVO = uBO.FindProjectChangeByProject(ProjectId, "Status=0");
            if (pVO.Count>0)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = pVO[0] };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }
        #endregion

        #region ProjectRefund
        /// <summary>
        /// 雇主添加或更新申请关闭项目信息
        /// </summary>
        /// <param name="projectrefundVO">申请关闭VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateProjectRefund"), HttpPost]
        public ResultObject UpdateProjectRefund([FromBody] ProjectRefundVO projectrefundVO, string token)
        {
            if (projectrefundVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            CustomerProfile uProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            ProjectViewVO pViewVO = uBO.FindProjectById(projectrefundVO.ProjectId);

            if (pViewVO.BusinessCustomerId != uProfile.CustomerId)
            {
                return new ResultObject() { Flag = 0, Message = "只有本项目雇主才能申请关闭项目!", Result = null };
            }
            if (projectrefundVO.ProjectRefundId < 1)
            {
                //删除之前销售没有确认的申请
                uBO.DeleteProjectRefund(projectrefundVO.ProjectId);

                projectrefundVO.CreatedAt = DateTime.Now;
                projectrefundVO.Status = 0;
                projectrefundVO.CreatedBy = uProfile.CustomerId;
                int pcId = uBO.AddProjectRefund(projectrefundVO);


                if (pcId > 0)
                {
                    //添加跟进信息
                    ProjectActionVO projectActionVO = new ProjectActionVO();
                    projectActionVO.ActionBy = projectrefundVO.CreatedBy;
                    projectActionVO.ActionDate = DateTime.Now;
                    projectActionVO.ActionType = "System";
                    projectActionVO.Description = "雇主申请关闭本项目";
                    projectActionVO.ProjectId = projectrefundVO.ProjectId;
                    uBO.AddProjectAction(projectActionVO);
                    //发送站内消息

                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    BusinessBO bBO = new BusinessBO(new CustomerProfile());
                    BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);

                    AgencyBO aBO = new AgencyBO(new CustomerProfile());
                    AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                    mBO.SendMessage("项目进度", "  " + aViewVO.CustomerName + ":雇主" + bViewVO.CompanyName + "申请关闭项目，请前往项目工作台进行处理。如果您7天以内没有进行同意或拒绝操作，雇主将有权单方面关闭项目。项目关闭您将拿不到剩余酬金。项目：" + pViewVO.ProjectCode + "！", aViewVO.CustomerId, MessageType.Project);
                    string msgContent = "尊敬的" + aViewVO.CustomerName + "：雇主" + bViewVO.CompanyName + "申请关闭项目，请前往项目工作台进行处理。如果您7天以内没有进行同意或拒绝操作，雇主将有权单方面关闭项目。项目关闭您将拿不到剩余酬金。项目：" + pViewVO.ProjectCode + "！【众销乐 -资源共享众包销售平台】";
                    string result = MessageTool.SendMobileMsg(msgContent, aViewVO.Phone);

                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = pcId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
        }

        /// <summary>
        /// 销售拒绝或同意关闭项目
        /// </summary>
        /// <param name="projectrefundVO">申请VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateProjectRefundStatus"), HttpPost]
        public ResultObject UpdateProjectRefundStatus([FromBody] ProjectRefundVO projectrefundVO, string token)
        {
            ProjectBO uBO = new ProjectBO((CustomerProfile)CacheManager.GetUserProfile(token));
            CustomerProfile uProfile = (CustomerProfile)CacheManager.GetUserProfile(token);



            ProjectRefundVO pcVO = uBO.FindProjectRefundById(projectrefundVO.ProjectRefundId);
            ProjectViewVO pViewVO = uBO.FindProjectById(pcVO.ProjectId);

            pcVO.RejectAt = DateTime.Now;
            pcVO.RejectBy = uProfile.CustomerId;
            pcVO.RejectReason = projectrefundVO.RejectReason;

            if (pViewVO.AgencyCustomerId != uProfile.CustomerId)
            {
                return new ResultObject() { Flag = 0, Message = "只有本项目销售才能同意关闭项目!", Result = null };
            }

            if (projectrefundVO.Status == 1)
            {
                //更改项目酬金
                ProjectVO pVO = new ProjectVO();
                pVO.ProjectId = pcVO.ProjectId;
                pVO.Status = 2;

                if (!uBO.UpdateProject(pVO))
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
                else {
                    decimal total = uBO.FindCommisionDelegationTotal(pcVO.ProjectId);//已托管金额
                    if (total > 0)
                    {
                        //扣除雇主委托酬金-剩余部分
                        if (uBO.TrusteeshipFunds(pcVO.ProjectId, pViewVO.BusinessCustomerId, -total, "剩余托管酬金返还钱包"))
                        {
                            CustomerBO cBO = new CustomerBO(new CustomerProfile());
                            if (cBO.PlusBalance(pViewVO.BusinessCustomerId, total))
                            {
                                CommissionIncomeVO ciVO = new CommissionIncomeVO();
                                ciVO.Commission = total;
                                ciVO.CustomerId = pViewVO.BusinessCustomerId;
                                ciVO.PayDate = DateTime.Now;
                                ciVO.ProjectId = pcVO.ProjectId;
                                ciVO.Purpose = "剩余托管酬金返还";
                                cBO.InsertCommissionIncome(ciVO);
                            }
                        }
                    }
                }

                pcVO.Status = 1;

                //添加跟进信息
                ProjectActionVO projectActionVO = new ProjectActionVO();
                projectActionVO.ActionBy = pcVO.RejectBy;
                projectActionVO.ActionDate = DateTime.Now;
                projectActionVO.ActionType = "System";
                projectActionVO.Description = "销售同意关闭项目";
                projectActionVO.ProjectId = pcVO.ProjectId;
                uBO.AddProjectAction(projectActionVO);
                //发送站内消息

                MessageBO mBO = new MessageBO(new CustomerProfile());
                BusinessBO bBO = new BusinessBO(new CustomerProfile());
                BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);

                AgencyBO aBO = new AgencyBO(new CustomerProfile());
                AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                mBO.SendMessage("项目进度", "  " + bViewVO.CustomerName + ":" + aViewVO.CustomerName + "同意关闭项目，项目：" + pViewVO.ProjectCode + "！", bViewVO.CustomerId, MessageType.Project);
            }
            else if (projectrefundVO.Status == 2)
            {
                pcVO.Status = 2;

                //添加跟进信息
                ProjectActionVO projectActionVO = new ProjectActionVO();
                projectActionVO.ActionBy = pcVO.RejectBy;
                projectActionVO.ActionDate = DateTime.Now;
                projectActionVO.ActionType = "System";
                projectActionVO.Description = "销售拒绝关闭项目";
                projectActionVO.ProjectId = pcVO.ProjectId;
                uBO.AddProjectAction(projectActionVO);
                //发送站内消息

                MessageBO mBO = new MessageBO(new CustomerProfile());
                BusinessBO bBO = new BusinessBO(new CustomerProfile());
                BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);

                AgencyBO aBO = new AgencyBO(new CustomerProfile());
                AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                mBO.SendMessage("项目进度", "  " + bViewVO.CustomerName + ":" + aViewVO.CustomerName + "拒绝关闭项目！", bViewVO.CustomerId, MessageType.Project);
            }
            else
            {
                pcVO.Status = projectrefundVO.Status;
            }
            bool isSuccess = uBO.UpdateProjectRefund(pcVO);

            if (isSuccess)
            {
                if (pcVO.Status == 1)
                    return new ResultObject() { Flag = 1, Message = "同意成功，已关闭项目!", Result = null };
                if (pcVO.Status == 2)
                    return new ResultObject() { Flag = 1, Message = "已拒绝关闭!", Result = null };
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            }
            else
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };

        }

        /// <summary>
        /// 雇主单方面关闭项目
        /// </summary>
        /// <param name="ProjectRefundId">申请Id</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("BusinessUpdateProjectRefundStatus"), HttpPost]
        public ResultObject BusinessUpdateProjectRefundStatus(int ProjectRefundId, string token)
        {
            ProjectBO uBO = new ProjectBO((CustomerProfile)CacheManager.GetUserProfile(token));
            CustomerProfile uProfile = (CustomerProfile)CacheManager.GetUserProfile(token);

            ProjectRefundVO pcVO = uBO.FindProjectRefundById(ProjectRefundId);
            ProjectViewVO pViewVO = uBO.FindProjectById(pcVO.ProjectId);

            pcVO.RejectAt = DateTime.Now;
            pcVO.RejectBy = uProfile.CustomerId;
            pcVO.Status = 1;



            //更改项目酬金
            ProjectVO pVO = new ProjectVO();
            pVO.ProjectId = pcVO.ProjectId;
            pVO.Status = 2;

            DateTime dt = pcVO.CreatedAt;
            dt = dt.AddDays(+7);
            if (DateTime.Now < dt) {
                return new ResultObject() { Flag = 0, Message = "更新失败!未到规定时间不能单方面取消", Result = null };
            }

            if (!uBO.UpdateProject(pVO))
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
            else
            {
                uBO.UpdateProjectRefund(pcVO);
                decimal total = uBO.FindCommisionDelegationTotal(pcVO.ProjectId);//已托管金额
                if (total > 0)
                {
                    //扣除雇主委托酬金-剩余部分
                    if (uBO.TrusteeshipFunds(pcVO.ProjectId, pViewVO.BusinessCustomerId, -total, "剩余托管酬金返还钱包"))
                    {
                        CustomerBO cBO = new CustomerBO(new CustomerProfile());
                        if (cBO.PlusBalance(pViewVO.BusinessCustomerId, total))
                        {
                            CommissionIncomeVO ciVO = new CommissionIncomeVO();
                            ciVO.Commission = total;
                            ciVO.CustomerId = pViewVO.BusinessCustomerId;
                            ciVO.PayDate = DateTime.Now;
                            ciVO.ProjectId = pcVO.ProjectId;
                            ciVO.Purpose = "剩余托管酬金返还";
                            cBO.InsertCommissionIncome(ciVO);
                        }
                    }
                }
            }

            pcVO.Status = 1;

            //添加跟进信息
            ProjectActionVO projectActionVO = new ProjectActionVO();
            projectActionVO.ActionBy = pcVO.RejectBy;
            projectActionVO.ActionDate = DateTime.Now;
            projectActionVO.ActionType = "System";
            projectActionVO.Description = "雇主关闭了项目";
            projectActionVO.ProjectId = pcVO.ProjectId;
            uBO.AddProjectAction(projectActionVO);
            //发送站内消息

            MessageBO mBO = new MessageBO(new CustomerProfile());
            BusinessBO bBO = new BusinessBO(new CustomerProfile());
            BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);

            AgencyBO aBO = new AgencyBO(new CustomerProfile());
            AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

            mBO.SendMessage("项目进度", "  " + aViewVO.CustomerName + ":雇主关闭了项目，项目：" + pViewVO.ProjectCode + "！", aViewVO.CustomerId, MessageType.Project);

            return new ResultObject() { Flag = 1, Message = "成功关闭!", Result = null };

        }

        /// <summary>
        /// 获取关闭项目的请求
        /// </summary>
        /// <param name="ProjectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProjectRefund"), HttpGet]
        public ResultObject GetProjectRefund(int ProjectId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ProjectRefundVO> pVO = uBO.FindProjectRefundByProject(ProjectId, "Status=0");
            if (pVO.Count > 0)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = pVO[0] };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }
        #endregion

        #region Project Refunds
        /// <summary>
        /// 添加或更新退款信息
        /// </summary>
        /// <param name="projectRefundsVO">退款VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateProjectRefunds"), HttpPost]
        public ResultObject UpdateProjectRefunds([FromBody] ProjectRefundsVO projectRefundsVO, string token)
        {
            if (projectRefundsVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);

            ProjectBO uBO = new ProjectBO(new CustomerProfile());

            if (projectRefundsVO.ProjectRefundsId < 1)
            {

                int refId = uBO.AddProjectRefunds(projectRefundsVO);
                if (refId > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = refId };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateProjectRefunds(projectRefundsVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 获取退款信息
        /// </summary>
        /// <param name="projectRefundsId">退款ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProjectRefunds"), HttpGet]
        public ResultObject GetProjectRefunds(int projectRefundsId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            ProjectRefundsVO uVO = uBO.FindProjectRefundsById(projectRefundsId);
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
        /// 获取退款列表
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProjectRefundsByProject"), HttpGet]
        public ResultObject GetProjectRefundsByProject(int projectId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ProjectRefundsViewVO> uVO = uBO.FindProjectRefundsByProject(projectId);
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
        /// 删除退款信息
        /// </summary>
        /// <param name="projectRefundsId">退款ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteProjectRefunds"), HttpGet]
        public ResultObject DeleteProjectRefunds(int projectRefundsId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            bool result = uBO.DeleteProjectRefunds(projectRefundsId);
            if (result)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }
        #endregion 

        #region Project Commission
        /// <summary>
        /// 添加或更新项目阶段付款信息
        /// </summary>
        /// <param name="projectCommissionVO">阶段付款VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateProjectCommission"), HttpPost]
        public ResultObject UpdateProjectCommission([FromBody] ProjectCommissionVO projectCommissionVO, string token)
        {
            if (projectCommissionVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            decimal remainCommission = uBO.FindRemainCommission(projectCommissionVO.ProjectId);

            if (projectCommissionVO.Commission <= 0)
            {
                return new ResultObject() { Flag = 0, Message = "请输入正确的申请金额!", Result = null };
            }
            if (projectCommissionVO.Commission > remainCommission) {
                return new ResultObject() { Flag = 0, Message = "申请金额不能超过当前剩余酬金!", Result = null };
            }

            if (projectCommissionVO.ProjectCommissionId < 1)
            {
                projectCommissionVO.CreatedAt = DateTime.Now;
                int pcId = uBO.AddProjectCommission(projectCommissionVO);


                if (pcId > 0)
                {
                    //添加跟进信息
                    ProjectActionVO projectActionVO = new ProjectActionVO();
                    projectActionVO.ActionBy = projectCommissionVO.CreatedBy;
                    projectActionVO.ActionDate = DateTime.Now;
                    projectActionVO.ActionType = "System";
                    projectActionVO.Description = "销售付款申请";
                    projectActionVO.ProjectId = projectCommissionVO.ProjectId;
                    uBO.AddProjectAction(projectActionVO);
                    //发送站内消息
                    ProjectViewVO pViewVO = uBO.FindProjectById(projectCommissionVO.ProjectId);
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    BusinessBO bBO = new BusinessBO(new CustomerProfile());
                    BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);

                    AgencyBO aBO = new AgencyBO(new CustomerProfile());
                    AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                    mBO.SendMessage("项目进度", "  " + bViewVO.CustomerName + ":" + aViewVO.CustomerName + "申请付款，项目：" + pViewVO.ProjectCode + "！", bViewVO.CustomerId, MessageType.Project);

                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = pcId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateProjectCommission(projectCommissionVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 获取阶段付款信息
        /// </summary>
        /// <param name="projectCommissionId">阶段付款ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProjectCommission"), HttpGet]
        public ResultObject GetProjectCommission(int projectCommissionId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            ProjectCommissionVO uVO = uBO.FindProjectCommissionById(projectCommissionId);
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
        /// 获取阶段付款列表
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProjectCommissionByProject"), HttpGet]
        public ResultObject GetProjectCommissionByProject(int projectId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ProjectCommissionViewVO> uVO = uBO.FindProjectCommissionByProject(projectId);
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
        /// 获取最新的付款申请
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetLatestProjectCommissionByProject"), HttpGet]
        public ResultObject GetLatestProjectCommissionByProject(int projectId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            ProjectCommissionViewVO pcVO = uBO.FindLatestProjectCommission(projectId);

            if (pcVO != null)
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = pcVO };
            else
                return new ResultObject() { Flag = 0, Message = "获取成功!", Result = null };

        }

        /// <summary>
        /// 获取剩余酬金
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetRemainCommissionByProject"), HttpGet]
        public ResultObject GetRemainCommissionByProject(int projectId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            decimal remainCommission = uBO.FindRemainCommission(projectId);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = remainCommission };

        }

        /// <summary>
        /// 拒绝或同意酬金支付
        /// </summary>
        /// <param name="projectCommissionVO">付款申请VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateProjectCommissionStatus"), HttpPost]
        public ResultObject UpdateProjectCommissionStatus([FromBody] ProjectCommissionVO projectCommissionVO, string token)
        {
            ProjectBO uBO = new ProjectBO((CustomerProfile)CacheManager.GetUserProfile(token));
            decimal remainCommission = uBO.FindRemainCommission(projectCommissionVO.ProjectId);

            ProjectCommissionVO pcVO = uBO.FindProjectCommissionById(projectCommissionVO.ProjectCommissionId);

            if (projectCommissionVO.Status == 3)
            {
                if (pcVO.Commission <= 0)
                {
                    return new ResultObject() { Flag = 0, Message = "请输入正确的申请金额!", Result = null };
                }
                if (pcVO.Commission > remainCommission)
                {
                    return new ResultObject() { Flag = 0, Message = "申请金额不能超过当前剩余酬金!", Result = null };
                }

                decimal total = uBO.FindCommisionDelegationTotal(pcVO.ProjectId);//已委托金额

                if (pcVO.Commission > total)
                {
                    return new ResultObject() { Flag = 0, Message = "当前已托管酬金不足以支付本次申请金额，请追加托管!", Result = null };
                }


                CustomerProfile uProfile = (CustomerProfile)CacheManager.GetUserProfile(token);

                if (!uBO.TrusteeshipFunds(projectCommissionVO.ProjectId, uProfile.CustomerId,-pcVO.Commission,"阶段付款扣除")) {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            


            bool isSuccess = uBO.UpdateProjectCommissionStatus(projectCommissionVO);

            if (isSuccess) {
                CustomerProfile uProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
                CustomerBO _bo = new CustomerBO(new CustomerProfile());

                List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("支付酬金");
                if (_bo.ZXBFindRequireCount("CustomerId = " + uProfile.CustomerId + " and type=" + zVO[0].ZxbConfigID) == 0)
                {
                    //发放乐币奖励
                    _bo.ZXBAddrequire(uProfile.CustomerId, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID);
                }

                List<ZxbConfigVO> zVO2 = _bo.ZXBAddrequirebyCode("申请酬金");
                if (_bo.ZXBFindRequireCount("CustomerId = " + pcVO.CreatedBy + " and type=" + zVO2[0].ZxbConfigID) == 0)
                {
                    //发放乐币奖励
                    _bo.ZXBAddrequire(pcVO.CreatedBy, zVO2[0].Cost, zVO2[0].Purpose, zVO2[0].ZxbConfigID);
                }
                
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            } 
            else
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };

        }

        #endregion 

        #region Complaints
        /// <summary>
        /// 添加或更新项目维权信息
        /// </summary>
        /// <param name="complaintsModelVO">项目维权VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateComplaints"), HttpPost]
        public ResultObject UpdateComplaints([FromBody] ComplaintsModel complaintsModelVO, string token)
        {
            if (complaintsModelVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);

            ProjectBO uBO = new ProjectBO(new CustomerProfile());

            ComplaintsVO complaintsVO = complaintsModelVO.Complaints;
            List<ComplaintsImgVO> complaintsImgVOList = complaintsModelVO.ComplaintsImg;


            if (complaintsVO.ComplaintsId < 1)
            {
                complaintsVO.CreatedAt = DateTime.Now;
                int complaintsId = uBO.AddComplaints(complaintsVO, complaintsImgVOList);
                if (complaintsId > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = complaintsId };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateComplaints(complaintsVO, complaintsImgVOList);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 获取项目维权信息
        /// </summary>
        /// <param name="complaintsId"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetComplaints"), HttpGet]
        public ResultObject GetComplaints(int complaintsId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            ComplaintsViewVO uVO = uBO.FindComplaintsById(complaintsId);
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
        /// 获取项目维权列表
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetComplaintsByProject"), HttpGet]
        public ResultObject GetComplaintsByProject(int projectId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ComplaintsViewVO> uVO = uBO.FindComplaintsByProject(projectId);
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
        /// 获取项目维权图片列表
        /// </summary>
        /// <param name="complaintsId"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetComplaintsImgByComplaints"), HttpGet]
        public ResultObject GetComplaintsImgByComplaints(int complaintsId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ComplaintsImgVO> uVO = uBO.FindComplaintsImgByComplaints(complaintsId);
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
        /// 伤处维权信息
        /// </summary>
        /// <param name="complaintsId"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteComplaints"), HttpGet]
        public ResultObject DeleteComplaints(int complaintsId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            bool result = uBO.DeleteComplaints(complaintsId);
            if (result)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        /// <summary>
        /// 对项目是否已经申请过维权
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("IsExistsComplaints"), HttpGet]
        public ResultObject IsExistsComplaints(int projectId, string token)
        {
            CustomerProfile cProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            bool result = uBO.IsHasComplaints(projectId, cProfile.CustomerId);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = result };

        }

        /// <summary>
        /// 更新维权状态或者添加原因
        /// </summary>
        /// <param name="complaintsVO">项目维权VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateComplaintsStatus"), HttpPost]
        public ResultObject UpdateComplaintsStatus([FromBody] ComplaintsVO complaintsVO, string token)
        {
            if (complaintsVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }


            ProjectBO uBO = new ProjectBO(new CustomerProfile());

            if (complaintsVO.ComplaintsId < 1)
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateComplaints(complaintsVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        #endregion 

        #region AgencyReview
        /// <summary>
        /// 添加或更新销售评价（雇主评价，销售得分）
        /// </summary>
        /// <param name="agencyReviewModelVO">评价VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateAgencyReview"), HttpPost]
        public ResultObject UpdateAgencyReview([FromBody] AgencyReviewModel agencyReviewModelVO, string token)
        {
            if (agencyReviewModelVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            CustomerProfile uProfile = (CustomerProfile)CacheManager.GetUserProfile(token);

            ProjectBO uBO = new ProjectBO(new CustomerProfile());

            AgencyReviewVO agencyReviewVO = agencyReviewModelVO.AgencyReview;
            List<AgencyReviewDetailVO> agencyReviewDetailVOList = agencyReviewModelVO.AgencyReviewDetail;

            ProjectViewVO pVO = uBO.FindProjectById(agencyReviewVO.ProjectId);
            agencyReviewVO.RequirementId = pVO.RequirementId;
            agencyReviewVO.AgencyId = pVO.AgencyId;
            agencyReviewVO.BusinessId = pVO.BusinessId;

            decimal mainScore = 0;

            for (int i = 0; i < agencyReviewDetailVOList.Count; i++)
            {
                mainScore += agencyReviewDetailVOList[i].Score;
            }

            if (agencyReviewDetailVOList.Count > 0)
                mainScore = mainScore / agencyReviewDetailVOList.Count;

            agencyReviewVO.Score = mainScore;

            if (agencyReviewVO.AgencyReviewId < 1)
            {
                agencyReviewVO.CreatedAt = DateTime.Now;
                int agencyReviewId = uBO.AddAgencyReview(agencyReviewVO, agencyReviewDetailVOList);
                if (agencyReviewId > 0)
                {
                    CustomerBO _bo = new CustomerBO(new CustomerProfile());
                    List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("任务完工");
                    _bo.ZXBAddrequire(uProfile.CustomerId, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID);

                    List<ZxbConfigVO> zVO2 = _bo.ZXBAddrequirebyCode("收到雇主评价");
                    AgencyBO aBO = new AgencyBO(new CustomerProfile());
                    AgencyViewVO aViewVO = aBO.FindAgencyById(agencyReviewVO.AgencyId);

                    if (_bo.ZXBFindRequireCount("CustomerId = " + aViewVO.CustomerId + " and type=" + zVO2[0].ZxbConfigID) == 0)
                    {
                        //发放乐币奖励
                        _bo.ZXBAddrequire(aViewVO.CustomerId, zVO2[0].Cost, zVO2[0].Purpose, zVO2[0].ZxbConfigID);
                    }
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = agencyReviewId };
                }   
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateAgencyReview(agencyReviewVO, agencyReviewDetailVOList);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 追加评价。销售对雇主追加评价
        /// </summary>
        /// <param name="agencyReviewVO">追加内容</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("PlusAgencyReview"), HttpPost]
        public ResultObject PlusAgencyReview([FromBody] AgencyReviewVO agencyReviewVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);

            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            agencyReviewVO.AddNoteAt = DateTime.Now;
            bool isSuccess = uBO.UpdateAgencyReview(agencyReviewVO);
            if (isSuccess)
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
        }

        /// <summary>
        /// 获取销售评价信息
        /// </summary>
        /// <param name="agencyReviewId">评价ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyReview"), HttpGet]
        public ResultObject GetAgencyReview(int agencyReviewId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            AgencyReviewViewVO uVO = uBO.FindAgencyReviewById(agencyReviewId);
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
        /// 获取销售各项评价平均分
        /// </summary>
        /// <param name="agencyId">销售ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencySumReview"), HttpGet]
        public ResultObject GetAgencySumReview(int agencyId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<AgencySumReviewVO> uVO = uBO.FindAgencySumReviewByAgencyId(agencyId);
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
        /// 获取项目销售评价信息
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyReviewByProject"), HttpGet]
        public ResultObject GetAgencyReviewByProject(int projectId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<AgencyReviewVO> uVO = uBO.FindAgencyReviewByProject(projectId);
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
        /// 删除销售的评价
        /// </summary>
        /// <param name="agencyReviewId">评价ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteAgencyReview"), HttpGet]
        public ResultObject DeleteAgencyReview(int agencyReviewId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            bool result = uBO.DeleteAgencyReview(agencyReviewId);
            if (result)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        #endregion 

        #region BusinessReview
        /// <summary>
        /// 添加或更新雇主评价（销售评价，雇主得分）
        /// </summary>
        /// <param name="businessReviewModelVO"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateBusinessReview"), HttpPost]
        public ResultObject UpdateBusinessReview([FromBody] BusinessReviewModel businessReviewModelVO, string token)
        {
            if (businessReviewModelVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            CustomerProfile uProfile = (CustomerProfile)CacheManager.GetUserProfile(token);

            ProjectBO uBO = new ProjectBO(new CustomerProfile());

            BusinessReviewVO businessReviewVO = businessReviewModelVO.BusinessReview;
            List<BusinessReviewDetailVO> businessReviewDetailVOList = businessReviewModelVO.BusinessReviewDetail;

            ProjectViewVO pVO = uBO.FindProjectById(businessReviewVO.ProjectId);
            businessReviewVO.RequirementId = pVO.RequirementId;
            businessReviewVO.AgencyId = pVO.AgencyId;
            businessReviewVO.BusinessId = pVO.BusinessId;

            decimal mainScore = 0;

            for (int i = 0; i < businessReviewDetailVOList.Count; i++)
            {
                mainScore += businessReviewDetailVOList[i].Score;
            }

            if (businessReviewDetailVOList.Count > 0)
                mainScore = mainScore / businessReviewDetailVOList.Count;

            businessReviewVO.Score = mainScore;

            if (businessReviewVO.BusinessReviewId < 1)
            {
                businessReviewVO.CreatedAt = DateTime.Now;
                int businessReviewId = uBO.AddBusinessReview(businessReviewVO, businessReviewDetailVOList);
                if (businessReviewId > 0)
                {
                    CustomerBO _bo = new CustomerBO(new CustomerProfile());
                    List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("任务完工");
                    _bo.ZXBAddrequire(uProfile.CustomerId, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID);

                    List<ZxbConfigVO> zVO2 = _bo.ZXBAddrequirebyCode("收到销售评价");
                    BusinessBO bBO = new BusinessBO(new CustomerProfile());
                    BusinessViewVO bViewVO = bBO.FindBusinessById(businessReviewVO.BusinessId);

                    if (_bo.ZXBFindRequireCount("CustomerId = " + bViewVO.CustomerId + " and type=" + zVO2[0].ZxbConfigID) == 0)
                    {
                        //发放乐币奖励
                        _bo.ZXBAddrequire(bViewVO.CustomerId, zVO2[0].Cost, zVO2[0].Purpose, zVO2[0].ZxbConfigID);
                    }
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = businessReviewId };
                }   
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateBusinessReview(businessReviewVO, businessReviewDetailVOList);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 追加评价。雇主对销售追加评价
        /// </summary>
        /// <param name="businessReviewVO">追加内容</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("PlusBusinessReview"), HttpPost]
        public ResultObject PlusBusinessReview([FromBody] BusinessReviewVO businessReviewVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);

            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            businessReviewVO.AddNoteAt = DateTime.Now;
            bool isSuccess = uBO.UpdateBusinessReview(businessReviewVO);
            if (isSuccess)
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
        }

        /// <summary>
        /// 解释。雇主对销售的解释
        /// </summary>
        /// <param name="businessReviewVO">追加内容</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ExpanationBusinessReview"), HttpPost]
        public ResultObject ExpanationBusinessReview([FromBody] BusinessReviewVO businessReviewVO, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);

            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            businessReviewVO.ExplanationAt = DateTime.Now;
            bool isSuccess = uBO.UpdateBusinessReview(businessReviewVO);
            if (isSuccess)
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
        }

        /// <summary>
        /// 获取雇主评价
        /// </summary>
        /// <param name="businessReviewId"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessReview"), HttpGet]
        public ResultObject GetBusinessReview(int businessReviewId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            BusinessReviewViewVO uVO = uBO.FindBusinessReviewById(businessReviewId);
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
        /// 获取项目雇主评价
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessReviewByProject"), HttpGet]
        public ResultObject GetBusinessReviewByProject(int projectId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<BusinessReviewVO> uVO = uBO.FindBusinessReviewByProject(projectId);
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
        /// 获取项目雇主各项评价平均分
        /// </summary>
        /// <param name="BusinessId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessSumReviewByBusinessId"), HttpGet]
        public ResultObject GetBusinessSumReviewByBusinessId(int BusinessId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<BusinessSumReviewVO> uVO = uBO.FindBusinessSumReviewByBusinessId(BusinessId);
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
        /// 获取系统所有销售各项评价平均分 匿名
        /// </summary>     

        /// <returns></returns>
        [Route("FindAllAgencyAverageScoreView"), HttpGet, Anonymous]
        public ResultObject FindAllAgencyAverageScoreView()
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<AllAgencyAverageScoreViewVO> uVO = uBO.FindAllAgencyAverageScoreView();
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
        /// 获取系统所有雇主各项评价平均分 匿名
        /// </summary>            
        /// <returns></returns>
        [Route("FindAllBusinessAverageScoreView"), HttpGet, Anonymous]
        public ResultObject FindAllBusinessAverageScoreView()
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<AllBusinessAverageScoreViewVO> uVO = uBO.FindAllBusinessAverageScoreView();
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
        /// 删除雇主评价信息
        /// </summary>
        /// <param name="businessReviewId">评价Id</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteBusinessReview"), HttpGet]
        public ResultObject DeleteBusinessReview(int businessReviewId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            bool result = uBO.DeleteBusinessReview(businessReviewId);
            if (result)
            {
                return new ResultObject() { Flag = 1, Message = "删除成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }

        #endregion 

        /// <summary>
        /// 获取微信支付数据
        /// </summary>
        /// <param name="openId">微信openId</param>
        /// <param name="total_fee">商品价格</param>
        /// <param name="body">商品描述</param>
        /// <param name="attach">附加数据</param>
        /// <param name="goods">商品标记</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("GetUnifiedOrderResult"), HttpGet]
        public ResultObject GetUnifiedOrderResult(string openId, string total_fee, string body, string attach, string goods, string token)
        {
            CustomerProfile uProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            JsApiPay Ja = new JsApiPay();
            string out_trade_no = GenerateOutTradeNoWx();
            string total_fee_1 = (Convert.ToDecimal(total_fee) * 100).ToString();
            string productId = GenerateOutTradeNo("WX");
            
            CustomerBO _bo = new CustomerBO(new CustomerProfile());
            PayinHistoryVO vo = new PayinHistoryVO();
            vo.CustomerId = uProfile.CustomerId;
            vo.Cost = Convert.ToDecimal(total_fee);
            vo.PayInOrder = productId;
            vo.PayInStatus = 0;
            vo.ThirdOrder = out_trade_no;
            vo.PayInDate = DateTime.Now;
            vo.Purpose = "微信充值";
            WxPayData wp = Ja.GetUnifiedOrderResult(out_trade_no, openId, total_fee_1, body, attach, goods);
            if (wp != null)
            {
                string reslut = Ja.GetJsApiParameters(wp);
                if (reslut != "")
                {
                    vo.PayInStatus = 1;
                    _bo.InsertPayinHistory(vo);
                    return new ResultObject() { Flag = 1, Message = "成功", Result = reslut };
                }
                else
                {
                    _bo.InsertPayinHistory(vo);
                    return new ResultObject() { Flag = 0, Message = "失败", Result = null };
                }
            }
            else
            {
                _bo.InsertPayinHistory(vo);
                return new ResultObject() { Flag = 0, Message = "失败", Result = null };
            }
        }

        /// <summary>
        /// 获取微信支付数据
        /// </summary>
        /// <param name="appid">小程序ID</param>
        /// <param name="openId">微信openId</param>
        /// <param name="total_fee">商品价格</param>
        /// <param name="body">商品描述</param>
        /// <param name="attach">附加数据</param>
        /// <param name="goods">商品标记</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("GetUnifiedOrderResult"), HttpGet]
        public ResultObject GetUnifiedOrderResult(string appid, string openId, string total_fee, string body, string attach, string goods, string token)
        {
            CustomerProfile uProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            JsApiPay Ja = new JsApiPay();
            string out_trade_no = GenerateOutTradeNoWx();
            string total_fee_1 = Convert.ToInt32((Convert.ToDecimal(total_fee) * 100)).ToString();
            string productId = GenerateOutTradeNo("WX");

            CustomerBO _bo = new CustomerBO(new CustomerProfile());
            PayinHistoryVO vo = new PayinHistoryVO();
            vo.CustomerId = uProfile.CustomerId;
            vo.Cost = Convert.ToDecimal(total_fee);
            vo.PayInOrder = productId;
            vo.PayInStatus = 0;
            vo.ThirdOrder = out_trade_no;
            vo.PayInDate = DateTime.Now;
            vo.Purpose = "微信充值";
            WxPayData wp = Ja.GetUnifiedOrderResult(appid,out_trade_no, openId, total_fee_1, body, attach, goods);
            if (wp != null)
            {
                string reslut = Ja.GetJsApiParameters(wp);
                if (reslut != "")
                {
                    //vo.PayInStatus = 1;
                    _bo.InsertPayinHistory(vo);
                    return new ResultObject() { Flag = 1, Message = "成功", Result = reslut };
                }
                else
                {
                    _bo.InsertPayinHistory(vo);
                    return new ResultObject() { Flag = 0, Message = "失败", Result = null };
                }
            }
            else
            {
                _bo.InsertPayinHistory(vo);
                return new ResultObject() { Flag = 0, Message = "失败", Result = null };
            }
        }

        /// <summary>
        /// 获取微信支付数据
        /// </summary>

        /// <param name="total_fee">商品价格</param>  
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("GetUnifiedOrderData"), HttpGet]
        public ResultObject GetUnifiedOrderData( string total_fee, string token)
        {
            CustomerProfile uProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
            JsApiPay Ja = new JsApiPay();
            string out_trade_no = GenerateOutTradeNoWx();
            string total_fee_1 = Convert.ToInt32((Convert.ToDecimal(total_fee) * 100)).ToString();
            string productId = GenerateOutTradeNo("WX");

            string body = "众销乐 - 资源共享众包销售平台客户钱包充值";
            string attach = "钱包充值";
            string goods = "钱包充值";

            CustomerBO _bo = new CustomerBO(new CustomerProfile());
            PayinHistoryVO vo = new PayinHistoryVO();
            vo.CustomerId = uProfile.CustomerId;
            vo.Cost = Convert.ToDecimal(total_fee);
            vo.PayInOrder = productId;
            vo.PayInStatus = 0;
            vo.ThirdOrder = out_trade_no;
            vo.PayInDate = DateTime.Now;
            vo.Purpose = "微信充值";

            string wp = Ja.GetUnifiedOrderData(out_trade_no, total_fee_1, body, attach, goods);
            if (wp != null)
            {
                //string reslut = Ja.GetJsApiParameters(wp);
                if (wp != "")
                {
                    //vo.PayInStatus = 1;
                    _bo.InsertPayinHistory(vo);
                    return new ResultObject() { Flag = 1, Message = "成功", Result = wp };
                }
                else
                {
                    _bo.InsertPayinHistory(vo);
                    return new ResultObject() { Flag = 0, Message = "失败", Result = null };
                }
            }
            else
            {
                _bo.InsertPayinHistory(vo);
                return new ResultObject() { Flag = 0, Message = "失败", Result = null };
            }
        }

        /// <summary>
        /// 根据商品ID获取Code_URL，返回给网页显示成二维码
        /// </summary>
        /// <param name="out_trade_no">订单号</param>
        /// <param name="productId">商品ID</param>
        /// <param name="total_fee">金额</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetCodeURL"), HttpGet]
        public ResultObject GetCodeURL(string out_trade_no, string productId, string total_fee, string token)
        {
            string url = "";
            try
            {

                NativePay nativePay = new NativePay();
                url = nativePay.GetPayUrl(out_trade_no, productId, total_fee);

                return new ResultObject() { Flag = 1, Message = "成功", Result = url };
            }
            catch (Exception err)
            {

                return new ResultObject() { Flag = 0, Message = "失败", Result = err.Message + "\r\n" + err.StackTrace };
            }
        }


        public static string GenerateOutTradeNoWx()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string GenerateOutTradeNo(string type)
        {
            var ran = new Random();
            if (type == "Wx")
                return string.Format("{0}{1}{2}{3}", type, WxPayConfig.MCHID, DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
            else
                return string.Format("{0}{1}{2}{3}", type, config.app_id, DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="token">口令</param>
        /// <param name="subject">标题</param>   
        ///  <param name="out_trade_no">订单号</param>
        /// <param name="body">商品描述</param>
        /// <param name="total_amout">金额</param>
        /// <returns></returns>
        [Route("AliPagePay"), HttpGet]
        public ResultObject AliPagePay(string out_trade_no, string subject, string total_amout, string body, string token)
        {
            //生产充值订单号，插入充值记录表，状态是支付中
            out_trade_no = GenerateOutTradeNo("Ali");
            CustomerProfile up = (CustomerProfile)CacheManager.GetUserProfile(token);
            CustomerBO _bo = new CustomerBO(up);
            PayinHistoryVO vo = new PayinHistoryVO();
            vo.CustomerId = up.CustomerId;
            vo.Cost = Convert.ToDecimal(total_amout);
            vo.PayInOrder = out_trade_no;
            vo.PayInStatus = 0;
            vo.PayInDate = DateTime.Now;
            vo.Purpose = "支付宝充值";
            _bo.InsertPayinHistory(vo);

            DefaultAopClient client = new DefaultAopClient(config.gatewayUrl, config.app_id, config.private_key, "json", "1.0", config.sign_type, config.alipay_public_key, config.charset, false);

            // 外部订单号，商户网站订单系统中唯一的订单号
            // string out_trade_no = WIDout_trade_no.Text.Trim();

            // 订单名称
            // string subject = WIDsubject.Text.Trim();

            // 付款金额
            // string total_amout = WIDtotal_amount.Text.Trim();

            // 商品描述
            // string body = WIDbody.Text.Trim();

            // 组装业务参数model
            AlipayTradePagePayModel model = new AlipayTradePagePayModel();
            model.Body = body;
            model.Subject = subject;
            model.TotalAmount = total_amout;
            model.OutTradeNo = out_trade_no;
            model.ProductCode = "FAST_INSTANT_TRADE_PAY";

            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
            // 设置同步回调地址
            request.SetReturnUrl(config.ReturnUrl);
            // 设置异步通知接收地址
            request.SetNotifyUrl(config.NotifyUrl);
            // 将业务model载入到request
            request.SetBizModel(model);

            AlipayTradePagePayResponse response = null;
            string returns = "";
            try
            {
                response = client.pageExecute(request, null, "post");
                returns = response.Body;

                return new ResultObject() { Flag = 1, Message = "成功", Result = returns };
            }
            catch (Exception exp)
            {

                return new ResultObject() { Flag = 0, Message = " 失败", Result = exp.Message };
            }


        }


        /// <summary>
        /// 获取商家评论列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessReviewList"), HttpPost]
        public ResultObject GetBusinessReviewList([FromBody] ConditionModel condition, string token)
        {
            // CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
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
            ProjectBO uBO = new ProjectBO(cProfile);
            List<BusinessReviewViewVO> list = uBO.FindBusinessReviewByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };

        }
        /// <summary>
        /// 获取商家评论数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessReviewListCount"), HttpPost]
        public ResultObject GetBusinessReviewListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
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
            ProjectBO uBO = new ProjectBO(cProfile);
            int count = uBO.FindBusinessReviewTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }





        /// <summary>
        /// 获取经纪人评论列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyReviewList"), HttpPost]
        public ResultObject GetAgencyReviewList([FromBody] ConditionModel condition, string token)
        {
            // CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
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
            ProjectBO uBO = new ProjectBO(cProfile);
            List<AgencyReviewViewVO> list = uBO.FindAgencyReviewByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };

        }
        /// <summary>
        /// 获取经纪人评论数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyReviewListCount"), HttpPost]
        public ResultObject GetAgencyReviewListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
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
            ProjectBO uBO = new ProjectBO(cProfile);
            int count = uBO.FindAgencyReviewTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }


        /// <summary>
        /// 添加或更新三方协议
        /// </summary>
        /// <param name="contractModelVO">三方协议VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateContract"), HttpPost]
        public ResultObject UpdateContract([FromBody] ContractModel contractModelVO, string token)
        {
            if (contractModelVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            ContractVO contractVO = contractModelVO.Contract;
            
            List<ContractStepsVO> contractStepsVOList = contractModelVO.ContractSteps;
            List<ContractFileVO> contractFileVOList = contractModelVO.ContractFile;

            if (contractVO != null)
            {
                if (contractStepsVOList.Count > 0) {
                    decimal Commission = 0;
                    for (int i = 0; i < contractStepsVOList.Count; i++)
                    {
                        Commission += contractStepsVOList[i].Cost;
                    }
                    if(contractVO.Commission!= Commission)
                        return new ResultObject() { Flag = 0, Message = "项目酬金与阶段酬金总和不一致!", Result = null };
                }
                if (contractVO.ContractId < 1)
                {
                    contractVO.CreatedAt = DateTime.Now;
                    contractVO.AgencyStatus = 0;
                    contractVO.BusinessStatus = 0;
                    contractVO.Status = 1;
                    //contractVO.ContractNote = CacheSystemConfig.GetSystemConfig().ContractNote;

                    int contractId = uBO.AddContract(contractVO, contractStepsVOList, contractFileVOList);
                    if (contractId > 0)
                    {
                        CustomerBO _bo = new CustomerBO(new CustomerProfile());
                        CustomerProfile cProfile = uProfile as CustomerProfile;

                        List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("选中销售");
                        if (_bo.ZXBFindRequireCount("CustomerId = " + cProfile.CustomerId + " and type=" + zVO[0].ZxbConfigID) == 0)
                        {
                            //发放乐币奖励
                            _bo.ZXBAddrequire(cProfile.CustomerId, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID);
                        }

                        List<ZxbConfigVO> zVO2 = _bo.ZXBAddrequirebyCode("被选中");
                        if (_bo.ZXBFindRequireCount("CustomerId = " + contractVO.CustomerId + " and type=" + zVO2[0].ZxbConfigID) == 0)
                        {
                            //发放乐币奖励
                            _bo.ZXBAddrequire(contractVO.CustomerId, zVO2[0].Cost, zVO2[0].Purpose, zVO2[0].ZxbConfigID);
                        }
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = contractId };
                    }  
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
                else
                {
                    ContractViewVO contractVO2 = uBO.FindContractById(contractVO.ContractId);
                    if (contractVO2.AgencyStatus == 1)
                    {
                        return new ResultObject() { Flag = 0, Message = "销售已经同意合同，请让销售取消同意后再修改合同!", Result = null };
                    }
                    if (contractVO2.BusinessStatus == 1)
                    {
                        return new ResultObject() { Flag = 0, Message = "雇主已经同意合同，请让雇主取消同意后再修改合同!", Result = null };
                    }
                    bool isSuccess = uBO.UpdateContract(contractVO, contractStepsVOList, contractFileVOList);
                    if (isSuccess)
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = contractVO.ContractId };
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
        /// 获取三方协议
        /// </summary>
        /// <param name="contractId">三方协议ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetContract"), HttpGet]
        public ResultObject GetContract(int contractId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            ContractViewVO uVO = uBO.FindContractById(contractId);
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
        /// 更新三方协议状态
        /// </summary>
        /// <param name="contractId">三方协议ID</param>
        /// <param name="status">状态</param>
        /// <param name="type">A : Agency,B :Business</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateContractStatus"), HttpPost]
        public ResultObject UpdateContractStatus(int contractId, int status, string type, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            try
            {
                ProjectBO uBO = new ProjectBO(new CustomerProfile());
                CustomerBO cBO = new CustomerBO(new CustomerProfile());
                ContractVO bVO = new ContractVO();
                bVO.ContractId = contractId;
                ContractViewVO pViewVO = uBO.FindContractById(contractId);
                if (type == "A")
                {
                    AgencyBO aBO=new AgencyBO(new CustomerProfile());
                    AgencyViewVO agVO = aBO.FindAgencyById(pViewVO.AgencyId);
                    if (agVO.RealNameStatus != 1 && status==1)
                        return new ResultObject() { Flag = 3, Message = "您没有进行销售实名认证，不能签订合同!", Result = null };
                    bVO.AgencyStatus = status;
                    if (status == 1) {
                        bVO.AgencySignDate = DateTime.Now;
                    }
                }
                else if (type == "B")
                {
                    BusinessBO bBO = new BusinessBO(new CustomerProfile());
                    BusinessViewVO buVO = bBO.FindBusinessById(pViewVO.BusinessId);
                    if(buVO.RealNameStatus!= 1 && status == 1)
                        return new ResultObject() { Flag = 3, Message = "您没有进行雇主实名认证，不能签订合同!", Result = null };
                    //如果没有托管不能签订合同
                    RequireBO rBO = new RequireBO(new CustomerProfile());
                    decimal DelegateCommision = rBO.FindRequireDelegateCommisionTotal(pViewVO.RequirementId);//任务托管金额
                    if ( cBO.GetFirstMandates(pViewVO.Commission) > DelegateCommision) {
                        return new ResultObject() { Flag = 4, Message = "任务至少托管"+ cBO.GetFirstMandates() + "%酬金才能签订合同，请前往任务修改页面托管酬金!", Result = pViewVO.RequirementId };
                    }
                    bVO.BusinessStatus = status;
                    if (status == 1)
                    {
                        bVO.BusinessSignDate = DateTime.Now;
                    }
                }
                

                if (pViewVO.AgencyStatus == 1 && pViewVO.BusinessStatus == 1)
                {
                    //表示为已生成项目状态
                    if (status == 0)
                    {
                        //如果已经创建项目就不能取消
                        return new ResultObject() { Flag = 0, Message = "已经创建项目不能取消!", Result = null };

                        //判断项目是否已经有支付行为，如果有不做更新，如果没有继续操作
                        /*ProjectVO pVO = uBO.FindProjectByContract(contractId);
                        if(pVO != null)
                        {
                            if (uBO.IsHasPayed(pVO.ProjectId))
                            {
                                return new ResultObject() { Flag = 0, Message = "项目已经产生支付行为，不能取消!", Result = null };
                            }
                        }

                        //已签订合同，一方取消，两方同时取消。再次需要同时确认

                        bVO.AgencyStatus = status;
                        bVO.AgencyStatus = status;*/
                    }
                }

                bool isSuccess = uBO.UpdateContract(bVO);

                // 发送站内信息
                // 1. 如果销售确认签约，发消息给雇主
                // 2. 如果雇主确认签约，发消息给销售
                // 3. 如果双方都已经签约，自动创建项目，并发消息给双方
                // 4. 如果已创建项目，任何一方取消签约，更新项目状态，更新任务酬金，更新任务状态

                if (pViewVO.AgencyStatus == 1 && pViewVO.BusinessStatus == 1)
                {
                    //判断项目是否已经有支付行为，如果有不做更新，如果没有继续操作
                    if (type == "A")
                    {
                        pViewVO.AgencyStatus = status;
                    }
                    else if (type == "B")
                    {
                        pViewVO.BusinessStatus = status;
                    }
                        //表示为已生成项目状态
                    if (status == 0)
                    {
                        
                        ProjectVO pVO = uBO.FindProjectByContract(contractId);
                        if (pVO != null)
                        {
                            if (!uBO.IsHasPayed(pVO.ProjectId))
                            {
                                //更新任务状态和委托酬金，更新项目状态，更新项目委托酬金
                                RequireBO rBO = new RequireBO(new CustomerProfile());

                                IRequireCommissionDelegationDAO rcdDAO = RequireManagementDAOFactory.CreateRequireCommissionDelegationDAO(new UserProfile());
                                RequireCommissionDelegationVO rcdVO = new RequireCommissionDelegationVO();
                                rcdVO.RequirementId = pVO.RequirementId;
                                rcdVO.Status = 1;
                                rcdVO.Commission = uBO.FindCommisionDelegationTotal(pVO.ProjectId);
                                rcdVO.DelegationDate = DateTime.Now;

                                rcdDAO.Insert(rcdVO);

                                RequirementVO rVO = new RequirementVO();
                                rVO.RequirementId = pVO.RequirementId;
                                rVO.Status = 1;

                                rBO.UpdateRequirement(rVO);

                                ProjectVO puVO = new ProjectVO();
                                puVO.ProjectId = pVO.ProjectId;
                                puVO.Status = 6;

                                uBO.UpdateProject(puVO);

                                uBO.RemoveComissionDelegation(puVO.ProjectId);
                            }
                        }
                    }
                }

                if (type == "A")
                {
                    if (pViewVO.AgencyStatus != status && status == 1)
                    {
                        //发送站内信息                            
                        MessageBO mBO = new MessageBO(new CustomerProfile());
                        BusinessBO bBO = new BusinessBO(new CustomerProfile());
                        BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);
                        AgencyBO aBO = new AgencyBO(new CustomerProfile());
                        AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                        string msgContent = "尊敬的" + bViewVO.CustomerName + "：" + aViewVO.AgencyName + "已经签订雇佣合同，项目：" + pViewVO.ProjectName + ",请到“会员中心>我是雇主>雇佣合同”处确认合同【众销乐 -资源共享众包销售平台】";
                        string result = MessageTool.SendMobileMsg(msgContent, bViewVO.Phone);
                        mBO.SendMessage("雇佣合同状态变更", "  " + bViewVO.CustomerName + ":" + aViewVO.CustomerName + "已经签订雇佣合同，项目：" + pViewVO.ProjectName, bViewVO.CustomerId, MessageType.Project);

                        pViewVO.AgencyStatus = status;
                    }
                }
                else if (type == "B")
                {
                    if (pViewVO.BusinessStatus != status && status == 1)
                    {
                        //发送站内信息                            
                        MessageBO mBO = new MessageBO(new CustomerProfile());
                        BusinessBO bBO = new BusinessBO(new CustomerProfile());
                        BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);
                        AgencyBO aBO = new AgencyBO(new CustomerProfile());
                        AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);
                        string statusName = "";

                        string msgContent = "尊敬的" + aViewVO.CustomerName + "：" + bViewVO.CompanyName + "已选中了您并已经签订雇佣合同，项目：" + pViewVO.ProjectName + ",请到“会员中心>我是销售>雇佣合同”处确认合同【众销乐 -资源共享众包销售平台】";
                        string result = MessageTool.SendMobileMsg(msgContent, aViewVO.Phone);
                        mBO.SendMessage("雇佣合同状态变更", "  " + aViewVO.CustomerName + ":" + bViewVO.CompanyName + "已选中了您并已经签订雇佣合同，项目：" + pViewVO.ProjectName, aViewVO.CustomerId, MessageType.Project);

                        pViewVO.BusinessStatus = status;
                    }
                }


                if (pViewVO.AgencyStatus == 1 && pViewVO.BusinessStatus == 1)
                {
                    //自动创建项目 
                    RequireBO rBO = new RequireBO(new CustomerProfile());
                    ProjectVO projectVO = new ProjectVO();

                    RequirementViewVO requireVO = rBO.FindRequireById(pViewVO.RequirementId);
                    
                    
                    if (requireVO.Status == 4)
                    {
                        return new ResultObject() { Flag = 0, Message = "已经存在项目，创建项目失败!", Result = null };
                    }

                    projectVO.RequirementId = pViewVO.RequirementId;
                    projectVO.Commission = pViewVO.Commission;
                    projectVO.Cost = pViewVO.Cost;
                    projectVO.StartDate = pViewVO.StartDate;
                    projectVO.EndDate = pViewVO.EndDate;
                    projectVO.CommissionType = 0;
                    projectVO.CustomerId = pViewVO.CustomerId;
                    projectVO.Status = 0;
                    projectVO.CreatedAt = DateTime.Now;
                    projectVO.ProjectCode = uBO.GetProjectCode();
                    projectVO.ContractId = pViewVO.ContractId;

                    int projectId = uBO.AddProject(projectVO);

                    //将任务复制到副本
                    rBO.RequirementCopy(projectId);

                    //将任务已委托酬金更新到项目已委托酬金
                    uBO.UpdateProjectCommission(pViewVO.RequirementId, projectId);

                    //如果已委托酬金超过总酬金20%，直接进入工作状态
                    ProjectViewVO projectViewVO = uBO.FindProjectById(projectId);
                    decimal total = uBO.FindCommisionDelegationTotal(projectId);//已托管金额
                    SystemBO sBO = new SystemBO(new UserProfile());
                    ConfigVO vo = sBO.FindConfig();
                    decimal platformCommission = projectViewVO.Commission * vo.CommissionPercentage / 100;//平台抽佣
                    
                    if (total >= cBO.GetFirstMandates(projectViewVO.Commission)) {
                        ProjectVO pVO = new ProjectVO();
                        pVO.Status = 1;
                        pVO.ProjectId = projectViewVO.ProjectId;
                        uBO.UpdateProject(pVO);
                    }

                    //生成图片，并保存到合同柜
                    
                    Bitmap m_bitmap = uBO.GenerateContractImage(contractId);
                    uBO.GenerateContractImage(m_bitmap, pViewVO.BusinessCustomerId, pViewVO.ProjectName);
                    uBO.GenerateContractImage(m_bitmap, pViewVO.AgencyCustomerId, pViewVO.ProjectName);

                    //合同本身也需要保存, 并更新合同字段
                    string filePath = "";
                    string folder = "/UploadFolder/ContractFile/";
                    string newFileName = DateTime.Now.ToString("yyyyMMddhhssmm") + ".png";
                    filePath = folder + newFileName;

                    string localPath = ConfigInfo.Instance.UploadFolder + folder;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string physicalPath = localPath + newFileName;
                    m_bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

                    ContractVO cfVO = new ContractVO();
                    cfVO.ContractId = contractId;
                    cfVO.ContractFile = ConfigInfo.Instance.APIURL + filePath; ;

                    uBO.UpdateContract(cfVO);
                    
                    //发送站内信息                            
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    BusinessBO bBO = new BusinessBO(new CustomerProfile());
                    BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.BusinessId);
                    AgencyBO aBO = new AgencyBO(new CustomerProfile());
                    AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);

                    mBO.SendMessage("项目创建", "  " + aViewVO.CustomerName + ":" + bViewVO.CompanyName + "已经雇佣您了！", aViewVO.CustomerId, MessageType.Project);

                    string msgContent = "尊敬的" + aViewVO.CustomerName + "：您已正式成为项目《" + pViewVO.ProjectName + "》的兼职销售了，请到“会员中心>我是销售>工具包”查看销售工具后，尽快开展任务销售吧！【众销乐 -资源共享众包销售平台】";
                    string result = MessageTool.SendMobileMsg(msgContent, aViewVO.Phone);
                    //发送给雇主

                    mBO.SendMessage("项目创建", "  " + bViewVO.CustomerName + ":您已经雇佣了" + aViewVO.AgencyName + ",请尽快托管酬金！", bViewVO.CustomerId, MessageType.Project);
                    if (projectId > 0)
                        return new ResultObject() { Flag = 2, Message = "已经成功创建项目!", Result = projectId };
                    else
                        return new ResultObject() { Flag = 0, Message = "创建项目失败!", Result = null };
                }

                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取协议内容
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetContractNote"), HttpGet]
        public ResultObject GetContractNote(string token)
        {
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = CacheSystemConfig.GetSystemConfig().ContractNote };
        }

        /// <summary>
        /// 获取三方协议附件
        /// </summary>
        /// <param name="contractId">三方协议ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetContractFile"), HttpGet]
        public ResultObject GetContractFile(int contractId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ContractFileVO> uVO = uBO.FindContractFileList(contractId);
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
        /// 获取三方协议步骤
        /// </summary>
        /// <param name="contractId">三方协议ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetContractSteps"), HttpGet]
        public ResultObject GetContractSteps(int contractId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ContractStepsVO> uVO = uBO.FindContractStepsList(contractId);
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
        /// 获取销售三方协议列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyContractList"), HttpPost]
        public ResultObject GetAgencyContractList([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
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
            ProjectBO uBO = new ProjectBO(cProfile);
            conditionStr = "Status = 1 and AgencyCustomerId = " + cProfile.CustomerId + " and " + conditionStr;
            List<ContractViewVO> list = uBO.FindContractAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };

        }
        /// <summary>
        /// 获取销售三方协议数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAgencyContractListCount"), HttpPost]
        public ResultObject GetAgencyContractListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
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
            ProjectBO uBO = new ProjectBO(cProfile);
            conditionStr = "Status = 1 and AgencyCustomerId = " + cProfile.CustomerId + " and " + conditionStr;
            int count = uBO.FindContractTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }

        /// <summary>
        /// 获取雇主三方协议列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessContractList"), HttpPost]
        public ResultObject GetBusinessContractList([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
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
            ProjectBO uBO = new ProjectBO(cProfile);
            conditionStr = "Status = 1 and BusinessCustomerId = " + cProfile.CustomerId + " and " + conditionStr;
            List<ContractViewVO> list = uBO.FindContractAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };

        }
        /// <summary>
        /// 获取雇主三方协议数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetBusinessContractListCount"), HttpPost]
        public ResultObject GetBusinessContractListCount([FromBody] ConditionModel condition, string token)
        {
            CustomerProfile cProfile = (CustomerProfile)CacheManager.GetUserProfile(token);
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
            ProjectBO uBO = new ProjectBO(cProfile);
            conditionStr = "Status = 1 and BusinessCustomerId = " + cProfile.CustomerId + " and " + conditionStr;
            int count = uBO.FindContractTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }

        /// <summary>
        /// 查看协议相关项目是否已经有过支付
        /// </summary>
        /// <param name="contractId">协议Id</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetIsHasPayed"), HttpGet]
        public ResultObject GetIsHasPayed(int contractId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            ProjectVO pVO = uBO.FindProjectByContract(contractId);
            if (pVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uBO.IsHasPayed(pVO.ProjectId) };
            }
            else
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = false };
            }
        }
        /// <summary>
        /// 作废雇佣合同
        /// </summary>
        /// <param name="contractId">三方协议ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateContractSatus"), HttpGet]
        public ResultObject UpdateContractSatus(int contractId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            ContractVO cVO = new ContractVO();
            cVO.ContractId = contractId;
            cVO.Status = 0;
            bool re = uBO.UpdateContract(cVO);
            if (re)
            {
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }
        /// <summary>
        /// 作废任务相关的雇佣合同
        /// </summary>
        /// <param name="requireId">任务ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRequireContractSatus"), HttpGet]
        public ResultObject UpdateRequireContractSatus(int requireId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());

            bool re = uBO.CancelContract(requireId);
                        
            if (re)
            {
                return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }
    }
}
