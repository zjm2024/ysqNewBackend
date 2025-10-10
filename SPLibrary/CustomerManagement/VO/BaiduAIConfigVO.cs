using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class BaiduAIConfigVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(BaiduAIConfigVO));

        [DataMember]
        public Int32 BaiduAIConfigID { get { return (Int32)GetValue(typeof(Int32), "BaiduAIConfigID"); } set { SetValue("BaiduAIConfigID", value); } }
        [DataMember]
        public DateTime Token_expires_in { get { return (DateTime)GetValue(typeof(DateTime), "Token_expires_in"); } set { SetValue("Token_expires_in", value); } }
        [DataMember]
        public String Access_token { get { return (String)GetValue(typeof(String), "Access_token"); } set { SetValue("Access_token", value); } }
        
        #region ICloneable Member
        public override object Clone()
        {
            BaiduAIConfigVO tmp = new BaiduAIConfigVO();
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