using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IAdPopupConfigDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        List<AdPopupConfigVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<AdPopupConfigVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        AdPopupConfigVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<AdPopupConfigVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<AdPopupConfigVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        void UpdateById(List<AdPopupConfigVO> voList);

        void UpdateListByParams(List<AdPopupConfigVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<AdPopupConfigVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<AdPopupConfigVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<AdPopupConfigVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<AdPopupConfigVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<AdPopupConfigVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<AdPopupConfigVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        List<AdPopupConfigVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters);

        List<AdPopupConfigVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}