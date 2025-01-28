<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmReparacion.aspx.cs" Inherits="_77NeoWeb.Forms.InventariosCompras.FrmReparacion" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="RpVw" %>
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
        }

        .CentrarContenedor {
            position: absolute;
            left: 50%;
            width: 97%;
            margin-left: -49%;
            height: 78%;
            padding: 5px;
        }

        .CentrarBotonesRepa {
            left: 50%;
            /*determinamos una anchura*/
            width: 97%;
            margin-left: 1%;
            height: 8%;
        }

        .SubTituloLicencia {
            width: 60%;
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
            $('#<%=DdlEstd.ClientID%>').chosen();
            $('#<%=DdlTipo.ClientID%>').chosen();
            $('#<%=DdlTransp.ClientID%>').chosen();
            $('#<%=DdlUbicac.ClientID%>').chosen();
            $('[id *=DdlSolPed]').chosen();
            $('[id *=DdlOtEstado]').chosen();
            $('[id *=DdlOTPrioridad]').chosen();
            $('[id *=DdlOtTaller]').chosen();
        }
        $(':text').on("focus", function () {
            //here set in localStorage id of the textbox
            localStorage.setItem("focusItem", this.id);
            //console.log(localStorage.getItem("focusItem"));test the focus element id
        });
        function ShowPopup() {
            $('#ModalBusqRepa').modal('show');
            $('#ModalBusqRepa').on('shown.bs.modal', function () {
                document.getElementById('<%= TxtModalBusq.ClientID %>').focus();
                document.getElementById('<%= TxtModalBusq.ClientID %>').select();
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
   <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <div id="ModalBusqRepa" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="LblTitModalBusqRepa" runat="server" Text="opciones de bùsqueda" />
                        <asp:Label ID="LblTitModalBusqProv" runat="server" Text="asignar proveedor" Visible="false" /></h4>
                </div>
                <div class="modal-body">
                    <asp:Table ID="TblMdlOpcBusRepa" runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:RadioButton ID="RdbMdlBusqRepa" runat="server" CssClass="LblEtiquet" Text="&nbsp reparacion" GroupName="BusqRp" />&nbsp&nbsp&nbsp                               
                                <asp:RadioButton ID="RdbMdlBusqPN" runat="server" CssClass="LblEtiquet" Text="&nbsp P/N" GroupName="BusqRp" />&nbsp&nbsp&nbsp
                                <asp:RadioButton ID="RdbMdlBusqSN" runat="server" CssClass="LblEtiquet" Text="&nbsp S/N" GroupName="BusqRp" />&nbsp&nbsp&nbsp
                                <asp:RadioButton ID="RdbMdlBusqOT" runat="server" CssClass="LblEtiquet" Text="&nbsp o.t." GroupName="BusqRp" />&nbsp&nbsp&nbsp
                                <asp:RadioButton ID="RdbMdlBusqPrv" runat="server" CssClass="LblEtiquet" Text="&nbsp proveedor" GroupName="BusqRp" />&nbsp&nbsp&nbsp
                                <asp:RadioButton ID="RdbMdlBusqPPT" runat="server" CssClass="LblEtiquet" Text="&nbsp propuesta" GroupName="BusqRp" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:Table ID="TblMdlOpcBusCotiza" runat="server" Visible="false">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:RadioButton ID="RdbMdlOpcBusqPrv" runat="server" CssClass="LblEtiquet" Text="&nbsp proveedor" GroupName="BusqProvee" />&nbsp&nbsp&nbsp                               
                                <asp:RadioButton ID="RdbMdlOpcBusqCotiz" runat="server" CssClass="LblEtiquet" Text="&nbsp cotizacion" GroupName="BusqProvee" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <table>
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
                        <asp:GridView ID="GrdModalBusqRepa" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="CodReparacion"
                            CssClass="GridControl DiseñoGrid" GridLines="Both" AllowPaging="true" OnRowCommand="GrdBusq_RowCommand" OnRowDataBound="GrdBusq_RowDataBound">
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
                                <asp:TemplateField HeaderText="reparacion">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCodRepa" Text='<%# Eval("CodReparacion") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="proveedor">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("RazonSocial") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("FechaReparacion") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="25%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("PN") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="S/N"  HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipo Elemento">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("TipoElem") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="cotizacion">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCotiza" Text='<%# Eval("CodCotizacion") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="pedido">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPedido" Text='<%# Eval("CodPedido") %>' runat="server" />
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
                                <asp:TemplateField HeaderText="ot">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CodNumORdenTrab") %>' runat="server" />
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
                        <asp:GridView ID="GrdMdlBusCotiza" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" Visible="false"
                            DataKeyNames="CodTercero, Cantidad, CodTipoCotizacion, IdPedido, CodPedido, Notas,CodAeronaveCT, LugarEntrega, IdPropuesta, DescricionServicio, Matricula, Monto, ValorTotalCot"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnRowCommand="GrdMdlBusCotiza_RowCommand" OnRowDataBound="GrdMdlBusCotiza_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:UpdatePanel ID="UplIr" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ImageButton ID="IbtIrCot" Width="30px" Height="30px" ImageUrl="~/images/IrV2.png" runat="server" CommandName="Ir" ToolTip="Ir" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="IbtIrCot" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Razon Social">
                                    <ItemTemplate>
                                        <asp:Label ID="LblRaznScl" Text='<%# Eval("RazonSocial") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cotizacion">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCodCtzc" Text='<%# Eval("CodCotizacion") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P/N">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="S/N">
                                    <ItemTemplate>
                                        <asp:Label ID="LblSN" Text='<%# Eval("Notas") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="moneda">
                                    <ItemTemplate>
                                        <asp:Label ID="LblMoneda" Text='<%# Eval("CodMoneda") %>' runat="server" />
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
                    <asp:Button ID="BtnCloseModalBusqPN" runat="server" CssClass="btn btn-default" Text="cerrar" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpPnlBtnPpl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br /><br />
            <div class="CentrarBotonesRepa DivMarco">
                <div id="BotonTipoRepa" class="row">
                    <div class="col-sm-2">
                        <asp:Button ID="BtnRepaExterna" CssClass="btn btn-outline-primary" runat="server" Text="externa" Width="100%" OnClick="BtnRepaExterna_Click" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Button ID="BtnRepaLocal" CssClass="btn btn-outline-primary" runat="server" Text="local" Width="100%" OnClick="BtnRepaLocal_Click" />
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="TxtCCosto" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Height="35px" Enabled="false" ToolTip="centro de costo" />
                    </div>
                    <div class="col-sm-2">
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
            </div>
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <div class="CentrarContenedor DivMarco">
                        <div id="Botones" class="row">
                            <div class="col-sm-1">
                                <asp:Button ID="BtnConsultar" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnConsultar_Click" OnClientClick="target ='';" Text="consultar" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnIngresar_Click" OnClientClick="target ='';" Text="nuevo" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnModificar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnModificar_Click" OnClientClick="target ='';" Text="modificar" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnOT" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnOT_Click" OnClientClick="target ='';" Text="O.T." />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnAsentar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnAsentar_Click" OnClientClick="target ='';" Text="asentar" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnImprimir" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnImprimir_Click" OnClientClick="target ='';" Text="imprimir" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnOpenCotiza" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnOpenCotiza_Click" OnClientClick="target ='';" Text="cotizacion" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnSolPedInter" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnSolPedInter_Click" OnClientClick="target ='';" Text="Pedido" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblCotizac" runat="server" CssClass="LblEtiquet" Text="Cotizacion" />
                                <asp:TextBox ID="TxtCotizac" runat="server" CssClass="form-control-sm heightCampo" Width="60%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblPedido" runat="server" CssClass="LblEtiquet" Text="Pedido" />
                                <asp:TextBox ID="TxtPedido" runat="server" CssClass="form-control-sm heightCampo" Width="60%" Enabled="false" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblNumRepa" runat="server" CssClass="LblEtiquet" Text="Nro." />
                                <asp:TextBox ID="TxtNumRepa" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFecha" runat="server" CssClass="LblEtiquet" Text="fecha" />
                                <asp:TextBox ID="TxtFecha" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="LblOT" runat="server" CssClass="LblEtiquet" Text="num O.T." />
                                <asp:TextBox ID="TxtOT" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" Visible ="false" />
                                <asp:TextBox ID="TxtCodigoOT" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="LblReserva" runat="server" CssClass="LblEtiquet" Text="reserva" />
                                <asp:TextBox ID="TxtReserva" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblHK" runat="server" CssClass="LblEtiquet" Text="Aeronave" />
                                <asp:TextBox ID="TxtHK" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="LblMoned" runat="server" CssClass="LblEtiquet" Text="moneda" />
                                <asp:TextBox ID="TxtMoned" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="LblPpt" runat="server" CssClass="LblEtiquet" Text="Propuesta" />
                                <asp:TextBox ID="TxtPpt" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                 <asp:Label ID="LblFactura" runat="server" CssClass="LblEtiquet" Text="factura ref" />
                                <asp:TextBox ID="TxtFactura" runat="server" CssClass="form-control-sm heightCampo TextR" MaxLength="50" Width="100%" step="0" onkeypress="return solonumeros(event);" Enabled="false" />
                            </div>
                        </div>
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
                            <div class="col-sm-2">
                                <asp:Label ID="LblEstd" runat="server" CssClass="LblEtiquet" Text="estado" />
                                <asp:DropDownList ID="DdlEstd" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblTipo" runat="server" CssClass="LblEtiquet" Text="tipo" />
                                <asp:DropDownList ID="DdlTipo" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblTransp" runat="server" CssClass="LblEtiquet" Text="transportador" />
                                <asp:DropDownList ID="DdlTransp" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-5">
                                <asp:Label ID="LblUbicac" runat="server" CssClass="LblEtiquet" Text="ubicacion de entrega" />
                                <asp:DropDownList ID="DdlUbicac" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblPN" runat="server" CssClass="LblEtiquet" Text="P/N" />
                                <asp:TextBox ID="TxtlPN" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblSN" runat="server" CssClass="LblEtiquet" Text="S/N" />
                                <asp:TextBox ID="TxtSN" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="LblCant" runat="server" CssClass="LblEtiquet" Text="Cantidad" />
                                <asp:TextBox ID="TxtCant" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblSubTtal" runat="server" CssClass="LblEtiquet" Text="sub total" />
                                <asp:TextBox ID="TxtSubTtal" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblTtl" runat="server" CssClass="LblEtiquet" Text="total" />
                                <asp:TextBox ID="TxtTtl" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label ID="LblRazonRemoc" runat="server" CssClass="LblEtiquet" Text="razon remoc" />
                                <asp:TextBox ID="TxtRazonRemoc" runat="server" CssClass="form-control-sm heightCampo" Width="100%" MaxLength="300" TextMode="MultiLine" Height="50px" Enabled="false" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblObserv" runat="server" CssClass="LblEtiquet" Text="Observaciones" />
                                <asp:TextBox ID="TxtObserv" runat="server" CssClass="form-control-sm heightCampo" Width="100%" MaxLength="200" TextMode="MultiLine" Height="50px" Enabled="false" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblBoletines" runat="server" CssClass="LblEtiquet" Text="boletines" />
                                <asp:TextBox ID="TxtBoletines" runat="server" CssClass="form-control-sm heightCampo" Width="100%" MaxLength="200" TextMode="MultiLine" Height="50px" Enabled="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-8">
                                <asp:Table ID="TbInstr" runat="server" CssClass="table-responsive">
                                    <asp:TableHeaderRow>
                                        <asp:TableHeaderCell ColumnSpan="20">
                                            <h6 class="TextoSuperior">
                                                <asp:Label ID="LblTitInstruc" runat="server" Text="Instruciones" Width="100%" /></h6>
                                        </asp:TableHeaderCell>
                                    </asp:TableHeaderRow>
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:CheckBox ID="CkbRepair" runat="server" CssClass="LblEtiquet" Text="reparacion" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                        <asp:TableCell>
                                            <asp:CheckBox ID="CkbBancoPrueb" runat="server" CssClass="LblEtiquet" Text="Banco de Prueba" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                        <asp:TableCell>
                                            <asp:CheckBox ID="CkbOH" runat="server" CssClass="LblEtiquet" Text="overhaul" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                        <asp:TableCell>
                                            <asp:CheckBox ID="CkbModifc" runat="server" CssClass="LblEtiquet" Text="modificacion" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                        <asp:TableCell>
                                            <asp:CheckBox ID="CkbCalibrac" runat="server" CssClass="LblEtiquet" Text="calibraacion" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                        <asp:TableCell>
                                            <asp:CheckBox ID="CkbOtros" runat="server" CssClass="LblEtiquet" Text="otros :" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox ID="TxtOtros" runat="server" CssClass="form-control-sm heightCampo" Width="150%" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                            <div class="col-sm-4">
                                <asp:Table ID="TblGarantia" runat="server">
                                    <asp:TableHeaderRow>
                                        <asp:TableHeaderCell ColumnSpan="20">
                                            <h6 class="TextoSuperior">
                                                <asp:Label ID="Label1" runat="server" Text="&nbsp" Width="100%" /></h6>
                                        </asp:TableHeaderCell>
                                    </asp:TableHeaderRow>
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:CheckBox ID="CkbGrtAOG" runat="server" CssClass="LblEtiquet" Text="A.O.G." Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                        <asp:TableCell>
                                            <asp:CheckBox ID="CkbGrtGrntia" runat="server" CssClass="LblEtiquet" Text="Garantía" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell>&nbsp&nbsp</asp:TableCell>
                                        <asp:TableCell>
                                            <asp:CheckBox ID="CkbGrtOH" runat="server" CssClass="LblEtiquet" Text="overhaul" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <table class="table-responsive">
                                    <tr>
                                        <td colspan="5">
                                            <h6 class="TextoSuperior">
                                                <asp:Label ID="LblTitInstrucGnrl" runat="server" Text="Instruciones generales" /></h6>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CkbLibera1" runat="server" CssClass="LblEtiquet" Text="AIRWORTHINESS RELEASE: (FAA 8130-3 OR JAA FORM ONE OR TC 24-0078 OR SERVICEABLE TAG)" Enabled="false" /></td>
                                        <td>&nbsp&nbsp</td>
                                        <td>
                                            <asp:CheckBox ID="CkbCertifCalib2" runat="server" CssClass="LblEtiquet" Text="certificado de calibracion" Enabled="false" /></td>
                                        <td>&nbsp&nbsp</td>
                                        <td>
                                            <asp:CheckBox ID="CkbTrabaPedi3" runat="server" CssClass="LblEtiquet" Text="Trabajo pedido o informe de desmonteje" Enabled="false" /></td>
                                    </tr>
                                </table>
                                <table class="table-responsive">
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CkbEstandUtili4" runat="server" CssClass="LblEtiquet" Text="Estandar utilizado con fecha de calibracion" Enabled="false" /></td>
                                        <td>&nbsp&nbsp</td>
                                        <td>
                                            <asp:CheckBox ID="CkbCumplirTodoBolet5" runat="server" CssClass="LblEtiquet" Text="Cumplir con todo s los boletines de servicios AD y SB" Enabled="false" /></td>
                                        <td>&nbsp&nbsp</td>
                                        <td>
                                            <asp:CheckBox ID="CkbTodoTrabReal6" runat="server" CssClass="LblEtiquet" Text="Todos el trabajo realizado sera incluido en el diario y la entrada sera los SB" Enabled="false" /></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw1SolPedInter" runat="server">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitSolPedInter" runat="server" Text="Solicitud de Pedido." />
                    </h6>
                    <div class="CentrarContenedor  DivMarco">
                        <asp:ImageButton ID="IbtCerrarSolPedInter" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarSolPedInter_Click" />
                        <asp:GridView ID="GrdSolPedInter" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true"
                            DataKeyNames="IDRepaDetSolPed,IdDetPedido,IdPedido"
                            CssClass="DiseñoGrid table table-sm SubTituloLicencia" GridLines="Both" AllowPaging="true"
                            OnRowCommand="GrdSolPedInter_RowCommand" OnRowDeleting="GrdSolPedInter_RowDeleting" OnRowDataBound="GrdSolPedInter_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Pedido" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCodPedido" Text='<%# Eval("CodPedido") %>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="DdlSolPed" runat="server" Width="100%" Height="28px" AutoPostBack="true" OnTextChanged="DdlSolPed_TextChanged" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtPn" runat="server" Width="100%" Enabled="false" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="referencia" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CodReferencia") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtRef" runat="server" Width="100%" Enabled="false" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtSn" runat="server" Width="100%" Enabled="false" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="descripcion" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtDescr" runat="server" Width="100%" Enabled="false" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="cant" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CantidadTotal") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtCant" runat="server" Width="100%" Enabled="false" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField FooterStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="GridFooterStyle" />
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        </asp:GridView>
                    </div>
                </asp:View>
                <asp:View ID="Vw2NewOT" runat="server">

                    <div class="CentrarContenedor  DivMarco">
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitNewOT" runat="server" Text="Generar Orden de trabajo" />
                        </h6>
                        <div id="BotonesOT" class="row">
                            <div class="col-sm-1">
                                <asp:Button ID="BtnOTNew" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnOTNew_Click" OnClientClick="target ='';" Text="nuevo" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnOpenOT" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnOpenOT_Click" OnClientClick="target ='';" Text="o.t." />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnOTCerrar" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnOTCerrar_Click" OnClientClick="target ='';" Text="cerrar" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblOtNumOT" runat="server" CssClass="LblEtiquet" Text="O.T." />
                                <asp:TextBox ID="TxtOtNumOT" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="60%" Height="30px" Visible ="false" />
                                <asp:TextBox ID="TxtOtCodigoOT" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="60%" Height="30px" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblOtEstado" runat="server" CssClass="LblEtiquet" Text="estado" />
                                <asp:DropDownList ID="DdlOtEstado" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblOtPrioridad" runat="server" CssClass="LblEtiquet" Text="prioridad:" />
                                <asp:DropDownList ID="DdlOTPrioridad" runat="server" CssClass="Campos" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblOtFechaReg" runat="server" CssClass="LblEtiquet" Text="fecha" />
                                <asp:TextBox ID="TxtOtFechaReg" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblOtFechaIni" runat="server" CssClass="LblEtiquet" Text="fecha inicial" />
                                <asp:TextBox ID="txtOtFechaIni" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblOtFechaFin" runat="server" CssClass="LblEtiquet" Text="fecha inicial" />
                                <asp:TextBox ID="TxtOtFechaFin" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblOtNumRepa" runat="server" CssClass="LblEtiquet" Text="Nro. Reparación" />
                                <asp:TextBox ID="TxtOtNumRepa" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label ID="LblOtTaller" runat="server" CssClass="LblEtiquet" Text="Taller" />
                                <asp:DropDownList ID="DdlOtTaller" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblOTTrabajo" runat="server" CssClass="LblEtiquet" Text="Trabajo Requerido" />
                                <asp:TextBox ID="TxtOTTrabajo" runat="server" CssClass="form-control-sm" TextMode="MultiLine" MaxLength="240" Width="100%" Font-Size="10px" Enabled="false" Height="50px" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblOTAccParc" runat="server" CssClass="LblEtiquet" Text="accion parcial" />
                                <asp:TextBox ID="TxtOTAccParc" runat="server" CssClass="form-control-sm" TextMode="MultiLine" MaxLength="240" Width="100%" Font-Size="10px" Enabled="false" Height="50px" />
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw3Imprimir" runat="server">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitImpresion" runat="server" Text="Impresión" />
                    </h6>
                    <%-- <asp:Button ID="BtnImprPpal" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="5%" OnClick="BtnImprPpal_Click" Text="Principal" />
                    <asp:Button ID="BtnImprDet" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="5%" OnClick="BtnImprDet_Click" Text="Detalle" />--%>
                    <asp:ImageButton ID="IbtCerrarImpr" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarImpr_Click" />
                    <br />
                    <RpVw:ReportViewer ID="RpVwAll" runat="server" Width="98%" />
                </asp:View>
                <asp:View ID="Vw4Asentar" runat="server">
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
                                        <asp:ImageButton ID="IbtAprobar" runat="server" ToolTip="aprobación" Width="60px" Height="60px" ImageUrl="~/images/UnCheck.png" OnClick="IbtAprobar_Click" />
                                        <asp:ImageButton ID="IbtDesAprobar" runat="server" ToolTip="desaprobación" Width="60px" Height="60px" ImageUrl="~/images/Check1.png" OnClick="IbtDesAprobar_Click" />
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
                                        <asp:ImageButton ID="IbtAsentar" runat="server" ToolTip="asentar" Width="60px" Height="60px" ImageUrl="~/images/UnCheck.png" OnClick="IbtAsentar_Click" />
                                        <asp:ImageButton ID="IbtDesasentar" runat="server" ToolTip="desasentar" Width="60px" Height="60px" ImageUrl="~/images/Check1.png" OnClick="IbtDesasentar_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnRepaExterna" />
            <asp:PostBackTrigger ControlID="BtnRepaLocal" />
            <asp:PostBackTrigger ControlID="BtnConsultar" />
            <asp:PostBackTrigger ControlID="BtnImprimir" />
            <%--<asp:PostBackTrigger ControlID="IbtDesAprobar" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
