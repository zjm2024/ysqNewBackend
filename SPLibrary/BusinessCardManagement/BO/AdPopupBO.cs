using CoreFramework.DAO;
using CoreFramework.VO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Drawing;
using System.IO;
using SPLibrary.WebConfigInfo;
using System.Linq;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.BusinessCardManagement.DAO;
using SPLibrary.CustomerManagement.BO;
using System.Text.RegularExpressions;

namespace SPLibrary.BusinessCardManagement.BO
{
    /// <summary>
    /// 广告弹框业务逻辑层
    /// </summary>
    public class AdPopupBO
    {
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();

        public AdPopupBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }

        #region 广告配置 CRUD

        /// <summary>
        /// 添加广告配置
        /// </summary>
        public int AddAdPopupConfig(AdPopupConfigVO vo)
        {
            try
            {
                IAdPopupConfigDAO dao = BusinessCardManagementDAOFactory.AdPopupConfigDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int id = dao.Insert(vo);
                    return id;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AdPopupBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新广告配置
        /// </summary>
        public bool UpdateAdPopupConfig(AdPopupConfigVO vo)
        {
            IAdPopupConfigDAO dao = BusinessCardManagementDAOFactory.AdPopupConfigDAO(this.CurrentCustomerProfile);
            try
            {
                dao.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AdPopupBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 删除广告配置（物理删除，慎用）
        /// </summary>
        public int DeleteAdPopupConfig(int adPopupConfigId)
        {
            IAdPopupConfigDAO dao = BusinessCardManagementDAOFactory.AdPopupConfigDAO(this.CurrentCustomerProfile);
            try
            {
                dao.DeleteByParams("AdPopupConfigId = " + adPopupConfigId);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 逻辑删除（将状态设为 0-删除）
        /// </summary>
        public bool SoftDeleteAdPopupConfig(int adPopupConfigId)
        {
            AdPopupConfigVO vo = FindAdPopupConfigById(adPopupConfigId);
            if (vo == null) return false;
            vo.Status = 0; // 0-删除
            return UpdateAdPopupConfig(vo);
        }

        /// <summary>
        /// 根据ID获取广告配置
        /// </summary>
        public AdPopupConfigVO FindAdPopupConfigById(int adPopupConfigId)
        {
            IAdPopupConfigDAO dao = BusinessCardManagementDAOFactory.AdPopupConfigDAO(this.CurrentCustomerProfile);
            return dao.FindById(adPopupConfigId);
        }

        /// <summary>
        /// 根据条件获取广告配置列表
        /// </summary>
        public List<AdPopupConfigVO> FindAdPopupConfigList(string condition)
        {
            IAdPopupConfigDAO dao = BusinessCardManagementDAOFactory.AdPopupConfigDAO(this.CurrentCustomerProfile);
            return dao.FindByParams(condition);
        }

        /// <summary>
        /// 分页获取广告配置列表
        /// </summary>
        public List<AdPopupConfigVO> FindAdPopupConfigAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IAdPopupConfigDAO dao = BusinessCardManagementDAOFactory.AdPopupConfigDAO(this.CurrentCustomerProfile);
            return dao.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取广告配置总数
        /// </summary>
        public int FindAdPopupConfigCount(string condition)
        {
            IAdPopupConfigDAO dao = BusinessCardManagementDAOFactory.AdPopupConfigDAO(this.CurrentCustomerProfile);
            return dao.FindTotalCount(condition);
        }

        #endregion

        #region 广告日志 CRUD

        /// <summary>
        /// 添加广告日志（曝光/点击/关闭）
        /// </summary>
        public int AddAdPopupLog(AdPopupLogVO vo)
        {
            try
            {
                IAdPopupLogDAO dao = BusinessCardManagementDAOFactory.AdPopupLogDAO(this.CurrentCustomerProfile);
                return dao.Insert(vo);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AdPopupBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 批量添加广告日志（用于批量上报）
        /// </summary>
        public bool AddAdPopupLogList(List<AdPopupLogVO> voList)
        {
            try
            {
                IAdPopupLogDAO dao = BusinessCardManagementDAOFactory.AdPopupLogDAO(this.CurrentCustomerProfile);
                dao.InsertList(voList);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(AdPopupBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 根据条件查询日志列表
        /// </summary>
        public List<AdPopupLogVO> FindAdPopupLogList(string condition)
        {
            IAdPopupLogDAO dao = BusinessCardManagementDAOFactory.AdPopupLogDAO(this.CurrentCustomerProfile);
            return dao.FindByParams(condition);
        }

        /// <summary>
        /// 获取某用户某广告今日已展示次数
        /// </summary>
        public int CountTodayLogs(long adPopupConfigId, string userId)
        {
            string condition = string.Format("AdPopupConfigId={0} AND UserId='{1}' AND Action=1 AND DATE(CreatedAt)=CURDATE()", adPopupConfigId, userId);
            IAdPopupLogDAO dao = BusinessCardManagementDAOFactory.AdPopupLogDAO(this.CurrentCustomerProfile);
            return dao.FindTotalCount(condition);
        }

        #endregion

        #region 核心业务方法

        /// <summary>
        /// 根据广告位ID获取当前有效的广告（自动校验时间、状态、页面匹配、频次）
        /// </summary>
        /// <param name="slotId">广告位唯一标识</param>
        /// <param name="userId">用户标识（openid），可为空</param>
        /// <returns>有效的广告配置VO，若无则返回null</returns>
        public AdPopupConfigVO GetValidAdBySlotId(string slotId,  string userId)
        {
            if (string.IsNullOrEmpty(slotId))
                return null;

            // 1. 查询该广告位下所有启用的配置（按优先级降序）
            string condition = string.Format("SlotId='{0}' AND Status=1", slotId);
            var list = FindAdPopupConfigList(condition);
            if (list == null || list.Count == 0)
                return null;

            // 按优先级降序排列，取优先级最高的
            var ordered = list.OrderByDescending(c => c.Priority).ToList();

            foreach (var config in ordered)
            {
                // 2. 时间范围校验
                if (config.StartTime.HasValue && config.StartTime > DateTime.Now)
                    continue;
                if (config.EndTime.HasValue && config.EndTime < DateTime.Now)
                    continue;

                //// 3. 页面匹配校验
                //if (!IsPageMatched(config, pagePath))
                //    continue;

                // 4. 频次限制校验（仅当用户已登录）
                if (!string.IsNullOrEmpty(userId) && config.FrequencyLimit > 0)
                {
                    int todayCount = CountTodayLogs(config.AdPopupConfigId, userId);
                    if (todayCount >= config.FrequencyLimit)
                        continue;
                }

                // 所有校验通过，返回该配置
                return config;
            }

            return null;
        }

        /// <summary>
        /// 判断广告配置是否匹配当前页面路径
        /// </summary>
        private bool IsPageMatched(AdPopupConfigVO config, string pagePath)
        {
            // PageMatchMode: 0-所有页面, 1-精确匹配, 2-前缀匹配
            if (config.PageMatchMode == 0)
                return true;

            if (string.IsNullOrEmpty(config.PagePaths))
                return false;

            // 解析 PagePaths JSON 数组
            List<string> pagePathList = null;
            try
            {
                pagePathList = JsonConvert.DeserializeObject<List<string>>(config.PagePaths);
            }
            catch
            {
                pagePathList = new List<string>();
            }

            if (pagePathList == null || pagePathList.Count == 0)
                return false;

            if (config.PageMatchMode == 1) // 精确匹配
            {
                return pagePathList.Contains(pagePath);
            }
            else if (config.PageMatchMode == 2) // 前缀匹配
            {
                return pagePathList.Any(p => pagePath.StartsWith(p));
            }

            return false;
        }

        /// <summary>
        /// 记录广告日志（仅登录用户）
        /// </summary>
        public void RecordAdLog(long adPopupConfigId, string userId, string pagePath, int action, string sessionId = null, string ip = null)
        {
            // 未登录用户不记录日志
            if (string.IsNullOrEmpty(userId)) return;

            AdPopupLogVO log = new AdPopupLogVO();
            log.AdPopupConfigId = adPopupConfigId;
            log.UserId = userId;
            log.PagePath = pagePath;
            log.Action = action; // 1-曝光 2-点击 3-关闭
            log.SessionId = sessionId;
            log.Ip = ip;
            log.CreatedAt = DateTime.Now;
            AddAdPopupLog(log);
        }

        #endregion
    }
}