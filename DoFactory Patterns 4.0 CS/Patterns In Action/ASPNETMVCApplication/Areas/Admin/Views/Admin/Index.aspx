<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Administration
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <h1>Administration</h1>
    
    <p>Administration pages like these are usually not available to the public and need
       to be protected. You do this by building a password protected
       administration area accessible only to personnel in your company. The 
       new membership and roles functionality in .NET 2.0 is great for 
       this purpose. There are two main tasks in the administration module: 
    </p>
    
    <br />
    
    <ul>
     <li><% = Html.ActionLink("Click here", "Customers")%> to maintain customers</li>
     <li><% = Html.ActionLink("Click here", "Orders")%> to view orders</li>
	</ul>

</asp:Content>
