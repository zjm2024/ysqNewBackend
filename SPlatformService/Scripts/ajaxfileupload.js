
jQuery.extend({


    createUploadIframe: function (id, uri) {
        //create frame
        var frameId = 'jUploadFrame' + id;

        if (window.ActiveXObject) {
            var io = document.createElement('<iframe id="' + frameId + '" name="' + frameId + '" />');
            if (typeof uri == 'boolean') {
                io.src = 'javascript:false';
            }
            else if (typeof uri == 'string') {
                io.src = uri;
            }
        }
        else {
            var io = document.createElement('iframe');
            io.id = frameId;
            io.name = frameId;
        }
        io.style.position = 'absolute';
        io.style.top = '-1000px';
        io.style.left = '-1000px';

        document.body.appendChild(io);

        return io
    },
    createUploadForm: function (id, fileElementId) {
        //create form
        var formId = 'jUploadForm' + id;
        var fileId = 'jUploadFile' + id;
        var form = $('<form  action="" method="POST" name="' + formId + '" id="' + formId + '" enctype="multipart/form-data"></form>');
        var oldElement = $('#' + fileElementId);
        var newElement = $(oldElement).clone();
        $(oldElement).attr('id', fileId);
        $(oldElement).before(newElement);
        $(oldElement).appendTo(form);
        //set attributes
        $(form).css('position', 'absolute');
        $(form).css('top', '-1200px');
        $(form).css('left', '-1200px');
        $(form).appendTo('body');
        return form;
    },
    addOtherRequestsToForm: function (form, data) {
        // add extra parameter
        var originalElement = $('<input type="hidden" name="" value="">');
        for (var key in data) {
            name = key;
            value = data[key];
            var cloneElement = originalElement.clone();
            cloneElement.attr({ 'name': name, 'value': value });
            $(cloneElement).appendTo(form);
        }
        return form;
    },

    ajaxFileUpload: function (s) {
        // TODO introduce global settings, allowing the client to modify them for all requests, not only timeout
        s = jQuery.extend({}, jQuery.ajaxSettings, s);
        var id = new Date().getTime()
        var form = jQuery.createUploadForm(id, s.fileElementId);
        if (s.data) form = jQuery.addOtherRequestsToForm(form, s.data);
        var io = jQuery.createUploadIframe(id, s.secureuri);
        var frameId = 'jUploadFrame' + id;
        var formId = 'jUploadForm' + id;
        // Watch for a new set of requests
        if (s.global && !jQuery.active++) {
            jQuery.event.trigger("ajaxStart");
        }
        var requestDone = false;
        // Create the request object
        var xml = {}
        if (s.global)
            jQuery.event.trigger("ajaxSend", [xml, s]);
        // Wait for a response to come back
        var uploadCallback = function (isTimeout) {
            var io = document.getElementById(frameId);
            try {
                if (io.contentWindow) {
                    xml.responseText = io.contentWindow.document.body ? io.contentWindow.document.body.innerHTML : null;
                    xml.responseXML = io.contentWindow.document.XMLDocument ? io.contentWindow.document.XMLDocument : io.contentWindow.document;

                } else if (io.contentDocument) {
                    xml.responseText = io.contentDocument.document.body ? io.contentDocument.document.body.innerHTML : null;
                    xml.responseXML = io.contentDocument.document.XMLDocument ? io.contentDocument.document.XMLDocument : io.contentDocument.document;
                }
            } catch (e) {
                jQuery.handleError(s, xml, null, e);
            }
            if (xml || isTimeout == "timeout") {
                requestDone = true;
                var status;
                try {
                    status = isTimeout != "timeout" ? "success" : "error";
                    // Make sure that the request was successful or notmodified
                    if (status != "error") {
                        // process the data (runs the xml through httpData regardless of callback)
                        var data = jQuery.uploadHttpData(xml, s.dataType);
                        // If a local callback was specified, fire it and pass it the data
                        if (s.success)
                            s.success(data, status);

                        // Fire the global callback
                        if (s.global)
                            jQuery.event.trigger("ajaxSuccess", [xml, s]);
                    } else
                        jQuery.handleError(s, xml, status);
                } catch (e) {
                    status = "error";
                    jQuery.handleError(s, xml, status, e);
                }

                // The request was completed
                if (s.global)
                    jQuery.event.trigger("ajaxComplete", [xml, s]);

                // Handle the global AJAX counter
                if (s.global && ! --jQuery.active)
                    jQuery.event.trigger("ajaxStop");

                // Process result
                if (s.complete)
                    s.complete(xml, status);

                jQuery(io).unbind()

                setTimeout(function () {
                    try {
                        $(io).remove();
                        $(form).remove();

                    } catch (e) {
                        jQuery.handleError(s, xml, null, e);
                    }

                }, 100)

                xml = null

            }
        }
        // Timeout checker
        if (s.timeout > 0) {
            setTimeout(function () {
                // Check to see if the request is still happening
                if (!requestDone) uploadCallback("timeout");
            }, s.timeout);
        }
        try {
            // var io = $('#' + frameId);
            var form = $('#' + formId);
            $(form).attr('action', s.url);
            $(form).attr('method', 'POST');
            $(form).attr('target', frameId);
            if (form.encoding) {
                form.encoding = 'multipart/form-data';
            }
            else {
                form.enctype = 'multipart/form-data';
            }
            $(form).submit();

        } catch (e) {
            jQuery.handleError(s, xml, null, e);
        }
        if (window.attachEvent) {
            document.getElementById(frameId).attachEvent('onload', uploadCallback);
        }
        else {
            document.getElementById(frameId).addEventListener('load', uploadCallback, false);
        }
        return { abort: function () { } };

    },

    uploadHttpData: function (r, type) {
        var data = !type;
        data = type == "xml" || data ? r.responseXML : r.responseText;
        // If the type is "script", eval it in global context
        if (type == "script")
            jQuery.globalEval(data);
        // Get the JavaScript object, if JSON is used.
        if (type == "json") {
            // If you add mimetype in your response,
            // you have to delete the '<pre></pre>' tag.
            // The pre tag in Chrome has attribute, so have to use regex to remove
            var data = r.responseText;
            var rx = new RegExp("<pre.*?>(.*?)</pre>", "i");
            var am = rx.exec(data);
            //this is the desired data extracted
            var data = (am) ? am[1] : data;    //the only submatch or empty
            eval("data = " + data);
        }
        // evaluate scripts within html
        if (type == "html")
            jQuery("<div>").html(data).evalScripts();
        //alert($('param', data).each(function(){alert($(this).attr('value'));}));
        return data;
    },

    handleError: function (s, xhr, status, e) {
        // If a local callback was specified, fire it
        if (s.error) {
            s.error.call(s.context || s, xhr, status, e);
        }
        // Fire the global callback
        if (s.global) {
            (s.context ? jQuery(s.context) : jQuery.event).trigger("ajaxError", [xhr, s, e]);
        }
    }
})

