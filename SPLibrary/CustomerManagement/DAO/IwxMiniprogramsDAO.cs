using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface IwxMiniprogramsDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<wxMiniprogramsVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<wxMiniprogramsVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        wxMiniprogramsVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<wxMiniprogramsVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<wxMiniprogramsVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<wxMiniprogramsVO> voList);
		
		void UpdateListByParams(List<wxMiniprogramsVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<wxMiniprogramsVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<wxMiniprogramsVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<wxMiniprogramsVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<wxMiniprogramsVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<wxMiniprogramsVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<wxMiniprogramsVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
        int Update(string data, string condition, params object[] parameters);
    }
}
