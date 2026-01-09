using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AgentLevelVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgentLevelVO));

        [DataMember]
        public Int32 AgentLevelID { get { return (Int32)GetValue(typeof(Int32), "AgentLevelID"); } set { SetValue("AgentLevelID", value); } }
        [DataMember]
        public String LevelName { get { return (String)GetValue(typeof(String), "LevelName"); } set { SetValue("LevelName", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public Int32 Discount { get { return (Int32)GetValue(typeof(Int32), "Discount"); } set { SetValue("Discount", value); } }
        [DataMember]
        public Int32 Mode { get { return (Int32)GetValue(typeof(Int32), "Mode"); } set { SetValue("Mode", value); } }

        [DataMember]
        public Int32 AgentCount { get; set; }

        #region ICloneable Member
        public override object Clone()
        {
            AgentLevelVO tmp = new AgentLevelVO();
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