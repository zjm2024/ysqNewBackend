using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.UserManagement.DAO;
using SPLibrary.UserManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.UserManagement.BO
{
    public class CategoryBO
    {
        private UserProfile CurrentUserProfile = new UserProfile();
        public CategoryBO(UserProfile userProfile)
        {
            this.CurrentUserProfile = userProfile;
        }
        public int Add(CategoryVO vo, List<CategoryVO> childVOList)
        {
            try
            {
                ICategoryDAO pDAO = UserManagementDAOFactory.CreateCategoryDAO(this.CurrentUserProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int categoryId = pDAO.Insert(vo);
                    if (childVOList != null)
                    {
                        foreach (CategoryVO cVO in childVOList)
                        {
                            cVO.ParentCategoryId = categoryId;
                        }

                        pDAO.InsertList(childVOList, 100);
                    }
                    return categoryId;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CategoryBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        public bool Update(CategoryVO vo, List<CategoryVO> childVOList)
        {
            try
            {
                ICategoryDAO pDAO = UserManagementDAOFactory.CreateCategoryDAO(this.CurrentUserProfile);

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    pDAO.UpdateById(vo);

                    //更新存在的，添加新增的                    

                    if (childVOList != null)
                    {
                        foreach (CategoryVO cVO in childVOList)
                        {
                            if (cVO.CategoryId > 0)
                            {
                                pDAO.UpdateById(cVO);
                            }
                            else
                            {
                                pDAO.Insert(cVO);
                            }
                        }
                    }

                };
                int result = t.Go();
                return result > 0;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CategoryBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        public CategoryVO FindById(int categoryId)
        {
            ICategoryDAO pDAO = UserManagementDAOFactory.CreateCategoryDAO(this.CurrentUserProfile);
            return pDAO.FindById(categoryId);
        }

        public List<CategoryVO> FindParentCategoryList(bool enable)
        {
            ICategoryDAO pDAO = UserManagementDAOFactory.CreateCategoryDAO(this.CurrentUserProfile);
            List<CategoryVO> voList = pDAO.FindByParams(" ParentCategoryId is null " + (enable ? " AND Status = true" : ""));
            return voList;
        }

        public List<CategoryVO> FindCategoryAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICategoryDAO pDAO = UserManagementDAOFactory.CreateCategoryDAO(this.CurrentUserProfile);
            conditionStr = string.IsNullOrEmpty(conditionStr) ? " ParentCategoryId is null " : conditionStr + " AND ParentCategoryId is null ";
            return pDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindCategoryTotalCount(string condition, params object[] parameters)
        {
            ICategoryDAO pDAO = UserManagementDAOFactory.CreateCategoryDAO(this.CurrentUserProfile);
            condition = string.IsNullOrEmpty(condition) ? " ParentCategoryId is null " : condition + " AND ParentCategoryId is null ";
            return pDAO.FindTotalCount(condition, parameters);
        }

        public bool IsCategoryExist(CategoryVO vo)
        {
            ICategoryDAO pDAO = UserManagementDAOFactory.CreateCategoryDAO(this.CurrentUserProfile);
            if (vo.CategoryId > 0)
            {
                List<CategoryVO> voList = pDAO.FindByParams("CategoryName = @CategoryName and CategoryCode = @CategoryCode and CategoryId <> @CategoryId", new object[] { DbHelper.CreateParameter("@CategoryCode", vo.CategoryCode), DbHelper.CreateParameter("@CategoryName", vo.CategoryName), DbHelper.CreateParameter("@CategoryId", vo.CategoryId) });
                return voList.Count > 0;
            }
            else
            {
                List<CategoryVO> voList = pDAO.FindByParams("CategoryName = @CategoryName and CategoryCode = @CategoryCode", new object[] { DbHelper.CreateParameter("@CategoryCode", vo.CategoryCode), DbHelper.CreateParameter("@CategoryName", vo.CategoryName) });
                return voList.Count > 0;
            }
        }



        public List<CategoryVO> FindCategoryByParent(int parentProvinceId, bool enable)
        {
            ICategoryDAO pDAO = UserManagementDAOFactory.CreateCategoryDAO(this.CurrentUserProfile);
            List<CategoryVO> voList = pDAO.FindByParams("ParentCategoryId = " + parentProvinceId + (enable ? " AND Status = true" : ""));
            return voList;
        }

        public List<CategoryVO> FindCategoryByParent(bool enable)
        {
            ICategoryDAO pDAO = UserManagementDAOFactory.CreateCategoryDAO(this.CurrentUserProfile);
            List<CategoryVO> voList = pDAO.FindByParams("ParentCategoryId > 0 " + (enable ? " AND Status = true" : ""));
            return voList;
        }
        public List<CategoryVO> FindAllCategory( bool enable)
        {
            ICategoryDAO pDAO = UserManagementDAOFactory.CreateCategoryDAO(this.CurrentUserProfile);
            List<CategoryVO> voList = pDAO.FindByParams("ParentCategoryId is not null " + ( enable ? " AND Status = true" : ""));
            return voList;
        }
        public bool IsChildCategoryExist(CategoryVO vo)
        {
            ICategoryDAO pDAO = UserManagementDAOFactory.CreateCategoryDAO(this.CurrentUserProfile);
            if (vo.ParentCategoryId > 0)
            {
                if (vo.CategoryId > 0)
                {
                    List<CategoryVO> voList = pDAO.FindByParams("CategoryName = @CategoryName and CategoryCode = @CategoryCode and ParentCategoryId = @ParentCategoryId and CategoryId <> @CategoryId",
                        new object[] { DbHelper.CreateParameter("@CategoryCode", vo.CategoryCode),
                            DbHelper.CreateParameter("@CategoryName", vo.CategoryName),
                            DbHelper.CreateParameter("@ParentCategoryId", vo.ParentCategoryId),
                            DbHelper.CreateParameter("@CategoryId", vo.CategoryId) });
                    return voList.Count > 0;
                }
                else
                {
                    List<CategoryVO> voList = pDAO.FindByParams("CategoryName = @CategoryName and CategoryCode = @CategoryCode and ParentCategoryId = @ParentCategoryId",
                        new object[] { DbHelper.CreateParameter("@CategoryCode", vo.CategoryCode),
                            DbHelper.CreateParameter("@ParentCategoryId", vo.ParentCategoryId),
                            DbHelper.CreateParameter("@CategoryName", vo.CategoryName) });
                    return voList.Count > 0;
                }
            }
            return false;
        }

        public List<CategoryViewVO> FindCategoryViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICategoryViewDAO pDAO = UserManagementDAOFactory.CreateCategoryViewDAO(this.CurrentUserProfile);           
            return pDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        public int FindCategoryViewTotalCount(string condition, params object[] parameters)
        {
            ICategoryViewDAO pDAO = UserManagementDAOFactory.CreateCategoryViewDAO(this.CurrentUserProfile);
            return pDAO.FindTotalCount(condition, parameters);
        }
    }
}
