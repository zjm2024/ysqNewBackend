using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CrmVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CrmVO));
        
		[DataMember]
		public Int32 CrmID { get { return (Int32)GetValue(typeof(Int32), "CrmID") ; } set {  SetValue("CrmID", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 PunchID { get { return (Int32)GetValue(typeof(Int32), "PunchID"); } set { SetValue("PunchID", value); } }
        [DataMember]
        public String ForPersonalID { get { return (String)GetValue(typeof(String), "ForPersonalID"); } set { SetValue("ForPersonalID", value); } }
        [DataMember]
        public String ApprovalPersonalID { get { return (String)GetValue(typeof(String), "ApprovalPersonalID"); } set { SetValue("ApprovalPersonalID", value); } }
        [DataMember]
        public String DisapprovalPersonalID { get { return (String)GetValue(typeof(String), "DisapprovalPersonalID"); } set { SetValue("DisapprovalPersonalID", value); } }
        
        [DataMember]
        public String Content { get { return (String)GetValue(typeof(String), "Content"); } set { SetValue("Content", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String Type { get { return (String)GetValue(typeof(String), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public Int32 Order_info { get { return (Int32)GetValue(typeof(Int32), "Order_info"); } set { SetValue("Order_info", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public Decimal priceA { get { return (Decimal)GetValue(typeof(Decimal), "priceA"); } set { SetValue("priceA", value); } }
        [DataMember]
        public Decimal priceB { get { return (Decimal)GetValue(typeof(Decimal), "priceB"); } set { SetValue("priceB", value); } }
        [DataMember]
        public String Field1 { get { return (String)GetValue(typeof(String), "Field1"); } set { SetValue("Field1", value); } }
        [DataMember]
        public String Field2 { get { return (String)GetValue(typeof(String), "Field2"); } set { SetValue("Field2", value); } }
        [DataMember]
        public String Field3 { get { return (String)GetValue(typeof(String), "Field3"); } set { SetValue("Field3", value); } }
        [DataMember]
        public String Field4 { get { return (String)GetValue(typeof(String), "Field4"); } set { SetValue("Field4", value); } }
        [DataMember]
        public String Field5 { get { return (String)GetValue(typeof(String), "Field5"); } set { SetValue("Field5", value); } }
        [DataMember]
        public String Field6 { get { return (String)GetValue(typeof(String), "Field6"); } set { SetValue("Field6", value); } }
        [DataMember]
        public String Field7 { get { return (String)GetValue(typeof(String), "Field7"); } set { SetValue("Field7", value); } }
        [DataMember]
        public String Field8 { get { return (String)GetValue(typeof(String), "Field8"); } set { SetValue("Field8", value); } }
        [DataMember]
        public String Field9 { get { return (String)GetValue(typeof(String), "Field9"); } set { SetValue("Field9", value); } }
        [DataMember]
        public String Field10 { get { return (String)GetValue(typeof(String), "Field10"); } set { SetValue("Field10", value); } }
        [DataMember]
        public String Field11 { get { return (String)GetValue(typeof(String), "Field11"); } set { SetValue("Field11", value); } }
        [DataMember]
        public Decimal Latitude { get { return (Decimal)GetValue(typeof(Decimal), "Latitude"); } set { SetValue("Latitude", value); } }
        [DataMember]
        public Decimal Longitude { get { return (Decimal)GetValue(typeof(Decimal), "Longitude"); } set { SetValue("Longitude", value); } }
        [DataMember]
        public DateTime StartTime { get { return (DateTime)GetValue(typeof(DateTime), "StartTime"); } set { SetValue("StartTime", value); } }
        [DataMember]
        public DateTime EndTime { get { return (DateTime)GetValue(typeof(DateTime), "EndTime"); } set { SetValue("EndTime", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CrmVO tmp = new CrmVO();
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