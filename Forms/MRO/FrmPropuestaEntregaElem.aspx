<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmPropuestaEntregaElem.aspx.cs" Inherits="_77NeoWeb.Forms.MRO.FrmPropuestaEntregaElem" %>

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
        function myFuncionddl() {
            $('#<%=DdlPpt.ClientID%>').chosen();
            $('#<%=DdlHk.ClientID%>').chosen();
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
                        <asp:Label ID="LblCliente" runat="server" CssClass="LblEtiquet" Text="" Font-Bold="true" Font-Size="Larger" />
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

                    <div class="col-sm-3">
                        <asp:Label ID="LblPpt" runat="server" CssClass="LblEtiquet" Text=" propuesta nro" />
                        <asp:DropDownList ID="DdlPpt" runat="server" CssClass="heightCampo" Width="70%" OnTextChanged="DdlPpt_TextChanged" AutoPostBack="true" />
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="LblHk" runat="server" CssClass="LblEtiquet" Text="aeronave" />
                        <asp:DropDownList ID="DdlHk" runat="server" CssClass="heightCampo" Width="70%" />
                    </div>
                    <div class="col-sm-1"></div>
                    <div class="col-sm-5">
                        <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="30%" OnClick="BtnIngresar_Click" OnClientClick="target ='';" Text="nuevo" Enabled="false" />
                    </div>
                </div>
                <div class="ScrollDet1">
                    <asp:GridView ID="GrdDetalle" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false"
                        DataKeyNames="CodElemento, CodIdAlmacen,CodBodega,IdentificadorElem,CodTipoElem,CodUndMed,CodIdUbicacion,IdDetPropHk"
                        CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%"
                        OnRowDataBound="GrdDetalle_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Sel." HeaderStyle-Width="1%">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CkbSelP" Checked='<%# Eval("Chk").ToString()=="1" ? true : false %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referencia" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="LblRef" Text='<%# Eval("CodReferencia") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripcion" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="LblSn" Text='<%# Eval("SNLOTE") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="cantidad" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="LblCant" Text='<%# Eval("Cantidad") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="almacén" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Almacen") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bodega" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Bodega") %>' runat="server" Width="100%" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
