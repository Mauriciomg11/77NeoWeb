<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmSalidaConsignacion.aspx.cs" Inherits="_77NeoWeb.Forms.MRO.FrmSalidaConsignacion" %>

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
        } function myFuncionddl() {
            $('#<%=DdlBodega.ClientID%>').chosen();
            $('#<%=DdlCliente.ClientID%>').chosen();
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
            <br />
            <br />
            <div class="CentrarContenedor DivMarco">
                <div class="row">
                    <div class="col-sm-12">
                        <asp:Label ID="LblObserv" runat="server" CssClass="LblEtiquet" Text="Observaciones" />
                        <asp:TextBox ID="TxtObserv" runat="server" CssClass="form-control-sm" Width="100%" MaxLength="350" TextMode="MultiLine" />
                    </div>
                </div>
                <br />
                <div class="row">

                    <div class="col-sm-3">
                        <asp:Label ID="LblBodega" runat="server" CssClass="LblEtiquet" Text=" propuesta nro" />
                        <asp:DropDownList ID="DdlBodega" runat="server" CssClass="heightCampo" Width="40%" OnTextChanged="DdlBodega_TextChanged" AutoPostBack="true" />
                    </div>
                    <div class="col-sm-5">
                        <asp:Label ID="LblCliente" runat="server" CssClass="LblEtiquet" Text="aeronave" />
                        <asp:DropDownList ID="DdlCliente" runat="server" CssClass="heightCampo" Width="70%" />
                    </div>
                    <div class="col-sm-0"></div>
                    <div class="col-sm-4">
                        <br />
                        <asp:Button ID="BtnEntregar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="30%" OnClick="BtnEntregar_Click" OnClientClick="target ='';" Text="nuevo" Enabled="false" />
                    </div>
                </div>
                <div class="ScrollDet1">
                    <asp:GridView ID="GrdDetalle" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false"
                        DataKeyNames="CodBodega, CodIdAlmacen, CodElemento, CodIdUbicacion, ElemConsignacion, EntradaConsignacion, IdentificadorElem, CodTipoElem, CodUndMed"
                        CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%">
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
                            <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="LblSn" Text='<%# Eval("SNLOTE") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripcion" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
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
                            <asp:TemplateField HeaderText="cantidad entre" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:TextBox ID="LblCant" Text='<%# Eval("Cantidad") %>' runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Number" step="1" onkeypress="return solonumeros(event);" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="cantidad Act" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="LblCantAct" Text='<%# Eval("CantActual") %>' runat="server" Width="100%" />
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
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="DdlBodega" EventName="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
