using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class AgencyReviewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencyReviewVO));

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
        public List<AgencyReviewDetailVO> AgencyReviewDetailList { get; set; }
        #region ICloneable Member
        public override object Clone()
        {
            AgencyReviewVO tmp = new AgencyReviewVO();
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