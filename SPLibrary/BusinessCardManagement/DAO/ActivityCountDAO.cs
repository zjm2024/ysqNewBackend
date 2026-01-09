using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class ActivityCountDAO : CommonDAO, IActivityCountDAO
    {
        public ActivityCountDAO(UserProfile userProfile)
        {
            base._tableName = "t_bc_activitycount";
            base._pkId = "ActCountId";
            base._voType = typeof(ActivityCountVO);
            base.CurrentUserProfile = userProfile;
        }

        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ActivityCountVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ActivityCountVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ActivityCountVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ActivityCountVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ActivityCountVO FindById(object id)
        {
            return base.FindById<ActivityCountVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ActivityCountVO> voList)
        {
            base.InsertList<ActivityCountVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ActivityCountVO> voList, int countInEveryRun)
        {
            base.InsertList<ActivityCountVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ActivityCountVO> voList)
        {
            base.UpdateListById<ActivityCountVO>(voList);
        }

        public void UpdateListByParams(List<ActivityCountVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ActivityCountVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<ActivityCountVO> voList, string conditon, List<string> columnList, int countEveryRun)
        {
            base.UpdateListByParams<ActivityCountVO>(voList, conditon, columnList, countEveryRun);
        }

        public void UpdateListByParams(List<ActivityCountVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ActivityCountVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<ActivityCountVO> voList, string conditon, int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ActivityCountVO>(voList, conditon, countEveryRun, columnList);
        }

        public void DeleteListByParams(List<ActivityCountVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }

        public void DeleteListByParams(List<ActivityCountVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

        public List<ActivityCountVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ActivityCountVO>(strSQL, parameters);
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
        public List<ActivityCountVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<ActivityCountVO>(strSQL, parameters);
        }
        public List<ActivityCountVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<ActivityCountVO>(strSQL, parameters);
        }
    }
}
