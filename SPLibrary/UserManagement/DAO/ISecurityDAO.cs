using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.UserManagement.DAO
{
    public partial interface ISecurityDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<SecurityVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<SecurityVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		SecurityVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<SecurityVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<SecurityVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<SecurityVO> voList);
		
		void UpdateListByParams(List<SecurityVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<SecurityVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<SecurityVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<SecurityVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<SecurityVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<SecurityVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<SecurityVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
