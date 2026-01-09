using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class CardBalanceDAO : CommonDAO, ICardBalanceDAO
    {
		public CardBalanceDAO(UserProfile userProfile)
		{
			base._tableName= "t_card_balance";
			base._pkId = "BalanceId";
			base._voType = typeof(CardBalanceVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CardBalanceVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CardBalanceVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CardBalanceVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CardBalanceVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CardBalanceVO FindById(object id)
        {
            return base.FindById<CardBalanceVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CardBalanceVO> voList)
        {
            base.InsertList<CardBalanceVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CardBalanceVO> voList, int countInEveryRun)
        {
            base.InsertList<CardBalanceVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CardBalanceVO> voList)
        {
            base.UpdateListById<CardBalanceVO>(voList);
        }
        
        public void UpdateListByParams(List<CardBalanceVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CardBalanceVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardBalanceVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CardBalanceVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CardBalanceVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CardBalanceVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CardBalanceVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CardBalanceVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CardBalanceVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CardBalanceVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CardBalanceVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CardBalanceVO>(strSQL, parameters);
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

        public bool ReduceBalance(int customerId, decimal balance,int AppType)
        {
           if (balance < 0)
               return false;

           int result = DbHelper.ExecuteNonQuery(" Update " + _tableName + " set Balance = Balance - " + balance + " where CustomerId = " + customerId + " and Balance >= " + balance+ " and AppType="+ AppType);
               return result > 0;
        }

        public bool PlusBalance(int customerId, decimal balance, int AppType)
        {
            if (balance < 0)
                return false;

            // 判断余额表是否存在信息
            if (this.FindByParams("CustomerId = " + customerId).Count < 1)
            {
                //添加余额表
                CardBalanceVO bVO = new CardBalanceVO();
                bVO.CustomerId = customerId;
                bVO.Balance = balance;
                bVO.AppType = AppType;
                return this.Insert(bVO) > 0;
            }
            else
            {
                int result = DbHelper.ExecuteNonQuery(" Update " + _tableName + " set Balance = Balance + " + balance + " where CustomerId = " + customerId+ " and AppType="+ AppType);
                return result > 0;
            }
        }
        public decimal GetTotalBalance(int AppType)
        {
            string strSQL = "SELECT sum(Balance) as TotalBalance FROM t_csc_balance where AppType="+ AppType;

            decimal TotalBalance = 0;
            try
            {
                TotalBalance = Convert.ToDecimal(DbHelper.ExecuteScalar(strSQL));
            }
            catch
            {
                TotalBalance = 0;
            }
            return TotalBalance;
          
        }
    }
}