using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class HandlePayoutHistoryDAO:CommonDAO,IHandlePayoutHistoryDAO
    {
		public HandlePayoutHistoryDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_handlepayouthistory";
			base._pkId = "HandlepayouthistoryId";
			base._voType = typeof(HandlePayoutHistoryVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<HandlePayoutHistoryVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<HandlePayoutHistoryVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<HandlePayoutHistoryVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<HandlePayoutHistoryVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public HandlePayoutHistoryVO FindById(object id)
        {
            return base.FindById<HandlePayoutHistoryVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<HandlePayoutHistoryVO> voList)
        {
            base.InsertList<HandlePayoutHistoryVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<HandlePayoutHistoryVO> voList, int countInEveryRun)
        {
            base.InsertList<HandlePayoutHistoryVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<HandlePayoutHistoryVO> voList)
        {
            base.UpdateListById<HandlePayoutHistoryVO>(voList);
        }
        
        public void UpdateListByParams(List<HandlePayoutHistoryVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<HandlePayoutHistoryVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<HandlePayoutHistoryVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<HandlePayoutHistoryVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<HandlePayoutHistoryVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<HandlePayoutHistoryVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<HandlePayoutHistoryVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<HandlePayoutHistoryVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<HandlePayoutHistoryVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<HandlePayoutHistoryVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<HandlePayoutHistoryVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<HandlePayoutHistoryVO>(strSQL, parameters);
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