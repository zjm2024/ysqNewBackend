using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class ToolFileVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ToolFileVO));

        [DataMember]
        public Int32 ToolFileId { get { return (Int32)GetValue(typeof(Int32), "ToolFileId"); } set { SetValue("ToolFileId", value); } }
        [DataMember]
        public String FileName { get { return (String)GetValue(typeof(String), "FileName"); } set { SetValue("FileName", value); } }
        [DataMember]
        public String FilePath { get { return (String)GetValue(typeof(String), "FilePath"); } set { SetValue("FilePath", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public Int32 TypeId { get { return (Int32)GetValue(typeof(Int32), "TypeId"); } set { SetValue("TypeId", value); } }
        [DataMember]
        public DateTime CreatedDate { get { return (DateTime)GetValue(typeof(DateTime), "CreatedDate"); } set { SetValue("CreatedDate", value); } }
        [DataMember]
        public Int32 CreatedBy { get { return (Int32)GetValue(typeof(Int32), "CreatedBy"); } set { SetValue("CreatedBy", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            ToolFileVO tmp = new ToolFileVO();
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