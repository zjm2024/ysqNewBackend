using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using SPLibrary.UserManagement.VO;

namespace SPlatformService.Models
{
    [DataContract]
    public class UserLoginModel
    {
        [DataMember]
        public UserViewVO User { get; set; }
        [DataMember]
        public string Token { get; set; }
    }
}