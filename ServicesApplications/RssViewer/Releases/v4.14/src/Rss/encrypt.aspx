<%@ Page Language="C#" AutoEventWireup="true" CodeFile="encrypt.aspx.cs" Inherits="encrypt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
			    <table id="center">
				    <tr>
					    <td align="left"><asp:Label ID="lblRssManager" Runat="server" Font-Size="15" ForeColor="Red">Rss Manager Sample Page</asp:Label></td>
				    </tr>
				    <tr>
					    <td>
						    <table>
							    <tr>
								    <td colspan="2">
									    <asp:HyperLink id="lnkRss" Visible="False" runat="server">HyperLink</asp:HyperLink>
								    </td>
							    </tr>
						    </table>
					    </td>
				    </tr>
			    </table>
    </div>
    </form>
</body>
</html>
