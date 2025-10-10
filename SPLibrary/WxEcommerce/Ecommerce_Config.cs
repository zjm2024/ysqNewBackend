using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.WxEcommerce
{
    /**
   * 	配置账号信息
   */
    public class EConfig
    {

        private static volatile Ecommerce_Config config;
        private static object syncRoot = new object();

        public static Ecommerce_Config GetConfig()
        {
            if (config == null)
            {
                lock (syncRoot)
                {
                    if (config == null)
                        config = new Ecommerce_Config();
                }
            }
            return config;
        }

    }
    /// <summary>
    /// 电商收付通配置类
    /// </summary>
    public class Ecommerce_Config
    {

        /// <summary>
        /// 获取小程序AppID
        /// </summary>
        /// <returns></returns>
        public string GetAppID()
        {
            return "wx9a65d7becbbb017a";
        }

        /// <summary>
        /// 获取商户号
        /// </summary>
        /// <returns></returns>
        public string GetMchID()
        {
            return "1610565126";
        }

        /// <summary>
        /// 获取API密钥
        /// </summary>
        /// <returns></returns>
        public string GetKey()
        {
            return "geFsXPf3i8QuZ3APc1VxgIV1Rym6X6A1";
        }

        /// <summary>
        /// 获取APIv3密钥
        /// </summary>
        /// <returns></returns>
        public string GetKeyV3()
        {
            return "XJ588tYXOkg8d1cXlMf8TKEv9wTVBIWU";
        }

        /// <summary>
        /// 获取证书路径
        /// </summary>
        /// <returns></returns>
        public string GetCertPath()
        {
            return @"C:/web/ca/1610565126_20210708_cert/apiclient_cert.p12";
        }

        /// <summary>
        /// 获取私钥路径
        /// </summary>
        /// <returns></returns>
        public string GetPrivateKeyPath()
        {
            return @"C:/web/ca/1610565126_20210708_cert/apiclient_key.pem";
        }

        /// <summary>
        /// 获取证书私钥
        /// </summary>
        /// <returns></returns>
        public string GetPrivateKey()
        {
            string key = @"MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDLuFphGM8dh87a
SKEGs7tz5iUEXQlM9dqvqB3vYWtNG4nt0YyUkRsGW5fKd90QH6vK83LVgPwZ2G64
iSuSAFZLps8hqvLCEDoaW0epberi+XN9rbC2qEpcWmgsQ50W9dWKun52Hx7ttX5l
ykC00c0ZRDWYtFdea6g5wgeqzbAZe0wG8stEldD48cqcqkPh69+3S1vmyw11XxDL
lG0LGo/XLbTxyseYmOgWU9TxnJjj6Os6kUMZkOVHpCRya0CICoEfygfGq2YnTcfG
3aZqVZVR6BoW4Wtt879MrU7DNFzK4tzDoqmVs9/d/A1cnroCqyU6Bwbd/tjDsS/9
FR7J+Iy9AgMBAAECggEBAIATTK7SnBOiuAc/tgyynl0FCayumRRLVMsuVHOUuZLm
h1Z+cXVNrMHYf5uIs/BZGPYlC8LyrovJKALYKg5GK/qvynUONkSPUxgS3wDD2/IK
hfMzaW/MacOaCvgZIf+3WiM3kB3O/vDPM8Hm37tOUzVZTqDMcEkqQb7eH1fgrJ+3
Zaoxz1kmxx/JNWnLsy65DjZdCnAPtw07q2ACsesbsUTFRtwH1BDxUhFkC76GrrIR
8HdUdp5RCX/CfhPQT1AyM/fq2CUtlLCrloxaQtvJXlRLM0s7c8wXxBxpGSJSS1ky
Gkg750ZFq8hqcC6dzwspTN/G15gucormfzJjzWvuyPkCgYEA/UvDgg3HmYN4TOWX
ByYrqoCNdGteSvn9B8Mhq8cMs5ZUEA4bE7rbQrwSBWKf2Kjx/FaCMrX47IVfAURf
EPb+gpkCpmeU8g9/GdJvU4CA2KJc5rJpk3kioKv9hFAM4lnECQUgc01OVWsV5J58
pxn8l7Bosw8QRRnl1L4pvWqH7AsCgYEAzeUaUz+wk9jV4BC2rZBq99eyDoUEaCKE
F7+A+K1cAkA85cKtVBomdTUjXvuLE86NF7d6mp4TRPdU5TTkaesuCsQS+IpokbQ4
D9CDWsRGxmr24TAcPaiFN4Etg2xPiyvVTWLJcbsASeeVUg32JeQFDdkGrWKb8q5S
18Rcv83WH1cCgYA8VtwQEzBFRahWySqdLV6hk1YfMaSYv8yjGLzgqt4/ucP0vyd8
H7szvpGcbs4dUsGylvkVE3xk+7CSILN02yHuAn07HtnAgCiLAjI69hW7hZIT8tRr
qSOymsYdyuQG19N3Qp1kWjoa5xOkDo0rUWl2yTNn1GLP00eI0nQnB0Wx2QKBgGOd
oOr+OZSOqIvMClZXNSnMdh2IReWMLZS/1sp6NbXxFoWjdvDl5g13NDHUSim/GYyL
/QmpyXDoqW5r9Mc6PgB/dR5C+Ozl4Onl3Geh5wxA3El/WWqLikice0TGtoWDI6RZ
U1VvQGiBX0Ff9+PFOpzEj/J7bzAvH229V5KFKD/pAoGAJYcJCZMeLVk+bHCwow7V
4XFRuAZG0PEsjJHVkhR9DY4m8QGrLs1AuORcIH05P6MMooNDc2+wo4eE2gTb3LBo
q8YtAAPfyWceTNlvEfpnHcibghHAotwQbdq0gUMc3g3uF59llFAP1L01iOfmoVbc
YB4svCMQfYMmNTwlvwSw4k0=";
            return key;
        }

        /// <summary>
        /// 获取证书公钥
        /// </summary>
        /// <returns></returns>
        public string GetPublicKey()
        {
            string key = @"MIID/DCCAuSgAwIBAgIUN1m3l6vp8N3+rVmj7YdCSiLAs2swDQYJKoZIhvcNAQEL
BQAwXjELMAkGA1UEBhMCQ04xEzARBgNVBAoTClRlbnBheS5jb20xHTAbBgNVBAsT
FFRlbnBheS5jb20gQ0EgQ2VudGVyMRswGQYDVQQDExJUZW5wYXkuY29tIFJvb3Qg
Q0EwHhcNMjEwNzA4MDM0NDM0WhcNMjYwNzA3MDM0NDM0WjCBjTETMBEGA1UEAwwK
MTYxMDU2NTEyNjEbMBkGA1UECgwS5b6u5L+h5ZWG5oi357O757ufMTkwNwYDVQQL
DDDlub/lt57ljY7pobrpnZLkuLrkv6Hmga/np5HmioDmnInpmZDotKPku7vlhazl
j7gxCzAJBgNVBAYMAkNOMREwDwYDVQQHDAhTaGVuWmhlbjCCASIwDQYJKoZIhvcN
AQEBBQADggEPADCCAQoCggEBAMu4WmEYzx2HztpIoQazu3PmJQRdCUz12q+oHe9h
a00bie3RjJSRGwZbl8p33RAfq8rzctWA/BnYbriJK5IAVkumzyGq8sIQOhpbR6lt
6uL5c32tsLaoSlxaaCxDnRb11Yq6fnYfHu21fmXKQLTRzRlENZi0V15rqDnCB6rN
sBl7TAbyy0SV0PjxypyqQ+Hr37dLW+bLDXVfEMuUbQsaj9cttPHKx5iY6BZT1PGc
mOPo6zqRQxmQ5UekJHJrQIgKgR/KB8arZidNx8bdpmpVlVHoGhbha23zv0ytTsM0
XMri3MOiqZWz3938DVyeugKrJToHBt3+2MOxL/0VHsn4jL0CAwEAAaOBgTB/MAkG
A1UdEwQCMAAwCwYDVR0PBAQDAgTwMGUGA1UdHwReMFwwWqBYoFaGVGh0dHA6Ly9l
dmNhLml0cnVzLmNvbS5jbi9wdWJsaWMvaXRydXNjcmw/Q0E9MUJENDIyMEU1MERC
QzA0QjA2QUQzOTc1NDk4NDZDMDFDM0U4RUJEMjANBgkqhkiG9w0BAQsFAAOCAQEA
rB4ofuVN+KckvApQY99jsRt9Sr06OTc48QY14jrNsfg7s4hVnG/URuwdLRz6FeuT
kvWdJunXhx48VK91IvRTUyBszunYvCTY4S9BxPMqTtxoek2Mo5vZYkLrLOkonro/
LGBgA2aaXHn3L9brjaGwSeyDrH724MpgAz6h3dm2t1pp51dhWLr6KsVS8mdVrwu0
3TCoSzSuh4YulOc5ifBSg2z3Nn+fDs+51X8c0mShAEEOVaLOjVQvUEn2NvKjmEpg
KNp1VVrc6exh+RckM0Q1Mr0ESzvB+HJMdlQlnj2+/J2q46ZdU7uBRA+tB9cJDnO0
pJISr20A815hHLjfGR3Jzg==";
            return key;
        }


        /// <summary>
        /// 获取证书序列号
        /// </summary>
        /// <returns></returns>
        public string GetSerial()
        {
            return "3759B797ABE9F0DDFEAD59A3ED87424A22C0B36B";
        }

        /// <summary>
        /// 获取平台证书序列号
        /// </summary>
        /// <returns></returns>
        public string GetWxSerial()
        {
            EcommerceBO eBO = new EcommerceBO();
            return eBO.FindCertificatesById().serial_no;
        }

        /// <summary>
        /// 获取平台证书公钥
        /// </summary>
        /// <returns></returns>
        public string GetWxPublicKey()
        {
            EcommerceBO eBO = new EcommerceBO();
            string PublicKey= eBO.FindCertificatesById().PublicKey;
            PublicKey = PublicKey.Replace($"-----BEGIN CERTIFICATE-----\n", "");
            PublicKey = PublicKey.Replace($"\n-----END CERTIFICATE-----", "");
            return PublicKey;
        }
    }
}
