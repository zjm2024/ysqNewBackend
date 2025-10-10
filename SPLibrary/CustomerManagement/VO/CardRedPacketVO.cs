using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable] 
    public partial class CardRedPacketVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardRedPacketVO));
       
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
        public String OrderNO { get { return (String)GetValue(typeof(String), "OrderNO"); } set { SetValue("OrderNO", value); } }
        [DataMember]
        public Int32 PartyID { get { return (Int32)GetValue(typeof(Int32), "PartyID"); } set { SetValue("PartyID", value); } }
        [DataMember]
        public Int32 CardID { get { return (Int32)GetValue(typeof(Int32), "CardID"); } set { SetValue("CardID", value); } }
        [DataMember]
        public DateTime payAt { get { return (DateTime)GetValue(typeof(DateTime), "payAt"); } set { SetValue("payAt", value); } }
        [DataMember]
        public Decimal ServiceCharge { get { return (Decimal)GetValue(typeof(Decimal), "ServiceCharge"); } set { SetValue("ServiceCharge", value); } }
        [DataMember]
        public String OpenId { get { return (String)GetValue(typeof(String), "OpenId"); } set { SetValue("OpenId", value); } }
        [DataMember]
        public Int32 RPRule { get { return (Int32)GetValue(typeof(Int32), "RPRule"); } set { SetValue("RPRule", value); } }
        [DataMember]
        public String Message { get { return (String)GetValue(typeof(String), "Message"); } set { SetValue("Message", value); } }


        #region ICloneable Member
        public override object Clone()
        {
            CardRedPacketVO tmp = new CardRedPacketVO();
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