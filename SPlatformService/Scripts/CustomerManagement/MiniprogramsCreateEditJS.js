$(document).ready(function () {
    Init();    
    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtAppName: {
                required: true
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtAppName: {
                required: "请输入小程序名称！"
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
        var hidAppType = parseInt($("#" + hidAppType).val());
        var systemMessageVO = GetSystemMessageVO();
        console.log(systemMessageVO);
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/AddMiniprograms?token=" + _Token,
            type: "POST",
            data: systemMessageVO,
            success: function (data) {
                console.log(data);
                if (data.Flag == 1) {
                    if (hidAppType < 1) {
                        hidAppType = data.Result;
                        $("#" + hidAppType).val(data.Result);
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
                                    window.location.href = "MiniprogramsBrowse.aspx";
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
        window.location.href = "MiniprogramsBrowse.aspx";
        return false;
    });
});
function GetSystemMessageVO() {
    var CardNewsVO = new Object();

    var objAppName = $("input[id*='txtAppName']");
    
    var objAppId = $("input[id*='txtAppId']");
    var objSecret = $("input[id*='txtSecret']");
    var objMCHID = $("input[id*='txtMCHID']");
    
    var objMCH_KEY = $("input[id*='txtMCH_KEY']");
    var objAPPSECRET = $("input[id*='txtAPPSECRET']");
    var objSSLCERT_PATH = $("input[id*='txtSSLCERT_PATH']");
    var objSSLCERT_PASSWORD = $("input[id*='txtSSLCERT_PASSWORD']");
    var objTBusinessID = $("input[id*='txtTBusinessID']");
    var objTPersonalID = $("input[id*='txtTPersonalID']");
    var objtemplateID = $("select[id*='templateID']");

    CardNewsVO.AppType = parseInt($("#" + hidAppType).val());
    CardNewsVO.AppName = objAppName.val();
    
    CardNewsVO.AppId = objAppId.val();
    
    CardNewsVO.Secret = objSecret.val();
    
    CardNewsVO.MCHID = objMCHID.val();
    CardNewsVO.MCH_KEY = objMCH_KEY.val();
    CardNewsVO.APPSECRET = objAPPSECRET.val();
    
    CardNewsVO.SSLCERT_PATH = objSSLCERT_PATH.val();
    CardNewsVO.SSLCERT_PASSWORD = objSSLCERT_PASSWORD.val();
    
    CardNewsVO.TBusinessID = objTBusinessID.val();
    CardNewsVO.TPersonalID = objTPersonalID.val();
    CardNewsVO.templateID = objtemplateID.val();
    
    return CardNewsVO;
}

function SetSystemMessage(CardNewsVO) {

    var objAppName = $("input[id*='txtAppName']");
    var objAppId = $("input[id*='txtAppId']");
    var objSecret = $("input[id*='txtSecret']");
    var objMCHID = $("input[id*='txtMCHID']");

    var objMCH_KEY = $("input[id*='txtMCH_KEY']");
    var objAPPSECRET = $("input[id*='txtAPPSECRET']");
    var objSSLCERT_PATH = $("input[id*='txtSSLCERT_PATH']");
    var objSSLCERT_PASSWORD = $("input[id*='txtSSLCERT_PASSWORD']");
    var objTBusinessID = $("input[id*='txtTBusinessID']");
    var objTPersonalID = $("input[id*='txtTPersonalID']");
    var objtemplateID = $("select[id*='templateID']");
	
    objAppName.val(CardNewsVO.AppName);
    objAppId.val(CardNewsVO.AppId);
    objSecret.val(CardNewsVO.Secret);
    objMCHID.val(CardNewsVO.MCHID);
    objMCH_KEY.val(CardNewsVO.MCH_KEY);
    objAPPSECRET.val(CardNewsVO.APPSECRET);
    objSSLCERT_PATH.val(CardNewsVO.SSLCERT_PATH);
    objSSLCERT_PASSWORD.val(CardNewsVO.SSLCERT_PASSWORD);
    objTBusinessID.val(CardNewsVO.TBusinessID);
    objTPersonalID.val(CardNewsVO.TPersonalID);
    objtemplateID.val(CardNewsVO.templateID);
}
function Init() {
    var AppType = parseInt($("#" + hidAppType).val());
    console.log(AppType);
    if (AppType > 0) {
        var objSendAt = $("#divSendAt");
        objSendAt.show();
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/GetMiniprograms?AppType=" + AppType + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                console.log(data);
                if (data.Flag == 1) {
                    var CardNoticeVO = data.Result;
                    SetSystemMessage(CardNoticeVO);
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
                                    window.location.href = "MiniprogramsBrowse.aspx";
                                }
                            }
                        }
                    });
                }
                load_hide();
            },
            error: function (data) {
                alert(data);
                load_hide();
            }
        });
    }
}
