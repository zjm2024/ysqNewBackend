using CoreFramework.DAO;
using CoreFramework.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.UserManagement.DAO;
using SPLibrary.UserManagement.VO;

namespace SPLibrary.UserManagement.BO
{
    public class RoleBO
    {
        private UserProfile CurrentUserProfile = new UserProfile();

        public RoleBO(UserProfile userProfile)
        {
            this.CurrentUserProfile = userProfile;
        }
        public int Add(RoleVO vo,List<RoleSecurityVO> roleSecurityVOList)
        {
            try
            {
                IRoleDAO rDAO = UserManagementDAOFactory.CreateRoleDAO(this.CurrentUserProfile);
                IRoleSecurityDAO rsDAO = UserManagementDAOFactory.CreateRoleSecurityDAO(this.CurrentUserProfile);

                if (roleSecurityVOList == null)
                    roleSecurityVOList = new List<RoleSecurityVO>();

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int roleId = rDAO.Insert(vo);
                    
                    foreach (RoleSecurityVO rsVO in roleSecurityVOList)
                    {
                        rsVO.RoleId = roleId;
                    }

                    rsDAO.InsertList(roleSecurityVOList, 100);

                    return roleId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(RoleBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool Update(RoleVO vo, List<RoleSecurityVO> roleSecurityVOList)
        {
            IRoleDAO rDAO = UserManagementDAOFactory.CreateRoleDAO(this.CurrentUserProfile);
            IRoleSecurityDAO rsDAO = UserManagementDAOFactory.CreateRoleSecurityDAO(this.CurrentUserProfile);
            try
            {
                //rDAO.UpdateById(vo);
                //return true;

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    rDAO.UpdateById(vo);

                    //删除不存在的，添加新增的
                    List<RoleSecurityVO> rsDBVOList = rsDAO.FindByParams("RoleId = " + vo.RoleId);

                    List<RoleSecurityVO> deleteVOList = new List<RoleSecurityVO>();

                    foreach (RoleSecurityVO dbVO in rsDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = roleSecurityVOList.Count - 1; i >= 0; i--)
                        {
                            RoleSecurityVO rsVO = roleSecurityVOList[i];
                            if (rsVO.SecurityId == dbVO.SecurityId)
                            {
                                roleSecurityVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            deleteVOList.Add(dbVO);
                        }
                    }

                    rsDAO.InsertList(roleSecurityVOList, 100);
                    foreach (RoleSecurityVO deleteVO in deleteVOList)
                    {
                        rsDAO.DeleteById(deleteVO.RoleSecurityId);
                    }

                };
                int result = t.Go();
                return result > 0;

            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(RoleBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool Delete(int roleId)
        {
            IRoleDAO rDAO = UserManagementDAOFactory.CreateRoleDAO(this.CurrentUserProfile);
            IRoleSecurityDAO rsDAO = UserManagementDAOFactory.CreateRoleSecurityDAO(this.CurrentUserProfile);
            IUserRoleDAO urDAO = UserManagementDAOFactory.CreateUserRoleDAO(this.CurrentUserProfile);
            try
            {
                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    //已经使用了的User，更新其RoleId为null
                    //DbHelper.ExecuteNonQuery("Update T_CSC_User set RoleId = null where RoleId = " + roleId);
                    urDAO.DeleteByParams("RoleId = " + roleId);
                    //delete role security
                    rsDAO.DeleteByParams("RoleId = " + roleId);
                    //delete role
                    rDAO.DeleteById(roleId);
                };
                int result = t.Go();

                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(RoleBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public RoleVO FindById(int roleId)
        {
            IRoleDAO rDAO = UserManagementDAOFactory.CreateRoleDAO(this.CurrentUserProfile);
            return rDAO.FindById(roleId);
        }

        public RoleVO FindByParams(string condition, params object[] parameters)
        {
            IRoleDAO rDAO = UserManagementDAOFactory.CreateRoleDAO(this.CurrentUserProfile);
            List<RoleVO> voList = rDAO.FindByParams(condition, parameters);
            if (voList.Count > 0)
                return voList[0];
            else
                return null;
        }

        public List<RoleVO> FindRoleAll(int companyId)
        {
            IRoleDAO rDAO = UserManagementDAOFactory.CreateRoleDAO(this.CurrentUserProfile);
            List<RoleVO> voList = rDAO.FindByParams("CompanyId = " + companyId);
            //for (int i = 0; i < voList.Count; i++)
            //{
            //    if (voList[i].RoleName == "系统管理员")
            //    {
            //        voList.RemoveAt(i);
            //        break;
            //    }
            //}
            return voList;
        }

        public List<SecurityTypeViewVO> FindAllSecurity()
        {
            ISecurityTypeViewDAO stDAO = UserManagementDAOFactory.CreateSecurityTypeViewDAO(this.CurrentUserProfile);
            ISecurityViewDAO sDAO = UserManagementDAOFactory.CreateSecurityViewDAO(this.CurrentUserProfile);
            List<SecurityTypeViewVO> stVOList = stDAO.FindByParams("1=1");
            stVOList = stVOList.OrderBy(u => u.GroupTypeId).ToList<SecurityTypeViewVO>();
            foreach (SecurityTypeViewVO stVO in stVOList)
            {
                stVO.SecurityVOList = sDAO.FindByParams("SecurityTypeId = " + stVO.SecurityTypeId);
            }
            return stVOList;
        }

        public List<RoleSecurityViewVO> FindAllSecurityByRole(int roleId)
        {
            IRoleSecurityViewDAO rsDAO = UserManagementDAOFactory.CreateRoleSecurityViewDAO(this.CurrentUserProfile);

            return rsDAO.FindByParams("RoleId = " + roleId);            
        }

        public List<RoleViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IRoleViewDAO rDAO = UserManagementDAOFactory.CreateRoleViewDAO(this.CurrentUserProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindTotalCount(string condition, params object[] parameters)
        {
            IRoleViewDAO rDAO = UserManagementDAOFactory.CreateRoleViewDAO(this.CurrentUserProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }

        public bool IsRoleExist(RoleVO vo)
        {
            IRoleDAO rDAO = UserManagementDAOFactory.CreateRoleDAO(this.CurrentUserProfile);
            if (vo.RoleId > 0)
            {
                List<RoleVO> voList = rDAO.FindByParams("CompanyId = @CompanyId and RoleName = @RoleName and RoleId <> @RoleId", new object[] { DbHelper.CreateParameter("@RoleName", vo.RoleName), DbHelper.CreateParameter("@RoleId", vo.RoleId), DbHelper.CreateParameter("@CompanyId", vo.CompanyId) });
                return voList.Count > 0;
            }
            else
            {
                List<RoleVO> voList = rDAO.FindByParams("CompanyId = @CompanyId and RoleName = @RoleName", new object[] { DbHelper.CreateParameter("@RoleName", vo.RoleName), DbHelper.CreateParameter("@CompanyId", vo.CompanyId) });
                return voList.Count > 0;
            }

        }
    }
}
