using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BusinessCard.GenerateIMG
{
    public partial class ProductIMG : System.Web.UI.Page
    {
        public string QRimg { set; get; }
        public PersonalViewVO PersonalVO { set; get; }
        public BusinessCardVO BusinessCardVO { set; get; }
        public InfoVO InfoVO { set; get; }
        public String imgstr;
        protected void Page_Load(object sender, EventArgs e)
        {
            int PersonalID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PersonalID"]) ? "0" : Request.QueryString["PersonalID"]);
            int InfoID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["InfoID"]) ? "0" : Request.QueryString["InfoID"]);
            QRimg = string.IsNullOrEmpty(Request.QueryString["QRimg"]) ? "" : Server.UrlDecode(Request.QueryString["QRimg"]);
            QRimg = QRimg.Replace("https", "http");
            if (PersonalID > 0)
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO = cBO.FindPersonalViewById(PersonalID);
                if (PersonalVO == null)
                {
                    PersonalVO = new PersonalViewVO();
                }else
                {
                    PersonalVO.Headimg = PersonalVO.Headimg.Replace("https", "http");
                }
            }
            else
            {
                PersonalVO = new PersonalViewVO();
            }

            if (InfoID > 0)
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                InfoVO = cBO.FindInfoById(InfoID);
                if (InfoVO == null)
                {
                    InfoVO = new InfoVO();
                }else
                {

                    List<String> img = FindImgList(InfoVO.Image);
                    if (img.Count > 0)
                    {
                        imgstr = img[0].Replace("https", "http");
                    }
                    BusinessCardVO = cBO.FindBusinessCardById(InfoVO.BusinessID);
                }
            }
            else
            {
                BusinessCardVO = new BusinessCardVO();
                InfoVO = new InfoVO();
            }
        }
        public List<String> FindImgList(string Content)
        {
            string input = Content;
            string pattern = @"\[img\](?<href>[^\[\s]*)\[\/img\]";
            List<String> ImgList = new List<string>();

            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(input, pattern))
            {
                ImgList.Add(match.Groups["href"].Value);
            }

            return ImgList;
        }
    }
}