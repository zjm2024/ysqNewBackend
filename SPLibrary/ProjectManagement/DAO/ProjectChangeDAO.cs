using System;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial class ProjectChangeDAO : CommonDAO, IProjectChangeDAO
    {
        public ProjectChangeDAO(UserProfile userProfile)
        {
            base._tableName = "t_bns_projectchange";
            base._pkId = "ProjectChangeId";
            base._voType = typeof(ProjectChangeVO);
            base.CurrentUserProfile = userProfile;
        }

        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ProjectChangeVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ProjectChangeVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ProjectChangeVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ProjectChangeVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ProjectChangeVO FindById(object id)
        {
            return base.FindById<ProjectChangeVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ProjectChangeVO> voList)
        {
            base.InsertList<ProjectChangeVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ProjectChangeVO> voList, int countInEveryRun)
        {
            base.InsertList<ProjectChangeVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ProjectChangeVO> voList)
        {
            base.UpdateListById<ProjectChangeVO>(voList);
        }

        public void UpdateListByParams(List<ProjectChangeVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ProjectChangeVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<ProjectChangeVO> voList, string conditon, List<string> columnList, int countEveryRun)
        {
            base.UpdateListByParams<ProjectChangeVO>(voList, conditon, columnList, countEveryRun);
        }

        public void UpdateListByParams(List<ProjectChangeVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ProjectChangeVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<ProjectChangeVO> voList, string conditon, int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ProjectChangeVO>(voList, conditon, countEveryRun, columnList);
        }

        public void DeleteListByParams(List<ProjectChangeVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }

        public void DeleteListByParams(List<ProjectChangeVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

        public List<ProjectChangeVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ProjectChangeVO>(strSQL, parameters);
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

        public List<ProjectChangeVO> FindAllByPage(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<ProjectChangeVO>(strSQL, parameters);
        }
    }
}
