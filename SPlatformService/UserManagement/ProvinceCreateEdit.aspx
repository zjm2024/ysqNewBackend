<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="ProvinceCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.ProvinceCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/UserManagement/ProvinceCreateEditJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
				
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">省（直辖市）编号 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtProvinceCode" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="10"></asp:TextBox>
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">省（直辖市）名称 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtProvinceName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="10"></asp:TextBox>
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">状态 </label>

                    <div class="col-sm-9">
                        <label>
                            <input name="status" class="ace" type="radio" id="radStatusEnable" checked="checked" />
                            <span class="lbl">启用</span>
                        </label>
                        <label>
                            <input name="status" class="ace" type="radio" id="radStatusDisable" />
                            <span class="lbl">禁用</span>
                        </label>
                    </div>
                </div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width:700px;text-align:center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="保存" >
                            保存
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                            返回
                        </button>
                    </div>
                </div>

                <div id="divCity">
                    <div class="form-group">
                        <h4 class="header">城市</h4>
                    </div>

                    <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                        <button class="wtbtn yjright" type="button" id="btn_new" onclick="return NewCity();" title="添加">
                            <i class="icon-ok bigger-110"></i>
                            添加
                        </button>

                        <div class="space-4"></div>
                        <table id="CityList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <thead>
                                <tr class="ui-jqgrid-labels">
                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                        <div class="ui-jqgrid-sortable" title="城市编号">城市编号</div>
                                    </th>
                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                        <div class="ui-jqgrid-sortable" title="城市名称">城市名称</div>
                                    </th>
                                    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                        <div class="ui-jqgrid-sortable" title="状态">状态</div>
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
                    <br />
                    <br />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidProvinceId" runat="server" />
</asp:Content>
