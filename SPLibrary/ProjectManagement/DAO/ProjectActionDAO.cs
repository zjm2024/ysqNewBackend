using System;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial class ProjectActionDAO:CommonDAO,IProjectActionDAO
    {
		public ProjectActionDAO(UserProfile userProfile)
		{
			base._tableName="t_bns_projectaction";
			base._pkId = "ProjectActionId";
			base._voType = typeof(ProjectActionVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ProjectActionVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ProjectActionVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ProjectActionVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ProjectActionVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ProjectActionVO FindById(object id)
        {
            return base.FindById<ProjectActionVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ProjectActionVO> voList)
        {
            base.InsertList<ProjectActionVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ProjectActionVO> voList, int countInEveryRun)
        {
            base.InsertList<ProjectActionVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ProjectActionVO> voList)
        {
            base.UpdateListById<ProjectActionVO>(voList);
        }
        
        public void UpdateListByParams(List<ProjectActionVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ProjectActionVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ProjectActionVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<ProjectActionVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<ProjectActionVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ProjectActionVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ProjectActionVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ProjectActionVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<ProjectActionVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<ProjectActionVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<ProjectActionVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ProjectActionVO>(strSQL, parameters);
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