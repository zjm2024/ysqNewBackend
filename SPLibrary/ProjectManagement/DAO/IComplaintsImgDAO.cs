using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.ProjectManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.ProjectManagement.DAO
{
    public partial interface IComplaintsImgDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<ComplaintsImgVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<ComplaintsImgVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		ComplaintsImgVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<ComplaintsImgVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<ComplaintsImgVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<ComplaintsImgVO> voList);
		
		void UpdateListByParams(List<ComplaintsImgVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<ComplaintsImgVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<ComplaintsImgVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<ComplaintsImgVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<ComplaintsImgVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<ComplaintsImgVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<ComplaintsImgVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
