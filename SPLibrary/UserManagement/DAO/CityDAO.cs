using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class CityDAO:CommonDAO,ICityDAO
    {
		public CityDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_city";
			base._pkId = "CityId";
			base._voType = typeof(CityVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<CityVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<CityVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<CityVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<CityVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public CityVO FindById(object id)
        {
            return base.FindById<CityVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<CityVO> voList)
        {
            base.InsertList<CityVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<CityVO> voList, int countInEveryRun)
        {
            base.InsertList<CityVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<CityVO> voList)
        {
            base.UpdateListById<CityVO>(voList);
        }
        
        public void UpdateListByParams(List<CityVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<CityVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CityVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<CityVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<CityVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<CityVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<CityVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<CityVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<CityVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<CityVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<CityVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<CityVO>(strSQL, parameters);
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