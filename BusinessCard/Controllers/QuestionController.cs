using BusinessCard.Models;
using CoreFramework.VO;
using Google.Protobuf.WellKnownTypes;
using Jayrock.Json;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using SPlatformService;
using SPlatformService.Controllers;
using SPlatformService.Models;
using SPlatformService.TokenMange;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.DAO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;


namespace BusinessCard.Controllers
{
    [RoutePrefix("SPWebAPI/Question")]
    [TokenProjector]
    public class QuestionController : ApiController
    {

        /// <summary>
        /// 新建问卷回调接口
        /// </summary>
        /// <param name="ActivityId"></param>
        /// <param name="ActivityName"></param>
        /// <param name="ActivityDomain"></param>
        /// <param name="ActivityPCUrl"></param>
        /// <param name="ActivityH5Url"></param>
        /// <param name="wjxparams"></param>
        /// <returns></returns>
        [Route("CreateOrUpdateQuestionnaire"), HttpPost, Anonymous]
        public ResultObject CreateOrUpdateQuestionnaire([FromBody] QuestionnaireFromVO questionnaireFromVO,
            string ActivityId,
            string ActivityName,
            string ActivityDomain,
            string ActivityPCUrl,
            string ActivityH5Url,
            string wjxparams = ""
            )
        {
            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                cBO.printQuestionnaire("加密的数据：" + questionnaireFromVO.Content);
                string aesKey = "defdaac404ac4ab59dd4729114e1d19e"; // 32字符的AES密钥（UTF-8编码后为32字节）
                byte[] encryptedData = Convert.FromBase64String(questionnaireFromVO.Content);

                string decryptedData = cBO.Decrypt(encryptedData, aesKey);
                cBO.printQuestionnaire("解密后的数据：" + decryptedData);
                SurveyResponse survey = JsonConvert.DeserializeObject<SurveyResponse>(decryptedData);
                //查询是否已创建该问卷
                QuestionnaireDataVO existingQuestionnaire = cBO.FindQuestionByActivityIdId(ActivityId);
                if (existingQuestionnaire != null)
                {
                    // 更新问卷
                    existingQuestionnaire.activity_id = ActivityId;
                    existingQuestionnaire.activity_name = ActivityName;
                    existingQuestionnaire.activity_domain = ActivityDomain;
                    existingQuestionnaire.activity_pc_url = ActivityPCUrl;
                    existingQuestionnaire.activity_h5_url = ActivityH5Url;
                    existingQuestionnaire.content = decryptedData;
                    existingQuestionnaire.custom_params = survey.Creater ?? "";
                    existingQuestionnaire.activity_description = survey.Description ?? "";
                    existingQuestionnaire.update_time = DateTime.Now;
                    existingQuestionnaire.txt1 = "粤省情数字";
                    existingQuestionnaire.sort = 10;
                    cBO.UpdateQuestionnaire(existingQuestionnaire);
                }
                else
                {
                    existingQuestionnaire = new QuestionnaireDataVO
                    {
                        activity_id = ActivityId,
                        activity_name = ActivityName,
                        activity_domain = ActivityDomain,
                        activity_pc_url = ActivityPCUrl,
                        activity_h5_url = ActivityH5Url,
                        content = decryptedData,
                        custom_params = survey.Creater ?? "",
                        activity_description = survey.Description ?? "",
                        create_time = DateTime.Now,
                        update_time = DateTime.Now,
                        sort = 10,
                        txt1 = "粤省情数字"
                    };
                    // 创建新问卷
                    cBO.CreateQuestionnaire(existingQuestionnaire);
                }

                return new ResultObject() { Flag = 0, Message = "创建成功！", Result = "" };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "创建失败，参数异常：" + ex.Message, Result = ex.ToString() };
            }
        }

        /// <summary>
        /// 提交答卷回调接口
        /// </summary>
        /// <param name="aes"></param>
        /// <returns></returns>
        [Route("CreateAnswerSheet"), HttpPost, Anonymous]
        public ResultObject CreateAnswerSheet()
        {
            try
            {   // 1. 获取请求输入流
                Stream stream = Request.Content.ReadAsStreamAsync().Result;
                // 重置流位置（防止读取异常）
                stream.Position = 0;

                // 2. 读取流数据到字节数组
                byte[] byteData = new byte[stream.Length];
                int bytesRead = stream.Read(byteData, 0, (int)stream.Length);

                // 3. 转换字节数组为UTF8字符串（JSON）
                string jsonData = Encoding.UTF8.GetString(byteData, 0, bytesRead);

                // 4. 这里可以添加JSON解析逻辑（例如使用Newtonsoft.Json）
                // 示例：解析为动态对象
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonData);
                // 移除指定字段
               
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                string modifiedJson = jsonData;
                QuestionnaireDataVO qvo = cBO.FindQuestionByActivityIdId(data.activity.ToString());
                //if (qvo != null && qvo.need_obtain_ip == 0) {
                //    modifiedJson = cBO.RemoveField(data, "ipAddress");
                //}
                
                cBO.printQuestionnaire("答卷的数据：" + data);

                AnswerSheetVO answerSheet = new AnswerSheetVO
                {
                    activity_id = data.activity,
                    content = modifiedJson,
                    PersonalID = data.sojumpparm,
                    createdAt = DateTime.Now
                };
                //// 创建新问卷
                cBO.CreateAnswerSheet(answerSheet);
                return new ResultObject() { Flag = 0, Message = "创建成功！", Result = "" };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "创建失败，参数异常：" + ex.Message, Result = "" };
            }
        }

        /// <summary>
        /// 获取问卷详情
        /// </summary>
        /// <param name="SortID"></param>
        /// <returns></returns>
        [Route("GetQuestionnaireiDetail"), HttpGet]
        public ResultObject GetQuestionnaireiDetail(int QuestionId, string token)
        {
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            QuestionnaireDataVO sVO = cBO.FindQuestionById(QuestionId);

            if (sVO != null)
            {
                //获取签名
                string wjxSign = BusinessCardBO.GenerateWjxSign(sVO.activity_id);
                var data = new { wjxSign, sVO };
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = data };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = null };
            }
        }


        /// <summary>
        /// 生成问卷签名
        /// </summary>
        /// <param name="activity_id"></param>
        /// <returns></returns>
        [Route("GenerateWjxSign"), HttpGet, Anonymous]
        public ResultObject GenerateWjxSign(string activity_id)
        {
            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                //获取签名
                string wjxSign = BusinessCardBO.GenerateWjxSign(activity_id);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = wjxSign };
            }
            catch (Exception)
            {
                return new ResultObject() { Flag = -1, Message = "获取失败!", Result = null };
            }

        }


        /// <summary>
        /// 查询问卷回调接口
        /// </summary>
        /// <param name="ActivityId"></param>

        /// <returns></returns>
        [Route("GetQuestionnaire"), HttpPost, Anonymous]
        public ResultObject GetQuestionnaire(string ActivityId)
        {
            try
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                QuestionnaireDataVO Questionnaire = cBO.FindQuestionByActivityIdId(ActivityId);
                SurveyResponse survey = new SurveyResponse();
                if (Questionnaire != null)
                {
                    // 反序列化为实体对象
                    survey = JsonConvert.DeserializeObject<SurveyResponse>(Questionnaire.content);
                }
                return new ResultObject() { Flag = 0, Message = "查询成功！", Result = survey };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "查询失败，参数异常：" + ex.Message, Result = "" };
            }
        }

        /// <summary>
        /// 更新问卷
        /// </summary>
        /// <param name="vO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("UpdateQuestionnair"), HttpPost]
        public ResultObject UpdateQuestionnair([FromBody] QuestionnaireDataVO vO, string token)
        {
            try
            {
                if (vO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;

                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

                var qVO = cBO.UpdateQuestionnaire(vO);

                if (qVO)
                {
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = qVO };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }
            }
            catch (Exception ex)
            {

                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = ex };
            }

        }

        /// <summary>
        /// 获取我的问卷列表
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("getMyQuestionnairelist"), HttpPost]
        public ResultObject getMyQuestionnairelist([FromBody] ConditionModel condition,string token)
        {
         
            try
            {
                if (condition == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }
                else if (condition.Filter == null || condition.PageInfo == null)
                {
                    return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
                }

                UserProfile uProfile = CacheManager.GetUserProfile(token);
                CustomerProfile cProfile = uProfile as CustomerProfile;
                int customerId = cProfile.CustomerId;
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);
                if (pVO == null)
                {
                    return new ResultObject() { Flag = 0, Message = "未找到个人信息!", Result = null };
                }
        
                Paging pageInfo = condition.PageInfo;
                string conditionStr = " PersonalID =" + pVO.PersonalID;
                string conditionStr2 = "1=1";
                List <AnswerSheetVO> aVO = cBO.GetAnswerSheetList(conditionStr, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
                // 提取有效且非空的activity_id（去重）
                var activityIds = aVO
                    .Where(vo => vo != null && !string.IsNullOrEmpty(vo.activity_id?.ToString()))
                    .Select(vo => vo.activity_id.ToString())
                    .Distinct()
                    .ToList();

                // 拼接IN条件（处理空集合情况，避免生成"in ()"无效语法）
                if (activityIds.Any())
                {
                    // 对ID进行SQL转义，防止SQL注入（根据实际ID类型调整）
                    var escapedIds = activityIds.Select(id => $"'{id.Replace("'", "''")}'");
                    conditionStr2 += $" AND activity_id IN ({string.Join(",", escapedIds)})";
                }
                else
                {
                    // 若没有有效ID，可根据业务需求处理（如添加恒假条件）
                    conditionStr2 += " AND 1=0"; // 确保不会查询到数据
                }
                List<QuestionnaireDataVO> qVO = cBO.GetQuestionnaireList(conditionStr2, (pageInfo.PageIndex - 1) * pageInfo.PageCount + 1, pageInfo.PageIndex * pageInfo.PageCount, pageInfo.SortName, pageInfo.SortType);
                var count = cBO.FindQuestionnaireDataCount(conditionStr2);
                if (qVO.Count > 0)
                {
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = qVO, Count = count };
                }
                return new ResultObject() { Flag = 1, Message = "未查询到数据!", Result = null };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = -1, Message = "接口异常!", Result = ex };
            }
          
        }


        /// <summary>
        /// 上传录音文件
        /// </summary>
        /// <param name="QuestionnaireID">问卷ID</param>
        /// <param name="token">用户令牌</param>
        /// <returns></returns>
        [Route("UploadRecording"), HttpPost]
        public ResultObject UploadRecording(string ActivityId,string id_timestamp, string token)
        {
            // 验证用户身份
            UserProfile uProfile = CacheManager.GetUserProfile(token);
            CustomerProfile cProfile = uProfile as CustomerProfile;
            int customerId = cProfile.CustomerId;

            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            PersonalVO pVO = cBO.FindPersonalByCustomerId(customerId);

            // 验证问卷是否存在
            QuestionnaireDataVO QVO = cBO.FindQuestionByActivityIdId(ActivityId);

            if (QVO == null)
            {
                return new ResultObject() { Flag = 0, Message = "问卷不存在!", Result = null };
            }

            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string folder = "/UploadFolder/Recording/" + DateTime.Now.ToString("yyyyMM") + "/";
            string filePath = "";

            if (hfc.Count > 0)
            {
                try
                {
                    FileInfo fi = new FileInfo(hfc[0].FileName);

                    string ext = fi.Extension.ToLower();
                    // 验证文件类型
                    if (ext != ".aac" && ext != ".mp3" && ext != ".wav" && ext != ".m4a")
                    {
                        return new ResultObject() { Flag = 0, Message = "只支持 aac, mp3, wav, m4a 格式的音频文件", Result = null };
                    }

                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + pVO.PersonalID.ToString() + ActivityId;

                    // 本地路径
                    string localPath = ConfigInfo.Instance.UploadFolder + folder;
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string PhysicalPath = localPath + newFileName;
                    filePath = "~" + folder + newFileName;
                    hfc[0].SaveAs(PhysicalPath);

                    // 网络路径
                    string url = ConfigInfo.Instance.BCAPIURL + folder + newFileName;
                    // 获取表单数据
                    var form = System.Web.HttpContext.Current.Request.Form;
                    int duration = 0;
                    int.TryParse(form["duration"], out duration);
                    string fileName = form["fileName"] ?? "recording" + fi.Extension;
                    string recordingConfig = form["config"] ?? "";



                    // 创建录音记录
                    RecordingRecordsVO rvo = new RecordingRecordsVO()
                    {
                        file_name = newFileName,
                        original_file_name = fileName,
                        file_path = url,
                        file_size = hfc[0].ContentLength,
                        duration = duration,
                        activityid = Convert.ToInt32(ActivityId),
                        personalid = pVO.PersonalID,
                        recording_config = recordingConfig,
                        id_timestamp = id_timestamp,
                        create_time = DateTime.Now,
                        modify_time = DateTime.Now,
                        status = "Active"
                    };
                    // 保存到数据库
                    cBO.CreateRecording(rvo);

                    return new ResultObject() { Flag = 1, Message = "上传成功", Result = url, Subsidiary = rvo };
                }
                catch (Exception er)
                {
                    return new ResultObject() { Flag = 0, Message = "上传失败", Result = er.Message, Subsidiary = "" };
                }
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "上传失败", Result = "文件为空", Subsidiary = "" };
            }
        }



        /// <summary>
        /// 更新问卷
        /// </summary>
        /// <param name="vO">VO</param>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("gettest"), HttpGet , Anonymous]
        public ResultObject gettest()
        {
            try
            {
             
                    return new ResultObject() { Flag = 1, Message = "获取成功!", Result = null };
            }
            catch (Exception ex)
            {

                return new ResultObject() { Flag = 0, Message = "更新失败!", Result = ex };
            }
        }



        public class QuestionnaireFromVO
        {
            //1） ActivityId 问卷编号（Url内携带）
            //2） ActivityName 问卷标题（Url内携带）
            //3） ActivityDomain 问卷访问域名（Url内携带）
            //4） ActivityPCUrl 问卷访问PC端链接（Url内携带）
            //5） ActivityH5Url 问卷访问移动端链接（Url内携带）
            //6） Content 问卷内容（AES加密）（消息体内携带）
            //7） wjxparams*** 自定义参数（Url内携带）
            public string ActivityId { get; set; }
            public string ActivityName { get; set; }
            public string ActivityDomain { get; set; }
            public string ActivityPCUrl { get; set; }
            public string ActivityH5Url { get; set; }
            public string Content { get; set; }
            public string Questions { get; set; }
            public string wjxparams { get; set; } // 用于标识问卷的唯一ID

        }

    }
}
