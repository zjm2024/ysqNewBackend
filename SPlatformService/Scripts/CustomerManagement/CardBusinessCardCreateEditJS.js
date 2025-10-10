$(document).ready(function () {
    Init();
    initDatePicker();
    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtBusinessName: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtNumber: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtExpirationAt: {
                required: true
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtBusinessName: {
                required: "请输入公司名称！"
            },
            ctl00$ContentPlaceHolder_Content$txtNumber: {
                required: "请输入限制人数！"
            },
            ctl00$ContentPlaceHolder_Content$txtExpirationAt: {
                required: "请选择到期时间！"
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

        var hidBusinessID = parseInt($("#" + hidBusinessID).val());
        var BusinessCardVO = GetBusinessCardVO();
        console.log(BusinessCardVO);
        $.ajax({
            url: "https://www.zhongxiaole.net/BusinessCard/SPWebAPI/BusinessCard/UpdateBusinessCard?token=" + _Token,
            type: "POST",
            data: BusinessCardVO,
            success: function (data) {
                console.log(data);
                if (data.Flag == 1) {
                    if (hidBusinessID < 1) {
                        hidBusinessID = data.Result;
                        $("#" + hidBusinessID).val(data.Result);
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
                                    window.location.href = "CardBusinessCardBrowse.aspx";
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
                console.log(data);
            }
        });
    });


    $("#btn_cancel").click(function () {
        window.location.href = "CardBusinessCardBrowse.aspx";
        return false;
    });


});

function change(uploadId) {
    uploadImg(uploadId, function (data) {
        if (data.Flag == 1) {
            $("img[id*='" + uploadId + "Pic']").attr("src", data.Result.FilePath.replace("~", _APIURL));
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
function GetBusinessCardVO() {
    var BusinessCardVO = new Object();

    var objBusinessName = $("input[id*='txtBusinessName']");
    var objLogoImg = $("img[id*='imgLogoImgPic']");
    var objIndustry = $("input[id*='txtIndustry']");
    var objNumber = $("input[id*='txtNumber']");
    var objSubsidiarySum = $("input[id*='tetSubsidiarySum']");
    var objExpirationAt = $("input[id*='txtExpirationAt']");
    var objBusinessLicenseImg = $("img[id*='imgBusinessLicenseImg']");

    var objDecimal = $("input[id*='radDecimal']:checked");
    var objOfficialProducts = $("select[id*='OfficialProducts']");
    var objisPayDecimal = $("input[id*='isPayDecimal']:checked");
    var objisAgentDecimal = $("input[id*='isAgentDecimal']:checked");

    if (objDecimal.length > 0)
        BusinessCardVO.isGroup = 1;
    else
        BusinessCardVO.isGroup = 0;

    if (objisPayDecimal.length > 0)
        BusinessCardVO.isPay = 1;
    else
        BusinessCardVO.isPay = 0;

    if (objisAgentDecimal.length > 0)
        BusinessCardVO.isAgent = 1;
    else
        BusinessCardVO.isAgent = 0;

    BusinessCardVO.BusinessID = parseInt($("#" + hidBusinessID).val());
    BusinessCardVO.BusinessName = objBusinessName.val();
    BusinessCardVO.LogoImg = objLogoImg.attr("src");
    BusinessCardVO.Industry = objIndustry.val();
    BusinessCardVO.Number = objNumber.val();
    BusinessCardVO.SubsidiarySum = objSubsidiarySum.val();
    BusinessCardVO.ExpirationAt = objExpirationAt.val();
    BusinessCardVO.BusinessLicenseImg = objBusinessLicenseImg.attr("src");
    BusinessCardVO.OfficialProducts = objOfficialProducts.val();

    return BusinessCardVO;
}

function SetSystemMessage(BusinessCardVO) {

    var objBusinessName = $("input[id*='txtBusinessName']");
    var objLogoImg = $("img[id*='imgLogoImgPic']");
    var objIndustry = $("input[id*='txtIndustry']");
    var objNumber = $("input[id*='txtNumber']");
    var objSubsidiarySum = $("input[id*='tetSubsidiarySum']");
    var objExpirationAt = $("input[id*='txtExpirationAt']");
    var objBusinessLicenseImg = $("img[id*='imgBusinessLicenseImg']");
    var objOfficialProducts = $("select[id*='OfficialProducts']");

    var objDecimal = $("input[id*='radDecimal']");
    var objPer = $("input[id*='radPer']");
    var objisPayDecimal = $("input[id*='isPayDecimal']");
    var objisPay = $("input[id*='isPay']");
    var objisAgentDecimal = $("input[id*='isAgentDecimal']");
    var objisAgent = $("input[id*='isAgent']");

    if (BusinessCardVO.isGroup == 1)
        objDecimal.attr("checked", true);
    else
        objPer.attr("checked", true);

    if (BusinessCardVO.isPay == 1)
        objisPayDecimal.attr("checked", true);
    else
        objisPay.attr("checked", true);

    if (BusinessCardVO.isAgent == 1)
        objisAgentDecimal.attr("checked", true);
    else
        objisAgent.attr("checked", true);

    objBusinessName.val(BusinessCardVO.BusinessName);
    objLogoImg.attr("src", BusinessCardVO.LogoImg);
    objBusinessLicenseImg.attr("src", BusinessCardVO.BusinessLicenseImg);
    objIndustry.val(BusinessCardVO.Industry);
    objNumber.val(BusinessCardVO.Number);
    objSubsidiarySum.val(BusinessCardVO.SubsidiarySum);
    objOfficialProducts.val(BusinessCardVO.OfficialProducts);

    if (new Date(BusinessCardVO.ExpirationAt).format("yyyy-MM-dd") != "1900-01-01")
        objExpirationAt.datepicker("setDate", new Date(BusinessCardVO.ExpirationAt));
}
function Init() {
    var BusinessID = parseInt($("#" + hidBusinessID).val());
    console.log(BusinessID);
    if (BusinessID > 0) {
        var objSendAt = $("#divSendAt");
        objSendAt.show();
        load_show();
        $.ajax({
            url: "https://www.zhongxiaole.net/BusinessCard/SPWebAPI/BusinessCard/getBusinessCard?BusinessID=" + BusinessID + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var CardNoticeVO = data.Result;
                    SetSystemMessage(CardNoticeVO);
                }
                load_hide();
            },
            error: function (data) {
                console.log(data)
                load_hide();
            }
        });
    }
}
function initDatePicker() {
    $('.date-picker').datepicker({
        format: "yyyy-mm-dd",
        language: "zh-CN",
        autoclose: true,
        todayHighlight: true
    }).next().on("click", function () {
        $(this).prev().focus();
    });
    $('.timepicker1').timepicker({
        minuteStep: 1,
        defaultTime: false,
        showSeconds: true,
        showMeridian: false, showWidgetOnAddonClick: false
    }).next().on("click", function () {
        $(this).prev().focus();
    });

    $('.date-picker-yyyy').datepicker({
        minViewMode: 'years',
        format: 'yyyy',
        autoclose: true,
        startViewMode: 'year',
        startDate: '1900',
        endDate: '2100',
        language: 'zh-CN'
    })
    .next().on("click", function () {
        $(this).prev().focus();
    });
}
