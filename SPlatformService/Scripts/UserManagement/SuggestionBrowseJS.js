$(function () {
    SetButton();
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "SuggestionList";
    grid.jqGrid.PagerID = "SuggestionListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=SuggestionList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("SuggestionId", "操作", false, "center", 50,
        function (obj, options, rowObject) {
            var result = '';
            result += '<input type=\'hidden\' name="test_' + obj + '" id="test_' + obj + '" value="' + obj + '"/><img  src="../Style/images/edit.png" id="btn_Edit" style="cursor:pointer" title="编辑" onclick="return EditSuggestion(this);"></img>';
            var isDelete = $("#" + hidIsDelete).val();
            if (isDelete)
                result += '<img  src="../Style/images/delet.png" id="btn_Delete" style="cursor:pointer" title="删除" onclick="return DeleteSuggestionOne(this);"></img>';
            return result;
        }, false);
    grid.jqGrid.AddHidColumn("SuggestionId");
			
	grid.jqGrid.AddColumn("ContactPerson", "联系人", true, null, 50);				
	grid.jqGrid.AddColumn("ContactPhone", "联系电话", true, null, 50);				
	grid.jqGrid.AddColumn("Title", "标题", true, null, 50);				
	grid.jqGrid.AddColumn("Description", "内容", true, null, 100);				
	grid.jqGrid.AddColumn("CreatedAt", "时间", true, null, 50);				
    grid.jqGrid.CreateTable();   
}


function EditSuggestion(suggestionObj) {
    window.location.href = "SuggestionCreateEdit.aspx?SuggestionId=" + $(suggestionObj).prev().val();
    return false;
}

function NewSuggestion() {
    window.location.href = "SuggestionCreateEdit.aspx";
    return false;
}

function DeleteSuggestionOne(suggestionObj) {
    bootbox.dialog({
        message: "是否确认删除?",
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    var suggestionId = $(suggestionObj).prev().prev().val();
                    DeleteAction(suggestionId);
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

function DeleteSuggestion() {
    var id = $("#SuggestionList").jqGrid('getGridParam', 'selarrrow');
    var idString = '';
    for (var i = 0; i < id.length; i++) {
        if (i != 0) {
            idString += ',';
        }
        idString += id[i];
    }
    if (id.length > 0) {
        bootbox.dialog({
            message: "是否确认删除?",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        DeleteAction(idString);
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

function DeleteAction(suggestionId) {
    $.ajax({
        url: _RootPath + "SPWebAPI/System/DeleteSuggestion?suggestionId=" + suggestionId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
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
                                window.location.href = "SuggestionBrowse.aspx";
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
            //load_hide();
        },
        error: function (data) {
            alert(data);
            //load_hide();
        }
    });
}

function SetButton() {
    var btnNew = $("#btn_new");
    var btnDelete = $("#btn_delete");
    var isEdit = $("#" + hidIsEdit).val();
    var isDelete = $("#" + hidIsDelete).val();

    if (isEdit == "true") {
        btnNew.show();
    } else {
        btnNew.hide();
    }
    
    if (isDelete == "true") {
        btnDelete.show();
    } else {
        btnDelete.hide();
    }
    
}

function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objSuggestionName = $("input[id*='txtSuggestionName']");
    grid.jqGrid.AddSearchParams("Title", "LIKE", objSuggestionName.val());

    grid.jqGrid.Search();
    return false;
}

