using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface IFarmGameTaskDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<FarmGameTaskVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<FarmGameTaskVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        FarmGameTaskVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<FarmGameTaskVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<FarmGameTaskVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<FarmGameTaskVO> voList);
		
		void UpdateListByParams(List<FarmGameTaskVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<FarmGameTaskVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<FarmGameTaskVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<FarmGameTaskVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<FarmGameTaskVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<FarmGameTaskVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<FarmGameTaskVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);

        List<FarmGameTaskVO> FindCardList(string condition, int limit = 0, params object[] parameters);
    }
}
