using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class FarmGameDAO : CommonDAO, IFarmGameDAO
    {
		public FarmGameDAO(UserProfile userProfile)
		{
			base._tableName= "t_card_farmgame";
			base._pkId = "FarmGameID";
			base._voType = typeof(FarmGameVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<FarmGameVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<FarmGameVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<FarmGameVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<FarmGameVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public FarmGameVO FindById(object id)
        {
            return base.FindById<FarmGameVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<FarmGameVO> voList)
        {
            base.InsertList<FarmGameVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<FarmGameVO> voList, int countInEveryRun)
        {
            base.InsertList<FarmGameVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<FarmGameVO> voList)
        {
            base.UpdateListById<FarmGameVO>(voList);
        }
        
        public void UpdateListByParams(List<FarmGameVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<FarmGameVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<FarmGameVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<FarmGameVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<FarmGameVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<FarmGameVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<FarmGameVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<FarmGameVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<FarmGameVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<FarmGameVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<FarmGameVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<FarmGameVO>(strSQL, parameters);
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

        public List<FarmGameVO> FindCardList(string condition,int limit=0, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += condition;
            strSQL += " order by DefaultCard desc,CreatedAt asc";

            if (limit > 0)
            {
                strSQL += " limit " + limit;
            }

            return DbHelper.ExecuteVO<FarmGameVO>(strSQL, parameters);
        }
    }
}