using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class OriginCustomerIdViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(OriginCustomerIdViewVO));

        [DataMember]
        public String CustomerId { get { return (String)GetValue(typeof(String), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public String originCustomerId { get { return (String)GetValue(typeof(String), "originCustomerId"); } set { SetValue("originCustomerId", value); } }
        [DataMember]
        public String originCustomerId2 { get { return (String)GetValue(typeof(String), "originCustomerId2"); } set { SetValue("originCustomerId2", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Boolean isVip { get { return (Boolean)GetValue(typeof(Boolean), "isVip"); } set { SetValue("isVip", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            OriginCustomerIdViewVO tmp = new OriginCustomerIdViewVO();
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