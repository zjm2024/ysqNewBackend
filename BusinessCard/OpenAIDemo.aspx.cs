using CoreFramework.VO;
using Newtonsoft.Json;
using SPLibrary.CoreFramework.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BusinessCard
{
    public partial class OpenAIDemo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = "Button1";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (TextBox2.Text != "")
            {
                string url = "";
                string Prompt = TextBox2.Text;
                url = "http://43.153.108.54/SPWebAPI/OpenAI/GetText?Prompt="+ Prompt;
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                ResultObject Result = JsonConvert.DeserializeObject<ResultObject>(jsonStr);
                if (Result.Flag == 1)
                {
                    TextBox1.Text = Result.Result.ToString();
                }else
                {
                    TextBox1.Text = "出现错误，请重试！";
                }
            }
            
        }
    }
}