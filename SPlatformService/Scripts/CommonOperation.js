jQuery.support.cors = true;
var Global_Token;
var Global_Version = "2.2.03"; 
var Is360;
var token = "";
var webApiServer = "";

//operation control
/**
 * @param {obj} 点击的元素
 * @param {type} 类型
  * @param {return} jq promise obj
*/
function takeOperationData(obj, type, name) {
    if (!obj) {
        obj = takeOperationData.caller.arguments.length > 0 ? takeOperationData.caller.arguments[0] : null;
        if(!obj)
            return;
        if (!!!name)
            name = $(obj).data("name");
    }
    
    var operationVO = {
        OperationName: name,  //IE9不支持HTML5特性dataset，使用jq方式兼容
        OperationType: type,
        Version: Global_Version,
        Browser:Is360
    };

    if (typeof Global_Token == "undefined") {
        Global_Token = token;
    }

    var postUrl = webApiServer + "Users/TakeDataOperation?" + $.param({ token: Global_Token });

    var operationDeferred = $.Deferred();
    $.when($.post(postUrl, operationVO)).always(
        function () {
            operationDeferred.resolve();

            if (type == 2) {
                var menuHref = $(obj).attr("href");
                if (!!menuHref) {
                    window.location.href = menuHref;
                }
            }
        }     
    );

    return operationDeferred.promise();
}

function DataOperation() {
    this.Operate = {
        Element: null,
        Type: 0,
        Name:"",
        Complete: null,
        Start: function () {
            var that = this;
            var srcObj = {
                Element: null,
                Type: 0,
                Name: ""
            };
            var targetObj = {
                Element: this.Element,
                Type: this.Type,
                Name:this.Name
            }
            jQuery.extend(srcObj, targetObj);

            if (srcObj.Element == null) {
                return false;  //简化
                //var parentFunc= arguments.callee.caller;
                //var Obj = parentFunc.arguments.length > 0 ? parentFunc.arguments[0] : null;
                //if(!Obj)
                //    return;
                //srcObj.Element = Obj;
                //srcObj.Name = $(Obj).data("name");
            }

            var operationVO = {
                OperationName: srcObj.Name,  
                OperationType: srcObj.Type,
                Version: Global_Version,
                Browser: Is360
            };

            if (typeof Global_Token == "undefined") {
                Global_Token = token;
            }

            var postUrl = webApiServer + "Users/TakeDataOperation?" + $.param({ token: Global_Token });

            var operationDeferred = $.Deferred();
            var xmlhttp = createxmlhttp();
            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 1) {
                    if (jQuery.isFunction(that.Complete)) {
                        that.Complete.call(that, srcObj.Element);
                    }
                    operationDeferred.resolve();                                    
                }
            };
            
            xmlhttp.open("post", postUrl);
            xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            xmlhttp.send($.param(operationVO));

            //$.when($.post(postUrl, operationVO)).always(
            //    function () {                   
            //        if (jQuery.isFunction(that.Complete))
            //        {
            //            alert(888);
            //            //that.Complete.call(that, srcObj.Element);
            //        }                    
            //        operationDeferred.resolve();
                    
            //    }
            //);
            return operationDeferred.promise();
        }
    }
}

//根据event获取触发的元素
function getTriggerEventObj(event) {
    if (!event) {
        return null;
    }
    var event = event ? event : window.event;
    var element = event.srcElement ? event.srcElement : event.target;
    return element;
}


