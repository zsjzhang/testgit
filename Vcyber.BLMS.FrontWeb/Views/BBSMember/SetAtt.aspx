<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BBSShared/Site.Master" Inherits="System.Web.Mvc.ViewPage<mymvc.Models.Member>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
个性设置 - <%: Vcyber.BLMS.FrontWeb.Models.BBS.Check.StieName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.BeginForm();%>
<div class="bordertop">
    <table>
        <tr>
            <td>个性签名：</td><td><%: Html.TextAreaFor(model => model.SetAtt,3,60,null) %><%: Html.ValidationMessage("SetAtt") %><br />（将在每个留言下方显示，限50字）</td>
        </tr>
        <tr>
            <td colspan="2"><input type="submit" value="保存资料" class="input_ok" /></td>
        </tr>
    </table>
</div>
<% Html.EndForm(); %>
</asp:Content>
