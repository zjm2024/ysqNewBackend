using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class PlatformCommissionViewVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(PlatformCommissionViewVO));

        [DataMember]
        public Int32 commissionId { get { return (Int32)GetValue(typeof(Int32), "commissionId"); } set { SetValue("commissionId", value); } }
        [DataMember]
        public Int32 ProjectId { get { return (Int32)GetValue(typeof(Int32), "ProjectId"); } set { SetValue("ProjectId", value); } }
        [DataMember]
        public Decimal ProjectCommission { get { return (Decimal)GetValue(typeof(Decimal), "ProjectCommission"); } set { SetValue("ProjectCommission", value); } }


        [DataMember]
        public Decimal CommissionPercentage { get { return (Decimal)GetValue(typeof(Decimal), "CommissionPercentage"); } set { SetValue("ProjectCommission", value); } }
        [DataMember]       
        public DateTime CommissionDate { get { return (DateTime)GetValue(typeof(DateTime), "CommissionDate"); } set { SetValue("CommissionDate", value); } }
        [DataMember]
        public String projectName { get { return (String)GetValue(typeof(String), "projectName"); } set { SetValue("projectName", value); } }

        [DataMember]
        public String RequirementName { get { return (String)GetValue(typeof(String), "RequirementName"); } set { SetValue("RequirementName", value); } }
  
        #region ICloneable Member
        public override object Clone()
        {
            PlatformCommissionViewVO tmp = new PlatformCommissionViewVO();
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