using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class AgencyExperienceVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencyExperienceVO));

        [DataMember]
        public Int32 AgencyExperienceId { get { return (Int32)GetValue(typeof(Int32), "AgencyExperienceId"); } set { SetValue("AgencyExperienceId", value); } }
        [DataMember]
        public Int32 AgencyId { get { return (Int32)GetValue(typeof(Int32), "AgencyId"); } set { SetValue("AgencyId", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public DateTime ProjectDate { get { return (DateTime)GetValue(typeof(DateTime), "ProjectDate"); } set { SetValue("ProjectDate", value); } }
        [DataMember]
        public String Description { get { return (String)GetValue(typeof(String), "Description"); } set { SetValue("Description", value); } }
        [DataMember]
        public String MainImg { get { return (String)GetValue(typeof(String), "MainImg"); } set { SetValue("MainImg", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public String ClientName { get { return (String)GetValue(typeof(String), "ClientName"); } set { SetValue("ClientName", value); } }
        [DataMember]
        public Decimal ContractAmount { get { return (Decimal)GetValue(typeof(Decimal), "ContractAmount"); } set { SetValue("ContractAmount", value); } }
        [DataMember]
        public List<AgencyExperienceImageVO> AgencyExperienceImageList { get; set; }
        #region ICloneable Member
        public override object Clone()
        {
            AgencyExperienceVO tmp = new AgencyExperienceVO();
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