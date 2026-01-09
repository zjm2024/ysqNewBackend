window.onload = function () {
    //document.getElementById("kefu").setAttribute("href", "tencent://message/?uin=2144426401&amp;site=qq&amp;menu=yes");

    //document.getElementById("go-top").addEventListener("click", function () {
        //$("html,body").animate({ scrollTop: 0 }, 500);
        //return false;
   // });

    var cityLocation = window.headerTopInfo.gLocalizedCityName || '全国';
    $("#lblCurrentCity").html(cityLocation);
    $("#lblCurrentCity2").html("当前城市：" + cityLocation);
    MyMessageList();//调用投标和项目消息弹窗

};
window.onscroll = function () {
    var c = document.documentElement.clientWidt;//手机不显示top
    if (c >= 780) {
        var t = document.documentElement.scrollTop || document.body.scrollTop;
        if (t > 0) {
            document.getElementById("header").setAttribute("class", "section g-header sticky-edge");


            $(".go-top").fadeIn();
            //document.getElementById("go-top").style.display = "block";
        }
        else {
            document.getElementById("header").setAttribute("class", "section g-header");


            $(".go-top").fadeOut();
            //document.getElementById("go-top").style.display = "none";
        }
    }

};
window.headerTopInfo = {
    gLocalizedCityId: gGetCookie('local_city_id'),
    gLocalizedCityName: gGetCookie('local_city_name')
}

function gGetCookie(name) {
    var value = document.cookie;
    var start = value.indexOf(" " + name + "=");
    if (start == -1) {
        start = value.indexOf(name + "=");
        if (start > 0) {
            return null;
        }
    }
    if (start == -1) {
        return null;
    }
    start = value.indexOf("=", start) + 1;
    var end = value.indexOf(";", start);
    if (end == -1) {
        end = value.length;
    }
    return unescape(value.substring(start, end));
}
function setCookie(name, value, expires) {
    if (document.location.protocol == 'https') {
        document.cookie = name + "=" + escape(value) + "; path=/;secure" +
        ((expires == null) ? "" : "; expires=" + expires.toGMTString());
    }
    else {
        document.cookie = name + "=" + escape(value) + "; path=/" +
        ((expires == null) ? "" : "; expires=" + expires.toGMTString());
    }
}

function delCookie(name) {
    document.cookie = name + "=; expires=Thu, 01-Jan-70 00:00:01 GMT" + "; path=/";
}

function changeCity(cityId, cityName) {
    if (cityId == -1) {
        delCookie("local_city_id");
        delCookie("local_city_name");
    } else {
        var exp = new Date();
        exp.setTime(exp.getTime() + (1000 * 60 * 60 * 24 * 100));
        setCookie("local_city_id", cityId, exp);
        setCookie("local_city_name", cityName, exp);
    }
    window.location.href = window.location.href;
}
