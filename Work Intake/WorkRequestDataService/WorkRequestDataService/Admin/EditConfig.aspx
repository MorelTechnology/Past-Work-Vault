<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditConfig.aspx.cs" Inherits="WorkRequestDataService.EditConfig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        table {
            border-collapse: collapse;
        }

        td {
            padding: 5px;
        }

        tr {
            margin: 5px;
        }

            tr:nth-child(even) {
                background: rgba(128, 128, 128, 0.09);
            }

            tr:nth-child(odd) {
                background: #FFF;
            }

        .auto-style1 {
            text-align: right;
        }

        .auto-style2 {
            color: #FF0000;
        }
    </style>
</head>
<body>
    <asp:Label ID="statusMessage" Visible="False" Font-Size="Large" runat="server" CssClass="auto-style2" Font-Bold="True" />
    <form id="form1" runat="server" aria-orientation="vertical">
        <table>
            <tr>
                <td class="auto-style3"><strong>Application Configuration</strong></td>
                <td class="auto-style4">&nbsp;</td>
                <td class="auto-style8">&nbsp;</td>
                <td class="auto-style7">&nbsp;</td>
            </tr>

            <tr>
                <td class="auto-style3"><strong>E-Mail Settings</strong></td>
                <td class="auto-style4">&nbsp;</td>
                <td class="auto-style8">&nbsp;</td>
                <td class="auto-style7">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style4">SMTP Server</td>
                <td class="auto-style8">
                    <asp:TextBox ID="txtSMTPServer" runat="server" OnInit="txtSMTPServer_Init"></asp:TextBox>
                </td>
                <td class="auto-style7">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style4">SMTP Port</td>
                <td class="auto-style8">
                    <asp:TextBox ID="txtSMTPPort" runat="server" OnInit="txtSMTPPort_Init"></asp:TextBox>
                </td>
                <td class="auto-style1">
                    <asp:Button ID="Save" runat="server" Text="Update Configuration" OnClick="Save_Click" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>