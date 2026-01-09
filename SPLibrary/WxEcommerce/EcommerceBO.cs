using CoreFramework.DAO;
using CoreFramework.VO;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxEcommerce;

namespace SPLibrary.WxEcommerce
{
    public class EcommerceBO
    {
        /// <summary>
        /// 图片上传API
        /// </summary>
        /// <param name="imgurl">图片链接</param>
        /// <returns></returns>
        public async Task<string> uploadimgGetmediaId(string imgurl)
        {
            imgurl = imgurl.Replace("https://www.zhongxiaole.net/SPManager", ConfigInfo.Instance.UploadFolder);
            imgurl = imgurl.Replace("https://api.leliaomp.com/SPManager", ConfigInfo.Instance.UploadFolder);
            ResultCode response = await HttpService.UploadFile(imgurl);
            
            if (response.code == "SUCCESS")
            {
                dynamic resultContent = JsonConvert.DeserializeObject(response.ResultStr, new { media_id = "" }.GetType());
                return resultContent.media_id;
            }
            else
            {
                LogBO _log = new LogBO(typeof(EcommerceBO));
                string strErrorMsg = "上传微信图片失败:" + JsonConvert.SerializeObject(response);
                _log.Error(strErrorMsg);

                return "";
            }
        }


        /// <summary>
        /// 敏感信息加密(使用平台证书加密)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Encryption(string text)
        {
            return RSAManager.RSAEncrypt(text, Encoding.UTF8.GetBytes(EConfig.GetConfig().GetWxPublicKey()));
        }

        /// <summary>
        /// 敏感信息解密（使用apiV3密钥解密）
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Decrypt(string associatedData, string nonce, string ciphertext)
        {
            return AesGcm.AesGcmDecrypt(associatedData, nonce, ciphertext);
        }

        /// <summary>
        /// 下载平台证书
        /// </summary>
        /// <returns></returns>
        public async Task<ResultCode> GetPlatformCert()
        {
            string url = "https://api.mch.weixin.qq.com/v3/certificates";
            ResultCode response = await HttpService.Get(url,false);
            
            if (response.code == "SUCCESS")
            {
                PlatformCert_ResultVO Result = JsonConvert.DeserializeObject<PlatformCert_ResultVO>(response.ResultStr);
                response.ResultObj = Result;

                //更新服务器平台证书

                ICertificatesDAO rDAO = CustomerManagementDAOFactory.CertificatesDAO(new CustomerProfile());
                try
                {
                    CertificatesVO cVO = FindCertificatesById();
                    if (Result.data.Count > 0)
                    {
                        cVO.serial_no = Result.data[0].serial_no;
                        cVO.effective_time = Result.data[0].effective_time;
                        cVO.expire_time = Result.data[0].expire_time;
                        cVO.algorithm = Result.data[0].encrypt_certificate.algorithm;
                        cVO.nonce = Result.data[0].encrypt_certificate.nonce;
                        cVO.PublicKey = Decrypt(Result.data[0].encrypt_certificate.associated_data, Result.data[0].encrypt_certificate.nonce, Result.data[0].encrypt_certificate.ciphertext);
                    }
                    
                    rDAO.UpdateById(cVO);
                }
                catch (Exception ex)
                {
                    LogBO _log = new LogBO(typeof(EcommerceBO));
                    string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                    _log.Error(strErrorMsg);
                }


            }

            return response;
        }

        /// <summary>
        /// 获取平台证书
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public CertificatesVO FindCertificatesById()
        {
            ICertificatesDAO rDAO = CustomerManagementDAOFactory.CertificatesDAO(new CustomerProfile());
            return rDAO.FindById(1);
        }

        /// <summary>
        /// 二级商户进件
        /// </summary>
        /// <param name="MerchantVO">提交资料</param>
        /// <returns></returns>
        public async Task<ResultCode> Applyments(MerchantVO MerchantVO)
        {
            //上传图片到微信服务器
            MerchantVO.id_card_info.id_card_national = await uploadimgGetmediaId(MerchantVO.id_card_info.id_card_national);
            MerchantVO.id_card_info.id_card_copy = await uploadimgGetmediaId(MerchantVO.id_card_info.id_card_copy);

            //敏感信息加密
            MerchantVO.id_card_info.id_card_name = Encryption(MerchantVO.id_card_info.id_card_name);
            MerchantVO.id_card_info.id_card_number = Encryption(MerchantVO.id_card_info.id_card_number);

            MerchantVO.contact_info.contact_name = Encryption(MerchantVO.contact_info.contact_name);
            MerchantVO.contact_info.contact_id_card_number = Encryption(MerchantVO.contact_info.contact_id_card_number);
            MerchantVO.contact_info.mobile_phone = Encryption(MerchantVO.contact_info.mobile_phone);
            MerchantVO.account_info.account_number = Encryption(MerchantVO.account_info.account_number);
            MerchantVO.account_info.account_name = Encryption(MerchantVO.account_info.account_name);

            string url = "https://api.mch.weixin.qq.com/v3/ecommerce/applyments/";
            string json = JsonConvert.SerializeObject(MerchantVO);

            ResultCode response = await HttpService.Post(json, url);
            response.PostBody = json;
            if (response.code == "SUCCESS")
            {
                response.ResultObj = JsonConvert.DeserializeObject<applyment_ResultVO>(response.ResultStr);
            }

            return response;
        }


