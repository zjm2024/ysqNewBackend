using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial interface IRequirementViewDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<RequirementViewVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<RequirementViewVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		RequirementViewVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<RequirementViewVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<RequirementViewVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<RequirementViewVO> voList);
		
		void UpdateListByParams(List<RequirementViewVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<RequirementViewVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<RequirementViewVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<RequirementViewVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<RequirementViewVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<RequirementViewVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<RequirementViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);

        List<RequirementViewVO> FindMatchRequireByPageIndex(int agencyId, string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);
        int FindMatchRequireTotalCount(int agencyId, string condition, params object[] parameters);

        int FindTotalSum(string sum, string condition, params object[] parameters);
    }
}
