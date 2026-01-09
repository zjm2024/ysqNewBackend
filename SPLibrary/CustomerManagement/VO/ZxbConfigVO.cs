using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class ZxbConfigVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ZxbConfigVO));

        [DataMember]
        public Int32 ZxbConfigID { get { return (Int32)GetValue(typeof(Int32), "ZxbConfigID"); } set { SetValue("ZxbConfigID", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public String Purpose { get { return (String)GetValue(typeof(String), "Purpose"); } set { SetValue("Purpose", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public String code { get { return (String)GetValue(typeof(String), "code"); } set { SetValue("code", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            ZxbConfigVO tmp = new ZxbConfigVO();
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