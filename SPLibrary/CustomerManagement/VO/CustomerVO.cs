using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class CustomerVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CustomerVO));

        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public String HeaderLogo { get { return (String)GetValue(typeof(String), "HeaderLogo"); } set { SetValue("HeaderLogo", value); } }
        [DataMember]
        public String CustomerCode { get { return (String)GetValue(typeof(String), "CustomerCode"); } set { SetValue("CustomerCode", value); } }
        [DataMember]
        public String CustomerAccount { get { return (String)GetValue(typeof(String), "CustomerAccount"); } set { SetValue("CustomerAccount", value); } }
        [DataMember]
        public String Password { get { return (String)GetValue(typeof(String), "Password"); } set { SetValue("Password", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public Boolean Sex { get { return (Boolean)GetValue(typeof(Boolean), "Sex"); } set { SetValue("Sex", value); } }
        [DataMember]
        public DateTime Birthday { get { return (DateTime)GetValue(typeof(DateTime), "Birthday"); } set { SetValue("Birthday", value); } }
        [DataMember]
        public String Email { get { return (String)GetValue(typeof(String), "Email"); } set { SetValue("Email", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Boolean Leliao { get { return (Boolean)GetValue(typeof(Boolean), "Leliao"); } set { SetValue("Leliao", value); } }
        [DataMember]
        public Boolean BusinessCard { get { return (Boolean)GetValue(typeof(Boolean), "BusinessCard"); } set { SetValue("BusinessCard", value); } }
        [DataMember]
        public Boolean LuckyDraw { get { return (Boolean)GetValue(typeof(Boolean), "LuckyDraw"); } set { SetValue("LuckyDraw", value); } }
        [DataMember]
        public Boolean SouKe { get { return (Boolean)GetValue(typeof(Boolean), "SouKe"); } set { SetValue("SouKe", value); } }
        [DataMember]
        public Int32 originCustomerId { get { return (Int32)GetValue(typeof(Int32), "originCustomerId"); } set { SetValue("originCustomerId", value); } }
        [DataMember]
        public Boolean Agent { get { return (Boolean)GetValue(typeof(Boolean), "Agent"); } set { SetValue("Agent", value); } }
        [DataMember]
        public Boolean isAdmin { get { return (Boolean)GetValue(typeof(Boolean), "isAdmin"); } set { SetValue("isAdmin", value); } }
        [DataMember]
        public Boolean isVip { get { return (Boolean)GetValue(typeof(Boolean), "isVip"); } set { SetValue("isVip", value); } }
        [DataMember]
        public DateTime ExpirationAt { get { return (DateTime)GetValue(typeof(DateTime), "ExpirationAt"); } set { SetValue("ExpirationAt", value); } }
        [DataMember]
        public Int32 ExpirationSendStatus { get { return (Int32)GetValue(typeof(Int32), "ExpirationSendStatus"); } set { SetValue("ExpirationSendStatus", value); } }
        [DataMember]
        public Int32 VipLevel { get { return (Int32)GetValue(typeof(Int32), "VipLevel"); } set { SetValue("VipLevel", value); } }

        [DataMember]
        public Boolean isIdCard { get { return (Boolean)GetValue(typeof(Boolean), "isIdCard"); } set { SetValue("isIdCard", value); } }
        [DataMember]
        public String IdCard_A { get { return (String)GetValue(typeof(String), "IdCard_A"); } set { SetValue("IdCard_A", value); } }
        [DataMember]
        public String IdCard_B { get { return (String)GetValue(typeof(String), "IdCard_B"); } set { SetValue("IdCard_B", value); } }

        [DataMember]
        public String FaceIdImg { get { return (String)GetValue(typeof(String), "FaceIdImg"); } set { SetValue("FaceIdImg", value); } }
        [DataMember]
        public String IdCardName { get { return (String)GetValue(typeof(String), "IdCardName"); } set { SetValue("IdCardName", value); } }
        [DataMember]
        public String IdCardNumber { get { return (String)GetValue(typeof(String), "IdCardNumber"); } set { SetValue("IdCardNumber", value); } }

        [DataMember]
        public String originType { get { return (String)GetValue(typeof(String), "originType"); } set { SetValue("originType", value); } }
        [DataMember]
        public Int32 originID { get { return (Int32)GetValue(typeof(Int32), "originID"); } set { SetValue("originID", value); } }

        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        [DataMember]
        public Int32 CardID { get; set; }
        [DataMember]
        public String CardPhone { get; set; }

        [DataMember]
        public String City { get { return (String)GetValue(typeof(String), "City"); } set { SetValue("City", value); } }
        [DataMember]
        public String Prov { get { return (String)GetValue(typeof(String), "Prov"); } set { SetValue("Prov", value); } }

        [DataMember]
        public Boolean isOntrial { get { return (Boolean)GetValue(typeof(Boolean), "isOntrial"); } set { SetValue("isOntrial", value); } }

        [DataMember]
        public Int32 ontrialtime { get { return (Int32)GetValue(typeof(Int32), "ontrialtime"); } set { SetValue("ontrialtime", value); } }

        [DataMember]
        public Int32 Couponcost { get { return (Int32)GetValue(typeof(Int32), "Couponcost"); } set { SetValue("Couponcost", value); } }
        [DataMember]
        public DateTime CouponAt { get { return (DateTime)GetValue(typeof(DateTime), "CouponAt"); } set { SetValue("CouponAt", value); } }

        [DataMember]
        public Int32 ExchangeCodeCount { get { return (Int32)GetValue(typeof(Int32), "ExchangeCodeCount"); } set { SetValue("ExchangeCodeCount", value); } }

        [DataMember]
        public Int32 U_ExchangeCodeCount { get { return (Int32)GetValue(typeof(Int32), "U_ExchangeCodeCount"); } set { SetValue("U_ExchangeCodeCount", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CustomerVO tmp = new CustomerVO();
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
    public class IdCardVO
    {
        [DataMember]
        public String IdCard_A { get; set; }
        [DataMember]
        public String IdCard_B { get; set; }
    }
}