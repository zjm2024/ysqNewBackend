using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class MarkServicesViewVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(MarkServicesViewVO));

        [DataMember]
        public Int32 MarkId { get { return (Int32)GetValue(typeof(Int32), "MarkId"); } set { SetValue("MarkId", value); } }
        [DataMember]
		 public Int32 MarkCustomerId { get { return (Int32)GetValue(typeof(Int32),"MarkCustomerId") ; } set {  SetValue("MarkCustomerId",value); } } 		[DataMember]
		 public Int32 ServicesId { get { return (Int32)GetValue(typeof(Int32),"ServicesId") ; } set {  SetValue("ServicesId",value); } } 		[DataMember]
		 public Int32 CityId { get { return (Int32)GetValue(typeof(Int32),"CityId") ; } set {  SetValue("CityId",value); } } 		[DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32),"CustomerId") ; } set {  SetValue("CustomerId",value); } } 		[DataMember]
		 public String Title { get { return (String)GetValue(typeof(String),"Title") ; } set {  SetValue("Title",value); } } 		[DataMember]
		 public Decimal Price { get { return (Decimal)GetValue(typeof(Decimal),"Price") ; } set {  SetValue("Price",value); } } 		[DataMember]
		 public Int32 Count { get { return (Int32)GetValue(typeof(Int32),"Count") ; } set {  SetValue("Count",value); } } 		[DataMember]
		 public String Description { get { return (String)GetValue(typeof(String),"Description") ; } set {  SetValue("Description",value); } } 		[DataMember]
		 public String MainImg { get { return (String)GetValue(typeof(String),"MainImg") ; } set {  SetValue("MainImg",value); } } 		[DataMember]
		 public Int32 Status { get { return (Int32)GetValue(typeof(Int32),"Status") ; } set {  SetValue("Status",value); } } 		[DataMember]
		public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime),"CreatedAt") ; } set {  SetValue("CreatedAt",value); } }
				[DataMember]
		 public String ServicesCode { get { return (String)GetValue(typeof(String),"ServicesCode") ; } set {  SetValue("ServicesCode",value); } } 		[DataMember]
		 public String CityName { get { return (String)GetValue(typeof(String),"CityName") ; } set {  SetValue("CityName",value); } } 		[DataMember]
		 public String CustomerName { get { return (String)GetValue(typeof(String),"CustomerName") ; } set {  SetValue("CustomerName",value); } } 		[DataMember]
		 public Int32 ProvinceId { get { return (Int32)GetValue(typeof(Int32),"ProvinceId") ; } set {  SetValue("ProvinceId",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            MarkServicesViewVO tmp = new MarkServicesViewVO();
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