using CoreFramework.VO;
using Newtonsoft.Json;
using SPlatformService.Controllers;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.ProjectManagement.VO;
using SPLibrary.RequireManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI
{
    public partial class Project : System.Web.UI.Page
    {
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
                return new CustomerPrincipal().CustomerProfile.CustomerId;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = string.IsNullOrEmpty(Request.QueryString["projectId"]) ? "0" : Request.QueryString["projectId"];
            if (id != "0")
            {
                RequireController requireCon = new RequireController();
                ResultObject result = requireCon.GetProjectSite(Convert.ToInt32(id));
                if (result == null)
                {
                    return;
                }
                ProjectViewVO projectVO = result.Result as ProjectViewVO;

                lblTitle.InnerHtml = projectVO.Title;
                lblBusinessName.InnerHtml = projectVO.BusinessName;
                //lblCommission.InnerHtml = projectVO.Commission.ToString("N2");
                lblCommission.InnerHtml = projectVO.RequirementCode;
                lblAgencyName.InnerHtml = projectVO.AgencyName;
                lblCreatedAt.InnerHtml = projectVO.CreatedAt.ToString("yyyy-MM-dd");
                
                ResultObject result1 = requireCon.GetRequireSite(projectVO.RequirementId);
                RequirementViewVO requireVO = result1.Result as RequirementViewVO;

                divDescription.InnerHtml = requireVO.Description;
            }
        }
    }
}