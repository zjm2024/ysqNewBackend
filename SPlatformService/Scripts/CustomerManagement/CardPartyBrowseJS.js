$(function () {
    onPageInit();
});

var grid = new JGrid();
function onPageInit() {
    var LuckDraw = $("#" + isLuckDraw).val();
    console.log(LuckDraw);
    grid.jqGrid.ID = "AgencyList";
    grid.jqGrid.PagerID = "AgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";

    if (LuckDraw == "true") {
        grid.jqGrid.Params = "table=PartyList&isLuckDraw=true";
    } else {
        grid.jqGrid.Params = "table=PartyList&isLuckDraw=false";
    }
    
    console.log(grid.jqGrid.Params)

    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("Title", "活动标题", true, null, 120);
    if (LuckDraw != "true") {
        grid.jqGrid.AddColumn("Type", "类型", true, "center", 50,
            function (obj, options, rowObject) {
                var str = "活动";
                if (obj == 2) {
                    str = "商品";
                }
                if (obj == 3) {
                    str = "抽奖";
                }
                return str;
            }, false);
    }
    grid.jqGrid.AddColumn("Name", "发起人", true, "center", 30,
        function (obj, options, rowObject) {
            var result = "<div onclick=\"showCustomerCard('" + rowObject.CustomerId + "')\"  style='color:#197e9c;cursor:pointer;'  >点击查看</div>";
            return result;
        }, false);
    grid.jqGrid.AddColumn("PartyID", "操作", true, "center",30,
        function (obj, options, rowObject) {
            return "<div onclick='SelectPartyCard(" + obj + ")' style='cursor:pointer;'>更改发起人</div>";
        }, false);
    if (LuckDraw != "true") {
        grid.jqGrid.AddColumn("PartyID", "操作", true, "center", 30,
            function (obj, options, rowObject) {
                var str = "";
                if (rowObject.Type == 3) {
                    str = "--";
                } else {
                    if (rowObject.Details != "头条活动") {
                        str = "<div onclick='SetCompanyPartyCard(" + obj + ")' style='cursor:pointer;'>绑定头条文章</div>";
                    } else {
                        str = "<div style='color:#882323;'>当前头条推荐活动</div>";
                    }

                }
                return str;
            }, false);
    }
    grid.jqGrid.AddColumn("QRCodeImg", "小程序码", true, "center", 30,
        function (obj, options, rowObject) {
            return "<div id=‘QRImg" + rowObject.PartyID + "’  onclick=\"showQRImg('" + obj + "')\"  style='color:red;cursor:pointer;'  >查看</div>";
        }, false);
    grid.jqGrid.AddColumn("QRImg", "小程序码（带名称）", true, "center", 30,
        function (obj, options, rowObject) {
            return "<div id=‘QRImg" + rowObject.PartyID + "’  onclick=\"showQRImg('" + obj + "')\"  style='color:red;cursor:pointer;'  >查看</div>";
        }, false);
    grid.jqGrid.AddColumn("PartyID", "活动链接", true, "center", 30,
        function (obj, options, rowObject) {
            return "<div   onclick=\"CopyUrl('" + obj + "'," + rowObject.Type + ")\"  style='color:red;cursor:pointer;'  >查看链接</div>";
        }, false);
    grid.jqGrid.AddColumn("ReadCount", "浏览次数", true, "center", 30);
    grid.jqGrid.AddColumn("RecordSignUpCount", "报名人数", true, "center", 30);
    /*
    grid.jqGrid.AddColumn("counts", "报名人数", true, "center", 30); 
    grid.jqGrid.AddColumn("Cost", "报名金额", true, "center", 30);
    */
    grid.jqGrid.AddColumn("CreatedAt", "创建时间", true, "center", 30);
    grid.jqGrid.AddColumn("StartTime", "开始时间", true, "center", 30);
    //grid.jqGrid.AddColumn("SignUpTime", "报名截止", true, "center", 30);
    //grid.jqGrid.AddColumn("EndTime", "结束时间", true, "center", 30);


    if (LuckDraw == "true") {
        grid.jqGrid.AddColumn("PartyID", "状态设置", true, "center", 80,
        function (obj, options, rowObject) {
            var str = "<div>";
            if (rowObject.Type == 3) {
                if (rowObject.isHot == 1)
                    str += "<span onclick='delHotParty(" + obj + ")' style='color:#882323;cursor:pointer;margin-right:10px'>取消推荐</span>";
                else {
                    str += "<span onclick='setHotParty(" + obj + ")' style='cursor:pointer;margin-right:10px'>推荐抽奖</span>";
                }
                if (rowObject.isIndex == 1)
                    str += "<span onclick='delIndexParty(" + obj + ")' style='color:#882323;cursor:pointer;margin-right:10px'>取消首页</span>";
                else {
                    str += "<span onclick='setIndexParty(" + obj + ")' style='cursor:pointer;margin-right:10px'>首页展示</span>";
                }
                if (rowObject.isNoDraw == 1)
                    str += "<span onclick='delNoDrawParty(" + obj + ")' style='color:#882323;cursor:pointer;margin-right:10px'>取消官方</span>";
                else {
                    str += "<span onclick='setNoDrawParty(" + obj + ")' style='cursor:pointer;margin-right:10px'>官方抽奖</span>";
                }
                if (rowObject.isRepeat == 1)
                    str += "<span onclick='delRepeatParty(" + obj + ")' style='color:#882323;cursor:pointer;margin-right:10px'>取消循环</span>";
                else {
                    str += "<span onclick='setRepeatParty(" + obj + ")' style='cursor:pointer;margin-right:10px'>循环开奖</span>";
                }
            }
            str += "</div>";
            return str;
        }, false);

        grid.jqGrid.AddColumn("PartyID", "操作", true, "center", 60,
        function (obj, options, rowObject) {
            var str = "<div>";
            str += "<span onclick='CopyPartyCard(" + obj + ")' style='cursor:pointer;margin-right:10px'>复制抽奖</span>";
            str += "<span onclick='SelectPartyPeople(" + obj + ")' style='cursor:pointer;margin-right:10px'>查看报名</span>";
            str += "<span onclick='DelParty(" + obj + ")' style='cursor:pointer'>删除抽奖</span>";
            str += "</div>";
            return str;
        }, false);
    } else {
        grid.jqGrid.AddColumn("PartyID", "操作", true, "center", 30,
         function (obj, options, rowObject) {
             var str = "<div>";
             str += "<span onclick='SelectPartyPeople(" + obj + ")' style='cursor:pointer'>查看报名</span>";
             str += "</div>";
             return str;
         }, false);
    }
    
    grid.jqGrid.CreateTable(); 
}



