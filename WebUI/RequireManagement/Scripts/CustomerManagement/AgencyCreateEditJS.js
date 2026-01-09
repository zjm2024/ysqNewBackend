var _AgencyId = 0;
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
           url: _RootPath + "SPWebAPI/Customer/UpdateAgencyRealNameStatus?token=" + _Token,
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
                               window.location.href = 'AgencyCreateEdit.aspx?page=RealName'
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

function SetAgency(AgencyVO) {
	if(AgencyVO.Status==0){
		$("#Status").html("未认证");	
	}else if(AgencyVO.Status==1){
		$("#Status").html("认证通过");	
	}else if(AgencyVO.Status==2){
		$("#Status").html("认证驳回");	
	}

	if (AgencyVO.RealNameStatus == 0) {
	    $(".RealNameStatus").html("未认证");
	} else if (AgencyVO.RealNameStatus == 1) {
	    $(".RealNameStatus").html("认证通过");
	    $(".RealNamebtn").hide();
	} else if (AgencyVO.RealNameStatus == 2) {
	    $(".RealNameStatus").html("认证驳回");
	} else if (AgencyVO.RealNameStatus == 3) {
	    $(".RealNameStatus").html("审核中");
	}

	if(AgencyVO.AgencyName!=""){
		$("#AgencyName").html(AgencyVO.AgencyName);
		$("input[name=AgencyName]").val(AgencyVO.AgencyName)
	}else{
		$("#AgencyName").html("<font class='red'>(未设置)</font>");
	}
	
	if(AgencyVO.CityName!=""){
		$("#CityName").html(AgencyVO.CityName);
		BindCity(AgencyVO.ProvinceId,AgencyVO.CityId);
	} else {
	    BindCity(1, 1);
		$("#CityName").html("<font class='red'>(未设置)</font>");
	}
	
	if(AgencyVO.School!=""){
		$("#School").html(AgencyVO.School);
		$("input[name=School]").val(AgencyVO.School)
	}else{
		$("#School").html("<font class='red'>(未设置)</font>");
	}
	
	if(AgencyVO.FamiliarProduct!=""){
		$("#FamiliarProduct").html(AgencyVO.FamiliarProduct);
		$("textarea[name=FamiliarProduct]").val(AgencyVO.FamiliarProduct)
	}else{
		$("#FamiliarProduct").html("<font class='red'>(未设置)</font>");
	}
	
	if(AgencyVO.Specialty!=""){
		$("#Specialty").html(AgencyVO.Specialty);
		$("textarea[name=Specialty]").val(AgencyVO.Specialty)
	}else{
		$("#Specialty").html("<font class='red'>(未设置)</font>");
	}
	
	if(AgencyVO.Feature!=""){
		$("#Feature").html(AgencyVO.Feature);
		$("textarea[name=Feature]").val(AgencyVO.Feature)
	}else{
		$("#Feature").html("<font class='red'>(未设置)</font>");
	}
	
	if(AgencyVO.Company!=""){
		$("#Company").html(AgencyVO.Company);
		$("input[name=Company]").val(AgencyVO.Company)
	}else{
		$("#Company").html("<font class='red'>(未设置)</font>");
	}
	
	if(AgencyVO.Position!=""){
		$("#Position").html(AgencyVO.Position);
		$("input[name=Position]").val(AgencyVO.Position)
	}else{
		$("#Position").html("<font class='red'>(未设置)</font>");
	}
	
	var Description=RemoveHtml(AgencyVO.Description);
	if(Description!=""){
		$("#Description").html(Description); 
		var ue3 = UE.getEditor("container3");
		ue3.ready(function () {
			this.setContent(AgencyVO.Description);
		});
	}else{
		$("#Description").html("<font class='red'>(未设置)</font>");
	}
	
	if(AgencyVO.ShortDescription!=""){
		$("#ShortDescription").html(AgencyVO.ShortDescription);
		$("input[name=ShortDescription]").val(AgencyVO.ShortDescription)
	}else{
		$("#ShortDescription").html("<font class='red'>(未设置)</font>");
	}
	
	if(AgencyVO.IDCard!=""){
		$("#IDCard").html(AgencyVO.IDCard);
		$("input[name=IDCard]").val(AgencyVO.IDCard);
	}else{
		$("#IDCard").html("<font class='red'>(未设置)</font>");
	}
	
	if(AgencyVO.PersonalCard!=""){
		$("#PersonalCard").html("<img src='"+AgencyVO.PersonalCard+"' class='fromimg'>");
		$("#imgPersonalCardPic").attr("data",AgencyVO.PersonalCard);
		$("#imgPersonalCardPic").attr("style","background-image:url("+AgencyVO.CompanyLogo+")");
	}else{
		$("#PersonalCard").html("<font class='red'>(未设置)</font>");
		$("#imgPersonalCardPic").attr("style", "background-image:url(../Style/images/upimg.png)");
	}
	
	var TargetCategory = RemoveComma(AgencyVO.TargetCategory);
	if(TargetCategory!=""){
		$("#AgencyCategory").html(TargetCategory);
	}else{
		$("#AgencyCategory").html("<font class='red'>(未设置)</font>");
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
                _AgencyId = customerVO.AgencyId;
                $.ajax({
                    url: _RootPath + "SPWebAPI/Customer/GetAgency?agencyId=" + _AgencyId + "&token=" + _Token,
                    type: "Get",
                    data: null,
                    success: function (data) {
                        if (data.Flag == 1) {
                            var AgencyVO = data.Result;
                            SetAgency(AgencyVO);

                            //获取擅长行业
                            $("#AgencyCategoryList .ui-widget-content").remove();
                            BindTargetCategory(_AgencyId);

                            //获取优势区域
                            $("#AgencyCityList .ui-widget-content").remove();
                            BindTargetCity(_AgencyId);

                            //获取身份证图片
                            BindCardByBusiness(_AgencyId);

                            //获取优势客户
                            $("#AgencySuperClientList .ui-widget-content").remove();
                            BindAgencySuperClient(_AgencyId);

                            //获取典型案例
                            $("#AgencySolutionList .ui-widget-content").remove();
                            BindAgencySolution(_AgencyId);
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
                    url: _RootPath + "SPWebAPI/Customer/GetAgencyCompleted?agencyId=" + _AgencyId + "&token=" + _Token,
                    type: "Get",
                    data: null,
                    success: function (data) {
                        if (data.Flag == 1) {
                           $("#Completed").html(data.Result+"%");
                        }
                    },
                    error: function (data) {
                        alert(data);
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
function BindTargetCategory(agencyId) { //获取擅长行业
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetAgencyCategoryByAgency?agencyId=" + agencyId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
				if(puVOList.length>0){
					var text="";
					for (var i = 0; i < puVOList.length; i++) {
						var puVO = puVOList[i];
                    	AddAgencyCategory(puVO);
					}
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
function BindTargetCity(AgencyId) {//获取优势区域
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetAgencyCityByAgency?agencyId=" + AgencyId + "&token=" + _Token,
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
                    	AddAgencyCity(puVO);
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
function BindCardByBusiness(agencyId) {//获取身份证图片
	$.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetAgencyIdCardByAgency?agencyId=" + agencyId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
				if(puVOList.length>0){
					var text="";
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

function BindAgencySuperClient(agencyId) {//优势客户
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetAgencySuperClientByAgency?agencyId=" + agencyId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
				var text="";
				for (var i = 0; i < puVOList.length; i++) {
					text+= puVOList[i].SuperClientName+"，";
					var puVO = puVOList[i];
                    AddAgencySuperClient(puVO);
				}
				if(text!=""&&text!="，")
				$("#newagencysuperclient").html(text);

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
            //alert(data);
        }
    });
}

function AddAgencySuperClient(puVO) {//优势客户
    var agencySuperClientTable = $("#AgencySuperClientList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"TargetSuperClient_" + puVO.AgencyId + "_" + puVO.AgencySuperClientId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.SuperClientName + "\">" + puVO.SuperClientName + "</td> \r\n";
    //oTR += "  <td class=\"center\" title=\"" + puVO.CompanyName + "\">" + puVO.CompanyName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.Description + "\">" + puVO.Description + "</td> \r\n";
    oTR += "</tr> \r\n";

    agencySuperClientTable.append(oTR);
}

function NewAgencySuperClient() {//优势客户

    onChooseAgencySuperClient();
}

function DeleteAgencySuperClient() {//优势客户
    var chkList = $("#AgencySuperClientList").find("tbody input[type='checkbox']:checked");

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

function onChooseAgencySuperClient() { //优势客户
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 40%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/CustomerManagement\/AgencySuperClientCreateEdit.aspx" height="350px" width="100%" frameborder="0"><\/iframe>',

        title: "添加优势客户",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {

                    var txtSuperClientNameObj = $(window.frames["iframe_1"].document).find("input[id*='txtSuperClientName']");
                    //var txtCompanyNameObj = $(window.frames["iframe_1"].document).find("input[id*='txtCompanyName']");
                    var txtDescriptionObj = $(window.frames["iframe_1"].document).find("textarea[id*='txtDescription']");

                    if (txtSuperClientNameObj.val() == "") {
                        bootbox.dialog({
                            message: "请输入客户名称?",
                            buttons:
                            {
                                "click":
                                {
                                    "label": "确定",
                                    "className": "btn-sm btn-primary",
                                    "callback": function () {
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
                    //else if (txtCompanyNameObj.val() == "") {
                    //    bootbox.dialog({
                    //        message: "请输入公司名称?",
                    //        buttons:
                    //        {
                    //            "click":
                    //            {
                    //                "label": "确定",
                    //                "className": "btn-sm btn-primary",
                    //                "callback": function () {
                    //                }
                    //            },
                    //            "Cancel":
                    //            {
                    //                "label": "取消",
                    //                "className": "btn-sm",
                    //                "callback": function () {
                    //                }
                    //            }
                    //        }
                    //    });
                    //    return false;
                    //}

                    var puVO = new Object();
                    puVO.AgencySuperClientId = -1;
                    puVO.AgencyId = _AgencyId;
                    puVO.SuperClientName = txtSuperClientNameObj.val();
                    puVO.CompanyName = "";
                    puVO.Description = txtDescriptionObj.val();

                    var isContain = false;
                    var puhidenObjList = $("#AgencySuperClientList").find("input[type='hidden']");
                    for (var i = 0; i < puhidenObjList.length; i++) {
                        var puhidenObj = $(puhidenObjList[i]);

                        var superClientName = puhidenObj.parent().next().html();
                        if (superClientName == puVO.SuperClientName) {
                            isContain = true;
                            break;
                        }
                    }
                    if (!isContain)
                        AddAgencySuperClient(puVO);

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

function NewAgencyCategory() { //擅长行业
    onChooseAgencyCategory();
}

function onChooseAgencyCategory() { //擅长行业
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
                        var puhidenObjList = $("#AgencyCategoryList").find("input[type='hidden']");
                        for (var i = 0; i < categoryList.length; i++) {
                            var categoryObj = categoryList[i];
                            var puVO = new Object();
                            puVO.AgencyId = _AgencyId;
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
                                AddAgencyCategory(puVO);
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
function AddAgencyCategory(puVO) { //擅长行业
    var agencyCategoryTable = $("#AgencyCategoryList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"TargetCategory_" + puVO.AgencyId + "_" + puVO.CategoryId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ParentCategoryName + "\">" + puVO.ParentCategoryName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CategoryName + "\">" + puVO.CategoryName + "</td> \r\n";
    oTR += "</tr> \r\n";

    agencyCategoryTable.append(oTR);
}
function DeleteAgencyCategory() { //擅长行业
    var chkList = $("#AgencyCategoryList").find("tbody input[type='checkbox']:checked");

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
function AddAgencyCity(puVO) { //优势区域
    var agencyCityTable = $("#AgencyCityList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"AgencyCity_" + puVO.AgencyId + "_" + puVO.CityId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ProvinceName + "\">" + puVO.ProvinceName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.CityName + "\">" + puVO.CityName + "</td> \r\n";
    oTR += "</tr> \r\n";

    agencyCityTable.append(oTR);
}

function NewAgencyCity() {//优势区域
    onChooseCity();
}

function DeleteAgencyCity() {//优势区域
    var chkList = $("#AgencyCityList").find("tbody input[type='checkbox']:checked");

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

function onChooseCity() {//优势区域
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
                        var puhidenObjList = $("#AgencyCityList").find("input[type='hidden']");
                        for (var i = 0; i < cityList.length; i++) {
                            var cityObj = cityList[i];
                            var puVO = new Object();
                            puVO.AgencyId = _AgencyId;
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
                                AddAgencyCity(puVO);
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
function BindAgencySolution(agencyId) {//典型案例
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetAgencySolutionByAgency?agencyId=" + agencyId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                console.log(data.Result);
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
					var text="";
					for (var i = 0; i < puVOList.length; i++) {
						text+= puVOList[i].ProjectName+"，";
						var puVO = puVOList[i];
                    	AddAgencySolution(puVO);
					}
					if(text!=""&&text!="，")
					$("#newagencysolution").html(text);
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

function AddAgencySolution(puVO) {//典型案例
    var agencySolutionTable = $("#AgencySolutionList");
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"TargetSolution_" + puVO.AgencyId + "_" + puVO.AgencySolutionId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ClientName + "\">" + puVO.ClientName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ProjectName + "\">" + puVO.ProjectName + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.Cost + "\">" + puVO.Cost + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.ProjectDate + "\">" + puVO.ProjectDate + "</td> \r\n";
    oTR += "  <td class=\"center\" > \r\n";
    for (var i = 0; i < puVO.AgencySolutionFileList.length; i++) {
        oTR += "<a target=\"_blank\" href=\"" + puVO.AgencySolutionFileList[i].FilePath + "\">" + puVO.AgencySolutionFileList[i].FileName + "</a>";
    }
    oTR += "  </td> \r\n";

    var PrivacyType=""
    if (puVO.PrivacyType == 0) {
        PrivacyType = "公开";
    } else if (puVO.PrivacyType == 1) {
        PrivacyType = "关键字过滤(" + puVO.Keyword + ")";
    } else if (puVO.PrivacyType == 2) {
        PrivacyType = "保密";
    }
    oTR += "  <td class=\"center\" title=\"" + puVO.PrivacyType + "\" Keyword=\"" + puVO.Keyword + "\">" + PrivacyType + "</td> \r\n";
    oTR += "</tr> \r\n";

    agencySolutionTable.append(oTR);
}

function NewAgencySolution() {//典型案例

    onChooseAgencySolution();
}

function DeleteAgencySolution() {//典型案例
    var chkList = $("#AgencySolutionList").find("tbody input[type='checkbox']:checked");

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
function myagencysolution() {//自动关联我的销售案例
    var filterModel = new Object();
    var filterObj = new Object();
    var pageInfoObj = new Object();

    filterModel.Filter = filterObj;
    filterModel.PageInfo = pageInfoObj;

    pageInfoObj.PageIndex = 1;
    pageInfoObj.PageCount = 50;
    pageInfoObj.SortName = "ProjectDate";
    pageInfoObj.SortType = "desc";

    filterObj.groupOp = "and";
    var rulesObj = new Array();
    filterObj.rules = rulesObj;
    var filterArray = new Array();
    filterObj.filter = filterArray;

    var ruleObj = new Object();
    rulesObj.push(ruleObj);

    ruleObj.field = "1";
    ruleObj.op = "eq";
    ruleObj.data = "1";

    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetAgencyExperienceList?token=" + _Token,
        type: "post",
        data: filterModel,
        success: function (data) {
            console.log(data);
            if (data.Flag == 1 && data.Result.length>0) {
                for (var i = 0; i < data.Result.length; i++)
                {
                    var puVO = new Object();
                    puVO.AgencySolutionId = -1;
                    puVO.AgencyId = _AgencyId;
                    puVO.ClientName = data.Result[i].ClientName;
                    puVO.ProjectName = data.Result[i].Title;
                    puVO.ProjectDate = data.Result[i].ProjectDate;
                    puVO.Cost = data.Result[i].ContractAmount;
                    puVO.PrivacyType = 0;
                    puVO.Keyword = "";

                    var agencySolutionFileList = new Array();
                    puVO.AgencySolutionFileList = agencySolutionFileList;
                    AddAgencySolution(puVO);
                }
            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}
function onChooseAgencySolution() {//典型案例
    bootbox.dialog({
        message:
        '<style type="text\/css">.modal-dialog{width: 40%; top: 10%;}<\/style>' +
        '<style type="text\/css">.modal-body{padding: 5px; !important}<\/style>' +
                '<style type="text\/css">.modal-footer{margin-top: 1px; !important}<\/style>' +
        '<style type="text\/css">.main-content{padding: 0px !important;} .bootbox-body{padding-top: 0px !important; padding-bottom: 0px !important;}<\/style>' +
        '<iframe id="iframe_1" name="iframe_1" src="..\/CustomerManagement\/AgencySolutionCreateEdit.aspx" height="400px" width="100%" frameborder="0"><\/iframe>',

        title: "添加典型案例",
        buttons:
        {
            "Confirm":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {

                    var txtClientNameObj = $(window.frames["iframe_1"].document).find("input[id*='txtClientName']");
                    var txtProjectNameObj = $(window.frames["iframe_1"].document).find("input[id*='txtProjectName']");
                    var txtProjectDateObj = $(window.frames["iframe_1"].document).find("input[id*='txtProjectDate']");
                    var txtCostObj = $(window.frames["iframe_1"].document).find("input[id*='txtCost']");
                    var txtPrivacyTypeObj = $(window.frames["iframe_1"].document).find("input[name*='PrivacyType']:checked");
                    var KeywordObj = $(window.frames["iframe_1"].document).find("input[id*='Keyword']");

                    if (txtClientNameObj.val() == "") {
                        bootbox.dialog({
                            message: "请输入客户名称?",
                            buttons:
                            {
                                "click":
                                {
                                    "label": "确定",
                                    "className": "btn-sm btn-primary",
                                    "callback": function () {
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
                    }else if (txtProjectNameObj.val() == ""){
                        bootbox.dialog({
                            message: "请输入项目名称?",
                            buttons:
                            {
                                "click":
                                {
                                    "label": "确定",
                                    "className": "btn-sm btn-primary",
                                    "callback": function () {
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
                    } else if (txtCostObj.val() == "") {
                        bootbox.dialog({
                            message: "请输入项目金额?",
                            buttons:
                            {
                                "click":
                                {
                                    "label": "确定",
                                    "className": "btn-sm btn-primary",
                                    "callback": function () {
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

                    var puVO = new Object();
                    puVO.AgencySolutionId = -1;
                    puVO.AgencyId = _AgencyId;
                    puVO.ClientName = txtClientNameObj.val();
                    puVO.ProjectName = txtProjectNameObj.val();
                    puVO.ProjectDate = txtProjectDateObj.val();
                    puVO.Cost = txtCostObj.val();
                    puVO.PrivacyType = txtPrivacyTypeObj.val();
                    puVO.Keyword = KeywordObj.val();

                    var agencySolutionFileList = new Array();
                    puVO.AgencySolutionFileList = agencySolutionFileList;

                    var puhidenObjList = $(window.frames["iframe_1"].document).find("table[id*='FileList']").find("input[type='hidden']");
                    for (var i = 0; i < puhidenObjList.length; i++) {

                        var agencySolutionFileVO = new Object();
                        agencySolutionFileList.push(agencySolutionFileVO);


                        var puhidenObj = $(puhidenObjList[i]);
                        var puValue = puhidenObj.val();
                        var agencySolutionId = puValue.split("_")[1];
                        var agencySolutionFileId = puValue.split("_")[2];
                        
                        agencySolutionFileVO.AgencySolutionId = agencySolutionId;
                        agencySolutionFileVO.AgencySolutionFileId = agencySolutionFileId;
                        agencySolutionFileVO.FileName = puhidenObj.parent().next().find("a").html();
                        agencySolutionFileVO.FilePath = puhidenObj.parent().next().find("a").attr("href");
                    }

                    var isContain = false;
                    var puhidenObjList = $("#AgencySolutionList").find("input[type='hidden']");
                    for (var i = 0; i < puhidenObjList.length; i++) {
                        var puhidenObj = $(puhidenObjList[i]);

                        var projectName = puhidenObj.parent().next().next().html();
                        if (projectName == puVO.ProjectName) {
                            isContain = true;
                            break;
                        }
                    }

                    if (!isContain)
                    AddAgencySolution(puVO);

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

function UpdateAgency(seller_data) //更新到服务器
{
	$.ajax({
            url: _RootPath + "SPWebAPI/Customer/UpdateAgency?token=" + _Token,
            type: "POST",
            data: seller_data,
            success: function (data) {
                if (data.Flag == 1) {
                    if (_AgencyId < 1) {
                        _AgencyId = data.Result;
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
                alert(data);
            }
        });
}
function savebtn(name) { //保存数据
    switch(name)
    {
        case 'AgencyName':
            var seller_data = {
                "Agency": {
                    "AgencyId": _AgencyId,
                    "CustomerId": _CustomerId,
                    "AgencyName": $("input[name*='AgencyName']").val(),
                }
            }
            seller_data.AgencyCategory=getagencyCategoryVOList();
            seller_data.AgencyCity=getagencyCityVOList();
            seller_data.AgencySuperClient=getagencySuperClientVOList();
            seller_data.AgencySolution=getagencySolutionVOList();
            seller_data.AgencyIdCard=getagencyIdCardList();
            UpdateAgency(seller_data);
            Newform_close('AgencyName_A');
            break;
        case 'CityName':
            var seller_data = {
                "Agency": {
                    "AgencyId": _AgencyId,
                    "CustomerId": _CustomerId,
                    "ProvinceId": $("select[name*='drpProvince']").val(),
                    "CityId": $("select[name*='drpCity']").val()
                }
            }
            seller_data.AgencyCategory=getagencyCategoryVOList();
            seller_data.AgencyCity=getagencyCityVOList();
            seller_data.AgencySuperClient=getagencySuperClientVOList();
            seller_data.AgencySolution=getagencySolutionVOList();
            seller_data.AgencyIdCard=getagencyIdCardList();
            UpdateAgency(seller_data);
            Newform_close('CityName_A');
            break;
        case 'AgencyCategory':
            var seller_data = {
                "Agency": {
                    "AgencyId": _AgencyId,
                    "CustomerId": _CustomerId,
                    "AgencyName": $("input[name*='AgencyName']").val(),
                },
			}
			seller_data.AgencyCategory=getagencyCategoryVOList();
			seller_data.AgencyCity=getagencyCityVOList();
			seller_data.AgencySuperClient=getagencySuperClientVOList();
			seller_data.AgencySolution=getagencySolutionVOList();
			seller_data.AgencyIdCard=getagencyIdCardList();
		  	UpdateAgency(seller_data);
		  	Newform_close('AgencyCategory_A');
		  break;
		case 'TCityName':
			var seller_data = {
			  "Agency": {
			    "AgencyId": _AgencyId,
			    "CustomerId": _CustomerId,
				"AgencyName": $("input[name*='AgencyName']").val(),
			  }
			}
			seller_data.AgencyCategory=getagencyCategoryVOList();
			seller_data.AgencyCity=getagencyCityVOList();
			seller_data.AgencySuperClient=getagencySuperClientVOList();
			seller_data.AgencySolution=getagencySolutionVOList();
			seller_data.AgencyIdCard=getagencyIdCardList();
		  	UpdateAgency(seller_data);
		  	Newform_close('TCityName_A');
		  break;
		case 'School':
			var seller_data = {
			  "Agency": {
				"AgencyId": _AgencyId,
				"CustomerId": _CustomerId,
				"School": $("input[name*='School']").val()
			  }
			}
			seller_data.AgencyCategory=getagencyCategoryVOList();
			seller_data.AgencyCity=getagencyCityVOList();
			seller_data.AgencySuperClient=getagencySuperClientVOList();
			seller_data.AgencySolution=getagencySolutionVOList();
			seller_data.AgencyIdCard=getagencyIdCardList();
		  	UpdateAgency(seller_data);
		  	Newform_close('School_A');
		  break;
		case 'FamiliarProduct':
			var seller_data = {
			  "Agency": {
				"AgencyId": _AgencyId,
				"CustomerId": _CustomerId,
				"FamiliarProduct": $("textarea[name*='FamiliarProduct']").val()
			  }
			}
			seller_data.AgencyCategory=getagencyCategoryVOList();
			seller_data.AgencyCity=getagencyCityVOList();
			seller_data.AgencySuperClient=getagencySuperClientVOList();
			seller_data.AgencySolution=getagencySolutionVOList();
			seller_data.AgencyIdCard=getagencyIdCardList();
		  	UpdateAgency(seller_data);
		  	Newform_close('FamiliarProduct_A');
		  break;
		case 'newagencysuperclient':
			var seller_data = {
			  "Agency": {
				"AgencyId": _AgencyId,
				"CustomerId": _CustomerId,
				"AgencyName": $("input[name*='AgencyName']").val(),
			  }
			}
			seller_data.AgencyCategory=getagencyCategoryVOList();
			seller_data.AgencyCity=getagencyCityVOList();
			seller_data.AgencySuperClient=getagencySuperClientVOList();
			seller_data.AgencySolution=getagencySolutionVOList();
			seller_data.AgencyIdCard=getagencyIdCardList();
		  	UpdateAgency(seller_data);
		  	Newform_close('newagencysuperclient_A');
		  break;
		case 'newagencysolution':
			var seller_data = {
			  "Agency": {
				"AgencyId": _AgencyId,
				"CustomerId": _CustomerId,
				"AgencyName": $("input[name*='AgencyName']").val(),
			  }
			}
			seller_data.AgencyCategory=getagencyCategoryVOList();
			seller_data.AgencyCity=getagencyCityVOList();
			seller_data.AgencySuperClient=getagencySuperClientVOList();
			seller_data.AgencySolution=getagencySolutionVOList();
			seller_data.AgencyIdCard=getagencyIdCardList();
		  	UpdateAgency(seller_data);
		  	Newform_close('newagencysolution_A');
		  break;
		case 'Specialty':
			var seller_data = {
			  "Agency": {
				"AgencyId": _AgencyId,
				"CustomerId": _CustomerId,
				"Specialty": $("textarea[name*='Specialty']").val(),
			  }
			}
			seller_data.AgencyCategory=getagencyCategoryVOList();
			seller_data.AgencyCity=getagencyCityVOList();
			seller_data.AgencySuperClient=getagencySuperClientVOList();
			seller_data.AgencySolution=getagencySolutionVOList();
			seller_data.AgencyIdCard=getagencyIdCardList();
		  	UpdateAgency(seller_data);
		  	Newform_close('Specialty_A');
		  break;
		case 'Feature':
			var seller_data = {
			  "Agency": {
				"AgencyId": _AgencyId,
				"CustomerId": _CustomerId,
				"Feature": $("textarea[name*='Feature']").val(),
			  }
			}
			seller_data.AgencyCategory=getagencyCategoryVOList();
			seller_data.AgencyCity=getagencyCityVOList();
			seller_data.AgencySuperClient=getagencySuperClientVOList();
			seller_data.AgencySolution=getagencySolutionVOList();
			seller_data.AgencyIdCard=getagencyIdCardList();
		  	UpdateAgency(seller_data);
		  	Newform_close('Feature_A');
		  break;
		case 'Company':
			var seller_data = {
			  "Agency": {
				"AgencyId": _AgencyId,
				"CustomerId": _CustomerId,
				"Company": $("input[name*='Company']").val(),
			  }
			}
			seller_data.AgencyCategory=getagencyCategoryVOList();
			seller_data.AgencyCity=getagencyCityVOList();
			seller_data.AgencySuperClient=getagencySuperClientVOList();
			seller_data.AgencySolution=getagencySolutionVOList();
			seller_data.AgencyIdCard=getagencyIdCardList();
		  	UpdateAgency(seller_data);
		  	Newform_close('Company_A');
		  break;
		case 'Position':
			var seller_data = {
			  "Agency": {
				"AgencyId": _AgencyId,
				"CustomerId": _CustomerId,
				"Position": $("input[name*='Position']").val(),
			  }
			}
			seller_data.AgencyCategory=getagencyCategoryVOList();
			seller_data.AgencyCity=getagencyCityVOList();
			seller_data.AgencySuperClient=getagencySuperClientVOList();
			seller_data.AgencySolution=getagencySolutionVOList();
			seller_data.AgencyIdCard=getagencyIdCardList();
		  	UpdateAgency(seller_data);
		  	Newform_close('Position_A');
		  break;
		case 'PersonalCard':
			var seller_data = {
			  "Agency": {
				"AgencyId": _AgencyId,
				"CustomerId": _CustomerId,
				"PersonalCard": $("#imgPersonalCardPic").attr("data"),
			  }
			}
			seller_data.AgencyCategory=getagencyCategoryVOList();
			seller_data.AgencyCity=getagencyCityVOList();
			seller_data.AgencySuperClient=getagencySuperClientVOList();
			seller_data.AgencySolution=getagencySolutionVOList();
			seller_data.AgencyIdCard=getagencyIdCardList();
		  	UpdateAgency(seller_data);
		  	Newform_close('PersonalCard_A');
		  break;
		case 'Description':
		 	var ue3 = UE.getEditor("container3");
			var seller_data = {
			  "Agency": {
				"AgencyId": _AgencyId,
				"CustomerId": _CustomerId,
				"Description": ue3.getContent(),
			  }
			}
			seller_data.AgencyCategory=getagencyCategoryVOList();
			seller_data.AgencyCity=getagencyCityVOList();
			seller_data.AgencySuperClient=getagencySuperClientVOList();
			seller_data.AgencySolution=getagencySolutionVOList();
			seller_data.AgencyIdCard=getagencyIdCardList();
		  	UpdateAgency(seller_data);
		  	Newform_close('Description_A');
		  break;
		case 'ShortDescription':
			var seller_data = {
			  "Agency": {
				"AgencyId": _AgencyId,
				"CustomerId": _CustomerId,
				"ShortDescription": $("input[name*='ShortDescription']").val(),
			  }
			}
			seller_data.AgencyCategory=getagencyCategoryVOList();
			seller_data.AgencyCity=getagencyCityVOList();
			seller_data.AgencySuperClient=getagencySuperClientVOList();
			seller_data.AgencySolution=getagencySolutionVOList();
			seller_data.AgencyIdCard=getagencyIdCardList();
		  	UpdateAgency(seller_data);
		  	Newform_close('ShortDescription_A');
		  break;
		case 'IDCard':
			var seller_data = {
			  "Agency": {
				"AgencyId": _AgencyId,
				"CustomerId": _CustomerId,
				"IDCard": $("input[name*='IDCard']").val(),
			  }
			}
			seller_data.AgencyCategory=getagencyCategoryVOList();
			seller_data.AgencyCity=getagencyCityVOList();
			seller_data.AgencySuperClient=getagencySuperClientVOList();
			seller_data.AgencySolution=getagencySolutionVOList();
			seller_data.AgencyIdCard=getagencyIdCardList();
		  	UpdateAgency(seller_data);
		  	Newform_close('IDCard_A');
		  break;
		case 'Card':
			var seller_data = {
			  "Agency": {
				"AgencyId": _AgencyId,
				"CustomerId": _CustomerId,
				"AgencyName": $("input[name*='AgencyName']").val(),
			  }
			}
			seller_data.AgencyCategory=getagencyCategoryVOList();
			seller_data.AgencyCity=getagencyCityVOList();
			seller_data.AgencySuperClient=getagencySuperClientVOList();
			seller_data.AgencySolution=getagencySolutionVOList();
			seller_data.AgencyIdCard=getagencyIdCardList();
		  	UpdateAgency(seller_data);
		  	Newform_close('Card_A');
		  break;
		default:
		break;
	}
}
function getagencySolutionVOList(){ //获取典型案例VO
	var agencySolutionVOList = new Array();
    var puhidenObjList = $("#AgencySolutionList").find("input[type='hidden']");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);
        var puValue = puhidenObj.val();
        var agencyId = puValue.split("_")[1];
        var agencySolutionId = puValue.split("_")[2];

        var agencySolutionVO = new Object();
        agencySolutionVOList.push(agencySolutionVO);
        agencySolutionVO.AgencyId = agencyId;
        agencySolutionVO.AgencySolutionId = agencySolutionId;
        agencySolutionVO.ClientName = puhidenObj.parent().next().html();
        agencySolutionVO.ProjectName = puhidenObj.parent().next().next().html();
        agencySolutionVO.Cost = puhidenObj.parent().next().next().next().html();
        agencySolutionVO.ProjectDate = puhidenObj.parent().next().next().next().next().html();
        agencySolutionVO.PrivacyType = puhidenObj.parent().next().next().next().next().next().next().attr("title");
        agencySolutionVO.Keyword = puhidenObj.parent().next().next().next().next().next().next().attr("Keyword");
        var fileList = puhidenObj.parent().next().next().next().next().next().find("a");
        var agencySolutionFileVOList = new Array();
        agencySolutionVO.AgencySolutionFileList = agencySolutionFileVOList;
        for (var j = 0; j < fileList.length; j++) {
            var fileVO = new Object();
            agencySolutionFileVOList.push(fileVO);

            fileVO.AgencySolutionId = agencySolutionId;
            fileVO.FileName = $(fileList[j]).html();
            fileVO.FilePath = $(fileList[j]).attr("href");
        }
    }
	return agencySolutionVOList;
}
function getagencySuperClientVOList(){ //获取优势客户VO
	var agencySuperClientVOList = new Array();
    var puhidenObjList = $("#AgencySuperClientList").find("input[type='hidden']");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);
        var puValue = puhidenObj.val();
        var agencyId = puValue.split("_")[1];
        var agencySuperClientId = puValue.split("_")[2];

        var agencySuperClientVO = new Object();
        agencySuperClientVOList.push(agencySuperClientVO);
        agencySuperClientVO.AgencyId = agencyId;
        agencySuperClientVO.AgencySuperClientId = agencySuperClientId;
        agencySuperClientVO.SuperClientName = puhidenObj.parent().next().html();
        //agencySuperClientVO.CompanyName = puhidenObj.parent().next().next().html();
        agencySuperClientVO.Description = puhidenObj.parent().next().next().html();
    }
	console.log(agencySuperClientVOList);
	return agencySuperClientVOList;
}
function getagencyCategoryVOList(){ //获取擅长行业VO
	var agencyCategoryVOList = new Array();
    var puhidenObjList = $("#AgencyCategoryList").find("input[type='hidden']");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);
        var puValue = puhidenObj.val();
        var agencyId = puValue.split("_")[1];
        var categoryId = puValue.split("_")[2];

        var agencyCategoryVO = new Object();
        agencyCategoryVOList.push(agencyCategoryVO);
        agencyCategoryVO.AgencyId = agencyId;
        agencyCategoryVO.CategoryId = categoryId;
    }
	return agencyCategoryVOList;
}
function getagencyCityVOList(){ //获取擅长行业VO
	var agencyCityVOList = new Array();
    var puhidenObjList = $("#AgencyCityList").find("input[type='hidden']");
    for (var i = 0; i < puhidenObjList.length; i++) {
        var puhidenObj = $(puhidenObjList[i]);
        var puValue = puhidenObj.val();
        var agencyId = puValue.split("_")[1];
        var cityId = puValue.split("_")[2];

        var agencyCityVO = new Object();
        agencyCityVOList.push(agencyCityVO);
        agencyCityVO.AgencyId = agencyId;
        agencyCityVO.CityId = cityId;
    }
	return agencyCityVOList;
}
function getagencyIdCardList(){ //获取身份证VO
	var agencyIdCardList = new Array();
	agencyIdCardList=[{
		"AgencyId": _AgencyId,
		"IDCardImg": $("#CardImgPic").attr("data")
	},
	{
		"AgencyId": _AgencyId,
		"IDCardImg": $("#CardImg2Pic").attr("data")
	}];
	return agencyIdCardList;
}