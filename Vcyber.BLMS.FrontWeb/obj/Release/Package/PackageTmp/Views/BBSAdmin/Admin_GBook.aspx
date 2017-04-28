<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BBSShared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Vcyber.BLMS.FrontWeb.Models.BBS.BBSGuestBook>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
留言管理 - <%: Vcyber.BLMS.FrontWeb.Models.BBS.Check.StieName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.BeginForm("Admin_GBook","Admin"); %>
<div class="bordertop">
    <table class="index_tb">
        <tr>
            <th class="td2">标题</th>
            <th class="td2">会员</th>
            <th class="td2">评论</th>
            <th class="td2">管理员回复</th>
            <th class="td2">隶属栏目</th>
            <th class="td2">留言时间</th>
            <th></th>
        </tr>

    <% foreach (var item in Model){ %>
    
        <tr>
            <td style="width:30%;">
            <a href="<%: Url.Action("Comment","Member",new{ id = item.Id }) %>" target="_blank"><%: item.G_Title%></a>            
            </td>
            <td class="td1"><%: item.Member.UserName%></td>
            <td class="td1">被评论:<%: item.Comment.Count()%>次</td>
            <td class="td1">
            <%if (item.G_HF_Content == null)
              { %>
            未回复
            <%}
              else
              { %>
            已回复
            <%}%>
            </td>
            <td class="td1"><%: item.Columns.Column_Name%></td>
            <td class="td1"><%: ((DateTime)item.G_Time).ToString("yyyy-MM-dd")%></td>
            <td style="width:20%;text-align:center;">
                <%: Html.ActionLink("回复/编辑", "Huifu", "Home", new { id = item.Id }, new { @target = "_blank" })%> |
                <%: Html.ActionLink("移动", "Move", new { id = item.Id }, new { @target = "_blank" })%> |
                <%: Html.ActionLink("查看", "Comment", "Member", new { id = item.Id }, new { @target = "_blank" })%> |
                <%: Html.ActionLink("删除", "Delete", "Home", new { id = item.Id }, new { @class = "DelLink" })%>
            </td>
        </tr>
    
    <% }%>
    <tr>
        <td colspan="7">
            <%= Html.ShowFPage(Url.Action("Admin_GBook", new { page = "{0}" }), (int)ViewData["PageIndex"], (int)ViewData["PageSize"], (int)ViewData["ReCordCount"], FPage.FPageMode.GroupNumeric)%>
        </td>
    </tr>

        <tr>
            <td colspan="7">
                <table class="tableborder">
                    <tr>
                    <td>关键字：</td>
                    <td><%:Html.TextBox("KeyWord", null, new { @class = "KeyWord" })%></td>
                    <td><input type="submit" value="" class="Search" /></td>
                    </tr>
                </table>
                 
            </td>
        </tr>
    </table>
    </div>
<% Html.EndForm(); %>
<script type="text/javascript" src="../../Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".DelLink").click(function () {
                return confirm("确定删除?");
            });
        });
    </script>
</asp:Content>

