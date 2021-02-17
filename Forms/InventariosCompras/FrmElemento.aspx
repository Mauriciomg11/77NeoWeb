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
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPnlCampos" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PnlCampos" runat="server">
                <table class="TablaCampos table-sm table table-responsive-sm">
                    <tr>
                        <td>
                            <asp:Label ID="LblCodigo" runat="server" CssClass="LblEtiquet" Text="Código:" /></td>
                        <td width="1%"></td>
                        <td width="12%">
                            <asp:TextBox ID="TxtCod" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>
                        <td>
                            <asp:Label ID="LblReferenc" runat="server" CssClass="LblEtiquet" Text="Referencia:" /></td>
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
                        <td>
                            <asp:Label ID="LblDescr" runat="server" CssClass="LblEtiquet" Text="Descripción:" /></td>
                        <td></td>
                        <td colspan="5">
                            <asp:TextBox ID="TxtDescr" runat="server" CssClass="form-control Campos" Enabled="false" /></td>
                        <td>
                            <asp:Label ID="LblLote" runat="server" CssClass="LblEtiquet" Text="Lote:" /></td>
                        <td>
                            <asp:TextBox ID="TxtLote" runat="server" CssClass="form-control Campos" Enabled="false" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblFechRec" runat="server" CssClass="LblEtiquet" Text="Fecha Recibo:" /></td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="TxtFecRec" runat="server" CssClass="form-control Campos" TextMode="Date" Enabled="false"></asp:TextBox></td>
                        <td>
                            <asp:Label ID="LblUndMed" runat="server" CssClass="LblEtiquet" Text="Und Med:" /></td>
                        <td>
                            <asp:TextBox ID="TxtUndMed" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>

                        <td>
                            <asp:Label ID="LblGrupo" runat="server" CssClass="LblEtiquet" Text="Grupo:" /></td>
                        <td>
                            <asp:DropDownList ID="DdlGrupo" runat="server" CssClass="form-control Campos" Enabled="false"></asp:DropDownList></td>
                        <td>
                            <asp:Label ID="LblAta" runat="server" CssClass="LblEtiquet" Text="Capitulo:" /></td>
                        <td>
                            <asp:TextBox ID="TxtAta" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblPosic" runat="server" CssClass="LblEtiquet" Text="Posición:" /></td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txtPosic" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>

                        <td>
                            <asp:Label ID="LblAerona" runat="server" CssClass="LblEtiquet" Text="Aeronave:" /></td>
                        <td>
                            <asp:TextBox ID="TxtHK" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>

                        <td>
                            <asp:Label ID="LblMayor" runat="server" CssClass="LblEtiquet" Text="Mayor:" /></td>
                        <td>
                            <asp:TextBox ID="TxtMayor" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>
                        <td>
                            <asp:Label ID="LblUbicTec" runat="server" CssClass="LblEtiquet" Text="Ubicación Técnica:" /></td>
                        <td>
                            <asp:TextBox ID="TxtUbiTec" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblSheLif" runat="server" CssClass="LblEtiquet" Text="Shelf-Life:" /></td>
                        <td>
                            <asp:ImageButton ID="IbtFechaI" runat="server" CssClass="BtnImagenCalender" ImageUrl="~/images/calendar.png" ImageAlign="AbsBottom" Height="18px" Width="15px" Enabled="false" OnClick="IbtFechaI_Click" /></td>
                        <td>
                            <asp:TextBox ID="TxtFecShelfLife" runat="server" CssClass="form-control Campos" Enabled="false"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalFechShelLife" runat="server" PopupButtonID="IbtFechaI" TargetControlID="TxtFecShelfLife" Format="dd/MM/yyyy" />
                            </div>
                            <td>
                                <asp:Label ID="LblEstad" runat="server" CssClass="LblEtiquet" Text="Estado:" /></td>
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
                                            <asp:RadioButton ID="RdbActivo" runat="server" GroupName="Activo" Enabled="false" Text="Activo" CssClass="LblEtiquet" /></td>
                                        <td>
                                            <asp:RadioButton ID="RdbInactivo" runat="server" GroupName="Activo" Enabled="false" CssClass="LblEtiquet" Text="Inactivo" /></td>
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
                                <h6 class="TituloContadoresAsig">
                                    <asp:Label ID="LblTitContAsig" runat="server" Text="Contadores asignados" /></h6>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:GridView ID="GrdCont" runat="server" EmptyDataText="Sin contadores asignados" AutoGenerateColumns="False"
                                CssClass="GridControl DiseñoGrid table" GridLines="Both">
                                <Columns>
                                    <asp:TemplateField HeaderText="Nombre">
                                        <ItemTemplate>
                                            <asp:Label ID="LblNo" Text='<%# Eval("Nombre") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Contador">
                                        <ItemTemplate>
                                            <asp:Label ID="LblConta" Text='<%# Eval("CodContador") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valor">
                                        <ItemTemplate>
                                            <asp:Label ID="LblValor" Text='<%# Eval("ValorActual") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
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
                <h6 class="TextoSuperior">
                    <asp:Label ID="LblTitOpcBusq" runat="server" Text="Opciones de búsqueda" /></h6>
                <table class="TablaBusqueda">
                    <tr>

                        <td width="10%">
                            <asp:RadioButton ID="RdbBusqPN" runat="server" GroupName="Busq" CssClass="LblEtiquet" Text="&nbsp P/N" /></td>
                        <td width="10%">
                            <asp:RadioButton ID="RdbBusqDesc" runat="server" GroupName="Busq" CssClass="LblEtiquet" Text="Descripción" /></td>
                        <td width="10%">
                            <asp:RadioButton ID="RdbBusqRef" runat="server" GroupName="Busq" CssClass="LblEtiquet" Text="Referencia" /></td>
                        <td width="10%">
                            <asp:RadioButton ID="RdbBusqSN" runat="server" GroupName="Busq" CssClass="LblEtiquet" Text="&nbsp S/N" /></td>
                    </tr>
                </table>
                <table class="TablaBusqueda">
                    <tr>
                        <td>
                            <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar"></asp:TextBox></td>
                        <td>
                            <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                        <td>
                            <asp:ImageButton ID="IbtCerrar" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrar_Click" /></td>
                    </tr>
                </table>
                <div class="DivGrid DivContendorGrid">
                    <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="CodElemento"
                        CssClass="GridControl DiseñoGrid table" GridLines="Both" AllowPaging="true" PageSize="7"
                        OnRowDataBound="GrdBusq_RowDataBound" OnSelectedIndexChanged="GrdBusq_SelectedIndexChanged" OnPageIndexChanging="GrdBusq_PageIndexChanging">
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        <Columns>
                            <asp:CommandField HeaderText="Select" SelectText="Select" ShowSelectButton="True" HeaderStyle-Width="33px" />
                            <asp:TemplateField HeaderText="Referencia">
                                <ItemTemplate>
                                    <asp:Label ID="LblRef" Text='<%# Eval("CodReferencia") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P/N">
                                <ItemTemplate>
                                    <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="S/N">
                                <ItemTemplate>
                                    <asp:Label ID="LblSn" Text='<%# Eval("Sn") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NumLote">
                                <ItemTemplate>
                                    <asp:Label ID="LblNumLote" Text='<%# Eval("NumLote") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Descripción">
                                <ItemTemplate>
                                    <asp:Label ID="LblDescripcion" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Recibo">
                                <ItemTemplate>
                                    <asp:Label ID="LblFechaRecibo" Text='<%# Eval("FechaRecibo") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CodUnidadMedida">
                                <ItemTemplate>
                                    <asp:Label ID="LblCodUnidadMedida" Text='<%# Eval("CodUnidadMedida") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Cod Grupo">
                                <ItemTemplate>
                                    <asp:Label ID="LblCodGrupo" Text='<%# Eval("CodGrupo") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Grupo">
                                <ItemTemplate>
                                    <asp:Label ID="LblGrupo" Text='<%# Eval("Grupo") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ATA">
                                <ItemTemplate>
                                    <asp:Label ID="LblATA" Text='<%# Eval("ATA") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Posicion Motor">
                                <ItemTemplate>
                                    <asp:Label ID="LblPosicionMotor" Text='<%# Eval("PosicionMotor") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Aeronave">
                                <ItemTemplate>
                                    <asp:Label ID="LblAeronave" Text='<%# Eval("Aeronave") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Mayor">
                                <ItemTemplate>
                                    <asp:Label ID="LblMayoro" Text='<%# Eval("Mayor") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="CodUbicacionFisica">
                                <ItemTemplate>
                                    <asp:Label ID="LblCodUbicacionFisica" Text='<%# Eval("CodUbicacionFisica") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Fecha ShelfLife">
                                <ItemTemplate>
                                    <asp:Label ID="LblFechaShelfLife" Text='<%# Eval("FechaShelfLife") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <asp:Label ID="LblEstado" Text='<%# Eval("Estado") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="FechaVence">
                                <ItemTemplate>
                                    <asp:Label ID="LblFechaVence" Text='<%# Eval("FechaVence") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="APU">
                                <ItemTemplate>
                                    <asp:Label ID="APU" Text='<%# Eval("APU") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Motor">
                                <ItemTemplate>
                                    <asp:Label ID="Motor" Text='<%# Eval("Motor") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Consignacion">
                                <ItemTemplate>
                                    <asp:Label ID="Consignacion" Text='<%# Eval("Consignacion") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Activo">
                                <ItemTemplate>
                                    <asp:Label ID="Activo" Text='<%# Eval("Activo") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="CodElemento">
                                <ItemTemplate>
                                    <asp:Label ID="CodElemento" Text='<%# Eval("CodElemento") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Identificador">
                                <ItemTemplate>
                                    <asp:Label ID="Identificador" Text='<%# Eval("Identificador") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                    </asp:GridView>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
