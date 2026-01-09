$(document).ready(function () {
    Init();
    $('#form1').validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {

            txtCommission: {
                required: true,
                number: true
            },

            txtReason: {
                required: true
            }

        },
        messages: {

            txtCommission: {
                required: "请输入申请金额！",
                number: "请输入数字！"
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
    var lblRemainObj = $("label[id*='lblRemainCommission']");
    var lblRemainObj2 = $("label[id*='lblpayoutCommission']");
    lblRemainObj.html(GetQueryString("Commission"));
    lblRemainObj2.html(GetQueryString("payout"));
}

function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
