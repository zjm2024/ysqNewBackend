<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CustomerPayOutCreateEidt.aspx.cs" Inherits="WebUI.CustomerManagement.CustomerPayOutCreateEidt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/CustomerPayOutCreateEidtJS.js"></script>

    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="form-group">
                        <label class="col-sm-2 control-label no-padding-right need">账户余额： </label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtBalance" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-2 control-label no-padding-right need">请输入提现金额 </label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtAmount"  runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-2 control-label no-padding-right">请选择银行卡 </label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="drpBankList" runat="server" CssClass="col-xs-10 col-sm-2">
                                <asp:ListItem Text="新建银行账户" Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                         <%-- <input class="wtbtn savebtn" type="button" id="btnAddBank" onclick=" return AddBank();"
                value="添加银行卡" title="添加银行卡" />--%>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-2 control-label no-padding-right need">银行名称 </label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtBankName" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                        </div>
                    </div>
                      <div class="form-group">
                        <label class="col-sm-2 control-label no-padding-right need">开户银行名称 </label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtSubBranch" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                        </div>
                    </div>
                        <div class="form-group">
                        <label class="col-sm-2 control-label no-padding-right need">账户名 </label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtAccountName" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-2 control-label no-padding-right need">银行账号（卡号） </label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtBankAccount" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                        </div>
                    </div>

                    <div class="clearfix form-actions">
                        <div class="col-sm-5" style="text-align: center;">
                            <button class="wtbtn savebtn" type="button" id="btn_submit" runat="server" title="提交申请">
                                提交申请
                            </button>
                        <%--    <button class="wtbtn savebtn" type="button" id="btn_save" runat="server" title="保存">
                                保存
                            </button>--%>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
