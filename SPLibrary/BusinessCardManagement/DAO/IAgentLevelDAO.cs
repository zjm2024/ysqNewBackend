using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using CoreFramework.DAO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IAgentLevelDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<AgentLevelVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<AgentLevelVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        AgentLevelVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<AgentLevelVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<AgentLevelVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<AgentLevelVO> voList);
		
		void UpdateListByParams(List<AgentLevelVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<AgentLevelVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<AgentLevelVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<AgentLevelVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<AgentLevelVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<AgentLevelVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<AgentLevelVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        List<AgentLevelVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters);

        List<AgentLevelVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
