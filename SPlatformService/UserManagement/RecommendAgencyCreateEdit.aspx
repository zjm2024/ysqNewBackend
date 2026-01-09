<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="RecommendAgencyCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.RecommendAgencyCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/UserManagement/RecommendAgencyCreateEditJS.js"></script>

    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">区域 </label>

                    <div class="col-sm-9">
                        <asp:DropDownList ID="drpProvince" runat="server" CssClass="col-xs-10 col-sm-2">
                        </asp:DropDownList>
                        <asp:DropDownList ID="drpCity" runat="server" CssClass="col-xs-10 col-sm-2">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">行业 </label>

                    <div class="col-sm-9">
                        <asp:DropDownList ID="drpParentCategory" runat="server" CssClass="col-xs-10 col-sm-2">
                        </asp:DropDownList>
                        <asp:DropDownList ID="drpCategory" runat="server" CssClass="col-xs-10 col-sm-2">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">销售推荐 </label>

                    <div class="col-sm-9">
                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                            <button class="wtbtn yjright" type="button" id="btn_newAgency" onclick="return NewAgency();" title="添加">
                                <i class="icon-ok bigger-110"></i>
                                添加
                            </button>
                            <button class="wtbtn yjright" type="button" id="btn_deleteAgency" onclick="return DeleteAgency();" title="删除">
                                <i class="icon-ok bigger-110"></i>
                                删除
                            </button>
                            <div class="space-4"></div>
                            <div id="divAgencyList" runat="server">
                                <table id="AgencyList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                    <thead>
                                        <tr class="ui-jqgrid-labels">
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                                <div class="" title="">
                                                    <input class="cbox" type="checkbox" onchange="checkAll(this, 'AgencyList')" />                                                    
                                                </div>
                                            </th>
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                <div class="ui-jqgrid-sortable" title="编号">编号</div>
                                            </th>
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                <div class="ui-jqgrid-sortable" title="会员名称">会员名称</div>
                                            </th>
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                <div class="ui-jqgrid-sortable" title="销售名称">销售名称</div>
                                            </th>
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 50px;">
                                                <div class="ui-jqgrid-sortable" title="序号">序号</div>
                                            </th>
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 80px;">
                                                <div class="ui-jqgrid-sortable" title="排序">排序</div>
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
                                            <td style="" title=""></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width: 700px; text-align: center;">
                        <button class="wtbtn savebtn" type="button" id="btn_submit" runat="server" title="保存">
                            保存
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
