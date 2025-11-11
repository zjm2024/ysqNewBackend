using Aop.Api.Domain;
using BusinessCard.Models;
using CoreFramework.VO;
using Google.Protobuf.WellKnownTypes;
using Jayrock.Json;
using Microsoft.JScript;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using NPOI.XWPF.UserModel;
using SPlatformService;
using SPlatformService.Controllers;
using SPlatformService.Models;
using SPlatformService.TokenMange;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.DAO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Services.Description;
using System.Web.UI.WebControls;


namespace BusinessCard.Controllers
{
    [RoutePrefix("SPWebAPI/Users")]
    [TokenProjector]
    public class UsersController : ApiController
    {

        /// <summary>
        /// 身份验证
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [Route("validAccount"), HttpGet, Anonymous]
        public ResultObject ValidAccount(string loginName, string password)
        {
            //加密密码
            password= Utilities.GetMD5(password);
            UserBO uBO = new UserBO(new UserProfile());
            UserVO uVO = uBO.FindUserByLoginInfo(loginName, password);

            UserLoginHistoryVO ulHistoryVO = new UserLoginHistoryVO();
            ulHistoryVO.LoginAt = DateTime.Now;
            ulHistoryVO.Status = true;
            ulHistoryVO.LoginOS = HttpContext.Current.Request.Browser.Platform;
            ulHistoryVO.LoginIP = HttpContext.Current.Request.UserHostAddress;
            ulHistoryVO.LoginBrowser = HttpContext.Current.Request.Browser.Browser;

            if (uVO != null)
            {
                UserViewVO uvVO = uBO.FindUserViewById(uVO.UserId);
                string token = CacheManager.TokenInsert(uvVO.CompanyId, uvVO.DepartmentId, uvVO.UserId);
                UserLoginModel ulm = new UserLoginModel();
                ulm.User = uvVO;
                ulm.Token = token;

                //记录登录信息               
                ulHistoryVO.UserId = uvVO.UserId;
                uBO.AddUserLoginHistory(ulHistoryVO);

                return new ResultObject() { Flag = 1, Message = "验证成功!", Result = ulm };
            }
            else
            {
                ulHistoryVO.Status = false;
                uBO.AddUserLoginHistory(ulHistoryVO);
                return new ResultObject() { Flag = 0, Message = "验证失败!", Result = null };
            }

        }


        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getUserAll"), HttpPost, Anonymous]
        public ResultObject GetUserAll([FromBody] dynamic queryParams,string token)
        {
            // 验证用户身份
            UserProfile uProfile = CacheManager.GetUserProfile(token);

            if (uProfile==null)
                return new ResultObject() { Flag = -1, Message = "token异常!", Result = null };

            string dataStr = JsonConvert.SerializeObject(queryParams);

            var paramsObj = new { PageInfo = new { PageIndex = 0, PageCount = 0, SearchText="", SortName = "CreatedAt", SortType = "asc" } };

            dynamic condition = JsonConvert.DeserializeAnonymousType(dataStr, paramsObj);

            try
            {
                if (condition == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }

                dynamic pageInfo = condition.PageInfo;
                UserBO uBO = new UserBO(uProfile);
                string conditionStr2 = "1=1";
                if (pageInfo.SearchText != "")
                    conditionStr2 += " and ((UserName like '%" + pageInfo.SearchText + "%') or (LoginName like '%" + pageInfo.SearchText + "%') or (Phone like '%" + pageInfo.SearchText + "%'))";

                List<UserViewVO> list = uBO.FindAllByPageIndex1(conditionStr2, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);



                var count = uBO.FindTotalCount1(conditionStr2);
                if (list.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = count };
                }
                return new ResultObject() { Flag = 0, Message = "未查询到数据!", Result = null };


       
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "接口异常!", Result = ex };
            }

        }


   
    }
}
