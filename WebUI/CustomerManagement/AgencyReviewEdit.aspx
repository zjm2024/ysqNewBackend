<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="AgencyReviewEdit.aspx.cs" Inherits="WebUI.CustomerManagement.AgencyReviewEdit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/CustomerManagement/AgencyReviewEditJS.js"></script>
    <script type="text/javascript" src="../Scripts/StartReview.js"></script>
    <link rel="stylesheet" href="../Style/css/StarReview.css" />
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <h4 class="header">项目内容</h4>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">项目编号</label>
                    <div class="col-sm-9">
                        <div id="divProjectCode" style="padding-top: 6px; margin-left: 10px;"></div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">项目名称</label>
                    <div class="col-sm-9">
                        <div id="divProjectName" style="padding-top: 6px; margin-left: 10px;"></div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">雇主</label>
                    <div class="col-sm-9">
                        <div id="divBusinessName" style="padding-top: 6px; margin-left: 10px;"></div>
                    </div>
                </div>
                <div class="form-group">
                    <h4 class="header">评价详情</h4>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">售前售中支持</label>
                    <div class="col-sm-9">
                        <div class="xzw_starSys">
                            <div class="xzw_starBox">
                                <ul class="star stars">
                                    <li><a href="javascript:void(0)" title="1" class="one-star">1</a></li>
                                    <li><a href="javascript:void(0)" title="2" class="two-stars">2</a></li>
                                    <li><a href="javascript:void(0)" title="3" class="three-stars">3</a></li>
                                    <li><a href="javascript:void(0)" title="4" class="four-stars">4</a></li>
                                    <li><a href="javascript:void(0)" title="5" class="five-stars">5</a></li>
                                </ul>
                                <div class="current-rating showb" style="width: 30px;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">售后服务</label>
                    <div class="col-sm-9">
                        <div class="xzw_starSys">
                            <div class="xzw_starBox">
                                <ul class="star stars">
                                    <li><a href="javascript:void(0)" title="1" class="one-star">1</a></li>
                                    <li><a href="javascript:void(0)" title="2" class="two-stars">2</a></li>
                                    <li><a href="javascript:void(0)" title="3" class="three-stars">3</a></li>
                                    <li><a href="javascript:void(0)" title="4" class="four-stars">4</a></li>
                                    <li><a href="javascript:void(0)" title="5" class="five-stars">5</a></li>
                                </ul>
                                <div class="current-rating showb" style="width: 30px;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">产品质量</label>
                    <div class="col-sm-9">
                        <div class="xzw_starSys">
                            <div class="xzw_starBox">
                                <ul class="star stars">
                                    <li><a href="javascript:void(0)" title="1" class="one-star">1</a></li>
                                    <li><a href="javascript:void(0)" title="2" class="two-stars">2</a></li>
                                    <li><a href="javascript:void(0)" title="3" class="three-stars">3</a></li>
                                    <li><a href="javascript:void(0)" title="4" class="four-stars">4</a></li>
                                    <li><a href="javascript:void(0)" title="5" class="five-stars">5</a></li>
                                </ul>
                                <div class="current-rating showb" style="width: 30px;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">付款及时性</label>
                    <div class="col-sm-9">
                        <div class="xzw_starSys">
                            <div class="xzw_starBox">
                                <ul class="star stars">
                                    <li><a href="javascript:void(0)" title="1" class="one-star">1</a></li>
                                    <li><a href="javascript:void(0)" title="2" class="two-stars">2</a></li>
                                    <li><a href="javascript:void(0)" title="3" class="three-stars">3</a></li>
                                    <li><a href="javascript:void(0)" title="4" class="four-stars">4</a></li>
                                    <li><a href="javascript:void(0)" title="5" class="five-stars">5</a></li>
                                </ul>
                                <div class="current-rating showb" style="width: 30px;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">诚信度</label>
                    <div class="col-sm-9">
                        <div class="xzw_starSys">
                            <div class="xzw_starBox">
                                <ul class="star stars">
                                    <li><a href="javascript:void(0)" title="1" class="one-star">1</a></li>
                                    <li><a href="javascript:void(0)" title="2" class="two-stars">2</a></li>
                                    <li><a href="javascript:void(0)" title="3" class="three-stars">3</a></li>
                                    <li><a href="javascript:void(0)" title="4" class="four-stars">4</a></li>
                                    <li><a href="javascript:void(0)" title="5" class="five-stars">5</a></li>
                                </ul>
                                <div class="current-rating showb" style="width: 30px;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">评价内容</label>
                    <div class="col-sm-9">
                        <div id="divAgencyReviewDescription" style="padding-top: 6px; margin-left: 10px;"></div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">追加评价</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtAddNote" runat="server" CssClass="description-textarea" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right">雇主回复</label>
                    <div class="col-sm-9">
                        <div id="divBusinessExplanation" style="padding-top: 6px; margin-left: 10px;"></div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label no-padding-right"></label>
                    <div class="col-sm-9">
                        <button class="wtbtn yjright" type="button" id="btn_NewReview" onclick="return PlusReview();" title="追评">
                            <i class="icon-ok bigger-110"></i>
                            追评
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
