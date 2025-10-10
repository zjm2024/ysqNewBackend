$(document).ready(function () {
    $("button[id*='btn_save']").click(function () {

        var CategoryId = $("select[id*='drpdemand_class']");
        var Description = $("textarea[name*='txtDescription']");
        var DemandEffectiveEndDate = $("input[name*='txtEffectiveEndDateCreateEdit']");

        if (Description.val() == "") {
            bootbox.dialog({
                message: "请输入您的需求",
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
		
		var DescriptionText=getFormatCode(Description.val());
        var seller_data = {
            "DemandId": _DemandId,
            "CategoryId": CategoryId.val(),
            "Description": DescriptionText,
            "EffectiveEndDate": DemandEffectiveEndDate.val(),
            "Status": 0
        }
        console.log(seller_data);
        $.ajax({
            url: _RootPath + "SPWebAPI/Require/UpdateDemand?code=&password=&Token=" + _Token,
            type: "POST",
            data: seller_data,
            success: function (data) {
                if (data.Flag == 1) {
                    Quick_release_close();
                    bootbox.dialog({
                        message: "更新成功",
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
    $("button[id*='btn_submit']").click(function () {
        var CategoryId = $("select[id*='drpdemand_class']");
        var Description = $("textarea[name*='txtDescription']");
        var DemandEffectiveEndDate = $("input[name*='txtEffectiveEndDateCreateEdit']");

        if (Description.val() == "") {
            bootbox.dialog({
                message: "请输入您的需求",
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
		var DescriptionText=getFormatCode(Description.val());
        var seller_data = {
            "DemandId": _DemandId,
            "CategoryId": CategoryId.val(),
            "Description": DescriptionText,
            "EffectiveEndDate": DemandEffectiveEndDate.val(),
            "Status":2
        }

        $.ajax({
            url: _RootPath + "SPWebAPI/Require/UpdateDemand?code=&password=&Token=" + _Token,
            type: "POST",
            data: seller_data,
            success: function (data) {
                if (data.Flag == 1) {
                    Quick_release_close();
                    bootbox.dialog({
                        message: "提交成功，请等待审核通过！",
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

    $("button[id*='btn_updaterequirestatus']").click(function () {
        bootbox.dialog({
            message: "是否确认取消发布?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        UpdateDemandStatusAction(_DemandId, 0);
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


    var objDemandEffectiveEndDate = $("input[name*='txtEffectiveEndDateCreateEdit']");

    objDemandEffectiveEndDate.datepicker({
        format: "yyyy-mm-dd",
        language: "zh-CN",
        autoclose: true,
        todayHighlight: true
    }).next().on("click", function () {
        $(this).prev().focus();
    });
});

function UpdateDemandStatusAction(DemandId, status) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Require/UpdateDemandStatus?DemandId=" + DemandId + "&status=" + status + "&token=" + _Token,
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
var getFormatCode = function (strValue) {//textarea 换行
    return strValue.replace(/\r\n/g, '<br/>').replace(/\n/g, '<br/>').replace(/\s/g, ' ');
}
