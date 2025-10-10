$(function () {
    $(".review-panel").mouseover(function () {
        $(".ui-shop-pop").show();
    });
    $(".review-panel").mouseout(function () {
        $(".ui-shop-pop").hide();
    });
    $("a[id*='lnkBusinessPhone']").click(function () {
        if (_CustomerId > 0) {
            $.ajax({
                url: _RootPath + "SPWebAPI/Customer/GetBusinessPhone?businessCustomerId=" + _BusinessCustomerId + "&token=" + _Token,
                type: "Get",
                data: null,
                async: false,
                success: function (pdata) {
                    if (pdata.Flag == 1) {
                        var objBusinessPhone = $("a[id*='lnkBusinessPhone']");
                        var lblBusinessPhone = $("span[id*='lblBusinessPhone']");
                        objBusinessPhone.hide();
                        lblBusinessPhone.text(pdata.Result);
                        lblBusinessPhone.show();
                    } else {
                        bootbox.dialog({
                            message: pdata.Message,
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

                }
            });
        } else {
            //先登录
            bootbox.dialog({
                message: "请先登录再查看！",
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
        return false;
    });
});

function GetBusinessDetil(businessId) {
    GetData("GetBusinessSite", businessId, function (data) {
        var titleObj = $("#lblTitle");
        var companyTypeObj = $("#lblCompanyType");
        var mainProductsObj = $("#lblMainProducts");
        var cityNameObj = $("#lblCityName");
        var setUpObj = $("#lblSetUpDate");
        var descriptionObj = $("#divDescription");

        var businessVO = data.Result;

        titleObj.html(businessVO.CompanyName);
        companyTypeObj.html(businessVO.CompanyType);
        mainProductsObj.html(businessVO.MainProducts);
        cityNameObj.html(businessVO.CityName);
        setUpObj.html(new Date(businessVO.SetupDate).format("yyyy-MM-dd"));
        descriptionObj.html(businessVO.Description);

    }, function (data) {        
        //load_hide();
    });
}