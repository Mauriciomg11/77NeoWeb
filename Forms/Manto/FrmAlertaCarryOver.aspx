<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmAlertaCarryOver.aspx.cs" Inherits="_77NeoWeb.Forms.Manto.FrmAlertaCarryOver" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarContenedor {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
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
            <div class="CentrarContenedor DivMarco">
                <table class="TablaBusqueda">
                    <tr>
                        <td>
                            <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                        <td>
                            <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                        <td>
                            <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                    </tr>
                </table>
                <br />
                <div class="GridDivScroll">
                    <asp:GridView ID="GrdDatos" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnRowDataBound="GrdDatos_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Matricula">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Matricula") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CodIdLvDetManto">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("CodIdLvDetManto") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha_Reporte">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Fecha_Reporte") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha_Proyectada">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Fecha_Proyectada") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CodLibroVuelo">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("CodLibroVuelo") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NumCasilla">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("NumCasilla") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reportado">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Reportado") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CodClaseReporteManto">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("CodClaseReporteManto") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CodcategoriaMel">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("CodcategoriaMel") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DocumentoRef">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("DocumentoRef") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reporte"  HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Reporte") %>' runat="server"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AccionParcial" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("AccionParcial") %>' runat="server" />
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
    </asp:UpdatePanel>
</asp:Content>
