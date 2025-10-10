using System.IO;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Web;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SPLibrary.WebConfigInfo
{
    /// <summary>
    /// 全国快递查询
    /// </summary>
    public class TencentExpress
    {
        public static String HMACSHA1Text(String EncryptText, String EncryptKey)
        {
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = System.Text.Encoding.UTF8.GetBytes(EncryptKey);

            byte[] dataBuffer = System.Text.Encoding.UTF8.GetBytes(EncryptText);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// 全国物流快递查询
        /// </summary>
        /// <param name="com">快递公司代码</param>
        /// <param name="num">快递单号</param>
        public static ExpressVO Main(string num, string com="")
        {
            try
            {
                String url = "https://service-ohohpvok-1300683954.gz.apigw.tencentcs.com/release/express";
                String method = "GET";
                String querys = "com="+ com + "&num="+ num;
                String source = "market";

                //云市场分配的密钥Id
                String secretId = "AKID6W52fJOmUfg57XQUafCtW3Ir8C08hG5Z5R0f";
                //云市场分配的密钥Key
                String secretKey = "5R10rgo87243XpQUvfAvAo4wWB55QuainWVyqN4t";

                String dt = DateTime.UtcNow.GetDateTimeFormats('r')[0];
                url = url + "?" + querys;

                String signStr = "x-date: " + dt + "\n" + "x-source: " + source;
                String sign = HMACSHA1Text(signStr, secretKey);

                String auth = "hmac id=\"" + secretId + "\", algorithm=\"hmac-sha1\", headers=\"x-date x-source\", signature=\"";
                auth = auth + sign + "\"";
                Console.WriteLine(auth + "\n");

                HttpWebRequest httpRequest = null;
                HttpWebResponse httpResponse = null;

                if (url.Contains("https://"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                }
                else
                {
                    httpRequest = (HttpWebRequest)WebRequest.Create(url);
                }

                httpRequest.Method = method;
                httpRequest.Headers.Add("Authorization", auth);
                httpRequest.Headers.Add("X-Source", source);
                httpRequest.Headers.Add("X-Date", dt);

                try
                {
                    httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                }
                catch (WebException ex)
                {
                    httpResponse = (HttpWebResponse)ex.Response;
                }

                Console.WriteLine(httpResponse.StatusCode);
                Console.WriteLine(httpResponse.Headers);
                Stream st = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));

                var model = JsonConvert.DeserializeObject<ExpressVO>(reader.ReadToEnd());
                return model;

            }
            catch
            {
                return null;
            }
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        #region 图片转base64
        /// <summary>
        /// 图片转base64
        /// </summary>
        /// <param name="path">图片路径</param>      
        /// <returns>返回一个base64字符串</returns>
        public static string decodeImageToBase64(string path)
        {
            string base64str = "";

            //站点文件目录
            string fileDir = HttpContext.Current.Server.MapPath("/");
            string[] arrfileDir = fileDir.Split('\\');
            fileDir = arrfileDir[0] + "\\" + arrfileDir[1] + "\\" + arrfileDir[2];
            try
            {
                //读图片转为Base64String
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(path);
                //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(path);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                bmp.Dispose();
                base64str = Convert.ToBase64String(arr);
            }
            catch (Exception e)
            {
                string mss = e.Message;
            }
            //return "data:image/jpg;base64," + base64str;
            return base64str;
        }
        #endregion
    }
    public class ExpressVO
    {
        public string code { get; set; }
        public string no { get; set; }
        public string type { get; set; }
        public List<Expresslist> list { get; set; }
        public string state { get; set; }
        public string msg { get; set; }
        public string name { get; set; }
        public string site { get; set; }
        public string phone { get; set; }
        public string logo { get; set; }
        public string courier { get; set; }
        public string courierPhone { get; set; }
        public string updateTime { get; set; }
        public string takeTime { get; set; }
    }
    public class Expresslist
    {
        public string content { get; set; }
        public string time { get; set; }
    }
}
