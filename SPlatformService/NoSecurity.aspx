<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="NoSecurity.aspx.cs" Inherits="SPlatformService.NoSecurity" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12">
                <div class="error-container">
                    <div class="well">
                        <h1 class="grey lighter smaller">
                            没有该页面权限！
                        </h1>

                        <hr>

                        <div>
                            <div class="space"></div>
                            <h4 class="smaller">您可以继续做如下操作:</h4>

                            <ul class="list-unstyled spaced inline bigger-110 margin-15">
                                <li>
                                    <i class="ace-icon fa fa-hand-o-right blue"></i>
                                    确认自己是否有该页面权限。
                                </li>

                                <li>
                                    <i class="ace-icon fa fa-hand-o-right blue"></i>
                                    联系系统管理员，加上权限。
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
