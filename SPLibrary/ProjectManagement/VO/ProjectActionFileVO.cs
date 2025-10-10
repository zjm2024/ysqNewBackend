using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class ProjectActionFileVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ProjectActionFileVO));
       
		[DataMember]
		 public Int32 ProjectActionFileId { get { return (Int32)GetValue(typeof(Int32),"ProjectActionFileId") ; } set {  SetValue("ProjectActionFileId",value); } } 		[DataMember]
		 public Int32 ProjectActionId { get { return (Int32)GetValue(typeof(Int32),"ProjectActionId") ; } set {  SetValue("ProjectActionId",value); } } 		[DataMember]
		 public String FileName { get { return (String)GetValue(typeof(String),"FileName") ; } set {  SetValue("FileName",value); } } 		[DataMember]
		 public String FilePath { get { return (String)GetValue(typeof(String),"FilePath") ; } set {  SetValue("FilePath",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            ProjectActionFileVO tmp = new ProjectActionFileVO();
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