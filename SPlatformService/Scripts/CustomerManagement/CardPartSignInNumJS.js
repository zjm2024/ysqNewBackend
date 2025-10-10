$(function () {
    onPageInit();
});


var grid = new JGrid();
var grid2 = new JGrid();
var grid3 = new JGrid();
function onPageInit() {

    var PartID = $("input[id*='PartID']").val();
    grid.jqGrid.ID = "PartSignList";
    grid.jqGrid.PagerID = "PartSignListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=PartSignList&PartID=" + PartID;
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "isAutoAdd asc,LuckDrawStatus";
    grid.jqGrid.DefaultSort = "asc";
    grid.jqGrid.Multiselect = true;


    grid.jqGrid.AddHidColumn("PartySignUpID");	
    grid.jqGrid.AddColumn("Name", "名字", true, "center", 50);
    grid.jqGrid.AddColumn("Phone", "联系电话", true, "center", 50);
    grid.jqGrid.AddColumn("CostName", "购买类型", true, "center", 50);
    grid.jqGrid.AddColumn("Cost", "金额", true, "center", 50, function (obj, options, rowObject) {
        if (rowObject.OrderStatus == 2) {
            var showdata = "";
            showdata += "<div>";
            showdata += obj;
            showdata += "<span style='color:#f00'>(已退款)</span></div>";
            return showdata;
        } else {
            return obj;
        }
    }, false);
    grid.jqGrid.AddColumn("SignUpForm", "报名信息", true, null, 50,
        function (obj, options, rowObject) {
            var dd = obj.split("</SignUpForm><SignUpForm>")
            var showdata = "";

            for (var i = 0; i < dd.length; i++) {
                if (dd[i].match(/<Name>.*<\/Name>/i) != "姓名" && dd[i].match(/<Name>.*<\/Name>/i) != "手机")
                {
                    showdata += "<div>";
                    showdata += dd[i].match(/<Name>.*<\/Name>/i)
                    showdata += "：";
                    showdata += dd[i].match(/<Value>.*<\/Value>/i)
                    showdata += "</div>";
                }
            }
            return showdata;
        }, false);
    grid.jqGrid.AddColumn("PromotionAwardName", "邀请人", true, "center", 30,
        function (obj, options, rowObject) {
            var result = "<div onclick=\"showCustomerCard('" + rowObject.InviterCID + "')\"  style='color:#197e9c;cursor:pointer;'  >" + obj + "</div>";
            return result;
        }, false);
    grid.jqGrid.AddColumn("PromotionAwardCost", "返佣", true, "center", 30,
        function (obj, options, rowObject) {
            return "<div style='color:#882323'>" + obj + "</div>";
        }, false);
    grid.jqGrid.AddColumn("CreatedAt", "报名时间", true, "center", 50);

    grid.jqGrid.AddColumn("FjUrl", "附件地址", true, "center", 50);

    grid.jqGrid.AddColumn("LuckDrawStatus", "是否中奖", true, "center", 50,
       function (obj, options, rowObject) {
           var r=""
           if (obj == 1) {
               r = "<div style='color:#882323'>" + rowObject.LuckDrawContent + "</div>";
           }else{
               r="-"
           }
           return r;
    }, false);

    grid.jqGrid.AddColumn("FjUrl", "下载附件", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div   onclick=\"downloadfile('" + obj + "')\"  style='color:#0066ff;cursor:pointer;'  >下载</div>";
        }, false);
    grid.jqGrid.AddColumn("CustomerId", "操作会员", true, "center", 50,
        function (obj, options, rowObject) {
            return "<div   onclick=\"EditCustomer('" + obj + "')\"  style='color:#0066ff;cursor:pointer;'  >查看会员(开通VIP)</div>";
        }, false);
    grid.jqGrid.CreateTable();  


    grid2.jqGrid.ID = "PartyInviterList";
    grid2.jqGrid.PagerID = "PartyInviterListDiv";
    grid2.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid2.jqGrid.Params = "table=PartyInviterList&PartID=" + PartID;
    grid2.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid2.jqGrid.DefaultSortCol = "CountNum";
    grid2.jqGrid.DefaultSort = "DESC";
    grid2.jqGrid.Multiselect = true;


    grid2.jqGrid.AddHidColumn("CustomerId");
    grid2.jqGrid.AddColumn("Name", "名字", true, "center", 50);
    grid2.jqGrid.AddColumn("Phone", "联系电话", true, "center", 50);
    grid2.jqGrid.AddColumn("CountNum", "邀约人数", true, "center", 50);
    grid2.jqGrid.CreateTable();


    grid3.jqGrid.ID = "CardAccessRecordsViewList";
    grid3.jqGrid.PagerID = "CardAccessRecordsViewListDiv";
    grid3.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid3.jqGrid.Params = "table=CardAccessRecordsViewList&PartID=" + PartID;
    grid3.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid3.jqGrid.DefaultSortCol = "AccessRecordsID";
    grid3.jqGrid.DefaultSort = "asc";
    grid3.jqGrid.Multiselect = true;

    grid3.jqGrid.AddHidColumn("AccessRecordsID");
    grid3.jqGrid.AddColumn("HeaderLogo", "头像", true, "center", 20,
        function (obj, options, rowObject) {

            var result = '';
            if (obj != null && obj != "unknown" && obj != "")
                result += '<img  src="' + obj + '" id="btn_Edit" style="cursor:pointer; " width="50" height="50" title="查看"></img>';
            else
                result += '-';
            return result;
        }, false);
    grid3.jqGrid.AddColumn("CustomerName", "名字", true, "center", 20,
        function (obj, options, rowObject) {
            var result = "";
            if (rowObject.CustomerId > 0)
                result = "<div onclick=\"showCustomerCard('" + rowObject.CustomerId + "')\"  style='color:#197e9c;cursor:pointer;'  >" + obj + "</div>";
            else
                result = "匿名";
            return result;
        }, false);
    grid3.jqGrid.AddColumn("Nation", "记录时间", true, "center", 20);
    grid3.jqGrid.AddColumn("Type", "操作", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == "ReadParty") {
                return "<div style='color:#058c9a;'>浏览活动</div>";
            }
            if (obj == "ForwardParty") {
                return "<div style='color:#bd3718;'>转发活动</div>";
            }
            if (obj == "SignUpParty") {
                return "<div style='color:#f00;'>报名活动</div>";
            }
            else {
                return "-";
            }
        });
    grid3.jqGrid.AddColumn("count", "次数", true, "center", 20);
    grid3.jqGrid.AddColumn("LoginIP", "登录IP", true, "center", 20);
    grid3.jqGrid.AddColumn("Province", "省", true, "center", 20);
    grid3.jqGrid.AddColumn("City", "市", true, "center", 20);
    grid3.jqGrid.AddColumn("District", "区", true, "center", 20);
    grid3.jqGrid.CreateTable();
}

