using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework.WebConfigInfo;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class UserViewDAO:CommonDAO,IUserViewDAO
    {
		public UserViewDAO(UserProfile userProfile)
		{
			base._tableName="V_UserView";
			base._pkId = "UserId";
			base._voType = typeof(UserViewVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<UserViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<UserViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<UserViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<UserViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public UserViewVO FindById(object id)
        {
            return base.FindById<UserViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<UserViewVO> voList)
        {
            base.InsertList<UserViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<UserViewVO> voList, int countInEveryRun)
        {
            base.InsertList<UserViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<UserViewVO> voList)
        {
            base.UpdateListById<UserViewVO>(voList);
        }
        
        public void UpdateListByParams(List<UserViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<UserViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<UserViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<UserViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<UserViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<UserViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<UserViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<UserViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<UserViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<UserViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

        public List<UserViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";
            
            strSQL += " SELECT * FROM ( \n";
            strSQL += " select* from V_UserView \n";
            strSQL += " where exists(select 0 from V_UserSecurity where SecurityTypeCode = N'查看公司数据' and UserId = " + CurrentUserProfile.UserId + ") \n";
            strSQL += " union \n";
            strSQL += " select uv.* from V_UserView uv \n";
            strSQL += " inner join T_CSC_Department dep on uv.DepartmentId = dep.DepartmentId \n";
            strSQL += " where exists( select 0 from V_UserSecurity where SecurityTypeCode = N'查看部门数据' and UserId = " + CurrentUserProfile.UserId + ") \n";
            strSQL += " union \n";
            strSQL += " select uv.* from V_UserView uv \n";
            strSQL += " where uv.UserId = " + CurrentUserProfile.UserId + " \n";
            strSQL += " ) t	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();


            return DbHelper.ExecuteVO<UserViewVO>(strSQL, parameters);
        }

        public int FindTotalCount(string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select Count(0) FROM (	 \n";
            strSQL += " select* from V_UserView \n";
            strSQL += " where exists(select 0 from V_UserSecurity where SecurityTypeCode = N'查看公司数据' and UserId = " + CurrentUserProfile.UserId + ") \n";
            strSQL += " union \n";
            strSQL += " select uv.* from V_UserView uv \n";
            strSQL += " inner join T_CSC_Department dep on uv.DepartmentId = dep.DepartmentId \n";
            strSQL += " where exists( select 0 from V_UserSecurity where SecurityTypeCode = N'查看部门数据' and UserId = " + CurrentUserProfile.UserId + ") \n";
            strSQL += " union \n";
            strSQL += " select uv.* from V_UserView uv \n";
            strSQL += " where uv.UserId = " + CurrentUserProfile.UserId + " \n";
            strSQL += " ) t	 \n";
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




        public List<UserViewVO> FindAllByPageIndex1(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

   
            strSQL += " select * from V_UserView \n";
         
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();


            return DbHelper.ExecuteVO<UserViewVO>(strSQL, parameters);
        }

        public int FindTotalCount1(string condition, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " select Count(0) from V_UserView \n";
         
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