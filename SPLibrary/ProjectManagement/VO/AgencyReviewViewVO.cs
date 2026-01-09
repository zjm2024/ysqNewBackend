using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class AgencyReviewViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencyReviewViewVO));

        [DataMember]
        public Int32 AgencyReviewId { get { return (Int32)GetValue(typeof(Int32), "AgencyReviewId"); } set { SetValue("AgencyReviewId", value); } }
        [DataMember]
        public Int32 ProjectId { get { return (Int32)GetValue(typeof(Int32), "ProjectId"); } set { SetValue("ProjectId", value); } }
        [DataMember]
        public Int32 RequirementId { get { return (Int32)GetValue(typeof(Int32), "RequirementId"); } set { SetValue("RequirementId", value); } }
        [DataMember]
        public Int32 AgencyId { get { return (Int32)GetValue(typeof(Int32), "AgencyId"); } set { SetValue("AgencyId", value); } }
        [DataMember]
        public Int32 BusinessId { get { return (Int32)GetValue(typeof(Int32), "BusinessId"); } set { SetValue("BusinessId", value); } }
        [DataMember]
        public Decimal Score { get { return (Decimal)GetValue(typeof(Decimal), "Score"); } set { SetValue("Score", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public String AddNote { get { return (String)GetValue(typeof(String), "AddNote"); } set { SetValue("AddNote", value); } }
        [DataMember]
        public DateTime AddNoteAt { get { return (DateTime)GetValue(typeof(DateTime), "AddNoteAt"); } set { SetValue("AddNoteAt", value); } }
        [DataMember]
        public String ProjectCode { get { return (String)GetValue(typeof(String), "ProjectCode"); } set { SetValue("ProjectCode", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public Int32 BusinessCustomerId { get { return (Int32)GetValue(typeof(Int32), "BusinessCustomerId"); } set { SetValue("BusinessCustomerId", value); } }
        [DataMember]
        public String BusinessCustomerCode { get { return (String)GetValue(typeof(String), "BusinessCustomerCode"); } set { SetValue("BusinessCustomerCode", value); } }
        [DataMember]
        public String BusinessCustomerName { get { return (String)GetValue(typeof(String), "BusinessCustomerName"); } set { SetValue("BusinessCustomerName", value); } }
        [DataMember]
        public Int32 AgencyCustomerId { get { return (Int32)GetValue(typeof(Int32), "AgencyCustomerId"); } set { SetValue("AgencyCustomerId", value); } }
        [DataMember]
        public String AgencyCustomerCode { get { return (String)GetValue(typeof(String), "AgencyCustomerCode"); } set { SetValue("AgencyCustomerCode", value); } }
        [DataMember]
        public String AgencyCustomerName { get { return (String)GetValue(typeof(String), "AgencyCustomerName"); } set { SetValue("AgencyCustomerName", value); } }

        [DataMember]
        public List<AgencyReviewDetailVO> AgencyReviewDetailList { get; set; }
        #region ICloneable Member
        public override object Clone()
        {
            AgencyReviewViewVO tmp = new AgencyReviewViewVO();
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