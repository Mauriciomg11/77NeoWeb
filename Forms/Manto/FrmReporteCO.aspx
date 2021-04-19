<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmReporteCO.aspx.cs" Inherits="_77NeoWeb.Forms.Manto.FrmReporteCO" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="RpVw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarDiv {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
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

        .ScrollDivGrid {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 63%;
        }

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
        function myFuncionddl() {
            $('#<%=DdlAeronave.ClientID%>').chosen();
            $('#<%=DdlStatus.ClientID%>').chosen();
            $('#<%=DdlOTPpl.ClientID%>').chosen();
            $('#<%=DdlRpteNro.ClientID%>').chosen();
        }
        function targetMeBlank() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <div class=" CentrarDiv DivMarco">
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblAeronave" runat="server" CssClass="LblEtiquet" Text="aeronave" />
                                <asp:DropDownList ID="DdlAeronave" runat="server" CssClass="heightCampo" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblStatus" runat="server" CssClass="LblEtiquet" Text="estado" />
                                <asp:DropDownList ID="DdlStatus" runat="server" CssClass="heightCampo" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblOTPpl" runat="server" CssClass="LblEtiquet" Text="Ot principal" />
                                <asp:DropDownList ID="DdlOTPpl" runat="server" CssClass="heightCampo" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblRpteNro" runat="server" CssClass="LblEtiquet" Text="nro reporte" />
                                <asp:DropDownList ID="DdlRpteNro" runat="server" CssClass="heightCampo" Width="100%" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblPN" runat="server" CssClass="LblEtiquet" Text="P/N" />
                                <asp:TextBox ID="TxtPN" runat="server" CssClass="form-control heightCampo" Width="100%" MaxLength="80" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblSN" runat="server" CssClass="LblEtiquet" Text="S/N" />
                                <asp:TextBox ID="TxtSN" runat="server" CssClass="form-control heightCampo" Width="100%" MaxLength="80" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechI" runat="server" CssClass="LblEtiquet" Text="fecha Inicial" />
                                <asp:TextBox ID="TxtFechI" runat="server" CssClass="form-control heightCampo" Width="100%" TextMode="Date" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechF" runat="server" CssClass="LblEtiquet" Text="fecha Final" />
                                <asp:TextBox ID="TxtFechF" runat="server" CssClass="form-control heightCampo" Width="100%" TextMode="Date" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Button ID="BtnConsult" runat="server" CssClass="btn btn-success botones" Width="100%" OnClick="BtnConsult_Click" Text="nuevo" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="BtnImprimir" runat="server" CssClass="btn btn-primary botones" Width="100%" OnClick="BtnImprimir_Click" Text="modificar" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="BtnAlertaCO" runat="server" CssClass="btn btn-primary" Width="100%" OnClick="BtnAlertaCO_Click" OnClientClick="target ='_blank';" Text="Alerta C-Over" />

                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="BtnExportar" runat="server" CssClass="btn btn-primary" Width="100%" OnClick="BtnExportar_Click" Text="Exportar" />
                            </div>
                        </div>
                        <br />
                        <div class="row ">
                            <div class="col-sm-12">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitReportes" runat="server" Text="reportes de mantenimiento" /></h6>
                            </div>
                        </div>
                        <div class="ScrollDivGrid">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both">
                                        <Columns>
                                            <asp:TemplateField HeaderText="CodStatus" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("CodStatus") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Matricula">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Matricula") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nroreporte">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Nroreporte") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Programado" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Programado") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ReportadoPor">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("ReportadoPor") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FechaReporte">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Fecha1")%>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reporte" HeaderStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:TextBox Text='<%# Eval("Reporte")%>' runat="server" Width="100%" TextMode="MultiLine" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FechaCumplimiento">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Fecha2") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AccionCorrectiva" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:TextBox Text='<%# Eval("AccionCorrectiva") %>' runat="server" Width="100%" TextMode="MultiLine" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Inspector">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Inspector") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NumLicenciaRM">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("NumLicenciaRM") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CodOT">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("CodOT") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LibroVuelo">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("LibroVuelo") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UbicacionTecnica">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("UbicacionTecnica") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("ParteNumero") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S/N">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("SerieNumero") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw1Imprimir" runat="server">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitImpresion" runat="server" Text="Impresión" />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarImpr" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarImpr_Click" />
                    <RpVw:ReportViewer ID="RpVwReporte" runat="server" Width="98%" />
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnImprimir" />
            <asp:PostBackTrigger ControlID="BtnExportar" />
            <asp:PostBackTrigger ControlID="BtnAlertaCO" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
