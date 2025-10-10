using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.BusinessCardManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class MyDataVO : CommonVO, ICommonVO,ICloneable
    {
        [DataMember]
        public Int32 PersonalID { get; set; }
        [DataMember]
        public Int32 BusinessID { get; set; }
        [DataMember]
		public Int32 TotalShare { get; set; }
        [DataMember]
        public Int32 TotalRead { get; set; }
        [DataMember]
        public Int32 TotalRanking { get; set; }
        [DataMember]
        public Int32 PersonalShare { get; set; }
        [DataMember]
        public Boolean PersonalShareUP { get; set; }
        [DataMember]
        public Int32 PersonalRead { get; set; }
        [DataMember]
        public Decimal PersonalConversion { get; set; }

        [DataMember]
        public Int32 NewsShare { get; set; }
        [DataMember]
        public Boolean NewsShareUP { get; set; }
        [DataMember]
        public Int32 NewsRead { get; set; }
        [DataMember]
        public Decimal NewsConversion { get; set; }

        [DataMember]
        public Int32 GreetingCardShare { get; set; }
        [DataMember]
        public Boolean GreetingCardShareUP { get; set; }
        [DataMember]
        public Int32 GreetingCardRead { get; set; }
        [DataMember]
        public Decimal GreetingCardConversion { get; set; }

        [DataMember]
        public Int32 ColorPageShare { get; set; }
        [DataMember]
        public Boolean ColorPageShareUP { get; set; }
        [DataMember]
        public Int32 ColorPageRead { get; set; }
        [DataMember]
        public Decimal ColorPageConversion { get; set; }

        [DataMember]
        public Int32 ProductShare { get; set; }
        [DataMember]
        public Boolean ProductShareUP { get; set; }
        [DataMember]
        public Int32 ProductRead { get; set; }
        [DataMember]
        public Decimal ProductConversion { get; set; }

        [DataMember]
        public Int32 ClueAdd { get; set; }
        [DataMember]
        public Boolean ClueAddUP { get; set; }

        [DataMember]
        public Int32 ClientsAdd { get; set; }
        [DataMember]
        public Boolean ClientsAddUP { get; set; }

        [DataMember]
        public Int32 GoOutAdd { get; set; }
        [DataMember]
        public Boolean GoOutAddUP { get; set; }

        [DataMember]
        public Int32 ContractAdd { get; set; }
        [DataMember]
        public Boolean ContractAddUP { get; set; }


        [DataMember]
        public Decimal DepartmentTarget { get; set; }
        [DataMember]
        public Decimal DepartmentTargetCompletion { get; set; }
        [DataMember]
        public Decimal DepartmentTargetCost { get; set; }
        [DataMember]
        public Decimal PersonalTarget { get; set; }
        [DataMember]
        public Decimal PersonalTargetCompletion { get; set; }
        [DataMember]
        public Decimal PersonalTargetCost { get; set; }
        [DataMember]
        public Decimal BusinessTarget { get; set; }
        [DataMember]
        public Decimal BusinessTargetCompletion { get; set; }
        [DataMember]
        public Decimal BusinessTargetCost { get; set; }
    }
}