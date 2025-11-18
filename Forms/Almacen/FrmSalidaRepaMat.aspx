<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmSalidaRepaMat.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmSalidaRepaMat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarCntndr {
            position: relative;
            left: 50%;
            width: 99%;
            margin-left: -49%;
            height: 85%;
            padding: 40px;
            top: 5px
        }

        .Interna {
            position: relative;
            top: 10%;
            left: 50%;
            transform: translate(-50%, -50%);
        }

        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
        }

        .WithTableRdb {
            width: 20%;
        }

        .WithTableNum {
            width: 10%;
        }

        .WithTable {
            width: 10%;
        }

        ScrollGrid {
            vertical-align: top;
            overflow: auto;
            width: 80%;
            height: 80px;
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
            $('#<%=DdlAlmacen.ClientID%>').chosen(); <%----%>
            $('#<%=DdlNumRepa.ClientID%>').chosen();
        }
        function ShowPopup() {
            $('#ModalCondManplc').modal('show');
            $('#ModalCondManplc').on('shown.bs.modal', function () {
                document.getElementById('<%= BtnCloseMdl.ClientID %>').focus();
                document.getElementById('<%= BtnCloseMdl.ClientID %>').select();
            });<%----%>
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
                            <div id="Almacenes" class="col-sm-3">
                                <asp:Label ID="LblAlmacen" runat="server" CssClass="LblEtiquet" Text="Almacén" />
                                <asp:DropDownList ID="DdlAlmacen" runat="server" CssClass="heightCampo" Width="100%" />
                            </div>
                            <div id="Observaciones" class="col-sm-6">
                                <asp:Label ID="LblObserv" runat="server" CssClass="LblEtiquet" Text="observaciones" />
                                <asp:TextBox ID="TxtObserv" runat="server" CssClass="form-control-sm" Width="100%" MaxLength="350" TextMode="MultiLine" Text="" />
                            </div>
                        </div>
                        <div id="TipoRepa" class="row">
                            <div id="Tipo" class="col-sm-3">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RdbNacional" runat="server" CssClass="LblEtiquet" GroupName="TipComp" Checked="true" Text="&nbspNacional &nbsp" OnCheckedChanged="RdbNacional_CheckedChanged" AutoPostBack="true" />&nbsp&nbsp&nbsp
                                            <asp:RadioButton ID="RdbInter" runat="server" CssClass="LblEtiquet" GroupName="TipComp" Text="&nbspInternacional &nbsp" OnCheckedChanged="RdbInter_CheckedChanged" AutoPostBack="true" />
                                        &nbsp&nbsp&nbsp
                                        <td>
                                    </tr>
                                </table>
                            </div>
                            <div id="Num_Repa" class="col-sm-2">
                                <asp:Label ID="LblNumRepa" runat="server" CssClass="LblEtiquet" Text="Documento" />
                                <asp:DropDownList ID="DdlNumRepa" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlNumRepa_TextChanged" AutoPostBack="true" />
                            </div>
                            <div id="Moneda" class="col-sm-1">
                                <asp:Label ID="LblMoneda" runat="server" CssClass="LblEtiquet" Text="Moneda" />
                                <asp:TextBox ID="TxtMoneda" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div id="Espacio" class="col-sm-4">
                            </div>
                            <div id="BotVisualizar" class="col-sm-2">
                                <%--<asp:Button ID="BtnVisualizar" runat="server" CssClass="btn btn-primary" Width="100%" OnClick="BtnVisualizar_Click" Text="Visualizar" />--%>
                                 <asp:Button ID="BtnGuardar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnGuardar_Click" OnClientClick="target ='';" Text="guardar" />
                            </div>
                        </div>
                        <div class="row">
                            <div id="GridDetRepa" class="col-sm-12">
                                <br />
                                <div class="ScrollGrid pre-scrollable">
                                    <asp:GridView ID="GrdDtlleRepa" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="CodReparacion,Posicion,FechaVencPN,Bloquear,DiaTasa, MesTasa, AñoTasa,PosSO, CodProveedor, ValorUnidad,Valor_Compra, CCostos, ValorUnidadP,PPT, CodPedido, Equivalencia"
                                        CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" OnRowCommand="GrdDtlleRepa_RowCommand" OnRowDataBound="GrdDtlleRepa_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="UplAbrir" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ImageButton ID="IbtAbrir" Width="30px" Height="30px" ImageUrl="~/images/ReportV1.png" runat="server" CommandName="Abrir" ToolTip="asignar reparacion" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="IbtAbrir" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="documento">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblNumDoc" Text='<%# Eval("Codigo") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="pos">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPos" Text='<%# Eval("Posicion") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="reparación">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblNumRepa" Text='<%# Eval("CodReparacion") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="referencia">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblRef" Text='<%# Eval("CodReferencia") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="descripcion">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="tipo">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblTipo" Text='<%# Eval("CodTipoElemento") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="identificador">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblIdentfc" Text='<%# Eval("IdentificadorElemR") %>' runat="server" />
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
                                            <asp:TemplateField HeaderText="cant repa">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantRepa" Text='<%# Eval("Cant_Repa") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="und repa">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblUndMedRepa" Text='<%# Eval("UND_Repa") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant ingresar">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantIngrsr" Text='<%# Eval("CantIngresar") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="und despch">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblUndMedDesp" Text='<%# Eval("CodUndMed") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="factura">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFact" Text='<%# Eval("NumFactura") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="fecha trm">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lblfectrm" Text='<%# Eval("FechaTRM") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="trm">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbltrm" Text='<%# Eval("TrmAcordado") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    </asp:GridView>
                                    <%-- --%>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw1Asignar" runat="server">
                    <br />
                    <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitAsigFis" runat="server" Text="asignar elemento" />
                    </h6>
                    <div class="CentrarCntndr">
                        <br />
                        <div class="row">
                            <div class="col-sm-8 Interna">
                                <div class="ScrollRsva pre-scrollable">                                                              
                                    <asp:Label ID="LblPNDescripcAsig" runat="server" CssClass="LblEtiquet" Text="" />
                                    <asp:Label ID="LblAsigCantSol" runat="server" CssClass="LblEtiquet" Text="cantidad solic: " />
                                    <asp:Label ID="LblAsigCantSolV" runat="server" CssClass="LblEtiquet" Text="" />
                                    <asp:GridView ID="GrdAsignar" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                        DataKeyNames="CodIdUbicacion,CodElemento,CodUbicaBodega, CodTercero, FechaVencimientoR, FechaShelfLife, CantRepa, CodReferencia, IdentificadorElemR, Activo, CodEstadoPn,CodBogIntaNals"
                                        CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" OnRowCommand="GrdAsignar_RowCommand" OnRowDataBound="GrdAsignar_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="IbtAsignr" Width="30px" Height="30px" ImageUrl="~/images/FlechaIr.png" runat="server" CommandName="Asignar" ToolTip="Asignar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                                                    <asp:TextBox ID="TxtCantRepa" Text='<%# Eval("CantRepa") %>' runat="server" Width="100%" Enabled="false" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
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
                            <div class="col-sm-4">
                                <asp:ImageButton ID="IbtCerrarAsing" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarAsing_Click" />
                            </div>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
