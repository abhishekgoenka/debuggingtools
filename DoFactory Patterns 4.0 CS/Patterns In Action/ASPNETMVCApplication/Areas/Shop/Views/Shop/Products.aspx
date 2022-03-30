<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SortedList<ASPNETMVCApplication.Areas.Shop.Models.ProductModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Products
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <h1>Products</h1>

   <% using (Html.BeginForm()) { %>
        <div>

           <div>
             Select a Category: &nbsp;
             <%= Html.DropDownList("CategoryId", ViewData["Categories"] as SelectList, new { onchange = "$('form').submit();", style="width:120px;" })%> 
           </div>

           <br /><br />

           <% Html.RenderPartial("ProductList", Model); %>
           
        </div>

    <% } %>
   

</asp:Content>
