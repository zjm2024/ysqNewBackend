using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class ProjectActionVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ProjectActionVO));

        [DataMember]
        public Int32 ProjectActionId { get { return (Int32)GetValue(typeof(Int32), "ProjectActionId"); } set { SetValue("ProjectActionId", value); } }
        [DataMember]
        public Int32 ProjectId { get { return (Int32)GetValue(typeof(Int32), "ProjectId"); } set { SetValue("ProjectId", value); } }
        [DataMember]
        public String ActionType { get { return (String)GetValue(typeof(String), "ActionType"); } set { SetValue("ActionType", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public DateTime ActionDate { get { return (DateTime)GetValue(typeof(DateTime), "ActionDate"); } set { SetValue("ActionDate", value); } }
        [DataMember]
        public Int32 ActionBy { get { return (Int32)GetValue(typeof(Int32), "ActionBy"); } set { SetValue("ActionBy", value); } }
        [DataMember]
        public List<ProjectActionFileVO> ProjectActionFileList { get; set; }
        #region ICloneable Member
        public override object Clone()
        {
            ProjectActionVO tmp = new ProjectActionVO();
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