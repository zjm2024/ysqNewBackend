using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial interface IAllBusinessAverageScoreViewDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<AllBusinessAverageScoreViewVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<AllBusinessAverageScoreViewVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		AllBusinessAverageScoreViewVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<AllBusinessAverageScoreViewVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<AllBusinessAverageScoreViewVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<AllBusinessAverageScoreViewVO> voList);
		
		void UpdateListByParams(List<AllBusinessAverageScoreViewVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<AllBusinessAverageScoreViewVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<AllBusinessAverageScoreViewVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<AllBusinessAverageScoreViewVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<AllBusinessAverageScoreViewVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<AllBusinessAverageScoreViewVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<AllBusinessAverageScoreViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
