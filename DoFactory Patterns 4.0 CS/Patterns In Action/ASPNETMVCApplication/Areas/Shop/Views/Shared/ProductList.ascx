<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SortedList<ASPNETMVCApplication.Areas.Shop.Models.ProductModel>>" %>


<span class="sortmessage">Click on headers to sort</span><br />
           
<table cellspacing="0" cellpadding="4" class="table-list">
    <tr class="table-header">
    <td align="center"><%= Html.Sorter(Model, "Id", "ProductId", "asc") %> </td>
    <td align="left"><%= Html.Sorter(Model, "Product Name", "ProductName", "asc") %></td>
    <td align="left"><%= Html.Sorter(Model, "Weight", "Weight", "asc") %></td>
    <td align="right"><%= Html.Sorter(Model, "Price", "UnitPrice", "asc") %></td>
    <td align="center">Details</td>
    </tr>

    <% 
        int count = 0;
        foreach (var item in Model.List) { %>
        <tr class='<% = ++count % 2 == 0 ? "tablerow" : "tablerow-alt" %>' >
        <td align="center"><%: item.ProductId %></td>
        <td align="left"><%: item.Name %></td>
        <td align="left"><%: item.Weight %></td>
        <td align="right"><%: item.Price %></td>
        <td align="center"><%: Html.ActionLink("View", "Product", new { productId = item.ProductId })%></td>
        </tr>

    <% } %>
</table>

<% = Html.Hidden("sort") %>
<% = Html.Hidden("order") %>

