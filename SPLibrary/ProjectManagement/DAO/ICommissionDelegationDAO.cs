using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial interface ICommissionDelegationDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<CommissionDelegationVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<CommissionDelegationVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		CommissionDelegationVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<CommissionDelegationVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<CommissionDelegationVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<CommissionDelegationVO> voList);
		
		void UpdateListByParams(List<CommissionDelegationVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<CommissionDelegationVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<CommissionDelegationVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<CommissionDelegationVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<CommissionDelegationVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<CommissionDelegationVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<CommissionDelegationVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
