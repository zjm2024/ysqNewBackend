<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXTestLoginPage.aspx.cs" Inherits="WebUI.ThirdLogin.WXTestLoginPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <div class="form-horizontal">
                <div>
                    <label>APPID</label>
                    <asp:TextBox ID="txtPhone" runat="server" />
                </div>

                <div>
                    <asp:Button runat="server" ID="btnRegister" Text="测试2" OnClick="Done1_Click" />
                    <asp:Button runat="server" ID="Button1" Text="测试1" OnClick="Done2_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
