using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class ProjectReportFileVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ProjectReportFileVO));

        [DataMember]
        public Int32 ProjectReportFileId { get { return (Int32)GetValue(typeof(Int32), "ProjectReportFileId"); } set { SetValue("ProjectReportFileId", value); } }
        [DataMember]
        public Int32 ProjectId { get { return (Int32)GetValue(typeof(Int32), "ProjectId"); } set { SetValue("ProjectId", value); } }
        [DataMember]
        public String ReportFileName { get { return (String)GetValue(typeof(String), "ReportFileName"); } set { SetValue("ReportFileName", value); } }
        [DataMember]
        public String ReportFilePath { get { return (String)GetValue(typeof(String), "ReportFilePath"); } set { SetValue("ReportFilePath", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public Int32 ReportTypeId { get { return (Int32)GetValue(typeof(Int32), "ReportTypeId"); } set { SetValue("ReportTypeId", value); } }
        [DataMember]
        public DateTime CreatedDate { get { return (DateTime)GetValue(typeof(DateTime), "CreatedDate"); } set { SetValue("CreatedDate", value); } }
        [DataMember]
        public Int32 CreatedBy { get { return (Int32)GetValue(typeof(Int32), "CreatedBy"); } set { SetValue("CreatedBy", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            ProjectReportFileVO tmp = new ProjectReportFileVO();
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