<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmAlertaReservaPenRevisar.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmAlertaReservaPenRevisar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarContenedor {
            position: absolute;
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

        .GridDivScroll {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 95%;
        }

        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .TamanAlert {
            height: 400px;
            width: 95%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
   <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <div id="ModalAlerta" class="modal fade " tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="LblTitAlrt" runat="server" Text="Alertas" /></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpPlAlert" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-12 DivMarco">
                                    <div class="CentrarGrid pre-scrollable">
                                        <h6 class="TextoSuperior">
                                            <asp:Label ID="LblTitAlerta" runat="server" Text="reservas nuevas" /></h6>
                                        <asp:GridView ID="GrdAlrta" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                            CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" OnRowDataBound="GrdAlrta_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="ot">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Orden") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Pos">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Pos") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ref">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Referencia") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="P/N">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("PN") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="cant sol">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Cant") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cant. entregada">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("CantidadEntregada") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="camt.stock">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Cant_Stock") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="cant. insp">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Cant_Insp") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Pedido">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Pedido") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Usuario">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Usuario") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="fecha">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("FechaModText") %>' runat="server" />
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
                        </ContentTemplate>
                        <Triggers>
                            <%--<asp:AsyncPostBackTrigger ControlID ="BtnExportarModl" EventName ="Click" />--%>
                             <asp:PostBackTrigger ControlID="BtnExportarModl" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                     <asp:Button ID="BtnExportarModl" runat="server" class="btn btn-default" Text="exportar" OnClick="BtnExportarModl_Click"/>
                    <asp:Button ID="BtnCerrarAlerta" runat="server" CssClass="btn btn-default" Text="cerrar" data-dismiss="modal" />
                </div>
            </div>

        </div>
    </div>
</asp:Content>
