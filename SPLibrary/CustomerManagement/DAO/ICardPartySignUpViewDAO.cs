using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface ICardPartySignUpViewDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<CardPartySignUpViewVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<CardPartySignUpViewVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        CardPartySignUpViewVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<CardPartySignUpViewVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<CardPartySignUpViewVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<CardPartySignUpViewVO> voList);
		
		void UpdateListByParams(List<CardPartySignUpViewVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<CardPartySignUpViewVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<CardPartySignUpViewVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<CardPartySignUpViewVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<CardPartySignUpViewVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<CardPartySignUpViewVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<CardPartySignUpViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);
        List<CardPartySignUpViewVO> FindSignUpViewInviterByCon(string conditionStr);
        int FindTotalCount(string condition, params object[] parameters);

        List<CardPartySignUpViewVO> FindAllByPageIndexByInviter(int PartyID, int start, int end, string sortcolname, string asc, params object[] parameters);
        int FindTotalCountByInviter(int PartyID, params object[] parameters);

        int Update(string data, string condition, params object[] parameters);
    }
}
