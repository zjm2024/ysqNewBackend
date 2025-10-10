<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="360fun.aspx.cs" Inherits="WebUI.Card._360fun" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta charset="UTF-8">
    <title>360度全景图</title>
    <style>
        html,body{ margin:0; padding:0;}
        .container{ text-align: center;
    line-height: 94vh;
    font-size: 5vw;
    font-weight: bold;
    color: #121212;
    text-shadow: -0.5vw 0.5vw 0.5vw #6a6a6a;}
    </style>
    <!-- 页面关键词-->
<script>

//必须在服务器上才能看到效果！
window.onload=function(){
    getTitleHeight();
    loadingAllImg();
}
//让全景图刚好撑满屏幕
var canvasHeight;
function getTitleHeight(){
    var titleHeight = 0;
    var maxHeight=window.innerHeight;
    canvasHeight=parseFloat(maxHeight-titleHeight)+'px';
}
//全景图参数配置函数
function loadingAllImg(){
    var div = document.getElementById('container');
    var PSV = new PhotoSphereViewer({
        // 全景图的完整路径
        panorama: '<%=imgurl%>',

        // 放全景图的元素
        container: div,

        // 可选，默认值为2000，全景图在time_anim毫秒后会自动进行动画。（设置为false禁用它）
        time_anim: <%=time_anim%>,

        // 可选值，默认为false。显示导航条。
        navbar: <%=navbar%>,

        // 可选，默认值null，全景图容器的最终尺寸。例如：{width: 500, height: 300}。
        size: {
            width: '100%',
            height: canvasHeight
        }
    });
}
</script>
    <script src="../Scripts/three.min.js"></script>
    <script src="../Scripts/photo-sphere-viewer.min.js"></script>
    <!---->
    <!--[if IE]>
        <script src="http://libs.useso.com/js/html5shiv/3.7/html5shiv.min.js"></script>
    <![endif]-->
</head>
<body>
        <div id="container" class="container"></div>
</body>
</html>