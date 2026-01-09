using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardExchangeCodeVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardExchangeCodeVO));
       
		[DataMember]
		 public Int32 ExchangeCodeID { get { return (Int32)GetValue(typeof(Int32), "ExchangeCodeID") ; } set {  SetValue("ExchangeCodeID", value); } }
        [DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 ToCustomerId { get { return (Int32)GetValue(typeof(Int32), "ToCustomerId"); } set { SetValue("ToCustomerId", value); } }
        [DataMember]
        public String ToCustomerName { get { return (String)GetValue(typeof(String), "ToCustomerName"); } set { SetValue("ToCustomerName", value); } }
        [DataMember]
        public String ToHeaderLogo { get { return (String)GetValue(typeof(String), "ToHeaderLogo"); } set { SetValue("ToHeaderLogo", value); } }
        [DataMember]
        public String Code { get { return (String)GetValue(typeof(String), "Code"); } set { SetValue("Code", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public DateTime ExpirationAt { get { return (DateTime)GetValue(typeof(DateTime), "ExpirationAt"); } set { SetValue("ExpirationAt", value); } }
        [DataMember]
        public DateTime UsedAt { get { return (DateTime)GetValue(typeof(DateTime), "UsedAt"); } set { SetValue("UsedAt", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Int32 Type { get { return (Int32)GetValue(typeof(Int32), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }
        [DataMember]
        public Int32 CardID { get { return (Int32)GetValue(typeof(Int32), "CardID"); } set { SetValue("CardID", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardExchangeCodeVO tmp = new CardExchangeCodeVO();
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