using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.RequireManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class DemandViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(DemandViewVO));

        [DataMember]
        public Int32 DemandId { get { return (Int32)GetValue(typeof(Int32), "DemandId"); } set { SetValue("DemandId", value); } }
        [DataMember]
        public Int32 CategoryId { get { return (Int32)GetValue(typeof(Int32), "CategoryId"); } set { SetValue("CategoryId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public DateTime EffectiveEndDate { get { return (DateTime)GetValue(typeof(DateTime), "EffectiveEndDate"); } set { SetValue("EffectiveEndDate", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String CategoryName { get { return (String)GetValue(typeof(String), "CategoryName"); } set { SetValue("CategoryName", value); } }
        [DataMember]
        public Int32 CategoryStatus { get { return (Int32)GetValue(typeof(Int32), "CategoryStatus"); } set { SetValue("CategoryStatus", value); } }
        [DataMember]
        public Int32 OfferCount { get { return (Int32)GetValue(typeof(Int32), "OfferCount"); } set { SetValue("OfferCount", value); } }

        [DataMember]
        public String HeaderLogo { get { return (String)GetValue(typeof(String), "HeaderLogo"); } set { SetValue("HeaderLogo", value); } }
        [DataMember]
        public String CustomerCode { get { return (String)GetValue(typeof(String), "CustomerCode"); } set { SetValue("CustomerCode", value); } }
        [DataMember]
        public String CustomerAccount { get { return (String)GetValue(typeof(String), "CustomerAccount"); } set { SetValue("CustomerAccount", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public Boolean Sex { get { return (Boolean)GetValue(typeof(Boolean), "Sex"); } set { SetValue("Sex", value); } }
        [DataMember]
        public DateTime Birthday { get { return (DateTime)GetValue(typeof(DateTime), "Birthday"); } set { SetValue("Birthday", value); } }
        [DataMember]
        public String Email { get { return (String)GetValue(typeof(String), "Email"); } set { SetValue("Email", value); } }
        [DataMember]
        public Int32 CustomerStatus { get { return (Int32)GetValue(typeof(Int32), "CustomerStatus"); } set { SetValue("CustomerStatus", value); } }
        [DataMember]
        public String SexName { get { return (String)GetValue(typeof(String), "SexName"); } set { SetValue("SexName", value); } }
        [DataMember]
        public String StatusName { get { return (String)GetValue(typeof(String), "StatusName"); } set { SetValue("StatusName", value); } }
        [DataMember]
        public Int32 BusinessId { get { return (Int32)GetValue(typeof(Int32), "BusinessId"); } set { SetValue("BusinessId", value); } }
        [DataMember]
        public Int32 BusinessStatus { get { return (Int32)GetValue(typeof(Int32), "BusinessStatus"); } set { SetValue("BusinessStatus", value); } }
        [DataMember]
        public Int32 AgencyId { get { return (Int32)GetValue(typeof(Int32), "AgencyId"); } set { SetValue("AgencyId", value); } }
        [DataMember]
        public Int32 AgencyStatus { get { return (Int32)GetValue(typeof(Int32), "AgencyStatus"); } set { SetValue("AgencyStatus", value); } }

        public Int32 InvitationCustomerID
        {
            get { return (Int32)GetValue(typeof(Int32), "InvitationCustomerID"); }
            set { SetValue("InvitationCustomerID", value); }
        }

        #region ICloneable Member
        public override object Clone()
        {
            DemandViewVO tmp = new DemandViewVO();
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
