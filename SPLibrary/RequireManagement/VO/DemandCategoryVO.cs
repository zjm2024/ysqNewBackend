using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.RequireManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class DemandCategoryVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(DemandCategoryVO));
        [DataMember]
        public Int32 CategoryId { get { return (Int32)GetValue(typeof(Int32), "CategoryId"); } set { SetValue("CategoryId", value); } }
        [DataMember]
        public String CategoryName { get { return (String)GetValue(typeof(String), "CategoryName"); } set { SetValue("CategoryName", value); } }
        [DataMember]
        public Int32 CategoryStatus { get { return (Int32)GetValue(typeof(Int32), "CategoryStatus"); } set { SetValue("CategoryStatus", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            DemandCategoryVO tmp = new DemandCategoryVO();
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
