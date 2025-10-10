<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CustomerRechange.aspx.cs" Inherits="WebUI.Pay.CustomerRechange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/assets/js/jquery1x.min.js")%>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/CustomerManagement/CustomerRechange.js")%>"></script>
    <script type="text/javascript" language="javascript">     

    </script>
    <div>



        <div class="form-group">
            <label class="col-sm-2 control-label no-padding-right need">请输入充值金额 </label>

            <div class="col-sm-9">
                <asp:textbox id="txtAmount" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
            </div>
        </div>
        <div class="form-group">
            <asp:label runat="server" id="Label1"></asp:label>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label no-padding-right">请选择支付类型:</label>
            <div class="col-sm-9">
                <label>
                    <input name="payType" class="ace" type="radio" id="radWx" checked="checked" />
                    <span class="lbl"></span>
                </label>
                <img src="../Style/images/pc_wxqrpay.png" style="width: 150px;" />
                <label>
                    <input name="payType" class="ace" type="radio" id="radAli" />
                    <span class="lbl"></span>
                </label>
                <img src="../Style/images/alipaypcnew.png" style="width: 150px;" />
            </div>
        </div>
                                 

        <div class="form-group" id="dvWx" runat="server" style="display: none">
            <div style="margin-left: 10px; color: #00CD00; font-size: 30px; font-weight: bolder;">请扫描支付</div>
            <br />
            <asp:image id="Image2" runat="server" style="width: 200px; height: 200px;" />
            <asp:image id="imgDescription" runat="server" style="width: 200px;" imageurl="~/Style/images/WXPayDes.png" />

        </div>

<%--        <div class="form-group" id="dvAli" runat="server">
             <label class="col-sm-2 control-label no-padding-right">测试结果和信息:</label>
            <div class="col-sm-9">              
                <asp:label runat="server" id="lblpayReturn" style="color:red" ></asp:label>
            </div>
        </div>--%>



        <div style="height: 100px"></div>
        <div class="clearfix form-actions">
            <div class="col-sm-5" style="width: 700px; text-align: center;">
                <asp:button runat="server" class="wtbtn savebtn" id="btn_save" text="立即支付" onclientclick=" return BtnPay_clientClick()" onclick="BtnPay_Click" />
                <%--  <button class="wtbtn savebtn" runat="server" type="button" id="btn_save" title="立即支付"  onserverclick="BtnPay_Click" >
                    立即支付
                </button>--%>
                <%--<asp:button runat="server" class="wtbtn savebtn" id="btn_test" text="测试支付验证" onclick="Btntest_Click" />--%>
            </div>
        </div>


    </div>


</asp:Content>
