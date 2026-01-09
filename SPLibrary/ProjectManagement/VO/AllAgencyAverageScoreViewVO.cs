using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AllAgencyAverageScoreViewVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AllAgencyAverageScoreViewVO));
       
		[DataMember]
		 public Decimal AverageScore { get { return (Decimal)GetValue(typeof(Decimal),"AverageScore") ; } set {  SetValue("AverageScore",value); } } 		[DataMember]
		 public Int32 ReviewType { get { return (Int32)GetValue(typeof(Int32),"ReviewType") ; } set {  SetValue("ReviewType",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            AllAgencyAverageScoreViewVO tmp = new AllAgencyAverageScoreViewVO();
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