using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Newtonsoft.Json;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.BO;
using SPlatformService.TokenMange;
using SPlatformService.Models;
using CoreFramework.VO;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// 广告弹框控制器
    /// </summary>
    [RoutePrefix("SPWebAPI/AdPopup")]
    [TokenProjector]
    public class AdPopupController : ApiController
    {
        /// <summary>
        /// 获取当前用户的 CustomerProfile（从 token 解析）
        /// </summary>
        private UserProfile GetCustomerProfile(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            return uProfile;
        }

        #region 小程序前端接口（需登录）

        /// <summary>
        /// 获取有效广告配置（供小程序调用）
        /// </summary>
        /// <param name="slotId">广告位标识（必填）</param>
        /// <param name="pagePath">当前页面路径（必填）</param>
        /// <param name="token">用户令牌</param>
        /// <returns></returns>

        [Route("GetAd"), HttpGet, Anonymous]
        public ResultObject GetAd(string slotId, string pagePath, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(slotId))
                {
                    return new ResultObject() { Flag = 0, Message = "参数缺失：slotId 为必填", Result = null };
                }

                // 获取用户ID（允许未登录）
                string userId = null;
                if (!string.IsNullOrEmpty(token))
                {
                    UserProfile cProfile = GetCustomerProfile(token);
                    if (cProfile != null)
                    {
                        userId = cProfile.UserId.ToString();
                    }
                }

                AdPopupBO adBO = new AdPopupBO(new CustomerProfile());
                AdPopupConfigVO config = adBO.GetValidAdBySlotId(slotId, userId);

                if (config != null )
                {
                    // 记录曝光日志（仅登录用户）
                    if (!string.IsNullOrEmpty(userId) && config.FrequencyLimit != 0)
                    {
                        try
                        {
                            adBO.RecordAdLog(config.AdPopupConfigId, userId, pagePath, 1);
                        }
                        catch { /* 日志记录失败不影响主流程 */ }
                    }

                    return new ResultObject() { Flag = 1, Message = "获取成功", Result = config };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "当前无有效广告", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "接口异常：" + ex.Message, Result = ex.ToString() };
            }
        }

        /// <summary>
        /// 上报广告交互日志（曝光/点击/关闭）
        /// </summary>
        /// <param name="logData">日志数据</param>
        /// <param name="token">用户令牌</param>
        /// <returns></returns>

        [Route("ReportLog"), HttpPost]
        public ResultObject ReportLog([FromBody] AdPopupLogVO logData, string token)
        {
            try
            {
                if (logData == null || logData.AdPopupConfigId <= 0)
                {
                    return new ResultObject() { Flag = 0, Message = "参数无效", Result = null };
                }

                // 获取用户ID
                string userId = null;
                if (!string.IsNullOrEmpty(token))
                {
                    UserProfile cProfile = GetCustomerProfile(token);
                    if (cProfile != null)
                    {
                        userId = cProfile.UserId.ToString();
                    }
                }

                // 未登录用户不记录日志
                if (string.IsNullOrEmpty(userId))
                {
                    return new ResultObject() { Flag = 1, Message = "未登录，无需记录", Result = null };
                }

                logData.UserId = userId;
                logData.CreatedAt = DateTime.Now;

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    logData.Ip = HttpContext.Current.Request.UserHostAddress;
                }

                AdPopupBO adBO = new AdPopupBO(new CustomerProfile());
                int result = adBO.AddAdPopupLog(logData);

                if (result > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "上报成功", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "上报失败", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "接口异常：" + ex.Message, Result = ex.ToString() };
            }
        }

        #endregion

        #region 后台管理接口（需管理员权限，此处简化）

        /// <summary>
        /// 分页查询广告配置列表（后台管理）
        /// </summary>
        [Route("GetConfigList"), HttpPost]
        public ResultObject GetConfigList([FromBody] ConditionModel condition, string token)
        {
            try
            {
                if (condition == null || condition.PageInfo == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空", Result = null };
                }

                UserProfile cProfile = GetCustomerProfile(token);
               
                AdPopupBO adBO = new AdPopupBO(new CustomerProfile());
                Paging pageInfo = condition.PageInfo;

                // 构建查询条件（可根据 Filter 扩展）
                string conditionStr = "1=1";
                if (!string.IsNullOrEmpty(pageInfo.SearchText))
                {
                    conditionStr += $" AND (Name LIKE '%{pageInfo.SearchText}%' OR SlotId LIKE '%{pageInfo.SearchText}%')";
                }

                List<AdPopupConfigVO> list = adBO.FindAdPopupConfigAllByPageIndex(
                    conditionStr,
                    (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1,
                    pageInfo.PageIndex * pageInfo.PageCount,
                    pageInfo.SortName ?? "AdPopupConfigId",
                    pageInfo.SortType ?? "DESC"
                );

                int total = adBO.FindAdPopupConfigCount(conditionStr);

                return new ResultObject()
                {
                    Flag = 1,
                    Message = "获取成功",
                    Result = list,
                    Count = total
                };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "接口异常：" + ex.Message, Result = ex.ToString() };
            }
        }

        /// <summary>
        /// 新增广告配置
        /// </summary>
        [Route("AddConfig"), HttpPost]
        public ResultObject AddConfig([FromBody] AdPopupConfigVO vo, string token)
        {
            try
            {
                if (vo == null || string.IsNullOrEmpty(vo.SlotId) || string.IsNullOrEmpty(vo.ImageUrl))
                {
                    return new ResultObject() { Flag = 0, Message = "关键参数缺失（SlotId、ImageUrl 必填）", Result = null };
                }

                UserProfile cProfile = GetCustomerProfile(token);
                if (cProfile == null)
                {
                    return new ResultObject() { Flag = -1, Message = "无效的token", Result = null };
                }

                AdPopupBO adBO = new AdPopupBO(new CustomerProfile());
                int id = adBO.AddAdPopupConfig(vo);

                if (id > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "添加成功", Result = id };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "添加失败", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "接口异常：" + ex.Message, Result = ex.ToString() };
            }
        }

        /// <summary>
        /// 更新广告配置
        /// </summary>
        [Route("UpdateConfig"), HttpPost]
        public ResultObject UpdateConfig([FromBody] AdPopupConfigVO vo, string token)
        {
            try
            {
                if (vo == null || vo.AdPopupConfigId <= 0)
                {
                    return new ResultObject() { Flag = 0, Message = "参数无效：缺少 AdPopupConfigId", Result = null };
                }

                UserProfile cProfile = GetCustomerProfile(token);
                if (cProfile == null)
                {
                    return new ResultObject() { Flag = -1, Message = "无效的token", Result = null };
                }

                AdPopupBO adBO = new AdPopupBO(new CustomerProfile());
                bool result = adBO.UpdateAdPopupConfig(vo);

                if (result)
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "接口异常：" + ex.Message, Result = ex.ToString() };
            }
        }

        /// <summary>
        /// 删除广告配置（物理删除）
        /// </summary>
        [Route("DeleteConfig"), HttpGet]
        public ResultObject DeleteConfig(int adPopupConfigId, string token)
        {
            try
            {
                if (adPopupConfigId <= 0)
                {
                    return new ResultObject() { Flag = 0, Message = "参数无效", Result = null };
                }

                UserProfile cProfile = GetCustomerProfile(token);
                if (cProfile == null)
                {
                    return new ResultObject() { Flag = -1, Message = "无效的token", Result = null };
                }

                AdPopupBO adBO = new AdPopupBO(new CustomerProfile());
                int result = adBO.DeleteAdPopupConfig(adPopupConfigId);

                if (result > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "删除成功", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "删除失败", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "接口异常：" + ex.Message, Result = ex.ToString() };
            }
        }

        #endregion

        #region 辅助类（若不存在则需引用）

        // 注意：ConditionModel、Paging 等需根据实际项目定义，此处仅示意
        public class ConditionModel
        {
            public Paging PageInfo { get; set; }
            public object Filter { get; set; }
        }

        public class Paging
        {
            public int PageIndex { get; set; }
            public int PageCount { get; set; }
            public string SearchText { get; set; }
            public string SortName { get; set; }
            public string SortType { get; set; }
        }

        #endregion
    }
}