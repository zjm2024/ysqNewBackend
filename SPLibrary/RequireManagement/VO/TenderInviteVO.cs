using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.RequireManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class TenderInviteVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(TenderInviteVO));
       
		[DataMember]
		 public Int32 TenderInviteId { get { return (Int32)GetValue(typeof(Int32),"TenderInviteId") ; } set {  SetValue("TenderInviteId",value); } } 		[DataMember]
		 public Int32 RequirementId { get { return (Int32)GetValue(typeof(Int32),"RequirementId") ; } set {  SetValue("RequirementId",value); } } 		[DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32),"CustomerId") ; } set {  SetValue("CustomerId",value); } } 		[DataMember]
		public DateTime InviteDate { get { return (DateTime)GetValue(typeof(DateTime),"InviteDate") ; } set {  SetValue("InviteDate",value); } }
		    		
    	#region ICloneable Member
    	public override object Clone()
        {
            TenderInviteVO tmp = new TenderInviteVO();
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