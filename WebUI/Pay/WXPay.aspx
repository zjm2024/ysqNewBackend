<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="WXPay.aspx.cs" Inherits="WebUI.Pay.WXPay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" language="javascript">
        function btnPayDone() {
            window.location.href = "../CustomerManagement/MyWallet.aspx";
            return false;

        }

    </script>
    <style type="text/css">
        .savebtn {
            font-family: "微软雅黑";
            background-color: #1272B8;
            color: rgb(255, 255, 255);
            padding: 10px 30px 30px 30px;
            font-size: 14px;
            font-weight: bold;
        }
    </style>
    <div runat="server" id="dvHide" style="display: none">
        <div style="margin-left: 150px; color: #ff0000; font-size: 30px; font-weight: bolder;">生成二维码失败，请重试！</div>
    </div>

    <div runat="server" id="dvshow">
        <div style="margin-left: 150px; color: #00CD00; font-size: 30px; font-weight: bolder;">请扫描支付</div>
        <br />
        <div style="margin-left: 150px;">
            <asp:Image ID="Image2" runat="server" Style="width: 200px; height: 200px;" />
        </div>
        <br />
        <div style="margin-left: 150px;">
            <asp:Image ID="imgDescription" runat="server" Style="width: 200px;" ImageUrl="~/Style/images/WXPayDes.png" />
        </div>

        <div style="height: 50px;">
        </div>
        <div style="margin-left: 150px;">
            <asp:Button runat="server" Height="50px" CssClass="savebtn" ID="done" Text=" 支付成功" OnClientClick=" return btnPayDone();" />
        </div>



    </div>
</asp:Content>
