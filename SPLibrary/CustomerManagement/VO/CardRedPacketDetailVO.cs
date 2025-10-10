using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable] 
    public partial class CardRedPacketDetailVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardRedPacketDetailVO));
       
		[DataMember]
		 public Int32 RPDetailId { get { return (Int32)GetValue(typeof(Int32), "RPDetailId") ; } set {  SetValue("RPDetailId", value); } }
        [DataMember]
		 public Int32 RedPacketId { get { return (Int32)GetValue(typeof(Int32), "RedPacketId") ; } set {  SetValue("RedPacketId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Decimal GetMoney { get { return (Decimal)GetValue(typeof(Decimal), "GetMoney"); } set { SetValue("GetMoney", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public DateTime GetDate { get { return (DateTime)GetValue(typeof(DateTime), "GetDate"); } set { SetValue("GetDate", value); } }



        #region ICloneable Member
        public override object Clone()
        {
            CardRedPacketDetailVO tmp = new CardRedPacketDetailVO();
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