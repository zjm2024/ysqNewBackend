using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class CommissionDelegationViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CommissionDelegationViewVO));

        [DataMember]
        public Int32 CommissionDelegationId { get { return (Int32)GetValue(typeof(Int32), "CommissionDelegationId"); } set { SetValue("CommissionDelegationId", value); } }
        [DataMember]
        public Int32 ProjectId { get { return (Int32)GetValue(typeof(Int32), "ProjectId"); } set { SetValue("ProjectId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public DateTime DelegationDate { get { return (DateTime)GetValue(typeof(DateTime), "DelegationDate"); } set { SetValue("DelegationDate", value); } }
        [DataMember]
        public Decimal Commission { get { return (Decimal)GetValue(typeof(Decimal), "Commission"); } set { SetValue("Commission", value); } }
        [DataMember]
        public Decimal PlatformCommission { get { return (Decimal)GetValue(typeof(Decimal), "PlatformCommission"); } set { SetValue("PlatformCommission", value); } }
        [DataMember]
        public Decimal TotalCommission { get { return (Decimal)GetValue(typeof(Decimal), "TotalCommission"); } set { SetValue("TotalCommission", value); } }
        [DataMember]
        public DateTime PayDate { get { return (DateTime)GetValue(typeof(DateTime), "PayDate"); } set { SetValue("PayDate", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public String Purpose { get { return (String)GetValue(typeof(String), "Purpose"); } set { SetValue("Purpose", value); } }
        [DataMember]
        public String projectName { get { return (String)GetValue(typeof(String), "projectName"); } set { SetValue("projectName", value); } }
        [DataMember]
        public String RequirementName { get { return (String)GetValue(typeof(String), "RequirementName"); } set { SetValue("RequirementName", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            CommissionDelegationViewVO tmp = new CommissionDelegationViewVO();
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