using System;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial class RequirementTargetCategoryViewDAO:CommonDAO,IRequirementTargetCategoryViewDAO
    {
		public RequirementTargetCategoryViewDAO(UserProfile userProfile)
		{
			base._tableName="v_requirementtargetcategoryview";
			base._pkId = "RequirementTargetCategoryId";
			base._voType = typeof(RequirementTargetCategoryViewVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<RequirementTargetCategoryViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<RequirementTargetCategoryViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<RequirementTargetCategoryViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<RequirementTargetCategoryViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public RequirementTargetCategoryViewVO FindById(object id)
        {
            return base.FindById<RequirementTargetCategoryViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<RequirementTargetCategoryViewVO> voList)
        {
            base.InsertList<RequirementTargetCategoryViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<RequirementTargetCategoryViewVO> voList, int countInEveryRun)
        {
            base.InsertList<RequirementTargetCategoryViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<RequirementTargetCategoryViewVO> voList)
        {
            base.UpdateListById<RequirementTargetCategoryViewVO>(voList);
        }
        
        public void UpdateListByParams(List<RequirementTargetCategoryViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<RequirementTargetCategoryViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<RequirementTargetCategoryViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<RequirementTargetCategoryViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<RequirementTargetCategoryViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<RequirementTargetCategoryViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<RequirementTargetCategoryViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<RequirementTargetCategoryViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<RequirementTargetCategoryViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<RequirementTargetCategoryViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<RequirementTargetCategoryViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<RequirementTargetCategoryViewVO>(strSQL, parameters);
        }

        public int FindTotalCount(string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select Count(0) FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += condition;
            int totalCount = 0;
            try
            {
                totalCount = Convert.ToInt32(DbHelper.ExecuteScalar(strSQL, parameters));
            }
            catch
            {
                totalCount = -1;
            }
            return totalCount;
        }
	}
}