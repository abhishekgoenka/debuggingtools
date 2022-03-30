<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Error
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <h1>Error</h1>

    <p>
      Sorry an error has occurred. <br /> 
    </p>
	
    <ul>
     <li>
      <% = Html.ActionLink("Click here", "Index", "Home", new { area = "" } ) %> to return to home page
     </li>
    </ul>
    
    <br />
    <br />
    <br />


</asp:Content>
