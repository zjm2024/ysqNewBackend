using CoreFramework.DAO;
using CoreFramework.VO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Drawing;
using System.IO;
using SPLibrary.WebConfigInfo;
using System.Linq;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.BusinessCardManagement.DAO;
using SPLibrary.LuckyDrawManagement.VO;
using SPLibrary.LuckyDrawManagement.DAO;
using SPLibrary.CustomerManagement.BO;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SPLibrary.LuckyDrawManagement.BO
{
    public class LuckyDrawBO
    {
        static public string appid = "wx00636efae4bd3df2";
        static public string secret = "a71e9ed24a77d5236d6470dced30c303";
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();
        public LuckyDrawBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }
        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string getOpenId(string code)
        {
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code";
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var result = new WeiXinAccessTokenResult();
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModel>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }

                return result.SuccessResult.openid;
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 获取Access_token
        /// </summary>
        /// <returns></returns>
        public string GetAccess_token()
        {
            try
            {
                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;
                string jsonStr = HttpHelper.HtmlFromUrlGet(url);
                var result = new WeiXinAccessTokenResult();
                if (jsonStr.Contains("errcode"))
                {
                    var errorResult = JsonConvert.DeserializeObject<WeiXinHelper.WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<WeiXinAccessTokenModel>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }

                return result.SuccessResult.access_token;
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 添加抽奖
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddLuckyDraw(LuckyDrawVO vo)
        {
            try
            {
                ILuckyDrawDAO rDAO = LuckyDrawManagementDAOFactory.LuckyDrawDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int LuckyDrawID = rDAO.Insert(vo);
                    return LuckyDrawID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(LuckyDrawBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新抽奖
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateLuckyDraw(LuckyDrawVO vo)
        {
            ILuckyDrawDAO rDAO = LuckyDrawManagementDAOFactory.LuckyDrawDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(LuckyDrawBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }
        /// <summary>
        /// 获取抽奖详情
        /// </summary>
        /// <param name="LuckyDrawID"></param>
        /// <returns></returns>
        public LuckyDrawVO FindLuckyDrawById(int LuckyDrawID)
        {
            ILuckyDrawDAO rDAO = LuckyDrawManagementDAOFactory.LuckyDrawDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(LuckyDrawID);
        }
        /// <summary>
        /// 获取抽奖列表（分页）
        /// </summary>
        /// <returns></returns>
        public List<LuckyDrawVO> FindLuckyDrawAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            ILuckyDrawDAO rDAO = LuckyDrawManagementDAOFactory.LuckyDrawDAO(this.CurrentCustomerProfile);
            List<LuckyDrawVO> cVO = rDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
            return cVO;
        }

        /// <summary>
        /// 获取抽奖数量
        /// </summary>
        /// <returns></returns>
        public int FindLuckyDrawCount(string condition)
        {
            ILuckyDrawDAO rDAO = LuckyDrawManagementDAOFactory.LuckyDrawDAO(this.CurrentCustomerProfile);
            return rDAO.FindTotalCount(condition);
        }

        /// <summary>
        /// 添加奖品
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddPrize(PrizeVO vo)
        {
            try
            {
                IPrizeDAO rDAO = LuckyDrawManagementDAOFactory.PrizeDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int PrizeID = rDAO.Insert(vo);
                    return PrizeID;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(LuckyDrawBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }

        /// <summary>
        /// 更新奖品
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdatePrize(PrizeVO vo)
        {
            IPrizeDAO rDAO = LuckyDrawManagementDAOFactory.PrizeDAO(this.CurrentCustomerProfile);
            try
            {
                rDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(LuckyDrawBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取奖品详情
        /// </summary>
        /// <param name="PrizeID"></param>
        /// <returns></returns>
        public PrizeVO FindPrizeById(int PrizeID)
        {
            IPrizeDAO rDAO = LuckyDrawManagementDAOFactory.PrizeDAO(this.CurrentCustomerProfile);
            return rDAO.FindById(PrizeID);
        }

        /// <summary>
        /// 获取奖品列表
        /// </summary>
        /// <param name="LuckyDrawID"></param>
        /// <returns></returns>
        public List<PrizeVO> FindPrizeByLuckyDrawID(int LuckyDrawID)
        {
            IPrizeDAO rDAO = LuckyDrawManagementDAOFactory.PrizeDAO(this.CurrentCustomerProfile);
            List<PrizeVO> cVO = rDAO.FindByParams("Status = 1 and LuckyDrawID = " + LuckyDrawID);
            return cVO;
        }
    }
}
