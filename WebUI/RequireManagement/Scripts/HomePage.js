$(function () { 
    //LoadHeaderPictrue();
    //var cityLocation = window.headerTopInfo.gLocalizedCityName || '全国';
    var cityLocation = '全国';
    $("#lblCurrentCity").html(cityLocation);
    $("#lblCurrentCity2").html("当前城市：" + cityLocation);
    
    var searchAllObj = $("#btnSearchAll");
    if (searchAllObj) {
        searchAllObj.click(function () {
            var objType = $("#drpSearchAll");
            var objSearch = $("#txtSearch");
            if (objType.val() == "1") {
                window.location.href = "RequireList.aspx?CityId=-1&CategoryId=-1&SearchValue=" + objSearch.val();
            } else if (objType.val() == "2") {
                window.location.href = "AgencyList.aspx?CityId=-1&CategoryId=-1&SearchValue=" + objSearch.val();
            } else if (objType.val() == "3") {
                window.location.href = "BusinessList.aspx?CityId=-1&CategoryId=-1&SearchValue=" + objSearch.val();
            }
            return false;
        });
    }

    bindConfig();
    //var publishRequireObj = $("#btnPublishRequire");
    //if (publishRequireObj) {
    //    publishRequireObj.click(function () {
    //        if (_CustomerId > 0) {
    //            var requireModelVO = new Object();
    //            var requirementVO = new Object();

    //            requireModelVO.Requirement = requirementVO;

    //            var objCityId = $("select[id*='drpCity2']");
    //            var objCategoryId = $("select[id*='drpCategory2']");
    //            var objCommission = $("input[id*='txtCommission']");
    //            var objDescription = $("textarea[id*='txtDescription']");


    //            if (objDescription.val() == "") {
    //                bootbox.dialog({
    //                    message: "请输入任务描述!",
    //                    buttons:
    //                    {
    //                        "Confirm":
    //                        {
    //                            "label": "确定",
    //                            "className": "btn-sm btn-primary",
    //                            "callback": function () {

    //                            }
    //                        }
    //                    }
    //                });
    //                return false;
    //            }
    //            if (objCommission.val() == "") {
    //                bootbox.dialog({
    //                    message: "请输入酬金!",
    //                    buttons:
    //                    {
    //                        "Confirm":
    //                        {
    //                            "label": "确定",
    //                            "className": "btn-sm btn-primary",
    //                            "callback": function () {

    //                            }
    //                        }
    //                    }
    //                });
    //                return false;
    //            }


    //            requirementVO.RequirementId = -1;
    //            requirementVO.CityId = objCityId.val();
    //            requirementVO.CategoryId = objCategoryId.val();
    //            requirementVO.Title = "";
    //            requirementVO.CommissionType = 1;
    //            requirementVO.Commission = objCommission.val();
    //            requirementVO.Description = objDescription.val();

    //            $.ajax({
    //                url: _RootPath + "SPWebAPI/Require/UpdateRequire?token=" + _Token,
    //                type: "POST",
    //                data: requireModelVO,
    //                success: function (data) {
    //                    if (data.Flag == 1) {
    //                        bootbox.dialog({
    //                            message: data.Message,
    //                            buttons:
    //                            {
    //                                "Confirm":
    //                                {
    //                                    "label": "确定",
    //                                    "className": "btn-sm btn-primary",
    //                                    "callback": function () {
    //                                        window.location.href = "RequireManagement/RequirementBrowse.aspx";
    //                                    }
    //                                }
    //                            }
    //                        });
    //                    } else {
    //                        bootbox.dialog({
    //                            message: data.Message,
    //                            buttons:
    //                            {
    //                                "Confirm":
    //                                {
    //                                    "label": "确定",
    //                                    "className": "btn-sm btn-primary",
    //                                    "callback": function () {

    //                                    }
    //                                }
    //                            }
    //                        });
    //                    }

    //                },
    //                error: function (data) {
    //                    alert(data);
    //                }
    //            });
    //            return false;
    //        } else {
    //            //先登录
    //            bootbox.dialog({
    //                message: "请先登录再进行操作！",
    //                buttons:
    //                {
    //                    "Confirm":
    //                    {
    //                        "label": "确定",
    //                        "className": "btn-sm btn-primary",
    //                        "callback": function () {
    //                            window.location.href = "Login.aspx";
    //                        }
    //                    }
    //                }
    //            });
    //        }
    //        return false;
    //    });
    //}
});

