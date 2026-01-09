<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="DepartmentCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.DepartmentCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/UserManagement/DepartmentCreateEditJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">部门名称 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtDepartmentName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="30"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">备注 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtDescription" runat="server" style="height: 150px;width: 420px;border: 1px solid #d7d3d3;background-color: #fff;padding-left: 10px;font-size: 12px;" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>

                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width:700px;text-align:center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="保存" >
                            保存
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_delete" title="删除" style="display:none;">
                            删除
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                            返回
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidDepartmentId" runat="server" />
    <asp:HiddenField ID="hidIsDelete" runat="server" />
    <asp:HiddenField ID="hidIsEdit" runat="server" />
</asp:Content>
