using System;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial class RequireCommissionDelegationviewDAO : CommonDAO, IRequireCommissionDelegationviewDAO
    {
		public RequireCommissionDelegationviewDAO(UserProfile userProfile)
		{
			base._tableName= "v_requirecommissiondelegationview";
			base._pkId = "RequireCommissionDelegationId";
			base._voType = typeof(RequireCommissionDelegationviewVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<RequireCommissionDelegationviewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<RequireCommissionDelegationviewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<RequireCommissionDelegationviewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<RequireCommissionDelegationviewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public RequireCommissionDelegationviewVO FindById(object id)
        {
            return base.FindById<RequireCommissionDelegationviewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<RequireCommissionDelegationviewVO> voList)
        {
            base.InsertList<RequireCommissionDelegationviewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<RequireCommissionDelegationviewVO> voList, int countInEveryRun)
        {
            base.InsertList<RequireCommissionDelegationviewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<RequireCommissionDelegationviewVO> voList)
        {
            base.UpdateListById<RequireCommissionDelegationviewVO>(voList);
        }
        
        public void UpdateListByParams(List<RequireCommissionDelegationviewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<RequireCommissionDelegationviewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<RequireCommissionDelegationviewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<RequireCommissionDelegationviewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<RequireCommissionDelegationviewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<RequireCommissionDelegationviewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<RequireCommissionDelegationviewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<RequireCommissionDelegationviewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<RequireCommissionDelegationviewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<RequireCommissionDelegationviewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<RequireCommissionDelegationviewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<RequireCommissionDelegationviewVO>(strSQL, parameters);
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

        public decimal FindTotalSumCommission(string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select sum(Commission) FROM " + this._tableName + " 	 \n";
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