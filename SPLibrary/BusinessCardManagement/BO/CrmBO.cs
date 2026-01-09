using CoreFramework.DAO;
using CoreFramework.VO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Drawing;
using System.IO;
using SPLibrary.WebConfigInfo;
using System.Linq;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.BusinessCardManagement.DAO;
using SPLibrary.CustomerManagement.BO;
using System.Text.RegularExpressions;

namespace SPLibrary.BusinessCardManagement.BO
{
    public class CrmBO
    {
        static public string appid = "wxc9245bafef27dddf";
        static public string secret = "76fe22240a699f0cceb12c3118b49cab";
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();
        public CrmBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }

        /// <summary>
        /// 添加Crm信息
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCrm(CrmVO vo)
        {
            try
            {
                ICrmDAO rDAO = BusinessCardManagementDAOFactory.CrmDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int CrmID = rDAO.Insert(vo);
                    return CrmID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新Crm信息
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCrm(CrmVO vo)
        {
            ICrmDAO rDAO = BusinessCardManagementDAOFactory.CrmDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 删除Crm信息
        /// </summary>
        /// <param name="CrmID"></param>
        /// <returns></returns>
        public int DeleteCrmById(int CrmID)
        {
            ICrmDAO rDAO = BusinessCardManagementDAOFactory.CrmDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("CrmID = " + CrmID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取Crm信息详情
        /// </summary>
        /// <param name="CrmID"></param>
        /// <returns></returns>
        public CrmVO FindCrmById(int CrmID)
        {
            ICrmDAO rDAO = BusinessCardManagementDAOFactory.CrmDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(CrmID);
        }

        /// <summary>
        /// 获取Crm信息详情(视图)
        /// </summary>
        /// <param name="CrmID"></param>
        /// <returns></returns>
        public CrmViewVO FindCrmViewById(int CrmID)
        {
            ICrmViewDAO rDAO = BusinessCardManagementDAOFactory.CrmViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(CrmID);
        }

        /// <summary>
        /// 获取Crm信息列表（视图，分页）
        /// </summary>
        /// <returns></returns>
        public List<CrmViewVO> FindCrmViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICrmViewDAO rDAO = BusinessCardManagementDAOFactory.CrmViewDAO(this.CurrentCustomerProfile);
            List<CrmViewVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            return cVO;
        }

        /// <summary>
        /// 获取Crm信息列表(视图)
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<CrmViewVO> FindCrmViewList(string condtion)
        {
            ICrmViewDAO rDAO = BusinessCardManagementDAOFactory.CrmViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取Crm信息列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<CrmVO> FindCrmList(string condtion)
        {
            ICrmDAO rDAO = BusinessCardManagementDAOFactory.CrmDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取Crm信息数量
        /// </summary>
        /// <returns></returns>
        public int FindCrmViewCount(string condition)
        {
            ICrmViewDAO rDAO = BusinessCardManagementDAOFactory.CrmViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 获取Crm信息数量
        /// </summary>
        /// <returns></returns>
        public int FindCrmCount(string condition)
        {
            ICrmDAO rDAO = BusinessCardManagementDAOFactory.CrmDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }
        /// <summary>
        /// 获取未读评论数量
        /// </summary>
        /// <returns></returns>
        public int FindCommentViewCountByCrmID(int CrmID, int toPersonalID)
        {
            ICommentViewDAO rDAO = BusinessCardManagementDAOFactory.CommentViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount("CrmID=" + CrmID + " and Status=1 and InfoToPersonalID=" + toPersonalID);
        }
        /// <summary>
        /// 获取未读评论数量
        /// </summary>
        /// <returns></returns>
        public int FindCommentViewCount(string CrmType,int toPersonalID, int toBusinessID)
        {
            ICommentViewDAO rDAO = BusinessCardManagementDAOFactory.CommentViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount("CrmType='"+ CrmType+ "' and Status=1 and InfoToPersonalID=" + toPersonalID + " and InfoToBusinessID=" + toBusinessID);
        }
        /// <summary>
        /// 获取未读评论数量
        /// </summary>
        /// <returns></returns>
        public int FindCommentViewCount(int toPersonalID, int toBusinessID)
        {
            ICommentViewDAO rDAO = BusinessCardManagementDAOFactory.CommentViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount("Status=1 and InfoToPersonalID=" + toPersonalID+ " and InfoToBusinessID="+ toBusinessID);
        }

        /// <summary>
        /// 判断是否有查看修改权限
        /// </summary>
        /// <returns></returns>
        public bool EntitledToEdit(int CrmID,int PersonalID)
        {
            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = bBO.FindPersonalById(PersonalID);
            CrmVO cVO = FindCrmById(CrmID);

            try
            {
                if (cVO == null || pVO == null)
                {
                    return false;
                }
                
                //创建者本人
                if(cVO.PersonalID== pVO.PersonalID)
                {
                    return true;
                }else
                {
                    if(cVO.Type== "GoOut" || cVO.Type == "Daily" || cVO.Type == "Weekly" || cVO.Type == "Monthly")
                    {
                        return false;
                    }
                }

                //部门主管
                if (cVO.PersonalID > 0)
                {
                    PersonalVO tpVO = bBO.FindPersonalById(cVO.PersonalID);
                    DepartmentVO dVO = bBO.FindDepartmentById(tpVO.DepartmentID);
                    if (dVO != null)
                    {
                        if (dVO.DirectorPersonalID == pVO.PersonalID)
                            return true;
                    }
                }
               

                //拥有客户管理权限
                if(cVO.Type== "Clue" || cVO.Type == "Chance" || cVO.Type == "Clients")
                {
                    if(bBO.FindJurisdiction(pVO.PersonalID, cVO.BusinessID, "Admin") || bBO.FindJurisdiction(pVO.PersonalID, cVO.BusinessID, "Clients"))
                    {
                        return true;
                    }
                }

                //拥有合同管理权限
                if (cVO.Type == "Contract")
                {
                    if (bBO.FindJurisdiction(pVO.PersonalID, cVO.BusinessID, "Admin") || bBO.FindJurisdiction(pVO.PersonalID, cVO.BusinessID, "Performance"))
                    {
                        return true;
                    }
                }

            }
            catch
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// 判断是否有修改权限（合同）
        /// </summary>
        /// <returns></returns>
        public bool EntitledToContractEdit(int CrmID, int PersonalID)
        {
            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = bBO.FindPersonalById(PersonalID);
            CrmVO cVO = FindCrmById(CrmID);

            try
            {
                if (cVO == null || pVO == null)
                {
                    return false;
                }

                //拥有合同管理权限
                if (cVO.Type == "Contract")
                {
                    if (bBO.FindJurisdiction(pVO.PersonalID, cVO.BusinessID, "Admin") || bBO.FindJurisdiction(pVO.PersonalID, cVO.BusinessID, "Performance"))
                    {
                        return true;
                    }
                }

            }
            catch
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// 获取审批资格和结果：0：未处理，1:已批准，2:已拒绝，3:非审批人
        /// </summary>
        /// <returns></returns>
        public int GetIsApproval(CrmViewVO cVO, int PersonalID)
        {
            if (cVO.ForPersonalID.IndexOf("#" + PersonalID + "#") <= -1||(cVO.Type!= "qingjia" && cVO.Type != "baoxiao" && cVO.Type != "chuchai" && cVO.Type != "jiaban"))
            {
                return 3;
            }
            else if (cVO.ApprovalPersonalID.IndexOf("#" + PersonalID + "#") > -1)
            {
                return 1;
            }
            else if (cVO.DisapprovalPersonalID.IndexOf("#" + PersonalID + "#") > -1)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取审批结果：0：处理中，1:已通过，2:未通过
        /// </summary>
        /// <returns></returns>
        public int GetIsApproval(CrmViewVO cVO)
        {
            if (cVO.DisapprovalPersonalID != "")
            {
                return 2;
            }

            int IsApproval = 1;
            if (cVO.ForPersonalID != "")
            {
                string[] PersonalIDIdArr = cVO.ForPersonalID.Split(',');
                for (int i = 0; i < PersonalIDIdArr.Length; i++)
                {
                    if (cVO.ApprovalPersonalID.IndexOf(PersonalIDIdArr[i]) <= -1)
                    {
                        IsApproval=0;
                    }
                }
            }

            return IsApproval;
        }

        /// <summary>
        /// 获取审批数量
        /// </summary>
        /// <returns></returns>
        public int FindApprovalCount(string CrmType, int toPersonalID, int toBusinessID)
        {
            ICrmDAO rDAO = BusinessCardManagementDAOFactory.CrmDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount("BusinessID=" + toBusinessID + " and Type='" + CrmType + "' and Status=1 and ForPersonalID like '%#" + toPersonalID + "#%' and (ApprovalPersonalID not like '%#" + toPersonalID + "#%' or ApprovalPersonalID IS NULL) and (DisapprovalPersonalID not like '%#" + toPersonalID + "#%'  or DisapprovalPersonalID IS NULL)");
        }

        /// <summary>
        /// 获取审批数量
        /// </summary>
        /// <returns></returns>
        public int FindApprovalCount(int toPersonalID, int toBusinessID)
        {
            ICrmDAO rDAO = BusinessCardManagementDAOFactory.CrmDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount("BusinessID=" + toBusinessID + " and (Type='qingjia' or Type='baoxiao' or Type='chuchai' or Type='jiaban') and Status=1 and ForPersonalID like '%#" + toPersonalID + "#%' and (ApprovalPersonalID not like '%#" + toPersonalID + "#%' or ApprovalPersonalID IS NULL) and (DisapprovalPersonalID not like '%#" + toPersonalID + "#%'  or DisapprovalPersonalID IS NULL)");
        }

        /// <summary>
        /// 获取当月审批记录
        /// </summary>
        /// <returns></returns>
        public string GetApprovalToMonth(DateTime dt, string Type,int BusinessID, int PersonalID)
        {
            BusinessCardBO bBO = new BusinessCardBO(new CustomerProfile());
            string rtext = "";
            string condtion = "Type='" + Type + "' and Status=1 and BusinessID=" + BusinessID + " and PersonalID=" + PersonalID;
            string sqlTime = "   and year(CreatedAt)='" + dt.Year + "' and month(CreatedAt)='" + dt.Month + "'";
            List<CrmViewVO> cVO = FindCrmViewList(condtion);
            for(int i=0;i< cVO.Count; i++)
            {
                string[] PersonalIDIdArr = cVO[i].ForPersonalID.Split(',');
                string name = "";
                if (GetIsApproval(cVO[i])==1 || GetIsApproval(cVO[i]) == 0)
                {
                    for (int j = 0; j < PersonalIDIdArr.Length; j++)
                    {
                        try
                        {

                            PersonalVO PersonalVO = bBO.FindPersonalById(Convert.ToInt32(PersonalIDIdArr[j].Replace("#", "")));
                            if (PersonalVO != null)
                            {
                                string status = "";

                                if (cVO[i].ApprovalPersonalID.IndexOf(PersonalIDIdArr[j]) > -1)
                                {
                                    status = "(批准)";
                                }
                                else if (cVO[i].DisapprovalPersonalID.IndexOf(PersonalIDIdArr[j]) > -1)
                                {
                                    status = "(驳回)";
                                }
                                else
                                {
                                    status = "(未处理)";
                                }

                                name += PersonalVO.Name + status;
                                if (j < PersonalIDIdArr.Length - 1)
                                {
                                    name += ",";
                                }
                            }
                        }
                        catch
                        {

                        }
                    }

                    if (Type == "qingjia")
                    {
                        rtext += cVO[i].Field4 + "，" + cVO[i].StartTime.ToString("MM月dd日 hh:mm") + "至" + cVO[i].EndTime.ToString("MM月dd日 hh:mm") + "，审批人:" + name + "；\r\n";
                    }
                    if (Type == "baoxiao")
                    {
                        rtext += cVO[i].Field6 + "，金额:" + cVO[i].priceA + "元" + "，审批人:" + name + "；\r\n";
                    }
                    if (Type == "chuchai")
                    {
                        rtext += cVO[i].Field6 + "，" + cVO[i].StartTime.ToString("MM月dd日 hh:mm") + "至" + cVO[i].EndTime.ToString("MM月dd日 hh:mm") + "，审批人:" + name + "；\r\n";
                    }
                    if (Type == "jiaban")
                    {
                        rtext += cVO[i].Field6 + "，" + cVO[i].StartTime.ToString("MM月dd日 hh:mm") + "至" + cVO[i].EndTime.ToString("MM月dd日 hh:mm") + "，审批人:" + name + "；\r\n";
                    }
                } 
            }
            return rtext;
        }
    }
}
