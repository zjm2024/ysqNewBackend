$(document).ready(function () {
    Init();
    $('#form1').validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {

            txtCommission: {
                required: true,
                number:true
            },

            txtReason: {
                required: true
            }

        },
        messages: {

            txtCommission: {
                required: "请输入申请金额！",
                number: "请输入数字！"
            },

            txtReason: {
                required: "请输入付款理由！"
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
});

function Init() {
    var projectId = GetQueryString("ProjectId");
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetRemainCommissionByProject?projectId=" + projectId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var lblRemainObj = $("label[id*='lblRemainCommission']");
                lblRemainObj.html(data.Result);
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}

function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
