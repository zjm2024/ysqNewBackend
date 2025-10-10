using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AgentIntegralVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgentIntegralVO));
        
		[DataMember]
		public Int32 AgentIntegralID { get { return (Int32)GetValue(typeof(Int32), "AgentIntegralID") ; } set {  SetValue("AgentIntegralID", value); } }
        [DataMember]
        public Decimal Balance { get { return (Decimal)GetValue(typeof(Decimal), "Balance"); } set { SetValue("Balance", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 OrderID { get { return (Int32)GetValue(typeof(Int32), "OrderID"); } set { SetValue("OrderID", value); } }
        [DataMember]
        public Int32 OperPersonalID { get { return (Int32)GetValue(typeof(Int32), "OperPersonalID"); } set { SetValue("OperPersonalID", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public String Note { get { return (String)GetValue(typeof(String), "Note"); } set { SetValue("Note", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            AgentIntegralVO tmp = new AgentIntegralVO();
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