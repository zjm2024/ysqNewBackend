using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AgentlevelCostVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgentlevelCostVO));
        
		[DataMember]
		public Int32 AgentlevelCostID { get { return (Int32)GetValue(typeof(Int32), "AgentlevelCostID") ; } set {  SetValue("AgentlevelCostID", value); } }
        [DataMember]
		public Int32 AgentLevelID { get { return (Int32)GetValue(typeof(Int32), "AgentLevelID") ; } set {  SetValue("AgentLevelID", value); } }
        [DataMember]
        public Int32 InfoID { get { return (Int32)GetValue(typeof(Int32), "InfoID"); } set { SetValue("InfoID", value); } }
        [DataMember]
        public Int32 Discount { get { return (Int32)GetValue(typeof(Int32), "Discount"); } set { SetValue("Discount", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            AgentlevelCostVO tmp = new AgentlevelCostVO();
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