$(document).ready(function () {
    //Init();
    //initDatePicker();
    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtRPCost: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtRpNum: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtRPOneCost: {
                required: true
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtRPCost: {
                required: "请输入红包总额！"
            },
            ctl00$ContentPlaceHolder_Content$txtRpNum: {
                required: "请输入红包个数"
            },
            ctl00$ContentPlaceHolder_Content$txtRPOneCost: {
                required: "单个红包最低金额"
            },
         
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

        var RedPacketId = parseInt($("#" + RedPacketId).val());
        RedPacketId=0
        console.log(RedPacketId);
        var systemMessageVO = GetSystemRedPacketVO();
        var txtRPOneCost = $("input[id*='txtRPOneCost']").val();

        console.log(txtRPOneCost);
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/AddOfficialCardRedPacket?txtRPOneCost=" + txtRPOneCost+"&token=" + _Token,
            type: "POST",
            data: systemMessageVO,
            success: function (data) {
                console.log(data);
                if (data.Flag == 1) {
                    if (RedPacketId < 1) {
                        RedPacketId = data.Result;
                        $("#" + RedPacketId).val(data.Result);
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
                                            window.location.href = "CardRedPacketBrowse.aspx";
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
        window.location.href = "CardRedPacketBrowse.aspx";
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
function GetSystemRedPacketVO() {
    var CardRedPacketVO = new Object();

    var txtRPCost = $("input[id*='txtRPCost']");
    var txtRpNum = $("input[id*='txtRpNum']");
    var txtRpConetnt = $("textarea[id*='txtRpContent']");



    CardRedPacketVO.RedPacketId = parseInt($("#" + RedPacketId).val());

    CardRedPacketVO.RPCost = txtRPCost.val();
    CardRedPacketVO.RPNum = txtRpNum.val();
    CardRedPacketVO.RPContent = txtRpConetnt.val();

    return CardRedPacketVO;
}

function SetSystemMessage(CardRedPacketVO) {

    var txtRPCost = $("input[id*='txtRPCost']");
    var txtRpNum = $("input[id*='txtRpNum']");
    var txtRpConetnt = $("textarea[id*='txtRpContent']");

    txtRPCost.val(CardRedPacketVO.RPCost);
    txtRpNum.val(CardRedPacketVO.RPNum);
    objUrl.txtRpConetnt(CardRedPacketVO.RPContent);
}
function Init() {
    var RedPacketId = parseInt($("#" + RedPacketId).val());
    console.log(RedPacketId);
   /*
     if (RedPacketId > 0) {
        var objSendAt = $("#divSendAt");
        objSendAt.show();
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/AddOfficialCardRedPacket?RedPacketId=" + RedPacketId + "&token=" + _Token,
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
    
    */
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
