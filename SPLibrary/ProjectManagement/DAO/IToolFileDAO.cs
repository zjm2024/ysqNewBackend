using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial interface IToolFileDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ToolFileVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<ToolFileVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		ToolFileVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<ToolFileVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ToolFileVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ToolFileVO> voList);
		
		void UpdateListByParams(List<ToolFileVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<ToolFileVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<ToolFileVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<ToolFileVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<ToolFileVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<ToolFileVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<ToolFileVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
