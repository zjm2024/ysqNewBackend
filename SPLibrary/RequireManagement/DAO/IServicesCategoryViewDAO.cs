using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial interface IServicesCategoryViewDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ServicesCategoryViewVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<ServicesCategoryViewVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		ServicesCategoryViewVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<ServicesCategoryViewVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ServicesCategoryViewVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ServicesCategoryViewVO> voList);
		
		void UpdateListByParams(List<ServicesCategoryViewVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<ServicesCategoryViewVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<ServicesCategoryViewVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<ServicesCategoryViewVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<ServicesCategoryViewVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<ServicesCategoryViewVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<ServicesCategoryViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
