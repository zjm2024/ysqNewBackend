using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using CoreFramework.DAO;
using SPLibrary.LuckyDrawManagement.VO;

namespace SPLibrary.LuckyDrawManagement.DAO
{
    public partial interface ILuckyDrawDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<LuckyDrawVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<LuckyDrawVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        LuckyDrawVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<LuckyDrawVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<LuckyDrawVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<LuckyDrawVO> voList);
		
		void UpdateListByParams(List<LuckyDrawVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<LuckyDrawVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<LuckyDrawVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<LuckyDrawVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<LuckyDrawVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<LuckyDrawVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<LuckyDrawVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
