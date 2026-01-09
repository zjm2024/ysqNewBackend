using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenAI_Code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAPIController : ControllerBase
    {
        // GET: api/<OpenAPIController>
        [HttpGet]
        public ResultObject Get()
        {
            return new ResultObject() { Flag = 0, Message = "请提交参数!"};
        }
        // GET api/<ValuesController>/5
        [HttpGet("{Prompt}")]
        async public Task<ResultObject> Get(string Prompt)
        {
            var openAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = "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"
            });

            var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("You are a smart AI assistant. Answer questions in Chinese by default."),
                ChatMessage.FromUser(Prompt),
                /*
                ChatMessage.FromAssistance("The Los Angeles Dodgers won the World Series in 2020."),
                ChatMessage.FromUser("Where was it played?")
                */
            },
                Model = Models.ChatGpt3_5Turbo,
                MaxTokens = 2048//optional
            });
            if (completionResult.Successful)
            {
                Console.WriteLine(completionResult.Choices.First().Message.Content);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = completionResult.Choices.First().Message.Content };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = completionResult };
            }
        }
    }

    public class ResultObject
    {
        [DataMember]
        public int Flag { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public object Result { get; set; }
        [DataMember]
        public object Count { get; set; }
        [DataMember]
        public object Subsidiary { get; set; }
    }
}
