using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface IAgencyViewDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<AgencyViewVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<AgencyViewVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		AgencyViewVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<AgencyViewVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<AgencyViewVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<AgencyViewVO> voList);
		
		void UpdateListByParams(List<AgencyViewVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<AgencyViewVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<AgencyViewVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<AgencyViewVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<AgencyViewVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<AgencyViewVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<AgencyViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);

        List<BusinessViewVO> FindAllMyBusinessByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindMyBusinessTotalCount(string condition, params object[] parameters);

        List<AgencyViewVO> FindMatchAgencyByPageIndex(int requireId, string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);
                int FindMatchAgencyTotalCount(int requireId, string condition, params object[] parameters);
    }
}
