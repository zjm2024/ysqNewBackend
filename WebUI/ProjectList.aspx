<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="ProjectList.aspx.cs" Inherits="WebUI.ProjectList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="Scripts/SiteDataJS.js"></script>
    <script type="text/javascript" src="Scripts/ProjectManagement/ProjectData.js"></script>
    <div class="section landscape" data-spm="nav">
            <div class="panel-search">
                <div id="divAgency" class="main-search">
                    <div class="city-search">
                        <label>行政区域</label>
                        <select id="drpProvince"  runat="server">
                        </select>
                        <select id="drpCity" runat="server"></select>
                        <%--</div>
                    <div class="city-search">--%>
                        <label>行业类别</label>
                        <select id="drpParentCategory" runat="server"></select>
                        <select id="drpCategory" runat="server"></select>
                    </div>
                    <div class="city-search">
                        <input class="input-search" type="text"  runat="server" placeholder="输入关键词搜索" id="txtSearcha"/>
                        <button id="btnSearch">搜索</button>
                    </div>
                </div>
            </div>
            <%--<div class="panel-condition">
                <div class="div-sxlist">
                    
                </div>
            </div>--%>
        </div>
        <div class="panel-data">
            <div class="div-data">
                <div id="divSort">
                    <ul>
                        <li sortname="1" sorttype="asc" class="sort mr-project"><a href="#" onclick="return onSortClick(this);">综合</a></li>
                        <%--<li sortname="2" sorttype="asc"><a href="#" onclick="return onSortClick(this);">成交量</a></li>
                        <li sortname="3" sorttype="asc"><a href="#" onclick="return onSortClick(this);">好评率</a></li>--%>
                        <li sortname="5" sorttype="asc"  class="mr-project"><a href="#" onclick="return onSortClick(this);">酬金</a></li>
                        <li sortname="4" style="width: 200px;">
                            <label>酬金</label>
                            <input type="text" id="pricestart" />-<input type="text" id="priceend" />
                            <button class="btn-primary" onclick="return onSortClick(this);">确定</button>
                        </li>
                    </ul>
                </div>
                <div id="divList" runat="server">
                    <%--<div class="sign-data">
                        <div class="fl">
                            <div class="price">￥2000元</div>
                            <div class="title">
                                <a target="_blank" title="大型网站开发" href="Require.aspx?requireId=1">大型网站开发</a>
                            </div>
                            <br />
                            <div class="content">具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求</div>
                        </div>
                        <div class="fr">
                            <div class="status">
                                <p>已选定销售</p>
                                <p>交易成功</p>
                            </div>
                        </div>
                    </div>                    
                    
                    <div class="sign-data">
                        <div class="fl">
                            <div class="price">￥2000元</div>
                            <div class="title">
                                <a target="_blank" title="大型网站开发" href="Require.aspx?requireId=1">大型网站开发</a>
                            </div>
                            <br />
                            <div class="content">具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求具体要求</div>
                        </div>
                        <div class="fr">
                            <div class="status">
                                <p>5参与 | 招标10</p>
                                <p>招标3天后截止</p>
                            </div>
                        </div>
                    </div>--%>

                </div>
            </div>
            <div class="div-page" id="pageList" runat="server">
                <ul>
                    <li class="div-up"><a href="#" onclick="return onPageUp();">上一页</a></li>
                    <li class="div-up"><a href="#" onclick="return onPageDown();">下一页</a></li>
                </ul>                
            </div>
            <asp:HiddenField ID="hidDataCount" runat="server" />
        </div>
</asp:Content>