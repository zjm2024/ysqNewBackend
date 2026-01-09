using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class AgencyCategoryViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencyCategoryViewVO));

        [DataMember]
        public Int32 AgencyCategoryId { get { return (Int32)GetValue(typeof(Int32), "AgencyCategoryId"); } set { SetValue("AgencyCategoryId", value); } }
        [DataMember]
        public Int32 AgencyId { get { return (Int32)GetValue(typeof(Int32), "AgencyId"); } set { SetValue("AgencyId", value); } }
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
            AgencyCategoryViewVO tmp = new AgencyCategoryViewVO();
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