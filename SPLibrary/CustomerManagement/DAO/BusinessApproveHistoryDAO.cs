using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class BusinessApproveHistoryDAO:CommonDAO,IBusinessApproveHistoryDAO
    {
		public BusinessApproveHistoryDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_businessapprovehistory";
			base._pkId = "BusinessApproveHistoryId";
			base._voType = typeof(BusinessApproveHistoryVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<BusinessApproveHistoryVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<BusinessApproveHistoryVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<BusinessApproveHistoryVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<BusinessApproveHistoryVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public BusinessApproveHistoryVO FindById(object id)
        {
            return base.FindById<BusinessApproveHistoryVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<BusinessApproveHistoryVO> voList)
        {
            base.InsertList<BusinessApproveHistoryVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<BusinessApproveHistoryVO> voList, int countInEveryRun)
        {
            base.InsertList<BusinessApproveHistoryVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<BusinessApproveHistoryVO> voList)
        {
            base.UpdateListById<BusinessApproveHistoryVO>(voList);
        }
        
        public void UpdateListByParams(List<BusinessApproveHistoryVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<BusinessApproveHistoryVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BusinessApproveHistoryVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<BusinessApproveHistoryVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<BusinessApproveHistoryVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<BusinessApproveHistoryVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BusinessApproveHistoryVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<BusinessApproveHistoryVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<BusinessApproveHistoryVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<BusinessApproveHistoryVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<BusinessApproveHistoryVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<BusinessApproveHistoryVO>(strSQL, parameters);
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

        public BusinessApproveHistoryVO FindLatestApprove(int businessId)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where BusinessId = " + businessId + "  \n";
            strSQL += " order by ApproveDate desc";

            List< BusinessApproveHistoryVO> voList = DbHelper.ExecuteVO<BusinessApproveHistoryVO>(strSQL);
            if (voList.Count > 0)
                return voList[0];
            else
                return null;
        }
    }
}