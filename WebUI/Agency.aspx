<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="Agency.aspx.cs" Inherits="WebUI.Agency" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="Scripts/CustomerManagement/AgencyDetail.js"></script>
    <div class="lmiddle">
         <div class="lshow">
         	<div class="lshow_title">
            	<div class="lshow_title_text">
                    <div class="d1">个人资料</div>
                    <div class="d2">
                        <p class="p1">细心从每一个小细节开始</p>
                        <p class="p2">Personal resume</p>
                    </div>
                </div>
                <div class="lshow_share">
                    <div class="bdsharebuttonbox"><a href="#" class="bds_more" data-cmd="more"></a><a href="#" class="bds_weixin" data-cmd="weixin" title="分享到微信"></a><a href="#" class="bds_sqq" data-cmd="sqq" title="分享到QQ好友"></a><a href="#" class="bds_tsina" data-cmd="tsina" title="分享到新浪微博"></a><a href="#" class="bds_tqq" data-cmd="tqq" title="分享到腾讯微博"></a><a href="#" class="bds_renren" data-cmd="renren" title="分享到人人网"></a></div>
                    <script>
                        window._bd_share_config = {
                            "common": {
                                "bdSnsKey": {},
                                "bdText": "",
                                "bdMini": "2",
                                "bdMiniList": false,
                                "bdPic": "",
                                "bdStyle": "0",
                                "bdSize": "24",
                                "bdUrl":""
                            },
                            "share": {},
                            "image": {
                                "viewList": ["weixin", "sqq", "tsina", "tqq", "renren"],
                                "viewText": "分享到：",
                                "viewSize": "16"
                            },
                            "selectShare": {
                                "bdContainerClass": null,
                                "bdSelectMiniList": ["weixin", "sqq", "tsina", "tqq", "renren"]
                            }
                        };
                        with (document) 0[(getElementsByTagName('head')[0] || body).appendChild(createElement('script')).src = 'static/api/js/share.js?v=89860593.js?cdnversion=' + ~(-new Date() / 36e5)];
                    </script>
				</div>
            </div>
            <div class="lshow_content">
            	<div class="lshow_content_title">
                	<div class="lshow_content_title_left">基本信息</div>
                    <div class="lshow_content_title_right">
                    	<%=str %>
                        <a class="line" title="咨询" href="ZXTIM.aspx?MessageTo=<%=agencyVO.CustomerId %>" target="_blank"></a>
                        <button type="button" class="lshow_btn" onclick="return onTender('<%=agencyVO.CustomerId %>')">邀请接单</button>
                    </div>
                </div>
                <div class="lshow_content_text">
                	<div class="headimg">
                    	<div class="headimg_img">
                        	<div class="img" style="background-image:url(<%=headImg%>)"></div>
                        </div>
                        <div class="headimg_info" id="divAgencyReview" runat="server">
                        	<div><span>工作态度：<font>0</font></span><span>诚信度：<font>0</font></span></div>
                            <div><span>沟通技巧：<font>0</font></span><span>客户关系：<font>0</font></span></div>
                            <div><span>打单能力：<font>0</font></span><span>综合得分：<font>0</font></span></div>
                        </div>
                    </div>
                	<ul class="info_list">
                        <%
                            string AgencyName = "";
                            if (agencyVO.AgencyName.Length >= 2)
                            {
                                AgencyName = agencyVO.AgencyName.Substring(0, 2) + "***";
                            }
                            else {
                                AgencyName = agencyVO.AgencyName;
                            }
                             %>
                    	<li class="name"><div class="d2"><%=AgencyName + (agencyVO.Sex ? "先生" : "女士") %></div></li>
                        <%
                            int Age = agencyVO.Birthday.ToString("yyyy-MM-dd") == "1900-01-01" ? 0 : DateTime.Now.Year - agencyVO.Birthday.Year;
                        %>
                        <%if(Age>0) {%><li><div class="d1">年龄：</div><div class="d2"><%=Age%></div></li><%}%>
                        <%if (agencyVO.Phone != ""){%><li><div class="d1">联系方式：</div><%if(isBusiness) {%><div class="d2"><%=agencyVO.Phone %></div><%}else {%><div class="d2 newform_red">请登录并通过雇主认证才能查看</div><%}%></li><%}%>
                        <li class="mt"><div class="d1">销售级别：</div><div class="d2"><%=levelName %></div></li>
                        <%if (agencyVO.CityName != ""){%><li><div class="d1">居住城市：</div><div class="d2"><%=agencyVO.CityName %></div></li><%}%>
                        <%if (agencyVO.School != ""){%><li><div class="d1">毕业院校：</div><div class="d2"><%=agencyVO.School %></div></li><%}%>
                        <%if (agencyVO.Position != ""){%><li><div class="d1">现任职业：</div><div class="d2"><%=agencyVO.Position %></div></li><%}%>
                        <%if (agencyVO.ShortDescription != ""){%><li><div class="d1">一语简介：</div><div class="d2"><%=agencyVO.ShortDescription %></div></li><%}%>
                        <%if (agencyVO.Specialty != ""){%><li><div class="d1">兴趣特长：</div><div class="d2"><%=agencyVO.Specialty %></div></li><%}%>
                        <%if (agencyVO.Feature != ""){%><li><div class="d1">性格特征：</div><div class="d2"><%=agencyVO.Feature %></div></li><%}%>
                        <%if (agencyVO.Technical != ""){%><li><div class="d1">技能特长：</div><div class="d2"><%=agencyVO.Technical %></div></li><%}%>
                        <%if (agencyVO.TargetCategory != ""){%><li><div class="d1">优势行业：</div><div class="d2"><%=RemoveComma(agencyVO.TargetCategory) %></div></li><%}%>
                        <%if (agencyVO.TargetClient != ""){%><li><div class="d1">优势客户：</div><div class="d2"><%=RemoveComma(agencyVO.TargetClient) %></div></li><%}%>
                        <%if (agencyVO.FamiliarProduct != ""){%><li><div class="d1">擅长产品：</div><div class="d2"><%=agencyVO.FamiliarProduct %></div></li><%}%>
                        <%if (agencyVO.TargetCity != ""){%><li><div class="d1">优势区域：</div><div class="d2"><%=RemoveComma(agencyVO.TargetCity) %></div></li><%}%>
                    </ul>
                    <div class="moy"><span>成交合同数量：<font><%=agencyVO.ProjectCount.ToString() %></font></span><span>成交合同金额：<font><%=agencyVO.ProjectCost.ToString("N2") %>&yen;</font></span><span>酬金收入总额：<font><%=agencyVO.ProjectCommission.ToString("N2") %>&yen;</font></span></div>
                </div>
                <%if (oTR != ""){%>
                <div class="lshow_content_title on">
                	<div class="lshow_content_title_left">典型案例</div>
                </div>
                <div class="lshow_content_text">
                    <table id="AgencySolutionList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <thead>
                            <tr class="ui-jqgrid-labels">
                                <th class="ui-state-default ui-th-column ui-th-ltr">
                                   <div class="ui-jqgrid-sortable" title="客户名称">客户名称</div>
                                </th>
                                <th class="ui-state-default ui-th-column ui-th-ltr">
                                    <div class="ui-jqgrid-sortable" title="合同名称">合同名称</div>
                                </th>
                                <th class="ui-state-default ui-th-column ui-th-ltr">
                                   <div class="ui-jqgrid-sortable" title="合同金额">合同金额</div>
                                </th>
                                <th class="ui-state-default ui-th-column ui-th-ltr"> 
                                    <div class="ui-jqgrid-sortable" title="成交时间">成交时间</div> 
                                </th> 
                                <th class="ui-state-default ui-th-column ui-th-ltr">
                                    <div class="ui-jqgrid-sortable" title="案例证明">案例证明</div>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        <%=oTR %>
                        </tbody>
                    </table>
                </div>
                <%}%>
                 <%if (dTR != ""){%>
                <div class="lshow_content_title on">
                	<div class="lshow_content_title_left">他的需求</div>
                </div>
                <div class="lshow_content_text">
                    <table id="AgencySolutionList2" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
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
                <%if (agencyVO.Description != ""){%>
                <div class="lshow_content_title on">
                	<div class="lshow_content_title_left">详情简历</div>
                </div>
                <div class="lshow_content_text">
                    <%=agencyVO.Description %>
                </div>
                <%}%>
                
            </div>
         </div>
    </div>
</asp:Content>

