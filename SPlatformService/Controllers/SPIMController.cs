
using CoreFramework.VO;
using Newtonsoft.Json;
using SPlatformService.Common;
using SPlatformService.Models;
using SPlatformService.TokenMange;
using SPLibrary.CoreFramework;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.DAO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;

namespace SPlatformService.Controllers
{
    [RoutePrefix("SPWebAPI/SPIM")]
    [TokenProjector]
    public class SPIMController : ApiController
    {
        /// <summary>
        /// 获取用户基础信息，我的信息、好友列表、群组列表
        /// </summary>
        /// <param name="customerId">会员ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetIMUserInit"), HttpGet]
        public IMResultObject GetIMUserInit(int customerId, string token)
        {
            ICustomerIMDAO cIMDAO = CustomerManagementDAOFactory.CreateCustomerIMDAO(new UserProfile());

            IMInitModel initModel = new IMInitModel();

            //获取会员 IM信息
            List<CustomerIMVO> cIMVOList = cIMDAO.FindByParams("CustomerId = " + customerId);
            if(cIMVOList.Count > 0)
            {
                IMUser imUser = new IMUser();
                imUser.id = cIMVOList[0].IMId;
                imUser.avatar = cIMVOList[0].HeaderLogo;
                imUser.sign = cIMVOList[0].Sign;
                imUser.status = (cIMVOList[0].Status == 1) ? "online" : "hide";
                imUser.username = cIMVOList[0].NickName;

                initModel.mine = imUser;
            }

            //获取好友信息            

            IMBO imBO = new IMBO(new CustomerProfile());
            List<RosterViewVO> friendVOList = imBO.GetRoster(customerId);

            List<IMGroupUser> groupUserList = new List<IMGroupUser>();

            IMGroupUser groupUserVO = new IMGroupUser() { groupname = "我的好友", id = 1, online = friendVOList.Count };
            
            List<IMUser> guUserList = new List<IMUser>();
            foreach (RosterViewVO friendVO in friendVOList)
            {
                IMUser imUser = new IMUser();
                imUser.id = friendVO.IMId;
                imUser.avatar = friendVO.HeaderLogo;
                imUser.sign = friendVO.Sign;
                imUser.status = (friendVO.Status == 1) ? "online" : "hide";
                imUser.username = friendVO.NickName;

                guUserList.Add(imUser);
            }
            groupUserVO.list = guUserList;

            groupUserList.Add(groupUserVO);

            initModel.friend = groupUserList;


            return new IMResultObject() { code = 0, msg = "", data = initModel };

        }
        
        /// <summary>
        /// 获取IM信息
        /// </summary>
        /// <param name="customerId">会员ID</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetIMuser"), HttpGet]
        public ResultObject GetIMuser(int customerId,string token)
        {
            ICustomerIMDAO cIMDAO = CustomerManagementDAOFactory.CreateCustomerIMDAO(new UserProfile());
            List<CustomerIMVO> voList = cIMDAO.FindByParams("CustomerId = " + customerId);

            if (voList.Count > 0)
                return new ResultObject() { Flag = 1, Message = "", Result = voList[0] };
            else
                return new ResultObject() { Flag = 0, Message = "获取失败", Result = null };

        }

