using CoreFramework.DAO;
using SPLibrary.BusinessCardManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IActivityTicketDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ActivityTicketVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<ActivityTicketVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        ActivityTicketVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<ActivityTicketVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ActivityTicketVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ActivityTicketVO> voList);

        void UpdateListByParams(List<ActivityTicketVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<ActivityTicketVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<ActivityTicketVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<ActivityTicketVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<ActivityTicketVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<ActivityTicketVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<ActivityTicketVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        List<ActivityTicketVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters);

        List<ActivityTicketVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}