function setHotParty(PartyID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/setHotParty?PartyID=" + PartyID + "&token=" + _Token,
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
                                window.location.href = "CardPartyBrowse.aspx?isLuckDraw=true";
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

function SetCompanyPartyCard(PartyID) {
    bootbox.dialog({
        message: "是否将该活动绑定到所有头条文章中",
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    $.ajax({
                        url: _RootPath + "SPWebAPI/Card/SetCompanyPartyCard?PartyID=" + PartyID + "&token=" + _Token,
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
                                                window.location.href = "CardPartyBrowse.aspx";
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
function DelParty(PartyID) {
    bootbox.dialog({
        message: "是否删除该活动，并删除所有报名",
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    $.ajax({
                        url: _RootPath + "SPWebAPI/Card/DelLuckDrawParty?PartyID=" + PartyID + "&token=" + _Token,
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
                                                //window.location.href = "CardPartyBrowse.aspx?isLuckDraw=true";
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
                                console.log(data);
                            }
                            load_hide();
                        },
                        error: function (data) {
                            console.log(data);
                            load_hide();
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

function CopyPartyCard(PartyID) {
    bootbox.dialog({
        message: "是否复制并创建抽奖",
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    $.ajax({
                        url: _RootPath + "SPWebAPI/Card/CopyPartyCard?PartyID=" + PartyID + "&token=" + _Token,
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
                                                window.location.href = "CardPartyBrowse.aspx?isLuckDraw=true";
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
                                console.log(data);
                            }
                            load_hide();
                        },
                        error: function (data) {
                            console.log(data);
                            load_hide();
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


function delHotParty(PartyID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/delHotParty?PartyID=" + PartyID + "&token=" + _Token,
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
                                window.location.href = "CardPartyBrowse.aspx?isLuckDraw=true";
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

function setIndexParty(PartyID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/setIndexParty?PartyID=" + PartyID + "&token=" + _Token,
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
                                window.location.href = "CardPartyBrowse.aspx?isLuckDraw=true";
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

function delIndexParty(PartyID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/delIndexParty?PartyID=" + PartyID + "&token=" + _Token,
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
                                window.location.href = "CardPartyBrowse.aspx?isLuckDraw=true";
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


function setNoDrawParty(PartyID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/setNoDrawParty?PartyID=" + PartyID + "&token=" + _Token,
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
                                window.location.href = "CardPartyBrowse.aspx?isLuckDraw=true";
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

function delNoDrawParty(PartyID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/delNoDrawParty?PartyID=" + PartyID + "&token=" + _Token,
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
                                window.location.href = "CardPartyBrowse.aspx?isLuckDraw=true";
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

function setRepeatParty(PartyID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/setRepeatParty?PartyID=" + PartyID + "&token=" + _Token,
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
                                window.location.href = "CardPartyBrowse.aspx?isLuckDraw=true";
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

function delRepeatParty(PartyID) {
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/delRepeatParty?PartyID=" + PartyID + "&token=" + _Token,
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
                                window.location.href = "CardPartyBrowse.aspx?isLuckDraw=true";
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

function SelectPartyPeople(obj) {
    window.open("CardPartSignInNum.aspx?PartID=" + obj);
}

function SelectPartyCard(obj) {
    window.open("SelectPartyCard.aspx?PartID=" + obj);
}

function CopyUrl(obj, Type) {
    var url = "pages/Party/PartyShow/PartyShow?PartyID=" + obj;
    if (Type == 3) {
        url = "package/package_sweepstakes/PartyShow/PartyShow?PartyID=" + obj;
    }
    bootbox.dialog({
        message: url,
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

function showQRImg(obj) {
    window.open(obj);
}

function EditAgency(agencyObj) {
    window.location.href = "AgencyCreateEdit.aspx?AgencyId=" + $(agencyObj).prev().val();
    return false;
}

function showCustomerCard(Obj) {
    window.open("CardBrowse.aspx?CustomerId=" + Obj);
    return false;
}

function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objAgencyName = $("input[id*='txtAgencyName']");    

    if (objAgencyName.val() != "") {

        //grid.jqGrid.AddSearchParams("AgencyName", "LIKE", objAgencyName.val());
        var filedArr = new Array();
        filedArr.push("Title");
        grid.jqGrid.AddLikeSearchForListParams(filedArr, objAgencyName.val());
    }
    grid.jqGrid.Search();
    return false;
}

