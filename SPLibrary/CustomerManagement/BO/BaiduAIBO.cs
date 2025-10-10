using CoreFramework.DAO;
using CoreFramework.VO;
using CoreFramework.DAO;
using Newtonsoft.Json;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SPLibrary.CustomerManagement.BO
{
    public class BaiduAIBO
    {
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();
        public BaiduAIBO() { }
        public BaiduAIBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }
        string API_Key = "eQP5c7n6K9h0Pl6s76XC7ccg";
        string Secret_Key = "sCdINX6LfSqxcLDTDTgYFCDEq7YPExqg";
        public string GetChat(string messages)
        {
            string url = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/eb-instant?access_token=" + GetAccessToken();

            string DataJson = string.Empty;
            
            DataJson += "{\"messages\": [";
           // DataJson += "{\"role\":\"user\",\"content\":\"请扮演乐聊名片的客服来与我对话，乐聊名片是2018年上线的基于微信小程序的智能营销工具，主要功能有电子名片，活动报名收费，商品发布，调查问卷，文章和短视频发布\"},";
          //  DataJson += "{\"role\":\"assistant\",\"content\":\"好的\"},";
            DataJson += "{";
            DataJson += "\"role\": \"user\",";
            DataJson += "\"content\": \"" + messages + "\"";
            DataJson += "}]}";

            string str = HttpHelper.HtmlFromUrlPost(url, DataJson);

            if (!(str.IndexOf("error_code") > -1))
            {
                BaiduChat BaiduChat = JsonConvert.DeserializeObject<BaiduChat>(str);
                return BaiduChat.result;
            }
            else
            {
                return str;
            }
        }

        public string GetAccessToken()
        {
            List<BaiduAIConfigVO> List = FindMiniprogramsByConditionStr("Token_expires_in > now()");

            if (List.Count > 0)
            {
                return List[0].Access_token;
            }
            else
            {
                string url = "https://aip.baidubce.com/oauth/2.0/token?client_id=" + API_Key + "&client_secret=" + Secret_Key + "&grant_type=client_credentials";
                string DataJson = string.Empty;
                string str = HttpHelper.HtmlFromUrlPost(url, DataJson);
                if (!(str.IndexOf("error") > -1))
                {
                    BaiduAccessToken AccessToken = JsonConvert.DeserializeObject<BaiduAccessToken>(str);

                    BaiduAIConfigVO cVO=new BaiduAIConfigVO();
                    cVO.Access_token = AccessToken.access_token;
                    cVO.Token_expires_in = DateTime.Now.AddSeconds(Convert.ToDouble(AccessToken.expires_in));
                    AddMiniprograms(cVO);
                    return AccessToken.access_token;
                }
                else
                {
                    return "";
                }
            }
        }


        /// <summary>
        /// 添加AccessToken
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddMiniprograms(BaiduAIConfigVO vo)
        {
            try
            {
                IBaiduAIConfigDAO uDAO = CustomerManagementDAOFactory.BaiduAIConfigDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int BaiduAIConfigID = uDAO.Insert(vo);
                    return BaiduAIConfigID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BaiduAIBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新AccessToken
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateMiniprograms(BaiduAIConfigVO vo)
        {
            IBaiduAIConfigDAO uDAO = CustomerManagementDAOFactory.BaiduAIConfigDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(BaiduAIBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 获取AccessToken列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<BaiduAIConfigVO> FindMiniprogramsByConditionStr(string condition)
        {
            IBaiduAIConfigDAO uDAO = CustomerManagementDAOFactory.BaiduAIConfigDAO(this.CurrentCustomerProfile);
            List<BaiduAIConfigVO> cVO = uDAO.FindByParams(condition);
            return cVO;
        }
        /// <summary>
        /// 获取AccessToken详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public BaiduAIConfigVO FindMiniprogramsById(int BaiduAIConfigID)
        {
            IBaiduAIConfigDAO uDAO = CustomerManagementDAOFactory.BaiduAIConfigDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(BaiduAIConfigID);
        }
    }
    public class BaiduAccessToken
    {
        public string refresh_token { get; set; }
        public string expires_in { get; set; }

        public string session_key { get; set; }
        public string access_token { get; set; }
        public string scope { get; set; }
        public string session_secret { get; set; }
    }
    public class BaiduChat
    {
        public string result { get; set; }
    }
}
