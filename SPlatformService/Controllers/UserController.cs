using SPlatformService.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using SPLibrary.UserManagement.VO;
using SPLibrary.UserManagement.BO;
using SPLibrary.CoreFramework.BO;
using SPlatformService.TokenMange;
using CoreFramework.VO;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using TencentCloud.Ckafka.V20190819.Models;
using TencentCloud.Tci.V20190318.Models;
using Jayrock.Json;
using System.Web.UI.WebControls;
using System.Web.ModelBinding;
using BroadSky.WeChatAppDecrypt;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Data; 
using Newtonsoft.Json;
using System.Drawing.Printing;
using static SPlatformService.Controllers.UserController;
using SPLibrary.CoreFramework.Logging.BO;
using Aop.Api.Domain;
using System.Threading.Tasks;
using System.Xml.Linq;
using SPLibrary.WebConfigInfo;
using System.Text.RegularExpressions;
using SPLibrary.BusinessCardManagement.BO;

namespace SPlatformService.Controllers
{
    /// <summary>
    /// 后台用户API
    /// </summary>
    [RoutePrefix("SPWebAPI/User")]
    [TokenProjector]
    public class UserController : ApiController
    {
        /// <summary>
        /// 身份验证
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [Route("ValidAccount"), HttpGet, Anonymous]
        public ResultObject ValidAccount(string loginName, string password)
        {
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
        /// 删除后台用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteUser"), HttpPost]
        public ResultObject DeleteUser(string userId, string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    string[] userIdArr = userId.Split(',');
                    bool isAllDelete = true;
                    UserBO uBO = new UserBO(CacheManager.GetUserProfile(token));
                    for (int i = 0; i < userIdArr.Length; i++)
                    {
                        try
                        {
                            uBO.Delete(Convert.ToInt32(userIdArr[i]));
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
        /// 获取后台用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetUser"), HttpGet]
        public ResultObject GetUser(int userId, string token)
        {
            UserBO uBO = new UserBO(CacheManager.GetUserProfile(token));
            UserVO uVO = uBO.FindById(userId);
            if (uVO != null)
            {
                UserModel umVO = new UserModel();
                umVO.User = uVO;
                umVO.UserRole = uBO.FindUserRoleByUserId(userId);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = umVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新用户
        /// </summary>
        /// <param name="userModelVO">用户VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateUser"), HttpPost]
        public ResultObject UpdateUser([FromBody] UserModel userModelVO, string token)
        {
            UserVO userVO = userModelVO.User;
            List<UserRoleVO> userRoleVOList = userModelVO.UserRole;
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            if (userVO != null)
            {
                UserBO uBO = new UserBO(uProfile);
                //判断LoginName 和 UserName是否重复
                if (uBO.IsUserExist(userVO))
                {
                    return new ResultObject() { Flag = 0, Message = "登录名称已存在!", Result = null };
                }
                if (uBO.IsUserNameExist(userVO))
                {
                    return new ResultObject() { Flag = 0, Message = "姓名已存在!", Result = null };
                }
                if (userVO.UserId < 1)
                {
                    userVO.CreatedAt = DateTime.Now;
                    userVO.CreatedBy = uProfile.UserId;
                    userVO.UpdatedAt = DateTime.Now;
                    userVO.UpdatedBy = uProfile.UserId;

                    userVO.Password = Utilities.GetMD5(userVO.Password);

                    int userId = uBO.Add(userVO, userRoleVOList);
                    if (userId > 0)
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = userId };
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
                else
                {
                    userVO.UpdatedAt = DateTime.Now;
                    userVO.UpdatedBy = uProfile.UserId;
                    //userVO.Password = null;
                    userVO.Restore("Password");
                    bool isSuccess = uBO.Update(userVO, userRoleVOList);
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
        /// 后台用户修改自己密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="password">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ChangePassword"), HttpPost]
        public ResultObject ChangePassword(int userId, string password, string newPassword, string token)
        {
            UserBO uBO = new UserBO(CacheManager.GetUserProfile(token));
            bool result = uBO.ChangePassword(userId, Utilities.GetMD5(password), Utilities.GetMD5(newPassword));
            if (result)
                return new ResultObject() { Flag = 1, Message = "修改成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "密码错误，请重新输入!", Result = null };
        }

        /// <summary>
        /// 管理员修改用户密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("ChangeUserPassword"), HttpPost]
        public ResultObject ChangeUserPassword(int userId, string newPassword, string token)
        {
            UserBO uBO = new UserBO(CacheManager.GetUserProfile(token));
            bool result = uBO.ChangeUserPassword(userId, Utilities.GetMD5(newPassword));
            if (result)
                return new ResultObject() { Flag = 1, Message = "修改成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "密码错误，请重新输入!", Result = null };
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetUserAll"), HttpGet]
        public ResultObject GetUserAll(string token)
        {
            UserBO uBO = new UserBO(CacheManager.GetUserProfile(token));
            List<UserVO> uVOList = uBO.FindAll("1=1");
            if (uVOList != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVOList };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取客服信息
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetContectService"), HttpGet]
        public ResultObject GetContectService(string token)
        {
            UserBO uBO = new UserBO(CacheManager.GetUserProfile(token));
            List<UserRoleViewVO> uVOList = uBO.FindUserRoleViewVOAll("RoleName=N'客服'");
            if (uVOList != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVOList };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 获取用户部门信息
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetUserByDepartment"), HttpGet]
        public ResultObject GetUserByDepartment(int departmentId, string token)
        {
            UserBO uBO = new UserBO(CacheManager.GetUserProfile(token));
            List<UserVO> uVOList = uBO.FindAll("DepartmentId=" + departmentId);
            if (uVOList != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uVOList };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新省
        /// </summary>
        /// <param name="cityModelVO">省VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateProvince"), HttpPost]
        public ResultObject UpdateProvince([FromBody] CityModel cityModelVO, string token)
        {
            ProvinceVO provinceVO = cityModelVO.Province;
            List<CityVO> cityVOList = cityModelVO.City;
            if (cityVOList == null)
                cityVOList = new List<CityVO>();
            if (provinceVO != null)
            {
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CityBO cBO = new CityBO(uProfile);
                //判断LoginName 和 UserName是否重复
                if (cBO.IsProvinceExist(provinceVO))
                {
                    return new ResultObject() { Flag = 0, Message = "省（直辖市）编号或名称已存在!", Result = null };
                }
                //判断城市编号和名称
                foreach (CityVO cityVO in cityVOList)
                {
                    if (cBO.IsCityExist(cityVO))
                    {
                        return new ResultObject() { Flag = 0, Message = "城市编号或名称已存在!", Result = null };
                    }
                }

                if (provinceVO.ProvinceId < 1)
                {
                    int userId = cBO.ProvinceAdd(provinceVO, cityVOList);
                    if (userId > 0)
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = userId };
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
                else
                {
                    bool isSuccess = cBO.ProvinceUpdate(provinceVO, cityVOList);
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
        /// 获取省
        /// </summary>
        /// <param name="provinceId">省ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetProvince"), HttpGet]
        public ResultObject GetProvince(int provinceId, string token)
        {
            CityBO uBO = new CityBO(CacheManager.GetUserProfile(token));
            ProvinceVO uVO = uBO.FindProvinceById(provinceId);
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
        /// 获取城市列表，匿名
        /// </summary>
        /// <param name="provinceId">省ID</param>
        /// <param name="enable">是否启用</param>
        /// <returns></returns>
        [Route("GetCityList"), HttpGet, Anonymous]
        public ResultObject GetCityList(int provinceId, bool enable)
        {
            CityBO uBO = new CityBO(new UserProfile());
            List<CityVO> uVO = new List<CityVO>();
            //if (provinceId == -1 || provinceId == -2)
            //    uVO = uBO.FindCityAll(enable);
            //else
            uVO = uBO.FindCityByProvince(provinceId, enable);
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
        /// 获取城市列表，匿名
        /// </summary>
        /// <param name="enable">是否启用</param>
        /// <returns></returns>
        [Route("GetCityListAll"), HttpGet, Anonymous]
        public ResultObject GetCityListAll(bool enable)
        {
            CityBO uBO = new CityBO(new UserProfile());
            List<CityVO> uVO = uBO.FindCityAll(enable);
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
        /// 获取城市
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        [Route("GetCity"), HttpGet, Anonymous]
        public ResultObject GetCity(int cityId)
        {
            CityBO uBO = new CityBO(new UserProfile());
            CityVO uVO = uBO.FindCityById(cityId);
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
        /// 获取省列表，匿名
        /// </summary>
        /// <param name="enable">是否启用</param>
        /// <returns></returns>
        [Route("GetProvinceList"), HttpGet, Anonymous]
        public ResultObject GetProvinceList(bool enable)
        {
            CityBO uBO = new CityBO(new UserProfile());
            List<ProvinceVO> uVO = uBO.FindProvinceList(enable);
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
        /// 添加或更新行业
        /// </summary>
        /// <param name="categoryModelVO">行业VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateCategory"), HttpPost]
        public ResultObject UpdateCategory([FromBody] CategoryModel categoryModelVO, string token)
        {
            CategoryVO categoryVO = categoryModelVO.Category;
            List<CategoryVO> childVOList = categoryModelVO.ChildCategory;
            if (childVOList == null)
                childVOList = new List<CategoryVO>();
            if (categoryVO != null)
            {
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CategoryBO cBO = new CategoryBO(uProfile);
                //判断LoginName 和 UserName是否重复
                if (cBO.IsCategoryExist(categoryVO))
                {
                    return new ResultObject() { Flag = 0, Message = "大类编号或名称已存在!", Result = null };
                }
                //判断城市编号和名称
                foreach (CategoryVO cVO in childVOList)
                {
                    if (cBO.IsChildCategoryExist(cVO))
                    {
                        return new ResultObject() { Flag = 0, Message = "小类编号或名称已存在!", Result = null };
                    }
                }

                if (categoryVO.CategoryId < 1)
                {
                    int userId = cBO.Add(categoryVO, childVOList);
                    if (userId > 0)
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = userId };
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
                else
                {
                    bool isSuccess = cBO.Update(categoryVO, childVOList);
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
        /// 获取行业
        /// </summary>
        /// <param name="categoryId">行业ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetCategory"), HttpGet, Anonymous]
        public ResultObject GetCategory(int categoryId)
        {
            CategoryBO uBO = new CategoryBO(new UserProfile());
            CategoryVO uVO = uBO.FindById(categoryId);
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
        /// 获取子行业，匿名
        /// </summary>
        /// <param name="parentCategoryId">父行业ID</param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [Route("GetChildCategoryList"), HttpGet, Anonymous]
        public ResultObject GetChildCategoryList(int parentCategoryId, bool enable)
        {
            CategoryBO uBO = new CategoryBO(new UserProfile());
            List<CategoryVO> uVO = new List<CategoryVO>();
            //if (parentCategoryId == -1 || parentCategoryId == -2)
            //    uVO = uBO.FindAllCategory(true);
            //else
            uVO = uBO.FindCategoryByParent(parentCategoryId, enable);
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
        /// 获取所有子行业，匿名
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        [Route("GetChildCategoryList"), HttpGet, Anonymous]
        public ResultObject GetChildCategoryList(bool enable)
        {
            CategoryBO uBO = new CategoryBO(new UserProfile());
            List<CategoryVO> uVO = new List<CategoryVO>();
            //if (parentCategoryId == -1 || parentCategoryId == -2)
            //    uVO = uBO.FindAllCategory(true);
            //else
            uVO = uBO.FindCategoryByParent(enable);
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
        /// 获取第一级行业列表
        /// </summary>
        /// <param name="enable">是否启用</param>
        /// <returns></returns>
        [Route("GetParentCategoryList"), HttpGet, Anonymous]
        public ResultObject GetParentCategoryList(bool enable)
        {
            CategoryBO uBO = new CategoryBO(new UserProfile());
            List<CategoryVO> uVO = uBO.FindParentCategoryList(enable);
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
        /// 经纪人推荐
        /// </summary>
        /// <param name="recommendAgencyModel">经纪人推荐列表</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRecommendAgency"), HttpPost]
        public ResultObject UpdateRecommendAgency([FromBody] RecommendAgencyModel recommendAgencyModel, string token)
        {
            if (recommendAgencyModel != null)
            {
                List<RecommendAgencyVO> recommendAgencyList = recommendAgencyModel.RecommendAgencyList;
                if (recommendAgencyList != null)
                {
                    UserProfile uProfile = CacheManager.GetUserProfile(token);
                    UserBO uBO = new UserBO(uProfile);
                    bool result = uBO.UpdateRemcommendAgency(recommendAgencyList);
                    if (result)
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                    else
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
        }

        /// <summary>
        /// 获取经纪人推荐
        /// </summary>
        /// <param name="provinceId">省</param>
        /// <param name="cityId">城市</param>
        /// <param name="parentCategoryId">大类</param>
        /// <param name="categoryId">小类</param>
        /// <returns></returns>
        [Route("GetRecommendAgencyList"), HttpGet, Anonymous]
        public ResultObject GetRecommendAgencyList(int provinceId, int cityId, int parentCategoryId, int categoryId)
        {
            UserBO uBO = new UserBO(new UserProfile());
            List<RecommendAgencyViewVO> voList = uBO.FindRecommendAgencyList(provinceId, cityId, parentCategoryId, categoryId);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = voList };
        }

        /// <summary>
        /// 任务推荐
        /// </summary>
        /// <param name="recommendRequireModel">任务推荐列表</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRecommendRequire"), HttpPost]
        public ResultObject UpdateRecommendRequire([FromBody] RecommendRequireModel recommendRequireModel, string token)
        {
            if (recommendRequireModel != null)
            {
                List<RecommendRequireVO> recommendRequireList = recommendRequireModel.RecommendRequireList;
                if (recommendRequireList != null)
                {
                    UserProfile uProfile = CacheManager.GetUserProfile(token);
                    UserBO uBO = new UserBO(uProfile);
                    bool result = uBO.UpdateRecommendRequire(recommendRequireList);
                    if (result)
                        return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                    else
                        return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
        }

        /// <summary>
        /// 获取任务推荐
        /// </summary>
        /// <param name="provinceId">省</param>
        /// <param name="cityId">城市</param>
        /// <param name="parentCategoryId">大类</param>
        /// <param name="categoryId">小类</param>
        /// <returns></returns>
        [Route("GetRecommendRequireList"), HttpGet, Anonymous]
        public ResultObject GetRecommendRequireList(int provinceId, int cityId, int parentCategoryId, int categoryId)
        {
            UserBO uBO = new UserBO(new UserProfile());
            List<RecommendRequireViewVO> voList = uBO.FindRecommendRequireList(provinceId, cityId, parentCategoryId, categoryId);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = voList };
        }

        /// <summary>
        /// 根据ID获取轮播信息
        /// </summary>
        /// <param name="CarouselID">ID</param>
        /// <returns></returns>
        [Route("GetCarousel"), HttpGet, Anonymous]
        public ResultObject GetCarousel(int CarouselID)
        {
            SystemBO uBO = new SystemBO(new CustomerProfile());
            CarouselVO uVO = uBO.FindCarouselById(CarouselID);
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
        /// 获取轮播信息列表
        /// </summary>
        /// <param name="Top">top</param>
        /// <returns></returns>
        [Route("GetCarouselList"), HttpGet, Anonymous]
        public ResultObject GetCarouselList(int Top)
        {
            SystemBO uBO = new SystemBO(new CustomerProfile());
            List<CarouselVO> voList = uBO.FindCarouselAllByPageIndex("1=1", 1, Top, "CreatedAt", "desc");
            if (voList != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = voList };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 删除轮播信息
        /// </summary>
        /// <param name="CarouselID">ID，多个ID用逗号分隔</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteCarousel"), HttpPost]
        public ResultObject DeleteCarousel(string CarouselID, string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(CarouselID))
                {
                    string[] customerIdArr = CarouselID.Split(',');
                    bool isAllDelete = true;
                    SystemBO uBO = new SystemBO(new CustomerProfile());
                    for (int i = 0; i < customerIdArr.Length; i++)
                    {
                        try
                        {
                            uBO.CarouselDelete(Convert.ToInt32(customerIdArr[i]));
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
        /// 添加或更新轮播消息
        /// </summary>
        /// <param name="carouselVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateCarousel"), HttpPost]
        public ResultObject UpdateCarousel([FromBody] CarouselVO carouselVO, string token)
        {
            CustomerProfile uProfile = new CustomerProfile();

            if (carouselVO != null)
            {
                SystemBO uBO = new SystemBO(new CustomerProfile());
                if (carouselVO.CarouselID < 1)
                {
                    carouselVO.CreatedAt = DateTime.Now;
                    int CarouselID = uBO.CarouselAdd(carouselVO);
                    if (CarouselID > 0)
                    {
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = CarouselID };
                    }
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
                else
                {
                    bool isSuccess = uBO.CarouselUpdate(carouselVO);
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
        /// 查询用户接口
        /// </summary>
        /// <param name="carouselVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("csUpdate"), HttpGet, Anonymous]
        public ResultObject csUpdate()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string JSONstring = "";
            var oint = 0;


            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                //查询
                List<string> message = new List<string>();
                string selectQuery = "SELECT * FROM t_dcxc_user";
                using (var command = new MySqlCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.HasRows)
                        {
                            JSONstring += "[";
                            while (reader.Read())
                            {
                                string strUserName = Convert.IsDBNull(reader["UserName"]) ? null : reader["UserName"].ToString();
                                JSONstring += "{";
                                JSONstring += "\"" + "UserId" + "\":\"" + reader.GetString("UserId") + "\",";
                                JSONstring += "\"" + "UserName" + "\":\"" + strUserName + "\",";
                                JSONstring += "\"" + "Account" + "\":\"" + reader.GetString("Account") + "\",";
                                JSONstring += "\"" + "Phone" + "\":\"" + reader.GetString("Phone") + "\",";
                                JSONstring += "\"" + "CreateDate" + "\":\"" + reader.GetString("CreateDate") + "\"";
                                JSONstring += "},";
                                //message.Add($"UserName:{reader["UserName"]},LoginName:{reader["LoginName"]}");
                                //Console.WriteLine($"{reader["UserName"]}, {reader["LoginName"]}");
                            }
                            JSONstring = JSONstring.Substring(0, JSONstring.Length - 1);
                            JSONstring += "]";

                            return new ResultObject() { Flag = 1, Message = "查询成功！", Result = JSONstring };
                        }
                        else
                        {

                            return new ResultObject() { Flag = 0, Message = "查询成功,但是没符合用户！", Result = JSONstring };

                        }

                    }
                    catch (MySqlException ex)
                    {

                        return new ResultObject() { Flag = -1, Message = "查询失败！", Result = ex.Message };
                    }

                }

            }


        }
        #region 查询用户接口

       
        /// <summary>
        /// 查询用户接口(单页查询)
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="phone">电话</param>
        /// <param name="pagesize">条数</param>
        /// <param name="pageindex">页数</param>
        /// <returns></returns>
        [Route("getUserPagingList"), HttpGet, Anonymous]
        public ResultObject getUserPagingList(string userName, string phone,int pagesize , int pageindex)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string JSONstring = "";


            using (var connection = new MySqlConnection(connectionString))
            {
                //查询
                string condition = "";

                //string selectQuery = "SELECT * FROM t_dcxc_user where 1=1";
                if (!string.IsNullOrEmpty(userName))
                {
                    condition += " AND UserName Like '%" + userName + "%'";
                }
                if (!string.IsNullOrEmpty(userName))
                {
                    condition += " AND Phone Like '%" + phone + "%'";
                }

                List<string> message = new List<string>();
                var selectQuery = GetSql("t_dcxc_user",pageindex,pagesize,"*",condition, "UserId");

                connection.Open();
                using (var command = new MySqlCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.HasRows)
                        {

                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            JSONstring = JsonConvert.SerializeObject(dataTable, Formatting.Indented);

                            return new ResultObject() { Flag = 1, Message = "查询成功！", Result = JSONstring };
                        }
                        else
                        {

                            return new ResultObject() { Flag = 0, Message = "查询成功,但是没符合用户！", Result = JSONstring };

                        }

                    }
                    catch (MySqlException ex)
                    {

                        return new ResultObject() { Flag = -1, Message = "查询失败！", Result = ex.Message };
                    }

                }

            }

        }


        /// <summary>
        /// 条件查询用户接口
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="phone">电话</param>
        /// <returns></returns>
        [Route("getUserList"), HttpGet, Anonymous]
        public ResultObject getUserList(string userName, string phone, int pagesize, int pageindex)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string JSONstring = "";


            using (var connection = new MySqlConnection(connectionString))
            {
                string selectQuery = "SELECT * FROM t_dcxc_user WHERE 1=1";
                if (!string.IsNullOrEmpty(userName))
                {
                    selectQuery += " AND UserName Like '%" + userName + "%'";
                }
                if (!string.IsNullOrEmpty(phone))
                {
                    selectQuery += " AND Phone Like '%" + phone + "%'";
                }
                selectQuery += " ORDER BY UserId DESC";
                List<string> message = new List<string>();
                connection.Open();
                using (var command = new MySqlCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.HasRows)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            DataTable dt = dataTable.Clone();
                            for (var i = 0; i < pagesize; i++)
                            {
                                dt.ImportRow(dataTable.Rows[i]);
                            }
                            JSONstring = JsonConvert.SerializeObject(dt, Formatting.Indented);

                            return new ResultObject() { Flag = 1, Message = "查询成功！", Result = JSONstring, Count = dataTable.Rows.Count };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "查询成功,但是没符合用户！", Result = JSONstring };
                        }

                    }
                    catch (MySqlException ex)
                    {
                        return new ResultObject() { Flag = -1, Message = "查询失败！", Result = ex.Message };
                    }
                }

            }

        }
        #endregion

        #region 查询作品
        /// <summary>
        /// 查询作品
        /// </summary>
        /// <param name="authorNmae">作者名</param>
        /// <param name="worksNmae">作品名</param>
        /// <returns></returns>
        [Route("getWorkList"), HttpGet, Anonymous]
        public ResultObject getWorkList(string authorNmae,string worksNmae, int pagesize, int pageindex)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string JSONstring = "";
            using (var connection = new MySqlConnection(connectionString))
            {
               
                //查询
                List<string> message = new List<string>();
                string selectQuery = "SELECT WorksID,WorksNmae,wk.UserId,UserName,WorksType,AuthorNmae,Region,TutorName,AddRess,Remarks,WorksUrl,wk.CreateDate" +
                    " FROM t_dcxc_user as us INNER JOIN t_dcxc_works as wk ON us.UserId = wk.UserId WHERE 1=1 ";
                if (!string.IsNullOrEmpty(authorNmae))
                {
                    selectQuery += " AND AuthorNmae Like '%" + authorNmae + "%'";
                }
                if (!string.IsNullOrEmpty(worksNmae))
                {
                    selectQuery += " AND WorksNmae Like '%" + worksNmae + "%'";
                }
                selectQuery += " ORDER BY WorksID DESC";
                connection.Open();
                using (var command = new MySqlCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.HasRows)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            DataTable dt = dataTable.Clone();
                            for (var i = 0; i < pagesize; i++)
                            {
                                dt.ImportRow(dataTable.Rows[i]);
                            }
                            JSONstring = JsonConvert.SerializeObject(dt, Formatting.Indented);

                            return new ResultObject() { Flag = 1, Message = "查询成功！", Result = JSONstring, Count = dataTable.Rows.Count };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "查询成功,但是没符合用户！", Result = JSONstring };
                        }

                    }
                    catch (MySqlException ex)
                    {

                        return new ResultObject() { Flag = -1, Message = "查询失败！", Result = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 查询作品
        /// </summary>
        /// <param name="authorNmae">作者名</param>
        /// <param name="worksNmae">作品名</param>
        /// <param name="pagesize">条数</param>
        /// <param name="pageindex">页数</param>
        /// <returns></returns>
        [Route("getWorkPagingList"), HttpGet, Anonymous]
        public ResultObject getWorkPagingList(string authorNmae, string worksNmae, int pagesize, int pageindex)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string JSONstring = "";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                //查询
                List<string> message = new List<string>();
                string selectQuery = "SELECT WorksID,WorksNmae,wk.UserId,UserName,WorksType,AuthorNmae,Region,TutorName,AddRess,Remarks,WorksUrl,wk.CreateDate" +
                  " FROM t_dcxc_user as us INNER JOIN t_dcxc_works as wk ON us.UserId = wk.UserId WHERE 1=1 ";
                if (!string.IsNullOrEmpty(authorNmae))
                {
                    selectQuery += " AND AuthorNmae Like '%" + authorNmae + "%'";
                }
                if (!string.IsNullOrEmpty(worksNmae))
                {
                    selectQuery += " AND WorksNmae Like '%" + worksNmae + "%'";
                }
                int page = ((pageindex - 1) * pagesize);
                selectQuery += " ORDER BY WorksID  DESC  LIMIT " + pagesize + " OFFSET " + page;
                using (var command = new MySqlCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.HasRows)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            JSONstring = JsonConvert.SerializeObject(dataTable, Formatting.Indented);

                            return new ResultObject() { Flag = 1, Message = "查询成功！", Result = JSONstring };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "查询成功,但是没符合用户！", Result = JSONstring };
                        }

                    }
                    catch (MySqlException ex)
                    {

                        return new ResultObject() { Flag = -1, Message = "查询失败！", Result = ex.Message };
                    }
                }
            }
        }
        #endregion

        #region 分页查询
        /// <summary>
        /// 根据条件分页数据
        /// </summary>
        /// <param name="table">需要查询的表名称</param>
        /// <param name="pageindex1">页下标</param>
        /// <param name="pagesize">每页显示条数</param>
        /// <param name="fieldnames">查询的字段</param>
        /// <param name="condition">约束条件</param>
        /// <param name="orderstr">排序条件（例如：Top Desc,TuiJian Desc,LastTime Desc）</param>
        /// <returns></returns>
        public static string GetSql(string table, int pageindex1, int pagesize, string fieldnames, string condition, string orderstr)
        {
            int page = ((pageindex1 - 1) * pagesize);
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT " + fieldnames + " FROM " + table + "  WHERE 1=1 " + condition +  " ORDER BY  " + orderstr + " DESC " + " LIMIT " + pagesize + " OFFSET " + page);
            return sql.ToString();
        }
        #endregion

        #region 导出作品信息
        /// <summary>
        /// 导出作品信息
        /// </summary>
        /// <param name="authorNmae">作者名</param>
        /// <param name="worksNmae">作品名</param>
        /// <returns></returns>
        [Route("WorkUpExprot"), HttpGet, Anonymous]
        public ResultObject WorkUpExprot(string authorNmae, string worksNmae)
        {
            try
            {
                CardBO cBO = new CardBO(new CustomerProfile(), 1);
                //获取作品
                var table = getWorkDataTable(authorNmae, worksNmae);
                DataTable dt = new DataTable();
                //表格列名
                dt.Columns.Add("作品名", typeof(String));
                dt.Columns.Add("作品类型", typeof(String));
                dt.Columns.Add("作者", typeof(String));
                dt.Columns.Add("作者单位", typeof(String));
                dt.Columns.Add("指导老师", typeof(String));
                dt.Columns.Add("调研地点", typeof(String));
                dt.Columns.Add("作品说明", typeof(String));
                dt.Columns.Add("作品地址", typeof(String));
                dt.Columns.Add("所在城市", typeof(String));
                dt.Columns.Add("所在区域", typeof(String));
                dt.Columns.Add("上传时间", typeof(DateTime));
                dt.Columns.Add("姓名", typeof(String));
                dt.Columns.Add("电话", typeof(String));
                dt.Columns.Add("上传单位", typeof(String));
                dt.Columns.Add("单位地址", typeof(String));
                dt.Columns.Add("上传人", typeof(String));
                dt.Columns.Add("上报类型", typeof(String));
                dt.Columns.Add("邮箱", typeof(String));
                dt.Columns.Add("邮政编码", typeof(String));
                dt.Columns.Add("获奖收件人", typeof(String));
                dt.Columns.Add("收件电话", typeof(String));
                dt.Columns.Add("收件地址", typeof(String));
                //补充数据
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow row = dt.NewRow();
             
                    row["作品名"] = table.Rows[i]["WorksNmae"];
                    row["作品类型"] = table.Rows[i]["WorksType"];
                    row["作者"] = table.Rows[i]["AuthorNmae"];
                    row["作者单位"] = table.Rows[i]["AuthorUnit"];
                    row["指导老师"] = table.Rows[i]["TutorName"];
                    row["调研地点"] = table.Rows[i]["AddRess"];
                    row["作品说明"] = table.Rows[i]["Remarks"];
                    row["作品地址"] = table.Rows[i]["WorksUrl"];
                    row["所在城市"] = table.Rows[i]["Region"];
                    row["所在区域"] = table.Rows[i]["District"];
                    row["上传时间"] = table.Rows[i]["CreateDate"];
                    row["姓名"] = table.Rows[i]["UserName"];
                    row["电话"] = table.Rows[i]["Phone"];
                    row["上传单位"] = table.Rows[i]["Unit"];
                    row["单位地址"] = table.Rows[i]["UnitAddRess"];
                    row["上传人"] = table.Rows[i]["UnitDirector"];
                    row["上报类型"] = table.Rows[i]["UserType"];
                    row["邮箱"] = table.Rows[i]["Email"];
                    row["邮政编码"] = table.Rows[i]["PostalCode"];
                    row["获奖收件人"] = table.Rows[i]["AwardRecipient"];
                    row["收件电话"] = table.Rows[i]["AwardPhone"];
                    row["收件地址"] = table.Rows[i]["DeliveryAddress"];
                    dt.Rows.Add(row);
                }
                string FileName = cBO.DataToExcel(dt, "WorkUpExcel/", "作品信息.xls");
      
                if (FileName != null)
                {
                    return new ResultObject() { Flag = 1, Message = "导出成功！", Result = FileName };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "导出失败!", Result = null };
                }
            }
            catch (MySqlException ex)
            {

                return new ResultObject() { Flag = -1, Message = "导出失败！", Result = ex.Message };
            }
        }

        /// <summary>
        /// 查询作品
        /// </summary>
        /// <param name="authorNmae">作者名</param>
        /// <param name="worksNmae">作品名</param>
        /// <returns></returns>
        public DataTable getWorkDataTable(string authorNmae, string worksNmae)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
           
            using (var connection = new MySqlConnection(connectionString))
            {

                //查询
                List<string> message = new List<string>();
                string selectQuery = "SELECT UserName, Account, Password, Phone, IsAdmin, Unit, UnitAddRess, UnitDirector, DirectorPhone,IsEnable, " +
                    " UserType, Email, PostalCode, IsInfo,  WorksNmae, WorksType, AuthorNmae, TutorName, AddRess, Remarks, WorksUrl, Region,District," +
                    "AwardRecipient,AwardPhone,DeliveryAddress,wk.AuthorUnit, wk.CreateDate" +
                    " FROM t_dcxc_user as us INNER JOIN t_dcxc_works as wk ON us.UserId = wk.UserId WHERE 1=1 ";
                if (!string.IsNullOrEmpty(authorNmae))
                {
                    selectQuery += " AND AuthorNmae Like '%" + authorNmae + "%'";
                }
                if (!string.IsNullOrEmpty(worksNmae))
                {
                    selectQuery += " AND WorksNmae Like '%" + worksNmae + "%'";
                }
                selectQuery += " ORDER BY WorksID DESC";
                connection.Open();
                using (var command = new MySqlCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.HasRows)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            return dataTable;
                        }
                        else
                        {
                            return new DataTable();
                        }

                    }
                    catch (MySqlException ex)
                    {

                        return new DataTable();
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 条件查询用户接口
        /// </summary>
        /// <param name="carouselVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("csQuery"), HttpGet, Anonymous]
        public ResultObject csQuery(string strLoginName, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string JSONstring = "";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                //查询
                List<string> message = new List<string>();
                string selectQuery = "SELECT * FROM t_dcxc_user WHERE Account = @value1 and Password = @value2";
                MySqlCommand command = new MySqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@value1", strLoginName);
                command.Parameters.AddWithValue("@value2", password);

                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string strUserName = Convert.IsDBNull(reader["UserName"]) ? "空" : reader["UserName"].ToString();
                                string strIsAdmin = Convert.IsDBNull(reader["IsAdmin"]) ? "" : reader["IsAdmin"].ToString();
                                JSONstring += "{";
                                JSONstring += "\"" + "UserId" + "\":\"" + reader.GetInt32("UserId") + "\",";
                                JSONstring += "\"" + "UserName" + "\":\"" + strUserName + "\",";
                                JSONstring += "\"" + "Account" + "\":\"" + reader.GetString("Account") + "\",";
                                JSONstring += "\"" + "IsInfo" + "\":\"" + reader.GetInt32("IsInfo") + "\",";
                                JSONstring += "\"" + "IsAdmin" + "\":\"" + strIsAdmin + "\",";
                                JSONstring += "\"" + "Phone" + "\":\"" + reader.GetString("Phone") + "\"";
                                JSONstring += "}";
                                //message.Add($"UserName:{reader["UserName"]},LoginName:{reader["LoginName"]}");
                                //Console.WriteLine($"{reader["UserName"]}, {reader["LoginName"]}");
                            }
                            return new ResultObject() { Flag = 1, Message = "查询成功！", Result = JSONstring };
                        }
                        else
                        {

                            return new ResultObject() { Flag = 0, Message = "查询成功,但是没符合用户！", Result = JSONstring };

                        }

                    }
                    catch (MySqlException ex)
                    {

                        return new ResultObject() { Flag = -1, Message = "查询失败！", Result = ex.Message };
                    }


                }



            }


        }

        /// <summary>
        /// 条件查询用户接口
        /// </summary>
        /// <param name="carouselVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("csNewQuery"), HttpGet, Anonymous]
        public ResultObject csNewQuery(string strLoginName, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string JSONstring = "";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                //查询
                List<string> message = new List<string>();
                //默认取第一条
                string selectQuery = "SELECT * FROM t_dcxc_user WHERE Account = @value1 and Password = @value2 LIMIT 0,1";
                MySqlCommand command = new MySqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@value1", strLoginName);
                command.Parameters.AddWithValue("@value2", password);

                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string strUserName = Convert.IsDBNull(reader["UserName"]) ? "空" : reader["UserName"].ToString();
                                string strIsAdmin = Convert.IsDBNull(reader["IsAdmin"]) ? "" : reader["IsAdmin"].ToString();
                                JSONstring += "{";
                                JSONstring += "\"" + "UserId" + "\":\"" + reader.GetInt32("UserId") + "\",";
                                JSONstring += "\"" + "UserName" + "\":\"" + strUserName + "\",";
                                JSONstring += "\"" + "Account" + "\":\"" + reader.GetString("Account") + "\",";
                                JSONstring += "\"" + "IsInfo" + "\":\"" + reader.GetInt32("IsInfo") + "\",";
                                JSONstring += "\"" + "IsAdmin" + "\":\"" + strIsAdmin + "\",";
                                JSONstring += "\"" + "Phone" + "\":\"" + reader.GetString("Phone") + "\"";
                                JSONstring += "}";
                                //message.Add($"UserName:{reader["UserName"]},LoginName:{reader["LoginName"]}");
                                //Console.WriteLine($"{reader["UserName"]}, {reader["LoginName"]}");
                            }
                            return new ResultObject() { Flag = 1, Message = "查询成功！", Result = JSONstring };
                        }
                        else
                        {

                            return new ResultObject() { Flag = 0, Message = "查询成功,但是没符合用户！", Result = JSONstring };

                        }

                    }
                    catch (MySqlException ex)
                    {

                        return new ResultObject() { Flag = -1, Message = "查询失败！", Result = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 条件查询用户接口
        /// </summary>
        /// <param name="carouselVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("LoginByphone"), HttpGet, Anonymous]
        public ResultObject LoginByphone(string phone)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string JSONstring = "";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                //查询  
                List<string> message = new List<string>();
                //默认取第一条
                string selectQuery = "SELECT * FROM t_dcxc_user WHERE Phone = @value1 LIMIT 0,1";
                MySqlCommand command = new MySqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@value1", phone);

                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string strUserName = Convert.IsDBNull(reader["UserName"]) ? "空" : reader["UserName"].ToString();
                                string strIsAdmin = Convert.IsDBNull(reader["IsAdmin"]) ? "" : reader["IsAdmin"].ToString();
                                JSONstring += "{";
                                JSONstring += "\"" + "UserId" + "\":\"" + reader.GetInt32("UserId") + "\",";
                                JSONstring += "\"" + "UserName" + "\":\"" + strUserName + "\",";
                                JSONstring += "\"" + "Account" + "\":\"" + reader.GetString("Account") + "\",";
                                JSONstring += "\"" + "IsInfo" + "\":\"" + reader.GetInt32("IsInfo") + "\",";
                                JSONstring += "\"" + "IsAdmin" + "\":\"" + strIsAdmin + "\",";
                                JSONstring += "\"" + "Phone" + "\":\"" + reader.GetString("Phone") + "\"";
                                JSONstring += "}";
                                //message.Add($"UserName:{reader["UserName"]},LoginName:{reader["LoginName"]}");
                                //Console.WriteLine($"{reader["UserName"]}, {reader["LoginName"]}");
                            }
                            return new ResultObject() { Flag = 1, Message = "查询成功！", Result = JSONstring };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "查询失败，没有符合用户！", Result = JSONstring };
                        }

                    }
                    catch (MySqlException ex)
                    {

                        return new ResultObject() { Flag = -1, Message = "查询失败！", Result = ex.Message };
                    }
                }
            }
        }


        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="carouselVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("usAdd"), HttpGet, Anonymous]
        public ResultObject usAdd(string phone, string loginname, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string usname = "lhz";
            var oint = 0;

            try
            {
                var IsExist = isExistUser(loginname, password);
                if (!IsExist)
                {
                    using (var connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        //增加
                        string insertQuery = "INSERT INTO t_dcxc_user (Account,Password,Phone,IsInfo,CreateDate) VALUES ( @column2,@column3,@column4,@colIsInfo,@createdAt)";
                        using (var command = new MySqlCommand(insertQuery, connection))
                        {
                            //command.Parameters.AddWithValue("@column1", username);
                            command.Parameters.AddWithValue("@column2", loginname);
                            command.Parameters.AddWithValue("@column3", password);
                            command.Parameters.AddWithValue("@colIsInfo", "0");
                            command.Parameters.AddWithValue("@column4", phone);
                            command.Parameters.AddWithValue("@createdAt", DateTime.Now); // 使用DateTime对象

                            oint = command.ExecuteNonQuery();
                        }
                    }
                    return new ResultObject() { Flag = oint, Message = "注册成功！", Result = null };
                }
                else {
                    return new ResultObject() { Flag = oint, Message = "注册失败！", Result = "账号已注册！" };
                }
            }
            catch (MySqlException ex)
            {
                return new ResultObject() { Flag = oint, Message = "注册失败！", Result = ex.Message };
            }


        }

        /// <summary>
        /// 条件查询用户接口
        /// </summary>
        /// <param name="carouselVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("isExistUser"), HttpGet, Anonymous]
        public bool isExistUser(string strLoginName, string password)
        {
            bool Flag = false;
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                //查询
                List<string> message = new List<string>();
                //默认取第一条
                string selectQuery = "SELECT UserId FROM t_dcxc_user WHERE Account = @value1  LIMIT 0,1";
                MySqlCommand command = new MySqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@value1", strLoginName);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        Flag = true;
                    else
                        Flag = false;
                }
            }
            return Flag;
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Email">邮箱</param>
        /// <param name="UserType">用户类型</param>
        /// <param name="PostalCode">邮编</param>
        /// <param name="Unit">单位</param>
        /// <param name="UnitaddRess">用户地址</param>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        [Route("usupdata"), HttpGet, Anonymous]
        public ResultObject usupdata(string UserName,string UserType,string Email,string PostalCode,string Unit,string UnitaddRess,int UserId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string usname = "lhz";
            var oint = 0;


            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 修改
                string query = "UPDATE t_dcxc_user SET UserName = @valUserName,UserType=@valUserType,Email=@valEmail,PostalCode=@valPostalCode,Unit=@valUnit,UnitaddRess=@valUnitaddRess,IsInfo=@valIsInfo WHERE UserId = @valUserId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@valUserName", UserName);
                    command.Parameters.AddWithValue("@valUserType", UserType);
                    command.Parameters.AddWithValue("@valEmail", Email);
                    command.Parameters.AddWithValue("@valPostalCode", PostalCode);
                    command.Parameters.AddWithValue("@valUnit", Unit);
                    command.Parameters.AddWithValue("@valUnitaddRess", UnitaddRess); 
                    command.Parameters.AddWithValue("@valIsInfo", "1");
                    command.Parameters.AddWithValue("@valUserId", UserId);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        return new ResultObject() { Flag = rowsAffected, Message = "修改成功！！", Result = null };
                    }
                    catch (MySqlException ex)
                    {
                        return new ResultObject() { Flag = -1, Message = "修改失败！！", Result = ex.Message };
                    }
                }

                
            }


        }

        /// <summary>
        /// 修改用户(团体/单位)
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="UserType">用户类型</param>
        /// <returns></returns>
        [Route("usupdatateam"), HttpGet, Anonymous]
        public ResultObject usupdatateam(string UserName, string UserType, string Email, string PostalCode, string Unit, string UnitaddRess,string UnitDirector,string DirectorPhone, int UserId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string usname = "lhz";
            var oint = 0;


            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 修改
                string query = "UPDATE t_dcxc_user SET UserName = @valUserName,UserType=@valUserType,Email=@valEmail,PostalCode=@valPostalCode,Unit=@valUnit,UnitaddRess=@valUnitaddRess,UnitDirector=@valUnitDirector,DirectorPhone=@valDirectorPhone,IsInfo=@valIsInfo WHERE UserId = @valUserId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@valUserName", UserName);
                    command.Parameters.AddWithValue("@valUserType", UserType);
                    command.Parameters.AddWithValue("@valEmail", Email);
                    command.Parameters.AddWithValue("@valPostalCode", PostalCode);
                    command.Parameters.AddWithValue("@valUnit", Unit);
                    command.Parameters.AddWithValue("@valUnitaddRess", UnitaddRess);
                    command.Parameters.AddWithValue("@valUnitDirector", UnitDirector);
                    command.Parameters.AddWithValue("@valDirectorPhone", DirectorPhone);
                    command.Parameters.AddWithValue("@valIsInfo", "1");
                    command.Parameters.AddWithValue("@valUserId", UserId);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        return new ResultObject() { Flag = rowsAffected, Message = "修改成功！！", Result = null };
                    }
                    catch (MySqlException ex)
                    {
                        return new ResultObject() { Flag = -1, Message = "修改失败！！", Result = ex.Message };
                    }
                }


            }


        }

