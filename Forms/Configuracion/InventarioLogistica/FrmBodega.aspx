<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmBodega.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.InventarioLogistica.FrmBodega" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .Scroll {
            vertical-align: top;
            overflow: auto;
            width: 70%;
            height: 570px;
            margin-left: auto;
            margin-right: auto;
        }

        .CentarGrid {
            width: 60%;
            margin-left: auto;
            margin-right: auto;
            width: 50%;
        }

        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">       
        function myFuncionddl() {
            $('#<%=DdlBusq.ClientID%>').chosen();
            $('#<%=DdlCliente.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
   <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplPpl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="Scroll">
                <br /><br />
                <div class="row">
                    <div class="col-sm-9">
                        <asp:Label ID="LblBusq" runat="server" CssClass="LblEtiquet" Text=" Consultar Persona" />
                        <asp:DropDownList ID="DdlBusq" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlBusq_TextChanged" AutoPostBack="true" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3">
                        <asp:Label ID="LblCod" runat="server" CssClass="LblEtiquet" Text="Cod" />
                        <asp:TextBox ID="TxtCod" runat="server" CssClass="form-control heightCampo" MaxLength="30" Enabled="false" Width="100%" />
                    </div>
                    <div class="col-sm-6">
                        <asp:Label ID="LblNombre" runat="server" CssClass="LblEtiquet" Text="NOm" />
                        <asp:TextBox ID="TxtNombre" runat="server" CssClass="form-control heightCampo" MaxLength="80" Enabled="false" Width="100%" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-9">
                        <asp:Label ID="LblCliente" runat="server" CssClass="LblEtiquet" Text=" Consultar Persona" Visible="false" />
                        <asp:DropDownList ID="DdlCliente" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" Visible="false"  />
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-2">
                        <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success botones" Width="100%" OnClick="BtnIngresar_Click" Text="nuevo" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Button ID="BtnModificar" runat="server" CssClass="btn btn-success botones" Width="100%" OnClick="BtnModificar_Click" Text="modificar" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Button ID="BtnEliminar" runat="server" CssClass="btn btn-success botones" Width="100%" OnClick="BtnEliminar_Click" Text="Elimina" />
                    </div>
                </div>
                <div class="DivGrid DivContendorGrid">
                    <asp:GridView ID="GrdDetalle" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodUbicaBodega, CodBodega"
                        CssClass="DiseñoGrid table-sm" GridLines="Both" AllowPaging="true" PageSize="7"
                        OnRowCommand="GrdDetalle_RowCommand" OnRowEditing="GrdDetalle_RowEditing" OnRowUpdating="GrdDetalle_RowUpdating"
                        OnRowCancelingEdit="GrdDetalle_RowCancelingEdit" OnRowDeleting="GrdDetalle_RowDeleting" OnRowDataBound="GrdDetalle_RowDataBound"
                        OnPageIndexChanging="GrdDetalle_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="F" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Fila") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtFila" Text='<%# Eval("Fila") %>' runat="server" MaxLength="10" Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtFilaPP" runat="server" MaxLength="10" Width="100%" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="C" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Columna") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtColum" Text='<%# Eval("Columna") %>' runat="server" MaxLength="10" Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TxtColumPP" runat="server" MaxLength="10" Width="100%" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acti" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CkbActP" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CkbAct" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="CkbActPP" runat="server" Checked="true" Enabled="false" />
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
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="IbtHisC1Excel" />
                    <asp:PostBackTrigger ControlID="IbtHisC2Excel" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
