using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.RequireManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class RequirementViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(RequirementViewVO));

        [DataMember]
        public Int32 RequirementId { get { return (Int32)GetValue(typeof(Int32), "RequirementId"); } set { SetValue("RequirementId", value); } }
        [DataMember]
        public Int32 CityId { get { return (Int32)GetValue(typeof(Int32), "CityId"); } set { SetValue("CityId", value); } }
        [DataMember]
        public Int32 CategoryId { get { return (Int32)GetValue(typeof(Int32), "CategoryId"); } set { SetValue("CategoryId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 BusinessId { get { return (Int32)GetValue(typeof(Int32), "BusinessId"); } set { SetValue("BusinessId", value); } }
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
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
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
        public String CityName { get { return (String)GetValue(typeof(String), "CityName"); } set { SetValue("CityName", value); } }
        [DataMember]
        public String CategoryName { get { return (String)GetValue(typeof(String), "CategoryName"); } set { SetValue("CategoryName", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public Int32 ProvinceId { get { return (Int32)GetValue(typeof(Int32), "ProvinceId"); } set { SetValue("ProvinceId", value); } }
        [DataMember]
        public Int32 ParentCategoryId { get { return (Int32)GetValue(typeof(Int32), "ParentCategoryId"); } set { SetValue("ParentCategoryId", value); } }
        [DataMember]
        public Int32 RemainDate { get { return (Int32)GetValue(typeof(Int32), "RemainDate"); } set { SetValue("RemainDate", value); } }
        [DataMember]
        public Int32 TenderInviteCount { get { return (Int32)GetValue(typeof(Int32), "TenderInviteCount"); } set { SetValue("TenderInviteCount", value); } }
        [DataMember]
        public Int32 AgencyTenderCount { get { return (Int32)GetValue(typeof(Int32), "AgencyTenderCount"); } set { SetValue("AgencyTenderCount", value); } }
        [DataMember]
        public String ClientName { get { return (String)GetValue(typeof(String), "ClientName"); } set { SetValue("ClientName", value); } }
        [DataMember]
        public String TargetAgency { get { return (String)GetValue(typeof(String), "TargetAgency"); } set { SetValue("TargetAgency", value); } }
        [DataMember]
        public String ProductDescription { get { return (String)GetValue(typeof(String), "ProductDescription"); } set { SetValue("ProductDescription", value); } }
        [DataMember]
        public String CompanyDescription { get { return (String)GetValue(typeof(String), "CompanyDescription"); } set { SetValue("CompanyDescription", value); } }
        [DataMember]
        public String AgencyCondition { get { return (String)GetValue(typeof(String), "AgencyCondition"); } set { SetValue("AgencyCondition", value); } }
        [DataMember]
        public String ContactPerson { get { return (String)GetValue(typeof(String), "ContactPerson"); } set { SetValue("ContactPerson", value); } }
        [DataMember]
        public String ContactPhone { get { return (String)GetValue(typeof(String), "ContactPhone"); } set { SetValue("ContactPhone", value); } }
        [DataMember]
        public String CompanyLogo { get { return (String)GetValue(typeof(String), "CompanyLogo"); } set { SetValue("CompanyLogo", value); } }
        [DataMember]
        public String CompanyName { get { return (String)GetValue(typeof(String), "CompanyName"); } set { SetValue("CompanyName", value); } }
        [DataMember]
        public Int32 ProjectStatus { get { return (Int32)GetValue(typeof(Int32), "ProjectStatus"); } set { SetValue("ProjectStatus", value); } }
        [DataMember]
        public Decimal BusinessReviewScore { get { return (Decimal)GetValue(typeof(Decimal), "BusinessReviewScore"); } set { SetValue("BusinessReviewScore", value); } }
        [DataMember]
        public Decimal CXDReviewScore { get { return (Decimal)GetValue(typeof(Decimal), "CXDReviewScore"); } set { SetValue("CXDReviewScore", value); } }
        [DataMember]
        public Decimal JSXReviewScore { get { return (Decimal)GetValue(typeof(Decimal), "JSXReviewScore"); } set { SetValue("JSXReviewScore", value); } }
        [DataMember]
        public String TargetCity { get { return (String)GetValue(typeof(String), "TargetCity"); } set { SetValue("TargetCity", value); } }
        [DataMember]
        public String TargetCategory { get { return (String)GetValue(typeof(String), "TargetCategory"); } set { SetValue("TargetCategory", value); } }
        [DataMember]
        public String TargetClient { get { return (String)GetValue(typeof(String), "TargetClient"); } set { SetValue("TargetClient", value); } }
        [DataMember]
        public Decimal DelegationCommission { get { return (Decimal)GetValue(typeof(Decimal), "DelegationCommission"); } set { SetValue("DelegationCommission", value); } }
        [DataMember]
        public String CommissionDescription { get { return (String)GetValue(typeof(String), "CommissionDescription"); } set { SetValue("CommissionDescription", value); } }
        [DataMember]
        public Int32 agencySum { get { return (Int32)GetValue(typeof(Int32), "agencySum"); } set { SetValue("agencySum", value); } }
        [DataMember]
        public Int32 agencyCount { get { return (Int32)GetValue(typeof(Int32), "agencyCount"); } set { SetValue("agencyCount", value); } }
        [DataMember]
        public String QRCodeImg { get { return (String)GetValue(typeof(String), "QRCodeImg"); } set { SetValue("QRCodeImg", value); } }
        [DataMember]
        public Int32 ReadCount { get { return (Int32)GetValue(typeof(Int32), "ReadCount"); } set { SetValue("ReadCount", value); } }
        [DataMember]
        public Int32 MarkCounts { get { return (Int32)GetValue(typeof(Int32), "MarkCounts"); } set { SetValue("MarkCounts", value); } }
        [DataMember]
        public Int32 Authentication { get { return (Int32)GetValue(typeof(Int32), "Authentication"); } set { SetValue("Authentication", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            RequirementViewVO tmp = new RequirementViewVO();
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