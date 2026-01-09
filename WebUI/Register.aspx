<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WebUI.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="Scripts/CustomerManagement/Register.js"></script>
    <style>.left_qq{ display:none}</style>
    <div class="login_div">
        <div class="lmiddle">
            <div class="login_info">
                <div class="login_info_div">
                        <div class="login_info_title"><font>众销乐平台账号注册</font></div>
                        <div class="login_info_input">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="login_info_inputtext">选择身份</label>
                                    <div class="login_info_inputcontent">
                                        <label for="radSexMale">
                                            <input name="shengfen" class="ace" value="1" type="radio" id="radSexMale" checked="checked"/>
                                            <span class="lbl">销售</span>
                                        </label>
                                        <label for="radSexFeMale">
                                            <input name="shengfen" class="ace" value="2" type="radio" id="radSexFeMale"/>
                                            <span class="lbl">雇主</span>
                                        </label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="login_info_inputtext">手机号码</label>
                                    <div class="login_info_inputcontent">
                                        <input type="text" id="txtPhone" name="txtPhone" placeholder="请输入要注册的手机号"/>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="login_info_inputtext">设置密码</label>
                                    <div class="login_info_inputcontent">
                                        <input type="password" id="txtPassword" name="txtPassword" placeholder="密码请设置6-20个字符"/>
                                        <span id="Eyes" class="Eyes ClosedEyes"></span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="login_info_inputtext">验证码</label>
                                    <div class="login_info_inputcontent">
                                        <input type="text" id="txtValidCode" class="login_info_Codeinput" name="txtValidCode"  placeholder="请输入短信验证码"/>
                                        <button type="button" class="btn-primary login_info_Codebtn" ID="btnSendValidCode" onclick="btnSendValidCodeClick()">点击发送</button>
                                        <input type="hidden" id="hidCode" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <a href="../Rules.aspx?HelpDocTypeId=6">《众销乐服务协议》</a>
                                </div>
                                <div class="form-group">
                                    <asp:Button runat="server" class="btn-primary Register-primary" ID="btnRegister" Text="同意协议并注册" />
                                </div>
                                <div class="form-group Register_link">
                                    <a href="Login.aspx">已有账号，<font>马上登录</font></a>
                                </div>
                            </div>
                        </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
