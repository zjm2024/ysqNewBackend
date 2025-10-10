using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class IMMessageHistoryVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(IMMessageHistoryVO));

        [DataMember]
        public String username { get { return (String)GetValue(typeof(String), "username"); } set { SetValue("username", value); } }
        [DataMember]
        public String id { get { return (String)GetValue(typeof(String), "id"); } set { SetValue("id", value); } }
        [DataMember]
        public String avatar { get { return (String)GetValue(typeof(String), "avatar"); } set { SetValue("avatar", value); } }
        [DataMember]
        public Int32 timestamp { get { return (Int32)GetValue(typeof(Int32), "timestamp"); } set { SetValue("timestamp", value); } }
        [DataMember]
        public String content { get { return (String)GetValue(typeof(String), "content"); } set { SetValue("content", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            IMMessageHistoryVO tmp = new IMMessageHistoryVO();
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