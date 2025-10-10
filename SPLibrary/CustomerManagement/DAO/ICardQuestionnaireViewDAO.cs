using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface ICardQuestionnaireViewDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<CardQuestionnaireViewVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<CardQuestionnaireViewVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        CardQuestionnaireViewVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<CardQuestionnaireViewVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<CardQuestionnaireViewVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<CardQuestionnaireViewVO> voList);
		
		void UpdateListByParams(List<CardQuestionnaireViewVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<CardQuestionnaireViewVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<CardQuestionnaireViewVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<CardQuestionnaireViewVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<CardQuestionnaireViewVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<CardQuestionnaireViewVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<CardQuestionnaireViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
        int Update(string data, string condition, params object[] parameters);
    }
}
