<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseRequireList.aspx.cs" Inherits="WebUI.RequireManagement.ChooseRequireList" %>

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
    <script src="<%= ResolveUrl("~/Scripts/assets/js/uncompressed/jqGrid/jquery.jqGrid.js")%>" type="text/javascript"></script>
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
    <script type="text/javascript">
        var _RootPath = "<%= ResolveUrl("~")%>";
        var _Token = "<%= Token%>";
        var _CustomerId = "<%= CustomerId%>"; 
    </script>
    <script src="<%= ResolveUrl("~/Scripts/RequireManagement/ChooseRequireListJS.js")%>" type="text/javascript"></script>
</head>
<body class="no-skin">
    <form id="form1" runat="server">
        <div class="main-container" id="main-container">
            <div class="main-content">
                <div class="page-content" style="height:350px;">
                    <%--<div class="search-condition">
                        <div class="col-xs-12">
                            <div class="form-horizontal">
                                <div class="form-group" style="margin-top: 10px;">
                                    <label class="col-xs-2 col-sm-2 control-label no-padding-right">行业大类</label>
                                    <div class="col-sm-2">
                                        <asp:DropDownList ID="drpParentCategory" runat="server" CssClass="col-xs-2 col-sm-2">
                                            <asp:ListItem Text="--选择--" Value="-1" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <label class="col-xs-2 col-sm-2 control-label no-padding-right">行业小类</label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtCategoryName" runat="server" CssClass="col-xs-3 col-sm-3" MaxLength="30"></asp:TextBox>
                                    </div>
                                    <div>
                        <button class="wtbtnsearch" type="submit" id="btn_search" title="查询" onclick="return OnSearch();">
                            查询
                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                    <div class="form-horizontal">
                        <div class="hr hr-dotted"></div>
                        <div class="col-xs-12" id="frame" style="width: 100%; height: 100%; padding: 0px 0px 0px 0px;">
                            <table id="ChooseRequireList" width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-striped table-bordered table-hover dataTable"></table>
                            <div id="ChooseRequireListDiv"></div>
                        </div>
                    </div>
                    <asp:HiddenField ID="hidRequireId" runat="server" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
