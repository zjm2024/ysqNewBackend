using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class GroupBuyMemberDAO : CommonDAO, IGroupBuyMemberDAO
    {
		public GroupBuyMemberDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_groupbuymember";
			base._pkId = "GroupBuyMemberID";
			base._voType = typeof(GroupBuyMemberVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<GroupBuyMemberVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<GroupBuyMemberVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<GroupBuyMemberVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<GroupBuyMemberVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public GroupBuyMemberVO FindById(object id)
        {
            return base.FindById<GroupBuyMemberVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<GroupBuyMemberVO> voList)
        {
            base.InsertList<GroupBuyMemberVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<GroupBuyMemberVO> voList, int countInEveryRun)
        {
            base.InsertList<GroupBuyMemberVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<GroupBuyMemberVO> voList)
        {
            base.UpdateListById<GroupBuyMemberVO>(voList);
        }
        
        public void UpdateListByParams(List<GroupBuyMemberVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<GroupBuyMemberVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<GroupBuyMemberVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<GroupBuyMemberVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<GroupBuyMemberVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<GroupBuyMemberVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<GroupBuyMemberVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<GroupBuyMemberVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<GroupBuyMemberVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<GroupBuyMemberVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<GroupBuyMemberVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<GroupBuyMemberVO>(strSQL, parameters);
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
        public List<GroupBuyMemberVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<GroupBuyMemberVO>(strSQL, parameters);
        }
        public List<GroupBuyMemberVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc,int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<GroupBuyMemberVO>(strSQL, parameters);
        }
        public int Update(string data, string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " UPDATE  " + this._tableName + " 	 \n";
            strSQL += " SET  " + data + "\n";
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