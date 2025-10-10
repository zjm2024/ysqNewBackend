<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="BusinessCreateEdit.aspx.cs" Inherits="WebUI.CustomerManagement.BusinessCreateEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/BusinessCreateEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
    <!-- 配置文件 -->
    <script type="text/javascript" src="../Scripts/UEditor/ueditor.config.js"></script>
    <!-- 编辑器源码文件 -->
    <script type="text/javascript" src="../Scripts/UEditor/ueditor.all.js"></script>
    <script type="text/javascript" charset="utf-8" src="../Scripts/UEditor/lang/zh-cn/zh-cn.js"></script>
    <!-- 实例化编辑器 -->
    <script type="text/javascript">
        var ue = UE.getEditor('container');
		ue.ready(function () {
			this.setHeight(300);
		});  
        var ue2 = UE.getEditor('container2');
		ue2.ready(function () {
			this.setHeight(300);
		});
        var pCount = <%=ProjectCount%>;
    </script>
    <div class="newform" <%if (page != "BusinessInfo") {%>style="display:none"<%} %>>
    	<div class="newform_caption">
        	<div><span>资料完成度：<font id="Completed">0%</font></span><span>资料认证：<font id="Status">未通过</font></span><span>实名认证：(<font class="RealNameStatus">未认证</font>) <a href="BusinessCreateEdit.aspx?page=RealName" class="link">前往认证</a></span></div>
        	<div>认证说明: <font class="newform_red">填写所有必填项</font>，资料认证自动通过并在平台公开展示。</div>
        </div>
        <div class="newform_title">
            <span>雇主详情</span>
        </div>
        <div class="newform_list">
            <ul>
                <li>
                    <div class="newform_left">雇主名称（<font class="newform_red">必填</font>）</div>
                    <div class="newform_content">
                        <div class="newform_text" id="CompanyName"  onclick="Newform_alert('CompanyName_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('CompanyName_A')">设置</div>
                        <div class="newformalert_content" id="CompanyName_A">
                        	<div class="close" onclick="Newform_close('CompanyName_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">雇主名称</div>
                                <div class="newformalert_input">
                                	<p>请填写您的公司名称</p>
                                	<input name="CompanyName" value=""/>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('CompanyName')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">雇主形象图片（<font class="newform_red">必填</font>）</div>
                    <div class="newform_content haveimg">
                        <div class="newform_text" id="CompanyLogo"  onclick="Newform_alert('CompanyLogo_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('CompanyLogo_A')">设置</div>
                        <div class="newformalert_content" id="CompanyLogo_A">
                        	<div class="close" onclick="Newform_close('CompanyLogo_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">雇主形象图片</div>
                                <div class="newformalert_upimg">
                                	<div class="newformalert_upimg_text">
                                    	<p>图片不允许涉及政治敏感与色情;</p>
										<p>图片格式必须为：png,bmp,jpeg,jpg,gif；不可大于10M；建议使用png格式图片，以保持最佳效果；建议图片尺寸为200px*200px</p>
                                    </div>
                                    <input class="newformalert_upimg_input" id="imgCompanyLogo" name="id-input-file" type="file" onchange="change2('imgCompanyLogo')" />
                                    <div class="newformalert_upimg_img" id="imgCompanyLogoPic" data="" style="background-image:url(../Style/images/upimg.png)" onclick="$('#imgCompanyLogo').click()"></div>
                                    <div class="newformalert_btn" onclick="savebtn('CompanyLogo')">确定</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">企业性质</div>
                    <div class="newform_content">
                        <div class="newform_text" id="CompanyType" onclick="Newform_alert('CompanyType_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('CompanyType_A')">设置</div>
                        <div class="newformalert_content" id="CompanyType_A">
                        	<div class="close" onclick="Newform_close('CompanyType_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">企业性质</div>
                                <div class="newformalert_input">
                                	<p>请选择企业性质</p>
                                    <label for="rad1">
                                        <input name="CompanyType" class="ace" value="生产制造型" type="radio" id="rad1" checked="checked"/>
                                        <span class="lbl">生产制造型</span>
                                    </label>
                                    <label for="rad2">
                                        <input name="CompanyType" class="ace" value="贸易型" type="radio" id="rad2"/>
                                        <span class="lbl">贸易型</span>
                                    </label>
                                    <label for="rad3">
                                        <input name="CompanyType" class="ace" value="服务型" type="radio" id="rad3"/>
                                        <span class="lbl">服务型</span>
                                    </label>
                                    <label for="rad4">
                                        <input name="CompanyType" class="ace" value="工程型" type="radio" id="rad4"/>
                                        <span class="lbl">工程型</span>
                                    </label>
                                    <label for="rad5">
                                        <input name="CompanyType" class="ace" value="政府" type="radio" id="rad5"/>
                                        <span class="lbl">政府</span>
                                    </label>
                                    <label for="rad6">
                                        <input name="CompanyType" class="ace" value="个体户" type="radio" id="rad6"/>
                                        <span class="lbl">个体户</span>
                                    </label>
                                    <label for="rad7">
                                        <input name="CompanyType" class="ace" value="个人或其他" type="radio" id="rad7"/>
                                        <span class="lbl">个人或其他</span>
                                    </label>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('CompanyType')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">公司所在区域</div>
                    <div class="newform_content">
                        <div class="newform_text" id="CityName" onclick="Newform_alert('CityName_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('CityName_A')">设置</div>
                        <div class="newformalert_content" id="CityName_A">
                        	<div class="close" onclick="Newform_close('CityName_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">选择区域</div>
                                <div class="newformalert_input newformalert_CityName">
                                	<p>请选择您公司所在区域</p>
                                    <select name="drpProvince"></select>
                                    <select name="drpCity"></select>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('CityName')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">公司所属行业</div>
                    <div class="newform_content">
                        <div class="newform_text" id="CategoryNames" onclick="Newform_alert('CategoryNames_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('CategoryNames_A')">设置</div>
                        <div class="newformalert_content" id="CategoryNames_A">
                        	<div class="close" onclick="Newform_close('CategoryNames_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">行业分类</div>
                                <button class="wtbtn yjright" type="button" id="btn_newbusinesscategory" onclick="return NewBusinessCategory();" title="添加">
                                     <i class="icon-ok bigger-110"></i>
                                     添加
                                </button>
                                <button class="wtbtn yjright" type="button" id="btn_deletebusinesscategory" onclick="return DeleteBusinessCategory();" title="删除">
                                     <i class="icon-ok bigger-110"></i>
                                     删除
                                </button>
                                <div class="newformalert_duo">
                                	<div class="ui-jqgrid-bdiv">       
                                        <div class="space-4"></div>
                                        <div id="divBusinessCategoryList" runat="server">
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
                                <div class="newformalert_btn" onclick="savebtn('CategoryNames')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">办公地址</div>
                    <div class="newform_content">
                        <div class="newform_text" id="Address" onclick="Newform_alert('Address_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('Address_A')">设置</div>
                        <div class="newformalert_content" id="Address_A">
                        	<div class="close" onclick="Newform_close('Address_A')"></div>	
                            <div class="newformalert_content_text">
                                <!--弹窗内容-->
                                <div class="newformalert_title">办公地址</div>
                                <div class="newformalert_input">
                                	<p>请填写公司的办公地址</p>
                                	<input name="Address" value=""/>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('Address')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">公司网站</div>
                    <div class="newform_content">
                        <div class="newform_text" id="CompanySite" onclick="Newform_alert('CompanySite_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('CompanySite_A')">设置</div>
                        <div class="newformalert_content" id="CompanySite_A">
                        	<div class="close" onclick="Newform_close('CompanySite_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">公司网站</div>
                                <div class="newformalert_input">
                                	<p>请填写公司的官方网站</p>
                                	<input name="CompanySite" value=""/>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('CompanySite')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">公司电话</div>
                    <div class="newform_content">
                        <div class="newform_text" id="CompanyTel" onclick="Newform_alert('CompanyTel_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('CompanyTel_A')">设置</div>
                        <div class="newformalert_content" id="CompanyTel_A">
                        	<div class="close" onclick="Newform_close('CompanyTel_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">公司电话</div>
                                <div class="newformalert_input">
                                	<p>请填写公司电话</p>
                                	<input name="CompanyTel" value=""/>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('CompanyTel')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li style="display:none">
                    <div class="newform_left">雇主简介</div>
                    <div class="newform_content">
                        <div class="newform_text" id="Description" onclick="Newform_alert('Description_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('Description_A')">设置</div>
                        <div class="newformalert_content" id="Description_A">
                        	<div class="close" onclick="Newform_close('Description_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">雇主简介</div>
                                <div class="newformalert_textarea">
                                	<p>请输入雇主介绍，请确认介绍内容不含国家相关法律法规禁止内容</p>
                                    <textarea name="Description"></textarea>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('Description')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">目标客户区域</div>
                    <div class="newform_content">
                        <div class="newform_text" id="TCityName" onclick="Newform_alert('TCityName_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('TCityName_A')">设置</div>
                        <div class="newformalert_content" id="TCityName_A">
                        	<div class="close" onclick="Newform_close('TCityName_A')"></div>	
                            <div class="newformalert_content_text">
                            	<div class="newformalert_title">目标客户区域</div>
                            	<!--弹窗内容-->
                                <button class="wtbtn yjright" type="button" id="btn_newtargetcity" onclick="return NewTargetCity();" title="添加">
                                    <i class="icon-ok bigger-110"></i>
                                    添加
                                </button>
                                <button class="wtbtn yjright" type="button" id="btn_deletetargetcity" onclick="return DeleteTargetCity();" title="删除">
                                    <i class="icon-ok bigger-110"></i>
                                    删除
                                </button>
                                <div class="newformalert_duo">
                                    <div class="ui-jqgrid-bdiv">
                                        <div class="space-4"></div>
                                        <div id="divTargetCityList" runat="server">
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
                                <div class="newformalert_btn" onclick="savebtn('TCityName')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">目标客户行业</div>
                    <div class="newform_content">
                        <div class="newform_text" id="TCategoryName" onclick="Newform_alert('TCategoryName_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('TCategoryName_A')">设置</div>
                        <div class="newformalert_content" id="TCategoryName_A">
                        	<div class="close" onclick="Newform_close('TCategoryName_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">目标客户行业</div>
                                <div class="newformalert_duo">
                                    <div class="ui-jqgrid-bdiv">
                                        <button class="wtbtn yjright" type="button" id="btn_newtargetcategory" onclick="return NewTargetCategory();" title="添加">
                                            <i class="icon-ok bigger-110"></i>
                                            添加
                                        </button>
                                        <button class="wtbtn yjright" type="button" id="btn_deletetargetcategory" onclick="return DeleteTargetCategory();" title="删除">
                                            <i class="icon-ok bigger-110"></i>
                                            删除
                                        </button>
                                        <div class="space-4"></div>
                                        <div id="divTargetCategoryList" runat="server">
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
                                <div class="newformalert_btn" onclick="savebtn('TCategoryName')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">雇主简介（<font class="newform_red">必填</font>）<!--原企业简介--></div>
                    <div class="newform_content">
                        <div class="newform_text" id="MainProducts" onclick="Newform_alert('MainProducts_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('MainProducts_A')">设置</div>
                        <div class="newformalert_content newformalert_ueditor" id="MainProducts_A">
                        	<div class="close" onclick="Newform_close('MainProducts_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">企业简介</div>
                                <div class="newformalert_ueditor_text">
                                	<script id="container2" name="content" type="text/plain" style="height:300px"></script>
									<asp:HiddenField ID="txtMainProducts" runat="server" />
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('MainProducts')">保存</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">产品介绍</div>
                    <div class="newform_content">
                        <div class="newform_text" id="ProductDescription" onclick="Newform_alert('ProductDescription_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('ProductDescription_A')">设置</div>
                        <div class="newformalert_content newformalert_ueditor" id="ProductDescription_A">
                        	<div class="close" onclick="Newform_close('ProductDescription_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">产品介绍</div>
                                <div class="newformalert_ueditor_text">
                                	<script id="container" name="content" type="text/plain" style="height:300px"></script>
									<asp:HiddenField ID="txtProductDescription" runat="server" />
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('ProductDescription')">保存</div>
                            </div>
                        </div>
                    </div>
                </li>
            </ul>
        </div>
