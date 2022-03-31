<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmMovimientoActivo.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmMovimientoActivo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarCntndr {
            position: absolute;
            left: 50%;
            width: 98%;
            margin-left: -49%;
            height: 85%;
            padding: 5px;
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
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
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
                    <div class="row">
                        <div class="col-sm-6">
                              <asp:Button ID="BtnEntReintegro" runat="server" CssClass="btn btn-success" OnClick="BtnEntReintegro_Click" Width="100%" Text="reintegro" ToolTip="devolución al almacen items no usado de una reserva" />
                        </div>
                        <div class="col-sm-6">
                            <asp:Button ID="BtnSldConsumo" runat="server" CssClass="btn btn-success" OnClick="BtnSldConsumo_Click" Width="100%" Text="consumo" ToolTip="entrega de los elementos a partir de una reserva" />
                        </div>
                    </div>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
