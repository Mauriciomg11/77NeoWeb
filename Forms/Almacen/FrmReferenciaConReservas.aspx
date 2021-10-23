<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmReferenciaConReservas.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmReferenciaConReservas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .CentrarBusq {
            position: absolute;
            left: 50%;
            width: 98%;
            margin-left: -49%;
            height: 85%;
            padding: 5px;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
        }

        .CentrarContenedor {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="CentrarBusq DivMarco">
                <table class="TablaBusqueda">
                    <tr>
                        <td colspan="3">
                            <asp:RadioButton ID="RdbSolPed" runat="server" CssClass="LblEtiquet" Text="&nbsp solicitud:" GroupName="Busq" />&nbsp&nbsp&nbsp
                            <asp:RadioButton ID="RdbPpt" runat="server" CssClass="LblEtiquet" Text="&nbsp propuesta:" GroupName="Busq" />
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                        <td>
                            <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                        <td>
                            <asp:ImageButton ID="IbtBusqueda" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtBusqueda_Click" /></td>
                    </tr>                   
                </table>
                <br />
                <div class="CentrarGrid pre-scrollable">
                     <asp:Button ID="BtnAprobar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnAprobar_Click" OnClientClick="target ='';" Text="aprobar" />
                     <asp:ImageButton ID="IbtAprDetAll" runat="server" ImageUrl="~/images/Check1.png" ImageAlign="AbsBottom" Height="30px" Width="30px" OnClick="IbtAprDetAll_Click" />                   
                    <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="IdDetPedido, CodPrioridad, CodEstadoPn,Bloquear,CodtipoSolPedido"
                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnRowDataBound="GrdBusq_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CkbAprobP" Checked='<%# Eval("OK").ToString()=="1" ? true : false %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="pedido">
                                <ItemTemplate>
                                    <asp:Label ID="LblCodPed" Text='<%# Eval("CodPedido") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="posicion">
                                <ItemTemplate>
                                    <asp:Label ID="LblPosc" Text='<%# Eval("Posicion") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="fecha">
                                <ItemTemplate>
                                    <asp:Label ID="LblFecha" Text='<%# Eval("Fecha_Pedido") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="prioridad">
                                <ItemTemplate>
                                    <asp:Label ID="LblPriord" Text='<%# Eval("Prioridad") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="estado">
                                <ItemTemplate>
                                    <asp:Label ID="LblEstd" Text='<%# Eval("CodEstado") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="peticion">
                                <ItemTemplate>
                                    <asp:Label ID="LblPetc" Text='<%# Eval("Num_Peticion") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="cant">
                                <ItemTemplate>
                                    <asp:Label ID="LblCant" Text='<%# Eval("CantidadTotal") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P/N">
                                <ItemTemplate>
                                    <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="referencia">
                                <ItemTemplate>
                                    <asp:Label ID="LblRef" Text='<%# Eval("CodReferencia") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="estado">
                                <ItemTemplate>
                                    <asp:Label ID="LblEstdPn" Text='<%# Eval("EstadoPN") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="descripcion">
                                <ItemTemplate>
                                    <asp:Label ID="LblDescr" Text='<%# Eval("Descripcion") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo">
                                <ItemTemplate>
                                    <asp:Label ID="LblTipo" Text='<%# Eval("Tipo") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="propuesta">
                                <ItemTemplate>
                                    <asp:Label ID="LblPPT" Text='<%# Eval("PPT") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="DdlBodega" EventName="TextChanged" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
