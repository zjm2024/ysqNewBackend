<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="DemandCreateEdit.aspx.cs" Inherits="WebUI.RequireManagement.DemandCreateEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/RequireManagement/DemandCreateEditJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="tabbable">
                    <ul class="nav nav-tabs padding-12 tab-color-blue background-blue" id="myTab4">
                        <li class="<%if (dropdown14) { Response.Write("active"); } %>">
                            <a data-toggle="tab" href="#home4">需求信息</a>
                        </li>
                        <li class="<%if (!dropdown14) { Response.Write("active"); } %>">
                            <a data-toggle="tab" href="#dropdown14">查看留言</a>
                        </li>
                    </ul>
                    <div class="tab-content">
                        <div id="home4" class="tab-pane <%if (dropdown14) { Response.Write("active"); } %>">
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right need">需求分类</label>

                                <div class="col-sm-9">
                                    <asp:dropdownlist id="drpdemand_class" runat="server" cssclass="col-xs-10 col-sm-2"></asp:dropdownlist>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right need">手机号码 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtPhone" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right need">有效日期 </label>

                                <div class="col-sm-9">
                                    <div class="input-group col-sm-2">
                                        <asp:textbox id="txtEffectiveEndDateCreateEdit" runat="server" cssclass="form-control date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:textbox>
                                        <span class="input-group-addon">
                                            <i class="fa fa-calendar bigger-110"></i>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">需求详情 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtDescription" runat="server" textmode="MultiLine" CssClass="description-textarea" maxlength="400"></asp:textbox>
                                </div>
                            </div>
                            
                            <div class="form-group" id="divStatus" style="display: none;">
                                <label class="col-sm-2 control-label no-padding-right need">状态 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtStatus" runat="server" cssclass="col-xs-10 col-sm-5" enabled="false"></asp:textbox>
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
                                </div>
                            </div>
                            <%--</div>
                        <div id="profile4" class="tab-pane">--%>
                        </div>
                        <div id="dropdown14" class="tab-pane <%if (!dropdown14) { Response.Write("active"); } %>">
                            <div class="form-group">
                                <%--<label class="col-sm-2 control-label no-padding-right">投标列表 </label>--%>

                                <div class="col-sm-9">
                                    <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;" id="divTenderInviteList" runat="server">
                                        <table id="TenderInviteList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                            <thead>
                                                <tr class="ui-jqgrid-labels">
                                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;"></th>
                                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                        <div class="ui-jqgrid-sortable" title="留言日期">留言日期</div>
                                                    </th>
                                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                        <div class="ui-jqgrid-sortable" title="留言人名称">留言人名称</div>
                                                    </th>
                                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                        <div class="ui-jqgrid-sortable" title="联系号码">联系号码</div>
                                                    </th>
                                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                        <div class="ui-jqgrid-sortable" title="留言">留言</div>
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                                    <td style="" title=""></td>
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
                </div>
            </div>
        </div>
    </div>

    <asp:hiddenfield id="hidStatus" runat="server" />
    <asp:hiddenfield id="hidRequirementId" runat="server" />
</asp:Content>