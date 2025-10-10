using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CustomerRecentLoginViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CustomerRecentLoginViewVO));
       
		[DataMember]
		 public Int32 CustomerLoginHistoryId { get { return (Int32)GetValue(typeof(Int32),"CustomerLoginHistoryId") ; } set {  SetValue("CustomerLoginHistoryId",value); } } 		[DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32),"CustomerId") ; } set {  SetValue("CustomerId",value); } } 		[DataMember]
		public DateTime LoginAt { get { return (DateTime)GetValue(typeof(DateTime),"LoginAt") ; } set {  SetValue("LoginAt",value); } }
        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }
        [DataMember]
        public Boolean isVip { get { return (Boolean)GetValue(typeof(Boolean), "isVip"); } set { SetValue("isVip", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CustomerRecentLoginViewVO tmp = new CustomerRecentLoginViewVO();
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