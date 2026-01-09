using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.ProjectManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class ComplaintsImgVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(ComplaintsImgVO));
       
		[DataMember]
		 public Int32 ComplaintsImgId { get { return (Int32)GetValue(typeof(Int32),"ComplaintsImgId") ; } set {  SetValue("ComplaintsImgId",value); } } 		[DataMember]
		 public Int32 ComplaintsId { get { return (Int32)GetValue(typeof(Int32),"ComplaintsId") ; } set {  SetValue("ComplaintsId",value); } } 		[DataMember]
		 public String ImagePath { get { return (String)GetValue(typeof(String),"ImagePath") ; } set {  SetValue("ImagePath",value); } } 		[DataMember]
		 public String ImageName { get { return (String)GetValue(typeof(String),"ImageName") ; } set {  SetValue("ImageName",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            ComplaintsImgVO tmp = new ComplaintsImgVO();
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