using System;
using System.Collections.Generic;
using SPLibrary.CustomerManagement.VO;
using CoreFramework;
using CoreFramework.DAO;
using SPLibrary.CoreFramework;
using CoreFramework.VO;

namespace SPLibrary.CustomerManagement.DAO
{
    public partial class AgencyExperienceImageDAO:CommonDAO,IAgencyExperienceImageDAO
    {
		public AgencyExperienceImageDAO(UserProfile userProfile)
		{
			base._tableName="t_csc_agencyexperienceimage";
			base._pkId = "AgencyExperienceImageId";
			base._voType = typeof(AgencyExperienceImageVO);
			base.CurrentUserProfile = userProfile;
		}    
    
        /// <summary>
        /// Find record(s) by some parameters
        /// </summary>
        /// <param name="condtion">condtion string</param>
        /// <param name="dbParameters">parameters for condtion string</param>
        /// <returns>VO list</returns>
        public List<AgencyExperienceImageVO> FindByParams(string condtion, params object[] dbParameters)
        {
            return base.FindByParams<AgencyExperienceImageVO>(condtion, dbParameters);
        }

        /// <summary>
        /// Find record(s) by filter
        /// </summary>
        /// <param name="fileter">query criteria object</param>
        /// <param name="dbParameters">parameters for filter</param>
        /// <returns>VO list</returns>
        public List<AgencyExperienceImageVO> FindByFilter(ISelectFilter filter)
        {
            return base.FindByFilter<AgencyExperienceImageVO>(filter);
        }

        /// <summary>
        /// Find a record by specified key(PK).
        /// </summary>
        /// <param name="id">PK value</param>
        /// <returns>VO</returns>
        public AgencyExperienceImageVO FindById(object id)
        {
            return base.FindById<AgencyExperienceImageVO>(id);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void InsertList(List<AgencyExperienceImageVO> voList)
        {
            base.InsertList<AgencyExperienceImageVO>(voList);
        }

        /// <summary>
        /// To insert records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        /// <param name="countInEveryRun">Update record number in every running for avoiding timeout</param>
        public void InsertList(List<AgencyExperienceImageVO> voList, int countInEveryRun)
        {
            base.InsertList<AgencyExperienceImageVO>(voList, countInEveryRun);
        }

        /// <summary>
        /// To update records to DB with VO list.
        /// </summary>
        /// <param name="voList">VO list</param>
        public void UpdateById(List<AgencyExperienceImageVO> voList)
        {
            base.UpdateListById<AgencyExperienceImageVO>(voList);
        }
        
        public void UpdateListByParams(List<AgencyExperienceImageVO> voList, string conditon, List<string> columnList)
        {
            base.UpdateListByParams<AgencyExperienceImageVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AgencyExperienceImageVO> voList, string conditon, List<string> columnList,int countEveryRun)
        {
            base.UpdateListByParams<AgencyExperienceImageVO>(voList, conditon, columnList,countEveryRun);
        }

        public void UpdateListByParams(List<AgencyExperienceImageVO> voList, string conditon, params string[] columnList)
        {
            base.UpdateListByParams<AgencyExperienceImageVO>(voList, conditon, columnList);
        }
        
        public void UpdateListByParams(List<AgencyExperienceImageVO> voList, string conditon,int countEveryRun, params string[] columnList)
        {
            base.UpdateListByParams<AgencyExperienceImageVO>(voList, conditon,countEveryRun, columnList);
        }
        
        public void DeleteListByParams(List<AgencyExperienceImageVO> voList, string condition, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, columnList);
        }
        
        public void DeleteListByParams(List<AgencyExperienceImageVO> voList, string condition, int countEveryRun, params string[] columnList)
        {
            base.DeleteListByParams(voList, condition, countEveryRun, columnList);
        }

		public List<AgencyExperienceImageVO> FindAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            string strSQL = "";

			strSQL += " SELECT * FROM " + this._tableName + " 	 \n";
            strSQL += " Where \n";
            strSQL += conditionStr;
            strSQL += " order by " + sortcolname + " " + asc;
            strSQL += " limit " + (start - 1).ToString() + " , " + (end - start + 1).ToString();

            return DbHelper.ExecuteVO<AgencyExperienceImageVO>(strSQL, parameters);
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