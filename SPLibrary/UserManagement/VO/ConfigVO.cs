using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class ConfigVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ConfigVO));

        [DataMember]
        public Int32 ConfigId { get { return (Int32)GetValue(typeof(Int32), "ConfigId"); } set { SetValue("ConfigId", value); } }
        [DataMember]
        public Int32 CompanyId { get { return (Int32)GetValue(typeof(Int32), "CompanyId"); } set { SetValue("CompanyId", value); } }
        [DataMember]
        public String MessageAPI { get { return (String)GetValue(typeof(String), "MessageAPI"); } set { SetValue("MessageAPI", value); } }
        [DataMember]
        public Int32 MessageNotiCount { get { return (Int32)GetValue(typeof(Int32), "MessageNotiCount"); } set { SetValue("MessageNotiCount", value); } }
        [DataMember]
        public String MessageNotiPhone { get { return (String)GetValue(typeof(String), "MessageNotiPhone"); } set { SetValue("MessageNotiPhone", value); } }
        [DataMember]
        public Boolean IsNeedApprove { get { return (Boolean)GetValue(typeof(Boolean), "IsNeedApprove"); } set { SetValue("IsNeedApprove", value); } }
        [DataMember]
        public Decimal CommissionPercentage { get { return (Decimal)GetValue(typeof(Decimal), "CommissionPercentage"); } set { SetValue("CommissionPercentage", value); } }
        [DataMember]
        public Decimal CommissionTotal { get { return (Decimal)GetValue(typeof(Decimal), "CommissionTotal"); } set { SetValue("CommissionTotal", value); } }
        [DataMember]
        public Int32 ProjectTotal { get { return (Int32)GetValue(typeof(Int32), "ProjectTotal"); } set { SetValue("ProjectTotal", value); } }
        [DataMember]
        public Boolean IsCommissionTotalShow { get { return (Boolean)GetValue(typeof(Boolean), "IsCommissionTotalShow"); } set { SetValue("IsCommissionTotalShow", value); } }
        [DataMember]
        public Boolean IsProjectTotalShow { get { return (Boolean)GetValue(typeof(Boolean), "IsProjectTotalShow"); } set { SetValue("IsProjectTotalShow", value); } }
        [DataMember]
        public String HeaderPic { get { return (String)GetValue(typeof(String), "HeaderPic"); } set { SetValue("HeaderPic", value); } }
        [DataMember]
        public String SiteName { get { return (String)GetValue(typeof(String), "SiteName"); } set { SetValue("SiteName", value); } }
        [DataMember]
        public String SiteDescription { get { return (String)GetValue(typeof(String), "SiteDescription"); } set { SetValue("SiteDescription", value); } }
        [DataMember]
        public String APPImage { get { return (String)GetValue(typeof(String), "APPImage"); } set { SetValue("APPImage", value); } }
        [DataMember]
        public String GZImage { get { return (String)GetValue(typeof(String), "GZImage"); } set { SetValue("GZImage", value); } }
        [DataMember]
        public String ServicePhone { get { return (String)GetValue(typeof(String), "ServicePhone"); } set { SetValue("ServicePhone", value); } }
        [DataMember]
        public String ServiceNote { get { return (String)GetValue(typeof(String), "ServiceNote"); } set { SetValue("ServiceNote", value); } }
        [DataMember]
        public String IOSAPPURL { get { return (String)GetValue(typeof(String), "IOSAPPURL"); } set { SetValue("IOSAPPURL", value); } }
        [DataMember]
        public String ContractNote { get { return (String)GetValue(typeof(String), "ContractNote"); } set { SetValue("ContractNote", value); } }
        [DataMember]
        public Int32 ViewAgencyCount { get { return (Int32)GetValue(typeof(Int32), "ViewAgencyCount"); } set { SetValue("ViewAgencyCount", value); } }
        [DataMember]
        public Int32 ViewBusinessCount { get { return (Int32)GetValue(typeof(Int32), "ViewBusinessCount"); } set { SetValue("ViewBusinessCount", value); } }

        [DataMember]
        public String zxbRegistered_text { get { return (String)GetValue(typeof(String), "zxbRegistered_text"); } set { SetValue("zxbRegistered_text", value); } }
        [DataMember]
        public Decimal zxbRegistered { get { return (Decimal)GetValue(typeof(Decimal), "zxbRegistered"); } set { SetValue("zxbRegistered", value); } }
        [DataMember]
        public String zxbCertification_text { get { return (String)GetValue(typeof(String), "zxbCertification_text"); } set { SetValue("zxbCertification_text", value); } }
        [DataMember]
        public Decimal zxbCertification { get { return (Decimal)GetValue(typeof(Decimal), "zxbCertification"); } set { SetValue("zxbCertification", value); } }
        [DataMember]
        public String zxbReleaseTheTask_text { get { return (String)GetValue(typeof(String), "zxbReleaseTheTask_text"); } set { SetValue("zxbReleaseTheTask_text", value); } }
        [DataMember]
        public Decimal zxbReleaseTheTask { get { return (Decimal)GetValue(typeof(Decimal), "zxbReleaseTheTask"); } set { SetValue("zxbReleaseTheTask", value); } }
        [DataMember]
        public String zxbHosting_text { get { return (String)GetValue(typeof(String), "zxbHosting_text"); } set { SetValue("zxbHosting_text", value); } }
        [DataMember]
        public Decimal zxbHosting { get { return (Decimal)GetValue(typeof(Decimal), "zxbHosting"); } set { SetValue("zxbHosting", value); } }
        [DataMember]
        public String zxbShare_text { get { return (String)GetValue(typeof(String), "zxbShare_text"); } set { SetValue("zxbShare_text", value); } }
        [DataMember]
        public Decimal zxbShare { get { return (Decimal)GetValue(typeof(Decimal), "zxbShare"); } set { SetValue("zxbShare", value); } }
        [DataMember]
        public String zxbReview_text { get { return (String)GetValue(typeof(String), "zxbReview_text"); } set { SetValue("zxbReview_text", value); } }
        [DataMember]
        public Decimal zxbReview { get { return (Decimal)GetValue(typeof(Decimal), "zxbReview"); } set { SetValue("zxbReview", value); } }
        [DataMember]
        public String zxbNote { get { return (String)GetValue(typeof(String), "zxbNote"); } set { SetValue("zxbNote", value); } }
        [DataMember]
        public Decimal FirstMandates { get { return (Decimal)GetValue(typeof(Decimal), "FirstMandates"); } set { SetValue("FirstMandates", value); } }
        [DataMember]
        public Int32 CompanyPartyID { get { return (Int32)GetValue(typeof(Int32), "CompanyPartyID"); } set { SetValue("CompanyPartyID", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            ConfigVO tmp = new ConfigVO();
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