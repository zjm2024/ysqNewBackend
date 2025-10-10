using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class AccessrecordsViewDAO : CommonDAO, IAccessrecordsViewDAO
    {
		public AccessrecordsViewDAO(UserProfile userProfile)
		{
			base._tableName= "v_bcaccessrecordsview";
			base._pkId = "AccessRecordsID";
			base._voType = typeof(AccessrecordsViewVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<AccessrecordsViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<AccessrecordsViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<AccessrecordsViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<AccessrecordsViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public AccessrecordsViewVO FindById(object id)
        {
            return base.FindById<AccessrecordsViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<AccessrecordsViewVO> voList)
        {
            base.InsertList<AccessrecordsViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<AccessrecordsViewVO> voList, int countInEveryRun)
        {
            base.InsertList<AccessrecordsViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<AccessrecordsViewVO> voList)
        {
            base.UpdateListById<AccessrecordsViewVO>(voList);
        }
        
        public void UpdateListByParams(List<AccessrecordsViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<AccessrecordsViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AccessrecordsViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<AccessrecordsViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<AccessrecordsViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<AccessrecordsViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AccessrecordsViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<AccessrecordsViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<AccessrecordsViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<AccessrecordsViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<AccessrecordsViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<AccessrecordsViewVO>(strSQL, parameters);
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

        //查询回递名片的数量
        public int FindReturnCardCount(int PersonalID, params object[] parameters)
        {
            string FROMSQL = "";
            FROMSQL += " Select * FROM " + this._tableName + " Where Type = 'ReturnCard' and ToPersonalID = " + PersonalID + " GROUP BY PersonalID";

            string strSQL = "Select Count(0) FROM ("+ FROMSQL+") r";
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

        //查询回递名片的数量
        public int FindReturnCardCount(int PersonalID, string condition, params object[] parameters)
        {
            string FROMSQL = "";
            FROMSQL += " Select * FROM " + this._tableName + " Where Type = 'ReturnCard' and ToPersonalID = " + PersonalID + " GROUP BY PersonalID";

            string strSQL = "Select Count(0) FROM (" + FROMSQL + ") r \n";
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

        public List<AccessrecordsViewVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<AccessrecordsViewVO>(strSQL, parameters);
        }
        public List<AccessrecordsViewVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc,int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<AccessrecordsViewVO>(strSQL, parameters);
        }
    }
}