using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class BalanceDAO:CommonDAO,IBalanceDAO
    {
		public BalanceDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_balance";
			base._pkId = "BalanceId";
			base._voType = typeof(BalanceVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<BalanceVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<BalanceVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<BalanceVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<BalanceVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public BalanceVO FindById(object id)
        {
            return base.FindById<BalanceVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<BalanceVO> voList)
        {
            base.InsertList<BalanceVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<BalanceVO> voList, int countInEveryRun)
        {
            base.InsertList<BalanceVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<BalanceVO> voList)
        {
            base.UpdateListById<BalanceVO>(voList);
        }
        
        public void UpdateListByParams(List<BalanceVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<BalanceVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BalanceVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<BalanceVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<BalanceVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<BalanceVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BalanceVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<BalanceVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<BalanceVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<BalanceVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<BalanceVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<BalanceVO>(strSQL, parameters);
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

        public bool ReduceBalance(int customerId, decimal balance)
        {
           int result = DbHelper.ExecuteNonQuery(" Update " + _tableName + " set Balance = Balance - " + balance + " where CustomerId = " + customerId + " and Balance >= " + balance);
            return result > 0;
        }

        public bool PlusBalance(int customerId, decimal balance)
        {
            // 判断余额表是否存在信息
            if (this.FindByParams("CustomerId = " + customerId).Count < 1)
            {
                //添加余额表
                BalanceVO bVO = new BalanceVO();
                bVO.CustomerId = customerId;
                bVO.Balance = balance;
                return this.Insert(bVO) > 0;
            }
            else
            {
                int result = DbHelper.ExecuteNonQuery(" Update " + _tableName + " set Balance = Balance + " + balance + " where CustomerId = " + customerId);
                return result > 0;
            }

        }
        public decimal GetTotalBalance()
        {
            string strSQL = "SELECT sum(Balance) as TotalBalance FROM t_csc_balance";

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