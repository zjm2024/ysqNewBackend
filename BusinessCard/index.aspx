<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="BusinessCard.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
    <div class="form">
        <div class="cardshow">
          <div class="head">
            <div class="left">
            <%if (PersonalVO.Headimg != "")
              { %>
              <div class="img" style=" background-image:url(<%=PersonalVO.Headimg%>)"></div>
              <%}
            else
            { %>
              <div class="img" style=" background-image:url(https://www.zhongxiaole.net/SPManager/Style/images/wxcard/noheadimg.jpg)"></div>
                <%} %>
            </div>
            <div class="right">
              <div class="name"><%=PersonalVO.Name %></div>
              <div class="position"><%=PersonalVO.Position %></div>
            </div>
          </div>
          <ul class="infoshow">
            <li>
              <div class='show_left'>姓　　名</div>
              <div class='show_right'><%=PersonalVO.Name %></div>
            </li>
            <li style="<%if(PersonalVO.Position==""){%>display:none<%}%>">
              <div class='show_left'>职　　位</div>
              <div class='show_right'><%=PersonalVO.Position %></div>
            </li>
            <li style="<%if(PersonalVO.Phone==""){%>display:none<%}%>">
              <div class='show_left'>手　　机</div>
              <div class='show_right haveoper'><%=PersonalVO.Phone %></div>
            </li>
            <li style="<%if(PersonalVO.WeChat==""){%>display:none<%}%>">
              <div class='show_left'>微　　信</div>
              <div class='show_right haveoper'><%=PersonalVO.WeChat %></div>
            </li>
            <li style="<%if(PersonalVO.Email==""){%>display:none<%}%>">
              <div class='show_left'>邮　　箱</div>
              <div class='show_right haveoper'><%=PersonalVO.Email %></div>
            </li>
            <li style="<%if(PersonalVO.Tel==""){%>display:none<%}%>">
              <div class='show_left'>固定电话</div>
              <div class='show_right haveoper'><%=PersonalVO.Tel %></div>
            </li>
            <li style="<%if(PersonalVO.Business==""){%>display:none<%}%>">
              <div class='show_left'>主营业务</div>
              <div class='show_right'><%=PersonalVO.Business %></div>
            </li>
            <li style="<%if(PersonalVO.Address==""){%>display:none<%}%>">
              <div class='show_left'>办公地点</div>
              <div class='show_right haveoper'><%=PersonalVO.Address %></div>
            </li>
          </ul>
          <div class="btn">编辑名片</div>
        </div>
      </div>
      <div class="qrcode">
        <div class="border"></div>
        <img src="<%=PersonalVO.QRimg %>" />
        <div class="text">微信<font>扫码</font>查看分享名片</div>
      </div>
</asp:Content>