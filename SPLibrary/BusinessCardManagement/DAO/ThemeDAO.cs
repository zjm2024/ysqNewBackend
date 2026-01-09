using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class ThemeDAO : CommonDAO, IThemeDAO
    {
		public ThemeDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_theme";
			base._pkId = "ThemeID";
			base._voType = typeof(ThemeVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ThemeVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ThemeVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ThemeVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ThemeVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ThemeVO FindById(object id)
        {
            return base.FindById<ThemeVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ThemeVO> voList)
        {
            base.InsertList<ThemeVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ThemeVO> voList, int countInEveryRun)
        {
            base.InsertList<ThemeVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ThemeVO> voList)
        {
            base.UpdateListById<ThemeVO>(voList);
        }
        
        public void UpdateListByParams(List<ThemeVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ThemeVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ThemeVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<ThemeVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<ThemeVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ThemeVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ThemeVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ThemeVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<ThemeVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<ThemeVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<ThemeVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ThemeVO>(strSQL, parameters);
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