</div>
<div class="newform" <%if (page != "RealName") {%>style="display:none"<%} %>>
        <div class="newform_caption">
            <div><span>实名认证：<font class="RealNameStatus">未认证</font></span></div>
        	<div>认证说明: 企业营业执照、个人身份证仅用于审核认证,不会向第三方透露,请放心上传！</div>
        </div>
        <div class="newform_title">
            <span>实名认证</span>
        </div>
        <div class="newform_list">
            <ul>
            	<li>
                    <div class="newform_left">营业执照号（<font class="newform_red">必填</font>）</div>
                    <div class="newform_content">
                        <div class="newform_text" id="BusinessLicense" onclick="Newform_alert('BusinessLicense_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn RealNamebtn" onclick="Newform_alert('BusinessLicense_A')">设置</div>
                        <div class="newformalert_content" id="BusinessLicense_A">
                        	<div class="close" onclick="Newform_close('BusinessLicense_A')"></div>	
                            <div class="newformalert_content_text">
                                <div class="newformalert_title">营业执照号</div>
                                <div class="newformalert_input">
                                	<p>请填写营业执照号</p>
                                	<input name="BusinessLicense" value=""/>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('BusinessLicense')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">营业执照（<font class="newform_red">必填</font>）</div>
                    <div class="newform_content haveimg">
                        <div class="newform_text" id="BusinessLicenseImg" onclick="Newform_alert('BusinessLicenseImg_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn RealNamebtn" onclick="Newform_alert('BusinessLicenseImg_A')">设置</div>
                        <div class="newformalert_content" id="BusinessLicenseImg_A">
                        	<div class="close" onclick="Newform_close('BusinessLicenseImg_A')"></div>	
                            <div class="newformalert_content_text">
                                <div class="newformalert_title">营业执照</div>
                                <div class="newformalert_upimg">
                                	<div class="newformalert_upimg_text">
                                    	<p>企业营业执照、个人身份证仅用于审核认证,不会向第三方透露,请放心上传！</p>
										<p>图片格式必须为：png,bmp,jpeg,jpg,gif；不可大于10M；</p>
                                    </div>
                                    <input class="newformalert_upimg_input" id="imgBusinessLicenseImg" name="id-input-file" type="file" onchange="change2('imgBusinessLicenseImg')" />
                                    <div class="newformalert_upimg_img" id="imgBusinessLicenseImgPic" data="" style="background-image:url(../Style/images/upimg.png)" onclick="$('#imgBusinessLicenseImg').click()"></div>
                                    <div class="newformalert_btn" onclick="savebtn('BusinessLicenseImg')">确定</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">授权委托书</div>
                    <div class="newform_content haveimg">
                        <div class="newform_text" id="EntrustImgPath" onclick="Newform_alert('EntrustImgPath_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn RealNamebtn" onclick="Newform_alert('EntrustImgPath_A')">设置</div>
                        <div class="newformalert_content" id="EntrustImgPath_A">
                        	<div class="close" onclick="Newform_close('EntrustImgPath_A')"></div>	
                            <div class="newformalert_content_text">
                                <div class="newformalert_title">授权委托书</div>
                                <div class="newformalert_upimg">
                                	<div class="newformalert_upimg_text">
                                    	<p>请上传您的授权委托书！</p>
										<p>图片格式必须为：png,bmp,jpeg,jpg,gif；不可大于10M；</p>
                                    </div>
                                    <input class="newformalert_upimg_input" id="imgEntrustImgPath" name="id-input-file" type="file" onchange="change2('imgEntrustImgPath')" />
                                    <div class="newformalert_upimg_img" id="imgEntrustImgPathPic" data="" style="background-image:url(../Style/images/upimg.png)" onclick="$('#imgEntrustImgPath').click()"></div>
                                    <div class="newformalert_btn" onclick="savebtn('EntrustImgPath')">确定</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </li>
                <li style="display:none">
                    <div class="newform_left">身份证照片（<font class="newform_red">必填</font>）</div>
                    <div class="newform_content haveimg">
                        <div class="newform_text" id="Card" onclick="Newform_alert('Card_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn RealNamebtn" onclick="Newform_alert('Card_A')">设置</div>
                        <div class="newformalert_content" id="Card_A">
                        	<div class="close" onclick="Newform_close('Card_A')"></div>	
                            <div class="newformalert_content_text">
                                <div class="newformalert_title">身份证照片</div>
                                <div class="newformalert_upimg">
                                	<div class="newformalert_upimg_text">
                                    	<p>个人身份证仅用于审核认证,不会向第三方透露,请放心上传！</p>
										<p>图片格式必须为：png,bmp,jpeg,jpg,gif；不可大于10M；</p>
                                    </div>
                                    <div class="newformalert_upimg_table">
                                        <div class="newformalert_upimg_left">
                                    	    <p>身份证正面</p>
                                    	    <input class="newformalert_upimg_input" id="CardImg" name="id-input-file" type="file" onchange="change2('CardImg')" />
                                    	    <div class="newformalert_upimg_img" id="CardImgPic" data="" style="background-image:url(../Style/images/upimg.png)" onclick="$('#CardImg').click()"></div>
                                        </div>
                                        <div class="newformalert_upimg_right">
                                    	    <p>身份证反面</p>
                                    	    <input class="newformalert_upimg_input" id="CardImg2" name="id-input-file" type="file" onchange="change2('CardImg2')" />
                                    	    <div class="newformalert_upimg_img" id="CardImg2Pic" data="" style="background-image:url(../Style/images/upimg.png)" onclick="$('#CardImg2').click()"></div>
                                        </div>
                                    </div>
                                    <div class="newformalert_btn" onclick="savebtn('Card')">确定</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </li>
            </ul>
            <div class="clearfix form-actions">
                 <div class="col-sm-5" style="text-align: center;">
                      <button class="wtbtn savebtn" type="button" id="btn_RealNameStatus" title="提交审核" runat="server">
                          提交审核
                      </button>
                 </div>
            </div>
        </div>
    </div>
</asp:Content>
