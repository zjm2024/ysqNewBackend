$(document).ready(function () {
    $("input[name='id-input-file']").ace_file_input({
        no_file: '请选择 ...',
        btn_choose: '选择',
        btn_change: 'Change',
        droppable: false,
        onchange: null,
        thumbnail: false
    });
    $(".ace-file-input").attr("style", "width: 41.6666%;");
    BindFile();
});
function changefile(uploadId) {
    var tempPath = "CustomerFile/" + _CustomerId + "/Toolkit";
    load_show();
    uploadFile(uploadId, tempPath, function (data) {
        if (data.Flag == 1) {
            var ToolFileVO = new Object();
            var objtxtfileDesc = $("textarea[id*='txtfileDesc']");
            ToolFileVO.FileName = data.Result.FileName;
            ToolFileVO.Description = objtxtfileDesc.val();
            ToolFileVO.FilePath = data.Result.FilePath.replace("~", _APIURL);
            ToolFileVO.TypeId = 2;//工具
            ToolFileVO.CreatedDate = new Date();
            ToolFileVO.CreatedBy = _CustomerId;

            //直接保存
            $.ajax({
                url: _RootPath + "SPWebAPI/Customer/UpdateToolFile?token=" + _Token,
                type: "POST",
                data: ToolFileVO,
                success: function (data) {
                    if (data.Flag == 1) {
                        ToolFileVO.ToolFileId = data.Result;
                        AddFile(ToolFileVO);
                        bootbox.dialog({
                            message: "上传成功!",
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
                    load_hide();
                },
                error: function (data) {
                    alert(data);
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
    });
}

function BindFile() {
    $("#FileList").find("tbody>tr").remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/Customer/GetToolFileByCustomer?CustomerId=" + _CustomerId + "&typeId=2&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];
                    AddFile(puVO);
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

function AddFile(puVO) {
    var fileTable = $("#FileList");
    var description = puVO.Description; 
    if (puVO.Description.length > 50)
        description = puVO.Description.substring(0, 49);
    var oTR = "";
    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
    oTR += "  <td class=\"center\"> \r\n";
    if (puVO.CreatedBy == _CustomerId)
        oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
    oTR += "    <input type=\"hidden\" value=\"File_" + puVO.ToolFileId + "\" /> \r\n";
    oTR += "  </td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.FileName + "\"><a target=\"_blank\" href=\"" + puVO.FilePath + "\">" + puVO.FileName + "</a></td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + puVO.Description + "\">" + description + "</td> \r\n";
    oTR += "  <td class=\"center\" title=\"" + new Date(puVO.CreatedDate).format("yyyy-MM-dd hh:mm:ss") + "\">" + new Date(puVO.CreatedDate).format("yyyy-MM-dd hh:mm:ss") + "</td> \r\n";
    oTR += "</tr> \r\n";

    fileTable.append(oTR);
}

function DeleteFile() {
    var chkList = $("#FileList").find("input[type='checkbox']:checked");

    if (chkList.length > 0) {
        bootbox.dialog({
            message: "是否确认删除?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        var idString = "";
                        for (var i = 0; i < chkList.length; i++) {
                            var chkObj = $(chkList[i]);
                            var ToolFileId = chkObj.next().val().split('_')[2];

                            if (ToolFileId != "-1") {
                                if (idString != "")
                                    idString += ',';
                                idString += ToolFileId;
                            }
                        }
                        $.ajax({
                            url: _RootPath + "SPWebAPI/Customer/DeleteToolFile?ToolFileIds=" + idString + "&token=" + _Token,
                            type: "Get",
                            data: null,
                            success: function (data) {
                                if (data.Flag == 1)
                                    chkList.parent().parent().remove();
                                else {
                                    bootbox.dialog({
                                        message: data.Message,
                                        buttons:
                                        {
                                            "click":
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
    }
    else {
        bootbox.dialog({
            message: "请至少选择一条数据！",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {

                    }
                }
            }
        });
    }
}

