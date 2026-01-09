<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="GenerateProject.aspx.cs" Inherits="WebUI.ProjectManagement.GenerateProject" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/ProjectManagement/GenerateProjectJS.js"></script>
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
    <style>.page-content{ background:none}</style>
    <div class="lg_Generate">
        <div class="lg_Generate_left">
             <div class="lg_Generate_left_title">合同参数</div>
             <div class="G_TextBox">
                  <label class="G_label">项目名称 </label>
                  <asp:TextBox ID="txtProjectName" runat="server" CssClass="" MaxLength="50"></asp:TextBox>
             </div>
             <div class="G_TextBox">
                  <label class="G_label">开始时间 </label>
                  <asp:TextBox ID="txtStartDate" runat="server" CssClass="date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:TextBox>
             </div>
             <div class="G_TextBox">
                  <label class="G_label">结束时间 </label>
                  <asp:TextBox ID="txtEndDate" runat="server" CssClass="date-picker" data-date-format="yyyy-mm-dd" onchange="$(this).valid();"></asp:TextBox>
             </div>
             <div class="G_TextBox">
                  <label class="G_label">合同金额 </label>
                  <asp:TextBox ID="txtCost" runat="server"  CssClass="">10000</asp:TextBox>
             </div>
             <div class="G_TextBox" onclick="ContractSteps_alert(0)">
                  <label class="G_label">项目酬金 </label>
                  <asp:TextBox ID="txtCommission" runat="server" CssClass="input_Commission"  Enabled="false">5000</asp:TextBox>
             </div>
             <div class="lg_Generate_left_title">阶段设置</div>
             <div class="G_JieDuan">
                  <div class="G_JieDuan_div">项目开工</div>
                  <div class="G_JieDuan_line"></div>
                  <div id="G_JieDuan_list">
                    
                  </div>
                  <div class="G_JieDuan_div G_JieDuan_Add" onclick="ContractSteps_Add()">添加阶段</div>
                  <div class="G_JieDuan_line"></div>
                  <div class="G_JieDuan_div">项目完工</div>
                  <div class="newformalert_content" id="G_JieDuan_input">
                       <div class="close" onclick="Newform_close('G_JieDuan_input')"></div>	
                       <div class="newformalert_content_text">
                       <!--弹窗内容-->
                            <div class="newformalert_title">阶段</div>
                            <div class="newformalert_input">
                                <p>本阶段的酬金：</p>
                                <asp:TextBox ID="txtCommission_input" runat="server" CssClass="">0</asp:TextBox>
                                <p style="margin-top:10px;">说明：</p>
                                <asp:TextBox ID="txtComment_input" runat="server" CssClass="" Rows="5"></asp:TextBox>
                            </div>
                            <div class="G_JieDuan_btn">
                                <div class="newformalert_btn" onclick="savebtn()">确定</div>
                                <div class="newformalert_btn newformalert_del" onclick="ContractSteps_del()">删除</div>
                            </div>
                       </div>
                   </div>                  
             </div>
             <div class="lg_Generate_left_title">上传线下合同</div>
             <div id="divFileList" runat="server" class="G_JieDuan_file_list">
                 <div class="G_JieDuan_file_add">
                     <input id="fileAdd" name="id-input-file" type="file" onchange="changefile('fileAdd')" />
                 </div>
             </div>
             <div class="G_btn">
                 <button class="wtbtn savebtn" type="button" id="btn_save" runat="server" title="保存协议" style="display: none;">
                     保存协议
                 </button>
                 <button class="wtbtn savebtn" type="button" id="btn_businessapprove" runat="server" title="雇主同意" style="display: none;">
                     雇主同意
                 </button>
                 <button class="wtbtn cancelbtn" type="button" id="btn_businesscancel" runat="server" title="雇主取消" style="display: none;">
                     雇主取消
                 </button>
                 <button class="wtbtn savebtn" type="button" id="btn_agencyapprove" runat="server" title="销售同意" style="display: none;">
                     销售同意
                 </button>
                 <button class="wtbtn cancelbtn" type="button" id="btn_agencycancel" runat="server" title="销售取消" style="display: none;">
                     销售取消
                 </button>
             </div>
        </div>
        <div class="lg_Generate_right">
            <div class="lg_Generate_text">
                <div class="G_title">项目销售外包服务协议</div>
                <div class="G_name">甲方（雇主）：<label id="lblCompanyName" runat="server"></label></div>
                <div class="G_name">乙方（销售）：<label id="lblAgencyName" runat="server"></label></div>
                <div class="G_content">
                    <p>甲乙双方本着诚实守信、平等互利、共同发展的原则，经双方协商一致，在 广州华顺青为信息科技有限责任公司运营的众销乐网络平台（以下简称“众销乐”） 监督下签订本销售外包合作协议，甲乙双方就项目合作达成协议如下：</p>
                    <p class="G_p">一、 合作内容</p>
                    <ul>
                        <li>项目的定义：所谓项目就是甲方在众销乐发布的编号为<label id="lblCode" runat="server"></label>任务对应的“<label id="lblrequirement" runat="server"></label>”项目。</li>
                        <li>乙方可以以甲方名义完成甲方产品_______在_______客户（以下简称“客户”）的销售工作。</li>
                    </ul>
                    <p class="G_p">二、 双方的权利、义务</p>
                    <ul>
                        <li>乙方代表甲方进行客户跟踪、销售、谈判、回款等工作并及时提交工作报告（表），推动完成本项目销售工作的顺利进行，并承担相应销售费用。</li>
                        <li>甲方负责完成本项目的销售工具、标书购买与制作、合同签订、设备供货、设备安装调试、售前售中及售后服务等全程技术支持工作等及对应的费用。</li>
                        <li>客户或招标单位如需到甲方参观考察，甲方有义务向乙方提供车辆、陪同、技术、介绍等相关支持，并负责一餐招待费用。其它费用由乙方承担（如：交通、住宿、娱乐、旅游等）。</li>
                        <li>当项目需要采用招投标方式完成销售时，如果涉及到招投标需要缴纳实施或质量保证金，则保证金由甲方支付。</li>
                        <li>甲方产品必须是通过国家相关部门检验检测的合法、合规、合格安全的产品。</li>
                        <li>乙方应当在甲方授权范围内代理销售，超越代理权限造成损失的乙方自行负责，由此造成甲方损失的乙方还须赔偿。</li>
                        <li>乙方有义务提供销售过程中取得的各种凭证，如乙方未向甲方提交导致甲方损失的乙方应当赔偿。</li>
                    </ul>
                    <p class="G_p">三、 佣金与结算方式</p>
                    <ul>
                        <li>鉴于销售工作的特殊性和延续性，客户本期合同和后期的新增系统（包括并不限于甲方其他产品销售和服务）销售业绩都归双方共同所有。并按照下述第2条佣金结算方式执行；</li>
                        <li>佣金结算方式：乙方完成甲方的任务且达到验收标准后，按甲方与客户_______最终签订销售合同支付条款中的支付比例背靠背执行，结算方式选择以下项
                            <p>① 乙方所得税后佣金收益为实际合同签约总额的_______%</p>
                            <p>② 乙方所得税后佣金收益为_______</p>
                        </li>
                        <li>付款方式：甲乙双方签订协议5工作日内，甲方须按照不低于项目预估合同额计算乙方应得佣金的20% 作为预押保证金（在该项目销售失败或结束后15日内自动退回至甲方账户）托管至众销乐账户后，乙方正式开始项目销售工作，待销售成功甲方同客户签订合同后阶段期款回笼1周内甲方须把期款对应佣金托管至众销乐账户，乙方完成阶段工作后15日内由众销乐把阶段佣金直接支付给乙方在众销乐平台的账户，甲方如延期托管乙方应得的款项至众销乐账户， 则须按每天3‰比例向乙方支付滞纳金。如遇节假日则结算周期、费用支付日期顺延。</li>
                    </ul>
                    <p class="G_p">四、 协议有效期</p>
                    <ul>
                        <li>协议自双方签订之日起生效，自项目销售结束乙方收完佣金之日起结束，对于周期较长的项目原则上协议有效期为1年，如果1年到期仍未结束的经过甲乙双方沟通一致愿意延期的可以书面签订延期补充协议并上传至众销乐平台。</li>
                        <li>自甲方把预押保证金托管至众销乐平台之日起30天为协议蜜月期，即在蜜月期内双方如果发现对方不合适则可以单方面解除该项目的雇佣关系并在众销乐平台申请项目终止，蜜月期内经过批准解除协议的相互之间互不追责。</li>
                    </ul>
                    <p class="G_p">五、 保密协议</p>
                    <ul>
                        <li>甲乙双方对与该项目销售有关的客户任何方面商业资料和信息应严格予以保密，不得在未获得对方书面认可的情况下将此类商业资料和信息直接或间接透露给众销乐以外的任何第三方。</li>
                        <li>商业资料和信息包括商业计划、策略、安排及相应文档、资料、报价等，亦包括工作过程中所展示的思维方式、工作策略、工作资料，甲乙双方均仅限于将该等商业资料或信息透露给与本协议的履行直接有关的人员，且均需告知有关人员此项保密义务。</li>
                    </ul>
                    <p class="G_p">六、 违约责任</p>
                    <ul>
                        <li>若甲方不能如前述第三款按期支付有关款项，则每延期一天甲方支付乙方当期应付金额的0.3%作为违约金。</li>
                        <li>在甲方按与客户所签项目合同要求提供产品及服务的前提之下、若因乙方原因导致客户拒绝支付合同进度款的，则甲方有权拒绝向乙方支付相关费用，乙方自行承担费用。因甲方没按与客户所签项目合同要求提供产品或服务等甲方原因导致客户延迟付款的，甲方仍需按与客户所签订的合同进度安排支付乙方所得佣金收益。</li>
                        <li>若因乙方原因导致合作项目不能通过客户验收，则乙方同意退回甲方支付给乙方的既付佣金；若因甲方原因导致合作项目不能通过客户验收，则甲方同意按本合同第三款结算方式支付乙方应得佣金，并赔偿乙方的相关损失。</li>
                        <li>任何一方违反本协议约定即构成该方违反本协议的事实：违约方须向守约方赔偿直接损失和相关费用（包括但不限于诉讼费、律师费、担保费、保全费等），赔偿金最高不超过该项目的佣金总额。在守约方的损失无法计算的情况下，双方同意按佣金总额做为对方损失。</li>
                    </ul>
                    <p class="G_p">七、 其它条款</p>
                    <ul>
                        <li>本协议只是甲乙双方合作的线上通用协议，如有需要双方可以线下签订工作补充协议等作为本协议的有效补充，并将补充协议上传至众销乐平台进行监管。</li>
                        <li>众销乐平台提倡并鼓励甲乙双方采用正当合法的营销手段和措施完成项目销售任务，对于双方在使用众销乐服务过程中（无论线上和线下）发布的信息、实施的所有言论和为达成销售结果采取的行动不承担任何连带责任，即如果双方在完成销售任务时有诸如行贿、受贿等违法行为则由当事人自行承担法律责任。</li>
                        <li>不可抗力：由于地震、风灾、水灾、战争、罢工、类似罢工的工业事件、停工、意外以及政府行政命令和法令的变更和其它不能遇见并且对其发生后果不能避免不能克服的不可抗力事件，导致直接影响本协议履行或者不能按照约定的条件履行时，遇有上述不可抗力事件的一方，应立即将事件情况通知对方，并提供有关证明性文件。</li>
                        <li>一方对其因不可抗力而未履行、部分未履行或延迟履行本协议而导致他方所遭受的损失，不承担赔偿责任。</li>
                        <li>凡因本协议引起的或与本协议有关的任何争议，双方应友好协商解决。如不能协商解决，应提交有管辖权的人民法院进行诉讼。</li>
                        <li>本协议的订立、执行和解释及争议的解决均应适用中国法律。</li>
                        <li>关于协议的终止：协议有效期满自然终止；在协议有效期满前，如公司注消，则本协议终止；如有正在履约的项目客户，则按客户的签约合同完成日期执行。</li>
                    </ul>
                    <p class="G_p">八、 补充协议及附件</p>
                    <ul>
                        <li>经双方同意，本合作协议任何未尽事宜，可进行进一步的协商，并达成补充协议。补充协议构成本协议不可分割的组成部分，具有与本协议相同的法律效力。</li>
                        <li>所有补充协议及附件均是本协议不可分割的一部分，与本协议具有同等的法律效力。</li>
                        <li>本合作协议书一式贰份，双方各持壹份，每份具有同等法律效力。</li>
                    </ul>
                </div>
                <div class="G_autograph">
                    <div class="G_autograph_left">
                        <p>甲 方（盖章）</p>
                        <p>营业执照号码： <label id="lblBusinessLicense" runat="server"></label></p>
                        <p>签署代表：<label id="lblCompanyName2" runat="server"></label></p>
                        <p>联系电话：<label id="lblBusinessTel" runat="server"></label></p>
                        <p>签署日期：<label id="lblBusinessTime" runat="server"></label></p>
                        <img src="../Style/images/C_95.png" id="huzhutonghi" runat="server" style="display:none"/>
                    </div>
                    <div class="G_autograph_right">
                        <p>乙 方（盖章）</p>
                        <p>营业执照（个人身份证）号码：<label id="lblIDCard" runat="server"></label></p>
                        <p>签署代表：<label id="lblAgencyName2" runat="server"></label></p>
                        <p>联系电话：<label id="lblAgencyTel" runat="server"></label></p>
                        <p>签署日期：<label id="lblAgencyTime" runat="server"></label></p>
                        <img src="../Style/images/C_106.png" id="xiaoshoutonghi" runat="server" style="display:none"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidContractId" runat="server" />
</asp:Content>
