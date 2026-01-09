<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardAdd.aspx.cs" Inherits="SPlatformService.RequireManagement.CardAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardAddJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right" style="margin-top:28px;">头像 </label>
                    <div class="col-sm-9">
                        <img src="<%=Headimg %>"" style="width:80px; height:80px; border-radius:100px;float:left;" id="Headimg"/>
                        <button class="HeadimgAdd" type="button" id="RandomHeadimg">更换头像</button>
                        <style>
                            .HeadimgAdd{float:left;margin-top:25px;margin-left:20px; background-color: #1272B8;color: rgb(255, 255, 255);padding: 0px 30px; line-height:30px; border-radius:3px; cursor:pointer; border:none}
                            .HeadimgAdd:active{background-color: #0a61a0;}
                        </style>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">姓名 </label>
                    <div class="col-sm-9">
                        <asp:textbox id="txtName" runat="server" cssclass="col-xs-10 col-sm-3" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">手机 </label>
                    <div class="col-sm-9">
                        <asp:textbox id="txtPhone" runat="server" cssclass="col-xs-10 col-sm-3"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">职位 </label>
                    <div class="col-sm-9">
                        <asp:textbox id="txtPosition" runat="server" cssclass="col-xs-10 col-sm-3"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">单位名称 </label>
                    <div class="col-sm-9">
                        <asp:textbox id="txtCorporateName" runat="server" cssclass="col-xs-10 col-sm-3"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">办公地址 </label>
                    <div class="col-sm-9">
                        <asp:textbox id="txtAddress" runat="server" cssclass="col-xs-10 col-sm-3"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">主营业务 </label>
                    <div class="col-sm-9">
                        <asp:textbox id="txtBusiness" runat="server" cssclass="col-xs-10 col-sm-3"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">微信 </label>
                    <div class="col-sm-9">
                        <asp:textbox id="txtWeChat" runat="server" cssclass="col-xs-10 col-sm-3"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">邮箱 </label>
                    <div class="col-sm-9">
                        <asp:textbox id="txtEmail" runat="server" cssclass="col-xs-10 col-sm-3"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">固定电话 </label>
                    <div class="col-sm-9">
                        <asp:textbox id="txtTel" runat="server" cssclass="col-xs-10 col-sm-3"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">官网 </label>
                    <div class="col-sm-9">
                        <asp:textbox id="txtWebSite" runat="server" cssclass="col-xs-10 col-sm-3"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">个人简介 </label>
                    <div class="col-sm-9">
                        <asp:textbox id="txtDetails" runat="server" cssclass="col-xs-10 col-sm-3" AcceptsReturn="true" TextMode="MultiLine" style="height:200px;"></asp:textbox>
                    </div>
                </div>

                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width: 700px; text-align: center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="创建名片">
                            创建名片</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:hiddenfield id="hidSoftArticleID" runat="server" />
</asp:Content>
