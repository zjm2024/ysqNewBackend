using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class BcPayOutHistoryVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BcPayOutHistoryVO));

        [DataMember]
        public Int32 PayOutHistoryId { get { return (Int32)GetValue(typeof(Int32), "PayOutHistoryId"); } set { SetValue("PayOutHistoryId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public Decimal PayOutCost { get { return (Decimal)GetValue(typeof(Decimal), "PayOutCost"); } set { SetValue("PayOutCost", value); } }
        [DataMember]
        public Decimal ServiceCharge { get { return (Decimal)GetValue(typeof(Decimal), "ServiceCharge"); } set { SetValue("ServiceCharge", value); } }
        [DataMember]
        public DateTime PayOutDate { get { return (DateTime)GetValue(typeof(DateTime), "PayOutDate"); } set { SetValue("PayOutDate", value); } }
        [DataMember]
        public DateTime HandleDate { get { return (DateTime)GetValue(typeof(DateTime), "HandleDate"); } set { SetValue("HandleDate", value); } }
        [DataMember]
        public Int32 PayOutStatus { get { return (Int32)GetValue(typeof(Int32), "PayOutStatus"); } set { SetValue("PayOutStatus", value); } }
        [DataMember]
        public String BankAccount { get { return (String)GetValue(typeof(String), "BankAccount"); } set { SetValue("BankAccount", value); } }
        [DataMember]
        public String BankName { get { return (String)GetValue(typeof(String), "BankName"); } set { SetValue("BankName", value); } }
        [DataMember]
        public String AccountName { get { return (String)GetValue(typeof(String), "AccountName"); } set { SetValue("AccountName", value); } }
        [DataMember]
        public String ThirdOrder { get { return (String)GetValue(typeof(String), "ThirdOrder"); } set { SetValue("ThirdOrder", value); } }
        [DataMember]
        public String HandleComment { get { return (String)GetValue(typeof(String), "HandleComment"); } set { SetValue("HandleComment", value); } }
        [DataMember]
        public String OpenId { get { return (String)GetValue(typeof(String), "OpenId"); } set { SetValue("OpenId", value); } }
        [DataMember]
        public String OrderID { get { return (String)GetValue(typeof(String), "OrderID"); } set { SetValue("OrderID", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }
        [DataMember]
        public Int32 Type { get { return (Int32)GetValue(typeof(Int32), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public Int32 BankAccountID { get { return (Int32)GetValue(typeof(Int32), "BankAccountID"); } set { SetValue("BankAccountID", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            BcPayOutHistoryVO tmp = new BcPayOutHistoryVO();
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