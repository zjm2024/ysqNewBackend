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
    public class BusinessReviewModel
    {
        [DataMember]
        public BusinessReviewVO BusinessReview { get; set; }
        [DataMember]
        public List<BusinessReviewDetailVO> BusinessReviewDetail { get; set; }
        
    }
}