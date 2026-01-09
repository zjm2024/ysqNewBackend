<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="RequirementCreateEdit_New.aspx.cs" Inherits="WebUI.RequireManagement.RequirementCreateEdit_New" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/RequireManagement/RequirementCreateEdit_NewJS.js"></script>
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
    <style>.page-content{ background:none}</style>
    <div class="RequireMiddle">
        <div class="NRequire_title">
            <span>发布任务&nbsp;<font id="NRequire_page">1/5</font></span>
            <a href="RequirementCreateEdit.aspx" target="_blank"><i class="fa fa-mail-reply"></i>切换到经典的发布页面</a>
        </div>
        <div class="NRequire_content">
            <div class="NRequire_content_box" id="box1">
                <div class="frm_control_group">
                    <label class="frm_label need">任务标题</label>
                    <div class="frm_controls">
                        <asp:textbox id="txtTitle" runat="server" maxlength="50"></asp:textbox>
                        <p class="frm_tips">请填写任务的标题以便在列表显示，不能超过50个字</p>
                    </div>
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">任务封面</label>
                    <div class="frm_controls">    
                        <input id="imgMain" name="id-input-file" type="file" onchange="change('imgMain')" />
                        <p class="frm_tips">为了更好的吸引销售人员接单，请上传任务封面</p>
                        <div class="form-group" id="dvimgMain">
                            <img alt="" id="imgMainPic" class="updivimg" src="" style="width: 150px; height: 150px;" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">公司所属行业</label>
                    <div class="frm_controls">
                        <div class="float_maxwidth">
                            <asp:dropdownlist id="drpCategory1" runat="server" cssclass="NRequire_select"></asp:dropdownlist>
                            <asp:dropdownlist id="drpCategory2" runat="server" cssclass="NRequire_select"></asp:dropdownlist>
                        </div>
                        <p class="frm_tips">请选择本任务所属行业，方便同个行业销售找到您</p>
                    </div>
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">办公所在区域</label>
                    <div class="frm_controls">
                        <div class="float_maxwidth">
                            <asp:dropdownlist id="drpProvince" runat="server" cssclass="NRequire_select"></asp:dropdownlist>
                            <asp:dropdownlist id="drpCity" runat="server" cssclass="NRequire_select"></asp:dropdownlist>
                        </div>
                        <p class="frm_tips">请选择本任务所属区域，方便同个地区销售找到您</p>
                    </div>
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">&nbsp;</label>
                    <div class="frm_controls">
                        <button class="wtbtn savebtn" type="button" title="下一步" onclick="NRequire_save_next(1)">
                            下一步
                        </button>
                    </div>
                </div>
            </div>
            <div class="NRequire_content_box" id="box2" style="display:none">
                <div class="frm_control_group">
                    <label class="frm_label need">目标客户行业</label>
                    <div class="frm_controls">
                        <div class="float_maxwidth">
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
                </div>
                <div class="frm_control_group">
                    <label class="frm_label need">目标客户区域</label>
                    <div class="frm_controls">
                        <div class="float_maxwidth">
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
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">目标客户</label>
                    <div class="frm_controls">
                        <div class="float_maxwidth">
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
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">&nbsp;</label>
                    <div class="frm_controls">
                        <button class="wtbtn savebtn" type="button" title="下一步" onclick="NRequire_save_next(2)">
                            下一步
                        </button>
                        <button class="wtbtn cancelbtn" type="button" title="上一步" onclick="NRequire_save_prev(2)">
                            上一步
                        </button>
                    </div>
                </div>
            </div>
            <div class="NRequire_content_box" id="box3" style="display:none">
                <div class="frm_control_group">
                    <label class="frm_label need">有效日期</label>
                    <div class="frm_controls">
                        <div class="float_maxwidth">
                            <div class="input-group">
                                <asp:textbox id="txtEffectiveEndDate" runat="server" cssclass="form-control date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:textbox>
                                <span class="input-group-addon">
                                    <i class="fa fa-calendar bigger-110"></i>
                                </span>
                            </div>
                        </div>
                        <p class="frm_tips">本任务接受投简历的有效日期，时间到了就自动关闭接受投简历</p>
                    </div>
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">预估合同额</label>
                    <div class="frm_controls">
                        <asp:textbox id="txtCost" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
                        <p class="frm_tips">预计任务成交的订单金额</p>
                    </div>
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">酬金类型</label>
                    <div class="frm_controls">
                        <div class="float_maxwidth">
                            <label class="NRequire_radio">
                                 <input name="CommissionType" class="ace" type="radio" id="radDecimal" runat="server" checked/>
                                 <span class="lbl">金额</span>
                            </label>
                            <label class="NRequire_radio">
                                 <input name="CommissionType" class="ace" type="radio" id="radPer" runat="server" />
                                 <span class="lbl">合同比例</span>
                            </label>
                        </div>
                    </div>
                </div>
                <div class="frm_control_group" id="divpercentage" style="display: none">
                    <label class="frm_label">比例（百分比) </label>
                    <div class="frm_controls">
                        <div class="float_maxwidth">
                            <asp:textbox id="txtpercentage" runat="server" cssclass="col-xs-2 col-sm-2 width40"></asp:textbox>
                            <label class="control-label">%</label>
                        </div>
                        <p class="frm_tips">按照预估合同额计算</p>
                    </div>
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">酬金</label>
                    <div class="frm_controls">
                        <asp:textbox id="txtCommission" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
                        <p class="frm_tips">支付给销售的酬金</p>
                    </div>
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">酬金说明</label>
                    <div class="frm_controls">
                        <asp:textbox id="txtCommissionDescription" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="50"></asp:textbox>
                        <p class="frm_tips">对酬金的说明</p>
                    </div>
                </div>
                <div class="frm_control_group" style="display:none">
                    <label class="frm_label">销售数量</label>
                    <div class="frm_controls">
                        <asp:textbox id="txtAgencySum" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
                        <p class="frm_tips">本任务可以选定多少个销售，达到数量自动停止接受投标。不填就默认只能选定一个销售</p>
                    </div>
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">&nbsp;</label>
                    <div class="frm_controls">
                        <button class="wtbtn savebtn" type="button" title="下一步" onclick="NRequire_save_next(3)">
                            下一步
                        </button>
                        <button class="wtbtn cancelbtn" type="button" title="上一步" onclick="NRequire_save_prev(3)">
                            上一步
                        </button>
                    </div>
                </div>
            </div>
            <div class="NRequire_content_box" id="box4" style="display:none">
                <div class="frm_control_group">
                    <label class="frm_label">联系人</label>
                    <div class="frm_controls">
                        <asp:textbox id="txtContactPerson" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                        <p class="frm_tips">本任务的联系人</p>
                    </div>
                </div>
                <div class="frm_control_group">
                    <label class="frm_label need">手机号码</label>
                    <div class="frm_controls">
                        <asp:textbox id="txtPhone" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                        <p class="frm_tips">联系人的手机号码</p>
                    </div>
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">更多联系方式</label>
                    <div class="frm_controls">
                        <asp:textbox id="txtContactPhone" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                        <p class="frm_tips">QQ或者微信等联系方式</p>
                    </div>
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">理想销售人员</label>
                    <div class="frm_controls">
                        <asp:textbox id="txtTargetAgency" runat="server" textmode="MultiLine" CssClass="description-textarea" maxlength="400"></asp:textbox>
                    </div>
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">&nbsp;</label>
                    <div class="frm_controls">
                        <button class="wtbtn savebtn" type="button" title="下一步" onclick="NRequire_save_next(4)">
                            下一步
                        </button>
                        <button class="wtbtn cancelbtn" type="button" title="上一步" onclick="NRequire_save_prev(4)">
                            上一步
                        </button>
                    </div>
                </div>
            </div>
            <div class="NRequire_content_box" id="box5" style="display:none">
                <div class="frm_control_group">
                    <label class="frm_label">任务详情</label>
                    <div class="frm_controls">
                        <div class="float_maxwidth">
                            <script id="container" name="content" type="text/plain" style="width:100%;">
       
                            </script>
                            <asp:hiddenfield id="hidDescription" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">附件列表</label>
                    <div class="frm_controls">
                        <div class="float_maxwidth">
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
                </div>
                <div class="frm_control_group">
                    <label class="frm_label">&nbsp;</label>
                    <div class="frm_controls">
                        <button class="wtbtn savebtn" type="button" title="提交审核" onclick="NRequire_save_next(5)">
                            提交审核
                        </button>
                        <button class="wtbtn cancelbtn" type="button" title="上一步" onclick="NRequire_save_prev(5)">
                            上一步
                        </button>
                    </div>
                </div>

                <!--任务编号-->
                <asp:textbox id="txtRequirementCode" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="50" enabled="false" Visible="false"></asp:textbox>
                <!--任务状态-->
                <asp:textbox id="txtStatus" runat="server" cssclass="col-xs-10 col-sm-5" enabled="false" Visible="false"></asp:textbox>
                <!--创建日期-->
                <asp:textbox id="txtCreatedAt" runat="server" cssclass="col-xs-10 col-sm-5" enabled="false" Visible="false"></asp:textbox>
                <!--对销售枪手详细要求-->
                <asp:textbox id="txtAgencyCondition" runat="server" textmode="MultiLine" CssClass="description-textarea" maxlength="400" Visible="false"></asp:textbox>
            </div>
        </div>
    </div>
    <asp:hiddenfield id="hidStatus" runat="server" />
    <asp:hiddenfield id="hidRequirementId" runat="server" />
</asp:Content>