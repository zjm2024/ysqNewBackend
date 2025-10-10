using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface IZxbConfigDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ZxbConfigVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<ZxbConfigVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        ZxbConfigVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<ZxbConfigVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ZxbConfigVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ZxbConfigVO> voList);

        void UpdateListByParams(List<ZxbConfigVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<ZxbConfigVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<ZxbConfigVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<ZxbConfigVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<ZxbConfigVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<ZxbConfigVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<ZxbConfigVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
