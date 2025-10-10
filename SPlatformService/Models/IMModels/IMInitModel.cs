using SPLibrary.UserManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPlatformService.Models
{
    [DataContract]
    public class IMInitModel
    {
        [DataMember]
        public IMUser mine { get; set; }
        [DataMember]
        public List<IMGroupUser> friend { get; set; }
        [DataMember]
        public List<IMGroup> group { get; set; }
    }

    [DataContract]
    public class IMUser
    {
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string sign { get; set; }
        [DataMember]
        public string avatar { get; set; }
        [DataMember]
        public int groupid { get; set; }
    }

    [DataContract]
    public class IMGroupUser
    {
        [DataMember]
        public string groupname { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int online { get; set; }
        [DataMember]
        public List<IMUser> list { get; set; }
    }


    [DataContract]
    public class IMGroup
    {
        [DataMember]
        public string groupname { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int avatar { get; set; }
    }    

}