<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="AgencyExperienceEdit.aspx.cs" Inherits="SPlatformService.CustomerManagement.AgencyExperienceEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/AgencyExperienceEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
   <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">标题 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">项目时间 </label>

                    <div class="col-sm-3">
                        <div class="input-group col-sm-9">
                        <asp:TextBox ID="txtProjectDate" runat="server" CssClass="form-control date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:TextBox>
                        <span class="input-group-addon">
                            <i class="fa fa-calendar bigger-110"></i>
                        </span>
                    </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">案例缩略图 </label>
                    <div class="col-sm-9">
                        <input id="imgMain" name="id-input-file" type="file" onchange="changeone('imgMain')" />
                        <div class="form-group" id="dvimgMain">
                            <div class="col-sm-9">
                                <img alt="" id="imgMainPic" src="" style="width: 150px; height: 150px;" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">案例细节图 </label>

                    <div class="col-sm-9">
                        <input id="imgDetail" name="id-input-file" type="file" onchange="change('imgDetail','divDetail')" />
                        <div>
                            <ul class="ace-thumbnails clearfix" id="divDetail">
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">描述 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="col-xs-10 col-sm-5" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="divApprove">
                    <label class="col-sm-2 control-label no-padding-right">驳回原因 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtApproveComment" runat="server" TextMode="MultiLine" CssClass="col-xs-10 col-sm-5" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="width: 700px; text-align: center;">                        
                        <button class="wtbtn savebtn" type="button" id="btn_save" style="display:none;" title="保存">
                            保存
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_updateagencyexperiencestatus" title="取消认证">
                            取消审核
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_updateagencyexperiencestatuscommit" title="认证通过">
                            通过
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_updateagencyexperiencestatusreject" title="认证驳回">
                            驳回
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                            返回
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidAgencyExperienceId" runat="server" />
</asp:Content>
