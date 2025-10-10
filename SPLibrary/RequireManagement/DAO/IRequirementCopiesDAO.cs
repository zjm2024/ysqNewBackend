using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial interface IRequirementCopiesDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<RequirementCopiesVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<RequirementCopiesVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		RequirementCopiesVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<RequirementCopiesVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<RequirementCopiesVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<RequirementCopiesVO> voList);
		
		void UpdateListByParams(List<RequirementCopiesVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<RequirementCopiesVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<RequirementCopiesVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<RequirementCopiesVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<RequirementCopiesVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<RequirementCopiesVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<RequirementCopiesVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);

        List<RequirementCopiesVO> FindRequirementCopiesByProjectId( int ProjectId, params object[] parameters);
    }
}
