using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class BankAccountVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BankAccountVO));
       
		[DataMember]
		 public Int32 BankAccountId { get { return (Int32)GetValue(typeof(Int32),"BankAccountId") ; } set {  SetValue("BankAccountId",value); } } 		[DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32),"CustomerId") ; } set {  SetValue("CustomerId",value); } } 		[DataMember]
		 public String BankAccount { get { return (String)GetValue(typeof(String),"BankAccount") ; } set {  SetValue("BankAccount",value); } }
        [DataMember]
		 public String BankName { get { return (String)GetValue(typeof(String),"BankName") ; } set {  SetValue("BankName",value); } }
        [DataMember]
        public Int32 Type { get { return (Int32)GetValue(typeof(Int32), "Type"); } set { SetValue("Type", value); } }
        [DataMember]
		 public String SubBranch { get { return (String)GetValue(typeof(String),"SubBranch") ; } set {  SetValue("SubBranch",value); } } 		[DataMember]
		 public String AccountName { get { return (String)GetValue(typeof(String),"AccountName") ; } set {  SetValue("AccountName",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            BankAccountVO tmp = new BankAccountVO();
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