using System;
using System.Web;
using SPLibrary.CoreFramework.BO;
using CoreFramework.VO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.CoreFramework;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.WebSockets;

namespace WebUI
{
    public partial class ZXTIM : CustomerBasePage
    {
        public string APIURL
        {
            get
            {
                return ConfigInfo.Instance.APIURL;
            }
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
        public int CustomerId
        {
            get
            {
                return CustomerProfile.CustomerId;
            }
        }
        public string CustomerName
        {
            get
            {
                return CustomerProfile.CustomerName;
            }
        }
        public string HeaderLogo;
        public CustomerProfile CustomerProfile
        {
            get { return new CustomerPrincipal().CustomerProfile; }
        }
        public int MessageTo;
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = string.IsNullOrEmpty(Request.QueryString["MessageTo"]) ? "0" : Request.QueryString["MessageTo"];
            int i;
            try
            {
                i = Convert.ToInt32(id);
            }
            catch
            {
                i = 0; //如果转换失败，处理异常
            }
            MessageTo = i;

            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CustomerViewVO uVO = uBO.FindById(CustomerId);

            HeaderLogo = uVO.HeaderLogo;
        }
    }
}