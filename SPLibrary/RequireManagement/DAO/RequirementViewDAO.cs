using System;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial class RequirementViewDAO:CommonDAO,IRequirementViewDAO
    {
		public RequirementViewDAO(UserProfile userProfile)
		{
			base._tableName="v_requirementview";
			base._pkId = "RequirementId";
			base._voType = typeof(RequirementViewVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<RequirementViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<RequirementViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<RequirementViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<RequirementViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public RequirementViewVO FindById(object id)
        {
            return base.FindById<RequirementViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<RequirementViewVO> voList)
        {
            base.InsertList<RequirementViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<RequirementViewVO> voList, int countInEveryRun)
        {
            base.InsertList<RequirementViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<RequirementViewVO> voList)
        {
            base.UpdateListById<RequirementViewVO>(voList);
        }
        
        public void UpdateListByParams(List<RequirementViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<RequirementViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<RequirementViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<RequirementViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<RequirementViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<RequirementViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<RequirementViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<RequirementViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<RequirementViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<RequirementViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<RequirementViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<RequirementViewVO>(strSQL, parameters);
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
        public List<RequirementViewVO> FindMatchRequireByPageIndex(int agencyId, string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL = " call sp_match_require(" + agencyId + "," + start + "," + end + ",'" + sortcolname + "','" + asc + "','" + conditionStr + "'); \n";


            return DbHelper.ExecuteVO<RequirementViewVO>(strSQL, parameters);
        }

        public int FindMatchRequireTotalCount(int agencyId, string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL = " call sp_match_require_count(" + agencyId + ",'" + condition + "'); \n";

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

        public int FindTotalSum(string sum, string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select Sum(" + sum + ") FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += condition;
            int totalCount = 0;
            try
            {
                totalCount = Convert.ToInt32(DbHelper.ExecuteScalar(strSQL, parameters));
            }
            catch
            {
                totalCount = 0;
            }
            return totalCount;
        }
    }
}