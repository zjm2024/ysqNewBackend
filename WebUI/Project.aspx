<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="Project.aspx.cs" Inherits="WebUI.Project" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <%--<script type="text/javascript" src="Scripts/SiteDataJS.js"></script>
    <script type="text/javascript" src="Scripts/ProjectManagement/ProjectDetail.js"></script>--%>
    <div class="page-content">
        <div class="infomation project-info-infomation">
            <div class="module-project-info">
                <div class="essential-information">
                    <div class="image-labeler">
                    </div>
                    <div class="text-area">
                        <h2 class="title" id="lblTitle" runat="server"></h2>
                        <div class="info-area">
                            <div class="info area-a">
                                <div class="icon-area name"><span class="icon-area-title">用户名称：</span><span  id="lblBusinessName" runat="server"></span></div>
                                <div class="icon-area  amount">任务编号：<span id="lblCommission" runat="server"></span></div>
                            </div>
                            <div class="info area-b">

                                <div class="icon-area type"><span class="icon-area-title">销售名称：</span><span  id="lblAgencyName" runat="server"></span></div>
                                <div class="icon-area time">成交时间：<span id="lblCreatedAt" runat="server"></span></div>
                            </div>
                            <div class="cb"></div>
                        </div>
                    </div>
                    <div class="cb"></div>
                </div>
                <div class="describe-area js-describe-area">
                    <h3 class="title">任务描述</h3>
                    <div class="describe">
                        <div class="js-describe-content" id="divDescription" runat="server">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>