using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface IBalanceDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<BalanceVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<BalanceVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		BalanceVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<BalanceVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<BalanceVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<BalanceVO> voList);
		
		void UpdateListByParams(List<BalanceVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<BalanceVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<BalanceVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<BalanceVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<BalanceVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<BalanceVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<BalanceVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);

        bool ReduceBalance(int customerId, decimal balance);

        bool PlusBalance(int customerId, decimal balance);
        decimal GetTotalBalance();
    }
}
