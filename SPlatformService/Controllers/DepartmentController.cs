using CoreFramework.VO;
using SPlatformService.Models;
using SPlatformService.TokenMange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;

namespace SPlatformService.Controllers
{
    /// <summary>
    /// 部门API
    /// </summary>
    [RoutePrefix("SPWebAPI/Department")]
    [TokenProjector]    
    public class DepartmentController : ApiController
    {
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteDepartment"), HttpPost]
        public ResultObject DeleteDepartment(string departmentId, string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(departmentId))
                {
                    DepartmentBO dBO = new DepartmentBO(CacheManager.GetUserProfile(token));
                    string[] depIdArr = departmentId.Split(',');
                    bool isAllDelete = true;
                    for (int i = 0; i < depIdArr.Length; i++)
                    {
                        try
                        {
                            dBO.Delete(Convert.ToInt32(depIdArr[i]));
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
        /// 获取部门信息
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetDepartment"), HttpGet]
        public ResultObject GetDepartment(int departmentId, string token)
        {
            DepartmentBO dBO = new DepartmentBO(CacheManager.GetUserProfile(token));
            DepartmentVO depVO = dBO.FindById(departmentId);
            if (depVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = depVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新部门
        /// </summary>
        /// <param name="departmentVO">部门VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateDepartment"), HttpPost]
        public ResultObject UpdateDepartment([FromBody] DepartmentVO departmentVO, string token)
        {
            if (departmentVO != null)
            {
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                DepartmentBO dBO = new DepartmentBO(uProfile);
                //判断DepartmentName是否重复
                if (dBO.IsDepartmentExist(departmentVO))
                {
                    return new ResultObject() { Flag = 0, Message = "部门名称已存在!", Result = null };
                }
                if (departmentVO.DepartmentId < 1)
                {
                    departmentVO.CreatedAt = DateTime.Now;
                    departmentVO.CreatedBy = uProfile.UserId;
                    departmentVO.UpdatedAt = DateTime.Now;
                    departmentVO.UpdatedBy = uProfile.UserId;
                    departmentVO.CompanyId = uProfile.CompanyId;
                    int depId = dBO.Add(departmentVO);
                    if (depId > 0)
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = depId };
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
                else
                {
                    departmentVO.UpdatedAt = DateTime.Now;
                    departmentVO.UpdatedBy = uProfile.UserId;
                    bool isSuccess = dBO.Update(departmentVO);
                    if(isSuccess)
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
        /// 获取所有的有权限看到的部门
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetDepartmentAll"), HttpGet]
        public ResultObject GetDepartmentAll(string token)
        {
            DepartmentBO dBO = new DepartmentBO(CacheManager.GetUserProfile(token));
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = dBO.FindDepartmentAll() };
        }        
    }
}
