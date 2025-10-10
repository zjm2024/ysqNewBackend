using BroadSky.WeChatAppDecrypt;
using CoreFramework.VO;
using SPlatformService;
using SPlatformService.Controllers;
using SPlatformService.Models;
using SPlatformService.TokenMange;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace BusinessCard.Controllers
{
    [RoutePrefix("SPWebAPI/LuckyDraw")]
    [TokenProjector]
    public class LuckyDrawController : ApiController
    {
        /// <summary>
        /// 获取我的名片
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyPersonal"), HttpGet]
        public ResultObject getMyPersonal(string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
            try
            {
                if (pVO != null)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = pVO };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }

        }

    }
}


