using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using CoreFramework.DAO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IRecordingRecordsDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<RecordingRecordsVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<RecordingRecordsVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        RecordingRecordsVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<RecordingRecordsVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<RecordingRecordsVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<RecordingRecordsVO> voList);
		
		void UpdateListByParams(List<RecordingRecordsVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<RecordingRecordsVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<RecordingRecordsVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<RecordingRecordsVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<RecordingRecordsVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<RecordingRecordsVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<RecordingRecordsVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        List<RecordingRecordsVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters);

        List<RecordingRecordsVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
