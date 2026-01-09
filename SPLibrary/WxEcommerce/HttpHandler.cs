using Aop.Api.Util;
using SPLibrary.WxEcommerce;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WxEcommerce
{
    // 使用方法
    // HttpClient client = new HttpClient(new HttpHandler("{商户号}", "{商户证书序列号}"));
    // ...
    // var response = client.GetAsync("https://api.mch.weixin.qq.com/v3/certificates");
    public class HttpHandler : DelegatingHandler
    {
        private readonly string merchantId;
        private readonly string serialNo;
        private readonly bool isWxSerial;//是否加入平台证书序列号
        private readonly string SignBody;//自定义用于签名的body

        public HttpHandler(string merchantId, string merchantSerialNo,bool isWechatpaySerial=true,string SignBody="")
        {
            InnerHandler = new HttpClientHandler();

            this.merchantId = merchantId;
            this.serialNo = merchantSerialNo;
            this.isWxSerial = isWechatpaySerial;
            this.SignBody = SignBody;
        }

        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var auth = await BuildAuthAsync(request);
            string value = $"WECHATPAY2-SHA256-RSA2048 {auth}";
            request.Headers.Add("Authorization", value);
            request.Headers.Add("Accept", "application/json");//如果缺少这句代码就会导致下单接口请求失败，报400错误（Bad Request）
            if (isWxSerial) request.Headers.Add("Wechatpay-Serial", EConfig.GetConfig().GetWxSerial());
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_0) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11");//如果缺少这句代码就会导致下单接口请求失败，报400错误（Bad Request）
            return await base.SendAsync(request, cancellationToken);
        }

        protected async Task<string> BuildAuthAsync(HttpRequestMessage request)
        {
            string method = request.Method.ToString();
            string body = "";
            if (method == "POST" || method == "PUT" || method == "PATCH")
            {
                var content = request.Content;
                if (SignBody != "")
                {
                    body = SignBody;//自定义签名body
                }
                else
                {
                    body = await content.ReadAsStringAsync();//debug的时候在这里打个断点，看看body的值是多少，如果跟你传入的参数不一致，说明是有问题的，一定参考我的方法
                }
                
            }

            string uri = request.RequestUri.PathAndQuery;

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));       
            var timestamp = (DateTime.Now.Ticks - startTime.Ticks) / 10000000;//除10000调整为13位

            string nonce = timestamp.ToString();

            string message = $"{method}\n{uri}\n{timestamp}\n{nonce}\n{body}\n";
            string signature = RSAManager.GenSign(message,EConfig.GetConfig().GetPrivateKey());
            string auth= $"mchid=\"{merchantId}\",serial_no=\"{serialNo}\",nonce_str=\"{nonce}\",timestamp=\"{timestamp}\",signature=\"{signature}\"";
            return auth;
        }
    }
}