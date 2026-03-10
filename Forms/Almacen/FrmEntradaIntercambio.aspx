<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmEntradaIntercambio.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmEntradaIntercambio" %>

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
            $('#<%=DdlNumInterc.ClientID%>').chosen();
            $('[id *=DdlPN_Alter]').chosen();
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
                            <div id="Almacenes" class="col-sm-3">
                                <asp:Label ID="LblAlmacen" runat="server" CssClass="LblEtiquet" Text="almacen" />
                                <asp:DropDownList ID="DdlAlmacen" runat="server" CssClass="heightCampo" Width="100%" />
                            </div>
                            <div id="Observaciones" class="col-sm-6">
                                <asp:Label ID="LblObserv" runat="server" CssClass="LblEtiquet" Text="observaciones" />
                                <asp:TextBox ID="TxtObserv" runat="server" CssClass="form-control-sm" Width="100%" MaxLength="350" TextMode="MultiLine" Text="" />
                            </div>
                        </div>
                        <div id="TipoCompra" class="row">
                            <div id="Tipo" class="col-sm-2">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RdbNacional" runat="server" CssClass="LblEtiquet" GroupName="TipComp" Checked="true" Text="nacional &nbsp" OnCheckedChanged="RdbNacional_CheckedChanged" AutoPostBack="true" />&nbsp&nbsp&nbsp
                                            <asp:RadioButton ID="RdbInter" runat="server" CssClass="LblEtiquet" GroupName="TipComp" Text="internacional &nbsp" OnCheckedChanged="RdbInter_CheckedChanged" AutoPostBack="true" />
                                        &nbsp&nbsp&nbsp
                                        <td>
                                    </tr>
                                </table>
                            </div>
                            <div id="Num_Inter" class="col-sm-2">
                                <asp:Label ID="LblNumInterc" runat="server" CssClass="LblEtiquet" Text="documento" />
                                <asp:DropDownList ID="DdlNumInterc" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlNumInterc_TextChanged" AutoPostBack="true" />
                            </div>
                            <div id="Moneda" class="col-sm-1">
                                <asp:Label ID="LblMoneda" runat="server" CssClass="LblEtiquet" Text="moneda" />
                                <asp:TextBox ID="TxtMoneda" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div id="Disponible" class="col-sm-5">
                            </div>
                            <div id="BotVisualizar" class="col-sm-2">
                                <asp:Button ID="BtnGuardar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnGuardar_Click" OnClientClick="target ='';" Text="guardar" />
                            </div>
                        </div>
                        <div class="row">
                            <div id="GridDetInterc" class="col-sm-12">
                                <br />
                                <div>
                                    <asp:GridView ID="GrdDtlleInterc" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" 
                                        DataKeyNames="IdDetOrdenCompra,Posicion,FechaVencPN,Bloquear,DiaTasa, MesTasa, AñoTasa,PosSO, CodProveedor, ValorUnidad,Valor_Compra, CCostos, ValorUnidadP,PPT, 
                                        CodPedido, Equivalencia, CodShippingOrder"
                                        CssClass="GridControl DiseñoGrid AutoColunmGV table-sm" GridLines="Both" OnRowCommand="GrdDtlleInterc_RowCommand" OnRowDataBound="GrdDtlleInterc_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="UplAbrir" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ImageButton ID="IbtAbrir" Width="30px" Height="30px" ImageUrl="~/images/ReportV1.png" runat="server" CommandName="Abrir" ToolTip="asignar interc" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="IbtAbrir" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="interc">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblNumIC" Text='<%# Eval("CodOrdenCompra") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pos">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPos" Text='<%# Eval("Posicion") %>' runat="server" />
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
                                            <asp:TemplateField HeaderText="cant interc">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantInterc" Text='<%# Eval("Cant_Compra") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="und interc">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblUndMedInerc" Text='<%# Eval("UND_Compra") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant recibida">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantRecb" Text='<%# Eval("CantRecibida") %>' runat="server" />
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
                                                    <asp:Label ID="LblFact" Text='<%# Eval("NumFacturaOC") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="fecha trm">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lblfectrm" Text='<%# Eval("FechaTRM") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TRM">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbltrm" Text='<%# Eval("TrmAcordado") %>' runat="server" />
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
                <asp:View ID="Vw1Asignar" runat="server">
                    <br />
                    <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitAsigFis" runat="server" Text="asignar elementos" />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarAsing" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarAsing_Click" />
                    <br />
                    <br />
                    <div class="CentrarCntndr">
                        <br />
                        <asp:Label ID="LblPNDescripcAsig" runat="server" CssClass="LblEtiquet" Text="" />
                        <asp:Label ID="LblAsigCantSol" runat="server" CssClass="LblEtiquet" Text="cantidad solic: " />
                        <asp:Label ID="LblAsigCantCompV" runat="server" CssClass="LblEtiquet" Text="" />                       
                        <asp:Label ID="LblFactAsign" runat="server" CssClass="LblEtiquet" Text=" | factura:" />
                        <asp:TextBox ID="TxtFact" runat="server" CssClass="form-control-sm heightCampo" Width="120px" />
                        <br />
                        <br />
                        <div class="row">
                            <div class="col-sm-10">
                                <asp:GridView ID="GrdAsignar" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true"
                                    CssClass="DiseñoGrid table-sm" GridLines="Both" DataKeyNames="CodReferencia, PN, IdentificadorElemR" EmptyDataText="No existen registros ..!"
                                    OnRowCommand="GrdAsignar_RowCommand" OnRowDataBound="GrdAsignar_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="IbtAsignr" Width="30px" Height="30px" ImageUrl="~/images/FlechaIr.png" runat="server" CommandName="Asignar" ToolTip="Asignar" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="30%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="DdlPN_Alter" runat="server" Width="100%" Height="28px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtSN" Text='<%# Eval("SN") %>' runat="server" Width="100%" MaxLength="80" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="cantidad" ItemStyle-CssClass="AutoColunmGV">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("CantIngr") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="cant despacho" ItemStyle-CssClass="AutoColunmGV">
                                            <ItemTemplate>
                                                <asp:Label ID="LblCantIntC" Text='<%# Eval("CantInterc") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="und medida" ItemStyle-CssClass="AutoColunmGV">
                                            <ItemTemplate>
                                                <asp:Label ID="LblUndMed" Text='<%# Eval("CodUndMedR") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="fecha vencimiento" ItemStyle-CssClass="AutoColunmGV">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtFecVen" runat="server" Width="100%" onkeypress="return Fecha(event);" TextMode="Date" />
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
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
