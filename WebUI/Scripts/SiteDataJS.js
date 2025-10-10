
function GetDataList(dataType, filterModel, success, fail) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/" + dataType,
        type: "POST",
        data: filterModel,
        async: false,
        success: function (data) {
            if (success) {
                success(data);
            }
        },
        error: function (data) {
            if (fail) {
                fail(data);
            }
        }
    });
}

function GetDataCount(dataType, filterModel, success, fail) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/" + dataType + "Count",
        type: "POST",
        data: filterModel,
        async: false,
        success: function (data) {
            if (success) {
                success(data);
            }
        },
        error: function (data) {
            if (fail) {
                fail(data);
            }
        }
    });
}

function GetData(dataType, id, success, fail) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/" + dataType + "?id=" + id,
        type: "Get",
        data: null,
        async: false,
        success: function (data) {
            if (success) {
                success(data);
            }
        },
        error: function (data) {
            if (fail) {
                fail(data);
            }
        }
    });
}

function MarkData(markVO, success, fail) {
    if (_CustomerId > 0) {
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/UpdateMark?token=" + _Token,
            type: "POST",
            data: markVO,
            success: function (data) {
                if (success) {
                    success(data);
                }
            },
            error: function (data) {
                if (fail) {
                    fail(data);
                }
            }
        });
    } else {
        bootbox.dialog({
            message: "您尚未登录，是否现在登录？",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                         //跳转到登录页面
                    }
                },
                "Cancel":
                {
                    "label": "取消",
                    "className": "btn-sm",
                    "callback": function () {
                    }
                }
            }
        });
    }
}

function DeleteMarkData(markId) {
    if (_CustomerId > 0) {
        $.ajax({
            url: _RootPath + "SPWebAPI/Customer/DeleteMark?markId=" + markId + "&token=" + _Token,
            type: "POST",
            data: null,
            success: function (data) {
                if (success) {
                    success(data);
                }
            },
            error: function (data) {
                if (fail) {
                    fail(data);
                }
            }
        });
    } else {
        bootbox.dialog({
            message: "您尚未登录，是否现在登录？",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        //跳转到登录页面
                    }
                },
                "Cancel":
                {
                    "label": "取消",
                    "className": "btn-sm",
                    "callback": function () {
                    }
                }
            }
        });
    }
}

function GetRequireList() {

}

function SetPaging(dataType,filterModel) {
    GetDataCount(dataType, filterModel, function (data) {

        var dataCount = data.Result / 20 + 1;
        $("input[id*='hidDataCount']").val(dataCount);
        var divPage = $("div[id*='pageList']");
        //设置分页控件
        var pageStr = "";
        pageStr += "<ul> \r\n";
        pageStr += "    <li class=\"div-up\"><a href=\"#\" onclick=\"return onPageUp();\">上一页</a></li> \r\n";
        pageStr += "    <li class=\"selected\"><a href=\"#\" onclick=\"return onPageGoTo(this);\">1</a></li> \r\n";

        for (var i = 2; i < dataCount; i++) {
            pageStr += "    <li><a href=\"#\" onclick=\"return onPageGoTo(this);\">" + i + "</a></li> \r\n";
        }

        pageStr += "    <li class=\"div-up\"><a href=\"#\" onclick=\"return onPageDown();\">下一页</a></li> \r\n";
        pageStr += "</ul> \r\n";

        divPage.html(pageStr);

    }, function (data) {

    });
}

function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function bindProvince(success, fail) {
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetProvinceList?enable=true",
        type: "Get",
        data: null,
        async: false,
        success: function (data) {
            if (data.Flag == 1) {
                if (success)
                    success(data);
            }
        },
        error: function (data) {
            if (fail)
                fail(data);
        }
    });
}

function bindCity(provinceId, success, fail) {
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetCityList?provinceId=" + provinceId + "&enable=true",
        type: "GET",
        data: null,
        async: false,
        success: function (data) {
            if (data.Flag == 1) {

                if (success)
                    success(data);
            }
        },
        error: function (data) {
            if (fail)
                fail(data);
        }
    });
}

function bindParentCategory(success, fail) {
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetParentCategoryList?enable=true",
        type: "Get",
        data: null,
        async: false,
        success: function (data) {
            if (data.Flag == 1) {

                if (success)
                    success(data);
            }
        },
        error: function (data) {
            if (fail)
                fail(data);
        }
    });
}

function bindCategory(parentCategoryId, success, fail) {
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetChildCategoryList?parentCategoryId=" + parentCategoryId + "&enable=true",
        type: "GET",
        data: null,
        async: false,
        success: function (data) {
            if (data.Flag == 1) {

                if (success)
                    success(data);
            }
        },
        error: function (data) {
            if (fail)
                fail(data);
        }
    });
}
