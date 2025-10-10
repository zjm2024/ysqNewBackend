using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class CardAgentFinanceDAO : CommonDAO, ICardAgentFinanceDAO
    {
		public CardAgentFinanceDAO(UserProfile userProfile)
		{
			base._tableName= "t_card_agentfinance";
			base._pkId = "FinanceID";
			base._voType = typeof(CardAgentFinanceVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CardAgentFinanceVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CardAgentFinanceVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CardAgentFinanceVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CardAgentFinanceVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CardAgentFinanceVO FindById(object id)
        {
            return base.FindById<CardAgentFinanceVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CardAgentFinanceVO> voList)
        {
            base.InsertList<CardAgentFinanceVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CardAgentFinanceVO> voList, int countInEveryRun)
        {
            base.InsertList<CardAgentFinanceVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CardAgentFinanceVO> voList)
        {
            base.UpdateListById<CardAgentFinanceVO>(voList);
        }
        
        public void UpdateListByParams(List<CardAgentFinanceVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CardAgentFinanceVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardAgentFinanceVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CardAgentFinanceVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CardAgentFinanceVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CardAgentFinanceVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardAgentFinanceVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CardAgentFinanceVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CardAgentFinanceVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CardAgentFinanceVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CardAgentFinanceVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CardAgentFinanceVO>(strSQL, parameters);
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