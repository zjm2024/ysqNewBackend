using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class CustomerViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CustomerViewVO));

        [DataMember]
        public Int32 CustomerId
        {
            get
            {
                return (Int32)GetValue(typeof(Int32), "CustomerId");
            }
            set
            {
                SetValue("CustomerId", value);
            }
        }
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
        public String SexName { get { return (String)GetValue(typeof(String), "SexName"); } set { SetValue("SexName", value); } }
        [DataMember]
        public String StatusName { get { return (String)GetValue(typeof(String), "StatusName"); } set { SetValue("StatusName", value); } }
        [DataMember]
        public Int32 BusinessId { get { return (Int32)GetValue(typeof(Int32), "BusinessId"); } set { SetValue("BusinessId", value); } }
        [DataMember]
        public Int32 BusinessStatus { get { return (Int32)GetValue(typeof(Int32), "BusinessStatus"); } set { SetValue("BusinessStatus", value); } }
        [DataMember]
        public Int32 BusinessRealNameStatus { get { return (Int32)GetValue(typeof(Int32), "BusinessRealNameStatus"); } set { SetValue("BusinessRealNameStatus", value); } }
        [DataMember]
        public Int32 AgencyId { get { return (Int32)GetValue(typeof(Int32), "AgencyId"); } set { SetValue("AgencyId", value); } }
        [DataMember]
        public Int32 AgencyStatus { get { return (Int32)GetValue(typeof(Int32), "AgencyStatus"); } set { SetValue("AgencyStatus", value); } }
        [DataMember]
        public Int32 AgencyRealNameStatus { get { return (Int32)GetValue(typeof(Int32), "AgencyRealNameStatus"); } set { SetValue("AgencyRealNameStatus", value); } }
        [DataMember]
        public Int32 InvitationCustomerID { get { return (Int32)GetValue(typeof(Int32), "InvitationCustomerID"); } set { SetValue("InvitationCustomerID", value); } }
        [DataMember]
        public Int32 originCustomerId { get { return (Int32)GetValue(typeof(Int32), "originCustomerId"); } set { SetValue("originCustomerId", value); } }
        [DataMember]
        public String AgencyName { get { return (String)GetValue(typeof(String), "AgencyName"); } set { SetValue("AgencyName", value); } }
        [DataMember]
        public String CompanyName { get { return (String)GetValue(typeof(String), "CompanyName"); } set { SetValue("CompanyName", value); } }
        [DataMember]
        public Boolean Agent { get { return (Boolean)GetValue(typeof(Boolean), "Agent"); } set { SetValue("Agent", value); } }
        [DataMember]
        public Boolean isAdmin { get { return (Boolean)GetValue(typeof(Boolean), "isAdmin"); } set { SetValue("isAdmin", value); } }
        [DataMember]
        public Boolean isVip { get { return (Boolean)GetValue(typeof(Boolean), "isVip"); } set { SetValue("isVip", value); } }
        [DataMember]
        public DateTime ExpirationAt { get { return (DateTime)GetValue(typeof(DateTime), "ExpirationAt"); } set { SetValue("ExpirationAt", value); } }
        [DataMember]
        public Boolean Leliao { get { return (Boolean)GetValue(typeof(Boolean), "Leliao"); } set { SetValue("Leliao", value); } }
        [DataMember]
        public Boolean BusinessCard { get { return (Boolean)GetValue(typeof(Boolean), "BusinessCard"); } set { SetValue("BusinessCard", value); } }
        [DataMember]
        public Boolean Party { get { return (Boolean)GetValue(typeof(Boolean), "Party"); } set { SetValue("Party", value); } }
        [DataMember]
        public Int32 VipLevel { get { return (Int32)GetValue(typeof(Int32), "VipLevel"); } set { SetValue("VipLevel", value); } }

        [DataMember]
        public Boolean isIdCard { get { return (Boolean)GetValue(typeof(Boolean), "isIdCard"); } set { SetValue("isIdCard", value); } }
        [DataMember]
        public String IdCard_A { get { return (String)GetValue(typeof(String), "IdCard_A"); } set { SetValue("IdCard_A", value); } }
        [DataMember]
        public String IdCard_B { get { return (String)GetValue(typeof(String), "IdCard_B"); } set { SetValue("IdCard_B", value); } }

        [DataMember]
        public String originType { get { return (String)GetValue(typeof(String), "originType"); } set { SetValue("originType", value); } }
        [DataMember]
        public Int32 originID { get { return (Int32)GetValue(typeof(Int32), "originID"); } set { SetValue("originID", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CustomerViewVO tmp = new CustomerViewVO();
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