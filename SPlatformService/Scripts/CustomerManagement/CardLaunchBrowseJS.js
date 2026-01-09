$(function () {
    onPageInit();
    initDatePicker();
});

var grid = new JGrid();
function onPageInit() {
    var CustomerId = parseInt($("#" + hidCustomerId).val());
    grid.jqGrid.ID = "AgencyList";
    grid.jqGrid.PagerID = "AgencyListDiv";
    grid.jqGrid.Source = "../Common/GetDataJsonGrid.ashx";
    grid.jqGrid.Params = "table=CardLaunchList";
    grid.jqGrid.LengthMenu = ["20", "50", "100", "150"];
    grid.jqGrid.DefaultSortCol = "CreatedAt";
    grid.jqGrid.DefaultSort = "desc";
    grid.jqGrid.Multiselect = true;
    grid.jqGrid.AddColumn("Type", "小程序", true, "center", 20,
        function (obj, options, rowObject) {
            if (obj == 4) {
                return "<div style='color:#ff6600;'>活动星选</div>";
            }
            if (obj == 2) {
                return "<div style='color:#0066ff;'>引流王</div>";
            }
            if (obj == 0) {
                return "<div style='color:#6600ff;'>企业名片</div>";
            }
            if (obj == 1) {
                return "<div style='color:#6077ff;'>乐聊名片</div>";
            }
            else {
                return "其他";
            }
        }, false);
    grid.jqGrid.AddColumn("appId", "来源小程序", true, "center", 50,
        function (obj, options, rowObject) {
            if (obj == "wx125c4d21e07ea73b") {
                return "<div style='color:#c500ad;'>微养鸡(" + obj + ")</div>";
            }
            if (obj == "wxce9ef8f5a289b382") {
                return "<div style='color:#ff6600;'>成语赚钱传(" + obj + ")</div>";
            }
            if (obj == "wxd73f0e0e08f3dabb") {
                return "<div style='color:#00a1c5;'>答对题赚钱(" + obj + ")</div>";
            }
            if (obj == "wxc250df0e29b98176") {
                return "<div style='color:#640794;'>答对领奖金(" + obj + ")</div>";
            }
            if (obj == "wxc57544f5ad8dd14e") {
                return "<div style='color:#f7157d;'>成语领奖金(" + obj + ")</div>";
            }
            if (obj == "wx692151a221921607") {
                return "<div style='color:#379407;'>小鹿零元购(" + obj + ")</div>";
            }
            if (obj == "wx584477316879d7e9") {
                return "<div>乐聊名片(" + obj + ")</div>";
            }
            if (obj == "wx83bf84d3847abf2f") {
                return "<div>活动星选(" + obj + ")</div>";
            }
            if (obj == "wx3c4c53653292320a") {
                return "<div>在线收费(" + obj + ")</div>";
            }
            if (obj == "wxd5c41df0d8f45688") {
                return "<div>报名收费(" + obj + ")</div>";
            }
            if (obj == "wxf1ce360baa9e19e4") {
                return "<div>活动组织(" + obj + ")</div>";
            }
            if (obj == "wx1c34dcce7c6687f6") {
                return "<div>聚个会(" + obj + ")</div>";
            }
            if (obj == "wxbb680a9f9588ef08") {
                return "<div>建名片(" + obj + ")</div>";
            }
            if (obj == "wx1208aba186ab4fea") {
                return "<div>名片聊天(" + obj + ")</div>";
            }
            if (obj == "wxa323f254ee2cec24") {
                return "<div>实时通讯录(" + obj + ")</div>";
            }
            if (obj == "wxc9245bafef27dddf") {
                return "<div>悦售(" + obj + ")</div>";
            }
            if (obj == "wxdc35ba3b02a97c92") {
                return "<div>乐聊人脉广场(" + obj + ")</div>";
            }
            else {
                return "未知(" + obj + ")";
            }
        }, false);
    grid.jqGrid.AddColumn("path", "页面路径", true, null, 50);
    grid.jqGrid.AddColumn("scene", "场景", true, "center", 30,
        function (obj, options, rowObject) {
            if (obj == 1037) {
                return "其他小程序";
            }
            if (obj == 1038) {
                return "其他小程序返回";
            }
            else {
                return "未知";
            }
        }, false);
    grid.jqGrid.AddColumn("openId", "openId", true, null, 30);
    grid.jqGrid.AddColumn("CreatedAt", "登录时间", true, "center", 20,
        function (obj, options, rowObject) {
            var dateTime = new Date(obj);;
            var year = dateTime.getFullYear();
            var month = dateTime.getMonth() + 1;
            var day = dateTime.getDate();
            var hour = dateTime.getHours();
            var minute = dateTime.getMinutes();
            if (hour < 10) {
                hour = "0" + hour;
            }
            if (minute < 10) {
                minute = "0" + minute;
            }

            var second = dateTime.getSeconds();
            var now = new Date();
            var now_new = Date.parse(now.toDateString());  //typescript转换写法

            var milliseconds = 0;
            var timeSpanStr;

            milliseconds = now - dateTime;

            if (milliseconds <= 1000 * 60 * 60 * 24) {
                timeSpanStr = '今天';
            }
            else if (1000 * 60 * 60 * 24 < milliseconds && milliseconds <= 1000 * 60 * 60 * 24 * 30) {
                timeSpanStr = Math.round(milliseconds / (1000 * 60 * 60 * 24)) + '天前';
            }
            else if (milliseconds > 1000 * 60 * 60 * 24 * 30 && year == now.getFullYear()) {
                timeSpanStr = month + '月' + day + '日';
            } else {
                timeSpanStr = year + '年' + month + '月' + day + '日';
            }
            return year + '年' + month + '月' + day + '日' + ' ' + hour + ':' + minute;
        });
    grid.jqGrid.AddColumn("LoginIP", "登录IP", true, null, 25);
    grid.jqGrid.CreateTable();   
}

