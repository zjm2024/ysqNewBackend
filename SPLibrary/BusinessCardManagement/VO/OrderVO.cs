using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class OrderVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(OrderVO));
        
		[DataMember]
		public Int32 OrderID { get { return (Int32)GetValue(typeof(Int32), "OrderID") ; } set {  SetValue("OrderID", value); } }
        [DataMember]
		public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID") ; } set {  SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public String FormId { get { return (String)GetValue(typeof(String), "FormId"); } set { SetValue("FormId", value); } }
        [DataMember]
        public String OpenId { get { return (String)GetValue(typeof(String), "OpenId"); } set { SetValue("OpenId", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public Decimal OriginalCost { get { return (Decimal)GetValue(typeof(Decimal), "OriginalCost"); } set { SetValue("OriginalCost", value); } }
        [DataMember]
        public Int32 CostID { get { return (Int32)GetValue(typeof(Int32), "CostID"); } set { SetValue("CostID", value); } }
        [DataMember]
        public String CostName { get { return (String)GetValue(typeof(String), "CostName"); } set { SetValue("CostName", value); } }
        [DataMember]
        public String OrderNO { get { return (String)GetValue(typeof(String), "OrderNO"); } set { SetValue("OrderNO", value); } }
        
        [DataMember]
        public String transaction_id { get { return (String)GetValue(typeof(String), "transaction_id"); } set { SetValue("transaction_id", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Int32 PayOutStatus { get { return (Int32)GetValue(typeof(Int32), "PayOutStatus"); } set { SetValue("PayOutStatus", value); } }
        [DataMember]
        public DateTime payAt { get { return (DateTime)GetValue(typeof(DateTime), "payAt"); } set { SetValue("payAt", value); } }
        [DataMember]
        public Int32 AgentPersonalID { get { return (Int32)GetValue(typeof(Int32), "AgentPersonalID"); } set { SetValue("AgentPersonalID", value); } }
        [DataMember]
        public Int32 InfoID { get { return (Int32)GetValue(typeof(Int32), "InfoID"); } set { SetValue("InfoID", value); } }
        [DataMember]
        public Int32 isUsed { get { return (Int32)GetValue(typeof(Int32), "isUsed"); } set { SetValue("isUsed", value); } }
        [DataMember]
        public Int32 isWriteOff { get { return (Int32)GetValue(typeof(Int32), "isWriteOff"); } set { SetValue("isWriteOff", value); } }
        [DataMember]
        public DateTime WriteOffAt { get { return (DateTime)GetValue(typeof(DateTime), "WriteOffAt"); } set { SetValue("WriteOffAt", value); } }
        [DataMember]
        public Int32 WriteOffPersonalID { get { return (Int32)GetValue(typeof(Int32), "WriteOffPersonalID"); } set { SetValue("WriteOffPersonalID", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String Address { get { return (String)GetValue(typeof(String), "Address"); } set { SetValue("Address", value); } }
        [DataMember]
        public Int32 GroupBuyID { get { return (Int32)GetValue(typeof(Int32), "GroupBuyID"); } set { SetValue("GroupBuyID", value); } }
        [DataMember]
        public Int32 isGroupBuy { get { return (Int32)GetValue(typeof(Int32), "isGroupBuy"); } set { SetValue("isGroupBuy", value); } }
        [DataMember]
        public Int32 isAgentBuy { get { return (Int32)GetValue(typeof(Int32), "isAgentBuy"); } set { SetValue("isAgentBuy", value); } }
        [DataMember]
        public Int32 isEcommerceBuy { get { return (Int32)GetValue(typeof(Int32), "isEcommerceBuy"); } set { SetValue("isEcommerceBuy", value); } }

        [DataMember]
        public Int32 Discount { get { return (Int32)GetValue(typeof(Int32), "Discount"); } set { SetValue("Discount", value); } }

        [DataMember]
        public Int32 RebateCid1 { get { return (Int32)GetValue(typeof(Int32), "RebateCid1"); } set { SetValue("RebateCid1", value); } }
        [DataMember]
        public Decimal RebateCost1 { get { return (Decimal)GetValue(typeof(Decimal), "RebateCost1"); } set { SetValue("RebateCost1", value); } }
        [DataMember]
        public Int32 RebateStatus1 { get { return (Int32)GetValue(typeof(Int32), "RebateStatus1"); } set { SetValue("RebateStatus1", value); } }

        [DataMember]
        public Int32 RebateCid2 { get { return (Int32)GetValue(typeof(Int32), "RebateCid2"); } set { SetValue("RebateCid2", value); } }
        [DataMember]
        public Decimal RebateCost2 { get { return (Decimal)GetValue(typeof(Decimal), "RebateCost2"); } set { SetValue("RebateCost2", value); } }
        [DataMember]
        public Int32 RebateStatus2 { get { return (Int32)GetValue(typeof(Int32), "RebateStatus2"); } set { SetValue("RebateStatus2", value); } }

        [DataMember]
        public Int32 RebateCid3 { get { return (Int32)GetValue(typeof(Int32), "RebateCid3"); } set { SetValue("RebateCid3", value); } }
        [DataMember]
        public Decimal RebateCost3 { get { return (Decimal)GetValue(typeof(Decimal), "RebateCost3"); } set { SetValue("RebateCost3", value); } }
        [DataMember]
        public Int32 RebateStatus3 { get { return (Int32)GetValue(typeof(Int32), "RebateStatus3"); } set { SetValue("RebateStatus3", value); } }

        [DataMember]
        public Int32 RebateCid4 { get { return (Int32)GetValue(typeof(Int32), "RebateCid4"); } set { SetValue("RebateCid4", value); } }
        [DataMember]
        public Decimal RebateCost4 { get { return (Decimal)GetValue(typeof(Decimal), "RebateCost4"); } set { SetValue("RebateCost4", value); } }
        [DataMember]
        public Int32 RebateStatus4 { get { return (Int32)GetValue(typeof(Int32), "RebateStatus4"); } set { SetValue("RebateStatus4", value); } }

        [DataMember]
        public Int32 PeopleRebateCid { get { return (Int32)GetValue(typeof(Int32), "PeopleRebateCid"); } set { SetValue("PeopleRebateCid", value); } }
        [DataMember]
        public Decimal PeopleRebateCost { get { return (Decimal)GetValue(typeof(Decimal), "PeopleRebateCost"); } set { SetValue("PeopleRebateCost", value); } }
        [DataMember]
        public Int32 PeopleRebateStatus { get { return (Int32)GetValue(typeof(Int32), "PeopleRebateStatus"); } set { SetValue("PeopleRebateStatus", value); } }

        [DataMember]
        public Int32 ProfitsharingCid { get { return (Int32)GetValue(typeof(Int32), "ProfitsharingCid"); } set { SetValue("ProfitsharingCid", value); } }
        [DataMember]
        public String ProfitsharingOpenId { get { return (String)GetValue(typeof(String), "ProfitsharingOpenId"); } set { SetValue("ProfitsharingOpenId", value); } }
        [DataMember]
        public Decimal ProfitsharingCost { get { return (Decimal)GetValue(typeof(Decimal), "ProfitsharingCost"); } set { SetValue("ProfitsharingCost", value); } }
        [DataMember]
        public Int32 ProfitsharingStatus { get { return (Int32)GetValue(typeof(Int32), "ProfitsharingStatus"); } set { SetValue("ProfitsharingStatus", value); } }
        [DataMember]
        public DateTime ProfitsharingAt { get { return (DateTime)GetValue(typeof(DateTime), "ProfitsharingAt"); } set { SetValue("ProfitsharingAt", value); } }


        [DataMember]
        public Int32 TowProfitsharingCid { get { return (Int32)GetValue(typeof(Int32), "TowProfitsharingCid"); } set { SetValue("TowProfitsharingCid", value); } }
        [DataMember]
        public String TowProfitsharingOpenId { get { return (String)GetValue(typeof(String), "TowProfitsharingOpenId"); } set { SetValue("TowProfitsharingOpenId", value); } }
        [DataMember]
        public Decimal TowProfitsharingCost { get { return (Decimal)GetValue(typeof(Decimal), "TowProfitsharingCost"); } set { SetValue("TowProfitsharingCost", value); } }
        [DataMember]
        public Int32 TowProfitsharingStatus { get { return (Int32)GetValue(typeof(Int32), "TowProfitsharingStatus"); } set { SetValue("TowProfitsharingStatus", value); } }
        [DataMember]
        public DateTime TowProfitsharingAt { get { return (DateTime)GetValue(typeof(DateTime), "TowProfitsharingAt"); } set { SetValue("TowProfitsharingAt", value); } }

        [DataMember]
        public String QRImg { get { return (String)GetValue(typeof(String), "QRImg"); } set { SetValue("QRImg", value); } }
        [DataMember]
        public Int32 IntegralID { get { return (Int32)GetValue(typeof(Int32), "IntegralID"); } set { SetValue("IntegralID", value); } }
        [DataMember]
        public Int32 AgentIntegralID { get { return (Int32)GetValue(typeof(Int32), "AgentIntegralID"); } set { SetValue("AgentIntegralID", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        [DataMember]
        public String sub_mchid { get { return (String)GetValue(typeof(String), "sub_mchid"); } set { SetValue("sub_mchid", value); } }
        [DataMember]
        public Decimal SplitCost { get { return (Decimal)GetValue(typeof(Decimal), "SplitCost"); } set { SetValue("SplitCost", value); } }
        [DataMember]
        public String sp_appid { get { return (String)GetValue(typeof(String), "sp_appid"); } set { SetValue("sp_appid", value); } }
        [DataMember]
        public Int32 IsSplitOut { get { return (Int32)GetValue(typeof(Int32), "IsSplitOut"); } set { SetValue("IsSplitOut", value); } }

        [DataMember]
        public Int32 GiveShopVipID { get { return (Int32)GetValue(typeof(Int32), "GiveShopVipID"); } set { SetValue("GiveShopVipID", value); } }
        [DataMember]
        public Int32 GiveShopVipDay { get { return (Int32)GetValue(typeof(Int32), "GiveShopVipDay"); } set { SetValue("GiveShopVipDay", value); } }
        [DataMember]
        public Int32 IsGiveShopVip { get { return (Int32)GetValue(typeof(Int32), "IsGiveShopVip"); } set { SetValue("IsGiveShopVip", value); } }

        [DataMember]
        public String LogisticsOrderNo { get { return (String)GetValue(typeof(String), "LogisticsOrderNo"); } set { SetValue("LogisticsOrderNo", value); } }
        [DataMember]
        public Int32 isDeliver { get { return (Int32)GetValue(typeof(Int32), "isDeliver"); } set { SetValue("isDeliver", value); } }
        [DataMember]
        public DateTime DeliverAt { get { return (DateTime)GetValue(typeof(DateTime), "DeliverAt"); } set { SetValue("DeliverAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            OrderVO tmp = new OrderVO();
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