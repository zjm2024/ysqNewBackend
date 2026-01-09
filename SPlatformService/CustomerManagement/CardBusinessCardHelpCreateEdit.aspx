<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardBusinessCardHelpCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.CardBusinessCardHelpCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardBusinessCardHelpCreateEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">标题</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">视频链接</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtUrl" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="255"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">上传视频 </label>

                    <div class="col-sm-9">
                        <input id="imgNewsImg" name="id-input-file" type="file" onchange="change('imgNewsImg')" />
                        <div id="divPersonalCardImg" runat="server">
                            <%--<ul class="ace-thumbnails clearfix" id="divPersonalCard">
                            </ul>--%>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">视频号ID</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtfinderUserName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="255"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">视频号视频ID</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtfeedId" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="255"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">是否跳转到视频号 </label>

                    <div class="col-sm-9">
                        <label>
                            <input name="Agent" class="ace" type="radio" id="isFinderEnable"/>
                            <span class="lbl">是</span>
                        </label>
                        <label>
                            <input name="Agent" class="ace" type="radio" id="isFinderDisable"  checked="checked" />
                            <span class="lbl">否</span>
                        </label>
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">简介</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtSynopsis" runat="server" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">跳转类型 </label>

                    <div class="col-sm-9">
                        <asp:DropDownList ID="Type" runat="server" CssClass="col-xs-10 col-sm-2">
                            <asp:ListItem Text="个人功能相关" Value="1"></asp:ListItem>
                            <asp:ListItem Text="企业功能相关" Value="2"></asp:ListItem>
                            <asp:ListItem Text="其他功能" Value="3"></asp:ListItem>
                        </asp:DropDownList>
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
    <asp:HiddenField ID="HelpID" runat="server" />
</asp:Content>
