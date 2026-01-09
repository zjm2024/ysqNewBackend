using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class ProjectFileViewVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ProjectFileViewVO));
       
		[DataMember]
		 public Int32 ProjectFileId { get { return (Int32)GetValue(typeof(Int32),"ProjectFileId") ; } set {  SetValue("ProjectFileId",value); } } 		[DataMember]
		 public Int32 ProjectId { get { return (Int32)GetValue(typeof(Int32),"ProjectId") ; } set {  SetValue("ProjectId",value); } } 		[DataMember]
		 public String FileName { get { return (String)GetValue(typeof(String),"FileName") ; } set {  SetValue("FileName",value); } } 		[DataMember]
		 public String FilePath { get { return (String)GetValue(typeof(String),"FilePath") ; } set {  SetValue("FilePath",value); } } 		[DataMember]
		public DateTime CreatedDate { get { return (DateTime)GetValue(typeof(DateTime),"CreatedDate") ; } set {  SetValue("CreatedDate",value); } }
				[DataMember]
		 public Int32 CreatedBy { get { return (Int32)GetValue(typeof(Int32),"CreatedBy") ; } set {  SetValue("CreatedBy",value); } } 		[DataMember]
		 public String Creator { get { return (String)GetValue(typeof(String),"Creator") ; } set {  SetValue("Creator",value); } } 		[DataMember]
		 public String ProjectCode { get { return (String)GetValue(typeof(String),"ProjectCode") ; } set {  SetValue("ProjectCode",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            ProjectFileViewVO tmp = new ProjectFileViewVO();
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