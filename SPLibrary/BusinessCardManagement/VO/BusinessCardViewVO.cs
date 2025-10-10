using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class BusinessCardViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BusinessCardViewVO));
       
		[DataMember]
		 public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID") ; } set {  SetValue("BusinessID", value); } }
        [DataMember]
        public String BusinessName { get { return (String)GetValue(typeof(String), "BusinessName"); } set { SetValue("BusinessName", value); } }
        [DataMember]
        public String Industry { get { return (String)GetValue(typeof(String), "Industry"); } set { SetValue("Industry", value); } }
        [DataMember]
        public String LogoImg { get { return (String)GetValue(typeof(String), "LogoImg"); } set { SetValue("LogoImg", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 Number { get { return (Int32)GetValue(typeof(Int32), "Number"); } set { SetValue("Number", value); } }
        [DataMember]
        public DateTime ExpirationAt { get { return (DateTime)GetValue(typeof(DateTime), "ExpirationAt"); } set { SetValue("ExpirationAt", value); } }
           
        [DataMember]
        public String Address { get { return (String)GetValue(typeof(String), "Address"); } set { SetValue("Address", value); } }
        [DataMember]
        public Decimal latitude { get { return (Decimal)GetValue(typeof(Decimal), "latitude"); } set { SetValue("latitude", value); } }
        [DataMember]
        public Decimal longitude { get { return (Decimal)GetValue(typeof(Decimal), "longitude"); } set { SetValue("longitude", value); } }
        [DataMember]
        public String Tel { get { return (String)GetValue(typeof(String), "Tel"); } set { SetValue("Tel", value); } }

        [DataMember]
        public Int32 isAddress { get { return (Int32)GetValue(typeof(Int32), "isAddress"); } set { SetValue("isAddress", value); } }
        [DataMember]
        public Int32 isTel { get { return (Int32)GetValue(typeof(Int32), "isTel"); } set { SetValue("isTel", value); } }
        [DataMember]
        public String JoinQR { get { return (String)GetValue(typeof(String), "JoinQR"); } set { SetValue("JoinQR", value); } }
        [DataMember]
        public Int32 PersonalCounts { get { return (Int32)GetValue(typeof(Int32), "PersonalCounts"); } set { SetValue("PersonalCounts", value); } }
        [DataMember]
        public Int32 isPay { get { return (Int32)GetValue(typeof(Int32), "isPay"); } set { SetValue("isPay", value); } }

        [DataMember]
        public String OfficialProducts { get { return (String)GetValue(typeof(String), "OfficialProducts"); } set { SetValue("OfficialProducts", value); } }
        [DataMember]
        public Int32 HeadquartersID { get { return (Int32)GetValue(typeof(Int32), "HeadquartersID"); } set { SetValue("HeadquartersID", value); } }
        [DataMember]
        public Int32 isGroup { get { return (Int32)GetValue(typeof(Int32), "isGroup"); } set { SetValue("isGroup", value); } }
        [DataMember]
        public String BusinessLicenseImg { get { return (String)GetValue(typeof(String), "BusinessLicenseImg"); } set { SetValue("BusinessLicenseImg", value); } }

        [DataMember]
        public Int32 ThemeID { get { return (Int32)GetValue(typeof(Int32), "ThemeID"); } set { SetValue("ThemeID", value); } }
        [DataMember]
        public String CustomColumn { get { return (String)GetValue(typeof(String), "CustomColumn"); } set { SetValue("CustomColumn", value); } }

        [DataMember]
        public Int32 AccessSetUpSignIn { get { return (Int32)GetValue(typeof(Int32), "AccessSetUpSignIn"); } set { SetValue("AccessSetUpSignIn", value); } }
        [DataMember]
        public Int32 AccessSetUpPhone { get { return (Int32)GetValue(typeof(Int32), "AccessSetUpPhone"); } set { SetValue("AccessSetUpPhone", value); } }
        [DataMember]
        public Int32 AccessSetUpExchange { get { return (Int32)GetValue(typeof(Int32), "AccessSetUpExchange"); } set { SetValue("AccessSetUpExchange", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            BusinessCardViewVO tmp = new BusinessCardViewVO();
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