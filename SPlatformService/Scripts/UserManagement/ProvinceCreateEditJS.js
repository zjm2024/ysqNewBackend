$(document).ready(function () {
    Init();  

    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {

            ctl00$ContentPlaceHolder_Content$txtProvinceCode: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtProvinceName: {
                required: true
            }
        },
        messages: {

            ctl00$ContentPlaceHolder_Content$txtProvinceCode: {
                required: "请输入编号！"
            },
            ctl00$ContentPlaceHolder_Content$txtProvinceName: {
                required: "请输入名称！"
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
        var puhidenObjList = $("#CityList").find("input[type='hidden']");
        for (var i = 0; i < puhidenObjList.length; i++) {
            var puhidenObj = $(puhidenObjList[i]);            
            var cityCode = puhidenObj.next().val();
            var cityName = puhidenObj.parent().next().children().val();
            if (cityCode == "" || cityName == "")
            {
                isEmpty = true;
                break;
            }
            for (var j = i + 1; j < puhidenObjList.length; j++) {
                var puhidenObj2 = $(puhidenObjList[j]);
                var cityCode2 = puhidenObj2.next().val();
                var cityName2 = puhidenObj2.parent().next().children().val();

                if (cityCode == cityCode2 || cityName == cityName2) {
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

        var provinceId = parseInt($("#" + hidProvinceId).val());
        var provinceVO = GetProvinceVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/User/UpdateProvince?token=" + _Token,
            type: "POST",
            data: provinceVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (provinceId < 1) {
                        provinceId = data.Result;
                        $("#" + hidProvinceId).val(data.Result);
                    }
                    //绑定City
                    BindCity(provinceId);

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
        window.location.href = "ProvinceBrowse.aspx";
        return false;
    });
          
    
});


function GetProvinceVO() {
    var cityModelVO = new Object();
    var provinceVO = new Object();

    cityModelVO.Province = provinceVO;

	var objProvinceCode = $("input[id*='txtProvinceCode']");
	var objProvinceName = $("input[id*='txtProvinceName']");
	var objEnable = $("input[id*='radStatusEnable']:checked");

	provinceVO.ProvinceId = parseInt($("#" + hidProvinceId).val());
	provinceVO.ProvinceCode = objProvinceCode.val();
	provinceVO.ProvinceName = objProvinceName.val();
	if (objEnable.length > 0)
	    provinceVO.Status = true;
	else
	    provinceVO.Status = false;



	var cityVOList = new Array();
	cityModelVO.City = cityVOList;
	var puhidenObjList = $("#CityList").find("input[type='hidden']");
	for (var i = 0; i < puhidenObjList.length; i++) {
	    var puhidenObj = $(puhidenObjList[i]);
	    var puValue = puhidenObj.val();
	    var cityId = puValue.split("_")[2];
	    var provinceId = puValue.split("_")[1];

	    var projectUserVO = new Object();
	    cityVOList.push(projectUserVO);
	    projectUserVO.ProvinceId = provinceId;
	    projectUserVO.CityId = cityId;
	    projectUserVO.Status = puhidenObj.parent().next().next().find("input[type=radio]")[0].checked;
	    projectUserVO.CityCode = puhidenObj.next().val();
	    projectUserVO.CityName = puhidenObj.parent().next().children().val();
	}

	return cityModelVO;
}

function SetProvince(provinceVO) {

	var objProvinceCode = $("input[id*='txtProvinceCode']");
	var objProvinceName = $("input[id*='txtProvinceName']");
	var objEnable = $("input[id*='radStatusEnable']");
	var objDisable = $("input[id*='radStatusDisable']");

	
	objProvinceCode.val(provinceVO.ProvinceCode);
	objProvinceName.val(provinceVO.ProvinceName);
	if (provinceVO.Status == 1)
	    objEnable.attr("checked", true);
	else
	    objDisable.attr("checked", true);
    
}


function Init() {
    var provinceId = parseInt($("#" + hidProvinceId).val());

    load_show();
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetProvince?provinceId=" + provinceId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var provinceVO = data.Result;

                SetProvince(provinceVO);

                //绑定City
                BindCity(provinceId);
            }            
            load_hide();
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });
}


function BindCity(provinceId) {
    var citytr = $("#CityList").find("tbody>tr");
    citytr.remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetCityList?provinceId=" + provinceId + "&enable=false&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];
                    //添加User Project
                    AddCity(puVO);
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

function AddCity(puVO) {
    var cityTable = $("#CityList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input type=\"hidden\" value=\"UserProject_" + puVO.ProvinceId + "_" + puVO.CityId + "\" /> \r\n";  
    oTR += "    <input maxlength=\"20\" id=\"\" class=\"text-right\" type=\"text\" value=\"" + puVO.CityCode + "\">\r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input maxlength=\"20\" id=\"\" class=\"text-right\" type=\"text\" value=\"" + puVO.CityName + "\">\r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "  <label> \r\n";
    oTR += "      <input name=\"citystatus_" + puVO.CityId + "\" class=\"ace\" type=\"radio\" "+ ((puVO.Status == 1) ? "checked=\"checked\"" : "" ) + "/> \r\n";
    oTR += "      <span class=\"lbl\">启用</span> \r\n";
    oTR += "  </label> \r\n";
    oTR += "  <label> \r\n";
    oTR += "      <input name=\"citystatus_" + puVO.CityId + "\" class=\"ace\" type=\"radio\" " + ((puVO.Status == 1) ? "": "checked=\"checked\"" ) + "/> \r\n";
    oTR += "      <span class=\"lbl\">禁用</span> \r\n";
    oTR += "  </label> \r\n";
    oTR += "  </td> \r\n";
    oTR += "</tr> \r\n";

    cityTable.append(oTR);
}

function NewCity() {
    var cityVO = new Object();
    cityVO.ProvinceId = parseInt($("#" + hidProvinceId).val());
    var puhidenObjList = $("#CityList").find("input[type='hidden']");
    cityVO.CityId = -1 * puhidenObjList.length;
    cityVO.CityCode = "";
    cityVO.CityName = "";
    cityVO.Status = 1;
    AddCity(cityVO);
}
