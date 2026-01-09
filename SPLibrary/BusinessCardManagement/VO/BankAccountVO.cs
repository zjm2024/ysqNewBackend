using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class BcBankAccountVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BcBankAccountVO));

        [DataMember]
        public Int32 BankAccountID { get { return (Int32)GetValue(typeof(Int32), "BankAccountID"); } set { SetValue("BankAccountID", value); } }
        [DataMember]
        public String Type { get { return (String)GetValue(typeof(String), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public String BankAccount { get { return (String)GetValue(typeof(String), "BankAccount"); } set { SetValue("BankAccount", value); } }
        [DataMember]
        public String BankName { get { return (String)GetValue(typeof(String), "BankName"); } set { SetValue("BankName", value); } }
        [DataMember]
        public String AccountName { get { return (String)GetValue(typeof(String), "AccountName"); } set { SetValue("AccountName", value); } }

        [DataMember]
        public String UniformCreditCodeNo { get { return (String)GetValue(typeof(String), "UniformCreditCodeNo"); } set { SetValue("UniformCreditCodeNo", value); } }
        [DataMember]
        public String Address { get { return (String)GetValue(typeof(String), "Address"); } set { SetValue("Address", value); } }
        [DataMember]
        public String Tel { get { return (String)GetValue(typeof(String), "Tel"); } set { SetValue("Tel", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            BcBankAccountVO tmp = new BcBankAccountVO();
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