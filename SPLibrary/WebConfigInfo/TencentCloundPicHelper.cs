using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Ocr.V20181119;
using TencentCloud.Ocr.V20181119.Models;

namespace SPLibrary.WebConfigInfo
{
    public class TencentBusinesscardResponse
    {
        public List<ImgInfo> result_list = new List<ImgInfo>();
    }

    public class ImgInfo
    {

        /// <summary>
        /// 错误码，0 为成功
        /// </summary>
        public int code { set; get; }

        /// <summary>
        /// 服务器返回的信息
        /// </summary>
        public string message { set; get; }

        /// <summary>
        /// 当前图片的 url
        /// </summary>
        public string url { set; get; }

        /// <summary>
        /// 具体查询数据，具体见实体
        /// </summary>
        public List<Data> data;
    }

    public class Data
    {
        public string item { set; get; }
        public string value { set; get; }
        public double confidence { set; get; }
    }
    [DataContract]
    public class IdCardData
    {
        [DataMember]
        public string Name { set; get; }
        [DataMember]
        public string Sex { set; get; }
        [DataMember]
        public string Nation { set; get; }
        [DataMember]
        public string Birth { set; get; }
        [DataMember]
        public string Address { set; get; }
        [DataMember]
        public string IdNum { set; get; }
        [DataMember]
        public string Authority { set; get; }
        [DataMember]
        public string ValidDate { set; get; }
    }

    public class TencentCloundPicHelper
    {
        /// <summary>
        /// 发送Post请求腾讯云
        /// </summary>
        public string SendPost(string postDataStr, string url = @"https://recognition.image.myqcloud.com/ocr/businesscard",string Method= "POST")
        {

            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                var request = (HttpWebRequest)HttpWebRequest.Create(url);

                request.Method = Method;

                SetHeaderValue(request.Headers, "content-type", "application/json");
                SetHeaderValue(request.Headers, "content-length", postDataStr.Length.ToString());
                SetHeaderValue(request.Headers, "Host", "recognition.image.myqcloud.com");
                SetHeaderValue(request.Headers, "authorization", GetSign());
                var memStream = new MemoryStream();

                var jsonByte = Encoding.GetEncoding("utf-8").GetBytes(postDataStr);
                memStream.Write(jsonByte, 0, jsonByte.Length);

                request.ContentLength = memStream.Length;
                var requestStream = request.GetRequestStream();
                memStream.Position = 0;
                var tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();

                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();

                var response = request.GetResponse();
                using (var s = response.GetResponseStream())
                {
                    var reader = new StreamReader(s, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException we)
            {
                if (we.Status == WebExceptionStatus.ProtocolError)
                {
                    using (var s = we.Response.GetResponseStream())
                    {
                        var reader = new StreamReader(s, Encoding.UTF8);
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    throw we;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        public static string GetSign()
        {
            var appId = "1258097819";//开发者的 APPID，接入智能图像时由系统生成
            var bucket = "tengxunyun";//Bucket，空间名称，即图片资源的组织管理单元
            var secretId = "AKIDOgnkZGmCSXR9f4AUadWElQ1Rp6EDrjNq";//Secret ID
            var secretKey = "RhmgPGuTr33xgLuIerusiXDiOzSky0rX";//secretKey
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var currentTime = Convert.ToInt64(ts.TotalSeconds);//当前时间戳，是一个符合 UNIX Epoch 时间戳规范的数值，单位为秒，多次签名时，e 应大于 t
            var expiredTime = Convert.ToInt64((DateTime.UtcNow.AddMinutes(180) - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);//签名的有效期，是一个符合 UNIX Epoch 时间戳规范的数值，单位为秒；单次签名时，e 必须设置为 0
            var rand = GetRandom();//随机串，无符号 10 进制整数，用户需自行生成，最长 10 位
            var userid = 0;//历史遗留字段，请填写为 0
            var fileid = "";//资源存储的唯一标识，单次签名必填；多次签名选填，如填写则会验证与当前操作的文件路径是否一致。
            var encryptText = $"a={appId}&b={bucket}&k={secretId}&e={expiredTime}&t={currentTime}&r={rand}&u=0&f=";
            var sign = Hmacsha1Encrypt(encryptText, secretKey);

            return sign;
        }

        /// <summary>
        /// 设置Http post请求头
        /// </summary>
        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }

        /// <summary>
        /// HMACSHA1算法加密
        /// </summary>
        private static string Hmacsha1Encrypt(string encryptText, string encryptKey)
        {
            using (HMACSHA1 mac = new HMACSHA1(Encoding.UTF8.GetBytes(encryptKey)))
            {
                var hash = mac.ComputeHash(Encoding.UTF8.GetBytes(encryptText));
                var pText = Encoding.UTF8.GetBytes(encryptText);
                var all = new byte[hash.Length + pText.Length];
                Array.Copy(hash, 0, all, 0, hash.Length);
                Array.Copy(pText, 0, all, hash.Length, pText.Length);
                return Convert.ToBase64String(all);
            }
        }

        /// <summary>
        /// 获取随机数
        /// </summary>
        private static int GetRandom()
        {
            var random = new Random();
            var rand = random.Next(10000, 999999999);
            return rand;
        }

        public async Task<string> IDCardOCR(string img,string CardSide) {
            try
            {
                Credential cred = new Credential
                {
                    SecretId = "AKIDOgnkZGmCSXR9f4AUadWElQ1Rp6EDrjNq",
                    SecretKey = "RhmgPGuTr33xgLuIerusiXDiOzSky0rX"
                };

                ClientProfile clientProfile = new ClientProfile();
                HttpProfile httpProfile = new HttpProfile();
                httpProfile.Endpoint = ("ocr.tencentcloudapi.com");
                clientProfile.HttpProfile = httpProfile;

                OcrClient client = new OcrClient(cred, "ap-guangzhou", clientProfile);
                IDCardOCRRequest req = new IDCardOCRRequest();
                string strParams = "{\"ImageUrl\":\"" + img + "\",\"CardSide\":\"" + CardSide + "\",\"Config\":\"{\\\"CopyWarn\\\":true,\\\"DetectPsWarn\\\":true,\\\"TempIdWarn\\\":true,\\\"InvalidDateWarn\\\":true}\"}";
                req = IDCardOCRRequest.FromJsonString<IDCardOCRRequest>(strParams);

                IDCardOCRResponse resp = new IDCardOCRResponse();
                resp = await client.IDCardOCR(req);
                return AbstractModel.ToJsonString(resp);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}
