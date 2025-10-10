using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class RecommendRequireVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(RecommendRequireVO));
       
		[DataMember]
		 public Int32 RecommendRequireId { get { return (Int32)GetValue(typeof(Int32),"RecommendRequireId") ; } set {  SetValue("RecommendRequireId",value); } } 		[DataMember]
		 public Int32 RequirementId { get { return (Int32)GetValue(typeof(Int32),"RequirementId") ; } set {  SetValue("RequirementId",value); } } 		[DataMember]
		 public Int32 ProvinceId { get { return (Int32)GetValue(typeof(Int32),"ProvinceId") ; } set {  SetValue("ProvinceId",value); } } 		[DataMember]
		 public Int32 CityId { get { return (Int32)GetValue(typeof(Int32),"CityId") ; } set {  SetValue("CityId",value); } } 		[DataMember]
		 public Int32 CategoryId { get { return (Int32)GetValue(typeof(Int32),"CategoryId") ; } set {  SetValue("CategoryId",value); } } 		[DataMember]
		 public Int32 ParentCategoryId { get { return (Int32)GetValue(typeof(Int32),"ParentCategoryId") ; } set {  SetValue("ParentCategoryId",value); } } 		[DataMember]
		 public Int32 Sort { get { return (Int32)GetValue(typeof(Int32),"Sort") ; } set {  SetValue("Sort",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            RecommendRequireVO tmp = new RecommendRequireVO();
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