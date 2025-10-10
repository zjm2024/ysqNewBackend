<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="ZxbConfigCreateEdit.aspx.cs" Inherits="SPlatformService.CustomerManagement.ZxbConfigCreateEdit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/ZxbConfigCreateEditJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">调用编码 </label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtcode" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">奖励提示文本 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPurpose" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">奖励金额</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCost" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="20"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">状态 </label>

                    <div class="col-sm-9">
                        <label>
                            <input name="status" class="ace" type="radio" id="radStatusEnable" checked="checked" />
                            <span class="lbl">启用</span>
                        </label>
                        <label>
                            <input name="status" class="ace" type="radio" id="radStatusDisable" />
                            <span class="lbl">禁用</span>
                        </label>
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
