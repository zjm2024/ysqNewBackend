<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="AgencyExperienceCreateEdit.aspx.cs" Inherits="WebUI.CustomerManagement.AgencyExperienceCreateEdit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/AgencyExperienceCreateEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">合同名称 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">成交时间（合同签订时间） </label>

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
                    <label class="col-sm-2 control-label no-padding-right">案例缩略图（合同封面） </label>
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
                    <label class="col-sm-2 control-label no-padding-right">案例细节图1（合同金额页） </label>

                    <div class="col-sm-9">

                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                            <input id="fileAdd" name="id-input-file" type="file" onchange="changefile('fileAdd')" />
                            <button class="wtbtn yjright" type="button" id="btn_deletefile" onclick="return DeleteFile();" title="删除">
                                <i class="icon-ok bigger-110"></i>
                                删除
                            </button>
                            <div class="space-4"></div>
                            <table id="FileList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                <thead>
                                    <tr class="ui-jqgrid-labels">
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                            <div class="" title="">
                                                <%--<input class="cbox" type="checkbox" onchange="checkAll(this, 'UserProjectList')" />--%>
                                            </div>
                                        </th>
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 100px;">
                                            <div class="ui-jqgrid-sortable" title="文件名称">文件名称</div>
                                        </th>

                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 100px;">
                                            <div class="ui-jqgrid-sortable" title="上传日期">上传日期</div>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>

                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">案例细节图2（合同签字页） </label>

                    <div class="col-sm-9">
                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                            <input id="fileAdd2" name="id-input-file" type="file" onchange="changefile('fileAdd2')" />
                            <button class="wtbtn yjright" type="button" id="btn_deletefile2" onclick="return DeleteFile2();" title="删除">
                                <i class="icon-ok bigger-110"></i>
                                删除
                            </button>
                            <div class="space-4"></div>
                            <table id="FileList2" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                <thead>
                                    <tr class="ui-jqgrid-labels">
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                            <div class="" title="">
                                                <%--<input class="cbox" type="checkbox" onchange="checkAll(this, 'UserProjectList')" />--%>
                                            </div>
                                        </th>
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 100px;">
                                            <div class="ui-jqgrid-sortable" title="文件名称">文件名称</div>
                                        </th>

                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 100px;">
                                            <div class="ui-jqgrid-sortable" title="上传日期">上传日期</div>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                        <%--     <input id="imgDetail2" name="id-input-file" type="file" onchange="change('imgDetail2','divDetail2')" />
                        <div>
                            <ul class="ace-thumbnails clearfix" id="divDetail2">
                            </ul>
                        </div>--%>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right ">描述（简单说明自己在成交订单合同中的价值和作用） </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="description-textarea" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">客户名称 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtClientName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">合同金额 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtContractAmount" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="divApprove" style="display: none;">
                    <label class="col-sm-2 control-label no-padding-right">驳回原因 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtApproveComment" runat="server" TextMode="MultiLine" CssClass="description-textarea" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix form-actions">
                    <div class="col-sm-5" style="text-align: center;">
                        <button class="wtbtn savebtn" type="button" id="btn_submit" style="display: none;" title="提交">
                            提交
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_save" style="display: none;" title="保存">
                            保存
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_delete" style="display: none;" title="删除">
                            删除
                        </button>
                        <asp:Label ID="lblNotice" class="col-sm-6 control-label" runat="server" Style="display: none;">已提交申请，请等待审核！ </asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidAgencyExperienceId" runat="server" />
</asp:Content>
