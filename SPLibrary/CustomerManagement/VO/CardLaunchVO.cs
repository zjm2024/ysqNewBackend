using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardLaunchVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardLaunchVO));

        [DataMember]
        public Int32 LaunchID { get { return (Int32)GetValue(typeof(Int32), "LaunchID"); } set { SetValue("LaunchID", value); } }
        [DataMember]
		public Int32 Type { get { return (Int32)GetValue(typeof(Int32), "Type") ; } set {  SetValue("Type", value); } }
        [DataMember]
        public String path { get { return (String)GetValue(typeof(String), "path"); } set { SetValue("path", value); } }
        [DataMember]
        public Int32 scene { get { return (Int32)GetValue(typeof(Int32), "scene"); } set { SetValue("scene", value); } }
        [DataMember]
        public String appId { get { return (String)GetValue(typeof(String), "appId"); } set { SetValue("appId", value); } }
        [DataMember]
        public String openId { get { return (String)GetValue(typeof(String), "openId"); } set { SetValue("openId", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); }}
        [DataMember]
        public String LoginIP { get { return (String)GetValue(typeof(String), "LoginIP"); } set { SetValue("LoginIP", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardLaunchVO tmp = new CardLaunchVO();
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