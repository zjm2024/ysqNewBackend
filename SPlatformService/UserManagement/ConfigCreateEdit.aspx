<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="ConfigCreateEdit.aspx.cs" Inherits="SPlatformService.UserManagement.ConfigCreateEdit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/UserManagement/ConfigCreateEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
    <!-- 配置文件 -->
    <script type="text/javascript" src="../Scripts/UEditor/ueditor.config.js"></script>
    <!-- 编辑器源码文件 -->
    <script type="text/javascript" src="../Scripts/UEditor/ueditor.all.js"></script>
    <script type="text/javascript" charset="utf-8" src="../Scripts/UEditor/lang/zh-cn/zh-cn.js"></script>
    <!-- 实例化编辑器 -->
    <script type="text/javascript">
        var ue = UE.getEditor('container');
        var ue2 = UE.getEditor('zxbNote');
    </script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">短信API </label>

                    <div class="col-sm-9">
                        <asp:textbox id="txtMessageAPI" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="200"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">短信提醒剩余条数 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="txtMessageNotiCount" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">提醒手机号码 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="txtMessageNotiPhone" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">发布是否需要审核 </label>
                    <div class="col-sm-9">
                        <label class="position-relative" style="margin-top: 6px;">
                            <input id="chk_IsNeedApprove" class="ace" type="checkbox" />
                            <span class="lbl"></span>
                        </label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">酬金提点比例 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="txtCommissionPercentage" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">已发酬金数量 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="txtCommissionTotal" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">成交案例数量 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="txtProjectTotal" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">是否显示已发酬金数量 </label>

                    <div class="col-sm-9">
                        <label class="position-relative" style="margin-top: 6px;">
                            <input id="chk_IsCommissionTotalShow" class="ace" type="checkbox" />
                            <span class="lbl"></span>
                        </label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">是否显示成交案例数量 </label>

                    <div class="col-sm-9">
                        <label class="position-relative" style="margin-top: 6px;">
                            <input id="chk_IsProjectTotalShow" class="ace" type="checkbox" />
                            <span class="lbl"></span>
                        </label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">查看销售联系方式个数 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="txtViewAgencyCount" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">查看雇主联系方式个数 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="txtViewBusinessCount" runat="server" cssclass="col-xs-10 col-sm-5"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">首次托管至少托管多少比例（%）</label>

                    <div class="col-sm-9">
                        <asp:textbox id="FirstMandates" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right need">通过注册奖励文本 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="zxbRegistered_text" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right need">通过注册奖励数目 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="zxbRegistered" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right need">完成认证奖励文本 </label>

                    <div class="col-sm-9" style="display:none">
                        <asp:textbox id="zxbCertification_text" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right need">完成认证奖励数目 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="zxbCertification" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right need">发布任务奖励文本 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="zxbReleaseTheTask_text" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right need">发布任务奖励数目 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="zxbReleaseTheTask" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right need">托管奖励文本 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="zxbHosting_text" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right need">托管奖励数目 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="zxbHosting" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right need">邀请奖励文本 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="zxbShare_text" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right need">邀请奖励数目 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="zxbShare" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right need">评价奖励文本 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="zxbReview_text" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right need">评价奖励数目 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="zxbReview" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">我的奖励说明 </label>

                    <div class="col-sm-9">
                        <script id="zxbNote" name="zxbNote" type="text/plain">
       
                        </script>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">网站名称 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="txtSiteName" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">网站描述 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="txtSiteDescription" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">客服联系方式 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="txtServicePhone" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="20"></asp:textbox>
                        <label class="col-sm-2 control-label no-padding-right">多个可以用";"分隔 </label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">客服标语 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="txtServiceNote" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="15"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">APP二维码 </label>
                    <div class="col-sm-9">
                        <input id="imgAPPImage" name="id-input-file" type="file" onchange="uploadHeaderImg('imgAPPImage')" />
                        <div class="form-group" id="dvimgAPPImage">
                            <div class="col-sm-9">
                                <img alt="" id="imgAPPImagePic" src="" style="width: 150px; height: 150px;" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">IOS APP下载地址 </label>

                    <div class="col-sm-9">
                        <asp:textbox id="txtIOSAPPURL" runat="server" cssclass="col-xs-10 col-sm-5" maxlength="200"></asp:textbox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">公众号二维码 </label>
                    <div class="col-sm-9">
                        <input id="imgGZImage" name="id-input-file" type="file" onchange="uploadHeaderImg('imgGZImage')" />
                        <div class="form-group" id="dvimgGZImage">
                            <div class="col-sm-9">
                                <img alt="" id="imgGZImagePic" src="" style="width: 150px; height: 150px;" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">首页滚动图片1 </label>
                    <div class="col-sm-9">
                        <input id="imgHead1" name="id-input-file" type="file" onchange="uploadHeaderImg('imgHead1')" />
                        <div class="form-group" id="dvimgHead">
                            <div class="col-sm-9">
                                <img alt="" id="imgHead1Pic" src="" style="width: 150px; height: 150px;" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">首页滚动图片2 </label>
                    <div class="col-sm-9">
                        <input id="imgHead2" name="id-input-file" type="file" onchange="uploadHeaderImg('imgHead2')" />
                        <div class="form-group" id="dvimgHead2">
                            <div class="col-sm-9">
                                <img alt="" id="imgHead2Pic" src="" style="width: 150px; height: 150px;" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">首页滚动图片3 </label>
                    <div class="col-sm-9">
                        <input id="imgHead3" name="id-input-file" type="file" onchange="uploadHeaderImg('imgHead3')" />
                        <div class="form-group" id="dvimgHead3">
                            <div class="col-sm-9">
                                <img alt="" id="imgHead3Pic" src="" style="width: 150px; height: 150px;" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">首页滚动图片4 </label>
                    <div class="col-sm-9">
                        <input id="imgHead4" name="id-input-file" type="file" onchange="uploadHeaderImg('imgHead4')" />
                        <div class="form-group" id="dvimgHead4">
                            <div class="col-sm-9">
                                <img alt="" id="imgHead4Pic" src="" style="width: 150px; height: 150px;" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">首页滚动图片5 </label>
                    <div class="col-sm-9">
                        <input id="imgHead5" name="id-input-file" type="file" onchange="uploadHeaderImg('imgHead5')" />
                        <div class="form-group" id="dvimgHead5">
                            <div class="col-sm-9">
                                <img alt="" id="imgHead5Pic" src="" style="width: 150px; height: 150px;" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group" style="display:none;">
                    <label class="col-sm-2 control-label no-padding-right need">雇佣合同内容 </label>

                    <div class="col-sm-9">
                        <script id="container" name="content" type="text/plain">
       
                        </script>
                    </div>
                </div>
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
    <asp:hiddenfield id="hidConfigId" runat="server" />
</asp:Content>
