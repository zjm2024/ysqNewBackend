using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class SecurityActionVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(SecurityActionVO));
       
		[DataMember]
		public Int32 SecurityActionId { get { return (Int32)GetValue(typeof(Int32),"SecurityActionId") ; } set {  SetValue("SecurityActionId",value); } }
		[DataMember]
		public String ActionCode { get { return (String)GetValue(typeof(String),"ActionCode") ; } set {  SetValue("ActionCode",value); } }
		[DataMember]
		public String ActionName { get { return (String)GetValue(typeof(String),"ActionName") ; } set {  SetValue("ActionName",value); } }
    		
    	#region ICloneable Member
    	public override object Clone()
        {
            SecurityActionVO tmp = new SecurityActionVO();
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