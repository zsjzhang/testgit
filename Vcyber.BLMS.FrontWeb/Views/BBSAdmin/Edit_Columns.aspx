<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BBSShared/Site.Master" Inherits="System.Web.Mvc.ViewPage<mymvc.BBSColumns>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
重命名栏目 - <%: Vcyber.BLMS.FrontWeb.Models.BBS.Check.StieName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
<div class="bordertop">
                <table>
                <tr>
                <td>栏目名:</td>
                <td><%: Html.TextBoxFor(model => model.Column_Name)%><%: Html.ValidationMessage("Column_Name") %></td>
                </tr>
                <tr>
                <td></td><td>(栏目名不得大于15个字)</td>
                </tr>
                <tr>
                <td>描述:</td>
                <td><%: Html.TextAreaFor(model => model.Info,3,40,null)%><%: Html.ValidationMessage("Info")%>
                </td>
                </tr>
                <tr>
                <td></td><td>(栏目描述不得大于30个字)</td>
                </tr>
                <tr>
                <td colspan="2"><input type="submit" value="保存" class="input_ok" /></td>
                </tr>
                </table>

    <% } %>

</div>
</asp:Content>

