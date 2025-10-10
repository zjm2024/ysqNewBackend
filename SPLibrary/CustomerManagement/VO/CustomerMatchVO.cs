using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CustomerMatchVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CustomerMatchVO));
       
		[DataMember]
		 public Int32 CustomerMatchId { get { return (Int32)GetValue(typeof(Int32),"CustomerMatchId") ; } set {  SetValue("CustomerMatchId",value); } } 		[DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32),"CustomerId") ; } set {  SetValue("CustomerId",value); } } 		[DataMember]
		 public String MatchType { get { return (String)GetValue(typeof(String),"MatchType") ; } set {  SetValue("MatchType",value); } } 		[DataMember]
		 public String AccessToken { get { return (String)GetValue(typeof(String),"AccessToken") ; } set {  SetValue("AccessToken",value); } } 		[DataMember]
		 public String OpenId { get { return (String)GetValue(typeof(String),"OpenId") ; } set {  SetValue("OpenId",value); } }
         public String UnionID { get { return (String)GetValue(typeof(String), "UnionID"); } set { SetValue("UnionID", value); } }
        [DataMember]
        public Int32 AppType { get { return (Int32)GetValue(typeof(Int32), "AppType"); } set { SetValue("AppType", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            CustomerMatchVO tmp = new CustomerMatchVO();
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