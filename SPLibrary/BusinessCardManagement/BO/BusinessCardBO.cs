using BroadSky.WeChatAppDecrypt;
using CoreFramework.DAO;
using CoreFramework.VO;
using ImportEXCEL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;
using NPOI.SS.Formula.PTG;
using SPLibrary.BusinessCardManagement.DAO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using TencentCloud.Cdn.V20180606.Models;
using TencentCloud.Scf.V20180416.Models;
using IBankAccountDAO = SPLibrary.BusinessCardManagement.DAO.IBankAccountDAO;

namespace SPLibrary.BusinessCardManagement.BO
{
    public class BusinessCardBO
    {
        static public string appid = "wx79943c188a5368a9";
        static public string secret = "ff88779918706db7fd2deebfc8161058";
        public int Type = 30;
        public int AppType = 30;
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();
        public BusinessCardBO(CustomerProfile customerProfile, int apptype = 30)
        {
            this.CurrentCustomerProfile = customerProfile;
            AppVO AppVO = AppBO.GetApp(apptype);
            appid = AppVO.AppId;
            secret = AppVO.Secret;
            Type = AppVO.AppType;
            AppType = AppVO.AppType;
        }
        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string getOpenId(string code, int AppType)
        {
            try
            {
                AppVO AppVO = AppBO.GetApp(AppType);
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + AppVO.AppId + "&secret=" + AppVO.Secret + "&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var result = new WeiXinAccessTokenResult();
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModel>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }

                return result.SuccessResult.openid;
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 获取Access_token
        /// </summary>
        /// <returns></returns>
        public string GetAccess_token()
        {
            try
            {
                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppBO.GetApp(0).AppId + "&secret=" + AppBO.GetApp(0).Secret;
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var result = new WeiXinAccessTokenResult();
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModel>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }

                return result.SuccessResult.access_token;
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 审核文本是否合法,true为合法，false为非法
        /// </summary>
        /// <returns></returns>
        public bool msg_sec_check(object obj)
        {
            try
            {
                string content = ObjectToJson(obj);
                content = Regex.Replace(content, @"[^\u4e00-\u9fa5]", "");
                string url = "https://api.weixin.qq.com/wxa/msg_sec_check?access_token=" + GetAccess_token();
                string DataJson = "{";
                DataJson += "\"content\": \"" + content + "\"";
                DataJson += "}";

                string jsonStr = HttpHelper.HtmlFromUrlPost(url, DataJson);

                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = jsonStr + "----------------------------------------" + DataJson;
                _log.Error(strErrorMsg);

                var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);

                if (errorResult.errcode == 87014)
                {
                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    ViolationVO ViolationVO = new ViolationVO();
                    ViolationVO.ViolationAt = DateTime.Now;
                    ViolationVO.ViolationText = content;
                    uBO.AddViolation(ViolationVO);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// 内存对象转换为json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJson(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 添加个人信息
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddPersonal(PersonalVO vo)
        {
            try
            {
                IPersonalDAO rDAO = BusinessCardManagementDAOFactory.PersonalDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int PersonalID = rDAO.Insert(vo);
                    return PersonalID;
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
        /// 添加企业名片
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddBusinessCard(BusinessCardVO vo)
        {
            try
            {
                IBusinessCardDAO rDAO = BusinessCardManagementDAOFactory.BusinessCardDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int BusinessID = rDAO.Insert(vo);
                    return BusinessID;
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
        /// 更新企业名片
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateBusinessCard(BusinessCardVO vo)
        {
            IBusinessCardDAO rDAO = BusinessCardManagementDAOFactory.BusinessCardDAO(this.CurrentCustomerProfile);
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
        /// 获取子公司列表（分页）
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<BusinessCardVO> FindBusinessCardAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IBusinessCardDAO rDAO = BusinessCardManagementDAOFactory.BusinessCardDAO(this.CurrentCustomerProfile);
            List<BusinessCardVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            return cVO;
        }

        /// <summary>
        /// 获取子公司数量
        /// </summary>
        /// <returns></returns>
        public int FindBusinessCardCount(string condition)
        {
            IBusinessCardDAO rDAO = BusinessCardManagementDAOFactory.BusinessCardDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }


        /// <summary>
        /// 获取子公司列表
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public List<BusinessCardVO> FindBusinessCardByHeadquartersID(int HeadquartersID)
        {
            IBusinessCardDAO rDAO = BusinessCardManagementDAOFactory.BusinessCardDAO(this.CurrentCustomerProfile);
            List<BusinessCardVO> cVO = rDAO.FindByParams("HeadquartersID = " + HeadquartersID);
            return cVO;
        }

        /// <summary>
        /// 获取企业名片详情
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public BusinessCardVO FindBusinessCardById(int BusinessID)
        {
            IBusinessCardDAO rDAO = BusinessCardManagementDAOFactory.BusinessCardDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(BusinessID);
        }

        /// <summary>
        /// 添加企业绑定
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddSecondBusiness(SecondBusinessVO vo)
        {
            try
            {
                ISecondBusinessDAO rDAO = BusinessCardManagementDAOFactory.SecondBusinessDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int SecondBusinessID = rDAO.Insert(vo);
                    return SecondBusinessID;
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
        /// 更新企业绑定
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateSecondBusiness(SecondBusinessVO vo)
        {
            ISecondBusinessDAO rDAO = BusinessCardManagementDAOFactory.SecondBusinessDAO(this.CurrentCustomerProfile);
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
        /// 获取我的企业绑定列表
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public List<SecondBusinessVO> FindSecondBusinessByPersonalID(int PersonalID)
        {
            ISecondBusinessDAO rDAO = BusinessCardManagementDAOFactory.SecondBusinessDAO(this.CurrentCustomerProfile);
            List<SecondBusinessVO> cVO = rDAO.FindByParams("PersonalID = " + PersonalID);
            return cVO;
        }

        /// <summary>
        /// 获取我的企业绑定列表
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public List<SecondBusinessVO> FindSecondBusinessByPersonalID(int PersonalID, int BusinessID)
        {
            ISecondBusinessDAO rDAO = BusinessCardManagementDAOFactory.SecondBusinessDAO(this.CurrentCustomerProfile);
            List<SecondBusinessVO> cVO = rDAO.FindByParams("PersonalID = " + PersonalID + " and " + "BusinessID = " + BusinessID);
            return cVO;
        }

        /// <summary>
        /// 获取绑定企业的成员列表
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public List<SecondBusinessVO> FindSecondBusinessByBusinessID(int BusinessID)
        {
            ISecondBusinessDAO rDAO = BusinessCardManagementDAOFactory.SecondBusinessDAO(this.CurrentCustomerProfile);
            List<SecondBusinessVO> cVO = rDAO.FindByParams("BusinessID = " + BusinessID);
            return cVO;
        }

        /// <summary>
        /// 获取绑定企业的部门成员列表
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public List<SecondBusinessVO> FindSecondBusinessByDepartmentID(int DepartmentID)
        {
            ISecondBusinessDAO rDAO = BusinessCardManagementDAOFactory.SecondBusinessDAO(this.CurrentCustomerProfile);
            List<SecondBusinessVO> cVO = rDAO.FindByParams("DepartmentID = " + DepartmentID);
            return cVO;
        }

        /// <summary>
        /// 获取绑定企业的成员数量
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public int FindSecondBusinessCountByBusinessID(int BusinessID)
        {
            ISecondBusinessDAO rDAO = BusinessCardManagementDAOFactory.SecondBusinessDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount("BusinessID = " + BusinessID);
        }

        /// <summary>
        /// 获取我的企业绑定列表(视图)
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public List<SecondBusinessViewVO> FindSecondBusinessViewByPersonalID(int PersonalID)
        {
            ISecondBusinessViewDAO rDAO = BusinessCardManagementDAOFactory.SecondBusinessViewDAO(this.CurrentCustomerProfile);
            List<SecondBusinessViewVO> cVO = rDAO.FindByParams("PersonalID = " + PersonalID + " GROUP BY BusinessID");
            return cVO;
        }

        /// <summary>
        /// 获取我的企业绑定列表(视图)
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public List<SecondBusinessViewVO> FindSecondBusinessViewByPersonalID(int PersonalID, int BusinessID)
        {
            ISecondBusinessViewDAO rDAO = BusinessCardManagementDAOFactory.SecondBusinessViewDAO(this.CurrentCustomerProfile);
            List<SecondBusinessViewVO> cVO = rDAO.FindByParams("PersonalID = " + PersonalID + " and " + "BusinessID = " + BusinessID);
            return cVO;
        }

        /// <summary>
        /// 获取绑定企业的成员列表(视图)
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public List<SecondBusinessViewVO> FindSecondBusinessViewByBusinessID(int BusinessID)
        {
            ISecondBusinessViewDAO rDAO = BusinessCardManagementDAOFactory.SecondBusinessViewDAO(this.CurrentCustomerProfile);
            List<SecondBusinessViewVO> cVO = rDAO.FindByParams("BusinessID = " + BusinessID + " GROUP BY PersonalID");
            return cVO;
        }

        /// <summary>
        /// 解除企业绑定
        /// </summary>
        /// <param name="InfoID"></param>
        /// <returns></returns>
        public int DeleteSecondBusinessById(int PersonalID, int BusinessID)
        {
            ISecondBusinessDAO rDAO = BusinessCardManagementDAOFactory.SecondBusinessDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("PersonalID = " + PersonalID + " and BusinessID = " + BusinessID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 加入公司（统一调用）
        /// </summary>
        /// <returns></returns>
        public bool JoinBusiness(PersonalVO pVO, int BusinessID, int AppType, bool isExternal = false)
        {
            //如果之前就加入其他公司就把当前公司改为附属
            if (pVO.BusinessID > 0 && pVO.BusinessID != BusinessID)
            {
                if (FindSecondBusinessByPersonalID(pVO.PersonalID, pVO.BusinessID).Count <= 0)
                {
                    //添加绑定
                    SecondBusinessVO sVO = new SecondBusinessVO();
                    sVO.PersonalID = pVO.PersonalID;
                    sVO.BusinessID = pVO.BusinessID;
                    sVO.DepartmentID = pVO.DepartmentID;
                    sVO.Position = pVO.Position;
                    sVO.isExternal = pVO.isExternal;
                    AddSecondBusiness(sVO);
                }
            }
            int PersonalCount = FindPersonalCountByBusinessID(BusinessID);
            pVO.BusinessID = BusinessID;
            pVO.DepartmentID = 0;
            pVO.isExternal = isExternal;
            if (UpdatePersonal(pVO))
            {
                //如果是第一个加入公司，则设置为超级管理员
                if (PersonalCount <= 0)
                {
                    DeleteJurisdiction(BusinessID, "Admin");
                    AddJurisdiction(pVO.PersonalID, BusinessID, "Admin");
                }
                pVO.QRimg = GetQRImgByHeadimg(pVO.PersonalID, AppType);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否已加入公司
        /// </summary>
        /// <returns></returns>
        public bool isJoinBusiness(PersonalVO pVO, int BusinessID)
        {
            if (pVO.BusinessID == BusinessID)
            {
                return true;
            }

            if (FindSecondBusinessByPersonalID(pVO.PersonalID, BusinessID).Count > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取企业名片详情
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public BusinessCardViewVO FindBusinessCardViewById(int BusinessID)
        {
            IBusinessCardViewDAO rDAO = BusinessCardManagementDAOFactory.BusinessCardViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(BusinessID);
        }

        /// <summary>
        /// 获取我的个人信息
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public PersonalVO FindPersonalByCustomerId(int CustomerId)
        {
            IPersonalDAO rDAO = BusinessCardManagementDAOFactory.PersonalDAO(this.CurrentCustomerProfile);
            List<PersonalVO> cVO = rDAO.FindByParams("CustomerId = " + CustomerId);

            if (cVO.Count > 0)
            {
                return cVO[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取个人信息详情
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public PersonalVO FindPersonalById(int PersonalID)
        {
            IPersonalDAO rDAO = BusinessCardManagementDAOFactory.PersonalDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(PersonalID);
        }

        /// <summary>
        /// 更新个人信息
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdatePersonal(PersonalVO vo)
        {
            IPersonalDAO rDAO = BusinessCardManagementDAOFactory.PersonalDAO(this.CurrentCustomerProfile);
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
        /// 获取个人信息列表
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public List<PersonalVO> FindPersonalByBusinessID(int BusinessID)
        {
            IPersonalDAO rDAO = BusinessCardManagementDAOFactory.PersonalDAO(this.CurrentCustomerProfile);
            List<PersonalVO> cVO = rDAO.FindByParams("BusinessID = " + BusinessID);

            List<SecondBusinessVO> sVO = FindSecondBusinessByBusinessID(BusinessID);

            for (int i = 0; i < sVO.Count; i++)
            {
                PersonalVO pVO = FindPersonalById(sVO[i].PersonalID);
                pVO.BusinessID = sVO[i].BusinessID;
                pVO.DepartmentID = sVO[i].DepartmentID;
                pVO.isExternal = sVO[i].isExternal;
                cVO.Add(pVO);
            }
            cVO.Sort((a, b) => a.Name.CompareTo(b.Name));

            return cVO;
        }

        /// <summary>
        /// 获取个人信息数量
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public int FindPersonalCountByBusinessID(int BusinessID)
        {
            IPersonalDAO rDAO = BusinessCardManagementDAOFactory.PersonalDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount("BusinessID = " + BusinessID) + FindSecondBusinessCountByBusinessID(BusinessID);
        }

        /// <summary>
        /// 获取公司成员个人信息列表(视图)
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public List<PersonalViewVO> FindPersonalViewByBusinessID(int BusinessID, int isExternal = 2)
        {
            IPersonalViewDAO rDAO = BusinessCardManagementDAOFactory.PersonalViewDAO(this.CurrentCustomerProfile);

            string sql = "BusinessID = " + BusinessID;
            if (isExternal != 2)
            {
                sql += " and isExternal=" + isExternal;
            }
            List<PersonalViewVO> cVO = rDAO.FindByParams(sql);
            List<SecondBusinessVO> sVO = FindSecondBusinessByBusinessID(BusinessID);
            for (int i = 0; i < sVO.Count; i++)
            {
                PersonalViewVO pVO = FindPersonalViewById(sVO[i].PersonalID);
                DepartmentVO dVO = FindDepartmentById(sVO[i].DepartmentID);
                pVO.Position = sVO[i].Position;
                pVO.isExternal = sVO[i].isExternal;

                if (dVO != null && dVO.DepartmentID > 0)
                {
                    pVO.DepartmentID = dVO.DepartmentID;
                    pVO.DepartmentName = dVO.DepartmentName;
                }
                else
                {
                    pVO.DepartmentID = 0;
                    pVO.DepartmentName = "";
                }

                cVO.Add(pVO);
            }
            cVO.Sort((a, b) => a.Name.CompareTo(b.Name));

            return cVO;
        }

        /// <summary>
        /// 获取某个公司成员的下属(视图)
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public List<PersonalVO> FindPersonalByPersonalID(int BusinessID, int PersonalID)
        {
            IPersonalDAO rDAO = BusinessCardManagementDAOFactory.PersonalDAO(this.CurrentCustomerProfile);
            string sql = "BusinessID = " + BusinessID + " LIMIT 0,100";//为了性能，默认取前100个成员
            List<PersonalVO> cVO = rDAO.FindByParams(sql);
            List<SecondBusinessVO> sVO = FindSecondBusinessByBusinessID(BusinessID);
            for (int i = 0; i < sVO.Count; i++)
            {
                PersonalVO pVO = FindPersonalById(sVO[i].PersonalID);
                pVO.Position = sVO[i].Position;
                pVO.isExternal = sVO[i].isExternal;
                pVO.DepartmentID = sVO[i].DepartmentID;
                cVO.Add(pVO);
            }


            //如果是管理员或有公司管理权限就返回全部
            bool isAdmin = FindJurisdiction(PersonalID, BusinessID, "Admin");
            bool isPersonnel = FindJurisdiction(PersonalID, BusinessID, "Personnel");

            if (isAdmin || isPersonnel)
            {
                cVO.Sort((a, b) => a.Name.CompareTo(b.Name));
                return cVO;
            }

            List<DepartmentVO> dVO = FindDepartmentList(BusinessID, PersonalID);
            //如果是部门主管，则可以看到下属的信息
            if (dVO.Count > 0)
            {
                List<PersonalVO> ListVO = new List<PersonalVO>();
                for (int i = 0; i < dVO.Count; i++)
                {
                    List<PersonalVO> pListVO = FindPersonalByDepartmentID(dVO[i].DepartmentID);

                    ListVO.AddRange(pListVO);
                }
                ListVO.Sort((a, b) => a.Name.CompareTo(b.Name));
                return ListVO;
            }
            else
            {
                //既不是管理员，也不是主管,就返回他自己
                List<PersonalVO> ListVO = new List<PersonalVO>();
                PersonalVO pVO = FindPersonalById(PersonalID);
                ListVO.Add(pVO);
                return ListVO;
            }

        }

        /// <summary>
        /// 获取个人信息详情(视图)
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public PersonalViewVO FindPersonalViewById(int PersonalID)
        {
            IPersonalViewDAO rDAO = BusinessCardManagementDAOFactory.PersonalViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(PersonalID);
        }

        /// <summary>
        /// 获取个人信息列表(分页)
        /// </summary>
        public List<PersonalViewVO> FindAllByPageIndexByPersonal(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IPersonalViewDAO rDAO = BusinessCardManagementDAOFactory.PersonalViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取个人信息数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindPersonalTotalCount(string condition, params object[] parameters)
        {
            IPersonalViewDAO rDAO = BusinessCardManagementDAOFactory.PersonalViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 保存分享图片
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public string GetPersonalIMG(int PersonalID)
        {
            Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.BCAPIURLByHttp + "/GenerateIMG/PersonalIMG.aspx?PersonalID=" + PersonalID, 500, 400, 500, 400);

            //保存
            string filePath = "";
            string folder = "/UploadFolder/PersonalFile/";
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".png";
            filePath = folder + newFileName;

            try
            {//删除旧图片
                PersonalVO pVO = FindPersonalById(PersonalID);
                if (pVO.PosterImg != "")
                {
                    string FilePath = pVO.PosterImg;
                    FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }
            }
            catch
            {

            }

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string PosterImg = ConfigInfo.Instance.APIURL + filePath;

            PersonalVO npVO = new PersonalVO();
            npVO.PersonalID = PersonalID;
            npVO.PosterImg = PosterImg;
            UpdatePersonal(npVO);
            return PosterImg;
        }


        /// <summary>
        /// 保存分享二维码（头像）
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public string GetQRImgByHeadimg(int PersonalID, int AppType, int BusinessID = 0, string Code = "")
        {
            PersonalVO pVO = FindPersonalById(PersonalID);
            if (BusinessID == 0)
                BusinessID = pVO.BusinessID;

            Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.BCAPIURLByHttp + "/GenerateIMG/QRIMG.aspx?PersonalID=" + PersonalID + "&BusinessID=" + BusinessID + "&Code=" + Code + "&AppType=" + AppType, 640, 640, 640, 640);

            //保存
            string filePath = "";
            string folder = "/UploadFolder/PersonalFile/";
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".png";
            filePath = folder + newFileName;

            try
            {//删除旧图片

                if (pVO.QRimg != "")
                {
                    string FilePath = pVO.QRimg;
                    FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }
            }
            catch
            {

            }

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            try
            {
                m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch
            {

            }
            string QRimg = ConfigInfo.Instance.APIURL + filePath;

            PersonalVO npVO = new PersonalVO();
            npVO.PersonalID = PersonalID;
            npVO.QRimg = QRimg;
            UpdatePersonal(npVO);
            return QRimg;
        }

        /// <summary>
        /// 保存分享二维码（海报）
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public string GetPosterIMG(int PersonalID, string url, int BusinessID, string Code, int AppType)
        {
            Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.BCAPIURLByHttp + "/GenerateIMG/PosterIMG.aspx?PersonalID=" + PersonalID + "&BusinessID=" + BusinessID + "&Posterback=" + url + "&Code=" + Code + "&AppType=" + AppType, 800, 800, 800, 800);
            //保存
            string filePath = "";
            string folder = "/UploadFolder/BusinessCardPosterFile/";
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".png";
            filePath = folder + newFileName;

            try
            {//删除旧图片
                PersonalVO pVO = FindPersonalById(PersonalID);
                if (pVO.PosterImg2 != "")
                {
                    string FilePath = pVO.PosterImg2;
                    FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }
            }
            catch
            {

            }

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string PosterImg2 = ConfigInfo.Instance.APIURL + filePath;

            PersonalVO npVO = new PersonalVO();
            npVO.PersonalID = PersonalID;
            npVO.PosterImg2 = PosterImg2;
            UpdatePersonal(npVO);
            return PosterImg2;
        }

        /// <summary>
        /// 保存分享名片图片（海报）
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public string GetPosterCardIMG(int PersonalID, int BusinessID)
        {
            Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.BCAPIURLByHttp + "/GenerateIMG/CardIMG.aspx?PersonalID=" + PersonalID + "&BusinessID=" + BusinessID, 800, 640, 800, 640);
            //保存
            string filePath = "";
            string folder = "/UploadFolder/BusinessCardCardFile/";
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".png";
            filePath = folder + newFileName;

            try
            {//删除旧图片
                PersonalVO pVO = FindPersonalById(PersonalID);
                if (pVO.PosterImg3 != "")
                {
                    string FilePath = pVO.PosterImg3;
                    FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }
            }
            catch
            {

            }

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string PosterImg3 = ConfigInfo.Instance.APIURL + filePath;

            PersonalVO npVO = new PersonalVO();
            npVO.PersonalID = PersonalID;
            npVO.PosterImg3 = PosterImg3;
            UpdatePersonal(npVO);
            return PosterImg3;
        }

        /// <summary>
        /// 获取分享二维码
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public string GetQRImg(int PersonalID, int BusinessID, string Code, int AppType)
        {
            try
            {//删除旧图片
                PersonalVO pVO = FindPersonalById(PersonalID);
                if (pVO.QRimg != "")
                {
                    string FilePath = pVO.QRimg;
                    FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }

            }
            catch
            {

            }
            string scene = PersonalID.ToString();
            if (BusinessID > 0)
                scene = PersonalID.ToString() + "," + BusinessID;


            if (Code != "")
                scene = PersonalID.ToString() + "," + BusinessID + "," + Code;

            string QRimg = GetQRcode(scene, 640, "pages/CardShow/CardShow/CardShow", "/UploadFolder/PersonalFile/", AppType);
            PersonalVO npVO = new PersonalVO();
            npVO.PersonalID = PersonalID;
            npVO.QRimg = QRimg;
            UpdatePersonal(npVO);
            return QRimg;
        }

        /// <summary>
        /// 获取信息二维码
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public string GetInfoQRimg(int InfoID, string Title, int AppType)
        {
            try
            {//删除旧图片
                InfoVO pVO = FindInfoById(InfoID);
                if (pVO.InfoQR != "")
                {
                    string FilePath = pVO.InfoQR;
                    FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }

            }
            catch
            {

            }
            string scene = InfoID.ToString();
            string QRimg = GetQRcode(scene, 640, "pages/SetUp/InfoShow/InfoShow", "/UploadFolder/InfoQRimgFile/", AppType, Title);
            InfoVO npVO = new InfoVO();
            npVO.InfoID = InfoID;
            npVO.InfoQR = QRimg;
            UpdateInfo(npVO);
            return QRimg;
        }

        /// <summary>
        /// 获取积分二维码
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public string GetIntegralQRimg(int PersonalID, int AppType)
        {
            try
            {//删除旧图片
                PersonalVO pVO = FindPersonalById(PersonalID);
                if (pVO.IntegralQRimg != "")
                {
                    string FilePath = pVO.IntegralQRimg;
                    FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }

            }
            catch
            {

            }
            string scene = PersonalID.ToString();
            string QRimg = GetQRcode(scene, 640, "pages/IntegraShow/IntegraShow", "/UploadFolder/IntegralQRimgFile/", AppType);
            PersonalVO npVO = new PersonalVO();
            npVO.PersonalID = PersonalID;
            npVO.IntegralQRimg = QRimg;
            UpdatePersonal(npVO);
            return QRimg;
        }

        /// <summary>
        /// 获取核销二维码
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public string GetOrderQRImg(int OrderID, int AppType)
        {
            try
            {//删除旧图片
                OrderVO pVO = FindOrderById(OrderID);
                if (pVO.QRImg != "")
                {
                    string FilePath = pVO.QRImg;
                    FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }

            }
            catch
            {

            }
            string scene = OrderID.ToString();
            string QRimg = GetQRcode(scene, 640, "pages/SetUp/OrderShow/OrderShow", "/UploadFolder/OrderFile/", AppType);
            OrderVO npVO = new OrderVO();
            npVO.OrderID = OrderID;
            npVO.QRImg = QRimg;
            UpdateOrder(npVO);
            return QRimg;
        }

        /// <summary>
        /// 获取彩页二维码（海报）
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public string GetColorPageIMG(int PersonalID, int BusinessID, int AppType, string Code = "")
        {
            try
            {
                //获取小程序码
                string scene = PersonalID.ToString() + "," + BusinessID;

                if (Code != "")
                    scene = PersonalID.ToString() + "," + BusinessID + "," + Code;

                string QRimg = GetQRcode(scene, 640, "pages/CardShow/ColorPage/ColorPage", "/UploadFolder/ColorPageFile/", AppType);
                Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.BCAPIURLByHttp + "/GenerateIMG/ColorPageIMG.aspx?PersonalID=" + PersonalID + "&BusinessID=" + BusinessID + "&QRimg=" + HttpUtility.UrlEncode(QRimg), 640, 880, 640, 880);
                //保存
                string filePath = "";
                string folder = "/UploadFolder/ColorPageFile/";
                string newFileName = PersonalID + "_" + BusinessID + ".png";
                filePath = folder + newFileName;

                try
                {//删除小程序码
                    if (QRimg != "")
                    {
                        string FilePath = QRimg;
                        FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                        FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                        File.Delete(FilePath);
                    }
                }
                catch
                {

                }
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string physicalPath = localPath + newFileName;
                m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);
                string PosterImg2 = ConfigInfo.Instance.APIURL + filePath;
                return PosterImg2;
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// 获取产品（海报）
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public string GetProductIMG(int PersonalID, int InfoID, int AppType, string Code = "", int isPoster = 1)
        {
            try
            {
                InfoVO Info = FindInfoById(InfoID);
                //获取小程序码
                string scene = PersonalID.ToString() + "," + Info.BusinessID + "," + InfoID;

                if (Code != "")
                    scene = PersonalID.ToString() + "," + Info.BusinessID + "," + InfoID + "," + Code;

                string QRimg = GetQRcode(scene, 640, "pages/CardShow/ProductShow/ProductShow", "/UploadFolder/ProductFile/", AppType);

                //如果不是海报就直接返回二维码
                if (isPoster == 0)
                {
                    return QRimg;
                }

                Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.BCAPIURLByHttp + "/GenerateIMG/ProductIMG.aspx?PersonalID=" + PersonalID + "&InfoID=" + InfoID + "&QRimg=" + HttpUtility.UrlEncode(QRimg), 900, 1100, 900, 1100);
                //保存
                string filePath = "";
                string folder = "/UploadFolder/ProductFile/";
                string newFileName = PersonalID + "_" + InfoID + ".png";
                filePath = folder + newFileName;

                try
                {//删除小程序码
                    if (QRimg != "")
                    {
                        string FilePath = QRimg;
                        FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                        FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                        File.Delete(FilePath);
                    }
                }
                catch
                {

                }


                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string physicalPath = localPath + newFileName;
                m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);
                string PosterImg2 = ConfigInfo.Instance.APIURL + filePath;
                return PosterImg2;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "";
            }

        }

        /// <summary>
        /// 获取加入公司二维码
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public string GetJoinQR(int BusinessID, int AppType)
        {
            try
            {//删除旧图片
                BusinessCardVO bVO = FindBusinessCardById(BusinessID);
                if (bVO.JoinQR != "" && bVO.JoinQR != null)
                {
                    string FilePath = bVO.JoinQR;
                    FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }

            }
            catch
            {

            }
            string QRimg = GetQRcode(BusinessID.ToString(), 640, "pages/Welcome/Welcome", "/UploadFolder/BusinessCardJoinFile/", AppType);
            BusinessCardVO npVO = new BusinessCardVO();
            npVO.BusinessID = BusinessID;
            npVO.JoinQR = QRimg;
            UpdateBusinessCard(npVO);
            return QRimg;
        }

        /// <summary>
        /// 获取加入集团二维码
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public string GetJoinGroupQR(int BusinessID, int AppType)
        {
            string QRimg = GetQRcode(BusinessID.ToString(), 640, "pages/SetUp/SubsidiaryJoin/SubsidiaryJoin", "/UploadFolder/BusinessCardJoinFile/", AppType);
            return QRimg;
        }

        /// <summary>
        /// 获取加入拼团二维码
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public string GetJoinGroupBuyQR(int GroupBuyID, int AppType)
        {
            string QRimg = GetQRcode(GroupBuyID.ToString(), 640, "pages/GroupBuy/JoinGroupBuy/JoinGroupBuy", "/UploadFolder/JoinGroupBuyFile/", AppType);
            return QRimg;
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="scene">页面参数</param>
        /// <param name="width">二维码宽度（px）</param>
        /// <param name="page">页面路径</param>
        /// <param name="folder">文件保存位置</param>
        /// <returns></returns>
        public string GetQRcode(string scene, int width, string page, string folder, int AppType, string Title = "")
        {
            AppVO AppVO = AppBO.GetApp(AppType);
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppVO.AppId + "&secret=" + AppVO.Secret;
            string jsonStr = HttpHelper.HtmlFromUrlGet(url);

            var result = new WeiXinAccessTokenResultDYH();
            if (jsonStr.Contains("errcode"))
            {
                var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                result.ErrorResult = errorResult;
                result.Result = false;
            }
            else
            {
                var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                result.SuccessResult = model;
                result.Result = true;
            }
            string DataJson = string.Empty;
            string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;
            DataJson = "{";
            DataJson += "\"scene\":\"" + scene + "\",";
            DataJson += string.Format("\"width\":{0},", width);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", page);//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
            DataJson += "\"line_color\":{";
            DataJson += string.Format("\"r\":\"{0}\",", "0");
            DataJson += string.Format("\"g\":\"{0}\",", "0");
            DataJson += string.Format("\"b\":\"{0}\"", "0");
            DataJson += "},";
            DataJson += "\"is_hyaline\":false";
            DataJson += "}";
            Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
            Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。
            //保存
            string filePath = "";
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);
            string QRimg = ConfigInfo.Instance.APIURL + filePath;
            if (Title == "") return QRimg;

            Bitmap m_Bitmap2 = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.BCAPIURLByHttp + "/GenerateIMG/AllQRIMG.aspx?url=" + HttpUtility.UrlEncode(QRimg) + "&text=" + HttpUtility.UrlEncode(Title) + "&AppType=" + AppType, 640, 640, 640, 640);

            try
            {//删除小程序码
                if (QRimg != "")
                {
                    string FilePath = QRimg;
                    FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }
            }
            catch
            {

            }

            //保存
            string filePath2 = "";
            string folder2 = "/UploadFolder/TitleFile/";
            string newFileName2 = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".png";
            filePath2 = folder2 + newFileName2;

            string localPath2 = ConfigInfo.Instance.UploadFolder + folder2;
            if (!Directory.Exists(localPath2))
            {
                Directory.CreateDirectory(localPath2);
            }
            string physicalPath2 = localPath2 + newFileName2;
            m_Bitmap2.Save(physicalPath2, System.Drawing.Imaging.ImageFormat.Png);
            string PosterImg2 = ConfigInfo.Instance.APIURL + filePath2;
            return PosterImg2;
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddInfo(InfoVO vo)
        {
            try
            {
                IInfoDAO rDAO = BusinessCardManagementDAOFactory.InfoDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int InfoID = rDAO.Insert(vo);
                    return InfoID;
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
        /// 更新信息
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateInfo(InfoVO vo)
        {
            IInfoDAO rDAO = BusinessCardManagementDAOFactory.InfoDAO(this.CurrentCustomerProfile);
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
        /// 更新信息
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateInfo(string data, string condition)
        {
            IInfoDAO rDAO = BusinessCardManagementDAOFactory.InfoDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.Update(data, condition);
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
        /// 删除信息
        /// </summary>
        /// <param name="InfoID"></param>
        /// <returns></returns>
        public int DeleteInfoById(int InfoID)
        {
            IInfoDAO rDAO = BusinessCardManagementDAOFactory.InfoDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("InfoID = " + InfoID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        /// 获取信息详情
        /// </summary>
        /// <param name="InfoID"></param>
        /// <returns></returns>
        public InfoVO FindInfoById(int InfoID)
        {
            IInfoDAO rDAO = BusinessCardManagementDAOFactory.InfoDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(InfoID);
        }
        /// <summary>
        /// 获取信息详情
        /// </summary>
        /// <param name="InfoID"></param>
        /// <returns></returns>
        public InfoViewVO FindInfoViewById(int InfoID)
        {
            IInfoViewDAO rDAO = BusinessCardManagementDAOFactory.InfoViewDAO(this.CurrentCustomerProfile);
            InfoViewVO IVO = rDAO.FindById(InfoID);
            IVO.ImgList = FindImgList(IVO.Content);
            return IVO;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<InfoVO> FindInfoByInfoID(string Type, int BusinessID)
        {
            IInfoDAO rDAO = BusinessCardManagementDAOFactory.InfoDAO(this.CurrentCustomerProfile);
            List<InfoVO> cVO = rDAO.FindByParams("Type='" + Type + "' and BusinessID = " + BusinessID);
            return cVO;
        }

        /// <summary>
        /// 获取信息列表（多类型）
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<InfoVO> FindInfoBycondtion(string condtion)
        {
            IInfoDAO rDAO = BusinessCardManagementDAOFactory.InfoDAO(this.CurrentCustomerProfile);
            List<InfoVO> cVO = rDAO.FindByParams(condtion);
            return cVO;
        }

        /// <summary>
        /// 获取贺卡模板详情
        /// </summary>
        /// <param name="InfoID"></param>
        /// <returns></returns>
        public GreetingCardVO FindGreetingCardById(int GreetingCardID)
        {
            IGreetingCardDAO rDAO = BusinessCardManagementDAOFactory.GreetingCardDAO(this.CurrentCustomerProfile);
            GreetingCardVO IVO = rDAO.FindById(GreetingCardID);
            return IVO;
        }
        /// <summary>
        /// 获取贺卡模板列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<GreetingCardVO> FindGreetingCard()
        {
            IGreetingCardDAO rDAO = BusinessCardManagementDAOFactory.GreetingCardDAO(this.CurrentCustomerProfile);
            List<GreetingCardVO> cVO = rDAO.FindByParams("1=1 Order By Order_info desc,GreetingCardID  desc");
            return cVO;
        }
        /// <summary>
        /// 获取贺卡模板列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<GreetingCardVO> FindGreetingCard(int PersonalID)
        {
            IGreetingCardDAO rDAO = BusinessCardManagementDAOFactory.GreetingCardDAO(this.CurrentCustomerProfile);
            List<GreetingCardVO> cVO = rDAO.FindByParams("PersonalID=" + PersonalID + " or PersonalID=0 Order By Order_info desc,GreetingCardID  desc");
            return cVO;
        }
        /// <summary>
        /// 添加贺卡
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddGreetingCard(GreetingCardVO vo)
        {
            try
            {
                IGreetingCardDAO rDAO = BusinessCardManagementDAOFactory.GreetingCardDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int GreetingCardID = rDAO.Insert(vo);
                    return GreetingCardID;
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
        /// 更新贺卡
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateGreetingCard(GreetingCardVO vo)
        {
            IGreetingCardDAO rDAO = BusinessCardManagementDAOFactory.GreetingCardDAO(this.CurrentCustomerProfile);
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
        /// 删除贺卡
        /// </summary>
        /// <param name="GreetingCardID"></param>
        /// <returns></returns>
        public int DeleteGreetingCardById(int GreetingCardID)
        {
            IGreetingCardDAO rDAO = BusinessCardManagementDAOFactory.GreetingCardDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("GreetingCardID = " + GreetingCardID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<InfoVO> FindInfoByCondtion(string condtion)
        {
            IInfoDAO rDAO = BusinessCardManagementDAOFactory.InfoDAO(this.CurrentCustomerProfile);
            List<InfoVO> cVO = rDAO.FindByParams(condtion);
            return cVO;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<InfoVO> FindInfoByInfoID(string Type, int BusinessID, int SortID)
        {
            IInfoDAO rDAO = BusinessCardManagementDAOFactory.InfoDAO(this.CurrentCustomerProfile);
            List<InfoVO> cVO = rDAO.FindAllByPageIndex("Type='" + Type + "' and BusinessID = " + BusinessID + " and SortID=" + SortID, "Order_info,CreatedAt", "desc");
            return cVO;
        }

        /// <summary>
        /// 获取信息列表（展示用）
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<InfoVO> FindInfoByInfoID(string Type, int BusinessID, int SortID, int limit)
        {
            IInfoDAO rDAO = BusinessCardManagementDAOFactory.InfoDAO(this.CurrentCustomerProfile);
            List<InfoVO> cVO = rDAO.FindAllByPageIndex("Type='" + Type + "' and BusinessID = " + BusinessID + " and SortID=" + SortID + " and Status=1", "Order_info", "desc", limit);
            return cVO;
        }

        /// <summary>
        /// 获取信息列表（展示用）
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<InfoVO> FindInfoByInfoID(string Type, int BusinessID, int SortID, string Order, int limit)
        {
            IInfoDAO rDAO = BusinessCardManagementDAOFactory.InfoDAO(this.CurrentCustomerProfile);
            List<InfoVO> cVO = rDAO.FindAllByPageIndex("Type='" + Type + "' and BusinessID = " + BusinessID + " and SortID=" + SortID + " and Status=1", Order, "desc", limit);
            return cVO;
        }

        /// <summary>
        /// 获取信息列表（展示用）
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<InfoViewVO> FindInfoViewByInfoID(string Type, int BusinessID, int SortID, string Order, int limit, string conditionStr = "")
        {
            string sql = "";
            if (conditionStr != "")
                sql = " and " + conditionStr;
            if (SortID > 0)
            {
                sql += " and SortID=" + SortID;
            }

            IInfoViewDAO rDAO = BusinessCardManagementDAOFactory.InfoViewDAO(this.CurrentCustomerProfile);

            List<InfoViewVO> cVO = rDAO.FindAllByPageIndex("Type='" + Type + "' and BusinessID = " + BusinessID + " and Status=1" + sql, Order, "", limit);
            for (int i = 0; i < cVO.Count; i++)
            {
                cVO[i].ImgList = FindImgList(cVO[i].Content);
            }
            return cVO;
        }

        /// <summary>
        /// 获取信息列表（CRM评论）
        /// </summary>
        /// <returns></returns>
        public List<InfoViewVO> FindInfoViewByInfoID(int CrmID, int limit)
        {
            IInfoViewDAO rDAO = BusinessCardManagementDAOFactory.InfoViewDAO(this.CurrentCustomerProfile);
            List<InfoViewVO> cVO = rDAO.FindAllByPageIndex("Type='Comment' and CrmID=" + CrmID + " and Status>=1", "CreatedAt", "asc", limit);
            return cVO;
        }

        public List<String> FindImgList(string Content)
        {
            string input = Content;
            string pattern = @"src='(?<href>[^>\s]*)'";
            List<String> ImgList = new List<string>();

            foreach (System.Text.RegularExpressions.Match match in Regex.Matches(input, pattern))
            {
                if (ImgList.Count < 3)
                {
                    string url = match.Groups["href"].Value;
                    if (url.Contains("mp4"))
                    {
                        try
                        {
                            string p = @"src='" + url + "' Subsidiary='(?<img>[^>\\s]*)'";
                            url = Regex.Matches(input, p)[0].Groups["img"].Value;
                            ImgList.Add(url);
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        ImgList.Add(url);
                    }


                }

            }

            return ImgList;
        }

        /// <summary>
        /// 获取信息列表（分页）
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<InfoViewVO> FindInfoViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IInfoViewDAO rDAO = BusinessCardManagementDAOFactory.InfoViewDAO(this.CurrentCustomerProfile);
            List<InfoViewVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            for (int i = 0; i < cVO.Count; i++)
            {
                cVO[i].ImgList = FindImgList(cVO[i].Content);
            }
            return cVO;
        }

        /// <summary>
        /// 获取信息数量
        /// </summary>
        /// <returns></returns>
        public int FindInfoViewCount(string condition)
        {
            IInfoViewDAO rDAO = BusinessCardManagementDAOFactory.InfoViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 添加信息分类
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddInfoSort(InfoSortVO vo)
        {
            try
            {
                IInfoSortDAO rDAO = BusinessCardManagementDAOFactory.InfoSortDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int SortID = rDAO.Insert(vo);
                    return SortID;
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
        /// 更新信息分类
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateInfoSort(InfoSortVO vo)
        {
            IInfoSortDAO rDAO = BusinessCardManagementDAOFactory.InfoSortDAO(this.CurrentCustomerProfile);
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
        /// 删除信息分类
        /// </summary>
        /// <param name="InfoID"></param>
        /// <returns></returns>
        public int DeleteInfoSortById(int SortID)
        {
            IInfoDAO iDAO = BusinessCardManagementDAOFactory.InfoDAO(this.CurrentCustomerProfile);
            IInfoSortDAO rDAO = BusinessCardManagementDAOFactory.InfoSortDAO(this.CurrentCustomerProfile);
            try
            {
                iDAO.Update("SortID=0", "SortID = " + SortID);
                rDAO.DeleteByParams("SortID = " + SortID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 删除子分类
        /// </summary>
        /// <param name="InfoID"></param>
        /// <returns></returns>
        public int DeleteInfoSortByToid(int SortID)
        {
            IInfoSortDAO rDAO = BusinessCardManagementDAOFactory.InfoSortDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("Toid = " + SortID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        /// 获取信息分类详情
        /// </summary>
        /// <param name="InfoID"></param>
        /// <returns></returns>
        public InfoSortVO FindInfoSortById(int SortID)
        {
            IInfoSortDAO rDAO = BusinessCardManagementDAOFactory.InfoSortDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(SortID);
        }
        /// <summary>
        /// 获取信息分类列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<InfoSortVO> FindInfoSortList(string Type, int BusinessID)
        {
            IInfoSortDAO rDAO = BusinessCardManagementDAOFactory.InfoSortDAO(this.CurrentCustomerProfile);
            List<InfoSortVO> cVO = rDAO.FindByParams("Type='" + Type + "' and BusinessID = " + BusinessID);
            return cVO;
        }

        /// <summary>
        /// 获取信息分类列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<InfoSortVO> FindInfoSortList(string condtion)
        {
            IInfoSortDAO rDAO = BusinessCardManagementDAOFactory.InfoSortDAO(this.CurrentCustomerProfile);
            List<InfoSortVO> cVO = rDAO.FindByParams(condtion);
            return cVO;
        }

        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<InfoSortVO> FindNavigationList(int BusinessID, int Toid = 0)
        {
            IInfoSortDAO rDAO = BusinessCardManagementDAOFactory.InfoSortDAO(this.CurrentCustomerProfile);
            List<InfoSortVO> cVO = rDAO.FindByParams("Type='Navigation' and BusinessID = " + BusinessID + " and Toid=" + Toid + " ORDER BY orderno desc,CreatedAt asc");
            return cVO;
        }

        /// <summary>
        /// 获取官网模块列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<InfoSortVO> FindInfoSortList(int BusinessID)
        {
            IInfoSortDAO rDAO = BusinessCardManagementDAOFactory.InfoSortDAO(this.CurrentCustomerProfile);
            List<InfoSortVO> cVO = rDAO.FindAllByPageIndex("(Type='ModularInfo' or Type='ModularList' or Type='ModularNews' or Type='ModularVideo' or Type='Modular360') and BusinessID = " + BusinessID, "orderno", "asc");
            return cVO;
        }

        /// <summary>
        /// 获取官网数据VO
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public WebVO FindWebByBusinessID(int BusinessID)
        {
            IBusinessCardDAO rDAO = BusinessCardManagementDAOFactory.BusinessCardDAO(this.CurrentCustomerProfile);
            BusinessCardVO bVO = rDAO.FindById(BusinessID);

            if (bVO.HeadquartersID > 0)
            {
                bVO = rDAO.FindById(bVO.HeadquartersID);
            }

            if (bVO == null)
                return null;

            WebVO WebVO = new WebVO();
            WebVO.BusinessID = bVO.BusinessID;
            WebVO.BusinessName = bVO.BusinessName;
            WebVO.CustomColumn = bVO.CustomColumn;
            WebVO.ProductColumn = bVO.ProductColumn;
            WebVO.Industry = bVO.Industry;
            WebVO.Address = bVO.Address;
            WebVO.latitude = bVO.latitude;
            WebVO.longitude = bVO.longitude;
            WebVO.LogoImg = bVO.LogoImg;
            WebVO.Tel = bVO.Tel;
            WebVO.DisplayCard = bVO.DisplayCard;
            if (BusinessID == 330)
            {
                WebVO.banner = FindInfoByInfoID("MallBanner", bVO.BusinessID);
            }
            else { WebVO.banner = FindInfoByInfoID("Banner", bVO.BusinessID); }
            WebVO.isAddress = bVO.isAddress;
            WebVO.isTel = bVO.isTel;

            if (BusinessID == 330 || BusinessID == 336)
            {
                WebVO.NewsSort = FindInfoSortList("special_column", bVO.BusinessID);
            }
            else
            {
                WebVO.NewsSort = FindInfoSortList("NewsSort", bVO.BusinessID);
            }
            InfoSortVO alls = new InfoSortVO();
            alls.SortID = 0;
            alls.SortName = "全部";
            WebVO.NewsSort.Add(alls);
            WebVO.NewsSort.Reverse();

            List<InfoSortVO> InfoSort = FindInfoSortList(bVO.BusinessID);
            for (int i = 0; i < InfoSort.Count; i++)
            {
                if (InfoSort[i].Type == "ModularList")
                {
                    InfoSort[i].Infolist = FindInfoByInfoID("ModularList", bVO.BusinessID, InfoSort[i].SortID);
                }
                if (InfoSort[i].Type == "ModularNews")
                {
                    if (BusinessID == 330 || BusinessID == 336)
                    {
                        InfoSort[i].InfoViewlist = FindInfoViewByInfoID("Case", bVO.BusinessID, InfoSort[i].Toid, "Order_info desc,CreatedAt desc", 3);
                    }
                    else
                    {
                        InfoSort[i].InfoViewlist = FindInfoViewByInfoID("News", bVO.BusinessID, InfoSort[i].Toid, "Order_info desc,CreatedAt desc", 3);
                    }

                }
            }
            WebVO.Modular = InfoSort;
            return WebVO;
        }

        /// <summary>
        /// 获取官网数据VO
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public WebVO FindYSQWebByBusinessID(int BusinessID)
        {
            IBusinessCardDAO rDAO = BusinessCardManagementDAOFactory.BusinessCardDAO(this.CurrentCustomerProfile);
            BusinessCardVO bVO = rDAO.FindById(BusinessID);

            if (bVO.HeadquartersID > 0)
            {
                bVO = rDAO.FindById(bVO.HeadquartersID);
            }

            if (bVO == null)
                return null;

            WebVO WebVO = new WebVO();
            WebVO.BusinessID = bVO.BusinessID;
            WebVO.BusinessName = bVO.BusinessName;
            WebVO.CustomColumn = bVO.CustomColumn;
            WebVO.ProductColumn = bVO.ProductColumn;
            WebVO.Industry = bVO.Industry;
            WebVO.Address = bVO.Address;
            WebVO.latitude = bVO.latitude;
            WebVO.longitude = bVO.longitude;
            WebVO.LogoImg = bVO.LogoImg;
            WebVO.Tel = bVO.Tel;
            WebVO.DisplayCard = bVO.DisplayCard;
            WebVO.banner = FindInfoByInfoID("Banner", bVO.BusinessID);
            WebVO.isAddress = bVO.isAddress;
            WebVO.isTel = bVO.isTel;
            WebVO.NewsSort = FindInfoSortList("NewsSort", bVO.BusinessID);
            InfoSortVO alls = new InfoSortVO();
            alls.SortID = 0;
            alls.SortName = "全部";
            WebVO.NewsSort.Add(alls);
            WebVO.NewsSort.Reverse();

            List<InfoSortVO> InfoSort = FindInfoSortList(bVO.BusinessID);
            for (int i = 0; i < InfoSort.Count; i++)
            {
                if (InfoSort[i].Type == "ModularList")
                {
                    InfoSort[i].Infolist = FindInfoByInfoID("ModularList", bVO.BusinessID, InfoSort[i].SortID);
                }
                if (InfoSort[i].Type == "ModularNews")
                {
                    InfoSort[i].InfoViewlist = FindInfoViewByInfoID("News", bVO.BusinessID, InfoSort[i].Toid, "Order_info desc,CreatedAt desc", 3);
                }
            }
            WebVO.Modular = InfoSort;
            return WebVO;
        }

        /// <summary>
        /// 上传官网数据VO
        /// </summary>
        /// <param name="WebVO"></param>
        /// <returns></returns>
        public bool UpdateWeb(WebVO WebVO)
        {
            if (WebVO == null)
                return false;

            BusinessCardVO bVO = new BusinessCardVO();
            bVO.BusinessID = WebVO.BusinessID;
            bVO.BusinessName = WebVO.BusinessName;
            bVO.Industry = WebVO.Industry;
            bVO.Address = WebVO.Address;
            bVO.latitude = WebVO.latitude;
            bVO.longitude = WebVO.longitude;
            bVO.LogoImg = WebVO.LogoImg;
            bVO.Tel = WebVO.Tel;
            bVO.isAddress = WebVO.isAddress;
            bVO.isTel = WebVO.isTel;

            try
            {
                UpdateBusinessCard(bVO);
            }
            catch
            {

            }

            //先删除不存在的banner
            List<InfoVO> oldinfo = FindInfoByInfoID("Banner", bVO.BusinessID);
            for (int k = 0; k < oldinfo.Count; k++)
            {
                try
                {
                    if (WebVO.banner.Find(delegate (InfoVO s) { if (s.InfoID == oldinfo[k].InfoID) return true; else return false; }) == null)
                    {
                        DeleteInfoById(oldinfo[k].InfoID);
                    }
                }
                catch
                {

                }
            }

            for (int i = 0; i < WebVO.banner.Count; i++)
            {
                try
                {
                    InfoVO iVO = new InfoVO();
                    iVO.InfoID = WebVO.banner[i].InfoID;
                    iVO.Type = "Banner";
                    iVO.Order_info = WebVO.banner.Count - i - 1;
                    iVO.BusinessID = WebVO.BusinessID;
                    iVO.Image = WebVO.banner[i].Image;
                    if (iVO.InfoID > 0)
                    {
                        UpdateInfo(iVO);
                    }
                    else
                    {
                        iVO.CreatedAt = DateTime.Now;
                        AddInfo(iVO);
                    }
                }
                catch
                {

                }
            }

            //先删除不存在的模块
            List<InfoSortVO> InfoSort = FindInfoSortList(bVO.BusinessID);
            for (int k = 0; k < InfoSort.Count; k++)
            {
                try
                {
                    if (WebVO.Modular.Find(delegate (InfoSortVO s) { if (s.SortID == InfoSort[k].SortID) return true; else return false; }) == null)
                    {
                        DeleteInfoSortById(InfoSort[k].SortID);
                    }
                }
                catch
                {

                }
            }

            for (int i = 0; i < WebVO.Modular.Count; i++)
            {
                try
                {
                    InfoSortVO iVO = new InfoSortVO();
                    iVO.SortID = WebVO.Modular[i].SortID;
                    iVO.Toid = WebVO.Modular[i].Toid;
                    iVO.Type = WebVO.Modular[i].Type;
                    iVO.orderno = i;
                    iVO.BusinessID = WebVO.BusinessID;
                    iVO.SortName = WebVO.Modular[i].SortName;
                    iVO.Image = WebVO.Modular[i].Image;
                    iVO.Remark = WebVO.Modular[i].Remark;

                    int SortID = iVO.SortID;
                    if (SortID > 0)
                    {
                        UpdateInfoSort(iVO);
                    }
                    else
                    {
                        iVO.CreatedAt = DateTime.Now;
                        SortID = AddInfoSort(iVO);
                    }

                    if (WebVO.Modular[i].Type == "ModularList")
                    {
                        //先删除不存在的
                        List<InfoVO> old = FindInfoByInfoID("ModularList", bVO.BusinessID, SortID);
                        for (int k = 0; k < old.Count; k++)
                        {
                            try
                            {
                                if (WebVO.Modular[i].Infolist.Find(delegate (InfoVO s) { if (s.InfoID == old[k].InfoID) return true; else return false; }) == null)
                                {
                                    DeleteInfoById(old[k].InfoID);
                                }
                            }
                            catch
                            {

                            }
                        }

                        for (int j = 0; j < WebVO.Modular[i].Infolist.Count; j++)
                        {
                            try
                            {

                                InfoVO iiVO = new InfoVO();
                                iiVO.InfoID = WebVO.Modular[i].Infolist[j].InfoID;
                                iiVO.Type = "ModularList";

                                //排序颠倒
                                iiVO.Order_info = WebVO.Modular[i].Infolist.Count - j - 1;
                                iiVO.BusinessID = WebVO.BusinessID;
                                iiVO.SortID = SortID;
                                iiVO.Title = WebVO.Modular[i].Infolist[j].Title;
                                iiVO.Image = WebVO.Modular[i].Infolist[j].Image;
                                if (iiVO.InfoID > 0)
                                {
                                    UpdateInfo(iiVO);
                                }
                                else
                                {
                                    iiVO.CreatedAt = DateTime.Now;
                                    AddInfo(iiVO);
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                }
                catch
                {

                }
            }
            return true;
        }

        /// <summary>
        /// 更新轮播图
        /// </summary>
        /// <param name="WebVO"></param>
        /// <returns></returns>
        public bool UpdateSwiper(WebVO WebVO)
        {
            if (WebVO == null)
                return false;

            //先删除不存在的banner
            List<InfoVO> oldinfo = FindInfoBycondtion("Type IN ('Banner','MyBanner','MallBanner') AND BusinessID=" + WebVO.BusinessID);
            for (int k = 0; k < oldinfo.Count; k++)
            {
                try
                {
                    if (WebVO.banner.Find(delegate (InfoVO s) { if (s.InfoID == oldinfo[k].InfoID) return true; else return false; }) == null)
                    {
                        DeleteInfoById(oldinfo[k].InfoID);
                    }
                }
                catch
                {

                }
            }
            for (int i = 0; i < WebVO.banner.Count; i++)
            {
                try
                {
                    InfoVO iVO = new InfoVO();
                    iVO.InfoID = WebVO.banner[i].InfoID;
                    iVO.Type = WebVO.banner[i].Type;
                    iVO.Order_info = WebVO.banner.Count - i - 1;
                    iVO.BusinessID = WebVO.BusinessID;
                    iVO.Image = WebVO.banner[i].Image;
                    iVO.JumpAddress = WebVO.banner[i].JumpAddress;
                    if (iVO.InfoID > 0)
                    {
                        UpdateInfo(iVO);
                    }
                    else
                    {
                        iVO.CreatedAt = DateTime.Now;
                        AddInfo(iVO);
                    }
                }
                catch
                {

                }
            }




            return true;
        }

        /// <summary>
        /// 复制官网数据VO
        /// </summary>
        /// <param name="WebVO"></param>
        /// <param name="BusinessID">复制源企业</param>
        /// <param name="toBusinessID">目标企业</param>
        /// <returns></returns>
        public bool CopyWeb(int BusinessID, int toBusinessID)
        {

            WebVO WebVO = FindWebByBusinessID(BusinessID);
            if (WebVO == null)
                return false;

            BusinessCardVO bVO = new BusinessCardVO();
            bVO.BusinessID = toBusinessID;
            bVO.Industry = WebVO.Industry;
            bVO.Address = WebVO.Address;
            bVO.latitude = WebVO.latitude;
            bVO.longitude = WebVO.longitude;
            bVO.LogoImg = WebVO.LogoImg;
            bVO.Tel = WebVO.Tel;
            bVO.isAddress = WebVO.isAddress;
            bVO.isTel = WebVO.isTel;

            try
            {
                UpdateBusinessCard(bVO);
            }
            catch
            {

            }

            //先删除不存在的banner
            List<InfoVO> oldinfo = FindInfoByInfoID("Banner", bVO.BusinessID);
            for (int k = 0; k < oldinfo.Count; k++)
            {
                try
                {
                    if (WebVO.banner.Find(delegate (InfoVO s) { if (s.InfoID == oldinfo[k].InfoID) return true; else return false; }) == null)
                    {
                        DeleteInfoById(oldinfo[k].InfoID);
                    }
                }
                catch
                {

                }
            }

            for (int i = 0; i < WebVO.banner.Count; i++)
            {
                try
                {
                    InfoVO iVO = new InfoVO();
                    iVO.InfoID = 0;
                    iVO.Type = "Banner";
                    iVO.Order_info = WebVO.banner.Count - i - 1;
                    iVO.BusinessID = bVO.BusinessID;
                    iVO.Image = WebVO.banner[i].Image;
                    if (iVO.InfoID > 0)
                    {
                        UpdateInfo(iVO);
                    }
                    else
                    {
                        iVO.CreatedAt = DateTime.Now;
                        AddInfo(iVO);
                    }
                }
                catch
                {

                }
            }

            //先删除不存在的模块
            List<InfoSortVO> InfoSort = FindInfoSortList(bVO.BusinessID);
            for (int k = 0; k < InfoSort.Count; k++)
            {
                try
                {
                    if (WebVO.Modular.Find(delegate (InfoSortVO s) { if (s.SortID == InfoSort[k].SortID) return true; else return false; }) == null)
                    {
                        DeleteInfoSortById(InfoSort[k].SortID);
                    }
                }
                catch
                {

                }
            }

            for (int i = 0; i < WebVO.Modular.Count; i++)
            {
                try
                {
                    InfoSortVO iVO = new InfoSortVO();
                    iVO.SortID = 0;
                    iVO.Toid = WebVO.Modular[i].Toid;
                    iVO.Type = WebVO.Modular[i].Type;
                    iVO.orderno = i;
                    iVO.BusinessID = bVO.BusinessID;
                    iVO.SortName = WebVO.Modular[i].SortName;
                    iVO.Image = WebVO.Modular[i].Image;
                    iVO.Remark = WebVO.Modular[i].Remark;
                    iVO.Content = WebVO.Modular[i].Content;

                    int SortID = iVO.SortID;
                    if (SortID > 0)
                    {
                        UpdateInfoSort(iVO);
                    }
                    else
                    {
                        iVO.CreatedAt = DateTime.Now;
                        SortID = AddInfoSort(iVO);
                    }

                    if (WebVO.Modular[i].Type == "ModularList")
                    {
                        //先删除不存在的
                        List<InfoVO> old = FindInfoByInfoID("ModularList", bVO.BusinessID, SortID);
                        for (int k = 0; k < old.Count; k++)
                        {
                            try
                            {
                                if (WebVO.Modular[i].Infolist.Find(delegate (InfoVO s) { if (s.InfoID == old[k].InfoID) return true; else return false; }) == null)
                                {
                                    DeleteInfoById(old[k].InfoID);
                                }
                            }
                            catch
                            {

                            }
                        }

                        for (int j = 0; j < WebVO.Modular[i].Infolist.Count; j++)
                        {
                            try
                            {

                                InfoVO iiVO = new InfoVO();
                                iiVO.InfoID = 0;
                                iiVO.Type = "ModularList";

                                //排序颠倒
                                iiVO.Order_info = WebVO.Modular[i].Infolist.Count - j - 1;
                                iiVO.BusinessID = bVO.BusinessID;
                                iiVO.SortID = SortID;
                                iiVO.Title = WebVO.Modular[i].Infolist[j].Title;
                                iiVO.Image = WebVO.Modular[i].Infolist[j].Image;
                                iiVO.Content = WebVO.Modular[i].Infolist[j].Content;
                                if (iiVO.InfoID > 0)
                                {
                                    UpdateInfo(iiVO);
                                }
                                else
                                {
                                    iiVO.CreatedAt = DateTime.Now;
                                    AddInfo(iiVO);
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                }
                catch
                {

                }
            }
            return true;
        }

        /// <summary>
        /// 复制官网信息VO
        /// </summary>
        /// <param name="WebVO"></param>
        /// <param name="BusinessID">复制源企业</param>
        /// <param name="toBusinessID">目标企业</param>
        /// <returns></returns>
        public bool CopyInfo(int BusinessID, int toBusinessID)
        {
            List<InfoVO> dVO = FindInfoByCondtion("(Type='News' or Type='Video' or Type='Case') and BusinessID=" + BusinessID + " and Status=1");
            foreach (InfoVO vo in dVO)
            {
                InfoVO iiVO = new InfoVO();
                iiVO.InfoID = 0;
                iiVO.Type = vo.Type;

                //排序颠倒
                iiVO.Order_info = vo.Order_info;
                iiVO.PersonalID = vo.PersonalID;
                iiVO.BusinessID = toBusinessID;
                iiVO.SortID = 0;
                iiVO.Title = vo.Title;
                iiVO.Image = vo.Image;
                iiVO.Content = vo.Content;
                iiVO.CreatedAt = DateTime.Now;
                iiVO.Status = vo.Status;
                AddInfo(iiVO);
            }
            return true;
        }


        /// <summary>
        /// 复制产品信息VO
        /// </summary>
        /// <param name="WebVO"></param>
        /// <param name="BusinessID">复制源企业</param>
        /// <param name="toBusinessID">目标企业</param>
        /// <returns></returns>
        public bool CopyProducts(int BusinessID, int toBusinessID)
        {
            List<InfoVO> dVO = FindInfoByCondtion("Type='Products' and BusinessID=" + BusinessID + " and Status=1");
            foreach (InfoVO vo in dVO)
            {
                InfoVO iiVO = new InfoVO();
                iiVO.InfoID = 0;
                iiVO.SortID = 0;
                iiVO.Type = vo.Type;

                //排序颠倒
                iiVO.Order_info = vo.Order_info;
                iiVO.PersonalID = vo.PersonalID;
                iiVO.BusinessID = toBusinessID;
                iiVO.Title = vo.Title;
                iiVO.Image = vo.Image;
                iiVO.Content = vo.Content;
                iiVO.CreatedAt = DateTime.Now;
                iiVO.Status = vo.Status;
                iiVO.Cost = vo.Cost;
                iiVO.CostName = vo.CostName;
                iiVO.isCost = vo.isCost;

                int InfoID = AddInfo(iiVO);

                if (InfoID > 0)
                {
                    List<InfoCostVO> InfoCostList = FindInfoCostList("Status=1 and InfoID=" + vo.InfoID);
                    if (InfoCostList != null)
                    {
                        foreach (InfoCostVO CostVO in InfoCostList)
                        {
                            InfoCostVO InfoCostVO = new InfoCostVO();
                            InfoCostVO.CostID = 0;
                            InfoCostVO.Cost = CostVO.Cost;
                            InfoCostVO.InfoID = InfoID;
                            InfoCostVO.CostName = CostVO.CostName;
                            InfoCostVO.CreatedAt = CostVO.CreatedAt;
                            InfoCostVO.Status = 1;
                            InfoCostVO.PerPersonLimit = CostVO.PerPersonLimit;
                            InfoCostVO.Attribute = CostVO.Attribute;

                            AddInfoCost(InfoCostVO);
                        }
                    }
                }

            }
            return true;
        }

        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddDepartment(DepartmentVO vo)
        {
            try
            {
                IDepartmentDAO rDAO = BusinessCardManagementDAOFactory.DepartmentDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int DepartmentID = rDAO.Insert(vo);
                    return DepartmentID;
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
        /// 更新部门
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateDepartment(DepartmentVO vo)
        {
            IDepartmentDAO rDAO = BusinessCardManagementDAOFactory.DepartmentDAO(this.CurrentCustomerProfile);
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
        /// 删除部门
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public int DeleteDepartmentById(int DepartmentID)
        {
            IDepartmentDAO rDAO = BusinessCardManagementDAOFactory.DepartmentDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("DepartmentID = " + DepartmentID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        /// 获取部门详情
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public DepartmentVO FindDepartmentById(int DepartmentID)
        {
            IDepartmentDAO rDAO = BusinessCardManagementDAOFactory.DepartmentDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(DepartmentID);
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public List<DepartmentVO> FindDepartmentList(int BusinessID)
        {
            IDepartmentDAO rDAO = BusinessCardManagementDAOFactory.DepartmentDAO(this.CurrentCustomerProfile);
            List<DepartmentVO> cVO = rDAO.FindByParams("BusinessID = " + BusinessID + " Order By Order_info desc");
            return cVO;
        }

        /// <summary>
        /// 获取主管的部门列表
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public List<DepartmentVO> FindDepartmentList(int BusinessID, int PersonalID)
        {
            IDepartmentDAO rDAO = BusinessCardManagementDAOFactory.DepartmentDAO(this.CurrentCustomerProfile);
            List<DepartmentVO> cVO = rDAO.FindByParams("BusinessID = " + BusinessID + " and DirectorPersonalID = " + PersonalID);
            return cVO;
        }

        /// <summary>
        /// 获取部门成员列表
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public List<PersonalVO> FindPersonalByDepartmentID(int DepartmentID)
        {
            IPersonalDAO rDAO = BusinessCardManagementDAOFactory.PersonalDAO(this.CurrentCustomerProfile);
            List<PersonalVO> cVO = rDAO.FindByParams("DepartmentID = " + DepartmentID);

            List<SecondBusinessVO> sVO = FindSecondBusinessByDepartmentID(DepartmentID);

            for (int i = 0; i < sVO.Count; i++)
            {
                PersonalVO pVO = FindPersonalById(sVO[i].PersonalID);
                pVO.BusinessID = sVO[i].BusinessID;
                pVO.DepartmentID = sVO[i].DepartmentID;
                pVO.isExternal = sVO[i].isExternal;
                cVO.Add(pVO);
            }
            return cVO;
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddJurisdiction(int PersonalID, int BusinessID, string Type)
        {
            JurisdictionVO vo = new JurisdictionVO();
            vo.BusinessID = BusinessID;
            vo.PersonalID = PersonalID;
            vo.Type = Type;
            try
            {
                IJurisdictionDAO rDAO = BusinessCardManagementDAOFactory.JurisdictionDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int DepartmentID = rDAO.Insert(vo);
                    return DepartmentID;
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
        /// 删除权限
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public int DeleteJurisdiction(int PersonalID, int BusinessID, string Type)
        {
            IJurisdictionDAO rDAO = BusinessCardManagementDAOFactory.JurisdictionDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("PersonalID = " + PersonalID + " and BusinessID=" + BusinessID + " and Type='" + Type + "'");
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public int DeleteJurisdiction(int BusinessID, string Type)
        {
            IJurisdictionDAO rDAO = BusinessCardManagementDAOFactory.JurisdictionDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("BusinessID=" + BusinessID + " and Type='" + Type + "'");
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public int DeleteJurisdiction(int PersonalID, int BusinessID)
        {
            IJurisdictionDAO rDAO = BusinessCardManagementDAOFactory.JurisdictionDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("PersonalID = " + PersonalID + " and BusinessID=" + BusinessID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 查询是否具备权限
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public bool FindJurisdiction(int PersonalID, int BusinessID, string Type)
        {
            if (BusinessID == 73)
            {
                return true;
            }

            IJurisdictionDAO rDAO = BusinessCardManagementDAOFactory.JurisdictionDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount("PersonalID = " + PersonalID + " and BusinessID=" + BusinessID + " and Type='" + Type + "'") > 0;
        }

        /// <summary>
        /// 获取公司的管理员
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public int FindJurisdiction(int BusinessID, int isCustomerId = 1)
        {
            IJurisdictionDAO rDAO = BusinessCardManagementDAOFactory.JurisdictionDAO(this.CurrentCustomerProfile);
            List<JurisdictionVO> jVO = rDAO.FindByParams("BusinessID=" + BusinessID + " and Type='Admin'");
            if (jVO.Count > 0)
            {
                if (isCustomerId == 1)
                {
                    PersonalVO pVO = FindPersonalById(jVO[0].PersonalID);
                    if (pVO != null)
                    {
                        return pVO.CustomerId;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return jVO[0].PersonalID;
                }

            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取个人所有权限
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public JurisdictionViewVO FindJurisdictionView(int PersonalID, int BusinessID)
        {
            JurisdictionViewVO jVO = new JurisdictionViewVO();
            if (BusinessID == 73)
            {
                jVO.Admin = true;
                jVO.Web = true;
                jVO.Product = true;
                jVO.Clients = true;
                jVO.Performance = true;
                jVO.Personnel = true;
                jVO.Order = true;
                jVO.CloudCall = true;
                jVO.AddAgent = true;
                return jVO;
            }
            PersonalVO pVO = FindPersonalById(PersonalID);

            if (pVO.isExternal)
            {
                jVO.Admin = false;
                jVO.Web = false;
                jVO.Product = false;
                jVO.Clients = false;
                jVO.Performance = false;
                jVO.Personnel = false;
                jVO.CloudCall = false;
                jVO.Order = true;
                jVO.AddAgent = false;
                return jVO;
            }

            jVO.Admin = FindJurisdiction(PersonalID, BusinessID, "Admin");
            jVO.Web = FindJurisdiction(PersonalID, BusinessID, "Web");
            jVO.Product = FindJurisdiction(PersonalID, BusinessID, "Product");
            jVO.Clients = FindJurisdiction(PersonalID, BusinessID, "Clients");
            jVO.Performance = FindJurisdiction(PersonalID, BusinessID, "Performance");
            jVO.Personnel = FindJurisdiction(PersonalID, BusinessID, "Personnel");
            jVO.Order = FindJurisdiction(PersonalID, BusinessID, "Order");
            jVO.CloudCall = FindJurisdiction(PersonalID, BusinessID, "CloudCall");
            jVO.AddAgent = FindJurisdiction(PersonalID, BusinessID, "AddAgent");
            return jVO;
        }

        /// <summary>
        /// 添加销售目标
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddTarget(TargetVO vo)
        {
            try
            {
                ITargetDAO rDAO = BusinessCardManagementDAOFactory.TargetDAO(this.CurrentCustomerProfile);
                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int TargetID = rDAO.Insert(vo);
                    return TargetID;
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
        /// 更新销售目标
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateTarget(TargetVO vo)
        {
            ITargetDAO rDAO = BusinessCardManagementDAOFactory.TargetDAO(this.CurrentCustomerProfile);
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
        /// 获取销售目标详情
        /// </summary>
        /// <param name="TargetID"></param>
        /// <returns></returns>
        public TargetVO FindTargetByTargetID(int TargetID)
        {
            ITargetDAO rDAO = BusinessCardManagementDAOFactory.TargetDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(TargetID);
        }

        /// <summary>
        /// 获取销售目标列表
        /// </summary>
        /// <param name="TargetID"></param>
        /// <returns></returns>
        public List<TargetVO> FindTargetByCondtion(string condtion)
        {
            ITargetDAO rDAO = BusinessCardManagementDAOFactory.TargetDAO(this.CurrentCustomerProfile);
            List<TargetVO> cVO = rDAO.FindByParams(condtion);
            return cVO;
        }

        /// <summary>
        /// 添加访问记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddAccessrecords(AccessrecordsVO vo)
        {
            try
            {
                IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int AccessRecordsID = rDAO.Insert(vo);
                    return AccessRecordsID;
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
        /// 删除访问记录
        /// </summary>
        /// <param name="AgentLevelID"></param>
        /// <returns></returns>
        public int DeleteAccessrecords(int ToPersonalID, int PersonalID)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("ToPersonalID = " + ToPersonalID + " and PersonalID=" + PersonalID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取访问记录列表
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public List<AccessrecordsVO> FindAccessrecordsByCondtion(string condtion)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);
            List<AccessrecordsVO> cVO = rDAO.FindByParams(condtion);
            return cVO;
        }

        /// <summary>
        /// 更新访问记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateAccessrecords(AccessrecordsVO vo)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);
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
        /// 获取访问记录详情
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public AccessrecordsVO FindAccessrecordsByAccessrecordsID(int AccessrecordsID)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(AccessrecordsID);
        }

        /// <summary>
        /// 获取访问记录人数
        /// </summary>
        /// <returns></returns>
        /// <param name="Mode">查询方式，0：针对信息的访问数据，1：针对人员的访问数据</param>
        public int FindNumberOfVisitors(string Type, int ById, int Mode = 0)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);

            string sql = " and ById = " + ById;
            if (Mode == 1)
            {
                sql = " and ToPersonalID = " + ById;
            }

            List<AccessrecordsVO> aVO = rDAO.FindAllByPageIndex("Type = '" + Type + "'" + sql + " GROUP BY PersonalID", "AccessAt", "desc");
            return aVO.Count;
        }

        /// <summary>
        /// 获取访问记录人数（时间）TimeType：1:当天数据；2:7天内数据；3:30天内数据
        /// </summary>
        /// <returns></returns>
        /// <param name="Mode">查询方式，0：针对信息的访问数据，1：针对人员的访问数据</param>
        public int FindNumberOfVisitors(string Type, int ById, int TimeType, int Mode = 0)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);

            string sqlTime = "";
            if (TimeType == 1)
            {
                sqlTime = " and to_days(AccessAt) = to_days(now())";
            }
            if (TimeType == 2)
            {
                sqlTime = " and DATE_SUB(CURDATE(), INTERVAL 7 DAY) <= date(AccessAt)";
            }
            if (TimeType == 3)
            {
                sqlTime = " and DATE_SUB(CURDATE(), INTERVAL 1 MONTH) <= date(AccessAt)";
            }

            string sql = "and ToPersonalID<>PersonalID and ById = " + ById;
            if (Mode == 1)
            {
                sql = " and ToPersonalID = " + ById;
            }

            List<AccessrecordsVO> aVO = rDAO.FindAllByPageIndex("Type = '" + Type + "'" + sql + sqlTime + "  GROUP BY PersonalID", "AccessAt", "desc");
            return aVO.Count;
        }



        /// <summary>
        /// 获取访问记录人数(总计)
        /// </summary>
        /// <returns></returns>
        /// <param name="Mode">查询方式，0：针对信息的访问数据，1：针对人员的访问数据</param>
        public int FindNumberOfVisitors(int ById, int Mode = 0)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);

            string sql = " ById = " + ById;
            if (Mode == 1)
            {
                sql = " ToPersonalID = " + ById;
            }

            List<AccessrecordsVO> aVO = rDAO.FindAllByPageIndex(sql + " GROUP BY PersonalID", "AccessAt", "desc");
            return aVO.Count;
        }

        /// <summary>
        /// 获取访问记录人数（时间 总计）TimeType：1:当天数据；2:7天内数据；3:30天内数据
        /// </summary>
        /// <returns></returns>
        /// <param name="Mode">查询方式，0：针对信息的访问数据，1：针对人员的访问数据</param>
        public int FindNumberOfVisitors(int ById, int TimeType, int Mode = 0)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);

            string sqlTime = "";
            if (TimeType == 1)
            {
                sqlTime = " and to_days(AccessAt) = to_days(now())";
            }
            if (TimeType == 2)
            {
                sqlTime = " and DATE_SUB(CURDATE(), INTERVAL 7 DAY) <= date(AccessAt)";
            }
            if (TimeType == 3)
            {
                sqlTime = " and DATE_SUB(CURDATE(), INTERVAL 1 MONTH) <= date(AccessAt)";
            }

            string sql = " ById = " + ById;
            if (Mode == 1)
            {
                sql = " ToPersonalID = " + ById;
            }

            List<AccessrecordsVO> aVO = rDAO.FindAllByPageIndex(sql + sqlTime + "  GROUP BY PersonalID", "AccessAt", "desc");
            return aVO.Count;
        }

        /// <summary>
        /// 获取访问记录人数（指定日期）
        /// </summary>
        /// <returns></returns>
        /// <param name="Mode">查询方式，0：针对信息的访问数据，1：针对人员的访问数据</param>
        public int FindNumberOfVisitors(string Type, int ById, DateTime dt, int Mode = 0)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);

            string sqlTime = " and to_days(AccessAt) = to_days('" + dt.ToString("yyyy-MM-dd") + "')";

            string sql = " and ById = " + ById;
            if (Mode == 1)
            {
                sql = " and ToPersonalID = " + ById;
            }

            List<AccessrecordsVO> aVO = rDAO.FindAllByPageIndex("Type = '" + Type + "'" + sql + sqlTime + "  GROUP BY PersonalID", "AccessAt", "desc");
            return aVO.Count;
        }

        /// <summary>
        /// 获取访问记录人数(指定日期 总计)
        /// </summary>
        /// <returns></returns>
        /// <param name="Mode">查询方式，0：针对信息的访问数据，1：针对人员的访问数据</param>
        public int FindNumberOfVisitors(int ById, DateTime dt, int Mode = 0)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);

            string sqlTime = " and to_days(AccessAt) = to_days('" + dt.ToString("yyyy-MM-dd") + "')";
            string sql = " ById = " + ById;
            if (Mode == 1)
            {
                sql = " ToPersonalID = " + ById;
            }

            List<AccessrecordsVO> aVO = rDAO.FindAllByPageIndex(sql + sqlTime + " GROUP BY PersonalID", "AccessAt", "desc");
            return aVO.Count;
        }

        /// <summary>
        /// 获取访问记录次数
        /// </summary>
        /// <returns></returns>
        /// <param name="Mode">查询方式，0：针对信息的访问数据，1：针对人员的访问数据</param>
        public int FindNumberOfVisits(string Type, int ById, int Mode = 0)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);

            string sql = " and ById = " + ById;
            if (Mode == 1)
            {
                sql = " and ToPersonalID = " + ById;
            }
            return rDAO.FindTotalCount("Type = '" + Type + "'" + sql);
        }

        /// <summary>
        /// 获取访问记录次数（时间）TimeType：1:当天数据；2:7天内数据；3:30天内数据
        /// </summary>
        /// <returns></returns>
        public int FindNumberOfVisits(string Type, int ById, int TimeType, int Mode = 0)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);

            string sqlTime = "";
            if (TimeType == 1)
            {
                sqlTime = " and to_days(AccessAt) = to_days(now())";
            }
            if (TimeType == 2)
            {
                sqlTime = " and DATE_SUB(CURDATE(), INTERVAL 7 DAY) <= date(AccessAt)";
            }
            if (TimeType == 3)
            {
                sqlTime = " and DATE_SUB(CURDATE(), INTERVAL 1 MONTH) <= date(AccessAt)";
            }

            string sql = " and ById = " + ById;
            if (Mode == 1)
            {
                sql = " and ToPersonalID = " + ById;
            }

            return rDAO.FindTotalCount("Type = '" + Type + "'" + sql + sqlTime);
        }

        /// <summary>
        /// 获取访问记录次数（总计）
        /// </summary>
        /// <returns></returns>
        /// <param name="Mode">查询方式，0：针对信息的访问数据，1：针对人员的访问数据</param>
        public int FindNumberOfVisits(int ById, int Mode = 0)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);

            string sql = " ById = " + ById;
            if (Mode == 1)
            {
                sql = " ToPersonalID = " + ById;
            }
            return rDAO.FindTotalCount(sql);
        }

        /// <summary>
        /// 获取访问记录次数（时间 总计）TimeType：1:当天数据；2:7天内数据；3:30天内数据
        /// </summary>
        /// <returns></returns>
        public int FindNumberOfVisits(int ById, int TimeType, int Mode = 0)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);

            string sqlTime = "";
            if (TimeType == 1)
            {
                sqlTime = " and to_days(AccessAt) = to_days(now())";
            }
            if (TimeType == 2)
            {
                sqlTime = " and DATE_SUB(CURDATE(), INTERVAL 7 DAY) <= date(AccessAt)";
            }
            if (TimeType == 3)
            {
                sqlTime = " and DATE_SUB(CURDATE(), INTERVAL 1 MONTH) <= date(AccessAt)";
            }

            string sql = " ById = " + ById;
            if (Mode == 1)
            {
                sql = " ToPersonalID = " + ById;
            }

            return rDAO.FindTotalCount(sql + sqlTime);
        }

        /// <summary>
        /// 获取访问记录次数（指定日期）
        /// </summary>
        /// <returns></returns>
        /// <param name="Mode">查询方式，0：针对信息的访问数据，1：针对人员的访问数据</param>
        public int FindNumberOfVisits(string Type, int ById, DateTime dt, int Mode = 0)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);

            string sqlTime = " and to_days(AccessAt) = to_days('" + dt.ToString("yyyy-MM-dd") + "')";

            string sql = " and ById = " + ById;
            if (Mode == 1)
            {
                sql = " and ToPersonalID = " + ById;
            }
            return rDAO.FindTotalCount("Type = '" + Type + "'" + sql + sqlTime);
        }

        /// <summary>
        /// 获取访问记录次数(指定日期 总计)
        /// </summary>
        /// <returns></returns>
        /// <param name="Mode">查询方式，0：针对信息的访问数据，1：针对人员的访问数据</param>
        public int FindNumberOfVisits(int ById, DateTime dt, int Mode = 0)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);

            string sqlTime = " and to_days(AccessAt) = to_days('" + dt.ToString("yyyy-MM-dd") + "')";
            string sql = " ById = " + ById;
            if (Mode == 1)
            {
                sql = " ToPersonalID = " + ById;
            }

            return rDAO.FindTotalCount(sql + sqlTime);
        }

        /// <summary>
        /// 获取访问记录列表(视图)
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public List<AccessrecordsViewVO> FindAccessrecordsViewByCondtion(string condtion)
        {
            IAccessrecordsViewDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsViewDAO(this.CurrentCustomerProfile);
            List<AccessrecordsViewVO> cVO = rDAO.FindByParams(condtion);
            return cVO;
        }

        /// <summary>
        /// 获取访问记录详情
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public AccessrecordsViewVO FindAccessrecordsById(int AccessrecordsID)
        {
            IAccessrecordsViewDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(AccessrecordsID);
        }

        /// <summary>
        /// 获取1个月内的访问数据,不包括本人
        /// </summary>
        /// <returns></returns>
        /// <param name="Type">null代表不限制</param>
        /// <param name="ToPersonalID">0代表不限制</param>
        /// <param name="ById">0代表不限制</param>
        public List<AccessrecordsViewVO> FindAccessrecordsListOfMonth(string Type, int ToPersonalID, int ById, int limit, bool isTime)
        {
            IAccessrecordsViewDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsViewGroupDAO(this.CurrentCustomerProfile);
            string sql = "1=1";
            if (Type != null)
            {
                sql += " and Type = '" + Type + "'";
            }

            if (ById > 0)
            {
                sql += " and ById = " + ById;
            }
            if (ToPersonalID > 0)
            {
                sql += " and ToPersonalID = " + ToPersonalID + " and PersonalID<>" + ToPersonalID;
            }

            //string sqlTime = " and DATE_SUB(CURDATE(), INTERVAL 1 MONTH) <= date(AccessAt)";
            /*
            if (isTime)
            {
                sql += sqlTime;
            }
            */
            List<AccessrecordsViewVO> aVO = rDAO.FindAllByPageIndex(sql, "AccessAt", "desc", limit);
            return aVO;
        }

        /// <summary>
        /// 获取访问数据（分页）
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<AccessrecordsViewVO> FindAccessrecordsViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IAccessrecordsViewDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsViewDAO(this.CurrentCustomerProfile);
            List<AccessrecordsViewVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);

            for (int i = 0; i < cVO.Count; i++)
            {
                if (cVO[i].Type == "Personal")
                {
                    cVO[i].Info = FindPersonalById(cVO[i].ById);
                }
                if (cVO[i].Type == "Product" || cVO[i].Type == "News" || cVO[i].Type == "Video" || cVO[i].Type == "Info")
                {
                    cVO[i].Info = FindInfoById(cVO[i].ById);
                }
                if (cVO[i].Type == "GreetingCard" || cVO[i].Type == "BrowseGreetingCard")
                {
                    InfoVO iVO = new InfoVO();
                    GreetingCardVO GreetingCardVO = FindGreetingCardById(cVO[i].ById);
                    iVO.Title = GreetingCardVO.Title;
                    iVO.InfoID = GreetingCardVO.GreetingCardID;
                    cVO[i].Info = iVO;
                }
            }
            return cVO;
        }

        /// <summary>
        /// 获取访问（数量）
        /// </summary>
        /// <returns></returns>
        public int FindAccessrecordsViewCount(string condition)
        {
            IAccessrecordsViewDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 获取访问（数量）
        /// </summary>
        /// <returns></returns>
        public int FindAccessrecordsCount(string condition)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 获取访问记录数量
        /// </summary>
        /// <returns></returns>
        public int FindAccessrecordsCount(string condition, bool isG = false)
        {
            IAccessrecordsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsDAO(this.CurrentCustomerProfile);
            string sql = "";
            if (isG)
            {
                sql = condition + " GROUP BY Type,ById";
            }
            return rDAO.FindTotalCount(sql);
        }

        /// <summary>
        /// 获取订单列表数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindPartyOrderTotalCount(string condition, params object[] parameters)
        {
            IBCPartyOrderDAO uDAO = BusinessCardManagementDAOFactory.BCPartyOrderDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 获取回递名片的数量
        /// </summary>
        /// <returns></returns>
        public int FindReturnCardCount(int PersonalID)
        {
            IAccessrecordsViewDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindReturnCardCount(PersonalID);
        }
        /// <summary>
        /// 获取回递名片的数量
        /// </summary>
        /// <returns></returns>
        public int FindReturnCardCount(int PersonalID, string condition, params object[] parameters)
        {
            IAccessrecordsViewDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindReturnCardCount(PersonalID, condition, parameters);
        }



        /// <summary>
        /// 获取访问数据（分页），分组
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<AccessrecordsViewVO> FindAccessrecordsViewGroupAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IAccessrecordsViewDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsViewGroupDAO(this.CurrentCustomerProfile);
            List<AccessrecordsViewVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);

            for (int i = 0; i < cVO.Count; i++)
            {
                try
                {
                    if (cVO[i].Type == "Personal")
                    {
                        cVO[i].Info = FindPersonalById(cVO[i].ById);
                    }
                    if (cVO[i].Type == "Product" || cVO[i].Type == "News" || cVO[i].Type == "Video" || cVO[i].Type == "Info")
                    {
                        cVO[i].Info = FindInfoById(cVO[i].ById);
                    }
                    if (cVO[i].Type == "GreetingCard" || cVO[i].Type == "BrowseGreetingCard")
                    {

                        InfoVO iVO = new InfoVO();
                        GreetingCardVO GreetingCardVO = FindGreetingCardById(cVO[i].ById);
                        if (GreetingCardVO != null)
                        {
                            iVO.Title = GreetingCardVO.Title;
                            iVO.InfoID = GreetingCardVO.GreetingCardID;
                            cVO[i].Info = iVO;
                        }
                    }
                }
                catch
                {

                }

            }
            return cVO;
        }

        /// <summary>
        /// 获取访问数据（不分页），分组
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<AccessrecordsViewVO> FindAccessrecordsViewGroupAll(string conditionStr, params object[] parameters)
        {
            IAccessrecordsViewDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsViewGroupDAO(this.CurrentCustomerProfile);
            List<AccessrecordsViewVO> cVO = rDAO.FindAllByPageIndex(conditionStr, "AccessAt", "desc", parameters);

            for (int i = 0; i < cVO.Count; i++)
            {
                if (cVO[i].Type == "Personal")
                {
                    cVO[i].Info = FindPersonalById(cVO[i].ById);
                }
                if (cVO[i].Type == "Product")
                {
                    cVO[i].Info = FindInfoById(cVO[i].ById);
                }
                if (cVO[i].Type == "News")
                {
                    cVO[i].Info = FindInfoById(cVO[i].ById);
                }
            }
            return cVO;
        }

        /// <summary>
        /// 获取访问（数量），分组
        /// </summary>
        /// <returns></returns>
        public int FindAccessrecordsViewGroupCount(string condition)
        {
            IAccessrecordsViewDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsViewGroupDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }


        /// <summary>
        /// 获取我访问他人的数据（分页），分组
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<AccessrecordsViewByRespondentsVO> FindAccessrecordsViewByRespondentsGroupAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IAccessrecordsViewByRespondentsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsViewByRespondentsDAO(this.CurrentCustomerProfile);
            List<AccessrecordsViewByRespondentsVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);

            for (int i = 0; i < cVO.Count; i++)
            {
                if (cVO[i].Type == "Personal")
                {
                    cVO[i].Info = FindPersonalById(cVO[i].ById);
                }
                if (cVO[i].Type == "Product")
                {
                    cVO[i].Info = FindInfoById(cVO[i].ById);
                }
                if (cVO[i].Type == "News")
                {
                    cVO[i].Info = FindInfoById(cVO[i].ById);
                }
            }
            return cVO;
        }

        /// <summary>
        /// 获取我访问他人的数据（不分页），分组
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<AccessrecordsViewByRespondentsVO> FindAccessrecordsViewByRespondentsGroupAll(string conditionStr, params object[] parameters)
        {
            IAccessrecordsViewByRespondentsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsViewByRespondentsDAO(this.CurrentCustomerProfile);
            List<AccessrecordsViewByRespondentsVO> cVO = rDAO.FindAllByPageIndex(conditionStr, "AccessAt", "desc", parameters);

            for (int i = 0; i < cVO.Count; i++)
            {
                if (cVO[i].Type == "Personal")
                {
                    cVO[i].Info = FindPersonalById(cVO[i].ById);
                }
                if (cVO[i].Type == "Product")
                {
                    cVO[i].Info = FindInfoById(cVO[i].ById);
                }
                if (cVO[i].Type == "News")
                {
                    cVO[i].Info = FindInfoById(cVO[i].ById);
                }
            }
            return cVO;
        }

        /// <summary>
        /// 获取我访问他人的数据（数量），分组
        /// </summary>
        /// <returns></returns>
        public int FindAccessrecordsViewByRespondentsGroupCount(string condition)
        {
            IAccessrecordsViewByRespondentsDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsViewByRespondentsDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 判断是否未感兴趣访客
        /// </summary>
        /// <param name="PersonalID">访客ID</param>
        /// <param name="ToPersonalID">被访人ID</param>
        /// <returns></returns>
        public int GetIsImportant(int PersonalID, int ToPersonalID, params object[] parameters)
        {
            string conditionStr = "ToPersonalID =" + ToPersonalID + " and PersonalID=" + PersonalID;
            IAccessrecordsViewDAO rDAO = BusinessCardManagementDAOFactory.AccessrecordsViewDAO(this.CurrentCustomerProfile);
            List<AccessrecordsViewVO> cVO = rDAO.FindAllByPageIndex(conditionStr, "AccessAt", "desc", parameters);

            int IsImportant = 0;

            if (cVO.Count == 0)
            {
                return IsImportant;
            }

            List<int> IsList = new List<int>();

            bool Is3 = cVO[0].ReturnCounts > 0;//有回递过名片的
            if (Is3) IsList.Add(3);

            bool Is2 = cVO.Sum(p => p.ResidenceAt) > 60000;//停留时间超过1分钟的
            if (Is2) IsList.Add(2);

            bool Is1 = cVO.Count >= 10;//访问次数或深度大于10的
            if (Is1) IsList.Add(1);

            if (IsList.Count > 0)
            {
                //随机抽取一个理由返回
                Random Rdm = new Random();
                int iRdm = Rdm.Next(0, IsList.Count - 1);
                IsImportant = IsList[iRdm];
            }

            return IsImportant;
        }

        /// <summary>
        /// 获取企业列表
        /// </summary>
        public List<BusinessCardViewVO> FindAllByPageIndexByBusinessCard(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IBusinessCardViewDAO pDAO = BusinessCardManagementDAOFactory.BusinessCardViewDAO(this.CurrentCustomerProfile);
            return pDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取企业数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindBusinessCardTotalCount(string condition, params object[] parameters)
        {
            IBusinessCardViewDAO pDAO = BusinessCardManagementDAOFactory.BusinessCardViewDAO(this.CurrentCustomerProfile);
            return pDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 获取代理商列表
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public List<AgentViewVO> FindAgentViewByBusinessID(int BusinessID)
        {
            IAgentViewDAO rDAO = BusinessCardManagementDAOFactory.AgentViewDAO(this.CurrentCustomerProfile);
            List<AgentViewVO> cVO = rDAO.FindByParams("BusinessID = " + BusinessID);
            return cVO;
        }

        /// <summary>
        /// 获取代理商列表
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public List<AgentViewVO> FindAgentViewByPersonalID(int BusinessID, int PersonalID)
        {
            IAgentViewDAO rDAO = BusinessCardManagementDAOFactory.AgentViewDAO(this.CurrentCustomerProfile);
            List<AgentViewVO> cVO = rDAO.FindByParams("BusinessID = " + BusinessID + " and PersonalID=" + PersonalID);
            return cVO;
        }

        /// <summary>
        /// 获取代理商列表
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public List<AgentViewVO> FindAgentViewByAgentLevelID(int AgentLevelID, int PersonalID)
        {
            IAgentViewDAO rDAO = BusinessCardManagementDAOFactory.AgentViewDAO(this.CurrentCustomerProfile);
            List<AgentViewVO> cVO = rDAO.FindByParams("AgentLevelID = " + AgentLevelID + " and PersonalID=" + PersonalID);
            return cVO;
        }

        /// <summary>
        /// 获取代理商列表
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public List<AgentViewVO> FindAgentViewByBusinessID(int BusinessID, int AgentLevelID)
        {
            IAgentViewDAO rDAO = BusinessCardManagementDAOFactory.AgentViewDAO(this.CurrentCustomerProfile);
            List<AgentViewVO> cVO = rDAO.FindByParams("BusinessID = " + BusinessID + " and AgentLevelID=" + AgentLevelID);
            return cVO;
        }

        /// <summary>
        /// 判断是否已加入该代理商级别
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public bool isJoinAgent(int PersonalID, int AgentLevelID)
        {
            IAgentViewDAO rDAO = BusinessCardManagementDAOFactory.AgentViewDAO(this.CurrentCustomerProfile);
            List<AgentViewVO> cVO = rDAO.FindByParams("PersonalID = " + PersonalID + " and AgentLevelID=" + AgentLevelID);
            return cVO.Count > 0;
        }

        /// <summary>
        /// 获取代理商级别列表
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public List<AgentLevelVO> FindAgentLevelByBusinessID(int BusinessID)
        {
            IAgentLevelDAO rDAO = BusinessCardManagementDAOFactory.AgentLevelDAO(this.CurrentCustomerProfile);
            List<AgentLevelVO> cVO = rDAO.FindByParams("BusinessID = " + BusinessID);
            return cVO;
        }

        /// <summary>
        /// 添加代理商级别
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddAgentLevel(AgentLevelVO vo)
        {
            try
            {
                IAgentLevelDAO rDAO = BusinessCardManagementDAOFactory.AgentLevelDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int AgentLevelID = rDAO.Insert(vo);
                    return AgentLevelID;
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
        /// 更新代理商级别
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateAgentLevel(AgentLevelVO vo)
        {
            IAgentLevelDAO rDAO = BusinessCardManagementDAOFactory.AgentLevelDAO(this.CurrentCustomerProfile);
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
        /// 获取代理商级别详情
        /// </summary>
        /// <param name="AgentLevelID"></param>
        /// <returns></returns>
        public AgentLevelVO FindAgentLevelById(int AgentLevelID)
        {
            IAgentLevelDAO rDAO = BusinessCardManagementDAOFactory.AgentLevelDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(AgentLevelID);
        }

        /// <summary>
        /// 删除代理商级别
        /// </summary>
        /// <param name="AgentLevelID"></param>
        /// <returns></returns>
        public int DeleteAgentLevelById(int AgentLevelID)
        {
            IAgentLevelDAO rDAO = BusinessCardManagementDAOFactory.AgentLevelDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("AgentLevelID = " + AgentLevelID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取代理商级别所属代理商列表
        /// </summary>
        /// <param name="AgentLevelID"></param>
        /// <returns></returns>
        public List<AgentVO> FindAgentByAgentLevelID(int AgentLevelID)
        {
            IAgentDAO rDAO = BusinessCardManagementDAOFactory.AgentDAO(this.CurrentCustomerProfile);
            List<AgentVO> cVO = rDAO.FindByParams("AgentLevelID = " + AgentLevelID);
            return cVO;
        }

        /// <summary>
        /// 添加代理商
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddAgent(AgentVO vo)
        {
            try
            {
                IAgentDAO rDAO = BusinessCardManagementDAOFactory.AgentDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int AgentID = rDAO.Insert(vo);
                    return AgentID;
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
        /// 更新代理商
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateAgent(AgentVO vo)
        {
            IAgentDAO rDAO = BusinessCardManagementDAOFactory.AgentDAO(this.CurrentCustomerProfile);
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
        /// 删除代理商
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public int DeleteAgentlById(int AgentID)
        {
            IAgentDAO rDAO = BusinessCardManagementDAOFactory.AgentDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("AgentID = " + AgentID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取代理商数量
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public int FindAgentCountByCondition(string condition)
        {
            IAgentDAO rDAO = BusinessCardManagementDAOFactory.AgentDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }
        /// <summary>
        /// 获取代理商级别详情
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public AgentVO FindAgentById(int AgentID)
        {
            IAgentDAO rDAO = BusinessCardManagementDAOFactory.AgentDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(AgentID);
        }


        /// <summary>
        /// 添加代理价格
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddAgentlevelCost(AgentlevelCostVO vo)
        {
            try
            {
                IAgentlevelCostDAO rDAO = BusinessCardManagementDAOFactory.AgentlevelCostDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int AgentlevelCostID = rDAO.Insert(vo);
                    return AgentlevelCostID;
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
        /// 更新代理价格
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateAgentlevelCost(AgentlevelCostVO vo)
        {
            IAgentlevelCostDAO rDAO = BusinessCardManagementDAOFactory.AgentlevelCostDAO(this.CurrentCustomerProfile);
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
        /// 获取代理价格
        /// </summary>
        /// <param name="AgentLevelID"></param>
        /// <returns></returns>
        public AgentlevelCostVO FindAgentlevelCostById(int AgentlevelCostID)
        {
            IAgentlevelCostDAO rDAO = BusinessCardManagementDAOFactory.AgentlevelCostDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(AgentlevelCostID);
        }

        /// <summary>
        /// 获取代理价格
        /// </summary>
        /// <param name="AgentLevelID"></param>
        /// <returns></returns>
        public decimal FindAgentlevelCostByPersonalID(int PersonalID, int InfoID, int CostID = 0)
        {
            InfoVO sVO = FindInfoById(InfoID);
            decimal Cost = sVO.Cost;
            if (CostID > 0)
            {
                InfoCostVO InfoCostVO = FindInfoCostById(CostID);
                Cost = InfoCostVO.Cost;
            }


            BusinessCardVO InfoBVO = FindBusinessCardById(sVO.BusinessID);
            if (InfoBVO.isAgent == 1)
            {
                List<AgentlevelCostVO> cVO = FindAgentByAgentlevelCostID(InfoID);
                IAgentlevelCostDAO rDAO = BusinessCardManagementDAOFactory.AgentlevelCostDAO(this.CurrentCustomerProfile);
                for (int i = 0; i < cVO.Count; i++)
                {
                    if (FindAgentViewByAgentLevelID(cVO[i].AgentLevelID, PersonalID).Count > 0)
                    {
                        if (cVO[i].Discount >= 0 && cVO[i].Discount <= 100)
                        {
                            Cost = Cost * cVO[i].Discount / 100;
                        }
                    }
                }
            }


            return Cost;
        }

        /// <summary>
        /// 删除代理价格
        /// </summary>
        /// <param name="AgentLevelID"></param>
        /// <returns></returns>
        public int DeleteAgentlevelCostById(int AgentlevelCostID)
        {
            IAgentlevelCostDAO rDAO = BusinessCardManagementDAOFactory.AgentlevelCostDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("AgentlevelCostID = " + AgentlevelCostID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 删除代理价格
        /// </summary>
        /// <param name="AgentLevelID"></param>
        /// <returns></returns>
        public int DeleteAgentlevelCostById(int InfoID, int AgentLevelID)
        {
            IAgentlevelCostDAO rDAO = BusinessCardManagementDAOFactory.AgentlevelCostDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("InfoID = " + InfoID + " and AgentLevelID=" + AgentLevelID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取产品代理价格列表
        /// </summary>
        /// <param name="AgentLevelID"></param>
        /// <returns></returns>
        public List<AgentlevelCostVO> FindAgentByAgentlevelCostID(int InfoID)
        {
            IAgentlevelCostDAO rDAO = BusinessCardManagementDAOFactory.AgentlevelCostDAO(this.CurrentCustomerProfile);
            List<AgentlevelCostVO> cVO = rDAO.FindByParams("InfoID = " + InfoID);
            return cVO;
        }
        /// <summary>
        /// 获取产品代理价格列表
        /// </summary>
        /// <param name="AgentLevelID"></param>
        /// <returns></returns>
        public List<AgentlevelCostVO> FindAgentByAgentlevelCostID(int InfoID, int AgentLevelID)
        {
            IAgentlevelCostDAO rDAO = BusinessCardManagementDAOFactory.AgentlevelCostDAO(this.CurrentCustomerProfile);
            List<AgentlevelCostVO> cVO = rDAO.FindByParams("InfoID = " + InfoID + " and AgentLevelID=" + AgentLevelID);
            return cVO;
        }

        /// <summary>
        /// 添加产品订单
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddOrder(OrderVO vo)
        {
            try
            {
                IOrderDAO rDAO = BusinessCardManagementDAOFactory.OrderDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int OrderID = rDAO.Insert(vo);
                    return OrderID;
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
        /// 更新产品订单
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateOrder(OrderVO vo)
        {
            IOrderDAO rDAO = BusinessCardManagementDAOFactory.OrderDAO(this.CurrentCustomerProfile);
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
        /// 获取产品订单
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public OrderVO FindOrderById(int OrderID)
        {
            IOrderDAO rDAO = BusinessCardManagementDAOFactory.OrderDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(OrderID);
        }
        /// <summary>
        /// 获取产品订单列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<OrderVO> FindOrderList(string condtion)
        {
            IOrderDAO rDAO = BusinessCardManagementDAOFactory.OrderDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取产品订单数量
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public int FindOrderByCondition(string condition)
        {
            IOrderDAO rDAO = BusinessCardManagementDAOFactory.OrderDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 开通企业名片
        /// </summary>
        /// <param name="condtion"></param>
        /// <param name="isUsed">是否单纯使用订单，不涉及金额结算</param>
        /// <returns></returns>
        public bool EstablishedBusinessCard(int InfoID, int PersonalID, int OrderID, int AppType, bool isNotGroupBuy = false)
        {
            try
            {
                PersonalVO pVO = FindPersonalById(PersonalID);
                InfoViewVO sVO = FindInfoViewById(InfoID);
                OrderVO OrderVO = FindOrderById(OrderID);

                //产品是拼团的，操作开团
                if (!isNotGroupBuy && OrderVO.isGroupBuy == 1)
                {
                    //团长购买
                    if (sVO.isGroupBuy == 1 && OrderVO.GroupBuyID == 0)
                    {
                        DeleteGroupBuyMember(pVO.CustomerId, sVO.InfoID);
                        GroupBuyVO gVO = new GroupBuyVO();
                        gVO.GroupBuyID = 0;
                        gVO.CustomerId = pVO.CustomerId;
                        gVO.BusinessID = sVO.BusinessID;
                        gVO.PersonalID = pVO.PersonalID;
                        gVO.AgentPersonalID = OrderVO.AgentPersonalID;
                        gVO.Name = pVO.Name + "的拼团";
                        gVO.PeopleNumber = sVO.GroupBuyPeopleNumber;
                        gVO.ExpireAt = DateTime.Now.AddDays(sVO.GroupBuyDays);
                        gVO.Discount = sVO.GroupBuyDiscount;
                        gVO.InfoID = OrderVO.InfoID;
                        gVO.GroupBuyID = AddGroupBuy(gVO);
                        if (gVO.GroupBuyID > 0)
                        {
                            OrderVO.GroupBuyID = gVO.GroupBuyID;


                            GroupBuyMemberVO gmVO = new GroupBuyMemberVO();
                            gmVO.GroupBuyMemberID = 0;
                            gmVO.GroupBuyID = gVO.GroupBuyID;
                            gmVO.CustomerId = gVO.CustomerId;
                            gmVO.BusinessID = gVO.BusinessID;
                            gmVO.PersonalID = gVO.PersonalID;
                            gmVO.InfoID = gVO.InfoID;
                            AddGroupMemberBuy(gmVO);
                        }

                        //人头返现（德伦口腔返现规则）
                        /*
                        OrderVO.PeopleRebateCid = pVO.CustomerId;
                        OrderVO.PeopleRebateCost = Convert.ToDecimal(OrderVO.OriginalCost / 10);
                        OrderVO.PeopleRebateStatus = 0;
                        OrderVO.GroupBuyID = gVO.GroupBuyID;

                        UpdateOrder(OrderVO);
                        sendGroupBuyMessage(OrderVO.OpenId, "拼团人头返现", OrderVO.PeopleRebateCost, OrderVO.OrderNO, sVO.BusinessID, OrderVO.InfoID, AppType);
                        */
                        OrderVO.Status = 3;//设置为拼团中
                        UpdateOrder(OrderVO);
                    }
                    //团员购买
                    else if (sVO.isGroupBuy == 1 && OrderVO.GroupBuyID > 0)
                    {
                        //加入拼团
                        GroupBuyMemberVO mVO = new GroupBuyMemberVO();
                        mVO.GroupBuyMemberID = 0;
                        mVO.GroupBuyID = OrderVO.GroupBuyID;
                        mVO.PersonalID = pVO.PersonalID;
                        mVO.CustomerId = pVO.CustomerId;
                        mVO.BusinessID = sVO.BusinessID;
                        mVO.InfoID = OrderVO.InfoID;

                        AddGroupMemberBuy(mVO);

                        GroupBuyVO gVO = FindGroupBuyById(OrderVO.GroupBuyID);
                        /*（德伦口腔返现规则）
                       
                        //判断是否为第一次购买
                        int myOrder = FindOrderByCondition("GroupBuyID=" + OrderVO.GroupBuyID + " and PersonalID=" + pVO.PersonalID + " and Status=1");
                        if (gVO != null && myOrder == 1)
                        {
                            OrderVO.PeopleRebateCid = gVO.CustomerId;
                            OrderVO.PeopleRebateCost = Convert.ToDecimal(OrderVO.OriginalCost / 10);
                            OrderVO.PeopleRebateStatus = 0;

                            //查找团长的OpenId
                            List<OrderGroupBuyViewVO> tuanzVO = FindOrderGroupBuyViewList("CustomerId=" + gVO.CustomerId);
                            if (tuanzVO.Count > 0)
                            {
                                sendGroupBuyMessage(tuanzVO[0].OpenId, "拼团人头返现", OrderVO.PeopleRebateCost, OrderVO.OrderNO, sVO.BusinessID, OrderVO.InfoID, AppType);
                            }

                            List<OrderGroupBuyViewVO> ogVO = FindOrderGroupBuyViewList("GroupBuyID=" + gVO.GroupBuyID + " and CustomerId<>" + pVO.CustomerId);
                            if (ogVO.Count > 0 && ogVO.Count <= 4)
                            {
                                //按付款人数降低折扣
                                if (ogVO.Count == 1)
                                {
                                    gVO.Discount = 80;
                                }
                                if (ogVO.Count == 2)
                                {
                                    gVO.Discount = 70;
                                }
                                if (ogVO.Count == 3)
                                {
                                    gVO.Discount = 60;
                                }
                                UpdateGroupBuy(gVO);

                                decimal RebateCost = Convert.ToDecimal(OrderVO.Cost / 10);
                                for (int i = 0; i < ogVO.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        OrderVO.RebateCid1 = ogVO[i].CustomerId;
                                        OrderVO.RebateCost1 = Convert.ToDecimal(FindOrderCost(ogVO[i].PersonalID, gVO.GroupBuyID) / 10);
                                        OrderVO.RebateStatus1 = 0;

                                        sendGroupBuyMessage(ogVO[i].OpenId, "拼团折扣返现", OrderVO.RebateCost1, OrderVO.OrderNO, sVO.BusinessID, OrderVO.InfoID, AppType);
                                    }
                                    if (i == 1)
                                    {
                                        OrderVO.RebateCid2 = ogVO[i].CustomerId;
                                        OrderVO.RebateCost2 = Convert.ToDecimal(FindOrderCost(ogVO[i].PersonalID, gVO.GroupBuyID) / 10);
                                        OrderVO.RebateStatus2 = 0;

                                        sendGroupBuyMessage(ogVO[i].OpenId, "拼团折扣返现", OrderVO.RebateCost2, OrderVO.OrderNO, sVO.BusinessID, OrderVO.InfoID, AppType);
                                    }
                                    if (i == 2)
                                    {
                                        OrderVO.RebateCid3 = ogVO[i].CustomerId;
                                        OrderVO.RebateCost3 = Convert.ToDecimal(FindOrderCost(ogVO[i].PersonalID, gVO.GroupBuyID) / 10);
                                        OrderVO.RebateStatus3 = 0;

                                        sendGroupBuyMessage(ogVO[i].OpenId, "拼团折扣返现", OrderVO.RebateCost3, OrderVO.OrderNO, sVO.BusinessID, OrderVO.InfoID, AppType);
                                    }
                                    if (i == 3)
                                    {
                                        OrderVO.RebateCid4 = ogVO[i].CustomerId;
                                        OrderVO.RebateCost4 = Convert.ToDecimal(FindOrderCost(ogVO[i].PersonalID, gVO.GroupBuyID) / 10);
                                        OrderVO.RebateStatus4 = 0;

                                        sendGroupBuyMessage(ogVO[i].OpenId, "拼团折扣返现", OrderVO.RebateCost4, OrderVO.OrderNO, sVO.BusinessID, OrderVO.InfoID, AppType);
                                    }
                                }
                            }
                            UpdateOrder(OrderVO);
                        }*/
                        int GroupBuyOrderNum = FindOrderByCondition("InfoID=" + sVO.InfoID + " and GroupBuyID=" + OrderVO.GroupBuyID + " and (Status=1 or Status=3)");
                        if (GroupBuyOrderNum >= gVO.PeopleNumber)
                        {
                            List<OrderVO> oVOList = FindOrderList("InfoID=" + sVO.InfoID + " and GroupBuyID=" + OrderVO.GroupBuyID + " and (Status=1 or Status=3)");
                            foreach (OrderVO item in oVOList)
                            {
                                item.Status = 1;
                                UpdateOrder(item);
                            }
                        }
                        else
                        {
                            OrderVO.Status = 3;//设置为拼团中
                            UpdateOrder(OrderVO);
                        }

                    }


                }


                if (sVO.OfficialProducts != "Basic" && sVO.OfficialProducts != "Standard" && sVO.OfficialProducts != "Advanced" && sVO.OfficialProducts != "SelfEmployed" && sVO.OfficialProducts != "SelfEmployed2")
                {
                    return false;
                }

                //开通
                if (pVO.BusinessID == 0 || OrderVO.isUsed == 2)
                {
                    BusinessCardVO BusinessCardVO = new BusinessCardVO();
                    BusinessCardVO.BusinessID = 0;
                    BusinessCardVO.BusinessName = pVO.Name + "的公司";
                    BusinessCardVO.OfficialProducts = sVO.OfficialProducts;

                    if (sVO.OfficialProducts == "SelfEmployed2")
                        BusinessCardVO.Number = 1;
                    if (sVO.OfficialProducts == "SelfEmployed")
                        BusinessCardVO.Number = 4;
                    if (sVO.OfficialProducts == "Basic")
                        BusinessCardVO.Number = 20;
                    if (sVO.OfficialProducts == "Standard")
                        BusinessCardVO.Number = 100;
                    if (sVO.OfficialProducts == "Advanced")
                        BusinessCardVO.Number = 250;

                    BusinessCardVO.OfficialProducts = sVO.OfficialProducts;
                    BusinessCardVO.CreatedAt = DateTime.Now;
                    BusinessCardVO.ExpirationAt = DateTime.Now.AddYears(1);
                    BusinessCardVO.isPay = 1;
                    int BusinessID = AddBusinessCard(BusinessCardVO);
                    if (BusinessID > 0)
                    {
                        JoinBusiness(pVO, BusinessID, AppType);//绑定公司

                        GetJoinQR(BusinessID, AppType);

                        OrderVO.BusinessID = BusinessID;
                        OrderVO.isUsed = 2;
                        UpdateOrder(OrderVO);

                        return true;
                    }
                    else
                        return false;
                }
                else
                {//续费
                    BusinessCardVO BusinessCardVO = FindBusinessCardById(pVO.BusinessID);
                    BusinessCardVO.OfficialProducts = sVO.OfficialProducts;

                    int Number = 0;
                    if (sVO.OfficialProducts == "SelfEmployed2")
                        Number = 1;
                    if (sVO.OfficialProducts == "SelfEmployed")
                        Number = 4;
                    if (sVO.OfficialProducts == "Basic")
                        Number = 20;
                    if (sVO.OfficialProducts == "Standard")
                        Number = 100;
                    if (sVO.OfficialProducts == "Advanced")
                        Number = 250;
                    if (Number > BusinessCardVO.Number)
                        BusinessCardVO.Number = Number;

                    if (BusinessCardVO.ExpirationAt > DateTime.Now)
                    {
                        BusinessCardVO.ExpirationAt = BusinessCardVO.ExpirationAt.AddYears(1);
                    }
                    else
                    {
                        BusinessCardVO.ExpirationAt = DateTime.Now.AddYears(1);
                    }


                    if (BusinessCardVO.isGroup == 1)
                    {
                        List<BusinessCardVO> bVOList = FindBusinessCardByHeadquartersID(BusinessCardVO.BusinessID);
                        for (int i = 0; i < bVOList.Count; i++)
                        {
                            bVOList[i].OfficialProducts = BusinessCardVO.OfficialProducts;
                            bVOList[i].ExpirationAt = BusinessCardVO.ExpirationAt;
                            bVOList[i].Number = BusinessCardVO.Number;
                            UpdateBusinessCard(bVOList[i]);
                        }
                    }


                    OrderVO.BusinessID = pVO.BusinessID;
                    OrderVO.isUsed = 1;
                    UpdateOrder(OrderVO);

                    return UpdateBusinessCard(BusinessCardVO);
                }
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
        /// 获取订单（分页）
        /// </summary>
        /// <returns></returns>
        public List<OrderViewVO> FindOrderViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IOrderViewDAO rDAO = BusinessCardManagementDAOFactory.OrderViewDAO(this.CurrentCustomerProfile);
            List<OrderViewVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            return cVO;
        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<OrderViewVO> FindOrderViewList(string condtion)
        {
            IOrderViewDAO rDAO = BusinessCardManagementDAOFactory.OrderViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取订单（订单数量）
        /// </summary>
        /// <returns></returns>
        public int FindOrderViewCount(string condition)
        {
            IOrderViewDAO rDAO = BusinessCardManagementDAOFactory.OrderViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }


        /// <summary>
        /// 获取订单（购买数量）
        /// </summary>
        /// <returns></returns>
        public decimal FindOrderNumberSum(string condition)
        {
            IOrderViewDAO rDAO = BusinessCardManagementDAOFactory.OrderViewDAO(this.CurrentCustomerProfile);
           
            return rDAO.FindTotalSum("Number", condition);
        }

        /// <summary>
        /// 获取订单（金额）
        /// </summary>
        /// <returns></returns>
        public decimal FindOrderViewSumCost(string sum, string condition)
        {
            IOrderViewDAO rDAO = BusinessCardManagementDAOFactory.OrderViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalSum(sum, condition + " and Status=1 and payAt is not NULL");
        }

        /// <summary>
        /// 获取拼团第一单金额
        /// </summary>
        /// <returns></returns>
        public decimal FindOrderCost(int PersonalID, int GroupBuyID)
        {
            IOrderDAO rDAO = BusinessCardManagementDAOFactory.OrderDAO(this.CurrentCustomerProfile);

            List<OrderVO> oVO = rDAO.FindAllByPageIndex("PersonalID=" + PersonalID + " and Status=1 and payAt is not NULL and GroupBuyID=" + GroupBuyID, "payAt", "asc");

            if (oVO.Count > 0)
            {
                if (oVO[0].OriginalCost > 0)
                {
                    return oVO[0].OriginalCost;
                }
                else
                {
                    return oVO[0].Cost;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取订单（金额）
        /// </summary>
        /// <returns></returns>
        public decimal FindOrderSumCost(string sum, string condition)
        {
            IOrderDAO rDAO = BusinessCardManagementDAOFactory.OrderDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalSum(sum, condition + " and Status=1 and payAt is not NULL");
        }

        /// <summary>
        /// 获取产品订单
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public OrderViewVO FindOrderViewById(int OrderID)
        {
            IOrderViewDAO rDAO = BusinessCardManagementDAOFactory.OrderViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(OrderID);
        }



        /// <summary>
        /// 获取产品订单
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public OrderViewVO FindOrderViewByOrderNo(string OrderNo)
        {
            IOrderViewDAO rDAO = BusinessCardManagementDAOFactory.OrderViewDAO(this.CurrentCustomerProfile);
            List<OrderViewVO> voList = rDAO.FindByParams("OrderNo = @OrderNo", new object[] { DbHelper.CreateParameter("@OrderNo", OrderNo) });
            if (voList.Count > 0)
                return voList[0];
            else
                return null;

        }



        /// <summary>
        /// 获取商家收入
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="isPayout">0:累计 1:可提现余额 2:已提现 3:提现中</param>
        /// <param name="postsql">sql</param>
        /// <param name="isRebateCost">0:不去除佣金  1:去除佣金 2:只返回佣金</param>
        /// <returns></returns>
        public decimal getBusinessCost(int BusinessID, int isPayout, string postsql = "", int isRebateCost = 1)
        {
            if (isPayout == 0)
            {
                string sql = "ProdustsBusinessID=" + BusinessID + " and payAt is not NULL and Status=1 and isAgentBuy=0 and isEcommerceBuy=0 and (ProfitsharingCost=0 or ProfitsharingStatus=1) and (TowProfitsharingCost=0 or TowProfitsharingStatus=1) " + postsql;
                decimal Cost = FindOrderViewSumCost("Cost", sql);
                decimal RebateCost1 = FindOrderViewSumCost("RebateCost1", sql);
                decimal RebateCost2 = FindOrderViewSumCost("RebateCost2", sql);
                decimal RebateCost3 = FindOrderViewSumCost("RebateCost3", sql);
                decimal RebateCost4 = FindOrderViewSumCost("RebateCost4", sql);
                decimal PeopleRebateCost = FindOrderViewSumCost("PeopleRebateCost", sql);
                decimal ProfitsharingCost = FindOrderViewSumCost("ProfitsharingCost", sql);
                decimal TowProfitsharingCost = FindOrderViewSumCost("TowProfitsharingCost", sql);

                if (isRebateCost == 0)
                {
                    return Cost;
                }
                else if (isRebateCost == 1)
                {
                    return Cost - RebateCost1 - RebateCost2 - RebateCost3 - RebateCost4 - PeopleRebateCost - ProfitsharingCost - TowProfitsharingCost;
                }
                else
                {
                    return RebateCost1 + RebateCost2 + RebateCost3 + RebateCost4 + PeopleRebateCost + ProfitsharingCost + TowProfitsharingCost;
                }

            }
            else if (isPayout == 1)
            {
                return getBalance(BusinessID);
            }
            else if (isPayout == 2)
            {
                return FindPayOutSumCost(BusinessID, 1);
            }
            else if (isPayout == 3)
            {
                return FindPayOutSumCost(BusinessID, 0);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取我的佣金
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="isPayout">0:累计 1:可提现余额 2:已提现 3:提现中</param>
        /// <returns></returns>
        public decimal getMyRebateCost(int CustomerId, int isPayout)
        {
            string sql = "RebateCid1=" + CustomerId + " and payAt is not NULL and Status=1";
            if (isPayout == 1)
            {
                sql += " and RebateStatus1=0";
            }
            if (isPayout == 2)
            {
                sql += " and RebateStatus1=1";
            }
            if (isPayout == 3)
            {
                sql += " and RebateStatus1=2";
            }
            decimal RebateCost1 = FindOrderSumCost("RebateCost1", sql);

            sql = "RebateCid2=" + CustomerId + " and payAt is not NULL and Status=1";
            if (isPayout == 1)
            {
                sql += " and RebateStatus2=0";
            }
            if (isPayout == 2)
            {
                sql += " and RebateStatus2=1";
            }
            if (isPayout == 3)
            {
                sql += " and RebateStatus2=2";
            }
            decimal RebateCost2 = FindOrderSumCost("RebateCost2", sql);

            sql = "RebateCid3=" + CustomerId + " and payAt is not NULL and Status=1";
            if (isPayout == 1)
            {
                sql += " and RebateStatus3=0";
            }
            if (isPayout == 2)
            {
                sql += " and RebateStatus3=1";
            }
            if (isPayout == 3)
            {
                sql += " and RebateStatus3=2";
            }
            decimal RebateCost3 = FindOrderSumCost("RebateCost3", sql);

            sql = "RebateCid4=" + CustomerId + " and payAt is not NULL and Status=1";
            if (isPayout == 1)
            {
                sql += " and RebateStatus4=0";
            }
            if (isPayout == 2)
            {
                sql += " and RebateStatus4=1";
            }
            if (isPayout == 3)
            {
                sql += " and RebateStatus4=2";
            }
            decimal RebateCost4 = FindOrderSumCost("RebateCost4", sql);

            sql = "PeopleRebateCid=" + CustomerId + " and payAt is not NULL and Status=1";
            if (isPayout == 1)
            {
                sql += " and PeopleRebateStatus=0";
            }
            if (isPayout == 2)
            {
                sql += " and PeopleRebateStatus=1";
            }
            if (isPayout == 3)
            {
                sql += " and PeopleRebateStatus=2";
            }
            decimal PeopleRebateCost = FindOrderSumCost("PeopleRebateCost", sql);

            sql = "ProfitsharingCid=" + CustomerId + " and payAt is not NULL and Status=1";
            if (isPayout == 1)
            {
                sql += " and ProfitsharingStatus=0";
            }
            if (isPayout == 2)
            {
                sql += " and ProfitsharingStatus=1";
            }
            if (isPayout == 3)
            {
                sql += " and ProfitsharingStatus=2";
            }
            decimal ProfitsharingCost = FindOrderSumCost("ProfitsharingCost", sql);

            sql = "TowProfitsharingCid=" + CustomerId + " and payAt is not NULL and Status=1";
            if (isPayout == 1)
            {
                sql += " and TowProfitsharingStatus=0";
            }
            if (isPayout == 2)
            {
                sql += " and TowProfitsharingStatus=1";
            }
            if (isPayout == 3)
            {
                sql += " and TowProfitsharingStatus=2";
            }
            decimal TowProfitsharingCost = FindOrderSumCost("TowProfitsharingCost", sql);


            return RebateCost1 + RebateCost2 + RebateCost3 + RebateCost4 + PeopleRebateCost + ProfitsharingCost + TowProfitsharingCost;
        }

        /// <summary>
        /// 获取我的佣金列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="isPayout">0:累计 1:可提现 2:已提现 3:提现中</param>
        /// <returns></returns>
        public List<RebateOrder> getMyRebateOrder(int CustomerId, int isPayout)
        {
            List<RebateOrder> RebateOrderList = new List<RebateOrder>();

            string sql = "RebateCid1=" + CustomerId + " and payAt is not NULL and Status=1";
            if (isPayout == 1)
            {
                sql += " and RebateStatus1=0";
            }
            if (isPayout == 2)
            {
                sql += " and RebateStatus1=1";
            }
            if (isPayout == 3)
            {
                sql += " and RebateStatus1=2";
            }
            List<OrderViewVO> ov1 = FindOrderViewList(sql);
            foreach (OrderViewVO vo in ov1)
            {
                RebateOrder ro = new RebateOrder();
                ro.Headimg = vo.Headimg;
                ro.Image = vo.Image;
                ro.Name = vo.Name;
                ro.Cost = vo.Cost;
                ro.payAt = vo.payAt;
                ro.OrderID = vo.OrderID;
                ro.ProdustsBusinessName = vo.ProdustsBusinessName;
                ro.ProdustsLogoImg = vo.ProdustsLogoImg;
                ro.Title = vo.Title;
                ro.RebateCost = vo.RebateCost1;
                ro.RebateStatus = vo.RebateStatus1;
                ro.Type = "折扣返现";
                RebateOrderList.Add(ro);
            }

            sql = "RebateCid2=" + CustomerId;
            if (isPayout == 1)
            {
                sql += " and RebateStatus2=0";
            }
            if (isPayout == 2)
            {
                sql += " and RebateStatus2=1";
            }
            if (isPayout == 3)
            {
                sql += " and RebateStatus2=2";
            }
            List<OrderViewVO> ov2 = FindOrderViewList(sql);
            foreach (OrderViewVO vo in ov2)
            {
                RebateOrder ro = new RebateOrder();
                ro.Headimg = vo.Headimg;
                ro.Image = vo.Image;
                ro.Name = vo.Name;
                ro.payAt = vo.payAt;
                ro.Cost = vo.Cost;
                ro.OrderID = vo.OrderID;
                ro.ProdustsBusinessName = vo.ProdustsBusinessName;
                ro.ProdustsLogoImg = vo.ProdustsLogoImg;
                ro.Title = vo.Title;
                ro.RebateCost = vo.RebateCost2;
                ro.RebateStatus = vo.RebateStatus2;
                ro.Type = "折扣返现";
                RebateOrderList.Add(ro);
            }

            sql = "RebateCid3=" + CustomerId;
            if (isPayout == 1)
            {
                sql += " and RebateStatus3=0";
            }
            if (isPayout == 2)
            {
                sql += " and RebateStatus3=1";
            }
            if (isPayout == 3)
            {
                sql += " and RebateStatus3=2";
            }
            List<OrderViewVO> ov3 = FindOrderViewList(sql);
            foreach (OrderViewVO vo in ov3)
            {
                RebateOrder ro = new RebateOrder();
                ro.Headimg = vo.Headimg;
                ro.Image = vo.Image;
                ro.Name = vo.Name;
                ro.payAt = vo.payAt;
                ro.Cost = vo.Cost;
                ro.OrderID = vo.OrderID;
                ro.ProdustsBusinessName = vo.ProdustsBusinessName;
                ro.ProdustsLogoImg = vo.ProdustsLogoImg;
                ro.Title = vo.Title;
                ro.RebateCost = vo.RebateCost3;
                ro.RebateStatus = vo.RebateStatus3;
                ro.Type = "折扣返现";
                RebateOrderList.Add(ro);
            }

            sql = "RebateCid4=" + CustomerId;
            if (isPayout == 1)
            {
                sql += " and RebateStatus4=0";
            }
            if (isPayout == 2)
            {
                sql += " and RebateStatus4=1";
            }
            if (isPayout == 3)
            {
                sql += " and RebateStatus4=2";
            }
            List<OrderViewVO> ov4 = FindOrderViewList(sql);
            foreach (OrderViewVO vo in ov4)
            {
                RebateOrder ro = new RebateOrder();
                ro.Headimg = vo.Headimg;
                ro.Image = vo.Image;
                ro.Name = vo.Name;
                ro.payAt = vo.payAt;
                ro.Cost = vo.Cost;
                ro.OrderID = vo.OrderID;
                ro.ProdustsBusinessName = vo.ProdustsBusinessName;
                ro.ProdustsLogoImg = vo.ProdustsLogoImg;
                ro.Title = vo.Title;
                ro.RebateCost = vo.RebateCost4;
                ro.RebateStatus = vo.RebateStatus4;
                ro.Type = "折扣返现";
                RebateOrderList.Add(ro);
            }

            sql = "PeopleRebateCid=" + CustomerId;
            if (isPayout == 1)
            {
                sql += " and PeopleRebateStatus=0";
            }
            if (isPayout == 2)
            {
                sql += " and PeopleRebateStatus=1";
            }
            if (isPayout == 3)
            {
                sql += " and PeopleRebateStatus=2";
            }
            List<OrderViewVO> Peopleov = FindOrderViewList(sql);
            foreach (OrderViewVO vo in Peopleov)
            {
                RebateOrder ro = new RebateOrder();
                ro.Headimg = vo.Headimg;
                ro.Image = vo.Image;
                ro.Name = vo.Name;
                ro.payAt = vo.payAt;
                ro.Cost = vo.Cost;
                ro.OrderID = vo.OrderID;
                ro.ProdustsBusinessName = vo.ProdustsBusinessName;
                ro.ProdustsLogoImg = vo.ProdustsLogoImg;
                ro.Title = vo.Title;
                ro.RebateCost = vo.PeopleRebateCost;
                ro.RebateStatus = vo.PeopleRebateStatus;
                ro.Type = "人头返现";
                RebateOrderList.Add(ro);
            }

            sql = "ProfitsharingCid=" + CustomerId;
            if (isPayout == 1)
            {
                sql += " and ProfitsharingStatus=0";
            }
            if (isPayout == 2)
            {
                sql += " and ProfitsharingStatus=1";
            }
            if (isPayout == 3)
            {
                sql += " and ProfitsharingStatus=2";
            }
            List<OrderViewVO> Profitsharingov = FindOrderViewList(sql);
            foreach (OrderViewVO vo in Profitsharingov)
            {
                RebateOrder ro = new RebateOrder();
                ro.Headimg = vo.Headimg;
                ro.Image = vo.Image;
                ro.Name = vo.Name;
                ro.payAt = vo.payAt;
                ro.Cost = vo.Cost;
                ro.OrderID = vo.OrderID;
                ro.ProdustsBusinessName = vo.ProdustsBusinessName;
                ro.ProdustsLogoImg = vo.ProdustsLogoImg;
                ro.Title = vo.Title;
                ro.RebateCost = vo.ProfitsharingCost;
                ro.RebateStatus = vo.ProfitsharingStatus;
                ro.Type = "推荐分成";
                RebateOrderList.Add(ro);
            }


            sql = "TowProfitsharingCid=" + CustomerId;
            if (isPayout == 1)
            {
                sql += " and TowProfitsharingStatus=0";
            }
            if (isPayout == 2)
            {
                sql += " and TowProfitsharingStatus=1";
            }
            if (isPayout == 3)
            {
                sql += " and TowProfitsharingStatus=2";
            }
            List<OrderViewVO> TowProfitsharingov = FindOrderViewList(sql);
            foreach (OrderViewVO vo in TowProfitsharingov)
            {
                RebateOrder ro = new RebateOrder();
                ro.Headimg = vo.Headimg;
                ro.Image = vo.Image;
                ro.Name = vo.Name;
                ro.payAt = vo.payAt;
                ro.Cost = vo.Cost;
                ro.OrderID = vo.OrderID;
                ro.ProdustsBusinessName = vo.ProdustsBusinessName;
                ro.ProdustsLogoImg = vo.ProdustsLogoImg;
                ro.Title = vo.Title;
                ro.RebateCost = vo.TowProfitsharingCost;
                ro.RebateStatus = vo.TowProfitsharingStatus;
                ro.Type = "二级推荐分成";
                RebateOrderList.Add(ro);
            }

            RebateOrderList.Sort((a, b) => a.payAt.CompareTo(b.payAt));
            RebateOrderList.Reverse();

            return RebateOrderList;
        }

        /// <summary>
        /// 获取拼团订单列表（按人分组）
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<OrderGroupBuyViewVO> FindOrderGroupBuyViewList(string condtion)
        {
            IOrderGroupBuyViewDAO rDAO = BusinessCardManagementDAOFactory.OrderGroupBuyViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取拼团订单列表（按拼团分组）
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<OrderByGroupbuyIdViewVO> FindOrderByGroupbuyIdViewList(string condtion)
        {
            IOrderByGroupbuyIdViewDAO rDAO = BusinessCardManagementDAOFactory.OrderByGroupbuyIdViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取拼团订单列表（无分组）
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<OrderGroupBuyNotGroupViewVO> FindOrderGroupBuyNotGroupViewList(string condtion)
        {
            IOrderGroupBuyNotGroupViewDAO rDAO = BusinessCardManagementDAOFactory.OrderGroupBuyNotGroupViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取拼团订单（人数）
        /// </summary>
        /// <returns></returns>
        public int FindOrderGroupBuyViewCount(string condition)
        {
            IOrderGroupBuyViewDAO rDAO = BusinessCardManagementDAOFactory.OrderGroupBuyViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 添加订阅消息会员
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddSubscription(int CustomerId, string OpenId)
        {
            ISubscriptionDAO rDAO = BusinessCardManagementDAOFactory.SubscriptionDAO(this.CurrentCustomerProfile);
            List<SubscriptionVO> sVOlist = rDAO.FindByParams("CustomerId=" + CustomerId);

            if (sVOlist.Count > 0)
            {
                return sVOlist[0].SubscriptionID;
            }

            SubscriptionVO sVO = new SubscriptionVO();
            sVO.SubscriptionID = 0;
            sVO.CustomerId = CustomerId;
            sVO.OpenId = OpenId;
            sVO.CreatedAt = DateTime.Now;

            try
            {
                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int SubscriptionID = rDAO.Insert(sVO);
                    return SubscriptionID;
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
        /// 判断会员是否订阅消息
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool IsSubscription(int CustomerId)
        {
            ISubscriptionDAO rDAO = BusinessCardManagementDAOFactory.SubscriptionDAO(this.CurrentCustomerProfile);
            List<SubscriptionVO> sVOlist = rDAO.FindByParams("CustomerId=" + CustomerId);
            if (sVOlist.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取订阅会员
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public SubscriptionVO FindSubscriptionByCustomerId(int CustomerId)
        {
            ISubscriptionDAO rDAO = BusinessCardManagementDAOFactory.SubscriptionDAO(this.CurrentCustomerProfile);
            List<SubscriptionVO> sVOlist = rDAO.FindByParams("CustomerId=" + CustomerId);
            if (sVOlist.Count > 0)
            {
                return sVOlist[0];
            }
            return null;
        }

        /// <summary>
        /// 发送订阅消息（名片访问提醒）
        /// </summary>
        /// <param name="CardID">要递的名片ID</param>
        /// <param name="CustomerId">接收人ID</param>
        /// <param name="formId"></param>
        /// <returns></returns>
        public string sendTemplateMessage(int CustomerId, string name, string details, int AppType)
        {
            try
            {
                AppVO AppVO = AppBO.GetApp(AppType);
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppVO.AppId + "&secret=" + AppVO.Secret + "";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var result = new WeiXinAccessTokenResultDYH();
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }
                string DataJson = string.Empty;
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token=" + result.SuccessResult.access_token;

                SubscriptionVO cVO = FindSubscriptionByCustomerId(CustomerId);

                if (cVO != null)
                {
                    DataJson = "{";
                    DataJson += "\"touser\": \"" + cVO.OpenId + "\",";
                    DataJson += "\"template_id\": \"xsO5GlPM85xugPaR-CUYjP9vnirHxXOvw21ckFlUEjg\",";
                    DataJson += "\"page\": \"pages/Statistics/AccessList/AccessList\",";
                    DataJson += "\"data\": {";
                    DataJson += "\"date1\": {";
                    DataJson += "\"value\": \"" + DateTime.Now + "\"";
                    DataJson += "},";
                    DataJson += "\"name3\": {";
                    DataJson += "\"value\": \"" + name + "\"";
                    DataJson += "},";
                    DataJson += "\"thing4\": {";
                    DataJson += "\"value\": \"" + details + "\"";
                    DataJson += "},";
                    DataJson += "\"thing2\": {";
                    DataJson += "\"value\": \"点击查看访问列表\"";
                    DataJson += "}";
                    DataJson += "}";
                    DataJson += "}";

                    string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);
                    return str;
                }
                return "找不到订阅会员";
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "error";
            }
        }

        /// <summary>
        /// 发送订阅消息（订单返现提醒）
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        public string sendGroupBuyMessage(string OpenId, string Title, decimal Cost, string OrderNO, int BusinessID, int InfoID, int AppType)
        {
            try
            {
                string url;
                AppVO AppVO = AppBO.GetApp(AppType);
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppVO.AppId + "&secret=" + AppVO.Secret + "";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var result = new WeiXinAccessTokenResultDYH();
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }
                string DataJson = string.Empty;
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token=" + result.SuccessResult.access_token;

                DataJson = "{";
                DataJson += "\"touser\": \"" + OpenId + "\",";
                DataJson += "\"template_id\": \"TELmLxrH9dSfUdU47JLfOvcU8YdcGPyKhWpNRWqoxyY\",";
                DataJson += "\"page\": \"pages/GroupBuy/MyGroupBuy/MyGroupBuy?BusinessID=" + BusinessID + "&InfoID=" + InfoID + "\",";
                DataJson += "\"data\": {";
                DataJson += "\"thing2\": {";
                DataJson += "\"value\": \"" + Title + "\"";
                DataJson += "},";
                DataJson += "\"amount4\": {";
                DataJson += "\"value\": \"" + Cost + "\"";
                DataJson += "},";
                DataJson += "\"character_string1\": {";
                DataJson += "\"value\": \"" + OrderNO + "\"";
                DataJson += "},";
                DataJson += "\"time5\": {";
                DataJson += "\"value\": \"" + DateTime.Now + "\"";
                DataJson += "}";
                DataJson += "}";
                DataJson += "}";

                string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);
                return str;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "error";
            }
        }


        /// <summary>
        /// 发送订阅消息（提现状态提醒）
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        public string sendPayOutMessage(BcPayOutHistoryVO pVO, int AppType)
        {
            try
            {
                AppVO AppVO = AppBO.GetApp(AppType);
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppVO.AppId + "&secret=" + AppVO.Secret + "";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var result = new WeiXinAccessTokenResultDYH();
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }
                string DataJson = string.Empty;
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token=" + result.SuccessResult.access_token;

                string Status = "";
                if (pVO.PayOutStatus == 1)
                {
                    Status = "提现成功";
                }
                else if (pVO.PayOutStatus == -2)
                {
                    Status = "提现失败";
                }
                else
                {
                    return "";
                }

                string page = "";
                if (pVO.Type == 1)
                {
                    page = "pages/RebatePayOut/RebatePayOut";
                }
                if (pVO.Type == 2)
                {
                    page = "pages/BusinessPayOut/BusinessPayOut";
                }

                DataJson = "{";
                DataJson += "\"touser\": \"" + pVO.OpenId + "\",";
                DataJson += "\"template_id\": \"VlyFvxobgOztzpWvaLDo755nTwFSgKO_T8ldCkXYRSc\",";
                DataJson += "\"page\": \"" + page + "\",";
                DataJson += "\"data\": {";
                DataJson += "\"amount1\": {";
                DataJson += "\"value\": \"" + pVO.PayOutCost + "元\"";
                DataJson += "},";
                DataJson += "\"amount2\": {";
                DataJson += "\"value\": \"" + pVO.Cost + "元\"";
                DataJson += "},";
                DataJson += "\"phrase4\": {";
                DataJson += "\"value\": \"" + Status + "\"";
                DataJson += "},";
                DataJson += "\"thing5\": {";
                DataJson += "\"value\": \"" + pVO.HandleComment + "\"";
                DataJson += "},";
                DataJson += "\"time6\": {";
                DataJson += "\"value\": \"" + pVO.HandleDate + "\"";
                DataJson += "}";
                DataJson += "}";
                DataJson += "}";

                string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);
                return str;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "error";
            }
        }

        /// <summary>
        /// 添加分享记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddShare(ShareVO vo)
        {
            try
            {
                IShareDAO rDAO = BusinessCardManagementDAOFactory.ShareDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int ShareID = rDAO.Insert(vo);
                    return ShareID;
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
        /// 更新分享记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateShare(ShareVO vo)
        {
            IShareDAO rDAO = BusinessCardManagementDAOFactory.ShareDAO(this.CurrentCustomerProfile);
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
        /// 获取分享记录
        /// </summary>
        /// <param name="ShareID"></param>
        /// <returns></returns>
        public ShareVO FindShareById(int ShareID)
        {
            IShareDAO rDAO = BusinessCardManagementDAOFactory.ShareDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(ShareID);
        }
        /// <summary>
        /// 获取分享记录列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<ShareVO> FindShareList(string condtion)
        {
            IShareDAO rDAO = BusinessCardManagementDAOFactory.ShareDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取分享记录数量
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int FindShareByCondition(string condition)
        {
            IShareDAO rDAO = BusinessCardManagementDAOFactory.ShareDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 获取我的页面数据（分享，销售）
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <param name="BusinessID"></param>
        /// <param name="TimeType">TimeType：1:昨天数据；2:7天内数据；3:30天内数据</param>
        /// <returns></returns>
        public MyDataVO GetMyData(int PersonalID, int BusinessID, int TimeType = 1)
        {
            CrmBO cBO = new CrmBO(new CustomerProfile());
            MyDataVO myVO = new MyDataVO();

            myVO.PersonalID = PersonalID;
            myVO.BusinessID = BusinessID;

            if (BusinessID <= 0)
            {
                return myVO;
            }

            string sqlTime = "";
            string sqlTime2 = "";

            if (TimeType == 1)
            {
                sqlTime = " and DATEDIFF(CreatedAt,NOW())=-1";
                sqlTime2 = " and DATEDIFF(CreatedAt,NOW())=-2";
            }
            if (TimeType == 2)
            {
                sqlTime = " and DATEDIFF(CreatedAt,NOW())<=-1 and DATEDIFF(CreatedAt,NOW())>=-7";
                sqlTime2 = " and DATEDIFF(CreatedAt,NOW())<-7 and DATEDIFF(CreatedAt,NOW())>=-14";
            }
            if (TimeType == 3)
            {
                sqlTime = " and DATEDIFF(CreatedAt,NOW())<=-1 and DATEDIFF(CreatedAt,NOW())>=-30";
                sqlTime2 = " and DATEDIFF(CreatedAt,NOW())<-30 and DATEDIFF(CreatedAt,NOW())>=-60";
            }

            myVO.TotalShare = FindShareByCondition("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=1" + sqlTime);
            myVO.TotalRead = FindShareByCondition("ToPersonalID=" + PersonalID + " and PersonalID<>" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=0" + sqlTime);

            //算出浏览量公司排名
            List<PersonalVO> pVO = FindPersonalByBusinessID(BusinessID);
            List<int> bRead = new List<int>();
            for (int i = 0; i < pVO.Count; i++)
            {
                if (pVO[i].PersonalID != PersonalID)
                {
                    int Read = 0;
                    Read = FindShareByCondition("ToPersonalID=" + pVO[i].PersonalID + " and PersonalID<>" + pVO[i].PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=0" + sqlTime);
                    bRead.Add(Read);
                }
            }

            int TotalRanking = 1;
            for (int i = 0; i < bRead.Count; i++)
            {
                if (bRead[i] > myVO.TotalShare)
                {
                    TotalRanking += 1;
                }
            }
            myVO.TotalRanking = TotalRanking;

            //企业名片
            myVO.PersonalShare = FindShareByCondition("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=1 and Type='Personal'" + sqlTime);
            myVO.PersonalRead = FindShareByCondition("ToPersonalID=" + PersonalID + " and PersonalID<>" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=0 and Type='Personal'" + sqlTime);
            myVO.PersonalShareUP = myVO.PersonalShare > FindShareByCondition("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=1 and Type='Personal'" + sqlTime2);
            myVO.PersonalConversion = myVO.PersonalShare == 0 ? 0 : decimal.Round((decimal)myVO.PersonalRead / (decimal)myVO.PersonalShare, 2);

            //企业软文
            myVO.NewsShare = FindShareByCondition("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=1 and Type='News'" + sqlTime);
            myVO.NewsRead = FindShareByCondition("ToPersonalID=" + PersonalID + " and PersonalID<>" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=0 and Type='News'" + sqlTime);
            myVO.NewsShareUP = myVO.NewsShare > FindShareByCondition("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=1 and Type='News'" + sqlTime2);
            myVO.NewsConversion = myVO.NewsShare == 0 ? 0 : decimal.Round((decimal)myVO.NewsRead / (decimal)myVO.NewsShare, 2);

            //贺卡
            myVO.GreetingCardShare = FindShareByCondition("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=1 and Type='GreetingCard'" + sqlTime);
            myVO.GreetingCardRead = FindShareByCondition("ToPersonalID=" + PersonalID + " and PersonalID<>" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=0 and Type='GreetingCard'" + sqlTime);
            myVO.GreetingCardShareUP = myVO.GreetingCardShare > FindShareByCondition("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=1 and Type='GreetingCard'" + sqlTime2);
            myVO.GreetingCardConversion = myVO.GreetingCardShare == 0 ? 0 : decimal.Round((decimal)myVO.GreetingCardRead / (decimal)myVO.GreetingCardShare, 2);

            //电子彩页
            myVO.ColorPageShare = FindShareByCondition("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=1 and Type='ColorPage'" + sqlTime);
            myVO.ColorPageRead = FindShareByCondition("ToPersonalID=" + PersonalID + " and PersonalID<>" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=0 and Type='ColorPage'" + sqlTime);
            myVO.ColorPageShareUP = myVO.ColorPageShare > FindShareByCondition("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=1 and Type='ColorPage'" + sqlTime2);
            myVO.ColorPageConversion = myVO.ColorPageShare == 0 ? 0 : decimal.Round((decimal)myVO.ColorPageRead / (decimal)myVO.ColorPageShare, 2);

            //产品
            myVO.ProductShare = FindShareByCondition("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=1 and Type='Product'" + sqlTime);
            myVO.ProductRead = FindShareByCondition("ToPersonalID=" + PersonalID + " and PersonalID<>" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=0 and Type='Product'" + sqlTime);
            myVO.ProductShareUP = myVO.ProductShare > FindShareByCondition("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and SendOrReceive=1 and Type='Product'" + sqlTime2);
            myVO.ProductConversion = myVO.ProductShare == 0 ? 0 : decimal.Round((decimal)myVO.ProductRead / (decimal)myVO.ProductShare, 2);

            //销售线索
            myVO.ClueAdd = cBO.FindCrmCount("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and Type='Clue'" + sqlTime);
            myVO.ClueAddUP = myVO.ClueAdd > cBO.FindCrmCount("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and Type='Clue'" + sqlTime2);

            //客户
            myVO.ClientsAdd = cBO.FindCrmCount("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and Type='Clients'" + sqlTime);
            myVO.ClientsAddUP = myVO.ClientsAdd > cBO.FindCrmCount("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and Type='Clients'" + sqlTime2);

            //拜访
            myVO.GoOutAdd = cBO.FindCrmCount("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and Type='GoOut'" + sqlTime);
            myVO.GoOutAddUP = myVO.GoOutAdd > cBO.FindCrmCount("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and Type='GoOut'" + sqlTime2);

            //合同
            myVO.ContractAdd = cBO.FindCrmCount("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and Type='Contract'" + sqlTime);
            myVO.ContractAddUP = myVO.ContractAdd > cBO.FindCrmCount("PersonalID=" + PersonalID + " and BusinessID=" + BusinessID + " and Type='Contract'" + sqlTime2);

            //部门销售指标
            PersonalVO PersonalVO = FindPersonalById(PersonalID);
            DateTime dt = DateTime.Now;
            if (PersonalVO.DepartmentID > 0)
            {
                List<TargetVO> tVO = FindTargetByCondtion("Type='Department' and Year=" + dt.Year + " and Month=" + dt.Month + " and DepartmentID=" + PersonalVO.DepartmentID);

                TargetVO DepartmentTVO = new TargetVO();
                //如果当月没有设置目标，自动创建
                if (tVO.Count == 0)
                {
                    DepartmentTVO.DepartmentID = PersonalVO.DepartmentID;
                    DepartmentTVO.BusinessID = PersonalVO.BusinessID;
                    DepartmentTVO.CreatedAt = DateTime.Now;

                    DateTime dt2 = dt.AddMonths(-1);
                    List<TargetVO> tVO2 = FindTargetByCondtion("Type='Department' and Year=" + dt2.Year + " and Month=" + dt2.Month + " and DepartmentID=" + PersonalVO.DepartmentID);
                    if (tVO2.Count > 0)
                    {
                        DepartmentTVO.Cost = tVO2[0].Cost;
                    }
                    else
                    {
                        DepartmentTVO.Cost = 0;
                    }
                    DepartmentTVO.TargetID = 0;
                    DepartmentTVO.Year = dt.Year;
                    DepartmentTVO.Month = dt.Month;
                    DepartmentTVO.Type = "Department";
                    DepartmentTVO.TargetID = AddTarget(DepartmentTVO);
                }
                else
                {
                    DepartmentTVO = tVO[0];
                }

                myVO.DepartmentTarget = DepartmentTVO.Cost;
                myVO.DepartmentTargetCost = FindTurnoverByDepartmentID(PersonalVO.DepartmentID);
                myVO.DepartmentTargetCompletion = myVO.DepartmentTarget == 0 ? 100 : decimal.Round(myVO.DepartmentTargetCost / (decimal)myVO.DepartmentTarget, 2) * 100;
            }
            else
            {
                myVO.DepartmentTarget = 0;
                myVO.DepartmentTargetCost = FindTurnoverByBusinessID(PersonalVO.BusinessID);
                myVO.DepartmentTargetCompletion = 100;
            }

            //个人销售指标
            TargetVO PersonalTVO = new TargetVO();
            List<TargetVO> ptVO = FindTargetByCondtion("Type='Personal' and Year=" + dt.Year + " and Month=" + dt.Month + " and PersonalID=" + PersonalVO.PersonalID + " and DepartmentID=" + PersonalVO.DepartmentID);

            TargetVO TargetVO = new TargetVO();
            //如果当月没有设置目标，自动创建
            if (ptVO.Count == 0)
            {
                TargetVO.PersonalID = PersonalVO.PersonalID;
                TargetVO.BusinessID = PersonalVO.BusinessID;
                TargetVO.CreatedAt = DateTime.Now;
                TargetVO.DepartmentID = PersonalVO.DepartmentID;

                DateTime dt2 = dt.AddMonths(-1);
                List<TargetVO> tVO2 = FindTargetByCondtion("Type='Personal' and Year=" + dt2.Year + " and Month=" + dt2.Month + " and PersonalID=" + PersonalVO.PersonalID + " and DepartmentID=" + PersonalVO.DepartmentID);
                if (tVO2.Count > 0)
                {
                    TargetVO.Cost = tVO2[0].Cost;
                }
                else
                {
                    TargetVO.Cost = 0;
                }
                TargetVO.TargetID = 0;
                TargetVO.Year = dt.Year;
                TargetVO.Month = dt.Month;
                TargetVO.Type = "Personal";
                TargetVO.TargetID = AddTarget(TargetVO);

                PersonalTVO = TargetVO;
            }
            else
            {
                PersonalTVO = ptVO[0];
            }

            myVO.PersonalTarget = PersonalTVO.Cost;
            myVO.PersonalTargetCost = FindTurnoverByPersonalID(PersonalVO.PersonalID);
            myVO.PersonalTargetCompletion = myVO.PersonalTarget == 0 ? 100 : decimal.Round(myVO.PersonalTargetCost / (decimal)myVO.PersonalTarget, 2) * 100;


            //总团队销售指标
            List<TargetVO> tbVO = FindTargetByCondtion("Type='Business' and Year=" + dt.Year + " and Month=" + dt.Month + " and BusinessID=" + PersonalVO.BusinessID);

            TargetVO BusinessTVO = new TargetVO();
            //如果当月没有设置目标，自动创建
            if (tbVO.Count == 0)
            {
                BusinessTVO.BusinessID = PersonalVO.BusinessID;
                BusinessTVO.CreatedAt = DateTime.Now;

                DateTime dt2 = dt.AddMonths(-1);
                List<TargetVO> tVO2 = FindTargetByCondtion("Type='Business' and Year=" + dt2.Year + " and Month=" + dt2.Month + " and BusinessID=" + PersonalVO.BusinessID);
                if (tVO2.Count > 0)
                {
                    BusinessTVO.Cost = tVO2[0].Cost;
                }
                else
                {
                    BusinessTVO.Cost = 0;
                }
                BusinessTVO.TargetID = 0;
                BusinessTVO.Year = dt.Year;
                BusinessTVO.Month = dt.Month;
                BusinessTVO.Type = "Business";
                BusinessTVO.TargetID = AddTarget(BusinessTVO);
            }
            else
            {
                BusinessTVO = tbVO[0];
            }

            myVO.BusinessTarget = BusinessTVO.Cost;
            myVO.BusinessTargetCost = FindTurnoverByBusinessID(PersonalVO.BusinessID);
            myVO.BusinessTargetCompletion = myVO.BusinessTarget == 0 ? 100 : decimal.Round(myVO.BusinessTargetCost / (decimal)myVO.BusinessTarget, 2) * 100;

            return myVO;
        }

        /// <summary>
        /// 获取个人当月合同成交额
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public decimal FindTurnoverByPersonalID(int PersonalID)
        {
            PersonalVO cVO = FindPersonalById(PersonalID);
            CrmBO cBO = new CrmBO(new CustomerProfile());
            DateTime dt = DateTime.Now;
            List<CrmVO> crm = cBO.FindCrmList("PersonalID=" + PersonalID + " and Type='Contract'" + " AND DATE_FORMAT(CreatedAt,'%y-%m-%d')>=DATE_FORMAT('" + dt.Year + "-" + dt.Month + "-01','%y-%m-%d') AND DATE_FORMAT(CreatedAt, '%y-%m-%d') < DATE_FORMAT('" + dt.AddMonths(1).Year + "-" + dt.AddMonths(1).Month + "-01', '%y-%m-%d')");
            decimal Cost = 0;
            for (int i = 0; i < crm.Count; i++)
            {
                Cost += crm[i].priceB;
            }
            return Cost;
        }
        /// <summary>
        /// 获取部门成员当月合同成交额
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public decimal FindTurnoverByDepartmentID(int DepartmentID)
        {
            IPersonalDAO rDAO = BusinessCardManagementDAOFactory.PersonalDAO(this.CurrentCustomerProfile);
            List<PersonalVO> cVO = rDAO.FindByParams("DepartmentID = " + DepartmentID);
            List<SecondBusinessVO> sVO = FindSecondBusinessByDepartmentID(DepartmentID);
            for (int i = 0; i < sVO.Count; i++)
            {
                PersonalVO pVO = FindPersonalById(sVO[i].PersonalID);
                pVO.BusinessID = sVO[i].BusinessID;
                pVO.DepartmentID = sVO[i].DepartmentID;
                pVO.isExternal = sVO[i].isExternal;
                cVO.Add(pVO);
            }
            decimal Cost = 0;
            for (int i = 0; i < cVO.Count; i++)
            {
                Cost += FindTurnoverByPersonalID(cVO[i].PersonalID);
            }
            return Cost;
        }


        /// <summary>
        /// 获取公司成员当月合同成交额
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public decimal FindTurnoverByBusinessID(int BusinessID)
        {
            IPersonalDAO rDAO = BusinessCardManagementDAOFactory.PersonalDAO(this.CurrentCustomerProfile);

            string sql = "BusinessID = " + BusinessID;
            List<PersonalVO> cVO = rDAO.FindByParams(sql);
            List<SecondBusinessVO> sVO = FindSecondBusinessByBusinessID(BusinessID);
            for (int i = 0; i < sVO.Count; i++)
            {
                PersonalVO pVO = FindPersonalById(sVO[i].PersonalID);
                cVO.Add(pVO);
            }

            decimal Cost = 0;
            for (int i = 0; i < cVO.Count; i++)
            {
                Cost += FindTurnoverByPersonalID(cVO[i].PersonalID);
            }
            return Cost;
        }

        /// <summary>
        /// 添加打卡记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddPunch(PunchVO vo)
        {
            try
            {
                IPunchDAO rDAO = BusinessCardManagementDAOFactory.PunchDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int OrderID = rDAO.Insert(vo);
                    return OrderID;
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
        /// 更新打卡记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdatePunch(PunchVO vo)
        {
            IPunchDAO rDAO = BusinessCardManagementDAOFactory.PunchDAO(this.CurrentCustomerProfile);
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
        /// 获取打卡记录
        /// </summary>
        /// <param name="PunchID"></param>
        /// <returns></returns>
        public PunchVO FindPunchById(int PunchID)
        {
            IPunchDAO rDAO = BusinessCardManagementDAOFactory.PunchDAO(this.CurrentCustomerProfile);
            PunchVO pVO = rDAO.FindById(PunchID);
            CrmBO CrmBO = new CrmBO(new CustomerProfile());
            List<CrmVO> cVO = CrmBO.FindCrmList("PunchID=" + pVO.PunchID + " and Status=1");
            pVO.CrmList = cVO;

            for (int i = 0; i < cVO.Count; i++)
            {
                if (cVO[i].Type == "Daily")
                    pVO.isDaily = true;
            }
            return pVO;
        }

        /// <summary>
        /// 根据日期获取打卡记录
        /// </summary>
        /// <param name="PunchID"></param>
        /// <returns></returns>
        public PunchVO FindPunchByDate(DateTime dt, int PersonalID, int BusinessID)
        {
            IPunchDAO rDAO = BusinessCardManagementDAOFactory.PunchDAO(this.CurrentCustomerProfile);
            string condtion = "  PersonalID=" + PersonalID + " and BusinessID=" + BusinessID;
            string sqlTime = "  AND DATE_FORMAT(PunchInAt,'%y-%m-%d') = DATE_FORMAT('" + dt.ToString("yyyy-MM-dd") + "','%y-%m-%d')";
            List<PunchVO> LpVO = rDAO.FindByParams(condtion + sqlTime);
            PunchVO pVO = new PunchVO();
            if (LpVO.Count <= 0)
            {
                return null;
            }
            else
            {
                pVO = LpVO[0];
            }

            CrmBO CrmBO = new CrmBO(new CustomerProfile());
            List<CrmVO> cVO = CrmBO.FindCrmList("PunchID=" + pVO.PunchID + " and Status=1");
            pVO.CrmList = cVO;
            for (int i = 0; i < cVO.Count; i++)
            {
                if (cVO[i].Type == "Daily")
                {
                    pVO.isDaily = true;
                    pVO.DailyID = cVO[i].CrmID;
                }
                if (cVO[i].Type == "GoOut")
                    pVO.GoOutCount += 1;
            }
            return pVO;
        }
        /// <summary>
        /// 根据日期获取当月打卡统计
        /// </summary>
        /// <param name="PunchID"></param>
        /// <param name="isday">是否按当月每天日期显示</param>
        /// <returns></returns>
        public List<PunchVO> FindMonthPunchByDate(DateTime dt, int PersonalID, int BusinessID, bool isday = true)
        {
            int days = DateTime.DaysInMonth(dt.Year, dt.Month);

            List<PunchVO> ListPVO = new List<PunchVO>();
            for (int i = 1; i <= days; i++)
            {
                DateTime day = new DateTime(dt.Year, dt.Month, i);
                PunchVO pVO = FindPunchByDate(day, PersonalID, BusinessID);
                if (isday)
                {
                    ListPVO.Add(pVO);
                }
                else
                {
                    if (pVO != null)
                        ListPVO.Add(pVO);
                }
            }
            return ListPVO;
        }

        /// <summary>
        /// 根据日期获取所有下属当月打卡统计
        /// </summary>
        /// <param name="PunchID"></param>
        /// <returns></returns>
        public List<PersonalPunchVO> FindMonthPersonalPunchByDate(DateTime dt, int PersonalID, int BusinessID)
        {
            int days = DateTime.DaysInMonth(dt.Year, dt.Month);

            List<PersonalPunchVO> ListPVO = new List<PersonalPunchVO>();

            List<PersonalVO> PersonalVO = FindPersonalByPersonalID(BusinessID, PersonalID);

            for (int i = 0; i < PersonalVO.Count; i++)
            {
                PersonalPunchVO PersonalPunchVO = new PersonalPunchVO();

                PersonalPunchVO.PersonalID = PersonalVO[i].PersonalID;
                PersonalPunchVO.BusinessID = BusinessID;
                PersonalPunchVO.Name = PersonalVO[i].Name;
                PersonalPunchVO.Position = PersonalVO[i].Position;
                PersonalPunchVO.Phone = PersonalVO[i].Phone;
                PersonalPunchVO.Headimg = PersonalVO[i].Headimg;

                List<PunchVO> PunchVO = FindMonthPunchByDate(dt, PersonalVO[i].PersonalID, BusinessID, false);

                int PunchInCount = PunchVO.Count;
                int PunchOutCount = 0;
                int DailyCount = 0;
                int GoOutCount = 0;

                for (int j = 0; j < PunchVO.Count; j++)
                {
                    if (PunchVO[j].isPunchOut)
                    {
                        PunchOutCount += 1;
                    }
                    if (PunchVO[j].isDaily)
                    {
                        DailyCount += 1;
                    }
                    GoOutCount += PunchVO[j].GoOutCount;
                }

                PersonalPunchVO.PunchInCount = PunchInCount;
                PersonalPunchVO.PunchOutCount = PunchOutCount;
                PersonalPunchVO.DailyCount = DailyCount;
                PersonalPunchVO.GoOutCount = GoOutCount;

                //今天是否在上班
                PersonalPunchVO.isWork = false;
                PunchVO pVO = FindPunchByDate(DateTime.Now, PersonalVO[i].PersonalID, BusinessID);
                if (pVO != null)
                {
                    if (!pVO.isPunchOut)
                    {
                        PersonalPunchVO.isWork = true;
                    }
                }


                //当前工作状态
                if (pVO != null)
                {
                    //上班中
                    PersonalPunchVO.WorkStatus = 1;
                    //外出中
                    if (pVO.GoOutCount > 0)
                    {
                        PersonalPunchVO.WorkStatus = 2;
                    }
                    //已下班
                    if (pVO.isPunchOut)
                    {
                        PersonalPunchVO.WorkStatus = 3;
                    }
                    if (pVO.isDaily)
                    {
                        PersonalPunchVO.WorkStatus = 4;
                    }
                }
                else
                {
                    PersonalPunchVO.WorkStatus = 0;
                }


                ListPVO.Add(PersonalPunchVO);
            }
            ListPVO.Sort((a, b) => a.PunchInCount.CompareTo(b.PunchInCount));
            ListPVO.Reverse();
            return ListPVO;
        }

        /// <summary>
        /// 获取地图轨迹
        /// </summary>
        /// <param name="dt">时间</param>
        /// <param name="BusinessID"></param>
        /// <param name="PersonalID">查询人id</param>
        /// <param name="ToPersonalID">指定查询id</param>
        /// <returns></returns>
        public List<PunchMapVO> FindPunchMapByList(DateTime dt, int PersonalID, int ToPersonalID, int BusinessID)
        {
            int days = DateTime.DaysInMonth(dt.Year, dt.Month);

            List<PunchMapVO> ListMapPVO = new List<PunchMapVO>();

            if (ToPersonalID > 0)
            {
                PunchVO PunchVO = FindPunchByDate(dt, ToPersonalID, BusinessID);

                PunchMapVO PunchMapVO = getPunchMap(PunchVO);
                if (PunchMapVO != null)
                {
                    ListMapPVO.Add(PunchMapVO);
                }
                return ListMapPVO;

            }
            List<PersonalVO> PersonalVO = FindPersonalByPersonalID(BusinessID, PersonalID);

            for (int i = 0; i < PersonalVO.Count; i++)
            {

                PunchVO PunchVO = FindPunchByDate(dt, PersonalVO[i].PersonalID, BusinessID);
                PunchMapVO PunchMapVO = getPunchMap(PunchVO);
                if (PunchMapVO != null)
                {
                    ListMapPVO.Add(PunchMapVO);
                }
            }

            return ListMapPVO;
        }

        /// <summary>
        /// 打卡记录转换为地图轨迹
        /// </summary>
        /// <param name="dt">时间</param>
        /// <param name="BusinessID"></param>
        /// <param name="PersonalID">查询人id</param>
        /// <param name="ToPersonalID">指定查询id</param>
        /// <returns></returns>
        public PunchMapVO getPunchMap(PunchVO PunchVO)
        {
            if (PunchVO == null)
            {
                return null;
            }
            PunchMapVO mVO = new PunchMapVO();
            mVO.MapList = new List<MapVO>();
            PersonalVO pVO = FindPersonalById(PunchVO.PersonalID);
            mVO.PersonalID = pVO.PersonalID;
            mVO.BusinessID = pVO.BusinessID;
            mVO.Name = pVO.Name;
            mVO.Phone = pVO.Phone;
            mVO.Headimg = GetAddressIMG(pVO.PersonalID, pVO.Headimg);
            mVO.HeadimgByPunchIn = GetAddressIMG(pVO.PersonalID, pVO.Headimg, "PunchIn");
            mVO.HeadimgByPunchOut = GetAddressIMG(pVO.PersonalID, pVO.Headimg, "PunchOut");
            mVO.HeadimgByGoOut = GetAddressIMG(pVO.PersonalID, pVO.Headimg, "GoOut");
            mVO.Position = pVO.Position;

            MapVO PunchInMapVO = new MapVO();
            PunchInMapVO.Address = PunchVO.PunchInAddress;
            PunchInMapVO.Latitude = PunchVO.PunchInLatitude;
            PunchInMapVO.Longitude = PunchVO.PunchInLongitude;
            PunchInMapVO.Date = PunchVO.PunchInAt;
            PunchInMapVO.Name = "上班打卡";
            PunchInMapVO.Type = "PunchIn";
            mVO.MapList.Add(PunchInMapVO);

            for (int i = 0; i < PunchVO.CrmList.Count; i++)
            {
                if (PunchVO.CrmList[i].Type == "GoOut")
                {
                    MapVO CrmMapVO = new MapVO();
                    CrmMapVO.Address = PunchVO.CrmList[i].Field1;
                    CrmMapVO.Latitude = PunchVO.CrmList[i].Latitude;
                    CrmMapVO.Longitude = PunchVO.CrmList[i].Longitude;
                    CrmMapVO.Date = PunchVO.CrmList[i].CreatedAt;
                    CrmMapVO.Name = PunchVO.CrmList[i].Title;
                    CrmMapVO.Type = "GoOut";
                    mVO.MapList.Add(CrmMapVO);
                }
            }
            if (PunchVO.isPunchOut)
            {
                MapVO PunchOutMapVO = new MapVO();
                PunchOutMapVO.Address = PunchVO.PunchOutAddress;
                PunchOutMapVO.Latitude = PunchVO.PunchOutLatitude;
                PunchOutMapVO.Longitude = PunchVO.PunchOutLongitude;
                PunchOutMapVO.Date = PunchVO.PunchOutAt;
                PunchOutMapVO.Name = "下班打卡";
                PunchOutMapVO.Type = "PunchOut";
                mVO.MapList.Add(PunchOutMapVO);
            }
            return mVO;
        }

        /// <summary>
        /// 保存地图坐标头像
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public string GetAddressIMG(int PersonalID, string Headimg, string Type = "")
        {
            Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.BCAPIURLByHttp + "/GenerateIMG/Address.aspx?Headimg=" + Headimg + "&Type=" + Type, 500, 500, 500, 500);

            //保存
            string filePath = "";
            string folder = "/UploadFolder/PersonalAddressFile/";
            string newFileName = Type + PersonalID + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;

            //使图片变透明
            m_Bitmap.MakeTransparent(Color.FromArgb(0, 0, 0));

            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string PosterImg = ConfigInfo.Instance.APIURL + filePath;
            return PosterImg;
        }

        /// <summary>
        /// 添加拼团
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddGroupBuy(GroupBuyVO vo)
        {
            try
            {
                IGroupBuyDAO rDAO = BusinessCardManagementDAOFactory.GroupBuyDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int GroupBuyID = rDAO.Insert(vo);
                    return GroupBuyID;
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
        /// 更新拼团
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateGroupBuy(GroupBuyVO vo)
        {
            IGroupBuyDAO rDAO = BusinessCardManagementDAOFactory.GroupBuyDAO(this.CurrentCustomerProfile);
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
        /// 获取拼团
        /// </summary>
        /// <param name="ShareID"></param>
        /// <returns></returns>
        public GroupBuyVO FindGroupBuyById(int GroupBuyID)
        {
            IGroupBuyDAO rDAO = BusinessCardManagementDAOFactory.GroupBuyDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(GroupBuyID);
        }

        /// <summary>
        /// 获取拼团列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<GroupBuyVO> FindGroupBuyList(string condtion)
        {
            IGroupBuyDAO rDAO = BusinessCardManagementDAOFactory.GroupBuyDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }


        /// <summary>
        /// 添加拼团成员
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddGroupMemberBuy(GroupBuyMemberVO vo)
        {
            try
            {
                IGroupBuyMemberDAO rDAO = BusinessCardManagementDAOFactory.GroupBuyMemberDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int GroupBuyMemberID = rDAO.Insert(vo);
                    return GroupBuyMemberID;
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
        /// 更新拼团成员
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateGroupBuyMember(GroupBuyMemberVO vo)
        {
            IGroupBuyMemberDAO rDAO = BusinessCardManagementDAOFactory.GroupBuyMemberDAO(this.CurrentCustomerProfile);
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
        /// 获取拼团成员
        /// </summary>
        /// <param name="ShareID"></param>
        /// <returns></returns>
        public GroupBuyMemberVO FindGroupBuyMemberById(int GroupBuyMemberID)
        {
            IGroupBuyMemberDAO rDAO = BusinessCardManagementDAOFactory.GroupBuyMemberDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(GroupBuyMemberID);
        }

        /// <summary>
        /// 获取拼团成员列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<GroupBuyMemberVO> FindGroupBuyMemberList(string condtion)
        {
            IGroupBuyMemberDAO rDAO = BusinessCardManagementDAOFactory.GroupBuyMemberDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取拼团成员列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<GroupBuyMemberVO> FindGroupBuyMemberList(string condtion, int limit)
        {
            IGroupBuyMemberDAO rDAO = BusinessCardManagementDAOFactory.GroupBuyMemberDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(condtion, "CreatedAt", "desc", limit);
        }

        /// <summary>
        /// 获取拼团成员列表(视图)
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<GroupBuyMemberViewVO> FindGroupBuyMemberViewList(string condtion, int limit)
        {
            IGroupBuyMemberViewDAO rDAO = BusinessCardManagementDAOFactory.GroupBuyMemberViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(condtion, "CreatedAt", "desc", limit);
        }

        /// <summary>
        /// 获取拼团成员列表（分页）
        /// </summary>
        /// <returns></returns>
        public List<GroupBuyMemberViewVO> FindGroupBuyMemberViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IGroupBuyMemberViewDAO rDAO = BusinessCardManagementDAOFactory.GroupBuyMemberViewDAO(this.CurrentCustomerProfile);
            List<GroupBuyMemberViewVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            return cVO;
        }

        /// <summary>
        /// 获取拼团成员列表（数量）
        /// </summary>
        /// <returns></returns>
        public int FindGroupBuyMemberViewCount(string condition)
        {
            IGroupBuyMemberViewDAO rDAO = BusinessCardManagementDAOFactory.GroupBuyMemberViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 成员退出该产品所有拼团
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public int DeleteGroupBuyMember(int CustomerId, int InfoID)
        {
            IGroupBuyMemberDAO rDAO = BusinessCardManagementDAOFactory.GroupBuyMemberDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("CustomerId = " + CustomerId + " and InfoID=" + InfoID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 添加提现记录（佣金提现）
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddPayOutHistoryByRebate(int CustomerId, string OpenId, string AccountName)
        {
            BcPayOutHistoryVO vo = new BcPayOutHistoryVO();
            vo.CustomerId = CustomerId;
            vo.PayOutDate = DateTime.Now;
            vo.PayOutStatus = 0;
            vo.OpenId = OpenId;
            vo.AccountName = AccountName;
            vo.Type = 1;

            decimal Balance = getMyRebateCost(CustomerId, 1);
            if (Balance < 1)
            {
                return -2;
            }

            string sql = "RebateCid1=" + CustomerId + " and payAt is not NULL and Status=1 and RebateStatus1=0";
            List<OrderViewVO> ov1 = FindOrderViewList(sql);
            foreach (OrderViewVO OrderViewVO in ov1)
            {
                OrderVO OrderVO = FindOrderById(OrderViewVO.OrderID);
                if (OrderVO.RebateStatus1 == 0)
                {
                    OrderVO.RebateStatus1 = 2;
                    if (UpdateOrder(OrderVO))
                    {
                        vo.PayOutCost += OrderVO.RebateCost1;
                        vo.OrderID += OrderVO.OrderID + "|RebateStatus1,";
                    }
                }
            }

            sql = "RebateCid2=" + CustomerId + " and payAt is not NULL and Status=1 and RebateStatus2=0";
            List<OrderViewVO> ov2 = FindOrderViewList(sql);
            foreach (OrderViewVO OrderViewVO in ov2)
            {
                OrderVO OrderVO = FindOrderById(OrderViewVO.OrderID);
                if (OrderVO.RebateStatus2 == 0)
                {
                    OrderVO.RebateStatus2 = 2;
                    if (UpdateOrder(OrderVO))
                    {
                        vo.PayOutCost += OrderVO.RebateCost2;
                        vo.OrderID += OrderVO.OrderID + "|RebateStatus2,";
                    }
                }
            }

            sql = "RebateCid3=" + CustomerId + " and payAt is not NULL and Status=1 and RebateStatus3=0";
            List<OrderViewVO> ov3 = FindOrderViewList(sql);
            foreach (OrderViewVO OrderViewVO in ov3)
            {
                OrderVO OrderVO = FindOrderById(OrderViewVO.OrderID);
                if (OrderVO.RebateStatus3 == 0)
                {
                    OrderVO.RebateStatus3 = 2;
                    if (UpdateOrder(OrderVO))
                    {
                        vo.PayOutCost += OrderVO.RebateCost3;
                        vo.OrderID += OrderVO.OrderID + "|RebateStatus3,";
                    }
                }
            }

            sql = "RebateCid4=" + CustomerId + " and payAt is not NULL and Status=1 and RebateStatus4=0";
            List<OrderViewVO> ov4 = FindOrderViewList(sql);
            foreach (OrderViewVO OrderViewVO in ov4)
            {
                OrderVO OrderVO = FindOrderById(OrderViewVO.OrderID);
                if (OrderVO.RebateStatus4 == 0)
                {
                    OrderVO.RebateStatus4 = 2;
                    if (UpdateOrder(OrderVO))
                    {
                        vo.PayOutCost += OrderVO.RebateCost4;
                        vo.OrderID += OrderVO.OrderID + "|RebateStatus4,";
                    }
                }
            }

            sql = "PeopleRebateCid=" + CustomerId + " and payAt is not NULL and Status=1 and PeopleRebateStatus=0";
            List<OrderViewVO> PRov = FindOrderViewList(sql);
            foreach (OrderViewVO OrderViewVO in PRov)
            {
                OrderVO OrderVO = FindOrderById(OrderViewVO.OrderID);
                if (OrderVO.PeopleRebateStatus == 0)
                {
                    OrderVO.PeopleRebateStatus = 2;
                    if (UpdateOrder(OrderVO))
                    {
                        vo.PayOutCost += OrderVO.PeopleRebateCost;
                        vo.OrderID += OrderVO.OrderID + "|PeopleRebateStatus,";
                    }
                }
            }

            //费率
            double rate = 0.006;

            //服务费
            double PayOutCost = Convert.ToDouble(vo.PayOutCost);
            double ServiceCharge = Math.Round(PayOutCost * rate, 2);
            vo.ServiceCharge = Convert.ToDecimal(ServiceCharge);

            //实转金额
            vo.Cost = vo.PayOutCost - vo.ServiceCharge;

            try
            {
                IBcPayOutHistoryDAO rDAO = BusinessCardManagementDAOFactory.BcPayOutHistoryDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int PayOutHistoryId = rDAO.Insert(vo);
                    return PayOutHistoryId;
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
        /// 添加提现记录（商家提现）
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddPayOutHistoryByBusiness(decimal Cost, int BusinessID, int CustomerId, string OpenId)
        {
            BcPayOutHistoryVO vo = new BcPayOutHistoryVO();
            vo.CustomerId = CustomerId;
            vo.BusinessID = BusinessID;
            vo.PayOutDate = DateTime.Now;
            vo.PayOutStatus = 0;
            vo.OpenId = OpenId;

            List<BcBankAccountVO> bankVO = FindBankAccountList("BusinessID=" + BusinessID);

            if (bankVO.Count == 0)
            {
                return -1;
            }

            vo.AccountName = bankVO[0].AccountName;
            vo.BankAccount = bankVO[0].BankAccount;
            vo.BankName = bankVO[0].BankName;
            vo.BankAccountID = bankVO[0].BankAccountID;
            vo.Type = 2;
            vo.PayOutCost = Cost;

            if (vo.PayOutCost < 1)
            {
                return -2;
            }

            //费率
            double rate = 0.01;

            //服务费
            double PayOutCost = Convert.ToDouble(vo.PayOutCost);
            double ServiceCharge = Math.Round(PayOutCost * rate, 2);
            vo.ServiceCharge = Convert.ToDecimal(ServiceCharge);

            //实转金额
            vo.Cost = vo.PayOutCost - vo.ServiceCharge;

            //减少钱包余额
            if (ReduceCardBalance(BusinessID, vo.PayOutCost))
            {
                try
                {
                    IBcPayOutHistoryDAO rDAO = BusinessCardManagementDAOFactory.BcPayOutHistoryDAO(this.CurrentCustomerProfile);

                    CommonTranscation t = new CommonTranscation();
                    t.TranscationContextWithReturn += delegate ()
                    {
                        int PayOutHistoryId = rDAO.Insert(vo);
                        return PayOutHistoryId;
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
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 商家订单结算
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int OrderSettlement(int BusinessID)
        {
            string sql = "ProdustsBusinessID=" + BusinessID + " and payAt is not NULL and Status=1 and PayOutStatus=0 and isAgentBuy=0 and isEcommerceBuy=0 and (ProfitsharingCost=0 or ProfitsharingStatus=1) and (TowProfitsharingCost=0 or TowProfitsharingStatus=1)";
            List<OrderViewVO> ov1 = FindOrderViewList(sql);
            decimal Cost = 0;
            string OrderID = "";

            foreach (OrderViewVO OrderViewVO in ov1)
            {
                OrderVO OrderVO = FindOrderById(OrderViewVO.OrderID);
                if (OrderVO.PayOutStatus == 0)
                {
                    OrderVO.PayOutStatus = 1;
                    UpdateOrder(OrderVO);
                    OrderID += OrderVO.OrderID + "|PayOutStatus,";
                }
            }

            BalanceHistoryVO vo = new BalanceHistoryVO();
            vo.Balance = Cost;
            vo.OrderID = OrderID;
            vo.BusinessID = BusinessID;

            //总收入
            decimal TotalBalance = getBusinessCost(BusinessID, 0);

            //可提现余额
            decimal Balance = getBusinessCost(BusinessID, 1);

            //已提现余额
            decimal PayOutBalance = getBusinessCost(BusinessID, 2);

            //提现中余额
            decimal PayOutInProgressBalance = getBusinessCost(BusinessID, 3);
            vo.oldBalance = Balance;
            vo.newBalance = TotalBalance - PayOutBalance - PayOutInProgressBalance;
            if (Cost <= 0 && vo.oldBalance == vo.newBalance) { return -1; }

            SetBalance(BusinessID, vo.newBalance);
            try
            {
                IBalanceHistoryDAO rDAO = BusinessCardManagementDAOFactory.BalanceHistoryDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int BalanceHistoryId = rDAO.Insert(vo);
                    return BalanceHistoryId;
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
        /// 添加金额变动记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddBalanceHistory(BalanceHistoryVO vo)
        {
            try
            {
                IBalanceHistoryDAO rDAO = BusinessCardManagementDAOFactory.BalanceHistoryDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int BalanceHistoryId = rDAO.Insert(vo);
                    return BalanceHistoryId;
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
        /// 更新提现记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateBcPayOutHistory(BcPayOutHistoryVO vo)
        {
            IBcPayOutHistoryDAO rDAO = BusinessCardManagementDAOFactory.BcPayOutHistoryDAO(this.CurrentCustomerProfile);
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
        /// 获取提现数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindBcPayoutHistoryTotalCount(string condition, params object[] parameters)
        {
            IBcPayOutHistoryDAO rDAO = BusinessCardManagementDAOFactory.BcPayOutHistoryDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 获取提现金额
        /// </summary>
        /// <returns></returns>
        public decimal FindPayOutSumCost(int CustomerId)
        {
            IBcPayOutHistoryDAO rDAO = BusinessCardManagementDAOFactory.BcPayOutHistoryDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalSum("PayOutCost", "CustomerId=" + CustomerId + " and Type=1 and (PayOutStatus=1 or PayOutStatus=0)");
        }

        /// <summary>
        /// 获取提现金额
        /// </summary>
        /// <returns></returns>
        public decimal FindPayOutSumCost(int BusinessID, int PayOutStatus)
        {
            IBcPayOutHistoryDAO rDAO = BusinessCardManagementDAOFactory.BcPayOutHistoryDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalSum("PayOutCost", "BusinessID=" + BusinessID + " and Type=2 and PayOutStatus=" + PayOutStatus);
        }


        /// <summary>
        /// 获取提现记录
        /// </summary>
        /// <returns></returns>
        public BcPayOutHistoryVO FindBcPayOutHistoryById(int PayoutHistoryId)
        {
            IBcPayOutHistoryDAO rDAO = BusinessCardManagementDAOFactory.BcPayOutHistoryDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(PayoutHistoryId);
        }

        /// <summary>
        /// 获取提现记录列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<BcPayOutHistoryVO> FindBcPayOutHistoryList(string condtion)
        {
            IBcPayOutHistoryDAO rDAO = BusinessCardManagementDAOFactory.BcPayOutHistoryDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取提现记录列表（分页）
        /// </summary>
        /// <returns></returns>
        public List<BcPayOutHistoryVO> FindBcPayOutHistoryAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IBcPayOutHistoryDAO rDAO = BusinessCardManagementDAOFactory.BcPayOutHistoryDAO(this.CurrentCustomerProfile);
            List<BcPayOutHistoryVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            return cVO;
        }

        /// <summary>
        /// 获取商家银行卡列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<BcBankAccountVO> FindBankAccountList(string condtion)
        {
            IBankAccountDAO rDAO = BusinessCardManagementDAOFactory.BankAccountDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 添加商家银行卡
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddBankAccount(BcBankAccountVO vo)
        {
            try
            {
                IBankAccountDAO rDAO = BusinessCardManagementDAOFactory.BankAccountDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int BalanceHistoryId = rDAO.Insert(vo);
                    return BalanceHistoryId;
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
        /// 更新商家银行卡
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateBankAccount(BcBankAccountVO vo)
        {
            IBankAccountDAO rDAO = BusinessCardManagementDAOFactory.BankAccountDAO(this.CurrentCustomerProfile);
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
        /// 判断收入是否合法
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="totalCommission"></param>
        /// <returns></returns>
        public bool isLegitimate(int customerId)
        {
            decimal TotalMoney = getMyRebateCost(customerId, 0);//总收入
            decimal CardBalance = FindPayOutSumCost(customerId);//已提现或提现中

            return TotalMoney > CardBalance;
        }

        /// <summary>
        /// 提现处理
        /// </summary>
        /// <param name="PayOutVO"></param>
        /// <returns></returns>
        public bool HandlePayOut(BcPayOutHistoryVO PayOutVO, int AppType)
        {
            //个人提现
            if (PayOutVO.Type == 1)
            {
                if (PayOutVO.PayOutStatus == 1)
                {
                    HandlePayOutByOrderID(PayOutVO.OrderID, 1);
                }
                else if (PayOutVO.PayOutStatus == -2)
                {
                    HandlePayOutByOrderID(PayOutVO.OrderID, 0);
                }
                else if (PayOutVO.PayOutStatus == 0)
                {
                    HandlePayOutByOrderID(PayOutVO.OrderID, 2);
                }
                else
                {
                    return false;
                }
            }

            //公司提现
            if (PayOutVO.Type == 2)
            {
                if (PayOutVO.PayOutStatus == -2)
                {
                    //返回提现金额
                    PlusCardBalance(PayOutVO.BusinessID, PayOutVO.PayOutCost);
                }
            }

            BcPayOutHistoryVO cVO = new BcPayOutHistoryVO();
            cVO.PayOutHistoryId = PayOutVO.PayOutHistoryId;
            cVO.PayOutStatus = PayOutVO.PayOutStatus;
            cVO.HandleComment = PayOutVO.HandleComment;
            cVO.ThirdOrder = PayOutVO.ThirdOrder;
            cVO.HandleDate = DateTime.Now;
            bool Payout = UpdateBcPayOutHistory(cVO);

            //发送通知
            PayOutVO.HandleDate = cVO.HandleDate;
            sendPayOutMessage(PayOutVO, AppType);

            return Payout;
        }

        /// <summary>
        /// 提现处理（返佣订单状态修改）
        /// </summary>
        /// <param name="PayOutVO"></param>
        /// <returns></returns>
        public bool HandlePayOutByOrderID(string OrderID, int RebateStatus)
        {
            LogBO _log = new LogBO(typeof(BusinessCardBO));
            try
            {
                if (!string.IsNullOrEmpty(OrderID))
                {
                    string[] messageIdArr = OrderID.Split(',');
                    for (int i = 0; i < messageIdArr.Length; i++)
                    {
                        try
                        {
                            if (messageIdArr[i] != "")
                            {
                                string Type = messageIdArr[i].Split('|')[1];
                                int ID = Convert.ToInt32(messageIdArr[i].Split('|')[0]);
                                string strErrorMsg = "ID:" + ID.ToString() + "\r\n  Type :" + Type;
                                _log.Error(strErrorMsg);

                                OrderVO OrderVO = FindOrderById(ID);

                                if (OrderVO != null)
                                {
                                    if (Type == "RebateStatus1")
                                    {
                                        OrderVO.RebateStatus1 = RebateStatus;
                                        UpdateOrder(OrderVO);
                                    }
                                    if (Type == "RebateStatus2")
                                    {
                                        OrderVO.RebateStatus2 = RebateStatus;
                                        UpdateOrder(OrderVO);
                                    }
                                    if (Type == "RebateStatus3")
                                    {
                                        OrderVO.RebateStatus3 = RebateStatus;
                                        UpdateOrder(OrderVO);
                                    }
                                    if (Type == "RebateStatus4")
                                    {
                                        OrderVO.RebateStatus4 = RebateStatus;
                                        UpdateOrder(OrderVO);
                                    }
                                    if (Type == "PeopleRebateStatus")
                                    {
                                        OrderVO.PeopleRebateStatus = RebateStatus;
                                        UpdateOrder(OrderVO);
                                    }
                                }
                            }

                        }
                        catch
                        {

                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 添加产品规格
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddInfoCost(InfoCostVO vo)
        {
            try
            {
                IInfoCostDAO rDAO = BusinessCardManagementDAOFactory.InfoCostDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int CostID = rDAO.Insert(vo);
                    return CostID;
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
        /// 更新产品规格
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateInfoCost(InfoCostVO vo)
        {
            IInfoCostDAO rDAO = BusinessCardManagementDAOFactory.InfoCostDAO(this.CurrentCustomerProfile);
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
        /// 获取产品规格
        /// </summary>
        /// <param name="ShareID"></param>
        /// <returns></returns>
        public InfoCostVO FindInfoCostById(int CostID)
        {
            IInfoCostDAO rDAO = BusinessCardManagementDAOFactory.InfoCostDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(CostID);
        }

        /// <summary>
        /// 获取产品规格列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<InfoCostVO> FindInfoCostList(string condtion)
        {
            IInfoCostDAO rDAO = BusinessCardManagementDAOFactory.InfoCostDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 减少余额
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public bool ReduceCardBalance(int BusinessID, decimal balance)
        {
            IBusinessBalanceDAO uDAO = BusinessCardManagementDAOFactory.BalanceDAO(this.CurrentCustomerProfile);
            return uDAO.ReduceBalance(BusinessID, balance);
        }

        /// <summary>
        /// 增加余额
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public bool PlusCardBalance(int BusinessID, decimal balance)
        {
            IBusinessBalanceDAO uDAO = BusinessCardManagementDAOFactory.BalanceDAO(this.CurrentCustomerProfile);
            return uDAO.PlusBalance(BusinessID, balance);
        }

        /// <summary>
        /// 设置余额
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public bool SetBalance(int BusinessID, decimal balance)
        {
            IBusinessBalanceDAO uDAO = BusinessCardManagementDAOFactory.BalanceDAO(this.CurrentCustomerProfile);
            return uDAO.SetBalance(BusinessID, balance);
        }

        /// <summary>
        /// 判断余额是否足够提现
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="totalCommission"></param>
        /// <returns></returns>
        public bool IsHasMoreBusinessBalance(int BusinessID, decimal totalCommission)
        {
            IBusinessBalanceDAO uDAO = BusinessCardManagementDAOFactory.BalanceDAO(this.CurrentCustomerProfile);
            List<BusinessBalanceVO> voList = uDAO.FindByParams("BusinessID = " + BusinessID);
            if (voList.Count > 0)
                return voList[0].Balance >= totalCommission;
            else
                return false;
        }

        /// <summary>
        /// 获取余额
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="totalCommission"></param>
        /// <returns></returns>
        public decimal getBalance(int BusinessID)
        {
            IBusinessBalanceDAO uDAO = BusinessCardManagementDAOFactory.BalanceDAO(this.CurrentCustomerProfile);
            List<BusinessBalanceVO> voList = uDAO.FindByParams("BusinessID = " + BusinessID);
            if (voList.Count > 0)
                return voList[0].Balance;
            else
                return 0;
        }

        /// <summary>
        /// 添加主题样式
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddTheme(ThemeVO vo)
        {
            try
            {
                IThemeDAO rDAO = BusinessCardManagementDAOFactory.ThemeDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int ThemeID = rDAO.Insert(vo);
                    return ThemeID;
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
        /// 更新主题样式
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateTheme(ThemeVO vo)
        {
            IThemeDAO rDAO = BusinessCardManagementDAOFactory.ThemeDAO(this.CurrentCustomerProfile);
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
        /// 获取主题样式
        /// </summary>
        /// <param name="ShareID"></param>
        /// <returns></returns>
        public ThemeVO FindThemeById(int ThemeID)
        {
            IThemeDAO rDAO = BusinessCardManagementDAOFactory.ThemeDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(ThemeID);
        }
        /// <summary>
        /// 获取主题样式列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<ThemeVO> FindThemeList(string condtion)
        {
            IThemeDAO rDAO = BusinessCardManagementDAOFactory.ThemeDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取主题样式数量
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int FindThemeByCondition(string condition)
        {
            IThemeDAO rDAO = BusinessCardManagementDAOFactory.ThemeDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 删除主题样式
        /// </summary>
        /// <param name="InfoID"></param>
        /// <returns></returns>
        public int DeleteThemeById(int ThemeID)
        {
            IThemeDAO rDAO = BusinessCardManagementDAOFactory.ThemeDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("ThemeID = " + ThemeID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 刷新名片分享海报
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public bool GetPosterCardIMGByBusinessID(int BusinessID)
        {
            try
            {
                IPersonalDAO rDAO = BusinessCardManagementDAOFactory.PersonalDAO(this.CurrentCustomerProfile);
                List<PersonalVO> cVO = rDAO.FindByParams("BusinessID = " + BusinessID);
                foreach (PersonalVO item in cVO)
                {
                    GetPosterCardIMG(item.PersonalID, BusinessID);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 添加积分记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddIntegral(IntegralVO vo)
        {
            try
            {
                IIntegralDAO rDAO = BusinessCardManagementDAOFactory.IntegralDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int IntegralID = rDAO.Insert(vo);
                    return IntegralID;
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
        /// 更新积分记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateIntegral(IntegralVO vo)
        {
            IIntegralDAO rDAO = BusinessCardManagementDAOFactory.IntegralDAO(this.CurrentCustomerProfile);
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
        /// 获取积分记录
        /// </summary>
        /// <param name="ShareID"></param>
        /// <returns></returns>
        public IntegralVO FindIntegralById(int IntegralID)
        {
            IIntegralDAO rDAO = BusinessCardManagementDAOFactory.IntegralDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(IntegralID);
        }
        /// <summary>
        /// 获取积分记录列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<IntegralVO> FindIntegralList(string condtion)
        {
            IIntegralDAO rDAO = BusinessCardManagementDAOFactory.IntegralDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取积分记录列表(视图)
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<IntegralViewVO> FindIntegralViewList(string condtion)
        {
            IIntegralViewDAO rDAO = BusinessCardManagementDAOFactory.IntegralViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取积分统计
        /// </summary>
        /// <returns></returns>
        public decimal FindIntegralSumCost(string condtion)
        {
            IIntegralDAO rDAO = BusinessCardManagementDAOFactory.IntegralDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalSum("Balance", "Status=1 and " + condtion);
        }

        /// <summary>
        /// 获取积分记录数量
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int FindIntegralByCondition(string condition)
        {
            IIntegralDAO rDAO = BusinessCardManagementDAOFactory.IntegralDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 获取我的积分
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public decimal FindMyIntegral(int BusinessID, int PersonalID)
        {
            return FindIntegralSumCost("BusinessID = " + BusinessID + " and PersonalID=" + PersonalID);
        }

        /// <summary>
        /// 添加帮助
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddHelp(HelpVO vo)
        {
            try
            {
                IHelpDAO uDAO = BusinessCardManagementDAOFactory.HelpDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int HelpID = uDAO.Insert(vo);
                    return HelpID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新帮助
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateHelp(HelpVO vo)
        {
            IHelpDAO uDAO = BusinessCardManagementDAOFactory.HelpDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 删除帮助
        /// </summary>
        /// <param name="HelpID"></param>
        /// <returns></returns>
        public int DeleteHelpById(int HelpID)
        {
            IHelpDAO uDAO = BusinessCardManagementDAOFactory.HelpDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.DeleteByParams("HelpID = " + HelpID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取帮助列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<HelpVO> FindHelpAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IHelpDAO uDAO = BusinessCardManagementDAOFactory.HelpDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取帮助列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<HelpVO> FindHelpByConditionStr(string condition)
        {
            IHelpDAO uDAO = BusinessCardManagementDAOFactory.HelpDAO(this.CurrentCustomerProfile);
            List<HelpVO> cVO = uDAO.FindByParams(condition);
            return cVO;
        }

        /// <summary>
        /// 获取帮助数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindHelpTotalCount(string condition, params object[] parameters)
        {
            IHelpDAO uDAO = BusinessCardManagementDAOFactory.HelpDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 获取帮助详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public HelpVO FindCardHelpById(int HelpID)
        {
            IHelpDAO uDAO = BusinessCardManagementDAOFactory.HelpDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(HelpID);
        }

        /// <summary>
        /// 获取呼叫中心列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CallCenterVO> GetCallCenterList(string condition)
        {
            ICallCenterDAO uDAO = BusinessCardManagementDAOFactory.CallCenterDAO(this.CurrentCustomerProfile);
            List<CallCenterVO> cVO = uDAO.FindByParams(condition);
            return cVO;
        }
        /// <summary>
        /// 获取号码列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<CallNumberVO> GetCallNumberList(int CallCenterID)
        {
            ICallNumberDAO uDAO = BusinessCardManagementDAOFactory.CallNumberDAO(this.CurrentCustomerProfile);
            List<CallNumberVO> cVO = uDAO.FindByParams("CallCenterID=" + CallCenterID);
            return cVO;
        }


        /// <summary>
        /// 添加代理商保证金记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddAgentIntegral(AgentIntegralVO vo)
        {
            try
            {
                IAgentIntegralDAO rDAO = BusinessCardManagementDAOFactory.AgentIntegralDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int AgentIntegralID = rDAO.Insert(vo);
                    return AgentIntegralID;
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
        /// 更新代理商保证金记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateAgentIntegral(AgentIntegralVO vo)
        {
            IAgentIntegralDAO rDAO = BusinessCardManagementDAOFactory.AgentIntegralDAO(this.CurrentCustomerProfile);
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
        /// 获取代理商保证金记录
        /// </summary>
        /// <param name="ShareID"></param>
        /// <returns></returns>
        public AgentIntegralVO FindAgentIntegralById(int AgentIntegralID)
        {
            IAgentIntegralDAO rDAO = BusinessCardManagementDAOFactory.AgentIntegralDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(AgentIntegralID);
        }
        /// <summary>
        /// 获取代理商保证金记录列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<AgentIntegralVO> FindAgentIntegralList(string condtion)
        {
            IAgentIntegralDAO rDAO = BusinessCardManagementDAOFactory.AgentIntegralDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取代理商保证金统计
        /// </summary>
        /// <returns></returns>
        public decimal FindAgentIntegralSumCost(string condtion)
        {
            IAgentIntegralDAO rDAO = BusinessCardManagementDAOFactory.AgentIntegralDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalSum("Balance", "Status=1 and " + condtion);
        }

        /// <summary>
        /// 获取代理商保证金记录数量
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int FindAgentIntegralByCondition(string condition)
        {
            IAgentIntegralDAO rDAO = BusinessCardManagementDAOFactory.AgentIntegralDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 获取我的代理商保证金
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public decimal FindMyAgentIntegral(int BusinessID, int PersonalID)
        {
            return FindAgentIntegralSumCost("BusinessID = " + BusinessID + " and PersonalID=" + PersonalID);
        }

        /// <summary>
        /// 获取企业权限
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public BusinessCard_JurisdictionVO getBusinessCard_Jurisdiction(int BusinessID)
        {
            BusinessCard_JurisdictionVO BJVO = new BusinessCard_JurisdictionVO();
            BusinessCardVO bVO = FindBusinessCardById(BusinessID);

            BJVO.BusinessID = BusinessID;
            BJVO.isEffective = false;
            BJVO.isVip = false;
            BJVO.isPay = false;
            BJVO.isAgent = false;
            BJVO.isGroup = false;
            BJVO.isCrm = false;
            BJVO.isSubsidiary = false;
            BJVO.isGreetingCard = false;
            BJVO.isWeb = false;
            BJVO.isProduct = false;
            BJVO.isNews = false;
            BJVO.isCase = false;
            BJVO.isColorPage = false;
            BJVO.isVideo = false;
            BJVO.isInfo = false;
            BJVO.isCrmStorehouse = false;
            BJVO.isShangji = false;
            BJVO.isPunch = false;
            BJVO.isTeam = false;
            BJVO.isSearch = false;
            BJVO.isStatistics = false;
            BJVO.isParty = false;
            BJVO.isMarketingData = false;
            BJVO.isGroupBuy = false;
            BJVO.isSeckill = false;

            if (bVO == null)
            {
                return BJVO;
            }

            BJVO.isVip = true;
            BJVO.isEffective = bVO.ExpirationAt > DateTime.Now;

            BJVO.isPay = bVO.isPay == 1;
            BJVO.isAgent = bVO.isAgent == 1;
            BJVO.isGroup = bVO.isGroup == 1;
            BJVO.isSubsidiary = bVO.HeadquartersID > 0;

            //微企版
            if (bVO.OfficialProducts == "SelfEmployed" || bVO.OfficialProducts == "SelfEmployed2")
            {
                BJVO.isWeb = true;
                BJVO.isProduct = true;
                BJVO.isNews = true;
                BJVO.isCase = true;
                BJVO.isParty = true;
            }
            //基础版
            if (bVO.OfficialProducts == "Basic")
            {
                BJVO.isGreetingCard = true;
                BJVO.isWeb = true;
                BJVO.isProduct = true;
                BJVO.isNews = true;
                BJVO.isCase = true;
                BJVO.isColorPage = true;
                BJVO.isVideo = true;
                BJVO.isShangji = true;
                BJVO.isPunch = true;
                BJVO.isStatistics = true;
                BJVO.isParty = true;
                BJVO.isGroupBuy = true;
                BJVO.isSeckill = true;
            }
            //标准版
            if (bVO.OfficialProducts == "Standard")
            {
                BJVO.isGreetingCard = true;
                BJVO.isWeb = true;
                BJVO.isProduct = true;
                BJVO.isNews = true;
                BJVO.isCrm = true;
                BJVO.isCase = true;
                BJVO.isColorPage = true;
                BJVO.isVideo = true;
                BJVO.isInfo = true;
                BJVO.isCrmStorehouse = true;
                BJVO.isShangji = true;
                BJVO.isPunch = true;
                BJVO.isStatistics = true;
                BJVO.isParty = true;
                BJVO.isSearch = true;
                BJVO.isMarketingData = true;
                BJVO.isGroupBuy = true;
                BJVO.isSeckill = true;
            }
            //专业版
            if (bVO.OfficialProducts == "Advanced")
            {
                BJVO.isCrm = true;
                BJVO.isGreetingCard = true;
                BJVO.isWeb = true;
                BJVO.isProduct = true;
                BJVO.isNews = true;
                BJVO.isCase = true;
                BJVO.isColorPage = true;
                BJVO.isVideo = true;
                BJVO.isInfo = true;
                BJVO.isCrmStorehouse = true;
                BJVO.isShangji = true;
                BJVO.isPunch = true;
                BJVO.isTeam = true;
                BJVO.isSearch = true;
                BJVO.isStatistics = true;
                BJVO.isParty = true;
                BJVO.isMarketingData = true;
                BJVO.isGroupBuy = true;
                BJVO.isSeckill = true;
            }
            //集团版
            if (bVO.OfficialProducts == "Group")
            {
                BJVO.isCrm = true;
                BJVO.isGreetingCard = true;
                BJVO.isWeb = true;
                BJVO.isProduct = true;
                BJVO.isNews = true;
                BJVO.isCase = true;
                BJVO.isColorPage = true;
                BJVO.isVideo = true;
                BJVO.isInfo = true;
                BJVO.isCrmStorehouse = true;
                BJVO.isShangji = true;
                BJVO.isPunch = true;
                BJVO.isTeam = true;
                BJVO.isSearch = true;
                BJVO.isStatistics = true;
                BJVO.isParty = true;
                BJVO.isMarketingData = true;
                BJVO.isGroupBuy = true;
                BJVO.isSeckill = true;
            }
            return BJVO;
        }


        /// <summary>
        /// 添加短链接
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddShortUrl(ShortUrlVO vo)
        {
            try
            {
                IShortUrlDAO rDAO = BusinessCardManagementDAOFactory.ShortUrlDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int OrderID = rDAO.Insert(vo);
                    return OrderID;
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
        /// 更新短链接
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateShortUrl(ShortUrlVO vo)
        {
            IShortUrlDAO rDAO = BusinessCardManagementDAOFactory.ShortUrlDAO(this.CurrentCustomerProfile);
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
        /// 获取短链接
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public string FindShortUrl(string code)
        {
            IShortUrlDAO rDAO = BusinessCardManagementDAOFactory.ShortUrlDAO(this.CurrentCustomerProfile);
            List<ShortUrlVO> ShortUrlVO = rDAO.FindByParams("Code = '" + code + "'");
            if (ShortUrlVO.Count > 0)
            {
                return ShortUrlVO[0].Url;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 生成外部打开小程序链接
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public string getWxUrl(string path, string query, int AppType)
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            string oldurl = "https://zhongxiaole.net/card/WxLink.aspx?path=" + HttpUtility.UrlEncode(path) + "&query=" + HttpUtility.UrlEncode(query) + "&AppType=" + AppType;
            ShortUrlVO sVO = new ShortUrlVO();
            sVO.Url = oldurl;
            sVO.Code = cBO.RndCode(8);
            if (AddShortUrl(sVO) > 0)
            {
                return "http://l.leliaomp.com/" + sVO.Code;
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// 获取海报背景列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<FileVO> GetPosterback(int PageCount, int PageIndex)
        {
            List<FileVO> FileList = new List<FileVO>();
            CardBO cBO = new CardBO(new CustomerProfile());
            string sql = "CustomerId = 0 and SizeType=1 order by Order_info desc,CardPoterID desc";
            List<CardPoterVO> fis = cBO.FindCardPoterByCondition(sql);
            foreach (CardPoterVO fi in fis)
            {
                FileVO fVO = new FileVO();
                fVO.Name = fi.FileName;
                fVO.Url = fi.Url;
                FileList.Add(fVO);
            }
            FileList = new List<FileVO>(FileList.Skip((PageIndex - 1) * PageCount).Take(PageCount));
            return FileList;
        }

        /// <summary>
        /// 获取海报数量
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public int GetPosterbackNum()
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            string sql = "CustomerId = 0 and SizeType=1 order by Order_info desc,CardPoterID desc";
            List<CardPoterVO> fis = cBO.FindCardPoterByCondition(sql);
            return fis.Count;
        }


        /// <summary>
        /// 添加店铺VIP
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddShopVip(ShopVipVO vo)
        {
            try
            {
                IShopVipDAO rDAO = BusinessCardManagementDAOFactory.ShopVipDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int AgentIntegralID = rDAO.Insert(vo);
                    return AgentIntegralID;
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
        /// 更新店铺VIP
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateShopVip(ShopVipVO vo)
        {
            IShopVipDAO rDAO = BusinessCardManagementDAOFactory.ShopVipDAO(this.CurrentCustomerProfile);
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
        /// 获取店铺VIP记录
        /// </summary>
        /// <param name="ShareID"></param>
        /// <returns></returns>
        public ShopVipVO FindShopVipById(int ShopVipID)
        {
            IShopVipDAO rDAO = BusinessCardManagementDAOFactory.ShopVipDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(ShopVipID);
        }
        /// <summary>
        /// 获取店铺VIP列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<ShopVipVO> FindShopVipList(string condtion)
        {
            IShopVipDAO rDAO = BusinessCardManagementDAOFactory.ShopVipDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 删除店铺VIP
        /// </summary>
        /// <param name="HelpID"></param>
        /// <returns></returns>
        public int DeleteShopVipById(int ShopVipID)
        {
            IShopVipDAO rDAO = BusinessCardManagementDAOFactory.ShopVipDAO(this.CurrentCustomerProfile);
            IShopVipPersonalDAO pDAO = BusinessCardManagementDAOFactory.ShopVipPersonalDAO(this.CurrentCustomerProfile);
            try
            {
                pDAO.DeleteByParams("ShopVipID = " + ShopVipID);
                rDAO.DeleteByParams("ShopVipID = " + ShopVipID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 添加VIP成员
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool AddShopVipPersonal(int PersonalID, int ShopVipID, int Days, int OrderID)
        {
            ShopVipVO ShopVipVO = FindShopVipById(ShopVipID);
            if (ShopVipVO == null)
            {
                return false;
            }

            List<ShopVipPersonalVO> sVO = FindShopVipPersonalList("ShopVipID=" + ShopVipID + " and PersonalID=" + PersonalID);
            if (sVO.Count > 0)
            {
                ShopVipPersonalVO SPVO = sVO[0];
                if (SPVO.ExpirationAt > DateTime.Now)
                {
                    SPVO.ExpirationAt = SPVO.ExpirationAt.AddDays(Days);
                }
                else
                {
                    SPVO.ExpirationAt = DateTime.Now.AddDays(Days);
                }
                SPVO.CreatedAt = DateTime.Now;
                UpdateShopVipPersonal(SPVO);

                OrderVO OrderVO = FindOrderById(OrderID);
                OrderVO.IsGiveShopVip = 1;
                UpdateOrder(OrderVO);

                return true;
            }
            else
            {
                ShopVipPersonalVO SPVO = new ShopVipPersonalVO();
                SPVO.ShopVipPersonalID = 0;
                SPVO.PersonalID = PersonalID;
                SPVO.BusinessID = ShopVipVO.BusinessID;
                SPVO.ShopVipID = ShopVipVO.ShopVipID;
                SPVO.ExpirationAt = DateTime.Now.AddDays(Days);
                AddShopVipPersonal(SPVO);

                OrderVO OrderVO = FindOrderById(OrderID);
                OrderVO.IsGiveShopVip = 1;
                UpdateOrder(OrderVO);

                return true;
            }
        }

        /// <summary>
        /// 添加VIP成员
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddShopVipPersonal(ShopVipPersonalVO vo)
        {
            try
            {
                IShopVipPersonalDAO rDAO = BusinessCardManagementDAOFactory.ShopVipPersonalDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int ShopVipPersonalID = rDAO.Insert(vo);
                    return ShopVipPersonalID;
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
        /// 更新VIP成员
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateShopVipPersonal(ShopVipPersonalVO vo)
        {
            IShopVipPersonalDAO rDAO = BusinessCardManagementDAOFactory.ShopVipPersonalDAO(this.CurrentCustomerProfile);
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
        /// 获取VIP成员
        /// </summary>
        /// <param name="ShareID"></param>
        /// <returns></returns>
        public ShopVipPersonalVO FindShopVipPersonal(int ShopVipPersonalID)
        {
            IShopVipPersonalDAO rDAO = BusinessCardManagementDAOFactory.ShopVipPersonalDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(ShopVipPersonalID);
        }

        /// <summary>
        /// 获取VIP成员列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<ShopVipPersonalVO> FindShopVipPersonalList(string condtion, int isExpirationAt = 0)
        {
            IShopVipPersonalDAO rDAO = BusinessCardManagementDAOFactory.ShopVipPersonalDAO(this.CurrentCustomerProfile);
            if (isExpirationAt == 1)
            {
                condtion += " and DATE_FORMAT(ExpirationAt,'%y-%m-%d')>=DATE_FORMAT(now(),'%y-%m-%d')";
            }
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取VIP成员数量
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int FindShopVipPersonalCount(string condition, int isExpirationAt = 0)
        {
            IIntegralDAO rDAO = BusinessCardManagementDAOFactory.IntegralDAO(this.CurrentCustomerProfile);
            if (isExpirationAt == 1)
            {
                condition += " and DATE_FORMAT(ExpirationAt,'%y-%m-%d')>=DATE_FORMAT(now(),'%y-%m-%d')";
            }
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 判断是否可以拿到返现并返回一级返现比例
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public int GetProfitsharing(int PersonalID, int InfoID)
        {
            InfoVO infoVO = FindInfoById(InfoID);
            if (infoVO.isProfitsharing == 0)
            {
                return 0;
            }
            if (infoVO.isProfitsharing == 1 && infoVO.isProfitsharingToVIP == 0)
            {
                return infoVO.Profitsharing;
            }
            if (infoVO.isProfitsharing == 1 && infoVO.isProfitsharingToVIP == 1)
            {
                PersonalVO pVO = FindPersonalById(PersonalID);
                List<ProfitsharingToJSON> JSON = JsonConvert.DeserializeObject<List<ProfitsharingToJSON>>(infoVO.ProfitsharingToJSON);
                int Profitsharing = 0;
                foreach (ProfitsharingToJSON item in JSON)
                {
                    List<ShopVipPersonalVO> soList = FindShopVipPersonalList("PersonalID=" + PersonalID + " and ShopVipID=" + item.ShopVipID, 1);
                    if (soList.Count > 0)
                    {
                        if (item.Profitsharing > Profitsharing)
                        {
                            Profitsharing = item.Profitsharing;
                        }
                    }
                }
                return Profitsharing;
            }
            return 0;
        }


        /// <summary>
        /// 判断是否可以拿到Vip优惠折扣
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public int GetVipDiscount(int PersonalID, int InfoID)
        {
            InfoVO infoVO = FindInfoById(InfoID);
            if (infoVO.isVipDiscount == 0)
            {
                return 100;
            }
            if (infoVO.isVipDiscount == 1)
            {
                PersonalVO pVO = FindPersonalById(PersonalID);
                List<VipDiscountToJSON> JSON = JsonConvert.DeserializeObject<List<VipDiscountToJSON>>(infoVO.VipDiscountToJSON);
                int Discount = 100;
                foreach (VipDiscountToJSON item in JSON)
                {
                    List<ShopVipPersonalVO> soList = FindShopVipPersonalList("PersonalID=" + PersonalID + " and ShopVipID=" + item.ShopVipID, 1);
                    if (soList.Count > 0)
                    {
                        if (item.VipDiscount < Discount)
                        {
                            Discount = item.VipDiscount;
                        }
                    }
                }
                return Discount;
            }
            return 100;
        }

        /// <summary>
        /// 添加活动
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddActivity(ActivityVO vo)
        {
            try
            {
                IActivityDAO rDAO = BusinessCardManagementDAOFactory.ActivityDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int ActivityID = rDAO.Insert(vo);
                    return ActivityID;
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
        /// 更新活动
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateActivity(ActivityVO vo)
        {
            IActivityDAO rDAO = BusinessCardManagementDAOFactory.ActivityDAO(this.CurrentCustomerProfile);
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
        /// 获取活动详情
        /// </summary>
        /// <param name="ActID"></param>
        /// <returns></returns>
        public ActivityVO FindActivityById(int ActID)
        {
            IActivityDAO rDAO = BusinessCardManagementDAOFactory.ActivityDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(ActID);
        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<ActivityVO> FindActivityList(string condtion)
        {
            IActivityDAO rDAO = BusinessCardManagementDAOFactory.ActivityDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }
        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="ActId"></param>
        /// <returns></returns>
        public int DeleteActivityById(int ActId)
        {
            IActivityDAO rDAO = BusinessCardManagementDAOFactory.ActivityDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("ActId = " + ActId);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取活动场次列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<ActivityCountVO> FindActivityCountList(string condtion)
        {
            IActivityCountDAO rDAO = BusinessCardManagementDAOFactory.ActivityCountDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取活动场次详情
        /// </summary>
        /// <param name="ActID"></param>
        /// <returns></returns>
        public ActivityCountVO FindActivityCountById(int ActCountId)
        {
            IActivityCountDAO rDAO = BusinessCardManagementDAOFactory.ActivityCountDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(ActCountId);
        }

        /// <summary>
        /// 活动场次
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddActivityCount(ActivityCountVO vo)
        {
            try
            {
                IActivityCountDAO rDAO = BusinessCardManagementDAOFactory.ActivityCountDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int ActivityCountID = rDAO.Insert(vo);
                    return ActivityCountID;
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
        /// 删除活动场次
        /// </summary>
        /// <param name="ActCountId"></param>
        /// <returns></returns>
        public int DeleteActivityCountById(int ActCountId)
        {
            IActivityCountDAO rDAO = BusinessCardManagementDAOFactory.ActivityCountDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("ActCountId = " + ActCountId);
                return 1;
            }
            catch
            {
                return -1;
            }
        }


        /// <summary>
        /// 添加拜访
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool AddVisit(VisitVO vo)
        {
            try
            {
                IVisitDAO rDAO = BusinessCardManagementDAOFactory.VisitDAO(this.CurrentCustomerProfile);
                rDAO.Insert(vo);
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
        /// 获取拜访单信息
        /// </summary>
        /// <param name="VisitID"></param>
        /// <returns></returns>
        public VisitViewVO FindVisitInfo(int VisitID)
        {
            IVisitViewDAO uDAO = BusinessCardManagementDAOFactory.VisitViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(VisitID);
        }
        /// <summary>
        /// 获取拜访单信息
        /// </summary>
        /// <param name="VisitID"></param>
        /// <returns></returns>
        public VisitVO FindVisit(int VisitID)
        {
            IVisitDAO uDAO = BusinessCardManagementDAOFactory.VisitDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(VisitID);
        }

        /// <summary>
        /// 更新拜访信息
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateVisit(VisitVO vo)
        {
            IVisitDAO rDAO = BusinessCardManagementDAOFactory.VisitDAO(this.CurrentCustomerProfile);
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
        /// 获取拜访（分页）
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<VisitViewVO> FindVisitAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IVisitViewDAO rDAO = BusinessCardManagementDAOFactory.VisitViewDAO(this.CurrentCustomerProfile);
            List<VisitViewVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);

            return cVO;
        }

        /// <summary>
        /// 获取拜访的数量
        /// </summary>
        /// <returns></returns>
        public int FindVisitCount(string condition, params object[] parameters)
        {
            IVisitViewDAO rDAO = BusinessCardManagementDAOFactory.VisitViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 获取活动门票列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<ActivityTicketVO> FindActivityTicketList(string condtion)
        {
            IActivityTicketDAO rDAO = BusinessCardManagementDAOFactory.ActivityTicketDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 添加活动门票
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddActivityTicket(ActivityTicketVO vo)
        {
            try
            {
                IActivityTicketDAO rDAO = BusinessCardManagementDAOFactory.ActivityTicketDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int ActivityTicketID = rDAO.Insert(vo);
                    return ActivityTicketID;
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
        /// 更新活动门票
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateActivityTicket(ActivityTicketVO vo)
        {
            IActivityTicketDAO rDAO = BusinessCardManagementDAOFactory.ActivityTicketDAO(this.CurrentCustomerProfile);
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
        /// 获取活动门票详情
        /// </summary>
        /// <param name="ActID"></param>
        /// <returns></returns>
        public ActivityTicketVO FindActivityTicketById(int ActivityTicketID)
        {
            IActivityTicketDAO rDAO = BusinessCardManagementDAOFactory.ActivityTicketDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(ActivityTicketID);
        }

        /// <summary>
        /// 删除活动门票
        /// </summary>
        /// <param name="ActTicketId"></param>
        /// <returns></returns>
        public int DeleteActivityTicketById(int ActTicketId)
        {
            IActivityTicketDAO rDAO = BusinessCardManagementDAOFactory.ActivityTicketDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("ActTicketId = " + ActTicketId);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 添加活动报名
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddActivitySignTicket(ActivitySignTicketVO vo)
        {
            try
            {
                IActivitySignTicketDAO rDAO = BusinessCardManagementDAOFactory.ActivitySignTicketDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int ActivitySignTicketID = rDAO.Insert(vo);
                    return ActivitySignTicketID;
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
        /// 获取活动报名详情
        /// </summary>
        /// <param name="ActID"></param>
        /// <returns></returns>
        public ActivitySignTicketVO FindActivitySignById(int ActivitySignTicketID)
        {
            IActivitySignTicketDAO rDAO = BusinessCardManagementDAOFactory.ActivitySignTicketDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(ActivitySignTicketID);
        }

        /// <summary>
        /// 获取活动报名列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<ActivitySignTicketVO> FindActivitySignList(string condtion)
        {
            IActivitySignTicketDAO rDAO = BusinessCardManagementDAOFactory.ActivitySignTicketDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 更新活动报名
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateActivitySignTicket(ActivitySignTicketVO vo)
        {
            IActivitySignTicketDAO rDAO = BusinessCardManagementDAOFactory.ActivitySignTicketDAO(this.CurrentCustomerProfile);
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
        /// 获取活动报名列表(排序)
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<ActivitySignTicketVO> FindActivitySignListByPageIndex(string conditionStr, string sortcolname, string asc, params object[] parameters)
        {
            IActivitySignTicketDAO rDAO = BusinessCardManagementDAOFactory.ActivitySignTicketDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr, sortcolname, asc, parameters);

        }

        /// <summary>
        /// 获取分享二维码（活动）
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public string GetActQRImg(int ActId, int PersonalID, int AppType, int BusinessID = 0, string Code = "")
        {
            try
            {//删除旧图片
                ActivityVO aVO = FindActivityById(ActId);
                if (aVO.CodeImg != "")
                {
                    string FilePath = aVO.CodeImg;
                    FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }
            }
            catch
            {
            }
            string scene = "0" + ActId.ToString() + "," + PersonalID.ToString();
            if (BusinessID > 0)
                scene = "0" + ActId.ToString() + "," + PersonalID.ToString() + "," + BusinessID;


            if (!string.IsNullOrEmpty(Code))
                scene = "0" + ActId.ToString() + "," + PersonalID.ToString() + "," + BusinessID + "," + Code;

            string QRimg = GetQRcode(scene, 640, "pages/SetUp/ActivityUi/ActivityUi", "/UploadFolder/ActFile/", AppType);
            return QRimg;
        }
        /// <summary>
        /// 生成核销二维码（活动核销码）
        /// </summary>
        /// <param name="PersonalID"></param>
        /// <returns></returns>
        public string GetActHeQRImg(int SignId, int ActId, int PersonalID, int AppType, int BusinessID = 0, string Code = "")
        {
            try
            {
                ActivityVO aVO = FindActivityById(ActId);
                ActivitySignTicketVO svo = FindActivitySignById(SignId);
                if (svo.CodeUrl != "")
                {
                    string FilePath = svo.CodeUrl;
                    FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                    FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                    File.Delete(FilePath);
                }
            }
            catch
            {
            }
            string scene = "1" + SignId.ToString() + ActId.ToString() + "," + PersonalID.ToString();
            if (BusinessID > 0)
                scene = "1" + SignId.ToString() + ActId.ToString() + "," + PersonalID.ToString() + "," + BusinessID;


            if (!string.IsNullOrEmpty(Code))
                scene = "1" + SignId.ToString() + ActId.ToString() + "," + PersonalID.ToString() + "," + BusinessID + "," + Code;

            string QRimg = GetQRcode(scene, 640, "pages/SetUp/ActivityUi/ActivityUi", "/UploadFolder/ActFile/", AppType);
            return QRimg;
        }

        /// <summary>
        /// 判断是否可以拿到返现并返回二级返现比例
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public int GetTowProfitsharing(int CustomerId, int InfoID)
        {
            InfoVO infoVO = FindInfoById(InfoID);
            if (infoVO.isProfitsharing == 0)
            {
                return 0;
            }
            if (infoVO.isProfitsharing == 1 && infoVO.isProfitsharingToVIP == 0)
            {
                return infoVO.TowProfitsharing;
            }
            if (infoVO.isProfitsharing == 1 && infoVO.isProfitsharingToVIP == 1)
            {
                PersonalVO pVO = FindPersonalByCustomerId(CustomerId);
                List<ProfitsharingToJSON> JSON = JsonConvert.DeserializeObject<List<ProfitsharingToJSON>>(infoVO.ProfitsharingToJSON);
                int Profitsharing = 0;
                foreach (ProfitsharingToJSON item in JSON)
                {
                    List<ShopVipPersonalVO> soList = FindShopVipPersonalList("PersonalID=" + pVO.PersonalID + " and ShopVipID=" + item.ShopVipID, 1);
                    if (soList.Count > 0)
                    {
                        if (item.TowProfitsharing > Profitsharing)
                        {
                            Profitsharing = item.TowProfitsharing;
                        }
                    }
                }
                return Profitsharing;
            }
            return 0;
        }

        /// <summary>
        /// 绑定上级
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddProfitsharing(int OrderID)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            OrderViewVO OrderVO = cBO.FindOrderViewById(OrderID);
            //绑定上级
            PersonalVO pVO = cBO.FindPersonalById(OrderVO.PersonalID);
            return cBO.AddProfitsharing(pVO.CustomerId, OrderVO.ProfitsharingCid, OrderVO.ProdustsBusinessID, OrderVO.ProfitsharingOpenId);
        }

        /// <summary>
        /// 添加分销上下级关系
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddProfitsharing(int CustomerId, int ToCustomerId, int BusinessID, string ToOpenID)
        {
            //如果已绑定了其他上级，则取消
            List<ProfitsharingVO> pList = FindProfitsharingList("CustomerId=" + CustomerId + " and BusinessID=" + BusinessID);
            if (pList.Count > 0 || CustomerId == ToCustomerId)
            {
                return -2;
            }

            ProfitsharingVO vo = new ProfitsharingVO();
            vo.ProfitsharingID = 0;
            vo.CustomerId = CustomerId;
            vo.ToCustomerId = ToCustomerId;
            vo.BusinessID = BusinessID;
            vo.ToOpenID = ToOpenID;
            try
            {
                IProfitsharingDAO rDAO = BusinessCardManagementDAOFactory.ProfitsharingDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int ProfitsharingID = rDAO.Insert(vo);
                    return ProfitsharingID;
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
        /// 获取分销上下级关系列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<ProfitsharingVO> FindProfitsharingList(string condtion)
        {
            IProfitsharingDAO rDAO = BusinessCardManagementDAOFactory.ProfitsharingDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }


        /// <summary>
        /// 减去库存
        /// </summary>
        /// <returns></returns>
        public void StoreAmount(int InfoID, int CostID,int InfoNum=1)
        {
            InfoVO sVO = FindInfoById(InfoID);

            if (sVO.StoreAmount > 0)
            {
                //扣除库存
                InfoVO InfoVO = new InfoVO();
                InfoVO.InfoID = sVO.InfoID;
                InfoVO.StoreAmount = sVO.StoreAmount - InfoNum;
                if (InfoVO.StoreAmount == 0) InfoVO.StoreAmount = -1;
                if (CostID > 0)
                {
                    InfoCostVO InfoCostVO = FindInfoCostById(CostID);
                    if (InfoCostVO.PerPersonLimit > 0)
                    {
                        InfoCostVO.PerPersonLimit = InfoCostVO.PerPersonLimit - InfoNum;
                        if (InfoCostVO.PerPersonLimit == 0) InfoCostVO.PerPersonLimit = -1;
                        UpdateInfoCost(InfoCostVO);
                    }
                }
                else
                {
                    if (sVO.PerPersonLimit > 0)
                    {
                        InfoVO.PerPersonLimit = sVO.PerPersonLimit - InfoNum;
                        if (InfoVO.PerPersonLimit == 0) InfoVO.PerPersonLimit = -1;
                    }
                }
                UpdateInfo(InfoVO);
            }

            //秒杀折扣价购买
            if (sVO.SeckillLimit > 0 && sVO.isSeckill == 1)
            {
                //扣除库存
                InfoVO InfoVO = new InfoVO();
                InfoVO.InfoID = sVO.InfoID;
                InfoVO.SeckillLimit = sVO.SeckillLimit - InfoNum;
                if (InfoVO.SeckillLimit == 0) InfoVO.SeckillLimit = -1;
                UpdateInfo(InfoVO);
            }
        }

        /// <summary>
        /// 添加广告
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddAd(AdVO vo)
        {
            try
            {
                IAdDAO rDAO = BusinessCardManagementDAOFactory.AdDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int AdID = rDAO.Insert(vo);
                    return AdID;
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
        /// 更新广告
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateAd(AdVO vo)
        {
            IAdDAO rDAO = BusinessCardManagementDAOFactory.AdDAO(this.CurrentCustomerProfile);
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
        /// 获取广告
        /// </summary>
        /// <param name="ShareID"></param>
        /// <returns></returns>
        public AdVO FindAdById(int AdID)
        {
            IAdDAO rDAO = BusinessCardManagementDAOFactory.AdDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(AdID);
        }
        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<AdVO> FindAdList(string condtion)
        {
            IAdDAO rDAO = BusinessCardManagementDAOFactory.AdDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 获取广告数量
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int FindAdByCondition(string condition)
        {
            IAdDAO rDAO = BusinessCardManagementDAOFactory.AdDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <param name="InfoID"></param>
        /// <returns></returns>
        public int DeleteAdById(int AdID)
        {
            IAdDAO rDAO = BusinessCardManagementDAOFactory.AdDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("AdID = " + AdID);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        #region 活动
        /// <summary>
        /// 添加活动
        /// </summary>
        /// <param name="vo">活动VO</param>
        /// <param name="BCPartySignUpFormVOList">报名信息VO列表</param>
        /// <param name="ContactsList">联系人VO列表</param>
        /// <param name="BCPartyCostVOList">费用VO列表</param>
        /// <returns></returns>
        public int AddParty(BCPartyVO vo, List<BCPartyContactsVO> ContactsList, List<BCPartySignUpFormVO> BCPartySignUpFormVOList, List<BCPartyCostVO> BCPartyCostVOList, bool isAutoPay = false)
        {
            try
            {
                IBCPartyDAO pDAO = BusinessCardManagementDAOFactory.BCPartyDAO(this.CurrentCustomerProfile);
                IBCPartySignUpFormDAO pFormDAO = BusinessCardManagementDAOFactory.BCPartySignUpFormDAO(this.CurrentCustomerProfile);
                IBCPartyContactsDAO pContactsDAO = BusinessCardManagementDAOFactory.BCPartyContactsDAO(this.CurrentCustomerProfile);
                IBCPartyCostDAO pCostDAO = BusinessCardManagementDAOFactory.BCPartyCostDAO(this.CurrentCustomerProfile);


                BCPartySignUpFormVO fVO = new BCPartySignUpFormVO();
                fVO.Name = "姓名";
                fVO.must = 1;
                fVO.Status = 2;

                BCPartySignUpFormVO fVO2 = new BCPartySignUpFormVO();
                fVO2.Name = "手机";
                fVO2.must = 1;
                fVO2.Status = 2;

                bool isName = false;
                bool isPhone = false;

                foreach (BCPartySignUpFormVO item in BCPartySignUpFormVOList)
                {
                    if (item.Name == fVO.Name)
                    {
                        isName = true;
                    }
                    if (item.Name == fVO2.Name)
                    {
                        isPhone = true;
                    }
                }

                if (!isName) BCPartySignUpFormVOList.Add(fVO);
                if (!isPhone) BCPartySignUpFormVOList.Add(fVO2);

                if (BCPartyCostVOList == null)
                {
                    BCPartyCostVOList = new List<BCPartyCostVO>();
                }

                if (ContactsList == null)
                {
                    ContactsList = new List<BCPartyContactsVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = vo.AppType;
                    int PartyID = pDAO.Insert(vo);

                    foreach (BCPartySignUpFormVO bcVO in BCPartySignUpFormVOList)
                    {
                        bcVO.PartyID = PartyID;
                        bcVO.AppType = vo.AppType;
                    }

                    pFormDAO.InsertList(BCPartySignUpFormVOList, 100);

                    foreach (BCPartyCostVO pcVO in BCPartyCostVOList)
                    {
                        pcVO.PartyID = PartyID;
                        if (!isAutoPay)
                        {
                            pcVO.isAutoPay = 0;
                        }
                        pcVO.AppType = vo.AppType;
                    }

                    pCostDAO.InsertList(BCPartyCostVOList, 100);

                    foreach (BCPartyContactsVO tcVO in ContactsList)
                    {
                        tcVO.PartyID = PartyID;
                        tcVO.AppType = vo.AppType;
                    }

                    pContactsDAO.InsertList(ContactsList, 100);

                    return PartyID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Info(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 获取我的报名列表和我发布的活动列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<BCPartySignUpViewVO> FindPartyAndSignUpByCustomerId(int CustomerId, int AppType)
        {
            string sql = "(HostCustomerId = " + CustomerId + " or CustomerId = " + CustomerId + ") and Status<>0 and SignUpStatus<>2 and AppType=" + AppType;
            try
            {
                IBCPartySignUpViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpViewDAO(this.CurrentCustomerProfile);
                return uDAO.FindByParams(sql + " GROUP BY PartyID ORDER BY StartTime DESC, CreatedAt DESC");
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

 

        /// <summary>
        ///  获取我发布的活动列表（分页）
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<BCPartyViewVO> FindPartyViewByPageIndex(string condtion, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IBCPartyViewDAO pDAO = BusinessCardManagementDAOFactory.BCPartyViewDAO(this.CurrentCustomerProfile);
            return pDAO.FindAllByPageIndex(condtion, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        ///  获取我发布的活动总数
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public int FindPartyViewCount(string condtion)
        {
            IBCPartyViewDAO pDAO = BusinessCardManagementDAOFactory.BCPartyViewDAO(this.CurrentCustomerProfile);
            return pDAO.FindTotalCount(condtion);
        }

        /// <summary>
        /// 获取首页全部的活动
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<BCPartySignUpViewVO> FindAllPartyAndSignUp(string condition, int AppType)
        {
            try
            {
                IBCPartySignUpViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpViewDAO(this.CurrentCustomerProfile);
                return uDAO.FindByParams(condition + " AND Status=1 GROUP BY PartyID ORDER BY StartTime DESC, CreatedAt DESC");
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }


        /// <summary>
        /// 更新活动
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateParty(BCPartyVO vo)
        {
            IBCPartyDAO pDAO = BusinessCardManagementDAOFactory.BCPartyDAO(this.CurrentCustomerProfile);
            try
            {
                pDAO.UpdateById(vo);
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
        /// 获取会员在该活动的报名列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<BCPartySignUpVO> FindSignUpByPartyID(int PartyID, int CustomerId)
        {
            IBCPartySignUpDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams("PartyID = " + PartyID + " and CustomerId = " + CustomerId + "  and SignUpStatus<>2 and AppType=" + AppType);
        }

        /// <summary>
        /// 判断是否已报名活动
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns> m  
        public List<BCPartySignUpVO> isJionBCParty(int CustomerId, int PartyID)
        {
            IBCPartySignUpDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpDAO(this.CurrentCustomerProfile);
            List<BCPartySignUpVO> cVO = uDAO.FindByParams("CustomerId = " + CustomerId + " and PartyID=" + PartyID + " and SignUpStatus<>2");
            return cVO;
        }

        /// <summary>
        /// 更新活动联系人
        /// </summary>
        /// <param name="vo">活动VO</param>
        /// <param name="CostList">费用VO列表</param>
        /// <param name="ContactsList">联系人VO列表</param>
        /// <returns></returns>
        public bool UpdateParty(BCPartyVO vo, List<BCPartyContactsVO> ContactsList, List<BCPartySignUpFormVO> BCPartySignUpFormVOList, List<BCPartyCostVO> BCPartyCostVOList)
        {
            try
            {
                IBCPartyDAO pDAO = BusinessCardManagementDAOFactory.BCPartyDAO(this.CurrentCustomerProfile);
                IBCPartySignUpFormDAO pFormDAO = BusinessCardManagementDAOFactory.BCPartySignUpFormDAO(this.CurrentCustomerProfile);
                IBCPartyContactsDAO pContactsDAO = BusinessCardManagementDAOFactory.BCPartyContactsDAO(this.CurrentCustomerProfile);
                IBCPartyCostDAO pCostDAO = BusinessCardManagementDAOFactory.BCPartyCostDAO(this.CurrentCustomerProfile);


                BCPartySignUpFormVO fVO = new BCPartySignUpFormVO();
                fVO.Name = "姓名";
                fVO.must = 1;
                fVO.Status = 2;

                BCPartySignUpFormVO fVO2 = new BCPartySignUpFormVO();
                fVO2.Name = "手机";
                fVO2.must = 1;
                fVO2.Status = 2;

                bool isName = false;
                bool isPhone = false;

                foreach (BCPartySignUpFormVO item in BCPartySignUpFormVOList)
                {
                    if (item.Name == fVO.Name)
                    {
                        isName = true;
                    }
                    if (item.Name == fVO2.Name)
                    {
                        isPhone = true;
                    }
                }

                if (!isName) BCPartySignUpFormVOList.Add(fVO);
                if (!isPhone) BCPartySignUpFormVOList.Add(fVO2);

                if (BCPartyCostVOList == null)
                {
                    BCPartyCostVOList = new List<BCPartyCostVO>();
                }

                if (ContactsList == null)
                {
                    ContactsList = new List<BCPartyContactsVO>();
                }

                CommonTranscation t = new CommonTranscation();
                t.TransactionContext += delegate ()
                {
                    try
                    {
                        if (vo != null)
                            pDAO.UpdateById(vo);
                    }
                    catch
                    {

                    }


                    //删除原来的项，添加新增的

                    List<BCPartySignUpFormVO> rtcDBVOList = pFormDAO.FindByParams("PartyID = " + vo.PartyID);
                    foreach (BCPartySignUpFormVO deleteVO in rtcDBVOList)
                    {
                        pFormDAO.DeleteById(deleteVO.SignUpFormID);
                    }
                    foreach (BCPartySignUpFormVO bcVO in BCPartySignUpFormVOList)
                    {
                        bcVO.PartyID = vo.PartyID;
                    }
                    if (BCPartySignUpFormVOList != null)
                        pFormDAO.InsertList(BCPartySignUpFormVOList, 100);

                    //删除原来的项，添加新增的

                    List<BCPartyCostVO> rCostDBVOList = pCostDAO.FindByParams("PartyID = " + vo.PartyID);
                    bool isAutoPay = false;
                    foreach (BCPartyCostVO deleteVO in rCostDBVOList)
                    {
                        if (deleteVO.isAutoPay == 1)
                        {
                            isAutoPay = true;
                        }
                        pCostDAO.DeleteById(deleteVO.PartyCostID);
                    }
                    foreach (BCPartyCostVO bcVO in BCPartyCostVOList)
                    {
                        if (!isAutoPay)
                        {
                            bcVO.isAutoPay = 0;
                        }
                        bcVO.PartyID = vo.PartyID;
                    }
                    if (BCPartyCostVOList != null)
                        pCostDAO.InsertList(BCPartyCostVOList, 100);


                    //删除不存在的，添加新增的
                    List<BCPartyContactsVO> rcsDBVOList = pContactsDAO.FindByParams("PartyID = " + vo.PartyID);
                    List<BCPartyContactsVO> rcsdeleteVOList = new List<BCPartyContactsVO>();
                    foreach (BCPartyContactsVO dbVO in rcsDBVOList)
                    {
                        bool isDelete = true;
                        for (int i = ContactsList.Count - 1; i >= 0; i--)
                        {
                            BCPartyContactsVO bcVO = ContactsList[i];
                            if (bcVO.PartyContactsID == dbVO.PartyContactsID)
                            {
                                ContactsList.RemoveAt(i);
                                isDelete = false;
                                break;
                            }
                        }
                        if (isDelete)
                        {
                            rcsdeleteVOList.Add(dbVO);
                        }
                    }
                    if (ContactsList != null)
                        pContactsDAO.InsertList(ContactsList, 100);
                    foreach (BCPartyContactsVO deleteVO in rcsdeleteVOList)
                    {
                        pContactsDAO.DeleteById(deleteVO.PartyContactsID);
                    }
                };
                int result = t.Go();
                return result > 0;
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
        /// 生成活动海报
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public string getPosterByPartyID(int PartyID, int style, int CustomerId = 0, int AppType1 = 0)
        {
            try
            {
                IBCPartySignUpDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpDAO(this.CurrentCustomerProfile);
                Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.BCAPIURLByHttp + "/GenerateIMG/CardIMG5.aspx?PartyID=" + PartyID + "&style=" + style + "&CustomerId=" + CustomerId + "&AppType=" + AppType1, 1080, 1920, 1080, 1920);
                if (style == 12) { m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.BCAPIURLByHttp + "/GenerateIMG/CardIMG5.aspx?PartyID=" + PartyID + "&style=" + style + "&CustomerId=" + CustomerId + "&AppType=" + AppType1, 850, 1080, 850, 1080); }
                if (style == 13) { m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.BCAPIURLByHttp + "/GenerateIMG/CardIMG5.aspx?PartyID=" + PartyID + "&style=" + style + "&CustomerId=" + CustomerId + "&AppType=" + AppType1, 650, 722, 650, 722); }
                if (style == 31 || style == 32 || style == 34 || style == 37 || style == 38) { m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.BCAPIURLByHttp + "/GenerateIMG/CardIMG5.aspx?PartyID=" + PartyID + "&style=" + style + "&CustomerId=" + CustomerId + "&AppType=" + AppType1, 1080, 1080, 1080, 1080); }
                if (style == 38) { m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(ConfigInfo.Instance.BCAPIURLByHttp + "/GenerateIMG/CardIMG5.aspx?PartyID=" + PartyID + "&style=" + style + "&CustomerId=" + CustomerId + "&AppType=" + AppType1, 1080, 864, 1080, 864); }

                string filePath = "";
                string folder = "/UploadFolder/BCPartyPosterFile/";
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
                filePath = folder + newFileName;

                try
                {//删除旧图片
                    BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                    BCPartyVO caVO = cBO.FindPartyById(PartyID);

                    if (caVO.PosterImg != "")
                    {
                        string FilePath = caVO.PosterImg;
                        FilePath = FilePath.Replace(ConfigInfo.Instance.APIURL, "");
                        FilePath = ConfigInfo.Instance.UploadFolder + FilePath;
                        File.Delete(FilePath);
                    }

                }
                catch (Exception ex)
                {
                    LogBO _log = new LogBO(typeof(BusinessCardBO));
                    string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                    _log.Error(strErrorMsg);
                }

                //可以修改为网络路径
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string physicalPath = localPath + newFileName;

                m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                string PosterImg = ConfigInfo.Instance.APIURL + filePath;

                BCPartyVO cVO = new BCPartyVO();
                cVO.PartyID = PartyID;
                cVO.PosterImg = PosterImg;
                UpdateParty(cVO);
                return PosterImg;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }


        /// <summary>
        /// 添加报名记录到活动
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCardToParty(BCPartySignUpVO vo)
        {
            try
            {
                IBCPartySignUpDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpDAO(this.CurrentCustomerProfile);
                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = Type;
                    int GroupCardID = uDAO.Insert(vo);
                    return GroupCardID;
                };
                int result = t.Go();
                BCPartyVO cpvo = FindPartyById(vo.PartyID);
                if (cpvo != null)
                {
                    if (cpvo.RecordSignUpCount <= 1)
                    {
                        cpvo.RecordSignUpCount = FindBCPartSignInSumCount("Number", "PartyID=" + cpvo.PartyID + " and (SignUpStatus=1 or SignUpStatus=0)");
                    }
                    else
                    {
                        cpvo.RecordSignUpCount += vo.Number;
                    }
                    UpdateParty(cpvo);
                }
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
        /// 是否是活动联系人
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool isPartyContacts(int PartyID, int CustomerId)
        {
            //ICardPartyContactsViewDAO pContactsDAO = BusinessCardManagementDAOFactory.CardPartyContactsViewDAO(this.CurrentCustomerProfile);
            //return pContactsDAO.FindTotalCount("PartyID=" + PartyID + " and CustomerId=" + CustomerId + " and AppType=" + AppType) > 0;
            return true;
        }

        /// <summary>
        /// 根据活动PartyId获取活动信息
        /// </summary>
        /// <param name="PartyId"></param>
        /// <returns></returns>
        public BCPartyVO FindPartyById(Int64 PartyId)
        {
            IBCPartyDAO pDAO = BusinessCardManagementDAOFactory.BCPartyDAO(this.CurrentCustomerProfile);
            return pDAO.FindById(PartyId);
        }



        /// <summary>
        /// 获取同个收费项所有报名
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<BCPartySignUpViewVO> PartyCostSignUpView(string CostName, decimal cost, int PartyID)
        {
            IBCPartySignUpViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpViewDAO(this.CurrentCustomerProfile);
            List<BCPartySignUpViewVO> cVO = uDAO.FindByParams("CostName = '" + CostName + "' and Cost=" + cost + " and PartyID=" + PartyID + " and PartySignUpID > 0 and SignUpStatus<>2 group by PartySignUpID ");
            return cVO;
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <returns></returns>
        public BCPartyOrderViewVO FindPartyOrderViewById(int PartyOrderID)
        {
            IBCPartyOrderViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartyOrderViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(PartyOrderID);
        }

        /// <summary>
        /// 获取活动的所有邀请者列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<BCPartySignUpViewVO> FindSignUpViewInviterByPartyID(int PartyID)
        {
            IBCPartySignUpViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpViewDAO(this.CurrentCustomerProfile);
            List<BCPartySignUpViewVO> list = uDAO.FindSignUpViewInviterByCon("PartyID = " + PartyID + " and PartySignUpID > 0 and InviterCID!=0");

            return list;
        }

        public List<BCPartyOrderViewVO> GetPartyOrderViewVO(string condtion, bool isAppType = true)
        {
            IBCPartyOrderViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartyOrderViewDAO(this.CurrentCustomerProfile);

            return uDAO.FindByParams(condtion);
        }

        /// <summary>
        /// 添加活动订单
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddPartyOrder(BCPartyOrderVO vo, int AppType)
        {
            try
            {
                IBCPartyOrderDAO uDAO = BusinessCardManagementDAOFactory.BCPartyOrderDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int PartyOrderID = uDAO.Insert(vo);
                    return PartyOrderID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新费用
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public bool UpdateCost(BCPartyCostVO vo)
        {
            IBCPartyCostDAO uDAO = BusinessCardManagementDAOFactory.BCPartyCostDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        public bool UpdatePartyOrder(BCPartyOrderVO vo)
        {
            IBCPartyOrderDAO uDAO = BusinessCardManagementDAOFactory.BCPartyOrderDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <returns></returns>
        public BCPartyOrderVO FindPartyOrderById(int PartyOrderID)
        {
            IBCPartyOrderDAO uDAO = BusinessCardManagementDAOFactory.BCPartyOrderDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(PartyOrderID);
        }

        /// <summary>
        /// 是否是活动联系人
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool AddTest()
        {
            try
            {
                IBCPartyContactsDAO pContactsDAO = BusinessCardManagementDAOFactory.BCPartyContactsDAO(this.CurrentCustomerProfile);
                BCPartyContactsVO cVO = new BCPartyContactsVO();
                cVO.PartyContactsID = 0;
                cVO.PersonalID = 1;
                cVO.PartyID = 1;
                pContactsDAO.Insert(cVO);
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 获取我的报名列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<BCPartySignUpViewVO> FindSignUpViewByCustomerId(int CustomerId, int AppType)
        {
            string sql = "CustomerId = " + CustomerId + " and Status > 0  and SignUpStatus<>2 and isAutoAdd=0 and AppType=" + AppType;
            if (Type != 4)
            {
                sql += " and Type<>3";
            }
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerVO CustomerVO = uBO.FindCustomenById(CustomerId);

            /*
            if (!CustomerVO.isVip)
            {
                sql += " and DATE_SUB(CURDATE(), INTERVAL 30 DAY) <= EndTime";
            }
            */

            try
            {
                IBCPartySignUpViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpViewDAO(this.CurrentCustomerProfile);
                return uDAO.FindByParams(sql + " GROUP BY PartyID ORDER BY StartTime DESC, CreatedAt DESC");
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取我报名的活动过期数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindSignUpViewBeoverdueCount(int CustomerId)
        {
            IBCPartySignUpViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpViewDAO(this.CurrentCustomerProfile);

            string sql = "CustomerId = " + CustomerId + " and Status > 0 and SignUpStatus<>2 and DATE_SUB(CURDATE(), INTERVAL 30 DAY) <= EndTime and AppType=" + Type;
            if (Type != 4)
            {
                sql += " and Type<>3";
            }
            return uDAO.FindTotalCount(sql);
        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<BCPartyVO> FindBCPartyByCondtion(string condtion)
        {
            IBCPartyDAO pDAO = BusinessCardManagementDAOFactory.BCPartyDAO(this.CurrentCustomerProfile);
            return pDAO.FindByParams(condtion);
        }
        /// <summary>
        /// 获取活动分页
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<BCPartyVO> FindBCPartyByPage(string condtion, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IBCPartyDAO pDAO = BusinessCardManagementDAOFactory.BCPartyDAO(this.CurrentCustomerProfile);
            return pDAO.FindAllByPageIndex(condtion, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取活动的所有报名填写信息列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<BCPartySignUpFormVO> FindSignUpFormByPartyID(int PartyID, int Status = 0)
        {
            IBCPartySignUpFormDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpFormDAO(this.CurrentCustomerProfile);
            if (Status == 0)
            {
                return uDAO.FindByParams("PartyID = " + PartyID + " and AppType=" + Type);
            }
            else
            {
                return uDAO.FindByParams("PartyID = " + PartyID + " and Status>0" + " and AppType=" + Type);
            }
        }

        /// <summary>
        /// 获取活动的所有报名列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<BCPartySignUpViewVO> FindSignUpViewByPartyID(int PartyID, bool isDisplayRefund = false)
        {
            IBCPartySignUpViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpViewDAO(this.CurrentCustomerProfile);
            string sql = "PartyID = " + PartyID + " and PartySignUpID > 0 and SignUpStatus<>2 and AppType=" + Type;
            if (isDisplayRefund)
            {
                sql = "PartyID = " + PartyID + " and PartySignUpID > 0  and AppType=" + Type;
            }

            return uDAO.FindByParams(sql);
        }


        /// <summary>
        /// 获取活动的所有报名列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<BCPartySignUpViewVO> FindSignUpViewIndexByPartyID(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            try
            {
                IBCPartySignUpViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpViewDAO(this.CurrentCustomerProfile);
                return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }


        /// <summary>
        /// 获取活动的所有费用信息列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<BCPartyCostVO> FindCostByPartyID(int PartyID)
        {
            IBCPartyCostDAO uDAO = BusinessCardManagementDAOFactory.BCPartyCostDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams("PartyID = " + PartyID + " and AppType=" + Type);
        }

        /// <summary>
        /// 获取活动的所有报名列表数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindBCPartSignInNumTotalCount(string condition, params object[] parameters)
        {
            IBCPartySignUpDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition + " and AppType=" + Type, parameters);
        }

        /// <summary>
        /// 获取活动的报名人数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindBCPartSignInSumCount(string Sum, string condition, params object[] parameters)
        {
            IBCPartySignUpDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalSum(Sum, condition + " and AppType=" + Type, parameters);
        }

        /// <summary>
        /// 获取活动的所有报名列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<BCPartySignUpVO> FindSignUpByCondtion(string condtion)
        {
            IBCPartySignUpDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams("SignUpStatus<>2 and AppType=" + Type + " and " + condtion);
        }

        /// <summary>
        /// 获取活动的邀请人邀请得报名列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <param name="InviterCID"></param>
        /// <returns></returns>
        public List<BCPartySignUpViewVO> FindSignUpViewByInviterCID(int PartyID, int InviterCID, int AppType)
        {
            IBCPartySignUpViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams("PartyID = " + PartyID + " and InviterCID=" + InviterCID + " and PartySignUpID > 0 and SignUpStatus<>2 and AppType=" + AppType + " group by PartySignUpID ");
        }

        ///// <summary>
        ///// 获取活动联系人
        ///// </summary>
        ///// <param name="PartyId"></param>
        ///// <returns></returns>
        //public List<BCPartyContactsViewVO> FindPartyContactsByPartyId(int PartyId)
        //{
        //    IBCPartyContactsViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartyContactsViewDAO(this.CurrentCustomerProfile);
        //    List<BCPartyContactsViewVO> cVO = uDAO.FindByParams("PartyId = " + PartyId);

        //    if (cVO.Count > 0)
        //    {
        //        return cVO;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// 获取活动的所有报名列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<BCPartySignUpVO> FindSignUpByPartyID(int PartyID)
        {
            IBCPartySignUpDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpDAO(this.CurrentCustomerProfile);
            return uDAO.FindByParams("PartyID = " + PartyID + "  and SignUpStatus<>2 ");
        }

        ///// <summary>
        ///// 获取活动联系人
        ///// </summary>
        ///// <param name="PartyId"></param>
        ///// <returns></returns>
        public List<BCPartyContactsVO> FindPartyContactsByPartyId(int PartyId)
        {
            IBCPartyContactsDAO uDAO = BusinessCardManagementDAOFactory.BCPartyContactsDAO(this.CurrentCustomerProfile);
            List<BCPartyContactsVO> cVO = uDAO.FindByParams("PartyId = " + PartyId);

            if (cVO.Count > 0)
            {
                return cVO;
            }
            else
            {
                return null;
            }
        }
        ///// <summary>
        ///// 获取活动联系人
        ///// </summary>
        ///// <param name="PartyId"></param>
        ///// <returns></returns>
        public List<BCPartyContactsViewVO> FindPartyContactsViewByPartyId(int PartyId)
        {
            IBCPartyContactsViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartyContactsViewDAO(this.CurrentCustomerProfile);
            List<BCPartyContactsViewVO> cVO = uDAO.FindByParams("PartyId = " + PartyId);

            if (cVO.Count > 0)
            {
                return cVO;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Datatable生成Excel表格并返回路径
        /// </summary>
        /// <param name="m_DataTable">Datatable</param>
        /// <param name="s_folder">自定义路径</param>
        /// <param name="s_FileName">文件名</param>
        /// <returns></returns>
        public string DataToExcel(System.Data.DataTable m_DataTable, string s_folder, string s_FileName)
        {
            try
            {
                string folder = "/UploadFolder/ExcelFile/" + s_folder;
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                string ExcelUrl = ConfigInfo.Instance.APIURL + folder + s_FileName;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }

                EXCELHelper.ImportExcel(m_DataTable, localPath, s_FileName);
                return ExcelUrl;        //返回生成文件的绝对路径
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);

                return null;
            }

        }

        /// <summary>
        /// 获取活动二维码 携带用户信息
        /// </summary>
        /// <param name="PartyID"></param>
        /// <param name="CustomerId"></param>
        /// <returns>返回一个图片的名字xx.png</returns>
        public string GetCardPartyQRByMessage(int PartyID, Int64 CustomerId, int AppType)
        {
            string jsonstr = "";
            try
            {
                LogBO _log = new LogBO(typeof(CardBO));
                AppVO AppVO = AppBO.GetApp(AppType);
                string url;
                _log.Error(PartyID + "----" + CustomerId);
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppVO.AppId + "&secret=" + AppVO.Secret + "";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var result = new WeiXinAccessTokenResultDYH();
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;

                    string strErrorMsg = "微信登陆错误:" + jsonStr;
                    _log.Error(strErrorMsg);
                }
                else
                {
                    _log.Error("微信返回：" + result);
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }

                string DataJson = string.Empty;
                string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;

                //string page = "package/package_party/Party/PartyShow/PartyShow";

                string page = "pages/index/index";
                //BCPartyVO pVO = FindPartyById(PartyID);

                DataJson = "{";
                DataJson += "\"scene\":\"" + PartyID + "-" + CustomerId + "\",";
                //DataJson += "\"scene\":\"123456\",";
                DataJson += string.Format("\"width\":{0},", 1280);
                DataJson += "\"auto_color\":false,";
                DataJson += string.Format("\"page\":\"{0}\",", page);//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
                DataJson += "\"line_color\":{";
                DataJson += string.Format("\"r\":\"{0}\",", "0");
                DataJson += string.Format("\"g\":\"{0}\",", "0");
                DataJson += string.Format("\"b\":\"{0}\"", "0");
                DataJson += "},";
                DataJson += "\"is_hyaline\":false";
                DataJson += "}";
                _log.Error("地址：【" + wxaurl + "】数据：" + DataJson);
                Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
                Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。
                                                   //保存
                string filePath = "";
                string folder = "/UploadFolder/BCPartyQRTemporaryFile/";
                string newFileName = PartyID + "" + CustomerId + ".png";
                filePath = folder + newFileName;

                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string physicalPath = localPath + newFileName;
                m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

                string Cardimg = ConfigInfo.Instance.APIURL + filePath;
                _log.Error("完成" + Cardimg);
                return Cardimg;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source + " jsonstr:" + jsonstr;
                _log.Error(strErrorMsg);
                return "";
            }
        }

        /// <summary>
        /// 获取报名详情
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <returns></returns>
        public BCPartySignUpViewVO FindSignUpViewById(int PartySignUpID)
        {
            IBCPartySignUpViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpViewDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(PartySignUpID);
        }
        /// <summary>
        /// 判断报名详情
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns> 
        public List<BCPartySignUpViewVO> FindSignUpViewById(int CustomerId, int PartyID, int AppType)
        {
            IBCPartySignUpViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpViewDAO(this.CurrentCustomerProfile);
            List<BCPartySignUpViewVO> cVO = uDAO.FindByParams("CustomerId = " + CustomerId + " and PartyID=" + PartyID + " and Status>0  and AppType=" + AppType + " GROUP BY PartySignUpID ORDER BY StartTime DESC, CreatedAt DESC");
            return cVO;
        }

        /// <summary>
        /// 获取同个收费项所有报名
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<BCPartySignUpViewVO> PartyCostSignUpView(string CostName, int PartyID)
        {
            IBCPartySignUpViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpViewDAO(this.CurrentCustomerProfile);
            List<BCPartySignUpViewVO> cVO = uDAO.FindByParams("CostName = '" + CostName + "' and PartyID=" + PartyID + " and PartySignUpID > 0 and SignUpStatus<>2 group by PartySignUpID ");
            return cVO;
        }

        /// <summary>
        /// 获取费用详情
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public BCPartyCostVO FindCostById(int PartyCostID)
        {
            IBCPartyCostDAO uDAO = BusinessCardManagementDAOFactory.BCPartyCostDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(PartyCostID);
        }

        /// <summary>
        /// 获取用户填写报名信息列表
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public List<BCPartySignUpFormVO> FindSignUpFormByFormStr(string FormStr)
        {
            //<SignUpForm><Name>姓名</Name><Value>黄德陆</Value></SignUpForm><SignUpForm><Name>手机</Name><Value>13905001313</Value></SignUpForm><SignUpForm><Name>工作单位</Name><Value>上海锦天城律师事务所</Value></SignUpForm><SignUpForm><Name>单位地址</Name><Value>福州</Value></SignUpForm><SignUpForm><Name>职位</Name><Value>律师</Value></SignUpForm><SignUpForm><Name>性别</Name><Value>男</Value></SignUpForm><SignUpForm><Name>微信</Name><Value>13905001313</Value></SignUpForm><SignUpForm><Name>人数</Name><Value>1</Value></SignUpForm><SignUpForm><Name>班级</Name><Value>高三一班</Value></SignUpForm>
            List<BCPartySignUpFormVO> sVO = new List<BCPartySignUpFormVO>();

            if (FormStr == "")
                return sVO;

            Regex reg = new Regex(@"<SignUpForm>\s*.*?\s*<\/SignUpForm>");
            System.Text.RegularExpressions.Match m = reg.Match(FormStr);
            while (m.Success)
            {
                BCPartySignUpFormVO cVO = new BCPartySignUpFormVO();
                cVO.Name = Regex.Match(m.Value, @"<Name>(?<text>[^>]+)<\/Name>").Groups["text"].Value.Trim();
                cVO.value = Regex.Match(m.Value, @"<Value>(?<text>[^>]+)<\/Value>").Groups["text"].Value.Trim();
                sVO.Add(cVO);

                m = m.NextMatch();
            }
            return sVO;
        }

        /// <summary>
        /// 获取入场券二维码
        /// </summary>
        /// <param name="PartySignUpID"></param>
        /// <returns></returns>
        public string GetBCPartySignUpQR(int PartySignUpID)
        {
            AppVO AppVO = AppBO.GetApp(Type);
            string url;
            LogBO _log = new LogBO(typeof(BusinessCardBO));
            _log.Error("进入生成二维码");
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppVO.AppId + "&secret=" + AppVO.Secret + "";
            string jsonStr = HttpHelper.HtmlFromUrlGet(url);
            var result = new WeiXinAccessTokenResultDYH();
            if (jsonStr.Contains("errcode"))
            {
                var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                _log.Error("请求失败" + errorResult);
                result.ErrorResult = errorResult;
                result.Result = false;
            }
            else
            {
                _log.Error("tonken请求成功");
                var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                result.SuccessResult = model;
                result.Result = true;
            }
            string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;
            string page = "package/package_party/Party/SignUpShow/SignUpShow";
            //string page = "pages/index/index";
            var data = new
            {
                scene = PartySignUpID.ToString(),
                width = 430,
                auto_color = false,
                page = page,
                line_color = new { r = "0", g = "0", b = "0" },
                is_hyaline = false
            };
            string DataJson = JsonConvert.SerializeObject(data);
            _log.Error("string：" + DataJson);
            Stream str = HttpHelper.HtmlFromUrlPostByStreamNew(wxaurl, DataJson);
            _log.Error("Stream：" + str);

            if (str == null || str.Length == 0)
            {
                _log.Error("获取二维码失败：空响应");
                return "";
            }

            try
            {
                // 尝试读取为图片
                using (Bitmap m_Bitmap = new Bitmap(str))//stream你读取的流。
                {
                    //保存
                    string filePath = "";
                    string folder = "/UploadFolder/BCPartySignFile/";
                    string newFileName = PartySignUpID + ".png";
                    filePath = folder + newFileName;

                    string localPath = ConfigInfo.Instance.UploadFolder + folder;
                    _log.Error("保存地址：" + localPath);
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string physicalPath = localPath + newFileName;
                    m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

                    string Cardimg = ConfigInfo.Instance.APIURL + filePath;

                    BCPartySignUpVO cVO = new BCPartySignUpVO();
                    cVO.PartySignUpID = PartySignUpID;
                    cVO.QRCodeImg = Cardimg;
                    _log.Error("二维码：" + Cardimg);
                    UpdateSignUp(cVO);
                    return Cardimg;
                }
            }
            catch (ArgumentException ex)
            {
                // 可能是非图片数据，读取响应内容查看错误
                str.Position = 0; // 重置流位置
                using (StreamReader reader = new StreamReader(str))
                {
                    string errorContent = reader.ReadToEnd();
                    _log.Error($"微信API错误响应：{errorContent}");
                }
                _log.Error(ex.Message);
                return "";
            }
        }

        public bool UpdateSignUp(BCPartySignUpVO vo)
        {
            IBCPartySignUpDAO uDAO = BusinessCardManagementDAOFactory.BCPartySignUpDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取活动二维码
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public string GetBCPartyQR(int PartyID)
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret + "";
            string jsonStr = HttpHelper.HtmlFromUrlGet(url);
            var result = new WeiXinAccessTokenResultDYH();
            if (jsonStr.Contains("errcode"))
            {
                var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                result.ErrorResult = errorResult;
                result.Result = false;
            }
            else
            {
                var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                result.SuccessResult = model;
                result.Result = true;
            }
            string DataJson = string.Empty;
            string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;


            //string page = "pages/Party/PartyShow/PartyShow";
            //if (Type == 4)
            //{
            //    page = "package/package_party/PartyShow/PartyShow";
            //}
            string page = "package/package_party/Party/PartyShow/PartyShow";

            //BCPartyVO pVO = FindPartyById(PartyID);
            //if ((pVO.Type == 3 || pVO.isBlindBox == 1) && Type == 4)
            //{
            //    page = "package/package_sweepstakes/PartyShow/PartyShow";
            //}

            DataJson = "{";
            DataJson += "\"scene\":\"" + PartyID + "\",";
            DataJson += string.Format("\"width\":{0},", 1280);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", page);//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
            DataJson += "\"line_color\":{";
            DataJson += string.Format("\"r\":\"{0}\",", "0");
            DataJson += string.Format("\"g\":\"{0}\",", "0");
            DataJson += string.Format("\"b\":\"{0}\"", "0");
            DataJson += "},";
            DataJson += "\"is_hyaline\":false";
            DataJson += "}";

            Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
            Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。
            //保存
            string filePath = "";
            string folder = "/UploadFolder/CardPartyFile/";
            string newFileName = PartyID + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string Cardimg = ConfigInfo.Instance.APIURL + filePath;

            BCPartyVO cVO = new BCPartyVO();
            cVO.PartyID = PartyID;
            cVO.QRCodeImg = Cardimg;
            UpdateParty(cVO);
            return Cardimg;
        }

        /// <summary>
        /// 获取核销二维码
        /// </summary>
        /// <param name="PartyID"></param>
        /// <returns></returns>
        public string GetBCPartySignUpQRByUser(int PartyID)
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret + "";
            string jsonStr = HttpHelper.HtmlFromUrlGet(url);
            var result = new WeiXinAccessTokenResultDYH();
            if (jsonStr.Contains("errcode"))
            {
                var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                result.ErrorResult = errorResult;
                result.Result = false;
            }
            else
            {
                var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                result.SuccessResult = model;
                result.Result = true;
            }
            string DataJson = string.Empty;
            string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;

            //string page = "pages/Party/SignUpShowByUesr/SignUpShowByUesr";
            //if (Type == 4)
            //{
            //    page = "package/package_party/SignUpShowByUesr/SignUpShowByUesr";
            //}
            //string page = "package/package_party/Party/SignUpShowByUesr/SignUpShowByUesr";
            string page = "pages/index/index";
            DataJson = "{";
            DataJson += "\"scene\":\"" + PartyID + "\",";
            DataJson += string.Format("\"width\":{0},", 640);
            DataJson += "\"auto_color\":false,";
            DataJson += string.Format("\"page\":\"{0}\",", page);//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
            DataJson += "\"line_color\":{";
            DataJson += string.Format("\"r\":\"{0}\",", "0");
            DataJson += string.Format("\"g\":\"{0}\",", "0");
            DataJson += string.Format("\"b\":\"{0}\"", "0");
            DataJson += "},";
            DataJson += "\"is_hyaline\":false";
            DataJson += "}";

            Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);
            Bitmap m_Bitmap = new Bitmap(str); //stream你读取的流。
            //保存
            string filePath = "";
            string folder = "/UploadFolder/BCPartySignByUserFile/";
            string newFileName = PartyID + ".png";
            filePath = folder + newFileName;

            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string physicalPath = localPath + newFileName;
            m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);

            string Cardimg = ConfigInfo.Instance.APIURL + filePath;
            BCPartyVO cVO = new BCPartyVO();
            cVO.PartyID = PartyID;
            cVO.QRSignInImg = Cardimg;
            UpdateParty(cVO);
            return Cardimg;
        }
        #endregion

        #region 公用
        public bool msg_sec_check1(object obj)
        {
            try
            {
                string content = ObjectToJson(obj);
                content = Regex.Replace(content, ",", "隔断");
                content = Regex.Replace(content, @"[^\u4e00-\u9fa5]", "");
                content = Regex.Replace(content, "姓名", "");
                content = Regex.Replace(content, "手机", "");
                content = Regex.Replace(content, "工作单位", "");
                content = Regex.Replace(content, "单位地址", "");
                content = Regex.Replace(content, "职位", "");
                content = Regex.Replace(content, "性别", "");
                content = Regex.Replace(content, "微信", "");
                content = Regex.Replace(content, "人数", "");
                content = Regex.Replace(content, @"[隔断]+", "隔断");
                content = Regex.Replace(content, "隔断", "【隔断】");

                string url = "https://api.weixin.qq.com/wxa/msg_sec_check?access_token=" + GetAccess_token();
                string DataJson = "{";
                DataJson += "\"content\": \"" + content + "\"";
                DataJson += "}";

                string jsonStr = HttpHelper.HtmlFromUrlPost(url, DataJson);

                LogBO _log = new LogBO(typeof(CardBO));
                var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);

                if (errorResult.errcode == 87014)
                {
                    CustomerBO uBO = new CustomerBO(new CustomerProfile());
                    ViolationVO ViolationVO = new ViolationVO();
                    ViolationVO.ViolationAt = DateTime.Now;
                    ViolationVO.ViolationText = content;
                    uBO.AddViolation(ViolationVO);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// 获取经纬度
        /// </summary>
        /// <returns></returns>
        public WeiXinGeocoder getLatitudeAndLongitude(string address)
        {
            try
            {
                string url = "https://apis.map.qq.com/ws/geocoder/v1/?key=QHBBZ-ZDAWI-TRVGE-5GWEO-PHEKO-Q5BXC&address=" + address;

                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var errorResult = JsonConvert.DeserializeObject<WeiXinGeocoder>(responseString);

                if (errorResult.status == 0)
                    return errorResult;
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CompanyBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string getOpenId(string code)
        {
            string jsonStr = "";
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code";
                jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var result = new WeiXinAccessTokenResult();
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModel>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }
                return result.SuccessResult.openid;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source + "\r\n jsonStr=" + jsonStr + " \r\n code=" + code;
                _log.Error(strErrorMsg);
                return "error";
            }
        }

        /// <summary>
        /// 获取所有的个人信息列表
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public List<PersonalVO> FindPersonalList(string condition)
        {

            IPersonalDAO rDAO = BusinessCardManagementDAOFactory.PersonalDAO(this.CurrentCustomerProfile);
            if (condition == "")
                condition = " 1 = 1";
            //分页，暂取20条数据
            List<PersonalVO> cVO = rDAO.FindAllByPageIndex(condition, 1, 20, "Name", "ASC");

            return cVO;
        }
        #endregion


        #region 给用户发送短信
        /// <summary>
        /// 发送短信通知
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <returns>包含操作结果的自定义响应对象</returns>
        public MessageSendResult SendVisitMessage(string phone, string value)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(phone))
            {
                return new MessageSendResult { Success = false, ErrorMessage = "手机号码不能为空" };
            }
            try
            {
                // 构建短信API请求URL（注意：此处可能存在安全风险，建议使用POST并加密参数）
                var smsApiUrl = $"https://api.leliaomp.com/SendServer/WebAPI/System/SendSMSNotice?phone={Uri.EscapeDataString(phone)}&value={value}";
                // 调用短信API
                var jsonResponse = HttpHelper.HtmlFromUrlGet(smsApiUrl);
                // 解析短信API响应
                var result = new WeiXinAccessTokenResultDYH();
                if (jsonResponse.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonResponse);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonResponse);
                    result.SuccessResult = model;
                    result.Result = true;
                }
                // 验证短信API调用结果
                if (!result.Result)
                {
                    return new MessageSendResult
                    {
                        Success = false,
                        ErrorMessage = $"短信API调用失败: {result.ErrorResult?.errmsg ?? "未知错误"}"
                    };
                }
                // 注意：此处代码逻辑可能存在问题
                // 1. 短信API返回的access_token可能并非微信公众号的access_token
                // 2. 微信订阅消息发送与短信发送的关联性不明确
                //string wxaurl = $"https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token={result.SuccessResult.access_token}";

                // 此处应调用微信API发送订阅消息，但原代码未实现
                // 建议添加以下代码：

                //var wxResponse = HttpHelper.HtmlFromUrlPost(wxaurl, wechatMessageBody);
                //var wxResult = JsonConvert.DeserializeObject<WechatSendResult>(wxResponse);
                //if (wxResult.errcode != 0)
                //{
                //    return new MessageSendResult { Success = false, ErrorMessage = $"微信消息发送失败: {wxResult.errmsg}" };
                //}

                // 由于原代码未实际调用微信API，这里返回成功（需根据实际业务调整）
                return new MessageSendResult { Success = true, Message = "发送成功" };
            }
            catch (Exception ex)
            {
                // 记录详细异常信息
                var logger = new LogBO(typeof(BusinessCardBO));
                var errorMessage = $"发送短信通知时发生异常: {ex.Message}\r\n堆栈跟踪: {ex.StackTrace}\r\n来源: {ex.Source}";
                logger.Error(errorMessage);

                // 返回友好的错误信息
                return new MessageSendResult { Success = false, ErrorMessage = "发送过程中发生错误，请稍后重试" };
            }
        }

        // 自定义结果类，替代简单字符串返回值
        public class MessageSendResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public string ErrorMessage { get; set; }
        }

        #endregion


        /// <summary>
        /// 获取我的订单列表
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<BCPartyOrderViewVO> FindPartyOrderViewByCustomerId(int CustomerId)
        {
            try
            {
                IBCPartyOrderViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartyOrderViewDAO(this.CurrentCustomerProfile);
                return uDAO.FindByParams("CustomerId = " + CustomerId + " and AppType=" + AppType);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }
        public List<BCPartyOrderViewVO> FindPartyOrderViewByCustomerId(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            try
            {
                IBCPartyOrderViewDAO uDAO = BusinessCardManagementDAOFactory.BCPartyOrderViewDAO(this.CurrentCustomerProfile);
                return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }


        #region 问卷星相关接口
        /// <summary>
        /// 添加问卷
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int CreateQuestionnaire(QuestionnaireDataVO vo)
        {
            try
            {
                IQuestionnaireDataDAO rDAO = BusinessCardManagementDAOFactory.QuestionnaireDataDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int QuestionId = rDAO.Insert(vo);
                    return QuestionId;
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
        /// 更新问卷
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateQuestionnaire(QuestionnaireDataVO vo)
        {
            IQuestionnaireDataDAO rDAO = BusinessCardManagementDAOFactory.QuestionnaireDataDAO(this.CurrentCustomerProfile);
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
        /// 添加问卷答题内容答案
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int CreateAnswerSheet(AnswerSheetVO vo)
        {
            try
            {
                IAnswerSheetDAO rDAO = BusinessCardManagementDAOFactory.AnswerSheetDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int QuestionId = rDAO.Insert(vo);
                    return QuestionId;
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
        /// 更新问卷答题内容答案
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateAnswerSheet(AnswerSheetVO vo)
        {
            IAnswerSheetDAO rDAO = BusinessCardManagementDAOFactory.AnswerSheetDAO(this.CurrentCustomerProfile);
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
        /// 查找问卷详情
        /// </summary>
        /// <param name="QuestionId"></param>
        /// <returns></returns>
        public QuestionnaireDataVO FindQuestionById(int QuestionId)
        {
            IQuestionnaireDataDAO rDAO = BusinessCardManagementDAOFactory.QuestionnaireDataDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(QuestionId);
        }

        /// <summary>
        /// 查找问卷星Id
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public QuestionnaireDataVO FindQuestionByActivityIdId(string ActivityId)
        {
            IQuestionnaireDataDAO rDAO = BusinessCardManagementDAOFactory.QuestionnaireDataDAO(this.CurrentCustomerProfile);
            List<QuestionnaireDataVO> VO = rDAO.FindByParams(" activity_id = " + ActivityId);
            if (VO.Count > 0)
            {
                return VO[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 问卷分页
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<QuestionnaireDataVO> GetQuestionnaireList(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            try
            {
                IQuestionnaireDataDAO uDAO = BusinessCardManagementDAOFactory.QuestionnaireDataDAO(this.CurrentCustomerProfile);
                return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 查询前几条问卷
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<QuestionnaireDataVO> FindQuestionnaireByFour(int limit)
        {
            IQuestionnaireDataDAO rDAO = BusinessCardManagementDAOFactory.QuestionnaireDataDAO(this.CurrentCustomerProfile);
            return rDAO.FindByFour(limit);
        }

        /// <summary>
        /// 查询前几条问卷
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public bool printQuestionnaire(string json)
        {
            LogBO _log = new LogBO(typeof(CardBO));
            string strErrorMsg = json;
            _log.Error(strErrorMsg);
            return true;
        }

        /// <summary>
        /// 获取问卷总数量
        /// </summary>
        /// <returns></returns>
        public int FindQuestionnaireDataCount(string condition)
        {
            IQuestionnaireDataDAO rDAO = BusinessCardManagementDAOFactory.QuestionnaireDataDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 答卷分页
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<AnswerSheetViewVO> GetAnswerSheetViewList(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            try
            {
                IAnswerSheetViewDAO uDAO = BusinessCardManagementDAOFactory.AnswerSheetViewDAO(this.CurrentCustomerProfile);
                List<AnswerSheetViewVO> cVO = uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
                return cVO;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }

        /// <summary>
        /// 查询用户已填写问卷数量
        /// </summary>
        /// <returns></returns>
        public int FindAnswerSheetViewCount(string condition)
        {
            IAnswerSheetViewDAO rDAO = BusinessCardManagementDAOFactory.AnswerSheetViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 问卷分页
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<AnswerSheetVO> GetAnswerSheetList(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            try
            {
                IAnswerSheetDAO uDAO = BusinessCardManagementDAOFactory.AnswerSheetDAO(this.CurrentCustomerProfile);
                return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }
        /// <summary>
        /// 移除指定字段
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string RemoveField(string jsonData, string fieldName)
        {
            // 解析JSON为JObject（可操作的JSON对象）
            JObject jsonObject = JObject.Parse(jsonData);

            // 移除指定字段
            if (jsonObject.ContainsKey(fieldName))
            {
                jsonObject.Remove(fieldName);
            }

            // 重新序列化为JSON字符串
            return jsonObject.ToString(Formatting.Indented); // 带缩进格式，更易读
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="aesKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string Decrypt(byte[] encryptedData, string aesKey)
        {
            try
            {

                //1）读取推送的BASE64数据为byte[] encryptedData;
                if (encryptedData == null || encryptedData.Length < 17)
                    return null;
                //2）取AES加解密密钥作为AES解密的KEY;
                byte[] key = Encoding.UTF8.GetBytes(aesKey);
                //3) 取byte[] encryptedData的前16位做为IV；
                byte[] iv = encryptedData.Take(16).ToArray();
                //4）取第16位后的字节数组做为待解密内容；
                encryptedData = encryptedData.Skip(16).ToArray();
                using (var aes = new RijndaelManaged())
                {
                    //5）解密模式使用CBC（密码块链模式）；
                    aes.Mode = CipherMode.CBC;
                    //6）填充模式使用PKCS #7（填充字符串由一个字节序列组成，每个字节填充该字节序列的长度）；
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = key;
                    aes.IV = iv;
                    var cryptoTransform = aes.CreateDecryptor();
                    //7）使用配置好的实例化AES对象执行解密
                    byte[] r = cryptoTransform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                    //8）使用UTF-8的方式，读取二进制数组得到原始数据
                    return Encoding.UTF8.GetString(r);
                }

            }
            catch (CryptographicException ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                throw new Exception("解密失败（密钥/IV不匹配或数据损坏）", ex);
            }
        }

        /// <summary>
        /// 生成问卷星签名
        /// </summary>
        /// <param name="appid">应用ID</param>
        /// <param name="appkey">应用密钥</param>
        /// <param name="vid">视频ID</param>
        /// <returns>生成的签名字符串</returns>
        /// <exception cref="ArgumentException">当必要参数为空时抛出</exception>
        public static string GenerateWjxSign(string vid)
        {
            var appid = "3301926";
            var appkey = "defdaac404ac4ab59dd4729114e1d19e";
            // 验证必要参数
            if (string.IsNullOrEmpty(vid))
                throw new ArgumentException("vid不能为空", nameof(vid));

            // 计算时间戳（Unix时间戳，秒级）
            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            string ts = ((long)timeSpan.TotalSeconds).ToString();

            // 使用排序字典构造参数（会自动按Key排序）
            var parameters = new SortedDictionary<string, string>(StringComparer.Ordinal)
            {
                ["appid"] = appid,
                ["vid"] = vid,
            };

            // 预分配足够容量以减少内存分配
            var toSign = new StringBuilder(
                appid.Length + ts.Length + 8 + vid.Length + 2 + // 固定参数长度
                appkey.Length // appkey长度
            );

            // 拼接签名原串
            foreach (var kvp in parameters)
            {
                if (!string.IsNullOrEmpty(kvp.Value))
                {
                    toSign.Append(kvp.Value);
                }
            }

            // 追加appkey
            toSign.Append(appkey);

            // SHA1计算
            using (var sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(toSign.ToString()));
                StringBuilder signBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    signBuilder.Append(b.ToString("x2"));
                }
                return signBuilder.ToString();
            }
        }

        #region 录音
        /// <summary>
        /// 添加录音内容
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int CreateRecording(RecordingRecordsVO vo)
        {
            try
            {
                IRecordingRecordsDAO rDAO = BusinessCardManagementDAOFactory.RecordingRecordsDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int QuestionId = rDAO.Insert(vo);
                    return QuestionId;
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
        /// 更新录音
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateRecording(RecordingRecordsVO vo)
        {
            IRecordingRecordsDAO rDAO = BusinessCardManagementDAOFactory.RecordingRecordsDAO(this.CurrentCustomerProfile);
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
        /// 查找录音详情
        /// </summary>
        /// <param name="QuestionId"></param>
        /// <returns></returns>
        public RecordingRecordsVO FindRecordingById(int recording_records_id)
        {
            IRecordingRecordsDAO rDAO = BusinessCardManagementDAOFactory.RecordingRecordsDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(recording_records_id);
        }

        /// <summary>
        /// 查找录音列表  lzm add
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>

        /// <summary>
        /// 查找录音列表  lzm add
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public DataTable FindRecordingByCondtion(string condtion, int param1, int param2, out int total)
        {

            string strSQL = "";
            strSQL += " SELECT count(0) as total from t_wjx_recording_records r left join t_bc_personal p on r.personalid=p.personalid \n";
            strSQL += " Where \n";
            strSQL += condtion;

            total = Convert.ToInt32(DbHelper.ExecuteDataTable(strSQL).Rows[0]["total"].ToString());


            strSQL = "";
            strSQL += " SELECT r.*,p.Name as username,p.Phone  as phone from t_wjx_recording_records r left join t_bc_personal p on r.personalid=p.personalid \n";
            strSQL += " Where \n";
            strSQL += condtion + " LIMIT " + param1 + ", " + param2;


            return DbHelper.ExecuteDataTable(strSQL);



            // IRecordingRecordsDAO rDAO = BusinessCardManagementDAOFactory.RecordingRecordsDAO(this.CurrentCustomerProfile);
            //     return rDAO.FindByParams(condtion);

        }

        #endregion

        #endregion

        #region 粤省情抽奖相关接口
        /// <summary>
        /// 添加中奖记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int CreateCJWinningRecords(CJWinningRecordsVO vo)
        {
            try
            {
                ICJWinningRecordsDAO rDAO = BusinessCardManagementDAOFactory.CJWinningRecordsDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int QuestionId = rDAO.Insert(vo);
                    return QuestionId;
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
        /// 更新抽奖记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCJWinningRecords(CJWinningRecordsVO vo)
        {
            ICJWinningRecordsDAO rDAO = BusinessCardManagementDAOFactory.CJWinningRecordsDAO(this.CurrentCustomerProfile);
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
        /// 添加抽奖活动
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddCJLotteries(CJLotteriesVO vo)
        {
            try
            {
                ICJLotteriesDAO rDAO = BusinessCardManagementDAOFactory.CJLotteriesDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int QuestionId = rDAO.Insert(vo);
                    return QuestionId;
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
        /// 更新抽奖
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCJLotteries(CJLotteriesVO vo)
        {
            ICJLotteriesDAO rDAO = BusinessCardManagementDAOFactory.CJLotteriesDAO(this.CurrentCustomerProfile);
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
        /// 抽奖详情
        /// </summary>
        /// <param name="lottery_id"></param>
        /// <returns></returns>
        public CJLotteriesVO FindCJLotteriesById(int lottery_id)
        {
            ICJLotteriesDAO rDAO = BusinessCardManagementDAOFactory.CJLotteriesDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(lottery_id);
        }

        /// <summary>
        /// 抽奖活动分页
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CJLotteriesVO> GetCJLotteriesList(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            try
            {
                ICJLotteriesDAO rDAO = BusinessCardManagementDAOFactory.CJLotteriesDAO(this.CurrentCustomerProfile);
                var cj = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
                return cj;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }
        /// <summary>
        /// 查询抽奖活动数量
        /// </summary>
        /// <returns></returns>
        public int FindCJLotteriesCount(string conditionStr, params object[] parameters)
        {
            try
            {
                ICJLotteriesDAO rDAO = BusinessCardManagementDAOFactory.CJLotteriesDAO(this.CurrentCustomerProfile);
                return rDAO.FindTotalCount(conditionStr, parameters);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        public int FindWinningRecordCount(string condition)
        {
            ICJWinningRecordsDAO rDAO = BusinessCardManagementDAOFactory.CJWinningRecordsDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        public decimal FindWinningRecordSum(string sum, string condition)
        {
            ICJWinningRecordsDAO rDAO = BusinessCardManagementDAOFactory.CJWinningRecordsDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalSum(sum, condition);
        }

        /// <summary>
        /// 抽奖记录
        /// </summary>
        /// <param name="lottery_id"></param>
        /// <returns></returns>
        public CJWinningRecordsVO FindCJWinningRecordsById(int winningrecords_id)
        {
            ICJWinningRecordsDAO rDAO = BusinessCardManagementDAOFactory.CJWinningRecordsDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(winningrecords_id);
        }

        /// <summary>
        /// 抽奖订单记录
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public List<CJWinningRecordsVO> FindCJWinningRecordsByorderNo(string orderNo)
        {
            ICJWinningRecordsDAO rDAO = BusinessCardManagementDAOFactory.CJWinningRecordsDAO(this.CurrentCustomerProfile);
            List<CJWinningRecordsVO> cVO = rDAO.FindByParams("payment_no = '" + orderNo + "'");
            return cVO;
        }

        /// <summary>
        /// 生成随机金额
        /// </summary>
        public decimal GenerateRandomAmount(decimal minValue, decimal maxValue, decimal totalAmount, decimal distributedAmount)
        {
            decimal remainingAmount = totalAmount - distributedAmount;

            if (remainingAmount <= 0)
                return 0;

            // 如果剩余金额小于最小金额，返回剩余金额
            if (remainingAmount < minValue)
                return Math.Round(remainingAmount, 2);

            // 如果剩余金额小于最大金额，调整最大金额
            if (remainingAmount < maxValue)
                maxValue = remainingAmount;

            // 生成随机金额
            Random random = new Random();
            double min = (double)minValue;
            double max = (double)maxValue;

            // 生成介于min和max之间的随机金额
            double randomAmount = random.NextDouble() * (max - min) + min;

            // 保留两位小数
            return Math.Round((decimal)randomAmount, 2);
        }

        /// <summary>
        /// 开奖领取
        /// </summary>
        /// <returns></returns>
        public int SaveWinningRecord(int lottery_id, int personal_id, decimal amount, string code, int AppType)
        {
            try
            {
                var openid = getOpenIdByGeneral(code, AppType);
                //  创建中奖记录
                CJWinningRecordsVO winningRecord = new CJWinningRecordsVO
                {
                    lottery_id = lottery_id,
                    personal_id = personal_id,
                    winning_amount = amount,
                    winning_time = DateTime.Now,
                    status = 0, // 未兑奖
                    openid = openid
                };
                int recordId = CreateCJWinningRecords(winningRecord);

                //CardBO cBO = new CardBO(new CustomerProfile());
                //// 8. 调用微信支付接口
                //string retu = cBO.PayforWXLotteries(amount, lottery_id, personal_id, openid, "问卷调查中奖奖金");
                //LogBO _log = new LogBO(typeof(BusinessCardBO));
                //_log.Error(retu);
                //if (retu == "SUCCESS")
                //{
                //    winningRecord.status = 1;
                //    winningRecord.payment_time = DateTime.Now;
                //    UpdateCJWinningRecords(winningRecord);
                //}
                return recordId;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return 0;
            }
        }

        ///// <summary>
        ///// 开奖领取
        ///// </summary>
        ///// <returns></returns>
        //public bool WinningRecordPayment(CJWinningRecordsVO vo, int AppType)
        //{
        //    try
        //    {

        //        // 8. 调用微信支付接口
        //        var retu = WechatPayToChange(vo.winning_amount, vo.lottery_id, vo.personal_id, vo.openid, AppType, "问卷调查中奖奖金");
        //        LogBO _log = new LogBO(typeof(BusinessCardBO));
        //        _log.Error("领取接口" + retu);
        //        if (retu == "SUCCESS")
        //        {
        //            vo.status = 1;
        //            vo.payment_time = DateTime.Now;
        //            UpdateCJWinningRecords(vo);
        //            // 更新抽奖活动中奖人数
        //            CJLotteriesVO lo = FindCJLotteriesById(vo.lottery_id);
        //            lo.winner_count = lo.winner_count + 1;
        //            UpdateCJLotteries(lo);
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogBO _log = new LogBO(typeof(BusinessCardBO));
        //        string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
        //        _log.Error(strErrorMsg);
        //        return false;
        //    }
        //}

        /// <summary>
        /// 开奖领取 - 返回完整微信支付结果
        /// </summary>
        /// <returns>微信支付完整结果</returns>
        public WeChatTransferResult WinningRecordPayment(CJWinningRecordsVO vo, int AppType)
        {
            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

                // 调用微信支付接口
                var transferResult = cBO.WechatPayToChange(vo.winning_amount, vo.lottery_id, vo.personal_id, vo.openid, AppType, "问卷调查中奖奖金");

                LogBO _log = new LogBO(typeof(BusinessCardBO));
                _log.Info($"领取接口返回: Success={transferResult.Success}, State={transferResult.State}, Message={transferResult.Message}");

                // 根据不同的状态处理业务逻辑
                if (transferResult.Success)
                {
                    switch (transferResult.State)
                    {
                        case "SUCCESS":
                            // 转账直接成功，更新数据库状态
                            vo.status = 1; // 已支付
                            vo.payment_time = DateTime.Now;
                            vo.payment_no = transferResult.OutBillNo; // 记录商户单号
                            UpdateCJWinningRecords(vo);

                            // 更新抽奖活动中奖人数
                            CJLotteriesVO lo = FindCJLotteriesById(vo.lottery_id);
                            if (lo != null)
                            {
                                lo.winner_count = lo.winner_count + 1;
                                UpdateCJLotteries(lo);
                            }

                            _log.Info($"转账直接成功，记录ID: {vo.lottery_id}, 微信转账单号: {transferResult.TransferBillNo}");
                            break;

                        case "WAIT_USER_CONFIRM":
                            // 需要用户确认收款，暂时不更新数据库状态
                            // 或者您可以更新为"等待确认"状态，比如 status = 2
                            vo.status = 2; // 等待用户确认
                            vo.payment_no = transferResult.OutBillNo; // 记录商户单号
                            UpdateCJWinningRecords(vo);

                            _log.Info($"需要用户确认收款，记录ID: {vo.lottery_id}, 商户单号: {transferResult.OutBillNo}");
                            break;

                        case "PROCESSING":

                            // 处理中，暂时不更新数据库状态
                            _log.Info($"转账处理中，记录ID: {vo.lottery_id}");
                            break;

                        default:
                            _log.Warn($"未知的转账状态: {transferResult.State}，记录ID: {vo.lottery_id}");
                            break;
                    }

                    // 记录微信转账单号到日志
                    if (!string.IsNullOrEmpty(transferResult.TransferBillNo))
                    {
                        _log.Info($"微信转账单号: {transferResult.TransferBillNo}");
                    }
                }
                else
                {
                    // 记录详细的错误信息
                    _log.Error($"微信转账失败: Code={transferResult.Code}, Message={transferResult.Message}, ErrorDetail={transferResult.ErrorDetail}");
                }

                return transferResult;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                string strErrorMsg = $"开奖领取异常 - Message:{ex.Message}, Stack:{ex.StackTrace}, Source:{ex.Source}";
                _log.Error(strErrorMsg);

                return WeChatTransferResult.FailResult($"系统异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string getOpenIdByGeneral(string code, int AppType)
        {
            string jsonStr = "";
            try
            {
                AppVO AppVO = AppBO.GetApp(AppType);
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + AppVO.AppId + "&secret=" + AppVO.Secret + "&js_code=" + code + "&grant_type=authorization_code";
                jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var result = new WeiXinAccessTokenResult();
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModel>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }
                return result.SuccessResult.openid;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source + "\r\n jsonStr=" + jsonStr + " \r\n code=" + code;
                _log.Error(strErrorMsg);
                return "error";
            }
        }
        #endregion

        #region 签到
        /// <summary>
        /// 添加签到表
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddQuestionnaire(CardRegistertableVO vo)
        {
            try
            {
                ICardRegistertableDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int AccessRecordsID = rDAO.Insert(vo);
                    return AccessRecordsID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新签到表
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardRegistertable(CardRegistertableVO vo)
        {
            ICardRegistertableDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 删除签到表
        /// </summary>
        /// <param name="QuestionnaireID"></param>
        /// <returns></returns>
        public int DeleteByQuestionnaireID(int QuestionnaireID)
        {
            ICardRegistertableDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("QuestionnaireID = " + QuestionnaireID + " and AppType=" + AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取签到表详情
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public CardRegistertableVO FindCardRegistertableByQuestionnaireID(int QuestionnaireID)
        {
            ICardRegistertableDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(QuestionnaireID);
        }

        /// <summary>
        /// 获取签到表列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<CardRegistertableVO> FindCardRegistertableByCondtion(string condtion)
        {
            int AppTypes = 30;//粤省情
            ICardRegistertableDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion + " and AppType=" + AppTypes);
        }

        /// <summary>
        /// 获取签到表列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardRegistertableVO> FindCardRegistertableAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardRegistertableDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取签到表数量
        /// </summary>
        /// <returns></returns>
        public int FindCardRegistertable(string condition)
        {
            ICardRegistertableDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition + " and AppType=" + AppType);
        }

        /// <summary>
        /// 获取签到表列表(视图)
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardRegistertableViewVO> FindCardRegistertableViewAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardRegistertableViewDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取签到表数量(视图)
        /// </summary>
        /// <returns></returns>
        public int FindCardRegistertableView(string condition)
        {
            ICardRegistertableViewDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableViewDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition + " and AppType=" + AppType);
        }

        /// <summary>
        /// 添加签到
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddQuestionnaireSignup(CardRegistertableSignupVO vo)
        {
            try
            {
                ICardRegistertableSignupDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableSignupDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int QuestionnaireSignupID = rDAO.Insert(vo);
                    return QuestionnaireSignupID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新签到
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateCardRegistertableSignup(CardRegistertableSignupVO vo)
        {
            ICardRegistertableSignupDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableSignupDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 删除签到
        /// </summary>
        /// <param name="QuestionnaireSignupID"></param>
        /// <returns></returns>
        public int DeleteByQuestionnaireSignupID(int QuestionnaireSignupID)
        {
            ICardRegistertableSignupDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableSignupDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("QuestionnaireSignupID = " + QuestionnaireSignupID + " and AppType=" + AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取签到数量
        /// </summary>
        /// <returns></returns>
        public int FindCardRegistertableSignup(string condition)
        {
            ICardRegistertableSignupDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableSignupDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition + " and AppType=" + AppType);
        }

        /// <summary>
        /// 获取签到详情
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public CardRegistertableSignupVO FindCardRegistertableSignupByQuestionnaireSignupID(int QuestionnaireSignupID)
        {
            ICardRegistertableSignupDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableSignupDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(QuestionnaireSignupID);
        }

        /// <summary>
        /// 获取签到列表
        /// </summary>
        /// <param name="condtion"></param>
        /// <returns></returns>
        public List<CardRegistertableSignupVO> FindCardRegistertableSignupByCondtion(string condtion)
        {
            ICardRegistertableSignupDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableSignupDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condtion + " and AppType=" + AppType);
        }

        /// <summary>
        /// 获取签到列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardRegistertableSignupVO> FindCardRegistertableSignupAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ICardRegistertableSignupDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableSignupDAO(this.CurrentCustomerProfile);
            return rDAO.FindAllByPageIndex(conditionStr + " and AppType=" + AppType, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 添加签到表管理员
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddQuestionnaireAdmin(CardRegistertableAdminVO vo)
        {
            try
            {
                ICardRegistertableAdminDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableAdminDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    vo.AppType = AppType;
                    int QuestionnaireAdminID = rDAO.Insert(vo);
                    return QuestionnaireAdminID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新签到表管理员
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateQuestionnaireAdmin(CardRegistertableAdminVO vo)
        {
            ICardRegistertableAdminDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableAdminDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 获取签到表管理员列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CardRegistertableAdminVO> FindQuestionnaireAdminByCondition(string condition, params object[] parameters)
        {
            ICardRegistertableAdminDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableAdminDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condition + " and AppType=" + AppType);
        }

        /// <summary>
        /// 获取签到表管理员数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindQuestionnaireAdminTotalCount(string condition, params object[] parameters)
        {
            ICardRegistertableAdminDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableAdminDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition + " and AppType=" + AppType, parameters);
        }
        /// <summary>
        /// 获取签到表管理员详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public CardRegistertableAdminVO FindQuestionnaireAdminById(int QuestionnaireAdminID)
        {
            ICardRegistertableAdminDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableAdminDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(QuestionnaireAdminID);
        }

        /// <summary>
        /// 是否是签到表管理员
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool isQuestionnaireAdmin(int QuestionnaireID, Int64 CustomerId)
        {
            ICardRegistertableAdminDAO rDAO = BusinessCardManagementDAOFactory.CardRegistertableAdminDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount("QuestionnaireID=" + QuestionnaireID + " and CustomerId=" + CustomerId + " and AppType=" + AppType) > 0;
        }


        /// <summary>
        /// 获取签到表二维码
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public string GetQuestionnaireQR(int QuestionnaireID, int AppType)
        {
            try
            {
                var logger = new LogBO(typeof(BusinessCardBO));
                AppVO AppVO = AppBO.GetApp(AppType);
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppVO.AppId + "&secret=" + AppVO.Secret + "";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var result = new WeiXinAccessTokenResultDYH();
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }
                string DataJson = string.Empty;
                string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;

                string page = "package/setup/SignInFormByUser/SignInFormByUser";
                //string page = "pages/home/home";

                DataJson = "{";
                DataJson += "\"scene\":\"" + QuestionnaireID + "\",";
                DataJson += string.Format("\"width\":{0},", 430);
                DataJson += "\"auto_color\":false,";
                DataJson += string.Format("\"page\":\"{0}\",", page);//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
                DataJson += "\"line_color\":{";
                DataJson += string.Format("\"r\":\"{0}\",", "0");
                DataJson += string.Format("\"g\":\"{0}\",", "0");
                DataJson += string.Format("\"b\":\"{0}\"", "0");
                DataJson += "},";
                DataJson += "\"is_hyaline\":false";
                DataJson += "}";
                Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);

                if (str == null)
                {
                    // 记录空流错误
                    logger.Error("微信接口返回流为空或不可读");

                    return null;
                }
                // 2. 将原始流复制到可查找的MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    str.CopyTo(ms); // 复制所有内容到内存流
                    str.Close();//关闭原始流，释放资源
                    ms.Position = 0; // 内存流支持重置位置，此时可安全操作

                    // 3. 后续操作使用内存流ms，而非原始流
                    byte[] buffer = ms.ToArray(); // 读取全部字节（无需访问Length）

                    // 验证是否为微信错误信息
                    string content = Encoding.UTF8.GetString(buffer);
                    if (content.Contains("errcode"))
                    {
                        logger.Error($"微信接口错误：{content}");
                        return null;
                    }

                    string filePath = "";
                    // 4. 用内存流创建Bitmap（此时ms.Position=0，可正常读取）
                    using (Bitmap m_Bitmap = new Bitmap(ms))
                    {

                        string folder = "/UploadFolder/QuestionnaireFile/";
                        string newFileName = QuestionnaireID + ".png";
                        filePath = folder + newFileName;

                        string localPath = ConfigInfo.Instance.UploadFolder + folder;
                        if (!Directory.Exists(localPath))
                        {
                            Directory.CreateDirectory(localPath);
                        }
                        string physicalPath = localPath + newFileName;
                        m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);
                    }


                    string Cardimg = ConfigInfo.Instance.BCAPIURL + filePath;
                    logger.Error("完成" + Cardimg);
                    CardRegistertableVO cVO = new CardRegistertableVO();
                    cVO.QuestionnaireID = Convert.ToInt32(QuestionnaireID);
                    cVO.QRImg = Cardimg;
                    UpdateCardRegistertable(cVO);
                    return Cardimg;
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "";
            }

        }


        /// <summary>
        /// 获取签到二维码 携带用户信息
        /// </summary>
        /// <param name="QuestionnaireID"></param>
        /// <param name="CustomerId"></param>
        /// <returns>返回一个图片的名字xx.png</returns>
        public string GetCardRegistertableSignupQR(Int64 QuestionnaireID, Int64 InviterCID, int AppType)
        {
            try
            {
                var logger = new LogBO(typeof(BusinessCardBO));
                AppVO AppVO = AppBO.GetApp(AppType);
                string url;
                url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppVO.AppId + "&secret=" + AppVO.Secret + "";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var result = new WeiXinAccessTokenResultDYH();
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }
                string DataJson = string.Empty;
                string wxaurl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + result.SuccessResult.access_token;

                string page = "package/setup/SignInFormByUser/SignInFormByUser";
                //string page = "pages/home/home";

                DataJson = "{";
                DataJson += "\"scene\":\"" + QuestionnaireID + "-" + InviterCID + "\",";
                DataJson += string.Format("\"width\":{0},", 430);
                DataJson += "\"auto_color\":false,";
                DataJson += string.Format("\"page\":\"{0}\",", page);//扫码所要跳转的地址，根路径前不要填加'/',不能携带参数（参数请放在scene字段里），如果不填写这个字段，默认跳主页面                
                DataJson += "\"line_color\":{";
                DataJson += string.Format("\"r\":\"{0}\",", "0");
                DataJson += string.Format("\"g\":\"{0}\",", "0");
                DataJson += string.Format("\"b\":\"{0}\"", "0");
                DataJson += "},";
                DataJson += "\"is_hyaline\":false";
                DataJson += "}";

                Stream str = HttpHelper.HtmlFromUrlPostByStream(wxaurl, DataJson);

                if (str == null)
                {
                    // 记录空流错误
                    logger.Error("微信接口返回流为空或不可读");

                    return null;
                }
                // 2. 将原始流复制到可查找的MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    str.CopyTo(ms); // 复制所有内容到内存流
                    str.Close();//关闭原始流，释放资源
                    ms.Position = 0; // 内存流支持重置位置，此时可安全操作

                    // 3. 后续操作使用内存流ms，而非原始流
                    byte[] buffer = ms.ToArray(); // 读取全部字节（无需访问Length）

                    // 验证是否为微信错误信息
                    string content = Encoding.UTF8.GetString(buffer);
                    if (content.Contains("errcode"))
                    {
                        var errorObj = JsonConvert.DeserializeObject<dynamic>(content);
                        int errcode = errorObj.errcode;
                        string errmsg = errorObj.errmsg;
                        logger.Error($"微信接口错误，错误码：{errcode}，错误信息：{errmsg}");
                        //logger.Error($"微信接口错误：{content}");
                        return null;
                    }

                    string filePath = "";
                    // 4. 用内存流创建Bitmap（此时ms.Position=0，可正常读取）
                    using (Bitmap m_Bitmap = new Bitmap(ms))
                    {
                        string folder = "/UploadFolder/QuestionnaireFile/";
                        string newFileName = QuestionnaireID + "_" + InviterCID + ".png";
                        filePath = folder + newFileName;

                        string localPath = ConfigInfo.Instance.UploadFolder + folder;
                        if (!Directory.Exists(localPath))
                        {
                            Directory.CreateDirectory(localPath);
                        }
                        string physicalPath = localPath + newFileName;
                        m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);
                    }


                    string Cardimg = ConfigInfo.Instance.BCAPIURL + filePath;
                    logger.Error("完成" + Cardimg);
                    return Cardimg;
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return "";
            }

        }

        /// <summary>
        /// 生成小程序码 名字头像二维码
        /// </summary>
        /// <param name="ID"></param>
        ///  <param name="IDType">1:名片，2:名片组，3:活动，5:签到表，6:软文，7:活动报名授权码</param>
        /// <returns></returns>
        public string getQRIMGByIDAndType(int ID, int IDType, int AppType, int CustomerId = 0)
        {
            try
            {
                var logger = new LogBO(typeof(BusinessCardBO));
                string imgurl = ConfigInfo.Instance.BCAPIURL + "/GenerateIMG/BusinessCardIMG2QR.aspx?ID=" + ID + "&IDType=" + IDType + "&AppType=" + AppType;
                if (CustomerId != 0)
                {
                    imgurl = ConfigInfo.Instance.BCAPIURL + "/GenerateIMG/BusinessCardIMG2QR.aspx?ID=" + ID + "&IDType=" + IDType + "&CustomerId=" + CustomerId + "&AppType=" + AppType;
                }

                Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(imgurl, 752, 974, 752, 974);

                string filePath = "";
                string folder = "";
                if (IDType == 5) { folder = "/UploadFolder/QuestionnaireFile/"; }
                else { folder = "/UploadFolder/QuestionnaireFile/"; }

                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".png";
                filePath = folder + newFileName;


                //可以修改为网络路径
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string physicalPath = localPath + newFileName;

                m_Bitmap.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);
                string ImgUrl = ConfigInfo.Instance.BCAPIURL + filePath;
                logger.Info("生成二维码" + ImgUrl);
                CardRegistertableVO cVO = new CardRegistertableVO();
                cVO.QuestionnaireID = ID;
                cVO.QRImg = ImgUrl;
                UpdateCardRegistertable(cVO);

                return ImgUrl;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return null;
            }
        }



        /// <summary>
        /// 删除签到表管理员
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public int DeleteQuestionnaireAdminById(int QuestionnaireID)
        {
            ICardNoticedDAO rDAO = BusinessCardManagementDAOFactory.CardNoticedDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("QuestionnaireID = " + QuestionnaireID + " and AppType=" + AppType);
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        #endregion



        #region 商家转账
        /// <summary>
        /// 微信商家转账到零钱(使用证书文件方案)
        /// </summary>
        /// <param name="money">转账金额</param>
        /// <param name="lottery_id">红包记录id</param>
        /// <param name="personal_id">个人记录id</param>
        /// <param name="openid1">用户的openid</param>
        /// <param name="AppType">应用类型</param>
        /// <param name="desc">转账描述</param>
        /// <returns>结构化的转账结果</returns>
        public WeChatTransferResult WechatPayToChange(Decimal money, int lottery_id, int personal_id, string openid1, int AppType, string desc = "")
        {
            LogBO _log = new LogBO(typeof(BusinessCardBO));

            // 参数验证
            if ((double)money < 0.3)
            {
                return WeChatTransferResult.FailResult("转账金额不能小于0.3元");
            }

            if (lottery_id < 0)
            {
                return WeChatTransferResult.FailResult("红包记录ID无效");
            }

            if (string.IsNullOrEmpty(openid1))
            {
                return WeChatTransferResult.FailResult("用户OpenID不能为空");
            }

            try
            {
                AppVO AppVO = AppBO.GetApp(AppType);

                string mchid = AppVO.MCHID;
                string appid = AppVO.AppId;
                string mchCertSerialNo = AppVO.MCH_CERT_SERIAL_NO;
                string certPath = AppVO.SSLCERT_PATH;
                string certPassword = AppVO.SSLCERT_PASSWORD;

                // 验证必要参数
                if (string.IsNullOrEmpty(certPath) || string.IsNullOrEmpty(certPassword))
                {
                    _log.Error("微信支付证书路径或密码未配置");
                    return WeChatTransferResult.FailResult("微信支付证书配置不完整");
                }

                // 生成商户单号
                string out_bill_no = GenerateOutBillNo(mchid, lottery_id, personal_id);
                int amount = Convert.ToInt32(money * 100);

                if (string.IsNullOrEmpty(desc))
                {
                    desc = "恭喜你，成功领取问卷现金红包！";
                }

                // 构造请求数据
                var requestData = new
                {
                    appid = appid,
                    out_bill_no = out_bill_no,
                    transfer_scene_id = "1000",
                    openid = openid1,
                    transfer_amount = amount,
                    transfer_remark = desc,
                    notify_url = "https://gx.gdsqzx.com.cn:8080/BusinessCard/Pay/Lottery_Notify_Url.aspx",
                    transfer_scene_report_infos = new[]{
                     new { info_type = "活动名称", info_content = "新会员有礼" },
                     new { info_type = "奖励说明", info_content = "关注分享，抽奖有礼" }
                     }
                };

                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                string url = "https://api.mch.weixin.qq.com";
                string endpoint = "/v3/fund-app/mch-transfer/transfer-bills";

                // 发送V3 API请求
                string responseJson = WeChatPayV3PostWithCertificate(url, endpoint, jsonData, mchid, mchCertSerialNo, certPath, certPassword);

                _log.Info($"微信商家转账API返回: {responseJson}");

                // 解析返回结果
                return ParseTransferResponse(responseJson, out_bill_no);
            }
            catch (Exception ex)
            {
                string strErrorMsg = $"微信商家转账异常 - Message: {ex.Message}, Stack: {ex.StackTrace}";
                _log.Error(strErrorMsg);
                return WeChatTransferResult.FailResult($"系统异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 生成商户单号
        /// </summary>
        private string GenerateOutBillNo(string mchid, int lottery_id, int personal_id)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string random = new Random().Next(1000, 9999).ToString();
            string outBillNo = $"CJ{timestamp}{lottery_id}{personal_id}{random}";

            // 确保不超过32位
            if (outBillNo.Length > 30)
            {
                outBillNo = outBillNo.Substring(0, 30);
            }

            return outBillNo;
        }

        /// <summary>
        /// 使用证书进行微信支付V3 API POST请求
        /// </summary>
        private string WeChatPayV3PostWithCertificate(string domain, string endpoint, string jsonData, string mchId, string certSerialNo, string certPath, string certPassword)
        {
            try
            {
                string nonce = Guid.NewGuid().ToString("N"); // 随机字符串
                string timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString(); // 时间戳
                string method = "POST";
                string body = jsonData;
                var url = domain.TrimEnd('/') + "/" + endpoint.TrimStart('/');
                // 构造签名原文
                string message = $"{method}\n{endpoint}\n{timestamp}\n{nonce}\n{body}\n";

                // 使用证书生成签名
                string signature = GenerateSignatureWithCertificate(message, certPath, certPassword);

                // 构造Authorization头
                string authorization = $"WECHATPAY2-SHA256-RSA2048 mchid=\"{mchId}\",nonce_str=\"{nonce}\",signature=\"{signature}\",timestamp=\"{timestamp}\",serial_no=\"{certSerialNo}\"";

                // 设置TLS 1.2
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                // 创建HTTP请求
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers.Add("Authorization", authorization);
                    client.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    client.Headers.Add("Accept", "application/json");
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Wechatpay-Serial", certSerialNo);
                    // 上传数据并获取响应
                    byte[] requestData = Encoding.UTF8.GetBytes(jsonData);
                    byte[] responseData = client.UploadData(url, "POST", requestData);
                    string responseContent = Encoding.UTF8.GetString(responseData);

                    LogBO _log = new LogBO(typeof(BusinessCardBO));
                    _log.Info($"V3 API证书请求成功，URL: {url}");

                    return responseContent;
                }
            }
            catch (WebException webEx)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));

                if (webEx.Response != null)
                {
                    using (var stream = webEx.Response.GetResponseStream())
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        string errorResponse = reader.ReadToEnd();
                        _log.Error($"V3 API证书请求Web异常 - Status: {((HttpWebResponse)webEx.Response).StatusCode}, Response: {errorResponse}");
                    }
                }
                else
                {
                    _log.Error($"V3 API证书请求Web异常 - Message: {webEx.Message}");
                }

                return null;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BusinessCardBO));
                _log.Error($"V3 API证书请求异常: {ex.Message}");
                return null;
            }
        }

        ///// <summary>
        ///// 使用证书生成签名
        ///// </summary>
        private string GenerateSignatureWithCertificate(string message, string certPath, string certPassword)
        {
            try
            {
                // 1. 将证书文件作为字节数组读取
                byte[] certData = File.ReadAllBytes(certPath);

                // 2. 从字节数组和密码加载证书
                X509Certificate2 cert = new X509Certificate2(certData, certPassword,
                    X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);

                // 3. 获取支持新算法的RSA实例
                using (RSA rsa = cert.GetRSAPrivateKey())
                {
                    if (rsa == null)
                    {
                        throw new Exception("无法从证书中获取支持算法的RSA私钥。");
                    }

                    byte[] data = Encoding.UTF8.GetBytes(message);
                    // 4. 使用支持SHA256的RSA实例进行签名
                    byte[] signature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    return Convert.ToBase64String(signature);
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                _log.Error($"使用新方法生成证书签名失败: {ex.Message}");
                throw new Exception($"使用新方法生成证书签名失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 解析转账响应
        /// </summary>
        private WeChatTransferResult ParseTransferResponse(string responseJson, string outBillNo)
        {
            if (string.IsNullOrEmpty(responseJson))
            {
                return WeChatTransferResult.FailResult("微信支付返回空响应");
            }

            try
            {
                dynamic resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject(responseJson);

                // 检查是否存在错误码
                if (resultObj.code != null)
                {
                    string errorCode = resultObj.code.ToString();
                    string errorMessage = resultObj.message?.ToString();
                    object errorDetail = resultObj.detail;

                    return WeChatTransferResult.FailResult(errorMessage, errorCode, errorDetail);
                }

                // 解析成功响应
                string state = resultObj.state?.ToString();
                string transferBillNo = resultObj.transfer_bill_no?.ToString();
                string packageInfo = resultObj.package_info?.ToString();
                string createTime = resultObj.create_time?.ToString();

                // 根据状态返回不同结果
                switch (state)
                {
                    case "WAIT_USER_CONFIRM":
                        return WeChatTransferResult.SuccessResult(
                            outBillNo,
                            transferBillNo,
                            state,
                            packageInfo
                        ).WithMessage("转账已提交，等待用户确认收款");

                    case "SUCCESS":
                        return WeChatTransferResult.SuccessResult(
                            outBillNo,
                            transferBillNo,
                            state
                        ).WithMessage("转账成功，款项已到账");

                    case "PROCESSING":
                        return WeChatTransferResult.SuccessResult(
                            outBillNo,
                            transferBillNo,
                            state
                        ).WithMessage("转账处理中");

                    case "FAILED":
                        string failReason = resultObj.fail_reason?.ToString();
                        return WeChatTransferResult.FailResult($"转账失败: {failReason}", "TRANSFER_FAILED");

                    default:
                        return WeChatTransferResult.FailResult($"未知状态: {state}", "UNKNOWN_STATE");
                }
            }
            catch (Exception jsonEx)
            {
                return WeChatTransferResult.FailResult($"解析微信返回JSON失败: {jsonEx.Message}");
            }
        }

        #endregion

        #region 排行榜
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddRank(RankVO vo)
        {
            try
            {
                IRankDAO rDAO = BusinessCardManagementDAOFactory.RankDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int AccessRecordsID = rDAO.Insert(vo);
                    return AccessRecordsID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateRank(RankVO vo)
        {
            IRankDAO rDAO = BusinessCardManagementDAOFactory.RankDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取榜单分页列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<RankVO> FindRankAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IRankDAO rDAO = BusinessCardManagementDAOFactory.RankDAO(this.CurrentCustomerProfile);
            List<RankVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);

            return cVO;
        }

        /// <summary>
        /// 获取榜单的数量
        /// </summary>
        /// <returns></returns>
        public int FindRankCount(string condition, params object[] parameters)
        {
            IRankDAO rDAO = BusinessCardManagementDAOFactory.RankDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 获取榜单详情
        /// </summary>
        /// <param name="rank_lists_id"></param>
        /// <returns></returns>
        public RankVO FindRankById(int rank_lists_id)
        {
            IRankDAO rDAO = BusinessCardManagementDAOFactory.RankDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(rank_lists_id);
        }



        /// <summary>
        /// 添加榜单项
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddRankItem(RankItemVO vo)
        {
            try
            {
                IRankItemDAO rDAO = BusinessCardManagementDAOFactory.RankItemDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int AccessRecordsID = rDAO.Insert(vo);
                    return AccessRecordsID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新榜单项
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateRankItem(RankItemVO vo)
        {
            IRankItemDAO rDAO = BusinessCardManagementDAOFactory.RankItemDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取榜单分页列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<RankItemVO> FindRankItemAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IRankItemDAO rDAO = BusinessCardManagementDAOFactory.RankItemDAO(this.CurrentCustomerProfile);
            List<RankItemVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);

            return cVO;
        }

        /// <summary>
        /// 获取榜单的数量
        /// </summary>
        /// <returns></returns>
        public int FindRankItemCount(string condition, params object[] parameters)
        {
            IRankItemDAO rDAO = BusinessCardManagementDAOFactory.RankItemDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 获取企业列表
        /// </summary>
        /// <param name="HeadquartersID"></param>
        /// <returns></returns>
        public List<BusinessCardVO> FindBusinessCardList(int status, string businessName)
        {
            IBusinessCardDAO rDAO = BusinessCardManagementDAOFactory.BusinessCardDAO(this.CurrentCustomerProfile);
            if (businessName != null)
            {
                List<BusinessCardVO> cVO = rDAO.FindByParams("Status = " + status + " And BusinessName LIKE '%" + businessName + "%' " + " order by createdAt desc");
                return cVO;
            }
            return rDAO.FindByParams("Status = " + status + " order by createdAt desc");
        }

        /// <summary>
        /// 删除贺卡
        /// </summary>
        /// <param name="GreetingCardID"></param>
        /// <returns></returns>
        public int DeleteRankItemById(int rank_items_id)
        {
            IRankItemDAO rDAO = BusinessCardManagementDAOFactory.RankItemDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.DeleteByParams("rank_items_id = " + rank_items_id);
                return 1;
            }
            catch
            {
                return -1;
            }
        }
        #endregion

    }
}
