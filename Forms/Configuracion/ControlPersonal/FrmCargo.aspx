<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmCargo.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.ControlPersonal.FrmCargo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>TA</title>
    <style type="text/css">      
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPanel" runat="server">
        <ContentTemplate>
                <table class="TablaBusqueda">
                    <tr>
                        <td>
                            <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                        <td>
                            <asp:TextBox ID="TxtBusqueda" runat="server" Width="450px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                        <td>
                            <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                    </tr>
                </table>
                <div class="CentrarGridV2" style="--grid-width: <%= AnchoGrid %>;">
                    <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodCargo"
                        CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" AllowPaging="true" PageSize="8"
                        OnRowCommand="GrdDatos_RowCommand" OnRowEditing="GrdDatos_RowEditing"
                        OnRowUpdating="GrdDatos_RowUpdating" OnRowCancelingEdit="GrdDatos_RowCancelingEdit" OnRowDeleting="GrdDatos_RowDeleting"
                        OnRowDataBound="GrdDatos_RowDataBound" OnPageIndexChanging="GrdDatos_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Código" ItemStyle-CssClass="AutoColunmGV">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("CodCargo") %>' runat="server"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripción" ItemStyle-CssClass="90%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtNomT" Text='<%# Eval("Descripcion") %>' runat="server" MaxLength="200" Width="100%"/>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtNomTPP" runat="server" MaxLength="200"/>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tripulante" ItemStyle-CssClass="AutoColunmGV">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CkbTripP" Checked='<%# Eval("Tripulante").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CkbTrip" Checked='<%# Eval("Tripulante").ToString()=="1" ? true : false %>' runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="CkbTripPP" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Genera Reporte" ItemStyle-CssClass="AutoColunmGV">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CkbGenRteP" Checked='<%# Eval("CrearReporte").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CkbGenRte" Checked='<%# Eval("CrearReporte").ToString()=="1" ? true : false %>' runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="CkbGenRtePP" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Despacho Reserva" ItemStyle-CssClass="AutoColunmGV">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CkbDespRvaP" Checked='<%# Eval("DespachoReserva").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CkbDespRva" Checked='<%# Eval("DespachoReserva").ToString()=="1" ? true : false %>' runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="CkbDespRvaPP" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Técnico" ItemStyle-CssClass="AutoColunmGV">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CkbTecnP" Checked='<%# Eval("Tecnico").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CkbTecn" Checked='<%# Eval("Tecnico").ToString()=="1" ? true : false %>' runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="CkbTecnPP" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Inspector" ItemStyle-CssClass="AutoColunmGV">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CkbInspP" Checked='<%# Eval("Inspector").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CkbInsp" Checked='<%# Eval("Inspector").ToString()=="1" ? true : false %>' runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="CkbInspPP" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-CssClass="AutoColunmGV">
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
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                    </asp:GridView>
                </div>
            <%--</div>--%>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
