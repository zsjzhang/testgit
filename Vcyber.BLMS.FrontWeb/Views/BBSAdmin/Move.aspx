<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BBSShared/Site.Master" Inherits="System.Web.Mvc.ViewPage<mymvc.BBSGuestBook>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
移动留言 - <%: Vcyber.BLMS.FrontWeb.Models.BBS.Check.StieName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.BeginForm(); %>
<div class="bordertop">

<table>
    <tr>
        <td>标题：</td>
        <td><%: Model.G_Title %></td>
    </tr>
    <tr>
        <td>原栏目:</td>
        <td><%:Model.Columns.Column_Name %></td>
    </tr>
    <tr>
        <td>移动到：</td>
        <td><%: Html.DropDownListFor(model => model.Column_Id,(IEnumerable<SelectListItem>)ViewData["PList"])%></td>
    </tr>
    <tr>
        <td colspan="2"><input type="submit" value="确定" class="input_ok" /></td>
    </tr>
</table>
</div>
<% Html.EndForm(); %>
</asp:Content>
