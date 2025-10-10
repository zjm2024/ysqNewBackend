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
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.WebConfigInfo;

public class CsharpTest
{
    public static String HMACSHA1Text(String EncryptText, String EncryptKey)
    {
        HMACSHA1 hmacsha1 = new HMACSHA1();
        hmacsha1.Key = System.Text.Encoding.UTF8.GetBytes(EncryptKey);

        byte[] dataBuffer = System.Text.Encoding.UTF8.GetBytes(EncryptText);
        byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
        return Convert.ToBase64String(hashBytes);
    }

    public static CsharpVO Main(string channel,int start)
    {
        try
        {
            String url = "https://service-aqvnjmiq-1257101137.gz.apigw.tencentcs.com/release/news/get";
            String method = "GET";
         
            String querys = "channel=" + HttpUtility.UrlEncode(channel) + "&num=40&start="+ start;
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
                LogBO _log = new LogBO(typeof(TencentFaceIdEntity));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
            }

            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));

        
            var model = JsonConvert.DeserializeObject<CsharpVO>(reader.ReadToEnd());
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
}
public class CsharpVO
{
    public int status { get; set;}
    public string msg { get; set; }
    public CsharpResult result { get; set; }
}

public class CsharpResult
{
    public string channel { get; set; }
    public string num { get; set; }
    public List<CsharpList> list { get; set; }
}
public class CsharpList
{
    public string title { get; set; }
    public DateTime time { get; set; }
    public string src { get; set; }
    public string category { get; set; }
    public string pic { get; set; }
    public string content { get; set; }
    public string url { get; set; }
    public string weburl { get; set; }
}