using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class BusinessCardDAO : CommonDAO, IBusinessCardDAO
    {
		public BusinessCardDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_business";
			base._pkId = "BusinessID";
			base._voType = typeof(BusinessCardVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<BusinessCardVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<BusinessCardVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<BusinessCardVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<BusinessCardVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public BusinessCardVO FindById(object id)
        {
            return base.FindById<BusinessCardVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<BusinessCardVO> voList)
        {
            base.InsertList<BusinessCardVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<BusinessCardVO> voList, int countInEveryRun)
        {
            base.InsertList<BusinessCardVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<BusinessCardVO> voList)
        {
            base.UpdateListById<BusinessCardVO>(voList);
        }
        
        public void UpdateListByParams(List<BusinessCardVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<BusinessCardVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BusinessCardVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<BusinessCardVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<BusinessCardVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<BusinessCardVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BusinessCardVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<BusinessCardVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<BusinessCardVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<BusinessCardVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<BusinessCardVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<BusinessCardVO>(strSQL, parameters);
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