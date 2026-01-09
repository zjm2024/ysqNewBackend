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

public class CsharpTest_GetWeather
{
    public static String HMACSHA1Text(String EncryptText, String EncryptKey)
    {
        HMACSHA1 hmacsha1 = new HMACSHA1();
        hmacsha1.Key = System.Text.Encoding.UTF8.GetBytes(EncryptKey);

        byte[] dataBuffer = System.Text.Encoding.UTF8.GetBytes(EncryptText);
        byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
        return Convert.ToBase64String(hashBytes);
    }

    public static GetWeatherVO Main(string ip)
    {
        try
        {
            String url = "https://service-is751ged-1257101137.ap-shanghai.apigateway.myqcloud.com/release/weather/query";
            String method = "GET";
            String querys = "ip=" + ip;
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

        
            var model = JsonConvert.DeserializeObject<GetWeatherVO>(reader.ReadToEnd());
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
public class GetWeatherVO
{
    public int status { get; set;}
    public string msg { get; set; }
    public WeatherData result { get; set; }
}
public class WeatherData
{
    public string city { get; set; }
    public string cityid { get; set; }
    public string citycode { get; set; }
    public string date { get; set; }
    public string week { get; set; }
    public string weather { get; set; }
    public string temp { get; set; }
    public string temphigh { get; set; }
    public string templow { get; set; }
    public string img { get; set; }
    public string humidity { get; set; }
    public string pressure { get; set; }
    public string windspeed { get; set; }
    public string winddirect { get; set; }
    public string windpower { get; set; }
    public string updatetime { get; set; }
    public List<daily> daily { get; set; }
}
public class daily
{
    public string sunrise { get; set; }
    public string sunset { get; set; }
}