using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.RequireManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class TenderInfoVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(TenderInfoVO));
       
		[DataMember]
		 public Int32 TenderInfoId { get { return (Int32)GetValue(typeof(Int32),"TenderInfoId") ; } set {  SetValue("TenderInfoId",value); } } 		[DataMember]
		 public Int32 RequirementId { get { return (Int32)GetValue(typeof(Int32),"RequirementId") ; } set {  SetValue("RequirementId",value); } } 		[DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32),"CustomerId") ; } set {  SetValue("CustomerId",value); } } 		[DataMember]
		public DateTime TenderDate { get { return (DateTime)GetValue(typeof(DateTime),"TenderDate") ; } set {  SetValue("TenderDate",value); } }
		    		
    	#region ICloneable Member
    	public override object Clone()
        {
            TenderInfoVO tmp = new TenderInfoVO();
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