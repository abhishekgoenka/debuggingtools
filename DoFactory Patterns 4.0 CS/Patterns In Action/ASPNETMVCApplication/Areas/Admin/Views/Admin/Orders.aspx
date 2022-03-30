<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SortedList<ASPNETMVCApplication.Areas.Admin.Models.CustomerModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Orders by customer
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Orders By Customer</h1>
    <br />

    <%= Html.ValidationSummary()%>

    <% using (Html.BeginForm()) { %>


    <span class="sortmessage">Click on headers to sort</span><br />
           
   <table cellspacing="0" cellpadding="4" class="table-list" style="width:660px;">
    <tr class="table-header">
     <td align="center"><%= Html.Sorter(Model, "Id", "CustomerId", "asc") %></td>
     <td align="left"><%= Html.Sorter(Model, "Customer Name", "CompanyName", "asc") %></td>
     <td align="left"><%= Html.Sorter(Model, "City", "City", "asc") %></td>
     <td align="center"><%= Html.Sorter(Model, "Country", "Country", "asc") %></td>
     <td align="center"><%= Html.Sorter(Model, "# Orders", "NumOrders", "asc") %></td>
     <td align="center"><%= Html.Sorter(Model, "Last Order", "LastOrderDate", "asc") %></td>
     <td align="center">View</td>
    </tr>
    <%
       int count = 0;
       foreach (var item in Model.List) { %>

       <tr class='<% = ++count % 2 == 0 ? "tablerow" : "tablerow-alt" %>' >
        <td align="center"><%: item.CustomerId %></td>
        <td align="left"><%: item.CompanyName%></td>
        <td align="left"><%: item.City %></td>
        <td align="center"><%: item.Country %></td>
        <td align="center"><%: item.NumOrders %></td>
        <td align="center"><%: item.LastOrderDate %></td>
        <td align="center"><%: Html.ActionLink("View", "CustomerOrders", new { customerId = item.CustomerId })%></td>
       </tr>

    <% } %>
  </table>

    <% = Html.Hidden("sort") %>
    <% = Html.Hidden("order") %>


    <% } %>

    <br />
    <br />
    <br />
    <br />

</asp:Content>
