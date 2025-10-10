using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using CoreFramework.DAO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IShareDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ShareVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<ShareVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        ShareVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<ShareVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ShareVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ShareVO> voList);
		
		void UpdateListByParams(List<ShareVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<ShareVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<ShareVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<ShareVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<ShareVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<ShareVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<ShareVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        List<ShareVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters);

        List<ShareVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
        int Update(string data, string condition, params object[] parameters);
    }
}
