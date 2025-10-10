<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CustomerCreateEdit.aspx.cs" Inherits="SPlatformService.CustomerManagement.CustomerCreateEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/CustomerCreateEditJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">头像 </label>

                    <div class="col-sm-9">
                            <span class="profile-picture">
                                <img id="imgHeaderLogo" src="../Style/images/logo.png" style="width: 150px"></img>
                            </span>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">会员编号 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">会员账号 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCustomerAccount" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">联系电话 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="20"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">会员名称 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">性别 </label>

                    <div class="col-sm-9">
                        <label>
                            <input name="sex" class="ace" type="radio" id="radSexMale" checked="checked" />
                            <span class="lbl">男</span>
                        </label>
                        <label>
                            <input name="sex" class="ace" type="radio" id="radSexFeMale" />
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
                    <label class="col-sm-2 control-label no-padding-right">个人描述 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="col-xs-10 col-sm-5" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">乐币操作</label>

                    <div class="col-sm-9">
                        <a href="ManualSetZXB.aspx?CustomerId=<%=mid %>" style="line-height: 26px;">发放奖励</a>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">雇主资料 </label>

                    <div class="col-sm-9">
                        <asp:Label CssClass="control-label"  ID="lblBusinessInfo" runat="server" Text="未认证"></asp:Label>
                        <button class="wtbtn wtbtn-create" type="button" id="btn_ViewBusiness" runat="server" title="查看">
                            查看
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
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">状态 </label>

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
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">代理商 </label>

                    <div class="col-sm-9">
                        <label>
                            <input name="Agent" class="ace" type="radio" id="radAgentEnable"/>
                            <span class="lbl">是</span>
                        </label>
                        <label>
                            <input name="Agent" class="ace" type="radio" id="radAgentDisable"  checked="checked" />
                            <span class="lbl">否</span>
                        </label>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right">VIP </label>

                    <div class="col-sm-9">
                        <label>
                            <input name="Vip" class="ace" type="radio" id="VipEnable"/>
                            <span class="lbl">是</span>
                        </label>
                        <label>
                            <input name="Vip" class="ace" type="radio" id="VipDisable"  checked="checked" />
                            <span class="lbl">否</span>
                        </label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">VIP级别 </label>
                    <div class="col-sm-9">
                        <asp:DropDownList ID="VipLevel" runat="server" CssClass="col-xs-10 col-sm-2">
                            <asp:ListItem Text="普通用户" Value="0"></asp:ListItem>
                            <asp:ListItem Text="五星会员" Value="1"></asp:ListItem>
                            <asp:ListItem Text="合伙人" Value="2"></asp:ListItem>
                            <asp:ListItem Text="分公司" Value="3"></asp:ListItem>
                            <asp:ListItem Text="三星会员" Value="4"></asp:ListItem>
                            <asp:ListItem Text="四星会员" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">Vip到期时间 </label>

                    <div class="col-sm-3 no-padding-right">
                        <div class="input-group col-sm-9">
                            <asp:TextBox ID="ExpirationAt" runat="server" CssClass="form-control date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:TextBox>
                            <span class="input-group-addon">
                                <i class="fa fa-calendar bigger-110"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">来源会员ID </label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="originCustomerId" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width: 700px; text-align: center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="保存">
                            保存
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                            返回
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_Finance" title="财务明细">
                            财务明细
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_PromotionAwards" title="发放推广奖励">
                            发放推广奖励
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidCustomerId" runat="server" />
    <asp:HiddenField ID="hidIsDelete" runat="server" />
    <asp:HiddenField ID="hidIsEdit" runat="server" />
</asp:Content>
