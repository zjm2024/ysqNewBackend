using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using CoreFramework.DAO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial interface IPersonalDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<PersonalVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<PersonalVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        PersonalVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<PersonalVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<PersonalVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<PersonalVO> voList);
		
		void UpdateListByParams(List<PersonalVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<PersonalVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<PersonalVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<PersonalVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<PersonalVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<PersonalVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<PersonalVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
