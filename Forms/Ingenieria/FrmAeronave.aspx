<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmAeronave.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmAeronave" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Manto</title>
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
        }
        /* .MyCalendar .ajax__calendar_container {
            border: 1px solid #646464;
            background-color: Gray;
            color: Black;
            font-family: Arial;
            font-size: 14px;
            font-weight: bold;
        }*/
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
        function Decimal(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode == 46) {
                var inputValue = $("#inputfield").val()
                if (inputValue.indexOf('.') < 1) {
                    return true;
                }
                return false;
            }
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function myFuncionddl() {
            $('#<%=DdlCcosto.ClientID%>').chosen();
            $('#<%=DdlBusqHK.ClientID%>').chosen();
            $('#<%=DdlModelo.ClientID%>').chosen();
            $('#<%=DdlTipo.ClientID%>').chosen();
            $('#<%=DdlPropie.ClientID%>').chosen();
            $('#<%=DdlEstado.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
   <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:MultiView ID="MlVwCampos" runat="server">
        <asp:View ID="Vw0LibroVuelo" runat="server">
            <asp:UpdatePanel ID="UpPlHk" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <br /><br />
                    <div class="CentrarContenedor DivMarco">
                        <asp:Label ID="LblBusqHK" runat="server" CssClass="LblEtiquet" Text="Seleccionar una Aeronave:" />
                        <asp:DropDownList ID="DdlBusqHK" runat="server" CssClass="Campos" OnTextChanged="DdlBusqHK_TextChanged" AutoPostBack="true" Width="20%" />
                        <asp:Label ID="LblCodHK" runat="server" CssClass="LblEtiquet" Text="Código:" />
                        <asp:TextBox ID="TxtCodHk" runat="server" CssClass="form-control-sm heightCampo" Width="10%" TextMode="Number" step="0.01" Enabled="false" />
                        <div class="row">
                            <div class="col-sm-12">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitCampos" runat="server" Text="Datos Aeronave" /></h6>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-1">
                                <asp:Label ID="LblMatr" runat="server" CssClass="LblEtiquet" Text="Matrícula:" />
                                <asp:TextBox ID="TxtMatr" runat="server" CssClass="form-control-sm heightCampo" MaxLength="20" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblSn" runat="server" CssClass="LblEtiquet" Text="S/N:" />
                                <asp:TextBox ID="TxtSn" runat="server" CssClass="form-control-sm heightCampo" MaxLength="50" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblCCosto" runat="server" CssClass="LblEtiquet" Text="C. Costo:" />
                                <asp:DropDownList ID="DdlCcosto" runat="server" CssClass="heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFecFabr" runat="server" CssClass="LblEtiquet" Text="Fecha Fabricación:" />
                                <asp:TextBox ID="TxtFecFabr" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" Font-Size="11px" TextMode="Date" />
                            </div>
                            <div class="col-sm-3">
                                <br />
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:CheckBox ID="CkbAdmon" runat="server" Text="" Enabled="false" ForeColor="#990000" />&nbsp&nbsp&nbsp
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:CheckBox ID="CkbPropiedad" runat="server" CssClass="LblEtiquet" Text="" Enabled="false" />&nbsp&nbsp&nbsp
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:CheckBox ID="CkbActiva" runat="server" Text="Activa" Enabled="false" CssClass="LblEtiquet" />
                                    </asp:TableCell>
                                </asp:TableRow>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblModelo" runat="server" CssClass="LblEtiquet" Text="Modelo:" />
                                <asp:DropDownList ID="DdlModelo" runat="server" CssClass="heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblTipo" runat="server" CssClass="LblEtiquet" Text="Tipo:" />
                                <asp:DropDownList ID="DdlTipo" runat="server" CssClass="heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblEstado" runat="server" CssClass="LblEtiquet" Text="Estado:" />
                                <asp:DropDownList ID="DdlEstado" runat="server" CssClass="heightCampo" Enabled="false" Width="100%" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label ID="LblPropie" runat="server" CssClass="LblEtiquet" Text="cliente adminitrador:" />
                                <asp:DropDownList ID="DdlPropie" runat="server" CssClass="heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblNomPropietario" runat="server" CssClass="LblEtiquet" Text="propietario" />
                                <asp:TextBox ID="TxtNomPropietario" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" MaxLength="240" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-8">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitContadores" runat="server" Text="Datos Contadores" /></h6>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblFecIngr" runat="server" CssClass="LblEtiquet" Text="Fecha Ingreso:" />
                                <asp:TextBox ID="TxtFecIngr" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" Font-Size="11px" TextMode="Date" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="LblTSN" runat="server" CssClass="LblEtiquet" Text="TSN:" />
                                <asp:TextBox ID="TxtTSN" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="LblCSN" runat="server" CssClass="LblEtiquet" Text="CSN:" />
                                <asp:TextBox ID="TxtCSN" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" Enabled="false" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblDescri" runat="server" CssClass="LblEtiquet" Text="Descripción:" />
                                <asp:TextBox ID="TxtDescri" runat="server" CssClass="form-control-sm" MaxLength="200" TextMode="MultiLine" Enabled="false" Width="100%" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-6">                               
                                <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success Font_btnCrud" OnClick="BtnIngresar_Click" Text="Ingresar" />
                                <asp:Button ID="BtnModificar" runat="server" CssClass="btn btn-success Font_btnCrud" OnClick="BtnModificar_Click" Text="Modificar" />
                                <asp:Button ID="BtnExpor" runat="server" CssClass="btn btn-primary Font_btnCrud" OnClick="BtnExpor_Click" Text="Exportar" ToolTip="Exportar a Excel todos los reportes" />
                                <asp:Button ID="BtnSolicitud" runat="server" CssClass="btn btn-success Font_btnCrud" OnClick="BtnSolicitud_Click" Text="Solicitud Reparación" ToolTip="Generar solicitud de reparación para que sea cumplida por un tercero" OnClientClick="return confirm('¿Desea generar una solicitud de reparación para la aeronave en pantalla?');" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BtnExpor" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
