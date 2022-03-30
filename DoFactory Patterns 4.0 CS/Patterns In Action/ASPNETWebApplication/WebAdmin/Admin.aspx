<%@ Page Language="C#" MasterPageFile="~/SiteMasterPage.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="ASPNETWebApplication.WebAdmin.Admin" Title="Administration" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <h1>Administration</h1>
    
    <p>Administration pages like these are usually not available to the public and need
       to be protected. You do this by building a password protected
       administration area accessible only to personnel in your company. The 
       new membership and roles functionality in .NET 2.0 is great for 
       this purpose. There are two main tasks in the administration module: 
    </p>
    
    <br />
    
    <ul>
     <li><a href="/admin/customers">Click here</a> to maintain customers</li>
     <li><a href="/admin/customers/orders">Click here</a> to view orders</li>
	</ul>

</asp:Content>
