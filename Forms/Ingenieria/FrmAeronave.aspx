<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmAeronave.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmAeronave" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Manto</title>
    <style type="text/css">
        .CentrarContenedor {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 98%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -49%;
            /*determinamos una altura*/
            height: 90%;
            /*indicamos que el margen superior, es la mitad de la altura*/
            padding: 5px;
        }

        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .MyCalendar .ajax__calendar_container {
            border: 1px solid #646464;
            background-color: Gray;
            color: Black;
            font-family: Arial;
            font-size: 14px;
            font-weight: bold;
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
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:MultiView ID="MlVwCampos" runat="server">
        <asp:View ID="Vw0LibroVuelo" runat="server">
            <asp:UpdatePanel ID="UpPlHk" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="CentrarContenedor DivMarco">
                        <asp:Label ID="LblBusqHK" runat="server" CssClass="LblEtiquet" Text="Seleccionar una Aeronave:" />
                        <asp:DropDownList ID="DdlBusqHK" runat="server" CssClass="Campos" OnTextChanged="DdlBusqHK_TextChanged" AutoPostBack="true" Width="20%" />
                        <asp:Label ID="LblCodHK" runat="server" CssClass="LblEtiquet" Text="Código:" />
                        <asp:TextBox ID="TxtCodHk" runat="server" CssClass="form-control-sm heightCampo" Width="10%" TextMode="Number" step="0.01" Enabled="false" />
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitCampos" runat="server" Text="Datos Aeronave" /></h6>
                        <asp:Table ID="TblCampos" runat="server">
                            <asp:TableRow>
                                <asp:TableCell Width="7%">
                                    <asp:Label ID="LblMatr" runat="server" CssClass="LblEtiquet" Text="Matrícula:" />&nbsp
                                    <asp:TextBox ID="TxtMatr" runat="server" CssClass="form-control-sm heightCampo" MaxLength="20" Enabled="false" Width="70%" />
                                </asp:TableCell>
                                <asp:TableCell Width="7%">
                                    <asp:Label ID="LblSn" runat="server" CssClass="LblEtiquet" Text="S/N:" />&nbsp
                                    <asp:TextBox ID="TxtSn" runat="server" CssClass="form-control-sm heightCampo" MaxLength="50" Enabled="false" Width="70%" />
                                </asp:TableCell>
                                <asp:TableCell Width="8%">
                                    <asp:Label ID="LblCCosto" runat="server" CssClass="LblEtiquet" Text="C. Costo:" />&nbsp&nbsp&nbsp&nbsp
                                    <asp:DropDownList ID="DdlCcosto" runat="server" CssClass="heightCampo" Enabled="false" Width="100%" />
                                </asp:TableCell>
                                <asp:TableCell Width="9%">
                                    <asp:Label ID="LblFecFabr" runat="server" CssClass="LblEtiquet" Text="Fecha Fabricación:" />&nbsp
                                     <%--<asp:ImageButton ID="IbtFecFabr" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="19px" Width="15px" Enabled="false" />--%>
                                    <asp:TextBox ID="TxtFecFabr" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="60%" Font-Size="11px" TextMode ="Date" />
                                    <%--<ajaxToolkit:CalendarExtender ID="CldFecFabr" runat="server" CssClass=" MyCalendar" PopupButtonID="IbtFecFabr" TargetControlID="TxtFecFabr" Format="dd/MM/yyyy" />--%>
                                </asp:TableCell>
                                <asp:TableCell Width="3%">
                                    <asp:CheckBox ID="CkbAdmon" runat="server" Text="" Enabled="false" ForeColor="#990000" />
                                </asp:TableCell>
                                <asp:TableCell Width="5%">
                                    <asp:CheckBox ID="CkbPropiedad" runat="server" CssClass="LblEtiquet" Text="" Enabled="false" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="LblModelo" runat="server" CssClass="LblEtiquet" Text="Modelo:" />&nbsp&nbsp&nbsp
                                    <asp:DropDownList ID="DdlModelo" runat="server" CssClass="heightCampo" Enabled="false" Width="100%" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="LblTipo" runat="server" CssClass="LblEtiquet" Text="Tipo:" />&nbsp
                                    <asp:DropDownList ID="DdlTipo" runat="server" CssClass="heightCampo" Enabled="false" Width="100%" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="LblPropie" runat="server" CssClass="LblEtiquet" Text="Propietario:" />&nbsp
                                    <asp:DropDownList ID="DdlPropie" runat="server" CssClass="heightCampo" Enabled="false" Width="80%" />
                                </asp:TableCell>
                                <asp:TableCell ColumnSpan="2">
                                    <asp:Label ID="LblEstado" runat="server" CssClass="LblEtiquet" Text="Estado:" />&nbsp
                                    <asp:DropDownList ID="DdlEstado" runat="server" CssClass="heightCampo" Enabled="false" Width="50%" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:CheckBox ID="CkbActiva" runat="server" Text="Activa" Enabled="false" CssClass="LblEtiquet" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell ColumnSpan="3">
                                    <h6 class="TextoSuperior">
                                        <asp:Label ID="LblTitContadores" runat="server" Text="Datos Contadores" /></h6>
                                </asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="LblFecIngr" runat="server" CssClass="LblEtiquet" Text="Fecha Ingreso:" />&nbsp
                                     <%--<asp:ImageButton ID="IbtFecIngr" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="19px" Width="15px" Enabled="false" />--%>
                                    <asp:TextBox ID="TxtFecIngr" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="60%" Font-Size="11px" TextMode ="Date" />
                                    <%--<ajaxToolkit:CalendarExtender ID="CldFecIng" runat="server" CssClass=" MyCalendar" PopupButtonID="IbtFecIngr" TargetControlID="TxtFecIngr" Format="dd/MM/yyyy" />--%>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="LblTSN" runat="server" CssClass="LblEtiquet" Text="TSN:" />&nbsp
                                     <asp:TextBox ID="TxtTSN" runat="server" CssClass="form-control-sm heightCampo" Width="45%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="LblCSN" runat="server" CssClass="LblEtiquet" Text="CSN:" />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                                     <asp:TextBox ID="TxtCSN" runat="server" CssClass="form-control-sm heightCampo" Width="45%" TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" Enabled="false" />
                                </asp:TableCell>
                                <asp:TableCell ColumnSpan="3">
                                    <asp:Label ID="LblDescri" runat="server" CssClass="LblEtiquet" Text="Descripción:" />&nbsp
                                    <asp:TextBox ID="TxtDescri" runat="server" CssClass="form-control-sm" MaxLength="200" TextMode="MultiLine" Enabled="false" Width="80%" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="6">
                                    <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success botones BtnEdicion" OnClick="BtnIngresar_Click" Text="Ingresar" />&nbsp
                                <asp:Button ID="BtnModificar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnModificar_Click" Text="Modificar" />&nbsp                               
                                  <asp:Button ID="BtnExpor" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnExpor_Click" Text="Exportar" ToolTip="Exportar a Excel todos los reportes" />&nbsp
                                 <asp:Button ID="BtnSolicitud" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnSolicitud_Click" Text="Solicitud Reparación" ToolTip="Generar solicitud de reparación para que sea cumplida por un tercero" OnClientClick="return confirm('¿Desea generar una solicitud de reparación para la aeronave en pantalla?');" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BtnExpor" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
