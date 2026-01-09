 /*  
 * @author James Lee
 * @date created 2007-6-27
 */
 var backgroundElement;// = document.createElement('div');
 var foregroundElement ;//= document.createElement('div');
 var Loading = "加载中";
 var G_LoadingInit = false;
 var G_LeftWidth =0;
 //var IsAppendToBody=false;
 function addEvent(element,eventName,eventMethod)
 {
    if(document.attachEvent)  element.attachEvent('on'+eventName,  eventMethod);  
   else  element.addEventListener(eventName,  eventMethod,  true);  
 }
 function removeEvent(element,eventName,eventMethod)
 {
    if(document.detachEvent)  element.detachEvent('on'+eventName,  eventMethod);  
   else  element.removeEventListener(eventName,  eventMethod,  true);  
 }
 function fireEvent(element,eventName)
 {
    if (document.createEventObject) 
     {
       element.fireEvent("on"+eventName);
     } else if (document.createEvent) 
    {
      var e = document.createEvent("HTMLEvents");
      e.initEvent(eventName, true, true);
      element.dispatchEvent(e);
    }    
 }
 var load_time = 0;
 function load_layout()
 {
  //debugger;
   var formuploadiframe = queryString('formuploadiframe');
// var scrollLeft = (document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft);
// var scrollTop = (document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop);
  var clientWidth;
  var clientHeight;
  var clientHeight1;
  var clientWidth1;
  var element = window;
// debugger;
       while(element.parent != element)
        {
            if(element.parent && element.parent.document.body.tagName!='BODY') //modified by ari. date:2007-08-23,reson:the frameset have no body,error occur when interate into the window.
              break;
            element = element.parent;            
        }
 
  var scrollLeft = (element.document.documentElement.scrollLeft ? element.document.documentElement.scrollLeft : element.document.body.scrollLeft);
  var scrollTop = (element.document.documentElement.scrollTop ? element.document.documentElement.scrollTop : element.document.body.scrollTop);
//debugger;
  if(navigator.userAgent.indexOf("MSIE")!=-1 || document.documentMode ==11)
  {
     clientWidth = element.document.documentElement.clientWidth;
	 clientHeight = element.document.documentElement.clientHeight;
	 if(formuploadiframe!= 1)
        {
            clientWidth = Math.max(element.parent.document.documentElement.clientWidth,clientWidth);
            clientHeight = Math.max(element.parent.document.documentElement.clientHeight,clientHeight);
        }

	 }
   else if(navigator.userAgent.indexOf("Firefox")!=-1||navigator.userAgent.indexOf("Safari")!=-1)
  {
    clientWidth = Math.min(element.innerWidth, element.document.documentElement.clientWidth);
	clientHeight = Math.min(element.innerHeight, element.document.documentElement.clientHeight);
	if(formuploadiframe!= 1)
        {
            clientWidth = Math.max(Math.min(element.parent.innerWidth, element.parent.document.documentElement.clientWidth),clientWidth);
            clientHeight = Math.max(Math.min(element.parent.innerHeight,element.parent.document.documentElement.clientHeight),clientHeight);
        }
  }  
   var scrollWidth = Math.max(element.document.documentElement.scrollWidth, element.document.body.scrollWidth);
   var scrollHeight = Math.max(element.document.documentElement.scrollHeight, element.document.body.scrollHeight);
   if(formuploadiframe!= 1)
    {
        scrollWidth = Math.max(scrollWidth,Math.max(element.parent.document.documentElement.scrollWidth, element.parent.document.body.scrollWidth));
        scrollHeight = Math.max(scrollHeight,Math.max(element.parent.document.documentElement.scrollHeight, element.parent.document.body.scrollHeight));
    }
  backgroundElement.style.width = Math.max(scrollWidth, clientWidth)+'px';
  backgroundElement.style.height = Math.max(scrollHeight, clientHeight)+'px'; 
  
  if(G_LeftWidth)
  {
   foregroundElement.style.top = Math.max(0,scrollTop+((clientHeight)/2))+'px';
   foregroundElement.style.left = Math.max(0,scrollLeft+((clientWidth -G_LeftWidth )/2))+'px';
  }
  else
  {
      clientHeight1 = clientHeight;
     clientWidth1 =clientWidth;
      if(element.parent!=window &&element.parent.document.body&&element.parent.document.body.tagName =="BODY"){
      
          scrollLeft = (element.parent.document.documentElement.scrollLeft ? element.parent.document.documentElement.scrollLeft : element.parent.document.body.scrollLeft);
          scrollTop = (element.parent.document.documentElement.scrollTop ? element.parent.document.documentElement.scrollTop : element.parent.document.body.scrollTop);
          if(navigator.userAgent.indexOf("MSIE")!=-1 || document.documentMode ==11)
          {
             clientWidth = element.parent.document.documentElement.clientWidth;
	         clientHeight = element.parent.document.documentElement.clientHeight;
	         }
            else if(navigator.userAgent.indexOf("Firefox")!=-1||navigator.userAgent.indexOf("Safari")!=-1)
          {
            clientWidth = Math.min(element.parent.innerWidth, element.parent.document.documentElement.clientWidth);
	        clientHeight = Math.min(element.parent.innerHeight,element.parent.document.documentElement.clientHeight);
          }
      }  
       if(clientHeight1>=clientHeight){

      foregroundElement.style.top = Math.max(0,scrollTop+((clientHeight-150)/2))+'px';
      }else
      {
        var height = document.body.offsetHeight - (element.document.body.clientTop ?element.document.body.clientTop * 2 : 0); 
      if(height<clientHeight1)
      foregroundElement.style.top = Math.max(0,(height-150)/2)+'px';
      else

      foregroundElement.style.top = Math.max(0,(clientHeight1-150)/2)+'px';
      }
      if(clientWidth1>=clientWidth){
      foregroundElement.style.left = Math.max(0,scrollLeft+((clientWidth-150)/2))+'px';
      }else
      {
      foregroundElement.style.left = Math.max(0,(clientWidth1-150)/2)+'px';
      }
  }
 }
 function load_init(){  

  //debugger;
  var tdwidth="30px";
  if(navigator.userAgent.indexOf("MSIE")!=-1 || document.documentMode ==11)
        tdwidth="20px";

   var element = window;
    var formuploadiframe = queryString('formuploadiframe');
    if (formuploadiframe != 1) {
        while (element.parent != element) {
            if (element.parent && element.parent.document.body.tagName != 'BODY') //modified by ari. date:2007-08-23,reson:the frameset have no body,error occur when interate into the window.
                break;
            element = element.parent;
        }
    }
     if(!backgroundElement)
      {
          
            backgroundElement=element.document.createElement('div');
  
            backgroundElement.id="loadingPageBackground";
 
            backgroundElement.className ="gis-message-page-background";//"gis-dialog";//
            //backgroundElement.style.cssText = "background-color:Gray;filter:alpha(opacity=70);opacity:0.7;";
            backgroundElement.style.display = '';
            backgroundElement.style.position = 'absolute';
            backgroundElement.style.left = '0px';
            backgroundElement.style.top = '0px';
            backgroundElement.style.zIndex = 100000;
            element.document.body.appendChild(backgroundElement);
             
            foregroundElement=element.document.createElement('div');
            foregroundElement.id="loadingPageForeground";
            foregroundElement.style.cssText = "z-index:100001;text-align:center;position:absolute;width:150px;vertical-align:middle; height:30px;background-color:#f2f2f2;border:1px #194576 solid;padding:10px;opacity:0.9;filter:alpha(opacity=90);"; 
            foregroundElement.innerHTML = "<table><tr><td width='"+tdwidth+"'></td><td rowspan=\"3\"><img src='"+ImgPath+"loading.gif' alt='Loading ...' /></td><td></td></tr><tr><td width='"+tdwidth+"'></td><td><span id=\"__processerBarText\" style=\"font-family: Helvetica,verdana, Arial, sans-serif; font-size: 12px; font-weight: bold; color: #000000; text-decoration: none\">"+Loading+"...</span></td></tr><tr><td width='"+tdwidth+"'></td><td><td></tr></table>";
            foregroundElement.style.display = 'none';
            element.document.body.appendChild(foregroundElement);
           // IsAppendToBody=true;
      }  
 }
 function load_show(sender, args)
 { 
     if(!G_LoadingInit)
     {
     load_init();
     G_LoadingInit =true;
     }
  

//  ShowDropDownList("none");
//  ForIE6(backgroundElement);
    if(typeof(ResizeFrame)!='undefined') ResizeFrame();

  load_time++;
   var element = window; 	
   while(element.parent != element){       
        element =element.parent;   
        if(element.HideWindow){ element.HideWindow(); break;}         
      }
  load_layout();
  
  backgroundElement.style.display = '';
  foregroundElement.style.display = '';	
    //ForIE6(foregroundElement);
     ForIE6(backgroundElement);
    foregroundElement.focus();
  
  addEvent(window, 'resize', load_layout);
  addEvent(window, 'scroll', load_layout);  
 }
 var topValue=1;
 function load_hide(sender, args)
 {
 if(!backgroundElement || !foregroundElement)
 {
    var element = window;
    var formuploadiframe = queryString('formuploadiframe');
    if (formuploadiframe != 1) {
        while (element.parent != element) {
            if (element.parent && element.parent.document.body.tagName != 'BODY') //modified by ari. date:2007-08-23,reson:the frameset have no body,error occur when interate into the window.
                break;
            element = element.parent;
        }
    }
    backgroundElement = element.document.getElementById("loadingPageBackground");
    foregroundElement = element.document.getElementById("loadingPageForeground");
 }
 if(backgroundElement!=null)
  backgroundElement.style.display = 'none';
 if(foregroundElement!=null)
  foregroundElement.style.display = 'none'; 
 // ForIE6(foregroundElement);
   ForIE6(backgroundElement);
  load_time--;
  if(load_time>=0)
  {       
      if(load_layout)
         removeEvent(window, 'resize', load_layout);         
      if(load_layout)
       removeEvent(window, 'scroll', load_layout);     
  }  
//  ShowDropDownList("block");
   var element = window; 	
   while(element.parent != element){       
        element =element.parent;   
        if(element.ShowWindow){ element.ShowWindow(); break;}         
      }
// debugger;
  if(typeof(ResizeFrame)!='undefined') ResizeFrame();
  if(navigator.userAgent.indexOf("Firefox")>-1)
  {
    if(parent)
    {
        topValue = topValue * (-1);
        parent.document.documentElement.scrollTop+=topValue;
    }
  }
 }
 function InitializeRequest(sender, args)
{ 
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    if (prm.get_isInAsyncPostBack()) 
    {       
          prm.abortPostBack();
     }
 }
 
 
 
 
 function ApplicationLoadHandler() {
    if(typeof(Sys)!="undefined")
    {
     Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(load_show);
     Sys.WebForms.PageRequestManager.getInstance().add_endRequest(load_hide);
     }
     if (!Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack())
    {
      Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(InitializeRequest);
    }
     } 
 if(typeof(Sys) != "undefined"){
    Sys.Application.add_load(ApplicationLoadHandler);    
      if(typeof(PromptingMessageHandler) != "undefined")
        Sys.Application.add_load(PromptingMessageHandler);
    }
 