        /// <summary>
        /// 根据IM账号获取IM信息
        /// </summary>
        /// <param name="IMId">IM账号</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetIMuserByIMId"), HttpGet]
        public ResultObject GetIMuserByIMId(string IMId, string token)
        {
            ICustomerIMDAO cIMDAO = CustomerManagementDAOFactory.CreateCustomerIMDAO(new UserProfile());
            List<CustomerIMVO> voList = cIMDAO.FindByParams("IMId = '" + IMId + "'");

            if (voList.Count > 0)
            {
                CustomerIMVO imVO = voList[0];
                IMUser imUser = new IMUser();
                imUser.id = imVO.IMId;
                imUser.avatar = imVO.HeaderLogo;
                imUser.sign = imVO.Sign;
                imUser.status = (imVO.Status == 1) ? "online" : "hide";
                imUser.username = imVO.NickName;
                imUser.groupid = 1;
                return new ResultObject() { Flag = 1, Message = "", Result = imUser };
            }
            else
                return new ResultObject() { Flag = 0, Message = "获取失败", Result = null };

        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="customerId">会员ID</param>
        /// <param name="status">状态，1 在线，0 隐身</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateIMUserStatus"), HttpGet]
        public ResultObject UpdateIMUserStatus(int customerId,int status, string token)
        {
            ICustomerIMDAO cIMDAO = CustomerManagementDAOFactory.CreateCustomerIMDAO(new UserProfile());
            CustomerIMVO vo = new CustomerIMVO();
            vo.CustomerId = customerId;
            vo.Status = status;

            cIMDAO.UpdateByParams(vo, "CustomerId = " + customerId);

            return new ResultObject() { Flag = 1, Message = "", Result = null };

        }

        /// <summary>
        /// 更新签名
        /// </summary>
        /// <param name="customerId">会员ID</param>
        /// <param name="sign">签名</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateIMUserSign"), HttpGet]
        public ResultObject UpdateIMUserSign(int customerId,string sign, string token)
        {
            ICustomerIMDAO cIMDAO = CustomerManagementDAOFactory.CreateCustomerIMDAO(new UserProfile());
            CustomerIMVO vo = new CustomerIMVO();
            vo.CustomerId = customerId;
            vo.Sign = sign;

            cIMDAO.UpdateByParams(vo, "CustomerId = " + customerId);

            return new ResultObject() { Flag = 1, Message = "", Result = null };

        }
                

        /// <summary>
        /// 保存消息，发送成功时保存，接收不需要保存。
        /// </summary>
        /// <param name="messageFrom">发送者</param>
        /// <param name="messageTo">接收者</param>
        /// <param name="content">消息内容</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateIMMessage"), HttpPost]
        public ResultObject UpdateIMMessage(string messageFrom,string messageTo,string content, string token)
        {
            IMBO imBO = new IMBO(new CustomerProfile());
            
            IMMessageVO imMessageVO = new IMMessageVO();
            imMessageVO.MessageFrom = imBO.GetIMCustomerByIMUser(messageFrom).CustomerIMId;
            imMessageVO.MessageTo = imBO.GetIMCustomerByIMUser(messageTo).CustomerIMId;
            imMessageVO.IMTargetType = "friend";
            imMessageVO.IMMessageType = "txt";
            imMessageVO.Message = content;
            imMessageVO.SendAt = DateTime.Now;

            int result = imBO.AddMessage(imMessageVO);
            return new ResultObject() { Flag = 1, Message = "", Result = result };

        }

        /// <summary>
        /// 获取消息记录，一次返回100条，按时间排序
        /// </summary>
        /// <param name="fromId">发送ID</param>
        /// <param name="toId">接收ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetIMMessageHistory"), HttpGet]
        public IMResultObject GetIMMessageHistory(string fromId, string toId, int pageIndex, string token)
        {
            IMBO imBO = new IMBO(new CustomerProfile());
            List<IMMessageHistoryVO> voList = imBO.GetIMMessage(imBO.GetIMCustomerByIMUser(fromId).CustomerIMId, imBO.GetIMCustomerByIMUser(toId).CustomerIMId, 1);
            return new IMResultObject() { code = 0, msg = "", data = voList };

        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UploadImage"), HttpPost]
        public IMResultObject UploadImage(string token)
        {
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            string folder = "/UploadFolder/Image/" + DateTime.Now.ToString("yyyyMM") + "/";
            if (hfc.Count > 0)
            {
                FileInfo fi = new FileInfo(hfc[0].FileName);

                string ext = fi.Extension.ToLower();
                if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
                {
                    return new IMResultObject() { code = 1, msg = "文件类型错误", data = null };
                }

                string newFileName = DateTime.Now.ToString("yyyyMMddhhssmm") + fi.Extension;
                
                //可以修改为网络路径
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string PhysicalPath = localPath + newFileName;
                hfc[0].SaveAs(PhysicalPath);

                IMResultObject result = new IMResultObject() { code = 0, msg = "上传成功", data = JsonConvert.DeserializeObject("{\"src\": \"" + ConfigInfo.Instance.APIURL + folder + newFileName + "\"}") };
                return result;
            }
            else
            {
                return new IMResultObject() { code = 1, msg = "上传失败,没有文件", data = null };
            }
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UploadFile"), HttpPost]
        public IMResultObject UploadFile(string token)
        {

            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            string folder = "/UploadFolder/Attached/" + DateTime.Now.ToString("yyyyMM") + "/"; ;

            if (hfc.Count > 0)
            {
                FileInfo fi = new FileInfo(hfc[0].FileName);

                string ext = fi.Extension.ToLower();
                if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
                {
                    return new IMResultObject() { code = 1, msg = "文件类型错误", data = null };
                }

                string newFileName = DateTime.Now.ToString("yyyyMMddhhssmm") + fi.Extension;
                
                //可以修改为网络路径
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string PhysicalPath = localPath + newFileName;
                hfc[0].SaveAs(PhysicalPath);
                
                IMResultObject result = new IMResultObject() { code = 0, msg = "上传成功", data = JsonConvert.DeserializeObject("{\"src\": \"" + ConfigInfo.Instance.APIURL + folder + newFileName + "\",\"name\": \""+ hfc[0].FileName + "\"}") };
                return result;
            }
            else
            {
                return new IMResultObject() { code = 1, msg = "上传失败,没有文件", data = null };
            }

        }

        /// <summary>
        /// 上传音频   PC不支持音频
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UploadAudio"), HttpPost]
        public IMResultObject UploadAudio(string token)
        {
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            string folder = "/UploadFolder/Audio/" + DateTime.Now.ToString("yyyyMM") + "/";

            if (hfc.Count > 0)
            {
                FileInfo fi = new FileInfo(hfc[0].FileName);

                string ext = fi.Extension.ToLower();
                if (ext.IndexOf("asp") > -1 || ext.IndexOf("aspx") > -1 || ext.IndexOf("php") > -1)
                {
                    return new IMResultObject() { code = 1, msg = "文件类型错误", data = null };
                }

                string newFileName = DateTime.Now.ToString("yyyyMMddhhssmm") + fi.Extension;
                
                //可以修改为网络路径
                string localPath = ConfigInfo.Instance.UploadFolder + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string PhysicalPath = localPath + newFileName;
                hfc[0].SaveAs(PhysicalPath);
                
                IMResultObject result = new IMResultObject() { code = 0, msg = "上传成功", data = JsonConvert.DeserializeObject("{\"src\": \"" + ConfigInfo.Instance.APIURL + folder + newFileName + "\",\"name\": \"" + hfc[0].FileName + "\"}") };
                return result;
            }
            else
            {
                return new IMResultObject() { code = 1, msg = "上传失败,没有文件", data = null };
            }

        }

        /// <summary>
        /// 添加IM 好友。
        /// </summary>
        /// <param name="friendCustomerId">好友Id</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("AddIMFriend"), HttpPost]
        public ResultObject AddIMFriend(int friendCustomerId,string token)
        {
            CustomerProfile cProfile = CacheManager.GetUserProfile(token) as CustomerProfile;
            IMBO imBO = new IMBO(new CustomerProfile());           

            bool result = imBO.AddFriend(cProfile.CustomerId, friendCustomerId);
            if (result)
            {
                CustomerIMVO imVO = imBO.GetIMCustomerByCustomer(friendCustomerId);
                IMUser imUser = new IMUser();
                imUser.id = imVO.IMId;
                imUser.avatar = imVO.HeaderLogo;
                imUser.sign = imVO.Sign;
                imUser.status = (imVO.Status == 1) ? "online" : "hide";
                imUser.username = imVO.NickName;
                imUser.groupid = 1;
                return new ResultObject() { Flag = 1, Message = "添加成功", Result = imUser };
            }
            else
                return new ResultObject() { Flag = 0, Message = "添加失败", Result = null };

        }

    }
}
