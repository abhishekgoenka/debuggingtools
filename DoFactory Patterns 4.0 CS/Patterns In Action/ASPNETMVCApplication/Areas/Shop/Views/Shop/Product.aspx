<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ASPNETMVCApplication.Areas.Shop.Models.ProductModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Product Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Content header and back button -->
    
    <ul class="headline">
      <li class="headline-left"><h1>Product Details</h1></li>
      <li class="headline-right"><a href="JavaScript:history.go(-1);">&lt; back to previous page</a></li>
    </ul>

    <img src='<% = ViewData["ProductImage"] %>' class="floatright" alt="" style="width:90px;height:90px;" />

    <table cellspacing="2" cellpadding="4" border="0" class="table-details">
      <tr>
          <td class="table-details-label"><%= Html.LabelFor(m => m.CategoryName) %> </td>
          <td class="table-details-title"><%= Html.DisplayFor(m => m.CategoryName) %></td>
      </tr>
      <tr>
          <td class="table-details-label"><%= Html.LabelFor(m => m.Name) %> </td>
          <td><%= Html.DisplayFor(m => m.Name) %></td>
      </tr>
      <tr>
          <td class="table-details-label"><%= Html.LabelFor(m => m.Price) %> </td>
          <td><%= Html.DisplayFor(m => m.Price) %></td>
      </tr>
      <tr>
          <td class="table-details-label"><%= Html.LabelFor(m => m.Weight) %> </td>
          <td><%= Html.DisplayFor(m => m.Weight) %></td>
      </tr>
      <tr>
          <td class="table-details-label"><%= Html.LabelFor(m => m.UnitsInStock) %> </td>
          <td><%= Html.DisplayFor(m => m.UnitsInStock) %></td>
      </tr>
    </table>
    
    <br />
    
    <br />
      
    <% using (Html.BeginForm()) { %>
      
       <input name="Quantity" id="Quantity" type="text" value="1" maxlength="2" style="width:30px;margin:0 5px 0 150px;" />
       <input type="submit" value=" Add to Cart " />
      
       <% = Html.Hidden("ProductId", Model.ProductId) %>

    <% } %>

    <br />
    <%= Html.ValidationSummary()%>
    <br />


</asp:Content>
