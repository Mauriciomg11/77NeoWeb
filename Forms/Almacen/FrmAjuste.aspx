<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmAjuste.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmAjuste" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">  

        function myFuncionddl() {
            $('#<%=DdlMvto.ClientID%>').chosen();
            $('#<%=DdlAlmac.ClientID%>').chosen();
            $('#<%=DdlCcost.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="CentrarContenedor DivMarco">     
                 <br /><br /><br /><br />
                <div class="row">
                    <div class="col-sm-2">
                        <asp:Label ID="LblMvto" runat="server" CssClass="LblEtiquet" Text="movimiento" />
                        <asp:DropDownList ID="DdlMvto" runat="server" CssClass="heightCampo" Width="100%" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="LblAlmac" runat="server" CssClass="LblEtiquet" Text="almacen" />
                        <asp:DropDownList ID="DdlAlmac" runat="server" CssClass="heightCampo" Width="100%" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="LblCcost" runat="server" CssClass="LblEtiquet" Text="c. costo" />
                        <asp:DropDownList ID="DdlCcost" runat="server" CssClass="heightCampo" Width="100%" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="LblFech" runat="server" CssClass="LblEtiquet" Text="fecha ajuste" />
                        <asp:TextBox ID="TxtFech" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="LblMotvo" runat="server" CssClass="LblEtiquet" Text="motivo" />
                        <asp:TextBox ID="TxtMotvo" runat="server" Width="100%" TextMode="MultiLine" Font-Size="10px" MaxLength="350" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-1">
                        <asp:Button ID="BtnCargarInvIni" runat="server" Text="Cargar archivo" CssClass=" btn btn-primary botones Font_btnCrud" OnClick="BtnCargarInvIni_Click" />
                        <asp:FileUpload ID="FUpCargaInvIni" runat="server" Font-Size="9px" Visible="false" />
                    </div>

                    <div class="col-sm-1">
                        <asp:Button ID="BtnSubirInventario" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnSubirInventario_Click" OnClientClick="target ='';" Text="Subir" Visible="false"/>
                    </div>
                </div>
                <br />
                <div id="Stock Almacen" class="row">
                    <div class="col-sm-12">
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitInconsist" runat="server" Text="" />
                        </h6>
                        <div class="ScrollStockAlma">
                            <asp:GridView ID="GrdInconsist" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both">
                                <Columns>
                                    <asp:TemplateField HeaderText="almacen">
                                        <ItemTemplate>
                                            <asp:Label ID="LblAlmac" Text='<%# Eval("CodAlmacen") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P/N">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPn" Text='<%# Eval("PNAJ") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="S/N">
                                        <ItemTemplate>
                                            <asp:Label ID="LblSn" Text='<%# Eval("Sn") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="lote">
                                        <ItemTemplate>
                                            <asp:Label ID="LblLot" Text='<%# Eval("Lote") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="cant">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCant" Text='<%# Eval("CantAjustarAJ") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="bodega">
                                        <ItemTemplate>
                                            <asp:Label ID="LblBod" Text='<%# Eval("Bodega") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="fila">
                                        <ItemTemplate>
                                            <asp:Label ID="Lblfl" Text='<%# Eval("Fila") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="col">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCln" Text='<%# Eval("Columna") %>' runat="server" />
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
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnCargarInvIni" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
