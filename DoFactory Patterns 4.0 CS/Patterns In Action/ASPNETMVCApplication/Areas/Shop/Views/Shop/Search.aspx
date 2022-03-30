<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SortedList<ASPNETMVCApplication.Areas.Shop.Models.ProductModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Search
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Search</h1>

    <%-- Search criteria --%>

    <% using (Html.BeginForm()) { %>

      Product Name: <% = Html.TextBox("ProductName", ViewData["ProductName"], new {style = "width:100px;"} ) %> &nbsp;&nbsp;&nbsp;
      Price range: <% = Html.DropDownList( "Ranges", ViewData["Ranges"] as SelectList, new {style = "width:130px;"}) %>

      &nbsp;&nbsp;<input type="submit" value=" Find " />
      &nbsp;&nbsp;<% = Html.ActionLink("reset", "Search") %>

    <hr />
    <br />

    <% if (Model.List.Count > 0) { %>
      <% Html.RenderPartial("ProductList", Model ); %>
    <% } %>


    <% } %>

    <br />
    <br />
    <br />
    <br />

</asp:Content>
