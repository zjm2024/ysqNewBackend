using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreFramework.VO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using TencentCloud.Ame.V20190916.Models;

namespace SPlatformService.UserManagement
{
    public partial class ChooseUpImg : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ListItem listItem = new ListItem("问候", "zhufu");
            DropDownList1.Items.Add(listItem);
            listItem = new ListItem("节气", "jieqi");
            DropDownList1.Items.Add(listItem);
            listItem = new ListItem("励志", "lizhi");
            DropDownList1.Items.Add(listItem);
            listItem = new ListItem("团队", "tuandui");
            DropDownList1.Items.Add(listItem);
            listItem = new ListItem("山水", "shanshui");
            DropDownList1.Items.Add(listItem);
            listItem = new ListItem("植物", "zhiwu");
            DropDownList1.Items.Add(listItem);
            listItem = new ListItem("美食", "meishi");
            DropDownList1.Items.Add(listItem);
            listItem = new ListItem("建筑", "jianzhu");
            DropDownList1.Items.Add(listItem);
            listItem = new ListItem("人物", "renwu");
            DropDownList1.Items.Add(listItem);
            listItem = new ListItem("动物", "dongwu");
            DropDownList1.Items.Add(listItem);

            listItem = new ListItem("新版海报", "1");
            DropDownList2.Items.Add(listItem);
            listItem = new ListItem("经典海报", "2");
            DropDownList2.Items.Add(listItem);
            listItem = new ListItem("背景音乐", "3");
            DropDownList2.Items.Add(listItem);
        }

        protected void FileUploadButton_Click(object sender, EventArgs e)
        {
            CardBO cBO = new CardBO(new CustomerProfile());
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            IList<HttpPostedFile> fileList= hfc.GetMultiple("MyFileUpload");
            string folder = "/UploadFolder/PoterFile/";

            int SizeType = Convert.ToInt32(DropDownList2.SelectedValue);
            if (SizeType == 1)
            {
                folder = "/UploadFolder/PoterFile/";
            }else if (SizeType == 2)
            {
                folder = "/UploadFolder/oldPoterFile/";
            }else if (SizeType == 3)
            {
                folder = "/UploadFolder/BGMFile/";
            }

            string path = ConfigInfo.Instance.UploadFolder + folder;              //指定文件路径，是项目下的一个文件夹；～表示当前网页所在的文件夹
            path = path.Replace("/", "\\");
            bool issusson = true;
            int exsum = 0;
            for (int i=0;i< fileList.Count; i++)
            {
                HttpPostedFile File = fileList[i];
                string FileName = Path.GetFileName(File.FileName);
                string fileExt = Path.GetExtension(FileName);

                string allowexts = "jpg|jpeg|png|gif|mp3|ogg|mp4";
                Regex allowext = new Regex(allowexts);
                if (allowext.IsMatch(fileExt))
                {
                    string newName = DateTime.Now.ToString("yyyyMMddHHmmssffff"+i) + fileExt;//新文件名
                   
                    try
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        File.SaveAs(path + newName);
                        Response.Write("<script>console.log('上传成功：" + newName + "');</script>");

                        CardPoterVO CardPoterVO = new CardPoterVO();
                        CardPoterVO.CardPoterID = 0;
                        CardPoterVO.CustomerId = 0;
                        CardPoterVO.FileName = FileName.Replace(fileExt,"");
                        CardPoterVO.Url = "https://www.zhongxiaole.net/SPManager"+ folder + newName;
                        CardPoterVO.Type= DropDownList1.SelectedValue;
                        CardPoterVO.SizeType= SizeType;
                        CardPoterVO.Order_info = 0;
                        CardPoterVO.CreatedAt = DateTime.Now;
                        cBO.AddCardPoter(CardPoterVO);
                    }
                    catch (Exception ee)
                    {
                        issusson = false;
                        exsum++;
                        Response.Write("<script>console.log('" + ee.Message + "');</script>");
                    }
                }
                else
                {
                    issusson = false;
                    exsum++;
                }
            }
            if (issusson)
            {
                Response.Write("<script>alert('上传成功！');</script>");
            }
            else
            {
                Response.Write("<script>alert('"+ exsum + "个文件上传失败！可能是文件类型不支持');</script>");
            }
            
        }
    }
}