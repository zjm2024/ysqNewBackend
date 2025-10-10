using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial interface ITenderInfoRequirementViewDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<TenderInfoRequirementViewVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<TenderInfoRequirementViewVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		TenderInfoRequirementViewVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<TenderInfoRequirementViewVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<TenderInfoRequirementViewVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<TenderInfoRequirementViewVO> voList);
		
		void UpdateListByParams(List<TenderInfoRequirementViewVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<TenderInfoRequirementViewVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<TenderInfoRequirementViewVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<TenderInfoRequirementViewVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<TenderInfoRequirementViewVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<TenderInfoRequirementViewVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<TenderInfoRequirementViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
