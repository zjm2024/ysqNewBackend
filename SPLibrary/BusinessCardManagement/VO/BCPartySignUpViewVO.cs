using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class BCPartySignUpViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BCPartySignUpViewVO));

        [DataMember]
        public Int32 PartySignUpID { get { return (Int32)GetValue(typeof(Int32), "PartySignUpID"); } set { SetValue("PartySignUpID", value); } }
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
        public String CorporateName { get { return (String)GetValue(typeof(String), "CorporateName"); } set { SetValue("CorporateName", value); } }
        [DataMember]
        public Int32 CardID { get { return (Int32)GetValue(typeof(Int32), "CardID"); } set { SetValue("CardID", value); } }
        [DataMember]
        public String FormId { get { return (String)GetValue(typeof(String), "FormId"); } set { SetValue("FormId", value); } }
        [DataMember]
        public String FjUrl { get { return (String)GetValue(typeof(String), "FjUrl"); } set { SetValue("FjUrl", value); } }
        [DataMember]
        public String OpenId { get { return (String)GetValue(typeof(String), "OpenId"); } set { SetValue("OpenId", value); } }
        [DataMember]
        public String SignUpQRCodeImg { get { return (String)GetValue(typeof(String), "SignUpQRCodeImg"); } set { SetValue("SignUpQRCodeImg", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 GroupID { get { return (Int32)GetValue(typeof(Int32), "GroupID"); } set { SetValue("GroupID", value); } }
        [DataMember]
        public String SignUpForm { get { return (String)GetValue(typeof(String), "SignUpForm"); } set { SetValue("SignUpForm", value); } }
        [DataMember]
        public Int32 PartyLuckDrawStatus { get { return (Int32)GetValue(typeof(Int32), "PartyLuckDrawStatus"); } set { SetValue("PartyLuckDrawStatus", value); } }
        [DataMember]
        public String LuckDrawNames { get { return (String)GetValue(typeof(String), "LuckDrawNames"); } set { SetValue("LuckDrawNames", value); } }
        [DataMember]
        public String LuckDrawContent { get { return (String)GetValue(typeof(String), "LuckDrawContent"); } set { SetValue("LuckDrawContent", value); } }
        [DataMember]
        public String LuckDrawNumber { get { return (String)GetValue(typeof(String), "LuckDrawNumber"); } set { SetValue("LuckDrawNumber", value); } }

        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public Int32 HostCustomerId { get { return (Int32)GetValue(typeof(Int32), "HostCustomerId"); } set { SetValue("HostCustomerId", value); } }
        [DataMember]
        public String MainImg { get { return (String)GetValue(typeof(String), "MainImg"); } set { SetValue("MainImg", value); } }
        [DataMember]
        public DateTime StartTime { get { return (DateTime)GetValue(typeof(DateTime), "StartTime"); } set { SetValue("StartTime", value); } }
        [DataMember]
        public DateTime EndTime { get { return (DateTime)GetValue(typeof(DateTime), "EndTime"); } set { SetValue("EndTime", value); } }
        [DataMember]
        public DateTime SignUpTime { get { return (DateTime)GetValue(typeof(DateTime), "SignUpTime"); } set { SetValue("SignUpTime", value); } }
        [DataMember]
        public String Address { get { return (String)GetValue(typeof(String), "Address"); } set { SetValue("Address", value); } }
        [DataMember]
        public String DetailedAddress { get { return (String)GetValue(typeof(String), "DetailedAddress"); } set { SetValue("DetailedAddress", value); } }
        [DataMember]
        public Decimal latitude { get { return (Decimal)GetValue(typeof(Decimal), "latitude"); } set { SetValue("latitude", value); } }
        [DataMember]
        public Decimal longitude { get { return (Decimal)GetValue(typeof(Decimal), "longitude"); } set { SetValue("longitude", value); } }
        [DataMember]
        public String Details { get { return (String)GetValue(typeof(String), "Details"); } set { SetValue("Details", value); } }
        [DataMember]
        public String QRCodeImg { get { return (String)GetValue(typeof(String), "QRCodeImg"); } set { SetValue("QRCodeImg", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public String HeaderLogo { get { return (String)GetValue(typeof(String), "HeaderLogo"); } set { SetValue("HeaderLogo", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public Int32 SignUpStatus { get { return (Int32)GetValue(typeof(Int32), "SignUpStatus"); } set { SetValue("SignUpStatus", value); } }
        [DataMember]
        public String Host { get { return (String)GetValue(typeof(String), "Host"); } set { SetValue("Host", value); } }
        [DataMember]
        public Int32 PartyOrderID { get { return (Int32)GetValue(typeof(Int32), "PartyOrderID"); } set { SetValue("PartyOrderID", value); } }
        [DataMember]
        public String CostName { get { return (String)GetValue(typeof(String), "CostName"); } set { SetValue("CostName", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public Int32 OrderStatus { get { return (Int32)GetValue(typeof(Int32), "OrderStatus"); } set { SetValue("OrderStatus", value); } }
        [DataMember]
        public Int32 PromotionAward { get { return (Int32)GetValue(typeof(Int32), "PromotionAward"); } set { SetValue("PromotionAward", value); } }
        //[DataMember]
        //public String PromotionAwardName { get { return (String)GetValue(typeof(String), "PromotionAwardName"); } set { SetValue("PromotionAwardName", value); } }
        [DataMember]
        public Decimal PromotionAwardCost { get { return (Decimal)GetValue(typeof(Decimal), "PromotionAwardCost"); } set { SetValue("PromotionAwardCost", value); } }
        [DataMember]
        public Int32 PromotionAwardStatus { get { return (Int32)GetValue(typeof(Int32), "PromotionAwardStatus"); } set { SetValue("PromotionAwardStatus", value); } }
        [DataMember]
        public Int32 Type { get { return (Int32)GetValue(typeof(Int32), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public Int32 LuckDrawType { get { return (Int32)GetValue(typeof(Int32), "LuckDrawType"); } set { SetValue("LuckDrawType", value); } }
        [DataMember]
        public Int32 LuckDrawStatus { get { return (Int32)GetValue(typeof(Int32), "LuckDrawStatus"); } set { SetValue("LuckDrawStatus", value); } }
        [DataMember]
        public DateTime LuckDrawAt { get { return (DateTime)GetValue(typeof(DateTime), "LuckDrawAt"); } set { SetValue("LuckDrawAt", value); } }
        [DataMember]
        public Int32 isStartTime { get { return (Int32)GetValue(typeof(Int32), "isStartTime"); } set { SetValue("isStartTime", value); } }
        [DataMember]
        public Int32 isEndTime { get { return (Int32)GetValue(typeof(Int32), "isEndTime"); } set { SetValue("isEndTime", value); } }

        [DataMember]
        public bool isSponsor { get { return (bool)GetValue(typeof(bool), "isSponsor"); } set { SetValue("isSponsor", value); } }

        [DataMember]
        public List<BCPartyContactsViewVO> BCPartyContactsView { get; set; }
        [DataMember]
        public String remarkName { get { return (String)GetValue(typeof(String), "remarkName"); } set { SetValue("remarkName", value); } }
        [DataMember]
        public Int32 InviterCID { get { return (Int32)GetValue(typeof(Int32), "InviterCID"); } set { SetValue("InviterCID", value); } }
        [DataMember]
        public Int32 CountNum { get { return (Int32)GetValue(typeof(Int32), "CountNum"); } set { SetValue("CountNum", value); } }
        [DataMember]
        public Int32 OrderStatusNum { get { return (Int32)GetValue(typeof(Int32), "OrderStatusNum"); } set { SetValue("OrderStatusNum", value); } }
        [DataMember]
        public List<BCPartySignUpVO> BCPartySignUp { get; set; }

        [DataMember]
        public String LogisticsOrderNo { get { return (String)GetValue(typeof(String), "LogisticsOrderNo"); } set { SetValue("LogisticsOrderNo", value); } }
        [DataMember]
        public Int32 isDeliver { get { return (Int32)GetValue(typeof(Int32), "isDeliver"); } set { SetValue("isDeliver", value); } }
        [DataMember]
        public DateTime DeliverAt { get { return (DateTime)GetValue(typeof(DateTime), "DeliverAt"); } set { SetValue("DeliverAt", value); } }

        [DataMember]
        public String HostHeadimg { get { return (String)GetValue(typeof(String), "HostHeadimg"); } set { SetValue("HostHeadimg", value); } }
        [DataMember]
        public String HostName { get { return (String)GetValue(typeof(String), "HostName"); } set { SetValue("HostName", value); } }

        [DataMember]
        public Int32 Number { get { return (Int32)GetValue(typeof(Int32), "Number"); } set { SetValue("Number", value); } }

        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        [DataMember]
        public Int32 Sequence { get; set; }

        [DataMember]
        public Int32 IsDisplayIndex { get { return (Int32)GetValue(typeof(Int32), "IsDisplayIndex"); } set { SetValue("IsDisplayIndex", value); } }

        [DataMember]
        public Int32 IntegrateCount { get { return (Int32)GetValue(typeof(Int32), "IntegrateCount"); } set { SetValue("IntegrateCount", value); } }


        #region ICloneable Member
        public override object Clone()
        {
            BCPartySignUpViewVO tmp = new BCPartySignUpViewVO();
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