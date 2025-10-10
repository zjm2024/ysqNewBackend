<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="QQLoginPage.aspx.cs" Inherits="WebUI.ThirdLogin.QQLoginPage" %>




<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/QQLogin.js"></script>

    <div class="main-container" style="background-color: #57d9f1;">
            <div class="main-content" style="margin-left: 0px;">
                <div class="row">
                    <div class="col-sm-10 col-sm-offset-1">
                        <div class="login-left">
                            <img src="../Style/images/logo.png" width="200" />
                        </div>
                        <div class="login-container login-right">

                            <div class="space-6"></div>

                            <div class="position-relative">
                                <div id="login-box" class="login-box visible widget-box no-border">
                                    <div class="widget-body" style="border-radius: 2px; background-color: #fcfcfc;">
                                        <div class="widget-main">
                                            <h4 class="header lighter bigger">
                                                <%--<i class="ace-icon fa fa-coffee green"></i>--%>
												授权成功，请填写基本资料！
                                            </h4>

                                            <div class="space-6"></div>

                                            <fieldset>
                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                         <asp:Label ID="Label1" runat="server" Text="手机号码"></asp:Label>
                                                        <asp:TextBox ID="txtPhone1" class="form-control" placeholder="手机号码" runat="server" />
                                                        <%--<i class="ace-icon fa fa-user"></i>--%>
                                                    </span>
                                                </label>

                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                           <asp:Label ID="Label2" runat="server" Text="密码"></asp:Label>
                                                        <asp:TextBox ID="txtPassword1" runat="server" class="form-control" TextMode="Password" placeholder="密码" />
                                                        <%--<i class="ace-icon fa fa-lock"></i>--%>
                                                    </span>
                                                </label>

                                                <div class="space"></div>

                                                <div class="clearfix">
                                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                                       <asp:Button runat="server" class="width-35 btn btn-sm btn-primary" ID="btnRegister" Text="完成" OnClick="Done_Click" />
                                                    <asp:HiddenField ID="hidOpenId" runat="server" />
                                                </div>
                                     <%--           <div style="margin-top:25px;">
                                                    <div style="float:left;width: 50%;text-align: center;" onclick="QQLogin();">
                                                        <img src="Style/images/img_login_qq.jpg" />
                                                        <br />
                                                        <span>QQ登录</span>
                                                    </div>
                                                    <div style="float:left;width: 50%;text-align: center;" onclick="WXLogin();">
                                                        <img src="Style/images/img_login_wechat.jpg" />
                                                        <br />
                                                        <span>微信登录</span>
                                                    </div>--%>
                                           <%--     </div>--%>
                                                <div class="space-4"></div>
                                                <div id="mol" runat="server"></div>
                                            </fieldset>
                                        </div>
                                    <%--    <div class="login-register">
                                            <a href="Register.aspx" target="_self">还没有账户，现在注册</a>
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

  <%--  <div class="main-container" style="background-color: #57d9f1;">
        <div class="main-content">
            <div class="row">
                <div class="col-sm-10 col-sm-offset-1">
                    <div class="login-left">
                        <img src="../Style/images/logo.png" width="200" />
                    </div>
                    <div class="login-container login-right">

                        <div class="position-relative">
                            <div id="login-box" class="login-box visible widget-box no-border">
                                <div class="widget-body" style="border-radius: 2px; background-color: #fcfcfc;">
                                    <div class="widget-main">
                                        <div class="center">
                                            <h2>
                                                <span class="grey" id="id-text2">授权成功，请填写基本资料！</span>
                                            </h2>

                                        </div>
                                        <div class="space-6"></div>
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label no-padding-right">手机号码</label>
                                                <asp:TextBox ID="txtPhone" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label no-padding-right">密码</label>
                                                <asp:TextBox ID="txtPassword" runat="server" />
                                            </div>
                                            <div class="form-group" style="height: 25px"></div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label no-padding-right"></label>
                                                <asp:Button runat="server" class="width-35 btn btn-sm btn-primary" ID="btnRegister" Text="完成" OnClick="Done_Click" />
                                                <asp:HiddenField ID="hidOpenId" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>
</asp:Content>
