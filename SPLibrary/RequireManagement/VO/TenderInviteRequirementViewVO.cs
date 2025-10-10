using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.RequireManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class TenderInviteRequirementViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(TenderInviteRequirementViewVO));

        [DataMember]
        public Int32 TenderInviteId { get { return (Int32)GetValue(typeof(Int32), "TenderInviteId"); } set { SetValue("TenderInviteId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public DateTime InviteDate { get { return (DateTime)GetValue(typeof(DateTime), "InviteDate"); } set { SetValue("InviteDate", value); } }
        [DataMember]
        public String TenderCustomerName { get { return (String)GetValue(typeof(String), "TenderCustomerName"); } set { SetValue("TenderCustomerName", value); } }
        [DataMember]
        public String AgencyLevel { get { return (String)GetValue(typeof(String), "AgencyLevel"); } set { SetValue("AgencyLevel", value); } }
        [DataMember]
        public String AgencyName { get { return (String)GetValue(typeof(String), "AgencyName"); } set { SetValue("AgencyName", value); } }
        [DataMember]
        public Int32 RequirementId { get { return (Int32)GetValue(typeof(Int32), "RequirementId"); } set { SetValue("RequirementId", value); } }
        [DataMember]
        public Int32 CityId { get { return (Int32)GetValue(typeof(Int32), "CityId"); } set { SetValue("CityId", value); } }
        [DataMember]
        public Int32 CategoryId { get { return (Int32)GetValue(typeof(Int32), "CategoryId"); } set { SetValue("CategoryId", value); } }
        [DataMember]
        public Int32 RequirementCustomerId { get { return (Int32)GetValue(typeof(Int32), "RequirementCustomerId"); } set { SetValue("RequirementCustomerId", value); } }
        [DataMember]
        public String RequirementCode { get { return (String)GetValue(typeof(String), "RequirementCode"); } set { SetValue("RequirementCode", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String MainImg { get { return (String)GetValue(typeof(String), "MainImg"); } set { SetValue("MainImg", value); } }
        [DataMember]
        public DateTime EffectiveEndDate { get { return (DateTime)GetValue(typeof(DateTime), "EffectiveEndDate"); } set { SetValue("EffectiveEndDate", value); } }
        [DataMember]
        public Int32 CommissionType { get { return (Int32)GetValue(typeof(Int32), "CommissionType"); } set { SetValue("CommissionType", value); } }
        [DataMember]
        public Decimal Commission { get { return (Decimal)GetValue(typeof(Decimal), "Commission"); } set { SetValue("Commission", value); } }
        [DataMember]
        public Int32 TenderCount { get { return (Int32)GetValue(typeof(Int32), "TenderCount"); } set { SetValue("TenderCount", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public DateTime PublishAt { get { return (DateTime)GetValue(typeof(DateTime), "PublishAt"); } set { SetValue("PublishAt", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public String CityName { get { return (String)GetValue(typeof(String), "CityName"); } set { SetValue("CityName", value); } }
        [DataMember]
        public String CategoryName { get { return (String)GetValue(typeof(String), "CategoryName"); } set { SetValue("CategoryName", value); } }
        [DataMember]
        public String RequirementCustomerName { get { return (String)GetValue(typeof(String), "RequirementCustomerName"); } set { SetValue("RequirementCustomerName", value); } }
        [DataMember]
        public Int32 BusinessId { get { return (Int32)GetValue(typeof(Int32), "BusinessId"); } set { SetValue("BusinessId", value); } }
        [DataMember]
        public Int32 ProvinceId { get { return (Int32)GetValue(typeof(Int32), "ProvinceId"); } set { SetValue("ProvinceId", value); } }
        [DataMember]
        public Int32 ParentCategoryId { get { return (Int32)GetValue(typeof(Int32), "ParentCategoryId"); } set { SetValue("ParentCategoryId", value); } }
        [DataMember]
        public Int32 RemainDate { get { return (Int32)GetValue(typeof(Int32), "RemainDate"); } set { SetValue("RemainDate", value); } }
        [DataMember]
        public Int32 RemainTenderCount { get { return (Int32)GetValue(typeof(Int32), "RemainTenderCount"); } set { SetValue("RemainTenderCount", value); } }
        [DataMember]
        public Int32 AgencyTenderCount { get { return (Int32)GetValue(typeof(Int32), "AgencyTenderCount"); } set { SetValue("AgencyTenderCount", value); } }
        [DataMember]
        public String ClientName { get { return (String)GetValue(typeof(String), "ClientName"); } set { SetValue("ClientName", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            TenderInviteRequirementViewVO tmp = new TenderInviteRequirementViewVO();
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