<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmConfigPantalla.aspx.cs" Inherits="_77NeoWeb.Forms.Seguridad.FrmConfigPantalla" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    <title>Pantalla</title>
    <style type="text/css">
        .GridControl {
            Width: 100%;
            border-width: 3px;
        }

        .centrarTexto {
            position: relative;
            /*nos posicionamos en el centro del navegador*/
            top: 37%;
            left: 38%;
            /*determinamos una anchura*/
            width: 800px;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -200px;
            /*determinamos una altura*/
            height: 195px;
            /*indicamos que el margen superior, es la mitad de la altura*/
            margin-top: -150px;
            border: 1px solid #808080;
            padding: 5px;
            background-color: cadetblue;
            top: 215px
        }

        .DimensionTexto {
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Script" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">
        function myFuncionddl() {

        }
    </script>
</asp:Content>
<asp:Content ID="TituloPagina" ContentPlaceHolderID="TituloPagina" runat="server">
   <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPanel" runat="server">
        <ContentTemplate>             
            <table class="TablaBusqueda">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Busqueda: " CssClass="LblTextoBusq"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar"></asp:TextBox></td>
                    <td>
                        <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                </tr>
            </table>
            <div class="centrarTexto">
                <asp:Label ID="LblDescripcion" runat="server" CssClass=" btn-info" Text="Descripción"></asp:Label><br />
                <asp:TextBox ID="TxtDescripcion" runat="server" CssClass=" form-control DimensionTexto" Height="30px" Enabled="false"></asp:TextBox>
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="CkbPpl" runat="server" Text="Principal" Font-Size="Smaller" Enabled="false" /></td>
                        <td>
                            <asp:CheckBox ID="CkbIng" runat="server" Text="Ingresar" Font-Size="Smaller" Enabled="false" /></td>
                        <td>
                            <asp:CheckBox ID="CkbMod" runat="server" Text="Modificar" Font-Size="Smaller" Enabled="false" /></td>
                        <td>
                            <asp:CheckBox ID="CkbCons" runat="server" Text="Consultar" Font-Size="Smaller" Enabled="false" /></td>
                        <td>
                            <asp:CheckBox ID="CkbImpr" runat="server" Text="Imprimir" Font-Size="Smaller" Enabled="false" /></td>
                        <td>
                            <asp:CheckBox ID="CkbElim" runat="server" Text="Eliminar" Font-Size="Smaller" Enabled="false" /></td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" CssClass=" btn-info" Text="Caso especial 1"></asp:Label></td>
                        <td>
                            <asp:Label ID="Label3" runat="server" CssClass=" btn-info" Text="Caso especial 2"></asp:Label></td>
                        <td>
                            <asp:Label ID="Label4" runat="server" CssClass=" btn-info" Text="Caso especial 3"></asp:Label></td>
                        <td>
                            <asp:Label ID="Label5" runat="server" CssClass=" btn-info" Text="Caso especial 4"></asp:Label></td>
                        <td>
                            <asp:Label ID="Label6" runat="server" CssClass=" btn-info" Text="Caso especial 5"></asp:Label></td>
                        <td>
                            <asp:Label ID="Label7" runat="server" CssClass=" btn-info" Text="Caso especial 6"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtCE1" runat="server" CssClass=" form-control DimensionTexto" MaxLength="15" Height="30" Enabled="false"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="TxtCE2" runat="server" CssClass=" form-control DimensionTexto" MaxLength="15" Height="30" Enabled="false"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="TxtCE3" runat="server" CssClass=" form-control DimensionTexto" MaxLength="15" Height="30" Enabled="false"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="TxtCE4" runat="server" CssClass=" form-control DimensionTexto" MaxLength="15" Height="30" Enabled="false"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="TxtCE5" runat="server" CssClass=" form-control DimensionTexto" MaxLength="15" Height="30" Enabled="false"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="TxtCE6" runat="server" CssClass=" form-control DimensionTexto" MaxLength="15" Height="30" Enabled="false"></asp:TextBox></td>
                    </tr>
                </table>
                <table style="width: 30%; height: 10px">
                    <tr>
                        <td>
                            <asp:Button ID="BtnModificar" runat="server" CssClass=" btn btn-success botones" Text="Modificar" OnClick="BtnModificar_Click" />
                        </td>

                    </tr>
                </table>
            </div>
            <br /><br /><br /><br /><br />
            <div class="DivGrid DivContendorGrid">
                <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="CodIdFormulario"
                    CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="5" OnPageIndexChanging="GrdDatos_PageIndexChanging"
                    OnRowCommand="GrdDatos_RowCommand">
                    <FooterStyle CssClass="GridFooterStyle" />
                    <HeaderStyle CssClass="GridCabecera" />
                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                    <Columns>
                        <asp:CommandField SelectText="Activar" ShowSelectButton="True" ControlStyle-Width="70px" />
                        <asp:TemplateField HeaderText="Pantalla">
                            <ItemTemplate>
                                <asp:TextBox ID="TxtDescr" Text='<%# Eval("DescSangria") %>' runat="server" Width="300px" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Principal">
                            <ItemTemplate>
                                <asp:CheckBox ID="CkbPpal" Checked='<%# Eval("Principal").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ingresar">
                            <ItemTemplate>
                                <asp:CheckBox ID="CkbIng" Checked='<%# Eval("IngresarF").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Modificar">
                            <ItemTemplate>
                                <asp:CheckBox ID="CkbMod" Checked='<%# Eval("ModificarF").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Consultar">
                            <ItemTemplate>
                                <asp:CheckBox ID="CkbCons" Checked='<%# Eval("ConsultarF").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Imprimir">
                            <ItemTemplate>
                                <asp:CheckBox ID="CkbImp" Checked='<%# Eval("ImprimirF").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Eliminar">
                            <ItemTemplate>
                                <asp:CheckBox ID="CkbEli" Checked='<%# Eval("EliminarF").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CasoEspeciaLF1" HeaderText="Caso 1" />
                        <asp:BoundField DataField="CasoEspeciaLF2" HeaderText="Caso 2" />
                        <asp:BoundField DataField="CasoEspeciaLF3" HeaderText="Caso 3" />
                        <asp:BoundField DataField="CasoEspeciaLF4" HeaderText="Caso 4" />
                        <asp:BoundField DataField="CasoEspeciaLF5" HeaderText="Caso 5" />
                        <asp:BoundField DataField="CasoEspeciaLF6" HeaderText="Caso 6" />
                        <asp:BoundField DataField="CodIdFormulario" HeaderText="CodIdFormulario" Visible="false" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Des" Visible="false" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
