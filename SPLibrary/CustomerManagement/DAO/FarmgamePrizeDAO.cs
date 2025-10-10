using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class FarmgamePrizeDAO : CommonDAO, IFarmgamePrizeDAO
    {
		public FarmgamePrizeDAO(UserProfile userProfile)
		{
			base._tableName= "t_card_farmgame_prize";
			base._pkId = "PrizeID";
			base._voType = typeof(FarmgamePrizeVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<FarmgamePrizeVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<FarmgamePrizeVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<FarmgamePrizeVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<FarmgamePrizeVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public FarmgamePrizeVO FindById(object id)
        {
            return base.FindById<FarmgamePrizeVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<FarmgamePrizeVO> voList)
        {
            base.InsertList<FarmgamePrizeVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<FarmgamePrizeVO> voList, int countInEveryRun)
        {
            base.InsertList<FarmgamePrizeVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<FarmgamePrizeVO> voList)
        {
            base.UpdateListById<FarmgamePrizeVO>(voList);
        }
        
        public void UpdateListByParams(List<FarmgamePrizeVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<FarmgamePrizeVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<FarmgamePrizeVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<FarmgamePrizeVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<FarmgamePrizeVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<FarmgamePrizeVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<FarmgamePrizeVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<FarmgamePrizeVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<FarmgamePrizeVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<FarmgamePrizeVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<FarmgamePrizeVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<FarmgamePrizeVO>(strSQL, parameters);
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

        public List<FarmgamePrizeVO> FindCardList(string condition,int limit=0, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += condition;
            strSQL += " order by DefaultCard desc,CreatedAt asc";

            if (limit > 0)
            {
                strSQL += " limit " + limit;
            }

            return DbHelper.ExecuteVO<FarmgamePrizeVO>(strSQL, parameters);
        }
    }
}