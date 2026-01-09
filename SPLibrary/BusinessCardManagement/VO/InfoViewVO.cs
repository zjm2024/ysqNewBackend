using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class InfoViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(InfoViewVO));
       
		[DataMember]
		public Int32 InfoID { get { return (Int32)GetValue(typeof(Int32), "InfoID") ; } set {  SetValue("InfoID", value); } }
        [DataMember]
		public Int32 SortID { get { return (Int32)GetValue(typeof(Int32), "SortID") ; } set {  SetValue("SortID", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public Int32 BoardID { get { return (Int32)GetValue(typeof(Int32), "BoardID"); } set { SetValue("BoardID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 ToPersonalID { get { return (Int32)GetValue(typeof(Int32), "ToPersonalID"); } set { SetValue("ToPersonalID", value); } }
        [DataMember]
        public String Image { get { return (String)GetValue(typeof(String), "Image"); } set { SetValue("Image", value); } }
        [DataMember]
        public String InfoQR { get { return (String)GetValue(typeof(String), "InfoQR"); } set { SetValue("InfoQR", value); } }
        [DataMember]
        public String Video { get { return (String)GetValue(typeof(String), "Video"); } set { SetValue("Video", value); } }
        [DataMember]
        public Int32 Duration { get { return (Int32)GetValue(typeof(Int32), "Duration"); } set { SetValue("Duration", value); } }
        [DataMember]
        public String Content { get { return (String)GetValue(typeof(String), "Content"); } set { SetValue("Content", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String Type { get { return (String)GetValue(typeof(String), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public String SubFile { get { return (String)GetValue(typeof(String), "SubFile"); } set { SetValue("SubFile", value); } }
        [DataMember]
        public Int32 Order_info { get { return (Int32)GetValue(typeof(Int32), "Order_info"); } set { SetValue("Order_info", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String ToName { get { return (String)GetValue(typeof(String), "ToName"); } set { SetValue("ToName", value); } }
        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public String Position { get { return (String)GetValue(typeof(String), "Position"); } set { SetValue("Position", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public String CostName { get { return (String)GetValue(typeof(String), "CostName"); } set { SetValue("CostName", value); } }
        [DataMember]
        public Int32 isCost { get { return (Int32)GetValue(typeof(Int32), "isCost"); } set { SetValue("isCost", value); } }
        [DataMember]
        public Int32 isGroupBuy { get { return (Int32)GetValue(typeof(Int32), "isGroupBuy"); } set { SetValue("isGroupBuy", value); } }
        [DataMember]
        public Int32 GroupBuyPeopleNumber { get { return (Int32)GetValue(typeof(Int32), "GroupBuyPeopleNumber"); } set { SetValue("GroupBuyPeopleNumber", value); } }
        [DataMember]
        public Int32 GroupBuyDays { get { return (Int32)GetValue(typeof(Int32), "GroupBuyDays"); } set { SetValue("GroupBuyDays", value); } }
        [DataMember]
        public Int32 GroupBuyDiscount { get { return (Int32)GetValue(typeof(Int32), "GroupBuyDiscount"); } set { SetValue("GroupBuyDiscount", value); } }

        [DataMember]
        public Int32 isSeckill { get { return (Int32)GetValue(typeof(Int32), "isSeckill"); } set { SetValue("isSeckill", value); } }
        [DataMember]
        public Int32 SeckillDiscount { get { return (Int32)GetValue(typeof(Int32), "SeckillDiscount"); } set { SetValue("SeckillDiscount", value); } }
        [DataMember]
        public DateTime SeckillStartTime { get { return (DateTime)GetValue(typeof(DateTime), "SeckillStartTime"); } set { SetValue("SeckillStartTime", value); } }
        [DataMember]
        public DateTime SeckillEndTime { get { return (DateTime)GetValue(typeof(DateTime), "SeckillEndTime"); } set { SetValue("SeckillEndTime", value); } }
        [DataMember]
        public Int32 SeckillLimit { get { return (Int32)GetValue(typeof(Int32), "SeckillLimit"); } set { SetValue("SeckillLimit", value); } }

        [DataMember]
        public String OfficialProducts { get { return (String)GetValue(typeof(String), "OfficialProducts"); } set { SetValue("OfficialProducts", value); } }
        [DataMember]
        public Int32 ReadCount { get { return (Int32)GetValue(typeof(Int32), "ReadCount"); } set { SetValue("ReadCount", value); } }
        [DataMember]
        public Int32 PerPersonLimit { get { return (Int32)GetValue(typeof(Int32), "PerPersonLimit"); } set { SetValue("PerPersonLimit", value); } }
        [DataMember]
        public Int32 ProductLimit { get { return (Int32)GetValue(typeof(Int32), "ProductLimit"); } set { SetValue("ProductLimit", value); } }
        [DataMember]
        public Int32 StoreAmount { get { return (Int32)GetValue(typeof(Int32), "StoreAmount"); } set { SetValue("StoreAmount", value); } }
        [DataMember]
        public Decimal GiveIntegral { get { return (Decimal)GetValue(typeof(Decimal), "GiveIntegral"); } set { SetValue("GiveIntegral", value); } }
        [DataMember]
        public Int32 Profitsharing { get { return (Int32)GetValue(typeof(Int32), "Profitsharing"); } set { SetValue("Profitsharing", value); } }
        [DataMember]
        public Int32 TowProfitsharing { get { return (Int32)GetValue(typeof(Int32), "TowProfitsharing"); } set { SetValue("TowProfitsharing", value); } }
        [DataMember]
        public Int32 isProfitsharing { get { return (Int32)GetValue(typeof(Int32), "isProfitsharing"); } set { SetValue("isProfitsharing", value); } }
        [DataMember]
        public Int32 isProfitsharingToVIP { get { return (Int32)GetValue(typeof(Int32), "isProfitsharingToVIP"); } set { SetValue("isProfitsharingToVIP", value); } }
        [DataMember]
        public String ProfitsharingToJSON { get { return (String)GetValue(typeof(String), "ProfitsharingToJSON"); } set { SetValue("ProfitsharingToJSON", value); } }

        [DataMember]
        public String SortName { get { return (String)GetValue(typeof(String), "SortName"); } set { SetValue("SortName", value); } }
        [DataMember]
        public Int32 GiveShopVipID { get { return (Int32)GetValue(typeof(Int32), "GiveShopVipID"); } set { SetValue("GiveShopVipID", value); } }
        [DataMember]
        public Int32 GiveShopVipDay { get { return (Int32)GetValue(typeof(Int32), "GiveShopVipDay"); } set { SetValue("GiveShopVipDay", value); } }
        [DataMember]
        public Int32 isVipDiscount { get { return (Int32)GetValue(typeof(Int32), "isVipDiscount"); } set { SetValue("isVipDiscount", value); } }
        [DataMember]
        public String VipDiscountToJSON { get { return (String)GetValue(typeof(String), "VipDiscountToJSON"); } set { SetValue("VipDiscountToJSON", value); } }

        [DataMember]
        public List<String> ImgList{ get; set; }
        [DataMember]
        public List<AgentlevelCostVO> AgentlevelCostList { get; set; }
        [DataMember]
        public List<InfoCostVO> InfoCostList { get; set; }

        #region ICloneable Member
        public override object Clone()
        {
            InfoViewVO tmp = new InfoViewVO();
            tmp.changeData = new Dictionary<string, object>(this.changeData);
            tmp.originData = new Dictionary<string, object>(this.originData);
            return tmp;
        }
        #endregion
         
    	#region ICommonVO Member
        List<string> ICommonVO.PropertyList
        {
            get { return _propertyList; }
        }
        #endregion
    }
}