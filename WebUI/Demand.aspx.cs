using CoreFramework.VO;
using SPlatformService.Controllers;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.RequireManagement.BO;
using System;
using System.Web;

namespace WebUI
{
    public partial class Demand : System.Web.UI.Page
    {
        public  DemandViewVO DemandVO;
        public string headImg = ConfigInfo.Instance.NoImg;
        public string Token
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["#Session#TOKEN"] == null)
                {
                    return "";

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
                return new CustomerPrincipal().CustomerProfile.CustomerId;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = string.IsNullOrEmpty(Request.QueryString["DemandId"]) ? "0" : Request.QueryString["DemandId"];
            if (id != "0")
            {
                DemandBO uBO = new DemandBO(new CustomerProfile());

                RequireController requireCon = new RequireController();
                ResultObject result = requireCon.GetDemandSite(Convert.ToInt32(id));
                if (result == null)
                {
                    return;
                }
                DemandVO = result.Result as DemandViewVO;
                headImg = uBO.GetDemandIMG(DemandVO.DemandId);
            }

        }
        public string RemoveComma(string input)
        {
            if (input.Length == 0)
                return input;

            char[] charList = input.ToCharArray();
            int first = 0, last = charList.Length - 1;
            if (charList[0] == ',')
                first++;
            if (charList[charList.Length - 1] == ',')
                last--;
            string result = "";
            for (int i = first; i <= last; i++)
            {
                result += charList[i];
            }
            return result;
        }
    }
}