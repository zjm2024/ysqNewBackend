using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using SPLibrary.CoreFramework.BO;
using CoreFramework.VO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.CoreFramework;
using Newtonsoft.Json;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using WebUI.Common;
using SPlatformService.Controllers;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;
using SPLibrary.CoreFramework.Logging.BO;

namespace WebUI.Shared
{
    public partial class MasterPageSite : System.Web.UI.MasterPage
    {
        private static DateTime saveTime { get; set; }
        public string SiteName
        {
            get
            {
                return SiteTitle.Text;
            }
            set {
                SiteTitle.Text = value;
            }
        }
        public string SiteShortName
        {
            get
            {
                return CacheSystemConfig.GetSystemConfig().SiteName;
            }
        }
        public string ServicePhone
        {
            get
            {
                return CacheSystemConfig.GetSystemConfig().ServicePhone;
            }
        }

        public string ServiceNote
        {
            get
            {
                return CacheSystemConfig.GetSystemConfig().ServiceNote;
            }
        }
        public string APPImage
        {
            get
            {
                if (string.IsNullOrEmpty(CacheSystemConfig.GetSystemConfig().APPImage))
                    return ResolveUrl("~/Style/images/zhongxiaoleapp.jpg");
                else
                    return CacheSystemConfig.GetSystemConfig().APPImage;
            }
        }
        public string GZImage
        {
            get
            {
                if (string.IsNullOrEmpty(CacheSystemConfig.GetSystemConfig().GZImage))
                    return ResolveUrl("~/Style/images/zhongxiaolegzh.jpg");
                else
                    return CacheSystemConfig.GetSystemConfig().GZImage;
            }
        }
        public CustomerProfile CustomerProfile
        {
            get { return new CustomerPrincipal().CustomerProfile; }
        }

        public string Token
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["#Session#TOKEN"] == null)
                {
                    return null;

                }
                else
                {
                    return HttpContext.Current.Session["#Session#TOKEN"].ToString();
                }
            }
        }

        public string APIURL
        {
            get
            {
                return ConfigInfo.Instance.APIURL;
            }
        }

        public string SITEURL
        {
            get
            {
                return ConfigInfo.Instance.SITEURL;
            }
        }


        public int CustomerId
        {
            get
            {
                if (CustomerProfile != null)
                    return CustomerProfile.CustomerId;
                else
                    return 0;
            }
        }
        public string IMUserID { get; set; }
        public string IMUserPD { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SiteTitle.Text == "") {
                if (string.IsNullOrEmpty(CacheSystemConfig.GetSystemConfig().SiteDescription))
                    SiteTitle.Text = CacheSystemConfig.GetSystemConfig().SiteName;
                else
                    SiteTitle.Text = CacheSystemConfig.GetSystemConfig().SiteName + "-" + CacheSystemConfig.GetSystemConfig().SiteDescription;
            }
            if (CustomerId > 0)
            {
                divLogin.Visible = false;
                divUser.Visible = true;
                index_zxt.Visible = true;
                lnkUser.InnerText = string.IsNullOrEmpty(CustomerProfile.CustomerName) ? CustomerProfile.CustomerAccount : CustomerProfile.CustomerName;

                SPIMController spIMCon = new SPIMController();
                ResultObject result = spIMCon.GetIMuser(CustomerId, Token);
                if (result.Flag == 1)
                {
                    CustomerIMVO imVO = result.Result as CustomerIMVO;

                    IMUserID = imVO.IMId;
                    IMUserPD = imVO.IMPWD;
                }
                else
                {
                    IMUserID = "";
                    IMUserPD = "";
                }

                ZXTBO uBO = new ZXTBO(new CustomerProfile());
                string conditionStr = "MessageTo = " + CustomerId+ " and Status=0";
                int MessageCount = uBO.FindZXTMessageCount(conditionStr);
                if (MessageCount > 0) {
                    string str1 = "";
                    if (MessageCount < 100)
                    {
                        str1 = MessageCount.ToString();
                    }
                    else
                    {
                        str1 = "99+";
                    }
                    index_zxt_text.InnerHtml = "未读聊天消息<font>" + str1 + "</font>条";
                }
                else{
                    index_zxt_text.InnerHtml = "没有未读聊天消息";
                }
            }
            else
            {
                divLogin.Visible = true;
                divUser.Visible = false;
                index_zxt.Visible = false;
                lnkUser.InnerText = "";
            }

            BindCity();

            //bind message count 
            if (!string.IsNullOrEmpty(Token))
            {
                CustomerController customerCon = new CustomerController();
                int count = Convert.ToInt32(customerCon.GetUnRedMessageCount(Token).Result);
                if (count > 0)
                    liaMessage.InnerHtml = "我的消息(<span style=\"color: red; \">" + count + "</span>)";
            }

            string[] strArr = ServicePhone.Split(';');
            string strPhone = "";
            foreach (string str in strArr)
            {
                if (strPhone != "")
                    strPhone += "<br />";
                strPhone += "<span>" + str + "</span>";

            }
            //divServicePhone.InnerHtml = strPhone;

            TimeSpan ts = DateTime.Now - saveTime;
            //if ((ts.TotalMinutes >= 10 || saveTime == null) && CustomerId <= 0)
            //saveHTMLfile();
        }
        private void saveHTMLfile()
        {
            saveTime = DateTime.Now;
            //生成index.html文件
            try
            {
                string url = ConfigInfo.Instance.SITEURL + "index.aspx";
                string Path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "index.html";
                string Str = HttpHelper.HtmlFromUrlGet(url);
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(url);
                System.Net.WebResponse wResp = wReq.GetResponse();

                System.IO.Stream respStream = wResp.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(respStream, System.Text.Encoding.GetEncoding("utf-8"));
                string str = reader.ReadToEnd();
                System.IO.StreamWriter sw = new System.IO.StreamWriter(Path, false, System.Text.Encoding.GetEncoding("utf-8"));
                sw.Write(str);
                sw.Flush();
                sw.Close();
            }
            catch(Exception ex)
            {
                LogBO _log = new LogBO(typeof(CardBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
            }
        }

        protected void UserLogout(object sender, EventArgs e)
        {
            HttpContext.Current.Session["#Session#TOKEN"] = null;
            IUserPrincipal iup = new UserPrincipal();
            FormsAuthentication.SignOut();
            UserPrincipal.ClearProfileSession();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }

        private void BindCity()
        {
            List<CityVO> cityList = SiteCommon.GetCityListAll();

            string cityStr = "<ul class=\"local-city-panel-bd clearfix\" id=\"divCitys\">";
            for (var i = 0; i < cityList.Count; i++)
            {
                cityStr += " <li class=\"city-bd\"> \r\n";
                cityStr += "      <a href=\"javascript:changeCity('" + cityList[i].CityId + "','" + cityList[i].CityName + "')\" title=\"" + cityList[i].CityName + "\" class=\"j-localize-city\" >" + cityList[i].CityName + "</a> \r\n";
                cityStr += "  </li> \r\n";
                if (i == 8)
                    break;
            }
            cityStr += "</ul>";
            divCityList.InnerHtml = cityStr;
        }
    }
}