using CoreFramework.VO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Web;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.DAO;
using SPLibrary.UserManagement.VO;

namespace SPLibrary.TokenMange
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
        private static void CacheInit()
        {
            if (HttpRuntime.Cache["PASSPORT.TOKEN"] == null)
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
                UserBO uBO = new UserBO(new UserProfile());
                List<TokenVO> dtList = uBO.FindTokeAll();
                foreach (TokenVO tVO in dtList)
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
                   

                //Cache的过期时间为 令牌过期时间*2
                HttpRuntime.Cache.Insert("PASSPORT.TOKEN", dt, null, DateTime.MaxValue, TimeSpan.FromDays(7 * 2));
            }
        }

        /// <summary>
        /// 获取用户UUID标识
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static int GetUserId(string token)
        {
            CacheInit();

            DataTable dt = (DataTable)HttpRuntime.Cache["PASSPORT.TOKEN"];
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
            CacheInit();

            DataTable dt = (DataTable)HttpRuntime.Cache["PASSPORT.TOKEN"];
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
            CacheInit();

            DataTable dt = (DataTable)HttpRuntime.Cache["PASSPORT.TOKEN"];
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
            CacheInit();

            DataTable dt = (DataTable)HttpRuntime.Cache["PASSPORT.TOKEN"];
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
            CacheInit();

            DataTable dt = (DataTable)HttpRuntime.Cache["PASSPORT.TOKEN"];
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
                dt.Rows.Remove(dr[0]);
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
            CacheInit();

            DataTable dt = (DataTable)HttpRuntime.Cache["PASSPORT.TOKEN"];
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
            CacheInit();

            //// token不存在则添加
            //if (!TokenIsExist(token))
            //{
            DataTable dt = (DataTable)HttpRuntime.Cache["PASSPORT.TOKEN"];
            DataRow dr = dt.NewRow();
            string token = Guid.NewGuid().ToString();
            dr["Token"] = token;
            dr["CompanyId"] = companyId;
            dr["DepartmentId"] = departmentId;
            dr["UserId"] = userId;
            dr["IsUser"] = true;
            dr["Timeout"] = DateTime.Now.AddSeconds(TokenTimeout);
            dt.Rows.Add(dr);
            HttpRuntime.Cache["PASSPORT.TOKEN"] = dt;

            //add to DB 平台账号不需要保存到DB，过期，关闭都需要重新登录
            TokenVO tVO = new TokenVO();
            tVO.Token = token;
            tVO.CompanyId = companyId;
            tVO.DepartmentId = departmentId;
            tVO.UserId = userId;
            tVO.IsUser = true;
            tVO.Timeout = DateTime.Now.AddSeconds(TokenTimeout);
            UserBO uBo = new UserBO(new UserProfile());
            uBo.InsertToken(tVO);

            return token;
        }

        public static string TokenInsert(int customerId)
        {
            CacheInit();
            
            DataTable dt = (DataTable)HttpRuntime.Cache["PASSPORT.TOKEN"];
            DataRow dr = dt.NewRow();
            string token = Guid.NewGuid().ToString();
            dr["Token"] = token;
            dr["CompanyId"] = 0;
            dr["DepartmentId"] = 0;
            dr["UserId"] = customerId;
            dr["IsUser"] = false;
            dr["Timeout"] = DateTime.Now.AddSeconds(TokenTimeout);
            dt.Rows.Add(dr);
            HttpRuntime.Cache["PASSPORT.TOKEN"] = dt;

            //add to DB 
            TokenVO tVO = new TokenVO();
            tVO.Token = token;
            tVO.CompanyId = 0;
            tVO.DepartmentId = 0;
            tVO.UserId = customerId;
            tVO.IsUser = false;
            tVO.Timeout = DateTime.Now.AddSeconds(TokenTimeout);
            UserBO uBo = new UserBO(new UserProfile());
            uBo.InsertToken(tVO);

            return token;
        }

    }
}