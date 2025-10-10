using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public class WebVO
    {
		[DataMember]
		public Int32 BusinessID { get; set ; }
        [DataMember]
        public List<InfoVO> banner { get; set; }
        [DataMember]
        public String LogoImg { get; set ; }
        [DataMember]
        public String BusinessName { get; set; }
        [DataMember]
        public String CustomColumn { get; set; }
        [DataMember]
        public String ProductColumn { get; set; }
        [DataMember]
        public String Industry { get; set; }
        [DataMember]
        public String Address { get; set; }
        [DataMember]
        public Decimal latitude { get; set; }
        [DataMember]
        public Decimal longitude { get; set; }
        [DataMember]
        public String Tel { get; set; }
        [DataMember]
        public Int32 isAddress { get; set; }
        [DataMember]
        public Int32 isTel { get; set; }
        [DataMember]
        public Int32 DisplayCard { get; set; }
        [DataMember]
        public List<InfoSortVO> Modular { get; set; }
        [DataMember]
        public List<InfoSortVO> NewsSort { get; set; }
    }
}