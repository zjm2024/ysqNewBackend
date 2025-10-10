using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class ZXTFriendViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ZXTFriendViewVO));

        [DataMember]
        public Int32 FriendID { get { return (Int32)GetValue(typeof(Int32), "FriendID"); } set { SetValue("FriendID", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 FriendTo { get { return (Int32)GetValue(typeof(Int32), "FriendTo"); } set { SetValue("FriendTo", value); } }
        [DataMember]
        public String ToHeaderLogo { get { return (String)GetValue(typeof(String), "ToHeaderLogo"); } set { SetValue("ToHeaderLogo", value); } }
        [DataMember]
        public String ToCustomerName { get { return (String)GetValue(typeof(String), "ToCustomerName"); } set { SetValue("ToCustomerName", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            ZXTFriendViewVO tmp = new ZXTFriendViewVO();
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
