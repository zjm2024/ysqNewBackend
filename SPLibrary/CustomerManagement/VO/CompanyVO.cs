using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CompanyVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CompanyVO));

        [DataMember]
        public Int32 CompanyID { get { return (Int32)GetValue(typeof(Int32), "CompanyID"); } set { SetValue("CompanyID", value); } }
        [DataMember]
        public String CompanyName { get { return (String)GetValue(typeof(String), "CompanyName"); } set { SetValue("CompanyName", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public String CompanyType { get { return (String)GetValue(typeof(String), "CompanyType"); } set { SetValue("CompanyType", value); } }
        [DataMember]
        public String Location { get { return (String)GetValue(typeof(String), "Location"); } set { SetValue("Location", value); } }
        [DataMember]
        public String CompanySize { get { return (String)GetValue(typeof(String), "CompanySize"); } set { SetValue("CompanySize", value); } }
        [DataMember]
        public String RegisteredCapital { get { return (String)GetValue(typeof(String), "RegisteredCapital"); } set { SetValue("RegisteredCapital", value); } }
        [DataMember]
        public String YearOfRegistration { get { return (String)GetValue(typeof(String), "YearOfRegistration"); } set { SetValue("YearOfRegistration", value); } }
        [DataMember]
        public String Address { get { return (String)GetValue(typeof(String), "Address"); } set { SetValue("Address", value); } }
        [DataMember]
        public String PostalCode { get { return (String)GetValue(typeof(String), "PostalCode"); } set { SetValue("PostalCode", value); } }
        [DataMember]
        public String Tel { get { return (String)GetValue(typeof(String), "Tel"); } set { SetValue("Tel", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String Contacts { get { return (String)GetValue(typeof(String), "Contacts"); } set { SetValue("Contacts", value); } }
        [DataMember]
        public Decimal latitude { get { return (Decimal)GetValue(typeof(Decimal), "latitude"); } set { SetValue("latitude", value); } }
        [DataMember]
        public Decimal longitude { get { return (Decimal)GetValue(typeof(Decimal), "longitude"); } set { SetValue("longitude", value); } }
        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public String MarkerHeadimg { get { return (String)GetValue(typeof(String), "MarkerHeadimg"); } set { SetValue("MarkerHeadimg", value); } }
        [DataMember]
        public Int32 CardID { get; set; }

        #region ICloneable Member
        public override object Clone()
        {
            CompanyVO tmp = new CompanyVO();
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
    public partial class CompanyList
    {
        public List<CompanyVO> List { get; set; }
        public int Count { get; set; }
    }
}