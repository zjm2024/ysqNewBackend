/**
 * provide some utility function for Windows.
 * @author James Lee
 * @date created 2007-4-29
 * @from CMS David Yan & others
 * 
 * Function List:
 * 1 , function getBrowserPlatform()
 *		: check the browser is MSIE or SAFARI,return "safari","ie","firefox";
 * 
 * 2 , function openWindow(theURL,winName,features), function openWindowOnCenter(htmlurl,vheight,vwidth)
 *		: open a window for target link,void return;
 * 
 * 3 , function openDialogOnCenter(htmlurl,vheight,vwidth)
 *		: open a dialog for target link,return a value;
 * 
 * 4 , function getCookie(Name), function setCookie(name, value, daysExpire)
 *		: get or set the cookie. 
 *
 * 5 , function copyit(maintext)
 *      : copy text to clipboard
 *
 * 6 , function getEvent()
 *		: get event for different browse.
 * 
 * 7 , function getEventInFireFox(eventType)
 *		: get event only for Firefox,eventType: Event; MouseEvent.
 *
 * 8 , function getMouseEvent()
 *		: get mouse event for different browse.
 * 
 * 9 , function getMousePosTop(e), function getMousePosLeft(e)
 *      : get mouse top/left position
 * 
 * 10, function findPosX(obj), function findPosY(obj)
 *       :get html element x/y position 
 */


function getBrowserPlatform()
{
	if (navigator.appVersion.indexOf("Safari")!=-1) return "safari"; //navigator.userAgent
	return navigator.appVersion.indexOf("MSIE")!=-1? "ie" : "firefox"; 
}

function getBrowser() {
    if (navigator.userAgent.indexOf("Safari") != -1) {
        if (navigator.userAgent.indexOf("Chrome") != -1) {
            return "chrome"
        }
        return "safari"; //navigator.userAgent
    }
    else if (navigator.userAgent.indexOf("Trident") != -1 || navigator.userAgent.indexOf("MSIE") != -1) {
        return "ie";
    }
    else if (navigator.userAgent.indexOf("Firefox") != -1) {
        return "firefox";
    }
    else {
        return "other";
    }
}

function getInternetExplorerVersion() {
    var u = window.navigator.userAgent.toLocaleLowerCase(),
        msie = /(msie) ([\d.]+)/,
        chrome = /(chrome)\/([\d.]+)/,
        firefox = /(firefox)\/([\d.]+)/,
        safari = /(safari)\/([\d.]+)/,
        opera = /(opera)\/([\d.]+)/,
        ie11 = /(trident)\/([\d.]+)/,
        b = u.match(msie) || u.match(chrome) || u.match(firefox) || u.match(safari) || u.match(opera);
    if (b == null && u.match(ie11)) {
        b = u.match(ie11);
        return { NAME: b[1], VERSION: parseInt(b[2]) + 4 };
    }
    return { NAME: b[1], VERSION: parseInt(b[2]) };
}

//IE8浏览器兼容，link Clike事件兼容
function LinkReBind() {
    $("a").each(function () {
        var $this = $(this);
        if ((typeof ($this[0].onclick) != "function") && ($this[0].onclick != null) && (typeof ($this[0].onclick) == "string")) {
            $this.button().on("click", function () {
                return eval($this[0].onclick.replace("return", ""));
            });
        }
    });
}

//IE8浏览器兼容，Radio Clike事件兼容
function RadioReBind() {
    $("input[type='radio']").each(function () {
        var $this = $(this);
        if ((typeof ($this[0].onclick) != "function") && ($this[0].onclick != null) && (typeof ($this[0].onclick) == "string")) {
            $this.button().on("click", function () {
                return eval($this[0].onclick.replace("return", ""));
            });
        }
    });
}

//IE8浏览器兼容，Img Clike事件兼容
function ImgReBind() {
    $("img").each(function () {
        var $this = $(this);
        if ((typeof ($this[0].onerror) != "function") && ($this[0].onerror != null) && (typeof ($this[0].onerror) == "string")) {
            $this.error(function () {
                eval($this[0].onerror.replace("return", ""));
                //this.src = "./App_Themes/Style01/images/defaultpic.png";
                //this.onerror = null;
            });
        }
        if ((typeof ($this[0].onclick) != "function") && ($this[0].onclick != null) && (typeof ($this[0].onclick) == "string")) {
            $this.button().on("click", function () {
                return eval($this[0].onclick.replace("return", ""));
            });
        }
    });
}

