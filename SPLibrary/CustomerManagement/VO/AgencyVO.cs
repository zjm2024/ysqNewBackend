using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class AgencyVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencyVO));

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
        public String FamiliarProduct { get { return (String)GetValue(typeof(String), "FamiliarProduct"); } set { SetValue("FamiliarProduct", value); } }

        [DataMember]
        public String FamiliarClient { get { return (String)GetValue(typeof(String), "FamiliarClient"); } set { SetValue("FamiliarClient", value); } }

        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Int32 RealNameStatus { get { return (Int32)GetValue(typeof(Int32), "RealNameStatus"); } set { SetValue("RealNameStatus", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
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
        public String QRCodeImg { get { return (String)GetValue(typeof(String), "QRCodeImg"); } set { SetValue("QRCodeImg", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            AgencyVO tmp = new AgencyVO();
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