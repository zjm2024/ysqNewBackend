using System;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Runtime.Serialization;

namespace SPLibrary.CustomerManagement.VO
{
    [DataContract]
    [Serializable]
    public partial class wxMerchantVO : CommonVO, ICommonVO, ICloneable
    {
        readonly static List<string> _propertyList = VOHelper.GetVOPropertyList(typeof(wxMerchantVO));

        [DataMember]
        public Int32 MerchantID { get { return (Int32)GetValue(typeof(Int32), "MerchantID"); } set { SetValue("MerchantID", value); } }
        [DataMember]
        public Int32 CustomerId { get { return (Int32)GetValue(typeof(Int32), "CustomerId"); } set { SetValue("CustomerId", value); } }
        [DataMember]
        public Int32 BusinessID { get { return (Int32)GetValue(typeof(Int32), "BusinessID"); } set { SetValue("BusinessID", value); } }
        [DataMember]
        public String out_request_no { get { return (String)GetValue(typeof(String), "out_request_no"); } set { SetValue("out_request_no", value); } }
        [DataMember]
        public String organization_type { get { return (String)GetValue(typeof(String), "organization_type"); } set { SetValue("organization_type", value); } }

        [DataMember]
        public String id_card_copy { get { return (String)GetValue(typeof(String), "id_card_copy"); } set { SetValue("id_card_copy", value); } }
        [DataMember]
        public String id_card_national { get { return (String)GetValue(typeof(String), "id_card_national"); } set { SetValue("id_card_national", value); } }
        [DataMember]
        public String id_card_name { get { return (String)GetValue(typeof(String), "id_card_name"); } set { SetValue("id_card_name", value); } }
        [DataMember]
        public String id_card_number { get { return (String)GetValue(typeof(String), "id_card_number"); } set { SetValue("id_card_number", value); } }
        [DataMember]
        public String id_card_valid_time { get { return (String)GetValue(typeof(String), "id_card_valid_time"); } set { SetValue("id_card_valid_time", value); } }
        [DataMember]
        public String id_card_valid_time_begin { get { return (String)GetValue(typeof(String), "id_card_valid_time_begin"); } set { SetValue("id_card_valid_time_begin", value); } }
        
        [DataMember]
        public String bank_account_type { get { return (String)GetValue(typeof(String), "bank_account_type"); } set { SetValue("bank_account_type", value); } }
        [DataMember]
        public String account_bank { get { return (String)GetValue(typeof(String), "account_bank"); } set { SetValue("account_bank", value); } }
        [DataMember]
        public String account_name { get { return (String)GetValue(typeof(String), "account_name"); } set { SetValue("account_name", value); } }
        [DataMember]
        public String bank_address_code { get { return (String)GetValue(typeof(String), "bank_address_code"); } set { SetValue("bank_address_code", value); } }
        [DataMember]
        public String account_number { get { return (String)GetValue(typeof(String), "account_number"); } set { SetValue("account_number", value); } }

        [DataMember]
        public String contact_type { get { return (String)GetValue(typeof(String), "contact_type"); } set { SetValue("contact_type", value); } }
        [DataMember]
        public String contact_name { get { return (String)GetValue(typeof(String), "contact_name"); } set { SetValue("contact_name", value); } }
        [DataMember]
        public String contact_id_card_number { get { return (String)GetValue(typeof(String), "contact_id_card_number"); } set { SetValue("contact_id_card_number", value); } }
        [DataMember]
        public String mobile_phone { get { return (String)GetValue(typeof(String), "mobile_phone"); } set { SetValue("mobile_phone", value); } }

        [DataMember]
        public String store_name { get { return (String)GetValue(typeof(String), "store_name"); } set { SetValue("store_name", value); } }
        [DataMember]
        public String store_url { get { return (String)GetValue(typeof(String), "store_url"); } set { SetValue("store_url", value); } }

        [DataMember]
        public String applyment_id { get { return (String)GetValue(typeof(String), "applyment_id"); } set { SetValue("applyment_id", value); } }
        [DataMember]
        public String applyment_state { get { return (String)GetValue(typeof(String), "applyment_state"); } set { SetValue("applyment_state", value); } }
        [DataMember]
        public String sub_mchid { get { return (String)GetValue(typeof(String), "sub_mchid"); } set { SetValue("sub_mchid", value); } }

        [DataMember]
        public Int32 SplitProportion { get { return (Int32)GetValue(typeof(Int32), "SplitProportion"); } set { SetValue("SplitProportion", value); } }
        
        #region ICloneable Member
        public override object Clone()
        {
            wxMerchantVO tmp = new wxMerchantVO();
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