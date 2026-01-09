using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class TargetCategoryViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(TargetCategoryViewVO));

        [DataMember]
        public Int32 TargetCategoryId { get { return (Int32)GetValue(typeof(Int32), "TargetCategoryId"); } set { SetValue("TargetCategoryId", value); } }
        [DataMember]
        public Int32 BusinessId { get { return (Int32)GetValue(typeof(Int32), "BusinessId"); } set { SetValue("BusinessId", value); } }
        [DataMember]
        public Int32 CategoryId { get { return (Int32)GetValue(typeof(Int32), "CategoryId"); } set { SetValue("CategoryId", value); } }
        [DataMember]
        public String CategoryCode { get { return (String)GetValue(typeof(String), "CategoryCode"); } set { SetValue("CategoryCode", value); } }
        [DataMember]
        public String CategoryName { get { return (String)GetValue(typeof(String), "CategoryName"); } set { SetValue("CategoryName", value); } }
        [DataMember]
        public Int32 ParentCategoryId { get { return (Int32)GetValue(typeof(Int32), "ParentCategoryId"); } set { SetValue("ParentCategoryId", value); } }
        [DataMember]
        public String ParentCategoryName { get { return (String)GetValue(typeof(String), "ParentCategoryName"); } set { SetValue("ParentCategoryName", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            TargetCategoryViewVO tmp = new TargetCategoryViewVO();
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