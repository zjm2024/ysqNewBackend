using System;
using System.Collections.Generic;
using SPLibrary.BusinessCardManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.BusinessCardManagement.DAO
{
    public partial class BCPartyViewDAO : CommonDAO, IBCPartyViewDAO
    { 
        public BCPartyViewDAO(UserProfile userProfile)
		{
			base._tableName= "v_bc_partyview";
			base._pkId = "PartyID";
			base._voType = typeof(BCPartyViewVO);
            base.CurrentUserProfile = userProfile;
        }    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<BCPartyViewVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<BCPartyViewVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<BCPartyViewVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<BCPartyViewVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public BCPartyViewVO FindById(object id)
        {
            return base.FindById<BCPartyViewVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<BCPartyViewVO> voList)
        {
            base.InsertList<BCPartyViewVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<BCPartyViewVO> voList, int countInEveryRun)
        {
            base.InsertList<BCPartyViewVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<BCPartyViewVO> voList)
        {
            base.UpdateListById<BCPartyViewVO>(voList);
        }
        
        public void UpdateListByParams(List<BCPartyViewVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<BCPartyViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BCPartyViewVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<BCPartyViewVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<BCPartyViewVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<BCPartyViewVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<BCPartyViewVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<BCPartyViewVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<BCPartyViewVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<BCPartyViewVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<BCPartyViewVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start).ToString() + " , " + (end).ToString();

            return DbHelper.ExecuteVO<BCPartyViewVO>(strSQL, parameters);
        }


        public List<BCPartyViewVO> FindSignUpViewInviterByCon(string conditionStr)
        {//t_csc_customer
            string strSQL = "";
            strSQL += " SELECT r.HeaderLogo as Headimg,r.CustomerName as Name,r.CustomerId,c.Phone,c.CorporateName,l.InviterCID,l.PartyID,l.CreatedAt,l.SignUpStatus,l.remarkName,COUNT(distinct l.PartySignUpID) as CountNum FROM " + this._tableName + " l left join  t_csc_customer r on l.InviterCID=r.CustomerId left join ( SELECT substring_index(group_concat(CorporateName order by DefaultCard desc,CreatedAt asc SEPARATOR ','),',',1) AS CorporateName, CustomerId,substring_index(group_concat(Phone ORDER BY DefaultCard DESC, CreatedAt ASC SEPARATOR ','),',',1) AS Phone from t_card_data  GROUP BY CustomerId) c on l.InviterCID = c.CustomerId  \n";
            strSQL += " Where \n";
            strSQL += conditionStr+ " \n";
            strSQL += " GROUP BY l.InviterCID ";

            return DbHelper.ExecuteVO<BCPartyViewVO>(strSQL);
        }

        public List<BCPartyViewVO> FindAllByPageIndexByInviter(int PartyID, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

            strSQL += "SELECT r.HeaderLogo AS Headimg,r.CustomerName AS Name,r.CustomerId,c.CorporateName,c.Phone,l.InviterCID,l.PartyID,l.CreatedAt,l.SignUpStatus,l.remarkName,";
            strSQL += "COUNT(DISTINCT l.PartySignUpID) AS CountNum \n";
            strSQL += "FROM \n";
            strSQL += "v_bc_partysignupview l \n";
            strSQL += "LEFT JOIN t_csc_customer r ON l.InviterCID = r.CustomerId \n";
            strSQL += "LEFT JOIN (SELECT substring_index(group_concat(CorporateName ORDER BY DefaultCard DESC,CreatedAt ASC SEPARATOR ','),',', 1) AS CorporateName, \n";
            strSQL += "CustomerId,";
            strSQL += "substring_index(group_concat(Phone ORDER BY DefaultCard DESC,CreatedAt ASC SEPARATOR ','),',', 1) AS Phone \n";
            strSQL += "FROM \n";
            strSQL += "t_card_data \n";
            strSQL += "GROUP BY CustomerId \n";
            strSQL += ") c ON l.InviterCID = c.CustomerId \n";
            strSQL += "Where \n";
            strSQL += "PartyID = " + PartyID + " and PartySignUpID > 0 and InviterCID!= 0 \n";
            strSQL += "GROUP BY l.InviterCID \n";
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<BCPartyViewVO>(strSQL, parameters);
        }

        public int FindTotalCountByInviter(int PartyID, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " Select Count(0) FROM v_bc_partysignupview 	 \n";
            strSQL += " Where \n";
            strSQL += "PartyID = " + PartyID + " and PartySignUpID > 0 and InviterCID!= 0 \n";
            strSQL += "GROUP BY l.InviterCID \n";
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

        public int Update(string data, string condition, params object[] parameters)
        {
            string strSQL = "";
            strSQL += " UPDATE  " + this._tableName + " 	 \n";
            strSQL += " SET  " + data + "\n";
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