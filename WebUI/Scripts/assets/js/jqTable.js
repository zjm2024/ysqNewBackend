function JTable() {
    this.jqTable = {
        ID: null, //页面table对象的id
        Columns: null, //列定义
        table: null, //CreateTable执行后生成的新table对象
        Params: null, //其它参数
        Source: null, //指向获取数据返回json的地址
        LengthMenu: null, //页数据量定义 例： [5,10,20,50,100,ALL]
        DefaultSortCol: null, //默认排序列序号
        DefaultSort: "asc", //默认排序方式
        //    test: [{ 'mData': 'BookId', 'bSortable': false, 'sClass': 'center', 'sWidth': '1%', 'sTitle': '', 'fnRender': function (obj) { return '<input type=\'hidden\' name="test_' + obj.aData.BookId + '" id="test_' + obj.aData.BookId + '" value="' + obj.aData.BookId + '"/><button class="btn btn-xs btn-info" id="btn_Edit" onclick="return EditBook(this);"><i class="ace-icon fa fa-pencil bigger-120"></i></button>'; } }
        //                            , { 'mData': 'BookName', 'sTitle': 'Book Name' }
        //                            , { 'mData': 'Price', 'sTitle': 'Price' }
        //                            , { 'mData': 'Remark', 'sTitle': 'Remark' }
        //                            ],
        CreateTable: function (/*允许翻页*/isPaginate, /*允许改变页数据量*/isLengthChange, /*允许排序*/isSort, /*初始页数据量*/DisplayLength, /*处理时是否显示Processing信息*/isUseProcessing, /*是否使用checkbox全选*/isHaveCheckBox) {
            var Context = {}
            Context.oLanguage = {
                "sLengthMenu": "每页显示 _MENU_ 条记录",
                "sZeroRecords": "抱歉， 没有找到",
                "sInfo": "从 _START_ 到 _END_ /共 _TOTAL_ 条数据",
                "sInfoEmpty": "没有数据",
                "sInfoFiltered": "(从 _MAX_ 条数据中检索)",
                "oPaginate": {
                    "sFirst": "首页",
                    "sPrevious": "前一页",
                    "sNext": "后一页",
                    "sLast": "尾页"
                },
                "sZeroRecords": "没有检索到数据",
                "sProcessing": "正在处理..."
            };
            Context.bPaginate = isPaginate;
            Context.bLengthChange = isLengthChange;
            Context.bFilter = false;
            Context.bSort = isSort;
            Context.iDisplayLength = DisplayLength;
            Context.bProcessing = isUseProcessing;
            Context.bServerSide = true;
            Context.sAjaxSource = this.GetUrl();
            Context.sServerMethod = 'POST';
            Context.bStateSave = true;
            Context.aoColumns = this.Columns;
            if (this.LengthMenu != null) {
                Context.aLengthMenu = this.LengthMenu;
            }
            if (this.DefaultSortCol != null) {
                Context.aaSorting = [[this.DefaultSortCol, this.DefaultSort]];
            }
            this.table = $("#" + this.ID).dataTable(Context);
            //            $("#" + this.ID).dataTable({
            //                'oLanguage': {
            //                    "sLengthMenu": "每页显示 _MENU_ 条记录",
            //                    "sZeroRecords": "抱歉， 没有找到",
            //                    "sInfo": "从 _START_ 到 _END_ /共 _TOTAL_ 条数据",
            //                    "sInfoEmpty": "没有数据",
            //                    "sInfoFiltered": "(从 _MAX_ 条数据中检索)",
            //                    "oPaginate": {
            //                        "sFirst": "首页",
            //                        "sPrevious": "前一页",
            //                        "sNext": "后一页",
            //                        "sLast": "尾页"
            //                    },
            //                    "sZeroRecords": "没有检索到数据",
            //                    "sProcessing": "正在处理..."
            //                },
            //                'bPaginate': isPaginate, //true,
            //                'bLengthChange': isLengthChange, //true,
            //                'bFilter': false,
            //                'bSort': isSort, //true,
            //                'iDisplayLength': DisplayLength, //10,
            //                //'sPaginationType': 'full_numbers',            
            //                'bProcessing': isUseProcessing, //true,
            //                'bServerSide': true,
            //                'sAjaxSource': this.GetUrl(), //'Handler1.ashx',
            //                'sServerMethod': 'POST',
            //                'aoColumns': this.Columns
            //                            [{ 'mData': 'BookId', 'bSortable': false, 'sClass': 'center', 'sWidth': '1%', 'sTitle': '', 'fnRender': function (obj) { return '<input type=\'hidden\' name="test_' + obj.aData.BookId + '" id="test_' + obj.aData.BookId + '" value="' + obj.aData.BookId + '"/><button class="btn btn-xs btn-info" id="btn_Edit" onclick="return EditBook(this);"><i class="ace-icon fa fa-pencil bigger-120"></i></button>'; } }
            //                            , { 'mData': 'BookName', 'sTitle': 'Book Name' }
            //                            , { 'mData': 'Price', 'sTitle': 'Price' }
            //                            , { 'mData': 'Remark', 'sTitle': 'Remark' }
            //                            ]


            //"fnServerParams": function (aoData) {
            //    aoData.push({ "name": "name1", "value": "value1" });
            //    aoData.push({ "name": "name2", "value": "value2" });
            //}
            //            });
            //this.table = $("#" + this.ID)[0];
            //            //            $("#" + this.ID + "_wrapper")[0].children[0].style = "height:50px";
            //            $("#" + this.ID + "_wrapper").find("div[class='row']").each(function () {
            //                this.style.height = "50px";
            //            });
            //            this.table.style.width = "100%";
            //            //this.table.style.display = "inline-block";
            //            this.table.style.clear = "none";
            if (isHaveCheckBox) {
                $(document).on('click', 'th input:checkbox', function () {
                    var that = this;
                    $(this).closest('table').find('tr > td:first-child input:checkbox')
		            .each(function () {
		                this.checked = that.checked;
		                $(this).closest('tr').toggleClass('selected');
		            });
                });
            }

            //this.table.find("i[class$='fa fa-angle-double-left']")[0]);
            var i1 = $("i[class='fa fa-angle-double-left']");
            i1.removeClass();
            i1.addClass("fa-angle-double-left");
            i1[0].setAttribute("style", "display:inline-block;font-family:FontAwesome;font-style:normal;font-weight:normal;");
            var i2 = $("i[class='fa fa-angle-left']");
            i2.removeClass();
            i2.addClass("fa-angle-left");
            i2[0].setAttribute("style", "display:inline-block;font-family:FontAwesome;font-style:normal;font-weight:normal;");
            var i3 = $("i[class='fa fa-angle-right']");
            i3.removeClass();
            i3.addClass("fa-angle-right");
            i3[0].setAttribute("style", "display:inline-block;font-family:FontAwesome;font-style:normal;font-weight:normal;");
            var i4 = $("i[class='fa fa-angle-double-right']");
            i4.removeClass();
            i4.addClass("fa-angle-double-right");
            i4[0].setAttribute("style", "display:inline-block;font-family:FontAwesome;font-style:normal;font-weight:normal;");
            $("div[class='dataTables_paginate paging_bootstrap']")[0].parentElement.style = "z-index:1000";
        },

        //生成url
        GetUrl: function () {
            if (this.Params != null) {
                return this.Source + "?" + this.Params;
            }
            return this.Source;
        },


        Column: function () {
            this.mData = null;
            this.sTitle = null;
            this.bSortable = null;
            this.sClass = null;
            this.sWidth = null;
            this.fnRender = null;
        },

        //添加列
        AddColumn: function (/*返回json中的列名*/columnname, /*页面里显示的列名*/title, /*是否允许该列排序*/isSortable, /*样式名*/cssclass, /*宽度*/width, /*渲染*/special) {
            if (this.Columns == null) {
                this.Columns = [];
            }
            var column = {}
            column.mData = columnname;
            if (title != null) {
                column.sTitle = title
            }
            if (isSortable != null) {
                column.bSortable = isSortable
            }
            if (cssclass != null) {
                column.sClass = cssclass
            }
            if (width != null) {
                column.sWidth = width
            }
            if (special != null) {
                column.fnRender = special
            }
            this.Columns.push(column);
        }
    };
}