//addEvent(window, 'load', load_init);
function ForIE6(elt) {
    // debugger;
    if (navigator.userAgent.indexOf("MSIE 6") != -1 || navigator.userAgent.indexOf("Firefox") != -1) {
        var element = window;

        while (element.parent != element) {
            if (element.parent && element.parent.document.body.tagName != 'BODY')//modified by ari. date:2007-08-23,reson:the frameset have no body,error occur when interate into the window.
                break;
            element = element.parent;
        }
        if(!elt)
        {
            return;
        }
        var childFrame = elt._hideWindowedElementsIFrame;
        if (!childFrame) {
            childFrame = element.document.createElement("iframe");
            childFrame.src = "javascript:'<html></html>';";
            childFrame.style.position = "absolute";
            childFrame.style.display = "none";
            childFrame.scrolling = "no";
            childFrame.frameBorder = "0";
            childFrame.tabIndex = "-1";
            childFrame.style.filter = "progid:DXImageTransform.Microsoft.Alpha(style=0,opacity=0)";
            element.document.body.insertBefore(childFrame, elt);
            elt._hideWindowedElementsIFrame = childFrame;

        }

        childFrame.style.left = elt.style.left;
        childFrame.style.top = elt.style.top;
        childFrame.style.width = elt.style.width;
        childFrame.style.height = elt.style.height;
        childFrame.style.display = elt.style.display;
        if (elt.style.zIndex) {
            childFrame.style.zIndex = elt.style.zIndex;
        }
    }
}
//function ForIE60(elt)
//{
////debugger;
// if(navigator.userAgent.indexOf("MSIE 6")!=-1) 
//     {                      
//                var childFrame = elt._hideWindowedElementsIFrame;
//                if (!childFrame) {                    
//                    childFrame = document.createElement("iframe");
//                    childFrame.src = "javascript:'<html></html>';";
//                    childFrame.style.position = "absolute";
//                    childFrame.style.display = "none";
//                    childFrame.scrolling = "no";
//                    childFrame.frameBorder = "0";
//                    childFrame.tabIndex = "-1";                    
//                    childFrame.style.filter = "progid:DXImageTransform.Microsoft.Alpha(style=0,opacity=0)";
//                     var element = window;
//    
//       while(element.parent != element)
//        {
//            if(element.parent && element.parent.document.body.tagName!='BODY') //modified by ari. date:2007-08-23,reson:the frameset have no body,error occur when interate into the window.
//              break;
//            element = element.top;            
//        }
//         element.document.appendChild(childFrame);
//                   // elt.parentNode.parentNode.appendChild(childFrame);                             
//         elt._hideWindowedElementsIFrame = childFrame;
//                   
//            }

