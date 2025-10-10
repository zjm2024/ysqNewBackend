using System;

namespace SPlatformService.TokenMange
{
    /// <summary>
    /// 匿名访问标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AnonymousAttribute : Attribute
    {
    }

    
}