using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class TargetCityViewDAO:CommonDAO,ITargetCityViewDAO
    {
		public TargetCityViewDAO(UserProfile userProfile)
		{
			base._tableName="v_targetcityview";
			base._pkId = "TargetCityId";
			base._voType = typeof(TargetCityViewVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<TargetCityViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<TargetCityViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<TargetCityViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<TargetCityViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public TargetCityViewVO FindById(object id)
        {
            return base.FindById<TargetCityViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<TargetCityViewVO> voList)
        {
            base.InsertList<TargetCityViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<TargetCityViewVO> voList, int countInEveryRun)
        {
            base.InsertList<TargetCityViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<TargetCityViewVO> voList)
        {
            base.UpdateListById<TargetCityViewVO>(voList);
        }
        
        public void UpdateListByParams(List<TargetCityViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<TargetCityViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<TargetCityViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<TargetCityViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<TargetCityViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<TargetCityViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<TargetCityViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<TargetCityViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<TargetCityViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<TargetCityViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<TargetCityViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<TargetCityViewVO>(strSQL, parameters);
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