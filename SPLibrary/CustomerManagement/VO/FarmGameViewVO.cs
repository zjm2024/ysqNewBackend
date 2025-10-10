using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class FarmGameViewVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(FarmGameViewVO));
       
		[DataMember]
		 public Int32 FarmGameID { get { return (Int32)GetValue(typeof(Int32), "FarmGameID") ; } set {  SetValue("FarmGameID", value); } }
        [DataMember]
		 public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId") ; } set {  SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 Gold { get { return (Int32)GetValue(typeof(Int32), "Gold"); } set { SetValue("Gold", value); } }

        [DataMember]
		 public String FieldsType1 { get { return (String)GetValue(typeof(String), "FieldsType1") ; } set {  SetValue("FieldsType1", value); } }
        [DataMember]
        public Int32 FieldsSum1 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum1"); } set { SetValue("FieldsSum1", value); } }

        [DataMember]
        public String FieldsType2 { get { return (String)GetValue(typeof(String), "FieldsType2"); } set { SetValue("FieldsType2", value); } }
        [DataMember]
        public Int32 FieldsSum2 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum2"); } set { SetValue("FieldsSum2", value); } }

        [DataMember]
        public String FieldsType3 { get { return (String)GetValue(typeof(String), "FieldsType3"); } set { SetValue("FieldsType3", value); } }
        [DataMember]
        public Int32 FieldsSum3 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum3"); } set { SetValue("FieldsSum3", value); } }

        [DataMember]
        public String FieldsType4 { get { return (String)GetValue(typeof(String), "FieldsType4"); } set { SetValue("FieldsType4", value); } }
        [DataMember]
        public Int32 FieldsSum4 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum4"); } set { SetValue("FieldsSum4", value); } }

        [DataMember]
        public String FieldsType5 { get { return (String)GetValue(typeof(String), "FieldsType5"); } set { SetValue("FieldsType5", value); } }
        [DataMember]
        public Int32 FieldsSum5 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum5"); } set { SetValue("FieldsSum5", value); } }

        [DataMember]
        public String FieldsType6 { get { return (String)GetValue(typeof(String), "FieldsType6"); } set { SetValue("FieldsType6", value); } }
        [DataMember]
        public Int32 FieldsSum6 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum6"); } set { SetValue("FieldsSum6", value); } }

        [DataMember]
        public String FieldsType7 { get { return (String)GetValue(typeof(String), "FieldsType7"); } set { SetValue("FieldsType7", value); } }
        [DataMember]
        public Int32 FieldsSum7 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum7"); } set { SetValue("FieldsSum7", value); } }

        [DataMember]
        public String FieldsType8 { get { return (String)GetValue(typeof(String), "FieldsType8"); } set { SetValue("FieldsType8", value); } }
        [DataMember]
        public Int32 FieldsSum8 { get { return (Int32)GetValue(typeof(Int32), "FieldsSum8"); } set { SetValue("FieldsSum8", value); } }

        [DataMember]
        public Int32 Apple_seed { get { return (Int32)GetValue(typeof(Int32), "Apple_seed"); } set { SetValue("Apple_seed", value); } }
        [DataMember]
        public Int32 Carrot_seed { get { return (Int32)GetValue(typeof(Int32), "Carrot_seed"); } set { SetValue("Carrot_seed", value); } }
        [DataMember]
        public Int32 Eggplant_seed { get { return (Int32)GetValue(typeof(Int32), "Eggplant_seed"); } set { SetValue("Eggplant_seed", value); } }
        [DataMember]
        public Int32 Tomato_seed { get { return (Int32)GetValue(typeof(Int32), "Tomato_seed"); } set { SetValue("Tomato_seed", value); } }

        [DataMember]
        public Int32 Water { get { return (Int32)GetValue(typeof(Int32), "Water"); } set { SetValue("Water", value); } }
        [DataMember]
        public Int32 Fertilizer { get { return (Int32)GetValue(typeof(Int32), "Fertilizer"); } set { SetValue("Fertilizer", value); } }

        [DataMember]
        public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt"); } set { SetValue("CreatedAt", value); } }

        [DataMember]
        public Int32 Diamond { get { return (Int32)GetValue(typeof(Int32), "Diamond"); } set { SetValue("Diamond", value); } }

        [DataMember]
        public String CustomerName { get { return (String)GetValue(typeof(String), "CustomerName"); } set { SetValue("CustomerName", value); } }

        [DataMember]
        public String HeaderLogo { get { return (String)GetValue(typeof(String), "HeaderLogo"); } set { SetValue("HeaderLogo", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            FarmGameViewVO tmp = new FarmGameViewVO();
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