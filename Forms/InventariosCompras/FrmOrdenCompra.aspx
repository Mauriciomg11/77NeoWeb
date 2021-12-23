<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmOrdenCompra.aspx.cs" Inherits="_77NeoWeb.Forms.InventariosCompras.FrmOrdenCompra" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="RpVw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .CentrarContNumCotiza {
            left: 50%;
            /*determinamos una anchura*/
            width: 98%;
            margin-left: 1%;
            height: 8%;
        }

        .CentrarContenedor {
            position: absolute;
            left: 50%;
            width: 98%;
            margin-left: -49%;
            height: 90%;
            padding: 5px;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
        }

        .ScrollDet2 {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 90%;
        }

        .CentrarBusq {
            position: absolute;
            left: 50%;
            width: 60%;
            margin-left: -30%;
            height: 85%;
            padding: 5px;
        }

        .TextR {
            text-align: right;
        }

        .CentrarExportar {
            position: absolute;
            left: 50%;
            width: 40%;
            margin-left: -20%;
            height: 15%;
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
            $('#<%=DdlProvee.ClientID%>').chosen();
            $('#<%=DdlEmplead.ClientID%>').chosen();
            $('#<%=DdlAutoriz.ClientID%>').chosen();
            $('#<%=DdlTipo.ClientID%>').chosen();
            $('#<%=DdlTransp.ClientID%>').chosen();
            $('#<%=DdlTipoPago.ClientID%>').chosen();
            $('#<%=DdlEstd.ClientID%>').chosen();
            $('#<%=DdlUbicac.ClientID%>').chosen();
            $('#<%=DdlEnvioFact.ClientID%>').chosen();
        }
        $(':text').on("focus", function () {
            //here set in localStorage id of the textbox
            localStorage.setItem("focusItem", this.id);
            //console.log(localStorage.getItem("focusItem"));test the focus element id
        });
        function ShowPopup() {
            $('#ModalBusqCompraCotiza').modal('show');
            $('#ModalBusqCompraCotiza').on('shown.bs.modal', function () {
                document.getElementById('<%= TxtModalBusq.ClientID %>').focus();
                document.getElementById('<%= TxtModalBusq.ClientID %>').select();
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <div id="ModalBusqCompraCotiza" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="LblTitModalBusqCompra" runat="server" Text="opciones de búsqueda" />
                        <asp:Label ID="LblTitModalBusqCotiza" runat="server" Text="asignar proveedor" Visible="false" /></h4>
                </div>
                <div class="modal-body">
                    <asp:Table ID="TblMdlOpcBusCompra" runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:RadioButton ID="RdbOpcMdlBusqCompra" runat="server" CssClass="LblEtiquet" Text="&nbsp compra" GroupName="BusqCompra" />&nbsp&nbsp&nbsp    
                                 <asp:RadioButton ID="RdbOpcMdlBusqPrv" runat="server" CssClass="LblEtiquet" Text="&nbsp proveedor" GroupName="BusqCompra" />&nbsp&nbsp&nbsp                        
                                <asp:RadioButton ID="RdbOpcMdlBusqPPT" runat="server" CssClass="LblEtiquet" Text="&nbsp propuesta" GroupName="BusqCompra" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:Table ID="TblMdlOpcBusCotiza" runat="server" Visible="false">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:RadioButton ID="RdbMdlOpcBusqCotizNum" runat="server" CssClass="LblEtiquet" Text="&nbsp cotizacion" GroupName="BusqCtz" />&nbsp&nbsp&nbsp 
                                 <asp:RadioButton ID="RdbMdlOpcBusqCotizPrv" runat="server" CssClass="LblEtiquet" Text="&nbsp proveedor" GroupName="BusqCtz" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <table class="TablaBusqueda">
                        <tr>
                            <td>
                                <asp:Label ID="LblModalBusq" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                            <td>
                                <asp:TextBox ID="TxtModalBusq" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                            <td>
                                <asp:ImageButton ID="IbtModalBusq" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtModalBusq_Click" /></td>
                        </tr>
                    </table>
                    <div class="CentrarGrid pre-scrollable">
                        <asp:GridView ID="GrdModalBusqCompra" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="DescPPT, TipoOrdenCompra"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnRowCommand="GrdModalBusqCompra_RowCommand" OnRowDataBound="GrdModalBusqCompra_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:UpdatePanel ID="UplIr" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ImageButton ID="IbtIr" Width="30px" Height="30px" ImageUrl="~/images/IrV2.png" runat="server" CommandName="Ir" ToolTip="Ir" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="IbtIr" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Compra">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCodCompra" Text='<%# Eval("CodOrdenCompra") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="proveedor">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("RazonSocial") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("FechaOC") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="estado">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Estado") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="moneda">
                                    <ItemTemplate>
                                        <asp:Label ID="LblMoneda" Text='<%# Eval("CodMoneda") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="propuesta">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPT" Text='<%# Eval("PPT") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        </asp:GridView>
                        <asp:ImageButton ID="IbtAprDetAll" runat="server" ImageUrl="~/images/Check1.png" ImageAlign="AbsBottom" Height="30px" Width="30px" OnClick="IbtAprDetAll_Click" Visible="false" />
                        <asp:GridView ID="GrdModalBusqCot" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" Visible="false"
                            DataKeyNames="CodReferencia, IdCotizacion, IdDetCotizacion, CodProveedor,CodMoneda, CodTipoCotizacion, TasaIVA, ValorIva, CodTipoPago"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CkbA" Checked='<%# Eval("Pasar").ToString()=="1" ? true : false %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="proveedor">
                                    <ItemTemplate>
                                        <asp:Label ID="LblRazSocl" Text='<%# Eval("RazonSocial") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="cotizacion">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCodCot" Text='<%# Eval("CodCotizacion") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pos.">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPs" Text='<%# Eval("PosDC") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P/N">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="descripcion">
                                    <ItemTemplate>
                                        <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="cant">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCant" Text='<%# Eval("cantidad") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="und med">
                                    <ItemTemplate>
                                        <asp:Label ID="LblUndM" Text='<%# Eval("CodUndMed") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="valor und">
                                    <ItemTemplate>
                                        <asp:Label ID="LblVlrUnd" Text='<%# Eval("ValorUnidad") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="valor Total">
                                    <ItemTemplate>
                                        <asp:Label ID="LbVlrTtl" Text='<%# Eval("ValorTotal") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="BtnAsignarModal" runat="server" class="btn btn-default" Text="asignar" OnClick="BtnAsignarModal_Click" />
                    <asp:Button ID="BtnCloseModalBusqCompra" runat="server" CssClass="btn btn-default" Text="cerrar" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <div class="CentrarContNumCotiza DivMarco">
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblNumCompra" runat="server" CssClass="LblEtiquet" Text="compra Nro.:" />
                                <asp:TextBox ID="TxtNumCompra" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblTtl" runat="server" CssClass="LblEtiquet" Text="Total" />
                                <asp:TextBox ID="TxtTtl" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFecha" runat="server" CssClass="LblEtiquet" Text="fecha" />
                                <asp:TextBox ID="TxtFecha" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblDatosPpt" runat="server" CssClass="LblEtiquet" Text="propuesta" />
                                <asp:TextBox ID="TxtDatosPpt" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" />
                            </div>
                        </div>
                    </div>
                    <div class="CentrarContenedor DivMarco">
                        <div id="Botones" class="row">
                            <div class="col-sm-4">
                                <asp:Button ID="BtnCompra" runat="server" CssClass="btn btn-outline-primary Font_btnCrud" OnClick="BtnCompra_Click" Width="32%" Font-Size="13px" Font-Bold="true" Text="compra" />
                                <asp:Button ID="BtnInterc" runat="server" CssClass="btn btn-outline-primary Font_btnCrud" OnClick="BtnInterc_Click" Width="32%" Font-Size="13px" Font-Bold="true" Text="intercambio" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnConsultar" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnConsultar_Click" OnClientClick="target ='';" Text="consultar" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnIngresar_Click" OnClientClick="target ='';" Text="nuevo" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnModificar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnModificar_Click" OnClientClick="target ='';" Text="modificar" />
                            </div>
                            <%-- <div class="col-sm-1">
                                <asp:Button ID="BtnCargaMaxiva" runat="server" CssClass="btn btn-success Font_btnCrud" OnClick="BtnCargaMaxiva_Click" Text="Cargar" Width="100%" Enabled="false" />
                            </div>--%>
                            <%-- <div class="col-sm-1">
                                <asp:Button ID="BtnEliminar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnEliminar_Click" OnClientClick="target ='';" Text="eliminar" />
                            </div>--%>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnAsentar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnAsentar_Click" OnClientClick="target ='';" Text="asentar" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnImprimir" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnImprimir_Click" OnClientClick="target ='';" Text="imprimir" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnAuxiliares" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnAuxiliares_Click" OnClientClick="target ='';" Text="Auxiliares" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnOpenCotiza" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnOpenCotiza_Click" OnClientClick="target ='';" Text="cotizacion" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label ID="LblProvee" runat="server" CssClass="LblEtiquet" Text="proveedor" />
                                <asp:DropDownList ID="DdlProvee" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblEmplead" runat="server" CssClass="LblEtiquet" Text="empleado" />
                                <asp:DropDownList ID="DdlEmplead" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblAutoriz" runat="server" CssClass="LblEtiquet" Text="autorizacion" />
                                <asp:DropDownList ID="DdlAutoriz" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-1">
                                <asp:Label ID="LblMoned" runat="server" CssClass="LblEtiquet" Text="moneda" />
                                <asp:TextBox ID="TxtMoned" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblTipo" runat="server" CssClass="LblEtiquet" Text="tipo" />
                                <asp:DropDownList ID="DdlTipo" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblTransp" runat="server" CssClass="LblEtiquet" Text="transportador" />
                                <asp:DropDownList ID="DdlTransp" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblTipoPago" runat="server" CssClass="LblEtiquet" Text="tipo pago" />
                                <asp:DropDownList ID="DdlTipoPago" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblEstd" runat="server" CssClass="LblEtiquet" Text="estado" />
                                <asp:DropDownList ID="DdlEstd" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6">
                                <asp:Label ID="LblUbicac" runat="server" CssClass="LblEtiquet" Text="ubicacion de entrega" />
                                <asp:DropDownList ID="DdlUbicac" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-6">
                                <asp:Label ID="LblEnvioFact" runat="server" CssClass="LblEtiquet" Text="ubicaciòn envìo factura" />
                                <asp:DropDownList ID="DdlEnvioFact" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3" style="">
                                <asp:Label ID="LblFacReferc" runat="server" CssClass="LblEtiquet" Text=" cotizacion referencia" />
                                <asp:TextBox ID="TxtFacReferc" runat="server" CssClass="form-control-sm heightCampo" MaxLength="100" Enabled="false" Width="100%" TextMode="MultiLine" Height="40px" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFactura" runat="server" CssClass="LblEtiquet" Text="factura ref" />
                                <asp:TextBox ID="TxtFactura" runat="server" CssClass="form-control-sm heightCampo TextR" MaxLength="240" Width="100%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                            </div>
                            <div class="col-md-3" style="">
                                <asp:Label ID="LblObsrv" runat="server" CssClass="LblEtiquet" Text="observaciones" />
                                <asp:TextBox ID="TxtObsrv" runat="server" CssClass="form-control-sm heightCampo" MaxLength="250" Enabled="false" Width="100%" TextMode="MultiLine" Height="40px" />
                            </div>

                            <div class="col-sm-2">
                                <br />
                                <asp:Table ID="TblAprob" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:CheckBox ID="CkbAprobad" runat="server" CssClass="LblEtiquet" Text="Aprobada" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                        <asp:TableCell>
                                            <asp:CheckBox ID="CkbAsentada" runat="server" CssClass="LblEtiquet" Text="Asentada" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                        </div>
                        <div id="valores" class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblSubTtal" runat="server" CssClass="LblEtiquet" Text="monto" />
                                <asp:TextBox ID="TxtSubTtal" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblIVA" runat="server" CssClass="LblEtiquet" Text="IVA" />
                                <asp:TextBox ID="TxtIVA" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblOtrImpt" runat="server" CssClass="LblEtiquet" Text="otros impuestos" />
                                <asp:TextBox ID="TxtOtrImptM" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                <asp:TextBox ID="TxtOtrImpt" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" OnTextChanged="TxtOtrImpt_TextChanged" AutoPostBack="true" Visible="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblRetencion" runat="server" CssClass="LblEtiquet" Text="retención" />
                                <asp:Table ID="TblRtfte" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell Width="80px">
                                            <asp:TextBox ID="TxtTasaRetefte" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" OnTextChanged="TxtTasaRetefte_TextChanged" AutoPostBack="true" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="150px">
                                            <asp:TextBox ID="TxtRetefteM" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                            <asp:TextBox ID="TxtRetefte" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" Visible="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblIca" runat="server" CssClass="LblEtiquet" Text="ICA" />
                                <asp:Table ID="TblIca" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell Width="80px">
                                            <asp:TextBox ID="TxtTasaICA" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" OnTextChanged="TxtTasaICA_TextChanged" AutoPostBack="true" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="150px">
                                            <asp:TextBox ID="TxtICAM" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                            <asp:TextBox ID="TxtICA" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" Visible="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblDescto" runat="server" CssClass="LblEtiquet" Text="Descuento" />
                                <asp:Table ID="TblDescto" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell Width="80px">
                                            <asp:TextBox ID="TxtTasaDescto" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" OnTextChanged="TxtTasaDescto_TextChanged" AutoPostBack="true" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="150px">
                                            <asp:TextBox ID="TxtDesctoM" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                            <asp:TextBox ID="TxtDescto" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" Visible="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                        </div>
                        <div class="ScrollDet2">
                            <asp:GridView ID="GrdDet" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true"
                                DataKeyNames="IdDetOrdenCompra, IdCotizacion, IdDetCotizacion"
                                CssClass="DiseñoGrid table table-sm" GridLines="Both" Width="100%" Enabled="false"
                                OnRowDeleting="GrdDet_RowDeleting" OnRowDataBound="GrdDet_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Pos." HeaderStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPosc" Text='<%# Eval("Posicion") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="cotizacion" HeaderStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCotz" Text='<%# Eval("CodCotizacion") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPN" Text='<%# Eval("Pn") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="descripc" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="cant" HeaderStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtCant" Text='<%# Eval("Cant") %>' runat="server" CssClass="TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="cant recibidas" HeaderStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtCantRecb" Text='<%# Eval("CantRecibida") %>' runat="server" CssClass="TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="und Medida" HeaderStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblUndMed" Text='<%# Eval("Und") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="valor und" HeaderStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtVlor" Text='<%# Eval("ValorUnidad") %>' runat="server" CssClass="TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total" HeaderStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblVlrTtl" Text='<%# Eval("ValorTotal") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField FooterStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
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
                </asp:View>
                <asp:View ID="Vw1Imprimir" runat="server">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitImpresion" runat="server" Text="Impresión" />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarImpr" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarImpr_Click" />
                    <br />
                    <RpVw:ReportViewer ID="RpVwAll" runat="server" Width="98%" />
                </asp:View>
                <asp:View ID="Vw2Exportar" runat="server">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitExport" runat="server" Text="Exportar" />
                    </h6>
                    <asp:ImageButton ID="IbtCloseExport" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCloseExport_Click" />
                    <div class="CentrarExportar DivMarco">
                        <div id="BtnesExport" class="row">
                            <div class="col-sm-6">
                                <asp:Button ID="BtnExportHistorico" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnExportHistorico_Click" OnClientClick="target ='';" Text="histo" />
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw3Asentar" runat="server">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitAsentar" runat="server" Text="Aprobar / Asentar Compra" />
                    </h6>
                    <asp:ImageButton ID="IbtCloseAsentar" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCloseAsentar_Click" />
                    <div class="CentrarExportar DivMarco">
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <h6 class="TextoSuperior">
                                            <asp:Label ID="LblTitOpcAprob" runat="server" Text="aprbar" />
                                        </h6>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="IbtAprobar" runat="server" ToolTip="aprobación" Width="60px" Height ="60px" ImageUrl="~/images/UnCheck.png" OnClick="IbtAprobar_Click" />
                                        <asp:ImageButton ID="IbtDesAprobar" runat="server" ToolTip="desaprobación"  Width="60px" Height ="60px" ImageUrl="~/images/Check1.png" OnClick="IbtDesAprobar_Click" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <h6 class="TextoSuperior">
                                            <asp:Label ID="LblTitOpcAsentr" runat="server" Text="asentar" />
                                        </h6>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="IbtAsentar" runat="server" ToolTip="asentar" Width="60px" Height ="60px" ImageUrl="~/images/UnCheck.png" OnClick="IbtAsentar_Click" />
                                        <asp:ImageButton ID="IbtDesasentar" runat="server" ToolTip="desasentar" Width="60px" Height ="60px" ImageUrl="~/images/Check1.png" OnClick="IbtDesasentar_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnConsultar" />
            <asp:PostBackTrigger ControlID="BtnImprimir" />
            <asp:PostBackTrigger ControlID="BtnExportHistorico" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
