using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial interface IProjectChangeDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ProjectChangeVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<ProjectChangeVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        ProjectChangeVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<ProjectChangeVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ProjectChangeVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ProjectChangeVO> voList);

        void UpdateListByParams(List<ProjectChangeVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<ProjectChangeVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<ProjectChangeVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<ProjectChangeVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<ProjectChangeVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<ProjectChangeVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<ProjectChangeVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        List<ProjectChangeVO> FindAllByPage(string conditionStr,string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
