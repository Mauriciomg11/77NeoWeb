<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmTipoAeronave.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.MaestIngPrg.FrmTipoAeronave" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Tipo_Aeronave</title>
    <style type="text/css">
        .DivGrid {
            position: absolute;
            width: 45%;
            height: 600px;
            top: 15%;
            left: 30%;
            margin-top: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPanel" runat="server">
        <ContentTemplate>
            <table class="TablaBusqueda">
                <tr>
                    <td>
                        <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar"></asp:TextBox></td>
                    <td>
                        <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                </tr>
            </table>
            <div class="DivGrid DivContendorGrid">
                <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdTipoAeronave,CodTipoAeronave"
                    CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="8"
                    OnRowCommand="GrdDatos_RowCommand" OnRowEditing="GrdDatos_RowEditing" OnRowUpdating="GrdDatos_RowUpdating" OnRowCancelingEdit="GrdDatos_RowCancelingEdit"
                    OnRowDeleting="GrdDatos_RowDeleting" OnRowDataBound="GrdDatos_RowDataBound" OnPageIndexChanging="GrdDatos_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Codigo">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("CodTipoAeronave") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descripción">
                            <ItemTemplate>
                                <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TxtDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="300px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TxtDescPP" runat="server" Width="300px" />
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
                    <FooterStyle CssClass="GridFooterStyle" />
                    <HeaderStyle CssClass="GridCabecera" />
                    <RowStyle CssClass="GridRowStyle" />
                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8"/>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
