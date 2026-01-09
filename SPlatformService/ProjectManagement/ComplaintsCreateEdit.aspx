<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="ComplaintsCreateEdit.aspx.cs" Inherits="SPlatformService.ProjectManagement.ComplaintsCreateEdit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/ProjectManagement/ComplaintsCreateEditJS.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">项目名称 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtProjectCode" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">雇主 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtBusinessName" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">销售 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtAgencyName" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">申请人 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCreator" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">申请时间 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCreatedAt" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">申请理由 </label>

                    <div class="col-sm-9">
                         <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="col-xs-10 col-sm-5" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">图片 </label>

                    <div class="col-sm-9">
                       <ul class="ace-thumbnails clearfix" id="divImgDetail">
                               
                            </ul>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">状态 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtStatus" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">处理结果 </label>

                    <div class="col-sm-9">
                         <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" CssClass="col-xs-10 col-sm-5" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width: 700px; text-align: center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="跟进">
                            跟进
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_reslove" title="解决">
                            解决
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                            返回
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidComplaintsId" runat="server" />
</asp:Content>
