<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardMessageCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.CardMessageCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardMessageCreateEditJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">内容 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtMessage" runat="server" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="divSendAt">
                    <label class="col-sm-2 control-label no-padding-right">发送日期 </label>

                    <div class="col-sm-9">
                        <div class="input-group col-sm-2">
                            <asp:TextBox ID="txtSendAt" runat="server" CssClass="form-control date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:TextBox>
                            <span class="input-group-addon">
                                <i class="fa fa-calendar bigger-110"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">接收 </label>

                    <div class="col-sm-9">
                        <select id="Style">
                            <option value="0">全部人员</option>
                            <option value="1">仅活动用户</option>
                        </select>
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
    <asp:HiddenField ID="NoticeID" runat="server" />
</asp:Content>
