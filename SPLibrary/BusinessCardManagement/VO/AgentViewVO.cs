using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AgentViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgentViewVO));

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
        [DataMember]
        public String LevelName { get { return (String)GetValue(typeof(String), "LevelName"); } set { SetValue("LevelName", value); } }
        [DataMember]
        public Int32 Discount { get { return (Int32)GetValue(typeof(Int32), "Discount"); } set { SetValue("Discount", value); } }

        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public String Position { get { return (String)GetValue(typeof(String), "Position"); } set { SetValue("Position", value); } }

        [DataMember]
        public String BusinessName { get { return (String)GetValue(typeof(String), "BusinessName"); } set { SetValue("BusinessName", value); } }
        [DataMember]
        public String Industry { get { return (String)GetValue(typeof(String), "Industry"); } set { SetValue("Industry", value); } }
        [DataMember]
        public String LogoImg { get { return (String)GetValue(typeof(String), "LogoImg"); } set { SetValue("LogoImg", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            AgentViewVO tmp = new AgentViewVO();
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