        /// <summary>
        /// 修改结算账号
        /// </summary>
        /// <param name="ModifySettlementVO">提交资料</param>
        /// <param name="sub_mchid">二级商户号</param>
        /// <returns></returns>
        public async Task<ResultCode> ModifySettlement(string sub_mchid,ModifySettlementVO ModifySettlementVO)
        {
            //敏感信息加密
            ModifySettlementVO.account_number = Encryption(ModifySettlementVO.account_number);

            string url = "https://api.mch.weixin.qq.com/v3/apply4sub/sub_merchants/"+ sub_mchid + "/modify-settlement";
            string json = JsonConvert.SerializeObject(ModifySettlementVO);

            ResultCode response = await HttpService.Post(json, url);
            response.PostBody = json;
            if (response.code == "SUCCESS")
            {
                //response.ResultObj = JsonConvert.DeserializeObject<applyment_ResultVO>(response.ResultStr);
            }

            return response;
        }

        /// <summary>
        /// 查询结算账号
        /// </summary>
        /// <returns></returns>
        public async Task<ResultCode> GetSettlement(string sub_mchid)
        {
            string url = "https://api.mch.weixin.qq.com/v3/apply4sub/sub_merchants/"+ sub_mchid + "/settlement";
            ResultCode response = await HttpService.Get(url, false);

            if (response.code == "SUCCESS")
            {
                ModifySettlementVO Result = JsonConvert.DeserializeObject<ModifySettlementVO>(response.ResultStr);
                response.ResultObj = Result;
            }

            return response;
        }

        /// <summary>
        /// 查询二级商户账户实时余额
        /// </summary>
        /// <returns></returns>
        public async Task<ResultCode> GetBalance(string sub_mchid)
        {
            string url = "https://api.mch.weixin.qq.com/v3/ecommerce/fund/balance/" + sub_mchid;
            ResultCode response = await HttpService.Get(url, false);

            if (response.code == "SUCCESS")
            {
                MerchantBalanceVO Result = JsonConvert.DeserializeObject<MerchantBalanceVO>(response.ResultStr);
                response.ResultObj = Result;
            }

            return response;
        }

        /// <summary>
        /// 查询申请状态
        /// </summary>
        /// <returns></returns>
        public async Task<ResultCode> GetApplyments(string applyment_id)
        {
            string url = "https://api.mch.weixin.qq.com/v3/ecommerce/applyments/"+ applyment_id;
            ResultCode response = await HttpService.Get(url, false);

            if (response.code == "SUCCESS")
            {
                getapplyment_ResultVO Result = JsonConvert.DeserializeObject<getapplyment_ResultVO>(response.ResultStr);
                response.ResultObj = Result;
            }

            return response;
        }

        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="sp_appid">应用appid</param>
        /// <param name="sub_mchid">二级商户号</param>
        /// <param name="description">商品描述</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="total">总金额，单位为分</param>
        /// <param name="sp_openid">支付者openid</param>
        /// <param name="notify_url">回调路径</param>
        /// <returns></returns>
        public async Task<AppletsPayDataVO> GetPay(string sp_appid, string sub_mchid, string description, string out_trade_no, int total, string sp_openid,string notify_url,bool profit_sharing=false)
        {
            string sp_mchid = EConfig.GetConfig().GetMchID();//服务商商户号

            var data = new {
                amount = new
                {
                    total = total,
                    currency = "CNY"
                },
                sp_mchid = sp_mchid,
                description = description,
                notify_url = notify_url,
                payer = new
                {
                    sp_openid = sp_openid
                },
                sp_appid = sp_appid,
                out_trade_no = out_trade_no,
                sub_mchid = sub_mchid,
                settle_info = new
                {
                    profit_sharing = profit_sharing
                }
            };
            string url = "https://api.mch.weixin.qq.com/v3/pay/partner/transactions/jsapi";
            string json = JsonConvert.SerializeObject(data);
            ResultCode response = await HttpService.Post(json, url);
            response.PostBody = json;
            if (response.code == "SUCCESS")
            {
                dynamic resultContent = JsonConvert.DeserializeObject(response.ResultStr, new { prepay_id = "" }.GetType());
                string prepay_id = resultContent.prepay_id;

                //生成支付数据
                AppletsPayDataVO aVO = new AppletsPayDataVO();

                aVO.appId = sp_appid;

                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
                aVO.timeStamp= ((DateTime.Now.Ticks - startTime.Ticks) / 10000000).ToString();
                aVO.nonceStr = Guid.NewGuid().ToString().Replace("-", "");
                aVO.package = "prepay_id="+ prepay_id;
                aVO.signType = "RSA";

                string message = $"{aVO.appId}\n{aVO.timeStamp}\n{aVO.nonceStr}\n{aVO.package}\n";
                aVO.paySign = RSAManager.GenSign(message, EConfig.GetConfig().GetPrivateKey());

                return aVO;
            }else
            {
                LogBO _log = new LogBO(typeof(EcommerceBO));
                string strErrorMsg = "二级商户支付错误:" + JsonConvert.SerializeObject(response);
                _log.Error(strErrorMsg);
            }

            return null;
        }

