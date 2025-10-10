<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="RoleCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.RoleCreateEdit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/UserManagement/RoleCreateEditJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">角色名称 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtRoleName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="30"></asp:TextBox>
                    </div>
                </div>
                
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">备注 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtDescription" runat="server" Style="height: 150px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <%--<div class="hr hr-dotted"></div>--%>
                <div class="form-group">
                    <h4 class="header">权限配置</h4>
                </div>
                <div class="ui-jqgrid-bdiv" style="height: 100%; width: 1050px;">
                    <table id="SecurityList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <thead>
                            <tr class="ui-jqgrid-labels">
                                <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                    <div class="ui-jqgrid-sortable" title="权限分类">权限分类</div>
                                </th>
                                <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                    <div class="ui-jqgrid-sortable" title="权限名称">权限名称</div>
                                </th>
                                <th class="ui-state-default ui-th-column ui-th-ltr">
                                    <div class="ui-jqgrid-sortable" title="设置">设置</div>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                <td style="" title=""></td>
                                <td style="" title=""></td>
                            </tr>
                            <%--<tr class="ui-widget-content jqgrow ui-row-ltr">
                                <td style="" title="工时录入">工时录入</td>
                                <td style="" title="">
                                    <label class="position-relative col-sm-2">
                                        <input class="ace" type="checkbox" />
                                        <span class="lbl">查看</span>
                                    </label>
                                </td>
                            </tr>
                            <tr class="ui-widget-content jqgrow ui-row-ltr">
                                <td style="" title="员工管理">员工管理</td>
                                <td style="" title="">
                                    <label class="position-relative col-sm-2">
                                        <input class="ace" type="checkbox" />
                                        <span class="lbl">查看</span>
                                    </label>
                                    <label class="position-relative col-sm-2">
                                        <input class="ace" type="checkbox" />
                                        <span class="lbl">编辑</span>
                                    </label>
                                    <label class="position-relative col-sm-2">
                                        <input class="ace" disabled type="checkbox" />
                                        <span class="lbl">删除</span>
                                    </label>
                                </td>
                            </tr>
                            <tr class="ui-widget-content jqgrow ui-row-ltr">
                                <td style="" title="员工管理">员工管理</td>
                                <td style="" title=""></td>
                            </tr>--%>
                        </tbody>
                    </table>
                </div>


                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width: 700px; text-align: center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="保存">
                            保存
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_delete" title="删除" style="display: none;">
                            删除
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                            返回
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidRoleId" runat="server" />
    <asp:HiddenField ID="hidIsDelete" runat="server" />
    <asp:HiddenField ID="hidIsEdit" runat="server" />
</asp:Content>
