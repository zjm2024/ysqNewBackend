using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class VisitViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(VisitViewVO));

        [DataMember]
        public Int32 VisitID { get { return (Int32)GetValue(typeof(Int32), "VisitID"); } set { SetValue("VisitID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 ToPersonalID { get { return (Int32)GetValue(typeof(Int32), "ToPersonalID"); } set { SetValue("ToPersonalID", value); } }
        [DataMember]
        public String Type { get { return (String)GetValue(typeof(String), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
		public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status") ; } set {  SetValue("Status", value); } }
        [DataMember]
        public String Img { get { return (String)GetValue(typeof(String), "Img"); } set { SetValue("Img", value); } }
        [DataMember]
        public String Remark { get { return (String)GetValue(typeof(String), "Remark"); } set { SetValue("Remark", value); } }
        [DataMember]
        public String Remark1 { get { return (String)GetValue(typeof(String), "Remark1"); } set { SetValue("Remark1", value); } }
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
		public Int32 TransactionAt { get { return (Int32)GetValue(typeof(Int32), "TransactionAt") ; } set {  SetValue("TransactionAt", value); } }
        [DataMember]
        public DateTime AccessAt { get { return (DateTime)GetValue(typeof(DateTime), "AccessAt"); } set { SetValue("AccessAt", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }
        [DataMember]
        public String Name { get { return (String)GetValue(typeof(String), "Name"); } set { SetValue("Name", value); } }
        [DataMember]
        public String Phone { get { return (String)GetValue(typeof(String), "Phone"); } set { SetValue("Phone", value); } }
        [DataMember]
        public String Headimg { get { return (String)GetValue(typeof(String), "Headimg"); } set { SetValue("Headimg", value); } }
        [DataMember]
        public String Position { get { return (String)GetValue(typeof(String), "Position"); } set { SetValue("Position", value); } }
        [DataMember]
        public String CompanyAddress { get { return (String)GetValue(typeof(String), "CompanyAddress"); } set { SetValue("CompanyAddress", value); } }
        [DataMember]
        public String BusinessName { get { return (String)GetValue(typeof(String), "BusinessName"); } set { SetValue("BusinessName", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public String VisitName { get { return (String)GetValue(typeof(String), "VisitName"); } set { SetValue("VisitName", value); } }
        [DataMember]
        public String VisitPhone { get { return (String)GetValue(typeof(String), "VisitPhone"); } set { SetValue("VisitPhone", value); } }
        [DataMember]
        public DateTime VisitDate { get { return (DateTime)GetValue(typeof(DateTime), "VisitDate"); } set { SetValue("VisitDate", value); } }

        [DataMember]
        public String ToName { get { return (String)GetValue(typeof(String), "ToName"); } set { SetValue("ToName", value); } }
        [DataMember]
        public String ToPhone { get { return (String)GetValue(typeof(String), "ToPhone"); } set { SetValue("ToPhone", value); } }
        [DataMember]
        public String ToHeadimg { get { return (String)GetValue(typeof(String), "ToHeadimg"); } set { SetValue("ToHeadimg", value); } }
        [DataMember]
        public String ToPosition { get { return (String)GetValue(typeof(String), "ToPosition"); } set { SetValue("ToPosition", value); } }
        [DataMember]
        public String ToAddress { get { return (String)GetValue(typeof(String), "ToAddress"); } set { SetValue("ToAddress", value); } }
        [DataMember]
        public Int32 ToBusinessID { get { return (Int32)GetValue(typeof(Int32), "ToBusinessID"); } set { SetValue("ToBusinessID", value); } }


        #region ICloneable Member
        public override object Clone()
        {
            VisitViewVO tmp = new VisitViewVO();
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