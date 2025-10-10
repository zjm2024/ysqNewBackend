using System;
using System.Collections.Generic;
using CoreFramework.VO;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;

namespace SPlatformService.CustomerManagement
{
    public partial class CardRedPacketBrowse : BasePage

    {
        public decimal TotalCost { set; get; }
        public decimal ResidueCost { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "红包管理";
                (this.Master as Shared.MasterPage).PageNameText = "红包列表";
            }
            CardBO cBO = new CardBO(new CustomerProfile());
            List<CardRedPacketViewVO> list = cBO.CountRedPacketTotalCost("RPType=0");

            for (int i=0;i<list.Count;i++) {
                TotalCost = TotalCost + list[i].RPCost;
                ResidueCost = ResidueCost + list[i].RPResidueCost;
            }
            

            base.ValidPageRight("红包列表", "Read");
        }
    }
}