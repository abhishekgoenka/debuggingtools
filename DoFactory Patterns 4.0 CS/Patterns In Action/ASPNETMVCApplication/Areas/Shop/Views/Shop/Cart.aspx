<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ASPNETMVCApplication.Areas.Shop.Models.CartModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Cart
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Your Shopping Cart</h1>

    <%-- Links along the top of page --%>
    
    <div class="cartline"><% = Html.ActionLink("Continue Shopping", "Products")%></div>
    <div class="cartline"><% = Html.ActionLink("Checkout", "Checkout") %></div>

    <br />

    <% using (Html.BeginForm()) { %>


     <table cellspacing="1" cellpadding="3" class="table-list" border="0">
      <tr class="table-header">
       <td width= "55" align="center">Qty</td>
       <td width="245" align="left"> Description</td>
       <td width="100" align="right">Unit Price </td>
       <td width="100" align="right">Price</td>
       <td width= "70" align="center">&nbsp;</td>
      </tr>

       <% int count = 0; 
           foreach (var item in Model.CartItems)
          { %>

         <tr class='<% = count++ % 2 == 0 ? "tablerow" : "tablerow-alt" %>' >
           <td align="center"><input name='<% = "prodid-" + item.ProductId %>' type="text" value='<% = item.Quantity %>' maxlength='2' style="width:30px;" /></td>
           <td align="left"><% = Html.ActionLink(item.ProductName, "Product", new { productId = item.ProductId}) %></td>
           <td align="right"><% = item.UnitPrice %> </td>
           <td align="right"><% = item.TotalPrice %> </td>
           <td align="center">
               <input type="image" src="/assets/images/app/remove.jpg" 
                  onclick="<% = string.Format("Javascript:$('#delete').val({0});$('form').submit();return false;",item.ProductId) %>" />  
           </td>
          </tr>

       <% } %>
     </table>

     
     <br />
     <hr />

     <table border="0" cellpadding="0" cellspacing="0">
       <tr>
        <td width="319" align="left">
           <% = Html.ActionLink("Recalculate", "Recalculate", null, new { id="recalculate" }) %>
        </td>
        <td width="100" align="right"><b>SubTotal:</b></td>
        <td width="100" align="right"><%: Model.SubTotal %></td>
        <td width="70">&nbsp;</td>
      </tr>
      <tr>
        <td align="right"> Ship via:
           
           <% = Html.DropDownList("shippingId", ViewData["Shipping"] as SelectList, new { style = "width:100px;" })%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          
        </td>
        <td align="right"><b>Shipping:</b></td>
        <td align="right"><%: Model.Shipping %></td>
        <td >&nbsp;</td>
      </tr>
      <tr><td colspan="4"><hr /></td></tr>
      <tr>
        <td width="290" align="left">&nbsp;</td>
        <td width="100" align="right"><b>Total:</b></td>
        <td width="100" align="right"><font color="red"><u><b><%: Model.Total %></b></u></font></td>
        <td width="70">&nbsp;</td>
      </tr>
      <tr>
        <td height="36" colspan="3" align="left" valign="middle"><% = Html.ActionLink("Checkout", "Checkout") %></td>
      </tr>
     </table>

     <% = Html.Hidden("delete") %>

     <% } %>

     <hr />

     <br />
     <br />
     <br />
     <br />
     <br />

     <script type="text/javascript">
         $(document).ready(function ()
         {
             // clear delete hidden value
             $('#delete').val("");

             // setup click event for recalculations
             $('#recalculate').click(function ()
             {
                 $("form").attr("action", $(this)[0].href).submit();
                 return false;
             });

             // setup click event for shipping method change
             $('#shippingId').change(function ()
             {
                 $("form").attr("action", "/shop/cart/shipping").submit();
             });
         });
     </script>


</asp:Content>
