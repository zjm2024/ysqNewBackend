using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class UserRoleDAO:CommonDAO,IUserRoleDAO
    {
		public UserRoleDAO(UserProfile userProfile)
		{
			base._tableName="T_CSC_UserRole";
			base._pkId = "UserRoleId";
			base._voType = typeof(UserRoleVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<UserRoleVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<UserRoleVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<UserRoleVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<UserRoleVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public UserRoleVO FindById(object id)
        {
            return base.FindById<UserRoleVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<UserRoleVO> voList)
        {
            base.InsertList<UserRoleVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<UserRoleVO> voList, int countInEveryRun)
        {
            base.InsertList<UserRoleVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<UserRoleVO> voList)
        {
            base.UpdateListById<UserRoleVO>(voList);
        }
        
        public void UpdateListByParams(List<UserRoleVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<UserRoleVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<UserRoleVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<UserRoleVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<UserRoleVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<UserRoleVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<UserRoleVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<UserRoleVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<UserRoleVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<UserRoleVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<UserRoleVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();


            return DbHelper.ExecuteVO<UserRoleVO>(strSQL, parameters);
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