using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using SPLibrary.UserManagement.VO;

namespace SPlatformService.Models
{
    [DataContract]
    public class RoleModel
    {
        [DataMember]
        public RoleVO Role { get; set; }
        [DataMember]
        public List<RoleSecurityVO> RoleSecurity { get; set; }
    }
}