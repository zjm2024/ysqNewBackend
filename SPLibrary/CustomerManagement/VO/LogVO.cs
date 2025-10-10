using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class LogVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(LogVO));

        [DataMember]
        public Int32 LogID { get { return (Int32)GetValue(typeof(Int32), "LogID"); } set { SetValue("LogID", value); } }
        [DataMember]
        public String Version { get { return (String)GetValue(typeof(String), "Version"); } set { SetValue("Version", value); } }
        [DataMember]
        public String Content { get { return (String)GetValue(typeof(String), "Content"); } set { SetValue("Content", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String Type { get { return (String)GetValue(typeof(String), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            LogVO tmp = new LogVO();
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