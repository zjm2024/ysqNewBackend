using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class CommissionIncomeViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CommissionIncomeViewVO));

        [DataMember]
        public Int32 commissionInComeId { get { return (Int32)GetValue(typeof(Int32), "commissionInComeId"); } set { SetValue("commissionInComeId", value); } }
        [DataMember]
        public Int32 ProjectId { get { return (Int32)GetValue(typeof(Int32), "ProjectId"); } set { SetValue("ProjectId", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Decimal Commission { get { return (Decimal)GetValue(typeof(Decimal), "Commission"); } set { SetValue("Commission", value); } }
        [DataMember]
        public DateTime PayDate { get { return (DateTime)GetValue(typeof(DateTime), "PayDate"); } set { SetValue("PayDate", value); } }

        [DataMember]
        public String projectName { get { return (String)GetValue(typeof(String), "projectName"); } set { SetValue("projectName", value); } }

        [DataMember]
        public String Purpose { get { return (String)GetValue(typeof(String), "Purpose"); } set { SetValue("Purpose", value); } }
        [DataMember]
        public String RequirementName { get { return (String)GetValue(typeof(String), "RequirementName"); } set { SetValue("RequirementName", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            CommissionIncomeViewVO tmp = new CommissionIncomeViewVO();
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