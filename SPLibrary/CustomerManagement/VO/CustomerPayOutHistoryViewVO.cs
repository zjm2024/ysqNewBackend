using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class CustomerPayOutHistoryViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CustomerPayOutHistoryViewVO));

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
        public String CustomerCode { get { return (String)GetValue(typeof(String), "CustomerCode"); } set { SetValue("CustomerCode", value); } }
        [DataMember]
        public String CustomerAccount { get { return (String)GetValue(typeof(String), "CustomerAccount"); } set { SetValue("CustomerAccount", value); } }
        [DataMember]
        public String Password { get { return (String)GetValue(typeof(String), "Password"); } set { SetValue("Password", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public Int32 BankAccountId { get { return (Int32)GetValue(typeof(Int32), "BankAccountId"); } set { SetValue("BankAccountId", value); } }

        [DataMember]
        public String BankName { get { return (String)GetValue(typeof(String), "BankName"); } set { SetValue("BankName", value); } }

        [DataMember]
        public String BankAccount { get { return (String)GetValue(typeof(String), "BankAccount"); } set { SetValue("BankAccount", value); } }

        [DataMember]
        public String SubBranch { get { return (String)GetValue(typeof(String), "SubBranch"); } set { SetValue("SubBranch", value); } }

        [DataMember]
        public String AccountName { get { return (String)GetValue(typeof(String), "AccountName"); } set { SetValue("AccountName", value); } }

        [DataMember]
        public String HandleComment { get { return (String)GetValue(typeof(String), "HandleComment"); } set { SetValue("HandleComment", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CustomerPayOutHistoryViewVO tmp = new CustomerPayOutHistoryViewVO();
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