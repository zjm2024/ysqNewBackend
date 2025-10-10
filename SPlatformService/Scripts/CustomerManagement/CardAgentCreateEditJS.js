$(document).ready(function () {
    Init();    
    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtAgentName: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$drpCity: {
                ddlrequired: true
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtAgentName: {
                required: "请输入代理名称！"
            },
            ctl00$ContentPlaceHolder_Content$drpCity: {
                ddlrequired: "请选择代理区域！"
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
        var CardAgentID = parseInt($("#" + hidCardAgentID).val());
        var systemMessageVO = GetSystemMessageVO();
        console.log(systemMessageVO);
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/AddCardAgent?token=" + _Token,
            type: "POST",
            data: systemMessageVO,
            success: function (data) {
                console.log(data);
                if (data.Flag == 1) {
                    if (CardAgentID < 1) {
                        CardAgentID = data.Result;
                        $("#" + hidCardAgentID).val(data.Result);
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
                                    window.location.href = "CardAgentBrowse.aspx";
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
        window.location.href = "CardAgentBrowse.aspx";
        return false;
    });
    $("select[id$='drpProvince']").change(function () {
        //更新Child
        var drp = $("select[id*='drpProvince']");
        var childDrp = $("select[id*='drpCity']");
        childDrp.empty();

        $.ajax({
            url: _RootPath + "SPWebAPI/user/GetCityList?provinceId=" + drp.val() + "&enable=true&token=" + _Token,
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
});

function GetSystemMessageVO() {
    var CardAgentVO = new Object();

    var objAgentName = $("input[id*='txtAgentName']");
    var objCityId = $("select[id*='drpCity']");

    CardAgentVO.CardAgentID = parseInt($("#" + hidCardAgentID).val());
    CardAgentVO.AgentName = objAgentName.val();
    CardAgentVO.CityId = objCityId.val();

    return CardAgentVO;
}

function SetSystemMessage(CardAgentVO) {

    var objAgentName = $("input[id*='txtAgentName']");
    var objCityId = $("select[id*='drpCity']");
	
    objAgentName.val(CardAgentVO.AgentName);
    objCityId.val(CardAgentVO.CityId);
}
function Init() {
    var CardAgentID = parseInt($("#" + hidCardAgentID).val());
    console.log(CardAgentID);
    if (CardAgentID > 0) {
        var objSendAt = $("#divSendAt");
        objSendAt.show();
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/GetCardAgent?CardAgentID=" + CardAgentID + "&token=" + _Token,
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
                alert(data);
                load_hide();
            }
        });
    }
}