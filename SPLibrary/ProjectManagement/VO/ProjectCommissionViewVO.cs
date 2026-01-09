using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class ProjectCommissionViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ProjectCommissionViewVO));

        [DataMember]
        public Int32 ProjectCommissionId { get { return (Int32)GetValue(typeof(Int32), "ProjectCommissionId"); } set { SetValue("ProjectCommissionId", value); } }
        [DataMember]
        public Int32 ProjectId { get { return (Int32)GetValue(typeof(Int32), "ProjectId"); } set { SetValue("ProjectId", value); } }
        [DataMember]
        public Decimal Commission { get { return (Decimal)GetValue(typeof(Decimal), "Commission"); } set { SetValue("Commission", value); } }
        [DataMember]
        public String Reason { get { return (String)GetValue(typeof(String), "Reason"); } set { SetValue("Reason", value); } }
        [DataMember]
        public String RejectReason { get { return (String)GetValue(typeof(String), "RejectReason"); } set { SetValue("RejectReason", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 CreatedBy { get { return (Int32)GetValue(typeof(Int32), "CreatedBy"); } set { SetValue("CreatedBy", value); } }
        [DataMember]
        public DateTime RejectAt { get { return (DateTime)GetValue(typeof(DateTime), "RejectAt"); } set { SetValue("RejectAt", value); } }
        [DataMember]
        public Int32 RejectBy { get { return (Int32)GetValue(typeof(Int32), "RejectBy"); } set { SetValue("RejectBy", value); } }
        [DataMember]
        public DateTime PayDate { get { return (DateTime)GetValue(typeof(DateTime), "PayDate"); } set { SetValue("PayDate", value); } }
        [DataMember]
        public String ProjectCode { get { return (String)GetValue(typeof(String), "ProjectCode"); } set { SetValue("ProjectCode", value); } }
        [DataMember]
        public String Creator { get { return (String)GetValue(typeof(String), "Creator"); } set { SetValue("Creator", value); } }
        [DataMember]
        public String Rejector { get { return (String)GetValue(typeof(String), "Rejector"); } set { SetValue("Rejector", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            ProjectCommissionViewVO tmp = new ProjectCommissionViewVO();
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