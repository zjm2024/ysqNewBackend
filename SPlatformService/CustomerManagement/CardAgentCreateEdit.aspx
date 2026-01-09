<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardAgentCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.CardAgentCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardAgentCreateEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">代理名称</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtAgentName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">代理区域</label>
                    <div class="col-sm-9">
                        <asp:DropDownList ID="drpProvince" runat="server" CssClass="col-xs-10 col-sm-2">
                        </asp:DropDownList>
                        <asp:DropDownList ID="drpCity" runat="server" CssClass="col-xs-10 col-sm-2">
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
    <asp:HiddenField ID="CardAgentID" runat="server" />
</asp:Content>
