using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface IZXTFriendDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ZXTFriendVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<ZXTFriendVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        ZXTFriendVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<ZXTFriendVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ZXTFriendVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ZXTFriendVO> voList);

        void UpdateListByParams(List<ZXTFriendVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<ZXTFriendVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<ZXTFriendVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<ZXTFriendVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<ZXTFriendVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<ZXTFriendVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<ZXTFriendVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
