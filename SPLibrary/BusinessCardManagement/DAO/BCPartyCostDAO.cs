using System;
using System.Collections.Generic;
using SPLibrary.BusinessCardManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class BCPartyCostDAO : CommonDAO, IBCPartyCostDAO
    { 
        public BCPartyCostDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_partycost";
			base._pkId = "PartyCostID";
			base._voType = typeof(BCPartyCostVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<BCPartyCostVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<BCPartyCostVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<BCPartyCostVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<BCPartyCostVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public BCPartyCostVO FindById(object id)
        {
            return base.FindById<BCPartyCostVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<BCPartyCostVO> voList)
        {
            base.InsertList<BCPartyCostVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<BCPartyCostVO> voList, int countInEveryRun)
        {
            base.InsertList<BCPartyCostVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<BCPartyCostVO> voList)
        {
            base.UpdateListById<BCPartyCostVO>(voList);
        }
        
        public void UpdateListByParams(List<BCPartyCostVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<BCPartyCostVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BCPartyCostVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<BCPartyCostVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<BCPartyCostVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<BCPartyCostVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BCPartyCostVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<BCPartyCostVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<BCPartyCostVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<BCPartyCostVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<BCPartyCostVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<BCPartyCostVO>(strSQL, parameters);
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

        public int Update(string data, string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " UPDATE  " + this._tableName + " 	 \n";
            strSQL += " SET  " + data + "\n";
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