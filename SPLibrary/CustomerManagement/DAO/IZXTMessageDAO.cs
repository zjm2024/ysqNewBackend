using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface IZXTMessageDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ZXTMessageVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<ZXTMessageVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        ZXTMessageVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<ZXTMessageVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ZXTMessageVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ZXTMessageVO> voList);

        void UpdateListByParams(List<ZXTMessageVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<ZXTMessageVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<ZXTMessageVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<ZXTMessageVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<ZXTMessageVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<ZXTMessageVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<ZXTMessageVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);

        int Update(string data, string condition, params object[] parameters);
    }
}
