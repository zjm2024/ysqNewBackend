<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebUI.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="Scripts/CustomerManagement/Login.js"></script>
    <style>.left_qq{ display:none}</style>
    <script type="text/javascript">
        function QQLogin() {
            var A = window.open("ThirdLogin/QQLoginPage.aspx", "QQ登录");
        }
        function WXLogin() {
            if (is_weixn()) {
                var A = window.open("ThirdLogin/WXLoginPage.aspx?wxtype=2", "微信公众号登录");
            } else {
                var A = window.open("ThirdLogin/WXLoginPage.aspx?wxtype=1", "微信登录");
            }
        }
        function is_weixn() {
            var ua = navigator.userAgent.toLowerCase();
            if (ua.match(/MicroMessenger/i) == "micromessenger") {
                return true;
            } else {
                return false;
            }
        }
    </script>
    <div class="login_div">
        <div class="lmiddle">
            <div class="login_info">
                <div class="login_info_div">
                    <fieldset>
                        <div class="login_info_title"><font>使用手机号登录</font></div>
                        <div class="login_info_input">
                            <label class="block clearfix">
                                 <span class="block input-icon input-icon-right">
                                     <%--<input class="form-control" placeholder="用户名" type="text">--%>
                                     <asp:TextBox ID="txtLoginName" class="form-control" placeholder="请输入手机号" runat="server" />
                                     <i class="ace-icon fa fa-user"></i>
                                 </span>
                            </label>
                            <label class="block clearfix">
                                 <span class="block input-icon input-icon-right">
                                    <asp:TextBox ID="txtPassword" runat="server" class="form-control" TextMode="Password" placeholder="密码" />
                                    <span id="Eyes" class="Eyes ClosedEyes"></span>
                                    <!--<i class="ace-icon fa fa-lock"></i>-->
                                 </span>
                            </label>
                            <div class="clearfix">
                                 <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                 <asp:Button runat="server" class="btn-primary" ID="btnLogin" Text="登录" OnClick="btnLogin_Click"/>
                                 <div class="login_info_link">
                                     <a class="RestPassword_link"  href="RestPassword.aspx">忘记密码<i class="ace-icon fa fa-angle-double-right"></i></a>
                                     <a class="pull-right"  href="Register.aspx">还没有众销乐帐号？<font>点击注册<i class="ace-icon fa fa-angle-double-right"></i></font></a>
                                 </div>
                            </div>
                        </div>
                        <div class="login_info_title"><font>使用第三方帐号登录</font></div>
                        <div class="login_info_qqlogin">
                             <div class="login_info_qqbtn" onclick="QQLogin();">
                                 <img src="Style/images/newpc/qq.jpg" />
                             </div>
                             <div class="login_info_qqbtn" onclick="WXLogin();">
                                 <img src="Style/images/newpc/wx.jpg" />
                             </div>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
