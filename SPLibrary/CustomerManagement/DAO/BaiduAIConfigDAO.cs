using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class BaiduAIConfigDAO : CommonDAO, IBaiduAIConfigDAO
    {
		public BaiduAIConfigDAO(UserProfile userProfile)
		{
			base._tableName= "t_baidu_config";
			base._pkId = "BaiduAIConfigID";
			base._voType = typeof(BaiduAIConfigVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<BaiduAIConfigVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<BaiduAIConfigVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<BaiduAIConfigVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<BaiduAIConfigVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public BaiduAIConfigVO FindById(object id)
        {
            return base.FindById<BaiduAIConfigVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<BaiduAIConfigVO> voList)
        {
            base.InsertList<BaiduAIConfigVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<BaiduAIConfigVO> voList, int countInEveryRun)
        {
            base.InsertList<BaiduAIConfigVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<BaiduAIConfigVO> voList)
        {
            base.UpdateListById<BaiduAIConfigVO>(voList);
        }
        
        public void UpdateListByParams(List<BaiduAIConfigVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<BaiduAIConfigVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BaiduAIConfigVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<BaiduAIConfigVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<BaiduAIConfigVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<BaiduAIConfigVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BaiduAIConfigVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<BaiduAIConfigVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<BaiduAIConfigVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<BaiduAIConfigVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<BaiduAIConfigVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<BaiduAIConfigVO>(strSQL, parameters);
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