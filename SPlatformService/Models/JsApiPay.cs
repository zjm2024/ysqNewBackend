using LitJson;
using SPLibrary.CoreFramework.Logging.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace SPlatformService.Models
{
    public class JsApiPay
    {
        /// <summary>
        /// 保存页面对象，因为要在类的方法中使用Page的Request对象
        /// </summary>
        private Page page { get; set; }

        /// <summary>
        /// openid用于调用统一下单接口
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// access_token用于获取收货地址js函数入口参数
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 商品金额，用于统一下单
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 统一下单接口返回结果
        /// </summary>
        public WxPayData unifiedOrderResult { get; set; }

        public JsApiPay()
        {

        }

        /**
	    * 
	    * 通过code换取网页授权access_token和openid的返回数据，正确时返回的JSON数据包如下：
	    * {
	    *  "access_token":"ACCESS_TOKEN",
	    *  "expires_in":7200,
	    *  "refresh_token":"REFRESH_TOKEN",
	    *  "openid":"OPENID",
	    *  "scope":"SCOPE",
	    *  "unionid": "o6_bmasdasdsad6_2sgVt7hMZOPfL"
	    * }
	    * 其中access_token可用于获取共享收货地址
	    * openid是微信支付jsapi支付接口统一下单时必须的参数
        * 更详细的说明请参考网页授权获取用户基本信息：http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
        * @失败时抛异常WxPayException
	    */
        public UserInfoModel GetOpenidAndAccessTokenFromCode(string code)
        {
            try
            {
                UserInfoModel um = new UserInfoModel();
                System.Web.Caching.Cache _cache = HttpRuntime.Cache;
                //构造获取openid及access_token的url
                if (_cache[code] == null)
                {
                    WxPayData data = new WxPayData();
                    data.SetValue("appid", WxPayConfig.APPID);
                    data.SetValue("secret", WxPayConfig.APPSECRET);
                    data.SetValue("code", code);
                    data.SetValue("grant_type", "authorization_code");
                    string url = "https://api.weixin.qq.com/sns/oauth2/access_token?" + data.ToUrl();

                    //请求url以获取数据
                    string result = HttpService.Get(url);

                    Log.Debug(this.GetType().ToString(), "GetOpenidAndAccessTokenFromCode response : " + result);
                    JsonData jd = JsonMapper.ToObject(result);
                    access_token = (string)jd["access_token"];
                    openid = (string)jd["openid"];
                    DateTime accTokenTime = DateTime.Now;
                    um.OpenId = openid;
                    um.access_token = access_token;
                    um.accTokenTime = accTokenTime;
                    //获取用户openid

                    //保存access_token，用于收货地址获取

                    _cache.Insert(code, um, null, DateTime.Now.AddHours(2), TimeSpan.Zero);

                }
                else
                {
                    um = (UserInfoModel)_cache[code];
                }
                //JsonData jd = JsonMapper.ToObject(codeResult);
                //access_token = (string)jd["access_token"];

                ////获取用户openid
                //openid = (string)jd["openid"];
                Log.Debug(this.GetType().ToString(), "Get openid : " + openid);
                Log.Debug(this.GetType().ToString(), "Get access_token : " + access_token);
                return um;
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex.ToString());
                throw new WxPayException(ex.ToString());
            }
        }

        /**
         * 调用统一下单，获得下单结果
         * @return 统一下单结果
         * @失败时抛异常WxPayException
         */
        public WxPayData GetUnifiedOrderResult(string out_trade_no, string openid, string total_fee, string body, string attach, string goods)
        {
            //统一下单
            WxPayData data = new WxPayData();
            //string _out_trade_no = WxPayApi.GenerateOutTradeNo();
            
            data.SetValue("body", body);
            data.SetValue("attach", attach);
            data.SetValue("out_trade_no", out_trade_no);
            data.SetValue("total_fee", total_fee);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("goods_tag", goods);
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", openid);
            
            WxPayData result = WxPayApi.UnifiedOrder(data);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error!");
            }
            result.SetValue("out_trade_no", out_trade_no);
            //   transaction_id
            return result;
        }
        /**
         * 调用统一下单，获得下单结果
         * @return 统一下单结果
         * @失败时抛异常WxPayException
         */
        public WxPayData GetUnifiedOrderResult(string appid, string out_trade_no, string openid, string total_fee, string body, string attach, string goods)
        {
            //统一下单
            WxPayData data = new WxPayData();
            //string _out_trade_no = WxPayApi.GenerateOutTradeNo();

            data.SetValue("body", body);
            data.SetValue("attach", attach);
            data.SetValue("out_trade_no", out_trade_no);
            data.SetValue("total_fee", total_fee);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("goods_tag", goods);
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", openid);

            WxPayData result = WxPayApi.UnifiedOrder(appid,data);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error!");
            }
            result.SetValue("out_trade_no", out_trade_no);
            //   transaction_id
            return result;
        }
        /**
         * 调用统一下单，获得下单结果
         * @return 统一下单结果
         * @失败时抛异常WxPayException
         */
        public WxPayData GetUnifiedOrderResult(string appid, string out_trade_no, string openid, string total_fee, string body, string attach, string goods,string notify_url,int AppType=1,int isprofit_sharing=0)
        {
            //统一下单
            WxPayData data = new WxPayData();
            //string _out_trade_no = WxPayApi.GenerateOutTradeNo();

            data.SetValue("body", body);
            data.SetValue("attach", attach);
            data.SetValue("out_trade_no", out_trade_no);
            data.SetValue("total_fee", total_fee);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("goods_tag", goods);
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", openid);
            data.SetValue("notify_url", notify_url);

            if (isprofit_sharing == 1)
            {
                data.SetValue("profit_sharing", "Y");
            }

            WxPayData result = WxPayApi.UnifiedOrder(appid, data,6, AppType);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error!");
            }
            result.SetValue("out_trade_no", out_trade_no);
            //   transaction_id
            return result;
        }
        /**

        /**
  * 调用统一下单，获得下单结果
  * @return 统一下单结果
  * @失败时抛异常WxPayException
  */
        public string GetUnifiedOrderData(string out_trade_no, string total_fee, string body, string attach, string goods)
        {
            //统一下单
            WxPayData data = new WxPayData();
            //string _out_trade_no = WxPayApi.GenerateOutTradeNo();

            data.SetValue("body", body);
            data.SetValue("attach", attach);
            data.SetValue("out_trade_no", out_trade_no);
            data.SetValue("total_fee", total_fee);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("goods_tag", goods);
            data.SetValue("trade_type", "APP");
            //data.SetValue("openid", openid);

            WxPayData result = WxPayApi.UnifiedOrderData(data);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error!");
            }
            
            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("appid", result.GetValue("appid").ToString());
            jsApiParam.SetValue("timestamp", WxPayApi.GenerateTimeStamp());
            jsApiParam.SetValue("noncestr", result.GetValue("nonce_str").ToString());
            jsApiParam.SetValue("partnerid", result.GetValue("mch_id").ToString());
            jsApiParam.SetValue("package", "Sign=WXPay");        
            jsApiParam.SetValue("prepayid", result.GetValue("prepay_id").ToString());
            jsApiParam.SetValue("sign", jsApiParam.MakeSign());
            //jsApiParam.SetValue("out_trade_no", unifiedOrderResult.GetValue("out_trade_no"));
            string parameters = jsApiParam.ToJson();
            //result.SetValue("out_trade_no", out_trade_no);
            //   transaction_id
            return jsApiParam.ToJson();
        }

        /**
         * 查询订单
         * @return return 成功时返回订单查询结果，其他抛异常
         * @out_trade_no 商户订单号
         */
        public OrderQueryData OrderQueryResult(string out_trade_no)
        {
            
            //查询订单
            WxPayData req = new WxPayData();
            req.SetValue("out_trade_no", out_trade_no);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                OrderQueryData data = new OrderQueryData();
                data.out_trade_no = res.GetValue("out_trade_no").ToString();
                data.trade_state = res.GetValue("trade_state").ToString();
                data.total = Convert.ToInt32(res.GetValue("total_fee"));
                return data;
            }
            else
            {
                return null;
            }
        }



        public WxPayData GetRefundResult(string openid, string total_fee, string refund_fee, string out_trade_no,int AppType=1, string appid="")
        {
            //申请退款
            WxPayData data = new WxPayData();

            string _out_refund_no = WxPayApi.GenerateOutTradeNo(AppType);

            // data.SetValue("body", body);
            // data.SetValue("attach", attach);
            data.SetValue("out_trade_no", out_trade_no);
            data.SetValue("out_refund_no", _out_refund_no);
            data.SetValue("total_fee", total_fee);
            data.SetValue("refund_fee", refund_fee);
            //data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            // data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            //data.SetValue("goods_tag", goods);
            // data.SetValue("trade_type", "JSAPI");
            data.SetValue("op_user_id", openid);

            WxPayData result = WxPayApi.Refund(data, appid,6, AppType);

            //if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            //{
            //    Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
            //    throw new WxPayException("UnifiedOrder response error!");
            //}
            result.SetValue("out_trade_no", out_trade_no);
            return result;
        }

        /// <summary>
        /// 请求分账
        /// </summary>
        /// <param name="openid">分账接收方账号openid</param>
        /// <param name="amount">分账金额（单位为分）</param>
        /// <param name="description">分账描述</param>
        /// <param name="transaction_id">微信订单号</param>
        /// <param name="out_order_no">商户分账单号</param>
        /// <param name="AppType">小程序编号</param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public Boolean GetProfitsharingResult(string receivers, string transaction_id, string out_order_no, int AppType = 1, string appid = "")
        {
            //请求分账
            WxPayData data = new WxPayData();
            data.SetValue("transaction_id", transaction_id);
            data.SetValue("out_order_no", out_order_no);
            data.SetValue("receivers", receivers);

            WxPayData result = WxPayApi.Profitsharing(data, appid, AppType);

            if(result.GetValue("return_code").ToString() == "SUCCESS" && result.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 添加分账接收方
        /// </summary>
        /// <param name="openid">分账接收方账号openid</param>
        /// <param name="AppType">小程序编号</param>
        /// <returns></returns>
        public Boolean GetProfitsharingResult(string openid ,int AppType = 1)
        {
            //请求分账
            WxPayData data = new WxPayData();
            var receivers = "{";
            receivers += "\"type\": \"PERSONAL_OPENID\",";
            receivers += "\"account\":\"" + openid + "\",";
            receivers += "\"relation_type\":\"USER\"";
            receivers += "}";
            data.SetValue("receiver", receivers);
            WxPayData result = WxPayApi.ProfitsharingAddReceiver(data, AppType);

            if (result.GetValue("return_code").ToString() == "SUCCESS" && result.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
        *  
        * 从统一下单成功返回的数据中获取微信浏览器调起jsapi支付所需的参数，
        * 微信浏览器调起JSAPI时的输入参数格式如下：
        * {
        *   "appId" : "wx2421b1c4370ec43b",     //公众号名称，由商户传入     
        *   "timeStamp":" 1395712654",         //时间戳，自1970年以来的秒数     
        *   "nonceStr" : "e61463f8efa94090b1f366cccfbbb444", //随机串     
        *   "package" : "prepay_id=u802345jgfjsdfgsdg888",     
        *   "signType" : "MD5",         //微信签名方式:    
        *   "paySign" : "70EA570631E4BB79628FBCA90534C63FF7FADD89" //微信签名 
        * }
        * @return string 微信浏览器调起JSAPI时的输入参数，json格式可以直接做参数用
        * 更详细的说明请参考网页端调起支付API：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7
        * 
        */
        public string GetJsApiParameters(WxPayData unifiedOrderResult,int AppType=1)
        {
            Log.Debug(this.GetType().ToString(), "JsApiPay::GetJsApiParam is processing...");

            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", unifiedOrderResult.GetValue("appid"));
            jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
            jsApiParam.SetValue("package", "prepay_id=" + unifiedOrderResult.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign(AppType));
            jsApiParam.SetValue("out_trade_no", unifiedOrderResult.GetValue("out_trade_no"));
            string parameters = jsApiParam.ToJson();

            Log.Debug(this.GetType().ToString(), "Get jsApiParam : " + parameters);
            return parameters;
        }


        /**
	    * 
	    * 获取收货地址js函数入口参数,详情请参考收货地址共享接口：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_9
	    * @return string 共享收货地址js函数需要的参数，json格式可以直接做参数使用
	    */
        public string GetEditAddressParameters()
        {
            string parameter = "";
            try
            {
                string host = page.Request.Url.Host;
                string path = page.Request.Path;
                string queryString = page.Request.Url.Query;
                //这个地方要注意，参与签名的是网页授权获取用户信息时微信后台回传的完整url
                string url = "http://" + host + path + queryString;

                //构造需要用SHA1算法加密的数据
                WxPayData signData = new WxPayData();
                signData.SetValue("appid", WxPayConfig.APPID);
                signData.SetValue("url", url);
                signData.SetValue("timestamp", WxPayApi.GenerateTimeStamp());
                signData.SetValue("noncestr", WxPayApi.GenerateNonceStr());
                signData.SetValue("accesstoken", access_token);
                string param = signData.ToUrl();

                Log.Debug(this.GetType().ToString(), "SHA1 encrypt param : " + param);
                //SHA1加密
                string addrSign = FormsAuthentication.HashPasswordForStoringInConfigFile(param, "SHA1");
                Log.Debug(this.GetType().ToString(), "SHA1 encrypt result : " + addrSign);

                //获取收货地址js函数入口参数
                WxPayData afterData = new WxPayData();
                afterData.SetValue("appId", WxPayConfig.APPID);
                afterData.SetValue("scope", "jsapi_address");
                afterData.SetValue("signType", "sha1");
                afterData.SetValue("addrSign", addrSign);
                afterData.SetValue("timeStamp", signData.GetValue("timestamp"));
                afterData.SetValue("nonceStr", signData.GetValue("noncestr"));

                //转为json格式
                parameter = afterData.ToJson();
                Log.Debug(this.GetType().ToString(), "Get EditAddressParam : " + parameter);
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex.ToString());
                throw new WxPayException(ex.ToString());
            }

            return parameter;
        }
    }
    public class OrderQueryData
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 订单状态：SUCCESS：支付成功 REFUND：转入退款 NOTPAY：未支付 CLOSED：已关闭 REVOKED：已撤销（仅付款码支付会返回）USERPAYING：用户支付中（仅付款码支付会返回）PAYERROR：支付失败（仅付款码支付会返回）
        /// </summary>
        public string trade_state { get; set; }
        /// <summary>
        /// 订单金额（单位：分）
        /// </summary>
        public int total { get; set; }
    }
}