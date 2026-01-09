<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="zxbRequire.aspx.cs" Inherits="WebUI.CustomerManagement.zxbRequire" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/CustomerManagement/zxbRequireJS.js"></script>   
     <div class="form-horizontal">
        <div class="form-group">
            <label class="col-sm-2 control-label no-padding-right need">乐币余额： </label>
            <div class="col-sm-5">
                <asp:TextBox ID="txtBalance" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label no-padding-right need">邀请人的手机号码： </label>
            <div class="col-sm-5">
                <asp:TextBox ID="InvitationTel" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                <button class="wtbtn savebtn lgbtn" onclick="return Recharge2()">提交</button>
            </div>
        </div>
    </div>
    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="zxbInstructions">
            <%=str %>
        </div>
        <div style="margin-bottom:10px;"> <button class="wtbtn savebtn" onclick="return Recharge3()">一键领取所有奖励</button></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="zxbRequireList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="zxbRequireListDiv"></div>
        </div>
    </div>
</asp:Content>
