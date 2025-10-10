$(document).ready(function () {
    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {
            ctl00$ContentPlaceHolder_Content$txtName: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtPhone: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtPosition: {
                required: true
            },
            ctl00$ContentPlaceHolder_Content$txtCorporateName: {
                required: true
            }
        },
        messages: {
            ctl00$ContentPlaceHolder_Content$txtName: {
                required: "请输入姓名！"
            },
            ctl00$ContentPlaceHolder_Content$txtPhone: {
                required: "请输入手机！"
            },
            ctl00$ContentPlaceHolder_Content$txtPosition: {
                required: "请输入职位！"
            },
            ctl00$ContentPlaceHolder_Content$txtCorporateName: {
                required: "请输入单位名称！"
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
        var CardVO = GetCardVO();
        console.log(CardVO);
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/AddDummyCard?token=" + _Token,
            type: "POST",
            data: CardVO,
            success: function (data) {
                console.log(data);
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
                                    window.location.href = "CardBrowse.aspx";
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
    $("#RandomHeadimg").click(function () {
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/GetRandomHeadimg",
            type: "get",
            success: function (data) {
                console.log(data)
                if (data.Flag == 1) {
                    var objNewsImg = $("img[id*='Headimg']");
                    objNewsImg.attr("src", data.Result);
                }
            },
            error: function (data) {
                console.log(data);
            }
        });
    });
});

function GetCardVO() {
    var CardVO = new Object();
    var objHeadimg = $("img[id*='Headimg']");
    var objName = $("input[id*='txtName']");
    var objPhone = $("input[id*='txtPhone']");
    var objPosition = $("input[id*='txtPosition']");
    var objCorporateName = $("input[id*='txtCorporateName']");
    var objAddress = $("input[id*='txtAddress']");
    var objBusiness = $("input[id*='txtBusiness']");
    var objWeChat = $("input[id*='txtWeChat']");
    var objEmail = $("input[id*='txtEmail']");
    var objTel = $("input[id*='txtTel']");
    var objWebSite = $("input[id*='txtWebSite']");
    var objDetails = $("textarea[id*='txtDetails']");

    CardVO.CardID = 0;
    CardVO.Name = objName.val();
    CardVO.Headimg = objHeadimg.attr("src");
    CardVO.Phone = objPhone.val();
    CardVO.Position = objPosition.val();
    CardVO.CorporateName = objCorporateName.val();
    CardVO.Address = objAddress.val();
    CardVO.Business = objBusiness.val();
    CardVO.WeChat = objWeChat.val();
    CardVO.Email = objEmail.val();
    CardVO.Tel = objTel.val();
    CardVO.WebSite = objWebSite.val();
    CardVO.Details = objDetails.val();

    return CardVO;
}