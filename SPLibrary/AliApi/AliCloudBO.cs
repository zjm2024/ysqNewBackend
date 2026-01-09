using AlibabaCloud.SDK.CCC20200701.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Tea;
using Tea.Utils;

namespace AlibabaCloud.SDK.Sample
{
    public class AliCloudBO
    {

        /**
         * 使用AK&SK初始化账号Client
         * @param accessKeyId
         * @param accessKeySecret
         * @return Client
         * @throws Exception
         */
        public static string accessKeyId = "LTAI4GFVPQ8TmBSqGDD4zV3N";
        public static string accessKeySecret = "6Z3Vuf5KczJtbfXkgcQ5k4Ah6m5dO9";
        public static AlibabaCloud.SDK.CCC20200701.Client CreateClient()
        {

            AlibabaCloud.OpenApiClient.Models.Config config = new AlibabaCloud.OpenApiClient.Models.Config
            {
                // 您的AccessKey ID
                AccessKeyId = accessKeyId,
                // 您的AccessKey Secret
                AccessKeySecret = accessKeySecret,
            };
            // 访问的域名
            config.Endpoint = "ccc.cn-shanghai.aliyuncs.com";

            return new AlibabaCloud.SDK.CCC20200701.Client(config);
        }

        /// <summary>
        /// 一键双呼
        /// </summary>
        /// <param name="InstanceId">呼叫中心实例ID</param>
        /// <param name="Caller">主叫号码</param>
        /// <param name="Callee">被叫号码</param>
        /// <param name="Broker">背靠背代理所用的中间号码。需要是呼叫中心的可外呼号码</param>
        /// <param name="AdditionalBroker">附加的中间号码，如果该参数存在，则呼叫callee时使用该参数指定的号码</param>
        /// <returns></returns>
        public static StartBack2BackCallResponse StartBack2BackCall(string InstanceId, string Caller, string Callee, string Broker)
        {
            CCC20200701.Client client = CreateClient();
            StartBack2BackCallRequest startBack2BackCallRequest = new StartBack2BackCallRequest
            {
                InstanceId = InstanceId,
                Caller = Caller,
                Callee = Callee,
                Broker = Broker
            };
            return client.StartBack2BackCall(startBack2BackCallRequest);
        }

        /// <summary>
        /// 根据通话ID获取通话的详细操作日志
        /// </summary>
        /// <param name="InstanceId">实例ID</param>
        /// <param name="ContactId">通话ID</param>
        public static GetCallDetailRecordResponse GetCallDetailRecord(string InstanceId,string ContactId)
        {
            CCC20200701.Client client = CreateClient();
            GetCallDetailRecordRequest getCallDetailRecordRequest = new GetCallDetailRecordRequest
            {
                InstanceId = InstanceId,
                ContactId = ContactId,
            };
            return client.GetCallDetailRecord(getCallDetailRecordRequest);
        }

        /// <summary>
        /// 查询所有呼叫中心
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static ListInstancesResponse ListInstances(int PageNumber,int PageSize)
        {
            CCC20200701.Client client = CreateClient();
            ListInstancesRequest listInstancesRequest = new ListInstancesRequest
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
            };
            // 复制代码运行请自行打印 API 的返回值
            return client.ListInstances(listInstancesRequest);
        }

        /// <summary>
        /// 通话记录列表
        /// </summary>
        /// <param name="InstanceId">呼叫中心ID</param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static ListCallDetailRecordsResponse ListCallDetailRecords(string InstanceId, int PageNumber, int PageSize)
        {
            CCC20200701.Client client = CreateClient();
            ListCallDetailRecordsRequest listCallDetailRecordsRequest = new ListCallDetailRecordsRequest
            {
                InstanceId = InstanceId,
                PageNumber = PageNumber,
                PageSize = PageSize,
            };
            // 复制代码运行请自行打印 API 的返回值
           return client.ListCallDetailRecords(listCallDetailRecordsRequest);
        }
    }
}
