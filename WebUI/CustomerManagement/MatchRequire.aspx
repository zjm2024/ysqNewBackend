<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="MatchRequire.aspx.cs" Inherits="WebUI.CustomerManagement.MatchRequire" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/MatchRequireJS.js"></script>
   <%--<div class="space-4"></div>--%>
    <%--<div class="search-condition">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group" style="margin-top: 10px;">
                    <label class="col-sm-1 control-label no-padding-right">任务状态</label>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="drpStatus" runat="server" CssClass="col-xs-12 col-sm-12">
                            <asp:ListItem Text="--选择--" Value="-1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="保存（取消发布）" Value="0" ></asp:ListItem>
                            <asp:ListItem Text="发布" Value="1" ></asp:ListItem>
                            <asp:ListItem Text="关闭" Value="2" ></asp:ListItem>
                            <asp:ListItem Text="暂停投标" Value="3" ></asp:ListItem>
                            <asp:ListItem Text="已创建项目" Value="4" ></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <label class="col-sm-1 control-label no-padding-right">任务标题 </label>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtRequirementName" runat="server" CssClass="col-xs-12 col-sm-12" MaxLength="30"></asp:TextBox>
                    </div>
                    <div>
                        <button class="wtbtnsearch" type="submit" id="btn_search" title="查询" onclick="return OnSearch();">
                            查询
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>

    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="RequirementList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="RequirementListDiv"></div>
        </div>
    </div>
</asp:Content>