function JGrid() {
    this.jqGrid = {
        ID: null, //页面table对象的id
        PagerID: null,
        Title: null,
        Columns: null, //列定义
        SubColumns: null, //列定义
        table: null, //CreateTable执行后生成的新table对象
        Params: null, //其它参数
        Source: null, //指向获取数据返回json的地址
        LengthMenu: null, //页数据量定义 例： [5,10,20,50,100,ALL]
        ColNames: null,
        DefaultSortCol: null, //默认排序列
        DefaultSort: "asc", //默认排序方式
        Data: null,
        Reader: null,
        Multiselect: false,
        Multiboxonly: false,
        Canhide: false, //表格是否可隐藏
        SearchParams: null,
        IndexPage: null,
        IsGrouping: false, //是否分组
        GroupingView: null, //分组视图组成

        Sub: null,
        SubParams: null, //指向获取数据返回json的地址
        SubGridRowExpanded: null,
        SubParam: null,

        BeforeRequest: null, //此事件发生在任何数据请求前，datatype为function时不触发

        CreateTable: function () {
            var jqThat = this;
            var Context = {}
            if (this.Data != null) {
                Context.data = this.Data;
                Context.datatype = 'local';
            }
            else {
                Context.url = this.GetUrl();
                Context.datatype = 'json';
            }
            Context.colNames = this.ColNames;
            Context.colModel = this.Columns;
            Context.pager = this.PagerID;
            Context.sortname = this.DefaultSortCol;
            Context.viewrecords = true;
            Context.sortorder = this.DefaultSort;
            Context.caption = this.Title;
            Context.hidegrid = this.Canhide;
            //Context.multiselect = true;
            if (this.IndexPage != null) {
                Context.page = this.IndexPage;
            }
            if (this.LengthMenu != null) {
                Context.rowList = this.LengthMenu;
                Context.rowNum = this.LengthMenu[1];
            }
            if (this.GroupingView != null) {
                Context.groupingView = this.GroupingView;
            }
            Context.grouping = this.IsGrouping;
            Context.multiselect = this.Multiselect;
            Context.multiboxonly = this.Multiboxonly;
            //            Context.multiboxonly = true;
            //Context.height = "100%";
            //Context.width = "100%";

            if (this.Sub!=null) {
                Context.subGrid = true;
                Context.subGridOptions = {
                    plusicon: "ace-icon fa fa-plus center bigger-110 blue",
                    minusicon: "ace-icon fa fa-minus center bigger-110 blue",
                    openicon: "ace-icon fa fa-chevron-right center orange"
                };
                if (this.SubGridRowExpanded == null) {
                    Context.subGridUrl = this.GetSubUrl();
                    Context.subGridModel = [this.SubColumns];
                } else {
                    Context.subGridRowExpanded = this.SubGridRowExpanded;
                }
            }

            if (this.BeforeRequest != null && $.isFunction(this.BeforeRequest)) {
                Context.beforeRequest = function () {
                    jqThat.BeforeRequest.call(jqThat, "");
                }
            }

            Context.altRows = true;
            Context.autoencode = true;
            Context.loadComplete = function () {
                var table = this;
                if (typeof (autoresize_s) == "undefined") {
                    autoresize(this.id);
                }
                else {
                    autoresize_s();
                }
                if (typeof (OtherJqGridBind) != "undefined") {
                    OtherJqGridBind();
                }
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                    title_column_header(table);
                    //search_filter_init();
                    if (getInternetExplorerVersion().VERSION < 9) {
                        ButtonReBind();
                        RadioReBind();
                        LinkReBind();
                        ImgReBind();
                    }
                }, 0);
            };
            Context.jsonReader = {
                root: "invdata",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                cell: "invrow"
            }
            //            Context.beforeSelectRow = function (rowid, e) {
            //                //                if (e.target.type == "button") {
            //                                    eval(e.target.onclick);
            //                //                }
            //                return false;
            //            };
            //            Context.onCellSelect = null; function (rowid, index, contents, event) {
            //                $("#" + rowid + "_planprice").change(function () {
            ////                    var btn = event.target;
            ////                    btn.click();
            //                    $("#" + rowid + "_planmoney").val($("#" + rowid + "_planprice").val() * $("#" + rowid + "_plannum").val());
            //                });
            //            };

            this.table = $("#" + this.ID).jqGrid(Context);
            $("#" + this.ID).jqGrid('navGrid', '#' + this.PagerID,
            {
                edit: false,
                add: false,
                del: false,
                search: false,
                refresh: false//,
                //searchtext: "查找",
                //                searchtext: "查找",
                //searchicon: 'ace-icon fa fa-search orange'
                //                refreshtext: "刷新",
                //refreshicon: 'ace-icon fa fa-refresh green'
            },
            {
            },
            {
            },
            {
            },
            {
                //search form
                recreateForm: true,
                afterShowSearch: function (e) {
                    var form = $(e[0]);
                    form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                    style_search_form(form);
                },
                afterRedraw: function () {
                    style_search_filters($(this));
                }
				,
                multipleSearch: true
                /**
                multipleGroup:true,
                showQuery: true
                */


                //closeOnEscape: true
            },
            {
                recreateForm: true,
                beforeShowForm: function (e) {
                    var form = $(e[0]);
                    form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                }
            }); //.searchGrid({ multipleSearch: true });

            //            $(window).resize(function () {
            //                $(window).unbind("onresize");
            //                $("#" + this.ID).jqGrid('setGridWidth', $(window).width() - 335);
            //                $("#" + this.ID).jqGrid('setGridHeight', $(window).height() - 335);
            //                var sc = $("#" + this.ID).prev()[0].parentElement.parentElement;
            //                if ($("#" + this.ID)[0].clientWidth < sc.clientWidth) {
            //                    $("#" + this.ID)[0].style.width = sc.clientWidth + "px";
            //                    $("table[class='ui-jqgrid-htable']")[0].style.width = sc.clientWidth + "px";
            //                }
            //                if (sc.clientHeight == sc.scrollHeight) {
            //                    sc.style.height = "100%";
            //                    sc.style.width = sc.clientWidth + 1 + "px";
            //                }
            //                $(window).bind("onresize", this);
            //            });
        },

        //生成url
        GetUrl: function () {
            if (this.Params != null) {
                return this.Source + "?" + this.Params;
            }
            return this.Source;
        },

        GetSubUrl: function () {
            if (this.SubParams != null) {
                return this.Source + "?" + this.SubParams;
            }
            return this.Source;
        },

        AddHidColumn: function (/*返回json中的列名*/columnname) {
            if (this.Columns == null) {
                this.Columns = [];
            }
            if (this.ColNames == null) {
                this.ColNames = [];
            }
            var column = {}
            column.name = columnname;
            column.index = columnname;
            //column.hidedlg = false;
            column.hidden = true;
            column.key = true;
            this.ColNames.push("");
            this.Columns.push(column);
        },

        //添加列
        AddColumn: function (/*返回json中的列名*/columnname, /*页面里显示的列名*/title, /*是否允许该列排序*/isSortable,  /*样式名*/cssclass, /*宽度*/width, /*渲染*/special,/*是否允许该列用于搜索*/isShowTitle) {
            if (this.Columns == null) {
                this.Columns = [];
            }
            if (this.ColNames == null) {
                this.ColNames = [];
            }
            var column = {}
            column.name = columnname;
            column.index = columnname;
            column.title = true;
            column.fixed = false;
            column.resizable = false;
            if (title != null) {
                this.ColNames.push(title);
            }
            else {
                this.ColNames.push("");
            }
            //            if (isSearch != null) {
            //                 column.search = isSearch;
            //            }
            if (isSortable != null) {
                column.sortable = isSortable
                column.search = isSortable;
            }
            if (cssclass != null) {
                column.classes = cssclass
            }
            //else {
            //    column.classes = "center";
            //}
            if (width != null) {
                column.width = width
            }
            if (special != null) {
                column.formatter = special;
                column.search = false;
            }
            if (isShowTitle != null) {
                column.title = isShowTitle;
            }
            column.searchoptions = { sopt: ['eq','ieq', 'ne', 'lt', 'le', 'gt', 'ge', 'bw', 'bn', 'ew', 'en'] };
            this.Columns.push(column);
        },

        //添加列
        AddSubColumn: function (/*返回json中的列名*/columnname, /*页面里显示的列名*/title, /*是否允许该列排序*/param,  /*样式名*/cssclass, /*宽度*/width) {
            if (this.SubColumns == null) {
                this.SubColumns = {};
                this.SubColumns.name = [];
                this.SubColumns.width = [];
                this.SubColumns.align = [];
                //this.SubColumns.params = [];
                this.SubColumns.params = ['invdate'];
                this.SubColumns.mapping = [];
            }
            this.SubColumns.name.push(title);
            this.SubColumns.width.push(width);
            this.SubColumns.align.push(cssclass);
            //if (param != null) {
            //    this.SubColumns.params.push(param);
            //}
            this.SubColumns.mapping.push(columnname);
            
        },

        ChangeColumn: function (/*返回json中的列名*/columnindex, /*返回json中的列名*/columnname, /*页面里显示的列名*/title, /*是否允许该列排序*/isSortable, /*是否允许该列用于搜索isSearch,*/ /*样式名*/cssclass, /*宽度*/width, /*渲染*/special) {
            if (title != null && this.ColNames[columnindex] != null) {
                this.ColNames[columnindex] = title;
            }

            if (this.Columns[columnindex] != null) {
                if (columnname != null) {
                    this.Columns[columnindex].name = columnname;
                    this.Columns[columnindex].index = columnname;
                }
                if (isSortable != null) {
                    this.Columns[columnindex].sortable = isSortable
                    this.Columns[columnindex].search = isSortable;
                }
                if (cssclass != null) {
                    this.Columns[columnindex].classes = cssclass
                }
                if (width != null) {
                    this.Columns[columnindex].width = width
                }
                if (special != null) {
                    this.Columns[columnindex].formatter = special
                }
            }
        },

        Search: function () {
            var postData = $("#" + this.ID).jqGrid("getGridParam", "postData");
            $.extend(postData, { Param: JSON.stringify(this.SearchParams) });
            $("#" + this.ID).jqGrid("setGridParam", { search: true }).trigger("reloadGrid", [{ page: 1 }]);
        },

        AddSearchParams: function (/*db中的列名*/field,/*对应符号的参数*/op,/*对应的数据*/data) {
            if (this.SearchParams == null) {
                this.SearchParams = {};
                this.SearchParams.groupOp = "AND";
                this.SearchParams.rules = [];
                this.SearchParams.filter = [];
            }
            var filter = {};
            filter.field = field;
            filter.op = GetOP(op);
            filter.data = data;
            this.SearchParams.rules.push(filter);
        },

        AddLikeSearchForListParams: function (/*db中的列名数组*/fieldarray,/*对应的数据*/data) {
            if (this.SearchParams == null) {
                this.SearchParams = {};
                this.SearchParams.groupOp = "AND";
                this.SearchParams.rules = [];
                this.SearchParams.filter = [];
            }
            var f = {};
            f.groupOp = "OR";
            f.rules = [];
            f.filter = [];
            for (var i = 0; i < fieldarray.length; i++)
            {
                var likefilter = {};
                likefilter.field = fieldarray[i];
                likefilter.op = "cn";
                likefilter.data = data;
                f.rules.push(likefilter);
            }
            this.SearchParams.filter.push(f);
        },

        InitSearchParams: function () {
            this.SearchParams = {};
            this.SearchParams.groupOp = "AND";
            this.SearchParams.rules = [];
            this.SearchParams.filter = [];
        }
    };
}

