<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmPropuestaValorizacion.aspx.cs" Inherits="_77NeoWeb.Forms.MRO.FrmPropuestaValorizacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
            font-weight: bold;
            width: 8%;
            height: 27px;
        }

        .Font_btnSelect {
            font-size: 12px;
            font-stretch: condensed;
            width: 12%;
            height: 27px;
        }

        .ScrollDet1 {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 600px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">
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
            $('#<%=DdlNumPpt.ClientID%>').chosen();
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
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <div class="row">
                    </div>
                    <table>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                    <div class="row">
                        <div class="col-sm-2">
                            <asp:Table ID="Table1" runat="server">
                                <asp:TableRow>
                                    <asp:TableCell Width="10%">
                                        <asp:Label ID="LblNumPpt" runat="server" CssClass="LblEtiquet" Text="Propuesta:" />
                                    </asp:TableCell>
                                    <asp:TableCell Width="80%">
                                        <asp:DropDownList ID="DdlNumPpt" runat="server" CssClass="Campos" Width="100%" OnTextChanged="DdlNumPpt_TextChanged" AutoPostBack="true" />
                                    </asp:TableCell>
                                    <asp:TableCell Width="10%">
                                        <asp:ImageButton ID="IbtConsult" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsult_Click" />
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                        <div class="col-sm-10">
                            <asp:Button ID="BtnValorizar" runat="server" CssClass="btn btn-success Font_btnCrud" OnClick="BtnValorizar_Click" Text="Valorizar" />
                            <asp:Button ID="BtnReValorizar" runat="server" CssClass="btn btn-success Font_btnCrud" OnClick="BtnReValorizar_Click" Text="Revalorizar" />
                            <asp:Button ID="BtnPlantilla" runat="server" CssClass="btn btn-success Font_btnCrud" OnClick="BtnPlantilla_Click" Text="Plantilla" />
                            <asp:Button ID="BtnExportar" runat="server" CssClass="btn btn-primary Font_btnSelect" OnClick="BtnExportar_Click" Text="exportar" />&nbsp&nbsp
                            <asp:Button ID="BtnPNSinValorizar" runat="server" CssClass="btn btn-primary Font_btnSelect" OnClick="BtnPNSinValorizar_Click" Text="PN sin Valorizar" />
                            <asp:Button ID="BtnSolPed" runat="server" CssClass="btn btn-primary Font_btnSelect" OnClick="BtnSolPed_Click" Text="solicitud pedido" />
                            <asp:Button ID="BtnCotizacion" runat="server" CssClass="btn btn-primary Font_btnSelect" OnClick="BtnCotizacion_Click" Text="cotización" />
                            <asp:Button ID="BtnCuadroComprtv" runat="server" CssClass="btn btn-primary Font_btnSelect" OnClick="BtnCuadroComprtv_Click" Text="cuadro comparativo" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <asp:Label ID="LblCliente" runat="server" CssClass="LblEtiquet" Text="cliente" />
                            <asp:TextBox ID="TxtCliente" runat="server" CssClass="Form-control  heightCampo" Enabled="false" Width="100%" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Label ID="LblDescTipoPPT" runat="server" CssClass="LblEtiquet" Text="tipo" />
                            <asp:TextBox ID="TxtDescTipoPPT" runat="server" CssClass="Form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Label ID="LblDesEstado" runat="server" CssClass="LblEtiquet" Text="estado" />
                            <asp:TextBox ID="TxtDesEstado" runat="server" CssClass="Form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Label ID="LblDescPptTipoSol" runat="server" CssClass="LblEtiquet" Text="tipo solicitud" />
                            <asp:TextBox ID="TxtDescPptTipoSol" runat="server" CssClass="Form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                    </div>
                    <table>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="ScrollDet1">
                                <asp:GridView ID="GrdDetValrzc" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false"
                                    CssClass="DiseñoGrid table table-sm" GridLines="Both" Width="150%" EmptyDataText="No existen registros ..!"
                                    OnRowDataBound="GrdDetValrzc_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Pos">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("Posicion") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Aprob">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CkbAprobP" Checked='<%# Eval("Aprobado").ToString()=="1" ? true : false %>' runat="server"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OTVAL">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("OTVAL") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NomServicio">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("NomServicio") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PnPropuesta">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("PnPropuesta") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Descripcion">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Cantidad Propuesta">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("CantidadPropuesta") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Cant RealPV">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("CantRealPV") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="UndMedProVa">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("UndMedProVa") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="UndCompra">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("UndCompraPV") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="PnStock">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("PnStock") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="CantStock">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("CantStock") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bodega">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("Bodega") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="NomBodega">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("NomBodega") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>                                         
                                         <asp:TemplateField HeaderText="ValorCompra">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtVlr" Text='<%# Eval("ValorCompra") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);"  OnTextChanged="TxtVlr_TextChanged" AutoPostBack="true"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MonedaProVa">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtMnda" Text='<%# Eval("MonedaProVa") %>' runat="server" Width="100%"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="FechaUltimaCompra">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("FechaUltimaCompra") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Entrega cotiza dias">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("TiempoEntregaDiasCoti") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DocReferencia">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("DocReferencia") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Tiempo entrega Dias">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("TiempoEntregaDias") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select SP">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CkbGenrSP" Checked='<%# Eval("SelectSolicitud").ToString()=="1" ? true : false %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Cant solicitar">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("CantidadSolicitud") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NumPedido">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("NumPedido") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="TipoCotizacion">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("TipoCotizacion") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MatriculaVal">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("MatriculaVal") %>' runat="server" Width="100%" />
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
    </asp:UpdatePanel>

</asp:Content>
