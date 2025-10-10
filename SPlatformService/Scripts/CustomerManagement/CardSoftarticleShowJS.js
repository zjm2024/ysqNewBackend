$(document).ready(function () {
    
    $("#btn_cancel").click(function () {
        window.location.href = "CardSoftarticleBrowse.aspx";
        return false;
    });
    $("#saveread").click(function () {
        var SoftArticleID = parseInt($("#" + hidSoftArticleID).val());
        var ReadCount = $("input[id*='Textbox3']");
        var ReprintCount = $("input[id*='Textbox4']");
        var GoodCount = $("input[id*='Textbox5']");
        $.ajax({
            url: _RootPath + "SPWebAPI/Card/SoftArticleEditReadCount?SoftArticleID=" + SoftArticleID + "&ReadCount=" + ReadCount.val() + "&ReprintCount=" + ReprintCount.val() + "&GoodCount=" + GoodCount.val() + "&token=" + _Token,
            type: "get",
            success: function (data) {
                console.log(data)
                if (data.Flag == 1) {
                    bootbox.dialog({
                        message: "保存成功",
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
                console.log(data);
            }
        });
    });
    $("#btn_save").click(function () {
        console.log(hidSoftArticleID)
        var SoftArticleID = parseInt($("#" + hidSoftArticleID).val());
        bootbox.dialog({
            message: "是否删除这篇文章",
            buttons:
            {
                "click":
                {
                    "label": "确定",
                    "className": "btn-sm btn-primary",
                    "callback": function () {
                        $.ajax({
                            url: _RootPath + "SPWebAPI/Card/delSoftArticle?SoftArticleID=" + SoftArticleID + "&token=" + _Token,
                            type: "get",
                            success: function (data) {
                                console.log(data)
                                if (data.Flag == 1) {
                                    bootbox.dialog({
                                        message: "删除成功",
                                        buttons:
                                        {
                                            "Confirm":
                                            {
                                                "label": "确定",
                                                "className": "btn-sm btn-primary",
                                                "callback": function () {
                                                    window.location.href = "CardSoftarticleBrowse.aspx";
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


        
    });
});