function OnSearch() {
    grid.jqGrid.InitSearchParams();    

    var StartDate = $("input[id*='txtEffectiveEndDate']");
    var EndDate = $("input[id*='txtEffectiveEndDate2']");

    if (StartDate.val() != "")
        grid.jqGrid.AddSearchParams("CreatedAt", ">=", StartDate.val());

    if (EndDate.val() != "")
        grid.jqGrid.AddSearchParams("CreatedAt", "<", EndDate.val());

    var objappId = $("select[id*='drpappId']");
    if (objappId.val() != "")
        grid.jqGrid.AddSearchParams("appId", "=", objappId.val());

    var objType = $("select[id*='drpType']");
    if (objType.val() > -1)
        grid.jqGrid.AddSearchParams("Type", "=", objType.val());

    var objscene = $("select[id*='drpscene']");
    if (objscene.val() > -1)
        grid.jqGrid.AddSearchParams("scene", "=", objscene.val());

    grid.jqGrid.Search();
    return false;
}

function initDatePicker() {
    $('.date-picker').datepicker({
        format: "yyyy-mm-dd 00:00:00",
        language: "zh-CN",
        autoclose: true,
        todayHighlight: true
    }).next().on("click", function () {
        $(this).prev().focus();
    });
    $('.timepicker1').timepicker({
        minuteStep: 1,
        defaultTime: false,
        showSeconds: true,
        showMeridian: false, showWidgetOnAddonClick: false
    }).next().on("click", function () {
        $(this).prev().focus();
    });

    $('.date-picker-yyyy').datepicker({
        minViewMode: 'years',
        format: 'yyyy',
        autoclose: true,
        startViewMode: 'year',
        startDate: '1900',
        endDate: '2100',
        language: 'zh-CN'
    })
    .next().on("click", function () {
        $(this).prev().focus();
    });
}

function OnExcel() {
    var objStartDate = $("input[id*='txtEffectiveEndDate']");
    var objEndDate = $("input[id*='txtEffectiveEndDate2']");

    var StartDate = "";
    var EndDate = "";
    var appId = "";
    var Type = "";
    var scene = "";

    if (objStartDate.val() != "")
        StartDate = objStartDate.val();

    if (objEndDate.val() != "")
        EndDate = objEndDate.val();

    var objappId = $("select[id*='drpappId']");
    if (objappId.val() != "")
        appId = objappId.val();

    var objType = $("select[id*='drpType']");
    if (objType.val() > -1)
        Type = objType.val();

    var objscene = $("select[id*='drpscene']");
    if (objscene.val() > -1)
        scene = objscene.val();


    var url = _RootPath + "SPWebAPI/Card/GetLaunchExcel?StartDate=" + StartDate + "&EndDate=" + EndDate + "&appId=" + appId + "&Type=" + Type + "&scene=" + scene;

    console.log(url);
    $.ajax({
        url: url,
        type: "get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                bootbox.dialog({
                    message: data.Message,
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "下载",
                            "className": "btn-sm btn-primary",
                            "callback": function () {
                                window.open(data.Result)
                            }
                        }
                    }
                });
            } else {
                console.log(data)
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
            console.log(data);
            //load_hide();
        }
    });
}