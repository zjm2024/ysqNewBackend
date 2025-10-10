using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.WxEcommerce
{
    /// <summary>
    /// 商户入驻提交资料
    /// </summary>
    [DataContract]
    public class MerchantVO
    {
        [DataMember]
        public string out_request_no { get; set; }//业务申请编号
        [DataMember]
        public string organization_type { get; set; }//主体类型
        [DataMember]
        public id_card_info id_card_info { get; set; }//经营者/法人身份证信息
        [DataMember]
        public account_info account_info { get; set; }//结算银行账户
        [DataMember]
        public contact_info contact_info { get; set; }//超级管理员信息
        [DataMember]
        public sales_scene_info sales_scene_info { get; set; }//店铺信息
        [DataMember]
        public string merchant_shortname { get; set; }//店铺简称
    }
    public class id_card_info
    {
        [DataMember]
        public string id_card_copy { get; set; }//身份证人像面照片MediaID
        [DataMember]
        public string id_card_national { get; set; }//身份证国徽面照片MediaID
        [DataMember]
        public string id_card_name { get; set; }//身份证姓名
        [DataMember]
        public string id_card_number { get; set; }//身份证号码
        [DataMember]
        public string id_card_valid_time_begin { get; set; }//身份证开始时间
        [DataMember]
        public string id_card_valid_time { get; set; }//身份证有效期限
    }
    public class account_info
    {
        [DataMember]
        public string bank_account_type { get; set; }//账户类型
        [DataMember]
        public string account_bank { get; set; }//开户银行
        [DataMember]
        public string account_name { get; set; }//开户名称
        [DataMember]
        public string bank_address_code { get; set; }//开户银行省市编码
        [DataMember]
        public string account_number { get; set; }//银行帐号
    }
    public class contact_info
    {
        [DataMember]
        public string contact_type { get; set; }//超级管理员类型
        [DataMember]
        public string contact_name { get; set; }//超级管理员姓名
        [DataMember]
        public string contact_id_card_number { get; set; }//超级管理员身份证件号码
        [DataMember]
        public string mobile_phone { get; set; }//超级管理员手机
    }
    public class sales_scene_info
    {
        [DataMember]
        public string store_name { get; set; }//店铺名称
        [DataMember]
        public string store_url { get; set; }//店铺链接
        [DataMember]
        public string store_qr_code { get; set; }//店铺二维码
    }

    /// <summary>
    /// 商户入驻提交返回数据
    /// </summary>
    public class applyment_ResultVO
    {
        [DataMember]
        public string applyment_id { get; set; }//微信支付申请单号
        [DataMember]
        public string out_request_no { get; set; }//业务申请编号
    }

    /// <summary>
    /// 获取平台证书返回数据
    /// </summary>
    public class PlatformCert_ResultVO
    {
        public List<ResultData> data { get; set; }
    }

    public class ResultData
    {
        public string serial_no { get; set; }//平台证书序列号
        public DateTime effective_time { get; set; }
        public DateTime expire_time { get; set; }
        public encrypt_certificate encrypt_certificate { get; set; }
    }

    public class encrypt_certificate
    {
        public string algorithm { get; set; }
        public string nonce { get; set; }
        public string associated_data { get; set; }
        public string ciphertext { get; set; }
    }

    //统一返回数据类
    public class ResultCode
    {
        public string code { get; set; }
        public string ResultStr { get; set; }
        public object ResultObj { get; set; }
        public string message { get; set; }
        public string PostBody { get; set; }
    }

    //查询申请状态返回数据
    public class getapplyment_ResultVO
    {
        public string applyment_state { get; set; }
        //CHECKING：资料校验中
        //ACCOUNT_NEED_VERIFY：待账户验证
        //AUDITING：审核中
        //REJECTED：已驳回
        //NEED_SIGN：待签约
        //FINISH：完成
        //FROZEN：已冻结

        public string applyment_state_desc { get; set; }

        public string sign_state { get; set; }
        //1、UNSIGNED：未签约。该状态下，电商平台可查询获取签约链接，引导二级商户的超级管理员完成签约；
        //2、SIGNED ：已签约。指二级商户的超级管理员已完成签约。注意：若申请单被驳回，商户修改了商户主体名称、法人名称、超级管理员信息、主体类型等信息，则需重新签约。
        //3、NOT_SIGNABLE：不可签约。该状态下，暂不支持超级管理员签约。一般为申请单处于已驳回、已冻结、机器校验中状态，无法签约。

        public string sign_url { get; set; }
        public string sub_mchid { get; set; }//二级商户号   
    }

    //修改结算提交数据
    public class ModifySettlementVO
    {
        [DataMember]
        public string account_type { get; set; }
        [DataMember]
        public string account_bank { get; set; }
        [DataMember]
        public string bank_address_code { get; set; }
        [DataMember]
        public string account_number { get; set; }
    }

    //查询余额返回数据
    public class MerchantBalanceVO
    {
        [DataMember]
        public string sub_mchid { get; set; }
        [DataMember]
        public int available_amount { get; set; }
        [DataMember]
        public int pending_amount { get; set; }
        [DataMember]
        public string account_type { get; set; }
    }

    //小程序支付数据
    public class AppletsPayDataVO
    {
        [DataMember]
        public string appId { get; set; }
        [DataMember]
        public string timeStamp { get; set; }
        [DataMember]
        public string nonceStr { get; set; }
        [DataMember]
        public string package { get; set; }
        [DataMember]
        public string signType { get; set; }
        [DataMember]
        public string paySign { get; set; }
    }

    //支付回调数据
    public class NotifyDataVO
    {
        [DataMember]
        public string create_time { get; set; }
        [DataMember]
        public string resource_type { get; set; }
        [DataMember]
        public string event_type { get; set; }
        [DataMember]
        public string summary { get; set; }
        [DataMember]
        public Resource resource { get; set; }
    }

    public class Resource
    {
        [DataMember]
        public string original_type { get; set; }
        [DataMember]
        public string algorithm { get; set; }
        [DataMember]
        public string ciphertext { get; set; }
        [DataMember]
        public string associated_data { get; set; }
        [DataMember]
        public string nonce { get; set; }
    }

    public class ResourceDecrypt
    {
        [DataMember]
        public string sp_appid { get; set; }
        [DataMember]
        public string sp_mchid { get; set; }
        [DataMember]
        public string out_trade_no { get; set; }
        [DataMember]
        public string transaction_id { get; set; }
        [DataMember]
        public string trade_state { get; set; }
    }

    public class receivers
    {
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string receiver_account { get; set; }
        [DataMember]
        public int amount { get; set; }
        [DataMember]
        public string description { get; set; }
    }

    public class ResultReceivers
    {
        [DataMember]
        public int amount { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string receiver_account { get; set; }
        [DataMember]
        public string receiver_mchid { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string detail_id { get; set; }
        [DataMember]
        public string fail_reason { get; set; }
        [DataMember]
        public string result { get; set; }
    }
}
