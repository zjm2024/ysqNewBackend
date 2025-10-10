using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class AgencySolutionFileVO :CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(AgencySolutionFileVO));
       
		[DataMember]
		 public Int32 AgencySolutionFileId { get { return (Int32)GetValue(typeof(Int32),"AgencySolutionFileId") ; } set {  SetValue("AgencySolutionFileId",value); } } 		[DataMember]
		 public Int32 AgencySolutionId { get { return (Int32)GetValue(typeof(Int32),"AgencySolutionId") ; } set {  SetValue("AgencySolutionId",value); } } 		[DataMember]
		 public String FileName { get { return (String)GetValue(typeof(String),"FileName") ; } set {  SetValue("FileName",value); } } 		[DataMember]
		 public String FilePath { get { return (String)GetValue(typeof(String),"FilePath") ; } set {  SetValue("FilePath",value); } }     		
    	#region ICloneable Member
    	public override object Clone()
        {
            AgencySolutionFileVO tmp = new AgencySolutionFileVO();
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