using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardPartySignUpVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardPartySignUpVO));

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
        public Int32 CardID { get { return (Int32)GetValue(typeof(Int32), "CardID"); } set { SetValue("CardID", value); } }
        [DataMember]
        public String FormId { get { return (String)GetValue(typeof(String), "FormId"); } set { SetValue("FormId", value); } }
        [DataMember]
        public String FjUrl { get { return (String)GetValue(typeof(String), "FjUrl"); } set { SetValue("FjUrl", value); } }
        [DataMember]
        public String OpenId { get { return (String)GetValue(typeof(String), "OpenId"); } set { SetValue("OpenId", value); } }
        [DataMember]
        public String QRCodeImg { get { return (String)GetValue(typeof(String), "QRCodeImg"); } set { SetValue("QRCodeImg", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 SignUpStatus { get { return (Int32)GetValue(typeof(Int32), "SignUpStatus"); } set { SetValue("SignUpStatus", value); } }
        [DataMember]
        public String SignUpForm { get { return (String)GetValue(typeof(String), "SignUpForm"); } set { SetValue("SignUpForm", value); } }
        [DataMember]
        public String remarkName { get { return (String)GetValue(typeof(String), "remarkName"); } set { SetValue("remarkName", value); } }
        [DataMember]
        public Int32 InviterCID { get { return (Int32)GetValue(typeof(Int32), "InviterCID"); } set { SetValue("InviterCID", value); } }
        [DataMember]
        public Int32 Number { get { return (Int32)GetValue(typeof(Int32), "Number"); } set { SetValue("Number", value); } }
        [DataMember]
        public Int32 LuckDrawStatus { get { return (Int32)GetValue(typeof(Int32), "LuckDrawStatus"); } set { SetValue("LuckDrawStatus", value); } }
        [DataMember]
        public String LuckDrawNames { get { return (String)GetValue(typeof(String), "LuckDrawNames"); } set { SetValue("LuckDrawNames", value); } }
        [DataMember]
        public String LuckDrawContent { get { return (String)GetValue(typeof(String), "LuckDrawContent"); } set { SetValue("LuckDrawContent", value); } }
        [DataMember]
        public String LuckDrawNumber { get { return (String)GetValue(typeof(String), "LuckDrawNumber"); } set { SetValue("LuckDrawNumber", value); } }
        [DataMember]
        public String LogisticsOrderNo { get { return (String)GetValue(typeof(String), "LogisticsOrderNo"); } set { SetValue("LogisticsOrderNo", value); } }
        [DataMember]
        public Int32 isDeliver { get { return (Int32)GetValue(typeof(Int32), "isDeliver"); } set { SetValue("isDeliver", value); } }
        [DataMember]
        public DateTime DeliverAt { get { return (DateTime)GetValue(typeof(DateTime), "DeliverAt"); } set { SetValue("DeliverAt", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }
        [DataMember]
        public Int32 LuckDrawPayStatus { get { return (Int32)GetValue(typeof(Int32), "LuckDrawPayStatus"); } set { SetValue("LuckDrawPayStatus", value); } }
        [DataMember]
        public Int32 LuckDrawOrder { get { return (Int32)GetValue(typeof(Int32), "LuckDrawOrder"); } set { SetValue("LuckDrawOrder", value); } }

        [DataMember]
        public Int32 isAutoAdd { get { return (Int32)GetValue(typeof(Int32), "isAutoAdd"); } set { SetValue("isAutoAdd", value); } }
        [DataMember]
        public String SeatNo { get { return (String)GetValue(typeof(String), "SeatNo"); } set { SetValue("SeatNo", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardPartySignUpVO tmp = new CardPartySignUpVO();
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