function homediplay(id) {
    $("#divAgency").hide();
    $("#divRequire").hide();
    $("#divPublish").hide();

    $("#" + id).show();
}

function bindProvince(success, fail) {
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetProvinceList?enable=true",
        type: "Get",
        data: null,
        async: false,
        success: function (data) {
            if (data.Flag == 1) {
                if (success)
                    success(data);
            }
        },
        error: function (data) {
            if (fail)
                fail(data);
        }
    });
}

function bindCity(provinceId, success, fail) { 
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetCityList?provinceId=" + provinceId + "&enable=true",
        type: "GET",
        async: false,
        data: null,
        success: function (data) {
            if (data.Flag == 1) {

                if (success)
                    success(data);
            }
        },
        error: function (data) {
            if (fail)
                fail(data);
        }
    });
}

function bindParentCategory(success, fail) {
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetParentCategoryList?enable=true",
        type: "Get",
        data: null,
        async: false,
        success: function (data) {
            if (data.Flag == 1) {

                if (success)
                    success(data);
            }
        },
        error: function (data) {
            if (fail)
                fail(data);
        }
    });
}
function LoadHeaderPictrue(pics1)
{
    var p = $('#slide');
    if (p.css('width') == undefined)
        return;
    var width = p.css('width').split('px')[0];
    //var path = $('#txtTotalCommission').val();
    //var pics1 = [{ url: 'Style/images/img_HomePage_head.jpg', link: 'http://www.zhongxiaole.net/#', time: 5000 }, { url: 'Style/images/Koala.jpg', link: 'http://www.zhongxiaole.net/#', time: 4000 }, { url: 'Style/images/Lighthouse.jpg', link: 'http://www.zhongxiaole.net/', time: 6000 }, { url: 'Style/images/Penguins.jpg', link: 'http://www.zhongxiaole.net/', time: 6000 }, { url: 'Style/images/Tulips.jpg', link: 'http://www.zhongxiaole.net/', time: 6000 }];
    initPicPlayer(pics1, width, "350");

}


