<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardNewsCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.CardNewsCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardNewsCreateEditJS.js"></script>
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
                    <label class="col-sm-2 control-label no-padding-right">缩略图 </label>

                    <div class="col-sm-9">
                        <input id="imgNewsImg" name="id-input-file" type="file" onchange="change('imgNewsImg')" />
                        <div id="divPersonalCardImg" runat="server">
                            <%--<ul class="ace-thumbnails clearfix" id="divPersonalCard">
                            </ul>--%>
                            <img id="imgNewsImgPic" src="https://www.zhongxiaole.net/SPManager/Style/images/logo.png" style="width: 150px" alt="" runat="server" ></img>
                        </div>
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">简介</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtSynopsis" runat="server" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">跳转类型 </label>

                    <div class="col-sm-9">
                        <asp:DropDownList ID="GoType" runat="server" CssClass="col-xs-10 col-sm-2">
                            <asp:ListItem Text="公众号" Value="1"></asp:ListItem>
                            <asp:ListItem Text="小程序" Value="2"></asp:ListItem>
                            <asp:ListItem Text="其他小程序" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">展示类型 </label>

                    <div class="col-sm-9">
                        <asp:DropDownList ID="ShowType" runat="server" CssClass="col-xs-10 col-sm-2">
                            <asp:ListItem Text="图文并排" Value="1"></asp:ListItem>
                            <asp:ListItem Text="单一大图" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">网址或小程序链接</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtUrl" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">其他小程序的AppID</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtAppId" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">排序</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="OrderNO" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
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
