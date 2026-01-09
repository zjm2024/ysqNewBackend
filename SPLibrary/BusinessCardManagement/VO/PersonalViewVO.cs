using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class PersonalViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(PersonalViewVO));
       
		[DataMember]
		 public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID") ; } set {  SetValue("PersonalID", value); } }
        [DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public String Position { get { return (String)GetValue(typeof(String), "Position"); } set { SetValue("Position", value); } }
        [DataMember]
        public String Business { get { return (String)GetValue(typeof(String), "Business"); } set { SetValue("Business", value); } }
        [DataMember]
        public String Address { get { return (String)GetValue(typeof(String), "Address"); } set { SetValue("Address", value); } }
        [DataMember]
        public Decimal latitude { get { return (Decimal)GetValue(typeof(Decimal), "latitude"); } set { SetValue("latitude", value); } }
        [DataMember]
        public Decimal longitude { get { return (Decimal)GetValue(typeof(Decimal), "longitude"); } set { SetValue("longitude", value); } }
        [DataMember]
        public String Tel { get { return (String)GetValue(typeof(String), "Tel"); } set { SetValue("Tel", value); } }
        [DataMember]
        public String Email { get { return (String)GetValue(typeof(String), "Email"); } set { SetValue("Email", value); } }
        [DataMember]
        public String WeChat { get { return (String)GetValue(typeof(String), "WeChat"); } set { SetValue("WeChat", value); } }
        [DataMember]
        public String Details { get { return (String)GetValue(typeof(String), "Details"); } set { SetValue("Details", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String QRimg { get { return (String)GetValue(typeof(String), "QRimg"); } set { SetValue("QRimg", value); } }
        [DataMember]
        public String PosterImg { get { return (String)GetValue(typeof(String), "PosterImg"); } set { SetValue("PosterImg", value); } }
        [DataMember]
        public Int32 DepartmentID { get { return (Int32)GetValue(typeof(Int32), "DepartmentID"); } set { SetValue("DepartmentID", value); } }
        [DataMember]
        public String BusinessName { get { return (String)GetValue(typeof(String), "BusinessName"); } set { SetValue("BusinessName", value); } }
        [DataMember]
        public String Industry { get { return (String)GetValue(typeof(String), "Industry"); } set { SetValue("Industry", value); } }
        [DataMember]
        public String LogoImg { get { return (String)GetValue(typeof(String), "LogoImg"); } set { SetValue("LogoImg", value); } }
        [DataMember]
        public String DepartmentName { get { return (String)GetValue(typeof(String), "DepartmentName"); } set { SetValue("DepartmentName", value); } }
        [DataMember]
        public Int32 ReadCounts { get { return (Int32)GetValue(typeof(Int32), "ReadCounts"); } set { SetValue("ReadCounts", value); } }
        [DataMember]
        public Boolean isExternal { get { return (Boolean)GetValue(typeof(Boolean), "isExternal"); } set { SetValue("isExternal", value); } }

        [DataMember]
        public JurisdictionViewVO Jurisdiction { get; set; }



        #region ICloneable Member
        public override object Clone()
        {
            PersonalViewVO tmp = new PersonalViewVO();
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