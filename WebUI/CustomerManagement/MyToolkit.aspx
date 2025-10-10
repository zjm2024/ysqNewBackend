<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="MyToolkit.aspx.cs" Inherits="WebUI.CustomerManagement.MyToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <script type="text/javascript" src="../Scripts/ajaxfileupload.js"></script>
      <script type="text/javascript" src="../Scripts/CustomerManagement/MyToolkitJS.js"></script>
  <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right">备注 </label>
        <div class="col-sm-9">
            <asp:TextBox ID="txtfileDesc" runat="server" Style="height: 100px; width: 350px; border: 1px solid #d7d3d3; background-color: #fff; padding-left: 10px; font-size: 12px; margin-top: 4px;" TextMode="MultiLine"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2 control-label no-padding-right"></label>
        <div class="col-sm-9">
            <div class="ui-jqgrid-bdiv" style="height: 100%; width: 100%;">
                <input id="fileAdd" name="id-input-file" type="file" onchange="changefile('fileAdd')" />
                <button class="wtbtn yjright" type="button" id="btn_deletefile" onclick="return Deletefile();" title="删除">
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
                                <div class="ui-jqgrid-sortable" title="备注">备注</div>
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

</asp:Content>
