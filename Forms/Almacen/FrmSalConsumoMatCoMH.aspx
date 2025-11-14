<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmSalConsumoMatCoMH.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmSalConsumoMatCoMH" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarCntndr {
            position: absolute;
            left: 50%;
            width: 98%;
            margin-left: -49%;
            height: 85%;
            padding: 5px;
            top: 150px
        }

        .Interna {
            position: absolute;
            top: 15%;
            left: 50%;
            transform: translate(-50%, -50%);
        }

        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .WithTableRdb {
            width: 18%;
        }

        .WithTable {
            width: 20%;
        }

        .WithTableNum {
            width: 20%;
        }

        .WithTableUsuR {
            width: 50%;
        }

        .WithTableEspac {
            width: 3%;
        }

        ScrollRsva {
            vertical-align: top;
            overflow: auto;
            width: 80%;
            height: 100px;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
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
            $('#<%=DdlAlmacen.ClientID%>').chosen();
            $('#<%=DdlAeronave.ClientID%>').chosen();
            $('#<%=DdlNumRsva.ClientID%>').chosen();
            $('#<%=DdlUsuRecibe.ClientID%>').chosen();
        }
        function ShowPopup() {
            $('#ModalCondManplc').modal('show');
            $('#ModalCondManplc').on('shown.bs.modal', function () {
                document.getElementById('<%= BtnCloseMdl.ClientID %>').focus();
                document.getElementById('<%= BtnCloseMdl.ClientID %>').select();
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <div id="ModalCondManplc" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-title" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="LblTitCondManiplc" runat="server" Text="condición de almacenamiento y manipulación" /></h4>
                </div>
                <div class="modal-body">
                    <div class="pre-scrollable">
                        <asp:GridView ID="GrdMdlCondManplc" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both">
                            <Columns>
                                <asp:TemplateField HeaderText="P/N">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TxtDescr" Text='<%# Eval("Descripcion") %>' runat="server" TextMode="MultiLine" Enabled="false" Width="100%" />
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
                    <asp:Button ID="BtnCloseMdl" runat="server" CssClass="btn btn-default" Text="cerrar" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <div class="CentrarCntndr">
                        <div id="Almacen" class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblAlmacen" runat="server" CssClass="LblEtiquet" Text="almacen" />
                                <asp:DropDownList ID="DdlAlmacen" runat="server" CssClass="heightCampo" Width="100%" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblAeronave" runat="server" CssClass="LblEtiquet" Text="aeronave" />
                                <asp:DropDownList ID="DdlAeronave" runat="server" CssClass="heightCampo" Width="100%" />
                            </div>
                            <div class="col-sm-6">
                                <asp:Label ID="LblObserv" runat="server" CssClass="LblEtiquet" Text="Observaciones" />
                                <asp:TextBox ID="TxtObserv" runat="server" CssClass="form-control-sm" Width="100%" MaxLength="350" TextMode="MultiLine" Text="" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <table width="100%">
                                    <tr>
                                        <td class="WithTableRdb">
                                            <asp:RadioButton ID="RdbNumRsva" runat="server" CssClass="LblEtiquet" GroupName="Rva" Checked="true" Text="reserva &nbsp" OnCheckedChanged="RdbNumRsva_CheckedChanged" AutoPostBack="true" />&nbsp&nbsp&nbsp
                                            <asp:RadioButton ID="RdbRvaOT" runat="server" CssClass="LblEtiquet" GroupName="Rva" Text="o.t. &nbsp" OnCheckedChanged="RdbRvaOT_CheckedChanged" AutoPostBack="true" />&nbsp&nbsp&nbsp
                                            <asp:RadioButton ID="RdbRvaRte" runat="server" CssClass="LblEtiquet" GroupName="Rva" Text="Reporte &nbsp" OnCheckedChanged="RdbRvaRte_CheckedChanged" AutoPostBack="true" /></td>
                                        <td class="WithTableNum">
                                            <asp:DropDownList ID="DdlNumRsva" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlNumRsva_TextChanged" AutoPostBack="true" />
                                        </td>

                                        <%-- <td class="WithTableEspac"></td>--%>
                                        <td>
                                            <asp:Label ID="LblUsuRecibe" runat="server" CssClass="LblEtiquet" Text="Recibe:" />
                                        </td>
                                        <td class="WithTableUsuR">
                                            <asp:DropDownList ID="DdlUsuRecibe" runat="server" CssClass="heightCampo" Width="100%" />
                                        </td>
                                        <td class="WithTable">
                                            <br />
                                            <asp:Button ID="BtnVisualizar" runat="server" CssClass="btn btn-primary" Width="100%" OnClick="BtnVisualizar_Click" Text="visualizar" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label ID="LblNumReserva" runat="server" CssClass="LblEtiquet" Text="" />
                                <asp:Label ID="LblNumOrdeTrabajo" runat="server" CssClass="LblEtiquet" Text="" />
                                <asp:Label ID="LblNumReporte" runat="server" CssClass="LblEtiquet" Text="" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="ScrollRsva pre-scrollable">
                                    <asp:GridView ID="GrdDtllRsva" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="CodIdDetalleRes,Estado"
                                        CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" OnRowCommand="GrdDtllRsva_RowCommand" OnRowDataBound="GrdDtllRsva_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="UplAbrir" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ImageButton ID="IbtAbrir" Width="30px" Height="30px" ImageUrl="~/images/ReportV1.png" runat="server" CommandName="Abrir" ToolTip="asignar ubicacion fisica" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="IbtAbrir" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pos">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPos" Text='<%# Eval("NumeroPosicion") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="referencia">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblRef" Text='<%# Eval("Codreferencia") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="descripcion">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="tipo">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblTipo" Text='<%# Eval("TipoElem") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="identificador">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblIdentfc" Text='<%# Eval("Identificador") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant solic">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantSol" Text='<%# Eval("CantidadSolicitada") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant entreg">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantEntr" Text='<%# Eval("CantidadEntregada") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant despacho">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantDesp" Text='<%# Eval("CantDespachar") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="und med">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblUndMed" Text='<%# Eval("CodUndMedR") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw1Busq" runat="server">
                    <br /><br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitAsigFis" runat="server" Text="Asignar elemento a la reserva" />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarAsing" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarAsing_Click" />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div class="CentrarCntndr">
                        <div class="col-sm-8 Interna">
                            <div class="ScrollRsva pre-scrollable">
                                 <br /> <br /> <br /> <br /> <br />
                                <div class="col-sm-3">
                                    <asp:Button ID="BtnAsignr" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnAsignr_Click" OnClientClick="target ='';" Text="asignar" />
                                </div>
                                <br />
                                <asp:Label ID="LblPNDescripcAsig" runat="server" CssClass="LblEtiquet" Text="" />
                                <asp:Label ID="LblAsigCantSol" runat="server" CssClass="LblEtiquet" Text="cantidad solic: " />
                                <asp:Label ID="LblAsigCantSolV" runat="server" CssClass="LblEtiquet" Text="" />
                                <asp:Label ID="LblAsigCantEntrg" runat="server" CssClass="LblEtiquet" Text=" | cantidad entre: " />
                                <asp:Label ID="LblAsigCantEntrgV" runat="server" CssClass="LblEtiquet" Text="" />
                                <asp:GridView ID="GrdUbicaFisc" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                    DataKeyNames="CodIdUbicacion,CodElemento,CodUbicaBodega, CodTercero, FechaVencimientoR, FechaShelfLife, CantidadSolicitada, CantidadEntregada, CodReferencia, IdentificadorElemR, Activo, CodEstadoPn"
                                    CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" OnRowDataBound="GrdUbicaFisc_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="estado P/N">
                                            <ItemTemplate>
                                                <asp:Label ID="LblEstado" Text='<%# Eval("EstadoPN") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P/N">
                                            <ItemTemplate>
                                                <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="S/N">
                                            <ItemTemplate>
                                                <asp:Label ID="LblSn" Text='<%# Eval("SN") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="lote">
                                            <ItemTemplate>
                                                <asp:Label ID="LblLot" Text='<%# Eval("NumLote") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="modelo P/N">
                                            <ItemTemplate>
                                                <asp:Label ID="LblModelPN" Text='<%# Eval("NSN") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="bodega">
                                            <ItemTemplate>
                                                <asp:Label ID="LblBodg" Text='<%# Eval("CodBodega") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="fila" HeaderStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblFila" Text='<%# Eval("Fila") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Columna" HeaderStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblColumn" Text='<%# Eval("Columna") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="stock">
                                            <ItemTemplate>
                                                <asp:Label ID="LblStock" Text='<%# Eval("Cantidad") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="cant despacho">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtCantDespa" Text='<%# Eval("CantDespchr") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="und medida">
                                            <ItemTemplate>
                                                <asp:Label ID="LblUndMed" Text='<%# Eval("CodUndMedR") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="fecha vencimiento">
                                            <ItemTemplate>
                                                <asp:Label ID="LblFecVence" Text='<%# Eval("FecSLMDY") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="GridCabecera" />
                                    <RowStyle CssClass="GridRowStyle" />
                                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw2Entrega" runat="server">
                    <br /> <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitVisualizaGuarda" runat="server" Text="Visualizar los elementos de entrega" />
                    </h6>
                    <asp:ImageButton ID="IbtCloseGuardar" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCloseGuardar_Click" />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div class="CentrarCntndr">
                        <div class="col-sm-8 Interna">
                            <div class="ScrollRsva pre-scrollable">
                                 <br /> <br /> <br /> <br /> <br />
                                <div class="col-sm-3">
                                    <asp:Button ID="BtnGuardar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnGuardar_Click" OnClientClick="target ='';" Text="guardar" />
                                </div>
                                <br />
                                <asp:Label ID="LblNumRvaGuardar" runat="server" CssClass="LblEtiquet" Text="reserva: " />
                                <asp:Label ID="LblNumRvaVlorGuardar" runat="server" CssClass="LblEtiquet" Text="" />
                                <asp:GridView ID="GrdVisualizar" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="CodIdDetalleRes"
                                    CssClass="GridControl DiseñoGrid table-sm" GridLines="Both">
                                    <Columns>
                                        <asp:TemplateField HeaderText="pos">
                                            <ItemTemplate>
                                                <asp:Label ID="LblPos" Text='<%# Eval("Pos") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="referencia">
                                            <ItemTemplate>
                                                <asp:Label ID="LblCodRef" Text='<%# Eval("CodReferencia") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P/N">
                                            <ItemTemplate>
                                                <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="S/N">
                                            <ItemTemplate>
                                                <asp:Label ID="LblSn" Text='<%# Eval("SN") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="lote">
                                            <ItemTemplate>
                                                <asp:Label ID="LblLot" Text='<%# Eval("NumLote") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="modelo P/N">
                                            <ItemTemplate>
                                                <asp:Label ID="LblModelPN" Text='<%# Eval("NSN") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="bodega">
                                            <ItemTemplate>
                                                <asp:Label ID="LblBodg" Text='<%# Eval("CodBodega") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="fila" HeaderStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblFila" Text='<%# Eval("Fila") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Columna" HeaderStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblColumn" Text='<%# Eval("Columna") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="stock">
                                            <ItemTemplate>
                                                <asp:Label ID="LblStock" Text='<%# Eval("Cantidad") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="cant despacho">
                                            <ItemTemplate>
                                                <asp:Label ID="LblCantDespc" Text='<%# Eval("CantDespchr") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="und medida">
                                            <ItemTemplate>
                                                <asp:Label ID="LblUndMed" Text='<%# Eval("CodUndMedR") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="GridCabecera" />
                                    <RowStyle CssClass="GridRowStyle" />
                                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID ="RdbNumRsva" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
