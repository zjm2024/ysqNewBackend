<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UPExcel.aspx.cs" Inherits="SPlatformService.UPExcel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <input type="file" name="MyFileUploadInput" runat="server" /><asp:Button 
            ID="InputFileUploadButton" runat="server" Text="上传" 
            onclick="InputFileUploadButton_Click" />
    </div>
    </form>
</body>
</html>
