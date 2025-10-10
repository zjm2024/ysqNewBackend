using CoreFramework.DAO;
using SPLibrary.BusinessCardManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IActivityCountDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ActivityCountVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<ActivityCountVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        ActivityCountVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<ActivityCountVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ActivityCountVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ActivityCountVO> voList);

        void UpdateListByParams(List<ActivityCountVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<ActivityCountVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<ActivityCountVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<ActivityCountVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<ActivityCountVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<ActivityCountVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<ActivityCountVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        List<ActivityCountVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters);

        List<ActivityCountVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}