<%@ Page Language="C#" MasterPageFile="~/SiteMasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ASPNETWebApplication.Default" Title="ASP.NET Application -- Patterns-in-Action 4.0" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Welcome</h1>
    
    <%--image--%>
    
    <div class="floatright" style="text-align:right;">
      <asp:Image ID="ImageDefault" runat="server" ImageUrl="~/assets/images/app/clouds.jpg" Width="162" Height="232" BorderWidth="0" />
    </div>
        
	<p>
	Thank you for using the Design Pattern Framework.
	You are running <span style="color:#bb1100;">Patterns in Action 4.0 with ASP.NET Web Forms</span> 
    which demonstrates when, where, and how design patterns are 
	used in a modern 3-tier e-commerce web application (please remember that the framework also 
    includes a <span style="color:#bb1100;">ASP.NET MVC 2</span> version of the same). 
	</p>
	
	<p>
	This application has been built around the most frequently used 
	Gang of Four (GoF) design patterns and associated best practices. 
	Also included are numerous Enterprise Patterns 
	as documented in Martin Fowler's book titled: 
    "Patterns of Enterprise Application Architecture"
    SOA and Messaging Patterns are found in the WCF Services layer which this application consumes.
    
	</p> 
	<br />
    <b>Getting Started</b>
    <br /><br />
	<p>
    As a first step, we recommend that you familiarize yourself with the 
    functionality of this application. Select the menu items on the left and 
    explore the options.  Secondly, we suggest you analyze the .NET Solution 
    and Project structure. This will provide a general overview of the 3-tier architecture 
    and some of the major pattern used to build this reference application. Once you 
    have an understanding of the overall design and architecture you'll want to explore  
    the details of the numerous design patterns used throughout the application.  
    Next, you can explore the Service and Hosting layers (Application Facade pattern). 
    Finally, we suggest you examine the other clients (<span style="color:#b00;">ASP.NET MVC</span>, 
    <span style="color:#b00;">Windows Forms</span>, 
    and <span style="color:#b00;">WPF</span>) 
    and confirm that all clients consume the exact same WCF Service, Hosting, Business, and Data layers.
    Please note that <span style="color:#b00;">Silverlight</span> patterns are demonstrated in a separate Solution. 
    </p>
    
    <p>
    <br />
    <b>Where To Find Documentation</b>
    <br /><br />
    Setup, functionality, design, architecture, and design patterns are discussed in
    the accompanying document named: <b>Patterns In Action 4.0.pdf</b>. &nbsp;
    The C# source code itself is generously commented. Finally, all projects in this solution
    come with their own class diagram located in folders named \_UML Diagram\.
    </p>
    
    <br />
    <b>A Great Learning Experience</b>
    <br /><br />
    <p>
    We are confident that the <strong>Patterns-in-Action 4.0</strong> reference application will provide a valuable 
    learning experience on the use of design patterns in the real world. 
    </p>
	<br />
    
    <br />
    <br />
    <br />
    <br />
    


</asp:Content>
