<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmConsultaMovimientos.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmConsultaMovimientos" %>

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

        .ScrollAlter {
            vertical-align: top;
            overflow: auto;
            width: 80%;
            height: 100px;
        }

        .ScrollStockAlma {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 150px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">  

        function myFuncionddl() {
            $('#<%=DdlPN.ClientID%>').chosen();
            $('#<%=DdlSN.ClientID%>').chosen();
            $('#<%=DdlLote.ClientID%>').chosen();
        }
        function ShowPopup() {
            $('#ModalBusqSP').modal('show');
            $('#ModalBusqSP').on('shown.bs.modal', function () {
                document.getElementById('<%= TxtModalBusq.ClientID %>').focus();
                document.getElementById('<%= TxtModalBusq.ClientID %>').select();
            });
        } <%-- --%>

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
   <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <div id="ModalBusqSP" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">                
                <div class="modal-header">                    
                    <h4 class="modal-title">
                        <asp:Label ID="LblTitModalBusqPN" runat="server" Text="opciones de busqueda" /></h4>
                </div>
                <div class="modal-body">
                    <table class="TablaBusqueda">
                        <tr>
                            <td colspan="3">
                                <asp:RadioButton ID="RdbMdlBusqPN" runat="server" CssClass="LblEtiquet" Text="&nbsp P/N" GroupName="BusqSP" />&nbsp&nbsp&nbsp                               
                                <asp:RadioButton ID="RdbMdlBusqSN" runat="server" CssClass="LblEtiquet" Text="&nbsp S/N" GroupName="BusqSP" />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                                <asp:RadioButton ID="RdbMdlBusqLote" runat="server" CssClass="LblEtiquet" Text="&nbsp lote" GroupName="BusqSP" />&nbsp&nbsp&nbsp
                                <asp:RadioButton ID="RdbMdlBusqDesc" runat="server" CssClass="LblEtiquet" Text="&nbsp descripcion" GroupName="BusqSP" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblModalBusq" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                            <td>
                                <asp:TextBox ID="TxtModalBusq" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                            <td>
                                <asp:ImageButton ID="IbtModalBusq" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtModalBusq_Click" /></td>
                        </tr>
                    </table>
                    <br /><br /><br />
                    <div class="CentrarGrid pre-scrollable">
                        <asp:GridView ID="GrdMdlBusq" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                            DataKeyNames="PN, SN, LOTE, CodReferencia"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnRowCommand="GrdMdlBusq_RowCommand" OnRowDataBound="GrdMdlBusq_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtIrPN" Width="30px" Height="30px" ImageUrl="~/images/IrV2.png" runat="server" CommandName="IrPN" ToolTip="Ir" />
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
                                        <asp:Label ID="LblLot" Text='<%# Eval("LOTE") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="descripcion">
                                    <ItemTemplate>
                                        <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="tipo">
                                    <ItemTemplate>
                                        <asp:Label ID="LblTipo" Text='<%# Eval("DescTipo") %>' runat="server" />
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
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <br /><br /><br /><br />
            <div class="CentrarContenedor DivMarco">
                <div class="row">
                    <div class="col-sm-3">
                        <asp:Label ID="LblPN" runat="server" CssClass="LblEtiquet" Text="P/N" />
                        <asp:DropDownList ID="DdlPN" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlPN_TextChanged" AutoPostBack="true" />
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="LblSN" runat="server" CssClass="LblEtiquet" Text="SN" />
                        <asp:DropDownList ID="DdlSN" runat="server" CssClass="heightCampo" Width="100%" />
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="LblLote" runat="server" CssClass="LblEtiquet" Text="Lote" />
                        <asp:DropDownList ID="DdlLote" runat="server" CssClass="heightCampo" Width="100%" />
                    </div>
                    <div class="col-sm-1">
                        <asp:Label ID="LblTipo" runat="server" CssClass="LblEtiquet" Text="Tipo" />
                        <asp:TextBox ID="TxtTipo" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" Font-Size="10px" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="LblDescrPn" runat="server" CssClass="LblEtiquet" Text="Descripcion" />
                        <asp:TextBox ID="TxtDescrPn" runat="server" CssClass="form-control-sm" Enabled="false" Width="100%" TextMode="MultiLine" Font-Size="10px" />
                    </div>
                </div>
                <br />
                <div id="Botones" class="row">
                    <div class="col-sm-1">
                        <asp:Button ID="BtnEjecutar" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnEjecutar_Click" OnClientClick="target ='';" Text="ejecutar" />
                    </div>
                    <div class="col-sm-1">
                        <asp:Button ID="BtnConsultar" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnConsultar_Click" OnClientClick="target ='';" Text="consultar" />
                    </div>
                    <div class="col-sm-1">
                        <asp:CheckBox ID="CkbAlterno" runat="server" CssClass="LblEtiquet" Text="alternos" />
                    </div>
                    <div class="col-sm-1">
                        <asp:Label ID="LblStockActual" runat="server" CssClass="LblEtiquet" Text="stock actual" />
                        <asp:TextBox ID="TxtStockActual" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" />
                    </div>
                    <div class="col-sm-1">
                        <asp:Button ID="BtnExport" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnExport_Click" OnClientClick="target ='';" Text="exportar" />
                    </div>
                    <div class="col-sm-4">
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitAlterno" runat="server" Text="partes alternos" />
                        </h6>
                        <div class="ScrollAlter pre-scrollable">
                            <asp:GridView ID="GrdAlterno" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" OnRowCommand="GrdMdlBusq_RowCommand" OnRowDataBound="GrdMdlBusq_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="P/N">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="estado">
                                        <ItemTemplate>
                                            <asp:Label ID="LblEsta" Text='<%# Eval("Estado") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="bloqueado">
                                        <ItemTemplate>
                                            <asp:Label ID="LblBloque" Text='<%# Eval("Bloqueado") %>' runat="server" />
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
                <div id="Stock Almacen" class="row">
                    <div class="col-sm-12">
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitStock" runat="server" Text="Stock" />
                        </h6>
                        <div class="ScrollStockAlma">
                            <asp:GridView ID="GrdStokAlma" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                DataKeyNames="CodElemento, CodIdUbicacion, CodUbicaBodega, CodReferencia"
                                CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnRowDataBound="GrdStokAlma_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="almacen">
                                        <ItemTemplate>
                                            <asp:Label ID="LblAlmac" Text='<%# Eval("DescAlmacen") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="cant">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCant" Text='<%# Eval("Cant") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Und Med">
                                        <ItemTemplate>
                                            <asp:Label ID="LblUndMed" Text='<%# Eval("Unidad") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="estado">
                                        <ItemTemplate>
                                            <asp:Label ID="LblStd" Text='<%# Eval("Estado") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P/N">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPn" Text='<%# Eval("Pn") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="S/N">
                                        <ItemTemplate>
                                            <asp:Label ID="LblSn" Text='<%# Eval("Sn") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="lote">
                                        <ItemTemplate>
                                            <asp:Label ID="LblLot" Text='<%# Eval("NumLote") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="bodega">
                                        <ItemTemplate>
                                            <asp:Label ID="LblBod" Text='<%# Eval("CodBodega") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="fila">
                                        <ItemTemplate>
                                            <asp:Label ID="Lblfl" Text='<%# Eval("Fila") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="col">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCln" Text='<%# Eval("Columna") %>' runat="server" />
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
                <div id="Movimientos" class="row">
                    <div class="col-sm-12">
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitMovimientos" runat="server" Text="movimientos" />
                        </h6>
                        <div class="ScrollStockAlma">
                            <asp:GridView ID="GrdMvtos" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                DataKeyNames="IdDoc" OnRowEditing="GrdMvtos_RowEditing" OnRowUpdating="GrdMvtos_RowUpdating" OnRowCancelingEdit="GrdMvtos_RowCancelingEdit"
                                CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" OnRowDataBound="GrdMvtos_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="doc">
                                        <ItemTemplate>
                                            <asp:Label ID="LblIdDoc" Text='<%# Eval("IdDoc") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="movimiento">
                                        <ItemTemplate>
                                            <asp:Label ID="LblMov" Text='<%# Eval("DescMov") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="fecha">
                                        <ItemTemplate>
                                            <asp:Label ID="LblFecha" Text='<%# Eval("FechaMov") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PN">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="S/N - lote">
                                        <ItemTemplate>
                                            <asp:Label ID="LblSnlot" Text='<%# Eval("Sn") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="entr.">
                                        <ItemTemplate>
                                            <asp:Label ID="LblEnt" Text='<%# Eval("Entrada") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="salida">
                                        <ItemTemplate>
                                            <asp:Label ID="LblSal" Text='<%# Eval("Salida") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="und med">
                                        <ItemTemplate>
                                            <asp:Label ID="LblUndMed" Text='<%# Eval("CodUnidMedida") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="otros datos">
                                        <ItemTemplate>
                                            <asp:Label ID="LblOtrDat" Text='<%# Eval("Documento") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="w.o">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCln" Text='<%# Eval("Reserva") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="posc">
                                        <ItemTemplate>
                                            <asp:Label ID="Lblposc" Text='<%# Eval("Pos") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="motivo" HeaderStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtMotvo" runat="server" Text='<%# Eval("Observ") %>' Enabled="false" Width="100%" TextMode="MultiLine" Font-Size="10px" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TxtMotvoE" runat="server" Text='<%# Eval("Observacion") %>' Width="100%" TextMode="MultiLine" Font-Size="10px" MaxLength="350" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="editar" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="actualizar" />
                                            <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="cancelar" />
                                        </EditItemTemplate>
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
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
