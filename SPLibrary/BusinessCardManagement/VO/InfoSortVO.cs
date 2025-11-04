using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class InfoSortVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(InfoSortVO));

        [DataMember]
        public Int32 SortID { get { return (Int32)GetValue(typeof(Int32), "SortID"); } set { SetValue("SortID", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public Int32 Toid { get { return (Int32)GetValue(typeof(Int32), "Toid"); } set { SetValue("Toid", value); } }
        [DataMember]
        public String Type { get { return (String)GetValue(typeof(String), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public String SortName { get { return (String)GetValue(typeof(String), "SortName"); } set { SetValue("SortName", value); } }
        [DataMember]
        public String Image { get { return (String)GetValue(typeof(String), "Image"); } set { SetValue("Image", value); } }
        [DataMember]
        public String Content { get { return (String)GetValue(typeof(String), "Content"); } set { SetValue("Content", value); } }
        [DataMember]
        public Int32 orderno { get { return (Int32)GetValue(typeof(Int32), "orderno"); } set { SetValue("orderno", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String Remark { get { return (String)GetValue(typeof(String), "Remark"); } set { SetValue("Remark", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }
        [DataMember]
        public List<InfoVO> Infolist { get; set; }
        [DataMember]
        public List<InfoViewVO> InfoViewlist { get; set; }
        [DataMember]
        public List<InfoSortVO> InfoSortlist { get; set; }

        #region ICloneable Member
        public override object Clone()
        {
            InfoSortVO tmp = new InfoSortVO();
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