$(document).ready(function () {
    Init();
    $.validator.addMethod("ddlrequired", function (value, element, params) {
        if (value == null || value == "" || value == "-1") {
            return false;
        } else {
            return true;
        }
    }, "请选择！");

    $.validator.addMethod("isTel", function (value, element, params) {
        var length = value.length;
        var mobile = /^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/;
        var tel = /^(\d{3,4}-?)?\d{7,9}$/g;
        return this.optional(element) || tel.test(value) || (length == 11 && mobile.test(value)) || length == 0;
    }, "请输入正确格式的电话");

   

    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {

            ctl00$ContentPlaceHolder_Content$txtCityId: {
                required: true
            },

            ctl00$ContentPlaceHolder_Content$txtTitle: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtPrice: {
                number: true
            },
            ctl00$ContentPlaceHolder_Content$txtCount: {
                number: true
            }

        },
        messages: {

            ctl00$ContentPlaceHolder_Content$txtCityId: {
                required: "请输入部门！"
            },
            ctl00$ContentPlaceHolder_Content$txtTitle: {
                required: "请输入部门！"
            },
            ctl00$ContentPlaceHolder_Content$txtPrice: {
                number: "请输入部门！"
            },
            ctl00$ContentPlaceHolder_Content$txtCount: {
                number: "请输入部门！"
            }
        },
        highlight: function (e) {
            $(e).closest('.form-group').removeClass('has-info').addClass('has-error');
        },

        success: function (e) {
            $(e).closest('.form-group').removeClass('has-error');
            $(e).remove();
        }
    });

    $("#btn_save").click(function () {

        if (!$("form[id*='aspnetForm']").valid()) {
            return false;
        }

        var servicesId = parseInt($("#" + hidServicesId).val());
        var servicesVO = GetServicesVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/Require/UpdateServices?token=" + _Token,
            type: "POST",
            data: servicesVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (servicesId < 1) {
                        $("#" + hidServicesId).val(data.Result);
                    }
                    bootbox.dialog({
                        message: data.Message,
                        buttons:
                        {
                            "Confirm":
                            {
                                "label": "确定",
                                "className": "btn-sm btn-primary",
                                "callback": function () {

                                }
                            }
                        }
                    });
                } else {
                    bootbox.dialog({
                        message: data.Message,
                        buttons:
                        {
                            "Confirm":
                            {
                                "label": "确定",
                                "className": "btn-sm btn-primary",
                                "callback": function () {

                                }
                            }
                        }
                    });
                }

            },
            error: function (data) {
                alert(data);
            }
        });
    });

    $("#btn_updateservicesstatus").click(function () {
        var servicesId = parseInt($("#" + hidServicesId).val());
        bootbox.dialog({
            message: "是否确认取消发布?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateServicesStatusAction(servicesId, 0);
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
    });

    $("#btn_cancel").click(function () {
        window.location.href = "ServicesBrowse.aspx";
        return false;
    });
          
    
});

function UpdateServicesStatusAction(servicesId, status) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/UpdateServicesStatus?servicesId=" + servicesId + "&status=" + status + "&token=" + _Token,
        type: "Post",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                bootbox.dialog({
                    message: data.Message,
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {
                                window.location.href = "ServicesBrowse.aspx";
                            }
                        }
                    }
                });
            } else {
                bootbox.dialog({
                    message: data.Message,
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {

                            }
                        }
                    }
                });
            }
            //load_hide();
        },
        error: function (data) {
            alert(data);
            //load_hide();
        }
    });
}


function GetServicesVO() {
    var servicesModelVO = new Object();
    var servicesVO = new Object();

    servicesModelVO.Services = servicesVO;

    var objCityId = $("select[id*='drpCity']");
	var objTitle = $("input[id*='txtTitle']");
	var objPrice = $("input[id*='txtPrice']");
	var objCount = $("input[id*='txtCount']");
	var ue = UE.getEditor('container');
	//var objMainImg = $("input[id*='txtMainImg']");
	//var objStatus = $("input[id*='txtStatus']");
	//var objCreatedAt = $("input[id*='txtCreatedAt']");

	servicesVO.ServicesId = parseInt($("#" + hidServicesId).val());
	servicesVO.CityId = objCityId.val();
	
	servicesVO.Title = objTitle.val();
	servicesVO.Price = objPrice.val();
	servicesVO.Count = objCount.val();
	servicesVO.Description = ue.getContent();
	//servicesVO.MainImg = objMainImg.val();
	//servicesVO.Status = objStatus.val();
    //servicesVO.CreatedAt = objCreatedAt.val();

	var targetCategoryVOList = new Array();
	servicesModelVO.ServicesCategory = targetCategoryVOList;
	var puhidenObjList = $("#TargetCategoryList").find("input[type='hidden']");
	for (var i = 0; i < puhidenObjList.length; i++) {
	    var puhidenObj = $(puhidenObjList[i]);
	    var puValue = puhidenObj.val();
	    var servicesId = puValue.split("_")[1];
	    var categoryId = puValue.split("_")[2];

	    var targetCategoryVO = new Object();
	    targetCategoryVOList.push(targetCategoryVO);
	    targetCategoryVO.ServicesId = servicesId;
	    targetCategoryVO.CategoryId = categoryId;
	}

	return servicesModelVO;
}

