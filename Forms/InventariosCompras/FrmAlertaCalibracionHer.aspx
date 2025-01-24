<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmAlertaCalibracionHer.aspx.cs" Inherits="_77NeoWeb.Forms.InventariosCompras.FrmAlertaCalibracionHer" %>

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
        }

        .CentrarContenedor {
            position: absolute;
            left: 50%;
            width: 97%;
            margin-left: -49%;
            height: 70%;
            padding: 5px;
        }

        .CentrarEncabezado {
            left: 50%;
            /*determinamos una anchura*/
            width: 97%;
            margin-left: 1%;
            height: 8%;
        }

        .CentrarSinConfg {
            position: absolute;
            left: 50%;
            width: 94%;
            margin-left: -47%;
            height: 70%;
            padding: 5px;
        }

        .GridDis {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 80%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">         
        function myFuncionddl() {
            $('#<%=DdlTipo.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPnlBtnPpl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br /><br />
            <div class="CentrarEncabezado DivMarco">
                <div class="row">
                    <div class="col-sm-2">
                        <asp:Button ID="BtnSinConfigurar" CssClass="btn btn-primary Font_btnCrud" runat="server" Text="Sin configurar" Width="100%" OnClick="BtnSinConfigurar_Click" OnClientClick="target ='';" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Button ID="BtnAbrirElem" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnAbrirElem_Click" OnClientClick="target ='';" Text="Elemento" />
                    </div>
                    <div class="col-sm-2">
                    </div>
                    <div class="col-sm-4">
                        <asp:Label ID="LblTipo" runat="server" CssClass="LblEtiquet" Text="tipo elemento" />
                        <asp:DropDownList ID="DdlTipo" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlTipo_TextChanged" AutoPostBack="true" />
                    </div>
                </div>
            </div>
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <div class="CentrarContenedor DivMarco">
                        <div class="row ">
                            <div class="col-sm-12">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitProxVenc" runat="server" Text="proximos vencimientos de elementos" /></h6>
                            </div>
                        </div>
                        <div class="GridDis">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:GridView ID="GrdProxVenc" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both" OnRowDataBound="GrdProxVenc_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="tipo" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Tipo") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Parte") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S/N">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Serie") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="referencia">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Referencia") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripcion" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fecha Vencimiento">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Fecha_Vencimiento") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Almacen">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Almacen") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bodega">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Bodega") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cantidad">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Cantidad") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Proyeccion">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Remanente") %>' runat="server" Width="100%" />
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
                <asp:View ID="Vw1SinConfigurar" runat="server">
                    <asp:ImageButton ID="IbtCloseSinConfg" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCloseSinConfg_Click" />
                    <div class="CentrarSinConfg DivMarco">
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitSinConf" runat="server" Text="Elementos sin asingar fecha vencimiento" />
                        </h6>
                        <div class="GridDis">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:GridView ID="GrdSinConfg" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both">
                                        <Columns>
                                            <asp:TemplateField HeaderText="tipo" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Tipo") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Parte") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S/N">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Serie") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="referencia">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Referencia") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripcion" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fecha Vencimiento">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Fecha_Vencimiento") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Almacen">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Almacen") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bodega">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Bodega") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cantidad">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Cantidad") %>' runat="server" Width="100%" />
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
    </asp:UpdatePanel>
</asp:Content>
