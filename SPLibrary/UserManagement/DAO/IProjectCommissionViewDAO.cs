using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.UserManagement.DAO
{
    public partial interface IProjectCommissionViewDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<PlatformCommissionViewVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<PlatformCommissionViewVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        PlatformCommissionViewVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<PlatformCommissionViewVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<PlatformCommissionViewVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<PlatformCommissionViewVO> voList);
		
		void UpdateListByParams(List<PlatformCommissionViewVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<PlatformCommissionViewVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<PlatformCommissionViewVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<PlatformCommissionViewVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<PlatformCommissionViewVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<PlatformCommissionViewVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<PlatformCommissionViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
