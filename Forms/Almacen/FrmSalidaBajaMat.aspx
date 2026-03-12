<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmSalidaBajaMat.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmSalidaBajaMat" %>

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
                        <div id="Aeronave-Observacion" class="row">
                            <div id="Almacenes" class="col-sm-3">
                                <asp:Label ID="LblAlmacen" runat="server" CssClass="LblEtiquet" Text="almacen" />
                                <asp:DropDownList ID="DdlAlmacen" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlAlmacen_TextChanged" AutoPostBack="true" />
                            </div>
                            <div id="Observaciones" class="col-sm-6">
                                <asp:Label ID="LblObserv" runat="server" CssClass="LblEtiquet" Text="observaciones" />
                                <asp:TextBox ID="TxtObserv" runat="server" CssClass="form-control-sm" Width="100%" MaxLength="350" TextMode="MultiLine" Text="" />
                            </div>
                             <div id="Disponible" class="col-sm-1">
                            </div>                           
                            <div id="BotVisualizar" class="col-sm-2">
                                 <br />
                                <asp:Button ID="BtnVisualizar" runat="server" CssClass="btn btn-primary" Width="100%" OnClick="BtnVisualizar_Click" Text="visualizar" />
                            </div>
                        </div>
                        <div class="row">
                            <div id="GridDetRecup" class="col-sm-12">
                                <br />
                                <div class="ScrollGrid pre-scrollable">
                                    <asp:GridView ID="GrdDtlleBaja" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                        DataKeyNames="CodElemento, CodReferencia, CCosto, CodUbicaBodega, CodTercero,CodTipoElemento, IdentificadorElemR,CodUndMedR,
                                                        Bloquear,CodIdUbicacion, CodMoneda, FechV"
                                        CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" OnRowCommand="GrdDtlleBaja_RowCommand" OnRowDataBound="GrdDtlleBaja_RowDataBound">
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
                                                    <asp:Label ID="LblLote" Text='<%# Eval("NumLote") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="descripcion">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cantidad">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCant" Text='<%# Eval("Cantidad") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="bodega">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblBod" Text='<%# Eval("CodBodega") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="f">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblF" Text='<%# Eval("Fila") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="c">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblC" Text='<%# Eval("Columna") %>' runat="server" />
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
                        <asp:Label ID="LblTitAsigFis" runat="server" Text="baja elemento" />
                    </h6>
                    <div class="CentrarCntndr">
                        <br />
                        <div class="row">
                            <div class="col-sm-8 Interna">
                                <div class="ScrollRsva pre-scrollable">
                                    <br />
                                    <asp:Button ID="BtnGuardar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="120px" OnClick="BtnGuardar_Click" OnClientClick="target ='';" Text="guardar" />
                                    <br />
                                    <br />
                                    <asp:GridView ID="GrdAsignar" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                        DataKeyNames=""
                                        CssClass="GridControl DiseñoGrid table-sm" GridLines="Both">
                                        <Columns>
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
                                            <asp:TemplateField HeaderText="descripc">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblDesc" Text='<%# Eval("DescrElem") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant baja">
                                                <ItemTemplate>
                                                    <asp:Label ID="TxtCantRec" Text='<%# Eval("CantIngr") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="und medida">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblUndMed" Text='<%# Eval("CodUM") %>' runat="server" />
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
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
