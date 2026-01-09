<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="ServicesCreateEdit.aspx.cs" Inherits="SPlatformService.RequireManagement.ServicesCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/RequireManagement/ServicesCreateEditJS.js"></script>
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
                    <label class="col-sm-2 control-label no-padding-right">服务类别 </label>

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
                            <table id="TargetCategoryList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                <thead>
                                    <tr class="ui-jqgrid-labels">
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                            <div class="" title="">
                                                <input class="cbox" type="checkbox" onchange="checkAll(this, 'UserProjectList')" />
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
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">标题 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">价格 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPrice" runat="server" CssClass="col-xs-10 col-sm-5" ></asp:TextBox>
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">数量 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCount" runat="server" CssClass="col-xs-10 col-sm-5" ></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">封面 </label>

                    <div class="col-sm-9">
                        <img id="imgMain" src="../Style/images/logo.png" style="width: 150px"></img>
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">描述 </label>

                    <div class="col-sm-9">
                        <script id="container" name="content" type="text/plain">
       
                        </script>
                    </div>
                </div>				
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">状态 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtStatus" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false"></asp:TextBox>
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">创建日期 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCreatedAt" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width:700px;text-align:center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="保存" >
                            保存
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_updateservicesstatus" title="取消发布" style="display:none;">
                            取消发布
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                            返回
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidServicesId" runat="server" />
</asp:Content>
