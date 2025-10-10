using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface ICardOrderViewDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<CardOrderViewVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<CardOrderViewVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        CardOrderViewVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<CardOrderViewVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<CardOrderViewVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<CardOrderViewVO> voList);
		
		void UpdateListByParams(List<CardOrderViewVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<CardOrderViewVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<CardOrderViewVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<CardOrderViewVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<CardOrderViewVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<CardOrderViewVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<CardOrderViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);
        List<CardOrderViewVO> FindAllByPageIndex(string conditionStr, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
        decimal FindTotalSum(string sum, string condition, params object[] parameters);
        int Update(string data, string condition, params object[] parameters);
    }
}
