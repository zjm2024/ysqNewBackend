using CoreFramework.DAO;
using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.CustomerManagement.BO
{
    public static class AppBO
    {
         public static AppVO GetApp(int AppType)
        {
            AppVO AppVO = new AppVO();

            switch (AppType)
            {
                case 0:
                    AppVO.AppName = "乐聊名片企业版";
                    AppVO.AppId = "wxc9245bafef27dddf";
                    AppVO.Secret = "76fe22240a699f0cceb12c3118b49cab";
                    //商户平台配置
                    AppVO.MCHID = "1494588702";
                    AppVO.KEY = "huashunqingweixuxiaoqingyueer888";
                    AppVO.APPSECRET = "4d60dea1857d13c37fe7c8d11731a785";
                    AppVO.SSLCERT_PATH = "C:/web/ca/1494588702_20240312_cert/apiclient_cert.p12";
                    AppVO.SSLCERT_PASSWORD = "1494588702";
                    AppVO.AppType = 1;
                    AppVO.TPersonalID = 6;
                    break;
                case 1: 
                    AppVO.AppName = "乐聊名片个人版";
                    AppVO.AppId = "wx584477316879d7e9";
                    AppVO.Secret = "1e08e16c76895ff8584b591d447b754e"; 
                    //商户平台配置
                    AppVO.MCHID = "1494588702";
                    AppVO.KEY = "huashunqingweixuxiaoqingyueer888";
                    AppVO.APPSECRET = "4d60dea1857d13c37fe7c8d11731a785";
                    AppVO.SSLCERT_PATH = "C:/web/ca/1494588702_20240312_cert/apiclient_cert.p12";
                    AppVO.SSLCERT_PASSWORD = "1494588702";
                    AppVO.AppType = 1;
                    AppVO.TPersonalID = 6;
                    break;
                case 2:
                    AppVO.AppName = "引流王";
                    AppVO.AppId = "wxbe6347ce9f00fd0b";
                    AppVO.Secret = "936b0905c776a207174039336a217bcb";
                    //商户平台配置
                    AppVO.MCHID = "1494588702";
                    AppVO.KEY = "huashunqingweixuxiaoqingyueer888";
                    AppVO.APPSECRET = "4d60dea1857d13c37fe7c8d11731a785";
                    AppVO.SSLCERT_PATH = "C:/web/ca/1494588702_20240312_cert/apiclient_cert.p12";
                    AppVO.SSLCERT_PASSWORD = "1494588702";
                    AppVO.AppType = 1;
                    AppVO.TPersonalID = 6;
                    break;
                case 3:
                    AppVO.AppName = "微云智推";
                    AppVO.AppId = "wx2c0ce94e903bea9a";
                    AppVO.Secret = "a642ce61f3ce2e2cddbcfb8b116f0579";
                    //商户平台配置
                    AppVO.MCHID = "1494588702";
                    AppVO.KEY = "weiyunzhituiLLMP20200814shanghup";
                    AppVO.APPSECRET = "weiyunzhituiLLMP20200814shanghup";
                    AppVO.SSLCERT_PATH = "C:/web/ca/1494588702_20240312_cert/apiclient_cert.p12";
                    AppVO.SSLCERT_PASSWORD = "1494588702";
                    AppVO.AppType = 3;
                    AppVO.TPersonalID = 6;
                    break;
                case 4:
                    AppVO.AppName = "活动星选";
                    AppVO.AppId = "wx83bf84d3847abf2f";
                    AppVO.Secret = "dcdddcd1f79943500e2fb210b1684185";
                    //商户平台配置
                    AppVO.MCHID = "1494588702";
                    AppVO.KEY = "huashunqingweixuxiaoqingyueer888";
                    AppVO.APPSECRET = "4d60dea1857d13c37fe7c8d11731a785";
                    AppVO.SSLCERT_PATH = "C:/web/ca/1494588702_20240312_cert/apiclient_cert.p12";
                    AppVO.SSLCERT_PASSWORD = "1494588702";
                    AppVO.AppType = 1;
                    AppVO.TPersonalID = 6;
                    break;
                case 5:
                    AppVO.AppName = "艾蒙中汇教育";
                    AppVO.AppId = "wx76559f644d7a1c77";
                    AppVO.Secret = "7c8752059553c81fef0c7081939d95a1";
                    //商户平台配置
                    AppVO.SSLCERT_PATH = "C:/web/ca/1494588702_20240312_cert/apiclient_cert.p12";
                    AppVO.SSLCERT_PASSWORD = "1494588702";
                    AppVO.AppType = 5;
                    AppVO.TPersonalID = 6;
                    break;
                case 30:
                    AppVO.AppName = "粤省情";
                    AppVO.AppId = "wx79943c188a5368a9";
                    AppVO.Secret = "ff88779918706db7fd2deebfc8161058";
                    //商户平台配置
                    AppVO.MCHID = "1623715013";
                    AppVO.KEY = "2weryYTG7698hjkwyr281AMKDUEasdSC";
                    AppVO.APPSECRET = "822bc05f9d5c51eaae3b14e2bc3cebbd";
                    AppVO.SSLCERT_PATH = "C:/cert/ca/ysq/1623715013_20250916_cert/apiclient_cert.p12";
                    AppVO.SSLCERT_PASSWORD = "1623715013";
                    AppVO.MCH_CERT_SERIAL_NO = "1D65F1C596C2DA416932DC336F1F3C6663CECB3B";
                    AppVO.API_V3_KEY = "Bd58dd54we8d5f48e7sdv4Md58798845";
                    AppVO.AppType = 30;
                    AppVO.TPersonalID = 18093;
                    break;
                case 31:
                    AppVO.AppName = "云米粒(禅宗)";
                    AppVO.AppId = "wxdc4628baa10f71ce";
                    AppVO.Secret = "65cadfc9f66ecb849a4345ff65bece20";
                    //商户平台配置
                    AppVO.MCHID = "1723564517";
                    AppVO.KEY = "bnedfcv2dfgCVGY68943HJREdeg7unjk";
                    AppVO.APPSECRET = "4d60dea1857d13c37fe7c8d11731a785";
                    AppVO.SSLCERT_PATH = "C:/cert/ca/yml/1723564517_20250728_cert/apiclient_cert.p12";
                    AppVO.SSLCERT_PASSWORD = "1723564517";
                    AppVO.AppType = 31;
                    AppVO.TPersonalID = 18063;
                    break;
                default:
                    AppVO = null;
                    break;
            }
            if (AppVO == null)
            {
                MiniprogramsBO mBO = new MiniprogramsBO();
                wxMiniprogramsVO mVO = mBO.FindMiniprogramsById(AppType);
                if (mVO != null)
                {
                    AppVO = new AppVO();
                    AppVO.AppName = mVO.AppName;
                    AppVO.AppId = mVO.AppId;
                    AppVO.Secret = mVO.Secret;
                    //商户平台配置
                    AppVO.MCHID = mVO.MCHID;
                    AppVO.KEY = mVO.MCH_KEY;
                    AppVO.APPSECRET = mVO.APPSECRET;
                    AppVO.SSLCERT_PATH = mVO.SSLCERT_PATH;
                    AppVO.SSLCERT_PASSWORD = mVO.SSLCERT_PASSWORD;
                    AppVO.AppType = mVO.AppType;
                    AppVO.TBusinessID = mVO.TBusinessID;

                    if (mVO.TPersonalID > 0)
                    {
                        AppVO.TPersonalID = mVO.TPersonalID;
                    }
                    else
                    {
                        BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                        AppVO.TPersonalID = cBO.FindJurisdiction(AppVO.TBusinessID,0);
                    }
                }
            }
            return AppVO;
        }
    }
    public class MiniprogramsBO
    {
        private CustomerProfile CurrentCustomerProfile = new CustomerProfile();
        public MiniprogramsBO()
        {
            
        }
        public MiniprogramsBO(CustomerProfile customerProfile)
        {
            this.CurrentCustomerProfile = customerProfile;
        }
        /// <summary>
        /// 添加小程序
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public int AddMiniprograms(wxMiniprogramsVO vo)
        {
            try
            {
                IwxMiniprogramsDAO uDAO = CustomerManagementDAOFactory.wxMiniprogramsDAO(this.CurrentCustomerProfile);

                CommonTranscation t = new CommonTranscation();
                t.TranscationContextWithReturn += delegate ()
                {
                    int AppType = uDAO.Insert(vo);
                    return AppType;
                };
                int result = t.Go();
                return Convert.ToInt32(t.TranscationReturnValue);
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(MiniprogramsBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return -1;
            }
        }
        /// <summary>
        /// 更新小程序
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public bool UpdateMiniprograms(wxMiniprogramsVO vo)
        {
            IwxMiniprogramsDAO uDAO = CustomerManagementDAOFactory.wxMiniprogramsDAO(this.CurrentCustomerProfile);
            try
            {
                uDAO.UpdateById(vo);
                return true;
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(MiniprogramsBO));
                string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                _log.Error(strErrorMsg);
                return false;
            }
        }

        /// <summary>
        /// 获取小程序列表
        /// </summary>
        /// <param name="conditionStr"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sortcolname"></param>
        /// <param name="asc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<wxMiniprogramsVO> FindMiniprogramsAllByPageIndex(string conditionStr, int start, int end, string sortcolname, string asc, params object[] parameters)
        {
            IwxMiniprogramsDAO uDAO = CustomerManagementDAOFactory.wxMiniprogramsDAO(this.CurrentCustomerProfile);
            return uDAO.FindAllByPageIndex(conditionStr, start, end, sortcolname, asc, parameters);
        }

        /// <summary>
        /// 获取小程序列表
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<wxMiniprogramsVO> FindMiniprogramsByConditionStr(string condition)
        {
            IwxMiniprogramsDAO uDAO = CustomerManagementDAOFactory.wxMiniprogramsDAO(this.CurrentCustomerProfile);
            List<wxMiniprogramsVO> cVO = uDAO.FindByParams(condition);
            return cVO;
        }

        /// <summary>
        /// 获取小程序数量
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int FindMiniprogramsTotalCount(string condition, params object[] parameters)
        {
            IwxMiniprogramsDAO uDAO = CustomerManagementDAOFactory.wxMiniprogramsDAO(this.CurrentCustomerProfile);
            return uDAO.FindTotalCount(condition, parameters);
        }

        /// <summary>
        /// 获取小程序详情
        /// </summary>
        /// <param name="NoticeID"></param>
        /// <returns></returns>
        public wxMiniprogramsVO FindMiniprogramsById(int AppType)
        {
            IwxMiniprogramsDAO uDAO = CustomerManagementDAOFactory.wxMiniprogramsDAO(this.CurrentCustomerProfile);
            return uDAO.FindById(AppType);
        }
    }
    public partial class AppVO
    {
        /// <summary>
        /// 小程序名称
        /// </summary>
        public String AppName { get; set; }
        public String AppId { get; set; }
        public String Secret { get; set; }
        /// <summary>
        /// 小程序数据编号
        /// </summary>
        public Int32 AppType { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public String MCHID { get; set; }
        /// <summary>
        /// 商户Key
        /// </summary>
        public String KEY { get; set; }
        /// <summary>
        /// APPSECRET
        /// </summary>
        public String APPSECRET { get; set; }
        /// <summary>
        /// 证书位置
        /// </summary>
        public String SSLCERT_PATH { get; set; }
        /// <summary>
        /// 证书密码
        /// </summary>
        public String SSLCERT_PASSWORD { get; set; }

        /// <summary>
        /// 默认公司id
        /// </summary>
        public int TBusinessID { get; set; }

        /// <summary>
        /// 默认名片id
        /// </summary>
        public int TPersonalID { get; set; }


        /// <summary>
        /// 商户证书序列号
        /// </summary>
        public string MCH_CERT_SERIAL_NO { get; set; }
        /// <summary>
        /// 商户API证书私钥内容（PEM格式）
        /// </summary>
        public string PRIVATE_KEY { get; set; }
        /// <summary>
        /// APIv3密钥
        /// </summary>
        public string API_V3_KEY { get; set; }
    }
}
