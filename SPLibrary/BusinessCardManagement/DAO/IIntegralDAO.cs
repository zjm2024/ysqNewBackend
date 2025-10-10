using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using CoreFramework.DAO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IIntegralDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<IntegralVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<IntegralVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        IntegralVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<IntegralVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<IntegralVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<IntegralVO> voList);
		
		void UpdateListByParams(List<IntegralVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<IntegralVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<IntegralVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<IntegralVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<IntegralVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<IntegralVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<IntegralVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        List<IntegralVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters);

        List<IntegralVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
        int Update(string data, string condition, params object[] parameters);
        decimal FindTotalSum(string sum, string condition, params object[] parameters);
    }
}