//IE8浏览器兼容，Button Clike事件兼容
function ButtonReBind() {
    $("button").each(function () {
        var $this = $(this);
        if ((typeof ($this[0].onclick) != "function") && ($this[0].onclick != null) && (typeof ($this[0].onclick) == "string")) {
            $this.button().on("click", function () {
                return eval($this[0].onclick.replace("return", ""));
            });
        }
    });
}

//IE9以下浏览器兼容，placeholder兼容
function PlaceholderFix()
{
    if (getInternetExplorerVersion().VERSION < 10) {
        $('input, textarea').placeholder();
    }
}

//Date格式转string格式
function DateConvertToString(datetime)
{
    var year = datetime.getFullYear();
    var month = datetime.getMonth() + 1;
    if (month.length == 1)
    {
        month = "0" + month;
    }
    var day = datetime.getDate();
    if (day.length == 1) {
        day = "0" + day;
    }
    var result = year + "-" + month + "-" + day;
    if (result != "1900-01-01")
    {
        return result;
    }
    return "";
}

//string格式转Date格式
function StringConvertToDate(str)
{
    if (getInternetExplorerVersion().VERSION < 9) {
        var array = str.split('-');
        var date = new Date(parseInt(array[0]), parseInt(array[1]), parseInt(array[2]));
        return date;
    }
    return new Date(str);
}

//dropdownlist同一生成方法
//function onGetDrpSuccess(result, typeObj, value) {
//    if (result.error != undefined) {
//        alert(result.error.Message);
//        load_hide();
//    } else {
//        var typeList = result.value;
//        typeObj.empty();
//        if (typeList.length == 0) {
//            return;
//        }
//        else {
//            for (var i = 0; i < typeList.length; i++) {
//                $("<option value='" + typeList[i].Value + "'>" + typeList[i].Key + "</option>").appendTo(typeObj);
//            }
//        }
//        typeObj.val(value);
//    }
//}

function onAjaxCallFailed(result) {
    alert(result.error.Message);
    load_hide();
}
//function getInternetExplorerVersion()
//{
//    var rv = -1; // Return value assumes failure.
//    var ua = navigator.userAgent;
//    if (getBrowser() == 'ie') {
//        var re = new RegExp("MSIE ([0-9]{1,}[/.0-9]{0,})");
//        if (re.exec(ua) != null)
//            rv = parseFloat(RegExp.$1);
//    }
//    else if (getBrowser() == 'firefox') {
//        var re = new RegExp("MSIE ([0-9]{1,}[/.0-9]{0,})");
//        if (re.exec(ua) != null)
//            rv = parseFloat(RegExp.$1);
//    }
//    return rv;
//} 

function openWindow(theURL,winName,features) 
{ 
	window.open(theURL,winName,features);
}

//open page on page center
function openWindowOnCenter(htmlurl,vheight,vwidth)
{
	window.open(htmlurl,'new','height=' + vheight + ',width=' + vwidth + ',left='+(screen.width/2-250)+',top='+(screen.height/2-140)+',toolbar=no,menubar=no,scrollbars=yes,resizable=yes,location=no,status=no');
}
//open Dialog on page center and return value
function openDialogOnCenter(htmlurl,vheight,vwidth)
{
	strFeatures ="dialogWidth=" + vwidth + "px;dialogHeight=" + vheight + "px;center:yes;edge:raised;scroll:yes;status:no;unadorned:yes;help:no";
	var sValue = showModalDialog(htmlurl,window,strFeatures);
	return sValue;
}

//to get or set the cookie
function getCookie(Name) {
	var search = Name + "="
	if (document.cookie.length > 0) { // if there are any cookies
		offset = document.cookie.indexOf(search)
		if (offset != -1) { // if cookie exists
			offset += search.length
			// set index of beginning of value
			end = document.cookie.indexOf(";", offset)
			// set index of end of cookie value
			if (end == -1)
				end = document.cookie.length
			return unescape(document.cookie.substring(offset, end))
		}
        else
		{
		    return "";
		}
	}
}

