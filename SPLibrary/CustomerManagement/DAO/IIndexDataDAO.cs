using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface IIndexDataDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<IndexDataVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<IndexDataVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        IndexDataVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<IndexDataVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<IndexDataVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<IndexDataVO> voList);
		
		void UpdateListByParams(List<IndexDataVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<IndexDataVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<IndexDataVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<IndexDataVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<IndexDataVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<IndexDataVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<IndexDataVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);

        List<IndexDataVO> FindCardList(string condition, int limit = 0, params object[] parameters);
    }
}
