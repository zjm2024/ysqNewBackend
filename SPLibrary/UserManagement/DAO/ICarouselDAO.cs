using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework.DAO;

namespace SPLibrary.UserManagement.DAO
{
    public partial interface ICarouselDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<CarouselVO> FindByParams(string condtion, params object[] dbParameters);
		
        /// <summary>
        /// Find record(s) by filter
        /// </summary>
		List<CarouselVO> FindByFilter(ISelectFilter filter);
		
        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
		CarouselVO FindById(object id);
		
        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
		void InsertList(List<CarouselVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<CarouselVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<CarouselVO> voList);
		
		void UpdateListByParams(List<CarouselVO> voList, string conditon, List<string> columnList);
		
		void UpdateListByParams(List<CarouselVO> voList, string conditon, List<string> columnList,int countEveryRun);
        
        void UpdateListByParams(List<CarouselVO> voList, string conditon, params string[] columnList);
        
        void UpdateListByParams(List<CarouselVO> voList, string conditon,int countEveryRun, params string[] columnList);
       
		void DeleteListByParams(List<CarouselVO> voList, string condition, params string[] columnList);
		
		void DeleteListByParams(List<CarouselVO> voList, string condition, int countEveryRun, params string[] columnList);

		List<CarouselVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);
    }
}
