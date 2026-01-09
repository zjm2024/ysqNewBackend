using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface IFarmgamePrizeDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<FarmgamePrizeVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<FarmgamePrizeVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        FarmgamePrizeVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<FarmgamePrizeVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<FarmgamePrizeVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<FarmgamePrizeVO> voList);
		
		void UpdateListByParams(List<FarmgamePrizeVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<FarmgamePrizeVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<FarmgamePrizeVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<FarmgamePrizeVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<FarmgamePrizeVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<FarmgamePrizeVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<FarmgamePrizeVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);

        List<FarmgamePrizeVO> FindCardList(string condition, int limit = 0, params object[] parameters);
    }
}
