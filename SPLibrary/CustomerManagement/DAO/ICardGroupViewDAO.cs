using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface ICardGroupViewDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<CardGroupViewVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<CardGroupViewVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        CardGroupViewVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<CardGroupViewVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<CardGroupViewVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<CardGroupViewVO> voList);
		
		void UpdateListByParams(List<CardGroupViewVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<CardGroupViewVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<CardGroupViewVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<CardGroupViewVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<CardGroupViewVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<CardGroupViewVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<CardGroupViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
        int Update(string data, string condition, params object[] parameters);
    }
}