function GetOP(op) {
    if (op == "=") {
        return "eq";
    }
    else if (op == "INTEQUAL") {
        return "ieq";
    }
    else if (op == "<>") {
        return "ne";
    }
    else if (op == "<") {
        return "lt";
    }
    else if (op == "<=") {
        return "le";
    }
    else if (op == ">") {
        return "gt";
    }
    else if (op == ">=") {
        return "ge";
    }
    else if (op == "LIKE") {
        return "cn";
    }
    else if (op == "NOT LIKE") {
        return "nc";
    }
    else if (op == "IN") {
        return "in";
    }
    else if (op == "NOT IN") {
        return "ni";
    }
    else if (op == "Between") {
        return "bet";
    }
    else if (op == "none") {
        return "none";
    }
    else {
        return "eq";
    }
}

var autotimes = 0;
function autoresize(id) {
    //if (autotimes > 5) {
    //    autotimes = 0;
    //    return;
    //} else {
        //$("#" + id).jqGrid('setGridWidth', width);//$("#gbox_" + id)[0].parentElement.clientWidth);
        //$("#" + id).jqGrid('setGridHeight', $(window).height() - 266);
        //$("#" + id).jqGrid('setGridHeight', '100%');
        var gbox = $("#gbox_" + id);
        var leftWidth = 40;
        if ($(window).width() > 991)
            leftWidth = 230;
        var width = $(window).width() - leftWidth;
        if (gbox.width() < width) {
            $("#" + id).jqGrid('setGridWidth', width);
            gbox.parent().width(width + 20);//$("#gbox_" + id)[0].parentElement.clientWidth);
        } else {
            gbox.parent().width(gbox.width() + 20);
            $(".search-condition").width(gbox.width());
        }

        var sc = $("#" + id).prev()[0].parentElement.parentElement;
        //if ($("#" + id)[0].clientWidth < sc.clientWidth) {
        //    $("#" + id)[0].style.width = sc.clientWidth + "px";
        //    $("table[class='ui-jqgrid-htable']")[0].style.width = sc.clientWidth + "px";
        //}
        //if (sc.clientHeight == sc.scrollHeight) {
        //    var p = sc.parentElement;
        //    for (var i = 0; i < p.children.length; i++) {
        //        if (p.children[i] != sc && p.children[i].clientWidth == sc.clientWidth) {
        //            p.children[i].style.width = sc.clientWidth + 1 + "px";
        //        }
        //    }
        sc.style.height = "100%";
        //sc.style.width = sc.clientWidth + 1 + "px";
        //}
        //autoresizeMain();
        //autotimes++;
        ////if (autotimes == 0) {
        //    var sumwidth = 0;
        //    var cb = $('th[id *= "_cb"]');
        //    if (cb.length > 0) {
        //        var sumwidth = cb.parent().parent().parent().width();
        //        var array = $('.ui-jqgrid-sortable').parent();
        //        for (var i = 0; i < array.length; i++) {
        //            if (array[i].style.display != "none" && array[i].style.width != "" && array[i].childNodes[0].textContent == "") {
        //                cb.width(cb.width() + parseInt(array[i].style.width));
        //                //sumwidth = sumwidth - parseInt(array[i].style.width);
        //                array[i].style.width = "0px";
        //            }
        //        }
        //        //cb.width(sumwidth);
        //    }
        ////}
        ////autotimes++;
    //}
}

