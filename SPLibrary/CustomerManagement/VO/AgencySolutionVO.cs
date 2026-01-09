using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class AgencySolutionVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencySolutionVO));

        [DataMember]
        public Int32 AgencySolutionId { get { return (Int32)GetValue(typeof(Int32), "AgencySolutionId"); } set { SetValue("AgencySolutionId", value); } }
        [DataMember]
        public Int32 AgencyId { get { return (Int32)GetValue(typeof(Int32), "AgencyId"); } set { SetValue("AgencyId", value); } }
        [DataMember]
        public String ClientName { get { return (String)GetValue(typeof(String), "ClientName"); } set { SetValue("ClientName", value); } }
        [DataMember]
        public String ProjectName { get { return (String)GetValue(typeof(String), "ProjectName"); } set { SetValue("ProjectName", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public DateTime ProjectDate { get { return (DateTime)GetValue(typeof(DateTime), "ProjectDate"); } set { SetValue("ProjectDate", value); } }
        [DataMember]
        public List<AgencySolutionFileVO> AgencySolutionFileList{ get; set;}
        [DataMember]
        public Int32 PrivacyType { get { return (Int32)GetValue(typeof(Int32), "PrivacyType"); } set { SetValue("PrivacyType", value); } }
        [DataMember]
        public String Keyword { get { return (String)GetValue(typeof(String), "Keyword"); } set { SetValue("Keyword", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            AgencySolutionVO tmp = new AgencySolutionVO();
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