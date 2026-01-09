<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="SystemMessageCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.SystemMessageCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/UserManagement/SystemMessageCreateEditJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">消息类型 </label>

                    <div class="col-sm-9">
                        <asp:DropDownList ID="drpMessageType" runat="server" CssClass="col-xs-10 col-sm-2">
                        </asp:DropDownList>
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">标题 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">内容 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtMessage" runat="server" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="divSendAt" style="display:none;">
                    <label class="col-sm-2 control-label no-padding-right">发送时间 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtSendAt" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width:700px;text-align:center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="保存" >
                            保存
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                            返回
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidSystemMessageId" runat="server" />
</asp:Content>
