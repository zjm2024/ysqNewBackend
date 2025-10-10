using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CoreFramework.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BusinessCard.Shared
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        public PersonalVO PersonalVO { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            CustomerProfile _customerProfile = new CustomerPrincipal().CustomerProfile;
            int customerId = _customerProfile.CustomerId;
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO = new PersonalVO();
            PersonalVO = cBO.FindPersonalByCustomerId(customerId);
        }
    }
}