<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CarouselCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.CarouselCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/UserManagement/CarouselCreateEditJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">轮播消息 </label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtText" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width: 700px; text-align: center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="保存">
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
    <asp:HiddenField ID="hidCustomerId" runat="server" />
    <asp:HiddenField ID="hidIsDelete" runat="server" />
    <asp:HiddenField ID="hidIsEdit" runat="server" />
</asp:Content>
