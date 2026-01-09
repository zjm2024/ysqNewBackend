<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardSoftarticleShow.aspx.cs" Inherits="SPlatformService.RequireManagement.CardSoftarticleShow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardSoftarticleShowJS.js"></script>
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
                                <label class="col-sm-2 control-label no-padding-right">标题 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="txtTitle" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">发布日期 </label>

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
                                <label class="col-sm-2 control-label no-padding-right">原创 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="Textbox1" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">转载 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="Textbox2" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">浏览量 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="Textbox3" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">转载量 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="Textbox4" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">点赞量 </label>

                                <div class="col-sm-9">
                                    <asp:textbox id="Textbox5" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">修改浏览量 </label>
                                <div class="col-sm-9">
                                    <button  class="wtbtn savebtn" type="button" id="saveread" title="修改浏览量" />
                                        保存修改
                                    </button>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label no-padding-right">软文详情 </label>

                                <div class="col-sm-9 Description" style=" width:545px;font-size: 14px;">
                                    <%=Description %>
                                </div>
                                <style>
                                    .Description{ padding:5px 12px;}
                                    .Description img{ width:100%;}
                                     .Description div{ padding-bottom:10px;}
                                </style>
                            </div>
                           
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width:700px;text-align:center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="删除文章" >
                            删除文章
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                            返回
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidSoftArticleID" runat="server" />
</asp:Content>
