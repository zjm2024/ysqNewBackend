using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.BusinessCardManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IBCPartyViewDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<BCPartyViewVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<BCPartyViewVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        BCPartyViewVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<BCPartyViewVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<BCPartyViewVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<BCPartyViewVO> voList);
		
		void UpdateListByParams(List<BCPartyViewVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<BCPartyViewVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<BCPartyViewVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<BCPartyViewVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<BCPartyViewVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<BCPartyViewVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<BCPartyViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);
        List<BCPartyViewVO> FindSignUpViewInviterByCon(string conditionStr);
        int FindTotalCount(string condition, params object[] parameters);

        List<BCPartyViewVO> FindAllByPageIndexByInviter(int PartyID, int start, int end, string sortcolname, string asc, params object[] parameters);
        int FindTotalCountByInviter(int PartyID, params object[] parameters);

        int Update(string data, string condition, params object[] parameters);
    }
}
