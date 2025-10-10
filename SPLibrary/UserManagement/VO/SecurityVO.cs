using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class SecurityVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(SecurityVO));
       
		[DataMember]
		public Int32 SecurityId { get { return (Int32)GetValue(typeof(Int32),"SecurityId") ; } set {  SetValue("SecurityId",value); } }
		[DataMember]
		public Int32 SecurityTypeId { get { return (Int32)GetValue(typeof(Int32),"SecurityTypeId") ; } set {  SetValue("SecurityTypeId",value); } }
		[DataMember]
		public String SecurityCode { get { return (String)GetValue(typeof(String),"SecurityCode") ; } set {  SetValue("SecurityCode",value); } }
		[DataMember]
		public String SecurityName { get { return (String)GetValue(typeof(String),"SecurityName") ; } set {  SetValue("SecurityName",value); } }
    		
    	#region ICloneable Member
    	public override object Clone()
        {
            SecurityVO tmp = new SecurityVO();
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