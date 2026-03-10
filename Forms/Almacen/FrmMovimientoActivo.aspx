<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmMovimientoActivo.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmMovimientoActivo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarCntndr {
            position: relative;
            left: 50%;
            top: 210px;
            width: 98%;
            margin-left: -49%;
            height: 85%;
            /*padding: 5px;*/
        }

        .Interna {
            position: absolute;
            top: 15%;
            left: 50%;
            transform: translate(-50%, -50%);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplMvmtsAlmcn" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="CentrarCntndr">
                <div class="col-sm-6 Interna">
                    <div class="row">
                        <div class="col-sm-6">
                            <h6 class="TextoSuperior">
                                <asp:Label ID="LblTitEntradas" runat="server" Text="movimientos de entrada" />
                            </h6>
                        </div>
                        <div class="col-sm-6">
                            <h6 class="TextoSuperior">
                                <asp:Label ID="LblTitSalidas" runat="server" Text="movimientos de Salidas" />
                            </h6>
                        </div>
                    </div>
                    <div id="Consumo_Reint" class="row">
                        <div class="col-sm-6">
                            <asp:Button ID="BtnEntReintegro" runat="server" CssClass="btn btn-success" OnClick="BtnEntReintegro_Click" Width="100%" Text="reintegro" ToolTip="devolución al almacen items no usado de una reserva" />
                        </div>
                        <div class="col-sm-6">
                            <asp:Button ID="BtnSldConsumo" runat="server" CssClass="btn btn-success" OnClick="BtnSldConsumo_Click" Width="100%" Text="consumo" ToolTip="entrega de los elementos a partir de una reserva" />
                        </div>
                    </div>
                    <div id="Compras" class="row">
                        <div class="col-sm-6">
                            <br />
                            <asp:Button ID="BtnEntCompra" runat="server" CssClass="btn btn-success" OnClick="BtnEntCompra_Click" Width="100%" Text="Compra" ToolTip="Entrada por Compra" />
                        </div>
                        <div class="col-sm-6">
                            <br />
                            <asp:Button ID="BtnDevCompra" runat="server" CssClass="btn btn-success" OnClick="BtnDevCompra_Click" Width="100%" Text="devolucion compra" ToolTip="devolución de la compra" Visible="true" />
                        </div>
                    </div>
                    <div id="Reparaciones" class="row">
                        <div class="col-sm-6">
                            <br />
                            <asp:Button ID="BtnEntReparacion" runat="server" CssClass="btn btn-success" OnClick="BtnEntReparacion_Click" Width="100%" Text="entrada reparación" ToolTip="Entrada por Reparación" />
                        </div>
                        <div class="col-sm-6">
                            <br />
                            <asp:Button ID="BtnSalReparacion" runat="server" CssClass="btn btn-success" OnClick="BtnSalReparacion_Click" Width="100%" Text="Salida Reparación" ToolTip="Salida por Reparación" />
                        </div>
                    </div>
                     <div id="Intercambio" class="row">
                        <div class="col-sm-6">
                            <br />
                            <asp:Button ID="BtnEntIntercambio" runat="server" CssClass="btn btn-success" OnClick="BtnEntIntercambio_Click" Width="100%" Text="entrada intercambio" ToolTip="entrada por intercambio" />
                        </div>
                        <div class="col-sm-6">
                            <br />
                            <asp:Button ID="BtnSalIntercambio" runat="server" CssClass="btn btn-success" OnClick="BtnSalIntercambio_Click" Width="100%" Text="salida interc" ToolTip="salida por interc" />
                        </div>
                    </div>
                     <div id="Recuperacion_Baja" class="row">
                        <div class="col-sm-6">
                            <br />
                            <asp:Button ID="BtnEntRecuperacion" runat="server" CssClass="btn btn-success" OnClick="BtnEntRecuperacion_Click" Width="100%" Text="entrada recuperacion" ToolTip="entrada por recuperacion de elementos removidos." />
                        </div>
                        <div class="col-sm-6">
                            <br />
                            <asp:Button ID="BtnSalBaja" runat="server" CssClass="btn btn-success" OnClick="BtnSalBaja_Click" Width="100%" Text="salida baja" ToolTip="salida por baja de elementos en mal estado." />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
