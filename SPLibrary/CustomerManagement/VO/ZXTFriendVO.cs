using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class ZXTFriendVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ZXTFriendVO));

        [DataMember]
        public Int32 FriendID { get { return (Int32)GetValue(typeof(Int32), "FriendID"); } set { SetValue("FriendID", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 FriendTo { get { return (Int32)GetValue(typeof(Int32), "FriendTo"); } set { SetValue("FriendTo", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            ZXTFriendVO tmp = new ZXTFriendVO();
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
