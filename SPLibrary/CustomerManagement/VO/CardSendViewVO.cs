using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardSendViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardSendViewVO));
       
		[DataMember]
		 public Int32 CardID { get { return (Int32)GetValue(typeof(Int32), "CardID") ; } set {  SetValue("CardID", value); } }
        [DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
		 public String CardImg { get { return (String)GetValue(typeof(String), "CardImg") ; } set {  SetValue("CardImg", value); } }
        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String Position { get { return (String)GetValue(typeof(String), "Position"); } set { SetValue("Position", value); } }
        [DataMember]
        public String CorporateName { get { return (String)GetValue(typeof(String), "CorporateName"); } set { SetValue("CorporateName", value); } }
        [DataMember]
        public String Business { get { return (String)GetValue(typeof(String), "Business"); } set { SetValue("Business", value); } }
        [DataMember]
        public String Address { get { return (String)GetValue(typeof(String), "Address"); } set { SetValue("Address", value); } }
        [DataMember]
        public Decimal latitude { get { return (Decimal)GetValue(typeof(Decimal), "latitude"); } set { SetValue("latitude", value); } }
        [DataMember]
        public Decimal longitude { get { return (Decimal)GetValue(typeof(Decimal), "longitude"); } set { SetValue("longitude", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String Tel { get { return (String)GetValue(typeof(String), "Tel"); } set { SetValue("Tel", value); } }
        [DataMember]
        public String Email { get { return (String)GetValue(typeof(String), "Email"); } set { SetValue("Email", value); } }
        [DataMember]
        public String WeChat { get { return (String)GetValue(typeof(String), "WeChat"); } set { SetValue("WeChat", value); } }
        [DataMember]
        public String WebSite { get { return (String)GetValue(typeof(String), "WebSite"); } set { SetValue("WebSite", value); } }
        [DataMember]
        public String Details { get { return (String)GetValue(typeof(String), "Details"); } set { SetValue("Details", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Int32 ReadCount { get { return (Int32)GetValue(typeof(Int32), "ReadCount"); } set { SetValue("ReadCount", value); } }
        [DataMember]
        public Int32 Collection { get { return (Int32)GetValue(typeof(Int32), "Collection"); } set { SetValue("Collection", value); } }
        [DataMember]
        public Int32 Forward { get { return (Int32)GetValue(typeof(Int32), "Forward"); } set { SetValue("Forward", value); } }
        [DataMember]
        public Int32 isDisplayPhone { get { return (Int32)GetValue(typeof(Int32), "isDisplayPhone"); } set { SetValue("isDisplayPhone", value); } }
        [DataMember]
        public Int32 isDisplayTel { get { return (Int32)GetValue(typeof(Int32), "isDisplayTel"); } set { SetValue("isDisplayTel", value); } }
        [DataMember]
        public Int32 isDisplayWeChat { get { return (Int32)GetValue(typeof(Int32), "isDisplayWeChat"); } set { SetValue("isDisplayWeChat", value); } }
        [DataMember]
        public Int32 isDisplayEmail { get { return (Int32)GetValue(typeof(Int32), "isDisplayEmail"); } set { SetValue("isDisplayEmail", value); } }
        [DataMember]
        public Int32 style { get { return (Int32)GetValue(typeof(Int32), "style"); } set { SetValue("style", value); } }

        [DataMember]
        public Int32 SendID { get { return (Int32)GetValue(typeof(Int32), "SendID"); } set { SetValue("SendID", value); } }
        [DataMember]
        public Int32 ReadStatus { get { return (Int32)GetValue(typeof(Int32), "ReadStatus"); } set { SetValue("ReadStatus", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardSendViewVO tmp = new CardSendViewVO();
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