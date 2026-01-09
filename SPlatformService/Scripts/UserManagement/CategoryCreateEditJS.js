$(document).ready(function () {
    Init();    

    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {

            ctl00$ContentPlaceHolder_Content$txtCategoryCode: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtCategoryName: {
                required: true
            }
        },
        messages: {

            ctl00$ContentPlaceHolder_Content$txtCategoryCode: {
                required: "请输入大类编号！"
            },
            ctl00$ContentPlaceHolder_Content$txtCategoryName: {
                required: "请输入大类名称！"
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

        var isDu = false;
        var isEmpty = false;
        var puhidenObjList = $("#ChildCategoryList").find("input[type='hidden']");
        for (var i = 0; i < puhidenObjList.length; i++) {
            var puhidenObj = $(puhidenObjList[i]);
            var cCode = puhidenObj.next().val();
            var cName = puhidenObj.parent().next().children().val();
            if (cCode == "" || cName == "") {
                isEmpty = true;
                break;
            }
            for (var j = i + 1; j < puhidenObjList.length; j++) {
                var puhidenObj2 = $(puhidenObjList[j]);
                var cCode2 = puhidenObj2.next().val();
                var cName2 = puhidenObj2.parent().next().children().val();

                if (cCode == cCode2 || cName == cName2) {
                    isDu = true;
                    break;
                }
            }
            if (isDu)
                break;
        }
        if (isEmpty) {
            bootbox.dialog({
                message: "城市编号和名称不能为空！",
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
            return;
        }
        if (isDu) {
            bootbox.dialog({
                message: "城市编号和名称不能重复！",
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
            return;
        }

        var categoryId = parseInt($("#" + hidCategoryId).val());
        var categoryVO = GetCategoryVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/User/UpdateCategory?token=" + _Token,
            type: "POST",
            data: categoryVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (categoryId < 1) {
                        categoryId = data.Result;
                        $("#" + hidCategoryId).val(data.Result);
                    }
                    BindChild(categoryId);
                    
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
       

    $("#btn_cancel").click(function () {
        window.location.href = "CategoryBrowse.aspx";
        return false;
    });
          
    
});


function GetCategoryVO() {
    var categoryModelVO = new Object();
    var categoryVO = new Object();

    categoryModelVO.Category = categoryVO;

	var objCategoryCode = $("input[id*='txtCategoryCode']");
	var objCategoryName = $("input[id*='txtCategoryName']");
	var objEnable = $("input[id*='radStatusEnable']:checked");

	categoryVO.CategoryId = parseInt($("#" + hidCategoryId).val());
	categoryVO.CategoryCode = objCategoryCode.val();
	categoryVO.CategoryName = objCategoryName.val();
	if (objEnable.length > 0)
	    categoryVO.Status = true;
	else
	    categoryVO.Status = false;

	var childVOList = new Array();
	categoryModelVO.ChildCategory = childVOList;
	var puhidenObjList = $("#ChildCategoryList").find("input[type='hidden']");
	for (var i = 0; i < puhidenObjList.length; i++) {
	    var puhidenObj = $(puhidenObjList[i]);
	    var puValue = puhidenObj.val();
	    var categoryId = puValue.split("_")[2];
	    var parentCategoryId = puValue.split("_")[1];

	    var projectUserVO = new Object();
	    childVOList.push(projectUserVO);
	    projectUserVO.ParentCategoryId = parentCategoryId;
	    projectUserVO.CategoryId = categoryId;
	    projectUserVO.Status = puhidenObj.parent().next().next().find("input[type=radio]")[0].checked;
	    projectUserVO.CategoryyCode = puhidenObj.next().val();
	    projectUserVO.categoryName = puhidenObj.parent().next().children().val();
	}

	return categoryModelVO;
}

function SetCategory(categoryVO) {

	var objCategoryCode = $("input[id*='txtCategoryCode']");
	var objCategoryName = $("input[id*='txtCategoryName']");
	var objEnable = $("input[id*='radStatusEnable']");
	var objDisable = $("input[id*='radStatusDisable']");

	objCategoryCode.val(categoryVO.CategoryCode);
	objCategoryName.val(categoryVO.CategoryName);
	if (categoryVO.Status == 1)
	    objEnable.attr("checked", true);
	else
	    objDisable.attr("checked", true);
}


function Init() {
    var categoryId = parseInt($("#" + hidCategoryId).val());

    load_show();
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetCategory?categoryId=" + categoryId,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var categoryVO = data.Result;
                SetCategory(categoryVO);

                //绑定City
                BindChild(categoryId);
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });
}


function BindChild(categoryId) {
    var ctr = $("#ChildCategoryList").find("tbody>tr");
    ctr.remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetChildCategoryList?parentCategoryId=" + categoryId + "&enable=false&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];

                    AddChild(puVO);
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

function AddChild(puVO) {
    var cTable = $("#ChildCategoryList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input type=\"hidden\" value=\"UserProject_" + puVO.ParentCategoryId + "_" + puVO.CategoryId + "\" /> \r\n";
    oTR += "    <input maxlength=\"50\" id=\"\" class=\"text-right\" type=\"text\" value=\"" + puVO.CategoryCode + "\">\r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input maxlength=\"50\" id=\"\" class=\"text-right\" type=\"text\" value=\"" + puVO.CategoryName + "\">\r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "  <label> \r\n";
    oTR += "      <input name=\"citystatus_" + puVO.CategoryId + "\" class=\"ace\" type=\"radio\" " + ((puVO.Status == 1) ? "checked=\"checked\"" : "") + "/> \r\n";
    oTR += "      <span class=\"lbl\">启用</span> \r\n";
    oTR += "  </label> \r\n";
    oTR += "  <label> \r\n";
    oTR += "      <input name=\"citystatus_" + puVO.CategoryId + "\" class=\"ace\" type=\"radio\" " + ((puVO.Status == 1) ? "" : "checked=\"checked\"") + "/> \r\n";
    oTR += "      <span class=\"lbl\">禁用</span> \r\n";
    oTR += "  </label> \r\n";
    oTR += "  </td> \r\n";
    oTR += "</tr> \r\n";

    cTable.append(oTR);
}

function NewChildCategory() {
    var cVO = new Object();
    cVO.ParentCategoryId = parseInt($("#" + hidCategoryId).val());
    var puhidenObjList = $("#ChildCategoryList").find("input[type='hidden']");
    cVO.CategoryId = -1 * puhidenObjList.length;
    cVO.CategoryCode = "";
    cVO.CategoryName = "";
    cVO.Status = 1;
    AddChild(cVO);
}