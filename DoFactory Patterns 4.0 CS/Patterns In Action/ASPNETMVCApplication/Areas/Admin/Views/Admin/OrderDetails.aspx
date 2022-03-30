<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<ASPNETMVCApplication.Areas.Admin.Models.OrderDetailModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	OrderDetails
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <ul class="headline">
      <li class="headline-left"><h1>Order Details</h1></li>
      <li class="headline-right"><a href="JavaScript:history.go(-1);" style="float:right;">&lt; back</a></li>
   </ul>
   <div class="clear"></div>

   
   <h3><%: Html.Display("OrderDate") %></h3>
   <br />

   <table cellspacing="0" cellpadding="4" class="table-list">
    <tr class="table-header">
     <td align="left"> Product</td>
     <td align="center"> Quantity</td>
     <td align="right">Unit Price </td>
     <td align="right">Discount </td>
    </tr>

     <%
       int count = 0;
       foreach (var item in Model) { %>

       <tr class='<% = ++count % 2 == 0 ? "tablerow" : "tablerow-alt" %>' >
        <td align="left"><%: item.ProductName %></td>
        <td align="center"><%: item.Quantity %></td>
        <td align="right"><%: item.UnitPrice %></td>
        <td align="right"><%: item.Discount %> </td>
       </tr>

    <% } %>

   </table>

   <br />
   <br />
   <br />
   <br />

</asp:Content>
