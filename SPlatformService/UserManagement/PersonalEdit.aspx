<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="PersonalEdit.aspx.cs" Inherits="SPlatformService.UserManagement.PersonalEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/UserManagement/PersonalEditJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">部门</label>

                    <div class="col-sm-9">
                        <asp:DropDownList ID="drpDepartment" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false">
                            <asp:ListItem Text="--选择--" Value="-1" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <%--<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">角色 </label>

                    <div class="col-sm-9">
                        <asp:DropDownList ID="drpRole" runat="server" CssClass="col-xs-10 col-sm-5"  Enabled="false">
                            <asp:ListItem Text="--选择--" Value="-1" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>--%>
                <div class="form-group" style="margin-top: 10px;">
                    <label class="col-sm-2 control-label no-padding-right need">角色 </label>
                    <%--<div class="col-sm-3">
                        <input type="text" id="listRoleFilter" class="form-control" onkeyup="return OnFilter(this,'listRole')" />
                        <asp:ListBox ID="listRole" runat="server" Style="height: 150px;" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                        <asp:HiddenField ID="hidSelectedRole" runat="server" />
                    </div>
                    <div class="col-sm-1" style="margin-top: 30px;">
                        <button class="wtbtnsearch" style="width: 40px;" type="submit" id="btn_AddRole" title="添加" onclick="return OnAddList('listRole','listRoleSelected','hidSelectedRole');">
                            添加
                        </button>
                        <br />
                        <br />
                        <button class="wtbtnsearch" style="width: 40px;" type="submit" id="btn_RemoveRole" title="移除" onclick="return OnRemoveList('listRole','listRoleSelected','hidSelectedRole');">
                            移除
                        </button>
                    </div>--%>
                    <div class="col-sm-3">
                        <%--<input type="text" id="listRoleSelectedFilter" class="form-control" onkeyup="return OnFilter(this,'listRoleSelected')" />--%>
                        <asp:ListBox ID="listRoleSelected" runat="server" Style="height: 150px;" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                    </div>
                </div>
                <hr />
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">用户姓名 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="30"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">登录名 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtLoginName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="30"></asp:TextBox>
                    </div>
                </div>                

                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">电话 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50" ></asp:TextBox>
                    </div>
                </div>
                
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">邮箱 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="100" ></asp:TextBox>
                    </div>
                </div>
                
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">性别 </label>

                    <div class="col-sm-9">
                        <label>
                            <input name="sex" class="ace" type="radio" id="radSexMale" checked="checked"  />
                            <span class="lbl">男</span>
                        </label>
                         <label>
                            <input name="sex" class="ace" type="radio" id="radSexFeMale"  />
                            <span class="lbl">女</span>
                        </label>
                    </div>
                </div>
                
                <%--<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">小时工资 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtSalary" runat="server" CssClass="col-xs-10 col-sm-5" TextMode="Email"></asp:TextBox>
                    </div>
                </div>

                <div class="space-4"></div>--%>

                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">备注 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtDescription" runat="server" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>

                <hr />

                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width: 700px; text-align: center;">                       
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="保存">
                            保存
                        </button> 
                        <%-- <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="取消">
                            取消
                        </button>  --%>                      
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidUserId" runat="server" />
</asp:Content>
