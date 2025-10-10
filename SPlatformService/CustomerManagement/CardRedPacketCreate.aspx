<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardRedPacketCreate.aspx.cs" Inherits="SPlatformService.UserManagement.CardRedPacketCreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/CardRedPacketCreate.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">红包总额</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtRPCost" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                 <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">红包个数</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtRpNum" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                       <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">单个红包最低金额</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtRPOneCost" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>(必须大于0.3)
                    </div>
                </div>
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">红包说明</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtRpContent" runat="server" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
              
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width:700px;text-align:center;">
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="发送" >
                            发送
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                            返回
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="RedPacketId" runat="server" />
</asp:Content>
