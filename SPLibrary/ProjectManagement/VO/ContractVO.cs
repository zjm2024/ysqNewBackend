using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class ContractVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ContractVO));

        [DataMember]
        public Int32 ContractId { get { return (Int32)GetValue(typeof(Int32), "ContractId"); } set { SetValue("ContractId", value); } }
        [DataMember]
        public Int32 RequirementId { get { return (Int32)GetValue(typeof(Int32), "RequirementId"); } set { SetValue("RequirementId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public String ProjectName { get { return (String)GetValue(typeof(String), "ProjectName"); } set { SetValue("ProjectName", value); } }
        [DataMember]
        public DateTime StartDate { get { return (DateTime)GetValue(typeof(DateTime), "StartDate"); } set { SetValue("StartDate", value); } }
        [DataMember]
        public DateTime EndDate { get { return (DateTime)GetValue(typeof(DateTime), "EndDate"); } set { SetValue("EndDate", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public Decimal Commission { get { return (Decimal)GetValue(typeof(Decimal), "Commission"); } set { SetValue("Commission", value); } }
        [DataMember]
        public String ContractNote { get { return (String)GetValue(typeof(String), "ContractNote"); } set { SetValue("ContractNote", value); } }
        [DataMember]
        public Int32 AgencyStatus { get { return (Int32)GetValue(typeof(Int32), "AgencyStatus"); } set { SetValue("AgencyStatus", value); } }
        [DataMember]
        public Int32 BusinessStatus { get { return (Int32)GetValue(typeof(Int32), "BusinessStatus"); } set { SetValue("BusinessStatus", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public String ContractFile { get { return (String)GetValue(typeof(String), "ContractFile"); } set { SetValue("ContractFile", value); } }
        [DataMember]
        public DateTime AgencySignDate { get { return (DateTime)GetValue(typeof(DateTime), "AgencySignDate"); } set { SetValue("AgencySignDate", value); } }
        [DataMember]
        public DateTime BusinessSignDate { get { return (DateTime)GetValue(typeof(DateTime), "BusinessSignDate"); } set { SetValue("BusinessSignDate", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            ContractVO tmp = new ContractVO();
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