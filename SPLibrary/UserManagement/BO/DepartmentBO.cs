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
    public class DepartmentBO
    {
        private UserProfile CurrentUserProfile = new UserProfile();
        public DepartmentBO(UserProfile userProfile)
        {
            this.CurrentUserProfile = userProfile;
        }
        public int Add(DepartmentVO vo)
        {
            try
            {
                IDepartmentDAO depDAO = UserManagementDAOFactory.CreateDepartmentDAO(this.CurrentUserProfile);
                return depDAO.Insert(vo);
            }
            catch(Exception ex)
            {
                LogBO _log = new LogBO(typeof(DepartmentBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool Update(DepartmentVO vo)
        {
            IDepartmentDAO depDAO = UserManagementDAOFactory.CreateDepartmentDAO(this.CurrentUserProfile);
            try
            {
                depDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(DepartmentBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public bool Delete(int departmentId)
        {
            IDepartmentDAO depDAO = UserManagementDAOFactory.CreateDepartmentDAO(this.CurrentUserProfile);
            IUserDAO uDAO = UserManagementDAOFactory.CreateUserDAO(this.CurrentUserProfile);
            IUserSecurityDAO usDAO = UserManagementDAOFactory.CreateUserSecurityDAO(this.CurrentUserProfile);
            IUserRoleDAO urDAO = UserManagementDAOFactory.CreateUserRoleDAO(this.CurrentUserProfile);

            try
            {
                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    urDAO.DeleteByParams("UserId in (Select UserId from T_CSC_User where DepartmentId = " + departmentId + ")");
                    //delete user security
                    usDAO.DeleteByParams("UserId in (Select UserId from T_CSC_User where DepartmentId = " + departmentId + ")");
                    //delete User
                    uDAO.DeleteByParams(" DepartmentId = " + departmentId );
                    //delete department
                    depDAO.DeleteById(departmentId);
                };
                int result = t.Go();               
                
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(DepartmentBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public DepartmentVO FindById(int departmentId)
        {
            IDepartmentDAO depDAO = UserManagementDAOFactory.CreateDepartmentDAO(this.CurrentUserProfile);
            return depDAO.FindById(departmentId);
        }

        public DepartmentVO FindByParams(string condition,params object[] parameters)
        {
            IDepartmentDAO depDAO = UserManagementDAOFactory.CreateDepartmentDAO(this.CurrentUserProfile);
            List<DepartmentVO> voList = depDAO.FindByParams(condition, parameters);
            if (voList.Count > 0)
                return voList[0];
            else
                return null;
        }

        public List<DepartmentVO> FindDepartmentAll()
        {
            IDepartmentDAO depDAO = UserManagementDAOFactory.CreateDepartmentDAO(this.CurrentUserProfile);
            return depDAO.FindAllBySecurity();
        }

        public List<DepartmentVO> FindAll(string condition,params object[] parameters)
        {
            IDepartmentDAO depDAO = UserManagementDAOFactory.CreateDepartmentDAO(this.CurrentUserProfile);
            return depDAO.FindAllBySecurity();
        }

        public List<DepartmentViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IDepartmentViewDAO depDAO = UserManagementDAOFactory.CreateDepartmentViewDAO(this.CurrentUserProfile);
            return depDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindTotalCount(string condition, params object[] parameters)
        {
            IDepartmentViewDAO depDAO = UserManagementDAOFactory.CreateDepartmentViewDAO(this.CurrentUserProfile);
            return depDAO.FindTotalCount(condition, parameters);
        }

        public bool IsDepartmentExist(DepartmentVO vo)
        {
            IDepartmentDAO depDAO = UserManagementDAOFactory.CreateDepartmentDAO(this.CurrentUserProfile);
            if (vo.DepartmentId > 0)
            {
                List<DepartmentVO> voList = depDAO.FindByParams("DepartmentName = @DepartmentName and DepartmentId <> @DepartmentId", new object[] { DbHelper.CreateParameter("@DepartmentName", vo.DepartmentName), DbHelper.CreateParameter("@DepartmentId", vo.DepartmentId) });
                return voList.Count > 0;
            }
            else
            {
                List<DepartmentVO> voList = depDAO.FindByParams("DepartmentName = @DepartmentName", new object[] { DbHelper.CreateParameter("@DepartmentName", vo.DepartmentName) });
                return voList.Count > 0;
            }

        }

    }
}
