using CoreFramework.DAO;
using SPLibrary.BusinessCardManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IActivitySignTicketDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        List<ActivitySignTicketVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<ActivitySignTicketVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        ActivitySignTicketVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<ActivitySignTicketVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ActivitySignTicketVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ActivitySignTicketVO> voList);

        void UpdateListByParams(List<ActivitySignTicketVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<ActivitySignTicketVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<ActivitySignTicketVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<ActivitySignTicketVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<ActivitySignTicketVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<ActivitySignTicketVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<ActivitySignTicketVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        List<ActivitySignTicketVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters);

        List<ActivitySignTicketVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, int limit, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}

