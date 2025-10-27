using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class RecordingRecordsVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(RecordingRecordsVO));

        [DataMember]
        public Int32 recording_records_id { get { return (Int32)GetValue(typeof(Int32), "recording_records_id"); } set { SetValue("recording_records_id", value); } }
        [DataMember]
        public String file_name { get { return (String)GetValue(typeof(String), "file_name"); } set { SetValue("file_name", value); } }
        [DataMember]
        public String original_file_name { get { return (String)GetValue(typeof(String), "original_file_name"); } set { SetValue("original_file_name", value); } }
        [DataMember]
        public String file_path { get { return (String)GetValue(typeof(String), "file_path"); } set { SetValue("file_path", value); } }
        [DataMember]
		public Int32 file_size { get { return (Int32)GetValue(typeof(Int32), "file_size") ; } set {  SetValue("file_size", value); } }
        [DataMember]
        public Int32 duration { get { return (Int32)GetValue(typeof(Int32), "duration"); } set { SetValue("duration", value); } }
        [DataMember]
        public Int32 activityid { get { return (Int32)GetValue(typeof(Int32), "activityid"); } set { SetValue("activityid", value); } }
        [DataMember]
		public Int32 personalid { get { return (Int32)GetValue(typeof(Int32), "personalid") ; } set {  SetValue("personalid", value); } }
        [DataMember]
        public String recording_config { get { return (String)GetValue(typeof(String), "recording_config"); } set { SetValue("recording_config", value); } }
        [DataMember]
        public String status { get { return (String)GetValue(typeof(String), "status"); } set { SetValue("status", value); } }
        [DataMember]
        public DateTime create_time { get { return (DateTime)GetValue(typeof(DateTime), "create_time"); } set { SetValue("create_time", value); } }

        [DataMember]
        public DateTime modify_time { get { return (DateTime)GetValue(typeof(DateTime), "modify_time"); } set { SetValue("modify_time", value); } }


        #region ICloneable Member
        public override object Clone()
        {
            RecordingRecordsVO tmp = new RecordingRecordsVO();
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