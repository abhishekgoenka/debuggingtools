<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Checkout
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Checkout</h1>

    <br />

    <p>
    This is where you would proceed to a checkout process. <br /> 
    Checkout includes collecting information on address, shipping, payment, etc.
    </p>
	
    <ul>
     <li>
	   <a href="/">Click here</a> to return to home page
     </li>
    </ul>

</asp:Content>
