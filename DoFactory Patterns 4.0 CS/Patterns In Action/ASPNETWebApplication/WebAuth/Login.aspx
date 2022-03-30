<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ASPNETWebApplication.WebAuth.Login" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
  <h1>Login</h1>
  
  
  <p><br />
    Login is required to access the Administration area. For demonstration purposes 
    use these<br /> credentials: &nbsp;<i>username:</i> '<font color='red'>debbie</font>',
    <i>password:</i> '<font color='red'>secret123</font>'.</p>
  
  <%-- panel allows default button to be set --%>
  
  <asp:Panel ID="Panel1" DefaultButton="ButtonSubmit" runat="server">
  
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
      <tr>
        <td width="10">&nbsp;</td>
        <td>
          <font color="red" face="Arial">
            <asp:Literal runat="server" ID="LiteralError"></asp:Literal></font>
          <br />
          <table bgcolor="#fff5dd" width="340" border="0" cellpadding="0" cellspacing="0">
            <tr>
              <td height="22" bgcolor="#000000" align="left" colspan="2">
                &nbsp;&nbsp;&nbsp;<span style="color:white">please login</span>
              </td>
            </tr>
            <tr>
              <td align="right" height="32">
                username:&nbsp;
              </td>
              <td>
                <asp:TextBox ID="TextboxUserName" runat="server" TextMode="SingleLine" TabIndex="1"
                  Width="200"></asp:TextBox>
              </td>
            </tr>
            <tr>
              <td align="right">
                password:&nbsp;
              </td>
              <td>
                <asp:TextBox ID="TextboxPassword" runat="server" TextMode="Password" TabIndex="2"
                  Width="200"></asp:TextBox>
              </td>
            </tr>
            <tr>
              <td height="40" bgcolor="fff5dd">
                &nbsp;
              </td>
              <td height="40" bgcolor="fff5dd" valign="middle">
                <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" OnClick="ButtonSubmit_Click">
                </asp:Button>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </asp:Panel>
  <br />
  <br />
  <br />
  <br />
</asp:Content>

