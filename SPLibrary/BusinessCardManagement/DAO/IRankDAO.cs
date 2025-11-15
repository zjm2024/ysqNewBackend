using CoreFramework.DAO;
using SPLibrary.BusinessCardManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IRankDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        List<RankVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<RankVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        RankVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<RankVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<RankVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<RankVO> voList);

        void UpdateListByParams(List<RankVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<RankVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<RankVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<RankVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<RankVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<RankVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<RankVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        List<RankVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters);

        List<RankVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