function SetServices(servicesVO) {

    var objCityId = $("select[id*='txtCityId']");
	var objCustomerId = $("input[id*='txtCustomerId']");
	var objTitle = $("input[id*='txtTitle']");
	var objPrice = $("input[id*='txtPrice']");
	var objCount = $("input[id*='txtCount']");
	var ue = UE.getEditor('container');
	var objMainImg = $("input[id*='txtMainImg']");
	var objStatus = $("input[id*='txtStatus']");
	var objCreatedAt = $("input[id*='txtCreatedAt']");

	objCityId.val(servicesVO.CityId);
	objTitle.val(servicesVO.Title);
	objPrice.val(servicesVO.Price);
	objCount.val(servicesVO.Count);
	ue.ready(function () {
	    this.setContent(servicesVO.Description);
	});
	//objMainImg.val(servicesVO.MainImg);
	if (servicesVO.Status == 0)
	    objStatus.val("保存");
	else if (servicesVO.Status == 1)
	    objStatus.val("发布");
	objCreatedAt.val(new Date(servicesVO.CreatedAt).format("yyyy-MM-dd"));
   
}

function Init() {
    var servicesId = parseInt($("#" + hidServicesId).val());

    load_show();
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetServices?servicesId=" + servicesId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var servicesVO = data.Result;

                SetServices(servicesVO);

                //绑定行业
                BindTargetCategory(servicesId);
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });
}

function BindTargetCategory(servicesId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/GetServicesCategoryByServices?servicesId=" + servicesId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddTargetCategory(puVO);
                }

            } else {
                bootbox.dialog({
                    message: data.Message,
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {

                            }
                        }
                    }
                });
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
        }
    });
}

function AddTargetCategory(puVO) {
    var targetCategoryTable = $("#TargetCategoryList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"TargetCategory_" + puVO.ServicesId + "_" + puVO.CategoryId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ParentCategoryName + "\">" + puVO.ParentCategoryName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CategoryName + "\">" + puVO.CategoryName + "</td> \r\n";
    oTR += "</tr> \r\n";

    targetCategoryTable.append(oTR);
}

function NewTargetCategory() {

    onChooseCategory();
}

function DeleteTargetCategory() {
    var chkList = $("#TargetCategoryList").find("input[type='checkbox']:checked");

    if (chkList.length > 0) {
        bootbox.dialog({
            message: "是否确认删除?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        chkList.parent().parent().remove();
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
    else {
        bootbox.dialog({
            message: "请至少选择一条数据！",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {

                    }
                }
            }
        });
    }
}

function onChooseCategory() {
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 60%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/UserManagement\/ChooseCategoryList.aspx" height="350px" width="100%" frameborder="0"><\/iframe>',

        title: "选择行业",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var seleCategoryObj = $(window.frames["iframe_1"].document).find("#ChooseCategoryList").find("input[type='checkbox']:checked");
                    var selectCategoryIdStr = "";
                    var categoryArray = new Array();
                    for (var i = 0; i < seleCategoryObj.length; i++) {
                        var category = new Object();
                        categoryArray.push(category);
                        var chk = $(seleCategoryObj[i]);
                        category.CategoryId = chk.parent().next()[0].innerText;
                        category.ParentCategoryName = chk.parent().next().next()[0].innerText;
                        category.CategoryCode = chk.parent().next().next().next()[0].innerText;
                        category.CategoryName = chk.parent().next().next().next().next()[0].innerText;
                    }

                    $(window.frames["iframe_1"].document).find("#hidCategoryId").val(JSON.stringify(categoryArray));

                    var categoryArray = $(window.frames["iframe_1"].document).find("#hidCategoryId").val();
                    if (categoryArray != "") {
                        var categoryList = JSON.parse(categoryArray);
                        var puhidenObjList = $("#TargetCategoryList").find("input[type='hidden']");
                        for (var i = 0; i < categoryList.length; i++) {
                            var categoryObj = categoryList[i];
                            var puVO = new Object();
                            puVO.ServicesId = parseInt($("#" + hidServicesId).val());
                            puVO.CategoryId = categoryObj.CategoryId;
                            puVO.ParentCategoryName = categoryObj.ParentCategoryName;
                            puVO.CategoryName = categoryObj.CategoryName;

                            var isContain = false;
                            for (var j = 0; j < puhidenObjList.length; j++) {
                                var puhidenObj = $(puhidenObjList[j]);
                                var puValue = puhidenObj.val();
                                var categoryId = puValue.split("_")[2];

                                if (categoryId == puVO.CategoryId) {
                                    isContain = true;
                                    break;
                                }
                            }
                            if (!isContain)
                                AddTargetCategory(puVO);
                        }

                    }
                    $("#iframe_1").parent().empty();
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
    return false;
}