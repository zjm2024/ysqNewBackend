using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class PunchDAO : CommonDAO, IPunchDAO
    {
		public PunchDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_punch";
			base._pkId = "PunchID";
			base._voType = typeof(PunchVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<PunchVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<PunchVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<PunchVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<PunchVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public PunchVO FindById(object id)
        {
            return base.FindById<PunchVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<PunchVO> voList)
        {
            base.InsertList<PunchVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<PunchVO> voList, int countInEveryRun)
        {
            base.InsertList<PunchVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<PunchVO> voList)
        {
            base.UpdateListById<PunchVO>(voList);
        }
        
        public void UpdateListByParams(List<PunchVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<PunchVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<PunchVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<PunchVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<PunchVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<PunchVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<PunchVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<PunchVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<PunchVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<PunchVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<PunchVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<PunchVO>(strSQL, parameters);
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
        public List<PunchVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<PunchVO>(strSQL, parameters);
        }
        public List<PunchVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc,int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<PunchVO>(strSQL, parameters);
        }
    }
}