<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="Require.aspx.cs" Inherits="WebUI.Require" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="Scripts/RequireManagement/RequireDetail.js"></script>
    <div class="lmiddle">
         <div class="lshow">
         	<div class="lshow_title">
            	<div class="lshow_title_text">
                    <div class="d1">任务详情</div>
                    <div class="d2">
                        <p class="p1">细心从每一个小细节开始</p>
                        <p class="p2">Details of the task</p>
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
                    	<%=str%>
                    </div>
                </div>
                <div class="lshow_content_text">
                	<%if (requireVO.MainImg != ""){%>
                    <div class="headimg">
                    	<div class="headimg_img requireimg">
                        	<div class="img" style="background-image:url(<%=requireVO.MainImg%>)"></div>
                        </div>
                    </div>
                    <%}%>
                	<ul class="info_list">
                    	<li class="name"><div class="d2"><%=requireVO.Title %></div></li>
                        <li><div class="d1">酬金：</div><div class="d2"><%=requireVO.Commission.ToString("N2")%></div></li>
                        <%if (requireVO.CompanyName != ""){%><li class="mt"><div class="d1">雇主名称：</div><div class="d2"><a href="Business.aspx?businessId=<%=requireVO.BusinessId %>" target="_blank" class="link"><%=requireVO.CompanyName %></a></div></li><%}%>
                        <li><div class="d1">发布时间：</div><div class="d2"><%=requireVO.CreatedAt.ToString("yyyy-MM-dd") %></div></li>
                        <%if (requireVO.Phone != ""){%><li><div class="d1">联系方式：</div><%if(CustomerId!=0) {%><div class="d2"><%=requireVO.Phone %></div><%}else {%><div class="d2 newform_red">请登录后查看</div><%}%></li><%}%>
                        <%if (requireVO.RequirementCode != ""){%><li><div class="d1">任务编号：</div><div class="d2"><%=requireVO.RequirementCode %></div></li><%}%>
                        <%if (requireVO.CommissionDescription != ""){%><li><div class="d1">酬金说明：</div><div class="d2"><%=requireVO.CommissionDescription %></div></li><%}%>
                        <li><div class="d1">有效日期：</div><div class="d2"><%=requireVO.EffectiveEndDate.ToString("yyyy-MM-dd") %></div></li>
                        <li><div class="d1">托管状态：</div><div class="d2"><%=((requireVO.DelegationCommission > 0) ? "已托管酬金" : "未托管酬金") %></div></li>
                        <div class="mt">
                          <%if (divtargetCategoryStr != ""){%><li><div class="d1">目标客户行业：</div><div class="d2"><%=divtargetCategoryStr%></div></li><%}%>
                          <%if (divtargetCityStr != ""){%><li><div class="d1">目标客户区域：</div><div class="d2"><%=divtargetCityStr%></div></li><%}%>
                          <%if (requireVO.TargetAgency != ""){%><li><div class="d1">理想销售人员：</div><div class="d2"><%=requireVO.TargetAgency %></div></li><%}%>
                        </div>
                    </ul>
                </div>
                <%if (oTR != ""){%>
                <div class="lshow_content_title on">
                	<div class="lshow_content_title_left">目标客户</div>
                </div>
                <div class="lshow_content_text">
                    <table id="BusinessClientList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <thead>
                             <tr class="ui-jqgrid-labels">
                                <th class="ui-state-default ui-th-column ui-th-ltr">
                                    <div class="ui-jqgrid-sortable" title="客户名称">客户名称</div>
                                </th>
                                <th class="ui-state-default ui-th-column ui-th-ltr">
                                    <div class="ui-jqgrid-sortable" title="用户名称">用户名称</div>
                                </th>
                                <th class="ui-state-default ui-th-column ui-th-ltr">
                                    <div class="ui-jqgrid-sortable" title="任务现状">任务现状</div>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        <%=oTR %>
                        </tbody>
                    </table>
                </div>
                <%}%>

                <%if (fTR != ""){%>
                <div class="lshow_content_title on">
                	<div class="lshow_content_title_left">附件列表</div>
                </div>
                <div class="lshow_content_text">
                    <table id="BusinessClientList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <thead>
                             <tr class="ui-jqgrid-labels">
                                <th class="ui-state-default ui-th-column ui-th-ltr">
                                    <div class="ui-jqgrid-sortable" title="文件名称">文件名称</div>
                                </th>
                                <th class="ui-state-default ui-th-column ui-th-ltr">
                                    <div class="ui-jqgrid-sortable" title="操作">操作</div>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        <%=fTR %>
                        </tbody>
                    </table>
                </div>
                <%}%>
                <%if (requireVO.Description != ""){%>
                <div class="lshow_content_title on">
                	<div class="lshow_content_title_left">任务详情</div>
                </div>
                <div class="lshow_content_text">
                    <%=requireVO.Description %>
                </div>
                <%}%>
            </div>
         </div>
    </div>
</asp:Content>
