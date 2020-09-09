<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmElemento.aspx.cs" Inherits="_77NeoWeb.Forms.InventariosCompras.FrmElemento" %>

<asp:Content ID="Titulo" ContentPlaceHolderID="head" runat="server">
    <title>Elemento</title>
    <style type="text/css">
        .DivGrid {
            position: absolute;
            OVERFLOW: auto;
            width: 98%;
            height: 73%;
            top: 24%;
            left: 1%;
            margin-top: 0px;
        }

        .TablaCampos {
            position: absolute;
            text-align: left;
            width: 98%;
        }

        .TablaContadores {
            position: absolute;
            text-align: left;
            width: 98%;
        }

        .TablaCheck {
            position: absolute;
            text-align: left;
            width: 17%;
            font-size: 100%;
        }

        .TablaActivo {
            position: absolute;
            text-align: left;
            width: 11%;
            font-size: 100%;
        }

        .Campos {
            Height: 30px;
            Width: 100%;
            font-size: 80%;
        }

        .TituloContadoresAsig {
            background-color: cadetblue; /*bg-info text-center*/
            text-align: center;
            color: aliceblue;
            width: 100%;
            /* font-size: 18px;*/
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">
        function myFuncionddl() {
            $('#<%=DdlPN.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>Configuración de Elementos</h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPnlCampos" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PnlCampos" runat="server">
                <table class="TablaCampos table-sm table table-responsive-sm">
                    <tr>
                        <td class="LblEtiquet">Código:</td>
                        <td width="1%"></td>
                        <td width="12%">
                            <asp:TextBox ID="TxtCod" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>
                        <td class="LblEtiquet">Referencia:</td>
                        <td width="18%">
                            <asp:TextBox ID="TxtRef" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>
                        <td class="LblEtiquet">P/N:</td>
                        <td width="35%">
                            <asp:DropDownList ID="DdlPN" runat="server" CssClass="form-control Campos" Font-Size="10px" Enabled="false"></asp:DropDownList></td>
                        <td class="LblEtiquet">S/N:</td>
                        <td width="35%">
                            <asp:TextBox ID="TxtSN" runat="server" CssClass="form-control Campos" Enabled="false" MaxLength="80"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="LblEtiquet">Descripción:</td>
                        <td></td>
                        <td colspan="5">
                            <asp:TextBox ID="TxtDescr" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>
                        <td class="LblEtiquet">Lote:</td>
                        <td>
                            <asp:TextBox ID="TxtLote" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="LblEtiquet">Fecha recibo:</td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="TxtFecRec" runat="server" CssClass="form-control Campos" TextMode="Date" Enabled="false"></asp:TextBox></td>
                        <td class="LblEtiquet">Und Med:</td>
                        <td>
                            <asp:TextBox ID="TxtUndMed" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>

                        <td class="LblEtiquet">Grupo:</td>
                        <td>
                            <asp:DropDownList ID="DdlGrupo" runat="server" CssClass="form-control Campos" Enabled="false"></asp:DropDownList></td>
                        <td class="LblEtiquet">Capitulo:</td>
                        <td>
                            <asp:TextBox ID="TxtAta" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="LblEtiquet">Posición:</td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txtPosic" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>

                        <td class="LblEtiquet">Aeronave:</td>
                        <td>
                            <asp:TextBox ID="TxtHK" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>

                        <td class="LblEtiquet">Mayor:</td>
                        <td>
                            <asp:TextBox ID="TxtMayor" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>
                        <td class="LblEtiquet">ubicación técnica:</td>
                        <td>
                            <asp:TextBox ID="TxtUbiTec" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="LblEtiquet">Fecha Shelf-Life:</td>
                        <td>
                            <asp:ImageButton ID="IbtFechaI" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" Enabled="false" OnClick="IbtFechaI_Click" /></td>
                        <td>
                            <asp:TextBox ID="TxtFecShelfLife" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalFechShelLife" runat="server" PopupButtonID="IbtFechaI" TargetControlID="TxtFecShelfLife" Format="dd/MM/yyyy" />
                            </div>
                            <td class="LblEtiquet">Estado:</td>
                            <td colspan="2">
                                <asp:TextBox ID="TxtEstado" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>
                            <td>
                                <table class="TablaCheck table-responsive">
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CkbApu" runat="server" CssClass="LblEtiquet" Text="Apu" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="CkbMot" runat="server" CssClass="LblEtiquet" Text="Motor" Enabled="false" /></td>
                                        <td>
                                            <asp:CheckBox ID="CkbConsig" runat="server" CssClass="LblEtiquet" Text="Consignación" Enabled="false" /></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table class="TablaActivo table-responsive">
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblActivo" runat="server" CssClass="LblEtiquet" Text="Activo"></asp:Label></td>
                                        <td>
                                            <asp:RadioButton ID="RdbActivo" runat="server" TextAlign="Left" GroupName="Activo" Enabled="false" /></td>
                                        <td>
                                            <asp:Label ID="LblInactivo" runat="server" CssClass="LblEtiquet" Text="Inactivo"></asp:Label></td>
                                        <td>
                                            <asp:RadioButton ID="RdbInactivo" runat="server" TextAlign="Left" GroupName="Activo" Enabled="false" /></td>
                                    </tr>
                                </table>
                            </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <table class="TablaCampos table-responsive-sm TablaActivo">
                                <tr>
                                    <td>
                                        <asp:Button ID="BtnModificar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnModificar_Click" Text="Modificar" /></td>
                                    <td>
                                        <asp:Button ID="BtnConsultar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnConsultar_Click" Text="Consultar" /></td>                                    
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <div>
                                <h6 class="TituloContadoresAsig">Contadores asignados</h6>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:GridView ID="GrdCont" runat="server" EmptyDataText="Sin contadores asignados"
                                CssClass="GridControl DiseñoGrid table" GridLines="Both">
                                <FooterStyle CssClass="GridFooterStyle" />
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />

                            </asp:GridView>
                        </td>
                    </tr>
                </table>

            </asp:Panel>
            <asp:Panel ID="PnlBusq" runat="server" Visible="false">
                <h6 class="TextoSuperior">Opciones de búsqueda</h6>
                <table class="TablaBusqueda">
                    <tr>
                        <td width="10%">
                            <asp:Label ID="Label3" runat="server" CssClass="LblEtiquet" Text="P/N"></asp:Label></td>
                        <td width="10%">
                            <asp:RadioButton ID="RdbBusqPN" runat="server" GroupName="Busq" /></td>
                        <td width="5%">
                            <asp:Label ID="Label4" runat="server" CssClass="LblEtiquet" Text="Descripción"></asp:Label></td>
                        <td width="10%">
                            <asp:RadioButton ID="RdbBusqDesc" runat="server" GroupName="Busq" /></td>
                        <td width="10%">
                            <asp:Label ID="Label5" runat="server" CssClass="LblEtiquet" Text="Referencia"></asp:Label></td>
                        <td width="10%">
                            <asp:RadioButton ID="RdbBusqRef" runat="server" GroupName="Busq" /></td>
                        <td width="10%">
                            <asp:Label ID="Label1" runat="server" CssClass="LblEtiquet" Text="S/N"></asp:Label></td>
                        <td width="10%">
                            <asp:RadioButton ID="RdbBusqSN" runat="server" GroupName="Busq" /></td>
                    </tr>
                </table>
                <table class="TablaBusqueda">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Busqueda: " CssClass="LblTextoBusq"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar"></asp:TextBox></td>
                        <td>
                            <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                        <td>
                            <asp:ImageButton ID="IbtCerrar" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrar_Click" /></td>
                    </tr>
                </table>
                <div class="DivGrid DivContendorGrid">
                    <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!"
                        CssClass="GridControl DiseñoGrid table" GridLines="Both" AllowPaging="true" PageSize="7"
                        OnRowDataBound="GrdBusq_RowDataBound" OnSelectedIndexChanged="GrdBusq_SelectedIndexChanged" OnPageIndexChanging="GrdBusq_PageIndexChanging">
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        <Columns>
                            <asp:CommandField HeaderText="Asignar" SelectText="Asignar" ShowSelectButton="True" HeaderStyle-Width="33px" />
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                    </asp:GridView>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
