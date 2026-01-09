using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial interface IServicesDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ServicesVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<ServicesVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		ServicesVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<ServicesVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ServicesVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ServicesVO> voList);
		
		void UpdateListByParams(List<ServicesVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<ServicesVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<ServicesVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<ServicesVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<ServicesVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<ServicesVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<ServicesVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
