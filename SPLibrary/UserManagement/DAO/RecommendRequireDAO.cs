using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class RecommendRequireDAO:CommonDAO,IRecommendRequireDAO
    {
		public RecommendRequireDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_recommendrequire";
			base._pkId = "RecommendRequireId";
			base._voType = typeof(RecommendRequireVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<RecommendRequireVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<RecommendRequireVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<RecommendRequireVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<RecommendRequireVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public RecommendRequireVO FindById(object id)
        {
            return base.FindById<RecommendRequireVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<RecommendRequireVO> voList)
        {
            base.InsertList<RecommendRequireVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<RecommendRequireVO> voList, int countInEveryRun)
        {
            base.InsertList<RecommendRequireVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<RecommendRequireVO> voList)
        {
            base.UpdateListById<RecommendRequireVO>(voList);
        }
        
        public void UpdateListByParams(List<RecommendRequireVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<RecommendRequireVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<RecommendRequireVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<RecommendRequireVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<RecommendRequireVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<RecommendRequireVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<RecommendRequireVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<RecommendRequireVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<RecommendRequireVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<RecommendRequireVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<RecommendRequireVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<RecommendRequireVO>(strSQL, parameters);
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