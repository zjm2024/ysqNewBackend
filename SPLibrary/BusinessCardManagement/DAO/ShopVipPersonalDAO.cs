using System;
using System.Collections.Generic;
using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class ShopVipPersonalDAO : CommonDAO, IShopVipPersonalDAO
    {
		public ShopVipPersonalDAO(UserProfile userProfile)
		{
			base._tableName= "t_bc_shopvippersonal";
			base._pkId = "ShopVipPersonalID";
			base._voType = typeof(ShopVipPersonalVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<ShopVipPersonalVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<ShopVipPersonalVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<ShopVipPersonalVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<ShopVipPersonalVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public ShopVipPersonalVO FindById(object id)
        {
            return base.FindById<ShopVipPersonalVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<ShopVipPersonalVO> voList)
        {
            base.InsertList<ShopVipPersonalVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<ShopVipPersonalVO> voList, int countInEveryRun)
        {
            base.InsertList<ShopVipPersonalVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<ShopVipPersonalVO> voList)
        {
            base.UpdateListById<ShopVipPersonalVO>(voList);
        }
        
        public void UpdateListByParams(List<ShopVipPersonalVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<ShopVipPersonalVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ShopVipPersonalVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<ShopVipPersonalVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<ShopVipPersonalVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<ShopVipPersonalVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<ShopVipPersonalVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<ShopVipPersonalVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<ShopVipPersonalVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<ShopVipPersonalVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<ShopVipPersonalVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<ShopVipPersonalVO>(strSQL, parameters);
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
        public List<ShopVipPersonalVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;

            return DbHelper.ExecuteVO<ShopVipPersonalVO>(strSQL, parameters);
        }
        public List<ShopVipPersonalVO> FindAllByPageIndex(string conditionStr, string sortcolname, string asc,int limit, params object[] parameters)
        {
            string strSQL = "";

            strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + limit;

            return DbHelper.ExecuteVO<ShopVipPersonalVO>(strSQL, parameters);
        }
    }
}