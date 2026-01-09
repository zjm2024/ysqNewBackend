function savebtn() {
    var OfferName = $("input[name*='OfferName']");
    var OfferDescription = $("textarea[name*='OfferDescription']");
    var OfferDemandId = $("input[name*='OfferDemandId']");
    var OfferPhone = $("input[name*='OfferPhone']");

    if (_CustomerId <= 0) {
        bootbox.dialog({
            message: "请先登录后再继续操作！",
            buttons:
            {
                "Confirm":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        window.location.href = "Login.aspx";
                    }
                }
            }
        });
    }

    if (OfferName.val() == "") {
        bootbox.dialog({
            message: "请输入你的名称",
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


    if (OfferPhone.val() == "") {
        bootbox.dialog({
            message: "请输入你的电话号码",
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

    var seller_data = {
        "DemandId": OfferDemandId.val(),
        "Name": OfferName.val(),
        "Phone": OfferPhone.val(),
        "OfferDescription": OfferDescription.val()
    }

    $.ajax({
        url: _RootPath + "SPWebAPI/Require/AddDemandOffer?Token=" + _Token,
        type: "POST",
        data: seller_data,
        success: function (data) {
            console.log(data);
            if (data.Flag == 1) {
                Newform_close('demandoffer');
                bootbox.dialog({
                    message: "留言成功",
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
            console.log(data);
            alert(data);
        }
    });
}