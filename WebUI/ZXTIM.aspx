<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZXTIM.aspx.cs" Inherits="WebUI.ZXTIM" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <link rel="shortcut icon" href="<%= ResolveUrl("~/Style/images/logoICON.ico")%>" />
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/assets/js/jquery1x.min.js")%>"></script>
    <link href="<%= ResolveUrl("~/Style/css/wap.css")%>" rel="stylesheet" />
    <link href="<%= ResolveUrl("~/Style/css/zxtim.css")%>" rel="stylesheet" />
    <script type="text/javascript">
        var _RootPath = "<%= ResolveUrl("~")%>";
        var _APIURL = "<%= APIURL%>";
        var _Token = "<%= Token%>";
        var _CustomerId = "<%= CustomerId%>";
        var _HeaderLogo = "<%= HeaderLogo%>";
        var _CustomerName = "<%= CustomerName%>";
        var _MessageTo = "<%= MessageTo%>";
    </script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ZXTIM.js")%>"></script>
    <title>乐聊</title>
</head>
<body>
    <form id="aspnetForm" runat="server" enctype="multipart/form-data" method="post">
        <div class="zxtim">
            <input type="checkbox" id="left_menu" style="display:none"/>
            <label for="left_menu" class="btn_menu"></label>
            <div class="zxtim_left">
                <div class="zxtim_left_info">
                	<div class="zxtim_left_info_div">
                    	<div class="headimg">
                            <div class="headimg_div" style="background-image:url(<%=HeaderLogo%>)"></div>
                    	</div>
                        <div class="user_info">
                        	<div class="uesr_name">
                            	<%=CustomerName%>
                            </div>
                            <div class="uesr_set">
                            	<div class="uesr_voice" title="关闭提示音"></div>
                                <a class="uesr_SetUp" href="<%= ResolveUrl("CustomerManagement/CustomerEdit.aspx")%>" title="修改资料" target="_blank"></a>
                            </div>
                        </div>
                    </div>
                    <div class="imSearch">
                    	<span class="Search_ico"></span>
                        <input type="text" placeholder="搜索：昵称/销售名称/雇主名称" oninput="imSearch(this.value);"/>
                        <div class="Search_list" id="Search_list">
                            <p class="tishi">请输入关键词搜索</p>
                        </div>
                    </div>
                </div>
                <div class="zxtim_left_tab">
                	<ul>
                    	<li class="on" onclick="zxtim_left_tab('Recent_contact',this)">最近联系</li>
                        <li onclick="zxtim_left_tab('All_contact',this)">全部联系人</li>
                    </ul>
                </div>
                <div class="contact_div">
                	<div id="Recent_contact" class="Recent_contact">
                    	<!--<ul class="contact_list">
                        	<li class="on">
                            	<div class="headimg">
                                    <div class="headimg_div" style="background-image:url(http://www.zhongxiaole.net/SPManager/UploadFolder/Image/201804/20180417114516.jpg)"></div>
                                </div>
                                <div class="user_info">
                                    <div class="uesr_name">
                                        王先生
                                    </div>
                                    <div class="uesr_message">
                                        你好
                                    </div>
                                    <div class="message_time">
                                        昨天
                                    </div>
                                </div>
                            </li>
                        </ul>-->
                    </div>
                    <div id="All_contact" class="Recent_contact All_contact" style="display:none">
                    	<!--<ul class="contact_list">
                        	<li class="on">
                            	<div class="headimg">
                                    <div class="headimg_div" style="background-image:url(http://www.zhongxiaole.net/SPManager/UploadFolder/Image/201804/20180417114516.jpg)"></div>
                                </div>
                                <div class="user_info">
                                    <div class="uesr_name">
                                        王先生
                                    </div>
                                </div>
                            </li>
                        </ul>-->
                    </div>
                </div>
            </div>
            <div class="zxtim_right">
            	<div class="im_div">
                	<div class="im_head">
                    	<span id="ChatName">正在获取</span>
                        <font class="Full_screen_ico" onclick="Full_screen()"></font>
                    </div>
                    <div class="im_content" id="im_content">
                    	<ul class="im_content_list" id="im_content_list">
                            <div class="sysmessage">正在获取聊天记录</div>
                        	<!--<li>
                                <div class="chat_user">
                                    <div class="headimg" style="background-image:url(http://www.zhongxiaole.net/SPManager/UploadFolder/Image/201804/20180417114516.jpg)"></div>
                                    <div class="chat_info">王先生<i>2018-06-29 10:07:57</i></div>
                                </div>
                                <div class="chat_text">你好</div>
                            </li>
                            <li class="chat_mine">
                                <div class="chat_user">
                                    <div class="headimg" style="background-image:url(http://www.zhongxiaole.net/SPManager/UploadFolder/Image/201804/20180417114516.jpg)"></div>
                                    <div class="chat_info MessageStatus0"><i>2018-06-29 10:07:57</i>王先生</div>
                                </div>
                                <div class="chat_text">你好</div>
                            </li>
                            <li class="chat_mine">
                                <div class="chat_user">
                                    <div class="headimg" style="background-image:url(http://www.zhongxiaole.net/SPManager/UploadFolder/Image/201804/20180417114516.jpg)"></div>
                                    <div class="chat_info MessageStatus1"><i>2018-06-29 10:07:57</i>王先生</div>
                                </div>
                                <div class="chat_text">你好</div>
                            </li>
                            <li class="chat_mine">
                                <div class="chat_user">
                                    <div class="headimg" style="background-image:url(http://www.zhongxiaole.net/SPManager/UploadFolder/Image/201804/20180417114516.jpg)"></div>
                                    <div class="chat_info MessageStatus2"><i>2018-06-29 10:07:57</i>王先生</div>
                                </div>
                                <div class="chat_img"><a target="_blank" href="http://www.zhongxiaole.net/SPManager/UploadFolder/Image/201804/20180417114516.jpg"><img src="http://www.zhongxiaole.net/SPManager/UploadFolder/Image/201804/20180417114516.jpg" /></a></div>
                            </li>-->
                        </ul>
                    </div>
                    <div class="im_input">
                        <ul class="im_input_list" id="facelist">
                            <li title="[微笑]"><img src="Style/images/zxtIM/face/0.gif"></li>
                            <li title="[嘻嘻]"><img src="Style/images/zxtIM/face/1.gif"></li>
                            <li title="[哈哈]"><img src="Style/images/zxtIM/face/2.gif"></li>
                            <li title="[可爱]"><img src="Style/images/zxtIM/face/3.gif"></li>
                            <li title="[可怜]"><img src="Style/images/zxtIM/face/4.gif"></li>
                            <li title="[挖鼻]"><img src="Style/images/zxtIM/face/5.gif"></li>
                            <li title="[吃惊]"><img src="Style/images/zxtIM/face/6.gif"></li>
                            <li title="[害羞]"><img src="Style/images/zxtIM/face/7.gif"></li>
                            <li title="[挤眼]"><img src="Style/images/zxtIM/face/8.gif"></li>
                            <li title="[闭嘴]"><img src="Style/images/zxtIM/face/9.gif"></li>
                            <li title="[鄙视]"><img src="Style/images/zxtIM/face/10.gif"></li>
                            <li title="[爱你]"><img src="Style/images/zxtIM/face/11.gif"></li>
                            <li title="[泪]"><img src="Style/images/zxtIM/face/12.gif"></li>
                            <li title="[偷笑]"><img src="Style/images/zxtIM/face/13.gif"></li>
                            <li title="[亲亲]"><img src="Style/images/zxtIM/face/14.gif"></li>
                            <li title="[生病]"><img src="Style/images/zxtIM/face/15.gif"></li>
                            <li title="[太开心]"><img src="Style/images/zxtIM/face/16.gif"></li>
                            <li title="[白眼]"><img src="Style/images/zxtIM/face/17.gif"></li>
                            <li title="[右哼哼]"><img src="Style/images/zxtIM/face/18.gif"></li>
                            <li title="[左哼哼]"><img src="Style/images/zxtIM/face/19.gif"></li>
                            <li title="[嘘]"><img src="Style/images/zxtIM/face/20.gif"></li>
                            <li title="[衰]"><img src="Style/images/zxtIM/face/21.gif"></li>
                            <li title="[委屈]"><img src="Style/images/zxtIM/face/22.gif"></li>
                            <li title="[吐]"><img src="Style/images/zxtIM/face/23.gif"></li>
                            <li title="[哈欠]"><img src="Style/images/zxtIM/face/24.gif"></li>
                            <li title="[抱抱]"><img src="Style/images/zxtIM/face/25.gif"></li>
                            <li title="[怒]"><img src="Style/images/zxtIM/face/26.gif"></li>
                            <li title="[疑问]"><img src="Style/images/zxtIM/face/27.gif"></li>
                            <li title="[馋嘴]"><img src="Style/images/zxtIM/face/28.gif"></li>
                            <li title="[拜拜]"><img src="Style/images/zxtIM/face/29.gif"></li>
                            <li title="[思考]"><img src="Style/images/zxtIM/face/30.gif"></li>
                            <li title="[汗]"><img src="Style/images/zxtIM/face/31.gif"></li>
                            <li title="[困]"><img src="Style/images/zxtIM/face/32.gif"></li>
                            <li title="[睡]"><img src="Style/images/zxtIM/face/33.gif"></li>
                            <li title="[钱]"><img src="Style/images/zxtIM/face/34.gif"></li>
                            <li title="[失望]"><img src="Style/images/zxtIM/face/35.gif"></li>
                            <li title="[酷]"><img src="Style/images/zxtIM/face/36.gif"></li>
                            <li title="[色]"><img src="Style/images/zxtIM/face/37.gif"></li>
                            <li title="[哼]"><img src="Style/images/zxtIM/face/38.gif"></li>
                            <li title="[鼓掌]"><img src="Style/images/zxtIM/face/39.gif"></li>
                            <li title="[晕]"><img src="Style/images/zxtIM/face/40.gif"></li>
                            <li title="[悲伤]"><img src="Style/images/zxtIM/face/41.gif"></li>
                            <li title="[抓狂]"><img src="Style/images/zxtIM/face/42.gif"></li>
                            <li title="[黑线]"><img src="Style/images/zxtIM/face/43.gif"></li>
                            <li title="[阴险]"><img src="Style/images/zxtIM/face/44.gif"></li>
                            <li title="[怒骂]"><img src="Style/images/zxtIM/face/45.gif"></li>
                            <li title="[互粉]"><img src="Style/images/zxtIM/face/46.gif"></li>
                            <li title="[心]"><img src="Style/images/zxtIM/face/47.gif"></li>
                            <li title="[伤心]"><img src="Style/images/zxtIM/face/48.gif"></li>
                            <li title="[猪头]"><img src="Style/images/zxtIM/face/49.gif"></li>
                            <li title="[熊猫]"><img src="Style/images/zxtIM/face/50.gif"></li>
                            <li title="[兔子]"><img src="Style/images/zxtIM/face/51.gif"></li>
                            <li title="[ok]"><img src="Style/images/zxtIM/face/52.gif"></li>
                            <li title="[耶]"><img src="Style/images/zxtIM/face/53.gif"></li>
                            <li title="[good]"><img src="Style/images/zxtIM/face/54.gif"></li>
                            <li title="[NO]"><img src="Style/images/zxtIM/face/55.gif"></li>
                            <li title="[赞]"><img src="Style/images/zxtIM/face/56.gif"></li>
                            <li title="[来]"><img src="Style/images/zxtIM/face/57.gif"></li>
                            <li title="[弱]"><img src="Style/images/zxtIM/face/58.gif"></li>
                            <li title="[草泥马]"><img src="Style/images/zxtIM/face/59.gif"></li>
                            <li title="[神马]"><img src="Style/images/zxtIM/face/60.gif"></li>
                            <li title="[囧]"><img src="Style/images/zxtIM/face/61.gif"></li>
                            <li title="[浮云]"><img src="Style/images/zxtIM/face/62.gif"></li>
                            <li title="[给力]"><img src="Style/images/zxtIM/face/63.gif"></li>
                            <li title="[围观]"><img src="Style/images/zxtIM/face/64.gif"></li>
                            <li title="[威武]"><img src="Style/images/zxtIM/face/65.gif"></li>
                            <li title="[奥特曼]"><img src="Style/images/zxtIM/face/66.gif"></li>
                            <li title="[礼物]"><img src="Style/images/zxtIM/face/67.gif"></li>
                            <li title="[钟]"><img src="Style/images/zxtIM/face/68.gif"></li>
                            <li title="[话筒]"><img src="Style/images/zxtIM/face/69.gif"></li>
                            <li title="[蜡烛]"><img src="Style/images/zxtIM/face/70.gif"></li>
                            <li title="[蛋糕]"><img src="Style/images/zxtIM/face/71.gif"></li>
                        </ul>
                    	<div class="im_input_head">
                        	<span class="im_input_biaoqing" id="im_input_biaoqing" title="表情" onclick="face()"></span>
                            <span class="im_input_img" title="发送图片"><input type="file" name="file"></span>
                            <span class="im_input_fine" title="发送文件"><input type="file" name="file"></span>
                            <font onclick="Record()">历史纪录</font>
                        </div>
                        <textarea class="im_textarea" name="im_textarea" id="im_textarea"></textarea>
                        <div class="im_input_sub">
                        	<span>enter发送，ctrl+enter换行</span>
                            <button type="button" class="im_input_btn" onclick="Send()">发送</button>
                        </div>
                    </div>
                </div>
                <div class="Records_div" id="Records_div">
                    <div class="Records_list" id="Records_list">
                        <div class="sysmessage">正在加载 . . .</div>
                    </div>
                    <div class="loading_text">正在加载更多</div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
