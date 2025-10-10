using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CommissionVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CommissionVO));
       
		[DataMember]
		 public Int32 CommissionId { get { return (Int32)GetValue(typeof(Int32),"CommissionId") ; } set {  SetValue("CommissionId",value); } } 		[DataMember]
		 public Int32 ProjectId { get { return (Int32)GetValue(typeof(Int32),"ProjectId") ; } set {  SetValue("ProjectId",value); } } 		[DataMember]
		 public Decimal ProjectCommission { get { return (Decimal)GetValue(typeof(Decimal),"ProjectCommission") ; } set {  SetValue("ProjectCommission",value); } } 		[DataMember]
		 public Decimal CommissionPercentage { get { return (Decimal)GetValue(typeof(Decimal),"CommissionPercentage") ; } set {  SetValue("CommissionPercentage",value); } } 		[DataMember]
		public DateTime CommissionDate { get { return (DateTime)GetValue(typeof(DateTime),"CommissionDate") ; } set {  SetValue("CommissionDate",value); } }
				[DataMember]
		 public Int32 Status { get { return (Int32)GetValue(typeof(Int32),"Status") ; } set {  SetValue("Status",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            CommissionVO tmp = new CommissionVO();
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