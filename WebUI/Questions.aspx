<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="Questions.aspx.cs" Inherits="WebUI.Questions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <style>.left_qq{ display:none}</style>
    <div class="main">
            <div class="main-help">
                <div class="main-help-menu">
                    <ul class="sidebar">
                        <li>
                            <a href="AboutUS.aspx">关于我们</a>
                        </li>
                        <li>
                            <a href="NewHelp.aspx">新手指南</a>
                        </li>
                        <li>
                            <a href="Questions.aspx" class="on">常见问题 </a>
                        </li>
                        <li>
                            <a href="BusinessQuestions.aspx">雇主问题</a>
                        </li>
                        <li>
                            <a href="AgencyQuestions.aspx">销售问题</a>
                        </li>
                        <li>
                            <a href="Contract.aspx">联系我们</a>
                        </li>
                        <li>
                            <a href="PlatformRule.aspx">众销乐规则</a>
                        </li>
                         <li>
                            <a href="Recognizance.aspx">诚信保证金</a>
                        </li>
                         <li>
                            <a href="ZXLDescription.aspx">众销乐介绍</a>
                        </li>
                         <li>
                            <a href="Working.aspx">工作机会</a>
                        </li>
                        <li>
                            <a href="ComissionDelegation.aspx">酬金托管制</a>
                        </li>
                        <li>
                            <a href="GCDTGZZ.aspx">过程动态跟踪制</a>
                        </li>
                        <li>
                            <a href="BusinessHZ.aspx">商务合作</a>
                        </li>
                    </ul>
                </div>
                <div class="help-main">
                    <div class="help-hd">
                        <h2 class="help-tit">常见问题</h2>
                    </div>
                    <div class="help-bd" id="divNote" runat="server">
                        
                    </div>
                </div>
            </div>
        </div>
</asp:Content>