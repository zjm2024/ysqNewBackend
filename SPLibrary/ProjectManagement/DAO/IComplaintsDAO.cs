using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial interface IComplaintsDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ComplaintsVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<ComplaintsVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		ComplaintsVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<ComplaintsVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ComplaintsVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ComplaintsVO> voList);
		
		void UpdateListByParams(List<ComplaintsVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<ComplaintsVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<ComplaintsVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<ComplaintsVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<ComplaintsVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<ComplaintsVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<ComplaintsVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
