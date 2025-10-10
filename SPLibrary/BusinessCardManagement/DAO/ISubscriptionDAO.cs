using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using CoreFramework.DAO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface ISubscriptionDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<SubscriptionVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<SubscriptionVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        SubscriptionVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<SubscriptionVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<SubscriptionVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<SubscriptionVO> voList);
		
		void UpdateListByParams(List<SubscriptionVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<SubscriptionVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<SubscriptionVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<SubscriptionVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<SubscriptionVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<SubscriptionVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<SubscriptionVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        List<SubscriptionVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters);

        List<SubscriptionVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
