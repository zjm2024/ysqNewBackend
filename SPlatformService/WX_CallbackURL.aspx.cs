using CoreFramework.VO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace SPlatformService
{
    public partial class WX_CallbackURL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.IO.Stream s = Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();

                string xml = builder.ToString();
                string MsgID = "";
                string ArticleUrl = "";

                LogBO _log = new LogBO(this.GetType());
                _log.Info(xml);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
                XmlNodeList nodes = xmlNode.ChildNodes;
                foreach (XmlNode xn in nodes)
                {
                    XmlElement xe = (XmlElement)xn;
                    if (xe.Name == "MsgID")
                    {
                        MsgID = xe.InnerText;
                    }
                    if (xe.Name == "ArticleUrl")
                    {
                        ArticleUrl = xe.InnerText;
                    }
                    if (xe.Name== "ArticleUrlResult")
                    {
                        XmlNodeList ResultList = xe.ChildNodes;
                        foreach (XmlNode xr in ResultList)
                        {
                            XmlElement List = (XmlElement)xr;
                            XmlNodeList item = List.ChildNodes;
                            foreach (XmlNode xi in item)
                            {
                                XmlText it = (XmlText)xi;
                                if (it.Name == "ArticleUrl")
                                {
                                    ArticleUrl = it.InnerText;
                                }
                            }
                        }
                    }
                }

                if (MsgID != "")
                {
                    CardBO cBO = new CardBO(new CustomerProfile());
                    List<MediaVO> MediaVO = cBO.FindMediaByCondition("msg_id='" + MsgID+"'");
                    if (MediaVO.Count > 0)
                    {
                        MediaVO[0].Status = 1;
                        MediaVO[0].ArticleUrl = ArticleUrl;
                        cBO.UpdateMedia(MediaVO[0]);
                    }
                }

                _log.Info(MsgID+":"+ ArticleUrl);
            }
            catch(Exception ex)
            {
                LogBO _log = new LogBO(typeof(WX_CallbackURL));
                string strErrorMsg = ex.Message;
                _log.Error(strErrorMsg);
            }
            
        }

        private bool checkSignature()
        {
            string signature = Request["signature"];
            string timestamp = Request["timestamp"];
            string nonce = Request["nonce"];

            string token = "Leliao2077";
            List<string> tmpArr = new List<string>();
            tmpArr.Add(token);
            tmpArr.Add(timestamp);
            tmpArr.Add(nonce);
            tmpArr.Sort();

            string tmp = "";

            foreach(string item in tmpArr)
            {
                tmp += item;
            }
            tmp = SHA1(tmp, Encoding.UTF8);
            if (tmp == signature)
            {
                return true;
            }else{
                return false;
            }
        }
        /// <summary>
        /// SHA1 加密，返回大写字符串
        /// </summary>
        /// <param name="content">需要加密字符串</param>
        /// <param name="encode">指定加密编码</param>
        /// <returns>返回40位大写字符串</returns>
        public static string SHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }
    }
}