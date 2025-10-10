using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class FarmGameTaskVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(FarmGameTaskVO));
       
		[DataMember]
		 public Int32 TaskID { get { return (Int32)GetValue(typeof(Int32), "TaskID") ; } set {  SetValue("TaskID", value); } }
        [DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }

        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String Type { get { return (String)GetValue(typeof(String), "Type"); } set { SetValue("Type", value); } }

        [DataMember]
        public Int32 Gold { get { return (Int32)GetValue(typeof(Int32), "Gold"); } set { SetValue("Gold", value); } }
        [DataMember]
        public Int32 Water { get { return (Int32)GetValue(typeof(Int32), "Water"); } set { SetValue("Water", value); } }
        [DataMember]
        public Int32 Fertilizer { get { return (Int32)GetValue(typeof(Int32), "Fertilizer"); } set { SetValue("Fertilizer", value); } }

        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }
       

        #region ICloneable Member
        public override object Clone()
        {
            FarmGameTaskVO tmp = new FarmGameTaskVO();
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