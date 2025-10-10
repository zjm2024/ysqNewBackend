using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CategoryViewVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CategoryViewVO));
       
		[DataMember]
		 public Int32 CategoryId { get { return (Int32)GetValue(typeof(Int32),"CategoryId") ; } set {  SetValue("CategoryId",value); } } 		[DataMember]
		 public Int32 ParentCategoryId { get { return (Int32)GetValue(typeof(Int32),"ParentCategoryId") ; } set {  SetValue("ParentCategoryId",value); } } 		[DataMember]
		 public String CategoryCode { get { return (String)GetValue(typeof(String),"CategoryCode") ; } set {  SetValue("CategoryCode",value); } } 		[DataMember]
		 public String CategoryName { get { return (String)GetValue(typeof(String),"CategoryName") ; } set {  SetValue("CategoryName",value); } } 		[DataMember]
		public Boolean Status { get { return (Boolean)GetValue(typeof(Boolean),"Status") ; } set {  SetValue("Status",value); } }
				[DataMember]
		 public String ParentCategoryName { get { return (String)GetValue(typeof(String),"ParentCategoryName") ; } set {  SetValue("ParentCategoryName",value); } } 		[DataMember]
		public Boolean ParentCategoryStatus { get { return (Boolean)GetValue(typeof(Boolean),"ParentCategoryStatus") ; } set {  SetValue("ParentCategoryStatus",value); } }
		    		
    	#region ICloneable Member
    	public override object Clone()
        {
            CategoryViewVO tmp = new CategoryViewVO();
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