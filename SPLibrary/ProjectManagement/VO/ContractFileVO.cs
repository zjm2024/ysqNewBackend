using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class ContractFileVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ContractFileVO));
       
		[DataMember]
		 public Int32 ContractFileId { get { return (Int32)GetValue(typeof(Int32),"ContractFileId") ; } set {  SetValue("ContractFileId",value); } } 		[DataMember]
		 public Int32 ContractId { get { return (Int32)GetValue(typeof(Int32),"ContractId") ; } set {  SetValue("ContractId",value); } } 		[DataMember]
		 public String FileName { get { return (String)GetValue(typeof(String),"FileName") ; } set {  SetValue("FileName",value); } } 		[DataMember]
		 public String FilePath { get { return (String)GetValue(typeof(String),"FilePath") ; } set {  SetValue("FilePath",value); } } 		[DataMember]
		 public Decimal FileSize { get { return (Decimal)GetValue(typeof(Decimal),"FileSize") ; } set {  SetValue("FileSize",value); } } 		[DataMember]
		public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime),"CreatedAt") ; } set {  SetValue("CreatedAt",value); } }
				[DataMember]
		 public Int32 CreatedBy { get { return (Int32)GetValue(typeof(Int32),"CreatedBy") ; } set {  SetValue("CreatedBy",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            ContractFileVO tmp = new ContractFileVO();
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