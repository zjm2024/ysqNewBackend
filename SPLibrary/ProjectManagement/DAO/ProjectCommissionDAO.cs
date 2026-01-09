using System;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial class ProjectCommissionDAO:CommonDAO,IProjectCommissionDAO
    {
		public ProjectCommissionDAO(UserProfile userProfile)
		{
			base._tableName="t_bns_projectcommission";
			base._pkId = "ProjectCommissionId";
			base._voType = typeof(ProjectCommissionVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ProjectCommissionVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ProjectCommissionVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ProjectCommissionVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ProjectCommissionVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ProjectCommissionVO FindById(object id)
        {
            return base.FindById<ProjectCommissionVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ProjectCommissionVO> voList)
        {
            base.InsertList<ProjectCommissionVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ProjectCommissionVO> voList, int countInEveryRun)
        {
            base.InsertList<ProjectCommissionVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ProjectCommissionVO> voList)
        {
            base.UpdateListById<ProjectCommissionVO>(voList);
        }
        
        public void UpdateListByParams(List<ProjectCommissionVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ProjectCommissionVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ProjectCommissionVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<ProjectCommissionVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<ProjectCommissionVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ProjectCommissionVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ProjectCommissionVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ProjectCommissionVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<ProjectCommissionVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<ProjectCommissionVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<ProjectCommissionVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ProjectCommissionVO>(strSQL, parameters);
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

        public decimal FindRemainCommission(int projectId)
        {
            string sql = "";
            sql = @"select RemainCommission from (
select p.ProjectId,ifnull(p.Commission,0) - ifnull(pc.ProjectCommission,0) as RemainCommission
from T_BNS_Project p
left join (
select ProjectId,sum(Commission) as ProjectCommission From t_BNS_ProjectCommission where Status = 3
group by ProjectId ) pc on p.projectId = pc.ProjectId
) t  ";

            sql += " \r\n where Projectid = " + projectId;

            object o = DbHelper.ExecuteScalar(sql);
            return Convert.ToDecimal(o);
        }
	}
}