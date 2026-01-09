using SPLibrary.CoreFramework.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using CoreFramework.VO;

namespace BusinessCard
{
    public partial class index : CustomerBasePage
    {
        public PersonalVO PersonalVO { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            int customerId = CustomerProfile.CustomerId;
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO = new PersonalVO();
            PersonalVO = cBO.FindPersonalByCustomerId(customerId);
            
        }
    }
}