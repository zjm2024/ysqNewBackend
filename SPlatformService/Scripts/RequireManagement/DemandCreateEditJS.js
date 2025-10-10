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

        var DescriptionText = getFormatCode(Description.val());
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
                    bootbox.dialog({
                        message: "更新成功，请重新通过审核",
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
        window.location.href = "DemandBrowse.aspx";
        return false;
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
var getFormatCode = function (strValue) {//textarea 换行
    return strValue.replace(/\r\n/g, '<br/>').replace(/\n/g, '<br/>').replace(/\s/g, ' ');
}
