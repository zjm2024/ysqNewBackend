using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class ActivityDAO : CommonDAO, IActivityDAO
    {
        public ActivityDAO(UserProfile userProfile)
        {
            base._tableName = "t_bc_activity";
            base._pkId = "ActId";
            base._voType = typeof(ActivityVO);
            base.CurrentUserProfile = userProfile;
        }

        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ActivityVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ActivityVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ActivityVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ActivityVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ActivityVO FindById(object id)
        {
            return base.FindById<ActivityVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ActivityVO> voList)
        {
            base.InsertList<ActivityVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ActivityVO> voList, int countInEveryRun)
        {
            base.InsertList<ActivityVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ActivityVO> voList)
        {
            base.UpdateListById<ActivityVO>(voList);
        }

        public void UpdateListByParams(List<ActivityVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ActivityVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<ActivityVO> voList, string conditon, List<string> columnList, int countEveryRun)
        {
            base.UpdateListByParams<ActivityVO>(voList, conditon, columnList, countEveryRun);
        }

        public void UpdateListByParams(List<ActivityVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ActivityVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<ActivityVO> voList, string conditon, int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ActivityVO>(voList, conditon, countEveryRun, columnList);
        }

        public void DeleteListByParams(List<ActivityVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }

        public void DeleteListByParams(List<ActivityVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

        public List<ActivityVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ActivityVO>(strSQL, parameters);
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
        public List<ActivityVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<ActivityVO>(strSQL, parameters);
        }
        public List<ActivityVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<ActivityVO>(strSQL, parameters);
        }
    }
}