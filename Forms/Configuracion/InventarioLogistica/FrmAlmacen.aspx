<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmAlmacen.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.InventarioLogistica.FrmAlmacen" %>

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
            $('#<%=DdlBase.ClientID%>').chosen();
            $('#<%=ddlUbicaFis.ClientID%>').chosen();
            $('[id *=DdlUsuPP]').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <div class="Scroll">
                        <div class="row">
                            <div class="col-sm-9">
                                <asp:Label ID="LblBusq" runat="server" CssClass="LblEtiquet" Text=" Consultar Persona" />
                                <asp:DropDownList ID="DdlBusq" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlBusq_TextChanged" AutoPostBack="true" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblCod" runat="server" CssClass="LblEtiquet" Text="Cod" />
                                <asp:TextBox ID="TxtCod" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-5">
                                <asp:Label ID="LblNombre" runat="server" CssClass="LblEtiquet" Text="NOm" />
                                <asp:TextBox ID="TxtNombre" runat="server" CssClass="form-control heightCampo" MaxLength="80" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-5">
                                <asp:Label ID="LblDescrip" runat="server" CssClass="LblEtiquet" Text="Desc" />
                                <asp:TextBox ID="TxtDescrip" runat="server" CssClass="form-control heightCampo" MaxLength="80" Enabled="false" Width="100%" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-5">
                                <asp:Label ID="LblBase" runat="server" CssClass="LblEtiquet" Text="Bas" />
                                <asp:DropDownList ID="DdlBase" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblUbicGeog" runat="server" CssClass="LblEtiquet" Text="Ubica Geo" />
                                <asp:TextBox ID="TxtUbicGeog" runat="server" CssClass="form-control heightCampo" MaxLength="80" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <br />
                                <asp:CheckBox ID="CkbActivo" runat="server" CssClass="LblEtiquet" Text="Act" Enabled="false" />
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
                            <div class="col-sm-2">
                                <asp:Button ID="BtnAsigPers" runat="server" CssClass="btn btn-primary botones" Width="100%" OnClick="BtnAsigPers_Click" Text="Elimina" />
                            </div>
                        </div>
                        <br />
                        <div class="DivGrid DivContendorGrid">
                            <h6 class="TextoSuperior">
                                <asp:Label ID="LblTitUbicaAsig" runat="server" Text="Ubica" /></h6>
                            <asp:GridView ID="GrdDetalle" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodUbicaBodega"
                                CssClass="DiseñoGrid table-sm" GridLines="Both" AllowPaging="true" PageSize="7"
                                OnRowCommand="GrdDetalle_RowCommand" OnRowDeleting="GrdDetalle_RowDeleting" OnRowDataBound="GrdDetalle_RowDataBound"
                                OnPageIndexChanging="GrdDetalle_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Bod" HeaderStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("CodBodega") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="F" HeaderStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Fila") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="C" HeaderStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Columna") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prp" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CkbActP" Checked='<%# Eval("Propied2").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField FooterStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                        </ItemTemplate>
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
                </asp:View>
                <asp:View ID="Vw1AgregarUbicaciones" runat="server">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitAsigarUbica" runat="server" Text="Asigar ubicaciones físicas" /></h6>
                    <asp:ImageButton ID="IbtCerrarAsigUbica" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarAsigUbica_Click" ImageAlign="Right" />
                    <div class="row">
                        <div class="col-sm-3">
                            <asp:Label ID="LblUbicaFis" runat="server" CssClass="LblEtiquet" Text=" Consultar Bodega" />
                            <asp:DropDownList ID="ddlUbicaFis" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="ddlUbicaFis_TextChanged" AutoPostBack="true" />
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-5">
                            <h6 class="TextoSuperior">
                                <asp:Label ID="LblTitUbicaDispo" runat="server" Text="Ubica Disponibles" /></h6>
                            <asp:CheckBox ID="CkbTodasUbica" runat="server" CssClass="LblEtiquet" Text="Asignar Todas" OnCheckedChanged="CkbTodasUbica_CheckedChanged" AutoPostBack="true" />
                            <asp:GridView ID="GrdUbicaDispo" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodUbicaBodega"
                                CssClass="DiseñoGrid table-sm" GridLines="Both" AllowPaging="true"
                                OnRowCommand="GrdUbicaDispo_RowCommand" OnRowDataBound="GrdUbicaDispo_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Asignar" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CkbAsigna" Checked='<%# Eval("Asignar").ToString()=="1" ? true : false %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bod" HeaderStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("CodBodega") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="F">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Fila") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="C">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Columna") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prp">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CkbProp" Checked='<%# Eval("Propiedad").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle CssClass="GridFooterStyle" />
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            </asp:GridView>

                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw2AsigUsu" runat="server">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitAsigUsu" runat="server" Text="Asigar Usuario" /></h6>
                    <asp:ImageButton ID="IbtCerrarAsigUsu" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarAsigUsu_Click" ImageAlign="Right" />
                     <div class="row">
                        <div class="col-sm-5">
                             <asp:GridView ID="GrdAsigUsu" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdPersonaAlmacen"
                                CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%" AllowPaging="true" PageSize="12"
                                OnRowCommand="GrdAsigUsu_RowCommand" OnRowEditing="GrdAsigUsu_RowEditing" OnRowUpdating="GrdAsigUsu_RowUpdating"
                                OnRowCancelingEdit="GrdAsigUsu_RowCancelingEdit" OnRowDeleting="GrdAsigUsu_RowDeleting" OnRowDataBound="GrdAsigUsu_RowDataBound"
                                OnPageIndexChanging="GrdAsigUsu_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Activo">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CkbActivoP" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="CkbActivo" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="CkbActivoPP" runat="server" Checked="true" Enabled="false" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Usuario" HeaderStyle-Width="70%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblUsuP" Text='<%# Eval("Persona") %>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="LblUsu" Text='<%# Eval("Persona") %>' runat="server" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="DdlUsuPP" runat="server" Width="100%" Height="28px" />
                                        </FooterTemplate>
                                    </asp:TemplateField>                                 
                                    <asp:TemplateField FooterStyle-Width="15%">
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
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
