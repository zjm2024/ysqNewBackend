using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardAccessRecordsViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardAccessRecordsViewVO));
       
		[DataMember]
		 public Int32 AccessRecordsID { get { return (Int32)GetValue(typeof(Int32), "AccessRecordsID") ; } set {  SetValue("AccessRecordsID", value); } }
        [DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 ToCustomerId { get { return (Int32)GetValue(typeof(Int32), "ToCustomerId"); } set { SetValue("ToCustomerId", value); } }
        [DataMember]
        public DateTime AccessAt { get { return (DateTime)GetValue(typeof(DateTime), "AccessAt"); } set { SetValue("AccessAt", value); } }
        [DataMember]
		 public String Type { get { return (String)GetValue(typeof(String), "Type") ; } set {  SetValue("Type", value); } }
        [DataMember]
        public Int32 ById { get { return (Int32)GetValue(typeof(Int32), "ById"); } set { SetValue("ById", value); } }
        [DataMember]
        public Int32 Counts { get { return (Int32)GetValue(typeof(Int32), "Counts"); } set { SetValue("Counts", value); } }
        [DataMember]
        public Int32 ResidenceAt { get { return (Int32)GetValue(typeof(Int32), "ResidenceAt"); } set { SetValue("ResidenceAt", value); } }
        [DataMember]
        public String OpenID { get { return (String)GetValue(typeof(String), "OpenID"); } set { SetValue("OpenID", value); } }
        [DataMember]
        public String LoginIP { get { return (String)GetValue(typeof(String), "LoginIP"); } set { SetValue("LoginIP", value); } }

        [DataMember]
        public String Nation { get { return (String)GetValue(typeof(String), "Nation"); } set { SetValue("Nation", value); } }
        [DataMember]
        public String Province { get { return (String)GetValue(typeof(String), "Province"); } set { SetValue("Province", value); } }
        [DataMember]
        public String City { get { return (String)GetValue(typeof(String), "City"); } set { SetValue("City", value); } }
        [DataMember]
        public String District { get { return (String)GetValue(typeof(String), "District"); } set { SetValue("District", value); } }

        [DataMember]
        public Decimal lat { get { return (Decimal)GetValue(typeof(Decimal), "lat"); } set { SetValue("lat", value); } }
        [DataMember]
        public Decimal lng { get { return (Decimal)GetValue(typeof(Decimal), "lng"); } set { SetValue("lng", value); } }

        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public String HeaderLogo { get { return (String)GetValue(typeof(String), "HeaderLogo"); } set { SetValue("HeaderLogo", value); } }

        [DataMember]
        public Int32 count { get { return (Int32)GetValue(typeof(Int32), "count"); } set { SetValue("count", value); } }

        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardAccessRecordsViewVO tmp = new CardAccessRecordsViewVO();
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