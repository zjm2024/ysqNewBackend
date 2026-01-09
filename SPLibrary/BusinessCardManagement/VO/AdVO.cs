using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AdVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AdVO));

        [DataMember]
        public Int32 AdID { get { return (Int32)GetValue(typeof(Int32), "AdID"); } set { SetValue("AdID", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String AdImg { get { return (String)GetValue(typeof(String), "AdImg"); } set { SetValue("AdImg", value); } }
        [DataMember]
        public String Position { get { return (String)GetValue(typeof(String), "Position"); } set { SetValue("Position", value); } }
        [DataMember]
		public Int32 GoType { get { return (Int32)GetValue(typeof(Int32), "GoType") ; } set {  SetValue("GoType", value); } }
        [DataMember]
        public String Url { get { return (String)GetValue(typeof(String), "Url"); } set { SetValue("Url", value); } }
        [DataMember]
        public String AppId { get { return (String)GetValue(typeof(String), "AppId"); } set { SetValue("AppId", value); } }
        [DataMember]
		public Int32 ClickCount { get { return (Int32)GetValue(typeof(Int32), "ClickCount") ; } set {  SetValue("ClickCount", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            AdVO tmp = new AdVO();
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