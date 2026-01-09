using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface ICustomerDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<CustomerVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<CustomerVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		CustomerVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<CustomerVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<CustomerVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<CustomerVO> voList);
		
		void UpdateListByParams(List<CustomerVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<CustomerVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<CustomerVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<CustomerVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<CustomerVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<CustomerVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<CustomerVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);

        string ViewAgencyPhone(int customerId, int agencyCustomerId);
        string ViewBusinessPhone(int customerId, int businessCustomerId);
    }
}
