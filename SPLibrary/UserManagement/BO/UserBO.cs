using CoreFramework.DAO;
using CoreFramework.VO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.UserManagement.DAO;
using SPLibrary.UserManagement.VO;

namespace SPLibrary.UserManagement.BO
{
    public class UserBO
    {
        private UserProfile CurrentUserProfile = new UserProfile();

        public UserBO(UserProfile userProfile)
        {
            this.CurrentUserProfile = userProfile;
        }

        public UserVO FindUserByLoginInfo(string loginName, string password)
        {
            IUserDAO uDAO = UserManagementDAOFactory.CreateUserDAO(new UserProfile());
            List<UserVO> voList = uDAO.FindByParams("LoginName = @LoginName and Password = @Password", new object[] { DbHelper.CreateParameter("@LoginName", loginName), DbHelper.CreateParameter("@Password", password) });
            if (voList.Count > 0)
            {
                return voList[0];
            }
            else
            {
                voList = uDAO.FindByParams("UserName = @UserName and Password = @Password", new object[] { DbHelper.CreateParameter("@UserName", loginName), DbHelper.CreateParameter("@Password", password) });
                if (voList.Count > 0)
                {
                    return voList[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public List<UserSecurityViewVO> FindSecurityByUser(int userId)
        {
            IUserSecurityViewDAO usDAO = UserManagementDAOFactory.CreateUserSecurityViewDAO(this.CurrentUserProfile);
            return usDAO.FindByParams("UserId = " + userId);
        }        

        public bool IsHasSecurity(int userId, string securityName, string actionName)
        {
            IUserSecurityViewDAO usDAO = UserManagementDAOFactory.CreateUserSecurityViewDAO(this.CurrentUserProfile);
            List<UserSecurityViewVO> voList = usDAO.FindByParams("UserId = " + userId + " and SecurityTypeName = @SecurityTypeName and SecurityCode = @SecurityCode",
                new object[] { DbHelper.CreateParameter("@SecurityTypeName", securityName), DbHelper.CreateParameter("@SecurityCode", actionName) });
            return voList.Count > 0;
        }

        public DataTable GetMenu(int userId)
        {
            IUserSecurityViewDAO usDAO = UserManagementDAOFactory.CreateUserSecurityViewDAO(this.CurrentUserProfile);
            return usDAO.FindDataTableByParams("UserId = " + userId + " and GroupTypeName = N'菜单权限' and SecurityCode = 'Read'");
        }

        public int Add(UserVO vo, List<UserRoleVO> userRoleVOList)
        {
            try
            {
                IUserDAO uDAO = UserManagementDAOFactory.CreateUserDAO(this.CurrentUserProfile);
                IUserRoleDAO urDAO = UserManagementDAOFactory.CreateUserRoleDAO(this.CurrentUserProfile);
                //return uDAO.Insert(vo);
                if (userRoleVOList == null)
                {
                    userRoleVOList = new List<UserRoleVO>();
                }
                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int userId = uDAO.Insert(vo);

                    foreach (UserRoleVO proDepVO in userRoleVOList)
                    {
                        proDepVO.UserId = userId;
                    }

                    urDAO.InsertList(userRoleVOList, 100);

                    return userId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(UserBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public bool Update(UserVO vo, List<UserRoleVO> userRoleVOList)
        {
            IUserDAO uDAO = UserManagementDAOFactory.CreateUserDAO(this.CurrentUserProfile);
            IUserRoleDAO urDAO = UserManagementDAOFactory.CreateUserRoleDAO(this.CurrentUserProfile);
            try
            {
                //uDAO.UpdateById(vo);
                //return true;
                if (userRoleVOList == null)
                    userRoleVOList = new List<UserRoleVO>();
                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    uDAO.UpdateById(vo);

                    //删除不存在的，添加新增的
                    List<UserRoleVO> urDBVOList = urDAO.FindByParams("UserId = " + vo.UserId);

                    List<UserRoleVO> deleteVOList = new List<UserRoleVO>();

                    foreach (UserRoleVO dbVO in urDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = userRoleVOList.Count - 1; i >= 0; i--)
                        {
                            UserRoleVO urVO = userRoleVOList[i];
                            if (urVO.RoleId == dbVO.RoleId)
                            {
                                userRoleVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            deleteVOList.Add(dbVO);
                        }
                    }
                    if (userRoleVOList != null)
                        urDAO.InsertList(userRoleVOList, 100);
                    foreach (UserRoleVO deleteVO in deleteVOList)
                    {
                        urDAO.DeleteById(deleteVO.UserRoleId);
                    }
                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(UserBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool Delete(int userId)
        {
            IUserDAO uDAO = UserManagementDAOFactory.CreateUserDAO(this.CurrentUserProfile);
            IUserSecurityDAO usDAO = UserManagementDAOFactory.CreateUserSecurityDAO(this.CurrentUserProfile);
            IUserRoleDAO urDAO = UserManagementDAOFactory.CreateUserRoleDAO(this.CurrentUserProfile);

            try
            {
                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    //delete User Role
                    urDAO.DeleteByParams("UserId = " + userId);
                    //delete user security
                    usDAO.DeleteByParams("UserId = " + userId);
                    //delete User
                    uDAO.DeleteById(userId);
                };
                int result = t.Go();

                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(UserBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public UserVO FindById(int userId)
        {
            IUserDAO uDAO = UserManagementDAOFactory.CreateUserDAO(this.CurrentUserProfile);
            return uDAO.FindById(userId);
        }

        public UserViewVO FindUserViewById(int userId)
        {
            IUserViewDAO uDAO = UserManagementDAOFactory.CreateUserViewDAO(this.CurrentUserProfile);
            return uDAO.FindById(userId);
        }

        public UserVO FindByParams(string condition, params object[] parameters)
        {
            IUserDAO uDAO = UserManagementDAOFactory.CreateUserDAO(this.CurrentUserProfile);
            List<UserVO> voList = uDAO.FindByParams(condition, parameters);
            if (voList.Count > 0)
                return voList[0];
            else
                return null;
        }

        public List<UserVO> FindAll(string condition, params object[] parameters)
        {
            IUserDAO uDAO = UserManagementDAOFactory.CreateUserDAO(this.CurrentUserProfile);
            
            return uDAO.FindAllBySecurity();            
        }


        public List<UserRoleViewVO> FindUserRoleViewVOAll(string condition, params object[] parameters)
        {
            IUserRoleViewDAO uDAO = UserManagementDAOFactory.CreateUserRoleViewDAO(this.CurrentUserProfile);

            return uDAO.FindByParams(condition, parameters);
        }

        public List<UserViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IUserViewDAO uDAO = UserManagementDAOFactory.CreateUserViewDAO(this.CurrentUserProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindTotalCount(string condition, params object[] parameters)
        {
            IUserViewDAO uDAO = UserManagementDAOFactory.CreateUserViewDAO(this.CurrentUserProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }


        public List<UserViewVO> FindAllByPageIndex1(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IUserViewDAO uDAO = UserManagementDAOFactory.CreateUserViewDAO(this.CurrentUserProfile);
            return uDAO.FindAllByPageIndex1(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindTotalCount1(string condition, params object[] parameters)
        {
            IUserViewDAO uDAO = UserManagementDAOFactory.CreateUserViewDAO(this.CurrentUserProfile);
            return uDAO.FindTotalCount1(condition, parameters);
        }


        public bool IsUserExist(UserVO vo)
        {
            IUserDAO uDAO = UserManagementDAOFactory.CreateUserDAO(this.CurrentUserProfile);
            //允许LoginName和UserName都能够登录，所以全部要判断是否重复
            if (vo.UserId > 0)
            {
                List<UserVO> voList = uDAO.FindByParams("(LoginName = @LoginName or UserName = @LoginName) and UserId <> @UserId", new object[] { DbHelper.CreateParameter("@LoginName", vo.LoginName), DbHelper.CreateParameter("@UserId", vo.UserId) });
                return voList.Count > 0;
            }
            else
            {
                List<UserVO> voList = uDAO.FindByParams("LoginName = @LoginName or UserName = @LoginName", new object[] { DbHelper.CreateParameter("@LoginName", vo.LoginName) });
                return voList.Count > 0;
            }

        }

        public bool IsUserNameExist(UserVO vo)
        {
            IUserDAO uDAO = UserManagementDAOFactory.CreateUserDAO(this.CurrentUserProfile);
            //允许LoginName和UserName都能够登录，所以全部要判断是否重复
            if (vo.UserId > 0)
            {
                List<UserVO> voList = uDAO.FindByParams("(LoginName = @UserName or UserName = @UserName) and UserId <> @UserId", new object[] { DbHelper.CreateParameter("@UserName", vo.UserName), DbHelper.CreateParameter("@UserId", vo.UserId) });
                return voList.Count > 0;
            }
            else
            {
                List<UserVO> voList = uDAO.FindByParams("LoginName = @UserName or UserName = @UserName", new object[] { DbHelper.CreateParameter("@UserName", vo.UserName) });
                return voList.Count > 0;
            }

        }

        public bool ChangePassword(int userId, string password, string newPassword)
        {
            IUserDAO uDAO = UserManagementDAOFactory.CreateUserDAO(this.CurrentUserProfile);
            if (uDAO.FindByParams("UserId = @UserId and Password = @Password", new object[] { DbHelper.CreateParameter("@UserId", userId), DbHelper.CreateParameter("@Password", password) }).Count > 0)
            {
                try
                {
                    string sql = "update T_CSC_User set Password = @NewPassword where UserId = @UserId and Password = @Password";
                    DbHelper.ExecuteNonQuery(sql, new object[] { DbHelper.CreateParameter("@NewPassword", newPassword), DbHelper.CreateParameter("@UserId", userId), DbHelper.CreateParameter("@Password", password) });
                    return true;
                }
                catch (Exception ex)
                {
                    LogBO _log = new LogBO(typeof(UserBO));
                    string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                    _log.Error(strErrorMsg);
                    return false;
                }
            }
            return false;
        }
        public bool ChangeUserPassword(int userId, string newPassword)
        {
            IUserDAO uDAO = UserManagementDAOFactory.CreateUserDAO(this.CurrentUserProfile);

            try
            {
                string sql = "update T_CSC_User set Password = @NewPassword where UserId = @UserId";
                DbHelper.ExecuteNonQuery(sql, new object[] { DbHelper.CreateParameter("@NewPassword", newPassword), DbHelper.CreateParameter("@UserId", userId) });
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(UserBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }            
        }

        public List<UserRoleVO> FindUserRoleByUserId(int userId)
        {
            IUserRoleDAO urDAO = UserManagementDAOFactory.CreateUserRoleDAO(this.CurrentUserProfile);
            return urDAO.FindByParams("UserId = " + userId);
        }
        

        public List<TokenVO> FindTokeAll()
        {
            ITokenDAO tDAO = UserManagementDAOFactory.CreateTokenDAO(CurrentUserProfile);
            return tDAO.FindByParams("1=1");
        }

        public List<TokenVO> FindTokeByUserId(string userId)
        {
            ITokenDAO tDAO = UserManagementDAOFactory.CreateTokenDAO(CurrentUserProfile);
            return tDAO.FindByParams("UserId=" + userId);
        }

        public List<TokenVO> FindTokeByToken(string token, int userId)
        {
            ITokenDAO tDAO = UserManagementDAOFactory.CreateTokenDAO(CurrentUserProfile);
            return tDAO.FindByParams("Token = '" + token + "' and UserId =" + userId);
        }

        public int InsertToken(TokenVO tVO)
        {
            //判断是否存在，如果存在则更新，如果不存在则新增
            //判断条件 IsUser == true && UserId == 1    || IsUser == false && CustomerId == 1
            ITokenDAO tDAO = UserManagementDAOFactory.CreateTokenDAO(CurrentUserProfile);
            List<TokenVO> voList = tDAO.FindByParams("CompanyId = '" + tVO.CompanyId + "' and DepartmentId = " + tVO.DepartmentId + " and UserId = " + tVO.UserId);

            if (voList.Count > 0)
            {
                tVO.TokenId = voList[0].TokenId;
                tDAO.UpdateById(tVO);
            }
            else
            {
                tDAO.Insert(tVO);
            }
            return 1;
        }

        public void UpdateTokenTime(TokenVO tVO)
        {
            ITokenDAO tDAO = UserManagementDAOFactory.CreateTokenDAO(CurrentUserProfile);
            tDAO.UpdateByParams(tVO, "Token = '" + tVO.Token + "'");
        }

        public void DeleteTokenbyToken(string token, int userId)
        {
            ITokenDAO tDAO = UserManagementDAOFactory.CreateTokenDAO(CurrentUserProfile);
            tDAO.DeleteByParams("Token = '" + token + "' and UserId= " + userId);
        }

        public bool AddUserLoginHistory(UserLoginHistoryVO ulHistoryVO)
        {
            IUserLoginHistoryDAO clDAO = UserManagementDAOFactory.CreateUserLoginHistoryDAO(new CustomerProfile());
            return clDAO.Insert(ulHistoryVO) > 0;
        }

        public bool UpdateRemcommendAgency(List<RecommendAgencyVO> recommendAgencyVOList)
        {
            //根据CityId和CategoryId来判断
            //删掉不存在的，添加新的，更新存在的
            //一次只更新ProvinceId + CityId + ParentCategoryId + CategoryId的组合

            IRecommendAgencyDAO raDAO = UserManagementDAOFactory.CreateRecommendAgencyDAO(CurrentUserProfile);
            try
            {
                if (recommendAgencyVOList == null)
                {
                    recommendAgencyVOList = new List<RecommendAgencyVO>();
                }                

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    //删除不存在的，添加新增的
                    string condition = " 1=1 ";
                    if (recommendAgencyVOList[0].ProvinceId > 0)
                        condition += " And ProvinceId = " + recommendAgencyVOList[0].ProvinceId;
                    else
                        condition += " And ProvinceId is null";
                    if (recommendAgencyVOList[0].CityId < 1)
                    {
                        condition += " and CityId is null";
                    }
                    else
                    {
                        condition += " and CityId = " + recommendAgencyVOList[0].CityId;
                    }

                    if (recommendAgencyVOList[0].ParentCategoryId > 0)
                        condition += " And ParentCategoryId = " + recommendAgencyVOList[0].ParentCategoryId;
                    else
                        condition += " And ParentCategoryId is null";

                    if (recommendAgencyVOList[0].CategoryId < 1)
                    {
                        condition += " and CategoryId is null";
                    }
                    else
                    {
                        condition += " and CategoryId = " + recommendAgencyVOList[0].CategoryId;
                    }

                    List<RecommendAgencyVO> bcDBVOList = raDAO.FindByParams(condition);
                    List<RecommendAgencyVO> bcdeleteVOList = new List<RecommendAgencyVO>();
                    List<RecommendAgencyVO> bcUpdateVOList = new List<RecommendAgencyVO>();
                    foreach (RecommendAgencyVO dbVO in bcDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = recommendAgencyVOList.Count - 1; i >= 0; i--)
                        {
                            RecommendAgencyVO bcVO = recommendAgencyVOList[i];
                            if (bcVO.AgencyId == dbVO.AgencyId)
                            {
                                bcVO.RecommendAgencyId = dbVO.RecommendAgencyId;
                                bcUpdateVOList.Add(bcVO);
                                recommendAgencyVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            bcdeleteVOList.Add(dbVO);
                        }
                    }
                    //新增
                    if (recommendAgencyVOList != null && recommendAgencyVOList.Count > 0)
                        raDAO.InsertList(recommendAgencyVOList, 100);
                    //删除不存在的
                    foreach (RecommendAgencyVO deleteVO in bcdeleteVOList)
                    {
                        raDAO.DeleteById(deleteVO.RecommendAgencyId);
                    }
                    //更新
                    foreach (RecommendAgencyVO updateVO in bcUpdateVOList)
                    {
                        raDAO.UpdateById(updateVO);
                    }

                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(UserBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }            
        }

        public bool UpdateRecommendRequire(List<RecommendRequireVO> recommendRequireVOList)
        {
            //根据CityId和CategoryId来判断
            //删掉不存在的，添加新的，更新存在的
            //一次只更新ProvinceId + CityId + ParentCategoryId + CategoryId的组合

            IRecommendRequireDAO raDAO = UserManagementDAOFactory.CreateRecommendRequireDAO(CurrentUserProfile);
            try
            {
                if (recommendRequireVOList == null)
                {
                    recommendRequireVOList = new List<RecommendRequireVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    //删除不存在的，添加新增的
                    string condition = " 1=1 ";
                    if (recommendRequireVOList[0].ProvinceId > 0)
                        condition += " And ProvinceId = " + recommendRequireVOList[0].ProvinceId;
                    else
                        condition += " And ProvinceId is null";
                   if (recommendRequireVOList[0].CityId < 1)
                    {
                        condition += " and CityId is null";
                    }
                    else
                    {
                        condition += " and CityId = " + recommendRequireVOList[0].CityId;
                    }

                    if (recommendRequireVOList[0].ParentCategoryId > 0)
                        condition += " And ParentCategoryId = " + recommendRequireVOList[0].ParentCategoryId;
                    else
                        condition += " And ParentCategoryId is null";                   
                    
                    if (recommendRequireVOList[0].CategoryId < 1)
                    {
                        condition += " and CategoryId is null";
                    }
                    else
                    {
                        condition += " and CategoryId = " + recommendRequireVOList[0].CategoryId;
                    }
                    List<RecommendRequireVO> bcDBVOList = raDAO.FindByParams(condition);
                    List<RecommendRequireVO> bcdeleteVOList = new List<RecommendRequireVO>();
                    List<RecommendRequireVO> bcUpdateVOList = new List<RecommendRequireVO>();
                    foreach (RecommendRequireVO dbVO in bcDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = recommendRequireVOList.Count - 1; i >= 0; i--)
                        {
                            RecommendRequireVO bcVO = recommendRequireVOList[i];
                            if (bcVO.RequirementId == dbVO.RequirementId)
                            {
                                bcVO.RecommendRequireId = dbVO.RecommendRequireId;
                                bcUpdateVOList.Add(bcVO);
                                recommendRequireVOList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            bcdeleteVOList.Add(dbVO);
                        }
                    }
                    //新增
                    if (recommendRequireVOList != null && recommendRequireVOList.Count > 0)
                        raDAO.InsertList(recommendRequireVOList, 100);
                    //删除不存在的
                    foreach (RecommendRequireVO deleteVO in bcdeleteVOList)
                    {
                        raDAO.DeleteById(deleteVO.RecommendRequireId);
                    }
                    //更新
                    if (bcUpdateVOList.Count > 0)
                    {
                        foreach (RecommendRequireVO updateVO in bcUpdateVOList)
                        {
                            raDAO.UpdateById(updateVO);
                        }                        
                    }

                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(UserBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public List<RecommendAgencyViewVO> FindRecommendAgencyList(int provinceId, int cityId, int parentCategoryId, int categoryId)
        {
            IRecommendAgencyViewDAO raDAO = UserManagementDAOFactory.CreateRecommendAgencyViewDAO(CurrentUserProfile);
            string condition = " 1=1 ";
            if (provinceId > 0)
                condition += " And ProvinceId = " + provinceId;
            else
                condition += " And ProvinceId is null";
            if (cityId > 0)
                condition += " And CityId = " + cityId;
            else
                condition += " And CityId is null";
            if (parentCategoryId > 0)
                condition += " And ParentCategoryId = " + parentCategoryId;
            else
                condition += " And ParentCategoryId is null";
            if (categoryId > 0)
                condition += " And CategoryId = " + categoryId;
            else
                condition += " And CategoryId is null";
            return raDAO.FindByParams(condition);
        }

        public List<RecommendRequireViewVO> FindRecommendRequireList(int provinceId, int cityId, int parentCategoryId, int categoryId)
        {
            IRecommendRequireViewDAO raDAO = UserManagementDAOFactory.CreateRecommendRequireViewDAO(CurrentUserProfile);
            string condition = " 1=1 ";
            if (provinceId > 0)
                condition += " And ProvinceId = " + provinceId;
            else
                condition += " And ProvinceId is null";
            if (cityId > 0)
                condition += " And CityId = " + cityId;
            else
                condition += " And CityId is null";
            if (parentCategoryId > 0)
                condition += " And ParentCategoryId = " + parentCategoryId;
            else
                condition += " And ParentCategoryId is null";
            if (categoryId > 0)
                condition += " And CategoryId = " + categoryId;
            else
                condition += " And CategoryId is null";
            return raDAO.FindByParams(condition);
        }
    }
}
