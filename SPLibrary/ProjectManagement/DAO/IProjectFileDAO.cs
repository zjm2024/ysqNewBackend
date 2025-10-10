using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial interface IProjectFileDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ProjectFileVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<ProjectFileVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		ProjectFileVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<ProjectFileVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ProjectFileVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ProjectFileVO> voList);
		
		void UpdateListByParams(List<ProjectFileVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<ProjectFileVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<ProjectFileVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<ProjectFileVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<ProjectFileVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<ProjectFileVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<ProjectFileVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
