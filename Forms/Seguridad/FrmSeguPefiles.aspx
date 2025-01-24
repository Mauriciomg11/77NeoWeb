<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmSeguPefiles.aspx.cs" Inherits="_77NeoWeb.Forms.Seguridad.FrmSeguPefiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Roles</title>
    <style type="text/css">
       
        .GridControl {
            Width: 100%;
            border-width: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>Configuración de roles</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPanel" runat="server">
        <ContentTemplate>
             <br /><br />
            <table class="TablaBusqueda">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Busqueda: " CssClass="LblTextoBusq"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="TxtBusqueda" runat="server" Width="450px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar"></asp:TextBox></td>
                    <td>
                        <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                </tr>
            </table>
            <div class="DivGrid DivContendorGrid">
                <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodIdGrupo"
                    CellPadding="3" CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="10" OnPageIndexChanging="GrdDatos_PageIndexChanging"
                    OnRowCommand="GrdDatos_RowCommand" OnRowEditing="GrdDatos_RowEditing" OnRowUpdating="GrdDatos_RowUpdating" OnRowCancelingEdit="GrdDatos_RowCancelingEdit"
                    OnRowDeleting="GrdDatos_RowDeleting" OnRowDataBound="GrdDatos_RowDataBound">
                    <FooterStyle CssClass="GridFooterStyle" />
                    <HeaderStyle CssClass="GridCabecera" />
                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                    <Columns>
                        <asp:TemplateField HeaderText="Nombre del grupo" FooterStyle-Width="80%">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("NombreGrupo") %>' runat="server" Width="100%" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TxtNombreGrupo" Text='<%# Eval("NombreGrupo") %>' runat="server" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TxtNombreGrupoPP" runat="server" Width="100%" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField FooterStyle-Width="10%">
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
