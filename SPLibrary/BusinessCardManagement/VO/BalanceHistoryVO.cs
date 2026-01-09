using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class BalanceHistoryVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BalanceHistoryVO));
        
		[DataMember]
		public Int32 BalanceId { get { return (Int32)GetValue(typeof(Int32), "BalanceId") ; } set {  SetValue("BalanceId", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public Decimal Balance { get { return (Decimal)GetValue(typeof(Decimal), "Balance"); } set { SetValue("Balance", value); } }
        [DataMember]
        public Decimal oldBalance { get { return (Decimal)GetValue(typeof(Decimal), "oldBalance"); } set { SetValue("oldBalance", value); } }
        [DataMember]
        public Decimal newBalance { get { return (Decimal)GetValue(typeof(Decimal), "newBalance"); } set { SetValue("newBalance", value); } }
        [DataMember]
        public String OrderID { get { return (String)GetValue(typeof(String), "OrderID"); } set { SetValue("OrderID", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            BalanceHistoryVO tmp = new BalanceHistoryVO();
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