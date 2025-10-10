using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class HelpDocViewVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(HelpDocViewVO));
       
		[DataMember]
		 public Int32 HelpDocId { get { return (Int32)GetValue(typeof(Int32),"HelpDocId") ; } set {  SetValue("HelpDocId",value); } } 		[DataMember]
		 public Int32 HelpDocTypeId { get { return (Int32)GetValue(typeof(Int32),"HelpDocTypeId") ; } set {  SetValue("HelpDocTypeId",value); } } 		[DataMember]
		 public String Title { get { return (String)GetValue(typeof(String),"Title") ; } set {  SetValue("Title",value); } } 		[DataMember]
		 public String Description { get { return (String)GetValue(typeof(String),"Description") ; } set {  SetValue("Description",value); } } 		[DataMember]
		public Boolean Status { get { return (Boolean)GetValue(typeof(Boolean),"Status") ; } set {  SetValue("Status",value); } }
				[DataMember]
		 public String HelpDocTypeName { get { return (String)GetValue(typeof(String),"HelpDocTypeName") ; } set {  SetValue("HelpDocTypeName",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            HelpDocViewVO tmp = new HelpDocViewVO();
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