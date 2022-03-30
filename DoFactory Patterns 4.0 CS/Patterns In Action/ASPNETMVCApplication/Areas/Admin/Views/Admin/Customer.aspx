<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ASPNETMVCApplication.Areas.Admin.Models.CustomerModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Customer
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

      <ul class="headline">
        <li class="headline-left"><h1>Customer Details</h1></li>
        <li class="headline-right"><a href="JavaScript:history.go(-1);">&lt; back to customer list</a></li>
      </ul>
      
      <img class="customer-image" src='<%: Html.Display("CustomerImage") %>' alt="customer image" />
      

      <% using (Html.BeginForm()) {%>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.CustomerId) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.CustomerId) %>
            </div>
            <div class="clear"></div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.CompanyName) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.CompanyName, new { @class = "text-box" })%>
            </div>
            <div class="clear"></div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.City) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.City, new { @class = "text-box" })%>
            </div>
            <div class="clear"></div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.Country) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Country, new { @class = "text-box" })%>
            </div>
            <div class="clear"></div>
            <%: Html.HiddenFor(model => model.Version) %>

            <div class="button-box">
                <input type="submit" value="Save" />&nbsp;&nbsp;
                <input type="submit" value="Cancel" onclick="window.location='/admin/customers';return false;" />
            </div>
       

    <% } %>
    
    <br />

    <%: Html.ValidationSummary() %>

    <br />
    <br />
    <br />


    <script type="text/javascript">
        $(document).ready(function () {
            $('#CompanyName').focus();
        });
    </script>

</asp:Content>

