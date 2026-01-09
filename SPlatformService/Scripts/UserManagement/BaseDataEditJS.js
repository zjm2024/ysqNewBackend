var editor;
$(document).ready(function () {
    var E = window.wangEditor;
    editor = new E('#editor');
    editor.customConfig.uploadImgShowBase64 = true;
    editor.create();

    InitData();
    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtDescription: {
                required: true
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtDescription: {
                required: "请输入详情！"
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

        var baseDataId = parseInt($("#" + hidBaseDataId).val());
        var baseDataVO = GetBaseDataVO();
        $.ajax({
            url: _RootPath + "SPWebAPI/User/UpdateBaseData?token=" + _Token,
            type: "POST",
            data: baseDataVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (baseDataId < 1) {
                        $("#" + hidBaseDataId).val(data.Result);
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

    $("select[id*='drpBaseData']").change(function () {
        InitData();
    }
    );
});

function InitData() {
    var objBaseDataType = $("select[id*='drpBaseData']");

    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetBaseData?baseDataType=" + objBaseDataType.val() + "&token=" + _Token,
        type: "GET",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {                
                var baseDataVO = data.Result;
                SetBaseData(baseDataVO);
            } else {
               
            }

        },
        error: function (data) {
            alert(data);
        }
    });
}

function GetBaseDataVO() {
    var baseDataVO = new Object();
   
    var objBaseDataType = $("select[id*='drpBaseData']");

    baseDataVO.BaseDataId = parseInt($("#" + hidBaseDataId).val());
    baseDataVO.BaseDataType = objBaseDataType.val();
    baseDataVO.Description = editor.txt.html();

    return baseDataVO;
}

function SetBaseData(baseDataVO) {

    var objBaseDataType = $("select[id*='drpBaseData']");

    $("#" + hidBaseDataId).val(baseDataVO.BaseDataId)
    editor.txt.html(baseDataVO.Description);
}
