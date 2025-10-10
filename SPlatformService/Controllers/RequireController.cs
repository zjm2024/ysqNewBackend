using CoreFramework.VO;
using SPlatformService.Models;
using SPlatformService.TokenMange;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.ProjectManagement.BO;
using SPLibrary.ProjectManagement.VO;
using SPLibrary.RequireManagement.BO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.CoreFramework.BO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SPLibrary.CoreFramework;
using System.Text.RegularExpressions;

namespace SPlatformService.Controllers
{
    /// <summary>
    /// 任务API
    /// </summary>
    [RoutePrefix("SPWebAPI/Require")]
    [TokenProjector]
    public class RequireController : ApiController
    {
        /// <summary>
        /// 添加或更新任务
        /// </summary>
        /// <param name="requireModelVO">任务VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRequire"), HttpPost]
        public ResultObject UpdateRequire([FromBody] RequireModel requireModelVO, string token)
        {
            if (requireModelVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);

            RequirementVO requireVO = requireModelVO.Requirement;

            List<RequirementTargetCategoryVO> requireCategoryVOList = requireModelVO.RequireCategory;
            List<RequirementTargetCityVO> requireCityVOList = requireModelVO.RequireCity;
            List<RequirementFileVO> requireFileVOList = requireModelVO.RequireFile;
            List<RequirementTargetClientVO> requireClientVOList = requireModelVO.RequireClient;

            if (requireVO != null)
            {
                RequireBO uBO = new RequireBO(new CustomerProfile());
                if (requireVO.CityId < 1)
                    requireVO.SetValue("CityId",DBNull.Value);
                if (requireVO.CategoryId < 1)
                    requireVO.SetValue("CategoryId", DBNull.Value);
                if (requireVO.RequirementId < 1)
                {
                    requireVO.CreatedAt = DateTime.Now;
                    requireVO.RequirementCode = uBO.GetRequireCode();
                    requireVO.Status = 0;
                    requireVO.agencySum = 1;
                    CustomerProfile cProfile = uProfile as CustomerProfile;
                    if (cProfile != null)
                        requireVO.CustomerId = cProfile.CustomerId;
                    //避免重复提交的情况，60秒内不得添加两次
                    int oldCount= uBO.FindRequireTotalCount("CustomerId = "+ requireVO.CustomerId+ " and NOW()-CreatedAt<60");
                    if (oldCount > 0)
                    {
                        return new ResultObject() { Flag = 0, Message = "你在刚刚已经添加了新任务，为避免重复添加，请先到任务列表查看!", Result = null };
                    }
                    else
                    {
                        int requireId = uBO.AddRequirement(requireVO, requireCategoryVOList, requireCityVOList, requireFileVOList, requireClientVOList);
                        if (requireId > 0)
                            return new ResultObject() { Flag = 1, Message = "添加成功!", Result = requireId };
                        else
                            return new ResultObject() { Flag = 0, Message = "添加失败1!", Result = null };
                    }
                }
                else
                {
                    RequirementViewVO rVO = uBO.FindRequireById(requireVO.RequirementId);
                    if (rVO != null) {
                        CustomerProfile cProfile = uProfile as CustomerProfile;
                        if (cProfile != null)
                        {
                            //如果提交者并非该任务所属的会员，直接禁止更新
                            if(cProfile.CustomerId!= rVO.CustomerId)
                                return new ResultObject() { Flag = 0, Message = "更新失败2!", Result = null };
                        }
                        else {
                            if (uProfile.UserId <= 0)
                            {
                                return new ResultObject() { Flag = 0, Message = "更新失败3!", Result = null };
                            }
                                
                        }
                    }
                    bool isSuccess = uBO.UpdateRequirement(requireVO, requireCategoryVOList, requireCityVOList, requireFileVOList, requireClientVOList);
                    if (isSuccess)
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = requireVO.RequirementId };
                    else
                        return new ResultObject() { Flag = 0, Message = "更新失败4!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
        }

        /// <summary>
        /// 获取任务信息
        /// </summary>
        /// <param name="requireId">任务ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetRequire"), HttpGet]
        public ResultObject GetRequire(int requireId, string token)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            RequirementViewVO uVO = uBO.FindRequireById(requireId);

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
        /// 获取任务快照
        /// </summary>
        /// <param name="ProjectId">项目ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetRequirementcopies"), HttpGet]
        public ResultObject GetRequirementcopies(int ProjectId, string token)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            List<RequirementCopiesVO> uVO = uBO.FindRequirementCopiesByProjectId(ProjectId);
            
            if (uVO != null)
            {
                if (uVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO[0] };
                }
                else {
                    return new ResultObject() { Flag = 0, Message = "没有该项目的任务快照!", Result = null };
                }
                
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }


        /// <summary>
        /// 获取任务行业列表
        /// </summary>
        /// <param name="requireId">任务ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetRequireCategoryByRequire"), HttpGet]
        public ResultObject GetRequireCategoryByRequire(int requireId, string token)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            List<RequirementTargetCategoryViewVO> uVO = uBO.FindTargetCategoryByRequire(requireId);
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
        /// 获取任务行业列表
        /// </summary>
        /// <param name="requireId">任务ID</param>      
        /// <returns></returns>
        [Route("GetRequireCategoryByRequireStie"), HttpGet, Anonymous]
        public ResultObject GetRequireCategoryByRequireStie(int requireId)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            List<RequirementTargetCategoryViewVO> uVO = uBO.FindTargetCategoryByRequire(requireId);
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
        /// 获取任务城市列表
        /// </summary>
        /// <param name="requireId">任务ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetRequireCityByRequire"), HttpGet]
        public ResultObject GetRequireCityByRequire(int requireId, string token)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            List<RequirementTargetCityViewVO> uVO = uBO.FindTargetCityByRequire(requireId);
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
        /// 获取任务城市列表
        /// </summary>
        /// <param name="requireId">任务ID</param>       
        /// <returns></returns>
        [Route("GetRequireCityByRequireStie"), HttpGet, Anonymous]
        public ResultObject GetRequireCityByRequireStie(int requireId)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            List<RequirementTargetCityViewVO> uVO = uBO.FindTargetCityByRequire(requireId);
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
        /// 获取任务附件列表
        /// </summary>
        /// <param name="requireId">任务ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetRequireFileByRequire"), HttpGet]
        public ResultObject GetRequireFileByRequire(int requireId, string token)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            List<RequirementFileVO> uVO = uBO.FindRequireFileByRequire(requireId);
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
        /// 更新任务状态
        /// </summary>
        /// <param name="requireId">任务ID</param>
        /// <param name="status">状态</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRequireStatus"), HttpPost]
        public ResultObject UpdateRequireStatus(string requireId, int status, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            //if (uProfile.UserId > 0)
            //{
                try
                {
                    RequireBO uBO = new RequireBO(new CustomerProfile());
                    if (!string.IsNullOrEmpty(requireId))
                    {
                        string[] bIdArr = requireId.Split(',');
                        bool isAllUpdate = true;

                        for (int i = 0; i < bIdArr.Length; i++)
                        {
                        try
                        {
                            RequirementVO bVO = new RequirementVO();
                            bVO.RequirementId = Convert.ToInt32(bIdArr[i]);
                            bVO.Status = status;

                            RequirementViewVO pViewVO = uBO.FindRequireById(Convert.ToInt32(bIdArr[i]));

                            CustomerProfile cProfile = uProfile as CustomerProfile;
                            if (cProfile != null)
                            {
                                if (cProfile.CustomerId != pViewVO.CustomerId) {
                                    return new ResultObject() { Flag = 0, Message = "不许修改别人的任务!", Result = null };
                                }
                            }
                            else {
                                return new ResultObject() { Flag = 0, Message = "请先登陆!", Result = null };
                            }

                            if (status == 1)
                            {
                                return new ResultObject() { Flag = 0, Message = "不能直接发布任务，请提交审核!", Result = null };
                                /*
                                bVO.PublishAt = DateTime.Now;
                                CustomerBO _bo = new CustomerBO(new CustomerProfile());
                                List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("发布任务");
                                if (_bo.ZXBFindRequireCount("CustomerId = "+ pViewVO.CustomerId+" and type="+ zVO[0].ZxbConfigID) == 0) {
                                    //发放乐币奖励
                                    _bo.ZXBAddrequire(pViewVO.CustomerId, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID);
                                }  */
                            }
                                
                            bool isSuccess = uBO.UpdateRequirement(bVO);

                            //如果是取消发布则退还酬金
                            if(status == 0)
                            {
                                uBO.ReduceRequireDelegateCommision(bVO.RequirementId);
                            }

                            if (status != pViewVO.Status)
                            {
                                //发送站内信息                            
                                MessageBO mBO = new MessageBO(new CustomerProfile());
                                BusinessBO bBO = new BusinessBO(new CustomerProfile());
                                BusinessViewVO bViewVO = bBO.FindBusinessByCustomerId(pViewVO.CustomerId);
                                //AgencyBO aBO = new AgencyBO(new CustomerProfile());
                                //AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);
                                string statusName = "";
                                switch (status)
                                {
                                    case 0:
                                        statusName = "保存";
                                        break;
                                    case 1:
                                        statusName = "发布";
                                        break;
                                    case 2:
                                        statusName = "关闭";
                                        break;
                                    case 3:
                                        statusName = "暂停投简历";
                                        break;
                                    case 4:
                                        statusName = "已选定项目";
                                        break;
                                    default:
                                        statusName = "保存";
                                        break;
                                }

                                mBO.SendMessage("任务状态变更", "  " + bViewVO.CustomerName + ":项目" + pViewVO.RequirementCode + "状态变为" + statusName + "！", bViewVO.CustomerId, MessageType.Project);
                            }

                        }
                        catch
                        {
                            isAllUpdate = false;
                        }
                        }
                        if (isAllUpdate)
                        {
                            return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 1, Message = "部分更新成功!", Result = null };
                        }

                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }
                }
                catch
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            //}
            //else
            //{
            //    return new ResultObject() { Flag = 0, Message = "没有权限执行此操作!", Result = null };
            //}
        }

