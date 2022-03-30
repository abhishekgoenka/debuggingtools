<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ASPNETMVCApplication.Areas.Auth.Models.LoginModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <h1>Login</h1>
   
   <p><br />
    Login is required to access the Administration area. For demonstration purposes 
    use these<br /> credentials: &nbsp;<i>username:</i> '<span style="color:#f00;">debbie</span>',
    <i>password:</i> '<font color='red'>secret123</font>'.</p>
   </p>

   <br />
  

   <% using (Html.BeginForm()) { %>
        <div>
            <div class="login-heading">please login</div>

            <div class="login-label">
                <%= Html.LabelFor(m => m.UserName) %>
            </div>
            <div class="login-field">
                <%= Html.TextBoxFor(m => m.UserName, new{ @class = "text-box" })%>
            </div>
            <div class="clear"></div>

            <div class="login-label">
                <%= Html.LabelFor(m => m.Password) %>
            </div>
            <div class="login-field">
                <%= Html.PasswordFor(m => m.Password, new{ @class = "text-box" })%>
            </div>
            <div class="clear"></div>

            <div class="login-box">
                <input type="submit" value="Submit" class="button" />
            </div>
        </div>
    <% } %>

    <br />
    <%= Html.ValidationSummary(true)%>
    <br />


    <script type="text/javascript">
        $(document).ready(function () {
            $('#UserName').focus();
        });
    </script>



</asp:Content>
