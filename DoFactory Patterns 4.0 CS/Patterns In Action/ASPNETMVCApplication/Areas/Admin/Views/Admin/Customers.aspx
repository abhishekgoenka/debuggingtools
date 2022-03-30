<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SortedList<ASPNETMVCApplication.Areas.Admin.Models.CustomerModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Customers
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Customers</h1>
    
    <% = Html.ResultSummary() %>

    <% = Html.ActionLink("Add new Customer", "Customer", new { customerId = 0 }, new { style = "margin:0 0 0 505px;" })%>
    <br />

    <% using (Html.BeginForm()) { %>

    <span class="sortmessage">Click on headers to sort</span><br />
           
   <table cellspacing="0" cellpadding="4" class="table-list" style="width:630px;">
    <tr class="table-header">
    <td align="center"><%= Html.Sorter(Model, "Id", "CustomerId", "asc") %> </td>
    <td align="left"><%= Html.Sorter(Model, "Customer Name", "CompanyName", "asc") %></td>
    <td align="left"><%= Html.Sorter(Model, "City", "City", "asc") %></td>
    <td align="center"><%= Html.Sorter(Model, "Country", "Country", "asc") %></td>
    <td align="center">Edit</td>
    <td align="center">Delete</td>
    </tr>

   <%
       int count = 0;
       foreach (var item in Model.List) { %>

       <tr class='<% = ++count % 2 == 0 ? "tablerow" : "tablerow-alt" %>' >
        <td align="center"><%: item.CustomerId %></td>
        <td align="left"><%: item.CompanyName%></td>
        <td align="left"><%: item.City %></td>
        <td align="center"><%: item.Country %></td>
        <td align="center"><%: Html.ActionLink("Edit", "Customer", new { customerId = item.CustomerId }) %></td>
        <td align="center"><%: Html.ActionLink("Delete", "Delete", new { customerId = item.CustomerId }, new { onclick = "return ConfirmDelete('" + item.CustomerId + "','" + item.CompanyName.Replace("'","") + "')"} ) %></td>
        </tr>

    <% } %>
  </table>

<% = Html.Hidden("sort") %>
<% = Html.Hidden("order") %>
<% = Html.Hidden("delete") %>


    <% } %>

    <br />
    <br />
    <br />
    <br />

    <script type="text/javascript">
         $(document).ready(function ()
         {
            $('#delete').val("");
         });
         function ConfirmDelete(id, name) 
         {
             if (confirm("Are you sure you wish to delete " + name + "?"))
             {
                 $('#delete').val(id); 
                 $('form').submit();
             }
             return false;
         }
    </script>

</asp:Content>
