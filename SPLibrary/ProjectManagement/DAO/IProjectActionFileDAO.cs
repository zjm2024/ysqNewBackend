using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial interface IProjectActionFileDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ProjectActionFileVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<ProjectActionFileVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		ProjectActionFileVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<ProjectActionFileVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ProjectActionFileVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ProjectActionFileVO> voList);
		
		void UpdateListByParams(List<ProjectActionFileVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<ProjectActionFileVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<ProjectActionFileVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<ProjectActionFileVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<ProjectActionFileVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<ProjectActionFileVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<ProjectActionFileVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
