using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BusinessCard.Models
{
    [DataContract]
    public class BCPartyModel
    {
        [DataMember]
        public BCPartyVO BCParty { get; set; }
        [DataMember]
        public PersonalVO Personal { get; set; }

        [DataMember]
        public List<BCPartyCostVO> BCPartyCost { get; set; }

        [DataMember]
        public List<BCPartyContactsVO> BCPartyContacts { get; set; }

        [DataMember]
        public List<BCPartyContactsViewVO> BCPartyContactsView { get; set; }

        [DataMember]
        public List<BCPartySignUpVO> BCPartySignUp { get; set; }

        [DataMember]
        public int BCPartySignCount { get; set; }
        [DataMember]
        public List<BCPartySignUpFormVO> BCPartySignUpForm { get; set; }

    }
}