<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmPropuestaReciboElemV2.aspx.cs" Inherits="_77NeoWeb.Forms.MRO.FrmPropuestaReciboElemV2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
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
            width: 14%;
            height: 27px;
        }

        .ScrollDet1 {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 400px;
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
        function myFuncionddl() {
            $('[id *=DdlAlmaPP]').chosen();
            $('[id *=DdlBodegPP]').chosen();
            $('[id *=DdlPnP],[id *=DdlSNPP]').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
   <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <br />
            <div class="CentrarContenedor DivMarco">
                <div class="row">
                    <div class="col-sm-12">
                        <asp:Label ID="LblIndicaciones" runat="server" CssClass="LblEtiquet" Text="indicaciones" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-12">
                        <asp:Label ID="LblObserv" runat="server" CssClass="LblEtiquet" Text="Observaciones" />
                        <asp:TextBox ID="TxtObserv" runat="server" CssClass="form-control-sm" Width="100%" MaxLength="350" TextMode="MultiLine" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-7">
                        <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="13%" OnClick="BtnIngresar_Click" OnClientClick="target ='';" Text="nuevo" />
                        <asp:Button ID="BtnOpenElem" runat="server" CssClass="btn btn-primary Font_btnSelect" OnClick="BtnOpenElem_Click" OnClientClick="target ='_blank';" Text="elemento" />
                    </div>
                </div>
                <div class="ScrollDet1">
                    <asp:GridView ID="GrdDetalle" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true"
                        CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%"
                        OnRowCommand="GrdDetalle_RowCommand" OnRowDeleting="GrdDetalle_RowDeleting" OnRowDataBound="GrdDetalle_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("PN") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlPnP" runat="server" Width="100%" Height="28px" AutoPostBack="true" OnTextChanged="DdlPnP_TextChanged" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referencia" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("CodReferencia") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxRefPP" runat="server" MaxLength="80" Width="100%" Enabled="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripcion" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtDescPNPP" runat="server" MaxLength="80" Width="100%" Enabled="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("SN") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlSNPP" runat="server" Width="100%" Height="28px" AutoPostBack="true" OnTextChanged="DdlSNPP_TextChanged" />
                                    <asp:TextBox ID="TxtSNPP" runat="server" MaxLength="80" Width="100%" Visible="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="cantidad" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Cantidad") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtCant" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Number" step="1" onkeypress="return solonumeros(event);" Text="0" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="almacén" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("NomAlmacen") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlAlmaPP" runat="server" Width="100%" Height="28px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bodega" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("NomBodega") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DdlBodegPP" runat="server" Width="100%" Height="28px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- --%>
                            <asp:TemplateField FooterStyle-Width="1%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnOpenElem" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
