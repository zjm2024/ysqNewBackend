using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class LogDAO : CommonDAO, ILogDAO
    { 
        public LogDAO(UserProfile userProfile)
		{
			base._tableName= "t_sys_log";
			base._pkId = "LogID";
			base._voType = typeof(LogVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<LogVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<LogVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<LogVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<LogVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public LogVO FindById(object id)
        {
            return base.FindById<LogVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<LogVO> voList)
        {
            base.InsertList<LogVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<LogVO> voList, int countInEveryRun)
        {
            base.InsertList<LogVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<LogVO> voList)
        {
            base.UpdateListById<LogVO>(voList);
        }
        
        public void UpdateListByParams(List<LogVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<LogVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<LogVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<LogVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<LogVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<LogVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<LogVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<LogVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<LogVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<LogVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<LogVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<LogVO>(strSQL, parameters);
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
    }
}