using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial interface IServicesCategoryDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ServicesCategoryVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<ServicesCategoryVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		ServicesCategoryVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<ServicesCategoryVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ServicesCategoryVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ServicesCategoryVO> voList);
		
		void UpdateListByParams(List<ServicesCategoryVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<ServicesCategoryVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<ServicesCategoryVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<ServicesCategoryVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<ServicesCategoryVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<ServicesCategoryVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<ServicesCategoryVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
