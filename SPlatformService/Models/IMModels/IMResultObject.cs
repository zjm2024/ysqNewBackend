using SPLibrary.UserManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPlatformService.Models
{
    [DataContract]
    public class IMResultObject
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public string msg { get; set; }
        [DataMember]
        public object data { get; set; }
    }
}