<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmAlertaVencSP.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmAlertaVencSP" %>
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
                                            <asp:Label ID="LblTitAlertSPVenc" runat="server" Text="pedidos" /></h6>
                                        <asp:GridView ID="GrdAlrta" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" CssClass="GridControl DiseñoGrid table-sm" GridLines="Both">
                                            <Columns>
                                                <asp:TemplateField HeaderText="pedido">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Pedido") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="estado">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Estado") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="fecha">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Fecha_Pedido") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Prioridad">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Prioridad") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Criterio">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Criterio") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Proyeccion">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Proyeccion") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Remanente">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Remanente") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Seguimiento">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Seguimiento") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Tipo">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Tipo") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Aprobada">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Aprobada") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="P/N">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Pn") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="cant">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Cant_Pend") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CantStock">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("CantStock") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="descripcion">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" />
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
                            <asp:PostBackTrigger ControlID="BtnExportarModl" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="BtnExportarModl" runat="server" class="btn btn-default" Text="exportar" OnClick="BtnExportarModl_Click" />
                    <asp:Button ID="BtnCerrarAlerta" runat="server" CssClass="btn btn-default" Text="cerrar" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