function setCookie(name, value, daysExpire) {
	if(daysExpire) {
		var expires = new Date();
		expires.setTime(expires.getTime() + 1000*60*60*24*daysExpire.expires);
	}
	document.cookie = name + "=" + escape(value) + (daysExpire == null ? "" : (";expires=" + expires.toGMTString())) + ";path=/";
}


 function copyit(maintext) {
ie = (document.all)? true:false;
	if(ie) {
		if (window.clipboardData) {
			text1=window.clipboardData.setData("Text",maintext);
			text2=window.clipboardData.getData("Text");
		}
	}
   else if (window.netscape) {
	   //modify by james lee, 2007 04 13
      try{	   
		  netscape.security.PrivilegeManager.enablePrivilege('UniversalXPConnect');
		  }
	  catch (e)  
		{ 
			var message ="The Copy&Paste is forbidden due to your firefox security settings.\n"+
			  "Resolve:\n  step1: input \"about:config\" in the address bar to display the item list;\n"+
			  "  step2: double-click the \"signed.applets.codebase_principal_support\" item to set true in the list above;\n"+
			  "  step3: retry.\n"; 
			//alert(message);
			Message_Show('',message,'alert');
		  return false; 
		} 
		  

      var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
      if (!clip) return;
      var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
      if (!trans) return;
      trans.addDataFlavor('text/unicode');
      var str = new Object();
      var len = new Object();
      var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
      var copytext=maintext;
      str.data=copytext;
      trans.setTransferData("text/unicode",str,copytext.length*2);
      var clipid=Components.interfaces.nsIClipboard;
      if (!clip) return false;
      clip.setData(trans,null,clipid.kGlobalClipboard);
      return true;
   }
//   else {
//    alert("This function is not  available for your browse!");
//   }
   return false;
}

function getEvent()
{
	if (getBrowserPlatform()!="firefox")
	{
		return window.event;
	}
    func=getEvent.caller;
    while(func!=null)
    {
        var arg0=func.arguments[0];
        if(arg0)
        {
            if(arg0.constructor==Event) //if it is event object
                return arg0;
        }
        func=func.caller;
    }
    return null;
}

function getEventInFireFox(eventType)
{
    func=getEventInFireFox.caller;
    while(func!=null)
    {
        var arg0=func.arguments[0];
        if(arg0)
        {
            if(arg0.constructor==eventType) //if it is event object
                return arg0;
        }
        func=func.caller;
    }
    return null;
}


function getMouseEvent()
{
	if (getBrowserPlatform()!="firefox")
	{
		return window.event;
	}
    func=getMouseEvent.caller;
    while(func!=null)
    {
        var arg0=func.arguments[0];
        if(arg0)
        {
            if(arg0.constructor==MouseEvent) //if it is event object
                return arg0;
        }
        func=func.caller;
    }
    return null;
}

function getMousePosTop(e){
		var t=e.offsetTop;
		while(e=e.offsetParent){
			t+=e.offsetTop;
		}
		return t;
}

function getMousePosLeft(e){
		var l=e.offsetLeft;
		while(e=e.offsetParent){
			l+=e.offsetLeft;
		}
		return l;
}

//get html element x/y position
function findPosX(obj)
{
	var curleft = 0;
	if (obj.offsetParent)
	{
		while (obj.offsetParent)
		{
			curleft += obj.offsetLeft
			obj = obj.offsetParent;
		}
	}
	else if (obj.x)
		curleft += obj.x;
	return curleft;
}
function findPosY(obj)
{
	var curtop = 0;
	if (obj.offsetParent)
	{
		while (obj.offsetParent)
		{
			curtop += obj.offsetTop
			obj = obj.offsetParent;
		}
	}
	else if (obj.y)
		curtop += obj.y;
	return curtop;
}

function addEventHanler(hanler)
{
    if (window.addEventListener)
        window.addEventListener("load", hanler, false);
     else if (window.attachEvent)
        window.attachEvent("onload", hanler);
     else
        window.onload=hanler;
}

function onAjaxCallFailed(result) {
    alert('Call Ajax failed');
    load_hide();
};


function checkAll(obj, tableId) {
    $("#" + tableId).find("input[type='checkbox']").prop("checked", obj.checked);
}