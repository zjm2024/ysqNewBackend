<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CustomerPayOutHandle.aspx.cs" Inherits="SPlatformService.CustomerManagement.CustomerPayOutHandle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">

    <script type="text/javascript" src="../Scripts/CustomerManagement/CustomerPayOutHandleJS.js"></script>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right need">会员编号 </label>

        <div class="col-sm-9">
            <asp:textbox id="txtCustomerCode"  readonly="true" runat="server" cssclass="col-xs-10 col-sm-5" enabled="false" maxlength="100"></asp:textbox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right ">会员账号 </label>

        <div class="col-sm-9">
            <asp:textbox id="txtCustomerAccount"  readonly="true" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="100"></asp:textbox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right ">联系电话 </label>

        <div class="col-sm-9">
            <asp:textbox id="txtPhone"  readonly="true" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right">会员名称 </label>

        <div class="col-sm-9">
            <asp:textbox id="txtCustomerName"  readonly="true" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="50"></asp:textbox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right ">提现金额 </label>
        <div class="col-sm-9">
            <asp:textbox id="txtAmount" runat="server" readonly="true" cssclass="col-xs-10 col-sm-5"></asp:textbox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right need">银行名称 </label>
        <div class="col-sm-9">
            <asp:textbox id="txtBankName" readonly="true" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right need">开户银行名称 </label>
        <div class="col-sm-9">
            <asp:textbox id="txtSubBranch" readonly="true" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right need">账户名 </label>
        <div class="col-sm-9">
            <asp:textbox id="txtAccountName"  readonly="true" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right ">银行账号（卡号） </label>
        <div class="col-sm-9">
            <asp:textbox id="txtBankAccount"  readonly="true" runat="server"  cssclass="col-xs-10 col-sm-5"></asp:textbox>
        </div>
    </div>

    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right need">转账订单号</label>
        <div class="col-sm-9">
            <asp:textbox id="txtThirdOrder" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
        </div>
    </div>

    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right need">处理情况说明：</label>
        <div class="col-sm-9">
            <asp:textbox id="txtHandleComment" runat="server" textmode="MultiLine" cssclass="col-xs-10 col-sm-5" style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" maxlength="400"></asp:textbox>
        </div>
    </div>


    <div class="form-group">
        <label></label>
    </div>

    <div class="clearfix form-actions">
        <div class="col-sm-5" style="width: 700px; text-align: center;">
            <button class="wtbtn savebtn" type="button" id="btn_submit" runat="server" title="转账成功">
                转账成功
            </button>
            <button class="wtbtn savebtn" type="button" id="btn_save" runat="server" title="转账失败">
                转账失败
            </button>

        </div>
    </div>
    <asp:hiddenfield id="hidPayOutHistoryId" runat="server" />
</asp:Content>
