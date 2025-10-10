using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class SecurityViewVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(SecurityViewVO));
       
		[DataMember]
		public Int32 GroupTypeId { get { return (Int32)GetValue(typeof(Int32),"GroupTypeId") ; } set {  SetValue("GroupTypeId",value); } }
		[DataMember]
		public Boolean IsSystem { get { return (Boolean)GetValue(typeof(Boolean),"IsSystem") ; } set {  SetValue("IsSystem",value); } }
		[DataMember]
		public String SecurityTypeName { get { return (String)GetValue(typeof(String),"SecurityTypeName") ; } set {  SetValue("SecurityTypeName",value); } }
		[DataMember]
		public String SecurityTypeCode { get { return (String)GetValue(typeof(String),"SecurityTypeCode") ; } set {  SetValue("SecurityTypeCode",value); } }
		[DataMember]
		public String SecurityTypeValue { get { return (String)GetValue(typeof(String),"SecurityTypeValue") ; } set {  SetValue("SecurityTypeValue",value); } }
		[DataMember]
		public Int32 SecurityId { get { return (Int32)GetValue(typeof(Int32),"SecurityId") ; } set {  SetValue("SecurityId",value); } }
		[DataMember]
		public Int32 SecurityTypeId { get { return (Int32)GetValue(typeof(Int32),"SecurityTypeId") ; } set {  SetValue("SecurityTypeId",value); } }
		[DataMember]
		public Int32 SecurityActionId { get { return (Int32)GetValue(typeof(Int32),"SecurityActionId") ; } set {  SetValue("SecurityActionId",value); } }
		[DataMember]
		public String ActionCode { get { return (String)GetValue(typeof(String),"ActionCode") ; } set {  SetValue("ActionCode",value); } }
		[DataMember]
		public String ActionName { get { return (String)GetValue(typeof(String),"ActionName") ; } set {  SetValue("ActionName",value); } }
    		
    	#region ICloneable Member
    	public override object Clone()
        {
            SecurityViewVO tmp = new SecurityViewVO();
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