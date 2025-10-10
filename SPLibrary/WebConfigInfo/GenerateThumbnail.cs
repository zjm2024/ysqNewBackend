using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CoreFramework;

namespace SPLibrary.WebConfigInfo
{
    public class Thumbnail
    {
        public string originalFilePath;
        public string outputFilePath;

        /// <summary>
        /// 借助ffmpeg生成缩略图
        /// </summary>
        /// <param name="originalFilePath">源文件</param>
        /// <param name="outputFilePath">输出文件</param>
        public void GenerateThumbnail(string originalFilePath,string outputFilePath)
        {
            try
            {
                //判断系统类型
                //如果是windows，直接使用ffmpeg.exe
                /*
                  windows:  ffmpeg.exe -i 333.jpg -q:v 31 -frames:v 1 -y image.jpg

                      -i 333.jpg 是输入文件
                      -q:v 31 是质量，值区间是2-31
                      -frames:v 1 是提取帧必要参数
                      -y 是遇到同名文件则覆盖 
                      image.jpg 输出文件名
                      还可以加 -s 160*100 表示输出宽高比为160*100
                 */
                string cmdPath = string.Empty;//ffmpeg工具对象
                string cmdParams = $" -i {originalFilePath} -q:v 31 -frames:v 1 -y {outputFilePath} ";//命令参数

                cmdPath = ConfigInfo.Instance.UploadFolder + "/ffmpeg.exe";//根据实际的ffmpeg.exe文件路径来

                using (System.Diagnostics.Process ffmpegProcess = new System.Diagnostics.Process())
                {
                    StreamReader errorReader;  // StringWriter to hold output from ffmpeg  
                                               // execute the process without opening a shell  
                    ffmpegProcess.StartInfo.UseShellExecute = false;
                    //ffmpegProcess.StartInfo.ErrorDialog = false;  
                    ffmpegProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    // redirect StandardError so we can parse it  
                    ffmpegProcess.StartInfo.RedirectStandardError = true;
                    // set the file name of our process, including the full path  
                    // (as well as quotes, as if you were calling it from the command-line)  
                    ffmpegProcess.StartInfo.FileName = cmdPath;

                    // set the command-line arguments of our process, including full paths of any files  
                    // (as well as quotes, as if you were passing these arguments on the command-line)  
                    ffmpegProcess.StartInfo.Arguments = cmdParams;

                    ffmpegProcess.Start();// start the process  

                    // now that the process is started, we can redirect output to the StreamReader we defined  
                    errorReader = ffmpegProcess.StandardError;

                    ffmpegProcess.WaitForExit();// wait until ffmpeg comes back  

                    //result = errorreader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                LogBO _log = new LogBO(typeof(Thumbnail));
                
                string strErrorMsg = "生成缩略图出错:" + ex.ToString();
                _log.Error(strErrorMsg);
            }
        }

        public void GenerateThumbnail()
        {
            GenerateThumbnail(originalFilePath, outputFilePath);
        }
    }
}
