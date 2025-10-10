using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
	[DataContract]
	[Serializable]
    public partial class CertificatesVO : CommonVO, ICommonVO,ICloneable
    {
    	readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(CertificatesVO));

        [DataMember]
        public Int32 CertificatesID { get { return (Int32)GetValue(typeof(Int32), "CertificatesID"); } set { SetValue("CertificatesID", value); } }
        [DataMember]
        public String serial_no { get { return (String)GetValue(typeof(String), "serial_no"); } set { SetValue("serial_no", value); } }
        [DataMember]
        public DateTime effective_time { get { return (DateTime)GetValue(typeof(DateTime), "effective_time"); } set { SetValue("effective_time", value); } }
        [DataMember]
        public DateTime expire_time { get { return (DateTime)GetValue(typeof(DateTime), "expire_time"); } set { SetValue("expire_time", value); } }
        [DataMember]
        public String algorithm { get { return (String)GetValue(typeof(String), "algorithm"); } set { SetValue("algorithm", value); } }
        [DataMember]
        public String nonce { get { return (String)GetValue(typeof(String), "nonce"); } set { SetValue("nonce", value); } }
        [DataMember]
        public String PublicKey { get { return (String)GetValue(typeof(String), "PublicKey"); } set { SetValue("PublicKey", value); } }

        #region ICloneable Member
        public override object Clone()
        {
            CertificatesVO tmp = new CertificatesVO();
            tmp.changeData = new Dictionary<string, object>(this.changeData);
            tmp.originData = new Dictionary<string, object>(this.originData);
            return tmp;
        }
        #endregion
         
    	#region ICommonVO Member
        List<string> ICommonVO.PropertyList
        {
            get { return _propertyList; }
        }
        #endregion
    }
}