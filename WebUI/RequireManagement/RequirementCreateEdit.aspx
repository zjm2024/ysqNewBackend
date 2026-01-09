<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="RequirementCreateEdit.aspx.cs" Inherits="WebUI.RequireManagement.RequirementCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/RequireManagement/RequirementCreateEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
    <!-- 配置文件 -->
    <script type="text/javascript" src="../Scripts/UEditor/ueditor.config.js"></script>
    <!-- 编辑器源码文件 -->
    <script type="text/javascript" src="../Scripts/UEditor/ueditor.all.js"></script>
    <script type="text/javascript" charset="utf-8" src="../Scripts/UEditor/lang/zh-cn/zh-cn.js"></script>
    <!-- 实例化编辑器 -->
    <script type="text/javascript">
        var ue = UE.getEditor('container');
    </script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="tabbable">
                    <ul class="nav nav-tabs padding-12 tab-color-blue background-blue" id="myTab4">
                        <li class="<%if (dropdown14) { Response.Write("active"); } %>">
                            <a data-toggle="tab" href="#home4">任务信息</a>
                        </li>
                        <li class="<%if (!dropdown14) { Response.Write("active"); } %>">
                            <a data-toggle="tab" href="#dropdown14">收到简历</a>
                        </li>
                    </ul>
                    <div class="tab-content">
                        <div id="home4" class="tab-pane <%if (dropdown14) { Response.Write("active"); } %>">
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right need">标题 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtTitle" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="50"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">封面 </label>

                                <div class="col-sm-9">
                                    <input id="imgMain" name="id-input-file" type="file" onchange="change('imgMain')" />
                                    <div class="form-group" id="dvimgMain">
                                        <div class="col-sm-9">
                                            <img alt="" id="imgMainPic" src="" style="width: 150px; height: 150px;" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">公司所属行业 </label>

                                <div class="col-sm-9">
                                    <asp:dropdownlist id="drpCategory1" runat="server" cssclass="col-xs-10 col-sm-2">
                                    </asp:dropdownlist>
                                    <asp:dropdownlist id="drpCategory2" runat="server" cssclass="col-xs-10 col-sm-2">
                                    </asp:dropdownlist>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">办公所在区域 </label>

                                <div class="col-sm-9">
                                    <asp:dropdownlist id="drpProvince" runat="server" cssclass="col-xs-10 col-sm-2">
                                    </asp:dropdownlist>
                                    <asp:dropdownlist id="drpCity" runat="server" cssclass="col-xs-10 col-sm-2">
                                    </asp:dropdownlist>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right need">目标客户行业 </label>

                                <div class="col-sm-9">
                                    <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                                        <button class="wtbtn yjright" type="button" id="btn_newtargetcategory" onclick="return NewTargetCategory();" title="添加">
                                            <i class="icon-ok bigger-110"></i>
                                            添加
                                        </button>
                                        <button class="wtbtn yjright" type="button" id="btn_deletetargetcategory" onclick="return DeleteTargetCategory();" title="删除">
                                            <i class="icon-ok bigger-110"></i>
                                            删除
                                        </button>
                                        <div class="space-4"></div>
                                        <div id="divTargetCategoryList" runat="server">
                                            <table id="TargetCategoryList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                                <thead>
                                                    <tr class="ui-jqgrid-labels">
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                                            <div class="" title="">
                                                                <input class="cbox" type="checkbox" onchange="checkAll(this, 'TargetCategoryList')" />
                                                            </div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="行业大类">行业大类</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="行业小类">行业小类</div>
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                                        <td style="" title=""></td>
                                                        <td style="" title=""></td>
                                                        <td style="" title=""></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right need">目标客户区域 </label>

                                <div class="col-sm-9">
                                    <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                                        <button class="wtbtn yjright" type="button" id="btn_newtargetcity" onclick="return NewTargetCity();" title="添加">
                                            <i class="icon-ok bigger-110"></i>
                                            添加
                                        </button>
                                        <button class="wtbtn yjright" type="button" id="btn_deletetargetcity" onclick="return DeleteTargetCity();" title="删除">
                                            <i class="icon-ok bigger-110"></i>
                                            删除
                                        </button>
                                        <div class="space-4"></div>
                                        <div id="divTargetCityList" runat="server">
                                            <table id="TargetCityList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                                <thead>
                                                    <tr class="ui-jqgrid-labels">
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                                            <div class="" title="">
                                                                <input class="cbox" type="checkbox" onchange="checkAll(this, 'TargetCityList')" />
                                                            </div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="省（直辖市）">省（直辖市）</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="城市">城市</div>
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                                        <td style="" title=""></td>
                                                        <td style="" title=""></td>
                                                        <td style="" title=""></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">目标客户 </label>

                                <div class="col-sm-9">
                                    <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                                        <button class="wtbtn yjright" type="button" id="btn_newrequireclient" onclick="return NewRequireClient();" title="添加">
                                            <i class="icon-ok bigger-110"></i>
                                            添加
                                        </button>
                                        <button class="wtbtn yjright" type="button" id="btn_deleterequireclient" onclick="return DeleteRequireClient();" title="删除">
                                            <i class="icon-ok bigger-110"></i>
                                            删除
                                        </button>
                                        <div class="space-4"></div>
                                        <div id="divRequireClientList" runat="server">
                                            <table id="RequireClientList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                                <thead>
                                                    <tr class="ui-jqgrid-labels">
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                                            <div class="" title="">
                                                                <input class="cbox" type="checkbox" onchange="checkAll(this, 'RequireClientList')" />
                                                            </div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="客户名称">客户名称</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="用户名称">用户名称</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="任务现状">任务现状</div>
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                                        <td style="" title=""></td>
                                                        <td style="" title=""></td>
                                                        <td style="" title=""></td>
                                                        <td style="" title=""></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" id="divRequireCode" style="display: none;">
                                <label class="col-sm-2 control-label no-padding-right">任务编号 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtRequirementCode" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="50" enabled="false"></asp:textbox>
                                </div>
                            </div>
                            
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right need">有效日期 </label>

                                <div class="col-sm-9">
                                    <div class="input-group col-sm-2">
                                        <asp:textbox id="txtEffectiveEndDate" runat="server" cssclass="form-control date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:textbox>
                                        <span class="input-group-addon">
                                            <i class="fa fa-calendar bigger-110"></i>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right need">预估合同额 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtCost" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right need">酬金类型 </label>

                                <div class="col-sm-9">
                                    <label>
                                        <input name="CommissionType" class="ace" type="radio" id="radDecimal" runat="server" />
                                        <span class="lbl">金额</span>
                                    </label>
                                    <label>
                                        <input name="CommissionType" class="ace" type="radio" id="radPer" runat="server" />
                                        <span class="lbl">合同比例</span>
                                    </label>
                                </div>
                            </div>
                            <div class="form-group" id="divpercentage" style="display: none">
                                <label class="col-sm-2 control-label no-padding-right need">比例（百分比) </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtpercentage" runat="server" cssclass="col-xs-2 col-sm-2"></asp:textbox>
                                    <label class="control-label" style="padding-top: 2px;">%</label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">酬金 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtCommission" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group" style="display:none">
                                <label class="col-sm-2 control-label no-padding-right">销售数量</label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtAgencySum" runat="server" cssclass="col-xs-10 col-sm-5">1</asp:textbox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">已托管酬金 </label>

                                <div class="col-sm-9">
                                    <asp:label cssclass="control-label" style="line-height: 30px;" id="lblRequireCommission" runat="server" text="0"></asp:label>
                                    <button class="wtbtn wtbtn-create" type="button" id="btn_plusrequirecommission" runat="server" style="display: none;" title="追加酬金">
                                        追加酬金
                                    </button>
                                    <button class="wtbtn wtbtn-create" type="button" id="btn_cancelrequirecommission" runat="server" style="display: none;" title="取消托管">
                                        取消托管
                                    </button>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right need">酬金说明 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtCommissionDescription" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="50"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">联系人 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtContactPerson" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right need">手机号码 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtPhone" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">更多联系方式 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtContactPhone" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                                </div>
                            </div>
                            
                            <%--<div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">产品介绍 </label>

                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtProductDescription" runat="server" TextMode="MultiLine" CssClass="description-textarea" MaxLength="400"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">公司介绍 </label>

                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtCompanyDescription" runat="server" TextMode="MultiLine" CssClass="description-textarea" MaxLength="400"></asp:TextBox>
                                </div>
                            </div>--%>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">理想销售人员 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtTargetAgency" runat="server" textmode="MultiLine" CssClass="description-textarea" maxlength="400"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group" style="display:none">
                                <label class="col-sm-2 control-label no-padding-right">对销售枪手详细要求 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtAgencyCondition" runat="server" textmode="MultiLine" CssClass="description-textarea" maxlength="400"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">任务详情 </label>

                                <div class="col-sm-9">
                                    <script id="container" name="content" type="text/plain">
       
                                    </script>
                                    <asp:hiddenfield id="hidDescription" runat="server" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">附件列表 </label>

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
                                        <div id="divFileList" runat="server">
                                            <table id="FileList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                                <thead>
                                                    <tr class="ui-jqgrid-labels">
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                                            <div class="" title="">
                                                                <input class="cbox" type="checkbox" onchange="checkAll(this, 'FileList')" />
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
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="form-group" id="divStatus" style="display: none;">
                                <label class="col-sm-2 control-label no-padding-right need">状态 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtStatus" runat="server" cssclass="col-xs-10 col-sm-5" enabled="false"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group" id="divCreatedAt" style="display: none;">
                                <label class="col-sm-2 control-label no-padding-right need">创建日期 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtCreatedAt" runat="server" cssclass="col-xs-10 col-sm-5" enabled="false"></asp:textbox>
                                </div>
                            </div>
                            <div class="clearfix form-actions">
                                <div class="col-sm-5" style="text-align: center;">
                                    <button class="wtbtn savebtn" type="button" id="btn_save" title="保存" runat="server">
                                        保存
                                    </button>
                                    <button class="wtbtn savebtn" type="button" id="btn_submit" title="提交审核" runat="server">
                                        提交审核
                                    </button>
                                    <button class="wtbtn savebtn" type="button" id="btn_updaterequirestatus" title="取消发布" runat="server">
                                        取消发布
                                    </button>
                                    <button class="wtbtn savebtn" type="button" id="btn_start" title="启动投简历" runat="server">
                                        启动投简历
                                    </button>
                                    <button class="wtbtn savebtn" type="button" id="btn_stop" title="暂停投简历" runat="server">
                                        暂停投简历
                                    </button>
                                    <button class="wtbtn savebtn" type="button" id="btn_refresh" title="刷新置顶" runat="server">
                                        刷新置顶
                                    </button>
                                </div>
                            </div>
                            <%--</div>
                        <div id="profile4" class="tab-pane">--%>
                        </div>
                        <%if (isRealNameStatus)
                            {%>
                        <div id="dropdown14" class="tab-pane <%if (!dropdown14) { Response.Write("active"); } %>"">
                            <div class="form-group">
                                <%--<label class="col-sm-2 control-label no-padding-right">投标列表 </label>--%>

                                <div class="col-sm-9">
                                    <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;" id="divTenderInviteList" runat="server">
                                        <table id="TenderInviteList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                            <thead>
                                                <tr class="ui-jqgrid-labels">
                                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;"></th>
                                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                        <div class="ui-jqgrid-sortable" title="行业大类">投递日期</div>
                                                    </th>
                                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                        <div class="ui-jqgrid-sortable" title="行业小类">销售名称</div>
                                                    </th>
                                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                        <div class="ui-jqgrid-sortable" title="行业小类">线上联系</div>
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                                    <td style="" title=""></td>
                                                    <td style="" title=""></td>
                                                    <td style="" title=""></td>
                                                    <td style="" title=""></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%}else{%>
                        <div id="dropdown14" class="tab-pane <%if (!dropdown14) { Response.Write("active"); } %>"">
                            完成实名认证才能查看投递的简历 <a href="../CustomerManagement/BusinessCreateEdit.aspx?page=RealName" class="link">前往认证</a>
                        </div>
                        <%} %>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:hiddenfield id="hidStatus" runat="server" />
    <asp:hiddenfield id="hidRequirementId" runat="server" />
</asp:Content>
