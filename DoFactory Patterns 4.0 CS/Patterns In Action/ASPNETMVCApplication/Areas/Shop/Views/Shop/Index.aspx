<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Shopping
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Shopping</h1>

	<p>This is where users start shopping. They select items from a catalog 
      of products and add these to a shopping cart. The shopping cart is a fully functional
      e-commerce shopping cart that can be enhanced easily with sales tax, insurance, shipping and 
      other calculations. If desirable, you could also make the cart persistent by using a 
      combination of cookies on the client and a new 'Cart' table in the database.
	</p>
	
	<br />
	
	<ul>
	 <li><% = Html.ActionLink("Click here", "Products") %> to start shopping <br /></li>
	 <li><% = Html.ActionLink("Click here", "Search") %> to search for products <br /></li>
	 <li><% = Html.ActionLink("Click here", "Cart") %> to view your shopping cart</li>
	</ul>

	<br />
	<br />

   
</asp:Content>
