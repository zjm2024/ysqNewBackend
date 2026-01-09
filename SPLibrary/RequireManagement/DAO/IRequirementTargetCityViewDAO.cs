using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial interface IRequirementTargetCityViewDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<RequirementTargetCityViewVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<RequirementTargetCityViewVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		RequirementTargetCityViewVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<RequirementTargetCityViewVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<RequirementTargetCityViewVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<RequirementTargetCityViewVO> voList);
		
		void UpdateListByParams(List<RequirementTargetCityViewVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<RequirementTargetCityViewVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<RequirementTargetCityViewVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<RequirementTargetCityViewVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<RequirementTargetCityViewVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<RequirementTargetCityViewVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<RequirementTargetCityViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
