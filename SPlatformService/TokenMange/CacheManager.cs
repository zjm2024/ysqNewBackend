using CoreFramework.VO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.DAO;
using SPLibrary.UserManagement.VO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Web;

namespace SPlatformService.TokenMange
{
    /// <summary>
    /// 缓存管理
    /// 将令牌、用户凭证以及过期时间的关系数据存放于Cache中
    /// </summary>
    public class CacheManager
    {
        //单位 秒
        private static int TokenTimeout = 3600 * 24 * 14;
        /// <summary>
        /// 初始化缓存数据结构
        /// </summary>
        /// token 令牌
        /// uuid 用户ID凭证
        /// userType 用户类别
        /// timeout 过期时间
        /// <remarks>
        /// </remarks>
        private static void CacheInit(string userId, bool isReset=false)
        {
            string cacheKey = $"PASSPORT.TOKEN.{userId}";
            if (HttpRuntime.Cache[cacheKey] == null|| isReset)
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("Token", Type.GetType("System.String"));
                dt.Columns["Token"].Unique = true;

                dt.Columns.Add("CompanyId", typeof(int));
                dt.Columns["CompanyId"].DefaultValue = 0;

                dt.Columns.Add("DepartmentId", typeof(int));
                dt.Columns["DepartmentId"].DefaultValue = 0;

                dt.Columns.Add("UserId", typeof(int));
                dt.Columns["UserId"].DefaultValue = 0;

                dt.Columns.Add("IsUser", typeof(bool));
                dt.Columns["IsUser"].DefaultValue = false;

                dt.Columns.Add("Timeout", Type.GetType("System.DateTime"));
                dt.Columns["Timeout"].DefaultValue = DateTime.Now.AddSeconds(TokenTimeout);

                DataColumn[] keys = new DataColumn[1];
                keys[0] = dt.Columns["Token"];
                dt.PrimaryKey = keys;

                //暂不考虑数据库存储，遇到IIS重启等其它因素，直接让使用者重新登录
                //var tempCaches = new DataTable();
                //DataRow dr = dt.NewRow();
                //dr["Token"] = "abcd";
                //dr["uuid"] = 10;
                ////dr["userType"] = "User";
                //dr["Timeout"] = DateTime.Now.AddDays(7);
                //dt.Rows.Add(dr);
                if (userId != "0")
                {
                    UserBO uBO = new UserBO(new UserProfile());
                    List<TokenVO> dtList = uBO.FindTokeByUserId(userId);
                    foreach (TokenVO tVO in dtList)
                    {
                        if (tVO.Token.IndexOf(".") != -1)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Token"] = tVO.Token;
                            dr["CompanyId"] = tVO.CompanyId;
                            dr["DepartmentId"] = tVO.DepartmentId;
                            dr["UserId"] = tVO.UserId;
                            dr["IsUser"] = tVO.IsUser;
                            dr["Timeout"] = tVO.Timeout;
                            dt.Rows.Add(dr);
                        }
                    }

                }

