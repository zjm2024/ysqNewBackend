using SPLibrary.CustomerManagement.VO;
using SPLibrary.ProjectManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPlatformService.Models
{
    [DataContract]
    public class ComplaintsModel
    {
        [DataMember]
        public ComplaintsVO Complaints { get; set; }
        [DataMember]
        public List<ComplaintsImgVO> ComplaintsImg { get; set; }
        
    }
}