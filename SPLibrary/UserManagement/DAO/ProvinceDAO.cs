using System;
using System.Collections.Generic;
using SPLibrary.UserManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.UserManagement.DAO
{
    public partial class ProvinceDAO:CommonDAO,IProvinceDAO
    {
		public ProvinceDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_province";
			base._pkId = "ProvinceId";
			base._voType = typeof(ProvinceVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ProvinceVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ProvinceVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ProvinceVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ProvinceVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ProvinceVO FindById(object id)
        {
            return base.FindById<ProvinceVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ProvinceVO> voList)
        {
            base.InsertList<ProvinceVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ProvinceVO> voList, int countInEveryRun)
        {
            base.InsertList<ProvinceVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ProvinceVO> voList)
        {
            base.UpdateListById<ProvinceVO>(voList);
        }
        
        public void UpdateListByParams(List<ProvinceVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ProvinceVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ProvinceVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<ProvinceVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<ProvinceVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ProvinceVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ProvinceVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ProvinceVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<ProvinceVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<ProvinceVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<ProvinceVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ProvinceVO>(strSQL, parameters);
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