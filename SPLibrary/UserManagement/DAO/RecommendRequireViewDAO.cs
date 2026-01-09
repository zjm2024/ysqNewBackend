using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class RecommendRequireViewDAO:CommonDAO,IRecommendRequireViewDAO
    {
		public RecommendRequireViewDAO(UserProfile userProfile)
		{
			base._tableName="v_recommendrequireview";
			base._pkId = "Sort";
			base._voType = typeof(RecommendRequireViewVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<RecommendRequireViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<RecommendRequireViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<RecommendRequireViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<RecommendRequireViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public RecommendRequireViewVO FindById(object id)
        {
            return base.FindById<RecommendRequireViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<RecommendRequireViewVO> voList)
        {
            base.InsertList<RecommendRequireViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<RecommendRequireViewVO> voList, int countInEveryRun)
        {
            base.InsertList<RecommendRequireViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<RecommendRequireViewVO> voList)
        {
            base.UpdateListById<RecommendRequireViewVO>(voList);
        }
        
        public void UpdateListByParams(List<RecommendRequireViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<RecommendRequireViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<RecommendRequireViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<RecommendRequireViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<RecommendRequireViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<RecommendRequireViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<RecommendRequireViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<RecommendRequireViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<RecommendRequireViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<RecommendRequireViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<RecommendRequireViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<RecommendRequireViewVO>(strSQL, parameters);
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