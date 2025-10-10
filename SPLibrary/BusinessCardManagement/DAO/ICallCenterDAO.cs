using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using CoreFramework.DAO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface ICallCenterDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<CallCenterVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<CallCenterVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        CallCenterVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<CallCenterVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<CallCenterVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<CallCenterVO> voList);
		
		void UpdateListByParams(List<CallCenterVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<CallCenterVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<CallCenterVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<CallCenterVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<CallCenterVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<CallCenterVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<CallCenterVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        List<CallCenterVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters);

        List<CallCenterVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
        decimal FindTotalSum(string sum, string condition, params object[] parameters);
    }
}
