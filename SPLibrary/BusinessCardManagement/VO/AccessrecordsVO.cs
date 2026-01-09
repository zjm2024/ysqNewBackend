using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AccessrecordsVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AccessrecordsVO));
        
		[DataMember]
		public Int32 AccessRecordsID { get { return (Int32)GetValue(typeof(Int32), "AccessRecordsID") ; } set {  SetValue("AccessRecordsID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 ToPersonalID { get { return (Int32)GetValue(typeof(Int32), "ToPersonalID"); } set { SetValue("ToPersonalID", value); } }
        [DataMember]
        public DateTime AccessAt { get { return (DateTime)GetValue(typeof(DateTime), "AccessAt"); } set { SetValue("AccessAt", value); } }
        [DataMember]
        public Int32 ResidenceAt { get { return (Int32)GetValue(typeof(Int32), "ResidenceAt"); } set { SetValue("ResidenceAt", value); } }
        [DataMember]
        public String Type { get { return (String)GetValue(typeof(String), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public Int32 ById { get { return (Int32)GetValue(typeof(Int32), "ById"); } set { SetValue("ById", value); } }
        [DataMember]
        public Int32 Counts { get { return (Int32)GetValue(typeof(Int32), "Counts"); } set { SetValue("Counts", value); } }
        [DataMember]
        public Int32 ReturnCounts { get { return (Int32)GetValue(typeof(Int32), "ReturnCounts"); } set { SetValue("ReturnCounts", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            AccessrecordsVO tmp = new AccessrecordsVO();
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