using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.BusinessCardManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IBCPartySignUpDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<BCPartySignUpVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<BCPartySignUpVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        BCPartySignUpVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<BCPartySignUpVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<BCPartySignUpVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<BCPartySignUpVO> voList);
		
		void UpdateListByParams(List<BCPartySignUpVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<BCPartySignUpVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<BCPartySignUpVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<BCPartySignUpVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<BCPartySignUpVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<BCPartySignUpVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<BCPartySignUpVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
        int Update(string data, string condition, params object[] parameters);
        int FindTotalSum(string sum, string condition, params object[] parameters);
    }
}
