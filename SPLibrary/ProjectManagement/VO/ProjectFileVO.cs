using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class ProjectFileVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ProjectFileVO));
       
		[DataMember]
		 public Int32 ProjectFileId { get { return (Int32)GetValue(typeof(Int32),"ProjectFileId") ; } set {  SetValue("ProjectFileId",value); } } 		[DataMember]
		 public Int32 ProjectId { get { return (Int32)GetValue(typeof(Int32),"ProjectId") ; } set {  SetValue("ProjectId",value); } } 		[DataMember]
		 public String FileName { get { return (String)GetValue(typeof(String),"FileName") ; } set {  SetValue("FileName",value); } } 		[DataMember]
		 public String FilePath { get { return (String)GetValue(typeof(String),"FilePath") ; } set {  SetValue("FilePath",value); } } 		[DataMember]
		public DateTime CreatedDate { get { return (DateTime)GetValue(typeof(DateTime),"CreatedDate") ; } set {  SetValue("CreatedDate",value); } }
				[DataMember]
		 public Int32 CreatedBy { get { return (Int32)GetValue(typeof(Int32),"CreatedBy") ; } set {  SetValue("CreatedBy",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            ProjectFileVO tmp = new ProjectFileVO();
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