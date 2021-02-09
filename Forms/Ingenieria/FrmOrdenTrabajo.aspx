<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmOrdenTrabajo.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmOrdenTrabajo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>OT</title>
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .TitMRO {
            width: 80%;
        }

        .TitDatosOT {
            width: 98%;
        }

        .TitOTTiempo {
            width: 52%;
        }

        .MyCalendar {
            border: 1px solid #646464;
            background-color: Gray;
            color: Black;
            font-family: Arial;
            font-size: 14px;
            font-weight: bold;
        }

        .LicenciaRva {
            width: 50%;
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
            $('#<%=DdlMroTaller.ClientID%>').chosen();
            $('#<%=DdlBusqOT.ClientID%>').chosen();
            $('#<%=DdlOTBase.ClientID%>').chosen();
            $('#<%=DdlOTAero.ClientID%>').chosen();
            $('#<%=DdlOtEstado.ClientID%>').chosen();
            $('#<%=DdlOtEstaSec.ClientID%>').chosen();
            $('#<%=DdlOtInsp.ClientID%>').chosen();
            $('#<%=DdlOtLicInsp.ClientID%>').chosen();
            $('#<%=DdlOtRespons.ClientID%>').chosen();
            $('#<%=DdlPasoEstado.ClientID%>').chosen();
            $('#<%=DdlOtCCosto.ClientID%>').chosen();
            $('#<%=DdlBusqRte.ClientID%>').chosen();
            $('#<%=DdlTipRte.ClientID%>').chosen();
            $('#<%=DdlFuente.ClientID%>').chosen();
            $('#<%=DdlTall.ClientID%>').chosen();
            $('#<%=DdlRteEstad.ClientID%>').chosen();
            $('#<%=DdlRteClasf.ClientID%>').chosen();
            $('#<%=DdlCatgr.ClientID%>').chosen();
            $('#<%=DdlPosRte.ClientID%>').chosen();
            $('#<%=DdlAtaRte.ClientID%>').chosen();
            $('#<%=DdlGenerado.ClientID%>').chosen();
            $('#<%=DdlLicGene.ClientID%>').chosen();
            $('#<%=DdlBasRte.ClientID%>').chosen();
            $('#<%=DdlCumpl.ClientID%>').chosen();
            $('#<%=DdlLicCump.ClientID%>').chosen();
            $('#<%=DdlPnRte.ClientID%>').chosen();
            $('#<%=DdlTecDif.ClientID%>').chosen();
            $('#<%=DdlVerif.ClientID%>').chosen();
            $('#<%=DdlLicVer.ClientID%>').chosen();
            $('#<%=DdlAeroRte.ClientID%>').chosen();
            $('#<%=DdlPasoTec.ClientID%>').chosen();
            $('#<%=DdlPasoLicTec.ClientID%>').chosen();
            $('#<%=DdlPasoInsp.ClientID%>').chosen();
            $('#<%=DdlPasoLicInsp.ClientID%>').chosen();
            $('#<%=DdlPrioridadOT.ClientID%>').chosen();
            $('[id *=DdlOTTecPP]').chosen();
            $('[id *=DdlOTLicPP]').chosen();
            $('[id *=DdlOTPNRFPP]').chosen();
            $('[id *=DdlRazonR]').chosen();
            $('[id *=DdlPosic]').chosen();
            $('[id *=DdlPNOn]').chosen();
            $('[id *=DdlPNOff]').chosen();
            $('[id *=DdlPNHta]').chosen();
            $('[id *=DdlLicenRFPP]').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatosPpal" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div runat="server" class="table-responsive">
                <asp:Label ID="LblOt" runat="server" CssClass="LblEtiquet" Text="O.T.:" />
                <asp:TextBox ID="TxtOt" runat="server" CssClass="Form-control-sm heightCampo" Width="10%" step="0.01" Enabled="false" />
                <asp:Label ID="LblOtPpal" runat="server" CssClass="LblEtiquet" Text="O.T. Master:" />
                <asp:TextBox ID="TxtOtPpal" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" step="0.01" Enabled="false" />
                <asp:Label ID="LblOtReporte" runat="server" CssClass="LblEtiquet" Text="Reporte:" />
                <asp:TextBox ID="TxtOtReporte" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" step="0.01" Enabled="false" />
                <asp:Label ID="LblOtRepacion" runat="server" CssClass="LblEtiquet" Text="Reparación:" />
                <asp:TextBox ID="TxtOtRepacion" runat="server" CssClass="Form-control-sm heightCampo" Width="10%" Enabled="false" />
                <asp:Label ID="LblOtPrioridad" runat="server" CssClass="LblEtiquet" Text="Prioridad:" />
                <asp:TextBox ID="TxtlOtPrioridad" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" Enabled="false" />
                <asp:Label ID="LblOtWS" runat="server" CssClass="LblEtiquet" Text="Work Sheet:" />
                <asp:TextBox ID="TxtOtWS" runat="server" CssClass="Form-control-sm heightCampo" Width="10%" Enabled="false" />
                <asp:Label ID="LblTitCancel" runat="server" CssClass="LblEtiquet" Text="CANCELADA" ForeColor="#800000" Font-Size="20px" Font-Bold="true" Visible="false" />
                <h6 class="TextoSuperior">
                    <asp:Label ID="LblTitoTGral" runat="server" Text="Datos Generales" /></h6>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:MultiView ID="MlVwOT" runat="server">
        <asp:View ID="Vw0OT" runat="server">
            <asp:UpdatePanel ID="UplOT" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="TitMRO">
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitCrearEDatosE" runat="server" Text="Datos MRO" /></h6>
                    </div>
                    <div class="table-responsive">
                        <asp:CheckBox ID="CkbEjePasos" runat="server" CssClass="LblEtiquet" Text="&nbspEjecutar pasos" Enabled="false" Visible="false" />
                        <asp:Button ID="BtnMroInsPre" CssClass="btn btn-outline-primary" runat="server" Text="Inspección Preliminar" OnClick="BtnMroInsPre_Click" Font-Size="11px" Visible="false" />
                        <asp:Button ID="BtnMroPrDes" CssClass="btn btn-outline-primary" runat="server" Text="Pruebas antes de desarme" OnClick="BtnMroPrDes_Click" Font-Size="11px" Visible="false" />
                        <asp:Button ID="BtnMroRteDes" CssClass="btn btn-outline-primary" runat="server" Text="Reporte del desarme" OnClick="BtnMroRteDes_Click" Font-Size="11px" Visible="false" />
                        <asp:Button ID="BtnMroDanOc" CssClass="btn btn-outline-primary" runat="server" Text="Daños Escondidos" OnClick="BtnMroDanOc_Click" Font-Size="11px" Visible="false" />
                        <asp:Button ID="BtnMroAccCorr" CssClass="btn btn-outline-primary" runat="server" Text="Acción Correctiva" OnClick="BtnMroAccCorr_Click" Font-Size="11px" Visible="false" />
                        <asp:Button ID="BtnMroPrueF" CssClass="btn btn-outline-primary" runat="server" Text="Prueba final" OnClick="BtnMroPrueF_Click" Font-Size="11px" Visible="false" />
                        <asp:Button ID="BtnMroCumpl" CssClass="btn btn-outline-primary" runat="server" Text="Cumplido / Verificado" OnClick="BtnMroCumpl_Click" Font-Size="11px" Visible="false" />
                        <asp:Button ID="BtnMroTrabEje" CssClass="btn btn-info" runat="server" Text="Trabajo ejecutado" OnClick="BtnMroTrabEje_Click" Font-Size="11px" Visible="false" />
                    </div>
                    <div>
                        <asp:Label ID="LblMroPpt" runat="server" CssClass="LblEtiquet" Text="Propuesta:" Visible="false" />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                        <asp:TextBox ID="TxtMroPpt" runat="server" CssClass="Form-control-sm heightCampo" Width="8%" step="0.01" Enabled="false" Visible="false" />
                        <asp:Label ID="LblMroCliente" runat="server" CssClass="LblEtiquet" Text="Cliente:" Visible="false" />
                        <asp:TextBox ID="TxtMroCliente" runat="server" CssClass="Form-control-sm heightCampo" Width="40%" Enabled="false" Visible="false" />
                        <asp:Label ID="LblMroTaller" runat="server" CssClass="LblEtiquet" Text="Taller:" Visible="false" />
                        <asp:DropDownList ID="DdlMroTaller" runat="server" CssClass="heightCampo" Width="15%" Enabled="false" Visible="false" />
                    </div>
                    <br />
                    <div class="TitDatosOT">
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitDatosGener" runat="server" Text="Datos Orden de Trabajo" /></h6>
                    </div>
                    <div>
                        <asp:Label ID="LblBusqOT" runat="server" CssClass="LblEtiquet" ForeColor="#800000" Font-Bold="true" Text="Consultar:" />
                        <asp:DropDownList ID="DdlBusqOT" runat="server" CssClass="heightCampo" Width="10%" OnTextChanged="DdlBusqOT_TextChanged" AutoPostBack="true" />&nbsp&nbsp
                        <asp:Label ID="LblAplicab" runat="server" CssClass="LblEtiquet" Text="Aplicabilidad:" />
                        <asp:TextBox ID="TxtAplicab" runat="server" CssClass="Form-control-sm heightCampo" Width="18%" Enabled="false" />
                        <asp:Label ID="LblOtPN" runat="server" CssClass="LblEtiquet" Text="P/N:" />
                        <asp:TextBox ID="TxtOtPN" runat="server" CssClass="Form-control-sm heightCampo" Width="18%" Enabled="false" />
                        <asp:Label ID="LblOTAero" runat="server" CssClass="LblEtiquet" Text="Aeronave:" />
                        <asp:DropDownList ID="DdlOTAero" runat="server" CssClass="heightCampo" Width="10%" Enabled="false" />
                        <asp:Label ID="LblOtCCosto" runat="server" CssClass="LblEtiquet" Text="C.costo:" />
                        <asp:DropDownList ID="DdlOtCCosto" runat="server" CssClass="heightCampo" Width="16%" Enabled="false" />                        
                    </div>
                    <div>
                        <asp:Label ID="LblOtEstado" runat="server" CssClass="LblEtiquet" Text="Estado:" />
                        <asp:DropDownList ID="DdlOtEstado" runat="server" CssClass="heightCampo" Width="16%" Enabled="false" OnTextChanged="DdlOtEstado_TextChanged" AutoPostBack="true" />
                        <asp:Label ID="LblOtEstaSec" runat="server" CssClass="LblEtiquet" Text="Estado Secundario:" />
                        <asp:DropDownList ID="DdlOtEstaSec" runat="server" CssClass="heightCampo" Width="10%" Enabled="false" />
                        <asp:Label ID="LblOTFechReg" runat="server" CssClass="LblEtiquet" Text="Fecha Registro:" />
                        <asp:TextBox ID="TxtOTFechReg" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="8%" />
                        <asp:Label ID="LblOTFechini" runat="server" CssClass="LblEtiquet" Text="Fecha Inicial:" />
                        <asp:TextBox ID="TxtOTFechini" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="8%" />
                        <asp:Label ID="LblOTFechFin" runat="server" CssClass="LblEtiquet" Text="Fecha Final:" />
                        <asp:TextBox ID="TxtOTFechFin" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="8%" />
                        <asp:Label ID="LblOTFechVenc" runat="server" CssClass="LblEtiquet" Text="Fecha Vence:" />
                        <asp:ImageButton ID="IbtOTFechVenc" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" Enabled="false" />
                        <asp:TextBox ID="TxtOTFechVenc" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="8%" />
                        <ajaxToolkit:CalendarExtender ID="CalOTFechVenc" CssClass=" MyCalendar" runat="server" PopupButtonID="IbtOTFechVenc" TargetControlID="TxtOTFechVenc" Format="dd/MM/yyyy" />
                    </div>
                    <div>
                        <asp:Label ID="lblOtInsp" runat="server" CssClass="LblEtiquet" Text="Inspector:" />
                        <asp:DropDownList ID="DdlOtInsp" runat="server" CssClass="heightCampo" Width="20%" Enabled="false" OnTextChanged="DdlOtInsp_TextChanged" AutoPostBack="true" />
                        <asp:Label ID="lblOtLicInsp" runat="server" CssClass="LblEtiquet" Text="Licencia:" />
                        <asp:DropDownList ID="DdlOtLicInsp" runat="server" CssClass="heightCampo" Width="8%" Enabled="false" />
                        <asp:Label ID="lblOtRespons" runat="server" CssClass="LblEtiquet" Text="Responsable:" />
                        <asp:DropDownList ID="DdlOtRespons" runat="server" CssClass="heightCampo" Width="20%" Enabled="false" />
                        <asp:Label ID="LblOTBase" runat="server" CssClass="LblEtiquet" Text="Base:" />
                        <asp:DropDownList ID="DdlOTBase" runat="server" CssClass="heightCampo" Width="16%" Enabled="false" />
                        <asp:CheckBox ID="CkbCancel" runat="server" CssClass="LblEtiquet" Text="&nbspCancelar O.T." Enabled="false" />&nbsp&nbsp
                        <asp:CheckBox ID="CkbOtBloqDet" runat="server" CssClass="LblEtiquet" Text="&nbspRercurso Bloqueado" Enabled="false" />
                    </div>
                    <div class="table-responsive">
                        <asp:Table runat="server" Width="98%">
                            <asp:TableRow>
                                <asp:TableCell Width="1%" >
                                    <asp:Label ID="LblOTTrabajo" runat="server" CssClass="LblEtiquet" Text="Trabajo Requerido:" />
                                </asp:TableCell>
                                <asp:TableCell Width="38%" >
                                    <asp:TextBox ID="TxtOTTrabajo" runat="server" CssClass="form-control-sm" TextMode="MultiLine" MaxLength="240" Width="100%" Font-Size="10px" Enabled="false" Height="50px" />
                                </asp:TableCell>
                                <asp:TableCell Width="1%">
                                    <asp:Label ID="LblOTAccParc" runat="server" CssClass="LblEtiquet" Text="Acción Parcial:" />
                                </asp:TableCell>
                                <asp:TableCell Width="38%">
                                    <asp:TextBox ID="TxtOTAccParc" runat="server" CssClass="form-control-sm" TextMode="MultiLine" MaxLength="240" Width="100%" Font-Size="10px" Enabled="false" Height="50px"/>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                    <div class="TitOTTiempo">
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitOtTiempo" runat="server" Text="Tiempos" /></h6>
                    </div>
                    <div>
                        <asp:Label ID="LblTSN" runat="server" CssClass="LblEtiquet" Text="TSN:" />
                        <asp:TextBox ID="TxtTSN" runat="server" CssClass="form-control-sm heightCampo" Width="6%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                        <asp:Label ID="LblTSO" runat="server" CssClass="LblEtiquet" Text="TSO:" />
                        <asp:TextBox ID="TxtTSO" runat="server" CssClass="form-control-sm heightCampo" Width="6%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                        <asp:Label ID="LblTSR" runat="server" CssClass="LblEtiquet" Text="TSR:" />
                        <asp:TextBox ID="TxtTSR" runat="server" CssClass="form-control-sm heightCampo" Width="6%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                        <asp:Label ID="LblCSN" runat="server" CssClass="LblEtiquet" Text="CSN:" />
                        <asp:TextBox ID="TxtCSN" runat="server" CssClass="form-control-sm heightCampo" Width="6%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                        <asp:Label ID="LblCSO" runat="server" CssClass="LblEtiquet" Text="CSO:" />
                        <asp:TextBox ID="TxtCSO" runat="server" CssClass="form-control-sm heightCampo" Width="6%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                        <asp:Label ID="LblCSR" runat="server" CssClass="LblEtiquet" Text="CSR:" />
                        <asp:TextBox ID="TxtCSR" runat="server" CssClass="form-control-sm heightCampo" Width="6%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                    </div>
                    <br />
                    <div id="Botones">
                        <asp:Button ID="BtnOtModificar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnOtModificar_Click" Text="Modificar" />
                        <asp:Button ID="BtnOTDetTec" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnOTDetTec_Click" Text="Técnicos" />
                        <asp:Button ID="BtnOTReserva" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnOTReserva_Click" Text="Reserva" />
                        <asp:Button ID="BtnOTConsultar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnOTConsultar_Click" Text="Consultar" />
                        <asp:Button ID="BtnOTImprimir" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnOTImprimir_Click" Text="Imprimir" />
                        <asp:Button ID="BtnOTEliminar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnOTEliminar_Click" Text="Eliminar" OnClientClick="return confirm('¿Desea eliminar el registro?');" />
                        <asp:Button ID="BtnOTReporte" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnOtReporte_Click" Text="Reportes" />
                        <asp:Button ID="BtnOTAbiertas8PasCump" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnOtAbiertas8PasCump_Click" Text="O.T. Abiertas" ToolTip="O.T. abiertas con el paso 8 cumplido" Visible="false" />
                        <asp:Button ID="BtNOTExportar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtNOTExportar_Click" Text="Exportar" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DdlBusqOT" EventName="TextChanged" />
                    <asp:PostBackTrigger ControlID="BtnOTDetTec" />
                    <asp:PostBackTrigger ControlID="BtnOTReserva" />
                    <asp:PostBackTrigger ControlID="BtnOTConsultar" />
                    <asp:PostBackTrigger ControlID="BtnOTImprimir" />
                    <asp:PostBackTrigger ControlID="BtnOtReporte" />
                    <asp:PostBackTrigger ControlID="BtNOTExportar" />
                    <asp:PostBackTrigger ControlID="BtnOTAbiertas8PasCump" />
                    <asp:PostBackTrigger ControlID="BtnMroInsPre" />
                    <asp:PostBackTrigger ControlID="BtnMroPrDes" />
                    <asp:PostBackTrigger ControlID="BtnMroRteDes" />
                    <asp:PostBackTrigger ControlID="BtnMroDanOc" />
                    <asp:PostBackTrigger ControlID="BtnMroAccCorr" />
                    <asp:PostBackTrigger ControlID="BtnMroPrueF" />
                    <asp:PostBackTrigger ControlID="BtnMroCumpl" />
                    <asp:PostBackTrigger ControlID="BtnMroTrabEje" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw1OTDetTec" runat="server">
            <asp:UpdatePanel ID="UplOTDetTec" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitOTDetTec" runat="server" Text="Datos de los técnicos" /></h6>
                    <asp:ImageButton ID="IbtCerrarOTDetTec" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarOTDetTec_Click" ImageAlign="Right" />

                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="LblBusqOTDetTec" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                            <td>
                                <asp:TextBox ID="TxtConsulOTDetTec" runat="server" Width="350px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                            <td>
                                <asp:ImageButton ID="IbtConsOTDetTec" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsOTDetTec_Click" /></td>
                        </tr>
                    </table>
                    <div id="GridOTDetTec">
                        <asp:GridView ID="GrdOTDetTec" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodIdDetTecniOT"
                            CssClass="DiseñoGrid table-sm" GridLines="Both" AllowPaging="true" PageSize="10" Width="95%"
                            OnRowCommand="GrdOTDetTec_RowCommand" OnRowEditing="GrdOTDetTec_RowEditing" OnRowUpdating="GrdOTDetTec_RowUpdating" OnRowCancelingEdit="GrdOTDetTec_RowCancelingEdit"
                            OnRowDeleting="GrdOTDetTec_RowDeleting" OnRowDataBound="GrdOTDetTec_RowDataBound" OnPageIndexChanging="GrdOTDetTec_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblOTFecTrabP" Text='<%# Eval("FechaTrabajo") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtOTFecTrab" Text='<%# Eval("FechaTrabajo") %>' runat="server" Width="75%" Enabled="false" />
                                        <asp:ImageButton ID="IbtOTFecTrab" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                        <ajaxToolkit:CalendarExtender ID="CalOTFecTrab" runat="server" PopupButtonID="IbtOTFecTrab" TargetControlID="TxtOTFecTrab" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtOTFecTrabPP" runat="server" Width="75%" Enabled="false" />
                                        <asp:ImageButton ID="IbtOTFecTrabPP" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                        <ajaxToolkit:CalendarExtender ID="CalOTFecTrabPP" runat="server" PopupButtonID="IbtOTFecTrabPP" TargetControlID="TxtOTFecTrabPP" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Técnico" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblOTTecP" Text='<%# Eval("Tecnico") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="LblOTTec" Text='<%# Eval("Tecnico") %>' runat="server" Width="100%" Enabled="false" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="DdlOTTecPP" runat="server" CssClass="heightCampo" Width="95%" OnTextChanged="DdlOTTecPP_TextChanged" AutoPostBack="true" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Licencia" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblOTLicP" Text='<%# Eval("NumLicenciaT") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="LblOTLic" Text='<%# Eval("NumLicenciaT") %>' runat="server" Width="100%" Enabled="false" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="DdlOTLicPP" runat="server" CssClass="heightCampo" Width="95%" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Horas" HeaderStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblNumHorasP" Text='<%# Eval("NumHoras") %>' runat="server" Width="100%" TextMode="Number" step="0.01" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtNumHoras" Text='<%# Eval("NumHoras") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtNumHorasPP" runat="server" Width="100%" Text="0" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Datos Pasos" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("DatosPasos") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Text='<%# Eval("DatosPasos") %>' runat="server" Width="100%" Enabled="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField FooterStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                        <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                        <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />

                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="GridFooterStyle" />
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarOTDetTec" />
                    <asp:PostBackTrigger ControlID="IbtConsOTDetTec" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw2OTRecurso" runat="server">
            <asp:UpdatePanel ID="UplOTRecurso" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="LblRecFRte" runat="server" CssClass="LblEtiquet" Text="Reporte:" />
                    <asp:TextBox ID="TxtRecurNumRte" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" step="0.01" Enabled="false" />
                    <asp:Label ID="LblRecFSubOt" runat="server" CssClass="LblEtiquet" Text="Sub OT / Reserva:" />
                    <asp:TextBox ID="TxtRecurSubOt" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" step="0.01" Enabled="false" />
                    <asp:Label ID="LblPrioridadOT" runat="server" CssClass="LblEtiquet" Text="Prioridad:" />
                    <asp:DropDownList ID="DdlPrioridadOT" runat="server" CssClass="Campos" Width="15%" />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTtlOTRecur" runat="server" Text="Recurso Físico" /></h6>
                    <asp:ImageButton ID="IbtOTCerrarRecur" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtOTCerrarRecur_Click" ImageAlign="Right" />
                    <table class="TablaBusqueda">
                        <tr>
                            <td>
                                <asp:Label ID="LblOtRecurBusq" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                            <td>
                                <asp:TextBox ID="TxtOTRecurConsulPn" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                            <td>
                                <asp:ImageButton ID="IbtOTRecurConsulPn" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtOTRecurConsulPn_Click" /></td>
                            <td>
                                <asp:ImageButton ID="IbtOTRecurExpExcelPn" runat="server" ToolTip="Exportar reserva" CssClass=" BtnExpExcel" ImageUrl="~/images/ExcelV1.png" OnClick="IbtOTRecurExpExcelPn_Click" /></td>
                        </tr>
                    </table>
                    <br />
                    <div>
                        <asp:Button ID="BtnOTCargaMasiva" runat="server" CssClass="btn btn-success botones BtnEdicion" OnClick="BtnOTCargaMasiva_Click" Text="Carga masiva" Width="10%" />
                        <asp:Button ID="BtnOTRecurNotif" runat="server" CssClass="btn btn-success botones BtnEdicion" OnClick="BtnOTRecurNotif_Click" Text="Notificar" Width="10%" />
                    </div>
                    <br />
                    <asp:GridView ID="GrdOTRecursoF" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodiddetalleRes"
                        CssClass="DiseñoGrid table-sm" GridLines="Both" AllowPaging="true" PageSize="6"
                        OnRowCommand="GrdOTRecursoF_RowCommand" OnRowEditing="GrdOTRecursoF_RowEditing" OnRowUpdating="GrdOTRecursoF_RowUpdating" OnRowCancelingEdit="GrdOTRecursoF_RowCancelingEdit"
                        OnRowDeleting="GrdOTRecursoF_RowDeleting" OnRowDataBound="GrdOTRecursoF_RowDataBound" OnPageIndexChanging="GrdOTRecursoF_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Pos" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="LblPosc" Text='<%# Eval("NumeroPosicion") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtPosRF" Text='<%# Eval("NumeroPosicion") %>' runat="server" Width="100%" Enabled="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtPosRFPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:Label ID="LblOTPn" Text='<%# Eval("PN") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtOTPNRF" Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlOTPNRFPP" runat="server" Width="100%" Height="28px" AutoPostBack="true" OnTextChanged="DdlOTPNRFPP_TextChanged" />
                                    <asp:TextBox ID="TxtOTPNRFPP" runat="server" MaxLength="80" Width="100%" Enabled="false" Visible="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtDesRF" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtDesRFPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cant" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="LblCantRF" Text='<%# Eval("CantidadSolicitada") %>' runat="server" Width="100%" TextMode="Number" step="0.01" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtCantRF" Text='<%# Eval("CantidadSolicitada") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtCantRFPP" runat="server" Width="100%" Text="1" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unidad Medida" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("CodUnidadMed") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtUMRF" Text='<%# Eval("CodUnidadMed") %>' runat="server" Width="100%" Enabled="false" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cant. Entreg." HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="LblCantEntrRF" Text='<%# Eval("CantidadEntregada") %>' runat="server" Width="100%" TextMode="Number" step="0.01" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IPC - FIG - ITEM" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("IPC") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtIPCRF" Text='<%# Eval("IPC") %>' runat="server" Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtIPCRFPP" runat="server" MaxLength="240" Width="100%" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                    <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                    <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />

                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                    </asp:GridView>
                    <br />
                    <div id="Licencias" class="LicenciaRva">
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitLicencia" runat="server" Text="Licencias" /></h6>
                        <asp:GridView ID="GrdLicen" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdSrvLic,CodIdLicencia"
                            CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="6"
                            OnRowCommand="GrdLicen_RowCommand" OnRowEditing="GrdLicen_RowEditing" OnRowUpdating="GrdLicen_RowUpdating" OnRowCancelingEdit="GrdLicen_RowCancelingEdit"
                            OnRowDeleting="GrdLicen_RowDeleting" OnRowDataBound="GrdLicen_RowDataBound" OnPageIndexChanging="GrdLicen_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Licencia" HeaderStyle-Width="18%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblLicenRF" Text='<%# Eval("CodLicencia") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtLicenRF" Text='<%# Eval("CodLicencia") %>' runat="server" Width="100%" Enabled="false" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="DdlLicenRFPP" runat="server" Width="100%" Height="28px" AutoPostBack="true" OnTextChanged="DdlLicenRFPP_TextChanged" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="45%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtDesLiRF" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtDesLiRFPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tiempo Estimado" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblTieEstRF" Text='<%# Eval("TiempoEstimado") %>' runat="server" Width="100%" TextMode="Number" step="0.01" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtTieEstRF" Text='<%# Eval("TiempoEstimado") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtTieEstRFPP" runat="server" Width="100%" Text="1" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField FooterStyle-Width="13%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                        <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                        <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="GridFooterStyle" />
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtOTCerrarRecur" />
                    <asp:PostBackTrigger ControlID="IbtOTRecurExpExcelPn" />
                </Triggers>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BtnOTCargaMasiva" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw3OTCargaMasiva" runat="server">
            <asp:UpdatePanel ID="UplOTCargMasiv" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="LblCargaMasRte" runat="server" CssClass="LblEtiquet" Text="Reporte:" />
                    <asp:TextBox ID="TxtCargaMasiRte" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" step="0.01" Enabled="false" />
                    <asp:Label ID="LblCargaMasOt" runat="server" CssClass="LblEtiquet" Text="Sub OT / Reserva:" />
                    <asp:TextBox ID="TxtCargaMasiOT" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" step="0.01" Enabled="false" />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitOTCargMasiv" runat="server" Text="Subir Evaluación" /></h6>
                    <asp:ImageButton ID="IbtOTCerrarCargMaxivo" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtOTCerrarCargMaxivo_Click" ImageAlign="Right" />
                    <asp:ImageButton ID="IbtOTSubirCargaMax" runat="server" ToolTip="Cargar archivo..." ImageUrl="~/images/SubirCarga.png" OnClick="IbtOTSubirCargaMax_Click" Width="30px" Height="30px" />
                    <asp:ImageButton ID="IbtOTGuardarCargaMax" runat="server" ToolTip="Guardar" ImageUrl="~/images/Descargar.png" OnClick="IbtOTGuardarCargaMax_Click" Width="30px" Height="30px" Enabled="false" OnClientClick="javascript:return confirm('¿Desea almacenar la información?', 'Mensaje de sistema')" />
                    <asp:GridView ID="GrdOTCargaMax" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="False"
                        CssClass="DiseñoGrid table-sm" GridLines="Both">
                        <Columns>
                            <asp:TemplateField HeaderText="Pos" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtPosRF" Text='<%# Eval("Posicion") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtPNRF" Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtDesRF" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cant" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtCantRF" Text='<%# Eval("Cantidad") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unidad Medida" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtUMRF" Text='<%# Eval("UndDespacho") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unidad Sistema" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtUMSysRF" Text='<%# Eval("UndSistema") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IPC - FIG - ITEM" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtIPCRF" Text='<%# Eval("IPC") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtOTCerrarCargMaxivo" />
                    <asp:PostBackTrigger ControlID="IbtOTGuardarCargaMax" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw4OTOpcBusq" runat="server">
            <h6 class="TextoSuperior">
                <asp:Label ID="LblTitOTOpcBusqueda" runat="server" Text="Opciones de búsqueda" />
            </h6>
            <asp:Table ID="TblOTBusq" runat="server" Visible="false" Width="25%">
                <asp:TableRow>
                    <asp:TableCell Width="7%">
                        <asp:RadioButton ID="RdbOTBusqNumOT" runat="server" CssClass="LblEtiquet" Text="&nbsp Orden de trabajo" GroupName="BusqOT" />
                    </asp:TableCell>
                    <asp:TableCell Width="3%">
                        <asp:RadioButton ID="RdbOTBusqSN" runat="server" CssClass="LblEtiquet" Text="&nbsp S/N" GroupName="BusqOT" />
                    </asp:TableCell>
                    <asp:TableCell Width="3%">
                        <asp:RadioButton ID="RdbOTBusqPN" runat="server" CssClass="LblEtiquet" Text="&nbsp P/N" GroupName="BusqOT" />
                    </asp:TableCell>
                    <asp:TableCell Width="4%">
                        <asp:RadioButton ID="RdbOTBusqHK" runat="server" CssClass="LblEtiquet" Text="&nbsp Matrícula" GroupName="BusqOT" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:Table ID="TblBusqRte" runat="server" Visible="false" Width="65%">
                <asp:TableRow>
                    <asp:TableCell Width="3%">
                        <asp:RadioButton ID="RdbBusqRteNum" runat="server" CssClass="LblEtiquet" Text="&nbsp Reporte" GroupName="BusqRte" />
                    </asp:TableCell>
                    <asp:TableCell Width="3%">
                        <asp:RadioButton ID="RdbBusqRteHk" runat="server" CssClass="LblEtiquet" Text="&nbsp Aeronave" GroupName="BusqRte" />
                    </asp:TableCell>
                    <asp:TableCell Width="3%">
                        <asp:RadioButton ID="RdbBusqRteAta" runat="server" CssClass="LblEtiquet" Text="&nbsp ATA" GroupName="BusqRte" />
                    </asp:TableCell>
                    <asp:TableCell Width="3%">
                        <asp:RadioButton ID="RdbBusqRteTecn" runat="server" CssClass="LblEtiquet" Text="&nbsp Técnico" GroupName="BusqRte" />
                    </asp:TableCell>
                    <asp:TableCell Width="8%">
                        <asp:RadioButton ID="RdbBusqRteDescRte" runat="server" CssClass="LblEtiquet" Text="&nbsp Descripción del reporte" GroupName="BusqRte" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <table>
                <tr>
                    <td>
                        <asp:TextBox ID="TxtOTBusq" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                    <td>
                        <asp:ImageButton ID="IbtOTConsultarBusq" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtOTConsultarBusq_Click" /></td>
                    <td>
                        <asp:ImageButton ID="IbtOTCerrarBusq" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtOTCerrarBusq_Click" /></td>
                    <td>
                        <asp:ImageButton ID="IbtOTExpBusqOT" runat="server" ToolTip="Exportar Resultado" CssClass=" BtnExpExcel" ImageUrl="~/images/ExcelV1.png" OnClick="IbtOTExpBusqOT_Click" /></td>
                </tr>
            </table>
            <br />
            <div class="DivGrid DivContendorGrid">
                <asp:GridView ID="GrdOTBusq" runat="server" EmptyDataText="No existen registros ..!"
                    CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" AllowPaging="true" PageSize="15"
                    OnSelectedIndexChanged="GrdOTBusq_SelectedIndexChanged" OnPageIndexChanging="GrdOTBusq_PageIndexChanging">
                    <FooterStyle CssClass="GridFooterStyle" />
                    <HeaderStyle CssClass="GridCabecera" />
                    <RowStyle CssClass="GridRowStyle" />
                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                    <Columns>
                        <asp:CommandField HeaderText="Selección" SelectText="Select" ShowSelectButton="True" HeaderStyle-Width="33px" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8"/>
                </asp:GridView>
            </div>
        </asp:View>
        <asp:View ID="Vw5OTImprimir" runat="server">
            <asp:UpdatePanel ID="UpPnlOTPrint" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitOTImpresion" runat="server" Text="Impresión de la orden de trabajo" /></h6>
                    <asp:ImageButton ID="IbtOTCerrarPrint" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtOTCerrarPrint_Click" ImageAlign="Right" />
                    <rsweb:ReportViewer ID="RvwOTPrint" runat="server" Width="98%" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtOTCerrarPrint" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw6OTPasos" runat="server">
            <asp:UpdatePanel ID="UplPasos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitPasos" runat="server" Text="Pasos" /></h6>
                    <asp:ImageButton ID="IbtCerrarPasos" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarPasos_Click" ImageAlign="Right" />
                    <br />
                    <div id="DvEstado">
                        <asp:Label ID="LblPasoEsta" runat="server" CssClass="LblEtiquet" Text="Estado:" Font-Bold="true" />
                        <asp:DropDownList ID="DdlPasoEstado" runat="server" CssClass="heightCampo" Width="10%" Enabled="false" />
                        <asp:Label ID="LblPasoAplic" runat="server" CssClass="LblEtiquet" Text="Aplicabilidad:" Font-Bold="true" />
                        <asp:TextBox ID="TxtPasoAplic" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="15%" />
                    </div>
                    <div id="DvOpcRealizado">
                        <asp:Label ID="LblPaosoRealizado" runat="server" CssClass="LblEtiquet" Text="Estado Secundario:" Font-Bold="true" />
                        <asp:RadioButton ID="RdbPasoMaManto" runat="server" CssClass="LblEtiquet" Text="&nbsp Manual de Manto" GroupName="PasoRealizado" Enabled="false" />
                        <asp:RadioButton ID="RdbPasoMaOH" runat="server" CssClass="LblEtiquet" Text="&nbsp Manual Overhaul" GroupName="PasoRealizado" Enabled="false" />
                        <asp:RadioButton ID="RdbPasoSRM" runat="server" CssClass="LblEtiquet" Text="&nbsp Manual de SRM" GroupName="PasoRealizado" Enabled="false" />
                        <asp:RadioButton ID="RdbPasoEO" runat="server" CssClass="LblEtiquet" Text="&nbsp Orden de ingeniería" GroupName="PasoRealizado" Enabled="false" />
                        <asp:RadioButton ID="RdbPasoOTHER" runat="server" CssClass="LblEtiquet" Text="&nbsp Otros" GroupName="PasoRealizado" Enabled="false" />&nbsp&nbsp&nbsp
                        <asp:CheckBox ID="CkbPasoOtro" runat="server" CssClass="LblEtiquet" Text="&nbspOtros" Enabled="false" Visible="false" />
                    </div>
                    <div>
                        <asp:Label ID="LblPasoRef" runat="server" CssClass="LblEtiquet" Text="Referencia:" Font-Bold="true" />
                        <asp:TextBox ID="TxtPasoRef" runat="server" CssClass="form-control-sm heightCampo" Width="25%" MaxLength="100" Enabled="false" />
                        <asp:Label ID="LblPasoDiscrep" runat="server" CssClass="LblEtiquet" Text="Discrepancia:" Font-Bold="true" />
                        <asp:TextBox ID="TxtPasoDiscrep" runat="server" CssClass="form-control-sm heightCampo" TextMode="MultiLine" Width="45%" MaxLength="240" Height="50px" Enabled="false" />
                    </div>
                    <div id="PasoFechas">
                        <asp:Label ID="LblPasoFecI" runat="server" CssClass="LblEtiquet" Text="Fecha Inicio:" Font-Bold="true" />
                        <asp:ImageButton ID="IbtPasoFI" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" Enabled="false" />
                        <asp:TextBox ID="TxtPasoFecI" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="8%" />
                        <ajaxToolkit:CalendarExtender ID="CalPasoFechI" CssClass=" MyCalendar" runat="server" PopupButtonID="IbtPasoFI" TargetControlID="TxtPasoFecI" Format="dd/MM/yyyy" />
                        <asp:Label ID="LblPasoFecF" runat="server" CssClass="LblEtiquet" Text="Fecha Final:" Font-Bold="true" />
                        <asp:TextBox ID="TxtPasoFecF" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="8%" />
                    </div>
                    <div id="PasoTec">
                        <asp:Label ID="LblPasoTec" runat="server" CssClass="LblEtiquet" Text="Tecnico:" Font-Bold="true" />
                        <asp:DropDownList ID="DdlPasoTec" runat="server" CssClass="heightCampo" Width="30%" OnTextChanged="DdlPasoTec_TextChanged" AutoPostBack="true" Enabled="false" />
                        <asp:Label ID="LblPasoLicTec" runat="server" CssClass="LblEtiquet" Text="Licencia Tecnico:" Font-Bold="true" />
                        <asp:DropDownList ID="DdlPasoLicTec" runat="server" CssClass="heightCampo" Width="10%" Enabled="false" />
                        <asp:Label ID="LblPasoHRealTec" runat="server" CssClass="LblEtiquet" Text="Hora Real:" Font-Bold="true" />
                        <asp:TextBox ID="TxtPasoHRealTec" runat="server" CssClass="form-control-sm heightCampo" Width="6%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                    </div>
                    <div id="PasoInsp">
                        <asp:Label ID="LblPasoInsp" runat="server" CssClass="LblEtiquet" Text="Inspector:" Font-Bold="true" />
                        <asp:DropDownList ID="DdlPasoInsp" runat="server" CssClass="heightCampo" Width="30%" OnTextChanged="DdlPasoInsp_TextChanged" AutoPostBack="true" Enabled="false" />
                        <asp:Label ID="LblPasoLicInsp" runat="server" CssClass="LblEtiquet" Text="Licencia Inspector:" Font-Bold="true" />
                        <asp:DropDownList ID="DdlPasoLicInsp" runat="server" CssClass="heightCampo" Width="10%" Enabled="false" />
                        <asp:Label ID="LblPasoHRealInsp" runat="server" CssClass="LblEtiquet" Text="Hora Real:" Font-Bold="true" />
                        <asp:TextBox ID="TxtPasoHRealInsp" runat="server" CssClass="form-control-sm heightCampo" Width="6%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" /><br />
                        <br />
                        <asp:Label ID="LblPasoNotas" runat="server" CssClass="LblEtiquet" Text="Notas:" Font-Bold="true" />
                        <asp:TextBox ID="TxtPasoNotas" runat="server" CssClass="form-control-sm heightCampo" TextMode="MultiLine" Width="75%" MaxLength="300" Height="30px" Enabled="false" />
                    </div>
                    <div id="PasoBotones">
                        <asp:Button ID="BtnPasoAceptar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnPasoAceptar_Click" Text="Aceptar" Width="20%" />
                        <asp:Button ID="BtnPasoRepte" runat="server" CssClass=" btn btn-success BtnEdicion" OnClick="BtnPasoRepte_Click" Text="Generar Reporte" Width="20%" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarPasos" />
                    <asp:AsyncPostBackTrigger ControlID="DdlPasoTec" EventName="TextChanged" />
                    <asp:PostBackTrigger ControlID="BtnPasoRepte" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw7Manto" runat="server">
            <asp:UpdatePanel ID="UpPnlRte" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:ImageButton ID="IbtCerrarRte" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarRte_Click" ImageAlign="Right" />
                    <asp:Label ID="LblRteBusq" runat="server" CssClass="LblEtiquet" ForeColor="#800000" Font-Bold="true" Text="Consultar:" />
                    <asp:DropDownList ID="DdlBusqRte" runat="server" CssClass="Campos" OnTextChanged="DdlBusqRte_TextChanged" AutoPostBack="true" Width="20%"></asp:DropDownList>
                    <asp:Label ID="LblAeroRte" runat="server" CssClass="LblEtiquet" Text="Aeronave:"></asp:Label>
                    <asp:DropDownList ID="DdlAeroRte" runat="server" CssClass="Campos" Width="15%" Enabled="false"></asp:DropDownList>
                    <asp:Label ID="LblOtSec" runat="server" CssClass="LblEtiquet" Text="Sub OT / Reserva:"></asp:Label>
                    <asp:TextBox ID="TxtOtSec" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" step="0.01" Enabled="false" />
                    <asp:Label ID="LblRteNumPaso" runat="server" CssClass="LblEtiquet" Text="Paso:"></asp:Label>
                    <asp:TextBox ID="TxtNumPaso" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" step="0.01" Enabled="false" />

                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitRteManto" runat="server" Text="Reportes de mantenimiento"></asp:Label></h6>
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblNroRte" runat="server" CssClass="LblEtiquet" Text="Número:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="3%">
                                <asp:TextBox ID="TxtNroRte" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="100%"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell Width="4%">
                                <asp:TextBox ID="TxtConsTall" runat="server" CssClass="form-control heightCampo" MaxLength="15" Enabled="false" Width="90%"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblTipRte" runat="server" CssClass="LblEtiquet" Text="Tipo Reporte:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="5%">
                                <asp:DropDownList ID="DdlTipRte" runat="server" CssClass="heightCampo" Enabled="false" Width="95%"></asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblFuente" runat="server" CssClass="LblEtiquet" Text="Fuente:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="4%">
                                <asp:DropDownList ID="DdlFuente" runat="server" CssClass="heightCampo" Enabled="false" Width="95%"></asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblCasi" runat="server" CssClass="LblEtiquet" Text="Casilla:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="3%">
                                <asp:TextBox ID="TxtCas" runat="server" CssClass="form-control heightCampo" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="100%" Font-Size="10px"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblTall" runat="server" CssClass="LblEtiquet" Text="Taller:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="7%">
                                <asp:DropDownList ID="DdlTall" runat="server" CssClass="heightCampo" Enabled="false" Width="95%" />
                            </asp:TableCell>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblEstad" runat="server" CssClass="LblEtiquet" Text="Estado:" />
                            </asp:TableCell>
                            <asp:TableCell Width="3%">
                                <asp:DropDownList ID="DdlRteEstad" runat="server" CssClass="heightCampo" Enabled="false" Width="95%" OnTextChanged="DdlRteEstad_TextChanged" AutoPostBack="true" />
                            </asp:TableCell>
                            <asp:TableCell Width="1%">
                                <asp:Label ID="LblNotif" runat="server" CssClass="LblEtiquet" Text="Notif:" Visible="false" />
                            </asp:TableCell>
                            <asp:TableCell Width="2%">
                                <asp:CheckBox ID="CkbNotif" runat="server" CssClass="LblEtiquet" Text="" Enabled="false" Visible="false" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="LblClasf" runat="server" CssClass="LblEtiquet" Text="Clasificación:" />
                            </asp:TableCell><asp:TableCell ColumnSpan="2">
                                <asp:DropDownList ID="DdlRteClasf" runat="server" CssClass="heightCampo" Enabled="false" Width="95%" OnTextChanged="DdlRteClasf_TextChanged" AutoPostBack="true" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="LblCatgr" runat="server" CssClass="LblEtiquet" Text="Categoria:" />
                            </asp:TableCell><asp:TableCell>
                                <asp:DropDownList ID="DdlCatgr" runat="server" CssClass="heightCampo" Enabled="false" Width="95%" OnTextChanged="DdlCatgr_TextChanged" AutoPostBack="true" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="LblDocRef" runat="server" CssClass="LblEtiquet" Text="Docum. Referenc.:" />
                            </asp:TableCell><asp:TableCell>
                                <asp:TextBox ID="TxtDocRef" runat="server" CssClass="form-control heightCampo" MaxLength="20" Enabled="false" Width="95%" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="LblPosRte" runat="server" CssClass="LblEtiquet" Text="Posición:" />
                            </asp:TableCell><asp:TableCell ColumnSpan="2">
                                <asp:DropDownList ID="DdlPosRte" runat="server" CssClass="heightCampo" Enabled="false" Width="100%" />
                            </asp:TableCell><asp:TableCell ColumnSpan="5">
                                <asp:Table runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="LblAta" runat="server" CssClass="LblEtiquet" Text="Ata:" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="99%">
                                            <asp:DropDownList ID="DdlAtaRte" runat="server" CssClass="heightCampo" Enabled="false" Width="100%" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="Generado" runat="server" CssClass="LblEtiquet" Text="Generado:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:DropDownList ID="DdlGenerado" runat="server" CssClass="heightCampo" Enabled="false" Width="95%" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblLicGene" runat="server" CssClass="LblEtiquet" Text="Licencia:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:DropDownList ID="DdlLicGene" runat="server" CssClass="heightCampo" Enabled="false" Width="95%"></asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="4">
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblFecDet" runat="server" CssClass="LblEtiquet" Text="Fecha:" /></td>
                                        <td>
                                            <asp:ImageButton ID="IbtFecDet" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="19px" Width="15px" Enabled="false" /></td>
                                        <td>
                                            <asp:TextBox ID="TxtRteFecDet" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="90%" Font-Size="11px" OnTextChanged="TxtRteFecDet_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CldFecDet" runat="server" CssClass=" MyCalendar" PopupButtonID="IbtFecDet" TargetControlID="TxtRteFecDet" Format="dd/MM/yyyy" />
                                        </td>
                                        <td>
                                            <asp:Label ID="LblFecProy" runat="server" CssClass="LblEtiquet" Text="Proyec.:" /></td>
                                        <td>
                                            <asp:ImageButton ID="IbtFecPry" runat="server" CssClass="BtnImagenCalender DiseñoCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="19px" Width="15px" Enabled="false" /></td>
                                        <td>
                                            <asp:TextBox ID="TxtFecPry" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="90%" Font-Size="10.5px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CldFecPry" runat="server" CssClass=" MyCalendar" PopupButtonID="IbtFecPry" TargetControlID="TxtFecPry" Format="dd/MM/yyyy" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:Label ID="LblOtRte" runat="server" CssClass="LblEtiquet" Text="OT Ppal:" />
                                <asp:TextBox ID="TxtRteOt" runat="server" CssClass="heightCampo" Enabled="false" Width="66%" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblBasRte" runat="server" CssClass="LblEtiquet" Text="Base:" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="3">
                                <asp:DropDownList ID="DdlBasRte" runat="server" CssClass="heightCampo" Enabled="false" Width="80%"></asp:DropDownList>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="LblCumpl" runat="server" CssClass="LblEtiquet" Text="Cumplido:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:DropDownList ID="DdlCumpl" runat="server" CssClass="heightCampo" Enabled="false" Width="95%"></asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblLicCump" runat="server" CssClass="LblEtiquet" Text="Licencia:"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:DropDownList ID="DdlLicCump" runat="server" CssClass="heightCampo" Enabled="false" Width="95%"></asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblFecCump" runat="server" CssClass="LblEtiquet" Text="Fecha Cumplim.:" />
                            </asp:TableCell>
                            <asp:TableCell ID="TbClFecCump">
                                <asp:Table runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:ImageButton ID="IbtFecCump" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="19px" Width="15px" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox ID="TxtFecCump" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="90%" Font-Size="11px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CldFecCump" runat="server" CssClass=" MyCalendar" PopupButtonID="IbtFecCump" TargetControlID="TxtFecCump" Format="dd/MM/yyyy" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="6">
                                <asp:Table runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="lblProgr" runat="server" CssClass="LblEtiquet" Text="Programado:" />
                                            &nbsp
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="LblPgSi" runat="server" CssClass="LblEtiquet" Text="Sí" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:RadioButton ID="RdbPgSi" runat="server" TextAlign="Left" GroupName="Prog" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="LblPgNo" runat="server" CssClass="LblEtiquet" Text="No" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:RadioButton ID="RdbPgNo" runat="server" TextAlign="Left" GroupName="Prog" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            &nbsp;&nbsp;&nbsp;&nbsp<asp:Label ID="LblFallC" runat="server" CssClass="LblEtiquet" Text="Falla Confirmada:" />&nbsp
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="LblSi" runat="server" CssClass="LblEtiquet" Text="Sí" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:RadioButton ID="RdbFlCSi" runat="server" TextAlign="Left" GroupName="FallaC" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="LblNo" runat="server" CssClass="LblEtiquet" Text="No" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:RadioButton ID="RdbFlCNo" runat="server" TextAlign="Left" GroupName="FallaC" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            &nbsp;&nbsp<asp:Label ID="LblRII" runat="server" CssClass="LblEtiquet" Text="R.I.I.:" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:CheckBox ID="CkbRII" runat="server" CssClass="LblEtiquet" Text="" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="LblPnRte" runat="server" CssClass="LblEtiquet" Text="P/N:" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:DropDownList ID="DdlPnRte" runat="server" CssClass="heightCampo" Enabled="false" Width="95%" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblSnRte" runat="server" CssClass="LblEtiquet" Text="S/N:" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TxtSnRte" runat="server" CssClass="form-control heightCampo" MaxLength="20" Enabled="false" Width="95%" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblTtlAKSN" runat="server" CssClass="LblEtiquet" Text="TT AK/Comp:" Visible="false" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TxtTtlAKSN" runat="server" CssClass="form-control heightCampo" Width="85%" step="0.01" TextMode="Number" onkeypress="return Decimal(event);" Enabled="false" Visible="false" OnTextChanged="TxtTtlAKSN_TextChanged" AutoPostBack="true" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblHPrxCu" runat="server" CssClass="LblEtiquet" Text="H. Prox. Cumpl.:" Visible="false" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:TextBox ID="TxtHPrxCu" runat="server" CssClass="form-control heightCampo" Width="85%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" Visible="false" OnTextChanged="TxtHPrxCu_TextChanged" AutoPostBack="true" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:Label ID="LblNexDue" runat="server" CssClass="LblEtiquet" Text="Next Due:" Visible="false" />
                                <asp:TextBox ID="TxtNexDue" runat="server" CssClass="Form-control-sm heightCampo" Width="55%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" Visible="false" />
                            </asp:TableCell>
                            <asp:TableCell>
                               
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="LblDescRte" runat="server" CssClass="LblEtiquet" Text="Descripción Reporte:" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="5">
                                <asp:TextBox ID="TxtDescRte" runat="server" CssClass=" form-control-sm TextMultiLine" Enabled="false" TextMode="MultiLine" MaxLength="1000" Width="80%" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblAccCorr" runat="server" CssClass="LblEtiquet" Text="Acción Correctiva:" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="6">
                                <asp:TextBox ID="txtAccCrr" runat="server" CssClass="form-control-sm TextMultiLine" Enabled="false" TextMode="MultiLine" MaxLength="1000" Width="80%" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="AcciParc" runat="server" CssClass="LblEtiquet" Text="Acción parcial:" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="4">
                                <asp:TextBox ID="TxtAcciParc" runat="server" CssClass="form-control TextMultiLine" Enabled="false" TextMode="MultiLine" MaxLength="254" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblTecDif" runat="server" CssClass="LblEtiquet" Text="Técnico Difiere:" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:DropDownList ID="DdlTecDif" runat="server" CssClass="heightCampo" Enabled="false" />
                            </asp:TableCell>
                            <asp:TableCell ColumnSpan="7">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitDatosVer" runat="server" Text="Datos de verificación" /></h6>
                                <asp:Table runat="server" Width="100%">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="LblVerif" runat="server" CssClass="LblEtiquet" Text="Verifica:" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="53%">
                                            <asp:DropDownList ID="DdlVerif" runat="server" CssClass="heightCampo" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="30%">
                                            <asp:DropDownList ID="DdlLicVer" runat="server" CssClass="heightCampo" Enabled="false" Font-Size="10px" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="17%">
                                            <asp:CheckBox ID="CkbTearDown" runat="server" CssClass="LblEtiquet" Text="Teardown" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="8">
                                <asp:Table ID="Botnes" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success botones BtnEdicion" OnClick="BtnIngresar_Click" Text="Ingresar" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnModificar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnModificar_Click" Text="Modificar" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnReserva" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnReserva_Click" Text="Reserva" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnConsultar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnConsultar_Click" Text="Consultar" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnImprimir" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnImprimir_Click" Text="Imprimir" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnEliminar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnEliminar_Click" Text="Eliminar" OnClientClick="return confirm('¿Desea eliminar el registro?');" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnSnOnOf" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnSnOnOf_Click" Text="S/N On/Off" ToolTip="Series removidas - instaladas / Herramientas" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnExporRte" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnExporRte_Click" Text="Exportar" ToolTip="Exportar a Excel todos los reportes" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button ID="BtnNotificar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnNotificar_Click" Text="Notificar" ToolTip="Notificar el reporte" OnClientClick="return confirm('¿Desea notificar el reporte?');" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BtnReserva" />
                    <asp:PostBackTrigger ControlID="BtnConsultar" />
                    <asp:PostBackTrigger ControlID="BtnImprimir" />
                    <asp:PostBackTrigger ControlID="BtnSnOnOf" />
                    <asp:PostBackTrigger ControlID="BtnExporRte" />
                    <asp:PostBackTrigger ControlID="IbtCerrarRte" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw8SNOnOff" runat="server">
            <asp:UpdatePanel ID="UplSnOnOff" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="LblSnONOfNumRte" runat="server" CssClass="LblEtiquet" Text="Reporte:"></asp:Label>
                    <asp:TextBox ID="TxtSnOnOffNumRte" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" step="0.01" Enabled="false" />
                    <asp:ImageButton ID="IbtCerrarSnOnOff" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarSnOnOff_Click" ImageAlign="Right" />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LlTitSnOnOff" runat="server" Text="Ingreseso de elementos On - Off"></asp:Label></h6>
                    <asp:GridView ID="GrdSnOnOff" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodIdDetLvDetManto"
                        CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="4"
                        OnRowCommand="GrdSnOnOff_RowCommand" OnRowEditing="GrdSnOnOff_RowEditing" OnRowUpdating="GrdSnOnOff_RowUpdating" OnRowCancelingEdit="GrdSnOnOff_RowCancelingEdit"
                        OnRowDeleting="GrdSnOnOff_RowDeleting" OnRowDataBound="GrdSnOnOff_RowDataBound" OnPageIndexChanging="GrdSnOnOff_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="LblFec" Text='<%# Eval("FechaRemocion") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtFec" Text='<%# Eval("FechaRemocion") %>' runat="server" Width="75%" Enabled="false" />
                                    <asp:ImageButton ID="IbtFecha" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                    <ajaxToolkit:CalendarExtender ID="CalFech" runat="server" PopupButtonID="IbtFecha" TargetControlID="TxtFec" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtFecPP" runat="server" Width="75%" Enabled="false" />
                                    <asp:ImageButton ID="IbtFechaPP" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                    <ajaxToolkit:CalendarExtender ID="CalFechPP" runat="server" PopupButtonID="IbtFechaPP" TargetControlID="TxtFecPP" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Razón del evento" HeaderStyle-Width="14%">
                                <ItemTemplate>
                                    <asp:Label ID="LblRazonR" Text='<%# Eval("Descripcion") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DdlRazonR" runat="server" Width="100%" Height="28px" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlRazonRPP" runat="server" Width="100%" Height="28px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Posición" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="LblPosic" Text='<%# Eval("Posicion") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DdlPosic" runat="server" Width="100%" Height="28px" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlPosicPP" runat="server" Width="100%" Height="28px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P/N ON" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="LblPNOn" Text='<%# Eval("CodPnOn") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DdlPNOn" runat="server" Width="100%" Height="28px" OnTextChanged="DdlPNOn_TextChanged" AutoPostBack="true" />
                                    <asp:ListBox ID="LtbSNOn" runat="server" Visible="false" Width="100%" OnSelectedIndexChanged="LtbSNOn_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlPNOnPP" runat="server" Width="100%" Height="28px" OnTextChanged="DdlPNOnPP_TextChanged" AutoPostBack="true" />
                                    <asp:ListBox ID="LtbSNOnPP" runat="server" Visible="false" Width="100%" OnSelectedIndexChanged="LtbSNOnPP_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="S/N ON" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("CodElementoOn") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtSNOn" Text='<%# Eval("CodElementoOn") %>' runat="server" Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtSNOnPP" runat="server" MaxLength="240" Width="100%" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("DesElemento") %>' runat="server" Width="100%" Font-Size="8px" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtDescElem" Text='<%# Eval("DesElemento") %>' runat="server" Width="100%" Font-Size="8px" Enabled="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtDescElemPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P/N OFF" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="LblPNOff" Text='<%# Eval("CodPnOff") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DdlPNOff" runat="server" Width="100%" Height="28px" OnTextChanged="DdlPNOff_TextChanged" AutoPostBack="true" />
                                    <asp:ListBox ID="LtbSNOff" runat="server" Visible="false" Width="100%" OnSelectedIndexChanged="LtbSNOff_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlPNOffPP" runat="server" Width="100%" Height="28px" OnTextChanged="DdlPNOffPP_TextChanged" AutoPostBack="true" />
                                    <asp:ListBox ID="LtbSNOffPP" runat="server" Visible="false" Width="100%" OnSelectedIndexChanged="LtbSNOffPP_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="S/N OFF" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("CodElementoOff") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtSNOff" Text='<%# Eval("CodElementoOff") %>' runat="server" Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtSNOffPP" runat="server" MaxLength="240" Width="100%" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cant" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="LblCant" Text='<%# Eval("CantDDLV") %>' runat="server" Width="100%" TextMode="Number" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtCant" Text='<%# Eval("CantDDLV") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtCantPP" runat="server" Width="100%" Text="1" TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterStyle-Width="13%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                    <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                    <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                    </asp:GridView>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitHta" runat="server" Text="Herramientas"></asp:Label></h6>
                    <asp:GridView ID="GrdHta" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdHerramientoManto"
                        CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="3" Width="80%"
                        OnRowCommand="GrdHta_RowCommand" OnRowEditing="GrdHta_RowEditing" OnRowUpdating="GrdHta_RowUpdating" OnRowCancelingEdit="GrdHta_RowCancelingEdit"
                        OnRowDeleting="GrdHta_RowDeleting" OnRowDataBound="GrdHta_RowDataBound" OnPageIndexChanging="GrdHta_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:Label ID="LblPNHta" Text='<%# Eval("PN") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DdlPNHta" runat="server" Width="100%" Height="28px" OnTextChanged="DdlPNHta_TextChanged" AutoPostBack="true" />
                                    <asp:ListBox ID="LtbSNHta" runat="server" Visible="false" Width="100%" OnSelectedIndexChanged="LtbSNHta_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlPNHtaPP" runat="server" Width="100%" Height="28px" OnTextChanged="DdlPNHtaPP_TextChanged" AutoPostBack="true" />
                                    <asp:ListBox ID="LtbSNHtaPP" runat="server" Visible="false" Width="100%" OnSelectedIndexChanged="LtbSNHtaPP_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("SN") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtSNHta" Text='<%# Eval("SN") %>' runat="server" Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtSNHtaPP" runat="server" MaxLength="240" Width="100%" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Font-Size="8px" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtDescHta" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Font-Size="8px" Enabled="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtDescHtaPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="LblFecVce" Text='<%# Eval("FechaVence") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtFecVce" Text='<%# Eval("FechaVence") %>' runat="server" Width="75%" Enabled="false" />
                                    <asp:ImageButton ID="IbtFechaVce" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                    <ajaxToolkit:CalendarExtender ID="CalFechVce" runat="server" PopupButtonID="IbtFechaVce" TargetControlID="TxtFecVce" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtFechVcePP" runat="server" Width="75%" Enabled="false" />
                                    <asp:ImageButton ID="IbtFechVcePP" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" />
                                    <ajaxToolkit:CalendarExtender ID="CalFechVcePP" runat="server" PopupButtonID="IbtFechVcePP" TargetControlID="TxtFechVcePP" Format="dd/MM/yyyy" CssClass="MyCalendar" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                    <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                    <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarSnOnOff" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw9Informe" runat="server">
            <asp:UpdatePanel ID="UpPnlInforme" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitImpresion" runat="server" Text="Impresión del reporte" /></h6>
                    <asp:ImageButton ID="IbtCerrarImpresion" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarImpresion_Click" ImageAlign="Right" />
                    <rsweb:ReportViewer ID="RvwReporte" runat="server" Width="98%" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarImpresion" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw10OTSinCerrar" runat="server">
            <h6 class="TextoSuperior">
                <asp:Label ID="LblTit8PasoOpen" runat="server" Text="Ordenes de trabajo abiertas con el octavo paso cumplido" />
            </h6>
            <table>
                <tr>
                    <td>
                        <asp:ImageButton ID="IbtCerrarOT8PasoClose" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarOT8PasoClose_Click" /></td>
                    <td>
                        <asp:ImageButton ID="IbtExportarOT8PasoClose" runat="server" ToolTip="Exportar" CssClass=" BtnExpExcel" ImageUrl="~/images/ExcelV1.png" OnClick="IbtExportarOT8PasoClose_Click" /></td>
                </tr>
            </table>
            <br />
            <div class="DivGrid DivContendorGrid">
                <asp:GridView ID="Grd8PasoCOTOpen" runat="server" EmptyDataText="Sin registros ..!"
                    CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" AllowPaging="true" PageSize="15"
                    OnSelectedIndexChanged="Grd8PasoCOTOpen_SelectedIndexChanged" OnPageIndexChanging="Grd8PasoCOTOpen_PageIndexChanging">
                    <FooterStyle CssClass="GridFooterStyle" />
                    <HeaderStyle CssClass="GridCabecera" />
                    <RowStyle CssClass="GridRowStyle" />
                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                    <Columns>
                        <asp:CommandField HeaderText="Selección" SelectText="Select" ShowSelectButton="True" HeaderStyle-Width="33px" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                </asp:GridView>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
