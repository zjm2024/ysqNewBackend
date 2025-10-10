using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class CardSoftArticleOrderDAO : CommonDAO, ICardSoftArticleOrderDAO
    { 
        public CardSoftArticleOrderDAO(UserProfile userProfile)
		{
			base._tableName= "t_card_softarticleorder";
			base._pkId = "SoftArticleOrderID";
			base._voType = typeof(CardSoftArticleOrderVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CardSoftArticleOrderVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CardSoftArticleOrderVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CardSoftArticleOrderVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CardSoftArticleOrderVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CardSoftArticleOrderVO FindById(object id)
        {
            return base.FindById<CardSoftArticleOrderVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CardSoftArticleOrderVO> voList)
        {
            base.InsertList<CardSoftArticleOrderVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CardSoftArticleOrderVO> voList, int countInEveryRun)
        {
            base.InsertList<CardSoftArticleOrderVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CardSoftArticleOrderVO> voList)
        {
            base.UpdateListById<CardSoftArticleOrderVO>(voList);
        }
        
        public void UpdateListByParams(List<CardSoftArticleOrderVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CardSoftArticleOrderVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardSoftArticleOrderVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CardSoftArticleOrderVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CardSoftArticleOrderVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CardSoftArticleOrderVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardSoftArticleOrderVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CardSoftArticleOrderVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CardSoftArticleOrderVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CardSoftArticleOrderVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CardSoftArticleOrderVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CardSoftArticleOrderVO>(strSQL, parameters);
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

        public decimal FindTotalSum(string sum,string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select Sum("+ sum + ") FROM " + this._tableName + " 	 \n";
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

        public int Update(string data, string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " UPDATE  " + this._tableName + " 	 \n";
            strSQL += " SET  " + data + "\n";
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