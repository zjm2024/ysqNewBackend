using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class BCPartyVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BCPartyVO));

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
        public Int32 GroupID { get { return (Int32)GetValue(typeof(Int32), "GroupID"); } set { SetValue("GroupID", value); } }
        [DataMember]
        public String PosterImg { get { return (String)GetValue(typeof(String), "PosterImg"); } set { SetValue("PosterImg", value); } }
        [DataMember]
        public String LuckDrawShareImg { get { return (String)GetValue(typeof(String), "LuckDrawShareImg"); } set { SetValue("LuckDrawShareImg", value); } }
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
        public String Host { get { return (String)GetValue(typeof(String), "Host"); } set { SetValue("Host", value); } }
        [DataMember]
        public Int32 ReadCount { get { return (Int32)GetValue(typeof(Int32), "ReadCount"); } set { SetValue("ReadCount", value); } }
        [DataMember]
        public Int32 RecordSignUpCount { get { return (Int32)GetValue(typeof(Int32), "RecordSignUpCount"); } set { SetValue("RecordSignUpCount", value); } }
        [DataMember]
        public Int32 Forward { get { return (Int32)GetValue(typeof(Int32), "Forward"); } set { SetValue("Forward", value); } }
        [DataMember]
        public String QRImg { get { return (String)GetValue(typeof(String), "QRImg"); } set { SetValue("QRImg", value); } }
        [DataMember]
        public Int32 limitPeopleNum { get { return (Int32)GetValue(typeof(Int32), "limitPeopleNum"); } set { SetValue("limitPeopleNum", value); } }
        
        [DataMember]
        public String QRSignInImg { get { return (String)GetValue(typeof(String), "QRSignInImg"); } set { SetValue("QRSignInImg", value); } }
        [DataMember]
        public String Content { get { return (String)GetValue(typeof(String), "Content"); } set { SetValue("Content", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public String Audio { get { return (String)GetValue(typeof(String), "Audio"); } set { SetValue("Audio", value); } }
        [DataMember]
        public String AudioName { get { return (String)GetValue(typeof(String), "AudioName"); } set { SetValue("AudioName", value); } }
        [DataMember]
        public Int32 style { get { return (Int32)GetValue(typeof(Int32), "style"); } set { SetValue("style", value); } }
        [DataMember]
        public Int32 Type { get { return (Int32)GetValue(typeof(Int32), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public Int32 LuckDrawType { get { return (Int32)GetValue(typeof(Int32), "LuckDrawType"); } set { SetValue("LuckDrawType", value); } }
        [DataMember]
        public Int32 PartyLuckDrawStatus { get { return (Int32)GetValue(typeof(Int32), "PartyLuckDrawStatus"); } set { SetValue("PartyLuckDrawStatus", value); } }
        [DataMember]
        public Int32 isRefund { get { return (Int32)GetValue(typeof(Int32), "isRefund"); } set { SetValue("isRefund", value); } }

        [DataMember]
        public Int32 StartSendStatus { get { return (Int32)GetValue(typeof(Int32), "StartSendStatus"); } set { SetValue("StartSendStatus", value); } }
        [DataMember]
        public Int32 SignUpSendStatus { get { return (Int32)GetValue(typeof(Int32), "SignUpSendStatus"); } set { SetValue("SignUpSendStatus", value); } }
        [DataMember]
        public DateTime LuckDrawAt { get { return (DateTime)GetValue(typeof(DateTime), "LuckDrawAt"); } set { SetValue("LuckDrawAt", value); } }

        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        [DataMember]
        public Int32 isHot { get { return (Int32)GetValue(typeof(Int32), "isHot"); } set { SetValue("isHot", value); } }
        [DataMember]
        public Int32 isIndex { get { return (Int32)GetValue(typeof(Int32), "isIndex"); } set { SetValue("isIndex", value); } }

        [DataMember]
        public Decimal LuckDrawCount { get { return (Decimal)GetValue(typeof(Decimal), "LuckDrawCount"); } set { SetValue("LuckDrawCount", value); } }

        [DataMember]
        public Int32 MediaID { get { return (Int32)GetValue(typeof(Int32), "MediaID"); } set { SetValue("MediaID", value); } }
        [DataMember]
        public Int32 isNoDraw { get { return (Int32)GetValue(typeof(Int32), "isNoDraw"); } set { SetValue("isNoDraw", value); } }
        [DataMember]
        public Int32 isRepeat { get { return (Int32)GetValue(typeof(Int32), "isRepeat"); } set { SetValue("isRepeat", value); } }
        [DataMember]
        public Int32 RepeatPartyID { get { return (Int32)GetValue(typeof(Int32), "RepeatPartyID"); } set { SetValue("RepeatPartyID", value); } }
        [DataMember]
        public Int32 isCashLuckDraw { get { return (Int32)GetValue(typeof(Int32), "isCashLuckDraw"); } set { SetValue("isCashLuckDraw", value); } }

        [DataMember]
        public String SendImg { get { return (String)GetValue(typeof(String), "SendImg"); } set { SetValue("SendImg", value); } }
        [DataMember]
        public Int32 isSendImg { get { return (Int32)GetValue(typeof(Int32), "isSendImg"); } set { SetValue("isSendImg", value); } }

        [DataMember]
        public String PaymentCode { get { return (String)GetValue(typeof(String), "PaymentCode"); } set { SetValue("PaymentCode", value); } }

        [DataMember]
        public Int32 SignupConditions { get { return (Int32)GetValue(typeof(Int32), "SignupConditions"); } set { SetValue("SignupConditions", value); } }
        [DataMember]
        public Int32 AuthorizationID { get { return (Int32)GetValue(typeof(Int32), "AuthorizationID"); } set { SetValue("AuthorizationID", value); } }
        [DataMember]
        public String SignupKeyWord { get { return (String)GetValue(typeof(String), "SignupKeyWord"); } set { SetValue("SignupKeyWord", value); } }
        [DataMember]
        public String ConditionsQR { get { return (String)GetValue(typeof(String), "ConditionsQR"); } set { SetValue("ConditionsQR", value); } }
        [DataMember]
        public Int32 IsDisplayIndex { get { return (Int32)GetValue(typeof(Int32), "IsDisplayIndex"); } set { SetValue("IsDisplayIndex", value); } }
        
        //[DataMember]
        //public String Name { get; set; }

        //[DataMember]
        //public String Headimg { get; set; }

        //[DataMember]
        //public BCPartyCostVO FirstPrize { get; set; }


        #region ICloneable Member
        public override object Clone()
        {
            BCPartyVO tmp = new BCPartyVO();
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
    //public class PartyAdVO
    //{
    //    public String imgurl { get;set;}
    //    public Int32 PartyID { get; set;}
    //    public Int32 Type { get; set; }
    //    public String GoUrl { get; set; }
    //}
}