<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditSettings.aspx.cs" Inherits="WorkRequestDataService.Admin.EditSettings" %>

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
            width: 382px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" aria-orientation="vertical">
        <table>
            <tr>
                <td class="auto-style3"><strong>Settings</strong></td>
                <td class="auto-style4">&nbsp;</td>
                <td class="auto-style8">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style7">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3" rowspan="2">&nbsp;</td>
                <td class="auto-style4" rowspan="2">Corporate Goal Statements</td>
                <td class="auto-style8" rowspan="2">
                    <asp:TextBox ID="txtCorporateGoals" runat="server" Width="300px"></asp:TextBox>
                    &nbsp;
                    <asp:Button ID="btnAddCorporateGoal" runat="server" Text="Add" OnClick="btnAddCorporateGoal_Click" />
                </td>
                <td class="auto-style1" rowspan="2">
                    <asp:ListBox ID="listCorporateGoals" runat="server" Width="375px" OnInit="listCorporateGoals_Init"></asp:ListBox>
                    &nbsp;
                    <asp:Button ID="btnDeleteCorporateGoal" runat="server" Text="Remove Selected Entry" Style="margin-bottom: 20px;" OnClick="btnDeleteCorporateGoal_Click" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>