$(document).ready(function () {

    //绑定一级菜单
    $("#sidebar").find(".menu-first").on("click", function () {
        var opeName = $(this).data("name");
        var type = 1;
        if (opeName.indexOf("首页")>-1) {
            type = 2;            
        }
        var dataOpe = new DataOperation();
        dataOpe.Operate.Element = this;
        dataOpe.Operate.Type = type;
        dataOpe.Operate.Name = opeName;
        dataOpe.Operate.Complete = function (obj) {
            if (type == 2) {
                var menuHref = $(obj).attr("href");
                if (!!menuHref) {
                    window.location.href = menuHref;
                }
            }           
        }
        dataOpe.Operate.Start();

        if (type == 2) {
            return false;
        }
    });

    //二级菜单
    $("#sidebar").find(".submenu > li > a").on("click", function (e) {
        var opeName = $(this).data("name");

        var dataOpe = new DataOperation();
        dataOpe.Operate.Element = this;
        dataOpe.Operate.Type = 2;
        dataOpe.Operate.Name = opeName;
        dataOpe.Operate.Complete = function (obj) {
            var menuHref = $(obj).attr("href");
            if (!!menuHref) {
                window.location.href = menuHref;
            }
        }
        dataOpe.Operate.Start();

        //takeOperationData(this, 2, opeName);
        //var href=$(this).attr("href");
        //setTimeout(function () {
        //    window.location.href = href;
        //}, 10);
        return false;
    });

    takeBtnOperation();
    Is360 = Is360Browser();
});


function takeBtnOperation() {
    $(".wfy-operationbtn").on("click", function (e) {
        var thisObj = $(this);
        var opeName = OperationPath + "-" + thisObj.data("name");//按钮路径及名称

        var dataOpe = new DataOperation();
        dataOpe.Operate.Element = this;
        dataOpe.Operate.Type = 3;
        dataOpe.Operate.Name = opeName;

        dataOpe.Operate.Start();

        //var promise = takeOperationData(this, 3,opeName);
        ////回调函数
        //promise.always(function () {           
        //    //to do something
        //});
        //return false;
    });

    $(".wfy-operationbtn-index").on("click", function (e) {
        var thisObj = $(this);
        var opeName = thisObj.data("name");//按钮路径及名称

        var dataOpe = new DataOperation();
        dataOpe.Operate.Element = this;
        dataOpe.Operate.Type = 3;
        dataOpe.Operate.Name = opeName;
        dataOpe.Operate.Complete = function (obj) {
            if (($(obj).hasClass("wfy-operationbtn-index"))) {
                var aElement = $(obj).find("a");
                if (aElement.length > 0) {
                    var btnHref = $(aElement[0]).attr("href");
                    window.location.href = btnHref;
                }
            }
        }
        dataOpe.Operate.Start();

        //var promise = takeOperationData(this, 3, opeName);
        ////回调函数
        //promise.always(function () {
        //    if ((thisObj.attr("class").indexOf("wfy-operationbtn-index") > -1)) {
        //        var aElement = thisObj.find("a");
        //        if (aElement.length > 0) {
        //            var btnHref = $(aElement[0]).attr("href");
        //            window.location.href = btnHref;
        //        }
        //    }
        //});
        return false;
    });

    $(".wfy-operationbtn-only").on("click", function (e) {
        var thisObj = $(this);
        var opeName = thisObj.data("name");//按钮路径及名称

        var dataOpe = new DataOperation();
        dataOpe.Operate.Element = this;
        dataOpe.Operate.Type = 3;
        dataOpe.Operate.Name = opeName;

        dataOpe.Operate.Start();

        //var promise = takeOperationData(this, 3, opeName);
        ////回调函数
        //promise.always(function () {
            
        //});
        return true;
    });
}

function Is360Browser() {
    this.isChromeIE = function () {
        var ua = window.navigator.userAgent.toLowerCase();
        return (ua.indexOf("chrome") > 1 || ua.indexOf("msie")>-1);
    }
    this._mime=function(option, value) {
        var mimeTypes = window.navigator.mimeTypes;
        for (var mt in mimeTypes) {
            if (mimeTypes[mt][option] == value) {
                return true;
            }
        }
        return false;
    }

    var is360 = this._mime("type", "application/vnd.chromium.remoting-viewer");
    if (this.isChromeIE() && is360) {
        return "360";
    }
    else {
        return null;
    }
}


function createxmlhttp() {
    var xmlhttp = false;
    try {
        xmlhttp = new XMLHttpRequest();
    } catch (trymicrosoft) {
        try {
            xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
        } catch (othermicrosoft) {
            try {
                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            } catch (failed) {
                xmlhttp = false;
            }
        }
    }
    return xmlhttp;
}



