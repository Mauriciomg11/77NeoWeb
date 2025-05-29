<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="ExportarTableroMoonFl.aspx.cs" Inherits="_77NeoWeb.ExportarTableroMoonFl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">
        function solonumeros(e) {
            var key;
            if (window.event) // IE
            {
                key = e.keyCode;
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                key = e.which;
            }
            if (key < 48 || key > 57) {
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="CentrarContenedor DivMarco">
                <br />
                <br />
                <br />
                <br />
                <br />
                <asp:RadioButton ID="RdbHK" runat="server" CssClass="LblEtiquet" Text="1 Aeronaves" GroupName="BusqSP" />&nbsp&nbsp&nbsp
                <asp:RadioButton ID="RdbCompContr" runat="server" CssClass="LblEtiquet" Text="2 Componentes_Controlados" GroupName="BusqSP" />&nbsp&nbsp&nbsp
                 <asp:RadioButton ID="RdbOTHH" runat="server" CssClass="LblEtiquet" Text="3 OT_HH" GroupName="BusqSP" />&nbsp&nbsp&nbsp
                 <asp:RadioButton ID="RdbOTRecur" runat="server" CssClass="LblEtiquet" Text="4 OT_Recurso" GroupName="BusqSP" />&nbsp&nbsp&nbsp
                 <asp:RadioButton ID="RdbRpte" runat="server" CssClass="LblEtiquet" Text="5 Reportes" GroupName="BusqSP" />&nbsp&nbsp&nbsp
                 <asp:RadioButton ID="RdbStatus" runat="server" CssClass="LblEtiquet" Text="6 Status_Report" GroupName="BusqSP" />&nbsp&nbsp&nbsp
                 <asp:RadioButton ID="RdbInventario" runat="server" CssClass="LblEtiquet" Text="7 Inventario" GroupName="BusqSP" />&nbsp&nbsp&nbsp
                <div class="row">
                    <div class="col-sm-0">
                        <asp:ImageButton ID="IbnExcel" runat="server" ToolTip="exportar consolidado" CssClass=" BtnExpExcel" Height="50px" Width="50px" ImageUrl="~/images/ExcelV1.png" OnClick="IbnExcel_Click" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4">
                        <asp:Label ID="LblOT" runat="server" CssClass="LblEtiquet" Text="Oden de trabajo" Visible="false" />
                        <asp:TextBox ID="TxtOT" runat="server" CssClass="form-control-sm heightCampo" Width="80%" TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" Text="0" Visible="false" />
                    </div>
                </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="IbnExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
