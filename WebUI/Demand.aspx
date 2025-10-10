<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="Demand.aspx.cs" Inherits="WebUI.Demand" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="Scripts/RequireManagement/DemandDetail.js"></script>
    <div class="lmiddle">
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
         <div class="lshow">
         	<div class="lshow_title">
            	<div class="lshow_title_text">
                    <div class="d1">需求详情</div>
                    <div class="d2">
                        <p class="p1">细心从每一个小细节开始</p>
                        <p class="p2">Details of demand</p>
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
                        <a class="line" title="咨询" href="ZXTIM.aspx?MessageTo=<%=DemandVO.CustomerId %>" target="_blank"></a>
                        <button type="button" class="lshow_btn" onclick="return Newform_demand_alert('<%=DemandVO.DemandId %>')">关注留言</button>
                    </div>
                </div>
                <div class="lshow_content_text">
                	<div class="headimg">
                    	<div class="headimg_img">
                        	<div class="img" style="background-image:url(<%=headImg%>)"></div>
                        </div>
                    </div>
                	<ul class="info_list">
                        <%
                            string CustomerName =DemandVO.CustomerName;
                             Match m = Regex.Match(CustomerName, "(1)\\d{10}");
                            if (m.Success) {
                                CustomerName = DemandVO.CustomerName.Substring(0, 3) + "****" + DemandVO.CustomerName.Substring(7, 4);
                            }
                             %>
                    	<li class="name"><div class="d2"><%=CustomerName%></div></li>
                        <%if (DemandVO.Phone != ""){%><li><div class="d1">联系方式：</div><%if(CustomerId!=0) {%><div class="d2"><%=DemandVO.Phone %></div><%}else {%><div class="d2 newform_red">请登录后查看</div><%}%></li><%}%>
                        <li><div class="d1">需求类别：</div><div class="d2"><%=DemandVO.CategoryName %></div></li>
                        <li><div class="d1">发布时间：</div><div class="d2"><%=DemandVO.CreatedAt %></div></li>
                        <li><div class="d1">截止时间：</div><div class="d2"><%=DemandVO.EffectiveEndDate %></div></li>
                        <li><div class="d1">收到留言：</div><div class="d2"><%=DemandVO.OfferCount %></div></li>
                    </ul>
                </div>
                <%if (DemandVO.Description != ""){%>
                <div class="lshow_content_title on">
                	<div class="lshow_content_title_left">需求详情</div>
                </div>
                <div class="lshow_content_text">
                    <%=DemandVO.Description %>
                </div>
                <%}%>
            </div>
         </div>
    </div>
</asp:Content>
