<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardBusinessCardCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.CardBusinessCardCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardBusinessCardCreateEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">公司名称</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtBusinessName" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">LOGO</label>
                    <div class="col-sm-9">
                        <input id="imgLogoImg" name="id-input-file" type="file" onchange="change('imgLogoImg')" />
                        <div id="divPersonalCardImg" runat="server">
                            <%--<ul class="ace-thumbnails clearfix" id="divPersonalCard">
                            </ul>--%>
                            <img id="imgLogoImgPic" src="" style="width: 150px" alt="" runat="server" ></img>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">行业</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtIndustry" runat="server" CssClass="col-xs-10 col-sm-5" ></asp:TextBox>
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">限制人数</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtNumber" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">限制子公司数量</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="tetSubsidiarySum" runat="server" CssClass="col-xs-10 col-sm-5" ></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">到期时间</label>
                    <div class="col-sm-3">
                        <div class="input-group col-sm-9">
                            <asp:TextBox ID="txtExpirationAt" runat="server" CssClass="form-control date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:TextBox>
                            <span class="input-group-addon">
                                <i class="fa fa-calendar bigger-110"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">营业执照</label>
                    <div class="col-sm-9">
                        <input id="imgBusinessLicenseImg" name="id-input-file" type="file" onchange="change('imgBusinessLicenseImg')" />
                        <div id="div1" runat="server">
                            <%--<ul class="ace-thumbnails clearfix" id="divPersonalCard">
                            </ul>--%>
                            <img id="imgBusinessLicenseImgPic" src="" style="width: 150px" alt="" runat="server" ></img>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">是否开通在线支付 </label>
                    <div class="col-sm-9">
                        <label>
                            <input name="isPay" class="ace" type="radio" id="isPayDecimal" checked="checked" />
                            <span class="lbl">是</span>
                        </label>
                        <label>
                            <input name="isPay" class="ace" type="radio" id="isPay" />
                            <span class="lbl">否</span>
                        </label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">是否开通代理商功能 </label>
                    <div class="col-sm-9">
                        <label>
                            <input name="isAgent" class="ace" type="radio" id="isAgentDecimal" checked="checked" />
                            <span class="lbl">是</span>
                        </label>
                        <label>
                            <input name="isAgent" class="ace" type="radio" id="isAgent" />
                            <span class="lbl">否</span>
                        </label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">是否为集团 </label>
                    <div class="col-sm-9">
                        <label>
                            <input name="isGroup" class="ace" type="radio" id="radDecimal" checked="checked" />
                            <span class="lbl">是</span>
                        </label>
                        <label>
                            <input name="isGroup" class="ace" type="radio" id="radPer" />
                            <span class="lbl">否</span>
                        </label>
                    </div>
                </div>  
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">企业版本 </label>

                    <div class="col-sm-9">
                        <asp:DropDownList ID="OfficialProducts" runat="server" CssClass="col-xs-10 col-sm-2">
                            <asp:ListItem Text="微企版" Value="SelfEmployed"></asp:ListItem>
                            <asp:ListItem Text="基础版" Value="Basic"></asp:ListItem>
                            <asp:ListItem Text="标准版" Value="Standard"></asp:ListItem>
                            <asp:ListItem Text="专业版" Value="Advanced"></asp:ListItem>
                            <asp:ListItem Text="集团版" Value="Group"></asp:ListItem>
                        </asp:DropDownList>
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
    <asp:HiddenField ID="BusinessID" runat="server" />
</asp:Content>