function updatePagerIcons(table) {
	var replacement = 
	{
		'ui-icon-seek-first' : 'ace-icon fa fa-angle-double-left bigger-140',
		'ui-icon-seek-prev' : 'ace-icon fa fa-angle-left bigger-140',
		'ui-icon-seek-next' : 'ace-icon fa fa-angle-right bigger-140',
		'ui-icon-seek-end' : 'ace-icon fa fa-angle-double-right bigger-140'
	};
	$('.ui-pg-table:not(.navtable) > tbody > tr > .ui-pg-button > .ui-icon').each(function(){
		var icon = $(this);
		var $class = $.trim(icon.attr('class').replace('ui-icon', ''));
						
		if($class in replacement) icon.attr('class', 'ui-icon '+replacement[$class]);
	})
}
			
function enableTooltips(table) {
	$('.navtable .ui-pg-button').tooltip({container:'body'});
	$(table).find('.ui-pg-div').tooltip({container:'body'});
}

function style_search_filters(form) {
    //form.find('.delete-rule').val('X');
    form.find('.delete-rule').attr("title", "删除");
    form.find('.add-rule').addClass('btn btn-xs btn-primary').attr("title", "添加");
	form.find('.add-group').addClass('btn btn-xs btn-success');
	form.find('.delete-group').addClass('btn btn-xs btn-danger');
}
function style_search_form(form) {
	var dialog = form.closest('.ui-jqdialog');
	var buttons = dialog.find('.EditTable')
	buttons.find('.EditButton a[id*="_reset"]').addClass('btn btn-sm btn-info').find('.ui-icon').attr('class', 'ace-icon fa fa-retweet');
	buttons.find('.EditButton a[id*="_query"]').addClass('btn btn-sm btn-inverse').find('.ui-icon').attr('class', 'ace-icon fa fa-comment-o');
	buttons.find('.EditButton a[id*="_search"]').addClass('btn btn-sm btn-info').find('.ui-icon').attr('class', 'ace-icon fa fa-search');
}

