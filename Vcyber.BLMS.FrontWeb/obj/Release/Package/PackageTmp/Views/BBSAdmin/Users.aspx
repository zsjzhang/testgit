<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BBSShared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<mymvc.Models.Member>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="bordertop">
    <table class="index_tb">
        <tr>
            <th class="td2">用户ID</th>
            <th class="td2">用户名</th>
            <th class="td2">权限</th>
            <th class="td2">性别</th>
            <th class="td2">邮箱</th>
            <th class="td2">个性签名</th>
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td class="td1"><%: item.Id %></td>
            <td class="td1"><%: item.UserName %></td>
            <td class="td1">
                <%
                    if(item.Power !=null)
                    if (int.Parse(item.Power) == 1)
                    {
                        Response.Write("管理员");
                    }
                    else {
                        Response.Write("会员");
                    }%>
            </td>
            <td class="td1"><%: item.Sex %></td>
            <td class="td3"><%: item.Email %></td>
            <td><%: item.SetAtt %></td>
            <td class="td1">
                <%: Html.ActionLink("删除", "Del_User", new { id = item.Id }, new { @class = "DelLink" })%>
            </td>
        </tr>
    
    <% } %>
        <tr>
            <td colspan="7">
<%= Html.ShowFPage(Url.Action("Users", new { page = "{0}" }), (int)ViewData["PageIndex"], (int)ViewData["PageSize"], (int)ViewData["ReCordCount"], FPage.FPageMode.GroupNumeric)%>
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

