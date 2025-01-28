<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmUbicacionElemento.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmUbicacionElemento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarContndAsig {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            left: 50%;
            /*determinamos una anchura*/
            width: 80%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -40%;
            /*determinamos una altura*/
            height: 85%;
            /*indicamos que el margen superior, es la mitad de la altura*/
            padding: 5px;
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
            top: 150px
        }

        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .ScrollDivGrid {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 70%;
        }

        .ScrollDivGridAsig {
            vertical-align: top;
            overflow: auto;
            width: 30%;
            height: 70%;
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
            $('#<%=DdlAlmacen.ClientID%>').chosen();
            $('#<%=DdlTipo.ClientID%>').chosen();
            $('#<%=DdlBodDest.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <div class="CentrarContenedor DivMarco">
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblAlmacen" runat="server" CssClass="LblEtiquet" Text="almacen" />
                                <asp:DropDownList ID="DdlAlmacen" runat="server" CssClass="heightCampo" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblTipo" runat="server" CssClass="LblEtiquet" Text="Tipo" />
                                <asp:DropDownList ID="DdlTipo" runat="server" CssClass="heightCampo" Width="100%" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Button ID="BtnConsult" runat="server" CssClass="btn btn-primary" Width="100%" OnClick="BtnConsult_Click" Text="consultar" />
                            </div>
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="LblBusqueda" runat="server" Text="P/N: " CssClass="LblTextoBusq" /></td>
                                <td>
                                    <asp:TextBox ID="TxtBusqueda" runat="server" Width="100%" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                                <td>
                                    <asp:Label ID="LblBusBodeg" runat="server" Text="Bodega: " CssClass="LblTextoBusq" /></td>
                                <td>
                                    <asp:TextBox ID="TxtBusBodeg" runat="server" Width="100%" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                            </tr>
                        </table>
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitUbicaFis" runat="server" Text="ubicaciones físicas" /></h6>
                        <div class="ScrollDivGrid">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="CodIdUbicacion,CodUbicaBodega,CodElemento,CodTipoElemento,IdentificadorElem,Activo,CodTercero,
                                            FechaVencimientoR,POSBAJA,BAJA,REPA,MANTO,INTA,NALS,INSP,Reparable,CodEstadoBodega"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both"
                                        OnRowCommand="GrdDatos_RowCommand" OnRowDataBound="GrdDatos_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Asignar" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <%--  <asp:UpdatePanel ID="UplAbrir" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>--%>
                                                    <asp:ImageButton ID="IbtAsig" Width="30px" Height="30px" ImageUrl="~/images/IrV2.png" runat="server" CommandName="TrasldBod" ToolTip="Traslado de ubicación" />
                                                    <%-- </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="IbtAsig" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SP" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CkbSPP" Checked='<%# Eval("SP").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S/N">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblSn" Text='<%# Eval("Sn") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lote" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblLote" Text='<%# Eval("NumLote") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CodReferencia">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("CodReferencia") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cantidad" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCant" Text='<%# Eval("Cantidad") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CodUndMed">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblUndM" Text='<%# Eval("CodUndMed") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CodBodega" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCodBod" Text='<%# Eval("CodBodega") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fila">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Fila") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Columna">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Columna") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="camb lot" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="IbtCambLot" Width="30px" Height="30px" ImageUrl="~/images/IrV2.png" runat="server" CommandName="CambLot" ToolTip="cambio lote" Visible="false" />
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
                <asp:View ID="Vw1CambioBod" runat="server">
                     <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitCambioBod" runat="server" Text="Traslado de Bodega" />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarCambioBod" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarCambioBod_Click" />
                    <div class="CentrarContndAsig DivMarco">
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblPN" runat="server" CssClass="LblEtiquet" Text="P/N" />
                                <asp:TextBox ID="TxtPN" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblSN" runat="server" CssClass="LblEtiquet" Text="S/N" />
                                <asp:TextBox ID="TxtSN" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblLote" runat="server" CssClass="LblEtiquet" Text="Lote" />
                                <asp:TextBox ID="TxtLote" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblBodOrig" runat="server" CssClass="LblEtiquet" Text="Bodega Origen" />
                                <asp:TextBox ID="TxtBodOrig" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblCantAct" runat="server" CssClass="LblEtiquet" Text="Cantidad Actual" />
                                <asp:TextBox ID="TxtCantAct" runat="server" CssClass="form-control-sm heightCampo" Width="80%" TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" Text="0" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblCantNew" runat="server" CssClass="LblEtiquet" Text="Cantidad a Transferir" />
                                <asp:TextBox ID="TxtCantNew" runat="server" CssClass="form-control-sm heightCampo" Width="40%" TextMode="Number" step="0.01" onkeypress="return solonmeros(event);" Text="0" />
                                <asp:TextBox ID="TxtUndM" runat="server" CssClass="form-control-sm heightCampo" Width="55%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechI" runat="server" CssClass="LblEtiquet" Text="fecha Vencimiento" Visible="false" />
                                <asp:TextBox ID="TxtFechI" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" Visible="false" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-6">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitBodDes" runat="server" Text="Bodega Destino" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblBodDest" runat="server" CssClass="LblEtiquet" Text="Bodega" />
                                <asp:DropDownList ID="DdlBodDest" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlBodDest_TextChanged" AutoPostBack="true" />
                            </div>
                        </div>
                        <br />
                        <div class="ScrollDivGrid">
                            <div class="row">
                                <div class="col-sm-6">
                                    <asp:GridView ID="GrdUbicaDes" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="Cod"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both"
                                        OnRowCommand="GrdUbicaDes_RowCommand" OnRowDataBound="GrdUbicaDes_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Trasladar">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="IbtAsigD" Width="30px" Height="30px" ImageUrl="~/images/FlechaIr.png" runat="server" CommandName="Asignar" ToolTip="Trasladar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fila">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Fila") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Columna">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Columna") %>' runat="server" />
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
                </asp:View>
                <asp:View ID="Vw2CamLote" runat="server">
                     <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitCambLote" runat="server" Text="Cambio de lote" />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarCambioLote" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarCambioLote_Click" />
                    <div class="CentrarContndAsig DivMarco">
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblCLPN" runat="server" CssClass="LblEtiquet" Text="P/N" />
                                <asp:TextBox ID="TxtCLPN" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblCLNumLot" runat="server" CssClass="LblEtiquet" Text="lote" />
                                <asp:TextBox ID="TxtCLNumLot" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblCLCantOrg" runat="server" CssClass="LblEtiquet" Text="Cantidad Actual" />
                                <asp:TextBox ID="TxtCLCantOrg" runat="server" CssClass="form-control-sm heightCampo" Width="80%" TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" Text="0" Enabled="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3"></div>

                            <div class="col-sm-3">
                                <asp:Label ID="LblCLNewLot" runat="server" CssClass="LblEtiquet" Text=" nuevo lote" />
                                <asp:TextBox ID="TxtCLNewLot" runat="server" CssClass="form-control-sm heightCampo" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblCLNewCant" runat="server" CssClass="LblEtiquet" Text="Cant a transferir" />
                                <asp:TextBox ID="TxtCLNewCant" runat="server" CssClass="form-control-sm heightCampo" Width="80%" TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" Text="0" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Button ID="BtnTranLote" runat="server" CssClass="btn btn-primary" Width="100%" OnClick="BtnTranLote_Click" Text="Transferir" />
                            </div>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="IbtCerrarCambioBod" />
            <%--<asp:PostBackTrigger ControlID="IbnExcel" />--%>
            <asp:PostBackTrigger ControlID="IbtCerrarCambioLote" />
            <%--<asp:PostBackTrigger ControlID="BtnTranLote" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
