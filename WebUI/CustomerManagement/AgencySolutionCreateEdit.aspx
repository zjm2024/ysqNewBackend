<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgencySolutionCreateEdit.aspx.cs" Inherits="WebUI.CustomerManagement.AgencySolutionCreateEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>众销乐-资源共享众包销售平台</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Scripts/assets/css/bootstrap.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Scripts/assets/font-awesome-4.4.0/css/font-awesome.min.css")%>" />

    <link rel="stylesheet" href="<%= ResolveUrl("~/Scripts/assets/css/ace-skins.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Scripts/assets/css/ace-rtl.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Scripts/assets/css/ace.onpage-help.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Scripts/assets/css/jquery-ui.custom.min.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Scripts/assets/css/chosen.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Scripts/assets/css/datepicker.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Scripts/assets/css/bootstrap-timepicker.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Scripts/assets/css/daterangepicker.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Scripts/assets/css/bootstrap-datetimepicker.css")%>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Scripts/assets/css/colorpicker.css")%>" />
    <!--[if lte IE 8]>
        <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Scripts/assets/css/ace-ie.min.css")%>" />
        <style type="text/css">
            html
            {
                position:static;
            }

        input.ace[type="radio"]
        {
            padding-left:25px;
        }
        </style>
		<![endif]-->
    <script src="<%= ResolveUrl("~/Scripts/Windows.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/ace-extra.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/jquery1x.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/Loading.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/Validation.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/StringExtend.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/DateExtend.js")%>" type="text/javascript"></script>

    <script src="<%= ResolveUrl("~/Scripts/assets/js/bootstrap.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/jquery-ui.custom.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/jquery.ui.touch-punch.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/jquery.easypiechart.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/jquery.sparkline.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/flot/jquery.flot.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/flot/jquery.flot.pie.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/flot/jquery.flot.resize.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/chosen.jquery.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/fuelux/fuelux.spinner.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/date-time/bootstrap-datepicker.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/date-time/bootstrap-timepicker.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/date-time/moment.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/date-time/daterangepicker.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/date-time/bootstrap-datetimepicker.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/bootstrap-colorpicker.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/messages_cn.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/jquery.knob.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/jquery.autosize.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/jquery.inputlimiter.1.3.1.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/jquery.maskedinput.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/bootstrap-tag.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/bootbox.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/ace-elements.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/date-time/locales/bootstrap-datepicker.zh-CN.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/jqTable.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/jqGrid/i18n/grid.locale-cn.js")%>" type="text/javascript"></script>
    <link href="<%= ResolveUrl("~/Scripts/assets/css/ui.jqgrid.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Style/css/jqGrid.css")%>" rel="stylesheet" type="text/css" />
    <script src="<%= ResolveUrl("/Scripts/assets/js/uncompressed/jqGrid/jquery.jqGrid.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/jqTable.js")%>" type="text/javascript"></script>
    <link rel="stylesheet" href="<%= ResolveUrl("~/Scripts/assets/css/ace.min.css")%>" />

    <!--[if lte IE 8]>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/assets/js/html5shiv.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/assets/js/respond.min.js")%>" type="text/javascript"></script>
    <![endif]-->

    <style type="text/css">
        .imgKefu {
            width: 48px;
            height: 48px;
            cursor: pointer;
            margin-top: 3px;
            margin-right: 5px;
        }

        #divmessage .badge {
            top: -12px !important;
        }

        #divmessage .badge-success {
            background-color: #DB3F3F !important;
        }
    </style>
    <link rel="stylesheet" href="<%= ResolveUrl("~/Style/css/ListPage.css")%>" />
    <link href="<%= ResolveUrl("~/Style/css/bootboxplus.css")%>" rel="stylesheet" />
    <link href="<%= ResolveUrl("~/Style/css/wap.css")%>" rel="stylesheet" />
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ajaxfileupload.js")%>"></script>
    <script type="text/javascript">
        var _RootPath = "<%= ResolveUrl("~")%>";
        var _Token = "<%= Token%>";
        var _CustomerId = "<%= CustomerId%>";
        var _APIURL = "<%= APIURL%>";
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

            initDatePicker();

            $("form[id*='form1'").validate({
                errorElement: 'div',
                errorClass: 'help-block',
                focusInvalid: true,
                rules: {
                    txtCost: {
                        number: true,
                        required: true
                    }
                },
                messages: {
                    txtCost: {
                        number: "请输入正确数值！",
                        required: "请输入项目金额！"
                    }
                },
                highlight: function (e) {
                    $(e).closest('.form-group').removeClass('has-info').addClass('has-error');
                },

                success: function (e) {
                    $(e).closest('.form-group').removeClass('has-error');
                    $(e).remove();
                }
            });

        });

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

        function changefile(uploadId) {
            var tempPath = new Date().format("yyyyMM");
            load_show();
            uploadFile(uploadId, tempPath, function (data) {
                if (data.Flag == 1) {
                    var solutionFileVO = new Object();

                    solutionFileVO.AgencySolutionId = -1;
                    solutionFileVO.FileName = data.Result.FileName;
                    solutionFileVO.FilePath = data.Result.FilePath.replace("~", _APIURL);
                    AddFile(solutionFileVO);
                    load_hide();
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

        function AddFile(puVO) {
            var fileTable = $("#FileList");
            var oTR = "";
            oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
            oTR += "  <td class=\"center\"> \r\n";
            oTR += "    <input class=\"cbox\" type=\"checkbox\" /> \r\n";
            oTR += "    <input type=\"hidden\" value=\"File_" + puVO.AgencySolutionId + "_" + puVO.AgencySolutionFileId + "\" /> \r\n";
            oTR += "  </td> \r\n";
            oTR += "  <td class=\"center\" title=\"" + puVO.FileName + "\"><a target=\"_blank\" href=\"" + puVO.FilePath + "\">" + puVO.FileName + "</a></td> \r\n";
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
                                chkList.parent().parent().remove();
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


    </script>
</head>
<body class="no-skin">
    <form id="form1" runat="server">
        <div class="main-container" id="main-container" style=" min-height:initial">
            <div class="main-content">
                <div class="page-content" style="height: 400px;">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="form-horizontal l_dxal">
                                <div class="form-group">
                                    <label class="col-sm-2 col-xs-2 control-label no-padding-right need">客户名称 </label>

                                    <asp:TextBox ID="txtClientName" runat="server" CssClass="col-sm-5 col-xs-5"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-2 col-xs-2 control-label no-padding-right need">项目名称 </label>

                                    <asp:TextBox ID="txtProjectName" runat="server" CssClass="col-sm-5 col-xs-5"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-2 col-xs-2 control-label no-padding-right need">项目金额 </label>

                                    <asp:TextBox ID="txtCost" runat="server" CssClass="col-sm-5 col-xs-5"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 col-xs-2 control-label no-padding-right need">成交时间 </label>

                                    <div class="col-sm-5 col-xs-5 input-group">
                                        <asp:TextBox ID="txtProjectDate" runat="server" CssClass="form-control date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <i class="fa fa-calendar bigger-110"></i>
                                        </span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 col-xs-2 control-label no-padding-right need">上传证明材料（如合同等）</label>

                                    <div class="col-sm-5 col-xs-5 input-group">
                                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                                            <input id="fileAdd" name="id-input-file" type="file" onchange="changefile('fileAdd')" />
                                            <button class="wtbtn yjright" type="button" id="btn_deleteattach" onclick="return DeleteFile();" title="删除" style="margin:0px;">
                                                <i class="icon-ok bigger-110"></i>
                                                删除
                                            </button>
                                            <div class="space-4"></div>
                                            <table id="FileList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                                <thead>
                                                    <tr class="ui-jqgrid-labels">
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                                            <div class="" title="">
                                                                <%--<input class="cbox" type="checkbox" onchange="checkAll(this, 'UserProjectList')" />--%>
                                                            </div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="文件名称">文件名称</div>
                                                        </th>
                                                        <%--<th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                                <div class="ui-jqgrid-sortable" title="上传日期">上传日期</div>
                                                            </th>--%>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <%--<tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                                        <td style="" title=""></td>
                                                        <td style="" title=""></td>
                                                        <td style="" title=""></td>
                                                    </tr>--%>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 col-xs-2 control-label no-padding-right need">隐私设置</label>
                                    <div class="col-sm-5 col-xs-5 input-group Privacy_style">
                                        <label for="Privacyrad1">
                                            <input name="PrivacyType" class="ace" value="0" type="radio" id="Privacyrad1" checked="checked" onclick="$('.alKeyword_div').hide();"/>
                                            <span class="lbl">公开</span>
                                        </label>
                                        <label for="Privacyrad2">
                                            <input name="PrivacyType" class="ace alKeyword_input" value="1" type="radio" id="Privacyrad2" onclick="$('.alKeyword_div').show();"/>
                                            <span class="lbl">关键字过滤</span>
                                        </label>
                                        <label for="Privacyrad3">
                                            <input name="PrivacyType" class="ace" value="2" type="radio" id="Privacyrad3" onclick="$('.alKeyword_div').hide();"/>
                                            <span class="lbl">保密</span>
                                        </label>
                                    </div>
                                </div>
                                <div class="form-group alKeyword_div">
                                    <label class="col-sm-2 col-xs-2 control-label no-padding-right need">关键字 </label>
                                    <asp:TextBox ID="Keyword" runat="server" CssClass="col-sm-5 col-xs-5" placeholder="如果有多个关键字请用 , 隔开"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    </form>
</body>
</html>
