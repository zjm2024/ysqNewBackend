using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.LuckyDrawManagement.VO;

namespace SPLibrary.LuckyDrawManagement.DAO
{
    public partial class PrizeDAO : CommonDAO, IPrizeDAO
    {
		public PrizeDAO(UserProfile userProfile)
		{
			base._tableName= "t_ld_prize";
			base._pkId = "PrizeID";
			base._voType = typeof(PrizeVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<PrizeVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<PrizeVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<PrizeVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<PrizeVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public PrizeVO FindById(object id)
        {
            return base.FindById<PrizeVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<PrizeVO> voList)
        {
            base.InsertList<PrizeVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<PrizeVO> voList, int countInEveryRun)
        {
            base.InsertList<PrizeVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<PrizeVO> voList)
        {
            base.UpdateListById<PrizeVO>(voList);
        }
        
        public void UpdateListByParams(List<PrizeVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<PrizeVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<PrizeVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<PrizeVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<PrizeVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<PrizeVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<PrizeVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<PrizeVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<PrizeVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<PrizeVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<PrizeVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<PrizeVO>(strSQL, parameters);
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