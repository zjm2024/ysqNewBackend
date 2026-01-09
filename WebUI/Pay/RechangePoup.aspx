<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="RechangePoup.aspx.cs" Inherits="WebUI.Pay.RechangePoup" %>

<%@ MasterType VirtualPath="~/Shared/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/assets/js/jquery1x.min.js")%>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ProjectManagement/RechangePoup.js")%>"></script>


    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="clearfix form-actions"></div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need" style="font-size: 15px">请输入充值金额:</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtAmount" runat="server" TextMode="Number" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                    </div>
                </div>
                <div style="height: 15px"></div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right" >请选择支付类型:</label>

                    <div class="col-sm-9">
                        <label>
                            <input name="payType" class="ace" type="radio" id="radWx" checked="checked" />
                            <span class="lbl"></span>
                        </label>
                        <img src="../Style/images/pc_wxqrpay.png" style="width: 200px;" />
                        <label>
                            <input name="payType" class="ace" type="radio" id="radAli" />
                            <span class="lbl"></span>
                        </label>
                        <img src="../Style/images/alipaypcnew.png" style="width: 200px;" />
                    </div>
                </div>

                <div class="form-group" id="dvWx" runat="server" style="display: none">
                    <div style="margin-left: 10px; color: #00CD00; font-size: 30px; font-weight: bolder;">请扫描支付</div>
                    <br />
                    <asp:Image ID="Image2" runat="server" Style="width: 200px; height: 200px;" />
                    <asp:Image ID="imgDescription" runat="server" Style="width: 200px;" ImageUrl="~/Style/images/WXPayDes.png" />

                </div>

                <div class="form-group" id="dvAli" runat="server" style="display: none">

                    <asp:Label runat="server" ID="payReturn"></asp:Label>


                </div>



                <div style="height: 100px"></div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width: 700px; text-align: center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="保存">
                            保存
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                            返回
                        </button>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
