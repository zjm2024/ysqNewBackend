using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardPartyViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardPartyViewVO));

        [DataMember]
        public Int32 PartyID { get { return (Int32)GetValue(typeof(Int32), "PartyID"); } set { SetValue("PartyID", value); } }
        [DataMember]
		public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String PartyTag { get { return (String)GetValue(typeof(String), "PartyTag"); } set { SetValue("PartyTag", value); } }
        [DataMember]
        public String MainImg { get { return (String)GetValue(typeof(String), "MainImg"); } set { SetValue("MainImg", value); } }
        [DataMember]
        public DateTime StartTime { get { return (DateTime)GetValue(typeof(DateTime), "StartTime"); } set { SetValue("StartTime", value); }}
        [DataMember]
        public DateTime EndTime { get { return (DateTime)GetValue(typeof(DateTime), "EndTime"); } set { SetValue("EndTime", value); }}
        [DataMember]
        public DateTime SignUpTime { get { return (DateTime)GetValue(typeof(DateTime), "SignUpTime"); } set { SetValue("SignUpTime", value); }}
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); }}
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
        public String Details2 { get { return (String)GetValue(typeof(String), "Details2"); } set { SetValue("Details2", value); } }
        [DataMember]
        public String QRCodeImg { get { return (String)GetValue(typeof(String), "QRCodeImg"); } set { SetValue("QRCodeImg", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Int32 Type { get { return (Int32)GetValue(typeof(Int32), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public Int32 LuckDrawType { get { return (Int32)GetValue(typeof(Int32), "LuckDrawType"); } set { SetValue("LuckDrawType", value); } }
        [DataMember]
        public Int32 PartyLuckDrawStatus { get { return (Int32)GetValue(typeof(Int32), "PartyLuckDrawStatus"); } set { SetValue("PartyLuckDrawStatus", value); } }
        [DataMember]
        public DateTime LuckDrawAt { get { return (DateTime)GetValue(typeof(DateTime), "LuckDrawAt"); } set { SetValue("LuckDrawAt", value); } }
        [DataMember]
        public String HeaderLogo { get { return (String)GetValue(typeof(String), "HeaderLogo"); } set { SetValue("HeaderLogo", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public Int32 GroupID { get { return (Int32)GetValue(typeof(Int32), "GroupID"); } set { SetValue("GroupID", value); } }
        [DataMember]
        public String PosterImg { get { return (String)GetValue(typeof(String), "PosterImg"); } set { SetValue("PosterImg", value); } }
        [DataMember]
        public Int32 isDisplayContacts { get { return (Int32)GetValue(typeof(Int32), "isDisplayContacts"); } set { SetValue("isDisplayContacts", value); } }
        [DataMember]
        public Int32 isDisplaySignup { get { return (Int32)GetValue(typeof(Int32), "isDisplaySignup"); } set { SetValue("isDisplaySignup", value); } }
        [DataMember]
        public Int32 isClickSignup { get { return (Int32)GetValue(typeof(Int32), "isClickSignup"); } set { SetValue("isClickSignup", value); } }
        [DataMember]
        public Int32 isDisplayCost { get { return (Int32)GetValue(typeof(Int32), "isDisplayCost"); } set { SetValue("isDisplayCost", value); } }
        [DataMember]
        public Int32 isPromotionAward { get { return (Int32)GetValue(typeof(Int32), "isPromotionAward"); } set { SetValue("isPromotionAward", value); } }
        [DataMember]
        public Int32 isPromotionSignup { get { return (Int32)GetValue(typeof(Int32), "isPromotionSignup"); } set { SetValue("isPromotionSignup", value); } }
        [DataMember]
        public Int32 isPromotionRead { get { return (Int32)GetValue(typeof(Int32), "isPromotionRead"); } set { SetValue("isPromotionRead", value); } }
        [DataMember]
        public Int32 isStartTime { get { return (Int32)GetValue(typeof(Int32), "isStartTime"); } set { SetValue("isStartTime", value); } }
        [DataMember]
        public Int32 isEndTime { get { return (Int32)GetValue(typeof(Int32), "isEndTime"); } set { SetValue("isEndTime", value); } }
        [DataMember]
        public Int32 isBlindBox { get { return (Int32)GetValue(typeof(Int32), "isBlindBox"); } set { SetValue("isBlindBox", value); } }
        [DataMember]
        public Int32 SignupCount { get { return (Int32)GetValue(typeof(Int32), "SignupCount"); } set { SetValue("SignupCount", value); } }
        [DataMember]
        public String Host { get { return (String)GetValue(typeof(String), "Host"); } set { SetValue("Host", value); } }
        [DataMember]
        public Int32 ReadCount { get { return (Int32)GetValue(typeof(Int32), "ReadCount"); } set { SetValue("ReadCount", value); } }
        [DataMember]
        public Int32 Forward { get { return (Int32)GetValue(typeof(Int32), "Forward"); } set { SetValue("Forward", value); } }

        [DataMember]
        public String Audio { get { return (String)GetValue(typeof(String), "Audio"); } set { SetValue("Audio", value); } }
        [DataMember]
        public String AudioName { get { return (String)GetValue(typeof(String), "AudioName"); } set { SetValue("AudioName", value); } }
        [DataMember]
        public Int32 style { get { return (Int32)GetValue(typeof(Int32), "style"); } set { SetValue("style", value); } }

        [DataMember]
        public List<CardPartySignUpVO> CardPartySignUp { get;set; }
        [DataMember]
        public List<CardPartyContactsViewVO> CardPartyContactsView { get; set; }
        [DataMember]
        public List<CardPartySignUpFormVO> CardPartySignUpForm { get; set; }
        [DataMember]
        public List<CardPartyCostVO> CardPartyCost { get; set; }
        [DataMember]
        public Int32 limitPeopleNum { get { return (Int32)GetValue(typeof(Int32), "limitPeopleNum"); } set { SetValue("limitPeopleNum", value); } }
        [DataMember]
        public Int32 maxImum { get { return (Int32)GetValue(typeof(Int32), "maxImum"); } set { SetValue("maxImum", value); } }
        
        [DataMember]
        public String Content { get { return (String)GetValue(typeof(String), "Content"); } set { SetValue("Content", value); } }
        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardPartyViewVO tmp = new CardPartyViewVO();
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