using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.RequireManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class RequireCommissionDelegationVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(RequireCommissionDelegationVO));
       
		[DataMember]
		 public Int32 RequireCommissionDelegationId { get { return (Int32)GetValue(typeof(Int32),"RequireCommissionDelegationId") ; } set {  SetValue("RequireCommissionDelegationId",value); } } 		[DataMember]
		 public Int32 RequirementId { get { return (Int32)GetValue(typeof(Int32),"RequirementId") ; } set {  SetValue("RequirementId",value); } } 		[DataMember]
		public DateTime DelegationDate { get { return (DateTime)GetValue(typeof(DateTime),"DelegationDate") ; } set {  SetValue("DelegationDate",value); } }
				[DataMember]
		 public Decimal Commission { get { return (Decimal)GetValue(typeof(Decimal),"Commission") ; } set {  SetValue("Commission",value); } } 		[DataMember]
		 public Int32 Status { get { return (Int32)GetValue(typeof(Int32),"Status") ; } set {  SetValue("Status",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            RequireCommissionDelegationVO tmp = new RequireCommissionDelegationVO();
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