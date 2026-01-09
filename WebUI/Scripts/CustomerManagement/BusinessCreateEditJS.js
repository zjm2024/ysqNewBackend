var _BusinessId = 0;
$(document).ready(function () {
   Init(); 
   $("select[name$='drpProvince']").change(function () { //省份变更
        //更新Child
        var drp = $("select[name*='drpProvince']");
        var childDrp = $("select[name*='drpCity']");
        childDrp.empty();

        $.ajax({
            url: _RootPath + "SPWebAPI/User/GetCityList?provinceId=" + drp.val() + "&enable=true",
            type: "GET",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var childList = data.Result;
                    for (var i = 0; i < childList.length; i++) {
                        childDrp.append("<option value=\"" + childList[i].CityId + "\">" + childList[i].CityName + "</option>");
                    }

                    if (childList.length > 0)
                        childDrp.val(childList[0].CityId);                    
                }
            },
            error: function (data) {
                alert(data);
            }
        });
   });

   $("button[id*='btn_RealNameStatus']").click(function () {

       $.ajax({
           url: _RootPath + "SPWebAPI/Customer/UpdateBusinessRealNameStatus?token=" + _Token,
           type: "post",
           data: null,
           success: function (data) {
               bootbox.dialog({
                   message: data.Message,
                   buttons:
                   {
                       "click":
                       {
                           "label": "确定",
                           "className": "btn-sm btn-primary",
                           "callback": function () {
                               window.location.href = 'BusinessCreateEdit.aspx?page=RealName'
                           }
                       }
                   }
               });
           },
           error: function (data) {
               
           }
       });
   });
});

