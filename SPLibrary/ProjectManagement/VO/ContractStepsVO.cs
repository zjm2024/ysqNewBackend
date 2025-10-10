using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class ContractStepsVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ContractStepsVO));
       
		[DataMember]
		 public Int32 ContractStepsId { get { return (Int32)GetValue(typeof(Int32),"ContractStepsId") ; } set {  SetValue("ContractStepsId",value); } } 		[DataMember]
		 public Int32 ContractId { get { return (Int32)GetValue(typeof(Int32),"ContractId") ; } set {  SetValue("ContractId",value); } } 		[DataMember]
		 public String Title { get { return (String)GetValue(typeof(String),"Title") ; } set {  SetValue("Title",value); } } 		[DataMember]
		 public String Comment { get { return (String)GetValue(typeof(String),"Comment") ; } set {  SetValue("Comment",value); } } 		[DataMember]
		 public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal),"Cost") ; } set {  SetValue("Cost",value); } } 		[DataMember]
		 public Int32 SortNO { get { return (Int32)GetValue(typeof(Int32),"SortNO") ; } set {  SetValue("SortNO",value); } } 		[DataMember]
		public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime),"CreatedAt") ; } set {  SetValue("CreatedAt",value); } }
		    		
    	#region ICloneable Member
    	public override object Clone()
        {
            ContractStepsVO tmp = new ContractStepsVO();
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