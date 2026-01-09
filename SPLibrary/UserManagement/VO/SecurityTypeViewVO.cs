using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class SecurityTypeViewVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(SecurityTypeViewVO));
       
		[DataMember]
		public String GroupTypeName { get { return (String)GetValue(typeof(String),"GroupTypeName") ; } set {  SetValue("GroupTypeName",value); } }
		[DataMember]
		public Int32 GroupSortNumber { get { return (Int32)GetValue(typeof(Int32),"GroupSortNumber") ; } set {  SetValue("GroupSortNumber",value); } }
		[DataMember]
		public Int32 SecurityTypeId { get { return (Int32)GetValue(typeof(Int32),"SecurityTypeId") ; } set {  SetValue("SecurityTypeId",value); } }
		[DataMember]
		public String SecurityTypeName { get { return (String)GetValue(typeof(String),"SecurityTypeName") ; } set {  SetValue("SecurityTypeName",value); } }
		[DataMember]
		public Int32 ParentSecurityTypeId { get { return (Int32)GetValue(typeof(Int32),"ParentSecurityTypeId") ; } set {  SetValue("ParentSecurityTypeId",value); } }
		[DataMember]
		public Int32 GroupTypeId { get { return (Int32)GetValue(typeof(Int32),"GroupTypeId") ; } set {  SetValue("GroupTypeId",value); } }
		[DataMember]
		public String SecurityTypeCode { get { return (String)GetValue(typeof(String),"SecurityTypeCode") ; } set {  SetValue("SecurityTypeCode",value); } }
		[DataMember]
		public String SecurityTypeValue { get { return (String)GetValue(typeof(String),"SecurityTypeValue") ; } set {  SetValue("SecurityTypeValue",value); } }
		[DataMember]
		public Boolean IsSystem { get { return (Boolean)GetValue(typeof(Boolean),"IsSystem") ; } set {  SetValue("IsSystem",value); } }
		[DataMember]
		public Int32 SortNumber { get { return (Int32)GetValue(typeof(Int32),"SortNumber") ; } set {  SetValue("SortNumber",value); } }
        [DataMember]
        public List<SecurityViewVO> SecurityVOList { get; set; }
        #region ICloneable Member
        public override object Clone()
        {
            SecurityTypeViewVO tmp = new SecurityTypeViewVO();
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