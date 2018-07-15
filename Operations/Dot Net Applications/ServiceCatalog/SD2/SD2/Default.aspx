<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SD2._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <script type="text/javascript">
    function PanelClick(animal)
    {
        var hdnfldVariable = document.getElementById("FeaturedContent_hdnfldVariable");
        //sessionStorage
        hdnfldVariable.value = animal;
        // hdnfldVariable.textContent = 'foo';
        __doPostBack('Panel1', 'Click');
    }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#FeaturedContent_Panel80").hover(function () {
                $("#FeaturedContent_Panel80").css("background-color", "#E6E3D6");
                $("#FeaturedContent_Panel80").css("cursor", "pointer");
            }, function () {
                $("#FeaturedContent_Panel80").css("background-color", "#F2F1EB");
            });
        });
    </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("#FeaturedContent_Panel81").hover(function () {
                    $("#FeaturedContent_Panel81").css("background-color", "#E6E3D6");
                    $("#FeaturedContent_Panel81").css("cursor", "pointer");
                }, function () {
                    $("#FeaturedContent_Panel81").css("background-color", "#F2F1EB");
                });
            });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#FeaturedContent_Panel82").hover(function ()
            {
                $("#FeaturedContent_Panel82").css("background-color", "#E6E3D6");
                $("#FeaturedContent_Panel82").css("cursor", "pointer");
            }, function ()
            {
                $("#FeaturedContent_Panel82").css("background-color", "#F2F1EB");
            });
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#FeaturedContent_Panel83").hover(function () {
                $("#FeaturedContent_Panel83").css("background-color", "#E6E3D6");
                $("#FeaturedContent_Panel83").css("cursor", "pointer");
            }, function () {
                $("#FeaturedContent_Panel83").css("background-color", "#F2F1EB");
            });
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#FeaturedContent_Panel84").hover(function () {
                $("#FeaturedContent_Panel84").css("background-color", "#E6E3D6");
                $("#FeaturedContent_Panel84").css("cursor", "pointer");
            }, function () {
                $("#FeaturedContent_Panel84").css("background-color", "#F2F1EB");
            });
        });
    </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("#FeaturedContent_Panel85").hover(function () {
                    $("#FeaturedContent_Panel85").css("background-color", "#E6E3D6");
                    $("#FeaturedContent_Panel85").css("cursor", "pointer");
                }, function () {
                    $("#FeaturedContent_Panel85").css("background-color", "#F2F1EB");
                });
            });
    </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("#FeaturedContent_Panel86").hover(function () {
                    $("#FeaturedContent_Panel86").css("background-color", "#E6E3D6");
                    $("#FeaturedContent_Panel86").css("cursor", "pointer");
                }, function () {
                    $("#FeaturedContent_Panel86").css("background-color", "#F2F1EB");
                });
            });
    </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("#FeaturedContent_Panel87").hover(function () {
                    $("#FeaturedContent_Panel87").css("background-color", "#E6E3D6");
                    $("#FeaturedContent_Panel87").css("cursor", "pointer");
                }, function () {
                    $("#FeaturedContent_Panel87").css("background-color", "#F2F1EB");
                });
            });
    </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("#FeaturedContent_Panel88").hover(function () {
                    $("#FeaturedContent_Panel88").css("background-color", "#E6E3D6");
                    $("#FeaturedContent_Panel88").css("cursor", "pointer");
                }, function () {
                    $("#FeaturedContent_Panel88").css("background-color", "#F2F1EB");
                });
            });
    </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("#FeaturedContent_Panel89").hover(function () {
                    $("#FeaturedContent_Panel89").css("background-color", "#E6E3D6");
                    $("#FeaturedContent_Panel89").css("cursor", "pointer");
                }, function () {
                    $("#FeaturedContent_Panel89").css("background-color", "#F2F1EB");
                });
            });
    </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("#FeaturedContent_Panel90").hover(function () {
                    $("#FeaturedContent_Panel90").css("background-color", "#E6E3D6");
                    $("#FeaturedContent_Panel90").css("cursor", "pointer");
                }, function () {
                    $("#FeaturedContent_Panel90").css("background-color", "#F2F1EB");
                });
            });
    </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("#FeaturedContent_Panel91").hover(function () {
                    $("#FeaturedContent_Panel91").css("background-color", "#E6E3D6");
                    $("#FeaturedContent_Panel91").css("cursor", "pointer");
                }, function () {
                    $("#FeaturedContent_Panel91").css("background-color", "#F2F1EB");
                });
            });
    </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("#FeaturedContent_Panel92").hover(function () {
                    $("#FeaturedContent_Panel92").css("background-color", "#E6E3D6");
                    $("#FeaturedContent_Panel92").css("cursor", "pointer");
                }, function () {
                    $("#FeaturedContent_Panel92").css("background-color", "#F2F1EB");
                });
            });
    </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("#FeaturedContent_Panel93").hover(function () {
                    $("#FeaturedContent_Panel93").css("background-color", "#E6E3D6");
                    $("#FeaturedContent_Panel93").css("cursor", "pointer");
                }, function () {
                    $("#FeaturedContent_Panel93").css("background-color", "#F2F1EB");
                });
            });
    </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("#FeaturedContent_Panel94").hover(function () {
                    $("#FeaturedContent_Panel94").css("background-color", "#E6E3D6");
                    $("#FeaturedContent_Panel94").css("cursor", "pointer");
                }, function () {
                    $("#FeaturedContent_Panel94").css("background-color", "#F2F1EB");
                });
            });
    </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("#FeaturedContent_Panel95").hover(function () {
                    $("#FeaturedContent_Panel95").css("background-color", "#E6E3D6");
                    $("#FeaturedContent_Panel95").css("cursor", "pointer");
                }, function () {
                    $("#FeaturedContent_Panel95").css("background-color", "#F2F1EB");
                });
            });
    </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("#FeaturedContent_Panel96").hover(function () {
                    $("#FeaturedContent_Panel96").css("background-color", "#E6E3D6");
                    $("#FeaturedContent_Panel96").css("cursor", "pointer");
                }, function () {
                    $("#FeaturedContent_Panel96").css("background-color", "#F2F1EB");
                });
            });
    </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("#FeaturedContent_Panel97").hover(function () {
                    $("#FeaturedContent_Panel97").css("background-color", "#E6E3D6");
                    $("#FeaturedContent_Panel97").css("cursor", "pointer");
                }, function () {
                    $("#FeaturedContent_Panel97").css("background-color", "#F2F1EB");
                });
            });
    </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $("#FeaturedContent_Panel98").hover(function () {
                    $("#FeaturedContent_Panel98").css("background-color", "#E6E3D6");
                    $("#FeaturedContent_Panel98").css("cursor", "pointer");
                }, function () {
                    $("#FeaturedContent_Panel98").css("background-color", "#F2F1EB");
                });
            });
    </script>






    <section class="featured">
            <asp:HiddenField ID="hdnfldVariable" runat="server" value="foo" />
        <div class="content-wrapper" >
            <asp:Panel ID="Panel1" runat="server" style="z-index: 1; left: 0px; top: 0px; position: absolute; height: 1564px; width: 1300px" BackColor="White">
            </asp:Panel>

            <asp:Panel  Wrap="true" ID="Panel2" style="z-index: 1; left: 17px; top: 67px; position: relative; height: auto; width: 14px"  runat="server" BackColor="Bisque" onclick="PanelClick('goat');" Visible="False">
                
                <asp:Panel ID="PanelLeft" runat="server" Width="14" Height="40" BackColor="SteelBlue">
                    <asp:Label ID="Label2" runat="server" Text="Items   " Font-Size="Medium" ForeColor="Maroon" />
                </asp:Panel>

                <asp:Panel ID="rrr" runat="server" BackColor="Wheat" Width="9" Height="12">











                </asp:Panel>

            </asp:Panel>


        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>W</h3>
    </asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .featured {
            margin-bottom: 133px;
            height: 14px;
        }
    </style>
</asp:Content>

