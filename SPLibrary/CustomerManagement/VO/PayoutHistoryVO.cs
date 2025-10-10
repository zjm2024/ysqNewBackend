using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class PayoutHistoryVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(PayoutHistoryVO));

        [DataMember]
        public Int32 PayOutHistoryId { get { return (Int32)GetValue(typeof(Int32), "PayOutHistoryId"); } set { SetValue("PayOutHistoryId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public DateTime PayOutDate { get { return (DateTime)GetValue(typeof(DateTime), "PayOutDate"); } set { SetValue("PayOutDate", value); } }
        [DataMember]
        public String PayOutOrder { get { return (String)GetValue(typeof(String), "PayOutOrder"); } set { SetValue("PayOutOrder", value); } }
        [DataMember]
        public Int32 PayOutStatus { get { return (Int32)GetValue(typeof(Int32), "PayOutStatus"); } set { SetValue("PayOutStatus", value); } }
        [DataMember]
        public String ThirdOrder { get { return (String)GetValue(typeof(String), "ThirdOrder"); } set { SetValue("ThirdOrder", value); } }
        [DataMember]
        public Int32 BankAccountId { get { return (Int32)GetValue(typeof(Int32), "BankAccountId"); } set { SetValue("BankAccountId", value); } }

        [DataMember]
        public String HandleComment { get { return (String)GetValue(typeof(String), "HandleComment"); } set { SetValue("HandleComment", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            PayoutHistoryVO tmp = new PayoutHistoryVO();
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