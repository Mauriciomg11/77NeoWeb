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
            $('[id *=DdlOTTecPP]').chosen();
            $('[id *=DdlOTLicPP]').chosen();
            $('[id *=DdlOTPNRFPP]').chosen();
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
                        <asp:Button ID="BtnMroTrabEje" CssClass="btn btn-outline-primary" runat="server" Text="Trabajo ejecutado" OnClick="BtnMroTrabEje_Click" Font-Size="11px" Visible="false" />
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
                        <asp:Label ID="LblOTBase" runat="server" CssClass="LblEtiquet" Text="Base:" />
                        <asp:DropDownList ID="DdlOTBase" runat="server" CssClass="heightCampo" Width="16%" Enabled="false" />
                        <asp:Label ID="LblOTAero" runat="server" CssClass="LblEtiquet" Text="Aeronave:" />
                        <asp:DropDownList ID="DdlOTAero" runat="server" CssClass="heightCampo" Width="10%" Enabled="false" />
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
                        <asp:DropDownList ID="DdlOtLicInsp" runat="server" CssClass="heightCampo" Width="10%" Enabled="false" />
                        <asp:Label ID="lblOtRespons" runat="server" CssClass="LblEtiquet" Text="Responsable:" />
                        <asp:DropDownList ID="DdlOtRespons" runat="server" CssClass="heightCampo" Width="20%" Enabled="false" />
                        <asp:CheckBox ID="CkbCancel" runat="server" CssClass="LblEtiquet" Text="&nbspCancelar O.T." Enabled="false" />&nbsp&nbsp
                        <asp:CheckBox ID="CkbOtBloqDet" runat="server" CssClass="LblEtiquet" Text="&nbspRercurso Bloqueado" Enabled="false" />
                    </div>
                    <div class="table-responsive">
                        <asp:Table runat="server" Width="98%">
                            <asp:TableRow>
                                <asp:TableCell Width="1%">
                                    <asp:Label ID="LblOTTrabajo" runat="server" CssClass="LblEtiquet" Text="Trabajo Requerido:" />
                                </asp:TableCell>
                                <asp:TableCell Width="38%">
                                    <asp:TextBox ID="TxtOTTrabajo" runat="server" CssClass="form-control-sm" TextMode="MultiLine" MaxLength="240" Width="100%" Font-Size="10px" Enabled="false" />
                                </asp:TableCell>
                                <asp:TableCell Width="1%">
                                    <asp:Label ID="LblOTAccParc" runat="server" CssClass="LblEtiquet" Text="Acción Parcial:" />
                                </asp:TableCell>
                                <asp:TableCell Width="38%">
                                    <asp:TextBox ID="TxtOTAccParc" runat="server" CssClass="form-control-sm" TextMode="MultiLine" MaxLength="240" Width="100%" Font-Size="10px" Enabled="false" />
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
                        <asp:Button ID="BtnOTAbiertas8PasCump" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnOtAbiertas8PasCump_Click" Text="O.T. Abiertas" ToolTip="O.T. abiertas con el paso 8 cumplido" />
                        <asp:Button ID="BtNOTExportar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtNOTExportar_Click" Text="Exportar" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DdlBusqOT" EventName="TextChanged" />
                    <asp:PostBackTrigger ControlID="BtnOTDetTec" />
                    <asp:PostBackTrigger ControlID="BtnOTReserva" />
                    <asp:PostBackTrigger ControlID="BtnOTConsultar" />
                    <asp:PostBackTrigger ControlID="BtnOTImprimir" />
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
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
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
                            <asp:TemplateField HeaderText="Parte número" HeaderStyle-Width="25%">
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
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtOTCerrarRecur" />
                    <asp:PostBackTrigger ControlID="IbtOTRecurExpExcelPn" />
                    <asp:PostBackTrigger ControlID="BtnOTCargaMasiva" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw3OTCargaMasiva" runat="server">
            <asp:UpdatePanel ID="UplOTCargMasiv" runat="server" UpdateMode="Conditional">
                <ContentTemplate>                    
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitOTCargMasiv" runat="server" Text="Subir Evaluación"/></h6>
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
                            <asp:TemplateField HeaderText="Parte número" HeaderStyle-Width="25%">
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
                <asp:Label ID="LblTitOTOpcBusqueda" runat="server" Text="Opciones de búsqueda"/>
            </h6>
            <asp:Table ID="TblOTBusq" runat="server" Visible="false" Width="25%" >
                <asp:TableRow>
                    <asp:TableCell Width="7%">
                        <asp:RadioButton ID="RdbOTBusqNumOT" runat="server" CssClass="LblEtiquet" Text="&nbsp Orden e trabajo" GroupName="BusqOT" />
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
            <table >
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
            </table><br />
            <div class="DivGrid DivContendorGrid">
                <asp:GridView ID="GrdOTBusq" runat="server" EmptyDataText="No existen registros ..!"
                    CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" AllowPaging="true" PageSize="15"
                    OnSelectedIndexChanged="GrdOTBusq_SelectedIndexChanged" OnPageIndexChanging="GrdOTBusq_PageIndexChanging">
                    <FooterStyle CssClass="GridFooterStyle" />
                    <HeaderStyle CssClass="GridCabecera" />
                    <RowStyle CssClass="GridRowStyle" />
                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                    <Columns>
                        <asp:CommandField HeaderText="Selección" SelectText="Enviar" ShowSelectButton="True" HeaderStyle-Width="33px" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                </asp:GridView>
            </div>
        </asp:View>
         <asp:View ID="Vw5OTImprimir" runat="server">
            <asp:UpdatePanel ID="UpPnlOTPrint" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitOTImpresion" runat="server" Text="Impresión de la orden de trabajo"/></h6>
                    <asp:ImageButton ID="IbtOTCerrarPrint" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtOTCerrarPrint_Click" ImageAlign="Right" />
                    <rsweb:ReportViewer ID="RvwOTPrint" runat="server" Width="98%"/>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtOTCerrarPrint" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
