$(function () {
    onPageInit();
});

var grid = new JGrid();
var grid2 = new JGrid();
function onPageInit() {
    grid.jqGrid.ID = "AgencyList";
    grid.jqGrid.PagerID = "AgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CardList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("CustomerId", "会员ID", true, "center", 25);
    grid.jqGrid.AddColumn("CustomerId", "操作", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div onclick='SelectPartyCard(" + obj + ")' style='cursor:pointer;'>设置为发起人</div>";
        }, false);
    
    grid.jqGrid.AddColumn("Headimg", "头像", true, "center", 20,
        function (obj, options, rowObject) {
            
            var result = '';
            if (obj != null && obj != "unknown" && obj != "")
                result += '<img  src="' + obj + '" id="btn_Edit" style="cursor:pointer; " width="50" height="50" title="查看"></img>';
            else
                result += '-';
            return result;
        }, false);
    grid.jqGrid.AddColumn("CardImg", "名片码", true, "center",20,
        function (obj, options, rowObject) {
            return "<div id=‘QRImg" + rowObject.CardID + "’  onclick=\"showQRImg('" + obj + "')\"  style='color:red;cursor:pointer;'  >查看</div>";
        }, false);
    grid.jqGrid.AddColumn("Name", "名称", true, null,25);
    grid.jqGrid.AddColumn("Position", "职位", true, null, 30);
    grid.jqGrid.AddColumn("Phone", "手机号码", true, null, 30);
    grid.jqGrid.AddColumn("WeChat", "微信号", true, null, 30);
    grid.jqGrid.AddColumn("CorporateName", "公司名称", true, null, 50);
    grid.jqGrid.AddColumn("Address", "地址", true, null, 50);
    grid.jqGrid.AddColumn("Business", "主营业务", true, null, 50);
    grid.jqGrid.AddColumn("ReadCount", "浏览", true, "center", 10);
    grid.jqGrid.AddColumn("Collection", "收藏", true, "center", 10);
    grid.jqGrid.AddColumn("Forward", "转发", true, "center", 10);
    
    grid.jqGrid.AddColumn("CreatedAt", "创建时间", true, null,30);
    grid.jqGrid.CreateTable();

    var PartID = parseInt($("#" + hidPartID).val());
    grid2.jqGrid.ID = "PartyContactsList";
    grid2.jqGrid.PagerID = "PartyContactsListDiv";
    grid2.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid2.jqGrid.Params = "table=PartyContactsList&PartID=" + PartID;
    grid2.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid2.jqGrid.DefaultSortCol = "CreatedAt";
    grid2.jqGrid.DefaultSort = "desc";
    grid2.jqGrid.Multiselect = true;
    grid2.jqGrid.AddColumn("CustomerId", "会员ID", true, "center", 25);
    grid2.jqGrid.AddColumn("CustomerId", "操作", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div onclick='SelectPartyCard(" + obj + ")' style='cursor:pointer;'>设置为发起人</div>";
        }, false);

    grid2.jqGrid.AddColumn("Headimg", "头像", true, "center", 20,
        function (obj, options, rowObject) {

            var result = '';
            if (obj != null && obj != "unknown" && obj != "")
                result += '<img  src="' + obj + '" id="btn_Edit" style="cursor:pointer; " width="50" height="50" title="查看"></img>';
            else
                result += '-';
            return result;
        }, false);
    grid2.jqGrid.AddColumn("CardImg", "名片码", true, "center", 20,
        function (obj, options, rowObject) {
            return "<div id=‘QRImg" + rowObject.CardID + "’  onclick=\"showQRImg('" + obj + "')\"  style='color:red;cursor:pointer;'  >查看</div>";
        }, false);
    grid2.jqGrid.AddColumn("Name", "名称", true, null, 25);
    grid2.jqGrid.AddColumn("Position", "职位", true, null, 30);
    grid2.jqGrid.AddColumn("Phone", "手机号码", true, null, 30);
    grid2.jqGrid.AddColumn("WeChat", "微信号", true, null, 30);
    grid2.jqGrid.AddColumn("CorporateName", "公司名称", true, null, 50);
    grid2.jqGrid.AddColumn("Address", "地址", true, null, 50);
    grid2.jqGrid.AddColumn("Business", "主营业务", true, null, 50);
    grid2.jqGrid.AddColumn("ReadCount", "浏览", true, "center", 10);
    grid2.jqGrid.AddColumn("Collection", "收藏", true, "center", 10);
    grid2.jqGrid.AddColumn("Forward", "转发", true, "center", 10);

    grid2.jqGrid.AddColumn("CreatedAt", "创建时间", true, null, 30);
    grid2.jqGrid.CreateTable();
}
function SelectPartyCard(obj) {
    var PartID = parseInt($("#" + hidPartID).val());
    console.log(obj)
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/SelectPartyCard?PartID=" + PartID + "&CustomerId=" + obj + "&token=" + _Token,
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
                                window.history.go(-1);
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
            alert(data.Message);
            load_hide();
        }
    });
}
function CopyUrl(obj) {
    bootbox.dialog({
        message: "pages/ShowCard/ShowCard?CardID=" + obj,
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



function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var objAgencyName = $("input[id*='txtAgencyName']");    

    if (objAgencyName.val() != "") {

        //grid.jqGrid.AddSearchParams("AgencyName", "LIKE", objAgencyName.val());
        var filedArr = new Array();
        filedArr.push("Name");
        filedArr.push("Position");
        filedArr.push("Phone");
        filedArr.push("CorporateName");
        grid.jqGrid.AddLikeSearchForListParams(filedArr, objAgencyName.val());
    }

    var objStatus = $("select[id*='drpStatus']");
    if (objStatus.val() > -1)
        grid.jqGrid.AddSearchParams("Status", "=", objStatus.val());

    var objRealNameStatus = $("select[id*='drpRealNameStatus']");
    if (objRealNameStatus.val() > -1)
        grid.jqGrid.AddSearchParams("RealNameStatus", "=", objRealNameStatus.val());

    grid.jqGrid.Search();
    return false;
}

