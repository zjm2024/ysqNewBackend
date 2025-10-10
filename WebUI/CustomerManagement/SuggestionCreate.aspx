<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="SuggestionCreate.aspx.cs" Inherits="WebUI.CustomerManagement.SuggestionCreate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/SuggestionCreateEditJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">标题 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">联系人 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtContactPerson" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">联系电话 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="20"></asp:TextBox>
                    </div>
                </div>       
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">反馈内容 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="description-textarea" MaxLength="400"></asp:TextBox>
                    </div>
                </div>               
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="text-align: center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="保存">
                            提交
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
