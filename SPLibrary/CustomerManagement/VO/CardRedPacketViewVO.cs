using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable] 
    public partial class CardRedPacketViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardRedPacketViewVO));
       
		[DataMember]
		 public Int32 RedPacketId { get { return (Int32)GetValue(typeof(Int32), "RedPacketId") ; } set {  SetValue("RedPacketId", value); } }
        [DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 RPType { get { return (Int32)GetValue(typeof(Int32), "RPType"); } set { SetValue("RPType", value); } }
        [DataMember]
        public Decimal RPCost { get { return (Decimal)GetValue(typeof(Decimal), "RPCost"); } set { SetValue("RPCost", value); } }
        [DataMember]
        public Decimal RPResidueCost { get { return (Decimal)GetValue(typeof(Decimal), "RPResidueCost"); } set { SetValue("RPResidueCost", value); } }
        [DataMember]
        public Int32 RPNum { get { return (Int32)GetValue(typeof(Int32), "RPNum"); } set { SetValue("RPNum", value); } }
        [DataMember]
        public Int32 RPResidueNum { get { return (Int32)GetValue(typeof(Int32), "RPResidueNum"); } set { SetValue("RPResidueNum", value); } }
        [DataMember]
        public String RPContent { get { return (String)GetValue(typeof(String), "RPContent"); } set { SetValue("RPContent", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Int32 isEqually { get { return (Int32)GetValue(typeof(Int32), "isEqually"); } set { SetValue("isEqually", value); } }
        [DataMember]
        public DateTime RPCreateDate { get { return (DateTime)GetValue(typeof(DateTime), "RPCreateDate"); } set { SetValue("RPCreateDate", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String HeaderLogo { get { return (String)GetValue(typeof(String), "HeaderLogo"); } set { SetValue("HeaderLogo", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public Int32 PartyID { get { return (Int32)GetValue(typeof(Int32), "PartyID"); } set { SetValue("PartyID", value); } }
        [DataMember]
        public Int32 CardID { get { return (Int32)GetValue(typeof(Int32), "CardID"); } set { SetValue("CardID", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String Party_QRImg { get { return (String)GetValue(typeof(String), "Party_QRImg"); } set { SetValue("Party_QRImg", value); } }
        [DataMember]
        public String Card_QRImg { get { return (String)GetValue(typeof(String), "Card_QRImg"); } set { SetValue("Card_QRImg", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public Int32 RPRule { get { return (Int32)GetValue(typeof(Int32), "RPRule"); } set { SetValue("RPRule", value); } }
        [DataMember]
        public String Message { get { return (String)GetValue(typeof(String), "Message"); } set { SetValue("Message", value); } }



        #region ICloneable Member
        public override object Clone()
        {
            CardRedPacketViewVO tmp = new CardRedPacketViewVO();
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