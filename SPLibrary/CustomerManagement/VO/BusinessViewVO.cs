using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class BusinessViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BusinessViewVO));

        [DataMember]
        public Int32 BusinessId { get { return (Int32)GetValue(typeof(Int32), "BusinessId"); } set { SetValue("BusinessId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 CityId { get { return (Int32)GetValue(typeof(Int32), "CityId"); } set { SetValue("CityId", value); } }
        [DataMember]
        public String CompanyName { get { return (String)GetValue(typeof(String), "CompanyName"); } set { SetValue("CompanyName", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public DateTime SetupDate { get { return (DateTime)GetValue(typeof(DateTime), "SetupDate"); } set { SetValue("SetupDate", value); } }
        [DataMember]
        public String CompanyType { get { return (String)GetValue(typeof(String), "CompanyType"); } set { SetValue("CompanyType", value); } }
        [DataMember]
        public String BusinessLicense { get { return (String)GetValue(typeof(String), "BusinessLicense"); } set { SetValue("BusinessLicense", value); } }
        [DataMember]
        public String BusinessLicenseImg { get { return (String)GetValue(typeof(String), "BusinessLicenseImg"); } set { SetValue("BusinessLicenseImg", value); } }
        [DataMember]
        public String CompanyLogo { get { return (String)GetValue(typeof(String), "CompanyLogo"); } set { SetValue("CompanyLogo", value); } }
        [DataMember]
        public String MainProducts { get { return (String)GetValue(typeof(String), "MainProducts"); } set { SetValue("MainProducts", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Int32 RealNameStatus { get { return (Int32)GetValue(typeof(Int32), "RealNameStatus"); } set { SetValue("RealNameStatus", value); } }
        [DataMember]
        public String EntrustImgPath { get { return (String)GetValue(typeof(String), "EntrustImgPath"); } set { SetValue("EntrustImgPath", value); } }
        [DataMember]
        public String ContactPersonImgPath { get { return (String)GetValue(typeof(String), "ContactPersonImgPath"); } set { SetValue("ContactPersonImgPath", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String CustomerCode { get { return (String)GetValue(typeof(String), "CustomerCode"); } set { SetValue("CustomerCode", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String CityName { get { return (String)GetValue(typeof(String), "CityName"); } set { SetValue("CityName", value); } }
        [DataMember]
        public Int32 ProvinceId { get { return (Int32)GetValue(typeof(Int32), "ProvinceId"); } set { SetValue("ProvinceId", value); } }
        [DataMember]
        public String CategoryIds { get { return (String)GetValue(typeof(String), "CategoryIds"); } set { SetValue("CategoryIds", value); } }
        [DataMember]
        public Int32 ProjectCount { get { return (Int32)GetValue(typeof(Int32), "ProjectCount"); } set { SetValue("ProjectCount", value); } }
        [DataMember]
        public Decimal ReviewScore { get { return (Decimal)GetValue(typeof(Decimal), "ReviewScore"); } set { SetValue("ReviewScore", value); } }
        [DataMember]
        public String CategoryNames { get { return (String)GetValue(typeof(String), "CategoryNames"); } set { SetValue("CategoryNames", value); } }
        [DataMember]
        public String TargetCategory { get { return (String)GetValue(typeof(String), "TargetCategory"); } set { SetValue("TargetCategory", value); } }
        [DataMember]
        public String TargetCity { get { return (String)GetValue(typeof(String), "TargetCity"); } set { SetValue("TargetCity", value); } }
        [DataMember]
        public String TargetClient { get { return (String)GetValue(typeof(String), "TargetClient"); } set { SetValue("TargetClient", value); } }
        [DataMember]
        public String ProductDescription { get { return (String)GetValue(typeof(String), "ProductDescription"); } set { SetValue("ProductDescription", value); } }
        [DataMember]
        public String CompanyDescription { get { return (String)GetValue(typeof(String), "CompanyDescription"); } set { SetValue("CompanyDescription", value); } }
        [DataMember]
        public String Address { get { return (String)GetValue(typeof(String), "Address"); } set { SetValue("Address", value); } }
        [DataMember]
        public String CompanySite { get { return (String)GetValue(typeof(String), "CompanySite"); } set { SetValue("CompanySite", value); } }
        [DataMember]
        public String CompanyTel { get { return (String)GetValue(typeof(String), "CompanyTel"); } set { SetValue("CompanyTel", value); } }
        [DataMember]
        public Decimal CXDReviewScore { get { return (Decimal)GetValue(typeof(Decimal), "CXDReviewScore"); } set { SetValue("CXDReviewScore", value); } }
        [DataMember]
        public Decimal JSXReviewScore { get { return (Decimal)GetValue(typeof(Decimal), "JSXReviewScore"); } set { SetValue("JSXReviewScore", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            BusinessViewVO tmp = new BusinessViewVO();
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