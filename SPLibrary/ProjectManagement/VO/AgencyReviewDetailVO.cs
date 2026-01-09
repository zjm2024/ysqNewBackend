using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AgencyReviewDetailVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencyReviewDetailVO));
       
		[DataMember]
		 public Int32 AgencyReviewDetailId { get { return (Int32)GetValue(typeof(Int32),"AgencyReviewDetailId") ; } set {  SetValue("AgencyReviewDetailId",value); } } 		[DataMember]
		 public Int32 AgencyReviewId { get { return (Int32)GetValue(typeof(Int32),"AgencyReviewId") ; } set {  SetValue("AgencyReviewId",value); } } 		[DataMember]
		 public Int32 ReviewType { get { return (Int32)GetValue(typeof(Int32),"ReviewType") ; } set {  SetValue("ReviewType",value); } } 		[DataMember]
		 public Int32 Score { get { return (Int32)GetValue(typeof(Int32), "Score") ; } set {  SetValue("Score", value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            AgencyReviewDetailVO tmp = new AgencyReviewDetailVO();
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