<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpenAIDemo.aspx.cs" Inherits="BusinessCard.OpenAIDemo" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="TextBox1" runat="server" Height="300px" TextMode="MultiLine" Width="600px" ReadOnly="True" ></asp:TextBox>
        </div>
        <div style="margin-top: 20px" >
            <asp:TextBox ID="TextBox2" runat="server" Height="30px" Width="600px" placeholder="请输入问题"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="发送" Height="30px" Width="106px" OnClick="Button1_Click"/>
        </div>
    </form>
</body>
</html>
