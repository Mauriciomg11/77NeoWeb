<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmPplOrdenesDeDespacho.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmPplOrdenesDeDespacho" %>

<%@ MasterType VirtualPath="~/MasterTransac.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarCntndr {
            position: relative;
            left: 50%;
            width: 98%;
            margin-left: -49%;
            height: 85%;
            padding: 5px;
            top: 40px /**/
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
            $('[id *=DdlBogDest]').chosen();/* */
            $('#<%=DdlAlmacenO.ClientID%>').chosen();
            $('#<%=DdlAlmacenD.ClientID%>').chosen();
            $('#<%=DdlUsuR.ClientID%>').chosen();
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
    <asp:MultiView ID="MultVw" runat="server">
        <asp:View ID="Vw0Datos" runat="server">
            <div class="CentrarCntndr">
                <div id="Almacen_Usu" class="row">
                    <div id="AlmacenesO" class="col-sm-4">
                        <asp:Label ID="LblAlmacenO" runat="server" CssClass="LblEtiquet" Text="almacen origen" />
                        <asp:DropDownList ID="DdlAlmacenO" runat="server" CssClass="heightCampo" OnTextChanged="DdlAlmacenO_TextChanged" AutoPostBack="true" Width="100%" />
                    </div>
                    <div id="AlmacenesD" class="col-sm-4">
                        <asp:Label ID="LblAlmacenD" runat="server" CssClass="LblEtiquet" Text="almacen destino" />
                        <asp:DropDownList ID="DdlAlmacenD" runat="server" CssClass="heightCampo" OnTextChanged="DdlAlmacenD_TextChanged" AutoPostBack="true" Width="100%" />
                    </div>
                    <div id="Usu_Recibe" class="col-sm-4">
                        <asp:Label ID="LblUsuR" runat="server" CssClass="LblEtiquet" Text="recibe" />
                        <asp:DropDownList ID="DdlUsuR" runat="server" CssClass="heightCampo" Width="100%" />
                    </div>
                </div>
                <div id="Botones_Obser" class="row">
                    <div id="BotGuardar" class="col-sm-3">
                        <br />
                        <asp:Button ID="BtnGuardar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnGuardar_Click" Text="guardar" />
                    </div>
                    <div id="Observaciones" class="col-sm-5">
                        <asp:Label ID="LblObserv" runat="server" CssClass="LblEtiquet" Text="observaciones" />
                        <asp:TextBox ID="TxtObserv" runat="server" CssClass="form-control-sm" Width="100%" MaxLength="350" TextMode="MultiLine" Text="" />
                    </div>
                    <div id="OD" class="col-sm-1">
                        <asp:Label ID="LblOD" runat="server" CssClass="LblEtiquet" Text="Actual" />
                        <asp:TextBox ID="TxtOD" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" />
                    </div>
                    <div class="col-sm-2">
                        <br />
                        <asp:ImageButton ID="IbtExportar" runat="server" ToolTip="exportar" CssClass=" BtnExpExcel" Height="38px" Width="40px" ImageUrl="~/images/ExcelV1.png" OnClick="IbtExportar_Click" />
                    </div>
                </div>
                <div id="GrdDetalle" class="row">
                    <div id="BotNewDet" class="col-sm-2">
                        <br />
                        <asp:Button ID="BtnNewDet" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnNewDet_Click" Text="nuevo detalle" />
                    </div>
                    <div class="col-sm-5">
                        <br />
                        <asp:GridView ID="GrdDet" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="CodIdUbicO"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnRowDeleting="GrdDet_RowDeleting" OnRowDataBound="GrdDet_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Pos" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPos" Text='<%# Eval("Pos") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PN" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SN" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lote" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:Label ID="LblLote" Text='<%# Eval("Lote") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="cantidad" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCant" Text='<%# Eval("Cantidad") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="und medida" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:Label ID="LblUM" Text='<%# Eval("UnidMed") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="origen" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:Label ID="LblBodO" Text='<%# Eval("NomBodO") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="destino" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:Label ID="LblBodD" Text='<%# Eval("NomBodD") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField FooterStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
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
        <asp:View ID="Vw1Asignar" runat="server">
            <br />
            <br />
            <h6 class="TextoSuperior">
                <asp:Label ID="LblTitAsigFis" runat="server" Text="asignar elementos" />
            </h6>
            <asp:ImageButton ID="IbtCerrarAsing" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarAsing_Click" />
            <table class="TablaBusqueda">
                <tr>
                    <td>
                        <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                    <td>
                        <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el parte a consultar" /></td>
                    <td>
                        <asp:ImageButton ID="IbtBusqueda" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtBusqueda_Click" /></td>
                </tr>
            </table>
            <div>

                <div id="Stock" class="row">
                    <div id="Grd_Stock" class="col-sm-10">
                        <br />
                        <br />
                        <br />
                        <asp:GridView ID="GrdStock" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="CodIdUbicacion,CodElemento, CodUbicaBodega,
                                        CodEstadoBodega, CodTipoElemento, IdentificadorElemR, CodTercero, CodReferencia,Bodega"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnRowDataBound="GrdStock_RowDataBound" OnRowCommand="GrdStock_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Select" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:UpdatePanel ID="UplAbrir" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ImageButton ID="IbtIr" Width="30px" Height="30px" ImageUrl="~/images/IrV2.png" runat="server" CommandName="Asignar" ToolTip="Ir" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="IbtIr" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PN" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SN" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lote" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:Label ID="LblLote" Text='<%# Eval("NumLote") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="identif" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Indetificador") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="cantidad" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCant" Text='<%# Eval("Cantidad") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="cant despacho" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TxtCantDespa" Text='<%# Eval("CantDespchr") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="und medida" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:Label ID="LblUM" Text='<%# Eval("CodUndMedR") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ubicacion" ItemStyle-CssClass="AutoColunmGV">
                                    <ItemTemplate>
                                        <asp:Label ID="LblUbica" Text='<%# Eval("Bodega") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="bodega destino">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="DdlBogDest" runat="server" Width="100%" Height="28px" />
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
</asp:Content>
