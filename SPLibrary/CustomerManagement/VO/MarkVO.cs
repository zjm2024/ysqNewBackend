using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class MarkVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(MarkVO));
       
		[DataMember]
		 public Int32 MarkId { get { return (Int32)GetValue(typeof(Int32),"MarkId") ; } set {  SetValue("MarkId",value); } } 		[DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32),"CustomerId") ; } set {  SetValue("CustomerId",value); } } 		[DataMember]
		 public Int32 MarkType { get { return (Int32)GetValue(typeof(Int32),"MarkType") ; } set {  SetValue("MarkType",value); } } 		[DataMember]
		 public Int32 MarkObjectId { get { return (Int32)GetValue(typeof(Int32),"MarkObjectId") ; } set {  SetValue("MarkObjectId",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            MarkVO tmp = new MarkVO();
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