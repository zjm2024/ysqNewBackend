using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework.WebConfigInfo;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class DepartmentViewDAO:CommonDAO,IDepartmentViewDAO
    {
		public DepartmentViewDAO(UserProfile userProfile)
		{
			base._tableName="V_DepartmentView";
			base._pkId = "DepartmentId";
			base._voType = typeof(DepartmentViewVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<DepartmentViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<DepartmentViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<DepartmentViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<DepartmentViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public DepartmentViewVO FindById(object id)
        {
            return base.FindById<DepartmentViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<DepartmentViewVO> voList)
        {
            base.InsertList<DepartmentViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<DepartmentViewVO> voList, int countInEveryRun)
        {
            base.InsertList<DepartmentViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<DepartmentViewVO> voList)
        {
            base.UpdateListById<DepartmentViewVO>(voList);
        }
        
        public void UpdateListByParams(List<DepartmentViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<DepartmentViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<DepartmentViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<DepartmentViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<DepartmentViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<DepartmentViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<DepartmentViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<DepartmentViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<DepartmentViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<DepartmentViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<DepartmentViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";
            
            strSQL += " SELECT *FROM ( \n";
            strSQL += " select* From V_DepartmentView d \n";
            strSQL += " where exists(select 0 from V_UserSecurity where SecurityTypeCode = N'查看部门数据' and UserId = " + CurrentUserProfile.UserId + ")  \n";
            strSQL += " union \n";
            strSQL += " select d.* From V_DepartmentView d \n";
            strSQL += " left join T_CSC_User u on d.DepartmentId = u.DepartmentId \n";
            strSQL += " where u.UserId = " + CurrentUserProfile.UserId + " \n";
            strSQL += " ) t	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();


            return DbHelper.ExecuteVO<DepartmentViewVO>(strSQL, parameters);
        }

        public int FindTotalCount(string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select Count(0) FROM ( \n";
            strSQL += " select* From V_DepartmentView d \n";
            strSQL += " where exists(select 0 from V_UserSecurity where SecurityTypeCode = N'查看部门数据' and UserId = " + CurrentUserProfile.UserId + ")  \n";
            strSQL += " union \n";
            strSQL += " select d.* From V_DepartmentView d \n";
            strSQL += " left join T_CSC_User u on d.DepartmentId = u.DepartmentId \n";
            strSQL += " where u.UserId = " + CurrentUserProfile.UserId + " \n";
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
	}
}