using System;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial class ProjectDAO:CommonDAO,IProjectDAO
    {
		public ProjectDAO(UserProfile userProfile)
		{
			base._tableName="t_bns_project";
			base._pkId = "ProjectId";
			base._voType = typeof(ProjectVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ProjectVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ProjectVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ProjectVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ProjectVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ProjectVO FindById(object id)
        {
            return base.FindById<ProjectVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ProjectVO> voList)
        {
            base.InsertList<ProjectVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ProjectVO> voList, int countInEveryRun)
        {
            base.InsertList<ProjectVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ProjectVO> voList)
        {
            base.UpdateListById<ProjectVO>(voList);
        }
        
        public void UpdateListByParams(List<ProjectVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ProjectVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ProjectVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<ProjectVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<ProjectVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ProjectVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ProjectVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ProjectVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<ProjectVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<ProjectVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<ProjectVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ProjectVO>(strSQL, parameters);
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

        public decimal FindPlatformCommission(int projectId)
        {
            string sql = "";
            sql = @"select sum(ifnull(PlatformCommission,0)) as PlatformCommission from T_BNS_Commissiondelegation ";

            sql += " \r\n where Projectid = " + projectId;

            object o = DbHelper.ExecuteScalar(sql);
            return Convert.ToDecimal(o);
        }
	}
}