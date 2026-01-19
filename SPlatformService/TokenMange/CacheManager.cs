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

            CacheInit(userId.ToString());
    

            DataTable dt = (DataTable)HttpRuntime.Cache[cacheKey];

            DataRow[] dr = new DataRow[0];
            Monitor.Enter(dt);
            try
            {
                dr = dt.Select("token = '" + token + "'");
            }
            catch { }
            finally
            {
                Monitor.Exit(dt);
            }

            if (dr.Length > 0)
            {
                return Convert.ToInt32(dr[0]["UserId"]);
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

            CacheInit(companyId.ToString());

            DataTable dt = (DataTable)HttpRuntime.Cache[cacheKey];

            DataRow[] dr = new DataRow[0];
            Monitor.Enter(dt);
            try
            {
                dr = dt.Select("token = '" + token + "'");
            }
            catch { }
            finally
            {
                Monitor.Exit(dt);
            }

            if (dr.Length > 0)
            {
                return Convert.ToInt32(dr[0]["CompanyId"]);
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

            CacheInit(userId.ToString());

            DataTable dt = (DataTable)HttpRuntime.Cache[cacheKey];

            DataRow[] dr = new DataRow[0];
            Monitor.Enter(dt);
            try
            {
                dr = dt.Select("token = '" + token + "'");
            }
            catch { }
            finally
            {
                Monitor.Exit(dt);
            }

            
            if (dr.Length > 0)
            {
                //不从DB获取，如果需要LoginName和Phone，另外再修改登录方法
                //UserProfile userProfile = new UserProfile();
                //int userId = Convert.ToInt32(dr[0]["UserId"]);
                //IUserViewDAO uDAO = new UserViewDAO(new UserProfile());
                //UserViewVO uVO = uDAO.FindById(userId);

                //userProfile.UserId = uVO.UserId;
                //userProfile.LoginName = uVO.LoginName;
                //userProfile.UserName = uVO.UserName;
                //userProfile.DeaprtmentId = uVO.DepartmentId;
                //userProfile.CompanyId = uVO.CompanyId;
                //userProfile.Phone = uVO.Phone;
                if (Convert.ToBoolean(dr[0]["IsUser"]))
                {
                    //平台用户
                    UserProfile userProfile = new UserProfile();
                    userProfile.UserId = Convert.ToInt32(dr[0]["UserId"]);
                    userProfile.DeaprtmentId = Convert.ToInt32(dr[0]["DepartmentId"]);
                    userProfile.CompanyId = Convert.ToInt32(dr[0]["CompanyId"]);

                    return userProfile;
                }
                else
                {
                    //会员
                    CustomerProfile customerProfile = new CustomerProfile();
                    customerProfile.CustomerId = Convert.ToInt32(dr[0]["UserId"]);

                    return customerProfile;
                }
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

            CacheInit(userId.ToString());

            DataTable dt = (DataTable)HttpRuntime.Cache[cacheKey];

            DataRow[] dr = new DataRow[0];
            Monitor.Enter(dt);
            try
            {
                dr = dt.Select("token = '" + token + "'");
            }
            catch { }
            finally
            {
                Monitor.Exit(dt);
            }

            if (dr.Length > 0)
            {
                var timeout = DateTime.Parse(dr[0]["Timeout"].ToString());
                if (timeout > DateTime.Now)
                {
                    //如果已经存在，更新timout
                    TokenTimeUpdate(token);
                    return true;
                }
                else
                {
                    RemoveToken(token);
                    return false;
                }
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

            CacheInit(userId.ToString());

            DataTable dt = (DataTable)HttpRuntime.Cache[cacheKey];
            DataRow[] dr = new DataRow[0];
            Monitor.Enter(dt);
            try
            {
                dr = dt.Select("token = '" + token + "'");
            }
            catch { }
            finally
            {
                Monitor.Exit(dt);
            }

            if (dr.Length > 0)
            {
                UserBO uBO = new UserBO(new UserProfile());
                try
                {
                    uBO.DeleteTokenbyToken(token, userId);
                    dt.Rows.Remove(dr[0]);
                }
                catch
                {

                }
            }
            return true;
        }

        /// <summary>
        /// 更新令牌过期时间
        /// </summary>
        /// <param name="Token">令牌</param>
        /// <param name="time">过期时间</param>
        public static void TokenTimeUpdate(string token)
        {
            int userId;
            string cacheKey;
            TokenSplit(token, out userId, out cacheKey);
            CacheInit(userId.ToString());

            DataTable dt = (DataTable)HttpRuntime.Cache[cacheKey];

            DataRow[] dr = new DataRow[0];
            Monitor.Enter(dt);
            try
            {
                dr = dt.Select("token = '" + token + "'");
            }
            catch { }
            finally
            {
                Monitor.Exit(dt);
            }

            if (dr.Length > 0)
            {
                dr[0]["Timeout"] = DateTime.Now.AddSeconds(TokenTimeout);

                TokenVO tVO = new TokenVO();
                tVO.Token = token;
                tVO.UserId = userId;
                tVO.Timeout = DateTime.Now.AddSeconds(TokenTimeout);
                if (!Boolean.Parse(dr[0]["IsUser"].ToString()))
                {
                    UserBO uBO = new UserBO(new UserProfile());
                    try
                    {
                        uBO.UpdateTokenTime(tVO);
                    }
                    catch
                    {

                    }
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
            string cacheKey = $"PASSPORT.TOKEN.{userId.ToString()}";
            CacheInit(userId.ToString());
            UserBO uBo = new UserBO(new UserProfile());
            DataTable dt = (DataTable)HttpRuntime.Cache[cacheKey];

            //// token不存在则添加
            bool isExists = false;
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["CompanyId"]) == companyId
                    && Convert.ToInt32(row["DepartmentId"]) == departmentId
                    && Convert.ToInt32(row["UserId"]) == userId
                    )
                {
                    isExists = true;
                    //存在，什么都不做，更新时间，并直接返回token
                    TokenVO tVO = new TokenVO();
                    tVO.Token = row["Token"].ToString();
                    tVO.CompanyId = companyId;
                    tVO.DepartmentId = departmentId;
                    tVO.UserId = userId;
                    tVO.Timeout = DateTime.Now.AddSeconds(TokenTimeout);

                    List<TokenVO> tokenlist = uBo.FindTokeByToken(row["Token"].ToString(), userId);

                    if (tokenlist.Count > 0)
                        uBo.UpdateTokenTime(tVO);
                    else
                        uBo.InsertToken(tVO);


                    row["Timeout"] = tVO.Timeout;
                    HttpRuntime.Cache[cacheKey] = dt;

                    return tVO.Token;

                }
            }
            if (!isExists)
            {

                DataRow dr = dt.NewRow();
                string guid = Guid.NewGuid().ToString();
                string base64Userid = Convert.ToBase64String(Encoding.UTF8.GetBytes(userId.ToString()));
                string token = $"{guid}.{base64Userid}";

                dr["Token"] = token;   //token 带userId传递
                dr["CompanyId"] = companyId;
                dr["DepartmentId"] = departmentId;
                dr["UserId"] = userId;
                dr["IsUser"] = true;
                dr["Timeout"] = DateTime.Now.AddSeconds(TokenTimeout);
                dt.Rows.Add(dr);


                //add to DB 
                TokenVO tVO = new TokenVO();
                tVO.Token = token;
                tVO.CompanyId = companyId;
                tVO.DepartmentId = departmentId;
                tVO.UserId = userId;
                tVO.IsUser = true;
                tVO.Timeout = DateTime.Now.AddSeconds(TokenTimeout);

                uBo.InsertToken(tVO);
                //保存成功后才更新缓存表
                HttpRuntime.Cache[cacheKey] = dt;
                return token;
            }
            else
                return "";
        }

        public static string TokenInsert(int customerId)
        {
            CacheInit(customerId.ToString());
            string cacheKey = $"PASSPORT.TOKEN.{customerId.ToString()}";
            DataTable dt = (DataTable)HttpRuntime.Cache[cacheKey];
            UserBO uBo = new UserBO(new UserProfile());
            bool isExists = false;

            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["CompanyId"]) == 0
                    && Convert.ToInt32(row["DepartmentId"]) == 0
                    && Convert.ToInt32(row["UserId"]) == customerId
                    )
                {
                    isExists = true;
                    //存在，什么都不做，更新时间，并直接返回token
                    TokenVO tVO = new TokenVO();
                    tVO.Token = row["Token"].ToString();
                    tVO.UserId = customerId;
                    tVO.CompanyId = 0;
                    tVO.DepartmentId = 0;
                    tVO.Timeout = DateTime.Now.AddSeconds(TokenTimeout);

                    List<TokenVO> tokenlist = uBo.FindTokeByToken(row["Token"].ToString(), customerId);

                    if (tokenlist.Count > 0)
                        uBo.UpdateTokenTime(tVO);
                    else
                        uBo.InsertToken(tVO);

                    row["Timeout"] = tVO.Timeout;
                    HttpRuntime.Cache[cacheKey] = dt;

                    return tVO.Token;
                }
            }

            if (!isExists)
            {
                try
                {
                    DataRow dr = dt.NewRow();
                    string guid = Guid.NewGuid().ToString();
                    string base64Userid = Convert.ToBase64String(Encoding.UTF8.GetBytes(customerId.ToString()));
                    string token = $"{guid}.{base64Userid}";
                    dr["Token"] = token;
                    dr["CompanyId"] = 0;
                    dr["DepartmentId"] = 0;
                    dr["UserId"] = customerId;
                    dr["IsUser"] = false;
                    dr["Timeout"] = DateTime.Now.AddSeconds(TokenTimeout);
                    dt.Rows.Add(dr);


                    //add to DB 
                    TokenVO tVO = new TokenVO();
                    tVO.Token = token;
                    tVO.CompanyId = 0;
                    tVO.DepartmentId = 0;
                    tVO.UserId = customerId;
                    tVO.IsUser = false;
                    tVO.Timeout = DateTime.Now.AddSeconds(TokenTimeout);

                    uBo.InsertToken(tVO);

                    //保存成功后才更新缓存表
                    HttpRuntime.Cache[cacheKey] = dt;

                    return token;
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString().Contains("内部索引已损坏"))
                    {
                        CacheInit(customerId.ToString(), true);
                    }
                    LogBO _log = new LogBO(typeof(CacheManager));
                    string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                    _log.Error(strErrorMsg);
                    return "";
                }

            }
            else
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
    }
}