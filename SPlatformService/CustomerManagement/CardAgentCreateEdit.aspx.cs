using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using CoreFramework.VO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SPLibrary.UserManagement.VO;

namespace SPlatformService.UserManagement
{
    public partial class CardAgentCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "乐聊名片";
                (this.Master as Shared.MasterPage).PageNameText = "代理商";
            }
            base.ValidPageRight("代理商", "Edit");

            int AgentID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["CardAgentID"]) ? "0" : Request.QueryString["CardAgentID"]);
            this.CardAgentID.Value = AgentID.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidCardAgentID='").Append(CardAgentID.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CardMessageCreateEdit", sb.ToString());

            BindData(AgentID);
        }
        public void BindData(int AgentID=0)
        {
            CityBO sBO = new CityBO(new UserProfile());
            int Province = 0;
            int City = 0;
            if (AgentID > 0)
            {
                CardBO cBO = new CardBO(new CustomerProfile(), UserProfile.AppType);
                CardAgentVO vo = cBO.FindCardAgentById(AgentID);
                if (vo != null)
                {
                    City=vo.CityId;
                    CityVO CityVO = sBO.FindCityById(City);
                    Province = CityVO.ProvinceId;
                }
            }

            drpProvince.Items.Clear();
            
            List<ProvinceVO> voList = sBO.FindProvinceList(true);
            int ProvinceIndex = 0;
            int i = 0;
            foreach (ProvinceVO pVO in voList)
            {
                drpProvince.Items.Add(new ListItem(pVO.ProvinceName, pVO.ProvinceId.ToString()));
                if(pVO.ProvinceId== Province)
                {
                    ProvinceIndex = i;
                }
                i++;
            }
            drpProvince.SelectedIndex = ProvinceIndex;

            drpCity.Items.Clear();
            List<CityVO> voChildList = sBO.FindCityByProvince(Convert.ToInt32(drpProvince.SelectedValue), true);
            int CityIndex = 0;
            i = 0;
            foreach (CityVO cVO in voChildList)
            {
                drpCity.Items.Add(new ListItem(cVO.CityName, cVO.CityId.ToString()));
                if (cVO.CityId == City)
                {
                    CityIndex = i;
                }
                i++;
            }
            drpCity.SelectedIndex = CityIndex;
        }
    }
}