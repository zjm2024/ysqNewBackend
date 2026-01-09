using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.UserManagement.DAO
{
    public partial interface IConfigDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ConfigVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<ConfigVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		ConfigVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<ConfigVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ConfigVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ConfigVO> voList);
		
		void UpdateListByParams(List<ConfigVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<ConfigVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<ConfigVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<ConfigVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<ConfigVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<ConfigVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<ConfigVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
