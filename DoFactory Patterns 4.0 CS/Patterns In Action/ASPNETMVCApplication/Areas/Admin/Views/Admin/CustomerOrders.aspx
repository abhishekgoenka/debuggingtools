<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<ASPNETMVCApplication.Areas.Admin.Models.OrderModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Customer Orders
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="headline">
      <li class="headline-left"><h1>Orders for: <span style="color:#000;font-size:12pt;"><%: ViewData["Company"] %></span></h1></li>
      <li class="headline-right"><% = Html.ActionLink("< back to orders", "Orders") %></li>
    </ul>
    <div class="clear"></div>

   <table cellspacing="0" cellpadding="4" class="table-list"">
    <tr class="table-header">
     <td align="center">Order Id</td>
     <td align="center">Order Date</td>
     <td align="center">Required Date</td>
     <td align="right">Shipping</td>
     <td align="center">Items</td>
    </tr>

    <%
       int count = 0;
       foreach (var item in Model) { %>

       <tr class='<% = ++count % 2 == 0 ? "tablerow" : "tablerow-alt" %>' >
        <td align="center"><%: item.OrderId %></td>
        <td align="center"><%: item.OrderDate%></td>
        <td align="center"><%: item.RequiredDate %></td>
        <td align="right"><%: item.Shipping %></td>
        <td align="center"><%: Html.ActionLink("View", "OrderDetails", new { customerId = ViewData["CustomerId"], orderId = item.OrderId }) %></td>
       </tr>

    <% } %>

   </table>

   <br />
   <br />
   <br />
   <br />

</asp:Content>
