<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="FarmgamePrizeCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.FarmgamePrizeCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/FarmgamePrizeCreateEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">奖品名称</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">缩略图 </label>

                    <div class="col-sm-9">
                        <input id="ImgUrl" name="id-input-file" type="file" onchange="change('ImgUrl')" />
                        <div id="divPersonalCardImg" runat="server">
                            <%--<ul class="ace-thumbnails clearfix" id="divPersonalCard">
                            </ul>--%>
                            <img id="ImgUrlPic" src="https://www.zhongxiaole.net/SPManager/Style/images/logo.png" style="width: 150px" alt="" runat="server" ></img>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">商品链接（内部使用）</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtProductUrl" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">兑换价格（钻石）</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPrice" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">排序</label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="OrderNO" runat="server" CssClass="col-xs-10 col-sm-5"></asp:TextBox>
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
    <asp:HiddenField ID="PrizeID" runat="server" />
</asp:Content>
