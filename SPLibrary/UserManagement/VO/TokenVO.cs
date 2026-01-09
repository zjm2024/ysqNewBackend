using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class TokenVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(TokenVO));

        [DataMember]
        public Int32 TokenId { get { return (Int32)GetValue(typeof(Int32), "TokenId"); } set { SetValue("TokenId", value); } }
        [DataMember]
		public String Token { get { return (String)GetValue(typeof(String),"Token") ; } set {  SetValue("Token",value); } }
		[DataMember]
		public Int32 CompanyId { get { return (Int32)GetValue(typeof(Int32),"CompanyId") ; } set {  SetValue("CompanyId",value); } }
		[DataMember]
		public Int32 DepartmentId { get { return (Int32)GetValue(typeof(Int32),"DepartmentId") ; } set {  SetValue("DepartmentId",value); } }
		[DataMember]
		public Int32 UserId { get { return (Int32)GetValue(typeof(Int32),"UserId") ; } set {  SetValue("UserId",value); } }
		[DataMember]
		public Boolean IsUser { get { return (Boolean)GetValue(typeof(Boolean),"IsUser") ; } set {  SetValue("IsUser",value); } }
		[DataMember]
		public DateTime Timeout { get { return (DateTime)GetValue(typeof(DateTime),"Timeout") ; } set {  SetValue("Timeout",value); } }
    		
    	#region ICloneable Member
    	public override object Clone()
        {
            TokenVO tmp = new TokenVO();
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