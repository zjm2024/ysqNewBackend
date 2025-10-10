$(document).ready(function () {
    Init();
    initDatePicker();
    $('#defaultSlider').on('input propertychange', function () {
        console.log($('#defaultSlider').val());
        $('#RangeValue').html($('#defaultSlider').val() + "%");
    });
    $("button[id$='btn_save']").click(function () {
        if (!$("form[id*='aspnetForm']").valid()) {
            return false;
        }
        var contractVO = GetContractVO();
        console.log(contractVO);
        $.ajax({
            url: _RootPath + "SPWebAPI/Project/UpdateContract?token=" + _Token,
            type: "POST",
            data: contractVO,
            success: function (data) {
                console.log(data);
                if (data.Flag == 1) {
                    $("#" + hidContractId).val(data.Result);
                    bootbox.dialog({
                        message: data.Message,
                        buttons:
                        {
                            "Confirm":
                            {
                                "label": "确定",
                                "className": "btn-sm btn-primary",
                                "callback": function () {
                                    window.location.href = "GenerateProject.aspx?ContractId=" + data.Result;
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
            }
        });
        return false;
    });
    $("button[id$='btn_businessapprove']").click(function () {
        var contractId = parseInt($("#" + hidContractId).val());
        UpdateContractStatus(contractId, 1, "B");
    });

    $("button[id$='btn_businesscancel']").click(function () {
        var contractId = parseInt($("#" + hidContractId).val());
        UpdateContractStatus(contractId, 0, "B");
    });

    $("button[id$='btn_agencyapprove']").click(function () {
        var contractId = parseInt($("#" + hidContractId).val());
        UpdateContractStatus(contractId, 1, "A");
    });

    $("button[id$='btn_agencycancel']").click(function () {
        var contractId = parseInt($("#" + hidContractId).val());
        UpdateContractStatus(contractId, 0, "A");
    });
});
var contractStepsList = new Array();//全局阶段VO
function Init() {
    ContractSteps_Init();
}
function ContractSteps_Init() {//把获取初始阶段VO
    ContractId = parseInt($("#" + hidContractId).val())
    if (ContractId > 0) {
        $.ajax({
            url: _RootPath + "SPWebAPI/Project/GetContractSteps?contractId=" + ContractId + "&token=" + _Token,
            type: "GET",
            data: null,
            async: false,
            success: function (data) {
                console.log(data);
                if (data.Flag == 1 && data.Result.length > 0) {
                    contractStepsList = data.Result;
                    ContractSteps_load();
                } else {
                    ContractSteps_default();
                }
            },
            error: function (data) {
                console.log(data);
            }
        });
    } else {
        ContractSteps_default();
    }
}
function ContractSteps_default() {//默认阶段设置
    var contractStepsList2 = new Array();
    var stepsVO = new Object();
    contractStepsList2.push(stepsVO);
    stepsVO.ContractId = ContractId;
    stepsVO.SortNO = 0;
    stepsVO.Cost = $("input[id*='txtCommission']").val();
    stepsVO.Title = "阶段一";
    contractStepsList = contractStepsList2
    ContractSteps_load();
}
function ContractSteps_load() {//把阶段VO打印到页面
    console.log(contractStepsList);
    var G_JieDuan_listObj = $('#G_JieDuan_list');
    var G_CommissionObj = $("input[id*='txtCommission']");
    var html = "";
    var Commission = 0;
    for (i = 0; i < contractStepsList.length; i++) {
        var k = new change(i +1+ "");
        contractStepsList[i].Title = "阶段" + k.pri_ary();
        contractStepsList[i].SortNO = i+1;
        html += "<div id=\"diy" + i + "\" class=\"G_JieDuan_div diy\" onclick=\"ContractSteps_alert("+i+")\">";
        html += "  " + contractStepsList[i].Title + "：" + contractStepsList[i].Cost + "元酬金";
        if (contractStepsList[i].Comment != "")
        {
            html += "   <div class=\"G_JieDuan_div_diy_set\">";
            html += contractStepsList[i].Comment;
            html += "   </div>";
        }
        html += "</div>";
        html += "<div class=\"G_JieDuan_line\"></div>";
        Commission += parseFloat(contractStepsList[i].Cost);
    }
    G_JieDuan_listObj.html(html);
    G_CommissionObj.val(Commission);
}
var index = 0;
function ContractSteps_alert(id) { //阶段设置弹窗
    index = id;
    var Commission_inputObj = $("input[id*='txtCommission_input']");
    var Comment_inputObj = $("input[id*='txtComment_input']");
    var newformalert_titleObj = $(".newformalert_title");
    Commission_inputObj.val(contractStepsList[index].Cost);
    Comment_inputObj.val(contractStepsList[index].Comment);
    newformalert_titleObj.html(contractStepsList[index].Title);
    $("#G_JieDuan_input").fadeIn(100);
}
function savebtn(){//设置阶段
    var Commission_inputObj = $("input[id*='txtCommission_input']");
    var Comment_inputObj = $("input[id*='txtComment_input']");
    contractStepsList[index].Cost = Commission_inputObj.val();
    contractStepsList[index].Comment = Comment_inputObj.val();
    ContractSteps_load();
    Newform_close('G_JieDuan_input');
}
function ContractSteps_del() {//删除阶段
    if (contractStepsList.length > 1) {
        bootbox.dialog({
            message: "是否删除阶段！",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        if (index > -1) {
                            contractStepsList.splice(index, 1);
                            console.log(contractStepsList);
                            ContractSteps_load();
                            Newform_close('G_JieDuan_input');
                        }
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
    } else {
        bootbox.dialog({
            message: "至少保留一个阶段！",
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
    
}
function ContractSteps_Add() {//添加阶段VO
    var stepsVO = new Object();
    contractStepsList.push(stepsVO);
    stepsVO.ContractId = parseInt($("#" + hidContractId).val());
    stepsVO.SortNO = 0;
    stepsVO.Cost = "0";
    stepsVO.Comment = "";
    ContractSteps_load();
}
function initDatePicker() {
    $('.date-picker').datepicker({
        format: "yyyy-mm-dd",
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
function GetContractVO() {
    var contractModelVO = new Object();
    var contractVO = new Object();
    contractModelVO.Contract = contractVO;

    var objStartDate = $("input[id*='txtStartDate']");
    var objProjectName = $("input[id*='txtProjectName']");
    var objEndDate = $("input[id*='txtEndDate']");
    var objCost = $("input[id*='txtCost']");
    var objCommission = $("input[id*='txtCommission']");
    var objdivContractNote = $("div[id*='divContractNote']");

    contractVO.ContractId = parseInt($("#" + hidContractId).val());
    contractVO.RequirementId = _RequireId;
    contractVO.CustomerId = _AgencyCustomerId;
    contractVO.ProjectName = objProjectName.val();
    contractVO.StartDate = objStartDate.val();
    contractVO.EndDate = objEndDate.val();
    contractVO.Cost = objCost.val();
    contractVO.Commission = objCommission.val();

    contractModelVO.ContractSteps = contractStepsList;

    var contractFileList = new Array();
    contractModelVO.ContractFile = contractFileList;
    var File_FileNameObjList = $("div[id*='divFileList']").find("input[name=\"File_FileName\"]");
    var File_FilePathObjList = $("div[id*='divFileList']").find("input[name=\"File_FilePath\"]");
    var File_CreatedAtObjList = $("div[id*='divFileList']").find("input[name=\"File_CreatedAt\"]");
    var File_ContractIdObjList = $("div[id*='divFileList']").find("input[name=\"File_ContractId\"]");

    for (var i = 0; i < File_FileNameObjList.length; i++) {
        var fileVO = new Object();
        contractFileList.push(fileVO);
        fileVO.ContractId = $(File_ContractIdObjList[i]).val();
        fileVO.FileName = $(File_FileNameObjList[i]).val();
        fileVO.FilePath = $(File_FilePathObjList[i]).val();
        fileVO.CreatedAt = $(File_CreatedAtObjList[i]).val();
    }

    return contractModelVO;
}
function SetButton() {
    //控制按钮
    //当前登录会员 ID   _CustomerId 
    //销售会员ID   _BusinessCustomerId 
    //雇主会员ID   _AgencyCustomerId

    //新建时当前登录会员是 雇主，只显示创建按钮
    if (_CustomerId == _BusinessCustomerId) {
        if (_AgencyStatus == 0 && _BusinessStatus == 0) {
            $("button[id$='btn_save']").show();
            $("button[id$='btn_deleteattach']").show();
            $("div[id$='divFileAdd']").show();
            $("button[id$='btn_newsteps']").show();
            $("button[id$='btn_deletesteps']").show();
        }
        else {
            $("button[id$='btn_save']").hide();
            $("button[id$='btn_deleteattach']").hide();
            $("div[id$='divFileAdd']").hide();
            $("button[id$='btn_newsteps']").hide();
            $("button[id$='btn_deletesteps']").hide();
        }

        if (_BusinessStatus == 0) {
            $("button[id$='btn_businesscancel']").hide();
            $("button[id$='btn_businessapprove']").show();
        }
        else {
            $("button[id$='btn_businesscancel']").show();
            $("button[id$='btn_businessapprove']").hide();
        }

        $("button[id$='btn_agencycancel']").hide();
        $("button[id$='btn_agencyapprove']").hide();

    } else if (_CustomerId == _AgencyCustomerId) {
        if (_AgencyStatus == 0 && _BusinessStatus == 0) {
            $("button[id$='btn_save']").show();
            $("button[id$='btn_deleteattach']").show();
            $("div[id$='divFileAdd']").show();
            $("button[id$='btn_newsteps']").show();
            $("button[id$='btn_deletesteps']").show();
        }
        else {
            $("button[id$='btn_save']").hide();
            $("button[id$='btn_deleteattach']").hide();
            $("div[id$='divFileAdd']").hide();
            $("button[id$='btn_newsteps']").hide();
            $("button[id$='btn_deletesteps']").hide();
        }

        if (_AgencyStatus == 0) {
            $("button[id$='btn_agencycancel']").hide();
            $("button[id$='btn_agencyapprove']").show();
        }
        else {
            $("button[id$='btn_agencycancel']").show();
            $("button[id$='btn_agencyapprove']").hide();
        }

        $("button[id$='btn_businesscancel']").hide();
        $("button[id$='btn_businessapprove']").hide();
    }
    if (_BusinessStatus == 1) { $("img[id$='huzhutonghi']").show(); } else { $("img[id$='huzhutonghi']").hide(); }
    if (_AgencyStatus == 1) { $("img[id$='xiaoshoutonghi']").show(); } else { $("img[id$='xiaoshoutonghi']").hide(); }
}
function UpdateContractStatus(contractId, status, type) {
    load_show();
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/UpdateContractStatus?contractId=" + contractId + "&status=" + status + "&type=" + type + "&token=" + _Token,
        type: "POST",
        data: null,
        success: function (data) {
            load_hide();
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
                                if (type == "B") {
                                    _BusinessStatus = status;
                                } else if (type == "A") {
                                    _AgencyStatus = status;
                                }
                                SetButton();
                            }
                        }
                    }
                });
            } else if (data.Flag == 2) {
                bootbox.dialog({
                    message: data.Message,
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {
                                if (type == "B") {
                                    window.location.href = "BusinessProjectCreateEdit.aspx?ProjectId=" + data.Result;
                                } else if (type == "A") {
                                    window.location.href = "AgencyProjectCreateEdit.aspx?ProjectId=" + data.Result;
                                }

                            }
                        }
                    }
                });
            } else if (data.Flag == 3) {
                bootbox.dialog({
                    message: data.Message,
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {
                                if (type == "B") {
                                    window.location.href = "../CustomerManagement/BusinessCreateEdit.aspx?page=RealName";
                                } else if (type == "A") {
                                    window.location.href = "../CustomerManagement/AgencyCreateEdit.aspx?page=RealName";
                                }

                            }
                        }
                    }
                });
            } else if (data.Flag == 4) {
                bootbox.dialog({
                    message: data.Message,
                    buttons:
                    {
                        "Confirm":
                        {
                            "label": "确定",
                            "className": "btn-sm btn-primary",
                            "callback": function () {
                                window.location.href = "../RequireManagement/RequirementCreateEdit.aspx?RequirementId=" + data.Result;
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
            load_hide();
            //alert(data);
        }
    });
}

function changefile(uploadId) {
    var tempPath = new Date().format("yyyyMM");
    uploadFile(uploadId, tempPath, function (data) {
        console.log(data);
        if (data.Flag == 1) {
            var fileVO = new Object();
            fileVO.ContractId = parseInt($("#" + hidContractId).val());
            fileVO.FileName = data.Result.FileName;
            fileVO.FilePath = data.Result.FilePath.replace("~", _APIURL);
            fileVO.CreatedAt = new Date();
            //fileVO.CreatedBy = _CustomerId;

            AddFile(fileVO);
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

function BindFile(contractId) {
    $("table[id*='FileList'").find("tbody>tr").remove();
    $.ajax({
        url: _RootPath + "SPWebAPI/Project/GetContractFile?contractId=" + contractId + "&token=" + _Token,
        type: "Get",
        data: null,
        success: function (data) {
            if (data.Flag == 1) {
                var puVOList = data.Result;
                for (var i = 0; i < puVOList.length; i++) {
                    var puVO = puVOList[i];
                    puVO.FilePath = puVO.FilePath.replace("~", _APIURL);
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
    var fileTable = $(".G_JieDuan_file_add");
    var oTR = "";
    oTR += "   <div class=\"jqgrow\" onclick=\"DeleteFile(this)\"> \r\n";
    var fileName = puVO.FileName;
    var suffixIndex = fileName.lastIndexOf(".");
    oTR += "<a href=\"" + puVO.FilePath + "\" title=\"" + puVO.FileName + "\">";
    var suffix = fileName.substring(suffixIndex + 1).toUpperCase();
    if (suffix != "BMP" && suffix != "JPG" && suffix != "JPEG" && suffix != "PNG" && suffix != "GIF") {
        oTR += "  <div class=\"File\"></div>\r\n";
    } else {
        oTR += "  <div class=\"img\" style=\"background-image:url(" + puVO.FilePath + ")\"></div>\r\n";
    }
    oTR += "</a>";
    oTR += "      <p class=\"center\" title=\"" + puVO.FileName + "\">" + puVO.FileName + "</p> \r\n";
    oTR += "      <p class=\"JqgrowDel\" title=\"删除\">删除</p> \r\n";
    oTR += "      <input type=\"hidden\" name=\"File_FileName\" value=\"" + puVO.FileName + "\" /> \r\n";
    oTR += "      <input type=\"hidden\" name=\"File_FilePath\" value=\"" + puVO.FilePath + "\" /> \r\n";
    oTR += "      <input type=\"hidden\" name=\"File_CreatedAt\" value=\"" + new Date(puVO.CreatedAt).format("yyyy-MM-dd hh:mm:ss") + "\" /> \r\n";
    oTR += "      <input type=\"hidden\" name=\"File_ContractId\" value=\"" + puVO.ContractId + "\" /> \r\n";
    oTR += "   </div> \r\n";
    
    fileTable.before(oTR);
}

function DeleteFile(obj) {
    var chkList = $(obj);
    bootbox.dialog({
        message: "是否确认删除?",
        buttons:
        {
            "click":
            {
                "label": "确定",
                "className": "btn-sm btn-primary",
                "callback": function () {
                    chkList.remove();
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
var _change = {
    ary0: ["零", "一", "二", "三", "四", "五", "六", "七", "八", "九"],
    ary1: ["", "十", "百", "千"],
    ary2: ["", "万", "亿", "兆"],
    init: function (name) {
        this.name = name;
    },
    strrev: function () {
        var ary = []
        for (var i = this.name.length; i >= 0; i--) {
            ary.push(this.name[i])
        }
        return ary.join("");
    }, //倒转字符串。
    pri_ary: function () {
        var $this = this
        var ary = this.strrev();
        var zero = ""
        var newary = ""
        var i4 = -1
        for (var i = 0; i < ary.length; i++) {
            if (i % 4 == 0) { //首先判断万级单位，每隔四个字符就让万级单位数组索引号递增
                i4++;
                newary = this.ary2[i4] + newary; //将万级单位存入该字符的读法中去，它肯定是放在当前字符读法的末尾，所以首先将它叠加入$r中，
                zero = ""; //在万级单位位置的“0”肯定是不用的读的，所以设置零的读法为空

            }
            //关于0的处理与判断。
            if (ary[i] == '0') { //如果读出的字符是“0”，执行如下判断这个“0”是否读作“零”
                switch (i % 4) {
                    case 0:
                        break;
                        //如果位置索引能被4整除，表示它所处位置是万级单位位置，这个位置的0的读法在前面就已经设置好了，所以这里直接跳过
                    case 1:
                    case 2:
                    case 3:
                        if (ary[i - 1] != '0') {
                            zero = "零"
                        }
                        ; //如果不被4整除，那么都执行这段判断代码：如果它的下一位数字（针对当前字符串来说是上一个字符，因为之前执行了反转）也是0，那么跳过，否则读作“零”
                        break;

                }

                newary = zero + newary;
                zero = '';
            }
            else { //如果不是“0”
                newary = this.ary0[parseInt(ary[i])] + this.ary1[i % 4] + newary; //就将该当字符转换成数值型,并作为数组ary0的索引号,以得到与之对应的中文读法，其后再跟上它的的一级单位（空、十、百还是千）最后再加上前面已存入的读法内容。
            }

        }
        if (newary.indexOf("零") == 0) {
            newary = newary.substr(1)
        }//处理前面的0
        return newary;
    }
}
//创建class类
function change() {
    this.init.apply(this, arguments);
}
change.prototype = _change