<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="DemandList.aspx.cs" Inherits="WebUI.DemandList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="Scripts/SiteDataJS.js"></script>
    <script type="text/javascript" src="Scripts/RequireManagement/DemandData.js"></script>
    <div class="lmiddle">
    	<div id="divSort" class="lsort Demandlsort">
        	<button type="button" class="addDemandbtn"  onClick="Quick_release('Quick_release_demand')">发布您的需求</button>
        </div>
        <div class="newformalert_content" id="demandoffer">
             <div class="close" onclick="Newform_close('demandoffer')"></div>	
             <div class="newformalert_content_text">
             <!--弹窗内容-->
                  <div class="newformalert_title">立即留言</div>
                  <div class="newformalert_demand">
                       <p>请输入你的名称</p>
                       <input name="OfferName" value=""/>
                       <p>请输入你的手机号码，方面雇主联系你</p>
                       <input name="OfferPhone" value=""/>
                       <p>描述</p>
                       <textarea name="OfferDescription"></textarea>
                       <input type="hidden" name="OfferDemandId" value="0"/>
                  </div>
                  <div class="newformalert_btn" onclick="savebtn()">确定</div>
             </div>
        </div>
        <div class="lAgency_list">
        	<div class="lsearchall">
            	<div class="lsearchall_li">
                    <label>需求分类：</label>
                    <div class="lsearchall_info">
                    	<p><select id="Quick_demand_class" class="drpCategory"><option value="0">全部</option></select></p>
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
        	<ul class="lAgency_lists Demand_lists">
                <div id="divList">
						
                </div>
                <div class="lloading" id="lloading">正在加载更多数据</div>
            </ul>
        </div>
    </div>
</asp:Content>
