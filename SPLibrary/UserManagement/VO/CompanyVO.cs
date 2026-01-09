using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CompanyVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CompanyVO));
       
		[DataMember]
		public Int32 CompanyId { get { return (Int32)GetValue(typeof(Int32),"CompanyId") ; } set {  SetValue("CompanyId",value); } }
		[DataMember]
		public String CompanyCode { get { return (String)GetValue(typeof(String),"CompanyCode") ; } set {  SetValue("CompanyCode",value); } }
		[DataMember]
		public String CompanyName { get { return (String)GetValue(typeof(String),"CompanyName") ; } set {  SetValue("CompanyName",value); } }
		[DataMember]
		public String Address { get { return (String)GetValue(typeof(String),"Address") ; } set {  SetValue("Address",value); } }
		[DataMember]
		public String ContactPerson { get { return (String)GetValue(typeof(String),"ContactPerson") ; } set {  SetValue("ContactPerson",value); } }
		[DataMember]
		public String ContactPhone { get { return (String)GetValue(typeof(String),"ContactPhone") ; } set {  SetValue("ContactPhone",value); } }
		[DataMember]
		public String Description { get { return (String)GetValue(typeof(String),"Description") ; } set {  SetValue("Description",value); } }
		[DataMember]
		public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime),"CreatedAt") ; } set {  SetValue("CreatedAt",value); } }
		[DataMember]
		public Int32 CreatedBy { get { return (Int32)GetValue(typeof(Int32),"CreatedBy") ; } set {  SetValue("CreatedBy",value); } }
		[DataMember]
		public DateTime UpdatedAt { get { return (DateTime)GetValue(typeof(DateTime),"UpdatedAt") ; } set {  SetValue("UpdatedAt",value); } }
		[DataMember]
		public Int32 UpdatedBy { get { return (Int32)GetValue(typeof(Int32),"UpdatedBy") ; } set {  SetValue("UpdatedBy",value); } }
    		
    	#region ICloneable Member
    	public override object Clone()
        {
            CompanyVO tmp = new CompanyVO();
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