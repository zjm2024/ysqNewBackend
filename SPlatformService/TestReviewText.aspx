<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="TestReviewText.aspx.cs" Inherits="SPlatformService.TestReviewText" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">    
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
				<div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">审核文本</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="TextBox1" runat="server"  TextMode="MultiLine"  Style="height: 400px; width: 550px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">审核结果</label>
                    <div class="col-sm-9">
                         <asp:TextBox ID="txtTitle" runat="server" CssClass="col-xs-10 col-sm-5" Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width:700px;text-align:center;">
                        <asp:Button ID="Button1" runat="server" class="wtbtn savebtn" OnClick="Button1_Click" Text="审核文本" Width="184px" />
                    </div>
                </div>
            </div>
        </div>
    </div>    
</asp:Content>