        /// <summary>
        /// 修改用户（管理员）
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        [Route("usisadmin"), HttpGet, Anonymous]
        public ResultObject usisadmin(int UserId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;


            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 修改
                string query = "UPDATE t_dcxc_user SET IsAdmin=@valIsAdmin WHERE UserId = @valUserId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@valIsAdmin", "1");
                    command.Parameters.AddWithValue("@valUserId", UserId);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        return new ResultObject() { Flag = rowsAffected, Message = "修改成功！！", Result = null };
                    }
                    catch (MySqlException ex)
                    {
                        return new ResultObject() { Flag = -1, Message = "修改失败！！", Result = ex.Message };
                    }
                }


            }


        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="carouselVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("usdel"), HttpPost, Anonymous]
        public ResultObject usdel()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string usname = "lhz";
            var oint = 0;


            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();


                // 删除
                string deleteQuery = "DELETE FROM t_csc_user WHERE UserCode = '3333'";
                using (var command = new MySqlCommand(deleteQuery, connection))
                {
                    oint = command.ExecuteNonQuery();
                }
                return new ResultObject() { Flag = oint, Message = "删除成功！", Result = null };
            }


        }

        /// <summary>
        /// 查询作品
        /// </summary>
        /// <param name="">VO</param>
        /// <param name="">口令</param>
        /// <returns></returns>
        [Route("selworks"), HttpGet, Anonymous]
        public ResultObject selworks()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string JSONstring = "";
            var oint = 0;


            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                //查询
                List<string> message = new List<string>();
                string selectQuery = "SELECT * FROM t_dcxc_user as us INNER JOIN t_dcxc_works as wk ON us.UserId = wk.UserId";
                using (var command = new MySqlCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.HasRows)
                        {
                            JSONstring += "[";
                            while (reader.Read())
                            {
                                string strTutorName = Convert.IsDBNull(reader["TutorName"]) ? null : reader["TutorName"].ToString();
                                string strAddRess = Convert.IsDBNull(reader["AddRess"]) ? null : reader["AddRess"].ToString();
                                string strUserName = Convert.IsDBNull(reader["UserName"]) ? "空" : reader["UserName"].ToString();
                                string strUserType = Convert.IsDBNull(reader["UserType"]) ? "空" : reader["UserType"].ToString();
                                string strRegion = Convert.IsDBNull(reader["Region"]) ? "空" : reader["Region"].ToString();
                                JSONstring += "{";
                                JSONstring += "\"" + "UserName" + "\":\"" + strUserName + "\",";
                                JSONstring += "\"" + "WorksID" + "\":\"" + reader.GetInt32("WorksID") + "\",";
                                JSONstring += "\"" + "UserId" + "\":\"" + reader.GetInt32("UserId") + "\",";
                                JSONstring += "\"" + "WorksNmae" + "\":\"" + reader.GetString("WorksNmae") + "\",";
                                JSONstring += "\"" + "WorksType" + "\":\"" + reader.GetString("WorksType") + "\",";
                                JSONstring += "\"" + "UserType" + "\":\"" + strUserType + "\",";
                                JSONstring += "\"" + "AuthorNmae" + "\":\"" + reader.GetString("AuthorNmae") + "\",";
                                JSONstring += "\"" + "Region" + "\":\"" + strRegion + "\",";
                                JSONstring += "\"" + "TutorName" + "\":\"" + strTutorName + "\",";
                                JSONstring += "\"" + "AddRess" + "\":\"" + strAddRess + "\",";
                                JSONstring += "\"" + "Remarks" + "\":\"" + reader.GetString("Remarks") + "\",";
                                JSONstring += "\"" + "WorksUrl" + "\":\"" + reader.GetString("WorksUrl") + "\",";
                                JSONstring += "\"" + "CreateDate" + "\":\"" + reader.GetString("CreateDate") + "\"";
                                JSONstring += "},";
                                //message.Add($"UserName:{reader["UserName"]},LoginName:{reader["LoginName"]}");
                                //Console.WriteLine($"{reader["UserName"]}, {reader["LoginName"]}");
                            }
                            JSONstring = JSONstring.Substring(0, JSONstring.Length - 1);
                            JSONstring += "]";

                            return new ResultObject() { Flag = 1, Message = "查询成功！", Result = JSONstring };
                        }
                        else
                        {

                            return new ResultObject() { Flag = 0, Message = "查询成功,但是没符合用户！", Result = JSONstring };

                        }

                    }
                    catch (MySqlException ex)
                    {

                        return new ResultObject() { Flag = -1, Message = "查询失败！", Result = ex.Message };
                    }

                }

            }


        }

        /// <summary>
        /// 查询作品上传记录
        /// </summary>
        /// <param name="">VO</param>
        /// <param name="">口令</param>
        /// <returns></returns>
        [Route("selsiguoinfo"), HttpGet, Anonymous]
        public ResultObject selsiguoinfo(int UserId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string JSONstring = "";
            var oint = 0;


            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                //查询
                List<string> message = new List<string>();
                string selectQuery = "SELECT * FROM t_dcxc_works as wk INNER JOIN t_dcxc_user as us ON us.UserId = wk.UserId WHERE us.UserId = @value1";
                MySqlCommand command = new MySqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@value1", UserId);
                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.HasRows)
                        {
                            JSONstring += "[";
                            while (reader.Read())
                            {
                                string strTutorName = Convert.IsDBNull(reader["TutorName"]) ? null : reader["TutorName"].ToString();
                                string strAddRess = Convert.IsDBNull(reader["AddRess"]) ? null : reader["AddRess"].ToString();
                                string strUserName = Convert.IsDBNull(reader["UserName"]) ? "空" : reader["UserName"].ToString();
                                string strUserType = Convert.IsDBNull(reader["UserType"]) ? "空" : reader["UserType"].ToString();

                                JSONstring += "{";
                                JSONstring += "\"" + "UserName" + "\":\"" + strUserName + "\",";
                                JSONstring += "\"" + "WorksNmae" + "\":\"" + reader.GetString("WorksNmae") + "\",";
                                JSONstring += "\"" + "WorksType" + "\":\"" + reader.GetString("WorksType") + "\",";
                                JSONstring += "\"" + "CreateDate" + "\":\"" + reader.GetString("CreateDate") + "\"";
                                JSONstring += "},";
                                //message.Add($"UserName:{reader["UserName"]},LoginName:{reader["LoginName"]}");
                                //Console.WriteLine($"{reader["UserName"]}, {reader["LoginName"]}");
                            }
                            JSONstring = JSONstring.Substring(0, JSONstring.Length - 1);
                            JSONstring += "]";

                            return new ResultObject() { Flag = 1, Message = "查询成功！", Result = JSONstring };
                        }
                        else
                        {

                            return new ResultObject() { Flag = 0, Message = "查询成功,但是没符合作品！", Result = JSONstring };

                        }

                    }
                    catch (MySqlException ex)
                    {
                        return new ResultObject() { Flag = -1, Message = "查询失败！", Result = ex.Message };
                    }

                }

            }
        }

        /// <summary>
        /// 查询作品上传记录()
        /// </summary>
        /// <param name="">VO</param>
        /// <param name="">口令</param>
        /// <returns></returns>
        [Route("newselsiguoinfo"), HttpGet, Anonymous]
        public ResultObject newselsiguoinfo(int UserId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string JSONstring = "";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                //查询
                List<string> message = new List<string>();
                string selectQuery = "SELECT * FROM t_dcxc_works  WHERE UserId = @value1";
                MySqlCommand command = new MySqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@value1", UserId);
                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.HasRows)
                        {
                            List<WorkInfo> list = new List<WorkInfo>();
                            while (reader.Read())
                            {
                                WorkInfo worinfo = new WorkInfo();
                                
                                string strAuthorNmae = Convert.IsDBNull(reader["AuthorNmae"]) ? null : reader["AuthorNmae"].ToString();
                                worinfo.WorksId = Convert.ToInt32(reader["WorksId"]);
                                worinfo.WorksNmae = Convert.IsDBNull(reader["WorksNmae"]) ? null : reader["WorksNmae"].ToString();
                                worinfo.WorksType = Convert.IsDBNull(reader["WorksType"]) ? null : reader["WorksType"].ToString();
                                worinfo.WorksUrl = Convert.IsDBNull(reader["WorksUrl"]) ? null : reader["WorksUrl"].ToString();
                                worinfo.AuthorNmae = Convert.IsDBNull(reader["AuthorNmae"]) ? null : reader["AuthorNmae"].ToString();
                                worinfo.Region = Convert.IsDBNull(reader["Region"]) ? null : reader["Region"].ToString();
                                worinfo.Remarks = Convert.IsDBNull(reader["Remarks"]) ? null : reader["Remarks"].ToString();
                                worinfo.AddRess = Convert.IsDBNull(reader["AddRess"]) ? null : reader["AddRess"].ToString();
                                worinfo.TutorName = Convert.IsDBNull(reader["TutorName"]) ? null : reader["TutorName"].ToString();
                                worinfo.ModifyDate = Convert.IsDBNull(reader["ModifyDate"]) ? null : Convert.ToDateTime(reader["ModifyDate"]).ToString("yyyy-MM-dd"); 
                                worinfo.CreateDate = Convert.IsDBNull(reader["CreateDate"]) ? null : Convert.ToDateTime(reader["CreateDate"]).ToString("yyyy-MM-dd");
                                worinfo.IsAllowEdit = Convert.IsDBNull(reader["ModifyDate"]) ? "1" : "0";

                                list.Add(worinfo);
                            }
                            //JSONstring = JSONstring.Substring(0, JSONstring.Length - 1);
                            //JSONstring += "]";
                           

                            return new ResultObject() { Flag = 1, Message = "查询成功！", Result = list };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "查询成功,但是没符合作品！", Result = JSONstring };
                        }
                    }
                    catch (MySqlException ex)
                    {
                        return new ResultObject() { Flag = -1, Message = "查询失败！", Result = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 添加作品
        /// </summary>
        /// <param name="carouselVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("Addworks"), HttpGet, Anonymous]
        public ResultObject Addworks(int UserId,string WorksNmae, string WorksType, string AuthorNmae, string TutorName, string AddRess, string Remarks, string WorksUrl, string Region)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string usname = "lhz";
            var oint = 0;

            try
            {

                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    //增加
                    string insertQuery = "INSERT INTO t_dcxc_works (UserId,WorksNmae,WorksType,AuthorNmae,TutorName,AddRess,Remarks,WorksUrl,Region,CreateDate) VALUES (@colUserId,@colWorksNmae,@colWorksType,@colAuthorNmae,@colTutorName,@colAddRess,@colRemarks,@colWorksUrl,@colRegion,@createdAt)";
                    using (var command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@colUserId", UserId);
                        command.Parameters.AddWithValue("@colWorksNmae", WorksNmae);
                        command.Parameters.AddWithValue("@colWorksType", WorksType);
                        command.Parameters.AddWithValue("@colAuthorNmae", AuthorNmae);
                        command.Parameters.AddWithValue("@colTutorName", TutorName);
                        command.Parameters.AddWithValue("@colAddRess", AddRess);
                        command.Parameters.AddWithValue("@colRemarks", Remarks);
                        command.Parameters.AddWithValue("@colWorksUrl", WorksUrl);
                        command.Parameters.AddWithValue("@colRegion", Region);
                        command.Parameters.AddWithValue("@createdAt", DateTime.Now); // 使用DateTime对象

                        oint = command.ExecuteNonQuery();
                    }
                }
                return new ResultObject() { Flag = oint, Message = "上报成功！", Result = null };
            }
            catch (MySqlException ex)
            {
                return new ResultObject() { Flag = -1, Message = "上报失败！", Result = ex.Message };
            }


        }

        /// <summary>
        /// 添加作品(post请求)
        /// </summary>
        /// <param name="workinfo">work实体</param>
        /// <returns></returns>
        //[EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
        [Route("NewAddworks"), HttpPost, Anonymous]
        public ResultObject NewAddworks([FromBody] WorkInfo workinfo)
        {
            
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            //string usname = "lhz";
            var oint = 0;

            try
            {

                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    //增加
                    string insertQuery = "INSERT INTO t_dcxc_works (UserId,WorksNmae,WorksType,AuthorNmae,TutorName,AddRess,Remarks,WorksUrl,Region,DeliveryAddress,AwardRecipient,AwardPhone,District,CreateDate)" +
                        " VALUES (@colUserId,@colWorksNmae,@colWorksType,@colAuthorNmae,@colTutorName,@colAddRess,@colRemarks,@colWorksUrl,@colRegion,@DeliveryAddress,@AwardRecipient,@AwardPhone,@District,@createdAt)";
                    using (var command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@colUserId", workinfo.UserId);
                        command.Parameters.AddWithValue("@colWorksNmae", workinfo.WorksNmae);
                        command.Parameters.AddWithValue("@colWorksType", workinfo.WorksType);
                        command.Parameters.AddWithValue("@colAuthorNmae", workinfo.AuthorNmae);
                        command.Parameters.AddWithValue("@colTutorName", workinfo.TutorName);
                        command.Parameters.AddWithValue("@colAddRess", workinfo.AddRess);
                        command.Parameters.AddWithValue("@colRemarks", workinfo.Remarks);
                        command.Parameters.AddWithValue("@colWorksUrl", workinfo.WorksUrl);
                        command.Parameters.AddWithValue("@colRegion", workinfo.Region);
                        command.Parameters.AddWithValue("@DeliveryAddress", workinfo.DeliveryAddress);
                        command.Parameters.AddWithValue("@AwardRecipient", workinfo.AwardRecipient);
                        command.Parameters.AddWithValue("@AwardPhone", workinfo.AwardPhone);
                        command.Parameters.AddWithValue("@District", workinfo.District);
                        command.Parameters.AddWithValue("@createdAt", DateTime.Now); // 使用DateTime对象
                        
                        oint = command.ExecuteNonQuery();
                    }
                }
                return new ResultObject() { Flag = oint, Message = "上报成功！", Result = null };
            }
            catch (MySqlException ex)
            {
                return new ResultObject() { Flag = -1, Message = "上报失败！", Result = ex.Message };
            }
        }

            /// <summary>
            /// 修改作品
            /// </summary>
            /// <param name="carouselVO">VO</param>
            /// <param name="token">口令</param>
            /// <returns></returns>
            [Route("Worksupdata"), HttpGet, Anonymous]
        public ResultObject Worksupdata(int wkid,string workName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            string usname = "lhz";
            var oint = 0;


            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 修改
                string updateQuery = "UPDATE t_dcxc_works SET WorksNmae=@param1 WHERE WorksID=@param3";

                using (var command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@param1", workName);
                    //cmd.Parameters.AddWithValue("@param2", value2);
                    command.Parameters.AddWithValue("@param3", wkid);
                    oint = command.ExecuteNonQuery();
                }

                return new ResultObject() { Flag = oint, Message = "修改成功！！", Result = null };
            }


        }

        /// <summary>
        /// 修改作品
        /// </summary>
        /// <param name="carouselVO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("updateWorks"), HttpGet, Anonymous]
        public ResultObject updateWorks(int worksId,string WorksNmae, string WorksType, string AuthorNmae, string TutorName, string AddRess, string Remarks, string WorksUrl, string Region)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;
            var oint = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 修改
                string updateQuery = "UPDATE t_dcxc_works SET WorksNmae=@colWorksNmae, WorksType=@colWorksType, AuthorNmae=@colAuthorNmae, TutorName=@colTutorName, AddRess=@colAddRess, Remarks=@colRemarks, WorksUrl=@colWorksUrl, ModifyDate=@ModifyDate, Region=@colRegion WHERE WorksID=@worksId;";

                using (var command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@worksId", worksId);
                    command.Parameters.AddWithValue("@colWorksNmae", WorksNmae);
                    command.Parameters.AddWithValue("@colWorksType", WorksType);
                    command.Parameters.AddWithValue("@colAuthorNmae",AuthorNmae);
                    command.Parameters.AddWithValue("@colTutorName", TutorName);
                    command.Parameters.AddWithValue("@colAddRess", AddRess);
                    command.Parameters.AddWithValue("@colRemarks", Remarks);
                    command.Parameters.AddWithValue("@colWorksUrl",WorksUrl);
                    command.Parameters.AddWithValue("@colRegion", Region);
                    command.Parameters.AddWithValue("@ModifyDate", DateTime.Now);
                    oint = command.ExecuteNonQuery();
                }

                return new ResultObject() { Flag = oint, Message = "修改成功！！", Result = null };
            }

        }

        //public async Task<CombinedResult> GetCombinedDataAsync(int userId, int orderId)
        //{
        //    var CardPartyVO = new CardPartyVO();

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();

        //        // 第一次查询：用户信息
        //        CardPartyVO = await connection.QueryFirstOrDefaultAsync<CardPartyVO>(
        //            "SELECT * FROM Users WHERE Id = @UserId",
        //            new { UserId = userId });

        //        // 第二次查询：订单信息
        //        result.Orders = await connection.QueryAsync<Order>(
        //            "SELECT * FROM Orders WHERE UserId = @UserId",
        //            new { UserId = userId });

        //        // 第三次查询：订单详情（存储过程）
        //        result.OrderDetails = await connection.QueryAsync<OrderDetail>(
        //            "sp_GetOrderDetails",
        //            new { OrderId = orderId },
        //            commandType: CommandType.StoredProcedure);
        //    }

        //    return result;
        //}

        /// <summary>
        /// 下载活动所有报名的Excel文件(多彩乡村网页端)
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        [Route("getSignUplistToExcel4"), HttpGet]
        public ResultObject getSignUplistToExcel4(int PartyID = 13900)
        {
            if (PartyID > 0)
            {
                CardBO cBO = new CardBO(new CustomerProfile(), 1);
                CardPartyVO CardPartyVO = cBO.FindPartyById(PartyID);

                if (CardPartyVO == null) { return new ResultObject() { Flag = 0, Message = "参数错误!", Result = null }; }

                List<CardPartySignUpViewVO> cVO = cBO.FindSignUpViewByTrue(PartyID);
                List<CardPartyCostVO> CardPartyCost = cBO.FindCostByPartyID(PartyID);

                if (cVO != null)
                {
                    if (cVO.Count > 0)
                    {
                        DataTable dt = new DataTable();

                        dt.Columns.Add("序号", typeof(Int32));
                        dt.Columns.Add("姓名", typeof(String));
                        dt.Columns.Add("手机", typeof(String));
                        dt.Columns.Add("附件地址", typeof(String));
                        dt.Columns.Add("报名时间", typeof(DateTime));

                        if (CardPartyCost.Count > 0)
                        {
                            dt.Columns.Add("报名项目", typeof(String));
                            dt.Columns.Add("报名金额", typeof(Decimal));
                        }

                        //获取所有填写信息
                        List<CardPartySignUpFormVO> SignUpFormVO = cBO.FindSignUpFormByPartyID(PartyID, 1);
                        for (int i = 0; i < SignUpFormVO.Count; i++)
                        {
                            if (SignUpFormVO[i].Name != "姓名" && SignUpFormVO[i].Name != "手机")
                                dt.Columns.Add(SignUpFormVO[i].Name, typeof(String));
                        }

                        for (int i = 0; i < cVO.Count; i++)
                        {

                            DataRow row = dt.NewRow();
                            row["序号"] = i + 1;
                            row["姓名"] = cVO[i].Name;
                            row["手机"] = cVO[i].Phone;
                            row["报名时间"] = cVO[i].CreatedAt;
                            if (cVO[i].FjUrl == "")
                            {
                                row["附件地址"] = "未上传";
                            }
                            else
                            {
                                row["附件地址"] = cVO[i].FjUrl;
                            }

                            if (CardPartyCost.Count > 0)
                            {
                                row["报名项目"] = cVO[i].CostName;
                                row["报名金额"] = cVO[i].Cost;
                            }

                            try
                            {
                                List<CardPartySignUpFormVO> svO = cBO.FindSignUpFormByFormStr(cVO[i].SignUpForm);
                                for (int j = 0; j < svO.Count; j++)
                                {
                                    if (svO[j].Name != "姓名" && svO[j].Name != "手机")
                                    {
                                        if (!dt.Columns.Contains(svO[j].Name))
                                        {
                                            dt.Columns.Add(svO[j].Name, typeof(String));
                                        }
                                        row[svO[j].Name] = svO[j].value;
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                LogBO _log = new LogBO(typeof(CardBO));
                                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                                _log.Error(strErrorMsg);
                            }

                            dt.Rows.Add(row);//这样就可以添加了 
                        }

                        string FileName = cBO.DataToExcel(dt, "PartySignUpExcel/", PartyID + ".xls");

                        if (FileName != null)
                        {
                            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = FileName };
                        }
                        else
                        {
                            return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                        }
                    }
                    else
                    {
                        return new ResultObject() { Flag = 0, Message = "暂无报名!", Result = null };
                    }
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }

        }

        /// <summary>
        /// 123
        /// </summary>
        /// <param name="code">第一次拉手返回Code</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("Test1"), HttpPost, Anonymous]
        public ResultObject Test1()
        {
            try
            {
                string folder = "/UploadFolder/FaceID/" + DateTime.Now.ToString("yyyyMM") + "/";
                //人脸识别
                EntityVO eVO = TencentFaceIdEntity.Main("C:/web/ServicesPlatform"+ folder, "1", "1");
               
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = eVO };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败,请重试!", Result = null };
            }
        }

        /// <summary>
        /// 生成问卷签名
        /// </summary>
        /// <param name="activity_id"></param>
        /// <returns></returns>
        [Route("GenerateWjxSign"), HttpGet, Anonymous]
        public ResultObject GenerateWjxSign(string activity_id)
        {
            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                //获取签名
                string wjxSign = BusinessCardBO.GenerateWjxSign(activity_id);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = wjxSign };
            }
            catch (Exception)
            {
                return new ResultObject() { Flag = -1, Message = "获取失败!", Result = null };
            }

        }
        /// <summary>
        /// 123
        /// </summary>
        /// <param name="code">第一次拉手返回Code</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("Test2"), HttpPost, Anonymous]
        public ResultObject Test2()
        {
            try
            {
                //人脸识别
                //CsharpVO CsharpVO = CsharpTest.Main("新闻", 0);
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile(), 1);
                var retu = cBO.WechatPayToChange(0.5M, 2, 23, "oYnrm7SHCMGz-Zws4aucMmNdjUHY", 30, "问卷调查中奖奖金");
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = retu };
            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "获取失败,请重试!", Result = null };
            }
        }

        private static readonly Regex pStartRegex = new Regex(@"<p[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex h2StartRegex = new Regex(@"<h2[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex divStartRegex = new Regex(@"<div[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex attrRegex = new Regex(@"\b(?:class|alt)="".*?""", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex titleRegex = new Regex(@"<div>原标题.*?</div></NTAG>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex contentRegex = new Regex(@"<!-- content end -->", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public string ConvertCsharpNews(string html)
        {
            // 预编译正则表达式（静态只读提升性能）

            try
            {
                // 1. 合并空白字符处理
                html = Regex.Replace(html, @"[\n\r\t]", "");

                // 2. 合并标签转换（减少中间字符串生成）
                html = html
                    .Replace("</p>", "</div></NTAG>")
                    .Replace("</h2>", "</div></NTAG>");

                // 3. 统一处理开始标签（使用预编译正则）
                html = pStartRegex.Replace(html, "<div>");
                html = h2StartRegex.Replace(html, "<div>");
                html = divStartRegex.Replace(html, "<div>");

                // 4. 合并属性移除（class和alt）
                html = attrRegex.Replace(html, "");

                // 5. 移除原标题段落
                html = titleRegex.Replace(html, "");
                html = contentRegex.Replace(html, "");
                
                // 6. 使用自定义解析器

                HtmlParser parser = new HtmlParser(html.Trim());
                parser.KeepTag(new string[] { "div", "NTAG", "img" });
                html = parser.Text();
                return html
               .Replace("<div></div>", "")
               .Replace("<div><img", "<img")
               .Replace("</NTAG></div>", "</NTAG>");
            }
            catch (Exception ex)
            {
                // 记录错误或返回安全值
                System.Diagnostics.Debug.WriteLine($"HTML解析失败: {ex.Message}");
                return html
               .Replace("<div></div>", "")
               .Replace("<div><img", "<img")
               .Replace("</NTAG></div>", "</NTAG>");
            }
            // 7. 合并清理操作（减少字符串分配）
           
        }

        public class works
        {
            public int UserId { get; set; }
        }

        public class userobj
        {

            public int depid { get; set; }
            public string UserName { get; set; }
            public string LoginName { get; set; }
            public string Password { get; set; }

            public string Phone { get; set; }

            public DateTime credate { get; set; }
        }

        public class WorkInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public int WorksId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int UserId { get; set; }
            /// <summary>
            /// 作品名称
            /// </summary>
            public string WorksNmae { get; set; }
            /// <summary>
            /// 作品类型
            /// </summary>
            public string WorksType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string AuthorNmae { get; set; }
            /// <summary>
            /// 作者1
            /// </summary>
            public string TutorName { get; set; }
            /// <summary>
            /// 调研地点
            /// </summary>
            public string AddRess { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string Remarks { get; set; }
            /// <summary>
            /// 作品文件URL
            /// </summary>
            public string WorksUrl { get; set; }
            /// <summary>
            /// 所在区域（市）
            /// </summary>
            public string Region { get; set; }
            /// <summary>
            /// 所在区域（区）
            /// </summary>
            public string District { get; set; }
            /// <summary>
            /// 获奖收件人
            /// </summary>
            public string AwardRecipient { get; set; }
            /// <summary>
            /// 收件电话
            /// </summary>
            public string AwardPhone { get; set; }
            /// <summary>
            /// 收件地址
            /// </summary>
            public string DeliveryAddress { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string ModifyDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string CreateDate { get; set; }
            /// <summary>
            /// 是否允许编辑
            /// </summary>
            public string IsAllowEdit { get; set; }

        }
    }
}
