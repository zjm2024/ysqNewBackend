using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface ICardSoftArticleOrderDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<CardSoftArticleOrderVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<CardSoftArticleOrderVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        CardSoftArticleOrderVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<CardSoftArticleOrderVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<CardSoftArticleOrderVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<CardSoftArticleOrderVO> voList);
		
		void UpdateListByParams(List<CardSoftArticleOrderVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<CardSoftArticleOrderVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<CardSoftArticleOrderVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<CardSoftArticleOrderVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<CardSoftArticleOrderVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<CardSoftArticleOrderVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<CardSoftArticleOrderVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
        decimal FindTotalSum(string sum, string condition, params object[] parameters);
        int Update(string data, string condition, params object[] parameters);


    }
}
