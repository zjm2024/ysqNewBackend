using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class RankDAO : CommonDAO, IRankDAO
    {

        public RankDAO(UserProfile userProfile)
        {
            base._tableName = "t_rank_lists";
            base._pkId = "rank_list_id";
            base._voType = typeof(RankVO);
            base.CurrentUserProfile = userProfile;
        }

        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<RankVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<RankVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<RankVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<RankVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public RankVO FindById(object id)
        {
            return base.FindById<RankVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<RankVO> voList)
        {
            base.InsertList<RankVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<RankVO> voList, int countInEveryRun)
        {
            base.InsertList<RankVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<RankVO> voList)
        {
            base.UpdateListById<RankVO>(voList);
        }

        public void UpdateListByParams(List<RankVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<RankVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<RankVO> voList, string conditon, List<string> columnList, int countEveryRun)
        {
            base.UpdateListByParams<RankVO>(voList, conditon, columnList, countEveryRun);
        }

        public void UpdateListByParams(List<RankVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<RankVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<RankVO> voList, string conditon, int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<RankVO>(voList, conditon, countEveryRun, columnList);
        }

        public void DeleteListByParams(List<RankVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }

        public void DeleteListByParams(List<RankVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

        public List<RankVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<RankVO>(strSQL, parameters);
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
        public List<RankVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<RankVO>(strSQL, parameters);
        }
        public List<RankVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<RankVO>(strSQL, parameters);
        }
    }
}
