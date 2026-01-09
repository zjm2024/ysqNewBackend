using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class HandlePayoutHistoryVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(HandlePayoutHistoryVO));
       
		[DataMember]
		 public Int32 HandlepayouthistoryId { get { return (Int32)GetValue(typeof(Int32),"HandlepayouthistoryId") ; } set {  SetValue("HandlepayouthistoryId",value); } } 		[DataMember]
		 public Int32 PayOutHistoryId { get { return (Int32)GetValue(typeof(Int32),"PayOutHistoryId") ; } set {  SetValue("PayOutHistoryId",value); } } 		[DataMember]
		public DateTime HandleDate { get { return (DateTime)GetValue(typeof(DateTime),"HandleDate") ; } set {  SetValue("HandleDate",value); } }
				[DataMember]
		 public Int32 HandleStatus { get { return (Int32)GetValue(typeof(Int32),"HandleStatus") ; } set {  SetValue("HandleStatus",value); } } 		[DataMember]
		 public String HandleComment { get { return (String)GetValue(typeof(String),"HandleComment") ; } set {  SetValue("HandleComment",value); } } 		[DataMember]
		 public String ThirdOrder { get { return (String)GetValue(typeof(String),"ThirdOrder") ; } set {  SetValue("ThirdOrder",value); } } 		[DataMember]
		 public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal),"Cost") ; } set {  SetValue("Cost",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            HandlePayoutHistoryVO tmp = new HandlePayoutHistoryVO();
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