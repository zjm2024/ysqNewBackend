<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="AgencyList.aspx.cs" Inherits="WebUI.AgencyList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="Scripts/SiteDataJS.js"></script>
    <script type="text/javascript" src="Scripts/CustomerManagement/AgencyData.js"></script>
    <div class="lmiddle">
    	<div id="divSort" class="lsort">
        	<ul>
            	<li sortname="1" sorttype="desc" class="s1"><a href="#" onclick="return onSortClick(this);">综合</a></li>
                <li sortname="2" sorttype="desc" class="s2"><a href="#" onclick="return onSortClick(this);">好评率</a></li>
                <li sortname="3" sorttype="desc" class="sort s3"><a href="#" onclick="return onSortClick(this);">成交合同数量</a></li>
                <li sortname="4" sorttype="desc" class="s4"><a href="#" onclick="return onSortClick(this);">成交合同金额</a></li>
                <li sortname="5" sorttype="desc" class="s5"><a href="#" onclick="return onSortClick(this);">酬金收入总额</a></li>
            </ul>
        </div>
        <div class="lAgency_list">
        	<div class="lsearchall">
            	<div class="lsearchall_li">
                    <label>行政区域：</label>
                    <div class="lsearchall_info">
                    	<p><select id="drpProvince" runat="server"></select></p>
                        <p><select id="drpCity" runat="server"></select></p>
                    </div>
                </div>
                <div class="lsearchall_li">
                    <label>行业类别：</label>
                    <div class="lsearchall_info">
                        <p><select id="drpParentCategory" runat="server"></select></p>
                        <p><select id="drpCategory" runat="server"></select></p>
                    </div>
                </div>
                <div class="lsearchall_li">
                    <label>列表搜索：</label>
                    <div class="lsearchall_info">
                         <input class="input-search" type="text" runat="server" placeholder="输入关键词搜索" id="txtSearcha" />
                         <button id="btnSearch" class="btnSearch">搜索</button>
                    </div>
                </div>
            </div>
        	<ul class="lAgency_lists">
                <div id="divList">

                </div>
                <div class="lloading" id="lloading">正在加载更多数据</div>
            </ul>
        </div>
    </div>
</asp:Content>
