using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.RequireManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class RequirementFileVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(RequirementFileVO));
       
		[DataMember]
		 public Int32 RequirementFileId { get { return (Int32)GetValue(typeof(Int32),"RequirementFileId") ; } set {  SetValue("RequirementFileId",value); } } 		[DataMember]
		 public Int32 RequirementId { get { return (Int32)GetValue(typeof(Int32),"RequirementId") ; } set {  SetValue("RequirementId",value); } } 		[DataMember]
		 public String FileName { get { return (String)GetValue(typeof(String),"FileName") ; } set {  SetValue("FileName",value); } } 		[DataMember]
		 public String FilePath { get { return (String)GetValue(typeof(String),"FilePath") ; } set {  SetValue("FilePath",value); } } 		[DataMember]
		public DateTime CreatedDate { get { return (DateTime)GetValue(typeof(DateTime),"CreatedDate") ; } set {  SetValue("CreatedDate",value); } }
				[DataMember]
		 public Int32 CreatedBy { get { return (Int32)GetValue(typeof(Int32),"CreatedBy") ; } set {  SetValue("CreatedBy",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            RequirementFileVO tmp = new RequirementFileVO();
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