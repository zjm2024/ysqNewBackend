using SPLibrary.UserManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPlatformService.Models
{
    [DataContract]
    public class RecommendRequireModel
    {
        [DataMember]
        public List<RecommendRequireVO> RecommendRequireList { get; set; }
    }
}