        /// <summary>
        /// 更新任务状态，后台专用
        /// </summary>
        /// <param name="requireId">任务ID</param>
        /// <param name="status">状态</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRequireStatusAction"), HttpPost]
        public ResultObject UpdateRequireStatusAction(string requireId, int status, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (uProfile.UserId > 0)
            {
            try
            {
                RequireBO uBO = new RequireBO(new CustomerProfile());
                if (!string.IsNullOrEmpty(requireId))
                {
                    string[] bIdArr = requireId.Split(',');
                    bool isAllUpdate = true;

                    for (int i = 0; i < bIdArr.Length; i++)
                    {
                        try
                        {
                            RequirementVO bVO = new RequirementVO();
                            bVO.RequirementId = Convert.ToInt32(bIdArr[i]);
                            bVO.Status = status;

                            RequirementViewVO pViewVO = uBO.FindRequireById(Convert.ToInt32(bIdArr[i]));

                            if (status == 1)
                            {
                                bVO.PublishAt = DateTime.Now;
                                CustomerBO _bo = new CustomerBO(new CustomerProfile());
                                List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("发布任务");
                                if (_bo.ZXBFindRequireCount("CustomerId = " + pViewVO.CustomerId + " and type=" + zVO[0].ZxbConfigID) == 0)
                                {
                                    //发放乐币奖励
                                    _bo.ZXBAddrequire(pViewVO.CustomerId, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID);
                                }
                            }

                            bool isSuccess = uBO.UpdateRequirement(bVO);

                            //如果是取消发布则退还酬金
                            if (status == 0)
                            {
                                uBO.ReduceRequireDelegateCommision(bVO.RequirementId);
                            }

                            if (status != pViewVO.Status)
                            {
                                //发送站内信息                            
                                MessageBO mBO = new MessageBO(new CustomerProfile());
                                BusinessBO bBO = new BusinessBO(new CustomerProfile());
                                BusinessViewVO bViewVO = bBO.FindBusinessByCustomerId(pViewVO.CustomerId);
                                //AgencyBO aBO = new AgencyBO(new CustomerProfile());
                                //AgencyViewVO aViewVO = aBO.FindAgencyById(pViewVO.AgencyId);
                                string statusName = "";
                                switch (status)
                                {
                                    case 0:
                                        statusName = "保存";
                                        break;
                                    case 1:
                                        statusName = "发布";
                                        break;
                                    case 2:
                                        statusName = "关闭";
                                        break;
                                    case 3:
                                        statusName = "暂停投简历";
                                        break;
                                    case 4:
                                        statusName = "已选定项目";
                                        break;
                                    default:
                                        statusName = "保存";
                                        break;
                                }
                                mBO.SendMessage("任务状态变更", "  " + bViewVO.CustomerName + ":项目" + pViewVO.RequirementCode + "状态变为" + statusName + "！", bViewVO.CustomerId, MessageType.Project);
                            }

                        }
                        catch
                        {
                            isAllUpdate = false;
                        }
                    }
                    if (isAllUpdate)
                    {
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分更新成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "没有权限执行此操作!", Result = null };
            }
        }

        /// <summary>
        /// 更新任务发布时间
        /// </summary>
        /// <param name="requireId">任务ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRequirePublishAt"), HttpPost]
        public ResultObject UpdateRequirePublishAt(string requireId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            try
            {
                RequireBO uBO = new RequireBO(new CustomerProfile());
                if (!string.IsNullOrEmpty(requireId))
                {
                    string[] bIdArr = requireId.Split(',');
                    bool isAllUpdate = true;

                    for (int i = 0; i < bIdArr.Length; i++)
                    {
                        try
                        {
                            RequirementVO bVO = new RequirementVO();
                            bVO.RequirementId = Convert.ToInt32(bIdArr[i]);
                            bVO.PublishAt = DateTime.Now;
                            bool isSuccess = uBO.UpdateRequirement(bVO);
                        }
                        catch
                        {
                            isAllUpdate = false;
                        }
                    }
                    if (isAllUpdate)
                    {
                        return new ResultObject() { Flag = 1, Message = "刷新成功!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "部分刷新成功!", Result = null };
                    }

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "刷新失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加邀请投标信息
        /// </summary>
        /// <param name="tenderInfoVO">邀请投标VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRequireTenderInfo"), HttpPost]
        public ResultObject UpdateRequireTenderInfo([FromBody] TenderInfoVO tenderInfoVO, string token)
        {
            if (tenderInfoVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            CustomerProfile uProfile = new CustomerProfile();

            RequireBO uBO = new RequireBO(uProfile);
            tenderInfoVO.TenderDate = DateTime.Now;
            //判断是否已经邀请
            if (uBO.IsAlreadyTenderInfo(tenderInfoVO))
            {
                return new ResultObject() { Flag = 0, Message = "已经邀请过了!", Result = null };
            }
            else
            {
                int tenderInfoId = uBO.AddTenderInfo(tenderInfoVO);
                if (tenderInfoId > 0)
                {
                    //发送站内消息
                    RequirementViewVO pViewVO = uBO.FindRequireById(tenderInfoVO.RequirementId);
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    BusinessBO bBO = new BusinessBO(new CustomerProfile());
                    BusinessViewVO bViewVO = bBO.FindBusinessByCustomerId(pViewVO.CustomerId);

                    AgencyBO aBO = new AgencyBO(new CustomerProfile());
                    AgencyViewVO aViewVO = aBO.FindAgencyByCustomerId(tenderInfoVO.CustomerId);

                    string msgContent = "尊敬的" + aViewVO.CustomerName + "：雇主" + bViewVO.CompanyName + "对您的丰富阅历感兴趣现已关注您，请到“会员中心>我是销售>面试邀请”处理以便用您的资源接单赚钱【众销乐 -资源共享众包销售平台】";
                    string result = MessageTool.SendMobileMsg(msgContent, aViewVO.Phone);
                    mBO.SendMessage("邀请投简历", "  " + aViewVO.CustomerName + ":" + bViewVO.CustomerName + "邀请您进行投简历，项目" + pViewVO.RequirementCode + "！", aViewVO.CustomerId, MessageType.Tender);

                    CustomerBO _bo = new CustomerBO(new CustomerProfile());
                    //发放乐币
                    List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("投标邀请");
                    if (_bo.ZXBFindRequireCount("CustomerId = " + aViewVO.CustomerId + " and type=" + zVO[0].ZxbConfigID) == 0)
                    {
                        //发放乐币奖励
                        _bo.ZXBAddrequire(aViewVO.CustomerId, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID);
                    }

                    return new ResultObject() { Flag = 1, Message = "邀请成功!", Result = tenderInfoId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "邀请失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取任务邀请投标信息
        /// </summary>
        /// <param name="requireId">任务ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetRequireTenderInfoByRequire"), HttpGet]
        public ResultObject GetRequireTenderInfoByRequire(int requireId, string token)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            List<TenderInfoViewVO> uVO = uBO.FindTenderInfoByRequire(requireId);
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
        /// 更新投标信息
        /// </summary>
        /// <param name="tenderInviteVO">投标VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRequireTenderInvite"), HttpPost]
        public ResultObject UpdateRequireTenderInvite([FromBody] TenderInviteVO tenderInviteVO, string token)
        {
            if (tenderInviteVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            CustomerProfile uProfile = new CustomerProfile();

            tenderInviteVO.InviteDate = DateTime.Now;

            RequireBO uBO = new RequireBO(uProfile);
            //判断是否已经投标
            if (uBO.IsAlreadyTenderInvite(tenderInviteVO))
            {
                return new ResultObject() { Flag = 0, Message = "已经申请过了!", Result = null };
            }
            else
            {
                int tenderInviteId = uBO.AddTenderInvite(tenderInviteVO);
                if (tenderInviteId > 0)
                {
                    //发送站内信
                    RequirementViewVO pViewVO = uBO.FindRequireById(tenderInviteVO.RequirementId);
                    MessageBO mBO = new MessageBO(new CustomerProfile());
                    BusinessBO bBO = new BusinessBO(new CustomerProfile());
                    BusinessViewVO bViewVO = bBO.FindBusinessByCustomerId(pViewVO.CustomerId);

                    AgencyBO aBO = new AgencyBO(new CustomerProfile());
                    AgencyViewVO aViewVO = aBO.FindAgencyByCustomerId(tenderInviteVO.CustomerId);

                    List<MessageViewVO> messageViewVO = mBO.FindAllMessageByPageIndex("MessageTypeId=5 and SendTo="+ pViewVO.CustomerId+ " and NOW()-SendAt<86400",1,20, "SendAt","desc");

                    if (messageViewVO.Count <= 5)
                    {
                        string msgContent = "尊敬的"+ bViewVO.CustomerName + "：销售" + aViewVO.AgencyName + "对您发布的任务《" + pViewVO.Title + "》投递了简历，请到“会员中心>我是雇主>我的任务>查看简历”！【众销乐-资源共享众包销售平台】";
                        string result = MessageTool.SendMobileMsg(msgContent, pViewVO.Phone);
                    }
                    
                    mBO.SendMessage("投递简历", "  " + bViewVO.CustomerName + ":销售" + aViewVO.CustomerName + "对您发布的任务《" + pViewVO.Title + "》投递了简历，请到“会员中心>我是雇主>我的任务>查看简历”！", pViewVO.CustomerId, MessageType.Tender);

                    CustomerBO _bo = new CustomerBO(new CustomerProfile());
                    //发放乐币
                    List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("投递简历");
                    if (_bo.ZXBFindRequireCount("CustomerId = " + aViewVO.CustomerId + " and type=" + zVO[0].ZxbConfigID) == 0)
                    {
                        //发放乐币奖励
                        _bo.ZXBAddrequire(aViewVO.CustomerId, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID);
                    }

                    return new ResultObject() { Flag = 1, Message = "投递成功!", Result = tenderInviteId };
                }
                else
                    return new ResultObject() { Flag = 0, Message = "投递失败!", Result = null };
            }
        }

        /// <summary>
        /// 接受投标邀请
        /// </summary>
        /// <param name="requireIds">任务ID，逗号分隔</param>
        /// <param name="customerId">销售会员ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRequireTenderInviteAll"), HttpPost]
        public ResultObject UpdateRequireTenderInviteAll(string requireIds,int customerId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);          
            try
            {
                RequireBO uBO = new RequireBO(new CustomerProfile());
                if (!string.IsNullOrEmpty(requireIds))
                {
                    string[] bIdArr = requireIds.Split(',');
                    bool isAllUpdate = true;

                    for (int i = 0; i < bIdArr.Length; i++)
                    {
                        try
                        {
                            TenderInviteVO tenderInviteVO = new TenderInviteVO();
                            tenderInviteVO.CustomerId = customerId;
                            tenderInviteVO.RequirementId = Convert.ToInt32(bIdArr[i]);
                            tenderInviteVO.InviteDate = DateTime.Now;

                            if (uBO.IsAlreadyTenderInvite(tenderInviteVO))
                            {
                                //return new ResultObject() { Flag = 0, Message = "已经投过标了!", Result = null };
                            }
                            else
                            {
                                int tenderInviteId = uBO.AddTenderInvite(tenderInviteVO);
                                //接受邀请之后，删掉邀请信息
                                uBO.DeleteTenderInfo(customerId, Convert.ToInt32(bIdArr[i]));

                                //发送站内信      
                                RequirementViewVO pViewVO = uBO.FindRequireById(tenderInviteVO.RequirementId);
                                MessageBO mBO = new MessageBO(new CustomerProfile());
                                BusinessBO bBO = new BusinessBO(new CustomerProfile());
                                BusinessViewVO bViewVO = bBO.FindBusinessById(pViewVO.CustomerId);

                                AgencyBO aBO = new AgencyBO(new CustomerProfile());
                                AgencyViewVO aViewVO = aBO.FindAgencyByCustomerId(tenderInviteVO.CustomerId);

                                mBO.SendMessage("投递简历", "  " + bViewVO.CustomerName + ":" + aViewVO.CustomerName + "接受了投递邀请，任务《" + pViewVO.Title + "》投递了简历！", bViewVO.CustomerId, MessageType.Tender);

                            }
                        }
                        catch
                        {
                            isAllUpdate = false;
                        }
                    }
                    //if (isAllUpdate)
                    //{
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                    //}
                    //else
                    //{
                    //    return new ResultObject() { Flag = 1, Message = "部分更新成功!", Result = null };
                    //}

                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
            
            
        }

        /// <summary>
        /// 获取投标信息列表
        /// </summary>
        /// <param name="requireId">任务ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetRequireTenderInviteByRequire"), HttpGet]
        public ResultObject GetRequireTenderInviteByRequire(int requireId, string token)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            List<TenderInviteViewVO> uVO = uBO.FindTenderInviteByRequire(requireId);
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
        /// 添加或更新服务信息
        /// </summary>
        /// <param name="servicesModelVO">服务VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateServices"), HttpPost]
        public ResultObject UpdateServices([FromBody] ServicesModel servicesModelVO, string token)
        {
            if (servicesModelVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            UserProfile uProfile = CacheManager.GetUserProfile(token);

            ServicesVO servicesVO = servicesModelVO.Services;
            List<ServicesCategoryVO> servicesCategoryVOList = servicesModelVO.ServicesCategory;

            if (servicesVO != null)
            {
                ServicesBO uBO = new ServicesBO(new CustomerProfile());

                if (servicesVO.ServicesId < 1)
                {
                    servicesVO.CreatedAt = DateTime.Now;
                    servicesVO.ServicesCode = uBO.GetServicesCode();
                    CustomerProfile cProfile = uProfile as CustomerProfile;
                    if (cProfile != null)
                        servicesVO.CustomerId = cProfile.CustomerId;

                    int servicesId = uBO.AddServices(servicesVO, servicesCategoryVOList);
                    if (servicesId > 0)
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = servicesId };
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
                else
                {
                    bool isSuccess = uBO.UpdateServices(servicesVO, servicesCategoryVOList);
                    if (isSuccess)
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
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
        /// 获取服务信息
        /// </summary>
        /// <param name="servicesId">服务ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetServices"), HttpGet]
        public ResultObject GetServices(int servicesId, string token)
        {
            ServicesBO uBO = new ServicesBO(new CustomerProfile());
            ServicesViewVO uVO = uBO.FindServicesById(servicesId);
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
        /// 获取服务行业列表
        /// </summary>
        /// <param name="servicesId">服务ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetServicesCategoryByServices"), HttpGet]
        public ResultObject GetServicesCategoryByServices(int servicesId, string token)
        {
            ServicesBO uBO = new ServicesBO(new CustomerProfile());
            List<ServicesCategoryViewVO> uVO = uBO.FindTargetCategoryByServices(servicesId);
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
        /// 更新服务状态
        /// </summary>
        /// <param name="servicesId">服务ID</param>
        /// <param name="status">状态</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateServicesStatus"), HttpPost]
        public ResultObject UpdateServicesStatus(string servicesId, int status, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            //if (uProfile.UserId > 0)
            //{
                try
                {
                    ServicesBO uBO = new ServicesBO(new CustomerProfile());
                    if (!string.IsNullOrEmpty(servicesId))
                    {
                        string[] bIdArr = servicesId.Split(',');
                        bool isAllUpdate = true;

                        for (int i = 0; i < bIdArr.Length; i++)
                        {
                            try
                            {
                                ServicesVO bVO = new ServicesVO();
                                bVO.ServicesId = Convert.ToInt32(bIdArr[i]);
                                bVO.Status = status;
                                bool isSuccess = uBO.UpdateServices(bVO);

                            }
                            catch
                            {
                                isAllUpdate = false;
                            }
                        }
                        if (isAllUpdate)
                        {
                            return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 1, Message = "部分更新成功!", Result = null };
                        }

                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }
                }
                catch
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            //}
            //else
            //{
            //    return new ResultObject() { Flag = 0, Message = "没有权限执行此操作!", Result = null };
            //}
        }

        /// <summary>
        /// 获取发布的任务列表，匿名
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetRequireList"), HttpPost, Anonymous]
        public ResultObject GetRequireList([FromBody] ConditionModel condition)
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
            //做一下修改，显示所有非保存的任务
            string conditionStr = "Status <> 0 and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            RequireBO uBO = new RequireBO(new CustomerProfile());
           
            List<RequirementViewVO> list = uBO.FindRequireAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].MainImg=uBO.getRequireIMG(list[i].RequirementId);
                }
            }
            int Count = uBO.FindRequireTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list ,Count= Count };
        }

        /// <summary>
        /// 获取发布的任务数量，匿名
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetRequireListCount"), HttpPost, Anonymous]
        public ResultObject GetRequireListCount([FromBody] ConditionModel condition)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = "Status <> 0 and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            RequireBO uBO = new RequireBO(new CustomerProfile());
            int count = uBO.FindRequireTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }

        /// <summary>
        /// 获取任务详情，匿名
        /// </summary>
        /// <param name="id">任务ID</param>
        /// <returns></returns>
        [Route("GetRequireSite"), HttpGet, Anonymous]
        public ResultObject GetRequireSite(int id)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            RequirementViewVO uVO = uBO.FindRequireById(id);
            if (uVO != null)
            {
                uVO.MainImg = uBO.getRequireIMG(uVO.RequirementId);
                try
                {
                    RequirementVO rVO = new RequirementVO();
                    rVO.RequirementId = uVO.RequirementId;
                    rVO.ReadCount = uVO.ReadCount + 1;
                    uBO.UpdateRequirement(rVO);
                }
                catch
                {

                }

                if (uVO.QRCodeImg == "")
                {
                    uVO.QRCodeImg = uBO.GetRequirementQR(uVO.RequirementId);
                }
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取销售列表，匿名
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetAgencyList"), HttpPost, Anonymous]
        public ResultObject GetAgencyList([FromBody] ConditionModel condition)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = "Status = 1 and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            BusinessBO bBO = new BusinessBO(new CustomerProfile());
            CustomerBO cBO = new CustomerBO(new CustomerProfile());

            List<AgencyViewVO> list = uBO.FindAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++) {
                    list[i].PersonalCard = uBO.getAgencyIMG(list[i].AgencyId);
                    list[i].Description = cBO.GetFilterText(list[i].Description);
                }
            }
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
        }

        /// <summary>
        /// 获取销售数量，匿名
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetAgencyListCount"), HttpPost, Anonymous]
        public ResultObject GetAgencyListCount([FromBody] ConditionModel condition)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = "Status = 1 and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            int count = uBO.FindTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }

        /// <summary>
        /// 获取销售详情，匿名
        /// </summary>
        /// <param name="id">销售ID</param>
        /// <returns></returns>
        [Route("GetAgencySite"), HttpGet, Anonymous]
        public ResultObject GetAgencySite(int id)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            AgencyViewVO uVO = uBO.FindAgencyById(id);
            CustomerBO cBO = new CustomerBO(new CustomerProfile());

            if (uVO != null)
            {
                if (uVO.QRCodeImg == "")
                {
                    uVO.QRCodeImg = uBO.GetAgencyQR(uVO.AgencyId);
                }
                uVO.PersonalCard = uBO.getAgencyIMG(uVO.AgencyId);
                uVO.Description = cBO.GetFilterText(uVO.Description);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取雇主列表，匿名
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetBusinessList"), HttpPost, Anonymous]
        public ResultObject GetBusinessList([FromBody] ConditionModel condition)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = "Status = 1 and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            AgencyBO aBO = new AgencyBO(new CustomerProfile());
            CustomerBO cBO = new CustomerBO(new CustomerProfile());

            List<BusinessViewVO> list = uBO.FindAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].CompanyLogo = uBO.getBusinessIMG(list[i].BusinessId);
                }
            }
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
        }


        /// <summary>
        /// 获取雇主数量，匿名
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetBusinessListCount"), HttpPost, Anonymous]
        public ResultObject GetBusinessListCount([FromBody] ConditionModel condition)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = "Status = 1 and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            int count = uBO.FindTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }

        /// <summary>
        /// 获取雇主详情，匿名
        /// </summary>
        /// <param name="id">服务ID</param>
        /// <returns></returns>
        [Route("GetBusinessSite"), HttpGet, Anonymous]
        public ResultObject GetBusinessSite(int id)
        {
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            BusinessViewVO uVO = uBO.FindBusinessById(id);
            if (uVO != null)
            {
                uVO.CompanyLogo = uBO.getBusinessIMG(uVO.BusinessId);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取项目列表，匿名
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetProjectList"), HttpPost, Anonymous]
        public ResultObject GetProjectList([FromBody] ConditionModel condition)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = "Status = 2 and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ProjectViewVO> list = uBO.FindProjectAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
        }

        /// <summary>
        /// 获取项目数量，匿名
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        [Route("GetProjectListCount"), HttpPost, Anonymous]
        public ResultObject GetProjectListCount([FromBody] ConditionModel condition)
        {
            if (condition == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            else if (condition.Filter == null || condition.PageInfo == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            string conditionStr = "Status = 2 and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            int count = uBO.FindProjectTotalCount(conditionStr);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = count };
        }

        /// <summary>
        /// 获取项目详情，匿名
        /// </summary>
        /// <param name="id">项目ID</param>
        /// <returns></returns>
        [Route("GetProjectSite"), HttpGet, Anonymous]
        public ResultObject GetProjectSite(int id)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            ProjectViewVO uVO = uBO.FindProjectById(id);
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
        /// 获取发布的任务列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetRequireListByBusiness"), HttpPost]
        public ResultObject GetRequireListByBusiness([FromBody] ConditionModel condition, string token)
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
            //只显示发布和暂停投标的。
            string conditionStr = " CustomerId = " + cProfile.CustomerId + " and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            RequireBO uBO = new RequireBO(cProfile);
            AgencyBO aBO = new AgencyBO(new CustomerProfile());
            CustomerBO cBO = new CustomerBO(new CustomerProfile());

            List<RequirementViewVO> list = uBO.FindRequireAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {

                    if (list[i].MainImg == "")
                    {
                        CustomerViewVO cVO = cBO.FindById(list[i].CustomerId);
                        if (list[i].CompanyLogo != "") {
                            list[i].MainImg = list[i].CompanyLogo;
                        } else if (cVO.HeaderLogo != "") {
                            list[i].MainImg = cVO.HeaderLogo;
                        } else if (cVO.AgencyId > 0) {
                            AgencyViewVO aVO = aBO.FindAgencyById(cVO.AgencyId);
                            if (aVO != null)
                            {
                                if (aVO.PersonalCard != "")
                                {
                                    list[i].MainImg = aVO.PersonalCard;
                                }
                                else
                                {
                                    list[i].MainImg = ConfigInfo.Instance.NoImg;
                                }
                            }
                        }
                        else
                        {
                            list[i].MainImg = ConfigInfo.Instance.NoImg;
                        }
                    }
                }
            }
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
        }

        /// <summary>
        /// 查询任务匹配
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetMatchRequireList"), HttpPost]
        public ResultObject GetMatchRequireList([FromBody] ConditionModel condition, string token)
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
            //
            AgencyBO aBO = new AgencyBO(new CustomerProfile());
            AgencyViewVO aVO = aBO.FindAgencyByCustomerId(cProfile.CustomerId);
            
            string conditionStr = condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            RequireBO uBO = new RequireBO(cProfile);
            List<RequirementViewVO> list = uBO.FindMatchRequireByPageIndex(aVO.AgencyId,conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
        }

        /// <summary>
        /// 任务酬金托管
        /// </summary>
        /// <param name="requireId">任务ID</param>
        /// <param name="commission">托管金额</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DelegateRequireCommission"), HttpGet]
        public ResultObject DelegateRequireCommission(int requireId,decimal commission, string token)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            CustomerBO cBO = new CustomerBO(new CustomerProfile());
            RequireCommissionDelegationVO rcdVO = new RequireCommissionDelegationVO();
            rcdVO.RequirementId = requireId;
            rcdVO.Commission = commission;
            rcdVO.DelegationDate = DateTime.Now;
            rcdVO.Status = 1;

            RequirementViewVO rVO = uBO.FindRequireById(requireId);
            if (commission < cBO.GetFirstMandates(rVO.Commission)&& uBO.FindRequireDelegateCommisionTotal(requireId) <=0)
            {
                return new ResultObject() { Flag = 2, Message = "首次委托金额不能少于酬金的" + cBO.GetFirstMandates() + "%!", Result = null };
            }

            int  back = uBO.AddRequireDelegateCommission(rcdVO);
            
            if (back >0)
            {
                int CustomerId = uBO.FindRequireById(requireId).CustomerId;
                CustomerBO _bo = new CustomerBO(new CustomerProfile());
                List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("托管酬金");
                if (_bo.ZXBFindRequireCount("CustomerId = " + CustomerId + " and type=" + zVO[0].ZxbConfigID) == 0)
                {
                    //发放乐币奖励
                    _bo.ZXBAddrequire(CustomerId, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID);
                }
                return new ResultObject() { Flag = 1, Message = "托管成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "余额不足，请先充值!", Result = null };
            }
        }

        /// <summary>
        /// 获取任务已委托金额
        /// </summary>
        /// <param name="requireId">任务ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetDelegateRequireCommission"), HttpGet]
        public ResultObject GetDelegateRequireCommission(int requireId,string token)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());

            decimal total = uBO.FindRequireDelegateCommisionTotal(requireId);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = total };

        }

        /// <summary>
        /// 取消任务酬金托管
        /// </summary>
        /// <param name="requireId">任务ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("CancelDelegateRequireCommission"), HttpGet]
        public ResultObject CancelDelegateRequireCommission(int requireId, string token)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
           
            int back = uBO.ReduceRequireDelegateCommision(requireId);

            if (back > 0)
            {
                return new ResultObject() { Flag = 1, Message = "取消成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "取消失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新雇主目标客户
        /// </summary>
        /// <param name="requireClientVO">雇主目标客户VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRequireClient"), HttpPost]
        public ResultObject UpdateRequireClient([FromBody] RequirementTargetClientVO requireClientVO, string token)
        {
            if (requireClientVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            CustomerProfile uProfile = CacheManager.GetUserProfile(token) as CustomerProfile;



            RequireBO uBO = new RequireBO(uProfile);

            if (requireClientVO.RequireClientId < 1)
            {
                int customerId = uProfile.CustomerId;
                int requireClientId = uBO.AddRequireClient(requireClientVO);
                if (requireClientId > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = requireClientId };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            else
            {
                bool isSuccess = uBO.UpdateRequireClient(requireClientVO);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }

        }

        /// <summary>
        /// 获取需求目标客户列表
        /// </summary>
        /// <param name="requireId">需求ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetRequireClientByRequire"), HttpGet]
        public ResultObject GetRequireClientByRequire(int requireId, string token)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            List<RequirementTargetClientVO> uVO = uBO.FindRequireClientByRequire(requireId);
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
        /// 获取需求目标客户列表
        /// </summary>
        /// <param name="requireId">需求ID</param>
        /// <returns></returns>
        [Route("GetRequireClientByRequireAnonymous"), HttpGet,Anonymous]
        public ResultObject GetRequireClientByRequireAnonymous(int requireId)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            List<RequirementTargetClientVO> uVO = uBO.FindRequireClientByRequire(requireId);
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
        /// 获取雇主目标客户
        /// </summary>
        /// <param name="requireClientId">雇主目标客户ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetRequireClient"), HttpGet]
        public ResultObject GetRequireClient(int requireClientId, string token)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            RequirementTargetClientVO uVO = uBO.FindRequireClientById(requireClientId);
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
        /// 删除销售优势客户
        /// </summary>
        /// <param name="requireClientId">销售优势客户ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteRequireClient"), HttpPost]
        public ResultObject DeleteRequireClient(string requireClientId, string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(requireClientId))
                {
                    string[] aeIdArr = requireClientId.Split(',');
                    bool isAllDelete = true;
                    RequireBO uBO = new RequireBO(new CustomerProfile());
                    for (int i = 0; i < aeIdArr.Length; i++)
                    {
                        try
                        {
                            uBO.DeleteRequireClient(Convert.ToInt32(aeIdArr[i]));
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
        /// 判断是否创建项目或合同
        /// </summary>
        /// <param name="requireId">任务ID</param>
        /// <param name="token">口令</param>
        /// <returns>Flag，0,表示获取失败，当做已创建项目；-1表示已创建项目，-2 表示已创建合同；1表示未创建</returns>
        [Route("GetRequireStatus"), HttpGet]
        public ResultObject GetRequireStatus(int requireId, string token)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            try
            {
                bool isGenerateProject = uBO.IsGenerateProject(requireId);
                if (isGenerateProject)
                {
                    return new ResultObject() { Flag = -1, Message = "已创建项目!", Result = null };

                }
                else
                {
                    bool isGenerateContract = uBO.IsGenerateContract(requireId);
                    if (isGenerateContract)
                    {
                        return new ResultObject() { Flag = -2, Message = "已创建合同!", Result = null };
                    }
                    else
                    {
                        return new ResultObject() { Flag = 1, Message = "未创建项目和合同!", Result = null };
                    }
                }
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 快速发布任务（匿名）
        /// </summary>
        /// <param name="requireModelVO">任务VO</param>
        /// <param name="code">验证码</param>
        /// <param name="password">登陆密码</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("QuickAddRequire"), HttpPost, Anonymous]
        public ResultObject QuickAddRequire([FromBody] RequireModel requireModelVO, string code, string password, string token)
        {

            if (requireModelVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            if (requireModelVO.Requirement.Phone == null || requireModelVO.Requirement.Phone == "")
            {
                return new ResultObject() { Flag = 0, Message = "手机号码不得为空!", Result = null };
            }

            System.Web.HttpContext context = System.Web.HttpContext.Current;

            /*
            if (!CacheManager.TokenIsExist(token)) {
               
                if (code == null || code == "")
                {
                    return new ResultObject() { Flag = 0, Message = "验证码不得为空!", Result = null };
                }
                if (context.Session["code"] == null)
                {
                    return new ResultObject() { Flag = 0, Message = "请先发送验证码!", Result = null };
                }
                else
                {
                    if (context.Session["code"].ToString() != code)
                    {
                        return new ResultObject() { Flag = 0, Message = "验证码错误!", Result = null };
                    }
                }
                if (context.Session["phone"] == null)
                {
                    return new ResultObject() { Flag = 0, Message = "手机号码不得为空!", Result = null };
                }
                if (context.Session["phone"].ToString() != requireModelVO.Requirement.Phone)
                {
                    return new ResultObject() { Flag = 0, Message = "手机号码错误，请重新发送验证码!", Result = null };
                }
            }
            */

            string account = requireModelVO.Requirement.Phone;
            int customerId = 0;
            if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(password))
            {
                CustomerBO cBO = new CustomerBO(new CustomerProfile());
                //判断LoginName 和 CustomerName是否重复
                CustomerVO customerVO = new CustomerVO();
                customerVO.CustomerCode = cBO.GetCustomerCode();
                customerVO.CustomerAccount = account;
                customerVO.CustomerName = account;
                customerVO.Phone = account;
                customerVO.Password = Utilities.GetMD5(password);
                customerVO.Status = 1;
                customerVO.CreatedAt = DateTime.Now;
                if (cBO.IsCustomerExist(customerVO))
                {
                    CustomerVO vo = cBO.FindByParams("CustomerAccount=" + account + " or Phone=" + account);
                    customerId = vo.CustomerId;//如果已经注册的把这条信息放在这会员下
                }
                else
                {
                    customerId = cBO.Add(customerVO);
                    if (customerId > 0)
                    {
                        //发放乐币奖励
                        //if(CacheSystemConfig.GetSystemConfig().zxbRegistered>0)
                         //   cBO.ZXBAddrequire(customerId, CacheSystemConfig.GetSystemConfig().zxbRegistered, CacheSystemConfig.GetSystemConfig().zxbRegistered_text, 1);

                        //通过认证，IM注册，存在则不添加，不存在则添加
                        IMBO imBO = new IMBO(new CustomerProfile());
                        imBO.RegisterIMUser(customerId, customerVO.CustomerCode, "$" + customerVO.CustomerCode, customerVO.CustomerName);
                    }
                }
            }
            else
            {
                UserProfile uProfile2 = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile2 = uProfile2 as CustomerProfile;
                customerId = cProfile2.CustomerId;
            }

            RequirementVO requireVO = requireModelVO.Requirement;
            List<RequirementTargetCategoryVO> requireCategoryVOList = requireModelVO.RequireCategory;
            List<RequirementTargetCityVO> requireCityVOList = requireModelVO.RequireCity;
            List<RequirementFileVO> requireFileVOList = requireModelVO.RequireFile;
            List<RequirementTargetClientVO> requireClientVOList = requireModelVO.RequireClient;

            if (requireVO != null)
            {
                RequireBO uBO = new RequireBO(new CustomerProfile());
                if (requireVO.CityId < 1)
                    requireVO.SetValue("CityId", DBNull.Value);
                if (requireVO.CategoryId < 1)
                    requireVO.SetValue("CategoryId", DBNull.Value);
                if (requireVO.RequirementId < 1)
                {
                    requireVO.CreatedAt = DateTime.Now;
                    requireVO.RequirementCode = uBO.GetRequireCode();
                    requireVO.Status = 5;
                    requireVO.CustomerId = customerId;
                    requireVO.CommissionType = 1;
                    requireVO.Commission = requireVO.Cost;
                    requireVO.EffectiveEndDate = DateTime.Now.AddMonths(2);
                    //避免重复提交的情况，60秒内不得添加两次
                    int oldCount = uBO.FindRequireTotalCount("CustomerId = " + requireVO.CustomerId + " and NOW()-CreatedAt<60");
                    if (oldCount > 0)
                    {
                        return new ResultObject() { Flag = 0, Message = "你在刚刚已经添加了新任务，为避免重复添加，请先到任务列表查看!", Result = null };
                    }
                    else
                    {
                        int requireId = uBO.AddRequirement(requireVO, requireCategoryVOList, requireCityVOList, requireFileVOList, requireClientVOList);
                        if (requireId > 0)
                            return new ResultObject() { Flag = 1, Message = "添加成功!", Result = requireId };
                        else
                            return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                    }
                }
                else
                {
                    bool isSuccess = uBO.UpdateRequirement(requireVO, requireCategoryVOList, requireCityVOList, requireFileVOList, requireClientVOList);
                    if (isSuccess)
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = requireVO.RequirementId };
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
        /// 添加或更新需求
        /// </summary>
        /// <param name="demandVO">需求VO</param>
        /// <param name="code">验证码</param>
        /// <param name="password">登陆密码</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateDemand"), HttpPost, Anonymous]
        public ResultObject UpdateDemand([FromBody] DemandVO demandVO,string code, string password, string token)
        {
            if (demandVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            DemandBO uBO = new DemandBO(new CustomerProfile());
            if (demandVO.DemandId < 1)
            {
                if (demandVO.Phone == null || demandVO.Phone == "")
                {
                    return new ResultObject() { Flag = 0, Message = "手机号码不得为空!", Result = null };
                }

                if (!CacheManager.TokenIsExist(token)) {
                    if (code == null || code == "")
                    {
                        return new ResultObject() { Flag = 0, Message = "验证码不得为空!", Result = null };
                    }

                    System.Web.HttpContext context = System.Web.HttpContext.Current;
                    if (context.Session["code"] == null)
                    {
                        return new ResultObject() { Flag = 0, Message = "请先发送验证码!", Result = null };
                    }
                    else
                    {
                        if (context.Session["code"].ToString() != code)
                        {
                            return new ResultObject() { Flag = 0, Message = "验证码错误!", Result = null };
                        }
                    }
                    if (context.Session["phone"] == null)
                    {
                        return new ResultObject() { Flag = 0, Message = "手机号码不得为空!", Result = null };
                    }
                    else
                    {
                        if (context.Session["phone"].ToString() != demandVO.Phone)
                        {
                            return new ResultObject() { Flag = 0, Message = "手机号码错误，请重新发送验证码!", Result = null };
                        }
                    }
                }

                string account = demandVO.Phone;
                int customerId = 0;
                if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(password))
                {
                    CustomerProfile uProfile = new CustomerProfile();
                    CustomerBO cBO = new CustomerBO(uProfile);
                    //判断LoginName 和 CustomerName是否重复
                    CustomerVO customerVO = new CustomerVO();
                    customerVO.CustomerCode = cBO.GetCustomerCode();
                    customerVO.CustomerAccount = account;
                    customerVO.CustomerName = account;
                    customerVO.Phone = account;
                    customerVO.Password = Utilities.GetMD5(password);
                    customerVO.Status = 1;
                    customerVO.CreatedAt = DateTime.Now;
                    if (cBO.IsCustomerExist(customerVO))
                    {
                        CustomerVO vo = cBO.FindByParams("CustomerAccount=" + account + " or Phone=" + account);
                        customerId = vo.CustomerId;//如果已经注册的把这条需求信息放在这会员下
                    }
                    else
                    {
                        customerId = cBO.Add(customerVO);
                        if (customerId > 0)
                        {
                            //发放乐币奖励
                            //if(CacheSystemConfig.GetSystemConfig().zxbRegistered>0)
                            //cBO.ZXBAddrequire(customerId, CacheSystemConfig.GetSystemConfig().zxbRegistered, CacheSystemConfig.GetSystemConfig().zxbRegistered_text, 1);

                            //通过认证，IM注册，存在则不添加，不存在则添加
                            IMBO imBO = new IMBO(new CustomerProfile());
                            imBO.RegisterIMUser(customerId, customerVO.CustomerCode, "$" + customerVO.CustomerCode, customerVO.CustomerName);
                        }
                    }
                }
                else {
                    UserProfile uProfile = CacheManager.GetUserProfile(token);
                    CustomerProfile cProfile = uProfile as CustomerProfile;
                    customerId = cProfile.CustomerId;
                }
                demandVO.CreatedAt = DateTime.Now;
                demandVO.Status = 2;
                demandVO.CustomerId = customerId;
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
        /// 更新需求状态
        /// </summary>
        /// <param name="DemandId">需求ID</param>
        /// <param name="status">状态</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateDemandStatus"), HttpPost]
        public ResultObject UpdateDemandStatus(string DemandId, int status, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            //if (status != 1)
            //{
                try
                {
                    DemandBO uBO = new DemandBO(new CustomerProfile());
                    if (!string.IsNullOrEmpty(DemandId))
                    {
                        string[] bIdArr = DemandId.Split(',');
                        bool isAllUpdate = true;

                        for (int i = 0; i < bIdArr.Length; i++)
                        {
                            try
                            {

                                DemandViewVO pViewVO = uBO.FindDemandById(Convert.ToInt32(bIdArr[i]));


                                if (status != pViewVO.Status && status == 1)
                                {
                                    //发送站内信息                            
                                    MessageBO mBO = new MessageBO(new CustomerProfile());
                                    mBO.SendMessage("商机状态变更", "你发布的商机需求已经通过审核", pViewVO.CustomerId, MessageType.Project);
                                }
                                DemandVO bVO = new DemandVO();
                                bVO.DemandId = Convert.ToInt32(bIdArr[i]);
                                bVO.Status = status;

                                bool isSuccess = uBO.UpdateDemand(bVO); 
                            }
                            catch
                            {
                                isAllUpdate = false;
                            }
                        }
                        if (isAllUpdate)
                        {
                            return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 1, Message = "部分更新成功!", Result = null };
                        }

                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null };
                    }
                }
                catch
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            //}
            //else
            //{
            //    return new ResultObject() { Flag = 0, Message = "没有权限执行此操作!", Result = null };
            //}
        }
        /// <summary>
        /// 添加需求报价
        /// </summary>
        /// <param name="demandofferVO">报价VO</param>
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
            if (demandofferVO.OfferId < 1)
            {
                demandofferVO.CreatedAt = DateTime.Now;
                demandofferVO.CustomerId = customerId;

                //避免重复提交的情况，10分钟内不得添加两次
                int oldCount = uBO.FindDemandOfferCount("CustomerId = " + customerId + " and DemandId="+ demandofferVO.DemandId + " and NOW()-CreatedAt<600");
                if (oldCount > 0)
                {
                    return new ResultObject() { Flag = 0, Message = "你在刚刚已经提交了该需求的留言，请10分钟后再提交!", Result = null };
                }
                int demandId = uBO.AddDemandOffer(demandofferVO);
                if (demandId > 0)
                {
                    //给雇主发消息
                    DemandViewVO dVO = uBO.FindDemandById(demandofferVO.DemandId);
                    MessageBO mBO = new MessageBO(new CustomerProfile());

                    List<MessageViewVO> messageViewVO = mBO.FindAllMessageByPageIndex("Title='商机需求留言' and SendTo=" + dVO.CustomerId + " and NOW()-SendAt<86400", 1, 20, "SendAt", "desc");

                    if (messageViewVO.Count <= 0)
                    {
                        string msgContent = "尊敬的" + dVO.CustomerName + "：会员" + demandofferVO.Name + "对您发布的商机需求很感兴趣，并进行了留言，请尽快登陆查看！新用户可搜索“众销乐”小程序或公众号快捷登录。【众销乐-资源共享众包销售平台】";
                        string result = MessageTool.SendMobileMsg(msgContent, dVO.Phone);
                    }

                    mBO.SendMessage("商机需求留言", demandofferVO.Name + "给你留言了", dVO.CustomerId, MessageType.Project);

                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = demandId };
                } 
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
            }
            return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
        }

        /// <summary>
        /// 获取需求详情，匿名
        /// </summary>
        /// <param name="id">需求ID</param>
        /// <returns></returns>
        [Route("GetDemandSite"), HttpGet, Anonymous]
        public ResultObject GetDemandSite(int id)
        {
            DemandBO uBO = new DemandBO(new CustomerProfile());
            DemandViewVO uVO = uBO.FindDemandById(id);
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
        /// 获取需求分类列表，匿名
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
        /// 获取需求留言列表
        /// </summary>
        /// <param name="DemandId">DemandId</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetOfferByDemand"), HttpGet]
        public ResultObject GetOfferByDemand(int DemandId, string token)
        {
            DemandBO uBO = new DemandBO(new CustomerProfile());
            List<DemandOfferVO> tenderInviteList = uBO.FindOfferByDemand(DemandId);
            if (tenderInviteList != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = tenderInviteList };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取发布的需求列表，匿名
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
            string conditionStr = "Status <> 0 and Status <> 2 and TO_DAYS(EffectiveEndDate) - TO_DAYS(now()) >=0 and " + condition.Filter.Result();
            Paging pageInfo = condition.PageInfo;
            DemandBO uBO = new DemandBO(new CustomerProfile());
            
            List<DemandViewVO> list = uBO.FindDemandAllByPageIndex(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].HeaderLogo=uBO.GetDemandIMG(list[i].DemandId);
                }
            }
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list };
        }
    }
}
