<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <script src="<%= ResolveUrl("~/Scripts/assets/js/jquery1x.min.js")%>" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <div>
        <input type="text" name="lname" onkeydown="add()"/>
    </div>
    
    <div id="text">

    </div>
        <script>
            var dd=[];
            function add(e) {
                var evt = window.event || e;
                if (evt.keyCode == 13) {
                    //回车事件
                    var objinput = $("input[name*='lname']");
                    var html = "";
                    var isr = false;
                    for (var i = 0; i < dd.length; i++) {
                        html = "<div>" + dd[i] + "</div>" + html;
                        if (dd[i] == objinput.val()) {
                            isr = true;
                        }
                    }
                    if (!isr) {
                        dd.push(objinput.val())
                        html = "<div>" + objinput.val() + "</div>" + html;
                    }
                    objinput.val("");
                    var div = $("div[id*='text']")
                    html = "<div>总计：" + dd.length + "</div>" + html;
                    div.html(html)
                }
                
            }
        </script>
</body>
</html>
