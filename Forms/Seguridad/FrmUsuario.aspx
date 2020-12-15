<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmUsuario.aspx.cs" Inherits="_77NeoWeb.Forms.FrmUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Usuarios</title>
    <style type="text/css">
        .DivGrid {
            position: absolute;
            width: 98%;
            height: 600px;
            top: 15%;
            left: 1%;
            margin-top: 0px;
        }

        .GridControl {
            Width: 100%;
            /* border-color:black; --BorderColor="#999999" */
            /*  border-style:double; -- BorderStyle="Double" */
            border-width: 3px;
            /*BorderWidth="1px"*/
        }
        .Scroll-table2 {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 400px
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">
        function myFuncionddl() {
            $('[id*=DdlUsuPP]').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>Datos de los usuarios</h1>
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
            <div class="table-responsive Scroll-table2"">
                <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodUsuario"
                    CellPadding="3" CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" AllowPaging="true" PageSize="8" OnPageIndexChanging="GrdDatos_PageIndexChanging"
                    OnRowCommand="GrdDatos_RowCommand" OnRowEditing="GrdDatos_RowEditing" OnRowUpdating="GrdDatos_RowUpdating" OnRowCancelingEdit="GrdDatos_RowCancelingEdit"
                    OnRowDeleting="GrdDatos_RowDeleting" OnRowDataBound="GrdDatos_RowDataBound">
                    <FooterStyle CssClass="GridFooterStyle" />
                    <HeaderStyle CssClass="GridCabecera" />
                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                    <Columns>
                        <asp:TemplateField HeaderText="Codigo / usuario">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("CodUsuario") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label Text='<%# Eval("CodUsuario") %>' runat="server" Enabled="false" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="DdlUsuPP" runat="server" Width="350px" Height="28px" OnTextChanged="DdlUsuPP_TextChanged" AutoPostBack="true" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Identificación">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("Identificacion") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TxtIden" Text='<%# Eval("Identificacion") %>' runat="server" Width="100px" Enabled="false" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TxtIdenPP" runat="server" Width="100px" Enabled="false" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre de usuario">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("Nombres") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TxtNombre" Text='<%# Eval("Nombres") %>' runat="server" Width="180px" Enabled="false" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TxtNombrePP" runat="server" Width="180px" Enabled="false" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FechaUltimoAcceso" HeaderText="FechaUltimoAcceso" />
                        <asp:TemplateField HeaderText="Usuario">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("Usuario") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TxtUsu" Text='<%# Eval("Usuario") %>' runat="server" Width="100px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TxtUsuPP" runat="server" Width="100px" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Clave">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("ClaveTxt") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TxtPassW" runat="server" Value='<%# Eval("PassWeb") %>' TextMode="Password" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TxtPassWPP" runat="server" Width="150px" TextMode="Password" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Activo">
                            <ItemTemplate>
                                <asp:CheckBox ID="CkbActivoP" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="CkbActivo" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="CkbActivoPP" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />

                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
