using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework.WebConfigInfo;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class UserDAO:CommonDAO,IUserDAO
    {
		public UserDAO(UserProfile userProfile)
		{
			base._tableName="T_CSC_User";
			base._pkId = "UserId";
			base._voType = typeof(UserVO);
            base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<UserVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<UserVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<UserVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<UserVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public UserVO FindById(object id)
        {
            return base.FindById<UserVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<UserVO> voList)
        {
            base.InsertList<UserVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<UserVO> voList, int countInEveryRun)
        {
            base.InsertList<UserVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<UserVO> voList)
        {
            base.UpdateListById<UserVO>(voList);
        }
        
        public void UpdateListByParams(List<UserVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<UserVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<UserVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<UserVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<UserVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<UserVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<UserVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<UserVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<UserVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<UserVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<UserVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();


            return DbHelper.ExecuteVO<UserVO>(strSQL, parameters);
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

        public List<UserVO> FindAllBySecurity()
        {
            string strSQL = "";
            strSQL += " select* from T_CSC_User \n";
            strSQL += " where exists(select 0 from V_UserSecurity where SecurityTypeCode = N'查看公司数据' and UserId = " + CurrentUserProfile.UserId + ") \n";
            strSQL += " union \n";
            strSQL += " select uv.* from T_CSC_User uv \n";
            strSQL += " inner join T_CSC_Department dep on uv.DepartmentId = dep.DepartmentId \n";
            strSQL += " where exists( select 0 from V_UserSecurity where SecurityTypeCode = N'查看部门数据' and UserId = " + CurrentUserProfile.UserId + ") and dep.DepartmentId = " + CurrentUserProfile.DeaprtmentId + " \n";
            strSQL += " union \n";
            strSQL += " select uv.* from T_CSC_User uv \n";
            strSQL += " where uv.UserId = " + CurrentUserProfile.UserId + " \n";

            return DbHelper.ExecuteVO<UserVO>(strSQL);
        }
    }
}