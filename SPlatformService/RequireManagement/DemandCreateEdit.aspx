<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="DemandCreateEdit.aspx.cs" Inherits="SPlatformService.RequireManagement.DemandCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/RequireManagement/DemandCreateEditJS.js"></script>
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
                                    <asp:textbox id="txtDescription" runat="server" textmode="MultiLine" CssClass="description-textarea col-sm-5" maxlength="400" Height="200"></asp:textbox>
                                </div>
                            </div>
                            
                            <div class="form-group" id="divStatus" style="display: none;">
                                <label class="col-sm-2 control-label no-padding-right need">状态 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtStatus" runat="server" cssclass="col-xs-10 col-sm-5" enabled="false"></asp:textbox>
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
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidRequirementId" runat="server" />
</asp:Content>