                //Cache的过期时间为 令牌过期时间*2
                HttpRuntime.Cache.Insert(cacheKey, dt, null, DateTime.MaxValue, TimeSpan.FromDays(7 * 2));
            }
        }

        /// <summary>
        /// 获取用户UUID标识
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static int GetUserId(string token)
        {
            int userId;
            string cacheKey;
            TokenSplit(token, out userId, out cacheKey);


            try
            {
                UserBO uBO = new UserBO(new UserProfile());
                List<TokenVO> dtList = uBO.FindTokeByToken(token, userId);
                if (dtList != null)
                {
                    if (dtList.Count > 0)
                        return Convert.ToInt32(dtList[0]["UserId"]);

                }
            }
            catch (Exception ex)
            {
                LogError($"GetUserId查找令牌行失败: {token}", ex);
                return 0;
            }

            return 0;
        }

        /// <summary>
        /// 获取CompanyID标识
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static int GetCompanyId(string token)
        {
            int companyId;
            string cacheKey;
            TokenSplit(token, out companyId, out cacheKey);

            try
            {
                UserBO uBO = new UserBO(new UserProfile());
                List<TokenVO> dtList = uBO.FindTokeByToken(token, companyId);
                if (dtList != null)
                {
                    if (dtList.Count > 0)
                        return Convert.ToInt32(dtList[0]["CompanyId"]);

                }
            }
            catch (Exception ex)
            {
                LogError($"GetCompanyId查找令牌行失败: {token}", ex);
                return 0;
            }

            return 0;
        }
        
        /// <summary>
        /// 获取UserProfile
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static UserProfile GetUserProfile(string token)
        {
            int userId;
            string cacheKey;
            TokenSplit(token, out userId, out cacheKey);

            try
            {
                UserBO uBO = new UserBO(new UserProfile());
                List<TokenVO> dtList = uBO.FindTokeByToken(token, userId);

                if (dtList != null)
                {
                    if (dtList.Count > 0)
                    {

                        if (Convert.ToBoolean(dtList[0]["IsUser"]))
                        {
                            //平台用户
                            UserProfile userProfile = new UserProfile();
                            userProfile.UserId = Convert.ToInt32(dtList[0]["UserId"]);
                            userProfile.DeaprtmentId = Convert.ToInt32(dtList[0]["DepartmentId"]);
                            userProfile.CompanyId = Convert.ToInt32(dtList[0]["CompanyId"]);

                            return userProfile;
                        }
                        else
                        {
                            //会员
                            CustomerProfile customerProfile = new CustomerProfile();
                            customerProfile.CustomerId = Convert.ToInt32(dtList[0]["UserId"]);

                            return customerProfile;
                        }




                    }

                }


            }
            catch (Exception ex)
            {
                LogError($"GetUserProfile查找令牌行失败: {token}", ex);
                return null;
            }

            return null;
        }        

        /// <summary>
        /// 判断令牌是否存在
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <returns></returns>
        public static bool TokenIsExist(string token)
        {
            int userId;
            string cacheKey;
            TokenSplit(token, out userId, out cacheKey);

            if (userId == 0)
                return false;

            try
            {
                UserBO uBO = new UserBO(new UserProfile());
                List<TokenVO> dtList = uBO.FindTokeByToken(token, userId);

                if (dtList != null)
                {
                    if (dtList.Count > 0)
                    {

                        var timeout = DateTime.Parse(dtList[0]["Timeout"].ToString());
                        var isUser = !Boolean.Parse(dtList[0]["IsUser"].ToString());
                        
                       if  (DateTime.Now>= timeout.AddDays(-2))  //最大超时2天之内才更新
                        //if (timeout > DateTime.Now)
                        {
                            //如果已经存在，更新timout
                            TokenTimeUpdate(token, isUser);
                            return true;
                        }

                        else if (timeout< DateTime.Now)
                        {
                            RemoveToken(token);
                            return false;
                        }
                        else
                        { 
                            return true;
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                LogError($"TokenIsExist查找令牌行失败: {token}", ex);
                return false;
            }

            return false;
        }

        /// <summary>
        /// 移除某令牌
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static bool RemoveToken(string token)
        {
            int userId;
            string cacheKey;
            TokenSplit(token, out userId, out cacheKey);

            try
            {
                UserBO uBO = new UserBO(new UserProfile());
                uBO.DeleteTokenbyToken(token, userId);
            }
            catch (Exception ex)
            {
                LogError($"RemoveToken移除令牌行失败: {token}", ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更新令牌过期时间
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="time">过期时间</param>
        public static void TokenTimeUpdate(string token ,bool isUser)
        {
            int userId;
            string cacheKey;
            TokenSplit(token, out userId, out cacheKey);
            TokenVO tVO = new TokenVO();
            tVO.Token = token;
            tVO.UserId = userId;
            tVO.Timeout = DateTime.Now.AddSeconds(TokenTimeout);
            if (isUser)  //不是平台用户
            {
                UserBO uBO = new UserBO(new UserProfile());
                try
                {
                    uBO.UpdateTokenTime(tVO);
                }
                catch (Exception ex)
                {
                    LogError($"TokenTimeUpdate更新令牌行失败: {token}", ex);
                }

            }
        }

        /// <summary>
        /// 添加令牌
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="departmentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string TokenInsert(int companyId,int departmentId, int userId)
        {
            try
            {
                UserBO uBo = new UserBO(new UserProfile());
                List<TokenVO> dtList = uBo.FindTokeByParams("CompanyId=" + companyId + " and DepartmentId=" + departmentId + " and UserId=" + userId);

                if (dtList != null)
                {
                    if (dtList.Count > 0)
                    {

                        //存在，什么都不做，更新时间，并直接返回token
                        TokenVO tVO = new TokenVO();
                        tVO.Token = dtList[0]["Token"].ToString();
                        tVO.CompanyId = companyId;
                        tVO.DepartmentId = departmentId;
                        tVO.UserId = userId;
                        tVO.Timeout = DateTime.Now.AddSeconds(TokenTimeout);

                        uBo.UpdateTokenTime(tVO);


                        return tVO.Token;

                    }
                    else
                    {
                        string guid = Guid.NewGuid().ToString();
                        string base64Userid = Convert.ToBase64String(Encoding.UTF8.GetBytes(userId.ToString()));
                        string token = $"{guid}.{base64Userid}";

                        //add to DB 
                        TokenVO tVO = new TokenVO();
                        tVO.Token = token;
                        tVO.CompanyId = companyId;
                        tVO.DepartmentId = departmentId;
                        tVO.UserId = userId;
                        tVO.IsUser = true;
                        tVO.Timeout = DateTime.Now.AddSeconds(TokenTimeout);

                        uBo.InsertToken(tVO);
                        //保存成功后才返回token
                        return token;

                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"TokenInsert插入令牌行失败: {userId}", ex);
                return "";
            }

            return "";
        }

        public static string TokenInsert(int customerId)
        {
            try
            {
                UserBO uBo = new UserBO(new UserProfile());
                List<TokenVO> dtList = uBo.FindTokeByParams("CompanyId=0 and DepartmentId=0 and UserId=" + customerId);

                if (dtList != null)
                {
                    if (dtList.Count > 0)
                    {
                        //存在，什么都不做，更新时间，并直接返回token
                        TokenVO tVO = new TokenVO();
                        tVO.Token = dtList[0]["Token"].ToString();
                        tVO.UserId = customerId;
                        tVO.CompanyId = 0;
                        tVO.DepartmentId = 0;
                        tVO.Timeout = DateTime.Now.AddSeconds(TokenTimeout);

                        uBo.UpdateTokenTime(tVO);

                        return tVO.Token;

                    }
                    else
                    {
                        string guid = Guid.NewGuid().ToString();
                        string base64Userid = Convert.ToBase64String(Encoding.UTF8.GetBytes(customerId.ToString()));
                        string token = $"{guid}.{base64Userid}";

                        //add to DB 
                        TokenVO tVO = new TokenVO();
                        tVO.Token = token;
                        tVO.CompanyId = 0;
                        tVO.DepartmentId = 0;
                        tVO.UserId = customerId;
                        tVO.IsUser = false;
                        tVO.Timeout = DateTime.Now.AddSeconds(TokenTimeout);

                        uBo.InsertToken(tVO);
                        return token;
                    }
                }

            }

            catch (Exception ex)
            {
                LogError($"TokenInsert插入令牌行失败: {customerId}", ex);
                return "";
            }


            return "";
        }

        public static void TokenSplit(string token, out int userId, out string cacheKey)
        {
            string[] parts = token.Split('.');
            if (parts.Length > 1)
            {
                string guid = parts[0];
                string base64Userid = parts[1];
                byte[] decodedBytes = Convert.FromBase64String(base64Userid);
                string userid = Encoding.UTF8.GetString(decodedBytes);
                userId = Convert.ToInt32(userid);
                cacheKey = $"PASSPORT.TOKEN.{userId}";
            }
            else
            {
                string guid = parts[0];
                userId = 0;
                cacheKey = $"PASSPORT.TOKEN.{userId}";
            }
        }


        /// <summary>
        /// 记录错误日志
        /// </summary>
        private static void LogError(string message, Exception ex = null)
        {
            try
            {
                LogBO log = new LogBO(typeof(CacheManager));
                string errorMsg = $"{message}\r\n";

                if (ex != null)
                {
                    errorMsg += $"Message: {ex.Message}\r\nStack: {ex.StackTrace}\r\nSource: {ex.Source}";
                }

                log.Error(errorMsg);
            }
            catch
            {
                // 日志记录失败，不抛出异常
            }
        }


    }
}