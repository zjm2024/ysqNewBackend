using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class CrmDAO : CommonDAO, ICrmDAO
    {
		public CrmDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_crm";
			base._pkId = "CrmID";
			base._voType = typeof(CrmVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CrmVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CrmVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CrmVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CrmVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CrmVO FindById(object id)
        {
            return base.FindById<CrmVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CrmVO> voList)
        {
            base.InsertList<CrmVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CrmVO> voList, int countInEveryRun)
        {
            base.InsertList<CrmVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CrmVO> voList)
        {
            base.UpdateListById<CrmVO>(voList);
        }
        
        public void UpdateListByParams(List<CrmVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CrmVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CrmVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CrmVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CrmVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CrmVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CrmVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CrmVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CrmVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CrmVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CrmVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CrmVO>(strSQL, parameters);
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
        public List<CrmVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<CrmVO>(strSQL, parameters);
        }
        public List<CrmVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc,int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<CrmVO>(strSQL, parameters);
        }
    }
}