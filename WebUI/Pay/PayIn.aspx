<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="PayIn.aspx.cs" Inherits="WebUI.Pay.PayIn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript">
        function GoToPay() {
            window.location.href = 'WXPay.aspx';
        }
    </script>
    <div>
        <span>众销乐-资源共享众包销售平台充值</span>
    </div>
    <div>
        <span>充值金额</span>
        <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
    </div>
    <div>
        <span>支付方式</span>
        <img src="../Style/images/WePayLogo.png" style="width:100px;" onclick="GoToPay()" />
        <img />支付宝支付
    </div>
    <div>
        <asp:Button ID="btnPay" runat="server" Text="立即支付" />
    </div>
</asp:Content>
