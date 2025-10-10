using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface ICustomerRecentLoginViewDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<CustomerRecentLoginViewVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<CustomerRecentLoginViewVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		CustomerRecentLoginViewVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<CustomerRecentLoginViewVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<CustomerRecentLoginViewVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<CustomerRecentLoginViewVO> voList);
		
		void UpdateListByParams(List<CustomerRecentLoginViewVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<CustomerRecentLoginViewVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<CustomerRecentLoginViewVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<CustomerRecentLoginViewVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<CustomerRecentLoginViewVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<CustomerRecentLoginViewVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<CustomerRecentLoginViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
