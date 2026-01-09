using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.RequireManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class TenderInviteViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(TenderInviteViewVO));

        [DataMember]
        public Int32 TenderInviteId { get { return (Int32)GetValue(typeof(Int32), "TenderInviteId"); } set { SetValue("TenderInviteId", value); } }
        [DataMember]
        public Int32 RequirementId { get { return (Int32)GetValue(typeof(Int32), "RequirementId"); } set { SetValue("RequirementId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 AgencyId { get { return (Int32)GetValue(typeof(Int32), "AgencyId"); } set { SetValue("AgencyId", value); } }

        [DataMember]
        public DateTime InviteDate { get { return (DateTime)GetValue(typeof(DateTime), "InviteDate"); } set { SetValue("InviteDate", value); } }
        [DataMember]
        public String CustomerAccount { get { return (String)GetValue(typeof(String), "CustomerAccount"); } set { SetValue("CustomerAccount", value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public String RequirementCode { get { return (String)GetValue(typeof(String), "RequirementCode"); } set { SetValue("RequirementCode", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public String AgencyName { get { return (String)GetValue(typeof(String), "AgencyName"); } set { SetValue("AgencyName", value); } }
        [DataMember]
        public String AgencyLevel { get { return (String)GetValue(typeof(String), "AgencyLevel"); } set { SetValue("AgencyLevel", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            TenderInviteViewVO tmp = new TenderInviteViewVO();
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