//            var width = elt.offsetWidth - (elt.clientLeft ? elt.clientLeft * 2 : 0);
//            var height = elt.offsetHeight - (elt.clientTop ? elt.clientTop * 2 : 0); 
//            childFrame.style.width =width+5+"px";
//            childFrame.style.height =height+5+"px";  
//            var top=elt.style.top;
//            var left=elt.style.left;
//            childFrame.style.top =Math.max(0,eval(top.substring(0,top.indexOf("px")))-2)+"px";
//            childFrame.style.left =Math.max(0,eval(left.substring(0,left.indexOf("px")))-2)+"px";            
//            childFrame.style.display = elt.style.display;
//            if (elt.style.zIndex) {
//                childFrame.style.zIndex = elt.style.zIndex;
//            }
//        }
//}
//function ResizeFrame(){
//                var current=window;                
//                while(current.parent!=current){
//                    if(current.dyniframesize){
//                        current.dyniframesize();
//                        return;
//                    }
//                    current=current.parent;
//                }
//            }
//            if (window.addEventListener)
//                window.addEventListener("load", ResizeFrame, false);
//             else if (window.attachEvent)
//                window.attachEvent("onload", ResizeFrame);
//             else
//                window.onload=ResizeFrame;
function ShowDropDownList(displayValue)
{
   var drpCtls=document.getElementsByTagName("select");
   for(var i=0;i<drpCtls.length;i++)
   {
      drpCtls[i].style.display=displayValue;
   }
}
function queryString(item) {
    var sValue = location.search.match(new RegExp("[\?\&]" + item + "=([^\&]*)(\&?)", "i"))
    return sValue ? sValue[1] : sValue
}   