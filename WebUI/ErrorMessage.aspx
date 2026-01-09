<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPageSite.Master" AutoEventWireup="true" CodeBehind="ErrorMessage.aspx.cs" Inherits="WebUI.ErrorMessage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12">
                <div class="error-container">
                    <div class="well">
                        <h1 class="grey lighter smaller">出现未知错误！
                        </h1>
                        <asp:HiddenField runat="server" ID="hidErrorTip" Value="hello" />
                        <asp:HiddenField runat="server" ID="hidErrorMessage" />
                        <hr />
                        <h3 class="lighter smaller">系统出现了一些问题，操作无法继续运行！</h3>

                        <div>
                            <div class="space"></div>
                            <h4 class="smaller">您可以继续做如下操作:</h4>

                            <ul class="list-unstyled spaced inline bigger-110 margin-15">
                                <li>
                                    <i class="ace-icon fa fa-hand-o-right blue"></i>
                                    再做一次，可能是网络问题。
                                </li>

                                <li>
                                    <i class="ace-icon fa fa-hand-o-right blue"></i>
                                    记下操作步骤，通知系统管理员。
                                </li>

                                <li>
                                    <i class="ace-icon fa fa-hand-o-right blue"></i>
                                    如果是系统有问题，可以先做其它工作，等维护人员修复。
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
