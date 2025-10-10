using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class TargetCityViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(TargetCityViewVO));

        [DataMember]
        public Int32 TargetCityId { get { return (Int32)GetValue(typeof(Int32), "TargetCityId"); } set { SetValue("TargetCityId", value); } }
        [DataMember]
        public Int32 BusinessId { get { return (Int32)GetValue(typeof(Int32), "BusinessId"); } set { SetValue("BusinessId", value); } }
        [DataMember]
        public Int32 CityId { get { return (Int32)GetValue(typeof(Int32), "CityId"); } set { SetValue("CityId", value); } }
        [DataMember]
        public String CityCode { get { return (String)GetValue(typeof(String), "CityCode"); } set { SetValue("CityCode", value); } }
        [DataMember]
        public String CityName { get { return (String)GetValue(typeof(String), "CityName"); } set { SetValue("CityName", value); } }
        [DataMember]
        public Int32 ProvinceId { get { return (Int32)GetValue(typeof(Int32), "ProvinceId"); } set { SetValue("ProvinceId", value); } }
        [DataMember]
        public String ProvinceName { get { return (String)GetValue(typeof(String), "ProvinceName"); } set { SetValue("ProvinceName", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            TargetCityViewVO tmp = new TargetCityViewVO();
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