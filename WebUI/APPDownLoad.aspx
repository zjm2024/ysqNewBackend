<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="APPDownLoad.aspx.cs" Inherits="WebUI.APPDownLoad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <div id="divNormal" class="download-pc">
        <div class="download-item">
            <h1>众销乐-资源共享众包销售平台</h1>
            <h2>快速寻找销售</h2>
        </div>
        <div class="download-item">
            <a href="javascript:void(0);" onclick="return iosAlert();">
                <img class="download-img" src="Style/images/ios-download.png" alt="" /></a>
            <a href="Download/zhongxiaoleAPP.apk">
                <img class="download-img" src="Style/images/android-download.png" alt="" /></a>

        </div>
        <div class="download-item">
            <img class="download-img" src="Style/images/zhongxiaoleapp.png" alt="" />
            <h2>立即扫码体验</h2>
        </div>
    </div>
    <img id="divWeichat" class="download-img" src="Style/images/android_guide.jpg" alt="" style="width: 100%; display: none;" />
    <script type="text/javascript">
        var browser = {
            versions: function () {
                var u = navigator.userAgent, app = navigator.appVersion;
                return {//移动终端浏览器版本信息
                    trident: u.indexOf('Trident') > -1, //IE内核
                    presto: u.indexOf('Presto') > -1, //opera内核
                    webKit: u.indexOf('AppleWebKit') > -1, //苹果、谷歌内核
                    gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') == -1, //火狐内核
                    mobile: !!u.match(/AppleWebKit.*Mobile.*/), //是否为移动终端
                    ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端
                    android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, //android终端或者uc浏览器
                    iPhone: u.indexOf('iPhone') > -1 || u.indexOf('Mac') > -1, //是否为iPhone或者QQHD浏览器
                    iPad: u.indexOf('iPad') > -1, //是否iPad
                    webApp: u.indexOf('Safari') == -1, //是否web应该程序，没有头部与底部
                    weichat: u.match(/MicroMessenger/i) == "MicroMessenger"
                };
            }(),
            language: (navigator.browserLanguage || navigator.language).toLowerCase()
        }

        if (browser.versions.mobile) {
            //控制浏览器显示内容            
        }

        if (browser.versions.weichat) {
            //如果是微信,微信
            if (browser.versions.ios) {
                //如果是苹果操作系统，跳转到苹果商店下载,上线之后即可拿到URL
                //window.location.href = "https://itunes.apple.com/cn/app/zhu-ba-jie-wei-ke-gai-bian/id597101749?ls=1&mt=8";
                alert("正在审核中，请耐心等待！");
            } else if (browser.versions.android) {
                //如果是安卓系统，引导在浏览器打开
                document.getElementById("divNormal").style.display = 'none';
                document.getElementById("divWeichat").style.display = '';
            }
        } else if (browser.versions.ios) {
            //如果是苹果操作系统，跳转到苹果商店下载,上线之后即可拿到URL
            //window.location.href = "https://itunes.apple.com/cn/app/zhu-ba-jie-wei-ke-gai-bian/id597101749?ls=1&mt=8";
            alert("正在审核中，请耐心等待！");
        } else if (browser.versions.android) {
            //如果是安卓系统，直接下载APK
            window.location.href = "Download/zhongxiaoleAPP.apk";
        }  else if (browser) {

        }

        function iosAlert() {
            alert("正在审核中，请耐心等待！");
            return false;
        }
    </script>
</asp:Content>
