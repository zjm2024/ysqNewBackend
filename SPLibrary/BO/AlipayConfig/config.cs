using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// config 的摘要说明
/// </summary>
public class config
{
    public config()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    // 应用ID,您的APPID
    public static string app_id = "2017102609537389";

    // 支付宝网关
    public static string gatewayUrl = "https://openapi.alipay.com/gateway.do";

    // 商户私钥，您的原始格式RSA私钥
    public static string private_key = "MIIEogIBAAKCAQEAxbGtVpnhZYkPvo22Gmx8FKK2OduFQFJPBAcutfO4ipxQXNTFSMnU+AyyK0jX3U1N9opHIvOPz9nWFuBjdaHhlr6JwiMopBCVWSRK1S+u+PcQCt/8TjpxFWkFQceVtYGL9K4sCaCumvtaKVO1oblT7CItj6vq7DxQYqGozY3OM3r4lNxNC1I3OaG5bmNIlEe4ILO5/oG9PZFw7NDU5qVEKtCd+1RrJ80BQwKLWWu2XTBpM9UCdMadW5RiJopU0Jv67ZnRhQew9fBUBe0sp03jAMJX0Rmy0GmGG1Gc0ejjWcIWbb1q6wHKUqDCFCWfN0ukWgG7VWg7SIX2eXCgdKClRwIDAQABAoIBADRrDRv2R0Zl1i/fYVaWRzwtfIXQx/8rE9pskR0bl2XhVomWvQy0v2xZ6XACq4H5Ol/RgFUrIYmqJ9L/AkAxgSA4O0ynh+Igp4jwT4tBTnKjB7TqePrBEjTsGMG9StvKkrJ48CJJYaKkXN6yXGmaOz9UPKJooiyrPWE5ngmKvQ7257mgbXgsh9CRg3k2KK8f6kMtBUSr00QAnArGwokD1jB9E8w/f8WUjJH47dlCv8wOKDX8P9332zmaJmbC9trXCQtYGmq1MUfklAIqhPikanLl8dHKs7b965zMKdukNj84AYJv0c0IRCh8iI6qw/AElP7GQ1HaSTkTUkK2ytkv/VkCgYEA7HmnCAHrOb/fiIO6oWd2CEFZ/yjcX/i8EAGlT5Jshala+YnnO6LbsoQl9HvqB9CufXnnyNNOIbg4FFtYK7HXD07HoR+D5bgrKQxaDmGMqc2ZsB1qUtcCWCXL9msdTscABkXPsKuxil5knYJ0K3obxKUXwQCN7E+GknoBV5g9Aq0CgYEA1gRP52e/LUAbYPKH+o3/f4j40L7ptSI+jO65LiXLl0TvooIkoTDvcvqX0orSNOxiggz6xB/JssJK4DAmVh1uIpwbsOwc+oe2d3fDINGZ32NizpGwvM5Jmm+Q9GGq3Kp5GVPa3e5Bofw1pTT+cq52Q4L0HLbqYU8N/DWiJXW8+kMCgYAFIJojjU8ZYHSrVBfe2gxwH2KhKrSkjBMf0blQ4V7jAwoK8ReHbn+O/liIVYt8geTWqacB7rr59jGZF2XxBJkiJtYjQzir6XNeH3twZmMTZ3bWn7yncSwG10L149qt05jHign3zbE6y9MoqyRzsOEbQW0jbSymnA7R7D1NNerUvQKBgAzDDRC7ewbzv640SC+EsCumu0+XUtBeTmSRB7HcxxsqHTAPmqeSB3VPxCXl6UsGnYkrxPhU/O4P7+8Wb/LKpnqh3HTcX44A8byGA7i9RvOvNYEj5uobUwBGjtpDyLCH6di8y1xYjMlgBMLpOhMUnDDCalQcBDXxROA5QoYBU1eJAoGAQnt5n3JfkCVtYrKa8Y/8GeZcu+c7huGxvQBVzEYoZPPX/4oDiVObCaysCS8n5X/0S/1YXpRTE8D3oEBR/6DifY6ko+ZVl5owGvCZZqTR2Sa78Dzi06l7x9/SZ1gDCizAQzQUB8IKPKQee2znZJa4gljEwgZ33NfY/CPtsHuQnz8=";

    // 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
    public static string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxbGtVpnhZYkPvo22Gmx8FKK2OduFQFJPBAcutfO4ipxQXNTFSMnU+AyyK0jX3U1N9opHIvOPz9nWFuBjdaHhlr6JwiMopBCVWSRK1S+u+PcQCt/8TjpxFWkFQceVtYGL9K4sCaCumvtaKVO1oblT7CItj6vq7DxQYqGozY3OM3r4lNxNC1I3OaG5bmNIlEe4ILO5/oG9PZFw7NDU5qVEKtCd+1RrJ80BQwKLWWu2XTBpM9UCdMadW5RiJopU0Jv67ZnRhQew9fBUBe0sp03jAMJX0Rmy0GmGG1Gc0ejjWcIWbb1q6wHKUqDCFCWfN0ukWgG7VWg7SIX2eXCgdKClRwIDAQAB";

    public static string alipay1_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkM9Bsy2M7AIhWsolAHnT/STVi/8ONUobUiM03Rf/9PlCPX/koDhIBAWxgKM56nyvA50niHYsyT5liLZM8zPE0Y7p/m4igdFAlGXWo0WsimKOp3aPFYISpEh/0aGxUDYoWKDDf7a2t1FdSZF1sv1vouwbEXmXNGtGoyRnwLY0BwYKr204FeTinS/CKaOMJkM8c+Pp9q5QizUAotUelQDzJSWbKRJzEAL8NIokjxedPND6i9vjCaiEznnHPiCUC50qjjNS3E6KD9Xx5exnlUmdE7WYyNfVXHJw+RmTyI91bNAbA91OPKjBYVzdBM9+pLf2BJZ2Qg2ZxK90tVtXO/7eawIDAQAB";

    // 签名方式
    public static string sign_type = "RSA2";

    // 编码格式
    public static string charset = "UTF-8";


    public static string ReturnUrl = "http://www.zhongxiaole.net/Pay/Return_url.aspx";


    public static string NotifyUrl = "http://www.zhongxiaole.net/Pay/Notify_url.aspx";

}