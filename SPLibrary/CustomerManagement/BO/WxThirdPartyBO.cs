using CoreFramework.DAO;
using CoreFramework.VO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Drawing.Imaging;
using System.IO;

namespace SPLibrary.CustomerManagement.BO
{
    /// <summary>
    /// 微信第三方平台接口
    /// </summary>
    public class WxThirdPartyBO
    {
        public static string appid = "wxdaa115a1853a4f00";
        public static string appsecret = "8320a3ce081eed819b546ea6e2e18659";
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();
        public WxThirdPartyBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }
        /// <summary>
        /// 添加Ticket
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddTicket(TicketVO vo)
        {
            try
            {
                ITicketDAO rDAO = CustomerManagementDAOFactory.TicketDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int TicketID = rDAO.Insert(vo);
                    return TicketID;
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
        /// 更新Ticket
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateTicket(TicketVO vo)
        {
            ITicketDAO rDAO = CustomerManagementDAOFactory.TicketDAO(this.CurrentCustomerProfile);
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
        /// 获取Ticket列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<TicketVO> FindTicketByCondition(string condition, params object[] parameters)
        {
            ITicketDAO rDAO = CustomerManagementDAOFactory.TicketDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condition);
        }

        /// <summary>
        /// 添加授权信息
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddAuthorization(AuthorizationVO vo)
        {
            try
            {
                IAuthorizationDAO rDAO = CustomerManagementDAOFactory.AuthorizationDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int AuthorizationID = rDAO.Insert(vo);
                    return AuthorizationID;
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
        /// 更新授权信息
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateAuthorization(AuthorizationVO vo)
        {
            IAuthorizationDAO rDAO = CustomerManagementDAOFactory.AuthorizationDAO(this.CurrentCustomerProfile);
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
        /// 获取授权信息列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<AuthorizationVO> FindAuthorizationByCondition(string condition, params object[] parameters)
        {
            IAuthorizationDAO rDAO = CustomerManagementDAOFactory.AuthorizationDAO(this.CurrentCustomerProfile);
            return rDAO.FindByParams(condition);
        }
        /// <summary>
        /// 获取授权信息详情
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public AuthorizationVO FindAuthorizationByID(int AuthorizationID)
        {
            IAuthorizationDAO rDAO = CustomerManagementDAOFactory.AuthorizationDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(AuthorizationID);
        }

        /// <summary>
        /// 获取component_access_token
        /// </summary>
        /// <returns></returns>
        public string GetComponent_Access_Token()
        {
            List<TicketVO> tVO = FindTicketByCondition("AppId = '" + appid + "'");

            if (tVO.Count > 0)
            {
                string wxaurl = "https://api.weixin.qq.com/cgi-bin/component/api_component_token";
                string DataJson = "";
                DataJson = "{";
                DataJson += "\"component_appid\": \"" + appid + "\",";
                DataJson += "\"component_appsecret\": \"" + appsecret + "\",";
                DataJson += "\"component_verify_ticket\": \"" + tVO[0].ComponentVerifyTicket + "\"";
                DataJson += "}";

                string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);

                if(str.IndexOf("component_access_token") >-1)
                {
                    dynamic resultContent = JsonConvert.DeserializeObject(str, new { component_access_token = "" }.GetType());
                    return resultContent.component_access_token;
                }
                return "";
            }
            return "";
        }

        /// <summary>
        /// 获取预授权码
        /// </summary>
        /// <returns></returns>
        public string GetPre_Auth_Code()
        {
            string wxaurl = "https://api.weixin.qq.com/cgi-bin/component/api_create_preauthcode?component_access_token="+ GetComponent_Access_Token();
            string DataJson = "";
            DataJson = "{";
            DataJson += "\"component_appid\": \"" + appid + "\"";
            DataJson += "}";

            string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);

            if (str.IndexOf("pre_auth_code") > -1)
            {
                dynamic resultContent = JsonConvert.DeserializeObject(str, new { pre_auth_code = "" }.GetType());
                return resultContent.pre_auth_code;
            }
            return "";
        }

        /// <summary>
        /// 获取授权链接
        /// </summary>
        /// <returns></returns>
        public string GetAuthorizationUrl(int CustomerId)
        {
            string redirectUrl = "https://www.zhongxiaole.net/card/WX_Redirect_Url.aspx?CustomerId="+ CustomerId;
            string wxaurl = "https://mp.weixin.qq.com/safe/bindcomponent?action=bindcomponent&no_scan=1&component_appid="+ appid + "&pre_auth_code="+ GetPre_Auth_Code() + "&redirect_uri="+ HttpUtility.UrlEncode(redirectUrl) + "&auth_type=1#wechat_redirect";
            return wxaurl;
        }

        /// <summary>
        /// 使用授权码获取授权信息
        /// </summary>
        /// <returns></returns>
        public AuthorizationVO GetApi_Query_Auth(string auth_code_value, int CustomerId)
        {
            string wxaurl = "https://api.weixin.qq.com/cgi-bin/component/api_query_auth?component_access_token=" + GetComponent_Access_Token();
            string DataJson = "";
            DataJson = "{";
            DataJson += "\"component_appid\": \"" + appid + "\",";
            DataJson += "\"authorization_code\": \"" + auth_code_value + "\"";
            DataJson += "}";

            string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);

            if (str.IndexOf("authorization_info") > -1)
            {
                dynamic resultContent = JsonConvert.DeserializeObject(str, new { authorization_info =new { authorizer_appid="", authorizer_access_token ="", expires_in="", authorizer_refresh_token =""} }.GetType());

                List<AuthorizationVO> aVO = FindAuthorizationByCondition("authorizer_appid='"+ resultContent.authorization_info.authorizer_appid + "' and AppId='"+ appid+"'");
                if (aVO.Count > 0)
                {
                    aVO[0].authorizer_appid = resultContent.authorization_info.authorizer_appid;
                    aVO[0].authorizer_access_token = resultContent.authorization_info.authorizer_access_token;
                    aVO[0].expires_in = resultContent.authorization_info.expires_in;
                    aVO[0].authorizer_refresh_token = resultContent.authorization_info.authorizer_refresh_token;
                    aVO[0].ExpiresAt = DateTime.Now.AddHours(2);
                    aVO[0].CustomerId = CustomerId;
                    aVO[0].Status = 1;
                    UpdateAuthorization(aVO[0]);
                    return aVO[0];
                }
                else
                {
                    AuthorizationVO AuthorizationVO = new AuthorizationVO();
                    AuthorizationVO.CustomerId = CustomerId;
                    AuthorizationVO.AppId = appid;
                    AuthorizationVO.authorizer_appid = resultContent.authorization_info.authorizer_appid;
                    AuthorizationVO.authorizer_access_token = resultContent.authorization_info.authorizer_access_token;
                    AuthorizationVO.expires_in = resultContent.authorization_info.expires_in;
                    AuthorizationVO.authorizer_refresh_token = resultContent.authorization_info.authorizer_refresh_token;
                    AuthorizationVO.CreatedAt = DateTime.Now;
                    AuthorizationVO.ExpiresAt = DateTime.Now.AddHours(2);
                    AuthorizationVO.Status = 1;
                    AuthorizationVO.AuthorizationID = AddAuthorization(AuthorizationVO);
                    return AuthorizationVO;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取/刷新接口调用令牌
        /// </summary>
        /// <returns></returns>
        public string GetApi_Authorizer_Token(int AuthorizationID)
        {
            AuthorizationVO aVO = FindAuthorizationByID(AuthorizationID);
            if (aVO == null) return "";

            string wxaurl = "https://api.weixin.qq.com/cgi-bin/component/api_authorizer_token?component_access_token=" + GetComponent_Access_Token();
            string DataJson = "";
            DataJson = "{";
            DataJson += "\"component_appid\": \"" + aVO.AppId + "\",";
            DataJson += "\"authorizer_appid\": \"" + aVO.authorizer_appid + "\",";
            DataJson += "\"authorizer_refresh_token\": \"" + aVO.authorizer_refresh_token + "\"";
            DataJson += "}";

            string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);

            if (str.IndexOf("authorizer_access_token") > -1)
            {
                dynamic resultContent = JsonConvert.DeserializeObject(str, new { authorizer_access_token = "", expires_in = "", authorizer_refresh_token = "" }.GetType());
                aVO.authorizer_access_token = resultContent.authorizer_access_token;
                aVO.expires_in = resultContent.expires_in;
                aVO.ExpiresAt = DateTime.Now.AddHours(2);
                UpdateAuthorization(aVO);
                return aVO.authorizer_access_token;
            }
            return "";
        }

        /// <summary>
        /// 获取授权方的帐号基本信息
        /// </summary>
        /// <returns></returns>
        public AuthorizationVO GetApi_Get_Authorizer_Info(int AuthorizationID)
        {
            AuthorizationVO aVO = FindAuthorizationByID(AuthorizationID);
            if (aVO == null) return null;

            string wxaurl = "https://api.weixin.qq.com/cgi-bin/component/api_get_authorizer_info?component_access_token=" + GetComponent_Access_Token();
            string DataJson = "";
            DataJson = "{";
            DataJson += "\"component_appid\": \"" + aVO.AppId + "\",";
            DataJson += "\"authorizer_appid\": \"" + aVO.authorizer_appid + "\"";
            DataJson += "}";

            string str = HttpHelper.HtmlFromUrlPost(wxaurl, DataJson);

            if (str.IndexOf("authorizer_info") > -1)
            {
                dynamic resultContent = JsonConvert.DeserializeObject(str, new { authorizer_info = new { nick_name = "", head_img = "", service_type_info = new {id=0}, verify_type_info = new { id = 0 }, user_name = "", principal_name = "", business_info=new { open_pay=0 }, alias = "", qrcode_url = "" } }.GetType());

                aVO.nick_name = resultContent.authorizer_info.nick_name;
                aVO.head_img = GetImgUrl(resultContent.authorizer_info.head_img);
                aVO.service_type_info = resultContent.authorizer_info.service_type_info.id;
                aVO.verify_type_info = resultContent.authorizer_info.verify_type_info.id;
                aVO.user_name = resultContent.authorizer_info.user_name;
                aVO.principal_name = resultContent.authorizer_info.principal_name;
                aVO.open_pay = resultContent.authorizer_info.business_info.open_pay;
                aVO.alias = resultContent.authorizer_info.alias;
                aVO.qrcode_url = GetImgUrl(resultContent.authorizer_info.qrcode_url);
                
                UpdateAuthorization(aVO);
                return aVO;
            }
            return null;
        }

        /// <summary>
        /// 下载外部图片
        /// </summary>
        /// <returns></returns>
        public string GetImgUrl(string Url)
        {
            //下载头像
            string folder = "/UploadFolder/Image/" + DateTime.Now.ToString("yyyyMM") + "/";
            string imgPath = "";
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
            //可以修改为网络路径
            string localPath = ConfigInfo.Instance.UploadFolder + folder;
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string PhysicalPath = localPath + newFileName;
            imgPath = ConfigInfo.Instance.APIURL + folder + newFileName;
            WebRequest wreq = WebRequest.Create(Url);
            HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
            Stream s = wresp.GetResponseStream();
            System.Drawing.Image img;
            img = System.Drawing.Image.FromStream(s);
            img.Save(PhysicalPath, ImageFormat.Jpeg);   //保存

            return imgPath;
        }

        /// <summary>
        /// 获取代理公众号AccessToken
        /// </summary>
        /// <param name="imgurl"></param>
        /// <returns></returns>
        public string RequestAccessToken(string authorizer_appid)
        {
            List<AuthorizationVO> aVO = FindAuthorizationByCondition("authorizer_appid='" + authorizer_appid + "' and AppId='" + appid + "'");
            if (aVO.Count > 0)
            {
                if (aVO[0].ExpiresAt < DateTime.Now)
                {
                    return GetApi_Authorizer_Token(aVO[0].AuthorizationID);
                }
                else
                {
                    return aVO[0].authorizer_access_token;
                }
            }
            return "";
        }

        /// <summary>
        /// 上传图片获取mediaId
        /// </summary>
        /// <param name="imgurl"></param>
        /// <returns></returns>
        public string uploadimgGetmediaId(string imgurl, string authorizer_appid)
        {
            string access_token = RequestAccessToken(authorizer_appid);
            string wxaurl = "https://api.weixin.qq.com/cgi-bin/media/upload?access_token=" + access_token + "&type=image";

            try
            {
                imgurl = imgurl.Replace("https://www.zhongxiaole.net/SPManager", ConfigInfo.Instance.UploadFolder);

                string str = HttpHelper.UploadFileEx(imgurl, wxaurl, new System.Collections.Specialized.NameValueCollection());
                dynamic resultContent = JsonConvert.DeserializeObject(str, new { media_id = "" }.GetType());
                if (resultContent != null && !string.IsNullOrWhiteSpace(resultContent.media_id))
                {
                    return resultContent.media_id;//返回media_id
                }
                else
                {
                    LogBO _log = new LogBO(typeof(WxThirdPartyBO));
                    string strErrorMsg = "Message:" + str + "\r\n " + resultContent;
                    _log.Error(strErrorMsg);
                    //请求失败，返回为空
                    return "";
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(WxThirdPartyBO));
                string strErrorMsg = "Message:" + ex.Message + "\r\n ";
                _log.Error(strErrorMsg);
                //请求失败，返回为空
                return "";
            }
        }
    }
}
