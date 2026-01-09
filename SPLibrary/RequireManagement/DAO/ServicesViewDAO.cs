using System;
using System.Collections.Generic;
using SPLibrary.RequireManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.RequireManagement.DAO
{
    public partial class ServicesViewDAO:CommonDAO,IServicesViewDAO
    {
		public ServicesViewDAO(UserProfile userProfile)
		{
			base._tableName="v_servicesview";
			base._pkId = "ServicesId";
			base._voType = typeof(ServicesViewVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ServicesViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ServicesViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ServicesViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ServicesViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ServicesViewVO FindById(object id)
        {
            return base.FindById<ServicesViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ServicesViewVO> voList)
        {
            base.InsertList<ServicesViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ServicesViewVO> voList, int countInEveryRun)
        {
            base.InsertList<ServicesViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ServicesViewVO> voList)
        {
            base.UpdateListById<ServicesViewVO>(voList);
        }
        
        public void UpdateListByParams(List<ServicesViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ServicesViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ServicesViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<ServicesViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<ServicesViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ServicesViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ServicesViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ServicesViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<ServicesViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<ServicesViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<ServicesViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ServicesViewVO>(strSQL, parameters);
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