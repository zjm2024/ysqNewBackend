using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IAdPopupLogDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        List<AdPopupLogVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<AdPopupLogVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        AdPopupLogVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<AdPopupLogVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<AdPopupLogVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        void UpdateById(List<AdPopupLogVO> voList);

        void UpdateListByParams(List<AdPopupLogVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<AdPopupLogVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<AdPopupLogVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<AdPopupLogVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<AdPopupLogVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<AdPopupLogVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<AdPopupLogVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        List<AdPopupLogVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters);

        List<AdPopupLogVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}