function title_column_header(form)
{
    $('.ui-jqgrid-sortable').each(function () {
        var div = $(this);
        if (div[0].textContent != null && div[0].textContent != "" && typeof (div[0].textContent) != "undefined")
        {
            div.attr("title", div[0].textContent);
        }
    })
}

function search_filter_init() {
    $('#btn_condition').button().on("click", function () {
        var ctrl = $('#contidion_filter')[0];
        if (ctrl.style.display == "block") {
            ctrl.style.display = "none";
        }
        else {
            ctrl.style.display = "block";
            ctrl.style.position = "fixed";
            ctrl.style.top = "190px";
            ctrl.style.right = "44px";
        }
    });
    $('#btn_close').button().on("click", function () {
        $('#contidion_filter')[0].style.display = "none";
    });
    if (typeof (UpdateFilter) != "undefined") {
        $('#btn_submit').button().on("click", function () {
            $(".filter").remove();
            UpdateFilter();
            $('#contidion_filter')[0].style.display = "none";
            return false;
        });
    }
    if (typeof (KeyWordFilter) != "undefined") {
        $('#btn_search').button().on("click", function () {
            KeyWordFilter();
            return false;
        });
    }
    if (typeof (FilterClear) != "undefined") {
        $('#btn_clear').button().on("click", function () {
            FilterClear();
            return false;
        });
    }
}

