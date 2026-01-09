using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial interface IDemandViewDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        List<DemandViewVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<DemandViewVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        DemandViewVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<DemandViewVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<DemandViewVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<DemandViewVO> voList);

        void UpdateListByParams(List<DemandViewVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<DemandViewVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<DemandViewVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<DemandViewVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<DemandViewVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<DemandViewVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<DemandViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
