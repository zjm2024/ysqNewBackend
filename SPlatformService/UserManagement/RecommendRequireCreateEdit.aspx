<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="RecommendRequireCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.RecommendRequireCreateEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/UserManagement/RecommendRequireCreateEditJS.js"></script>  

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
                    <label class="col-sm-2 control-label no-padding-right need">任务推荐 </label>

                    <div class="col-sm-9">
                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                            <button class="wtbtn yjright" type="button" id="btn_newRequire" onclick="return NewRequire();" title="添加">
                                <i class="icon-ok bigger-110"></i>
                                添加
                            </button>
                            <button class="wtbtn yjright" type="button" id="btn_deleteRequire" onclick="return DeleteRequire();" title="删除">
                                <i class="icon-ok bigger-110"></i>
                                删除
                            </button>
                            <div class="space-4"></div>
                            <div id="divRequireList" runat="server">
                                <table id="RequireList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                    <thead>
                                        <tr class="ui-jqgrid-labels">
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                                <div class="" title="">
                                                    <input class="cbox" type="checkbox" onchange="checkAll(this, 'RequireList')" />                                                    
                                                </div>
                                            </th>                                            
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                <div class="ui-jqgrid-sortable" title="任务编号">任务编号</div>
                                            </th>
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                <div class="ui-jqgrid-sortable" title="任务标题">任务标题</div>
                                            </th>
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                <div class="ui-jqgrid-sortable" title="酬金">酬金</div>
                                            </th>
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 50px;">
                                                <div class="ui-jqgrid-sortable" title="序号">序号</div>
                                            </th>
                                            <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 40px;">
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
