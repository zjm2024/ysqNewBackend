using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Card
{
    public partial class _360fun : System.Web.UI.Page
    {
        public string imgurl;
        public string time_anim;
        public string navbar;
        protected void Page_Load(object sender, EventArgs e)
        {
            imgurl = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ImgUrl"]) ? "https://api.leliaomp.com/SPManager/style/images/360/1658296748000.jpg" : HttpContext.Current.Request.QueryString["ImgUrl"];
            time_anim= string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["time_anim"]) ? "2000" : HttpContext.Current.Request.QueryString["time_anim"];
            navbar = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["navbar"]) ? "true" : HttpContext.Current.Request.QueryString["navbar"];
        }
    }
}