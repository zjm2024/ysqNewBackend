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
    public partial class RankItemDAO : CommonDAO, IRankItemDAO
    {

        public RankItemDAO(UserProfile userProfile)
        {
            base._tableName = "t_rank_items";
            base._pkId = "rank_items_id";
            base._voType = typeof(RankItemVO);
            base.CurrentUserProfile = userProfile; 
        }

        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<RankItemVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<RankItemVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<RankItemVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<RankItemVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public RankItemVO FindById(object id)
        {
            return base.FindById<RankItemVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<RankItemVO> voList)
        {
            base.InsertList<RankItemVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<RankItemVO> voList, int countInEveryRun)
        {
            base.InsertList<RankItemVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<RankItemVO> voList)
        {
            base.UpdateListById<RankItemVO>(voList);
        }

        public void UpdateListByParams(List<RankItemVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<RankItemVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<RankItemVO> voList, string conditon, List<string> columnList, int countEveryRun)
        {
            base.UpdateListByParams<RankItemVO>(voList, conditon, columnList, countEveryRun);
        }

        public void UpdateListByParams(List<RankItemVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<RankItemVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<RankItemVO> voList, string conditon, int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<RankItemVO>(voList, conditon, countEveryRun, columnList);
        }

        public void DeleteListByParams(List<RankItemVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }

        public void DeleteListByParams(List<RankItemVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

        public List<RankItemVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<RankItemVO>(strSQL, parameters);
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
        public List<RankItemVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<RankItemVO>(strSQL, parameters);
        }
        public List<RankItemVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<RankItemVO>(strSQL, parameters);
        }
    }
}
