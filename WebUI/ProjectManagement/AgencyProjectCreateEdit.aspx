<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="AgencyProjectCreateEdit.aspx.cs" Inherits="WebUI.ProjectManagement.AgencyProjectCreateEdit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/ProjectManagement/AgencyProjectCreateEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
    <script type="text/javascript" src="../Scripts/StartReview.js"></script>
    <link rel="stylesheet" href="../Style/css/StarReview.css" />
    <script type="text/javascript">
        var _CustomerName = "<%= CustomerName%>";
    </script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="tabbable">
                    <ul class="nav nav-tabs padding-12 tab-color-blue background-blue" id="myTab4">
                        <li class="active">
                            <a data-toggle="tab" href="#home4">项目进度器</a>
                        </li>

                        <li class="">
                            <a data-toggle="tab" href="#profile3">项目附件</a>
                        </li>

                        <li class="">
                            <a data-toggle="tab" href="#profile4">任务</a>
                        </li>
                        <li class="">
                            <a data-toggle="tab" href="#dropdown14">销售</a>
                        </li>
                        <li class="">
                            <a data-toggle="tab" href="#report">项目报告器</a>
                        </li>

                    </ul>

                    <div class="tab-content">
                        <div id="home4" class="tab-pane active">
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">项目编号 </label>

                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtProjectCode" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right need">开始时间 </label>
                                <div class="col-sm-3">
                                    <div class="input-group col-sm-9">
                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <i class="fa fa-calendar bigger-110"></i>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right need">结束时间 </label>
                                <div class="col-sm-3">
                                    <div class="input-group col-sm-9">
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <i class="fa fa-calendar bigger-110"></i>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right need">合同金额 </label>

                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtCost" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display:none">
                                <label class="col-sm-2 control-label no-padding-right need">酬金类型 </label>

                                <div class="col-sm-9">
                                    <label>
                                        <input name="CommissionType" class="ace" type="radio" id="radDecimal" checked="checked" />
                                        <span class="lbl">金额</span>
                                    </label>
                                    <label>
                                        <input name="CommissionType" class="ace" type="radio" id="radPer" />
                                        <span class="lbl">合同比例</span>
                                    </label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right need">总酬金 </label>

                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtCommission" runat="server" CssClass="control-label" Enabled="false"></asp:TextBox>
                                    <input type="hidden" id="ProjectChangeId" value="0"/>
                                    <input type="hidden" id="ProjectRefundId" value="0"/>
                                    <div class="newChange">
                                        雇主申请将总酬金更改为：<asp:Label CssClass="control-label" Style="color:#ff0000" ID="newChange" runat="server" Text="0"></asp:Label>
                                        <button class="wtbtn yjright" type="button" id="btn_newChange_Decide" title="同意更改">
                                           <i class="icon-ok bigger-110"></i>
                                           同意更改
                                        </button>
                                        <button class="wtbtn yjright" type="button" id="btn_newChange_cancel" title="拒绝更改">
                                           <i class="icon-ok bigger-110"></i>
                                           拒绝更改
                                        </button>

                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                               <label class="col-sm-2 control-label no-padding-right">剩余未发放酬金</label>

                               <div class="col-sm-9">
                                   <asp:Label CssClass="control-label" Style="line-height: 30px;" ID="lblRemainCommission" runat="server" Text="0"></asp:Label>
                               </div>
                            </div>
                            <div id="divStep" data-target="#step-container">
                                <ul class="wizard-steps">
                                    <li data-target="#step0" class="complete">
                                        <span class="step">1</span>
                                        <span class="title">选定销售</span>
                                    </li>
                                    <li data-target="#step1" class="complete">
                                        <span class="step">2</span>
                                        <span class="title">在线签订雇佣合同</span>
                                    </li>
                                    <li data-target="#step2" class="complete">
                                        <span class="step">3</span>
                                        <span class="title">生成项目</span>
                                    </li>

                                    <li data-target="#step3" class="complete">
                                        <span class="step">4</span>
                                        <span class="title">托管阶段酬金</span>
                                    </li>

                                    <li data-target="#step4" class="active">
                                        <span class="step">5</span>
                                        <span class="title">工作中</span>
                                    </li>
                                    <li data-target="#step5" class="active">
                                        <span class="step">6</span>
                                        <span class="title">阶段酬金支付</span>
                                    </li>

                                    <li data-target="#step6">
                                        <span class="step">7</span>
                                        <span class="title">完工</span>
                                    </li>
                                    <li data-target="#step7">
                                        <span class="step">8</span>
                                        <span class="title">相互评价</span>
                                    </li>
                                </ul>
                            </div>
                            <div class="form-group" id="div_NewComplaints" style="display: none;">
                                <label class="col-sm-2 control-label no-padding-right red">无法达成共识可申请平台介入</label>
                                <div class="col-sm-9">
                                    <button class="wtbtn yjright" type="button" id="btn_NewComlaints" onclick="return NewComplaints();" title="申请维权">
                                        <i class="icon-ok bigger-110"></i>
                                        申请维权
                                    </button>
                                </div>
                            </div>
                            <div id="divComplaints" style="margin-left: 10px; margin-right: 10px; display: none;">
                                <div class="form-group">
                                    <h4 class="header">维权进度</h4>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-9">
                                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                                            <table id="ProjectComlaintsList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                                <thead>
                                                    <tr class="ui-jqgrid-labels">
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 100px;">
                                                            <div class="ui-jqgrid-sortable" title="申请人">申请人</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="申请日期">申请日期</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="申请理由">申请理由</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="状态">状态</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="原因">原因</div>
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <%--<tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                    </tr>--%>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divCommissionDelegate" style="display: none;">
                                <%--酬金委托，雇主进行酬金委托。销售显示等待雇主委托酬金。可以修改项目资料--%>
                            </div>
                            <div id="divWorking" style="margin-left: 10px; margin-right: 10px; display: none;">
                                <%--工作中，不能修改项目资料
                                销售添加工作进度，雇主查看工作进度；
                                销售申请分期付款，雇主确认分期付款；
                                销售完成工作，并申请尾款支付，雇主确认完成，并支付尾款。--%>
                                <div class="form-group">
                                    <h4 class="header">工作进度</h4>
                                </div>
                                <div class="form-group">
                                    <%--<label class="col-sm-2 control-label no-padding-right">工作进度 </label>--%>

                                    <div class="col-sm-9">
                                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                                            <button class="wtbtn yjright" type="button" id="btn_NewAction" title="添加">
                                                <i class="icon-ok bigger-110"></i>
                                                添加
                                            </button>
                                            <button class="wtbtn yjright" type="button" id="btn_DeleteAction" onclick="return DeleteProjectAction();" title="删除">
                                                <i class="icon-ok bigger-110"></i>
                                                删除
                                            </button>
                                            <div class="space-4"></div>
                                            <table id="ProjectActionList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                                <thead>
                                                    <tr class="ui-jqgrid-labels">
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                                            <div class="" title="">
                                                            </div>
                                                        </th>
                                                        <%--<th class="ui-state-default ui-th-column ui-th-ltr" style="width: 100px;">
                                                            <div class="ui-jqgrid-sortable" title="跟进人">跟进人</div>
                                                        </th>--%>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 100px;">
                                                            <div class="ui-jqgrid-sortable" title="跟进日期">跟进日期</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="跟进内容">跟进内容</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="附件">附件</div>
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <%--<tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                    </tr>--%>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="space-4"></div>
                                <div class="form-group">
                                    <div class="col-sm-9">
                                        <button class="wtbtn yjright" type="button" id="btn_NewProjectCommission" onclick="return onAddProjectCommission();" title="付款申请">
                                            <i class="icon-ok bigger-110"></i>
                                            付款申请
                                        </button>
                                    </div>
                                </div>
                                <div id="divProjectCommissionInfo" style="display: none;">
                                    <div class="form-group">
                                        <h4 class="header">付款申请详情</h4>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-xs-2 control-label no-padding-right">申请金额</label>
                                        <div class="col-sm-9">
                                            <label class="col-xs-1 control-label no-padding-right" id="lblProjectCommission"></label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-xs-2 control-label no-padding-right">申请理由</label>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-xs-2 control-label no-padding-right"></label>
                                        <div class="col-sm-9">
                                            <p id="txtReason"></p>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <h4 class="header">付款历史</h4>
                                </div>
                                <div class="form-group">
                                    <%-- <label class="col-sm-2 control-label no-padding-right">付款历史 </label>--%>

                                    <div class="col-sm-9">
                                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">

                                            <table id="ProjectCommissionList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                                <thead>
                                                    <tr class="ui-jqgrid-labels">
                                                        <%-- <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                                            <div class="" title="">
                                                            </div>
                                                        </th>--%>
                                                        <%--<th class="ui-state-default ui-th-column ui-th-ltr" style="width: 100px;">
                                                            <div class="ui-jqgrid-sortable" title="申请人">申请人</div>
                                                        </th>--%>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="申请日期">申请日期</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="金额">金额</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="申请理由">申请理由</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="状态">状态</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="拒绝原因">拒绝原因</div>
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <%--<tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                    </tr>--%>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divWorkEnd" style="display: none; margin-left: 10px; margin-right: 10px;">
                                <%--评价阶段。双方评价对方。双方申请维权--%>
                                <div id="divReviewBusiness">
                                    <div class="form-group">
                                        <h4 class="header">对雇主评价</h4>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label no-padding-right">售前售中支持</label>
                                        <div class="col-sm-9">
                                            <div class="xzw_starSys">
                                                <div class="xzw_starBox">
                                                    <ul class="star stars">
                                                        <li><a href="javascript:void(0)" title="1" class="one-star">1</a></li>
                                                        <li><a href="javascript:void(0)" title="2" class="two-stars">2</a></li>
                                                        <li><a href="javascript:void(0)" title="3" class="three-stars">3</a></li>
                                                        <li><a href="javascript:void(0)" title="4" class="four-stars">4</a></li>
                                                        <li><a href="javascript:void(0)" title="5" class="five-stars">5</a></li>
                                                    </ul>
                                                    <div class="current-rating showb" style="width: 30px;"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label no-padding-right">售后服务</label>
                                        <div class="col-sm-9">
                                            <div class="xzw_starSys">
                                                <div class="xzw_starBox">
                                                    <ul class="star stars">
                                                        <li><a href="javascript:void(0)" title="1" class="one-star">1</a></li>
                                                        <li><a href="javascript:void(0)" title="2" class="two-stars">2</a></li>
                                                        <li><a href="javascript:void(0)" title="3" class="three-stars">3</a></li>
                                                        <li><a href="javascript:void(0)" title="4" class="four-stars">4</a></li>
                                                        <li><a href="javascript:void(0)" title="5" class="five-stars">5</a></li>
                                                    </ul>
                                                    <div class="current-rating showb" style="width: 30px;"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label no-padding-right">产品质量</label>
                                        <div class="col-sm-9">
                                            <div class="xzw_starSys">
                                                <div class="xzw_starBox">
                                                    <ul class="star stars">
                                                        <li><a href="javascript:void(0)" title="1" class="one-star">1</a></li>
                                                        <li><a href="javascript:void(0)" title="2" class="two-stars">2</a></li>
                                                        <li><a href="javascript:void(0)" title="3" class="three-stars">3</a></li>
                                                        <li><a href="javascript:void(0)" title="4" class="four-stars">4</a></li>
                                                        <li><a href="javascript:void(0)" title="5" class="five-stars">5</a></li>
                                                    </ul>
                                                    <div class="current-rating showb" style="width: 30px;"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label no-padding-right">付款及时性</label>
                                        <div class="col-sm-9">
                                            <div class="xzw_starSys">
                                                <div class="xzw_starBox">
                                                    <ul class="star stars">
                                                        <li><a href="javascript:void(0)" title="1" class="one-star">1</a></li>
                                                        <li><a href="javascript:void(0)" title="2" class="two-stars">2</a></li>
                                                        <li><a href="javascript:void(0)" title="3" class="three-stars">3</a></li>
                                                        <li><a href="javascript:void(0)" title="4" class="four-stars">4</a></li>
                                                        <li><a href="javascript:void(0)" title="5" class="five-stars">5</a></li>
                                                    </ul>
                                                    <div class="current-rating showb" style="width: 30px;"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label no-padding-right">诚信度</label>
                                        <div class="col-sm-9">
                                            <div class="xzw_starSys">
                                                <div class="xzw_starBox">
                                                    <ul class="star stars">
                                                        <li><a href="javascript:void(0)" title="1" class="one-star">1</a></li>
                                                        <li><a href="javascript:void(0)" title="2" class="two-stars">2</a></li>
                                                        <li><a href="javascript:void(0)" title="3" class="three-stars">3</a></li>
                                                        <li><a href="javascript:void(0)" title="4" class="four-stars">4</a></li>
                                                        <li><a href="javascript:void(0)" title="5" class="five-stars">5</a></li>
                                                    </ul>
                                                    <div class="current-rating showb" style="width: 30px;"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label no-padding-right">评价内容</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtReviewDescription" runat="server" CssClass="description-textarea" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group" id="divAddNote" style="display:none;">
                                        <label class="col-sm-2 control-label no-padding-right">追评内容</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtReviewAddNote" runat="server" CssClass="description-textarea" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group" id="divExplanation" style="display:none;">
                                        <label class="col-sm-2 control-label no-padding-right">雇主回复</label>
                                        <div class="col-sm-9">
                                            <div id="divBusinessExplanation" style="padding-top: 6px; margin-left: 10px;"></div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label no-padding-right"></label>
                                        <div class="col-sm-9">
                                            <button class="wtbtn yjright" type="button" id="btn_NewReview" onclick="return NewReview();" title="提交评价">
                                                <i class="icon-ok bigger-110"></i>
                                                提交评价
                                            </button>
                                            <button class="wtbtn yjright" type="button" id="btn_AddNoteReview" onclick="return AddNoteReview();" title="追加评价">
                                                <i class="icon-ok bigger-110"></i>
                                                追加评价
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <div id="divReviewAgency" style="display: none;">
                                    <div class="form-group">
                                        <h4 class="header">雇主对您的评价</h4>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label no-padding-right">工作态度</label>
                                        <div class="col-sm-9">
                                            <div class="xzw_starSys">
                                                <div class="xzw_starBox">
                                                    <ul class="star stars">
                                                        <li><a href="javascript:void(0)" title="1" class="one-star">1</a></li>
                                                        <li><a href="javascript:void(0)" title="2" class="two-stars">2</a></li>
                                                        <li><a href="javascript:void(0)" title="3" class="three-stars">3</a></li>
                                                        <li><a href="javascript:void(0)" title="4" class="four-stars">4</a></li>
                                                        <li><a href="javascript:void(0)" title="5" class="five-stars">5</a></li>
                                                    </ul>
                                                    <div class="current-rating showb" style="width: 30px;"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label no-padding-right">沟通技巧</label>
                                        <div class="col-sm-9">
                                            <div class="xzw_starSys">
                                                <div class="xzw_starBox">
                                                    <ul class="star stars">
                                                        <li><a href="javascript:void(0)" title="1" class="one-star">1</a></li>
                                                        <li><a href="javascript:void(0)" title="2" class="two-stars">2</a></li>
                                                        <li><a href="javascript:void(0)" title="3" class="three-stars">3</a></li>
                                                        <li><a href="javascript:void(0)" title="4" class="four-stars">4</a></li>
                                                        <li><a href="javascript:void(0)" title="5" class="five-stars">5</a></li>
                                                    </ul>
                                                    <div class="current-rating showb" style="width: 30px;"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label no-padding-right">打单能力</label>
                                        <div class="col-sm-9">
                                            <div class="xzw_starSys">
                                                <div class="xzw_starBox">
                                                    <ul class="star stars">
                                                        <li><a href="javascript:void(0)" title="1" class="one-star">1</a></li>
                                                        <li><a href="javascript:void(0)" title="2" class="two-stars">2</a></li>
                                                        <li><a href="javascript:void(0)" title="3" class="three-stars">3</a></li>
                                                        <li><a href="javascript:void(0)" title="4" class="four-stars">4</a></li>
                                                        <li><a href="javascript:void(0)" title="5" class="five-stars">5</a></li>
                                                    </ul>
                                                    <div class="current-rating showb" style="width: 30px;"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label no-padding-right">诚信度</label>
                                        <div class="col-sm-9">
                                            <div class="xzw_starSys">
                                                <div class="xzw_starBox">
                                                    <ul class="star stars">
                                                        <li><a href="javascript:void(0)" title="1" class="one-star">1</a></li>
                                                        <li><a href="javascript:void(0)" title="2" class="two-stars">2</a></li>
                                                        <li><a href="javascript:void(0)" title="3" class="three-stars">3</a></li>
                                                        <li><a href="javascript:void(0)" title="4" class="four-stars">4</a></li>
                                                        <li><a href="javascript:void(0)" title="5" class="five-stars">5</a></li>
                                                    </ul>
                                                    <div class="current-rating showb" style="width: 30px;"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label no-padding-right">客户关系</label>
                                        <div class="col-sm-9">
                                            <div class="xzw_starSys">
                                                <div class="xzw_starBox">
                                                    <ul class="star stars">
                                                        <li><a href="javascript:void(0)" title="1" class="one-star">1</a></li>
                                                        <li><a href="javascript:void(0)" title="2" class="two-stars">2</a></li>
                                                        <li><a href="javascript:void(0)" title="3" class="three-stars">3</a></li>
                                                        <li><a href="javascript:void(0)" title="4" class="four-stars">4</a></li>
                                                        <li><a href="javascript:void(0)" title="5" class="five-stars">5</a></li>
                                                    </ul>
                                                    <div class="current-rating showb" style="width: 30px;"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label no-padding-right">评价内容</label>
                                        <div class="col-sm-9">
                                            <div id="divBusinessReviewDescription" style="padding-top: 6px; margin-left: 10px;"></div>
                                        </div>
                                    </div>
                                    <div class="form-group" id="divBusinessAddNote">
                                        <label class="col-sm-2 control-label no-padding-right">追评内容</label>
                                        <div class="col-sm-9">
                                            <div id="divBusinessReviewAddNote" style="padding-top: 6px; margin-left: 10px;"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix form-actions">
                                <div class="col-sm-5" style="text-align: center;">
                                    <button class="wtbtn savebtn" type="button" id="btn_save" title="保存" style="display: none;">
                                        保存
                                    </button>
                                    <button class="wtbtn savebtn" type="button" id="btn_commissiondelegate" title="委托酬金" style="display: none;">
                                        委托酬金
                                    </button>
                                    <button class="wtbtn savebtn" type="button" id="btn_completeproject" title="项目完工" style="display: none;">
                                        项目完工
                                    </button>
                                    <button class="wtbtn savebtn" type="button" id="btn_comfirmproject" title="确认完工" style="display: none;">
                                        确认完工
                                    </button>
                                    <%--<button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                                        返回
                                    </button>--%>
                                </div>
                            </div>
                        </div>
                        <div id="profile3" class="tab-pane">
                            <div class="form-group">
                                <%--<label class="col-sm-2 control-label no-padding-right">附件列表 </label>--%>

                                <div class="col-sm-9">
                                    <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                                        <%--<button class="wtbtn yjright" type="button" id="btn_newattach" onclick="return NewAttach();" title="添加">
                                <i class="icon-ok bigger-110"></i>
                                添加
                            </button>--%>
                                        <input id="fileAdd" name="id-input-file" type="file" onchange="changefile('fileAdd')" />
                                        <button class="wtbtn yjright" type="button" id="btn_deleteattach" onclick="return DeleteFile();" title="删除">
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
                                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                        <div class="ui-jqgrid-sortable" title="上传日期">上传日期</div>
                                                    </th>
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
                        </div>
                        <div id="profile4" class="tab-pane">
                            <div class="form-group" style="display:none">
                                <label class="col-sm-2 control-label no-padding-right">行业类型 </label>

                                <div class="col-sm-9">
                                    <asp:DropDownList ID="drpCategory1" runat="server" CssClass="col-xs-10 col-sm-2" Enabled="false">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="drpCategory2" runat="server" CssClass="col-xs-10 col-sm-2" Enabled="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group" style="display:none">
                                <label class="col-sm-2 control-label no-padding-right">区域 </label>

                                <div class="col-sm-9">
                                    <asp:DropDownList ID="drpProvince" runat="server" CssClass="col-xs-10 col-sm-2" Enabled="false">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="drpCity" runat="server" CssClass="col-xs-10 col-sm-2" Enabled="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">任务编号 </label>

                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtRequirementCode" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">标题 </label>

                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtTitle" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">手机号码 </label>

                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="20" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">描述 </label>

                                <div class="col-sm-9" id="divDescription">
                                </div>
                            </div>
                        </div>

                        <div id="dropdown14" class="tab-pane">
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">销售姓名 </label>

                                <div class="col-sm-9">
                                    <label id="lblAgencyName" class="col-sm-2 control-label no-padding-right" style="text-align: left;"></label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">年龄 </label>

                                <div class="col-sm-9">
                                    <label id="lblAge" class="col-sm-2 control-label no-padding-right" style="text-align: left;"></label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">销售级别 </label>

                                <div class="col-sm-9">
                                    <asp:DropDownList ID="drpAgencyLevel" runat="server" CssClass="col-xs-10 col-sm-2" Enabled="false">
                                        <asp:ListItem Text="金牌销售" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="银牌销售" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="铜牌销售" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">手机号码 </label>

                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtAgencyPhone" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="20" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="report" class="tab-pane">
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">类型 </label>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddlreportType" runat="server" CssClass="col-xs-10 col-sm-2">
                                        <asp:ListItem Text="周报" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="月报" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">描述 </label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtReportDesc" runat="server" CssClass="description-textarea" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right"></label>
                                <div class="col-sm-9">
                                    <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                                        <input id="reportfileAdd" name="id-input-file" type="file" onchange="changereportfile('reportfileAdd')" />
                                        <button class="wtbtn yjright" type="button" id="btn_deleteReport" onclick="return DeleteReport();" title="删除">
                                            <i class="icon-ok bigger-110"></i>
                                            删除
                                        </button>
                                        <div class="space-4"></div>
                                        <table id="reportList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                            <thead>
                                                <tr class="ui-jqgrid-labels">
                                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                                        <div class="" title="">
                                                            <%--<input class="cbox" type="checkbox" onchange="checkAll(this, 'UserProjectList')" />--%>
                                                        </div>
                                                    </th>
                                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 100px;">
                                                        <div class="ui-jqgrid-sortable" title="报表名称">报表名称</div>
                                                    </th>
                                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 100px;">
                                                        <div class="ui-jqgrid-sortable" title="类型">类型</div>
                                                    </th>
                                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 100px;">
                                                        <div class="ui-jqgrid-sortable" title="描述">描述</div>
                                                    </th>
                                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 100px;">
                                                        <div class="ui-jqgrid-sortable" title="上传日期">上传日期</div>
                                                    </th>
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
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidProjectId" runat="server" />
    <input type="hidden" id="hidProjectStatus" />
</asp:Content>
