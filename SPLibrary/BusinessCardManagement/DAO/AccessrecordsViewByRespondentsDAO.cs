using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class AccessrecordsViewByRespondentsDAO : CommonDAO, IAccessrecordsViewByRespondentsDAO
    {
		public AccessrecordsViewByRespondentsDAO(UserProfile userProfile)
		{
			base._tableName= "v_bcaccessrecordsviewbyrespondents";
			base._pkId = "AccessRecordsID";
			base._voType = typeof(AccessrecordsViewByRespondentsVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<AccessrecordsViewByRespondentsVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<AccessrecordsViewByRespondentsVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<AccessrecordsViewByRespondentsVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<AccessrecordsViewByRespondentsVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public AccessrecordsViewByRespondentsVO FindById(object id)
        {
            return base.FindById<AccessrecordsViewByRespondentsVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<AccessrecordsViewByRespondentsVO> voList)
        {
            base.InsertList<AccessrecordsViewByRespondentsVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<AccessrecordsViewByRespondentsVO> voList, int countInEveryRun)
        {
            base.InsertList<AccessrecordsViewByRespondentsVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<AccessrecordsViewByRespondentsVO> voList)
        {
            base.UpdateListById<AccessrecordsViewByRespondentsVO>(voList);
        }
        
        public void UpdateListByParams(List<AccessrecordsViewByRespondentsVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<AccessrecordsViewByRespondentsVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AccessrecordsViewByRespondentsVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<AccessrecordsViewByRespondentsVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<AccessrecordsViewByRespondentsVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<AccessrecordsViewByRespondentsVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AccessrecordsViewByRespondentsVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<AccessrecordsViewByRespondentsVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<AccessrecordsViewByRespondentsVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<AccessrecordsViewByRespondentsVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<AccessrecordsViewByRespondentsVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<AccessrecordsViewByRespondentsVO>(strSQL, parameters);
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

        public List<AccessrecordsViewByRespondentsVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<AccessrecordsViewByRespondentsVO>(strSQL, parameters);
        }
        public List<AccessrecordsViewByRespondentsVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc,int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<AccessrecordsViewByRespondentsVO>(strSQL, parameters);
        }
    }
}