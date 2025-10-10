<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CustomerEdit.aspx.cs" Inherits="WebUI.CustomerManagement.CustomerEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/CustomerCreateEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">头像 </label>

                    <div class="col-sm-9">
                            <span class="profile-picture">
                                <img id="imgHeaderLogoPic" src="../Style/images/logo.png" style="width: 150px" alt="" runat="server" ></img>
                            </span>
                            <input id="imgHeaderLogo" name="id-input-file" type="file" onchange="change('imgHeaderLogo')" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">会员编号 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">会员账号 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCustomerAccount" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">联系电话 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="20"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">会员名称 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                        <p style="width:100%;float: left;color: rgba(66, 64, 64, 0.72)">为了您的信息安全，会员名称请勿使用手机号或QQ等联系方式</p>
                    </div>
                    
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">性别 </label>

                    <div class="col-sm-9">
                        <label>
                            <input name="sex" class="ace" type="radio" id="radSexMale" runat="server"/>
                            <span class="lbl">男</span>
                        </label>
                        <label>
                            <input name="sex" class="ace" type="radio" id="radSexFeMale"  runat="server" />
                            <span class="lbl">女</span>
                        </label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">生日 </label>

                    <div class="col-sm-3 no-padding-right">
                        <div class="input-group col-sm-9">
                            <asp:TextBox ID="txtBirthday" runat="server" CssClass="form-control date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:TextBox>
                            <span class="input-group-addon">
                                <i class="fa fa-calendar bigger-110"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">邮箱 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">雇主资料 </label>

                    <div class="col-sm-9">
                        <asp:Label CssClass="control-label"  ID="lblBusinessInfo" runat="server" Text="未认证"></asp:Label>
                        <button class="wtbtn wtbtn-create" type="button" id="btn_ViewBusiness" runat="server" title="查看">
                            查看
                        </button>
                        <button class="wtbtn wtbtn-create" type="button" id="btn_ApplicantBusiness" runat="server" title="申请认证">
                            申请认证
                        </button>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">销售资料 </label>

                    <div class="col-sm-9">
                        <asp:Label CssClass="control-label" ID="lblAgencyInfo" runat="server" Text="未认证"></asp:Label>
                        <button class="wtbtn wtbtn-create" type="button" id="btn_ViewAgency" runat="server" title="查看">
                            查看
                        </button>
                        <button class="wtbtn wtbtn-create" type="button" id="btn_ApplicantAgency" runat="server" title="申请认证">
                            申请认证
                        </button>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right">个人描述 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="description-textarea"  MaxLength="400"></asp:TextBox>
                    </div>
                </div>               
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="text-align: center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="保存">
                            保存
                        </button>
                        <button class="wtbtn cancelbtn" type="button" runat="Server"  onserverclick="UserLogout" title="退出登陆">
                            退出登陆
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidCustomerId" runat="server" />
</asp:Content>
