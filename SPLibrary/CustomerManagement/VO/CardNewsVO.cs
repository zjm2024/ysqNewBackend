using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CardNewsVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CardNewsVO));

        [DataMember]
        public Int32 NewsID { get { return (Int32)GetValue(typeof(Int32), "NewsID"); } set { SetValue("NewsID", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String Synopsis { get { return (String)GetValue(typeof(String), "Synopsis"); } set { SetValue("Synopsis", value); } }
        [DataMember]
        public String Url { get { return (String)GetValue(typeof(String), "Url"); } set { SetValue("Url", value); } }
        [DataMember]
        public String AppId { get { return (String)GetValue(typeof(String), "AppId"); } set { SetValue("AppId", value); } }
        [DataMember]
        public Int32 isSend { get { return (Int32)GetValue(typeof(Int32), "isSend"); } set { SetValue("isSend", value); } }
        [DataMember]
        public String NewsImg { get { return (String)GetValue(typeof(String), "NewsImg"); } set { SetValue("NewsImg", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 isDefault { get { return (Int32)GetValue(typeof(Int32), "isDefault"); } set { SetValue("isDefault", value); } }
        [DataMember]
        public Int32 isAlert { get { return (Int32)GetValue(typeof(Int32), "isAlert"); } set { SetValue("isAlert", value); } }

        [DataMember]
        public Int32 GoType { get { return (Int32)GetValue(typeof(Int32), "GoType"); } set { SetValue("GoType", value); } }
        [DataMember]
        public Int32 ShowType { get { return (Int32)GetValue(typeof(Int32), "ShowType"); } set { SetValue("ShowType", value); } }
        [DataMember]
        public Int32 OrderNO { get { return (Int32)GetValue(typeof(Int32), "OrderNO"); } set { SetValue("OrderNO", value); } }

        [DataMember]
        public Int32 ClickCount { get { return (Int32)GetValue(typeof(Int32), "ClickCount"); } set { SetValue("ClickCount", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CardNewsVO tmp = new CardNewsVO();
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