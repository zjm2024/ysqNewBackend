<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="FinancialManagement.aspx.cs" Inherits="SPlatformService.UserManagement.FinancialManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
      <%--<script type="text/javascript" src="../Scripts/UserManagement/FinancialManagementJS.js"></script>--%>
     <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">平台总金额 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtTotal" runat="server" CssClass="col-xs-10 col-sm-5" ReadOnly="true" ></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">用户钱包总额 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtTotalBalance" runat="server" CssClass="col-xs-10 col-sm-5" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">平台酬金总提成 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtProjectCommission" runat="server" CssClass="col-xs-10 col-sm-5" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>                

                
            </div>
        </div>
    </div>

</asp:Content>
