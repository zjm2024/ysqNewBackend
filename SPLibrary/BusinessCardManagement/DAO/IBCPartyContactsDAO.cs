using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.BusinessCardManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IBCPartyContactsDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<BCPartyContactsVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<BCPartyContactsVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        BCPartyContactsVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<BCPartyContactsVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<BCPartyContactsVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<BCPartyContactsVO> voList);
		
		void UpdateListByParams(List<BCPartyContactsVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<BCPartyContactsVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<BCPartyContactsVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<BCPartyContactsVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<BCPartyContactsVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<BCPartyContactsVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<BCPartyContactsVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
        int Update(string data, string condition, params object[] parameters);
    }
}
