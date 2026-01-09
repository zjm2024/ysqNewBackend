<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="AgencyCreateEdit.aspx.cs" Inherits="SPlatformService.CustomerManagement.AgencyCreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/AgencyCreateEditJS.js"></script>
    <!-- 配置文件 -->
    <script type="text/javascript" src="../Scripts/UEditor/ueditor.config.js"></script>
    <!-- 编辑器源码文件 -->
    <script type="text/javascript" src="../Scripts/UEditor/ueditor.all.js"></script>
    <script type="text/javascript" charset="utf-8" src="../Scripts/UEditor/lang/zh-cn/zh-cn.js"></script>
    <!-- 实例化编辑器 -->
    <script type="text/javascript">
        var ue = UE.getEditor('container');
    </script>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">居住区域 </label>

                    <div class="col-sm-9">
                        <asp:DropDownList ID="drpProvince" runat="server" CssClass="col-xs-10 col-sm-2">
                        </asp:DropDownList>
                        <asp:DropDownList ID="drpCity" runat="server" CssClass="col-xs-10 col-sm-2">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">擅长行业 </label>

                    <div class="col-sm-9">
                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                            <button class="wtbtn yjright" type="button" id="btn_newagencycategory" onclick="return NewAgencyCategory();" title="添加">
                                <i class="icon-ok bigger-110"></i>
                                添加
                            </button>
                            <button class="wtbtn yjright" type="button" id="btn_deleteagencycategory" onclick="return DeleteAgencyCategory();" title="删除">
                                <i class="icon-ok bigger-110"></i>
                                删除
                            </button>
                            <div class="space-4"></div>
                            <table id="AgencyCategoryList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                <thead>
                                    <tr class="ui-jqgrid-labels">
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                            <div class="" title="">
                                                <input class="cbox" type="checkbox" onchange="checkAll(this, 'AgencyCategoryList')" />
                                            </div>
                                        </th>
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                            <div class="ui-jqgrid-sortable" title="行业大类">行业大类</div>
                                        </th>
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                            <div class="ui-jqgrid-sortable" title="行业小类">行业小类</div>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">我的优势区域 </label>

                    <div class="col-sm-9">
                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                            <button class="wtbtn yjright" type="button" id="btn_newagencycity" onclick="return NewAgencyCity();" title="添加">
                                <i class="icon-ok bigger-110"></i>
                                添加
                            </button>
                            <button class="wtbtn yjright" type="button" id="btn_deleteagencycity" onclick="return DeleteAgencyCity();" title="删除">
                                <i class="icon-ok bigger-110"></i>
                                删除
                            </button>
                            <div class="space-4"></div>
                            <table id="AgencyCityList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                <thead>
                                    <tr class="ui-jqgrid-labels">
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                            <div class="" title="">
                                                <input class="cbox" type="checkbox" onchange="checkAll(this, 'AgencyCityList')" />
                                            </div>
                                        </th>
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                            <div class="ui-jqgrid-sortable" title="省（直辖市）">省（直辖市）</div>
                                        </th>
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                            <div class="ui-jqgrid-sortable" title="城市">城市</div>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">销售名称 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtAgencyName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="20"></asp:TextBox>
                    </div>
                </div>
                 <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">兴趣特长 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtSpecialty" runat="server" TextMode="MultiLine" CssClass="col-xs-10 col-sm-5" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">性格特征 </label>

                    <div class="col-sm-9">
                       <asp:TextBox ID="txtFeature" runat="server" TextMode="MultiLine" CssClass="col-xs-10 col-sm-5" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">一句话简介 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtShortDescription" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">工作单位 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCompany" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">职位 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPosition" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="45"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">个人名片 </label>

                    <div class="col-sm-9">
                        <input id="imgPersonalCard" name="id-input-file" type="file" onchange="changeOne('imgPersonalCard','divPersonalCard')" />
                        <div id="divPersonalCardImg" runat="server">
                            <%--<ul class="ace-thumbnails clearfix" id="divPersonalCard">
                            </ul>--%>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">简历 </label>

                    <div class="col-sm-9">
                        <script id="container" name="content" type="text/plain">
       
                        </script>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">身份证 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtIDCard" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="30"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">身份证照片(正反面) </label>

                    <div class="col-sm-9">
                        <ul class="ace-thumbnails clearfix" id="divIdCard">
                        </ul>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right need">技能 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtTechnical" runat="server" TextMode="MultiLine" CssClass="col-xs-10 col-sm-5" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label class="col-sm-2 control-label no-padding-right need">技能证明 </label>

                    <div class="col-sm-9">
                        <ul class="ace-thumbnails clearfix" id="divTechnical">
                        </ul>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">销售级别 </label>

                    <div class="col-sm-9">
                        <asp:DropDownList ID="drpAgencyLevel" runat="server" CssClass="col-xs-10 col-sm-2">
                            <asp:ListItem Text="金牌销售" Value="1"></asp:ListItem>
                            <asp:ListItem Text="银牌销售" Value="2"></asp:ListItem>
                            <asp:ListItem Text="铜牌销售" Value="3"></asp:ListItem>
                            <asp:ListItem Text="普通销售" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">人脉（客户）资源 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtContactsResources" runat="server" TextMode="MultiLine" CssClass="col-xs-10 col-sm-5" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">擅长产品 </label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtFamiliarProduct" runat="server" TextMode="MultiLine" CssClass="col-xs-10 col-sm-5" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                      <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">毕业院校 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtSchool" runat="server" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">其他 </label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtFamiliarClient" runat="server" TextMode="MultiLine" CssClass="col-xs-10 col-sm-5" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">优势客户 </label>

                    <div class="col-sm-9">
                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                            <button class="wtbtn yjright" type="button" id="btn_newagencysuperclient" onclick="return NewAgencySuperClient();" title="添加">
                                <i class="icon-ok bigger-110"></i>
                                添加
                            </button>
                            <button class="wtbtn yjright" type="button" id="btn_deleteagencysuperclient" onclick="return DeleteAgencySuperClient();" title="删除">
                                <i class="icon-ok bigger-110"></i>
                                删除
                            </button>
                            <div class="space-4"></div>
                            <table id="AgencySuperClientList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                <thead>
                                    <tr class="ui-jqgrid-labels">
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                            <div class="" title="">
                                                <input class="cbox" type="checkbox" onchange="checkAll(this, 'AgencySuperClientList')" />
                                            </div>
                                        </th>
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                            <div class="ui-jqgrid-sortable" title="客户名称">客户名称</div>
                                        </th>
                                         <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 300px;">
                                                <div class="ui-jqgrid-sortable" title="具体优势">具体优势</div>
                                            </th>
                                    <%--    <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                            <div class="ui-jqgrid-sortable" title="联系方式">联系方式</div>
                                        </th>--%>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                        <%--<td style="" title=""></td>--%>
                                        <td style="" title=""></td>
                                        <%--<td style="" title=""></td>--%>
                                        <td style="" title=""></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">典型案例 </label>

                    <div class="col-sm-9">
                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                            <button class="wtbtn yjright" type="button" id="btn_newagencysolution" onclick="return NewAgencySolution();" title="添加">
                                <i class="icon-ok bigger-110"></i>
                                添加
                            </button>
                            <button class="wtbtn yjright" type="button" id="btn_deleteagencysolution" onclick="return DeleteAgencySolution();" title="删除">
                                <i class="icon-ok bigger-110"></i>
                                删除
                            </button>
                            <div class="space-4"></div>
                            <table id="AgencySolutionList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                <thead>
                                    <tr class="ui-jqgrid-labels">
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                            <div class="" title="">
                                                <input class="cbox" type="checkbox" onchange="checkAll(this, 'AgencySolutionList')" />
                                            </div>
                                        </th>
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                            <div class="ui-jqgrid-sortable" title="客户名称">客户名称</div>
                                        </th>
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                            <div class="ui-jqgrid-sortable" title="合同名称">合同名称</div>
                                        </th>
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                            <div class="ui-jqgrid-sortable" title="合同金额">合同金额</div>
                                        </th>
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                            <div class="ui-jqgrid-sortable" title="案例证明">案例证明</div>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                        <%--<td style="" title=""></td>--%>
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                        <td style="" title=""></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">身份状态 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtStatus" runat="server" CssClass="col-xs-5 col-sm-3" Enabled="false"></asp:TextBox>
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
                        <button class="wtbtn savebtn" type="button" id="btn_save" title="保存">
                            保存
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_updateagencystatus" title="取消实名认证">
                            取消认证
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_updateagencystatuscommit" title="实名认证通过">
                            认证通过
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_updateagencystatusreject" title="实名认证驳回">
                            认证驳回
                        </button>
                        <button class="wtbtn cancelbtn" type="button" id="btn_cancel" title="返回">
                            返回
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidAgencyId" runat="server" />
</asp:Content>
