using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AgentVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgentVO));

        [DataMember]
        public Int32 AgentID { get { return (Int32)GetValue(typeof(Int32), "AgentID"); } set { SetValue("AgentID", value); } }
        [DataMember]
		public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID") ; } set {  SetValue("BusinessID", value); } }
        [DataMember]
		public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID") ; } set {  SetValue("PersonalID", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 AgentLevelID { get { return (Int32)GetValue(typeof(Int32), "AgentLevelID"); } set { SetValue("AgentLevelID", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            AgentVO tmp = new AgentVO();
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