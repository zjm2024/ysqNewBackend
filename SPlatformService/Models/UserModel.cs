using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using SPLibrary.UserManagement.VO;

namespace SPlatformService.Models
{
    [DataContract]
    public class UserModel
    {
        [DataMember]
        public UserVO User { get; set; }
        [DataMember]
        public List<UserRoleVO> UserRole { get; set; }
    }
}