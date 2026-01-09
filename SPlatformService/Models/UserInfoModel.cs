using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPlatformService.Models
{
    [DataContract]
    public class UserInfoModel
    {
        [DataMember]
        public String OpenId { get; set; }
        [DataMember]
        public String access_token { get; set; }

        [DataMember]
        public DateTime accTokenTime { get; set; }
        
    }
}