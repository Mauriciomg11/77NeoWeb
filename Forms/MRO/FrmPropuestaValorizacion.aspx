<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmPropuestaValorizacion.aspx.cs" Inherits="_77NeoWeb.Forms.MRO.FrmPropuestaValorizacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
            font-weight: bold;
            width: 8%;
            height: 27px;
        }

        .Font_btnSelect {
            font-size: 12px;
            font-stretch: condensed;
            width: 14%;
            height: 27px;
        }

        .Font_btnExportar {
            font-size: 12px;
            font-stretch: condensed;
            width: 40%;
            height: 27px;
        }

        .ScrollDet1 {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 480px;
        }

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

        .CentrarExportar {
            position: absolute;
            left: 50%;
            width: 40%;
            margin-left: -20%;
            height: 70%;
            padding: 5px;
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
            $('#<%=DdlNumPpt.ClientID%>').chosen();
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
                    <div class="CentrarContenedor DivMarco">
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Table ID="Table1" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell Width="30%">
                                            <asp:Label ID="LblNumPpt" runat="server" CssClass="LblEtiquet" Text="Propuesta:" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="60%">
                                            <asp:DropDownList ID="DdlNumPpt" runat="server" CssClass="Campos" Width="100%" OnTextChanged="DdlNumPpt_TextChanged" AutoPostBack="true" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                            <div class="col-sm-9">
                                <asp:Button ID="BtnValorizar" runat="server" CssClass="btn btn-success Font_btnCrud" OnClick="BtnValorizar_Click" OnClientClick="target ='';" Text="Valorizar" />
                                <asp:Button ID="BtnReValorizar" runat="server" CssClass="btn btn-success Font_btnCrud" OnClick="BtnReValorizar_Click" OnClientClick="target ='';" Text="Revalorizar" />
                                <asp:Button ID="BtnPlantilla" runat="server" CssClass="btn btn-success Font_btnCrud" OnClick="BtnPlantilla_Click" OnClientClick="target ='';" Text="Plantilla" />
                                <asp:Button ID="BtnExportar" runat="server" CssClass="btn btn-primary Font_btnCrud" OnClick="BtnExportar_Click" OnClientClick="target ='';" Text="exportar" />&nbsp&nbsp
                                <asp:Button ID="BtnPNSinValorizar" runat="server" CssClass="btn btn-primary Font_btnSelect" OnClick="BtnPNSinValorizar_Click" OnClientClick="target ='';" Text="PN sin Valorizar" />
                                <asp:Button ID="BtnSolPed" runat="server" CssClass="btn btn-primary Font_btnSelect" OnClick="BtnSolPed_Click" OnClientClick="target ='_blank';" Text="solicitud pedido" />
                                <asp:Button ID="BtnCotizacion" runat="server" CssClass="btn btn-primary Font_btnSelect" OnClick="BtnCotizacion_Click" OnClientClick="target ='_blank';" Text="cotización" />
                                <asp:Button ID="BtnCuadroComprtv" runat="server" CssClass="btn btn-primary Font_btnSelect" OnClick="BtnCuadroComprtv_Click" OnClientClick="target ='_blank';" Text="cuadro comparativo" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-1">
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="IbtAprDet1All" runat="server" ImageUrl="~/images/Check1.png" ImageAlign="AbsBottom" Height="30px" Width="30px" OnClick="IbtAprDet1All_Click" /></td>
                                        <td>
                                            <asp:ImageButton ID="IbtGrarSP" runat="server" ImageUrl="~/images/AddOrder.png" ImageAlign="AbsBottom" Height="30px" Width="30px" OnClick="IbtGrarSP_Click" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblCliente" runat="server" CssClass="LblEtiquet" Text="cliente" />
                                <asp:TextBox ID="TxtCliente" runat="server" CssClass="Form-control  heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblDescTipoPPT" runat="server" CssClass="LblEtiquet" Text="tipo" />
                                <asp:TextBox ID="TxtDescTipoPPT" runat="server" CssClass="Form-control heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblDesEstado" runat="server" CssClass="LblEtiquet" Text="estado" />
                                <asp:TextBox ID="TxtDesEstado" runat="server" CssClass="Form-control heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblDescPptTipoSol" runat="server" CssClass="LblEtiquet" Text="tipo solicitud" />
                                <asp:TextBox ID="TxtDescPptTipoSol" runat="server" CssClass="Form-control heightCampo" Enabled="false" Width="100%" />
                            </div>
                        </div>
                        <table>
                            <tr>
                                <td></td>
                            </tr>
                        </table>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="ScrollDet1">
                                    <asp:GridView ID="GrdDetValrzc" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false"
                                        DataKeyNames="IdServicio,CodReferencia,PnStock,CodIdUbicacion,Bodega,StockMinimo,CodTipoCotiza,SelectBodeg, CantidadSolicitud, ObservacionValorizar,
                                                      NomBodega,IdReporte,UnidMinCompra,CodEstado,PnAlternoPV,IdDetPropSrv,RepaExterna,EquivalenciaPV,CodAeronaveVal,SNElementoV"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both" Width="120%" EmptyDataText="No existen registros ..!"
                                        OnRowDataBound="GrdDetValrzc_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Pos">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPos" Text='<%# Eval("Posicion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Aprob">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CkbAprobP" Checked='<%# Eval("Aprobado").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select SP">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CkbGenrSP" Checked='<%# Eval("SelectSolicitud").ToString()=="1" ? true : false %>' runat="server" OnCheckedChanged="CkbGenrSP_CheckedChanged" AutoPostBack="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cant solicitar" HeaderStyle-Width="2%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtCantSP" Text='<%# Eval("CantidadSolicitud") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NumPedido">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblNumSP" Text='<%# Eval("NumPedido") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TipoCotizacion">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblTipoCot" Text='<%# Eval("TipoCotizacion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OTVAL">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblOtVal" Text='<%# Eval("OTVAL") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NomServicio">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblNomSvc" Text='<%# Eval("NomServicio") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PnPropuesta">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPnPpt" Text='<%# Eval("PnPropuesta") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripcion">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblDescElem" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cantidad Propuesta" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="CantPpt" Text='<%# Eval("CantidadPropuesta") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cant RealPV" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCntReal" Text='<%# Eval("CantRealPV") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ValorCompra">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtVlr" Text='<%# Eval("ValorCompra") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" OnTextChanged="TxtVlr_TextChanged" AutoPostBack="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MonedaProVa" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtMnda" Text='<%# Eval("MonedaProVa") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UndMedProVa">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblUndMPt" Text='<%# Eval("UndMedProVa") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UndCompra">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblUndMedCmpra" Text='<%# Eval("UndCompraPV") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FechaUltimaCompra">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFechUlmCmp" Text='<%# Eval("FechUltComprText") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entrega cotiza dias" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtTiemEntrDiaCot" Text='<%# Eval("TiempoEntregaDiasCoti") %>' runat="server" Width="100%" TextMode="Number" step="1" onkeypress="return solonumeros(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DocReferencia">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblDocRef" Text='<%# Eval("DocReferencia") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CantStock">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCntStk" Text='<%# Eval("CantStock") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MatriculaVal">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblMatric" Text='<%# Eval("MatriculaVal") %>' runat="server" Width="100%" />
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
                <asp:View ID="Vw1Exportar" runat="server">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitOpcExportar" runat="server" Text="Opciones de la exportación" />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarExportar" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarExportar_Click" />
                    <div class="CentrarExportar DivMarco">
                        <div class="col-sm-12">
                            <asp:Button ID="BtnDetPpt" runat="server" CssClass="btn btn-primary Font_btnExportar" OnClick="BtnDetPpt_Click" Text="Detalle" />
                            <asp:Button ID="BtnExpPlantilla" runat="server" CssClass="btn btn-primary Font_btnExportar" OnClick="BtnExpPlantilla_Click" Text="PLantilla" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw2ElementosNoValorizados" runat="server">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitEleNoValorizado" runat="server" Text="parte no encontradas en la valorización" />
                    </h6>
                    <asp:ImageButton ID="IbtClosePNoValorizado" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtClosePNoValorizado_Click" />
                    <br />
                    <br />
                    <div class="CentrarBusq DivMarco">
                        <div class="CentrarGrid pre-scrollable">
                            <asp:GridView ID="GrdPnNoValorizado" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both">
                                <Columns>
                                    <asp:TemplateField HeaderText="Reporte">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPpt" Text='<%# Eval("Reporte") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="OT">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("OT") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CodReferencia">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("CodReferencia") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P/N">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Pn") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FechaReserva">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("FechaReserva") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fec_crea_PN">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Fec_crea_PN") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FechaNotificacion">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("FechaNotificacion") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FechaValorizado">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("FechaValorizado") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            </asp:GridView>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnDetPpt" />
            <asp:PostBackTrigger ControlID="BtnExpPlantilla" />
            <%--            <asp:PostBackTrigger ControlID="IbtAprDet1All" />--%>
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
