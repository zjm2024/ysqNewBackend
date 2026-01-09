<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExcelUP.aspx.cs" Inherits="WebUI.Card.ExcelUP" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/jquery.min.js")%>" type="text/javascript"></script>
    <title>导入Excel文件</title>
    <style>
        .content {
            padding-top:5rem;
            text-align:center;
            font-size:4vw;
        }
        .excelbtn {
            font-size:4vw;
            line-height:6vw;
            padding:5px;
            background:#3a8bd7;
            color:white;
            border-radius:4vw;
        }
        .excelbtn:active{background: #999;}
        .excelbtn.btn {
            width:60vw;
        }
        .excelbtn.btn1 {
            width:30vw;
        }
        .excelbtn.btn2 {
            margin-left:15%;
            float:left;
            width:15vw;
        }
        .excelbtn.btn2.btncancel {
            background:red;
        }
        
        .exceltitle {
            color:#999;
            width:30vw;
            text-align:left;
            padding-left:2vw;
            float:left;
            margin-top:10vw;
        }
        .exceltext {
            width:60vw;
            float:left;
            margin-top:10vw;
        }
        .btndiv {
            
        }
        select {
            width:21rem;
            height:5rem;
            font-size:4vw;
        }
        table {
            width:94%;
        }
        td {
            
            height:6rem;
            font-size:4vw;
        }
        .tdtop {
            width:30vw;
        }
        .td1 {
            color:#999;
            width:30vw;
            text-align:left;
            padding-left:1vw;
        }
        .td2 {
            width:60vw;
            text-align:right;
        }
        .td2 select{
            width:100%;
            text-align:right;
            border:none;
        }
        input[type=text] {
            width:60vw;
            height:5rem;
            font-size:4vw;
            border:none;
            text-align:right;
        }
        .ck {
            width:4rem;height:4rem;
        }
        .ckdiv {
            line-height:4rem;
        }
        .ServiceProvider_TitleList{ display: table; width: 100%; margin-bottom: 2vw; padding-left: 1vw;margin-top:10vw;}
        .ServiceProvider_TitleList .ServiceProvider_TitleLi{float: left; width: 24vw; text-align: center; font-size: 3.4vw; color: #999;}
        .ServiceProvider_TitleList .ServiceProvider_TitleLi .d{ width: 2vw; height: 2vw; margin: 0 auto; margin-bottom: 5vw; position: relative;}
        .ServiceProvider_TitleList .ServiceProvider_TitleLi .d::after{content: '';width: 2vw; height: 2vw; background: #84c6f3; border-radius: 2vw; position: relative; display:block}
        .ServiceProvider_TitleList .ServiceProvider_TitleLi .d::before {content: '';position: absolute;width: 22.5vw;height: 1px;background: #e9e9e9;transform-origin: 0 1px;transform: scaleY(0.3);box-sizing: border-box;bottom: 1vw;}
        .ServiceProvider_TitleList .ServiceProvider_TitleLi:last-child .d::before{ display: none;}

        .ServiceProvider_TitleList .ServiceProvider_TitleLi.on{color: #0693f1;}
        .ServiceProvider_TitleList .ServiceProvider_TitleLi.on .d{margin-bottom: 6vw; }
        .ServiceProvider_TitleList .ServiceProvider_TitleLi.on .d::after{background: #fff; border:solid 1vw #0693f1;  margin-top:-1vw ;margin-left:-1vw ;}
        .ServiceProvider_TitleList .ServiceProvider_TitleLi.on .d::before{bottom: 0vw;}

        .ServiceProvider_TitleList .ServiceProvider_TitleLi.on1 .d::after{ background: #0693f1; }
        .ServiceProvider_TitleList .ServiceProvider_TitleLi.on1 .d::before{background: #0693f1; }
    </style>
    <script type="text/javascript">
        $(function () {
            var nextindexs =<%=nextindex %>;
            $(".ServiceProvider_TitleLi").eq(nextindexs).siblings().removeClass("on");
            $(".ServiceProvider_TitleLi").eq(nextindexs).siblings().removeClass("on1");
            $(".ServiceProvider_TitleLi").eq(nextindexs).prevAll().addClass("on1");
            $(".ServiceProvider_TitleLi").eq(nextindexs).addClass("on");
        });
        function selTableChange(e) {
            var tableindex = e.selectedIndex;
            $("#td" + tableindex).css("display", "block");
            $("#td" + tableindex).siblings().css("display", "none");
        }
        function nextBtnComfig() {
            var nextindexs =<%=nextindex %>;
            if (nextindexs == 3) {
                return confirm('确定提交吗？');
            }
            return true;
        }
        //取消
        function cancelBtnComfig() {
            return confirm('您确定取消吗？');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
         <div class="ServiceProvider_TitleList">
             <div class="ServiceProvider_TitleLi ">
                 <div class="d"></div>
                 <div class="name">上传文件</div>
             </div>
             <div class="ServiceProvider_TitleLi ">
                 <div class="d"></div>
                 <div class="name">选择表</div>
             </div>
             <div class="ServiceProvider_TitleLi ">
                 <div class="d"></div>
                 <div class="name">输入行数</div>
             </div>
             <div class="ServiceProvider_TitleLi ">
                 <div class="d"></div>
                 <div class="name">选择字段</div>
             </div>
         </div>
         <div  class="content">
             <div >
                 <div style="line-height:7rem;height:70rem;">
                     <%=GetText() %>
                 </div>
                     
             </div>
             <div class="btndiv">
                <asp:Button ID="upBtn" Visible="false" runat="server"  CssClass="excelbtn btn2"  Text="上一步" OnClick="upBtn_Click" />
                <asp:Button ID="nextBtn" Visible="false" runat="server"  CssClass="excelbtn btn2"  Text="下一步" OnClick="nextBtn_Click" OnClientClick="return nextBtnComfig();" />
                <asp:Button ID="cancelBtn" Visible="false" runat="server"  CssClass="excelbtn btn2 btncancel"  Text="取消" OnClick="cancelBtn_Click" OnClientClick="return cancelBtnComfig();" />
                <asp:FileUpload ID="ExcelFileUpload" CssClass="excelbtn btn"  runat="server" />
                <asp:Button ID="UploadBtn" runat="server" CssClass="excelbtn btn1" Text="确定上传" OnClick="UploadBtn_Click" />
             </div>
         </div>
     </form>
</body>
</html>
