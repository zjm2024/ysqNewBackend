$(document).ready(function () {
    Init();  

    $.validator.addMethod("isTel", function (value, element, params) {
        var length = value.length;
        var mobile = /^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/;
        var tel = /^(\d{3,4}-?)?\d{7,9}$/g;
        return this.optional(element) || tel.test(value) || (length == 11 && mobile.test(value)) || length == 0;
    }, "请输入正确格式的电话");   

    $.validator.addMethod("email", function (value, element, params) {
        var ema = /^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$/;
        return this.optional(element) || (ema.test(value));
    }, "请输入正确格式的电子邮箱");

    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtMessageNotiCount: {
                number: true
            },
            ctl00$ContentPlaceHolder_Content$txtMessageNotiPhone: {
                isTel: true
            },
            ctl00$ContentPlaceHolder_Content$txtCommissionPercentage: {
                number: true
            },
            ctl00$ContentPlaceHolder_Content$txtCommissionTotal: {
                number: true
            },
            ctl00$ContentPlaceHolder_Content$txtProjectTotal: {
                number: true
            },
            ctl00$ContentPlaceHolder_Content$txtSiteName: {
                required: true
            }
            ,
            ctl00$ContentPlaceHolder_Content$txtServicePhone: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtServiceNote: {
                required: true
            }
            ,
            ctl00$ContentPlaceHolder_Content$txtViewAgencyCount: {
                number: true
            }
            ,
            ctl00$ContentPlaceHolder_Content$txtViewBusinessCount: {
                number: true
            }
            ,
            ctl00$ContentPlaceHolder_Content$FirstMandates: {
                number: true
            }
        },
        messages: {

            ctl00$ContentPlaceHolder_Content$txtMessageNotiCount: {
                number: "请输入正确格式数值！"
            },
            ctl00$ContentPlaceHolder_Content$txtMessageNotiPhone: {
                isTel: "请输入正确格式的电话！"
            },
            ctl00$ContentPlaceHolder_Content$txtCommissionPercentage: {
                number: "请输入正确格式数值！"
            },
            ctl00$ContentPlaceHolder_Content$txtCommissionTotal: {
                number: "请输入正确格式数值！"
            },
            ctl00$ContentPlaceHolder_Content$txtProjectTotal: {
                number: "请输入正确格式数值！"
            },
            ctl00$ContentPlaceHolder_Content$txtSiteName: {
                required: "请输入网站名称！"
            }
            ,
            ctl00$ContentPlaceHolder_Content$txtServicePhone: {
                required: "请输入客服联系方式！"
            },
            ctl00$ContentPlaceHolder_Content$txtServiceNote: {
                required: "请输入客服标语!"
            },
            ctl00$ContentPlaceHolder_Content$txtViewAgencyCount: {
                number: "请输入正确格式数值！"
            }
            ,
            ctl00$ContentPlaceHolder_Content$FirstMandates: {
                number: "请输入正确格式数值！"
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

        var configId = parseInt($("#" + hidConfigId).val());
        var configVO = GetConfigVO();

        if (configVO.APPImage == "") {
            bootbox.dialog({
                message: "请上传APP二维码",
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
            return false;
        }
        if (configVO.GZImage == "") {
            bootbox.dialog({
                message: "请上传公众号二维码",
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
            return false;
        }

        $.ajax({
            url: _RootPath + "SPWebAPI/System/UpdateConfig?token=" + _Token,
            type: "POST",
            data: configVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (configId < 1) {
                        $("#" + hidConfigId).val(data.Result);
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
          
    
});


function GetConfigVO() {
    var configVO = new Object();

	var objMessageAPI = $("input[id*='txtMessageAPI']");
	var objMessageNotiCount = $("input[id*='txtMessageNotiCount']");
	var objMessageNotiPhone = $("input[id*='txtMessageNotiPhone']");
	var objIsNeedApprove = $("input[id*='chk_IsNeedApprove']:checked");
	var objCommissionPercentage = $("input[id*='txtCommissionPercentage']");
	var objCommissionTotal = $("input[id*='txtCommissionTotal']");
	var objProjectTotal = $("input[id*='txtProjectTotal']");
	var objViewAgencyCount = $("input[id*='txtViewAgencyCount']");
	var objViewBusinessCount = $("input[id*='txtViewBusinessCount']");
	var objFirstMandates = $("input[id*='FirstMandates']");
	var objIsCommissionTotalShow = $("input[id*='chk_IsCommissionTotalShow']:checked");
	var objIsProjectTotalShow = $("input[id*='chk_IsProjectTotalShow']:checked");
	var objimgHead1Pic = $("img[id*='imgHead1Pic']");
	var objimgHead2Pic = $("img[id*='imgHead2Pic']");
	var objimgHead3Pic = $("img[id*='imgHead3Pic']");
	var objimgHead4Pic = $("img[id*='imgHead4Pic']");
	var objimgHead5Pic = $("img[id*='imgHead5Pic']");

	var objzxbRegistered_text = $("input[id*='zxbRegistered_text']");
	var objzxbRegistered = $("input[id$='zxbRegistered']");
	var objzxbCertification_text = $("input[id*='zxbCertification_text']");
	var objzxbCertification = $("input[id$='zxbCertification']");
	var objzxbReleaseTheTask_text = $("input[id*='zxbReleaseTheTask_text']");
	var objzxbReleaseTheTask = $("input[id$='zxbReleaseTheTask']");
	var objzxbHosting_text = $("input[id*='zxbHosting_text']");
	var objzxbHosting = $("input[id$='zxbHosting']");
	var objzxbShare_text = $("input[id*='zxbShare_text']");
	var objzxbShare = $("input[id$='zxbShare']");
	var objzxbReview_text = $("input[id*='zxbReview_text']");
	var objzxbReview = $("input[id$='zxbReview']");

	var objSiteName = $("input[id*='txtSiteName']");
	var objSiteDescription = $("input[id*='txtSiteDescription']");
	var objServicePhone = $("input[id*='txtServicePhone']");
	var objServiceNote = $("input[id*='txtServiceNote']");
	var objIOSAPPURL = $("input[id*='txtIOSAPPURL']");
	var objimgAPPImagePic = $("img[id*='imgAPPImagePic']");
	var objimgGZImagePic = $("img[id*='imgGZImagePic']");

	var ue = UE.getEditor('container');
	var ue2 = UE.getEditor('zxbNote');
	
	configVO.ConfigId = parseInt($("#" + hidConfigId).val());
	configVO.MessageAPI = objMessageAPI.val();
	configVO.MessageNotiCount = objMessageNotiCount.val();
	configVO.MessageNotiPhone = objMessageNotiPhone.val();
	configVO.HeaderPic = objimgHead1Pic.attr("src") + ";" + objimgHead2Pic.attr("src") + ";" + objimgHead3Pic.attr("src") + ";" + objimgHead4Pic.attr("src") + ";" + objimgHead5Pic.attr("src") ;
	if (objIsNeedApprove.length > 0)
	    configVO.IsNeedApprove = true;
	else
	    configVO.IsNeedApprove = false;

	configVO.CommissionPercentage = objCommissionPercentage.val();
	configVO.CommissionTotal = objCommissionTotal.val();
	configVO.ProjectTotal = objProjectTotal.val();
	configVO.ViewAgencyCount = objViewAgencyCount.val();
	configVO.ViewBusinessCount = objViewBusinessCount.val();
	configVO.FirstMandates = objFirstMandates.val();

	configVO.zxbRegistered_text = objzxbRegistered_text.val();
	configVO.zxbRegistered = objzxbRegistered.val();
	configVO.zxbCertification_text = objzxbCertification_text.val();
	configVO.zxbCertification = objzxbCertification.val();
	configVO.zxbReleaseTheTask_text = objzxbReleaseTheTask_text.val();
	configVO.zxbReleaseTheTask = objzxbReleaseTheTask.val();
	configVO.zxbHosting_text = objzxbHosting_text.val();
	configVO.zxbHosting = objzxbHosting.val();
	configVO.zxbShare_text = objzxbShare_text.val();
	configVO.zxbShare = objzxbShare.val();
	configVO.zxbReview_text = objzxbReview_text.val();
	configVO.zxbReview = objzxbReview.val();

	if (objIsCommissionTotalShow.length > 0)
	    configVO.IsCommissionTotalShow = true;
	else
	    configVO.IsCommissionTotalShow = false;
	if (objIsProjectTotalShow.length > 0)
	    configVO.IsProjectTotalShow = true;
	else
	    configVO.IsProjectTotalShow = false;

	configVO.SiteName = objSiteName.val();
	configVO.SiteDescription = objSiteDescription.val();
	configVO.ServicePhone = objServicePhone.val();
	configVO.ServiceNote = objServiceNote.val();
	configVO.IOSAPPURL = objIOSAPPURL.val();
	configVO.APPImage = objimgAPPImagePic.attr("src");
	configVO.GZImage = objimgGZImagePic.attr("src");

	//configVO.ContractNote = ue.getContent();
	configVO.zxbNote = ue2.getContent();

    return configVO;
}

function SetConfig(configVO) {
	var objMessageAPI = $("input[id*='txtMessageAPI']");
	var objMessageNotiCount = $("input[id*='txtMessageNotiCount']");
	var objMessageNotiPhone = $("input[id*='txtMessageNotiPhone']");
	var objIsNeedApprove = $("input[id*='chk_IsNeedApprove']");
	var objCommissionPercentage = $("input[id*='txtCommissionPercentage']");
	var objCommissionTotal = $("input[id*='txtCommissionTotal']");
	var objProjectTotal = $("input[id*='txtProjectTotal']");
	var objViewAgencyCount = $("input[id*='txtViewAgencyCount']");
	var objViewBusinessCount = $("input[id*='txtViewBusinessCount']");
	var objFirstMandates = $("input[id*='FirstMandates']");
	var objIsCommissionTotalShow = $("input[id*='chk_IsCommissionTotalShow']");
	var objIsProjectTotalShow = $("input[id*='chk_IsProjectTotalShow']");

	var objzxbRegistered_text = $("input[id*='zxbRegistered_text']");
	var objzxbRegistered = $("input[id$='zxbRegistered']");
	var objzxbCertification_text = $("input[id*='zxbCertification_text']");
	var objzxbCertification = $("input[id$='zxbCertification']");
	var objzxbReleaseTheTask_text = $("input[id*='zxbReleaseTheTask_text']");
	var objzxbReleaseTheTask = $("input[id$='zxbReleaseTheTask']");
	var objzxbHosting_text = $("input[id*='zxbHosting_text']");
	var objzxbHosting = $("input[id$='zxbHosting']");
	var objzxbShare_text = $("input[id*='zxbShare_text']");
	var objzxbShare = $("input[id$='zxbShare']");
	var objzxbReview_text = $("input[id*='zxbReview_text']");
	var objzxbReview = $("input[id$='zxbReview']");

	var objimgHead1Pic = $("img[id*='imgHead1Pic']");
	var objimgHead2Pic = $("img[id*='imgHead2Pic']");
	var objimgHead3Pic = $("img[id*='imgHead3Pic']");
	var objimgHead4Pic = $("img[id*='imgHead4Pic']");
	var objimgHead5Pic = $("img[id*='imgHead5Pic']");

	var objSiteName = $("input[id*='txtSiteName']");
	var objSiteDescription = $("input[id*='txtSiteDescription']");
	var objServicePhone = $("input[id*='txtServicePhone']");
	var objServiceNote = $("input[id*='txtServiceNote']");
	var objIOSAPPURL = $("input[id*='txtIOSAPPURL']");
	var objimgAPPImagePic = $("img[id*='imgAPPImagePic']");
	var objimgGZImagePic = $("img[id*='imgGZImagePic']");

	var ue = UE.getEditor('container');
	var ue2 = UE.getEditor('zxbNote');

	parseInt($("#" + hidConfigId).val(configVO.ConfigId)); 
	objMessageAPI.val(configVO.MessageAPI);
	objMessageNotiCount.val(configVO.MessageNotiCount);
	objMessageNotiPhone.val(configVO.MessageNotiPhone);
	objIsNeedApprove.val(configVO.IsNeedApprove);
	objCommissionPercentage.val(configVO.CommissionPercentage);
	objCommissionTotal.val(configVO.CommissionTotal);
	objProjectTotal.val(configVO.ProjectTotal);
	objViewAgencyCount.val(configVO.ViewAgencyCount);
	objViewBusinessCount.val(configVO.ViewBusinessCount);
	objFirstMandates.val(configVO.FirstMandates);
	objIsCommissionTotalShow.val(configVO.IsCommissionTotalShow);
	objIsProjectTotalShow.val(configVO.IsProjectTotalShow);

	objzxbRegistered_text.val(configVO.zxbRegistered_text);
	objzxbRegistered.val(configVO.zxbRegistered);
	objzxbCertification_text.val(configVO.zxbCertification_text);
	objzxbCertification.val(configVO.zxbCertification);
	objzxbReleaseTheTask_text.val(configVO.zxbReleaseTheTask_text);
	objzxbReleaseTheTask.val(configVO.zxbReleaseTheTask);
	objzxbHosting_text.val(configVO.zxbHosting_text);
	objzxbHosting.val(configVO.zxbHosting);
	objzxbShare_text.val(configVO.zxbShare_text);
	objzxbShare.val(configVO.zxbShare);
	objzxbReview_text.val(configVO.zxbReview_text);
	objzxbReview.val(configVO.zxbReview);

	objSiteName.val(configVO.SiteName);
	objSiteDescription.val(configVO.SiteDescription);
	objServicePhone.val(configVO.ServicePhone);
	objServiceNote.val(configVO.ServiceNote);
	objIOSAPPURL.val(configVO.IOSAPPURL);

	var path = configVO.HeaderPic.split(";");
    if(path[0]!=undefined)
        objimgHead1Pic.attr("src", path[0]);
    if (path[1] != undefined)
        objimgHead2Pic.attr("src", path[1]);
    if (path[2] != undefined)
        objimgHead3Pic.attr("src", path[2]);
    if (path[3] != undefined)
        objimgHead4Pic.attr("src", path[3]);
    if (path[4] != undefined)
        objimgHead5Pic.attr("src", path[4]);

    objimgAPPImagePic.attr("src", configVO.APPImage);
    objimgGZImagePic.attr("src", configVO.GZImage);

	if (configVO.IsNeedApprove == 1)
	    objIsNeedApprove.attr("checked", true);
	if (configVO.IsCommissionTotalShow == 1)
	    objIsCommissionTotalShow.attr("checked", true);
	if (configVO.IsProjectTotalShow == 1)
	    objIsProjectTotalShow.attr("checked", true);

	ue.ready(function () {
	    this.setContent(configVO.ContractNote);
	});
	ue2.ready(function () {
	    this.setContent(configVO.zxbNote);
	});
}

function Init() {
    $("input[name='id-input-file']").ace_file_input({
        no_file: '请选择 ...',
        btn_choose: '选择',
        btn_change: 'Change',
        droppable: false,
        onchange: null,
        thumbnail: false
    });
    $(".ace-file-input").attr("style", "width: 41.6666%;");
    load_show();
    $.ajax({
        url: _RootPath + "SPWebAPI/System/GetConfig",
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var configVO = data.Result;
                SetConfig(configVO);
            }
            load_hide();
        },
        error: function (data) {
            alert(data);
            load_hide();
        }
    });
}

function uploadHeaderImg(uploadId) {
    uploadImgWithPath(uploadId, "Image/HeaderPage", function (data) {
        if (data.Flag == 1) {
            $("#" + uploadId + "Pic").attr("src", data.Result.FilePath.replace("~/", _APIURL));
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