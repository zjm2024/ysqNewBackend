using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class BusinessApproveHistoryVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BusinessApproveHistoryVO));
       
		[DataMember]
		 public Int32 BusinessApproveHistoryId { get { return (Int32)GetValue(typeof(Int32),"BusinessApproveHistoryId") ; } set {  SetValue("BusinessApproveHistoryId",value); } } 		[DataMember]
		 public Int32 BusinessId { get { return (Int32)GetValue(typeof(Int32),"BusinessId") ; } set {  SetValue("BusinessId",value); } } 		[DataMember]
		public DateTime ApproveDate { get { return (DateTime)GetValue(typeof(DateTime),"ApproveDate") ; } set {  SetValue("ApproveDate",value); } }
				[DataMember]
		 public Int32 ApproveStatus { get { return (Int32)GetValue(typeof(Int32),"ApproveStatus") ; } set {  SetValue("ApproveStatus",value); } } 		[DataMember]
		 public String ApproveComment { get { return (String)GetValue(typeof(String),"ApproveComment") ; } set {  SetValue("ApproveComment",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            BusinessApproveHistoryVO tmp = new BusinessApproveHistoryVO();
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