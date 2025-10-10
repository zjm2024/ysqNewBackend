using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.BusinessCardManagement.VO;
using CoreFramework.DAO;
using SPLibrary.CustomerManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IBCPartyOrderViewDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<BCPartyOrderViewVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<BCPartyOrderViewVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        BCPartyOrderViewVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<BCPartyOrderViewVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<BCPartyOrderViewVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<BCPartyOrderViewVO> voList);

        void UpdateListByParams(List<BCPartyOrderViewVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<BCPartyOrderViewVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<BCPartyOrderViewVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<BCPartyOrderViewVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<BCPartyOrderViewVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<BCPartyOrderViewVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<BCPartyOrderViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
        decimal FindSumCost(string condition, params object[] parameters);

        int Update(string data, string condition, params object[] parameters);
    }
}
