using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CoreFramework.VO;
using OpenAI_API.Completions;
using OpenAI_API.Models;

namespace OpenAIAPP.Controllers
{
    [RoutePrefix("SPWebAPI/OpenAI")]
    public class OpenAIController : ApiController
    {
        /// <summary>
        /// 获取我的名片
        /// </summary>
        /// <param name="token">口令</param>
        /// <returns></returns>
        [Route("GetText"), HttpGet]
        public async Task<ResultObject> GetText(string Prompt,double temperature=0.3,int max_tokens= 2048,string apikey= "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")
        {
            try
            {
                var api = new OpenAI_API.OpenAIAPI(apikey);
                var result = await api.Completions.CreateCompletionAsync(new CompletionRequest(Prompt, model: Model.DefaultModel, temperature: temperature, max_tokens: max_tokens));
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = result.ToString()};
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex };
            }
        }
    }
}
