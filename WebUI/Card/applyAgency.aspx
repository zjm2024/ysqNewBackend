<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="applyAgency.aspx.cs" Inherits="WebUI.Card.applyAgency" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>申请认证</title>
</head>
<body>
    <form id="form1" method="post" action="applyAgency_ok.aspx">
    <div class="container">
		<div class="contact">
			<div class="text1">CONTACT</div>
			<div class="text2">/联系方式</div>
		</div>
		<div class="li">
			<div class="text">您要认证的类型</div>
			<div class="input-wrap">
				<div class="input"><%=priceclass %></div>
			</div>
		</div>
		<div class="li">
			<div class="text">您的姓名</div>
			<div class="input-wrap">
				<input  placeholder="点击此处填写您的姓名" class="input" name="name"/>
			</div>
		</div>
		<div class="li">
			<div class="text">您的手机号码</div>
			<div class="input-wrap">
				<input type="number" placeholder="点击此处填写您的手机号" class="input" name="phone"//>
			</div>
		</div>
		<div class="li">
			<div class="text">您的公司名称</div>
			<div class="input-wrap">
				<input  placeholder="点击此处填写您的公司名称" class="input"  name="corporateName"//>
			</div>
		</div>
		<div class="li">
			<div class="text">您的职位</div>
			<div class="input-wrap">
				<input  placeholder="点击此处填写您的职位" class="input" name="position"//>
			</div>
		</div>
		<div class="li">
			<div class="text">您的留言</div>
			<div class="input-wrap">
				<input placeholder="点击此处填写您的备注或留言" class="input" name="content"//>
			</div>
		</div>
		<div>
            <input name="Type" value="<%=Type %>" type="hidden"/>
            <input name="CustomerId" value="<%=CustomerId %>" type="hidden"/>
            <input name="Token" value="<%=Token %>" type="hidden"/>
            <button class="btn" type="submit">提交</button>
		</div>
		<div class="tip">
			<div class="iconfont iconicon_tips"></div>
			信息提交成功后，48小时内会有工作人员跟您联系，请保持手机畅通
		</div>
	</div>
        <style>
text,
scroll-view,
swiper,
button,
form,
input,
textarea,
label,
navigator,
image,
div {
 box-sizing: border-box;
 border: none;
}
body,html{ margin:0; padding:0}
            .container {
    min-height: 100vh;
    background-color: #fff;
    padding: 0 8vw;
    overflow: hidden;
}

.container .contact {
    color: #808080;
    display: flex;
    align-items: baseline;
    margin: 6.2vw 0;
}

.container .contact .text1 {
    font-size: 5.7vw;
    font-weight: bold;
    line-height: 5.7vw;
}

.container .contact .text2 {
    font-size: 3.2vw;
    font-weight: 400;
}

.container .li {
    margin-bottom: 6.2vw;
}

.container .li .text {
    font-size: 3.7vw;
    line-height: 3.7vw;
    margin-bottom: 3.2vw;
    color: #666;
}

.container .li .text .status {
    display: inline;
    font-size: 3.7vw;
    color: #ee4848;
}

.container .li .input-wrap {
    display: flex;
    align-items: center;
    box-shadow: 0 0 0 1px #ccc;
}

.container .li .input {
    flex: 1;
    height: 10.7vw;
    line-height: 10.7vw;
    color: #333;
    font-size: 4.3vw;
    padding-left: 2.8vw;
    overflow: hidden;
    text-overflow: ellipsis;
    display: -webkit-box;
    -webkit-line-clamp: 1;
    -webkit-box-orient: vertical;

}

.container .li .input.true {
    color: #ccc;
}

.oper {
    height: 10.7vw;
    padding: 0 2.5vw;
    color: #0066b3;
    line-height: 10.7vw;
    font-size: 3.2vw;
    background-color: #fff;
    display: inline-block;
}

.oper:active {
    color: #999;
}

.input-placeholder {
    color: #ccc;
}

.container .li .input-placeholder {
    color: #ccc;
}

.container .btn {
    width: 100%;
    height: 10.7vw;
    line-height: 10.7vw;
    font-size: 4.3vw;
    color: #fff;
    text-align: center;
    background: linear-gradient(180deg, #ce974a, #885e24);
    margin-bottom: 6.2vw;
}

.btn-hover .btn {
    background: #885e24;
}

.container .tip {
    font-size: 3.2vw;
    color: #999999;
    line-height: 4.8vw;
}

.container .tip .iconfont {
    display: inline;
    font-size: 2.8vw;
}
        </style>
    </form>
</body>
</html>
