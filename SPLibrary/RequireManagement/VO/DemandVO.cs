using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.RequireManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class DemandVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(DemandVO));

        [DataMember]
        public Int32 DemandId { get { return (Int32)GetValue(typeof(Int32), "DemandId"); } set { SetValue("DemandId", value); } }
        [DataMember]
        public Int32 CategoryId { get { return (Int32)GetValue(typeof(Int32), "CategoryId"); } set { SetValue("CategoryId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public DateTime EffectiveEndDate { get { return (DateTime)GetValue(typeof(DateTime), "EffectiveEndDate"); } set { SetValue("EffectiveEndDate", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public Boolean isEndDate { get { return (Boolean)GetValue(typeof(Boolean), "isEndDate"); } set { SetValue("isEndDate", value); } }
        [DataMember]
        public String Image { get { return (String)GetValue(typeof(String), "Image"); } set { SetValue("Image", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public String Province { get { return (String)GetValue(typeof(String), "Province"); } set { SetValue("Province", value); } }
        [DataMember]
        public String City { get { return (String)GetValue(typeof(String), "City"); } set { SetValue("City", value); } }
        [DataMember]
        public Int32 ReadCount { get { return (Int32)GetValue(typeof(Int32), "ReadCount"); } set { SetValue("ReadCount", value); } }

        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            DemandVO tmp = new DemandVO();
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
