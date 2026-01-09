using System;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial class CommissionDelegationViewDAO:CommonDAO,ICommissionDelegationViewDAO
    {
		public CommissionDelegationViewDAO(UserProfile userProfile)
		{
			base._tableName="v_commissiondelegationview";
			base._pkId = "CommissionDelegationId";
			base._voType = typeof(CommissionDelegationViewVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CommissionDelegationViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CommissionDelegationViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CommissionDelegationViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CommissionDelegationViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CommissionDelegationViewVO FindById(object id)
        {
            return base.FindById<CommissionDelegationViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CommissionDelegationViewVO> voList)
        {
            base.InsertList<CommissionDelegationViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CommissionDelegationViewVO> voList, int countInEveryRun)
        {
            base.InsertList<CommissionDelegationViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CommissionDelegationViewVO> voList)
        {
            base.UpdateListById<CommissionDelegationViewVO>(voList);
        }
        
        public void UpdateListByParams(List<CommissionDelegationViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CommissionDelegationViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CommissionDelegationViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CommissionDelegationViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CommissionDelegationViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CommissionDelegationViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CommissionDelegationViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CommissionDelegationViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CommissionDelegationViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CommissionDelegationViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CommissionDelegationViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CommissionDelegationViewVO>(strSQL, parameters);
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