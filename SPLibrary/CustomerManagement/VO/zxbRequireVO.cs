using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class zxbRequireVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(zxbRequireVO));
       
		[DataMember]
		 public Int32 ZXBrequireId { get { return (Int32)GetValue(typeof(Int32), "ZXBrequireId") ; } set {  SetValue("ZXBrequireId", value); } }
        [DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
		 public Decimal Cost { get { return (Decimal)GetValue(typeof(Decimal), "Cost") ; } set {  SetValue("Cost", value); } }
        [DataMember]
         public DateTime Date { get { return (DateTime)GetValue(typeof(DateTime), "Date"); } set { SetValue("Date", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
        [DataMember]
        public String Purpose { get { return (String)GetValue(typeof(String), "Purpose"); } set { SetValue("Purpose", value); } }
        [DataMember]
        public Int32 type { get { return (Int32)GetValue(typeof(Int32), "type"); } set { SetValue("type", value); } }
        #region ICloneable Member
        public override object Clone()
        {
            zxbRequireVO tmp = new zxbRequireVO();
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