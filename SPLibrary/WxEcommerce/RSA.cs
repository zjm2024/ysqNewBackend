using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using System.Security.Cryptography.X509Certificates;

namespace WxEcommerce
{
    class RSACryptoItem
    {
        public RSACryptoServiceProvider Provider;
        public List<byte> PubKeyBytes;
    }

    public class RSAManager
    {
        private RSACryptoItem item;

        public RSAManager()
        {
            item = GenRSACryptoItem();
        }

        private RSACryptoItem GenRSACryptoItem()
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            List<byte> pubKeyBytes = new List<byte>(provider.ExportCspBlob(false));
            return new RSACryptoItem
            {
                Provider = provider,
                PubKeyBytes = pubKeyBytes,
            };
        }   

        /// <summary>
        /// 使用公钥加密
        /// </summary>
        /// <param name="inBytes"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static byte[] EncryptDataByPublicKey(byte[] inBytes, string publicKey)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            publicKey = RSAKeyConverter.ToXmlPublicKey(publicKey);
            provider.FromXmlString(publicKey);
            return provider.Encrypt(inBytes, false);
        }
        public static string RSAEncrypt(string text, byte[] publicKey)
        {
            var x509 = new X509Certificate2(publicKey);
            using (var rsa = (RSACryptoServiceProvider)x509.PublicKey.Key)
            {
                var buff = rsa.Encrypt(Encoding.UTF8.GetBytes(text), true);

                return Convert.ToBase64String(buff);
            }
        }

        /// <summary>
        /// 使用私钥解密
        /// </summary>
        /// <param name="inBytes"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static byte[] DecryptDataByPrivateKey(byte[] inBytes, string privateKey)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            privateKey = RSAKeyConverter.ToXmlPrivateKey(privateKey);
            provider.FromXmlString(privateKey);
            return provider.Decrypt(inBytes, false);
        }


        /// <summary>
        /// 生成数字签名
        /// </summary>
        /// <param name="originalText">原文</param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string GenSign(string originalText, string privateKey)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(originalText);
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            privateKey = RSAKeyConverter.ToXmlPrivateKey(privateKey);
            provider.FromXmlString(privateKey);

            //使用SHA256进行摘要算法，生成签名
            byteData = provider.SignData(byteData, "SHA256");
            return Convert.ToBase64String(byteData);
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="originalText">原文</param>
        /// <param name="SignedData">签名</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static bool VerifySigned(string originalText, string signedData, string publicKey)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            publicKey = RSAKeyConverter.ToXmlPublicKey(publicKey);
            provider.FromXmlString(publicKey);
            byte[] byteData = Encoding.UTF8.GetBytes(originalText);
            byte[] signData = Convert.FromBase64String(signedData);
            return provider.VerifyData(byteData, new SHA256CryptoServiceProvider(), signData);
        }

        public byte[] EncryptData(byte[] inBytes)
        {
            if (item != null)
                return item.Provider.Encrypt(inBytes, false);
            return null;
        }

        public byte[] DecryptData(byte[] inBytes)
        {
            if (item != null)
                return item.Provider.Decrypt(inBytes, false);
            return null;
        }

        public static string Sign(string contentForSign, string priKeyFile, string keyPwd)
        {
            var rsa = GetPrivateKey(priKeyFile, keyPwd);
            // Create a new RSACryptoServiceProvider
            var rsaClear = new RSACryptoServiceProvider();
            // Export RSA parameters from 'rsa' and import them into 'rsaClear'
            var paras = rsa.ExportParameters(true);
            rsaClear.ImportParameters(paras);
            using (var sha256 = new SHA256CryptoServiceProvider())
            {
                var signData = rsa.SignData(Encoding.UTF8.GetBytes(contentForSign), sha256);
                return BytesToHex(signData);
            }
        }

        public bool VerifySign(string contentForSign, string signedData, string pubKeyFile)
        {
            var rsa = GetPublicKey(pubKeyFile);

            using (var sha256 = new SHA256CryptoServiceProvider())
            {
                return rsa.VerifyData(Encoding.UTF8.GetBytes(contentForSign), sha256, HexToBytes(signedData));
            }
        }

        /// <summary>
        /// 获取签名证书私钥
        /// </summary>
        /// <param name="priKeyFile"></param>
        /// <param name="keyPwd"></param>
        /// <returns></returns>
        private static RSACryptoServiceProvider GetPrivateKey(string priKeyFile, string keyPwd)
        {
            var pc = new X509Certificate2(priKeyFile, keyPwd, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
            return (RSACryptoServiceProvider)pc.PrivateKey;
        }

        /// <summary>
        /// 获取验签证书
        /// </summary>
        /// <param name="pubKeyFile"></param>
        /// <returns></returns>
        private static RSACryptoServiceProvider GetPublicKey(string pubKeyFile)
        {
            var pc = new X509Certificate2(pubKeyFile);
            return (RSACryptoServiceProvider)pc.PublicKey.Key;
        }
        public static byte[] HexToBytes(string text)
        {
            if (text.Length % 2 != 0)
                throw new ArgumentException("text 长度为奇数。");

            List<byte> lstRet = new List<byte>();
            for (int i = 0; i < text.Length; i = i + 2)
            {
                lstRet.Add(Convert.ToByte(text.Substring(i, 2), 16));
            }
            return lstRet.ToArray();
        }

        /// <summary>
        /// bytes转换hex
        /// </summary>
        /// <param name="data">bytes</param>
        /// <returns>转换后的hex字符串</returns>
        public static string BytesToHex(byte[] data)
        {
            StringBuilder sbRet = new StringBuilder(data.Length * 2);
            for (int i = 0; i < data.Length; i++)
            {
                sbRet.Append(Convert.ToString(data[i], 16).PadLeft(2, '0'));
            }
            return sbRet.ToString();
        }
    }
    public class RSAKeyConverter
    {
        /// <summary>
        /// xml private key -> base64 private key string
        /// </summary>
        /// <param name="xmlPrivateKey"></param>
        /// <returns></returns>
        public static string FromXmlPrivateKey(string xmlPrivateKey)
        {
            string result = string.Empty;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPrivateKey);
                RSAParameters param = rsa.ExportParameters(true);
                RsaPrivateCrtKeyParameters privateKeyParam = new RsaPrivateCrtKeyParameters(
                    new BigInteger(1, param.Modulus), new BigInteger(1, param.Exponent),
                    new BigInteger(1, param.D), new BigInteger(1, param.P),
                    new BigInteger(1, param.Q), new BigInteger(1, param.DP),
                    new BigInteger(1, param.DQ), new BigInteger(1, param.InverseQ));
                PrivateKeyInfo privateKey = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKeyParam);

                result = Convert.ToBase64String(privateKey.ToAsn1Object().GetEncoded());
            }
            return result;
        }

        /// <summary>
        /// xml public key -> base64 public key string
        /// </summary>
        /// <param name="xmlPublicKey"></param>
        /// <returns></returns>
        public static string FromXmlPublicKey(string xmlPublicKey)
        {
            string result = string.Empty;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPublicKey);
                RSAParameters p = rsa.ExportParameters(false);
                RsaKeyParameters keyParams = new RsaKeyParameters(
                    false, new BigInteger(1, p.Modulus), new BigInteger(1, p.Exponent));
                SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keyParams);
                result = Convert.ToBase64String(publicKeyInfo.ToAsn1Object().GetEncoded());
            }
            return result;
        }

        /// <summary>
        /// base64 private key string -> xml private key
        /// </summary>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string ToXmlPrivateKey(string privateKey)
        {
            RsaPrivateCrtKeyParameters privateKeyParams =
                PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey)) as RsaPrivateCrtKeyParameters;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                RSAParameters rsaParams = new RSAParameters()
                {
                    Modulus = privateKeyParams.Modulus.ToByteArrayUnsigned(),
                    Exponent = privateKeyParams.PublicExponent.ToByteArrayUnsigned(),
                    D = privateKeyParams.Exponent.ToByteArrayUnsigned(),
                    DP = privateKeyParams.DP.ToByteArrayUnsigned(),
                    DQ = privateKeyParams.DQ.ToByteArrayUnsigned(),
                    P = privateKeyParams.P.ToByteArrayUnsigned(),
                    Q = privateKeyParams.Q.ToByteArrayUnsigned(),
                    InverseQ = privateKeyParams.QInv.ToByteArrayUnsigned()
                };
                rsa.ImportParameters(rsaParams);
                return rsa.ToXmlString(true);
            }
        }

        /// <summary>
        /// base64 public key string -> xml public key
        /// </summary>
        /// <param name="pubilcKey"></param>
        /// <returns></returns>
        public static string ToXmlPublicKey(string pubilcKey)
        {
            RsaKeyParameters p =
                PublicKeyFactory.CreateKey(Convert.FromBase64String(pubilcKey)) as RsaKeyParameters;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                RSAParameters rsaParams = new RSAParameters
                {
                    Modulus = p.Modulus.ToByteArrayUnsigned(),
                    Exponent = p.Exponent.ToByteArrayUnsigned()
                };
                rsa.ImportParameters(rsaParams);
                return rsa.ToXmlString(false);
            }
        }
    }
}