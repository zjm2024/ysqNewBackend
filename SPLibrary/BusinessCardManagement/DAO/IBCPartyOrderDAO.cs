using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.BusinessCardManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IBCPartyOrderDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        List<BCPartyOrderVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<BCPartyOrderVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        BCPartyOrderVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<BCPartyOrderVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<BCPartyOrderVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<BCPartyOrderVO> voList);

        void UpdateListByParams(List<BCPartyOrderVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<BCPartyOrderVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<BCPartyOrderVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<BCPartyOrderVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<BCPartyOrderVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<BCPartyOrderVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<BCPartyOrderVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
        int Update(string data, string condition, params object[] parameters);
    }
}
