<%@ Page Language="C#"  MasterPageFile="~/Shared/MasterPageSite.Master"  AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WebUI.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="Scripts/indexJS.js"></script>
        <div class="flexslider">
            <ul class="slides">
                <!--<li style="background:url(Style/images/newpc/banner1.jpg) 50% 0 no-repeat;"><a class="banner1_btn" href="ZXLDescription.aspx" target="_blank"></a></li>
                <li style="background:url(Style/images/newpc/banner2.jpg) 50% 0 no-repeat;"></li>-->
                <li style="background:url(Style/images/newpc/banner3.jpg) 50% 0 no-repeat;"><a class="banner3_btn" href="ZXLDescription.aspx" target="_blank"></a></li>
            </ul>
        </div>
        <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.cxscroll.min.js")%>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.flexslider-min.js")%>"></script>
        <script type="text/javascript">
            $(document).ready(function(){
               $('.flexslider').flexslider({
                   directionNav: true,
                   pauseOnAction: false
               });
            });
        </script>
        <div class="ldemand">
    	    <div class="lmiddle">
                <div class="lleft">
                    <img src="Style/images/newpc/7.jpg"/>
                    <img src="Style/images/newpc/2.jpg"/>
                </div>
                <div class="lright">
            	    <div class="ltitle">最新商机需求<a class="lmode" href="DemandList.aspx">查看更多</a></div>
                    <div id="pic_list_3" class="scroll_vertical">
                        <div class="demand_show box">
                            <ul class="lindex_demand_list list" id="lindex_demand_list" runat="server">
                	            <li>
                    	            <p>广东省广州市华顺青为有限公司采购电脑一批</p>
                     	            <span>发布时间：一分钟前</span><span class="lr">收到报价：1条</span>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <script>
                        $('#pic_list_3').cxScroll({
	                        direction: 'bottom',
	                        speed: 500,
	                        time: 1500,
	                        plus: false,
	                        minus: false
                        });
                    </script>
                </div>
            </div>
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
        <div class="lindex_title">
    	    <div class="lmiddle">
                <p>最新任务</p>
                <span>THE LATEST MISSION</span>
                <a class="lmode" href="RequireList.aspx">查看更多</a>
            </div>
        </div>
        <div class="lindex_Require_list">
    	    <div class="lmiddle" id="divRequireList" runat="server">
                <ul>
                    <li>
                	    <a href="#">
                    	    <div class="img" style="background-image:url(Style/images/newpc/3.jpg)"></div>
                            <div class="laddess">广州市</div>
                            <div class="ltitle">
                        	    广州威尔特胶原姬面膜（已在京东和苏宁销售）诚招全国各地县级市代理
                            </div>
                            <div class="ltitle_right">
                        	    未托管
                            </div>
                            <div class="lcost">&yen;500,000</div>
                            <div class="lbtn">免费咨询</div>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
        <div class="lindex_title">
    	    <div class="lmiddle">
                <p>优秀销售</p>
                <span>TOP SALES</span>
                <a class="lmode" href="AgencyList.aspx">查看更多</a>
            </div>
        </div>
        <div class="lindex_Agency_list">
    	    <div class="lmiddle" id="divAgencyList" runat="server">
        	    <ul>
            	    <li>
                	    <a href="#">
                    	    <div class="img" style="background-image:url(Style/images/newpc/4.jpg)"></div>
                            <div class="info">
                        	    <p class="p1"><span class="s1">徐先生</span><span class="s2">广州市</span><span class="s3">评分：5.73</span></p>
                                <p class="p2">销售总监</p>
                                <p>优势客户：肇庆农业局</p>
                                <p>擅长产品：IT、电器类</p>
                                <p>优势行业：不限</p>
                                <p>已成交：3</p>
                                <p class="p3">酬金收入&nbsp;￥100.00<span class="lbtn">查看详情</span></p>
                            </div>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
        <div class="lsection">
            <div class="lmiddle">
        	    <div class="lli lleft">
            	    <div class="img"><img src="Style/images/newpc/5.jpg" /></div>
                    <div class="info">
                	    <p class="p1">成功案列</p>
                	    <p class="p2"><span id="txtProjectCount" runat="server">100个</span></p>
                    </div>
                </div>
                <div class="lli lright">
            	    <div class="img"><img src="Style/images/newpc/6.jpg" /></div>
                    <div class="info">
                	    <p class="p1">酬金发放</p>
                	    <p class="p2"><span id="txtTotalCommission" runat="server">50万元</span></p>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
