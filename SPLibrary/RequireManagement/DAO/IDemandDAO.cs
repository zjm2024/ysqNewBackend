using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial interface IDemandDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<DemandVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<DemandVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        DemandVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<DemandVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<DemandVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<DemandVO> voList);

        void UpdateListByParams(List<DemandVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<DemandVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<DemandVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<DemandVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<DemandVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<DemandVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<DemandVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
