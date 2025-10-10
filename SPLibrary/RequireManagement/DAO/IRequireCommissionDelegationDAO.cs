using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial interface IRequireCommissionDelegationDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<RequireCommissionDelegationVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<RequireCommissionDelegationVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		RequireCommissionDelegationVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<RequireCommissionDelegationVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<RequireCommissionDelegationVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<RequireCommissionDelegationVO> voList);
		
		void UpdateListByParams(List<RequireCommissionDelegationVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<RequireCommissionDelegationVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<RequireCommissionDelegationVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<RequireCommissionDelegationVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<RequireCommissionDelegationVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<RequireCommissionDelegationVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<RequireCommissionDelegationVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
