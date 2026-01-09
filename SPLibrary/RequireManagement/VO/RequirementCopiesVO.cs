using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.RequireManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class RequirementCopiesVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(RequirementCopiesVO));

        [DataMember]
        public Int32 RequirementCopiesID { get { return (Int32)GetValue(typeof(Int32), "RequirementCopiesID"); } set { SetValue("RequirementCopiesID", value); } }
        [DataMember]
        public Int32 RequirementId { get { return (Int32)GetValue(typeof(Int32), "RequirementId"); } set { SetValue("RequirementId", value); } }
        [DataMember]
        public Int32 ProjectId { get { return (Int32)GetValue(typeof(Int32), "ProjectId"); } set { SetValue("ProjectId", value); } }
        [DataMember]
        public Int32 CityId { get { return (Int32)GetValue(typeof(Int32), "CityId"); } set { SetValue("CityId", value); } }
        [DataMember]
        public Int32 CategoryId { get { return (Int32)GetValue(typeof(Int32), "CategoryId"); } set { SetValue("CategoryId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public String RequirementCode { get { return (String)GetValue(typeof(String), "RequirementCode"); } set { SetValue("RequirementCode", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String MainImg { get { return (String)GetValue(typeof(String), "MainImg"); } set { SetValue("MainImg", value); } }
        [DataMember]
        public Int32 CommissionType { get { return (Int32)GetValue(typeof(Int32), "CommissionType"); } set { SetValue("CommissionType", value); } }
        [DataMember]
        public Decimal Commission { get { return (Decimal)GetValue(typeof(Decimal), "Commission"); } set { SetValue("Commission", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public DateTime PublishAt { get { return (DateTime)GetValue(typeof(DateTime), "PublishAt"); } set { SetValue("PublishAt", value); } }
        [DataMember]
        public String TargetAgency { get { return (String)GetValue(typeof(String), "TargetAgency"); } set { SetValue("TargetAgency", value); } }
        [DataMember]
        public String AgencyCondition { get { return (String)GetValue(typeof(String), "AgencyCondition"); } set { SetValue("AgencyCondition", value); } }
        [DataMember]
        public String ContactPerson { get { return (String)GetValue(typeof(String), "ContactPerson"); } set { SetValue("ContactPerson", value); } }
        [DataMember]
        public String ContactPhone { get { return (String)GetValue(typeof(String), "ContactPhone"); } set { SetValue("ContactPhone", value); } }
        [DataMember]
        public String CommissionDescription { get { return (String)GetValue(typeof(String), "CommissionDescription"); } set { SetValue("CommissionDescription", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            RequirementCopiesVO tmp = new RequirementCopiesVO();
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