using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.UserManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CarouselVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CarouselVO));
       
		[DataMember]
		 public Int32 CarouselID { get { return (Int32)GetValue(typeof(Int32), "CarouselID") ; } set {  SetValue("CarouselID", value); } }
        [DataMember]
		 public DateTime CreatedAt { get { return (DateTime)GetValue(typeof(DateTime), "CreatedAt") ; } set {  SetValue("CreatedAt", value); } }
        [DataMember]
		 public String Text { get { return (String)GetValue(typeof(String), "Text") ; } set {  SetValue("Text", value); } }
		    		
    	#region ICloneable Member
    	public override object Clone()
        {
            CarouselVO tmp = new CarouselVO();
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