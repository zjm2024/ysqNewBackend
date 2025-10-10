<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="BusinessCreateEdit.aspx.cs" Inherits="SPlatformService.CustomerManagement.BusinessCreateEdit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
     <script type="text/javascript" src="../Scripts/CustomerManagement/BusinessCreateEditJS.js"></script> 
    <!-- 配置文件 -->
    <script type="text/javascript" src="../Scripts/UEditor/ueditor.config.js"></script>
    <!-- 编辑器源码文件 -->
    <script type="text/javascript" src="../Scripts/UEditor/ueditor.all.js"></script>
    <script type="text/javascript" charset="utf-8" src="../Scripts/UEditor/lang/zh-cn/zh-cn.js"></script>
    <!-- 实例化编辑器 -->
    <script type="text/javascript">
        var ue = UE.getEditor('container');
        var ue2 = UE.getEditor('container2');
    </script>  
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">公司所在区域 </label>

                    <div class="col-sm-9">
                        <asp:DropDownList ID="drpProvince" runat="server" CssClass="col-xs-10 col-sm-2">
                        </asp:DropDownList>
                        <asp:DropDownList ID="drpCity" runat="server" CssClass="col-xs-10 col-sm-2">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">公司所属行业 </label>

                    <div class="col-sm-9">
                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                            <button class="wtbtn yjright" type="button" id="btn_newbusinesscategory" onclick="return NewBusinessCategory();" title="添加">
                                <i class="icon-ok bigger-110"></i>
                                添加
                            </button>
                            <button class="wtbtn yjright" type="button" id="btn_deletebusinesscategory" onclick="return DeleteBusinessCategory();" title="删除">
                                <i class="icon-ok bigger-110"></i>
                                删除
                            </button>
                            <div class="space-4"></div>
                            <table id="BusinessCategoryList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                <thead>
                                    <tr class="ui-jqgrid-labels">
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                            <div class="" title="">
                                                <input class="cbox" type="checkbox" onchange="checkAll(this, 'BusinessCategoryList')" />
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
                    <label class="col-sm-2 control-label no-padding-right need">雇主名称 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCompanyName" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="30"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">办公地址 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="200"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">公司网站 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCompanySite" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="200"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">公司电话 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCompanyTel" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="20"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">雇主简介 </label>

                    <div class="col-sm-9">
                         <asp:TextBox ID="Description" runat="server" TextMode="MultiLine" CssClass="col-xs-10 col-sm-5" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">目标客户区域 </label>

                    <div class="col-sm-9">
                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                            <button class="wtbtn yjright" type="button" id="btn_newtargetcity" onclick="return NewTargetCity();" title="添加">
                                <i class="icon-ok bigger-110"></i>
                                添加
                            </button>
                            <button class="wtbtn yjright" type="button" id="btn_deletetargetcity" onclick="return DeleteTargetCity();" title="删除">
                                <i class="icon-ok bigger-110"></i>
                                删除
                            </button>
                            <div class="space-4"></div>
                            <table id="TargetCityList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                <thead>
                                    <tr class="ui-jqgrid-labels">
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                            <div class="" title="">
                                                <input class="cbox" type="checkbox" onchange="checkAll(this, 'TargetCityList')" />
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
                    <label class="col-sm-2 control-label no-padding-right">目标客户行业 </label>

                    <div class="col-sm-9">
                        <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                            <button class="wtbtn yjright" type="button" id="btn_newtargetcategory" onclick="return NewTargetCategory();" title="添加">
                                <i class="icon-ok bigger-110"></i>
                                添加
                            </button>
                            <button class="wtbtn yjright" type="button" id="btn_deletetargetcategory" onclick="return DeleteTargetCategory();" title="删除">
                                <i class="icon-ok bigger-110"></i>
                                删除
                            </button>
                            <div class="space-4"></div>
                            <table id="TargetCategoryList" class="table table-striped table-bordered table-hover dataTable ui-jqgrid-btable" width="100%" cellspacing="0" cellpadding="0" border="0">
                                <thead>
                                    <tr class="ui-jqgrid-labels">
                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 30px;">
                                            <div class="" title="">
                                                <input class="cbox" type="checkbox" onchange="checkAll(this, 'TargetCategoryList')" />
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
                    <label class="col-sm-2 control-label no-padding-right need">成立日期 </label>

                    <div class="col-sm-3">
                        <div class="input-group col-sm-9">
                            <asp:TextBox ID="txtSetupDate" runat="server" CssClass="form-control date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:TextBox>
                            <span class="input-group-addon">
                                <i class="fa fa-calendar bigger-110"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">企业性质 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCompanyType" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">企业简介 </label>

                    <div class="col-sm-9">
                        <script id="container2" name="content" type="text/plain">
                        
                        </script>

                    </div>
                </div>
                 <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">产品介绍 </label>

                    <div class="col-sm-9">
                        <script id="container" name="content" type="text/plain">
                        
                        </script>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">公司介绍 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtCompanyDescription" runat="server" TextMode="MultiLine" CssClass="col-xs-10 col-sm-5" Style="height: 200px; width: 420px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px;" MaxLength="400"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right need">营业执照号 </label>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtBusinessLicense" runat="server" CssClass="col-xs-10 col-sm-5" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">营业执照 </label>

                    <div class="col-sm-9">
                        <span class="profile-picture">
                            <a id="linkBusinessLicense" href="#" target="_blank"><img id="imgBusinessLicense" src="../Style/images/logo.png" style="width: 150px"></img></a>
                        </span>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">公司LOGO </label>

                    <div class="col-sm-9">
                        <span class="profile-picture">
                            <img id="imgCompanyLogo" src="../Style/images/logo.png" style="width: 150px"></img>
                        </span>
                    </div>
                </div>


                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">授权委托书 </label>

                    <div class="col-sm-9">
                        <span class="profile-picture">
                            <a id="linkEntrust" href="#" target="_blank"><img id="imgEntrust" src="../Style/images/logo.png" style="width: 150px"></img></a>
                        </span>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">身份证照片（正反面） </label>

                    <div class="col-sm-9">
                        <div id="divIDCardImg" runat="server">
                            <ul class="ace-thumbnails clearfix" id="divIdCard">
                            </ul>
                        </div>
                    </div>
                </div>               
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">身份状态 </label>

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
                        <button class="wtbtn savebtn" type="button" id="btn_updatebusinessstatus" title="取消实名认证">
                            取消认证
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_updatebusinessstatuscommit" title="实名认证通过">
                            认证通过
                        </button>
                        <button class="wtbtn savebtn" type="button" id="btn_updatebusinessstatusreject" title="实名认证驳回">
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
    <asp:HiddenField ID="hidBusinessId" runat="server" />
</asp:Content>
