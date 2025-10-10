<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="BaseDataEdit.aspx.cs" Inherits="SPlatformService.UserManagement.BaseDataEdit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/UserManagement/BaseDataEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/wangEditor.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">数据类型</label>

                    <div class="col-sm-9">
                        <asp:DropDownList ID="drpBaseData" runat="server" CssClass="col-xs-10 col-sm-5">
                            <asp:ListItem Text="用户协议" Value="用户协议" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="关于我们" Value="关于我们"></asp:ListItem>
                            <asp:ListItem Text="雇主规则" Value="雇主规则"></asp:ListItem>
                            <asp:ListItem Text="押金说明" Value="押金说明"></asp:ListItem>
                            <asp:ListItem Text="租客规则" Value="租客规则"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">备注 </label>

                    <div class="col-sm-9">
                        <%--<asp:TextBox ID="txtDescription" runat="server" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" TextMode="MultiLine"></asp:TextBox>--%>
                        <div id="editor">
                            
                        </div>
                    </div>
                </div>

                <hr />

                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width: 700px; text-align: center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="保存">
                            保存
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidBaseDataId" runat="server" />
</asp:Content>
