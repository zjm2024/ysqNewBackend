using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using SPLibrary.CustomerManagement.BO;
using CoreFramework.VO;

namespace SPLibrary.WebConfigInfo
{
    /// <summary>
    ///Action 的摘要说明
    /// </summary>
    public static class TaskAction
    {
        private static string content = "";
        /// <summary>
        /// 输出信息存储的地方.
        /// </summary>
        public static string Content
        {
            get { return TaskAction.content; }
            set { TaskAction.content += "<div>" + value + "</div>"; }
        }
        /// <summary>
        /// 定时器委托任务 调用的方法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public static void SetContent(object source, ElapsedEventArgs e)
        {
            Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //检查活动开始时间，发送活动提醒
            //CardBO cBO = new CardBO(new CustomerProfile());
            //cBO.sendStartMess();
        }
        /// <summary>
        /// 应用池回收的时候调用的方法
        /// </summary>
        public static void SetContent()
        {
            Content = "END: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}

 
