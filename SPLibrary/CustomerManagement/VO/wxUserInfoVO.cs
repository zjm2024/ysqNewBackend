using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class wxUserInfoVO
    {
        [DataMember]
        public String encryptedData { get; set; }
        [DataMember]
        public String errMsg { get; set; }
        [DataMember]
        public String iv { get; set; }
        [DataMember]
        public String signature { get; set; }
        [DataMember]
        public String rawData { get; set; }
        [DataMember]
        public wxUserInfo userInfo { get; set; }
    }
    public partial class wxUserInfo
    {
        [DataMember]
        public String avatarUrl{ get;set;}
        [DataMember]
        public String city { get; set; }
        [DataMember]
        public String country { get; set; }
        [DataMember]
        public int gender { get; set; }
        [DataMember]
        public String language { get; set; }
        [DataMember]
        public String nickName { get; set; }
        [DataMember]
        public String province { get; set; }
    }
    public partial class wxPhoneVO
    {
        [DataMember]
        public String encryptedData { get; set; }
        [DataMember]
        public String errMsg { get; set; }
        [DataMember]
        public String iv { get; set; }
    }

    public partial class myFileInfo
    {
        [DataMember]
        public String FileName { get; set; }
        [DataMember]
        public String Url { get; set; }
        [DataMember]
        public String Type { get; set; }
        [DataMember]
        public Int32 index { get; set; }
        [DataMember]
        public Boolean isVip { get; set; }
        [DataMember]
        public Int32 CardPoterID { get; set; }
    }

    public partial class myFileInfoAndCompanyVO
    {
        [DataMember]
        public List<myFileInfo> myFileInfo { get; set; }
        [DataMember]
        public List<CompanyVO> CompanyVO { get; set; }
    }
}