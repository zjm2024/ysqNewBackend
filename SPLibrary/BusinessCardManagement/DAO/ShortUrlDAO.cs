using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class ShortUrlDAO : CommonDAO, IShortUrlDAO
    { 
        public ShortUrlDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_shorturl";
			base._pkId = "ShortUrlID";
			base._voType = typeof(ShortUrlVO);
            base.CurrentUserProfile = userProfile;
        }
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ShortUrlVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ShortUrlVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ShortUrlVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ShortUrlVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ShortUrlVO FindById(object id)
        {
            return base.FindById<ShortUrlVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ShortUrlVO> voList)
        {
            base.InsertList<ShortUrlVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ShortUrlVO> voList, int countInEveryRun)
        {
            base.InsertList<ShortUrlVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ShortUrlVO> voList)
        {
            base.UpdateListById<ShortUrlVO>(voList);
        }
        
        public void UpdateListByParams(List<ShortUrlVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ShortUrlVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ShortUrlVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<ShortUrlVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<ShortUrlVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ShortUrlVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ShortUrlVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ShortUrlVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<ShortUrlVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<ShortUrlVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<ShortUrlVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ShortUrlVO>(strSQL, parameters);
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
        public List<ShortUrlVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<ShortUrlVO>(strSQL, parameters);
        }
        public List<ShortUrlVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc,int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<ShortUrlVO>(strSQL, parameters);
        }

        public decimal FindTotalSum(string sum, string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select Sum(" + sum + ") FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += condition;
            decimal totalCount = 0;
            try
            {
                totalCount = Convert.ToDecimal(DbHelper.ExecuteScalar(strSQL, parameters));
            }
            catch
            {
                totalCount = 0;
            }
            return totalCount;
        }
    }
}