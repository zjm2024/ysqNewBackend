$(document).ready(function () {
    Init();   

    $("#btn_save").click(function () {       

        var complaintsId = parseInt($("#" + hidComplaintsId).val());
        var complaintsVO = new Object();
        complaintsVO.ComplaintsId = complaintsId;
        complaintsVO.Status = 1;
        $.ajax({
            url: _RootPath + "SPWebAPI/Project/UpdateComplaints?token=" + _Token,
            type: "POST",
            data: complaintsVO,
            success: function (data) {
                if (data.Flag == 1) {
                    if (complaintsId < 1) {
                        $("#" + hidComplaintsId).val(data.Result);
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

    $("#btn_cancel").click(function () {
        window.location.href = "ComplaintsBrowse.aspx";
        return false;
    });      
    
   
});

function SetComplaints(complaintsVO) {

    var objProjectCode = $("input[id*='txtProjectCode']");
    var objBusinessName = $("input[id*='txtBusinessName']");
    var objAgencyName = $("input[id*='txtAgencyName']");
    var objCreator = $("input[id*='txtCreator']");
    var objCreatedAt = $("input[id*='txtCreatedAt']");
	var objDescription = $("textarea[id*='txtDescription']");
	var objStatus = $("input[id*='txtStatus']");

	
	objProjectCode.val(complaintsVO.ProjectCode);
	objBusinessName.val(complaintsVO.BusinessName);
	objAgencyName.val(complaintsVO.AgencyName);
	objCreator.val(complaintsVO.CustomerName);
	objCreatedAt.val(new Date(complaintsVO.CreatedAt).format("yyyy-MM-dd"));
	objDescription.val(complaintsVO.Description);

	if (complaintsVO.Status == 0)
	    objStatus.val("申请中");
	else if (complaintsVO.Status == 1)
	    objStatus.val("已解决");
}


function Init() {
    var complaintsId = parseInt($("#" + hidComplaintsId).val());
    if (complaintsId > 0) {
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Project/GetComplaints?complaintsId=" + complaintsId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var complaintsVO = data.Result;
                    SetComplaints(complaintsVO);
                    BindImageDetail(complaintsId);
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
                load_hide();
            }
        });
    }
}

function BindImageDetail(complaintsId) {
    var complaintsId = parseInt($("#" + hidComplaintsId).val());
    if (complaintsId > 0) {
        load_show();
        $.ajax({
            url: _RootPath + "SPWebAPI/Project/GetComplaintsImgByComplaints?complaintsId=" + complaintsId + "&token=" + _Token,
            type: "Get",
            data: null,
            success: function (data) {
                if (data.Flag == 1) {
                    var complaintsImgVOList = data.Result;
                    //BindImageDetail(complaintsId);
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
                load_hide();
            }
        });
    }
}