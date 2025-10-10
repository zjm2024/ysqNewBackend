using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;
using System.Numerics;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class VisitVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(VisitVO));

        [DataMember]
        public Int32 VisitID { get { return (Int32)GetValue(typeof(Int32), "VisitID"); } set { SetValue("VisitID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 ToPersonalID { get { return (Int32)GetValue(typeof(Int32), "ToPersonalID"); } set { SetValue("ToPersonalID", value); } }
        [DataMember]
        public String Type { get { return (String)GetValue(typeof(String), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public String ById { get { return (String)GetValue(typeof(String), "ById"); } set { SetValue("ById", value); } }
        [DataMember]
		public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status") ; } set {  SetValue("Status", value); } }
        [DataMember]
        public String CompanyName { get { return (String)GetValue(typeof(String), "CompanyName"); } set { SetValue("CompanyName", value); } }
        [DataMember]
        public String Address { get { return (String)GetValue(typeof(String), "Address"); } set { SetValue("Address", value); } }
        [DataMember]
        public String DetailedAddress { get { return (String)GetValue(typeof(String), "DetailedAddress"); } set { SetValue("DetailedAddress", value); } }
        [DataMember]
        public Decimal latitude { get { return (Decimal)GetValue(typeof(Decimal), "Latitude"); } set { SetValue("Latitude", value); } }
        [DataMember]
        public Decimal longitude { get { return (Decimal)GetValue(typeof(Decimal), "Longitude"); } set { SetValue("Longitude", value); } }
        [DataMember]
        public String Img { get { return (String)GetValue(typeof(String), "Img"); } set { SetValue("Img", value); } }
        [DataMember]
        public String VisitName { get { return (String)GetValue(typeof(String), "VisitName"); } set { SetValue("VisitName", value); } }
        [DataMember]
        public String VisitPhone { get { return (String)GetValue(typeof(String), "VisitPhone"); } set { SetValue("VisitPhone", value); } }
        [DataMember]
        public DateTime VisitDate { get { return (DateTime)GetValue(typeof(DateTime), "VisitDate"); } set { SetValue("VisitDate", value); } }
        [DataMember]
        public String Remark { get { return (String)GetValue(typeof(String), "Remark"); } set { SetValue("Remark", value); } }
        [DataMember]
        public String Remark1 { get { return (String)GetValue(typeof(String), "Remark1"); } set { SetValue("Remark1", value); } }
        [DataMember]
        public DateTime AccessAt { get { return (DateTime)GetValue(typeof(DateTime), "AccessAt"); } set { SetValue("AccessAt", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            VisitVO tmp = new VisitVO();
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