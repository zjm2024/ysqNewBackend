using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class AdDAO : CommonDAO, IAdDAO
    {
		public AdDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_ad";
			base._pkId = "AdID";
			base._voType = typeof(AdVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<AdVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<AdVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<AdVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<AdVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public AdVO FindById(object id)
        {
            return base.FindById<AdVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<AdVO> voList)
        {
            base.InsertList<AdVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<AdVO> voList, int countInEveryRun)
        {
            base.InsertList<AdVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<AdVO> voList)
        {
            base.UpdateListById<AdVO>(voList);
        }
        
        public void UpdateListByParams(List<AdVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<AdVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AdVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<AdVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<AdVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<AdVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AdVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<AdVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<AdVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<AdVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<AdVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<AdVO>(strSQL, parameters);
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
        public List<AdVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<AdVO>(strSQL, parameters);
        }
        public List<AdVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc,int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<AdVO>(strSQL, parameters);
        }
    }
}