function AddFilter(colname, op, value, show, jqgrid, id, defaultvalue) {
    var hidden="";
    if (op != ">=" && op != "<=") {
        jqgrid.jqGrid.AddSearchParams(colname, "=", value);
        hidden += '<input type="hidden" value="' + colname + '|' + value + '" class="filter eq"/>';
    } else if(op == "<=") {
        jqgrid.jqGrid.AddSearchParams(colname, "<=", value);
        hidden += '<input type="hidden" value="' + colname + '|' + value + '" class="filter le"/>';
    } else if(op == ">=") {
        jqgrid.jqGrid.AddSearchParams(colname, ">=", value);
        hidden += '<input type="hidden" value="' + colname + '|' + value + '" class="filter be"/>';
    }
    if (show != "") {
        $("#condition").after('<button class="filter filterbefore" onclick="return false;">' + show + '</button>' + hidden + '<button class="filter filterafter" onclick="return RemoveFilter(\'' + id + '\',\'' + defaultvalue + '\',this);">X</button>');
    }
}

function RemoveFilter(controlId,value,obj)
{
    $('#' + controlId).val(value);
    $(".filter").remove();
    UpdateFilter();
    return false;
}


//添加条件标示
function addFilterLable(show, id, value) {
    var ids = "null";
    var values = "null";
    for (var i = 0; i < id.length; i++) {
        ids += ",'" + id[i] + "'";
        values += ",'" + value[i] + "'";
    }
    $("#condition").after('<button class="filter filterbefore" onclick="return false;">' + show + '</button><button class="filter filterafter" onclick="return removeFilterLable([' + ids + '],[' + values + ']);">X</button>');
}

//删除条件标示
function removeFilterLable(id, value) {
    for (var i = 1; i < id.length; i++) {
        $("#" + id[i]).val(value[i]);
    }
    if (typeof (Search) != "undefined")
        Search();
    return false;
}

//关键字查询按钮
function onKeyWordClick() {
    //if ($('#searchtext').val() != "") {
    //    var msg = $('#searchtext').val();
    //    $('#keyWord')[0].style.display = '';
    //    $('#keyWord')[0].innerHTML = '搜索"' + msg + '" /';
    //    $('#keyWord')[0].title = msg;
    //}
    //else {
    //    $('#keyWord')[0].style.display = 'none';
    //}
    if (typeof (Search) != "undefined")
        Search();
    return false;
}


//关闭查询弹窗按钮
function onClose() {
    $('#contidion_filter')[0].style.display = "none";
    return false;
}
//查询按钮
function onSearch() {
    $('#contidion_filter')[0].style.display = "none";
    if (typeof (Search) != "undefined")
        Search();
    return false;
}
//还原原本列宽
function Widthdeal(id) {
    var obj = document.getElementById(id);
    obj.style = "width:" + obj.style.width + " !important";
}
