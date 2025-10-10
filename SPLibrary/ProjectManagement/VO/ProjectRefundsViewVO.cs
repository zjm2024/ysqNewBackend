using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class ProjectRefundsViewVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ProjectRefundsViewVO));
       
		[DataMember]
		 public Int32 ProjectRefundsId { get { return (Int32)GetValue(typeof(Int32),"ProjectRefundsId") ; } set {  SetValue("ProjectRefundsId",value); } } 		[DataMember]
		 public Int32 ProjectId { get { return (Int32)GetValue(typeof(Int32),"ProjectId") ; } set {  SetValue("ProjectId",value); } } 		[DataMember]
		 public String Reason { get { return (String)GetValue(typeof(String),"Reason") ; } set {  SetValue("Reason",value); } } 		[DataMember]
		 public String RejectReason { get { return (String)GetValue(typeof(String),"RejectReason") ; } set {  SetValue("RejectReason",value); } } 		[DataMember]
		public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime),"CreatedAt") ; } set {  SetValue("CreatedAt",value); } }
				[DataMember]
		 public Int32 CreatedBy { get { return (Int32)GetValue(typeof(Int32),"CreatedBy") ; } set {  SetValue("CreatedBy",value); } } 		[DataMember]
		 public Int32 Status { get { return (Int32)GetValue(typeof(Int32),"Status") ; } set {  SetValue("Status",value); } } 		[DataMember]
		 public String ProjectCode { get { return (String)GetValue(typeof(String),"ProjectCode") ; } set {  SetValue("ProjectCode",value); } } 		[DataMember]
		 public String Creator { get { return (String)GetValue(typeof(String),"Creator") ; } set {  SetValue("Creator",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            ProjectRefundsViewVO tmp = new ProjectRefundsViewVO();
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