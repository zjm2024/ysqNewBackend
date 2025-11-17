using CoreFramework.DAO;
using SPLibrary.BusinessCardManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IRankItemDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters 
        /// </summary>
        List<RankItemVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<RankItemVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        RankItemVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<RankItemVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<RankItemVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<RankItemVO> voList);

        void UpdateListByParams(List<RankItemVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<RankItemVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<RankItemVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<RankItemVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<RankItemVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<RankItemVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<RankItemVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        List<RankItemVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters);

        List<RankItemVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