function SetBusiness(businessVO) {
	if(businessVO.Status==0){
		$("#Status").html("未通过");	
	}else if(businessVO.Status==1){
		$("#Status").html("认证通过");	
	}else if(businessVO.Status==2){
		$("#Status").html("认证驳回");	
	}

	if (businessVO.RealNameStatus == 0) {
	    $(".RealNameStatus").html("未认证");
	} else if (businessVO.RealNameStatus == 1) {
	    $(".RealNameStatus").html("认证通过");
	    $(".RealNamebtn").hide();
	} else if (businessVO.RealNameStatus == 2) {
	    $(".RealNameStatus").html("认证驳回");
	} else if (businessVO.RealNameStatus == 3) {
	    $(".RealNameStatus").html("审核中");
	}
	
	if(businessVO.CompanyName!=""){
		$("#CompanyName").html(businessVO.CompanyName);
		$("input[name=CompanyName]").val(businessVO.CompanyName)
	}else{
		$("#CompanyName").html("<font class='red'>(未设置)</font>");
	}
	
	if(businessVO.CityName!=""){
		$("#CityName").html(businessVO.CityName);
		BindCity(businessVO.ProvinceId,businessVO.CityId);
	} else {
	    BindCity(1, 1);
		$("#CityName").html("<font class='red'>(未设置)</font>");
	}
	var CategoryNames = RemoveComma(businessVO.CategoryNames);
	if(CategoryNames!=""){
		$("#CategoryNames").html(CategoryNames);
	}else{
		$("#CategoryNames").html("<font class='red'>(未设置)</font>");
	}
	
	if(businessVO.Address!=""){
		$("#Address").html(businessVO.Address);
		$("input[name=Address]").val(businessVO.Address)
	}else{
		$("#Address").html("<font class='red'>(未设置)</font>");
	}
	
	if(businessVO.CompanySite!=""){
		$("#CompanySite").html(businessVO.CompanySite);
		$("input[name=CompanySite]").val(businessVO.CompanySite)
	}else{
		$("#CompanySite").html("<font class='red'>(未设置)</font>");
	}
	
	if(businessVO.Description!=""){
		$("#Description").html(businessVO.Description);
		$("textarea[name=Description]").val(businessVO.Description)
	}else{
		$("#Description").html("<font class='red'>(未设置)</font>");
	}
	
	var MainProducts=RemoveHtml(businessVO.MainProducts);
	if(MainProducts!=""){
		$("#MainProducts").html(MainProducts); 
		var ue2 = UE.getEditor("container2");
		ue2.ready(function () {
			this.setContent(businessVO.MainProducts);
		});
	}else{
		$("#MainProducts").html("<font class='red'>(未设置)</font>");
	}
	
	var ProductDescription=RemoveHtml(businessVO.ProductDescription);
	if(ProductDescription!=""){
		$("#ProductDescription").html(ProductDescription);
		var ue = UE.getEditor("container");
		ue.ready(function () {
			this.setContent(businessVO.ProductDescription);
		});  
	}else{
		$("#ProductDescription").html("<font class='red'>(未设置)</font>");
	}
	
	if(businessVO.CompanyLogo!=""){
		$("#CompanyLogo").html("<img src='"+businessVO.CompanyLogo+"' class='fromimg'>");
		$("#imgCompanyLogoPic").attr("data",businessVO.CompanyLogo);
		$("#imgCompanyLogoPic").attr("style","background-image:url("+businessVO.CompanyLogo+")");
	}else{
		$("#CompanyLogo").html("<font class='red'>(未设置)</font>");
		$("#imgCompanyLogoPic").attr("style", "background-image:url(../Style/images/upimg.png)");
	}
	
	if(businessVO.CompanyType!=""){
		$("#CompanyType").html(businessVO.CompanyType);
		for (i = 1; i <= 7; i++) {
		    if ($("#rad" + i).val() == businessVO.CompanyType) {
		        $("#rad" + i).prop("checked", true);
		    }
		}
	}else{
		$("#CompanyType").html("<font class='red'>(未设置)</font>");
	}
	
	if(businessVO.CompanyTel!=""){
		$("#CompanyTel").html(businessVO.CompanyTel);
		$("input[name=CompanyTel]").val(businessVO.CompanyTel);
	}else{
		$("#CompanyTel").html("<font class='red'>(未设置)</font>");
	}
	
	if(businessVO.BusinessLicense!=""){
		$("#BusinessLicense").html(businessVO.BusinessLicense);
		$("input[name=BusinessLicense]").val(businessVO.BusinessLicense);
	}else{
		$("#BusinessLicense").html("<font class='red'>(未设置)</font>");
	}
	
	if(businessVO.BusinessLicenseImg!=""){
		$("#BusinessLicenseImg").html("<img src='"+businessVO.BusinessLicenseImg+"' class='fromimg'>");
		$("#imgBusinessLicenseImgPic").attr("data",businessVO.BusinessLicenseImg);
		$("#imgBusinessLicenseImgPic").attr("style","background-image:url("+businessVO.BusinessLicenseImg+")");
	}else{
		$("#BusinessLicenseImg").html("<font class='red'>(未设置)</font>");
		$("#imgBusinessLicenseImgPic").attr("style", "background-image:url(../Style/images/upimg.png)");
	}
	
	if(businessVO.EntrustImgPath!="")
	{
		$("#EntrustImgPath").html("<img src='"+businessVO.EntrustImgPath+"' class='fromimg'>");
		$("#imgEntrustImgPathPic").attr("data",businessVO.EntrustImgPath);
		$("#imgEntrustImgPathPic").attr("style","background-image:url("+businessVO.EntrustImgPath+")");
	} else {
		$("#EntrustImgPath").html("<font class='red'>(未设置)</font>");
		$("#imgEntrustImgPathPic").attr("style", "background-image:url(../Style/images/upimg.png)");
	}
}
function Init() {
	$.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetCustomer?customerId=" + _CustomerId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var customerVO = data.Result;
                _BusinessId = customerVO.BusinessId;
                $.ajax({
                    url: _RootPath + "SPWebAPI/Customer/GetBusiness?businessId=" + _BusinessId + "&token=" + _Token,
                    type: "Get",
                    data: null,
                    success: function (data) {
                        if (data.Flag == 1) {
                            var businessVO = data.Result;
                            SetBusiness(businessVO);
                            //绑定行业
                            $("#BusinessCategoryList .ui-widget-content").remove();
                            BindBusinessCategory(_BusinessId);

                            //获取目标客户行业
                            $("#TargetCityList .ui-widget-content").remove();
                            BindTargetCategory(_BusinessId);

                            //获取目标客户区域
                            $("#TargetCategoryList .ui-widget-content").remove();
                            BindTargetCity(_BusinessId);
                            //获取身份证图片
                            BindCardByBusiness(_BusinessId);
                        } else {
                            BindCity(1, 1);
                        }
                    },
                    error: function (data) {
                        alert(data);
                        load_hide();
                    }
                });
				$.ajax({
                    url: _RootPath + "SPWebAPI/Customer/GetBusinessCompleted?businessId=" + _BusinessId + "&token=" + _Token,
                    type: "Get",
                    data: null,
                    success: function (data) {
                        if (data.Flag == 1) {
                           $("#Completed").html(data.Result+"%");
                        }
                    },
                    error: function (data) {
                        console.log(data);
                        load_hide();
                    }
                });
            } else {
                //未申请认证
            }
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });
}
function BindTargetCategory(businessId) { //获取目标客户行业
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetTargetCategoryByBusiness?businessId=" + businessId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
				if(puVOList.length>0){
					var text="";
					for (var i = 0; i < puVOList.length; i++) {
						text+= puVOList[i].CategoryName+"，";
						var puVO = puVOList[i];
                    	AddTargetCategory(puVO);
					}
					if(text!=""&&text!="，")
					$("#TCategoryName").html(text);
				}
            } else {
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
        }
    });
}
function BindTargetCity(businessId) {//获取目标客户区域
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetTargetCityByBusiness?businessId=" + businessId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
				if(puVOList.length>0){	
					var text="";
					for (var i = 0; i < puVOList.length; i++) {
						text+= puVOList[i].CityName+"，";
						var puVO = puVOList[i];
                    	AddTargetCity(puVO);
					}
					if(text!=""&&text!="，")
					$("#TCityName").html(text);
				}
            } else {
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
        }
    });
}
function BindCardByBusiness(businessId) {//获取身份证图片
	$.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetBusinessIdCardByBusiness?businessId=" + businessId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            console.log(data)
            if (data.Flag == 1) {
                var puVOList = data.Result;
                if (puVOList.length > 0) {
                    var text = "";
                    for (var i = 0; i < puVOList.length; i++) {
                        if (puVOList[i].IDCardImg == "") {
                            text += "<font class='red'>(未设置)</font> ";
                        } else {
                            text += "<img src='" + puVOList[i].IDCardImg + "' class='fromimg'>";
                        }
                        if (i == 0) {
                            if (puVOList[i].IDCardImg != "") {
                                $("#CardImgPic").attr("data", puVOList[i].IDCardImg);
                                $("#CardImgPic").attr("style", "background-image:url(" + puVOList[i].IDCardImg + ")");
                            }
                        }
                        if (i == 1) {
                            if (puVOList[i].IDCardImg != "") {
                                $("#CardImg2Pic").attr("data", puVOList[i].IDCardImg);
                                $("#CardImg2Pic").attr("style", "background-image:url(" + puVOList[i].IDCardImg + ")");
                            }
                        }
                    }
                    $("#Card").html(text);
                }
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
        }
    });
}

