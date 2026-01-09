using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class SecurityActionDAO:CommonDAO,ISecurityActionDAO
    {
		public SecurityActionDAO(UserProfile userProfile)
		{
			base._tableName="T_CSC_SecurityAction";
			base._pkId = "SecurityActionId";
			base._voType = typeof(SecurityActionVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<SecurityActionVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<SecurityActionVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<SecurityActionVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<SecurityActionVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public SecurityActionVO FindById(object id)
        {
            return base.FindById<SecurityActionVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<SecurityActionVO> voList)
        {
            base.InsertList<SecurityActionVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<SecurityActionVO> voList, int countInEveryRun)
        {
            base.InsertList<SecurityActionVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<SecurityActionVO> voList)
        {
            base.UpdateListById<SecurityActionVO>(voList);
        }
        
        public void UpdateListByParams(List<SecurityActionVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<SecurityActionVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<SecurityActionVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<SecurityActionVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<SecurityActionVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<SecurityActionVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<SecurityActionVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<SecurityActionVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<SecurityActionVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<SecurityActionVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<SecurityActionVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();


            return DbHelper.ExecuteVO<SecurityActionVO>(strSQL, parameters);
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