<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="ContractCreateEdit.aspx.cs" Inherits="SPlatformService.ProjectManagement.ContractCreateEdit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/ProjectManagement/ContractCreateEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">项目名称 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtProjectName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">开始时间 </label>

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
                    <label class="col-sm-2 control-label no-padding-right">结束时间 </label>

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
                    <div class="hourinput">
                        <label class="col-sm-2 control-label no-padding-right need">合同金额 </label>

                        <div class="col-sm-9">
                            <asp:TextBox ID="txtCost" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="hourinput">
                        <label class="col-sm-2 control-label no-padding-right need">酬金 </label>

                        <div class="col-sm-9">
                            <asp:TextBox ID="txtCommission" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <h4 class="header">上传线下合同</h4>
                </div>
                <div class="form-group">
                    <div class="col-sm-9">
                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                            <div id="divFileAdd" runat="server">
                                <input id="fileAdd" name="id-input-file" type="file" onchange="changefile('fileAdd')" />
                            </div>
                            <button class="wtbtn yjright" type="button" id="btn_deleteattach" runat="server" onclick="return DeleteFile();" title="删除">
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
                <div class="form-group">
                    <h4 class="header">阶段设置</h4>
                </div>
                <div class="form-group">
                    <div class="col-sm-9">
                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                            <button class="wtbtn yjright" type="button" id="btn_newsteps" runat="server" onclick="return NewContractSteps();" title="添加">
                                <i class="icon-ok bigger-110"></i>
                                添加
                            </button>
                            <button class="wtbtn yjright" type="button" id="btn_deletesteps" runat="server" onclick="return DeleteContractSteps();" title="删除">
                                <i class="icon-ok bigger-110"></i>
                                删除
                            </button>
                            <div class="space-4"></div>
                            <div id="divContractStepsList" runat="server">
                                <table id="ContractStepsList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                    <thead>
                                        <tr class="ui-jqgrid-labels">
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                                <div class="" title="">
                                                    <input class="cbox" type="checkbox" onchange="checkAll(this, 'ContractStepsList')" />
                                                </div>
                                            </th>
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 50px;">
                                                <div class="ui-jqgrid-sortable" title="序号">序号</div>
                                            </th>
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                <div class="ui-jqgrid-sortable" title="阶段名称">阶段名称</div>
                                            </th>
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                <div class="ui-jqgrid-sortable" title="酬金">酬金</div>
                                            </th>
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 250px;">
                                                <div class="ui-jqgrid-sortable" title="内容">内容</div>
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
                <div class="form-group">
                    <h4 class="header">协议内容</h4>
                </div>
                <div id="divContractNote" runat="server"></div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="text-align: center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" runat="server" title="保存协议" style="display: none;">
                            保存协议
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_businessapprove" runat="server" title="雇主同意" style="display: none;">
                            雇主同意
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_businesscancel" runat="server" title="雇主取消" style="display: none;">
                            雇主取消
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_agencyapprove" runat="server" title="销售同意" style="display: none;">
                            销售同意
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_agencycancel" runat="server" title="销售取消" style="display: none;">
                            销售取消
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidContractId" runat="server" />
</asp:Content>
