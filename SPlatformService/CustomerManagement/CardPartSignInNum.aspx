<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardPartSignInNum.aspx.cs" Inherits="SPlatformService.CustomerManagement.CardPartSignInNum" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
       <script type="text/javascript" src="../Scripts/CustomerManagement/CardPartSignInNumJS.js"></script>
       <div class="search-condition" style=" display: table; padding: 20px 0; height:auto;">        
            <table class="wtright" style="height: 100%; float: left; line-height:28px;">
                
                <%for (int i = 0; i < PCostList.Count; i++) {  %>
                    <tr>     
                        <td style="padding-left:80px;">
                           购买类型：<%=PCostList[i].CostName %>
                        </td>          
                        <td style="padding-left:80px;">
                           收入金额：<%=PCostList[i].Cost %>元
                        </td>
                         <td style="padding-left:80px;">
                           报名人数：<%=PCostList[i].People %>
                        </td>
                         
                    </tr>
                <%} %>
                <tr style="color:chocolate">               
                    <td style="padding-left:80px;">
                       全部总计：<%=PCostList.Count %>项
                    </td>
                     <td style="padding-left:80px;">
                         总计金额：<%=PEarning %>元
                    </td>
                     <td style="padding-left:80px;">
                         总计人数：<%=SignupOfPeople %>/<%=PnumberOfPeople %>
                    </td>
                </tr>
            </table> 
         </div>
        <div class="search-condition" style="margin-top:10px;">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group" style="margin-top: 10px;">                    
                    <label class="col-sm-1 control-label no-padding-right">添加随机报名</label>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtRequirementName" runat="server" CssClass="col-xs-12 col-sm-12" MaxLength="30"  ToolTip="请输入添加的人数" placeholder="请输入添加的人数"></asp:TextBox>
                    </div>
                    <div>
                        <button class="wtbtnsearch" type="button" id="btn_search" title="添加" onclick="return OnSearch();">
                            添加
                        </button>
                        <button class="wtbtn" type="button" id="btn_search2" title="添加" onclick="return getExcel();" style="margin-left:20px;">
                            导出Excel
                        </button>
                    </div>
                    
                </div>
            </div>
        </div>
        
    </div>
    <div class="search-condition" style="margin-top:10px;">
    <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group" style="margin-top: 10px;">                    
                    <label class="col-sm-1 control-label no-padding-right">添加浏览次数</label>
                    <div class="col-sm-3">
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="col-xs-12 col-sm-12" MaxLength="30"  ToolTip="请输入添加的次数" placeholder="请输入添加的次数"></asp:TextBox>
                    </div>
                    <div>
                        <button class="wtbtnsearch" type="button" id="btn_search3" title="添加" onclick="return OnSearch2();">
                            添加
                        </button>
                    </div>
                    
                </div>
            </div>
        </div>
   </div>
    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="PartSignList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="PartSignListDiv"></div>
        </div>
    </div>
    <div class="form-group">
        <h4 class="header">邀约人数排行
        </h4>
    </div>
    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="PartyInviterList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="PartyInviterListDiv"></div>
        </div>
    </div>
    <div class="form-group">
        <h4 class="header">访问列表
        </h4>
    </div>
    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="CardAccessRecordsViewList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="CardAccessRecordsViewListDiv"></div>
        </div>
    </div>
     <asp:HiddenField ID="PartID" runat="server" />
</asp:Content>
