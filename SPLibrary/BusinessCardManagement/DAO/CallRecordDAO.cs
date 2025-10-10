using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class CallRecordDAO : CommonDAO, ICallRecordDAO
    { 
        public CallRecordDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_callrecord";
			base._pkId = "RecordID";
			base._voType = typeof(CallRecordVO);
            base.CurrentUserProfile = userProfile;
        }
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CallRecordVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CallRecordVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CallRecordVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CallRecordVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CallRecordVO FindById(object id)
        {
            return base.FindById<CallRecordVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CallRecordVO> voList)
        {
            base.InsertList<CallRecordVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CallRecordVO> voList, int countInEveryRun)
        {
            base.InsertList<CallRecordVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CallRecordVO> voList)
        {
            base.UpdateListById<CallRecordVO>(voList);
        }
        
        public void UpdateListByParams(List<CallRecordVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CallRecordVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CallRecordVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CallRecordVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CallRecordVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CallRecordVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CallRecordVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CallRecordVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CallRecordVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CallRecordVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CallRecordVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CallRecordVO>(strSQL, parameters);
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
        public List<CallRecordVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<CallRecordVO>(strSQL, parameters);
        }
        public List<CallRecordVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc,int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<CallRecordVO>(strSQL, parameters);
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