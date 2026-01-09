using CoreFramework.VO;
using SPlatformService.Models;
using SPlatformService.TokenMange;
using System;
using System.Collections.Generic;
using System.Web.Http;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;

namespace SPlatformService.Controllers
{
    /// <summary>
    /// 角色 API
    /// </summary>
    [RoutePrefix("SPWebAPI/Role")]
    [TokenProjector]
    public class RoleController : ApiController
    {
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("DeleteRole"), HttpPost]
        public ResultObject DeleteRole(string roleId, string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(roleId))
                {
                    string[] roleIdArr = roleId.Split(',');
                    bool isAllDelete = true;
                    RoleBO rBO = new RoleBO(CacheManager.GetUserProfile(token));
                    for (int i = 0; i < roleIdArr.Length; i++)
                    {
                        try
                        {
                            rBO.Delete(Convert.ToInt32(roleIdArr[i]));
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
        /// 获取角色信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetRole"), HttpGet]
        public ResultObject GetRole(int roleId, string token)
        {
            RoleBO rBO = new RoleBO(CacheManager.GetUserProfile(token));
            RoleVO roleVO = rBO.FindById(roleId);
            if (roleVO != null)
            {
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = roleVO };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }

        /// <summary>
        /// 添加或更新角色
        /// </summary>
        /// <param name="roleModelVO">角色VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateRole"), HttpPost]
        public ResultObject UpdateRole([FromBody] RoleModel roleModelVO, string token)
        {
            if (roleModelVO != null)
            {
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                RoleBO rBO = new RoleBO(uProfile);
                RoleVO roleVO = roleModelVO.Role;
                List<RoleSecurityVO> roleSecurityVOList = roleModelVO.RoleSecurity;
                //判断RoleName是否重复
                roleVO.CompanyId = uProfile.CompanyId;
                if (rBO.IsRoleExist(roleVO))
                {
                    return new ResultObject() { Flag = 0, Message = "角色名称已存在!", Result = null };
                }
                if (roleVO.RoleId < 1)
                {
                    roleVO.CreatedAt = DateTime.Now;
                    roleVO.CreatedBy = uProfile.UserId;
                    roleVO.UpdatedAt = DateTime.Now;
                    roleVO.UpdatedBy = uProfile.UserId;                    
                    int roleId = rBO.Add(roleVO, roleSecurityVOList);
                    if (roleId > 0)
                        return new ResultObject() { Flag = 1, Message = "添加成功!", Result = roleId };
                    else
                        return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };
                }
                else
                {
                    roleVO.UpdatedAt = DateTime.Now;
                    roleVO.UpdatedBy = uProfile.UserId;
                    bool isSuccess = rBO.Update(roleVO, roleSecurityVOList);
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
        /// 获取所有权限
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAllSecurity"), HttpGet]
        public ResultObject GetAllSecurity(string token)
        {
            RoleBO rBO = new RoleBO(CacheManager.GetUserProfile(token));
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = rBO.FindAllSecurity() };
        }

        /// <summary>
        /// 根据User获取所有权限
        /// </summary>
        /// <param name="userId">平台账号ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAllSecurityByUser"), HttpGet]
        public ResultObject GetAllSecurityByUser(int userId,string token)
        {
            UserBO uBO = new UserBO(CacheManager.GetUserProfile(token));
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = uBO.FindSecurityByUser(userId) };
        }

        /// <summary>
        /// 根据角色获取所有权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetAllSecurityByRole"), HttpGet]
        public ResultObject GetAllSecurityByRole(int roleId, string token)
        {
            RoleBO rBO = new RoleBO(CacheManager.GetUserProfile(token));
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = rBO.FindAllSecurityByRole(roleId) };
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetRoleAll"), HttpGet]
        public ResultObject GetRoleAll(string token)
        {
            RoleBO rBO = new RoleBO(CacheManager.GetUserProfile(token));
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = rBO.FindRoleAll(new UserPrincipal().UserProfile.CompanyId) };
        }
    }
}
