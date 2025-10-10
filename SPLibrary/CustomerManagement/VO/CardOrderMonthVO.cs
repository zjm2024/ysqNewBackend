using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardOrderMonthVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardOrderMonthVO));

        [DataMember]
        public String MONTH { get { return (String)GetValue(typeof(String), "MONTH"); } set { SetValue("MONTH", value); } }
        [DataMember]
        public Int32 AgentCustomerId { get { return (Int32)GetValue(typeof(Int32), "AgentCustomerId"); } set { SetValue("AgentCustomerId", value); } }
        [DataMember]
        public Int32 OneRebateAgentCustomerId { get { return (Int32)GetValue(typeof(Int32), "OneRebateAgentCustomerId"); } set { SetValue("OneRebateAgentCustomerId", value); } }
        [DataMember]
        public Int32 TwoRebateAgentCustomerId { get { return (Int32)GetValue(typeof(Int32), "TwoRebateAgentCustomerId"); } set { SetValue("TwoRebateAgentCustomerId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardOrderMonthVO tmp = new CardOrderMonthVO();
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
