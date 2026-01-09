<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="RestPassword.aspx.cs" Inherits="WebUI.RestPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="Scripts/CustomerManagement/RestPassword.js"></script>
    <style>.left_qq{ display:none}</style>
    <div class="login_div">
        <div class="lmiddle">
            <div class="login_info">
                <div class="login_info_div">
                        <div class="login_info_title"><font>众销乐平台密码重置</font></div>
                        <div class="login_info_input">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="login_info_inputtext">手机号码</label>
                                    <div class="login_info_inputcontent">
                                        <input type="text" id="txtPhone" name="txtPhone" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="login_info_inputtext">新密码</label>
                                    <div class="login_info_inputcontent">
                                        <input type="password" id="txtPassword" name="txtPassword" placeholder="密码请设置8-20个字符"/>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="login_info_inputtext">验证码</label>
                                    <div class="login_info_inputcontent">
                                        <input type="text" id="txtValidCode" class="login_info_Codeinput" name="txtValidCode" placeholder="请输入短信验证码"/>
                                        <asp:Button runat="server" class="btn-primary login_info_Codebtn" ID="btnSendValidCode" Text="点击发送" />
                                        <input type="hidden" id="hidCode" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Button runat="server" class="btn-primary Register-primary" ID="btnRest" Text="重置密码" />
                                </div>
                            </div>
                        </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
