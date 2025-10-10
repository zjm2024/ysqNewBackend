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

public class CsharpTest_GetYan
{
    public static GetYanVO Main()
    {
        try
        {
            String url = "https://api.7585.net.cn/yan/api.php?lx=mj";
            String method = "GET";

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

            string ReadText = reader.ReadToEnd();

            GetYanVO YanVO = new GetYanVO();

            if (ReadText.IndexOf("---") > -1)
            {
                YanVO.Utterance = ReadText.Split(new char[3] { '-', '-', '-' })[0].Trim();
                YanVO.Author = ReadText.Split(new char[3] { '-', '-', '-' })[3].Trim();
                YanVO.Splits = ReadText.Split(new char[3] { '-', '-', '-' });
            }
            else if (ReadText.IndexOf("——") > -1)
            {
                YanVO.Utterance = ReadText.Split(new char[2] { '—', '—' })[0].Trim();
                YanVO.Author = ReadText.Split(new char[2] { '—', '—' })[2].Trim();
                YanVO.Splits = ReadText.Split(new char[2] { '—', '—' });
            }
            else
            {
                YanVO = null;
            }

            return YanVO;
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
    public class GetYanVO
    {
        public string Utterance { get; set; }
        public string Author { set; get; }
        public string[] Splits { set; get; }
    }
}