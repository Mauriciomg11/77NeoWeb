<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmReportesLogistica.aspx.cs" Inherits="_77NeoWeb.Forms.InventariosCompras.FrmReportesLogistica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /**/
        .CentrarContndr {
            position: relative;
            left: 50%;
            width: 98%;
            margin-left: -49%;
            height: 85%;
            padding: 5px;
        }

        .Interna {
            position: relative;
            top: 15%;
            left: 50%;
            transform: translate(-50%, -50%);
        }

        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .heightBtns {
            height: 35px;
            width: 95%;
            font-size: 12px;
        }

        .CentarGrid {
            text-align: left;
            width: 100%;
            margin: auto;
            border: 1px solid black;
        }

        .wrp {
            width: 100%;
            text-align: center;
        }

        .frm {
            text-align: left;
            width: 80%;
            margin: auto;
            border: 1px solid black;
        }

        .fldLbl {
            white-space: nowrap;
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

        function myFuncionddl() {
            $('#<%=DdlAlmacenInv.ClientID%>').chosen();
            $('#<%=DdlGrupoInv.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplRteIngPpl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MlVw" runat="server">
                <asp:View ID="Vw0Principal" runat="server">
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div class="CentrarContndr DivMarco">
                        <div class="col-sm-6 Interna">
                            <div class="row">
                                <div class="col-sm-6">
                                    <br />
                                    <asp:Button ID="BtnInventario" runat="server" CssClass="btn btn-primary heightBtns" OnClick="BtnInventario_Click" OnClientClick="target ='_blank';" Text="inventario" ToolTip="Inventario por grupo." />
                                </div>
                                <div class="col-sm-6">
                                    <br />
                                    <asp:Button ID="BtnReparaciones" runat="server" CssClass="btn btn-primary heightBtns" OnClick="BtnReparaciones_Click" OnClientClick="target ='_blank';" Text="Reparaciones" ToolTip="Informe de reparaciones en un rango de fecha." />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                    <br />
                                    <asp:Button ID="BtnCompraPend" runat="server" CssClass="btn btn-primary heightBtns" OnClick="BtnCompraPend_Click" OnClientClick="target ='_blank';" Text="compras" ToolTip="estado de las compras." />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw1Reparaciones" runat="server">
                    <br />
                    <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitReparaciones" runat="server" Text="reparaciones" />
                    </h6>
                    <div class="CentrarContenedor DivMarco">
                        <asp:ImageButton ID="IbtCerrarImpr" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarImpr_Click" />
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechI" runat="server" CssClass="LblEtiquet" Text="Fecha Inicial" />
                                <asp:TextBox ID="TxtFechI" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechF" runat="server" CssClass="LblEtiquet" Text="Fecha Final" />
                                <asp:TextBox ID="TxtFechF" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-2">
                                <asp:ImageButton ID="IbtExpRepaPend" runat="server" ToolTip="exportar reparaciones" CssClass=" BtnExpExcel" Height="38px" Width="40px" ImageUrl="~/images/ExcelV1.png" OnClick="IbtExpRepaPend_Click" />
                                <asp:ImageButton ID="IbtExcelHisAlmaRepa" runat="server" ToolTip="Exportar reparaciones" CssClass=" BtnExpExcel" Height="38px" Width="40px" ImageUrl="~/images/ExcelV1.png" OnClick="IbtExcelHisAlmaRepa_Click" />
                            </div>
                            <div class="col-sm-0">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <table class="">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RdbRpAll" runat="server" CssClass="LblEtiquet" Text="&nbsp todos" GroupName="Repa" />&nbsp&nbsp&nbsp
                                            <asp:RadioButton ID="RdbRpCot" runat="server" CssClass="LblEtiquet" Text="&nbsp cotizacion" GroupName="Repa" />&nbsp&nbsp&nbsp
                                            <asp:RadioButton ID="RdbRpCodRepa" runat="server" CssClass="LblEtiquet" Text="&nbsp reparacion" GroupName="Repa" />&nbsp&nbsp&nbsp                               
                                            <asp:RadioButton ID="RdbRpPN" runat="server" CssClass="LblEtiquet" Text="&nbsp P/N" GroupName="Repa" />&nbsp&nbsp&nbsp
                                            <asp:RadioButton ID="RdbRpSN" runat="server" CssClass="LblEtiquet" Text="&nbsp S/N" GroupName="Repa" />&nbsp&nbsp&nbsp
                                            <asp:RadioButton ID="RdbRpProv" runat="server" CssClass="LblEtiquet" Text="&nbsp proveedor" GroupName="Repa" />&nbsp&nbsp&nbsp
                                            <asp:CheckBox ID="CkbRpPend" runat="server" CssClass="LblEtiquet" Text="&nbsp pendientes" Checked="true" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtRpDocBusq" runat="server" Width="250px" Height="28px" CssClass=" heightCampo" placeholder="Ingrese el dato a consultar" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="IbtRpBusqueda" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtRpBusqueda_Click" /></td>
                                    </tr>
                                </table>
                                <div class="ScrollDet1">
                                    <asp:GridView ID="GrdDetRepa" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true"
                                        DataKeyNames=""
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" Width="130%" AllowSorting="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="pedido" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodPed" Text='<%# Eval("CodPedido") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="f. pedido" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblRpFcPed" Text='<%# Eval("Fechapedido") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cotizacion" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodPCot" Text='<%# Eval("CodCotizacion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="reparacion" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodRep" Text='<%# Eval("CodReparacion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="f. reparacion" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFchRepa" Text='<%# Eval("FechaReparacion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="tipo" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LbltipoRep" Text='<%# Eval("CodTipoOrdenRepa") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="sn pendiente" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblSNpend" Text='<%# Eval("SN_Ped") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SN" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblSNRepa" Text='<%# Eval("SN_Repa") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant repa" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantRepa" Text='<%# Eval("CantRepa") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant salida" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantSal" Text='<%# Eval("Cant_Salida") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant entrada" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantEnt" Text='<%# Eval("Cant_Entrada") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="estado" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblEstd" Text='<%# Eval("Estado") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="exportacion" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblExpor" Text='<%# Eval("Exportacion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="importac" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblImpor" Text='<%# Eval("Importacion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Doc proveedor" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodProv" Text='<%# Eval("CodProveedor") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="proveedor" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblProv" Text='<%# Eval("RazonSocial") %>' runat="server" Width="100%" />
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
                    </div>
                </asp:View>
                <asp:View ID="Vw2Inventario" runat="server">
                    <br />
                    <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitInventario" runat="server" Text="Inventario por grupo a partir de un corte de fecha" />
                    </h6>
                    <div class="CentrarContenedor DivMarco">
                        <asp:ImageButton ID="IbtCerrarInvetr" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarInvetr_Click" />
                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label ID="LblAlmacenInv" runat="server" CssClass="LblEtiquet" Text="almacen" />
                                <asp:DropDownList ID="DdlAlmacenInv" runat="server" CssClass="heightCampo" Width="100%" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblGrupoInv" runat="server" CssClass="LblEtiquet" Text="grupo" />
                                <asp:DropDownList ID="DdlGrupoInv" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlGrupoInv_TextChanged" AutoPostBack="true" />
                            </div>
                            <div class="col-sm-2">
                                <br />
                                <asp:RadioButton ID="RdbSrlzdInv" runat="server" CssClass="LblEtiquet" GroupName="Grp" Checked="false" Text="serializado &nbsp" Enabled="false" />&nbsp&nbsp&nbsp
                                <asp:RadioButton ID="RdbNoSrlzdInv" runat="server" CssClass="LblEtiquet" GroupName="Grp" Checked="false" Text="no serializado &nbsp" Enabled="false" />&nbsp&nbsp&nbsp
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechCorte" runat="server" CssClass="LblEtiquet" Text="Fecha corte" />
                                <asp:TextBox ID="TxtFechCorte" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-0">
                                <br />
                                <asp:ImageButton ID="IbtExprtrInvtr" runat="server" ToolTip="Exportar inventario" CssClass=" BtnExpExcel" Height="38px" Width="40px" ImageUrl="~/images/ExcelV1.png" OnClick="IbtExprtrInvtr_Click" />
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw3EstadoCompra" runat="server">
                    <br />
                    <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitCompraPend" runat="server" Text="estado de las ordenes de compra" />
                    </h6>
                    <div class="CentrarContenedor DivMarco">
                        <asp:ImageButton ID="IbtCerrarCompPend" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarCompPend_Click" />
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechECI" runat="server" CssClass="LblEtiquet" Text="Fecha Inicial" />
                                <asp:TextBox ID="TxtFechECI" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechECF" runat="server" CssClass="LblEtiquet" Text="Fecha Final" />
                                <asp:TextBox ID="TxtFechECF" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-2">
                                <asp:ImageButton ID="IbtExpECompPend" runat="server" ToolTip="exportar estado compras" CssClass=" BtnExpExcel" Height="38px" Width="40px" ImageUrl="~/images/ExcelV1.png" OnClick="IbtExpECompPend_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <table class="">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RdbECAll" runat="server" CssClass="LblEtiquet" Text="&nbsp todos" GroupName="EstdComp" />&nbsp&nbsp&nbsp
                                            <asp:RadioButton ID="RdbECCot" runat="server" CssClass="LblEtiquet" Text="&nbsp cotizacion" GroupName="EstdComp" />&nbsp&nbsp&nbsp
                                            <asp:RadioButton ID="RdbECComp" runat="server" CssClass="LblEtiquet" Text="&nbsp Compra" GroupName="EstdComp" />&nbsp&nbsp&nbsp                               
                                            <asp:RadioButton ID="RdbECPN" runat="server" CssClass="LblEtiquet" Text="&nbsp P/N" GroupName="EstdComp" />&nbsp&nbsp&nbsp
                                            <asp:RadioButton ID="RdbECProv" runat="server" CssClass="LblEtiquet" Text="&nbsp proveedor" GroupName="EstdComp" />&nbsp&nbsp&nbsp
                                            <asp:CheckBox ID="CkbECPend" runat="server" CssClass="LblEtiquet" Text="&nbsp pendientes" Checked="true" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtECDocBusq" runat="server" Width="250px" Height="28px" CssClass=" heightCampo" placeholder="Ingrese el dato a consultar" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="IbtECBusqueda" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtECBusqueda_Click" /></td>
                                    </tr>
                                </table>
                                <div class="ScrollDet1">
                                    <asp:GridView ID="GrdDetEstdComp" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true"
                                        DataKeyNames=""
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" Width="130%" AllowSorting="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="pedido" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodPed" Text='<%# Eval("CodPedido") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="f. pedido" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblRpFcPed" Text='<%# Eval("Fechapedido") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cotizacion" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodPCot" Text='<%# Eval("CodCotizacion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="reparacion" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodOC" Text='<%# Eval("CodOrdenCompra") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="f. reparacion" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFchCom" Text='<%# Eval("FechaOC") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="tipo" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblTipo" Text='<%# Eval("Tipo") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="aprobad" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblAprob" Text='<%# Eval("Aprobado") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pos." HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPos" Text='<%# Eval("Posicion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="identifi" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblIdntfc" Text='<%# Eval("Identificador") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCant" Text='<%# Eval("Cantidad") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant entrad" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantEnt" Text='<%# Eval("Cant_Entrada") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant salida" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantSal" Text='<%# Eval("Cant_Salida") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant pend" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantPend" Text='<%# Eval("Cant_Pendiente") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="und compra" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblundComp" Text='<%# Eval("UndCompra") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="und despach" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblUndDesp" Text='<%# Eval("UndDespacho") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="estado" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblEstd" Text='<%# Eval("Estado") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="importac" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblImpor" Text='<%# Eval("Importacion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="exportacion" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblExpor" Text='<%# Eval("Exportacion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="propuesta" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPPT" Text='<%# Eval("CodigoPPT") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Doc proveedor" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodProv" Text='<%# Eval("CodProveedor") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="proveedor" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblProv" Text='<%# Eval("RazonSocial") %>' runat="server" Width="100%" />
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
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="IbtExcelHisAlmaRepa" />
            <asp:PostBackTrigger ControlID="IbtExpRepaPend" />
            <asp:PostBackTrigger ControlID="IbtExprtrInvtr" />
            <asp:PostBackTrigger ControlID="IbtExpECompPend" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
