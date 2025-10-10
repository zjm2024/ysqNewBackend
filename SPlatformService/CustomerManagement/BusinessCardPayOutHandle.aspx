<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="BusinessCardPayOutHandle.aspx.cs" Inherits="SPlatformService.CustomerManagement.BusinessCardPayOutHandle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">

    <script type="text/javascript" src="../Scripts/CustomerManagement/BusinessCardPayOutHandle.js"></script>
     <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right ">申请类型 </label>

        <div class="col-sm-9">
            <asp:textbox id="textType"  readonly="true" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox> <div style="color:red;line-height:31px;">0为银行卡提现，1为微信自动付款，2为对公账号</div>
        </div>
       
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right ">申请金额 </label>
        <div class="col-sm-9">
            <asp:textbox id="txtPayOutCost"  readonly="true" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right">服务费 </label>
        <div class="col-sm-9">
            <asp:textbox id="txtServiceCharge"  readonly="true" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="50"></asp:textbox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right ">实转金额 </label>
        <div class="col-sm-9">
            <asp:textbox id="txtAmount" runat="server" readonly="true" cssclass="col-xs-10 col-sm-5" Style="color:#8d0000"></asp:textbox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right ">AppID </label>

        <div class="col-sm-9">
            <asp:textbox id="Textbox1"  readonly="true" runat="server" cssclass="col-xs-10 col-sm-5">wx584477316879d7e9</asp:textbox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right ">OpenID </label>

        <div class="col-sm-9">
            <asp:textbox id="txtOpenID"  readonly="true" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right need">银行名称 </label>
        <div class="col-sm-9">
            <asp:textbox id="txtBankName" readonly="true" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
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
        <div class="col-sm-5" style="width: 700px; text-align: center; margin-top:20px;">
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
