<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BBSShared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<mymvc.BBSGuestBook>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
回收站 - <%: Vcyber.BLMS.FrontWeb.Models.BBS.Check.StieName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="bordertop">
<table class="index_tb">
    <tr>
        <th class="td2">标题</th>
        <th class="td2">会员</th>
        <th class="td2">栏目</th>
        <th class="td2">留言时间</th>
        <th class="td2">评论数量</th>
        <th class="td2">操作</th>
    </tr>
<% foreach (var item in Model){%>
    <tr>
        <td><%: item.G_Title %></td>
        <td class="td1"><%: item.Member.UserName %></td> 
        <td class="td1"><%: item.Columns.Column_Name %></td> 
        <td class="td1"><%: ((DateTime)item.G_Time).ToString("yyyy年MM月dd日") %></td>
        <td class="td1"><%: item.Comment.Count() %></td>
        <td class="td1">
        <%: Html.ActionLink("删除", "Delete", new { id = item.Id }, new { @class = "DelLink" })%>
        |
        <%: Html.ActionLink("还原", "Restore", new { id = item.Id })%>
        </td>
    </tr>
<%} %>
    <tr>
        <td colspan="6">
<%= Html.ShowFPage(Url.Action("Recycle", new { page = "{0}" }), (int)ViewData["PageIndex"], (int)ViewData["PageSize"], (int)ViewData["ReCordCount"], FPage.FPageMode.GroupNumeric)%>
        </td>
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
