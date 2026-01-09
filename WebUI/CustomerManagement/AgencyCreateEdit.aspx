<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="AgencyCreateEdit.aspx.cs" Inherits="WebUI.CustomerManagement.AgencyCreateEdit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/AgencyCreateEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
    <!-- 配置文件 -->
    <script type="text/javascript" src="../Scripts/UEditor/ueditor.config.js"></script>
    <!-- 编辑器源码文件 -->
    <script type="text/javascript" src="../Scripts/UEditor/ueditor.all.js"></script>
    <script type="text/javascript" charset="utf-8" src="../Scripts/UEditor/lang/zh-cn/zh-cn.js"></script>
    <!-- 实例化编辑器 -->
    <script type="text/javascript">
        var ue3 = UE.getEditor('container3');
		ue3.ready(function () {
			this.setHeight(300);
		});
        var pCount = <%=ProjectCount%>;
    </script>
    <div class="newform" <%if (page != "AgencyInfo") {%>style="display:none"<%} %>>
    	<div class="newform_caption">
        	<div><span>资料完成度：<font id="Completed">0%</font></span><span>认证状态：<font id="Status">未认证</font></span><span>实名认证：(<font class="RealNameStatus">未认证</font>) <a href="AgencyCreateEdit.aspx?page=RealName" class="link">前往认证</a></span></div>
        	<div>认证说明: <font class="newform_red">填写所有必填项</font>，认证自动通过。个人身份证仅用于审核认证,不会向第三方透露,请放心上传！</div>
        </div>
        <div class="newform_title">
            <span>销售详情</span>
        </div>
        <div class="newform_list">
            <ul>
                <li>
                    <div class="newform_left">销售姓名（<font class="newform_red">必填</font>）</div>
                    <div class="newform_content">
                        <div class="newform_text" id="AgencyName" onclick="Newform_alert('AgencyName_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('AgencyName_A')">设置</div>
                        <div class="newformalert_content" id="AgencyName_A">
                        	<div class="close" onclick="Newform_close('AgencyName_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">销售姓名</div>
                                <div class="newformalert_input">
                                	<p>请填写您的名称</p>
                                	<input name="AgencyName" value=""/>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('AgencyName')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">销售头像（<font class="newform_red">必填</font>）</div>
                    <div class="newform_content haveimg">
                        <div class="newform_text" id="PersonalCard" onclick="Newform_alert('PersonalCard_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('PersonalCard_A')">设置</div>
                        <div class="newformalert_content" id="PersonalCard_A">
                        	<div class="close" onclick="Newform_close('PersonalCard_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">销售头像</div>
                                <div class="newformalert_upimg">
                                	<div class="newformalert_upimg_text">
                                    	<p>图片不允许涉及政治敏感与色情;</p>
										<p>图片格式必须为：png,bmp,jpeg,jpg,gif；不可大于10M；建议使用png格式图片，以保持最佳效果；建议图片尺寸为200px*200px</p>
                                    </div>
                                    <input class="newformalert_upimg_input" id="imgPersonalCard" name="id-input-file" type="file" onchange="change2('imgPersonalCard')" />
                                    <div class="newformalert_upimg_img" id="imgPersonalCardPic" data="" style="background-image:url(../Style/images/upimg.png)" onclick="$('#imgPersonalCard').click()"></div>
                                    <div class="newformalert_btn" onclick="savebtn('PersonalCard')">确定</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">居住区域</div>
                    <div class="newform_content">
                        <div class="newform_text" id="CityName" onclick="Newform_alert('CityName_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('CityName_A')">设置</div>
                        <div class="newformalert_content" id="CityName_A">
                        	<div class="close" onclick="Newform_close('CityName_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">选择区域</div>
                                <div class="newformalert_input newformalert_CityName">
                                	<p>请选择您现在所居住的区域</p>
                                    <select name="drpProvince"></select>
                                    <select name="drpCity"></select>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('CityName')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">擅长行业</div>
                    <div class="newform_content">
                        <div class="newform_text" id="AgencyCategory" onclick="Newform_alert('AgencyCategory_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('AgencyCategory_A')">设置</div>
                        <div class="newformalert_content" id="AgencyCategory_A">
                        	<div class="close" onclick="Newform_close('AgencyCategory_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">擅长行业</div>
                                <button class="wtbtn yjright" type="button" id="btn_newagencycategory" onclick="return NewAgencyCategory();" title="添加">
                                    <i class="icon-ok bigger-110"></i>
                                    添加
                                </button>
                                <button class="wtbtn yjright" type="button" id="btn_deleteagencycategory" onclick="return DeleteAgencyCategory();" title="删除">
                                    <i class="icon-ok bigger-110"></i>
                                    删除
                                </button>
                                <div class="newformalert_duo">
                                	<div class="ui-jqgrid-bdiv">       
                                        <div class="space-4"></div>
                                        <div id="divAgencyCategoryList" runat="server">
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
                                <div class="newformalert_btn" onclick="savebtn('AgencyCategory')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">优势区域</div>
                    <div class="newform_content">
                        <div class="newform_text" id="TCityName" onclick="Newform_alert('TCityName_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('TCityName_A')">设置</div>
                        <div class="newformalert_content" id="TCityName_A">
                        	<div class="close" onclick="Newform_close('TCityName_A')"></div>	
                            <div class="newformalert_content_text">
                            	<div class="newformalert_title">我的优势区域</div>
                            	<!--弹窗内容-->
                                <button class="wtbtn yjright" type="button" id="btn_newagencycity" onclick="return NewAgencyCity();" title="添加">
                                    <i class="icon-ok bigger-110"></i>
                                    添加
                                </button>
                                <button class="wtbtn yjright" type="button" id="btn_deleteagencycity" onclick="return DeleteAgencyCity();" title="删除">
                                    <i class="icon-ok bigger-110"></i>
                                    删除
                                </button>
                                <div class="newformalert_duo">
                                    <div class="ui-jqgrid-bdiv">
                                        <div class="space-4"></div>
                                        <div id="divAgencyCityList" runat="server">
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
                                <div class="newformalert_btn" onclick="savebtn('TCityName')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">毕业院校</div>
                    <div class="newform_content">
                        <div class="newform_text" id="School" onclick="Newform_alert('School_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('School_A')">设置</div>
                        <div class="newformalert_content" id="School_A">
                        	<div class="close" onclick="Newform_close('School_A')"></div>	
                            <div class="newformalert_content_text">
                                <!--弹窗内容-->
                                <div class="newformalert_title">毕业院校</div>
                                <div class="newformalert_input">
                                	<p>请填写您的毕业院校</p>
                                	<input name="School" value=""/>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('School')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">擅长产品（<font class="newform_red">必填</font>）</div>
                    <div class="newform_content">
                        <div class="newform_text" id="FamiliarProduct" onclick="Newform_alert('FamiliarProduct_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('FamiliarProduct_A')">设置</div>
                        <div class="newformalert_content" id="FamiliarProduct_A">
                        	<div class="close" onclick="Newform_close('FamiliarProduct_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">擅长产品</div>
                                <div class="newformalert_textarea">
                                	<p>请填写你擅长销售的产品</p>
                                    <textarea name="FamiliarProduct"></textarea>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('FamiliarProduct')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">优势客户</div>
                    <div class="newform_content">
                        <div class="newform_text" id="newagencysuperclient" onclick="Newform_alert('newagencysuperclient_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('newagencysuperclient_A')">设置</div>
                        <div class="newformalert_content newformalert_ueditor" id="newagencysuperclient_A">
                        	<div class="close" onclick="Newform_close('newagencysuperclient_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">我的优势客户</div>
                                <button class="wtbtn yjright" type="button" id="btn_newagencysuperclient" onclick="return NewAgencySuperClient();" title="添加">
                                    <i class="icon-ok bigger-110"></i>
                                    添加
                                </button>
                                <button class="wtbtn yjright" type="button" id="btn_deleteagencysuperclient" onclick="return DeleteAgencySuperClient();" title="删除">
                                    <i class="icon-ok bigger-110"></i>
                                    删除
                                </button>
                                <div class="newformalert_duo">
                                    <div class="ui-jqgrid-bdiv">
                                    <div class="space-4"></div>
                                    <div id="divAgencySuperClientList" runat="server">
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
                                                        <%--比如曾跟踪某个项目虽然跟丢但是跟决策人很熟--%>
                                                    </th>
                                                    <%--       <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                        <div class="ui-jqgrid-sortable" title="联系方式">联系方式</div>
                                                    </th>--%>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                                    <td style="" title=""></td>
                                                    <td style="" title=""></td>
                                                    <%-- <td style="" title=""></td>--%>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                 </div>
                               </div>
                               <div class="newformalert_btn" onclick="savebtn('newagencysuperclient')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">典型案例</div>
                    <div class="newform_content">
                        <div class="newform_text" id="newagencysolution" onclick="Newform_alert('newagencysolution_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('newagencysolution_A')">设置</div>
                        <div class="newformalert_content newformalert_ueditor" id="newagencysolution_A">
                        	<div class="close" onclick="Newform_close('newagencysolution_A')"></div>	
                            <div class="newformalert_content_text">
                            	<div class="newformalert_title">典型案例</div>
                                <button class="wtbtn yjright" type="button" id="btn_newagencysolution" onclick="return NewAgencySolution();" title="添加">
                                     <i class="icon-ok bigger-110"></i>
                                     添加
                                </button>
                                <button class="wtbtn yjright" type="button" id="btn_deleteagencysolution" onclick="return DeleteAgencySolution();" title="删除">
                                     <i class="icon-ok bigger-110"></i>
                                     删除
                                </button>
                                <button class="wtbtn yjright" type="button" id="btn_myagencysolution" onclick="return myagencysolution();" title="自动关联我的销售案例">
                                     <i class="icon-ok bigger-110"></i>
                                     自动关联我的销售案例
                                </button>
                            	<!--弹窗内容-->
                                <div class="newformalert_duo">
                                    <div class="ui-jqgrid-bdiv">
                                        <div class="space-4"></div>
                                        <div id="divAgencySolutionList" runat="server">
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
                                                            <div class="ui-jqgrid-sortable" title="成交时间">成交时间</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="案例证明">案例证明</div>
                                                        </th>
                                                        <th class="ui-state-default ui-th-column ui-th-ltr" style="width: 150px;">
                                                            <div class="ui-jqgrid-sortable" title="隐私设置">隐私设置</div>
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr class="ui-widget-content jqgrow ui-row-ltr" style="display: none;">
                                                        <td style="" title=""></td>
                                                        <td style="" title=""></td>
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
                                <div class="newformalert_btn" onclick="savebtn('newagencysolution')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">兴趣特长</div>
                    <div class="newform_content">
                        <div class="newform_text" id="Specialty" onclick="Newform_alert('Specialty_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('Specialty_A')">设置</div>
                        <div class="newformalert_content" id="Specialty_A">
                        	<div class="close" onclick="Newform_close('Specialty_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">兴趣特长</div>
                                <div class="newformalert_textarea">
                                	<p>请填写你的兴趣特长</p>
                                    <textarea name="Specialty"></textarea>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('Specialty')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">性格特征</div>
                    <div class="newform_content">
                        <div class="newform_text" id="Feature" onclick="Newform_alert('Feature_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('Feature_A')">设置</div>
                        <div class="newformalert_content" id="Feature_A">
                        	<div class="close" onclick="Newform_close('Feature_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">性格特征</div>
                                <div class="newformalert_textarea">
                                	<p>请填写你的性格特征</p>
                                    <textarea name="Feature"></textarea>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('Feature')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">工作单位</div>
                    <div class="newform_content">
                        <div class="newform_text" id="Company" onclick="Newform_alert('Company_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('Company_A')">设置</div>
                        <div class="newformalert_content" id="Company_A">
                        	<div class="close" onclick="Newform_close('Company_A')"></div>	
                            <div class="newformalert_content_text">
                                <!--弹窗内容-->
                                <div class="newformalert_title">工作单位</div>
                                <div class="newformalert_input">
                                	<p>请填写您的工作单位</p>
                                	<input name="Company" value=""/>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('Company')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">职位</div>
                    <div class="newform_content">
                        <div class="newform_text" id="Position" onclick="Newform_alert('Position_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('Position_A')">设置</div>
                        <div class="newformalert_content" id="Position_A">
                        	<div class="close" onclick="Newform_close('Position_A')"></div>	
                            <div class="newformalert_content_text">
                                <!--弹窗内容-->
                                <div class="newformalert_title">职位</div>
                                <div class="newformalert_input">
                                	<p>请填写您的职位</p>
                                	<input name="Position" value=""/>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('Position')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                
                <li>
                    <div class="newform_left">简历（<font class="newform_red">必填</font>）</div>
                    <div class="newform_content">
                        <div class="newform_text" id="Description" onclick="Newform_alert('Description_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('Description_A')">设置</div>
                        <div class="newformalert_content newformalert_ueditor" id="Description_A">
                        	<div class="close" onclick="Newform_close('Description_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">个人简历</div>
                                <div class="newformalert_ueditor_text">
                                	<script id="container3" name="content" type="text/plain" style="height:300px"></script>
									<asp:HiddenField ID="txtDescription" runat="server" />
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('Description')">保存</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">一句话简介</div>
                    <div class="newform_content">
                        <div class="newform_text" id="ShortDescription" onclick="Newform_alert('ShortDescription_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn" onclick="Newform_alert('ShortDescription_A')">设置</div>
                        <div class="newformalert_content" id="ShortDescription_A">
                        	<div class="close" onclick="Newform_close('ShortDescription_A')"></div>	
                            <div class="newformalert_content_text">
                                <!--弹窗内容-->
                                <div class="newformalert_title">一句话简介</div>
                                <div class="newformalert_input">
                                	<p>请用一句话介绍你自己</p>
                                	<input name="ShortDescription" value=""/>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('ShortDescription')">确定</div>
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
        	<div>认证说明: 个人身份证仅用于审核认证,不会向第三方透露,请放心上传！</div>
        </div>
        <div class="newform_title">
            <span>实名认证</span>
        </div>
        <div class="newform_list">
            <ul>
            	<li>
                    <div class="newform_left">身份证号码</div>
                    <div class="newform_content">
                        <div class="newform_text" id="IDCard" onclick="Newform_alert('IDCard_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn RealNamebtn" onclick="Newform_alert('IDCard_A')">设置</div>
                        <div class="newformalert_content" id="IDCard_A">
                        	<div class="close" onclick="Newform_close('IDCard_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
                                <div class="newformalert_title">身份证号码</div>
                                <div class="newformalert_input">
                                	<p>请填写你的身份证号码</p>
                                	<input name="IDCard" value=""/>
                                </div>
                                <div class="newformalert_btn" onclick="savebtn('IDCard')">确定</div>
                            </div>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="newform_left">身份证照片（<font class="newform_red">必填</font>）</div>
                    <div class="newform_content haveimg">
                        <div class="newform_text" id="Card" onclick="Newform_alert('Card_A')"><font class="red">(未设置)</font></div>
                        <div class="newform_btn RealNamebtn" onclick="Newform_alert('Card_A')">设置</div>
                        <div class="newformalert_content" id="Card_A">
                        	<div class="close" onclick="Newform_close('Card_A')"></div>	
                            <div class="newformalert_content_text">
                            	<!--弹窗内容-->
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
