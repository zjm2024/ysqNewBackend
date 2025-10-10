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

public class CsharpTest_GetCity
{
    public static String HMACSHA1Text(String EncryptText, String EncryptKey)
    {
        HMACSHA1 hmacsha1 = new HMACSHA1();
        hmacsha1.Key = System.Text.Encoding.UTF8.GetBytes(EncryptKey);

        byte[] dataBuffer = System.Text.Encoding.UTF8.GetBytes(EncryptText);
        byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
        return Convert.ToBase64String(hashBytes);
    }

    public static GetCityVO Main(string mobile)
    {
        try
        {
            String url = "https://service-av27cw4h-1257598706.ap-shanghai.apigateway.myqcloud.com/release/mobile";
            String method = "GET";
            String querys = "mobile="+ mobile;
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
            }

            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));

        
            var model = JsonConvert.DeserializeObject<GetCityVO>(reader.ReadToEnd());
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
public class GetCityVO
{
    public int code { get; set;}
    public string message { get; set; }
    public string trade_no { get; set; }
    public GetCityData data { get; set; }
}
public class GetCityData
{
    public string types { get; set; }
    public string lng { get; set; }
    public string city { get; set; }
    public string num { get; set; }
    public string isp { get; set; }
    public string area_code { get; set; }
    public string city_code { get; set; }
    public string prov { get; set; }
    public string zip_code { get; set; }
    public string lat { get; set; }
}