function uploadImgWithPath(uploadId, path, callback) {
    if ($("#" + uploadId).val().length > 0) {
        if (isPicture(uploadId)) {
            if (getFileSize(uploadId) <= 2048) {
                ajaxFileUpload(uploadId, path, callback);
            } else {
                if (callback) {
                    var result = new Object();
                    result.Flag = 0;
                    result.Message = "图片大小不能超过2M";

                    callback(result);
                }
            }
        } else {
            if (callback) {
                var result = new Object();
                result.Flag = 0;
                result.Message = "图片类型必须是.gif,jpeg,jpg,png";

                callback(result);
            }
        }
    }
}
function uploadImg(uploadId, callback) {
    if ($("#" + uploadId).val().length > 0) {
        if (isPicture(uploadId)) {
            if (getFileSize(uploadId) <= 102400) {
                ajaxFileUpload(uploadId, "Image", callback);
            } else {
                if (callback) {
                    var result = new Object();
                    result.Flag = 0;
                    result.Message = "大小不能超过100M";

                    callback(result);
                }
            }
        } else {
            if (callback) {
                var result = new Object();
                result.Flag = 0;
                result.Message = "类型必须是.gif,jpeg,jpg,png,mp4";

                callback(result);
            }
        }
    }
}
function uploadFile(uploadId,  callback) {
    //判断大小
    if ($("#" + uploadId).val().length > 0) {
        if (getFileSize(uploadId) <= 102400) {
            ajaxFileUpload(uploadId, "Attached/", callback);
        } else {
            if (callback) {
                var result = new Object();
                result.Flag = 0;
                result.Message = "单个附件大小不能超过100M";

                callback(result);
            }
        }
    }
}
function uploadFile(uploadId, requirementCode, callback) {
    //判断大小
    if ($("#" + uploadId).val().length > 0) {
        if (getFileSize(uploadId) <= 102400) {
            ajaxFileUpload(uploadId, "Attached/" + requirementCode, callback);
        } else {
            if (callback) {
                var result = new Object();
                result.Flag = 0;
                result.Message = "单个附件大小不能超过100M";

                callback(result);
            }
        }
    }
}

function isPicture(uploadId) {
    var f = $("#" + uploadId).val();
    if (!/\.(gif|jpg|jpeg|png|mp4|GIF|JPG|PNG|MP4)$/.test(f)) {
        //alert("图片类型必须是.gif,jpeg,jpg,png中的一种")
        return false;
    }
    return true;
}

function getFileSize(uploadId) {
    var objTemp = $("#" + uploadId);
    if (objTemp.length < 1)
        return 0;
    var obj = objTemp[0];
    var fileSize = 0;
    var isIE = /msie/i.test(navigator.userAgent) && !window.opera;
    if (isIE && !obj.files) {
        var filePath = obj.value;
        var fileSystem = new ActiveXObject("Scripting.FileSystemObject");
        var file = fileSystem.GetFile(filePath);
        fileSize = file.Size;
    } else {
        fileSize = obj.files[0].size;
    }
    fileSize = Math.round(fileSize / 1024 * 100) / 100; //单位为KB

    return fileSize;
}


function ajaxFileUpload(uploadId, subDirectory, callback) {
    $.ajaxFileUpload
    (
        {
            url: _RootPath + 'Common/UploadHandler.ashx?SubDirectory=' + subDirectory, //用于文件上传的服务器端请求地址
            secureuri: false, //一般设置为false
            fileElementId: uploadId, //文件上传空间的id属性  <input type="file" id="file" name="file" />
            dataType: 'json', //返回值类型 一般设置为json
            type: "POST",
            success: function (data, status)  //服务器成功响应处理函数
            {
                if (data.Flag != 1) {
                    alert(data.Message);
                } else {
                    if (callback)
                        callback(data);
                    //$("#" + uploadId + "Pic").attr("src", data.Result.replace("~", ".."));
                }
            },
            error: function (data, status, e)//服务器响应失败处理函数
            {
                alert(e);
            }
        }
    )
    return false;
}
