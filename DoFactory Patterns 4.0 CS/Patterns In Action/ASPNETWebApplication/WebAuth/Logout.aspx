<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPage.Master" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="ASPNETWebApplication.WebAuth.Logout" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <h1>Logout</h1>

    <p>You have successfully logged out.</p> 

    <br />
    
     <ul>
     <li><asp:HyperLink ID="HyperLinkHome" runat="server" NavigateUrl="~/" Text="Click here"></asp:HyperLink> to return to home page</li>
    </ul>
</asp:Content>
