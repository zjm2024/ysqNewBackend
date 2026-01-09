using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class AgencyViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencyViewVO));

        [DataMember]
        public Int32 AgencyId { get { return (Int32)GetValue(typeof(Int32), "AgencyId"); } set { SetValue("AgencyId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 CityId { get { return (Int32)GetValue(typeof(Int32), "CityId"); } set { SetValue("CityId", value); } }
        [DataMember]
        public String AgencyName { get { return (String)GetValue(typeof(String), "AgencyName"); } set { SetValue("AgencyName", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public String IDCard { get { return (String)GetValue(typeof(String), "IDCard"); } set { SetValue("IDCard", value); } }
        [DataMember]
        public String Technical { get { return (String)GetValue(typeof(String), "Technical"); } set { SetValue("Technical", value); } }
        [DataMember]
        public String ProjectExperience { get { return (String)GetValue(typeof(String), "ProjectExperience"); } set { SetValue("ProjectExperience", value); } }
        [DataMember]
        public String AgencyLevel { get { return (String)GetValue(typeof(String), "AgencyLevel"); } set { SetValue("AgencyLevel", value); } }
        [DataMember]
        public String ContactsResources { get { return (String)GetValue(typeof(String), "ContactsResources"); } set { SetValue("ContactsResources", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Int32 RealNameStatus { get { return (Int32)GetValue(typeof(Int32), "RealNameStatus"); } set { SetValue("RealNameStatus", value); } }
        [DataMember]
        public String FamiliarProduct { get { return (String)GetValue(typeof(String), "FamiliarProduct"); } set { SetValue("FamiliarProduct", value); } }

        [DataMember]
        public String FamiliarClient { get { return (String)GetValue(typeof(String), "FamiliarClient"); } set { SetValue("FamiliarClient", value); } }


        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String CustomerCode { get { return (String)GetValue(typeof(String), "CustomerCode"); } set { SetValue("CustomerCode", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public Boolean Sex { get { return (Boolean)GetValue(typeof(Boolean), "Sex"); } set { SetValue("Sex", value); } }
        [DataMember]
        public DateTime Birthday { get { return (DateTime)GetValue(typeof(DateTime), "Birthday"); } set { SetValue("Birthday", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String CityName { get { return (String)GetValue(typeof(String), "CityName"); } set { SetValue("CityName", value); } }
        [DataMember]
        public Int32 ProvinceId { get { return (Int32)GetValue(typeof(Int32), "ProvinceId"); } set { SetValue("ProvinceId", value); } }
        [DataMember]
        public String CategoryIds { get { return (String)GetValue(typeof(String), "CategoryIds"); } set { SetValue("CategoryIds", value); } }
        [DataMember]
        public String ParentCategoryIds { get { return (String)GetValue(typeof(String), "ParentCategoryIds"); } set { SetValue("ParentCategoryIds", value); } }
        [DataMember]
        public Int32 ProjectCount { get { return (Int32)GetValue(typeof(Int32), "ProjectCount"); } set { SetValue("ProjectCount", value); } }
        [DataMember]
        public Decimal ReviewScore { get { return (Decimal)GetValue(typeof(Decimal), "ReviewScore"); } set { SetValue("ReviewScore", value); } }
        [DataMember]
        public String HeaderLogo { get { return (String)GetValue(typeof(String), "HeaderLogo"); } set { SetValue("HeaderLogo", value); } }
        [DataMember]
        public Decimal ProjectCost { get { return (Decimal)GetValue(typeof(Decimal), "ProjectCost"); } set { SetValue("ProjectCost", value); } }
        [DataMember]
        public Decimal ProjectCommission { get { return (Decimal)GetValue(typeof(Decimal), "ProjectCommission"); } set { SetValue("ProjectCommission", value); } }
        [DataMember]
        public String TargetCategory { get { return (String)GetValue(typeof(String), "TargetCategory"); } set { SetValue("TargetCategory", value); } }
        [DataMember]
        public String TargetClient { get { return (String)GetValue(typeof(String), "TargetClient"); } set { SetValue("TargetClient", value); } }
        [DataMember]
        public String TargetSolution { get { return (String)GetValue(typeof(String), "TargetSolution"); } set { SetValue("TargetSolution", value); } }
        [DataMember]
        public String Specialty { get { return (String)GetValue(typeof(String), "Specialty"); } set { SetValue("Specialty", value); } }
        [DataMember]
        public String Feature { get { return (String)GetValue(typeof(String), "Feature"); } set { SetValue("Feature", value); } }
        [DataMember]
        public String ShortDescription { get { return (String)GetValue(typeof(String), "ShortDescription"); } set { SetValue("ShortDescription", value); } }
        [DataMember]
        public String Company { get { return (String)GetValue(typeof(String), "Company"); } set { SetValue("Company", value); } }
        [DataMember]
        public String Position { get { return (String)GetValue(typeof(String), "Position"); } set { SetValue("Position", value); } }
        [DataMember]
        public String PersonalCard { get { return (String)GetValue(typeof(String), "PersonalCard"); } set { SetValue("PersonalCard", value); } }

        [DataMember]
        public String School { get { return (String)GetValue(typeof(String), "School"); } set { SetValue("School", value); } }
        [DataMember]
        public String TargetCity { get { return (String)GetValue(typeof(String), "TargetCity"); } set { SetValue("TargetCity", value); } }
        [DataMember]
        public String QRCodeImg { get { return (String)GetValue(typeof(String), "QRCodeImg"); } set { SetValue("QRCodeImg", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            AgencyViewVO tmp = new AgencyViewVO();
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