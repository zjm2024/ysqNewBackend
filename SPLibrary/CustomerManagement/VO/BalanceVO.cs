using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class BalanceVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BalanceVO));
       
		[DataMember]
		 public Int32 BalanceId { get { return (Int32)GetValue(typeof(Int32),"BalanceId") ; } set {  SetValue("BalanceId",value); } } 		[DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32),"CustomerId") ; } set {  SetValue("CustomerId",value); } } 		[DataMember]
		 public Decimal Balance { get { return (Decimal)GetValue(typeof(Decimal),"Balance") ; } set {  SetValue("Balance",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            BalanceVO tmp = new BalanceVO();
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