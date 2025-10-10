using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial interface ISignInDAO : ICommonDAO
    {
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
		List<SignInVO> FindByParams(string condtion, params object[] dbParameters);

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        List<SignInVO> FindByFilter(ISelectFilter filter);

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        SignInVO FindById(object id);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        void InsertList(List<SignInVO> voList);

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        void InsertList(List<SignInVO> voList, int splitCount);

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
		void UpdateById(List<SignInVO> voList);

        void UpdateListByParams(List<SignInVO> voList, string conditon, List<string> columnList);

        void UpdateListByParams(List<SignInVO> voList, string conditon, List<string> columnList, int countEveryRun);

        void UpdateListByParams(List<SignInVO> voList, string conditon, params string[] columnList);

        void UpdateListByParams(List<SignInVO> voList, string conditon, int countEveryRun, params string[] columnList);

        void DeleteListByParams(List<SignInVO> voList, string condition, params string[] columnList);

        void DeleteListByParams(List<SignInVO> voList, string condition, int countEveryRun, params string[] columnList);

        List<SignInVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters);

        int FindTotalCount(string condition, params object[] parameters);

        int DeleteSignInBycondition(string condition, params object[] parameters);

        int UpdateSignInBycondition(string Setdata,string condition, params object[] parameters);
    }
}
