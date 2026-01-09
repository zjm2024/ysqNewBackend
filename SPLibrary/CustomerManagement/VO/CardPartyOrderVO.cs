using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardPartyOrderVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardPartyOrderVO));

        [DataMember]
        public Int32 PartyOrderID { get { return (Int32)GetValue(typeof(Int32), "PartyOrderID"); } set { SetValue("PartyOrderID", value); } }
        [DataMember]
		public Int32 PartyID { get { return (Int32)GetValue(typeof(Int32), "PartyID") ; } set {  SetValue("PartyID", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public Int32 CardID { get { return (Int32)GetValue(typeof(Int32), "CardID"); } set { SetValue("CardID", value); } }
        [DataMember]
        public String FormId { get { return (String)GetValue(typeof(String), "FormId"); } set { SetValue("FormId", value); } }
        [DataMember]
        public String OpenId { get { return (String)GetValue(typeof(String), "OpenId"); } set { SetValue("OpenId", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String SignUpForm { get { return (String)GetValue(typeof(String), "SignUpForm"); } set { SetValue("SignUpForm", value); } }
        [DataMember]
        public String CostName { get { return (String)GetValue(typeof(String), "CostName"); } set { SetValue("CostName", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public String OrderNO { get { return (String)GetValue(typeof(String), "OrderNO"); } set { SetValue("OrderNO", value); } }
        [DataMember]
        public DateTime payAt { get { return (DateTime)GetValue(typeof(DateTime), "payAt"); } set { SetValue("payAt", value); } }
        [DataMember]
        public Int32 PartySignUpID { get { return (Int32)GetValue(typeof(Int32), "PartySignUpID"); } set { SetValue("PartySignUpID", value); } }
        [DataMember]
        public Int32 IsPayOut { get { return (Int32)GetValue(typeof(Int32), "IsPayOut"); } set { SetValue("IsPayOut", value); } }
        [DataMember]
        public Int32 InviterCID { get { return (Int32)GetValue(typeof(Int32), "InviterCID"); } set { SetValue("InviterCID", value); } }
        [DataMember]
        public Int32 Number { get { return (Int32)GetValue(typeof(Int32), "Number"); } set { SetValue("Number", value); } }

        [DataMember]
        public Int32 PromotionAward { get { return (Int32)GetValue(typeof(Int32), "PromotionAward"); } set { SetValue("PromotionAward", value); } }
        [DataMember]
        public Decimal PromotionAwardCost { get { return (Decimal)GetValue(typeof(Decimal), "PromotionAwardCost"); } set { SetValue("PromotionAwardCost", value); } }
        [DataMember]
        public Int32 PromotionAwardStatus { get { return (Int32)GetValue(typeof(Int32), "PromotionAwardStatus"); } set { SetValue("PromotionAwardStatus", value); } }
        [DataMember]
        public Int32 PromotionSignupStatus { get { return (Int32)GetValue(typeof(Int32), "PromotionSignupStatus"); } set { SetValue("PromotionSignupStatus", value); } }
        [DataMember]
        public Int32 PromotionReadStatus { get { return (Int32)GetValue(typeof(Int32), "PromotionReadStatus"); } set { SetValue("PromotionReadStatus", value); } }

        [DataMember]
        public Int32 RefundStatus { get { return (Int32)GetValue(typeof(Int32), "RefundStatus"); } set { SetValue("RefundStatus", value); } }
        [DataMember]
        public DateTime RefundAt { get { return (DateTime)GetValue(typeof(DateTime), "RefundAt"); } set { SetValue("RefundAt", value); } }
        [DataMember]
        public String sub_mchid { get { return (String)GetValue(typeof(String), "sub_mchid"); } set { SetValue("sub_mchid", value); } }
        [DataMember]
        public String transaction_id { get { return (String)GetValue(typeof(String), "transaction_id"); } set { SetValue("transaction_id", value); } }
        [DataMember]
        public Decimal SplitCost { get { return (Decimal)GetValue(typeof(Decimal), "SplitCost"); } set { SetValue("SplitCost", value); } }
        [DataMember]
        public String sp_appid { get { return (String)GetValue(typeof(String), "sp_appid"); } set { SetValue("sp_appid", value); } }
        [DataMember]
        public Int32 IsSplitOut { get { return (Int32)GetValue(typeof(Int32), "IsSplitOut"); } set { SetValue("IsSplitOut", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardPartyOrderVO tmp = new CardPartyOrderVO();
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