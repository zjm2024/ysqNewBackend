using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class TargetVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(TargetVO));
       
		[DataMember]
		public Int32 TargetID { get { return (Int32)GetValue(typeof(Int32), "TargetID") ; } set {  SetValue("TargetID", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public String Type { get { return (String)GetValue(typeof(String), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
        public Int32 PersonalID { get { return (Int32)GetValue(typeof(Int32), "PersonalID"); } set { SetValue("PersonalID", value); } }
        [DataMember]
        public Int32 DepartmentID { get { return (Int32)GetValue(typeof(Int32), "DepartmentID"); } set { SetValue("DepartmentID", value); } }
        [DataMember]
        public Int32 Year { get { return (Int32)GetValue(typeof(Int32), "Year"); } set { SetValue("Year", value); } }
        [DataMember]
        public Int32 Month { get { return (Int32)GetValue(typeof(Int32), "Month"); } set { SetValue("Month", value); } }
        [DataMember]
        public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost"); } set { SetValue("Cost", value); } }
        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String Headimg { get; set; }
        [DataMember]
        public String DepartmentName { get; set; }

        #region ICloneable Member
        public override object Clone()
        {
            TargetVO tmp = new TargetVO();
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