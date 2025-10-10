using CoreFramework.VO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SPLibrary.WebConfigInfo
{
    public class CacheSystemConfig
    {
        private static void CacheInit(bool isResetting = false)
        {
            if (HttpRuntime.Cache["PASSPORT.SYSTEMCONFIG"] == null || isResetting)
            {

                SystemBO sBO = new SystemBO(new UserProfile());
                ConfigVO cVO = sBO.FindConfig();
                
                //Cache的过期时间为 令牌过期时间*2
                HttpRuntime.Cache.Insert("PASSPORT.SYSTEMCONFIG", cVO, null, DateTime.MaxValue, TimeSpan.FromHours(1));
            }
        }

        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <returns></returns>
        public static ConfigVO GetSystemConfig(bool isResetting=false)
        {
            CacheInit(isResetting);

            ConfigVO cVO = (ConfigVO)HttpRuntime.Cache["PASSPORT.SYSTEMCONFIG"];
            
            return cVO;
        }

        /// <summary>
        /// 更新系统配置
        /// </summary>
        /// <param name="configVO"></param>
        /// <returns></returns>
        public static void UpdateSystemConfig(ConfigVO configVO)
        {
            CacheInit();

            HttpRuntime.Cache["PASSPORT.SYSTEMCONFIG"] = configVO;

        }
        
    }
}
