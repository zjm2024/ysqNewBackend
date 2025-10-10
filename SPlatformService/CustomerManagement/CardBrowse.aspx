<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardBrowse.aspx.cs" Inherits="SPlatformService.CustomerManagement.CardBrowse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardBrowseJS.js"></script>
    <div class="space-4"></div>
       <div class="search-condition">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group" style="margin-top: 10px;">       
                    <button class="wtbtn yjright" type="button" id="btn_new" onclick="return CardAdd();" style="float: left;margin-left: 10px;" title="新建">
                        <i class="icon-ok bigger-110"></i>
                        新建名片
                    </button>           
                    <label class="col-sm-1 control-label no-padding-right">关键字 </label>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtAgencyName" runat="server" CssClass="col-xs-12 col-sm-12" MaxLength="30"></asp:TextBox>
                    </div>
                    <div>
                        <button class="wtbtnsearch" type="submit" id="btn_search" title="查询" onclick="return OnSearch();">
                            查询
                        </button>
                        <span style="margin-left:20px;">
                            名片总数：<%=CardNum %>
                        </span>
                        <span style="margin-left:20px;">
                            用户总数：<%=CardCustomerNum %>
                        </span>
                        <span style="margin-left:20px;">
                            现有Vip：<%=VipNum %>
                        </span>
                        <span style="margin-left:20px;">
                            已过期Vip：<%=VipNum2 %>
                        </span>
                        <span style="margin-left:20px;display:none">
                            历史Vip总数：<%=VipNum3 %>
                        </span>
                         <span style="margin-left:20px;">
                            兑换Vip次数：<%=VipNum4 %>
                        </span>
                        <span style="margin-left:20px;">
                            活动名片：<%=PartyNum %>(<%=decimal.Round((decimal)PartyNum/(decimal)CardNum,2)*100  %>%)
                        </span>
                        <span style="margin-left:20px;">
                            表格名片：<%=QuestionnaireNum %>(<%=decimal.Round((decimal)QuestionnaireNum/(decimal)CardNum,2)*100 %>%)
                        </span>
                        <!--<span style="margin-left:20px;">马甲名片：<%=DummyNum %></span>-->
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="form-horizontal">
        <div class="hr hr-dotted"></div>
        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
            <table id="AgencyList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
            <div id="AgencyListDiv"></div>
        </div>
    </div>
    <asp:HiddenField ID="CustomerId" runat="server" />
</asp:Content>
