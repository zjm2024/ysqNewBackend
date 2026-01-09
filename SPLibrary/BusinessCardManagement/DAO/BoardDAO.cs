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
    public partial class BoardDAO : CommonDAO, IBoardDAO
    {
        public BoardDAO(UserProfile userProfile)
        {
            base._tableName = "t_bc_board";
            base._pkId = "BoardID";
            base._voType = typeof(BoardVO);
            base.CurrentUserProfile = userProfile;
        }
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<BoardVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<BoardVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<BoardVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<BoardVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public BoardVO FindById(object id)
        {
            return base.FindById<BoardVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<BoardVO> voList)
        {
            base.InsertList<BoardVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<BoardVO> voList, int countInEveryRun)
        {
            base.InsertList<BoardVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<BoardVO> voList)
        {
            base.UpdateListById<BoardVO>(voList);
        }

        public void UpdateListByParams(List<BoardVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<BoardVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<BoardVO> voList, string conditon, List<string> columnList, int countEveryRun)
        {
            base.UpdateListByParams<BoardVO>(voList, conditon, columnList, countEveryRun);
        }

        public void UpdateListByParams(List<BoardVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<BoardVO>(voList, conditon, columnList);
        }

        public void UpdateListByParams(List<BoardVO> voList, string conditon, int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<BoardVO>(voList, conditon, countEveryRun, columnList);
        }

        public void DeleteListByParams(List<BoardVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }

        public void DeleteListByParams(List<BoardVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

        public List<BoardVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<BoardVO>(strSQL, parameters);
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
        public List<BoardVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<BoardVO>(strSQL, parameters);
        }
        public List<BoardVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<BoardVO>(strSQL, parameters);
        }

        public decimal FindTotalSum(string sum, string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select Sum(" + sum + ") FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += condition;
            decimal totalCount = 0;
            try
            {
                totalCount = Convert.ToDecimal(DbHelper.ExecuteScalar(strSQL, parameters));
            }
            catch
            {
                totalCount = 0;
            }
            return totalCount;
        }
    }
}
