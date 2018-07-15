<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditDenials.aspx.cs" Inherits="WorkRequestDataService.Admin.EditDenials" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <asp:Panel ID="Status" runat="server" Visible="false">
        <h2 style="color: lightgray;">Processing...</h2>
    </asp:Panel>
    <p>
        Below are previously submitted requests, which have been denied. You can use this menu to review the details, or to reactivate a request.
                Reactivating a request will re-submit it.
    </p>
    <form id="frmDeniedRequests" runat="server">
        <div class="auto-style1">
            <asp:GridView ID="DeniedRequests" runat="server" OnInit="DeniedRequests_Init" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="RequestID" HeaderText="Request ID" />
                    <asp:BoundField DataField="Title" HeaderText="Title" />
                    <asp:BoundField DataField="Requestor" HeaderText="Requested By" />
                    <asp:BoundField DataField="Manager" HeaderText="Approved By" />
                    <asp:BoundField DataField="LastModified" HeaderText="Last Modified On" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnViewDetails" runat="server" Text="View Details" OnClick="btnViewDetails_Click" OnClientClick="document.forms[0].target = '_blank';" CommandArgument='<%# Eval("RequestID") %>' />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnReactivate" runat="server" Text="Reactivate" OnClick="btnReactivate_Click" OnClientClick="return confirm('Are you sure you wish to reactivate this Work Request?');" CommandArgument='<%# Eval("RequestID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>