using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AgencyExperienceApproveHistoryVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencyExperienceApproveHistoryVO));
       
		[DataMember]
		 public Int32 AgencyExperienceApproveHistoryId { get { return (Int32)GetValue(typeof(Int32),"AgencyExperienceApproveHistoryId") ; } set {  SetValue("AgencyExperienceApproveHistoryId",value); } } 		[DataMember]
		 public Int32 AgencyExperienceId { get { return (Int32)GetValue(typeof(Int32),"AgencyExperienceId") ; } set {  SetValue("AgencyExperienceId",value); } } 		[DataMember]
		public DateTime ApproveDate { get { return (DateTime)GetValue(typeof(DateTime),"ApproveDate") ; } set {  SetValue("ApproveDate",value); } }
				[DataMember]
		 public Int32 ApproveStatus { get { return (Int32)GetValue(typeof(Int32),"ApproveStatus") ; } set {  SetValue("ApproveStatus",value); } } 		[DataMember]
		 public String ApproveComment { get { return (String)GetValue(typeof(String),"ApproveComment") ; } set {  SetValue("ApproveComment",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            AgencyExperienceApproveHistoryVO tmp = new AgencyExperienceApproveHistoryVO();
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