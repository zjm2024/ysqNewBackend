using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.LuckyDrawManagement.VO;

namespace SPLibrary.LuckyDrawManagement.DAO
{
    public partial class LuckyDrawDAO : CommonDAO, ILuckyDrawDAO
    {
		public LuckyDrawDAO(UserProfile userProfile)
		{
			base._tableName= "t_ld_luckydraw";
			base._pkId = "LuckyDrawID";
			base._voType = typeof(LuckyDrawVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<LuckyDrawVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<LuckyDrawVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<LuckyDrawVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<LuckyDrawVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public LuckyDrawVO FindById(object id)
        {
            return base.FindById<LuckyDrawVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<LuckyDrawVO> voList)
        {
            base.InsertList<LuckyDrawVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<LuckyDrawVO> voList, int countInEveryRun)
        {
            base.InsertList<LuckyDrawVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<LuckyDrawVO> voList)
        {
            base.UpdateListById<LuckyDrawVO>(voList);
        }
        
        public void UpdateListByParams(List<LuckyDrawVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<LuckyDrawVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<LuckyDrawVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<LuckyDrawVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<LuckyDrawVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<LuckyDrawVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<LuckyDrawVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<LuckyDrawVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<LuckyDrawVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<LuckyDrawVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<LuckyDrawVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<LuckyDrawVO>(strSQL, parameters);
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