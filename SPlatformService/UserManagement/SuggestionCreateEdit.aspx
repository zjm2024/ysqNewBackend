<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="SuggestionCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.SuggestionCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/UserManagement/SuggestionCreateEditJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">				
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">联系人 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtContactPerson" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="30" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">会员 </label>

                    <div class="col-sm-9">
                        <a href="../CustomerManagement/CustomerCreateEdit.aspx?CustomerId=<%=CustomerId%>" target="_blank" style="line-height:31px;">查看会员</a>
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">联系电话 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtContactPhone" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="30" Enabled="false"></asp:TextBox>
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">标题 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="20" Enabled="false"></asp:TextBox>
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">内容 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtDescription" runat="server" Enabled="false" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">时间 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCreatedAt" runat="server" CssClass="col-xs-10 col-sm-5"  Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width:700px;text-align:center;">
                        <button class="wtbtn savebtn" type="button" id="btn_Reply" title="回复">
                            回复
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_delete" title="删除" style="display:none;">
                            删除
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                            返回
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidSuggestionId" runat="server" />
    <asp:HiddenField ID="hidIsDelete" runat="server" />
    <asp:HiddenField ID="hidIsEdit" runat="server" />
</asp:Content>
