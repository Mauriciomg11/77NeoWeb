<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmCuadroComparativoCotiza.aspx.cs" Inherits="_77NeoWeb.Forms.InventariosCompras.FrmCuadroComparativoCotiza" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .CentrarContNumCotiza {
            left: 50%;
            /*determinamos una anchura*/
            width: 98%;
            margin-left: 1%;
            height: 8%;
        }

        .CentrarContenedor {
            position: absolute;
            left: 50%;
            width: 98%;
            margin-left: -49%;
            height: 90%;
            padding: 5px;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
        }

        .ScrollDet1 {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 480px;
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
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br /><br />
            <div class="CentrarContNumCotiza DivMarco">
                <div id="Botones" class="row">
                    <div class="col-sm-8">
                        <table class="">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="RdbBusqSP" runat="server" CssClass="LblEtiquet" Text="&nbsp pedido" GroupName="Busq" />&nbsp&nbsp&nbsp
                                <asp:RadioButton ID="RdbBusqCot" runat="server" CssClass="LblEtiquet" Text="&nbsp cotización" GroupName="Busq" />&nbsp&nbsp&nbsp                               
                                <asp:RadioButton ID="RdbBusqPet" runat="server" CssClass="LblEtiquet" Text="&nbsp peticion" GroupName="Busq" />&nbsp&nbsp&nbsp
                                <asp:RadioButton ID="RdbBusqPPT" runat="server" CssClass="LblEtiquet" Text="&nbsp propusta" GroupName="Busq" />&nbsp&nbsp&nbsp
                                <asp:RadioButton ID="RdbBusqPN" runat="server" CssClass="LblEtiquet" Text="&nbsp P/N" GroupName="Busq" /></td>
                                <td>
                                    <asp:TextBox ID="TxtBusqPN" runat="server" Width="200px" Height="28px" CssClass=" heightCampo" placeholder="P/N" />
                                    <asp:TextBox ID="TxtBusqueda" runat="server" Width="170px" Height="28px" CssClass=" heightCampo" placeholder="Ingrese el dato a consultar" />
                                    <%-- TextMode="Number" onkeypress="return solonumeros(event);"--%>
                                </td>
                                <td>
                                    <asp:ImageButton ID="IbtBusqueda" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtBusqueda_Click" /></td>
                            </tr>
                        </table>
                    </div>
                    <div class="col-sm-2">
                        <asp:Button ID="BtnOpenCompra" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnOpenCompra_Click" OnClientClick="target ='';" Text="orden compra" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Button ID="BtnOpenRepa" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnOpenRepa_Click" OnClientClick="target ='';" Text="reparacion" />
                    </div>
                </div>
            </div>
            <div class="ScrollDet2">
                <table>
                    <tr>
                        <td>
                            <asp:ImageButton ID="IbtAprPNAll" runat="server" ImageUrl="~/images/Check1.png" ImageAlign="AbsBottom" Height="30px" Width="30px" OnClick="IbtAprPNAll_Click" />
                            <asp:ImageButton ID="IbtDesAprAll" runat="server" ImageUrl="~/images/UnCheck.png" ImageAlign="AbsBottom" Height="30px" Width="30px" OnClick="IbtDesAprAll_Click" />
                        </td>

                        <td>
                            <asp:Button ID="BtnAprob" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnAprob_Click" OnClientClick="target ='';" Text="aprobar" /></td>
                        <td>
                            <asp:Button ID="BtnExport" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnExport_Click" OnClientClick="target ='';" Text="exportar" /></td>
                    </tr>
                </table>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="ScrollDet1">
                            <asp:GridView ID="GrdDet" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true"
                                DataKeyNames="IdDetCotizacion, CodPrioridad"
                                CssClass="DiseñoGrid table table-sm" GridLines="Both" Width="130%" AllowSorting="true"
                                OnRowDataBound="GrdDet_RowDataBound" OnSorting="GrdDet_Sorting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sel." HeaderStyle-Width="1%" SortExpression="Aprobacion">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CkbAprob" Checked='<%# Eval("Aprobacion").ToString()=="1" ? true : false %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="apro prop." HeaderStyle-Width="1%" SortExpression="AprobDetPr">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CkbAprobPPT" Checked='<%# Eval("AprobDetPr").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pos" HeaderStyle-Width="1%" SortExpression="PosDetPr">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPos" Text='<%# Eval("PosDetPr") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="fecha" HeaderStyle-Width="1%" SortExpression="FechaSolicitudPet">
                                        <ItemTemplate>
                                            <asp:Label ID="LblFecSP" Text='<%# Eval("FechaSolicitudPet") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="cotizacion" HeaderStyle-Width="4%" SortExpression="CodCotizacion">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCotiza" Text='<%# Eval("CodCotizacion") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Proveedor" HeaderStyle-Width="12%" SortExpression="Proveedor">
                                        <ItemTemplate>
                                            <asp:Label ID="LblProvee" Text='<%# Eval("Proveedor") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="10%" SortExpression="Pn">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPNSolct" Text='<%# Eval("Pn") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripcion" HeaderStyle-Width="10%" SortExpression="Descripcion">
                                        <ItemTemplate>
                                            <asp:Label ID="LblDescPN" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="parte Alterno" HeaderStyle-Width="10%" SortExpression="Alterno">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPnAlter" Text='<%# Eval("Alterno") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="und medida" HeaderStyle-Width="3%" SortExpression="CodUndMed">
                                        <ItemTemplate>
                                            <asp:Label ID="LblUndMed" Text='<%# Eval("CodUndMed") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cant" SortExpression="Cantidad">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCant" Text='<%# Eval("Cantidad") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cant MinC" SortExpression="UndMinimaCompra">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCantMinC" Text='<%# Eval("UndMinimaCompra") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="valor und" SortExpression="ValorUnidad">
                                        <ItemTemplate>
                                            <asp:Label ID="LblVlrUnd" Text='<%# Eval("ValorUnidad") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="moneda" SortExpression="CodMoneda">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCodMoned" Text='<%# Eval("CodMoneda") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="descuento" SortExpression="Descuento">
                                        <ItemTemplate>
                                            <asp:Label ID="LblDescto" Text='<%# Eval("Descuento") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="estado Elemento" SortExpression="Estado">
                                        <ItemTemplate>
                                            <asp:Label ID="LblEstdElem" Text='<%# Eval("Estado") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="tiempo entrega" SortExpression="TiempoEntrega">
                                        <ItemTemplate>
                                            <asp:Label ID="LblTimeEntrg" Text='<%# Eval("TiempoEntrega") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="tipo pago" HeaderStyle-Width="15%" SortExpression="TipoPago">
                                        <ItemTemplate>
                                            <asp:Label ID="LbltipoPag" Text='<%# Eval("TipoPago") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="lugar Entrega" HeaderStyle-Width="20%" SortExpression="LugarEntrega">
                                        <ItemTemplate>
                                            <asp:Label ID="LblLgrEntrg" Text='<%# Eval("LugarEntrega") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="observaciones" HeaderStyle-Width="10%" SortExpression="Observaciones">
                                        <ItemTemplate>
                                            <asp:Label ID="LblObservac" Text='<%# Eval("Observaciones") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="tiempo entrega propuesta" SortExpression="TiempEntregaPropuesta">
                                        <ItemTemplate>
                                            <asp:Label ID="LblTimEntrgPPT" Text='<%# Eval("TiempEntregaPropuesta") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo cotización" SortExpression="TipoCotizacion">
                                        <ItemTemplate>
                                            <asp:Label ID="LblTipoCot" Text='<%# Eval("TipoCotizacion") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="propuesta" SortExpression="IdPropuesta">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPPT" Text='<%# Eval("IdPropuesta") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="garantia" SortExpression="GarantiaDC">
                                        <ItemTemplate>
                                            <asp:Label ID="LblGarantia" Text='<%# Eval("GarantiaDC") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle CssClass="GridFooterStyle" />
                                <HeaderStyle CssClass="GridCabecera1" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnAprob" />
            <asp:PostBackTrigger ControlID="BtnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
