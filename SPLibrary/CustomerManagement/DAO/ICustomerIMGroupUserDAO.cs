using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface ICustomerIMGroupUserDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<CustomerIMGroupUserVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<CustomerIMGroupUserVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		CustomerIMGroupUserVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<CustomerIMGroupUserVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<CustomerIMGroupUserVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<CustomerIMGroupUserVO> voList);
		
		void UpdateListByParams(List<CustomerIMGroupUserVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<CustomerIMGroupUserVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<CustomerIMGroupUserVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<CustomerIMGroupUserVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<CustomerIMGroupUserVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<CustomerIMGroupUserVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<CustomerIMGroupUserVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
