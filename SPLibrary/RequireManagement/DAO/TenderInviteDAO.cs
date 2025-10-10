using System;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial class TenderInviteDAO:CommonDAO,ITenderInviteDAO
    {
		public TenderInviteDAO(UserProfile userProfile)
		{
			base._tableName="t_pro_tenderinvite";
			base._pkId = "TenderInviteId";
			base._voType = typeof(TenderInviteVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<TenderInviteVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<TenderInviteVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<TenderInviteVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<TenderInviteVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public TenderInviteVO FindById(object id)
        {
            return base.FindById<TenderInviteVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<TenderInviteVO> voList)
        {
            base.InsertList<TenderInviteVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<TenderInviteVO> voList, int countInEveryRun)
        {
            base.InsertList<TenderInviteVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<TenderInviteVO> voList)
        {
            base.UpdateListById<TenderInviteVO>(voList);
        }
        
        public void UpdateListByParams(List<TenderInviteVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<TenderInviteVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<TenderInviteVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<TenderInviteVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<TenderInviteVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<TenderInviteVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<TenderInviteVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<TenderInviteVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<TenderInviteVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<TenderInviteVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<TenderInviteVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<TenderInviteVO>(strSQL, parameters);
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