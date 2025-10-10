using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CustomerLoginHistoryVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CustomerLoginHistoryVO));
       
		[DataMember]
		 public Int32 CustomerLoginHistoryId { get { return (Int32)GetValue(typeof(Int32),"CustomerLoginHistoryId") ; } set {  SetValue("CustomerLoginHistoryId",value); } } 		[DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32),"CustomerId") ; } set {  SetValue("CustomerId",value); } } 		[DataMember]
		public DateTime LoginAt { get { return (DateTime)GetValue(typeof(DateTime),"LoginAt") ; } set {  SetValue("LoginAt",value); } }
				[DataMember]
		 public String LoginIP { get { return (String)GetValue(typeof(String),"LoginIP") ; } set {  SetValue("LoginIP",value); } } 		[DataMember]
		 public String LoginOS { get { return (String)GetValue(typeof(String),"LoginOS") ; } set {  SetValue("LoginOS",value); } } 		[DataMember]
		 public String LoginBrowser { get { return (String)GetValue(typeof(String),"LoginBrowser") ; } set {  SetValue("LoginBrowser",value); } } 		[DataMember]
		public Boolean Status { get { return (Boolean)GetValue(typeof(Boolean),"Status") ; } set {  SetValue("Status",value); } }
        [DataMember]
        public String Nation { get { return (String)GetValue(typeof(String), "Nation"); } set { SetValue("Nation", value); } }
        [DataMember]
        public String Province { get { return (String)GetValue(typeof(String), "Province"); } set { SetValue("Province", value); } }
        [DataMember]
        public String City { get { return (String)GetValue(typeof(String), "City"); } set { SetValue("City", value); } }
        [DataMember]
        public String District { get { return (String)GetValue(typeof(String), "District"); } set { SetValue("District", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CustomerLoginHistoryVO tmp = new CustomerLoginHistoryVO();
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