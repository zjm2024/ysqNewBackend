<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="RequireList.aspx.cs" Inherits="WebUI.RequireList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="Scripts/SiteDataJS.js"></script>
    <script type="text/javascript" src="Scripts/RequireManagement/RequireData.js"></script>
    <div class="lmiddle">
    	<div id="divSort" class="lsort RequireSort">
        	<ul>
                <li sortname="1" sorttype="desc" class="sort s9"><a href="#" onclick="return onSortClick(this);">综合</a></li>
                <li sortname="2" sorttype="desc" class="s10"><a href="#" onclick="return onSortClick(this);">好评率</a></li>
                <li sortname="3" sorttype="desc" class="s11"><a href="#" onclick="return onSortClick(this);">诚信度</a></li>
                <li sortname="4" sorttype="desc" class="s12"><a href="#" onclick="return onSortClick(this);">付款即时性</a></li>
                <!--<li sortname="5" sorttype="desc" class="s13"><a href="#" onclick="return onSortClick(this);">发布时间</a></li>-->
                <li sortname="6" sorttype="asc" class="s14"><a href="#" onclick="return onSortClick(this);">剩余时间</a></li>
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
                <div class="lsearchall_li">
                    <label>酬金范围：</label>
                    <div class="lsearchall_info">
                        <input type="text" id="commissionstart" /><font>-</font><input type="text" id="commissionend" />
                        <button class="btn-primary" onclick="return onSortClick(this);">确定</button>
                    </div>
                </div>
                <ul class="lsearchall_li"  id="conCommission">
                    <label>预估酬金：</label>
                    <div class="lsearchall_info">
                        <li class="color" filtertype="all"><a href="#" onclick="return onFilterClick(this);" style="width:30px;">全部</a></li>
                        <li filtertype="1"><a href="#" onclick="return onFilterClick(this);">1000元以下</a></li>
                        <li filtertype="2"><a href="#" onclick="return onFilterClick(this);">1000-10000元</a></li>
                        <li filtertype="3"><a href="#" onclick="return onFilterClick(this);">10000-10万元</a></li>
                        <li filtertype="4"><a href="#" onclick="return onFilterClick(this);">10万元以上</a></li>
                    </div>
                </ul>
                <ul class="lsearchall_li">
                    <label>任务状态：</label>
                    <div class="lsearchall_info"  id="conCreatedAt">
                        <li class="color" filtertype="all"><a href="#" onclick="return onFilterClick(this);" style="width:30px;">全部</a></li>
                        <li filtertype="1"><a href="#" onclick="return onFilterClick(this);">最新发布</a></li>
                        <li filtertype="2"><a href="#" onclick="return onFilterClick(this);">更早发布</a></li>
                        <li filtertype="3"><a href="#" onclick="return onFilterClick(this);">交易中</a></li>
                        <li filtertype="4"><a href="#" onclick="return onFilterClick(this);">已结束</a></li>
                    </div>
                </ul>
            </div>
            <ul class="lAgency_lists">
                <div id="divList">

                </div>
                <div class="lloading" id="lloading">正在加载更多数据</div>
            </ul>
        </div>
    </div>
</asp:Content>
