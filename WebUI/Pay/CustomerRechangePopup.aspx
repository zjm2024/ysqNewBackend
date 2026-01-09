<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerRechangePopup.aspx.cs" Inherits="WebUI.Pay.CustomerRechangePopup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>众销乐-资源共享众包销售平台充值</title>

</head>



<body>
    <form id="form1" runat="server">
            <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/assets/js/jquery1x.min.js")%>"></script>
           <script type="text/javascript" language="javascript">   
               
         var _RootPath = "<%= APIURL%>";
        var _Token = "<%= Token%>";
               var _CustomerId = "<%= CustomerId%>"; 

               function GetRadioListValue(radioList) {
                   var value="";         
                   if(radioList.tagName.toLowerCase()=="table")
                   {               
                       for(var i=0;i<radioList.rows.length;i++){
                           var row=radioList.rows[i];
                           for(var j=0;j<row.cells.length;j++){
                               var td=row.cells[j];
                               for(var c=0;c<td.childNodes.length;c++){
                                   if(td.childNodes[c].tagName.toLowerCase()=="input" && td.childNodes[c].type=="radio")  
                                   {
                                       if(td.childNodes[c].checked) value=td.childNodes[c].value;
                                   }
                               }
                           }
                       }              
                   }
                   return value; 
               }  

       function doPay()
       {
           debugger
           var txtAmount = document.getElementById("txtAmount");
           if (txtAmount.value == "")
           {
               alert("请输入充值金额");
               return false;

           }
           txtAmount.disabled = true;
           document.getElementById("dvpaytype").style.display = "";
           var rbPayType = document.getElementById("rbPayType");
           
           if (GetRadioListValue(rbPayType) == "2") {
               var out_trade_no = "201709281415001008";
               var subject = "众销乐-资源共享众包销售平台测试支付"
               var total_amout = txtAmount.value;
               var body = "服务描述"
               var payReturn = document.getElementById("payReturn");
               $.ajax({
                   url: _RootPath + "SPWebAPI/Project/AliPagePay?out_trade_no=" + out_trade_no + "&subject=" + subject + "&total_amout=" + total_amout + "&body=" + body + "&token=" + _Token,
                   type: "Get",
                   data: null,
                   success: function (data) {
                       if (data.Flag == 1) {

                           alert(data.Message);
                           //bootbox.dialog({
                           //    message: data.Message,
                           //    buttons:
                           //    {
                           //        "Confirm":
                           //        {
                           //            "label": "确定",
                           //            "className": "btn-sm btn-primary",
                           //            "callback": function () {
                           //                window.location.href = "MessageBrowse.aspx";
                           //            }
                           //        }
                           //    }
                           //});
                       } else {
                           alert(data.Message);
                           //bootbox.dialog({
                           //    message: data.Message,
                           //    buttons:
                           //    {
                           //        "Confirm":
                           //        {
                           //            "label": "确定",
                           //            "className": "btn-sm btn-primary",
                           //            "callback": function () {

                           //            }
                           //        }
                           //    }
                           //});
                       }
                       //load_hide();
                   },
                   error: function (data) {
                       alert(data);
                       //load_hide();
                   }
               });
           }
           return false;

          
       }
       function doback() {
           debugger
           var txtAmount = document.getElementById("txtAmount");
           txtAmount.disabled = false;
           document.getElementById("dvpaytype").style.display = "none";

        

       }

   </script>
    <div>
           <div class="section-main g-header-main"></div>
    
    <div>
        <span>请输入充值金额</span>
        <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
    </div>
    <div id="dvpaytype" runat="server" style="display:none">  <span> 请选择支付方式</span>
      <asp:RadioButtonList runat="server" ID="rbPayType" AutoPostBack="true" OnSelectedIndexChanged="rbPayType_OnSelectedIndexChanged" >
          <asp:ListItem Value="1">微信</asp:ListItem>
          <asp:ListItem Value="2">支付宝</asp:ListItem>
      </asp:RadioButtonList>
     <div id="dvWx" runat="server" style="display:none">
        <div style="margin-left: 10px;color:#00CD00;font-size:30px;font-weight: bolder;">请扫描支付</div><br/>
	<asp:Image ID="Image2" runat="server" style="width:200px;height:200px;"/>
    <asp:Image ID="imgDescription" runat="server" style="width:200px;" ImageUrl="~/Style/images/WXPayDes.png"/>

        </div>

           <div id="dvAli" runat="server" style="display:none">

              <asp:Label runat="server" ID="payReturn" ></asp:Label>
        支付宝支付

        </div>


    </div>
    <div>
        <asp:Button ID="btnPay" runat="server" Text="立即支付" OnClientClick="return doPay();" />
        <%--<asp:Button ID="btnPrivew" runat="server" Text="返回" OnClientClick="return doback();" />--%>
    </div>
    </div>
    </form>
</body>
</html>
