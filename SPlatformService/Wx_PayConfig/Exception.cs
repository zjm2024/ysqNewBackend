using System;
using System.Collections.Generic;
using System.Web;

namespace SPlatformService
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {
            Log.Error("WxPayException", msg);
        }
     }
}