function showCustomerCard(Obj) {
    window.open("CardBrowse.aspx?CustomerId=" + Obj);
    return false;
}

function downloadfile(Obj) {
    console.log("下载附件", Obj)
    let dowmloadDom = document.createElement('a');
    dowmloadDom.href = Obj;
    //dowmloadDom.download = filename 修改文件名
    document.body.appendChild(dowmloadDom);
    dowmloadDom.click();
    document.body.removeChild(dowmloadDom);
}

function EditCustomer(Obj) {
    window.open("CustomerCreateEdit.aspx?CustomerId=" + Obj);
    return false;
}

function getExcel() {
    var PartID = $("input[id*='PartID']").val();
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/getSignUplistToExcel2?PartyID=" + PartID + "&token=" + _Token,
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
                                window.open(data.Result)
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
            load_hide();
        }
    });
}

function OnSearch() {
    var objRequirementName = $("input[id*='txtRequirementName']");
    var PartID = $("input[id*='PartID']").val();
    
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/autoSignUpByPartyID?PartyID=" + PartID + "&Count=" + objRequirementName.val()+ "&token=" + _Token,
        type: "GET",
        success: function (data) {
            console.log(data)
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
                                location.reload();
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
            console.log(data)
        }
    });

    return false;
}

function OnSearch2() {
    var TextBox1 = $("input[id*='TextBox1']");
    var PartID = $("input[id*='PartID']").val();
    
    $.ajax({
        url: _RootPath + "SPWebAPI/Card/addLookByPartyID?PartyID=" + PartID + "&Count=" + TextBox1.val()+ "&token=" + _Token,
        type: "GET",
        success: function (data) {
            console.log(data)
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
                                location.reload();
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
            console.log(data)
        }
    });

    return false;
}
