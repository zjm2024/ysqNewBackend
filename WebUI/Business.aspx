<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="Business.aspx.cs" Inherits="WebUI.Business" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="Scripts/RequireManagement/BusinessDetail.js"></script>
    <div class="lmiddle">
         <div class="lshow">
         	<div class="lshow_title">
            	<div class="lshow_title_text">
                    <div class="d1">雇主资料</div>
                    <div class="d2">
                        <p class="p1">细心从每一个小细节开始</p>
                        <p class="p2">Details of the employer</p>
                    </div>
                </div>
                <div class="lshow_share">
                    <div class="bdsharebuttonbox"><a href="#" class="bds_more" data-cmd="more"></a><a href="#" class="bds_weixin" data-cmd="weixin" title="分享到微信"></a><a href="#" class="bds_sqq" data-cmd="sqq" title="分享到QQ好友"></a><a href="#" class="bds_tsina" data-cmd="tsina" title="分享到新浪微博"></a><a href="#" class="bds_tqq" data-cmd="tqq" title="分享到腾讯微博"></a><a href="#" class="bds_renren" data-cmd="renren" title="分享到人人网"></a></div>
    <script>window._bd_share_config={"common":{"bdSnsKey":{},"bdText":"","bdMini":"2","bdMiniList":false,"bdPic":"","bdStyle":"0","bdSize":"24"},"share":{},"image":{"viewList":["weixin","sqq","tsina","tqq","renren"],"viewText":"分享到：","viewSize":"16"},"selectShare":{"bdContainerClass":null,"bdSelectMiniList":["weixin","sqq","tsina","tqq","renren"]}};with(document)0[(getElementsByTagName('head')[0]||body).appendChild(createElement('script')).src='static/api/js/share.js?v=89860593.js?cdnversion='+~(-new Date()/36e5)];
    </script>
				</div>
            </div>
            <div class="lshow_content">
            	<div class="lshow_content_title">
                	<div class="lshow_content_title_left">基本信息</div>
                    <div class="lshow_content_title_right">
                        <a class="line" title="咨询" href="ZXTIM.aspx?MessageTo=<%=businessVO.CustomerId %>" target="_blank"></a>
                    </div>
                </div>
                <div class="lshow_content_text">
                	<div class="headimg">
                    	<div class="headimg_img">
                        	<div class="img" style="background-image:url(<%=headImg%>)"></div>
                        </div>
                        <div class="headimg_info" id="divBusinessReview" runat="server">
                        	<div><span>工作态度：<font>0</font></span><span>诚信度：<font>0</font></span></div>
                            <div><span>沟通技巧：<font>0</font></span><span>客户关系：<font>0</font></span></div>
                            <div><span>打单能力：<font>0</font></span><span>综合得分：<font>0</font></span></div>
                        </div>
                    </div>
                	<ul class="info_list">
                    	<li class="name"><div class="d2"><%=businessVO.CompanyName %></div></li>
                        <%if (businessVO.Phone != ""){%><li><div class="d1">联系方式：</div><%if(CustomerId!=0) {%><div class="d2"><%=businessVO.Phone %></div><%}else {%><div class="d2 newform_red">请登录后查看</div><%}%></li><%}%>
                        <li><div class="d1">所在区域：</div><div class="d2"><%=businessVO.CityName %></div></li>
                        <%if (businessVO.CategoryNames != ""){%><li class="mt"><div class="d1">所属行业：</div><div class="d2"><%=RemoveComma(businessVO.CategoryNames) %></div></li><%}%>
                        <!--<li><div class="d1">创建日期：</div><div class="d2"><%=businessVO.SetupDate.ToString("yyyy-MM-dd") %></div></li>-->
                        <%if (businessVO.Address != ""){%><li><div class="d1">办公地址：</div><div class="d2"><%=businessVO.Address %></div></li><%}%>
                        <%
                            string CompanySite = businessVO.CompanySite;
                            if(CompanySite.IndexOf("http")>-1)
                            {
                                CompanySite = "<a href='"+CompanySite+"' target='_blank' class='link'>"+CompanySite+"</a>";
                            }
                            else
                            {
                                CompanySite = "<a href='http://"+CompanySite+"' target='_blank' class='link'>"+CompanySite+"</a>";
                            }
                        %>
                        <%if (businessVO.CompanyType != ""){%><li><div class="d1">企业性质：</div><div class="d2"><%=businessVO.CompanyType %></div></li><%}%>
                        <%if (businessVO.CompanySite != ""){%><li><div class="d1">公司官网：</div><div class="d2"><%=CompanySite %></div></li><%}%>
                        <%if (businessVO.Description != ""){%><li><div class="d1">雇主简介：</div><div class="d2"><%=Description %></div></li><%}%>
                        <div class="mt">
                        <%if (businessVO.TargetCity != ""){%><li><div class="d1">目标区域：</div><div class="d2"><%=RemoveComma(businessVO.TargetCity) %></div></li><%}%>
                        <%if (businessVO.TargetCategory != ""){%><li><div class="d1">目标行业：</div><div class="d2"><%=RemoveComma(businessVO.TargetCategory) %></div></li><%}%>
                        <li><div class="d1">成交数量：</div><div class="d2"><%=businessVO.ProjectCount.ToString() %></div></li>
                        </div>
                    </ul>
                </div>
                <%if (oTR != ""){%>
                <div class="lshow_content_title on">
                	<div class="lshow_content_title_left">客户案例</div>
                </div>
                <div class="lshow_content_text">
                    <table id="BusinessClientList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <thead>
                             <tr class="ui-jqgrid-labels">
                                <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                    <div class="ui-jqgrid-sortable" title="客户名称">客户名称</div>
                                </th>
                                <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                    <div class="ui-jqgrid-sortable" title="公司名称">公司名称</div>
                                </th>
                                <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                    <div class="ui-jqgrid-sortable" title="详情">详情</div>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        <%=oTR %>
                        </tbody>
                    </table>
                </div>
                <%}%>
                <%if (businessVO.MainProducts != ""){%>
                <div class="lshow_content_title on">
                	<div class="lshow_content_title_left">企业简介</div>
                </div>
                <div class="lshow_content_text">
                    <%=businessVO.MainProducts %>
                </div>
                <%}%>
                <%if (businessVO.ProductDescription != ""){%>
                <div class="lshow_content_title on">
                	<div class="lshow_content_title_left">产品介绍</div>
                </div>
                <div class="lshow_content_text">
                    <%=businessVO.ProductDescription %>
                </div>
                <%}%>

                <%if (rTR != ""){%>
                <div class="lshow_content_title on">
                	<div class="lshow_content_title_left">他的任务</div>
                </div>
                <div class="lshow_content_text">
                    <table id="BusinessClientList2" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <thead>
                             <tr class="ui-jqgrid-labels">
                                <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                    <div class="ui-jqgrid-sortable" title="任务标题">任务标题</div>
                                </th>
                                <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                    <div class="ui-jqgrid-sortable" title="详情">任务详情</div>
                                </th>
                                 <th class="ui-state-default ui-th-column ui-th-ltr" style="width:80px;">
                                    <div class="ui-jqgrid-sortable" title="操作">操作</div>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        <%=rTR %>
                        </tbody>
                    </table>
                </div>
                <%}%>
                <%if (dTR != ""){%>
                <div class="lshow_content_title on">
                	<div class="lshow_content_title_left">他的需求</div>
                </div>
                <div class="lshow_content_text">
                    <table id="BusinessClientList3" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <thead>
                             <tr class="ui-jqgrid-labels">
                                <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                    <div class="ui-jqgrid-sortable" title="详情">需求详情</div>
                                </th>
                                 <th class="ui-state-default ui-th-column ui-th-ltr" style="width:80px;">
                                    <div class="ui-jqgrid-sortable" title="操作">操作</div>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        <%=dTR %>
                        </tbody>
                    </table>
                </div>
                <%}%>
            </div>
         </div>
    </div>
</asp:Content>
