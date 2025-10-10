using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPlatformService.Models
{
    [DataContract]
    public class PartyModel
    {
        [DataMember]
        public CardPartyVO CardParty { get; set; }
        [DataMember]
        public CardDataVO CardData { get; set; }
        [DataMember]
        public List<CardPartyCostVO> CardPartyCost { get; set; }
        [DataMember]
        public List<CardPartyContactsVO> CardPartyContacts { get; set; }
        [DataMember]
        public List<CardPartyContactsViewVO> CardPartyContactsView { get; set; }
        [DataMember]
        public List<CardPartySignUpVO> CardPartySignUp { get; set; }
        [DataMember]
        public int CardPartySignCount { get; set; }
        [DataMember]
        public List<CardPartySignUpFormVO> CardPartySignUpForm { get; set; }
        [DataMember]
        public List<CardPartySignUpViewVO> CardPartyInviterList { get; set; }
        
    }
}