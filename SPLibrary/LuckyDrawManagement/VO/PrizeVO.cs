using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.LuckyDrawManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class PrizeVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(PrizeVO));
       
		[DataMember]
		public Int32 PrizeID { get { return (Int32)GetValue(typeof(Int32), "PrizeID") ; } set {  SetValue("PrizeID", value); } }
        [DataMember]
        public Int32 LuckyDrawID { get { return (Int32)GetValue(typeof(Int32), "LuckyDrawID"); } set { SetValue("LuckyDrawID", value); } }
        [DataMember]
        public String Title { get { return (String)GetValue(typeof(String), "Title"); } set { SetValue("Title", value); } }
        [DataMember]
        public String Image { get { return (String)GetValue(typeof(String), "Image"); } set { SetValue("Image", value); } }
        [DataMember]
        public Int32 Number { get { return (Int32)GetValue(typeof(Int32), "Number"); } set { SetValue("Number", value); } }
        [DataMember]
        public Int32 Status { get { return (Int32)GetValue(typeof(Int32), "Status"); } set { SetValue("Status", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            PrizeVO tmp = new PrizeVO();
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