function BindBusinessCategory(businessId) {//绑定行业
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetBusinessCategoryByBusiness?businessId=" + businessId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];
                    AddBusinessCategory(puVO);
                }

            } else {
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
        }
    });
}

function BindCity(provinceId,cityId) {//绑定区域
    var objProvince = $("select[name='drpProvince']");
    var objCity = $("select[name='drpCity']");

	$.ajax({
        url: _RootPath + "SPWebAPI/User/GetProvinceList?enable=true",
        type: "GET",
        data: null,
        async:false,
        success: function (data) {
            if (data.Flag == 1) {
                var childList = data.Result;
                objProvince.empty();//清除已有的option
                for (var i = 0; i < childList.length; i++) {
                    objProvince.append("<option value=\"" + childList[i].ProvinceId + "\">" + childList[i].ProvinceName + "</option>");
                }
                if (childList.length > 0 && provinceId > 0)
                	 objProvince.val(provinceId);  
				 objCity.empty();
					$.ajax({
						url: _RootPath + "SPWebAPI/User/GetCityList?provinceId=" + provinceId + "&enable=true",
						type: "GET",
						data: null,
						async:false,
						success: function (data2) {
							if (data2.Flag == 1) {
								var childList = data2.Result;
								for (var i = 0; i < childList.length; i++) {
									objCity.append("<option value=\"" + childList[i].CityId + "\">" + childList[i].CityName + "</option>");
								}
				
								if (childList.length > 0 && cityId > 0)
									objCity.val(cityId);                    
							}
						},
						error: function (data) {
							alert(data);
						}
					});                 
            }
        },
        error: function (data) {
            alert(data);
        }
    });
   
}
function change2(uploadId) {//上传图片
    uploadImg(uploadId, function (data) {
        if (data.Flag == 1) {
            $("#"+uploadId+"Pic").attr("data", data.Result.FilePath.replace("~", _APIURL));
			$("#"+uploadId+"Pic").attr("style", "background-image:url("+data.Result.FilePath.replace("~", _APIURL)+")");
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
    });
}
function RemoveComma(input)  //格式化行业分类
{
  if (input.length == 0)
    return input;
  var charList = new Array()
  charList = input.split(',');
  var first = 0;
  var last = charList.length - 1;
  if (charList[0] == '')
    first++;
  if (charList[charList.length - 1] == '')
    last--;
  var result = "";
  for (var i = first;i <= last;i++)
  {
    result += charList[i];
    if (i < last)
    result +="，";
  }
  return result;
}
function RemoveHtml(input) //清除html
{
	var dd = input;
    dd = dd.replace(/<\/?.+?>/g, "")
    var dds = dd.replace(/ /g, "");
	return dds;
}
function UpdateBusiness(seller_data) //更新到服务器
{
    console.log(seller_data);
	$.ajax({
            url: _RootPath + "SPWebAPI/Customer/UpdateBusiness?token=" + _Token,
            type: "POST",
            data: seller_data,
            success: function (data) {
                if (data.Flag == 1) {
                    if (_BusinessId < 1) {
                        _BusinessId = data.Result;
                    }
					Init();
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
                console.log(data);
                alert(data);
            }
        });
}
function NewBusinessCategory() {
    onChooseBusinessCategory();
}
function onChooseBusinessCategory() { //选择行业
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
                        var puhidenObjList = $("#BusinessCategoryList").find("input[type='hidden']");
                        for (var i = 0; i < categoryList.length; i++) {
                            var categoryObj = categoryList[i];
                            var puVO = new Object();
                            puVO.BusinessId = _BusinessId;
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
                                AddBusinessCategory(puVO);
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
function AddBusinessCategory(puVO) { //添加行业
    var businessCategoryTable = $("#BusinessCategoryList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"TargetCategory_" + puVO.BusinessId + "_" + puVO.CategoryId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ParentCategoryName + "\">" + puVO.ParentCategoryName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CategoryName + "\">" + puVO.CategoryName + "</td> \r\n";
    oTR += "</tr> \r\n";

    businessCategoryTable.append(oTR);
}
function DeleteBusinessCategory() {//删除行业
    var chkList = $("#BusinessCategoryList").find("tbody input[type='checkbox']:checked");

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
function NewTargetCity() {//添加目标客户区域
    onChooseCity();
}
function onChooseCity() {
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 60%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/UserManagement\/ChooseCityList.aspx" height="350px" width="100%" frameborder="0"><\/iframe>',

        title: "选择城市",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var seleCityObj = $(window.frames["iframe_1"].document).find("#ChooseCityList").find("input[type='checkbox']:checked");
                    var selectCityIdStr = "";
                    var cityArray = new Array();
                    for (var i = 0; i < seleCityObj.length; i++) {
                        var city = new Object();
                        cityArray.push(city);
                        var chk = $(seleCityObj[i]);
                        city.CityId = chk.parent().next()[0].innerText;
                        city.ProvinceName = chk.parent().next().next()[0].innerText;
                        city.CityCode = chk.parent().next().next().next()[0].innerText;
                        city.CityName = chk.parent().next().next().next().next()[0].innerText;
                    }

                    $(window.frames["iframe_1"].document).find("#hidCityId").val(JSON.stringify(cityArray));

                    var cityArray = $(window.frames["iframe_1"].document).find("#hidCityId").val();
                    if (cityArray != "") {
                        var cityList = JSON.parse(cityArray);
                        var puhidenObjList = $("#TargetCityList").find("input[type='hidden']");
                        for (var i = 0; i < cityList.length; i++) {
                            var cityObj = cityList[i];
                            var puVO = new Object();
                            puVO.BusinessId = _BusinessId;
                            puVO.CityId = cityObj.CityId;
                            puVO.ProvinceName = cityObj.ProvinceName;
                            puVO.CityName = cityObj.CityName;

                            var isContain = false;
                            for (var j = 0; j < puhidenObjList.length; j++) {
                                var puhidenObj = $(puhidenObjList[j]);
                                var puValue = puhidenObj.val();
                                var cityId = puValue.split("_")[2];

                                if (cityId == puVO.CityId) {
                                    isContain = true;
                                    break;
                                }
                            }
                            if (!isContain)
                                AddTargetCity(puVO);
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
function AddTargetCity(puVO) {
    var targetCityTable = $("#TargetCityList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"TargetCity_" + puVO.BusinessId + "_" + puVO.CityId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ProvinceName + "\">" + puVO.ProvinceName + "</td> \r\n";    
    oTR += "  <td class=\"center\" title=\"" + puVO.CityName + "\">" + puVO.CityName + "</td> \r\n";
    oTR += "</tr> \r\n";

    targetCityTable.append(oTR);
}
function DeleteTargetCity() { //删除目标客户区域
    var chkList = $("#TargetCityList").find("tbody input[type='checkbox']:checked");

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
function NewTargetCategory() {//添加客户行业
    onChooseCategory();
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
                            puVO.BusinessId = _BusinessId;
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
function AddTargetCategory(puVO) {
    var targetCategoryTable = $("#TargetCategoryList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"TargetCategory_" + puVO.BusinessId + "_" + puVO.CategoryId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ParentCategoryName + "\">" + puVO.ParentCategoryName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CategoryName + "\">" + puVO.CategoryName + "</td> \r\n";
    oTR += "</tr> \r\n";
	
    targetCategoryTable.append(oTR);
}
function DeleteTargetCategory() {//删除目标客户行业
    var chkList = $("#TargetCategoryList").find("tbody input[type='checkbox']:checked");

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

function savebtn(name) { //保存数据
	switch(name)
	{
		case 'CompanyName':
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"CompanyName": $("input[name*='CompanyName']").val(),
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
		  	UpdateBusiness(seller_data);
		 	Newform_close('CompanyName_A');
		  break;
		case 'CityName':
			var seller_data = {
			  "business": {
			    "BusinessId": _BusinessId,
			    "CustomerId": _CustomerId,
				"ProvinceId": $("select[name*='drpProvince']").val(),
				"CityId": $("select[name*='drpCity']").val()
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
		  	UpdateBusiness(seller_data);
		  	Newform_close('CityName_A');
		  break;
		case 'Address':
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"Address": $("input[name*='Address']").val(),
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
		  	UpdateBusiness(seller_data);
		  	Newform_close('Address_A');
		  break;
		case 'CompanySite':
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"CompanySite": $("input[name*='CompanySite']").val(),
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
		  	UpdateBusiness(seller_data);
		  	Newform_close('CompanySite_A');
		  break;
		case 'Description':
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"Description": $("textarea[name*='Description']").val(),
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
		  	UpdateBusiness(seller_data);
		  	Newform_close('Description_A');
		  break;
		case 'CompanyType':
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"CompanyType": $("input[name*='CompanyType']:checked").val(),
			  }
			}
			console.log(seller_data)
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
		  	UpdateBusiness(seller_data);
		  	Newform_close('CompanyType_A');
		  break;
		case 'CompanyTel':
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"CompanyTel": $("input[name*='CompanyTel']").val(),
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
		  	UpdateBusiness(seller_data);
		  	Newform_close('CompanyTel_A');
		  break;
		case 'BusinessLicense':
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"BusinessLicense": $("input[name*='BusinessLicense']").val(),
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
		  	UpdateBusiness(seller_data);
		  	Newform_close('BusinessLicense_A');
		  break;
		case 'CategoryNames':
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"CompanyName": $("input[name*='CompanyName']").val(),
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
			UpdateBusiness(seller_data);
		    Newform_close('CategoryNames_A');
		  break;
		case 'TCityName':
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"CompanyName": $("input[name*='CompanyName']").val(),
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
			UpdateBusiness(seller_data);
		    Newform_close('TCityName_A');
		  break;
		case 'TCategoryName':
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"CompanyName": $("input[name*='CompanyName']").val(),
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
			UpdateBusiness(seller_data);
		    Newform_close('TCategoryName_A');
		  break;
		case 'CompanyLogo':
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"CompanyLogo": $("#imgCompanyLogoPic").attr("data")
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
			UpdateBusiness(seller_data);
		    Newform_close('CompanyLogo_A');
		  break;
		case 'BusinessLicenseImg':
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"BusinessLicenseImg": $("#imgBusinessLicenseImgPic").attr("data")
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
			UpdateBusiness(seller_data);
		    Newform_close('BusinessLicenseImg_A');
		  break;
		case 'EntrustImgPath':
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"EntrustImgPath": $("#imgEntrustImgPathPic").attr("data")
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
			UpdateBusiness(seller_data);
		    Newform_close('EntrustImgPath_A');
		  break;
		case 'Card':
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"CompanyName": $("input[name*='CompanyName']").val(),
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
			UpdateBusiness(seller_data);
		    Newform_close('Card_A');
		  break;
		  case 'MainProducts':
		    var ue2 = UE.getEditor("container2");
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"MainProducts": ue2.getContent(),
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
			UpdateBusiness(seller_data);
		    Newform_close('MainProducts_A');
		  break;
		  case 'ProductDescription':
		    var ue = UE.getEditor("container");
			var seller_data = {
			  "business": {
				"BusinessId": _BusinessId,
				"CustomerId": _CustomerId,
				"ProductDescription": ue.getContent(),
			  }
			}
			seller_data.BusinessCategory = getbusinessCategoryVOList();
			seller_data.TargetCity = gettargetCityVOList();
			seller_data.TargetCategory = gettargetCategoryVOList();
			seller_data.BusinessIdCard =getBusinessIdCardVOList();
			UpdateBusiness(seller_data);
		    Newform_close('ProductDescription_A');
		  break;
		default:
		break;
	}
}
function getBusinessIdCardVOList(){ //获取身份证VO
	var BusinessIdCardVOList = new Array();
	BusinessIdCardVOList=[{
		"BusinessIDCardId": 0,
		"BusinessId": _BusinessId,
		"IDCardImg": $("#CardImgPic").attr("data")
	},
	{
		"BusinessIDCardId": 0,
		"BusinessId": _BusinessId,
		"IDCardImg": $("#CardImg2Pic").attr("data")
	}];
	return BusinessIdCardVOList;
}
function getbusinessCategoryVOList(){ //获取行业分类VO
	var businessCategoryVOList = new Array();
	var puhidenObjList = $("#BusinessCategoryList").find("input[type='hidden']");
	for (var i = 0; i < puhidenObjList.length; i++) {
		var puhidenObj = $(puhidenObjList[i]);
		var puValue = puhidenObj.val();
		var businessId = puValue.split("_")[1];
		var categoryId = puValue.split("_")[2];
		
		var businessCategoryVO = new Object();
		businessCategoryVOList.push(businessCategoryVO);
		businessCategoryVO.BusinessId = businessId;
		businessCategoryVO.CategoryId = categoryId;
	}	
	return businessCategoryVOList;
}
function gettargetCityVOList(){//获取目标客户区域VO
	var targetCityVOList = new Array();
	var puhidenObjList = $("#TargetCityList").find("input[type='hidden']");
	for (var i = 0; i < puhidenObjList.length; i++) {
		var puhidenObj = $(puhidenObjList[i]);
		var puValue = puhidenObj.val();
		var businessId = puValue.split("_")[1];
		var cityId = puValue.split("_")[2];
		
		var targetCityVO = new Object();
		targetCityVOList.push(targetCityVO);
		targetCityVO.BusinessId = businessId;
		targetCityVO.CityId = cityId;
	}	
	return targetCityVOList;
}
function gettargetCategoryVOList(){//获取目标客户行业VO
	var targetCategoryVOList = new Array();
	var puhidenObjList = $("#TargetCategoryList").find("input[type='hidden']");
	for (var i = 0; i < puhidenObjList.length; i++) {
	    var puhidenObj = $(puhidenObjList[i]);
	    var puValue = puhidenObj.val();
	    var businessId = puValue.split("_")[1];
	    var categoryId = puValue.split("_")[2];

	    var targetCategoryVO = new Object();
	    targetCategoryVOList.push(targetCategoryVO);
	    targetCategoryVO.BusinessId = businessId;
	    targetCategoryVO.CategoryId = categoryId;
	}
	return targetCategoryVOList;
}