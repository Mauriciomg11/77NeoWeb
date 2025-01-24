<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmRazonRemocion.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.MaestIngPrg.FrmRazonRemocion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>RazonRemocion</title>
    <style type="text/css">
      /*  .DivGrid {
            margin: 0 auto;
            text-align: left;
            width: 85%;
            height: 600px;
            top: 15%;
            margin-top: 0px;
        }*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPanel" runat="server">
        <ContentTemplate>
            <div class="CentrarTable">
                <table class="TablaBusqueda">
                    <tr>
                        <td>
                            <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq"/></td>
                        <td>
                            <asp:TextBox ID="TxtBusqueda" runat="server" Width="450px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar"/></td>
                        <td>
                            <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                        <td>
                            <asp:ImageButton ID="IbtExpExcel" runat="server" ToolTip="Exportar" CssClass=" BtnExpExcel" ImageUrl="~/images/ExcelV1.png" OnClick="IbtExpExcel_Click" /></td>
                    </tr>
                </table>
                <div class="DivGrid DivContendorGrid">
                    <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdRazonRemocion, CodRemocion"
                        CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" AllowPaging="true" PageSize="8"
                        OnRowCommand="GrdDatos_RowCommand" OnSelectedIndexChanged="GrdDatos_SelectedIndexChanged" OnRowEditing="GrdDatos_RowEditing"
                        OnRowUpdating="GrdDatos_RowUpdating" OnRowCancelingEdit="GrdDatos_RowCancelingEdit"
                        OnRowDeleting="GrdDatos_RowDeleting" OnRowDataBound="GrdDatos_RowDataBound" OnPageIndexChanging="GrdDatos_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Descripción">
                                <ItemTemplate>
                                    <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%"/>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtDes" Text='<%# Eval("Descripcion") %>' runat="server" MaxLength="100" Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtDesPP" runat="server" MaxLength="200" Width="100%" />
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
                                    <asp:CheckBox ID="CkbActivoPP" runat="server" Checked="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterStyle-Width="5%">
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
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="IbtExpExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
