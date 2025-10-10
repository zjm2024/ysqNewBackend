
using CoreFramework.VO;
using SPlatformService.Controllers;
using SPlatformService.TokenMange;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Card
{
    public partial class SoftArticle : System.Web.UI.Page
    {
        public string Token = "";
        public int CustomerId = 0;
        public int SoftArticleID = 0;
        public CardSoftArticleVO cVO = new CardSoftArticleVO();
        public CardPartyVO pVO = new CardPartyVO();
        public ViewBag ViewBag;
        protected void Page_Load(object sender, EventArgs e)
        {
            //wxlogin.login();
            SoftArticleID = Convert.ToInt32(string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["SoftArticleID"]) ? "0" : HttpContext.Current.Request.QueryString["SoftArticleID"]);
            //Token = wxlogin.Token;
            //CustomerId = wxlogin.CustomerId;

            CardBO cBO = new CardBO(new CustomerProfile());
            cVO.Card = new CardDataVO();
            cVO.OriginalCard = new CardDataVO();
            cVO = cBO.FindSoftArticleById(SoftArticleID);
            if (cVO != null)
            {
                CardSoftArticleVO CardSoftArticleVO = new CardSoftArticleVO();
                CardSoftArticleVO.SoftArticleID = cVO.SoftArticleID;
                CardSoftArticleVO.ExposureCount = cVO.ExposureCount + 3;


                if (cVO.OriginalSoftArticleID > 0)
                {
                    CardSoftArticleVO ocVO = cBO.FindSoftArticleById(cVO.OriginalSoftArticleID);
                    ocVO.ReadCount += 1;
                    if (!IsPostBack)
                    {
                        cBO.UpdateSoftArticle(ocVO);
                    }
                    cVO.ReadCount = ocVO.ReadCount;
                    cVO.ReprintCount = ocVO.ReprintCount;
                    cVO.GoodCount = ocVO.GoodCount;
                }
                else
                {
                    CardSoftArticleVO.ReadCount += cVO.ReadCount + 1;
                }
                if (!IsPostBack)
                {
                    cBO.UpdateSoftArticle(CardSoftArticleVO);
                }

                if (cVO.Description !="")
                {
                    cVO.Description = cVO.Description.Replace("<video ", "<video  controls='controls' playsinline='true' webkit-playsinline = 'true'  x5-playsinline = 'true'");
                }

                if (cVO.CardID > 0)
                {
                    cVO.Card = cBO.FindCardById(cVO.CardID);
                }
                else
                {
                    List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(cVO.CustomerId);
                    if (CardDataVO.Count > 0)
                    {
                        cVO.Card = CardDataVO[0];
                    }
                }

                if (cVO.CustomerId != cVO.OriginalCustomerId)
                {
                    List<CardDataVO> CardDataVO2 = cBO.FindCardByCustomerId(cVO.OriginalCustomerId);
                    if (CardDataVO2.Count > 0)
                    {
                        cVO.OriginalCard = CardDataVO2[0];
                    }
                }else
                {
                    cVO.OriginalCard = cVO.Card;
                }
                if (cVO.PartyID > 0)
                {
                    pVO = cBO.FindPartyById(cVO.PartyID);
                }
                if (cVO.QRImg == "")
                {
                    cVO.QRImg = cBO.GetSoftArticleQR(cVO.SoftArticleID);
                }
            }

            GetWX();
        }
        /// <summary>
        /// 获取微信分享接口参数
        /// </summary>
        public void GetWX()
        {
            WX_JSSDK jssdk = new WX_JSSDK();
            ViewBag = jssdk.getSignPackage();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            if (cVO.OriginalSoftArticleID > 0)
            {
                CardSoftArticleVO ocVO = cBO.FindSoftArticleById(cVO.OriginalSoftArticleID);
                ocVO.GoodCount += 1;
                cBO.UpdateSoftArticle(ocVO);

                cVO.ReadCount = ocVO.ReadCount;
                cVO.ReprintCount = ocVO.ReprintCount;
                cVO.GoodCount = ocVO.GoodCount;
            }
            else
            {
                cVO.GoodCount += 1;
            }
            cBO.UpdateSoftArticle(cVO);

            HttpCookie myCookie = new HttpCookie("zan"+ cVO.SoftArticleID);
            myCookie.Value = "true";
            Response.Cookies.Add(myCookie);
        }
        /// <summary>
        /// 去除html标签
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public static string NoHTML(string htmlStr)
        {
            if (htmlStr == null)
            {
                return "";
            }
            else
            {
                //删除脚本
                htmlStr = Regex.Replace(htmlStr, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                //删除HTML
                htmlStr = Regex.Replace(htmlStr, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, @"([rn])[s]+", "", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, @"-->", "", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, @"<!--.*", "", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, @"&(quot|#34);", "", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, @"&(iexcl|#161);", "xa1", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, @"&(cent|#162);", "xa2", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, @"&(pound|#163);", "xa3", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, @"&(copy|#169);", "xa9", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, @"&#(d+);", "", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, " ", "", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, "/r", "", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, "/n", "", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, "\r", "", RegexOptions.IgnoreCase);
                htmlStr = Regex.Replace(htmlStr, "\n", "", RegexOptions.IgnoreCase);
                //特殊的字符
                htmlStr = htmlStr.Replace("<", "");
                htmlStr = htmlStr.Replace(">", "");
                htmlStr = htmlStr.Replace("*", "");
                htmlStr = htmlStr.Replace("-", "");
                htmlStr = htmlStr.Replace("?", "");
                htmlStr = htmlStr.Replace(",", "");
                htmlStr = htmlStr.Replace("/", "");
                htmlStr = htmlStr.Replace(";", "");
                htmlStr = htmlStr.Replace("*/", "");
                htmlStr = htmlStr.Replace("rn", "");
                htmlStr = HttpContext.Current.Server.HtmlEncode(htmlStr).Trim();
                return htmlStr;
            }
        }

        /// <summary>
        /// 截取指定长度中英文字符串方法
        /// 该方法是按照每个汉字两个字节计算，∴如要截取20个字符，需要将length设置为40
        /// </summary>
        /// <param name="stringToSub"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetFirstString(string stringToSub, int length)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
            char[] stringChar = stringToSub.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int nLength = 0;
            bool isCut = false;
            for (int i = 0; i < stringChar.Length; i++)
            {
                if (regex.IsMatch((stringChar[i]).ToString()))
                {
                    sb.Append(stringChar[i]);
                    nLength += 2;
                }
                else
                {
                    sb.Append(stringChar[i]);
                    nLength = nLength + 1;
                }

                if (nLength > length)
                {
                    isCut = true;
                    break;
                }
            }
            if (isCut)
                return sb.ToString() + "..";
            else
                return sb.ToString();
        }
    }
}