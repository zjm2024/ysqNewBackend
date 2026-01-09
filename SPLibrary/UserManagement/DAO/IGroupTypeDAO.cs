using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.UserManagement.DAO
{
    public partial interface IGroupTypeDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<GroupTypeVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<GroupTypeVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		GroupTypeVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<GroupTypeVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<GroupTypeVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<GroupTypeVO> voList);
		
		void UpdateListByParams(List<GroupTypeVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<GroupTypeVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<GroupTypeVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<GroupTypeVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<GroupTypeVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<GroupTypeVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<GroupTypeVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
