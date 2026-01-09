using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class HelpDocTypeVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(HelpDocTypeVO));
       
		[DataMember]
		 public Int32 HelpDocTypeId { get { return (Int32)GetValue(typeof(Int32),"HelpDocTypeId") ; } set {  SetValue("HelpDocTypeId",value); } } 		[DataMember]
		 public Int32 ParentHelpDocTypeId { get { return (Int32)GetValue(typeof(Int32),"ParentHelpDocTypeId") ; } set {  SetValue("ParentHelpDocTypeId",value); } } 		[DataMember]
		 public String HelpDocTypeName { get { return (String)GetValue(typeof(String),"HelpDocTypeName") ; } set {  SetValue("HelpDocTypeName",value); } } 		[DataMember]
		public Boolean Status { get { return (Boolean)GetValue(typeof(Boolean),"Status") ; } set {  SetValue("Status",value); } }
		    		
    	#region ICloneable Member
    	public override object Clone()
        {
            HelpDocTypeVO tmp = new HelpDocTypeVO();
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