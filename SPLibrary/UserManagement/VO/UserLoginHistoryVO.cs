using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class UserLoginHistoryVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(UserLoginHistoryVO));
       
		[DataMember]
		 public Int32 UserLoginHistoryId { get { return (Int32)GetValue(typeof(Int32),"UserLoginHistoryId") ; } set {  SetValue("UserLoginHistoryId",value); } } 		[DataMember]
		 public Int32 UserId { get { return (Int32)GetValue(typeof(Int32),"UserId") ; } set {  SetValue("UserId",value); } } 		[DataMember]
		public DateTime LoginAt { get { return (DateTime)GetValue(typeof(DateTime),"LoginAt") ; } set {  SetValue("LoginAt",value); } }
				[DataMember]
		 public String LoginIP { get { return (String)GetValue(typeof(String),"LoginIP") ; } set {  SetValue("LoginIP",value); } } 		[DataMember]
		 public String LoginOS { get { return (String)GetValue(typeof(String),"LoginOS") ; } set {  SetValue("LoginOS",value); } } 		[DataMember]
		 public String LoginBrowser { get { return (String)GetValue(typeof(String),"LoginBrowser") ; } set {  SetValue("LoginBrowser",value); } } 		[DataMember]
		public Boolean Status { get { return (Boolean)GetValue(typeof(Boolean),"Status") ; } set {  SetValue("Status",value); } }
		    		
    	#region ICloneable Member
    	public override object Clone()
        {
            UserLoginHistoryVO tmp = new UserLoginHistoryVO();
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