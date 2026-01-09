using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class PersonalDAO : CommonDAO, IPersonalDAO
    {
		public PersonalDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_personal";
			base._pkId = "PersonalID";
			base._voType = typeof(PersonalVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<PersonalVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<PersonalVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<PersonalVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<PersonalVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public PersonalVO FindById(object id)
        {
            return base.FindById<PersonalVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<PersonalVO> voList)
        {
            base.InsertList<PersonalVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<PersonalVO> voList, int countInEveryRun)
        {
            base.InsertList<PersonalVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<PersonalVO> voList)
        {
            base.UpdateListById<PersonalVO>(voList);
        }
        
        public void UpdateListByParams(List<PersonalVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<PersonalVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<PersonalVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<PersonalVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<PersonalVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<PersonalVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<PersonalVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<PersonalVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<PersonalVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<PersonalVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<PersonalVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<PersonalVO>(strSQL, parameters);
        }

        public int FindTotalCount(string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select Count(0) FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += condition;
            int totalCount = 0;
            try
            {
                totalCount = Convert.ToInt32(DbHelper.ExecuteScalar(strSQL, parameters));
            }
            catch
            {
                totalCount = -1;
            }
            return totalCount;
        }
    }
}