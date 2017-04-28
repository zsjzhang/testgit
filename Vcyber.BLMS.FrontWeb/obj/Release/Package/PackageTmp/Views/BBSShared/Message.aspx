<%@ Page Language="C#" MasterPageFile="~/Views/BBSShared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
  提示信息！
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="bordertop">
<div class="ErrorMessage">
<div class="ErrorMessage_Title">  提示信息！</div>
<div class="ErrorMessage_Content">
<%: ViewData["Message"] %><br /><br />
还有<span id="miao">5</span> 秒自动跳转到<%: ViewData["Message1"] %>!
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            NextMiao(5);
        });
        function NextMiao(m) {
            if (m != 0) {
                $("#miao").html(m);
                setTimeout("NextMiao(" + (m - 1) + ");", 1000);
            } else {
                location = '<%: ViewData["Url"] %>';
            }
        }
    </script>
</div>
</div>
</div>
</asp:Content>
