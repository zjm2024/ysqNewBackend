using Newtonsoft.Json;
using SPLibrary.CoreFramework.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.WebConfigInfo
{
    public class TencentShop
    {
        public string appid = "";
        public string secret = "";
        public TencentShop(string Appid, string Secret)
        {
            appid = Appid;
            secret = Secret;
        }
        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <returns></returns>
        public string get_access_token()
        {
            string url;
            url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret + "";
            string jsonStr = HttpHelper.HtmlFromUrlGet(url);

            var result = new WeiXinAccessTokenResultDYH();
            if (jsonStr.Contains("errcode"))
            {
                var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                result.ErrorResult = errorResult;
                result.Result = false;
                return "";
            }
            else
            {
                var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModelDYH>(jsonStr);
                result.SuccessResult = model;
                result.Result = true;
                return result.SuccessResult.access_token;
            }
        }
        public bool AddProduct() {
            return false;
        }
    }
    public class WxProductVO{
        public int out_product_id;
        public string title;
        public string path;
        public List<string> head_img;
        public List<string> qualification_pics;
    }
    public class desc_infoVO
    {
        public string desc;
        public List<string> imgs;
    }
}
