using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class RecommendAgencyViewDAO:CommonDAO,IRecommendAgencyViewDAO
    {
		public RecommendAgencyViewDAO(UserProfile userProfile)
		{
			base._tableName="v_recommendagencyview";
			base._pkId = "Sort";
			base._voType = typeof(RecommendAgencyViewVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<RecommendAgencyViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<RecommendAgencyViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<RecommendAgencyViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<RecommendAgencyViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public RecommendAgencyViewVO FindById(object id)
        {
            return base.FindById<RecommendAgencyViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<RecommendAgencyViewVO> voList)
        {
            base.InsertList<RecommendAgencyViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<RecommendAgencyViewVO> voList, int countInEveryRun)
        {
            base.InsertList<RecommendAgencyViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<RecommendAgencyViewVO> voList)
        {
            base.UpdateListById<RecommendAgencyViewVO>(voList);
        }
        
        public void UpdateListByParams(List<RecommendAgencyViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<RecommendAgencyViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<RecommendAgencyViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<RecommendAgencyViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<RecommendAgencyViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<RecommendAgencyViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<RecommendAgencyViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<RecommendAgencyViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<RecommendAgencyViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<RecommendAgencyViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<RecommendAgencyViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<RecommendAgencyViewVO>(strSQL, parameters);
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