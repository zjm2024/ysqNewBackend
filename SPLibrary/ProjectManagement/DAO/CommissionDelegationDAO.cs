using System;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial class CommissionDelegationDAO:CommonDAO,ICommissionDelegationDAO
    {
		public CommissionDelegationDAO(UserProfile userProfile)
		{
			base._tableName="t_bns_commissiondelegation";
			base._pkId = "CommissionDelegationId";
			base._voType = typeof(CommissionDelegationVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CommissionDelegationVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CommissionDelegationVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CommissionDelegationVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CommissionDelegationVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CommissionDelegationVO FindById(object id)
        {
            return base.FindById<CommissionDelegationVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CommissionDelegationVO> voList)
        {
            base.InsertList<CommissionDelegationVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CommissionDelegationVO> voList, int countInEveryRun)
        {
            base.InsertList<CommissionDelegationVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CommissionDelegationVO> voList)
        {
            base.UpdateListById<CommissionDelegationVO>(voList);
        }
        
        public void UpdateListByParams(List<CommissionDelegationVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CommissionDelegationVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CommissionDelegationVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CommissionDelegationVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CommissionDelegationVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CommissionDelegationVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CommissionDelegationVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CommissionDelegationVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CommissionDelegationVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CommissionDelegationVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CommissionDelegationVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CommissionDelegationVO>(strSQL, parameters);
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