        /// <summary>
        /// 请求分账
        /// </summary>
        /// <param name="sp_appid">应用appid</param>
        /// <param name="sub_mchid">二级商户号</param>
        /// <param name="description">商品描述</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="total">总金额，单位为分</param>
        /// <returns></returns>
        public async Task<ResultCode> GetProfitsharing(string sp_appid, string sub_mchid,string transaction_id, string out_order_no, int total)
        {
            string sp_mchid = EConfig.GetConfig().GetMchID();//服务商商户号

            receivers receivers = new receivers();
            receivers.type = "MERCHANT_ID";
            receivers.receiver_account = sp_mchid;
            receivers.amount = total;
            receivers.description = "平台抽成";

            List<receivers> rList = new List<receivers>();
            rList.Add(receivers);

            var data = new
            {
                appid = sp_appid,
                sub_mchid = sub_mchid,
                transaction_id = transaction_id,
                out_order_no = out_order_no,
                receivers = rList,
                finish = true
            };
            string url = "https://api.mch.weixin.qq.com/v3/ecommerce/profitsharing/orders";
            string json = JsonConvert.SerializeObject(data);
            ResultCode response = await HttpService.Post(json, url);
            response.PostBody = json;
            return response;
        }

        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="sp_appid">应用appid</param>
        /// <param name="sub_mchid">二级商户号</param>
        /// <param name="description">商品描述</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="total">总金额，单位为分</param>
        /// <returns></returns>
        public async Task<ResultCode> Refund(string sp_appid, string sub_mchid, string out_order_no, int total)
        {
            string sp_mchid = EConfig.GetConfig().GetMchID();//服务商商户号

            var data = new
            {
                sp_appid = sp_appid,
                sub_mchid = sub_mchid,
                out_trade_no = out_order_no,
                out_refund_no = out_order_no,
                amount =new {
                    refund= total,
                    total= total,
                    currency = "CNY"
                }
            };
            string url = "https://api.mch.weixin.qq.com/v3/ecommerce/refunds/apply";
            string json = JsonConvert.SerializeObject(data);
            ResultCode response = await HttpService.Post(json, url);
            response.PostBody = json;
            return response;
        }

        /// <summary>
        /// 添加二级商户记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddMerchant(wxMerchantVO vo)
        {
            try
            {
                IwxMerchantDAO rDAO = CustomerManagementDAOFactory.wxMerchantDAO(new CustomerProfile());

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int MerchantID = rDAO.Insert(vo);
                    return MerchantID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(EcommerceBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新二级商户记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateMerchant(wxMerchantVO vo)
        {
            IwxMerchantDAO rDAO = CustomerManagementDAOFactory.wxMerchantDAO(new CustomerProfile());
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(EcommerceBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 更新二级商户记录
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int UpdateMerchant(string data, string condition)
        {
            IwxMerchantDAO rDAO = CustomerManagementDAOFactory.wxMerchantDAO(new CustomerProfile());
            return rDAO.Update(data,condition);
        }
        /// <summary>
        /// 获取二级商户记录列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<wxMerchantVO> FindMerchantByCondition(string condition, params object[] parameters)
        {
            IwxMerchantDAO rDAO = CustomerManagementDAOFactory.wxMerchantDAO(new CustomerProfile());
            return rDAO.FindByParams(condition);
        }

        /// <summary>
        /// 获取会员是否有二级商户
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public wxMerchantVO getMyMerchant(int CustomerId)
        {
            List<wxMerchantVO> mVO = FindMerchantByCondition("CustomerId=" + CustomerId + " and applyment_state='FINISH' and Status=1");
            if (mVO.Count > 0)
            {
                return mVO[0];
            }else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取企业是否有二级商户
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public wxMerchantVO getBusinessMerchant(int BusinessID)
        {
            List<wxMerchantVO> mVO = FindMerchantByCondition("BusinessID=" + BusinessID + " and applyment_state='FINISH' and Status=1");
            if (mVO.Count > 0)
            {
                return mVO[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取二级商户记录数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindMerchantTotalCount(string condition, params object[] parameters)
        {
            IwxMerchantDAO rDAO = CustomerManagementDAOFactory.wxMerchantDAO(new CustomerProfile());
            return rDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 获取二级商户记录详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public wxMerchantVO FindMerchantById(int MerchantID)
        {
            IwxMerchantDAO rDAO = CustomerManagementDAOFactory.wxMerchantDAO(new CustomerProfile());
            return rDAO.FindById(MerchantID);
        }
    }
}
