<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CommWeb._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">


    <script type="text/javascript">
        function highlight(tableRow, active)
        {
            if (active)
            {
                 tableRow.style.backgroundColor = '#cfc';
            }
            else
            {
                 tableRow.style.backgroundColor = '#fff';
            }
        }

        function select(tableRow)
        {
            var hdnfldVariable = document.getElementById("FeaturedContent_hdnfldVariable");
            //sessionStorage
            //hdnfldVariable.value = tabl;
            // hdnfldVariable.textContent = 'foo';
            hdnfldVariable.value = tableRow.cells[0].innerText;
            //hdnfldVariable.value = tableRow.innerHTML;
            __doPostBack('Table1', 'Click');

            //if (tableRow.style.backgroundColor != '#adf')
            //{
            //    //tableRow.style.backgroundColor = '#cef';
            //    tableRow.style.backgroundColor = '#adf';
            //}
            //else
            //{
            //    tableRow.style.backgroundColor = '#fff';
            //}
        }

function link(Url)
{
document.location.href = Url;
}

function clearlabel() {
    document.getElementById('FeaturedContent_LabelWait').innerText = " ";
    FeaturedContent_LabelWait.innerText = " ";
    alert('foo');
}

    </script>


    <section class="featured">
        <asp:HiddenField ID="hdnfldVariable" runat="server" value="foo" />
        <div class="content-wrapper">
            <asp:Panel ID="Panel3" runat="server"  style="z-index: 1; left: 19px; top: 5px; position: absolute; height: 59px; width: 1353px" BackColor="#F2E7E1" >
                    <asp:Image ID="Image2" runat="server" style="z-index: 104; left: 15px; top: 10px; position: absolute; width: 78px; height: 40px" Height="79px" ImageUrl="~/Images/RS.png" />
                    <asp:Label ID="Label3" runat="server" style="z-index: 104; left: 111px; top: 16px; position: absolute; width: 288px; height: 35px" Text="Commutation Tool" Font-Names="Arial" Font-Bold="True" Font-Size="X-Large" ForeColor="DimGray"></asp:Label>
                    <asp:Label ID="lblUserLegend" runat="server" style="z-index: 104; left: 1162px; top: 10px; position: absolute; width: 169px; height: 23px; text-align:center; display:inline-block;" Text="Welcome" Font-Names="Arial" Font-Bold="True" Font-Size="Large" ForeColor="DimGray"></asp:Label>
                    <asp:Label ID="lblUser" runat="server" style="z-index: 104; left: 1162px; top: 32px; position: absolute; width: 169px; height: 23px; text-align:center; display:inline-block;" Text="" Font-Names="Arial" Font-Bold="True" Font-Size="Small" ForeColor="DimGray"></asp:Label>
            </asp:Panel>
            <asp:Table ID="Table1" Style="z-index: 103;position: absolute; top:386px; left:22px; height: 118px; width: 792px;"  runat="server" GridLines="Both" ViewStateMode="Enabled" Font-Names="Arial" Font-Size="Small">
            </asp:Table>
                
            <asp:Label ID="lblGridHeader" runat="server" style="z-index: 1; left: 24px; top: 365px; position: absolute; width: 1345px; height: 18px" Text="   " Font-Names="Arial" Font-Bold="True"></asp:Label>

            <asp:Panel ID="pnlNewCommID" runat="server" style="z-index: 1; left: 19px; top: 68px; position: absolute; height: 218px; width: 1353px" BackColor="#F2E7E1" Visible="False">
                <asp:Label ID="lblCommNameMayNotBeBlank" runat="server" style="z-index: 1; left: 216px; top: 112px; position: absolute; width: 234px; height: 44px" Text="The Comm Name may not be blank" Font-Bold="True" Font-Names="Arial" ForeColor="#990000" Font-Size="Small"></asp:Label>
                <asp:Label ID="lblCommIDMayNotBeBlank" runat="server" style="z-index: 1; left: 57px; top: 112px; position: absolute; width: 125px; height: 44px" Text="The Comm ID may not be blank" Font-Bold="True" Font-Names="Arial" ForeColor="#990000" Font-Size="Small"></asp:Label>
                <asp:Label ID="lblCommNameTooLong" runat="server" style="z-index: 1; left: 215px; top: 112px; position: absolute; width: 203px; height: 44px" Text="The Comm Name must not be longer than 10 characters" Font-Bold="True" Font-Names="Arial" ForeColor="#990000" Font-Size="Small"></asp:Label>
                <asp:Label ID="Label7" runat="server" style="z-index: 1; left: 216px; top: 50px; position: absolute; width: 172px; height: 18px" Text="Enter Comm Name" Font-Bold="True" Font-Names="Arial" ForeColor="DimGray" Font-Size="Small"></asp:Label>
                <asp:Label ID="Label5" runat="server" style="z-index: 1; left: 24px; top: 10px; position: absolute; width: 229px; height: 21px" Text="Create new Comm ID" Font-Bold="True" Font-Names="Arial" ForeColor="DimGray" Font-Size="Large"></asp:Label>
                <asp:Label ID="Label6" runat="server" style="z-index: 1; left: 57px; top: 50px; position: absolute; width: 144px; height: 18px" Text="Enter Comm ID" Font-Bold="True" Font-Names="Arial" ForeColor="DimGray" Font-Size="Small"></asp:Label>
                <asp:Button ID="btnCreateCommID" runat="server" Height="40px" OnClick="btnCreateCommID_Click" Style="z-index: 103;position: absolute; top:61px; left:468px; width: 128px;" Text="Create" />
                <asp:Button ID="btnCancelCreateCommID" runat="server" Height="40px" OnClick="btnCancelCreateCommID_Click" Style="z-index: 103;position: absolute; top:61px; left:607px; width: 128px;" Text="Cancel" />
            <asp:TextBox ID="txtCommName" runat="server" Style="z-index: 103;position: absolute; top:65px; height:26px; left:214px; width: 192px;" BorderColor="#E2E2E2" Height="26px" BorderStyle="Solid" BorderWidth="1px" />
            <asp:TextBox ID="txtCommID" runat="server" Style="z-index: 103;position: absolute; top:65px; height:26px; left:55px; width: 116px;" BorderColor="#E2E2E2" Height="26px" BorderStyle="Solid" BorderWidth="1px" />
            </asp:Panel>

            <asp:Panel ID="pnlControls" runat="server" style="z-index: 1; left: 19px; top: 68px; position: absolute; height: 218px; width: 1353px" BackColor="#F2E7E1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Label ID="LabelWait" runat="server" style="z-index: 104; left: 902px; top: 4px; position: absolute; width: 356px; height: 35px" Text="Please Wait..." Font-Names="Arial" Font-Bold="True" Font-Size="XX-Large"></asp:Label>
            </ContentTemplate>
            </asp:UpdatePanel>
            <asp:ListBox ID="lbAvailableCommID" runat="server"  Style="z-index: 103;position: absolute; top:32px; left:20px; height: 162px; width: 244px; margin-left: 0px;"  ></asp:ListBox>
            <asp:ListBox ID="lbSelectedCommID" runat="server"  Style="z-index: 103;position: absolute; top:32px; left:421px; height: 162px; width: 244px; margin-left: 0px;" OnSelectedIndexChanged="lbSelectedCommID_SelectedIndexChanged" AutoPostBack="True"  ></asp:ListBox>
                <asp:Button ID="btnAddCommID" runat="server" Height="40px" OnClick="btnAddCommID_Click" Style="z-index: 103;position: absolute; top:64px; left:279px; width: 128px;" Text="Add  &gt;&gt;&gt;" />
                <asp:Button ID="btnRemoveCommID" runat="server" Height="40px" OnClick="btnRemoveCommID_Click" Style="z-index: 103;position: absolute; top:115px; left:279px; width: 128px;" Text="&lt;&lt;&lt;  Remove" />
                <asp:Label ID="Label1" runat="server" style="z-index: 1; left: 24px; top: 10px; position: absolute; width: 229px; height: 18px" Text="Available Comm ID's" Font-Bold="True" Font-Names="Arial" ForeColor="DimGray"></asp:Label>
                <asp:Label ID="Label2" runat="server" style="z-index: 1; left: 425px; top: 10px; position: absolute; width: 203px; height: 18px" Text="Selected Comm ID's" Font-Names="Arial" Font-Bold="True" ForeColor="DimGray"></asp:Label>
                <asp:Button ID="btnExposure" runat="server" OnClick="btnExposure_Click" Text="Generate Exposure Spreadsheet" Height="40px" Style="z-index: 103;position: absolute; top:40px; left:899px; width: 396px;" />
                <asp:Button ID="btnOpenBalance" runat="server" Height="40px" OnClick="btnOpenBalance_Click" Style="z-index: 103;position: absolute; top:146px; left:900px; width: 396px;" Text="Generate Open Balance Spreadsheet" />
                <asp:Button ID="btnContract" runat="server" OnClick="btnContract_Click" Style="z-index: 103;position: absolute; top:93px; left:900px; width: 396px; height: 39px;" Text="Generate Contract Spreadsheet" />
                <asp:Button ID="btnViewCommIDS" runat="server" OnClick="btnViewCommIDS_Click" Style="z-index: 103;position: absolute; top:40px; left:689px; width: 181px; height: 94px; bottom: 73px;" Text="View Comm ID List" />
                <asp:Button ID="btnAddNewCommID" runat="server" OnClick="btnAddNewCommID_Click" Style="z-index: 103;position: absolute; top:146px; left:689px; width: 181px; height: 40px; bottom: 21px;" Text="Add New Comm ID" />
            </asp:Panel>
                
            <asp:Panel ID="Panel2" runat="server"  style="z-index: 1; left: 19px; top: 290px; position: absolute; height: 59px; width: 1353px" BackColor="#F2E7E1" >
            <asp:Button ID="btnClearCommID" runat="server" Height="40px" Style="z-index: 103;position: absolute; top:9px; left:22px; width: 396px;" OnClick="btnClearCommID_Click" Visible="False" />
            <asp:Button ID="btnSetCommID" runat="server" Height="40px" Style="z-index: 103;position: absolute; top:9px; left:431px; width: 457px;" OnClick="btnSetCommID_Click" Visible="False" />
            <asp:TextBox ID="txtSearchBox" runat="server" Style="z-index: 103;position: absolute; top:14px; height:26px; left:1107px; width: 192px;" BorderColor="#E2E2E2" Height="26px" BorderStyle="Solid" BorderWidth="1px" />
                <asp:ImageButton ID="Image1" runat="server" ImageUrl="~/Images/search.png" Style="z-index: 103;position: absolute; top:17px; left:1314px; width: auto;" OnClick="btnSearch_Click"  />
            </asp:Panel>
        </div>
    </section>
</asp:Content>
