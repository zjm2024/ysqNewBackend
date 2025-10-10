var _BusinessReviewId = 0;
$(document).ready(function () {
    Init();
    
    $("form[id*='aspnetForm']").validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: true,
        rules: {

            ctl00$ContentPlaceHolder_Content$txtAddNote: {
                required: true
            }
        },
        messages: {

            ctl00$ContentPlaceHolder_Content$txtAddNote: {
                ddlrequired: "请输入回复内容！"
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
       
    $("textarea[id$='txtAddNote']").attr("maxlength", "400");
});

function PlusReview() {
    if (!$("form[id*='aspnetForm']").valid()) {
        return false;
    }

    var businessReviewVO = GetBusinessReviewVO();
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/PlusBusinessReview?token=" + _Token,
        type: "POST",
        data: businessReviewVO,
        success: function (data) {
            if (data.Flag == 1) {
                $("#btn_NewReview").hide();
                bootbox.dialog({
                    message: "追评成功！",
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
}

function GetBusinessReviewVO() {
    var businessReviewVO = new Object();

    var objNote = $("textarea[id*='txtAddNote']");

    businessReviewVO.BusinessReviewId = _BusinessReviewId;
    businessReviewVO.AddNote = objNote.val();

    return businessReviewVO;
}


function Init() {
        
    BindReview(_BusinessReviewId);
}

function BindReview(businessReviewId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetBusinessReview?businessReviewId=" + businessReviewId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVO = data.Result;
                $("#divProjectCode").html(puVO.ProjectCode);
                $("#divProjectName").html(puVO.Title);
                $("#divBusinessName").html(puVO.BusinessCustomerName);
                $("#divAgencyReviewDescription").html(puVO.Description);
                $("#divBusinessExplanation").html(puVO.Explanation);

                var txtReviewNote = $("textarea[id*='txtAddNote']");
                if (puVO.AddNote != "") {
                    txtReviewNote.val(puVO.AddNote);
                    $("button[id$='btn_NewReview']").hide();
                }

                var detailList = puVO.BusinessReviewDetailList;
                var divReviewList = $(".xzw_starBox");
                for (var i = 0; i < divReviewList.length; i++) {
                    $(divReviewList[i]).find(".showb").css({ "width": 30 * detailList[i].Score });
                }

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
            load_hide();
        },
        error: function (data) {
            alert(data);
        }
    });
}