function bindConfig() {
    $.ajax({
        url: _RootPath + "SPWebAPI/System/GetConfig",
        type: "Get",
        data: null,
        async: false,
        success: function (data) {
            if (data.Flag == 1) {
                var configVO = data.Result;
                var pics1 = new Array();
                if (configVO.HeaderPic == undefined || configVO.HeaderPic == "") {
                    var pic = new Object();
                    pic.url = 'Style/images/img_HomePage_head.jpg';
                    pic.link = 'http://www.zhongxiaole.net/#';
                    pic.time = 5000;
                    pics1.push(pic);
                    //$("#liHeaderImage").css("background-image", "url(Style/images/img_HomePage_head.jpg)");
                    //var pics1 = [{ url: 'Style/images/img_HomePage_head.jpg', link: 'http://www.zhongxiaole.net/#', time: 5000 }];
                    LoadHeaderPictrue(pics1);
                }
                else {
                    var pics1 = new Array();
                    var picpath = configVO.HeaderPic.replace(';;;;', '').replace(';;;', '').replace(';;', '').replace(';', '');
                    var paths = configVO.HeaderPic.split(";");
                    if (paths.length == 0 || picpath=="") {
                        var pic = new Object();
                        pic.url = 'Style/images/img_HomePage_head.jpg';
                        pic.link = 'http://www.zhongxiaole.net/#';
                        pic.time = 5000;
                        pics1.push(pic);
                    } else {
                        for (var i = 0; i < paths.length; i++) {
                            if (paths[i] != "") {
                                var url =  paths[i];
                                var pic = new Object();
                                pic.url = url;
                                pic.link = 'http://www.zhongxiaole.net/#';
                                pic.time = 5000;                               
                                pics1.push(pic);
                            }
                        }
                    }
                    //var pics2 = [{ url: 'Style/images/img_HomePage_head.jpg', link: 'http://www.zhongxiaole.net/#', time: 5000 }];
                    LoadHeaderPictrue(pics1);
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
            load_hide();
        }
    });
}

function bindCategory(parentCategoryId, success, fail) {
    $.ajax({
        url: _RootPath + "SPWebAPI/User/GetChildCategoryList?parentCategoryId=" + parentCategoryId + "&enable=true",
        type: "GET",
        data: null,
        async: false,
        success: function (data) {
            if (data.Flag == 1) {

                if (success)
                    success(data);
            }
        },
        error: function (data) {
            if (fail)
                fail(data);
        }
    });
}


// 
// 
function initPicPlayer(pics, w, h) {
    //选中的图片 
    var selectedItem;
    //选中的按钮 
    var selectedBtn;
    //自动播放的id 
    var playID;
    //选中图片的索引 
    var selectedIndex;
    //容器 
    var p = $('#slide');
    p.text('');
    p.append('<div id="piccontent" style=" margin:0 auto;/*height:' + h + 'px";*/overflow:hidden></div>');
    var c = $('#piccontent');
    for (var i = 0; i < pics.length; i++) {
        //添加图片到容器中 
        //c.append('<a href="' + pics[i].link + '" target="_blank"><img id="picitem' + i + '" style="height:' + h + 'px;display: none;z-index:1;max-width:1200px;width:expression(this.width > 1200 ? "1200px" : this.width);" src="' + pics[i].url + '" /></a>');
        c.append('<a href="' + pics[i].link + '" target="_blank"><img id="picitem' + i + '" class="lgbanner_img" style="/*height:' + h + 'px;*/display: none;z-index:1;width:100%;" src="' + pics[i].url + '" /></a>');
    }
    //按钮容器，绝对定位在右下角 
    p.append('<div id="picbtnHolder" style="position:absolute;top:445px;width:' + w + 'px;height:20px;z-index:2;"></div>');
    // 
    var btnHolder = $('#picbtnHolder');
    btnHolder.append('<div id="picbtns" style="float:right; padding-right:1px;"></div>');
    var btns = $('#picbtns');
    // 
    for (var i = 0; i < pics.length; i++) {
        //增加图片对应的按钮 
        btns.append('<span id="picbtn' + i + '" style="cursor:pointer; border:solid 1px #ccc;background-color:#eee; display:inline-block;"> ' + (i + 1) + ' </span> ');
        $('#picbtn' + i).data('index', i);
        $('#picbtn' + i).click(
            function (event) {
                //if (selectedItem.attr('src') == $('#picitem' + $(this).data('index')).attr('src')) {
                //    return;
                //}
                setSelectedItem($(this).data('index'));
            }
        );
    }
    btns.append(' ');
    /// 
    setSelectedItem(0);
    //显示指定的图片index 
    function setSelectedItem(index) {
        selectedIndex = index;
        clearInterval(playID);
        ////alert(index); 
        //if (selectedItem) selectedItem.fadeOut('fast');
        //selectedItem = $('#picitem' + index);
        //selectedItem.fadeIn('slow');

        if (selectedItem) selectedItem.hide();
        selectedItem = $('#picitem' + index);
        selectedItem.show();
        // 
        if (selectedBtn) {
            selectedBtn.css('backgroundColor', '#eee');
            selectedBtn.css('color', '#000');
        }
        selectedBtn = $('#picbtn' + index);
        selectedBtn.css('backgroundColor', '#000');
        selectedBtn.css('color', '#fff');
        //自动播放 
        playID = setInterval(function () {
            var index = selectedIndex + 1;
            if (index > pics.length - 1) index = 0;
            setSelectedItem(index);
        }, pics[index].time);
    }
}
