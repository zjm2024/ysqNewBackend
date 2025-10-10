using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class ComplaintsVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ComplaintsVO));

        [DataMember]
        public Int32 ComplaintsId { get { return (Int32)GetValue(typeof(Int32), "ComplaintsId"); } set { SetValue("ComplaintsId", value); } }
        [DataMember]
        public Int32 ProjectId { get { return (Int32)GetValue(typeof(Int32), "ProjectId"); } set { SetValue("ProjectId", value); } }
        [DataMember]
        public Int32 Creator { get { return (Int32)GetValue(typeof(Int32), "Creator"); } set { SetValue("Creator", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 UpdatedBy { get { return (Int32)GetValue(typeof(Int32), "UpdatedBy"); } set { SetValue("UpdatedBy", value); } }
        [DataMember]
        public DateTime UpdatedAt { get { return (DateTime)GetValue(typeof(DateTime), "UpdatedAt"); } set { SetValue("UpdatedAt", value); } }
        [DataMember]
        public String Reason { get { return (String)GetValue(typeof(String), "Reason"); } set { SetValue("Reason", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            ComplaintsVO tmp = new ComplaintsVO();
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