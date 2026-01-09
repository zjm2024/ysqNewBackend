using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using System.Data;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class UserSecurityViewDAO:CommonDAO,IUserSecurityViewDAO
    {
		public UserSecurityViewDAO(UserProfile userProfile)
		{
			base._tableName="V_UserSecurity";
			base._pkId = "UserId";
			base._voType = typeof(UserSecurityViewVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<UserSecurityViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<UserSecurityViewVO>(condtion, dbParameters);
        }

        public DataTable FindDataTableByParams(string condtion, params object[] dbParameters)
        {
            string sql = "Select * from V_UserSecurity where 1=1 and " + condtion + " order by SortNumber asc";
            return DbHelper.ExecuteDataTable(sql, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<UserSecurityViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<UserSecurityViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public UserSecurityViewVO FindById(object id)
        {
            return base.FindById<UserSecurityViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<UserSecurityViewVO> voList)
        {
            base.InsertList<UserSecurityViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<UserSecurityViewVO> voList, int countInEveryRun)
        {
            base.InsertList<UserSecurityViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<UserSecurityViewVO> voList)
        {
            base.UpdateListById<UserSecurityViewVO>(voList);
        }
        
        public void UpdateListByParams(List<UserSecurityViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<UserSecurityViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<UserSecurityViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<UserSecurityViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<UserSecurityViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<UserSecurityViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<UserSecurityViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<UserSecurityViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<UserSecurityViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<UserSecurityViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<UserSecurityViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();


            return DbHelper.ExecuteVO<UserSecurityViewVO>(strSQL, parameters);
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