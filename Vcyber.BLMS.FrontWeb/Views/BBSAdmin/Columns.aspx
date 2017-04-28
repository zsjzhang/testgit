<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BBSShared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<mymvc.BBSColumns>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
栏目管理 - <%: Vcyber.BLMS.FrontWeb.Models.BBS.Check.StieName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="bordertop">
    <table class="index_tb">
        <tr>
            <th class="td2">栏目名称</th>
            <th class="td2">栏目描述</th>
            <th class="td2">留言数量</th>
            <th class="td2">操作</th>
        </tr>
    <% foreach (var item in Model) { %>
    
        <tr>
            <td class="td3"><%: item.Column_Name %></td>
            <td><%: item.Info %></td>
            <td class="td3">
            <% if (item.GuestBook.Count() == 0){ %>
               暂无留言
            <%}else{%>
            共有<%:item.GuestBook.Count()%>条留言
            <%}%></td>
            <td class="td3">
                <%: Html.ActionLink("编辑", "Edit_Columns", new { id=item.Id }) %> |
                <%: Html.ActionLink("删除", "Del_Columns", new { id = item.Id }, new { @class = "DelLink" })%>
            </td>

        </tr>
    
    <% } %>
            <tr>
            <td><%: Html.ActionLink("添加栏目", "Add_Columns") %></td>
            <td colspan="2"></td>
        </tr>
    </table>
</div>
<script type="text/javascript" src="../../Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".DelLink").click(function () {
                return confirm("确定删除?");
            });
        });
    </script>
</asp:Content>

