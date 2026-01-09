using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using CoreFramework.DAO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface ICallRecordDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<CallRecordVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<CallRecordVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        CallRecordVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<CallRecordVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<CallRecordVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<CallRecordVO> voList);
		
		void UpdateListByParams(List<CallRecordVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<CallRecordVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<CallRecordVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<CallRecordVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<CallRecordVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<CallRecordVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<CallRecordVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        List<CallRecordVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters);

        List<CallRecordVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
        decimal FindTotalSum(string sum, string condition, params object[] parameters);
    }
}
