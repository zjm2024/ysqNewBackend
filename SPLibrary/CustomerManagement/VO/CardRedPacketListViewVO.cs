using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable] 
    public partial class CardRedPacketListViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardRedPacketListViewVO));
       
		[DataMember]
		 public Int32 RPListId { get { return (Int32)GetValue(typeof(Int32), "RPListId") ; } set {  SetValue("RPListId", value); } }
        [DataMember]
		 public Int32 RedPacketId { get { return (Int32)GetValue(typeof(Int32), "RedPacketId") ; } set {  SetValue("RedPacketId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Decimal RPOneCost { get { return (Decimal)GetValue(typeof(Decimal), "RPOneCost"); } set { SetValue("RPOneCost", value); } }
        [DataMember]
        public Int32 isReceive { get { return (Int32)GetValue(typeof(Int32), "isReceive"); } set { SetValue("isReceive", value); } }
        [DataMember]
        public DateTime ReceiveDate { get { return (DateTime)GetValue(typeof(DateTime), "ReceiveDate"); } set { SetValue("ReceiveDate", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String HeaderLogo { get { return (String)GetValue(typeof(String), "HeaderLogo"); } set { SetValue("HeaderLogo", value); } }
        [DataMember]
        public String CardPhone { get { return (String)GetValue(typeof(String), "CardPhone"); } set { SetValue("CardPhone", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String CorporateName { get { return (String)GetValue(typeof(String), "CorporateName"); } set { SetValue("CorporateName", value); } }
        [DataMember]
        public String Position { get { return (String)GetValue(typeof(String), "Position"); } set { SetValue("Position", value); } }
        [DataMember]
        public String OpenId { get { return (String)GetValue(typeof(String), "OpenId"); } set { SetValue("OpenId", value); } }
        [DataMember]
        public Int32 LikeType { get { return (Int32)GetValue(typeof(Int32), "LikeType"); } set { SetValue("LikeType", value); } }


        #region ICloneable Member
        public override object Clone()
        {
            CardRedPacketListViewVO tmp = new CardRedPacketListViewVO();
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